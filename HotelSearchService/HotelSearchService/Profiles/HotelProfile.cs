using AutoMapper;
using HotelSearchService.DTOs;
using HotelSearchService.Models;

namespace HotelSearchService.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
        }
    }
}
