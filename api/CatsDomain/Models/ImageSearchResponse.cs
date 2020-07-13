using System;
using System.Collections.Generic;
using System.Text;

namespace TheCatsDomain.Models
{
    public class ImageSearchResponse
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
