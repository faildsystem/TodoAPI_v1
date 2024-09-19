﻿namespace TodoAPI.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
