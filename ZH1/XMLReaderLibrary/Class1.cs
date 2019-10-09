using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XMLReaderLibrary
{
    public class XMLReader
    {
        public List<T> ReadXML<T>(string file)
        {
            List<T> list = new List<T>();

            XDocument xdoc = new XDocument(file);

            var v = from x in xdoc.Root.Descendants("person")
                    select x;

            ;

            return list;
        }
    }
}
