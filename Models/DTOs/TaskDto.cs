using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public TimeSpan? TimeToComplete { get; set; }
    }
}