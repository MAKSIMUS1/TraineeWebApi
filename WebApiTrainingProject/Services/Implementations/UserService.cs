using AutoMapper;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return _mapper.Map<UserProfileDto>(user);
        }
    }

}
