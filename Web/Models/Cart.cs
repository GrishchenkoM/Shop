using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(IProduct product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(x => x.Product.Id == product.Id);

            if (line != null)
                line.Quantity += quantity;
            else
            {
                lineCollection.Add(new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    });
            }
        }

        public void RemoveItem(IProduct product)
        {
            lineCollection.RemoveAll(x => x.Product.Id == product.Id);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(x => x.Product.Cost * x.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public bool IsEmpty
        {
            get { return lineCollection.Count == 0; }
        }
    }

    public class CartLine
    {
        public IProduct Product { get; set; }
        public int Quantity { get; set; }
        public bool IsMine { get; set; }
    }

}