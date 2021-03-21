using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevBlog.Domain.Entities;

namespace DevBlog.Domain.EntityConfigurations
{
	public class PostConfiguration : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(x => x.Title).IsRequired();
			builder.Property(x => x.Title).HasMaxLength(100);
			builder.Property(x => x.Slug).HasMaxLength(100);
			builder.Property(x => x.HeaderImage).HasMaxLength(512);
			builder.Property(x => x.PublishDate).IsRequired();
		}
	}
}

