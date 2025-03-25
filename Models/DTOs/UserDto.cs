using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}