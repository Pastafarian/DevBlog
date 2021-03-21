﻿using System.IO;

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
	}
}