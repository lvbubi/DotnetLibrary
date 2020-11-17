using System.ComponentModel.DataAnnotations.Schema;

namespace netcore_gyakorlas.Models
{
    public class Library : AbstractEntity
    {
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Count { get; set; }
    }
}
