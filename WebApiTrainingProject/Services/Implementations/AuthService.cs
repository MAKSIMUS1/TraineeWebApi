using Microsoft.AspNetCore.Identity;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _userRepository.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateAccessToken(user);

            return new TokenDto { AccessToken = token };
        }

        public async Task<TokenDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password.");
            }

            var token = _tokenService.GenerateAccessToken(user);

            return new TokenDto { AccessToken = token };
        }
    }
}
