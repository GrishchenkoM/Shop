using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Domain.Entities;
using Domain.Entities.Interfaces;

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

        private IEnumerable<IProduct> _products;
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductsCustomers> ProductsCustomerses { get; set; }
    }
}
