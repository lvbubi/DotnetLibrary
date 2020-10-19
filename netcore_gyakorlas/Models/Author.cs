using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_gyakorlas.Models
{
    public class Author : AbstractEntity
    {
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public string Nation { get; set; }
    }
}
