using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevBlog.TestHelper
{
	public static class TestHelpers
	{
		public static MemoryStream GetMemoryStream()
		{
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			writer.Write("test content");
			writer.Flush();
			ms.Position = 0;

			return ms;
		}

        public static JsonSerializerOptions SerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters ={
                new JsonStringEnumConverter()
            }
        };
    }
}