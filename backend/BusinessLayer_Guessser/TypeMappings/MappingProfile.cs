using AutoMapper;
using BusinessLayer_Guesser.DTO;
using BusinessLayer_Guesser.DTO.Requests;
using BusinessLayer_Guesser.DTO.Responses;
using DataLayer_Guesser.Models;

namespace BusinessLayer_Guesser.TypeMappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User

            CreateMap<User, UserResponse>().ReverseMap();

            CreateMap<UserRequest, User>()
                .ForMember(p => p.FullName, opt => opt.MapFrom(dm => $"{dm.FirstName} {dm.LastName}"))
                .ReverseMap();

            #endregion

            #region Game

            CreateMap<Game, GameResponse>().ForMember(p => p.Player, opt => opt.MapFrom(dm => dm.Player)).ReverseMap();

            #endregion

        }
    }
}
