namespace DevBlog.Application.Requests
{
    public class SubmitContactRequestDto
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}