using System;
using System.Threading.Tasks;
using TaskManagementApi.Data.Repositories;
using TaskManagementApi.Helpers;
using TaskManagementApi.Models;
using TaskManagementApi.Models.DTOs;

namespace TaskManagementApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(IUserRepository userRepository, JwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<UserDto> RegisterAsync(UserRegistrationDto userRegistrationDto)
        {
            userRegistrationDto.Username = userRegistrationDto.Username!.ToLower();

            if (await _userRepository.UserExistsAsync(userRegistrationDto.Username))
                throw new ApplicationException("Username already exists");

            PasswordHasher.CreatePasswordHash(
                userRegistrationDto.Password!,
                out string passwordHash,
                out string passwordSalt);

            var userToCreate = new User
            {
                Username = userRegistrationDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddAsync(userToCreate);
            await _userRepository.SaveChangesAsync();

            var userToReturn = new UserDto
            {
                Id = userToCreate.Id,
                Username = userToCreate.Username,
                Token = _tokenGenerator.GenerateToken(userToCreate)
            };

            return userToReturn;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(userLoginDto.Username!.ToLower());

            if (user == null)
                throw new ApplicationException("Invalid username or password");

            if (!PasswordHasher.VerifyPasswordHash(
                userLoginDto.Password!, user.PasswordHash!, user.PasswordSalt!))
                throw new ApplicationException("Invalid username or password");

            var userToReturn = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Token = _tokenGenerator.GenerateToken(user)
            };

            return userToReturn;
        }
    }
}