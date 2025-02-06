using AutoMapper;
using HousingManagementAPI.DTOs;
using HousingManagementAPI.Models;

namespace HousingManagementAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<House, HouseDTO>().ReverseMap();
            CreateMap<HouseCreateModel, House>();
            CreateMap<House, HouseDetailsModel>();

            CreateMap<Apartment, ApartmentDTO>().ReverseMap();
            CreateMap<ApartmentCreateModel, Apartment>();
            CreateMap<Apartment, ApartmentDetailsModel>()
                .ForMember(dest => dest.Residents, opt => opt.MapFrom(src => src.ApartmentResidents!.Select(ar => ar.Resident).ToList()))
                .ForMember(dest => dest.OwnerResidentId, opt =>
                {
                    opt.PreCondition(src => src.ApartmentResidents!.Any(ar => ar.IsOwner));
                    opt.MapFrom(src => src.ApartmentResidents!.First(ar => ar.IsOwner).ResidentId);
                });

            CreateMap<Resident, ResidentDTO>().ReverseMap();
            CreateMap<ResidentCreateModel, Resident>();
            CreateMap<Resident, ResidentDetailsModel>()
                .ForMember(dest => dest.Apartments, opt => opt.MapFrom(src => src.ApartmentResidents!.Select(ar => ar.Apartment).ToList()));

            CreateMap<ApartmentResident, ApartmentResidentDTO>().ReverseMap();
        }
    }
}
