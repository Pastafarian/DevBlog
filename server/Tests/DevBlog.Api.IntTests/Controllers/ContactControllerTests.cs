using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevBlog.Api.Controllers;
using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Requests;
using DevBlog.Application.Settings;
using Xunit;

namespace DevBlog.Api.IntTests.Controllers
{
    [Collection(TestData.TestCollectionName)]
    public class ContactControllerTests
    {
        private readonly ContactController sut;
        private readonly ControllerFixture fixture;

        public ContactControllerTests(ControllerFixture controllerFixture)
        {
            sut = new ContactController(controllerFixture.Mediator, new AppSettings());
            fixture = controllerFixture;
        }

        [Fact]
        public async Task GivenInvalidContact_WhenExecuted_ThenErrorReturned()
        {
            // Arrange
            var invalidRequest = new SubmitContactRequestDto();

            // Act
            var response = await sut.Post(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GivenValidContact_WhenExecuted_ThenOkResponseReturned()
        {
            // Arrange
            var validRequest = new SubmitContactRequestDto
            {
                Name = TestData.RandomString(10),
                Email = TestData.RandomEmail(),
                Message = TestData.RandomString(20)
            };

            // Act
            var response = await sut.Post(validRequest);

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
            var contact = await fixture.Context.Contacts.SingleAsync(x => x.Email == validRequest.Email);
            Assert.Equal(validRequest.Message, contact.Message);
            Assert.Equal(validRequest.Name, contact.Name);
        }
    }
}