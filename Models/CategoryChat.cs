using System.ComponentModel.DataAnnotations;

namespace devchat3.Models
{
    public class CategoryChat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
