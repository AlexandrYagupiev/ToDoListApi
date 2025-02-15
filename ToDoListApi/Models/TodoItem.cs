using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}