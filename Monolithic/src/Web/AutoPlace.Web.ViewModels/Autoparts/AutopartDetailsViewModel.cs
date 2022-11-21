namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class AutopartDetailsViewModel : BaseAutopartsViewModel, IMapFrom<Autopart>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int CountViews { get; set; }

        public bool IsInFavorites { get; set; }

        public string ConditionName { get; set; }

        public string CarManufacturerName { get; set; }

        public string CarModelName { get; set; }

        public string CategoryName { get; set; }

        public string OwnerEmail { get; set; }

        public string OwnerUserName { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<string> Images { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Autopart, AutopartDetailsViewModel>()
                .ForMember(x => x.CarManufacturerName, opt =>
                opt.MapFrom(x => x.Car.Model.Manufacturer.Name))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(x =>
                    x.Images.FirstOrDefault().RemoteImageUrl ??
                    $"/Images/Autoparts/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extension}"))
                .ForMember(x => x.Images, opt =>
                    opt.MapFrom(x =>
                    x.Images.Select(x => $"/Images/Autoparts/{x.Id}.{x.Extension}")));
        }
    }
}
