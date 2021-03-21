namespace DevBlog.Domain.Entities
{
	public class Contact : Entity
	{
		public string Name { get; set; }
		public string Message { get; set; }
		public string Email { get; set; }
	}
}