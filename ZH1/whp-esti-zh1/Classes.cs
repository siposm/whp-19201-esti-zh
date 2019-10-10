﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace whp_esti_zh1
{
    class Detector
    {
        public void DetectWorkerClasses()
        {
            Type[] types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterface("IWorker") != null)
                .OrderByDescending( x => x.FullName )
                .ToArray();

            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("workers", new XAttribute("count", types.Length)));

            foreach (var item in types)
                xdoc.Root.Add(new XElement("class",
                        new XElement("name", item.Name),
                        new XElement("hash", item.Name.GetHashCode())));

            xdoc.Save("workerClasses.xml");
        }
    }

    class FirstFloorWorker : Worker
    {

    }

    class SecondFloorWorker : Worker
    {

    }

    class ThirdFloorWorker : Worker
    {

    }

    

    class XMLReader
    {
        public IEnumerable<string> ReadXML(XDocument xdoc)
        {
            List<string> x = new List<string>();
            foreach (var item in xdoc.Root.Descendants("person"))
                x.Add(item.Element("name").Value);
            return x;
        }

        public XDocument ReadXMLToXDoc(string filename)
        {
            return XDocument.Load(filename);
        }

        public IEnumerable<Worker> ReadXML(string filename)
        {
            XDocument xdoc = XDocument.Load(filename);
            List<Worker> list = new List<Worker>();

            foreach (var item in xdoc.Root.Descendants("person"))
            {
                list.Add(new Worker()
                {
                    Name = item.Element("name").Value,
                    Email = item.Element("email").Value,
                    Dept = item.Element("dept").Value,
                    Phone = item.Element("phone").Value,
                    Rank = item.Element("rank").Value,
                    Room = item.Element("room").Value
                });
            }

            return list;
        }
    }


    [AttributeUsage(AttributeTargets.Property)]
    class EmailValidatorAttribute : Attribute
    {
        public char Character { get; set; }
        public int Length { get; set; }
    }

    
    class Validator
    {
        public bool CheckEmail(object obj)
        {
            if (obj.GetType().GetProperty("Email") != null)
            {
                PropertyInfo prop = obj.GetType().GetProperty("Email");
                foreach (Attribute att in prop.GetCustomAttributes())
                {
                    EmailValidatorAttribute eva = (EmailValidatorAttribute)att;
                    if ((obj as Worker).Email.Contains(eva.Character))
                        if ((obj as Worker).Email.Length > eva.Length)
                            return true;
                }
            }
            return false;
        }
    }

    interface IWorker
    {
        string Name { get; set; }
        string Dept { get; set; }
        string Rank { get; set; }
        string Phone { get; set; }
        string Room { get; set; }
    }

    class Worker : IWorker
    {
        public string Name { get; set; }
        public string Dept { get; set; }
        public string Rank { get; set; }
        public string Phone { get; set; }
        public string Room { get; set; }

        [EmailValidator(Character = '@', Length = 5)]
        public string Email { get; set; }

        public override string ToString()
        {
            return this.Name + " [" + this.Email + "]";
        }
    }
}
