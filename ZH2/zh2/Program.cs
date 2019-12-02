using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zh2
{
    public class Food
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
    }

    public class GroceryShop
    {
        public List<Food> ShoppingCart { get; private set; }

        public GroceryShop()
        {
            this.ShoppingCart = new List<Food>();
        }

        public void AddToCart(Food f)
        {
            this.ShoppingCart.Add(f);
        }

        public void RemoveFromCart(int id)
        {
            ShoppingCart.RemoveAt(id);
        }

        public List<Food> SelectFoodByCriteria(Predicate<Food> pred)
        {
            return (from x in ShoppingCart
                   where pred(x)
                   select x).ToList();
        }

        public double CalculateFinalSumPrice(int discountValue) // 10% 30%...
        {
            if (discountValue <= 0)
                throw new DiscountException("too low disc. value");

            int sum = 0;
            foreach (var item in ShoppingCart)
                sum += (item.Price) * item.Qty;

            return Math.Round((double)(sum - (sum * (discountValue / 100))), 3);
        }

        public void PlaceOrderTXT()
        {
            Task t1 = new Task(() =>
            {
                Console.WriteLine("\t> t1 working...");
                Thread.Sleep(2000); // 2 msp

                // write to file
                XDocument x = new XDocument();
                x.Add(new XElement("names_root"));
                foreach (var item in ShoppingCart)
                    x.Root.Add(new XElement("name", item.Name));
                x.Save("names.xml");
                //StreamWriter sw = new StreamWriter("names.txt");
                //foreach (var item in ShoppingCart)
                //    sw.WriteLine(item.Name);
                //sw.Close();

                Console.WriteLine("t1 finished");
            });

            Task t2 = new Task(() =>
            {
                Console.WriteLine("\t> t2 working...");
                Thread.Sleep(10000); // 10 msp

                // write to file
                XDocument x = new XDocument();
                x.Add(new XElement("prices_root"));
                foreach (var item in ShoppingCart)
                    x.Root.Add(new XElement("price", item.Price));
                x.Save("prices.xml");
                //StreamWriter sw = new StreamWriter("prices.txt");
                //foreach (var item in ShoppingCart)
                //    sw.WriteLine(item.Price);
                //sw.Close();

                Console.WriteLine("t2 finished");
            });

            Task t3 = new Task(() =>
            {
                Console.WriteLine("\t> t3 working...");
                Thread.Sleep(5000); // 5 msp

                // write to file
                XDocument x = new XDocument();
                x.Add(new XElement("quantities_root"));
                foreach (var item in ShoppingCart)
                    x.Root.Add(new XElement("quantity", item.Qty));
                x.Save("qties.xml");
                //StreamWriter sw = new StreamWriter("quantities.txt");
                //foreach (var item in ShoppingCart)
                //    sw.WriteLine(item.Qty);
                //sw.Close();

                Console.WriteLine("t3 finished");
            });

            t1.Start();
            t2.Start();
            t3.Start();

            Task.WhenAll(t1, t2, t3).ContinueWith(x =>
            {
                Console.WriteLine("\n\n----------\n>>> ALL TASKS FINISHED");

            }).ContinueWith( x =>
            {
                int avgTemp = 0;
                var doc = XDocument.Load("http://users.nik.uni-obuda.hu/siposm/db/weatherdata_v2_1.xml");

                foreach (var item in doc.Descendants("temperature"))
                    avgTemp += int.Parse(item.Value);

                Console.WriteLine("\n\tAVG TEMP: " + avgTemp/ doc.Descendants("temperature").Count());

            }).Wait();
        }

        
        private void WriteToFile<T>(IEnumerable<T> coll, string elementType, string saveAs)
        {
            XDocument x = new XDocument();
            x.Add(new XElement("root"));
            foreach (var item in coll)
                x.Root.Add(new XElement(elementType));
            x.Save(saveAs);
        }
    }

    public class DiscountException : Exception
    {
        public DiscountException(string s) : base(s) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GroceryShop gs = new GroceryShop();

            gs.AddToCart(new Food() { Name = "food-1", Price = 1220, Qty = 3 });
            gs.AddToCart(new Food() { Name = "food-2", Price = 990, Qty = 16 });
            gs.AddToCart(new Food() { Name = "food-3", Price = 20300, Qty = 1 });
            gs.AddToCart(new Food() { Name = "food-4", Price = 450, Qty = 2 });

            gs.PlaceOrderTXT();
        }
    }
}
