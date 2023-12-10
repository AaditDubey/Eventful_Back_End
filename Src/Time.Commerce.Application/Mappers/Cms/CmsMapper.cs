using AutoMapper;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Domains.Entities.Cms;

namespace Time.Commerce.Application.Mappers.Cms
{
    public class CmsMapper : Profile
    {
        public CmsMapper()
        {
            CreateMap<Content, ContentView>();
            CreateMap<CreateContentModel, Content>();

            CreateMap<BlogPost, BlogPostView>();
            CreateMap<BlogPost, BlogPostDetailsView>();
            CreateMap<CreateBlogPostModel, BlogPost>();

            CreateMap<Message, MessageView>();
            CreateMap<CreateMessageModel, Message>();

            CreateMap<Widget, WidgetView>();
            CreateMap<WidgetCarousel, WidgetCarouselView>();
            CreateMap<WidgetMenus, WidgetMenusView>();
            CreateMap<WidgetFooters, WidgetFootersView>();
            CreateMap<WidgetFooterColumn, WidgetFooterColumnView>();
            CreateMap<WidgetFootersRow, WidgetFootersRowView>();
        }
    }
}
