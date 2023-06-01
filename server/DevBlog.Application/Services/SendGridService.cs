using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DevBlog.Application.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly ILogger<SendGridService> _logger;
        private readonly ISendGridClient _client;
        
        public SendGridService(ILogger<SendGridService> logger, ISendGridClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<bool> SendEmailAsync(string name, string email, string message)
        {
            _logger.LogInformation("SendEmailAsync called. Name:{Name}, Email:{Email}, Message: {Message}.", name, email, message);

            var from = new EmailAddress("steve@stephenadam.dev", name);
         
            var to = new EmailAddress("stephen.adam@gmail.com", "Steve");
            var msg = MailHelper.CreateSingleEmail(from, to, "Email from site", $"{email} {message}", $"{email} {message}");
            var response = await _client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("SendEmailAsync received success status code from SendGrid");
                return true;
            }

            var httpResponseBody = await response.Body.ReadAsStringAsync();
            _logger.LogError("SendEmailAsync received error code from SendGrid. StatusCode:{StatusCode}, HttpResponseBody:{HttpResponseBody}, Headers: {Headers}", response.StatusCode, httpResponseBody, response.Headers.ToString());

            return false;
        }
    }
}
