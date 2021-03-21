namespace DevBlog.Application.Response
{
	public class EntityCreatedResponseDto
	{
		public int Id { get; set; }

		public EntityCreatedResponseDto() { }

		private EntityCreatedResponseDto(int id)
		{
			Id = id;
		}

		public static EntityCreatedResponseDto Create(int id)
		{
			return new EntityCreatedResponseDto(id);
		}
	}
}