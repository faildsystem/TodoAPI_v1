using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Dtos.TodoItem
{
    public class CreateTodoItemRequestDto
    {

        [Required]
        [MaxLength(100, ErrorMessage = "Title can't be over 100 characters")]
        public string Title { get; set; } = null!;

        [MaxLength(280, ErrorMessage = "Description can't be over 280 characters")]
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}
