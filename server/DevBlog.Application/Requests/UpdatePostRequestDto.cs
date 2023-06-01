using System;

namespace DevBlog.Application.Requests;

public class UpdatePostRequestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string HeaderImage { get; set; }
    public string Slug
    {
        get => _slug.ToLower();
        set => _slug = value.ToLower();
    }

    private string _slug;
    public DateTime? PublishDate { get; set; }
}