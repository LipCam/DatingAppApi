using AutoMapper;
using DatingAppApi.BLL.DTOs.Messages;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Extensions;

namespace DatingAppApi.BLL.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUsers, MemberDTO>()
                .ForMember(p => p.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
                .ForMember(p => p.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<Photos, PhotosDTO>();
            CreateMap<MemberUpdateDTO, AppUsers>();
            CreateMap<RegisterDTO, AppUsers>();
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
            CreateMap<Messages, MessagesDTO>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        }
    }
}
