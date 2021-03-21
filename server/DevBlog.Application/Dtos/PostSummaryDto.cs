﻿using System;

namespace DevBlog.Application.Dtos
{
	public class PostSummaryDto
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public string HeaderImage { get; set; }
		public string Slug { get; set; }
		public DateTime? PublishDate { get; set; }
		public int ReadMinutes { get; set; }
		public bool IsPublished { get; set; }
	}
}