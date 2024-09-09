using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjectManagementSystem.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tasks")]
        public int TaskId { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
