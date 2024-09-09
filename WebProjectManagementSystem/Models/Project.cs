using System.ComponentModel.DataAnnotations;
namespace WebProjectManagementSystem.Models
{

    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}

        // Навигационно свойство към задачи
        public ICollection<Task> Tasks { get; set; }

    }
}
