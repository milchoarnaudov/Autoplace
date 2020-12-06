namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System.Linq;

    using AutoMapper;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class AutopartsListItemViewModel : IMapFrom<Autopart>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int ConditionId { get; set; }

        public int CategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Autopart, AutopartsListItemViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(x =>
                    x.Images.FirstOrDefault().RemoteImageUrl ??
                    $"/Images/Autoparts/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extension}"));
        }
    }
}
