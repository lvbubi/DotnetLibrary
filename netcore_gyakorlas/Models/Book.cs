using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netcore_gyakorlas.Models
{
    public class Book : AbstractEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public Author Author { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public int PublishedYear { get; set; }
        public int PageNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string ISBN { get; set; }
        [Range(0, 100)]
        public int ageLimit { get; set; }
    }
}
