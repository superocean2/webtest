using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace Testing.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public string PictureDisplayUrl { get; set; }
        public Rectangle TextRectangle { get; set; }

    }
}