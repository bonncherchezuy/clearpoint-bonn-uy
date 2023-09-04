using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Models
{
    public class TodoItemForUpdateDto
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
