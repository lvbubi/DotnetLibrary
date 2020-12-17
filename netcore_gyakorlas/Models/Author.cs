using System.ComponentModel.DataAnnotations;

namespace netcore_gyakorlas.Models
{
    public class Author : AbstractEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int BirthYear { get; set; }
        [Required]
        [StringLength(100)]
        public string Nation { get; set; }
    }
}
