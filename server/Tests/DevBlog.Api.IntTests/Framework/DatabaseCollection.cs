using Xunit;

namespace DevBlog.Api.IntTests.Framework
{
    [CollectionDefinition(TestData.TestCollectionName)]
    public class DatabaseCollection : ICollectionFixture<ControllerFixture>
    {
    }
}