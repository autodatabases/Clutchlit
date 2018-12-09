using Clutchlit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Data
{
    public class ProductContext
    {
        public string ConnectionString { get; set; }

        public ProductContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        
        public List<Product> GetAllProducts(string id)
        {
            List<Product> list = new List<Product>();
            
          
            return list;
        }

    }
}
