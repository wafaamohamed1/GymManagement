

using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SesisonViewModels;
using GymManagement.DAL.Models;
using Member = GymManagement.DAL.Models.Member;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSessions();

            MapMembers();

        }

        private void MapMembers()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest=>dest.Address, opt=>opt.MapFrom(src=> $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));



            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));


            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }));



            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();


            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }

        private void MapSessions()
        {
            CreateMap</*SRC*/Session, /*DEST*/SessionViewModel>()
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
             .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name));


            CreateMap<CreateSessionViewModel, Session>();


            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel >()
                .ForMember(dest=>dest.Name , opt=>opt.MapFrom(src=>src.CategoryName));


            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
