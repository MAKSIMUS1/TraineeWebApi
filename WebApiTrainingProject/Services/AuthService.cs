using Microsoft.AspNetCore.Identity;
using Serilog;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtTokenService _tokenService;

        public AuthService(
            IUnitOfWork uow,
            IJwtTokenService tokenService)
        {
            _uow = uow;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto request)
        {
            try
            {
                var existingUser = await _uow.Users.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    Log.Warning("Registration failed. User with email {Email} already exists.", request.Email);
                    throw new InvalidOperationException("User already exists.");
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = hashedPassword
                };

                var result = await _uow.Users.AddAsync(user);
                if (result == null || result.Id == Guid.Empty)
                {
                    Log.Error("Failed to save new user {Email} to the database.", request.Email);
                    throw new Exception("Failed to create user.");
                }

                await _uow.SaveChangesAsync();

                var token = _tokenService.GenerateAccessToken(user);

                Log.Information("New user registered: {Email}", user.Email);

                return new TokenDto { AccessToken = token };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during user registration: {Email}", request.Email);
                throw;
            }
        }

        public async Task<TokenDto> LoginAsync(LoginDto request)
        {
            try
            {
                var user = await _uow.Users.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    Log.Warning("Login failed. User with email {Email} not found.", request.Email);
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    Log.Warning("Login failed. Incorrect password for user {Email}.", request.Email);
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                var token = _tokenService.GenerateAccessToken(user);

                Log.Information("User logged in: {Email}", user.Email);

                return new TokenDto { AccessToken = token };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during login for {Email}", request.Email);
                throw;
            }
        }
    }
}
