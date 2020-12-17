using System.ComponentModel.DataAnnotations;

namespace netcore_gyakorlas.Models
{
    public abstract class AbstractEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }
    }
}
