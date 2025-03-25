using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    public class UserTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        [NotMapped]
        public TimeSpan? TimeToComplete => EndDate.HasValue ? EndDate.Value - StartDate : null;

        public User? User { get; set; }

        [NotMapped]
        public DateTime StartDateLocal => TimeZoneInfo.ConvertTimeFromUtc(StartDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        [NotMapped]
        public DateTime? EndDateLocal => EndDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(EndDate.Value, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")) : null;
    }
}