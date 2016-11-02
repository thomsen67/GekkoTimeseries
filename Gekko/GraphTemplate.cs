using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;



namespace Gekko
{
    [XmlRoot(ElementName = "gekkoplot")]    
    public class Gpt
    {        
        [XmlElement(ElementName = "title")]
        public string title;
        [XmlElement(ElementName = "subtitle")]
        public string subtitle;
        [XmlElement(ElementName = "font")]
        public string font;
        [XmlElement(ElementName = "fontsize")]
        public string fontsize;
        [XmlElement(ElementName = "grid")]
        public string grid;
        [XmlElement(ElementName = "border")]
        public string border;
        //--------------------------------------
        [XmlElement(ElementName = "boxgap")]
        public string boxgap;
        [XmlElement(ElementName = "boxwidth")]
        public string boxwidth;
        [XmlElement(ElementName = "boxstack")]
        public string boxstack;
        //--------------------------------------
        [XmlElement(ElementName = "size")]
        public string size;
        [XmlElement(ElementName = "key")]
        public string key;
        [XmlElement(ElementName = "palette")]
        public string palette;
        //--------------------------------------
        [XmlElement(ElementName = "x")]
        public X x;
        [XmlElement(ElementName = "y")]
        public Y y;
        [XmlElement(ElementName = "y2")]
        public Y y2;
        [XmlElement(ElementName = "labels")]
        public List<Label> labels;
        [XmlElement(ElementName = "arrows")]
        public List<Arrow> arrows;
        [XmlElement(ElementName = "lines")]        
        public List<Line> plotLines;      
    }

    public class Label
    {
        [XmlElement(ElementName = "label")]
        public string label;
    }

    public class Arrow
    {
        [XmlElement(ElementName = "arrow")]
        public string arrow;
    }

    public class X
    {
        [XmlElement(ElementName = "label")]
        public string label;        
        [XmlElement(ElementName = "line")]
        public string line;
        [XmlElement(ElementName = "lineafter")]
        public string lineafter;
        [XmlElement(ElementName = "linebefore")]
        public string linebefore;
        [XmlElement(ElementName = "tics")]
        public string tics;        
    }

    public class Y
    {
        [XmlElement(ElementName = "label")]
        public string label;
        [XmlElement(ElementName = "zeroaxis")]
        public string zeroaxis;
        [XmlElement(ElementName = "line")]
        public string line;
        [XmlElement(ElementName = "tics")]
        public string tics;
        [XmlElement(ElementName = "min")]
        public string min;
        [XmlElement(ElementName = "minsoft")]
        public string minsoft;
        [XmlElement(ElementName = "minhard")]
        public string minhard;
        [XmlElement(ElementName = "max")]
        public string max;
        [XmlElement(ElementName = "maxsoft")]
        public string maxsoft;
        [XmlElement(ElementName = "maxhard")]
        public string maxhard;
    }    

    public class Line
    {
        [XmlElement(ElementName = "type")]
        public string type;  //lines, points, linespoints, impulses, dots, steps, boxes
        [XmlElement(ElementName = "dashtype")]
        public string dashtype;  //1, 2, ...    
        [XmlElement(ElementName = "linewidth")]
        public string linewidth; //number
        [XmlElement(ElementName = "linecolor")]
        public string linecolor;
        [XmlElement(ElementName = "pointtype")]
        public string pointtype;  //none, plus, kryds, ...
        [XmlElement(ElementName = "pointsize")]
        public string pointsize;
        [XmlElement(ElementName = "fillstyle")]
        public string fillstyle;     //empty, solid       
        [XmlElement(ElementName = "y2")]
        public string y2;  //left, right   
        [XmlElement(ElementName = "label")]
        public string label;  //text
    }


    class GraphXml
    {   
        
        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                using (FileStream fs = Program.WaitForFileStream(filePath, Program.GekkoFileReadOrWrite.Write))
                using (writer = G.GekkoStreamWriter(fs))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    writer = new StreamWriter(filePath, append);
                    serializer.Serialize(writer, objectToWrite);
                }
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                using (FileStream fs = Program.WaitForFileStream(filePath, Program.GekkoFileReadOrWrite.Read))
                using (reader = new StreamReader(fs))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
