using System;
using System.Collections.Generic;
using System.Text;
using Clutchlit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Clutchlit.Data
{
    public class AMysqlContext : DbContext
    {
        public AMysqlContext(DbContextOptions<AMysqlContext> options)
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
        public DbSet<B_orders> Orders_spcom { get; set; }
        public DbSet<B_order_state> Orders_states_spcom { get; set; }
        public DbSet<B_customer> Customers_spcom { get; set; }
        public DbSet<B_address> Addresses_spcom { get; set; }
        public DbSet<B_message> Messages_spcom { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Host=mysql-sprzegla.nano.pl;Database=db100006140;Username=db100006140_mik;Password=eX6eZeCZ");
        }
    }
}
