using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjectManagementSystem.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("Users")]
        public int AssignedTo { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
    }
}
