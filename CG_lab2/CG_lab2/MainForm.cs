using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace lab2
{
    public partial class MainForm : Form
    {
        private Dictionary<String, String> tags = new Dictionary<String, String>();
        List<string> uploaded_files = new List<string>();
        List<Image_description> image_list = new List<Image_description>();

        private List<String> tagsNames = new List<String>()
        {
            "File Name",
            "Image Width",
            "Image Height",
            "Bits Per Pixel",
            "Compression",
            "X Resolution",
            "Y Resolution",
            "X Max",
            "Y Max",
            "Horizontal DPI",
            "Vertical DPI",
            "Compression Type",
            "Pixels Per Unit X",
            "Pixels Per Unit Y",
            "Bits Per Pixel",
            "Data Precision",
            "Bits Per Sample",
            "Color Planes",
            "Bits per Pixel"

        };

        public MainForm()
        {
            InitializeComponent();
        }

       
        private void update()
        {

            foreach (string file in uploaded_files)
            {
                tags.Clear();
                string name = "";
                int width = 0;
                int height = 0;
                int h_dpi = 0;
                int v_dpi = 0;
                string compression = "Unknown";
                string color_depth = "";
                string depth_description = "";

                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file);
                foreach (var directory in directories)
                {
                    foreach (var tag in directory.Tags)
                    {
                        if (tagsNames.Contains(tag.Name))
                        {
                            tags[tag.Name] = tag.Description;
                        }

                    }
                }

                foreach (var t in tags)
                {

                    if (t.Key.Equals("File Name"))
                    {
                        name = t.Value;
                    }

                    else if (t.Key.Equals("Compression Type") || t.Key.Equals("Compression"))
                    {
                        compression = t.Value;
                    }


                    else if (t.Key.Equals("Image Width") || t.Key.Equals("X Max"))
                    {
                        width = parse_to_int(t.Value);
                    }

                    else if (t.Key.Equals("Image Height") || t.Key.Equals("Y Max"))
                    {
                        height = parse_to_int(t.Value);
                    }

                    else if (t.Key.Equals("Horizontal DPI") || t.Key.Equals("X Resolution"))
                    {
                        h_dpi = parse_to_int(t.Value);
                    }

                    else if (t.Key.Equals("Vertical DPI") || t.Key.Equals("Y Resolution"))
                    {
                        v_dpi = parse_to_int(t.Value);
                    }

                    else if (t.Key.Equals("Pixels Per Unit X"))
                    {
                        h_dpi = (int)(parse_to_int(t.Value) / 100f * 2.54f);
                    }

                    else if (t.Key.Equals("Pixels Per Unit Y"))
                    {
                        v_dpi = (int)(parse_to_int(t.Value) / 100f * 2.54f);
                    }

                    else if (t.Key.ToLower().Equals("Bits Per Pixel".ToLower()) ||
                        t.Key.Equals("Data Precision") ||
                        t.Key.Equals("Bits Per Sample") ||
                        t.Key.Equals("Color Planes"))
                    {
                        color_depth = t.Value;
                        depth_description = t.Key;
                    }

                }


                image_list.Add(new Image_description(name, width, height, String.Format("({0}, {1})",
                         h_dpi, v_dpi), color_depth, depth_description, compression));
            }

            dataGridView1.DataSource = image_list;
            dataGridView1.Columns[4].Width = 250;


        }

        
        private int parse_to_int(string str)
        {
            return int.Parse(Regex.Match(str, @"\d+").Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            image_list.Clear();
            uploaded_files.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                uploaded_files.Add(openFileDialog1.FileName);
                textBox1.Text = uploaded_files.First();
            }

            dataGridView1.DataSource = null;
            update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            image_list.Clear();
            uploaded_files.Clear();
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = System.IO.Directory.GetCurrentDirectory();
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    uploaded_files = new List<string>(System.IO.Directory.GetFiles(fbd.SelectedPath));
                    textBox1.Text = fbd.SelectedPath;
                }
            }
            dataGridView1.DataSource = null;
            update();
        }
    }
}
