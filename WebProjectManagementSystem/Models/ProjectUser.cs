using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjectManagementSystem.Models
{
    public class ProjectUser
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
