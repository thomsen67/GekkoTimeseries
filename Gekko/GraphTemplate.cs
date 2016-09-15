using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;



namespace Gekko
{
    [XmlRoot(ElementName = "gekkoplot")]    
    public class PlotTemplate
    {
        [XmlElement(ElementName = "main")]
        public PlotMain plotMain;
        [XmlElement(ElementName = "x-axis")]
        public PlotXAxis plotXAxis;
        [XmlElement(ElementName = "y-axis")]
        public PlotYAxis plotYAxis;
        [XmlElement(ElementName = "y-axis2")]
        public PlotYAxis2 plotYAxis2;
        [XmlElement(ElementName = "lines")]        
        public PlotLines plotLines;      
    }

    public class PlotMain
    {
        [XmlElement(ElementName = "title")]
        public string title;
    }   

    public class PlotXAxis
    {
        [XmlElement(ElementName = "title")]
        public string title;
    }

    public class PlotRange
    {
        [XmlElement(ElementName = "min")]
        public string plotRangeMin;
        [XmlElement(ElementName = "max")]
        public string plotRangeMax;
    }

    public class PlotYAxis
    {
        [XmlElement(ElementName = "title")]
        public string title;
        [XmlElement(ElementName = "range")]
        public PlotRange plotRange;
    }

    public class PlotYAxis2
    {
        [XmlElement(ElementName = "title")]
        public string title;
        [XmlElement(ElementName = "range")]
        public PlotRange plotRange;
    }

    public class PlotLines
    {
        [XmlElement(ElementName = "line")]
        public List<PlotLine> plotLine;
    }    

    public class PlotLine
    {
        public string legend;  //text
        public string type;  //lines, points, linespoints, impulses, dots, steps, boxes
        public string width; //number
        public string color;
        public string point;  //plus, kryds, ...
        public string size; //number
        public string scale; //number
        [XmlElement(ElementName = "y-axis")]
        public string yAxis;  //left, right
        public string dashtype;  //1, 2, ...

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
