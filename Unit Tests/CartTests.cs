using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using NUnit.Framework;
using Web.Controllers;
using Web.Models;

namespace Unit_Tests
{
    [TestFixture]
    public class CartTests
    {
        [Test(Description = "Return Cart object")]
        public void GetCart()
        {
            var cartController = new CartController(new DataManager(null,null,null,null,null));
            var cart = cartController.GetCart();
            Assert.IsInstanceOf(typeof (Cart), cart);
        }
    }
}
