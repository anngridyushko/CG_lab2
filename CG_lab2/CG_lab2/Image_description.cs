using System;

namespace lab2
{
    class Image_description
    {
        public string name { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public string dpi { get; set; }

        public string color_depth { get; set; }

        public string depth_description { get; set; }

        public string compression { get; set; }

        public Image_description(string name, int width, int height, string dpi, string color_depth, string depth_description, string compression)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.dpi = dpi;
            this.color_depth = color_depth;
            this.depth_description = depth_description;
            this.compression = compression;
        }

    }
}
