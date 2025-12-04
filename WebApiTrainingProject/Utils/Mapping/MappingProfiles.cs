using AutoMapper;
using System.Text.RegularExpressions;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Utils.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileDto>();
        }
    }
}
