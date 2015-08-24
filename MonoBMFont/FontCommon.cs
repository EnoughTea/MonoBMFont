using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MonoBMFont {
    [DataContract]
    public class FontCommon {
        [XmlAttribute("lineHeight")]
        public int LineHeight { get; set; }

        [XmlAttribute("base")]
        public int Base { get; set; }

        [XmlAttribute("scaleW")]
        public int TextureWidth { get; set; }

        [XmlAttribute("scaleH")]
        public int TextureHeight { get; set; }

        [XmlAttribute("pages")]
        public int TextureCount { get; set; }

        [XmlAttribute("packed")]
        public int Packed { get; set; }

        [XmlAttribute("alphaChnl")]
        public int AlphaChannel { get; set; }

        [XmlAttribute("redChnl")]
        public int RedChannel { get; set; }

        [XmlAttribute("greenChnl")]
        public int GreenChannel { get; set; }

        [XmlAttribute("blueChnl")]
        public int BlueChannel { get; set; }
    }
}