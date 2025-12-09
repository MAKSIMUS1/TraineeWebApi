using AutoMapper;
using Serilog;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(userId);

                if (user == null)
                {
                    Log.Warning("User {UserId} not found", userId);
                    throw new Exception("User not found");
                }

                Log.Information("Loaded profile for user {UserId}", userId);

                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting profile for user {UserId}", userId);
                throw;
            }
        }
    }
}
