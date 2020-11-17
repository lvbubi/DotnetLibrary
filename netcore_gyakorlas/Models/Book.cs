using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_gyakorlas.Models
{
    public class Book : AbstractEntity
    {
        public string Title { get; set; }
        public Author Author { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public int PublishedYear { get; set; }
        public int PageNumber { get; set; }
        public string ISBN { get; set; }
    }
}
