using System;
using System.Collections.Generic;
using System.Text;
using Clutchlit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Clutchlit.Data
{
    public class MysqlContext : DbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options)
            : base(options)
        {
        }
        //
        //
        //
        //
        //
        //
        //public DbSet<Product> Products { get; set; }
        public DbSet<A_orders> Orders_sp24 { get; set; }
        public DbSet<A_order_state> Orders_states { get; set; }
        public DbSet<A_customer> Customers_sp24 { get; set; }
        public DbSet<A_address> Addresses_sp24 { get; set; }
        public DbSet<A_message> Messages_sp24 { get; set; }
        public DbSet<A_cart> Carts_sp24 { get; set; }
        public DbSet<A_cart_main> Cart_main { get; set; }
        public DbSet<A_products> Products_sp24 { get; set; }
        public DbSet<A_product_price> Products_prices_sp24 { get; set; }
        public DbSet<A_Cart_ip> Ip_cart_sp24 { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Host=mysql-sprzegla.nano.pl;Database=db100006076;Username=db100006076_mik;Password=Mx5Zx6dW");
        }
    }
}
