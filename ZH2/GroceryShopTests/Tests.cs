using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

using zh2; // make classes public !


namespace GroceryShopTests
{
    [TestFixture]
    public class Tests
    {
        private GroceryShop gs;

        [SetUp]
        public void Init()
        {
            gs = new GroceryShop();

            gs.AddToCart(new Food() { Name = "food-1", Price = 1220, Qty = 3 });
            gs.AddToCart(new Food() { Name = "food-2", Price = 990, Qty = 16 });
            gs.AddToCart(new Food() { Name = "food-3", Price = 20300, Qty = 1 });
            gs.AddToCart(new Food() { Name = "food-4", Price = 450, Qty = 2 });
        }

        [Test]
        public void AddingToCart_Test()
        {
            // arrange => setup

            // assert
            gs.AddToCart(new Food() { Name = "Watermelon", Price = 770 });

            // act
            Assert.That(gs.ShoppingCart[gs.ShoppingCart.Count-1].Name, Is.EqualTo("Watermelon"));
            Assert.That(gs.ShoppingCart[gs.ShoppingCart.Count-1].Price, Is.EqualTo(770));
        }

        [Test]
        public void FinalSumPrice_Exception_Test()
        {
            Assert.Throws<DiscountException>(() => gs.CalculateFinalSumPrice(0));
            Assert.That(() => gs.CalculateFinalSumPrice(0), Throws.TypeOf<DiscountException>());
        }

        [Test]
        public void FinalSumPrice_Calculation_Test()
        {
            //int sum = gs.ShoppingCart.Sum(x => x.Price);
            int sumQty = gs.ShoppingCart.Sum(x => (x.Price * x.Qty));
            int discValue = 10;
            int sumWithDisc = sumQty - (sumQty * (discValue / 100));

            Assert.That(
                gs.CalculateFinalSumPrice(discValue),
                Is.EqualTo(sumWithDisc));
        }

        [Test]
        public void SelectFoodByCriteria_Test()
        {
            Assert.That(
                gs.SelectFoodByCriteria(new Predicate<Food>(x => x.Qty == 1)).Count,
                Is.EqualTo(1));

            Assert.That(
                gs.SelectFoodByCriteria(new Predicate<Food>(x => x.Qty == 1)),
                Is.Not.Null);
        }
    }
}
