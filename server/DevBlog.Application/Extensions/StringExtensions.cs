using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace DevBlog.Application.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Chops text on last space if under maxLength. Appends ellipsis by default.
		/// </summary>
		/// <returns></returns>
		public static string TruncateText(this string input, int maxLength, bool appendEllipsis = true)
		{
			if (input == null) return string.Empty;

			input = input.Trim();

			if (input.Length <= maxLength) return input;

			input = input.Substring(0, maxLength);

			if (!input.Contains(' ')) return input.Substring(0, maxLength) + "...";

			var choppedString = input.Substring(0, input.LastIndexOf(' '));

			if (appendEllipsis) return choppedString + "...";

			return choppedString;
		}

		public static string HtmlToPlainText(this string html)
		{
			if (string.IsNullOrWhiteSpace(html)) return string.Empty;

			var result = string.Empty;
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(html);
			return htmlDoc.DocumentNode.ChildNodes.Aggregate(result, (current, element) => current + element.InnerText);
		}


		public static int CalculateReadMinutes(this string content)
		{
			var splitChars = new[] { ' ', '\r', '\n' };
			return Math.Max(content.HtmlToPlainText().Split(splitChars, StringSplitOptions.RemoveEmptyEntries).Length / 220, 1);
		}

		/// <summary>
		/// Turns foobarfoobar.baz into foobar.baz
		/// Reduce length of file names for safety when saving to DB. 
		/// </summary>
		/// <returns></returns>
		public static string TruncateFilename(this string filename, int maxLength)
		{
			if (maxLength < 10) throw new ApplicationException("TruncateFilename called with a max length of less than 10.");

			filename = filename?.Trim();

			if (string.IsNullOrEmpty(filename) || filename.Length <= maxLength) return filename;

			var extension = Path.GetExtension(filename);

			if (string.IsNullOrWhiteSpace(extension) || extension.Length > maxLength) return filename.Substring(0, maxLength);

			var fileNameSansExtension = Path.GetFileNameWithoutExtension(filename);

			if (string.IsNullOrWhiteSpace(fileNameSansExtension)) return filename;

			return fileNameSansExtension.Substring(0, maxLength - extension.Length) + extension;
		}
	}
}
