using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleLoggerLibrary;

namespace whp_esti_zh1
{
    class Program
    {
        static void Main(string[] args)
        {
            // FELADAT 0. (dll)
            // class lib > build > using

            // FELADAT 1. (reflexió)
            Detector d = new Detector();
            d.DetectWorkerClasses();

            // FELADAT 2. (xml to object)
            XMLReader reader = new XMLReader();
            IEnumerable<Worker> w = reader.ReadXML("workers.xml");

            // FELADAT 3. (attribute + reflection)
            ConsoleLogger cl = new ConsoleLogger();
            Validator v = new Validator();
            foreach (var item in w) cl.ConsoleLog(v.CheckEmail(item));

            // FELADAT 4. (linq)
            XDocument doc = XDocument.Load("workers.xml");

            // 4.1. kérdezzük le a tamásokat
            var f1 = from x in doc.Descendants("person")
                          where x.Element("name").Value.ToUpper().Contains("TAMÁS")
                          select x;

            // 4.2. egyes intézetekben hányan dolgoznak
            var f2 = from x in doc.Descendants("person")
                     group x by x.Element("dept").Value into g
                     orderby g.Count() descending
                     orderby g.Key
                     select new
                     {
                         DEPT = g.Key,
                         COUNT = g.Count()
                     };
            ;
            // 4.3.

            // 4.4.
        }
    }
}
