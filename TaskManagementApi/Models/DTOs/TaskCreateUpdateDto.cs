using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models.DTOs
{
    public class TaskCreateUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}