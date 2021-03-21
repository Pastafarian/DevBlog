using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevBlog.Domain.Entities;

namespace DevBlog.Domain.EntityConfigurations
{
	public class FileConfiguration : IEntityTypeConfiguration<File>
	{
		public void Configure(EntityTypeBuilder<File> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.FileName).HasMaxLength(150);
		}
	}
}