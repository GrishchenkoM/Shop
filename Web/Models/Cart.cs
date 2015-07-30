using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class Cart
    {
        public void AddItem(IProduct product, int quantity)
        {
            var line = _lineCollection.FirstOrDefault(x => x.Product.Id == product.Id);

            if (line != null)
                line.Quantity += quantity;
            else
            {
                _lineCollection.Add(new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    });
            }
        }

        public void RemoveItem(IProduct product)
        {
            _lineCollection.RemoveAll(x => x.Product.Id == product.Id);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(x => x.Product.Cost * x.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        }

        public bool IsEmpty
        {
            get { return _lineCollection.Count == 0; }
        }

        private readonly List<CartLine> _lineCollection = new List<CartLine>();
    }

    public class CartLine
    {
        public IProduct Product { get; set; }

        public int Quantity { get; set; }

        public bool IsMine { get; set; }
    }

}