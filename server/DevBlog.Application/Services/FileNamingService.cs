using DevBlog.Application.Extensions;
using System;
using System.IO;
using System.Linq;

namespace DevBlog.Application.Services
{
	public class FileNamingService : IFileNamingService
	{
		private const int MaxFilenameLength = 50;

		public string GenerateFileName(string name, string extension)
		{
			name = Path.GetFileNameWithoutExtension(name);

			name = name.Replace(" ", "_");

			foreach (var c in Path.GetInvalidFileNameChars().Append('#'))
			{
				name = name.Replace(c.ToString(), string.Empty);
			}

			name = name.TruncateFilename(MaxFilenameLength);

			return $"{Guid.NewGuid()}_{name}.{extension}";
		}
	}
}