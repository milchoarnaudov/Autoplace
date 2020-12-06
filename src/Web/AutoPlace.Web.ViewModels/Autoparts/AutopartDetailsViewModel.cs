﻿namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System.Linq;

    using AutoMapper;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class AutopartDetailsViewModel : IMapFrom<Autopart>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int CountViews { get; set; }

        public string ConditionName { get; set; }

        public string CarManufacturerName { get; set; }

        public string CarModelName { get; set; }

        public string OwnerName { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Autopart, AutopartDetailsViewModel>()
                .ForMember(x => x.CarManufacturerName, opt =>
                opt.MapFrom(x => x.Car.Model.Manufacturer.Name))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(x =>
                    x.Images.FirstOrDefault().RemoteImageUrl ??
                    $"/Images/Autoparts/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extension}"));
        }
    }
}