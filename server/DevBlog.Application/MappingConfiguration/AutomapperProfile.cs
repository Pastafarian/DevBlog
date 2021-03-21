using AutoMapper;
using DevBlog.Application.Dtos;
using DevBlog.Application.Extensions;
using DevBlog.Domain.Entities;
using System;
using DevBlog.Application.Requests;

namespace DevBlog.Application.MappingConfiguration
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Post, PostDto>()
				.ForMember(x => x.IsPublished, y => y.MapFrom(z => z.PublishDate.HasValue && z.PublishDate <= DateTime.UtcNow));

			CreateMap<PostDto, Post>()
				.ForMember(x => x.ReadMinutes, y => y.MapFrom(z => z.Body.CalculateReadMinutes()));

			CreateMap<CreatePostDto, Post>()
				.ForMember(x => x.Id, y => y.Ignore())
				.ForMember(x => x.ReadMinutes, opt => opt.Ignore());

			CreateMap<Post, PostSummaryDto>()
				.ForMember(x => x.Body, y => y.MapFrom(x => x.Body.TruncateText(40, true)))
				.ForMember(x => x.IsPublished, y => y.MapFrom(z => z.PublishDate.HasValue && z.PublishDate <= DateTime.UtcNow));

            CreateMap<Contact, ContactDto>();

            CreateMap<SubmitContactRequestDto, Contact>()
                .ForMember(x => x.Id, y=>y.Ignore());
		}
	}
}