using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AutoMapper;

namespace AssetManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<AssetDetailsViewDto, AssetDetails>().ReverseMap();
            CreateMap<ItemTypeViewDto, ItemTypes>().ReverseMap();
            CreateMap<VendorViewDto, Vendor>().ReverseMap();
            CreateMap<AssetViewDto, Asset>().ReverseMap();
            //CreateMap<List<ItemTypeViewDto>, List<ItemTypes>>().ReverseMap();
        }
    }
}
