using System;
using System.Collections.Generic;
using System.Text;

namespace TheCatsDomain.Models
{
    public class TheCatSettings
    {
        public string BaseURL { get; set; }
        public string BreedsMethod { get; set; }
        public string CategoryMethod { get; set; }
        public string ImageMethod { get; set; }
        public List<string> ImageCategoryFilter { get; set; } = new List<string>();
    }
}
