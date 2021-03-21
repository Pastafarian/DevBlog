using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevBlog.Domain.Entities;

namespace DevBlog.Domain.EntityConfigurations
{
	public class ContactConfiguration : IEntityTypeConfiguration<Contact>
	{
		public void Configure(EntityTypeBuilder<Contact> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Message).HasMaxLength(500);
			builder.Property(x => x.Email).HasMaxLength(150);
        }
	}
}