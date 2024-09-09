using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjectManagementSystem.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tasks")]
        public int TaskId { get; set; }
        public string FilePath { get; set; }

        [ForeignKey("Users")]
        public string UploadedBy { get; set; }
        public string UploadedAt { get; set; }
    }
}
