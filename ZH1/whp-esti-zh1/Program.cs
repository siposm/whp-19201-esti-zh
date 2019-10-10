using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleLoggerLibrary;

namespace whp_esti_zh1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Worker s1 = new Worker() { Name = "Worker 1", Email = "wor@ker.com" };
            //Worker s2 = new Worker() { Name = "Worker 2", Email = "wor" };

            //Validator v = new Validator();

            //Console.WriteLine(v.CheckEmail(s1));
            //Console.WriteLine(v.CheckEmail(s2));

            // ==================================================================

            XMLReader reader = new XMLReader();
            IEnumerable<Worker> w = reader.ReadXML("workers.xml");


            Detector d = new Detector();
            d.DetectWorkerClasses();

            ConsoleLogger cl = new ConsoleLogger();
            Validator v = new Validator();

            foreach (var item in w)
            {
                //cl.ConsoleLog(item);
                cl.ConsoleLog(v.CheckEmail(item));
            }

        }
    }
}
