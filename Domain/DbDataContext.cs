using System.Collections.Generic;
using System.Data.Entity;
using Domain.Entities;

namespace Domain
{
    public class DbDataContext : DbContext
    {
        public DbDataContext(string connectionString)
        {
            #region EF
            Database.Connection.ConnectionString = connectionString;
            #endregion
        }

        #region EF
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<ProductsCustomers> ProductsCustomerses { get; set; }
        #endregion

        public string ConnectionString
        {
            get { return Db.ConnectionString; }
        }

        public IEnumerable<Customer> Customers { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ProductsCustomers> ProductsCustomerses { get; set; }
    }
}
