using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace MonoBMFont {
    [DataContract]
    public class FontInfo {
        private Rectangle _padding;
        private Point _spacing;

        [XmlAttribute("face")]
        public string Face { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

        [XmlAttribute("bold")]
        public int Bold { get; set; }

        [XmlAttribute("italic")]
        public int Italic { get; set; }

        [XmlAttribute("charset")]
        public string CharSet { get; set; }

        [XmlAttribute("unicode")]
        public int Unicode { get; set; }

        [XmlAttribute("stretchH")]
        public int StretchHeight { get; set; }

        [XmlAttribute("smooth")]
        public int Smooth { get; set; }

        [XmlAttribute("aa")]
        public int SuperSampling { get; set; }

        [XmlAttribute("padding")]
        public string Padding {
            get { return _padding.X + "," + _padding.Y + "," + _padding.Width + "," + _padding.Height; }
            set {
                var padding = value.Split(',');
                _padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]),
                    Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));
            }
        }

        [XmlAttribute("spacing")]
        public string Spacing {
            get { return _spacing.X + "," + _spacing.Y; }
            set {
                var spacing = value.Split(',');
                _spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
            }
        }

        [XmlAttribute("outline")]
        public int OutLine { get; set; }
    }
}