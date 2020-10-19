using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_gyakorlas.Models
{
    public class Book : AbstractEntity
    {
        public string Title { get; set; }
        public Author Author { get; set; }
        public int PublishedYear { get; set; }
        public int PageNumber { get; set; }
        public string ISBN { get; set; }
    }
}
