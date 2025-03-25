using System;
using System.Threading.Tasks;
using TaskManagementApi.Data.Repositories;
using TaskManagementApi.Helpers;
using TaskManagementApi.Models;
using TaskManagementApi.Models.DTOs;

namespace TaskManagementApi.Services
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(UserRegistrationDto userRegistrationDto);
        Task<UserDto> LoginAsync(UserLoginDto userLoginDto);
    }
}