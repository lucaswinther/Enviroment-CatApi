using System;
using System.Collections.Generic;
using System.Text;

namespace TheCatsDomain.Models
{
    public class BreedsSearchResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Temperament { get; set; }
        public string Description { get; set; }
    }
}
