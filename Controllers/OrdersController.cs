using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Clutchlit.Data;
using Clutchlit.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clutchlit.Controllers
{


    [Authorize]
    public class OrdersController : Controller
    {
        private readonly MysqlContext _contextSp24;
        private readonly AMysqlContext _contextSp;
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context, MysqlContext contextSp24, AMysqlContext contextSp)
        {
            _contextSp24 = contextSp24;
            _contextSp = contextSp;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            return View();
        }
        public IActionResult GetCartAdditionalInfo(int Cart_id, string Shop_id)
        {
            var result = "";
            if (Shop_id == "Sp1")
            {
                var info = _contextSp.Ip_cart_spcom.Where(i => i.Id_cart == Cart_id);
                var results = from i in info select new A_Cart_ip { Id_cart = i.Id_cart, Ip_address = i.Ip_address, Additional = (i.Ip_address == "204.8.220.150" ? "Biuro" : "Zewnętrzne") };
                foreach(A_Cart_ip s in results)
                {
                    result = "<tr><td><b>Adres</b></td><td>" + s.Ip_address+ "</td><td>" + s.Additional + "</td></tr>";
                }
            }
            else
            {
                var info = _contextSp24.Ip_cart_sp24.Where(i => i.Id_cart == Cart_id);
                var results = from i in info select new A_Cart_ip { Id_cart = i.Id_cart, Ip_address = i.Ip_address, Additional = (i.Ip_address == "204.8.220.150" ? "Biuro" : "Zewnętrzne") };
                foreach (A_Cart_ip s in results)
                {
                    result = "<tr><td><b>Adres</b></td><td>" + s.Ip_address + "</td><td>" + s.Additional + "</td></tr>";
                }

            }
            return Json(result);
        }
        public IActionResult GetCartProducts(int Cart_id, string Shop_id)
        {
            var result = "";
            if (Shop_id == "Sp1")
            {
                var cart_products = _contextSp.Carts_spcom.Where(c => c.Id_cart == Cart_id);
                var products = _contextSp.Products_spcom;
                var price = _contextSp24.Products_prices_sp24;

                var results = from cp in cart_products
                          join p in products on cp.Id_product equals p.Id_product
                              join pp in price on cp.Id_product equals pp.Id_product
                              select new A_products { Cart_Quantity = cp.Quantity, Id_product = p.Id_product, Name = p.Name, Cart_price = Math.Round((pp.Price * (decimal)1.23), 0) };

                foreach (A_products s in results)
                {
                    result = result + "<tr><td><b>"+s.Name+"</b></td><td>" + s.Cart_price + "</td><td>" + s.Cart_Quantity + "</td><td>"+s.Id_product+"</td></tr>";
                }
                return Json(result);
            }
            else if (Shop_id == "Sp2")
            {
                var cart_products = _contextSp24.Carts_sp24.Where(c => c.Id_cart == Cart_id);
                var products = _contextSp24.Products_sp24;
                var price = _contextSp24.Products_prices_sp24;
                var results = from cp in cart_products
                              join p in products on cp.Id_product equals p.Id_product
                              join pp in price on cp.Id_product equals pp.Id_product
                              select new A_products { Cart_Quantity = cp.Quantity, Id_product = p.Id_product, Name = p.Name, Cart_price= Math.Round((pp.Price * (decimal)1.23),0) };

                foreach (A_products s in results)
                {
                    result = result + "<tr><td><b>" + s.Name + "</b></td><td>" + s.Cart_price + "</td><td>" + s.Cart_Quantity + "</td><td>" + s.Id_product + "</td></tr>";
                }
                return Json(result);
            }
            else
            {
                return Json("<tr><td><b>Brak danych. Błędny sklep</b></td><td></td><td></td></tr>");
            }
            
        }
        public IActionResult GetAllSp24(string FlagShop, string FlagPayment, string FlagStatus)
        {
            // jakoś trzeba będzie tymi flagami obrobić
            string flagShop = FlagShop;
            string flagPayment = FlagPayment;
            string flagStatus = FlagStatus;
            //

            var sp24 = _contextSp24.Orders_sp24;
            var spC = _contextSp.Orders_spcom;

            var res_sp24 = Enumerable.Empty<A_orders_display>().AsQueryable();
            var res_spC = Enumerable.Empty<A_orders_display>().AsQueryable();

            // 1) wszystko puste
            if (flagShop == "0" && flagPayment == "0" && flagStatus == "0")
            {


                res_sp24 = (from orders in sp24
                            join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                            join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                            join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                            join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                            join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                            from msg1 in gh.DefaultIfEmpty()
                            select new A_orders_display()
                            {
                                Shop = "Sp2",
                                Created = orders.Created,
                                Id_carrier = orders.Id_carrier,
                                Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                Id_order = orders.Id_order,
                                Current_state = states.Name,
                                Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                Id_cart = orders.Id_cart,
                                Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                Payment = orders.Payment,
                                Reference = orders.Reference,
                                Total_paid = orders.Total_paid,
                                Total_paid_products = orders.Total_paid_products,
                                Total_shipping = orders.Total_shipping,
                                AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                            });
                res_spC = (from orders in spC
                           join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                           join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                           join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                           join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                           join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                           from msg1 in gh.DefaultIfEmpty()
                           select new A_orders_display()
                           {
                               Shop = "Sp1",
                               Created = orders.Created,
                               Id_carrier = orders.Id_carrier,
                               Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                               Id_order = orders.Id_order,
                               Current_state = states.Name,
                               Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                               Id_cart = orders.Id_cart,
                               Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                               Payment = orders.Payment,
                               Reference = orders.Reference,
                               Total_paid = orders.Total_paid,
                               Total_paid_products = orders.Total_paid_products,
                               Total_shipping = orders.Total_shipping,
                               AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                           });

            }
            // 2) Przynajmniej sklep wybrany 
            else if (flagShop != "0")
            {
                if (flagShop == "1")
                {
                    if(flagPayment != "0" && flagStatus == "0")
                    {
                        res_spC = (from orders in spC
                                   join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                                   join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                                   join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                                   join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                                   join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                                   where orders.Module == flagPayment
                                   from msg1 in gh.DefaultIfEmpty()
                                   select new A_orders_display()
                                   {
                                       Shop = "Sp1",
                                       Created = orders.Created,
                                       Id_carrier = orders.Id_carrier,
                                       Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                       Id_order = orders.Id_order,
                                       Current_state = states.Name,
                                       Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                       Id_cart = orders.Id_cart,
                                       Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                       Payment = orders.Payment,
                                       Reference = orders.Reference,
                                       Total_paid = orders.Total_paid,
                                       Total_paid_products = orders.Total_paid_products,
                                       Total_shipping = orders.Total_shipping,
                                       AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                   });
                    }
                    else if(flagStatus != "0" && flagPayment == "0")
                    {
                        res_spC = (from orders in spC
                                   join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                                   join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                                   join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                                   join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                                   join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                                   where orders.Current_state == int.Parse(flagStatus)
                                   from msg1 in gh.DefaultIfEmpty()
                                   select new A_orders_display()
                                   {
                                       Shop = "Sp1",
                                       Created = orders.Created,
                                       Id_carrier = orders.Id_carrier,
                                       Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                       Id_order = orders.Id_order,
                                       Current_state = states.Name,
                                       Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                       Id_cart = orders.Id_cart,
                                       Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                       Payment = orders.Payment,
                                       Reference = orders.Reference,
                                       Total_paid = orders.Total_paid,
                                       Total_paid_products = orders.Total_paid_products,
                                       Total_shipping = orders.Total_shipping,
                                       AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                   });
                    }
                    else if(flagPayment != "0" && flagStatus != "0")
                    {
                        res_spC = (from orders in spC
                                   join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                                   join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                                   join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                                   join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                                   join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                                   where orders.Module == flagPayment && orders.Current_state == int.Parse(flagStatus)
                                   from msg1 in gh.DefaultIfEmpty()
                                   select new A_orders_display()
                                   {
                                       Shop = "Sp1",
                                       Created = orders.Created,
                                       Id_carrier = orders.Id_carrier,
                                       Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                       Id_order = orders.Id_order,
                                       Current_state = states.Name,
                                       Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                       Id_cart = orders.Id_cart,
                                       Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                       Payment = orders.Payment,
                                       Reference = orders.Reference,
                                       Total_paid = orders.Total_paid,
                                       Total_paid_products = orders.Total_paid_products,
                                       Total_shipping = orders.Total_shipping,
                                       AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                   });
                    }
                    else // podstawowy przypadek
                    {
                        res_spC = (from orders in spC
                                   join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                                   join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                                   join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                                   join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                                   join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                                   from msg1 in gh.DefaultIfEmpty()
                                   select new A_orders_display()
                                   {
                                       Shop = "Sp1",
                                       Created = orders.Created,
                                       Id_carrier = orders.Id_carrier,
                                       Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                       Id_order = orders.Id_order,
                                       Current_state = states.Name,
                                       Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                       Id_cart = orders.Id_cart,
                                       Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                       Payment = orders.Payment,
                                       Reference = orders.Reference,
                                       Total_paid = orders.Total_paid,
                                       Total_paid_products = orders.Total_paid_products,
                                       Total_shipping = orders.Total_shipping,
                                       AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                   });
                    }
                }
                else
                {
                    if (flagPayment != "0" && flagStatus == "0")
                    {
                        res_sp24 = (from orders in sp24
                                    join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                    join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                    join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                    join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                    join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                    where orders.Module == flagPayment
                                    from msg1 in gh.DefaultIfEmpty()
                                    select new A_orders_display()
                                    {
                                        Shop = "Sp2",
                                        Created = orders.Created,
                                        Id_carrier = orders.Id_carrier,
                                        Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                        Id_order = orders.Id_order,
                                        Current_state = states.Name,
                                        Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                        Id_cart = orders.Id_cart,
                                        Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                        Payment = orders.Payment,
                                        Reference = orders.Reference,
                                        Total_paid = orders.Total_paid,
                                        Total_paid_products = orders.Total_paid_products,
                                        Total_shipping = orders.Total_shipping,
                                        AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                    });
                    }
                    else if (flagStatus != "0" && flagPayment == "0")
                    {
                        res_sp24 = (from orders in sp24
                                    join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                    join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                    join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                    join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                    join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                    where orders.Current_state == int.Parse(flagStatus)
                                    from msg1 in gh.DefaultIfEmpty()
                                    select new A_orders_display()
                                    {
                                        Shop = "Sp2",
                                        Created = orders.Created,
                                        Id_carrier = orders.Id_carrier,
                                        Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                        Id_order = orders.Id_order,
                                        Current_state = states.Name,
                                        Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                        Id_cart = orders.Id_cart,
                                        Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                        Payment = orders.Payment,
                                        Reference = orders.Reference,
                                        Total_paid = orders.Total_paid,
                                        Total_paid_products = orders.Total_paid_products,
                                        Total_shipping = orders.Total_shipping,
                                        AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                    });
                    }
                    else if (flagPayment != "0" && flagStatus != "0")
                    {
                        res_sp24 = (from orders in sp24
                                    join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                    join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                    join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                    join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                    join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                    where orders.Current_state == int.Parse(flagStatus) && orders.Payment == flagPayment
                                    from msg1 in gh.DefaultIfEmpty()
                                    select new A_orders_display()
                                    {
                                        Shop = "Sp2",
                                        Created = orders.Created,
                                        Id_carrier = orders.Id_carrier,
                                        Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                        Id_order = orders.Id_order,
                                        Current_state = states.Name,
                                        Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                        Id_cart = orders.Id_cart,
                                        Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                        Payment = orders.Payment,
                                        Reference = orders.Reference,
                                        Total_paid = orders.Total_paid,
                                        Total_paid_products = orders.Total_paid_products,
                                        Total_shipping = orders.Total_shipping,
                                        AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                    });
                    }
                    else // podstawowy przypadek
                    {
                        res_sp24 = (from orders in sp24
                                    join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                    join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                    join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                    join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                    join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                    from msg1 in gh.DefaultIfEmpty()
                                    select new A_orders_display()
                                    {
                                        Shop = "Sp2",
                                        Created = orders.Created,
                                        Id_carrier = orders.Id_carrier,
                                        Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                        Id_order = orders.Id_order,
                                        Current_state = states.Name,
                                        Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                        Id_cart = orders.Id_cart,
                                        Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                        Payment = orders.Payment,
                                        Reference = orders.Reference,
                                        Total_paid = orders.Total_paid,
                                        Total_paid_products = orders.Total_paid_products,
                                        Total_shipping = orders.Total_shipping,
                                        AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                    });
                    }
                }
            }
            else if (flagShop == "0")
            {
                if (flagPayment != "0" && flagStatus == "0")
                {
                    res_sp24 = (from orders in sp24
                                join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                where orders.Module == flagPayment
                                from msg1 in gh.DefaultIfEmpty()
                                select new A_orders_display()
                                {
                                    Shop = "Sp2",
                                    Created = orders.Created,
                                    Id_carrier = orders.Id_carrier,
                                    Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                    Id_order = orders.Id_order,
                                    Current_state = states.Name,
                                    Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                    Id_cart = orders.Id_cart,
                                    Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                    Payment = orders.Payment,
                                    Reference = orders.Reference,
                                    Total_paid = orders.Total_paid,
                                    Total_paid_products = orders.Total_paid_products,
                                    Total_shipping = orders.Total_shipping,
                                    AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                });
                    res_spC = (from orders in spC
                               join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                               join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                               join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                               join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                               join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                               where orders.Module == flagPayment
                               from msg1 in gh.DefaultIfEmpty()
                               select new A_orders_display()
                               {
                                   Shop = "Sp1",
                                   Created = orders.Created,
                                   Id_carrier = orders.Id_carrier,
                                   Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                   Id_order = orders.Id_order,
                                   Current_state = states.Name,
                                   Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                   Id_cart = orders.Id_cart,
                                   Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                   Payment = orders.Payment,
                                   Reference = orders.Reference,
                                   Total_paid = orders.Total_paid,
                                   Total_paid_products = orders.Total_paid_products,
                                   Total_shipping = orders.Total_shipping,
                                   AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                               });
                }
                else if (flagStatus != "0" && flagPayment == "0")
                {
                    res_sp24 = (from orders in sp24
                                join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                where orders.Current_state == int.Parse(flagStatus)
                                from msg1 in gh.DefaultIfEmpty()
                                select new A_orders_display()
                                {
                                    Shop = "Sp2",
                                    Created = orders.Created,
                                    Id_carrier = orders.Id_carrier,
                                    Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                    Id_order = orders.Id_order,
                                    Current_state = states.Name,
                                    Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                    Id_cart = orders.Id_cart,
                                    Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                    Payment = orders.Payment,
                                    Reference = orders.Reference,
                                    Total_paid = orders.Total_paid,
                                    Total_paid_products = orders.Total_paid_products,
                                    Total_shipping = orders.Total_shipping,
                                    AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                });
                    res_spC = (from orders in spC
                               join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                               join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                               join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                               join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                               join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                               where orders.Current_state == int.Parse(flagStatus)
                               from msg1 in gh.DefaultIfEmpty()
                               select new A_orders_display()
                               {
                                   Shop = "Sp1",
                                   Created = orders.Created,
                                   Id_carrier = orders.Id_carrier,
                                   Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                   Id_order = orders.Id_order,
                                   Current_state = states.Name,
                                   Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                   Id_cart = orders.Id_cart,
                                   Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                   Payment = orders.Payment,
                                   Reference = orders.Reference,
                                   Total_paid = orders.Total_paid,
                                   Total_paid_products = orders.Total_paid_products,
                                   Total_shipping = orders.Total_shipping,
                                   AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                               });
                }
                else if (flagPayment != "0" && flagStatus != "0")
                {
                    res_sp24 = (from orders in sp24
                                join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                where orders.Module == flagPayment && orders.Current_state == int.Parse(flagStatus)
                                from msg1 in gh.DefaultIfEmpty()
                                select new A_orders_display()
                                {
                                    Shop = "Sp2",
                                    Created = orders.Created,
                                    Id_carrier = orders.Id_carrier,
                                    Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                    Id_order = orders.Id_order,
                                    Current_state = states.Name,
                                    Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                    Id_cart = orders.Id_cart,
                                    Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                    Payment = orders.Payment,
                                    Reference = orders.Reference,
                                    Total_paid = orders.Total_paid,
                                    Total_paid_products = orders.Total_paid_products,
                                    Total_shipping = orders.Total_shipping,
                                    AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                });
                    res_spC = (from orders in spC
                               join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                               join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                               join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                               join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                               join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh
                               where orders.Module == flagPayment && orders.Current_state == int.Parse(flagStatus)
                               from msg1 in gh.DefaultIfEmpty()
                               select new A_orders_display()
                               {
                                   Shop = "Sp1",
                                   Created = orders.Created,
                                   Id_carrier = orders.Id_carrier,
                                   Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                   Id_order = orders.Id_order,
                                   Current_state = states.Name,
                                   Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                   Id_cart = orders.Id_cart,
                                   Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                   Payment = orders.Payment,
                                   Reference = orders.Reference,
                                   Total_paid = orders.Total_paid,
                                   Total_paid_products = orders.Total_paid_products,
                                   Total_shipping = orders.Total_shipping,
                                   AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                               });
                }
                else // podstawowy przypadek
                {
                    res_sp24 = (from orders in sp24
                                join states in _contextSp24.Orders_states on orders.Current_state equals states.Id
                                join customers in _contextSp24.Customers_sp24 on orders.Id_customer equals customers.Id_customer
                                join addresses in _contextSp24.Addresses_sp24 on orders.Id_address_d equals addresses.Id_address
                                join invoice in _contextSp24.Addresses_sp24 on orders.Id_address_i equals invoice.Id_address
                                join msg in _contextSp24.Messages_sp24 on orders.Id_order equals msg.Id_order into gh
                                from msg1 in gh.DefaultIfEmpty()
                                select new A_orders_display()
                                {
                                    Shop = "Sp2",
                                    Created = orders.Created,
                                    Id_carrier = orders.Id_carrier,
                                    Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                    Id_order = orders.Id_order,
                                    Current_state = states.Name,
                                    Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                    Id_cart = orders.Id_cart,
                                    Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                    Payment = orders.Payment,
                                    Reference = orders.Reference,
                                    Total_paid = orders.Total_paid,
                                    Total_paid_products = orders.Total_paid_products,
                                    Total_shipping = orders.Total_shipping,
                                    AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                                });
                    res_spC = (from orders in spC
                               join states in _contextSp.Orders_states_spcom on orders.Current_state equals states.Id
                               join customers in _contextSp.Customers_spcom on orders.Id_customer equals customers.Id_customer
                               join addresses in _contextSp.Addresses_spcom on orders.Id_address_d equals addresses.Id_address
                               join invoice in _contextSp.Addresses_spcom on orders.Id_address_i equals invoice.Id_address
                               join msg in _contextSp.Messages_spcom on orders.Id_order equals msg.Id_order into gh                             
                               from msg1 in gh.DefaultIfEmpty()
                               select new A_orders_display()
                               {
                                   Shop = "Sp1",
                                   Created = orders.Created,
                                   Id_carrier = orders.Id_carrier,
                                   Id_address_i = string.Format("{0} {1} <br /> {2} {3} <br/><b>NIP:</b> {4}", invoice.Address1, invoice.Address2, invoice.ZipCode, invoice.City, invoice.Nip),
                                   Id_order = orders.Id_order,
                                   Current_state = states.Name,
                                   Id_address_d = string.Format("{0} {1} {2} {3}", addresses.Address1, addresses.Address2, addresses.ZipCode, addresses.City),
                                   Id_cart = orders.Id_cart,
                                   Id_customer = string.Format("{0} {1} <br/><b>{2}</b> ", customers.FirstName, customers.LastName, customers.Company),
                                   Payment = orders.Payment,
                                   Reference = orders.Reference,
                                   Total_paid = orders.Total_paid,
                                   Total_paid_products = orders.Total_paid_products,
                                   Total_shipping = orders.Total_shipping,
                                   AdditionalInfo = string.Format("<b>Uwagi:</b> {0} <br /> <b>VIN:</b> {1}", addresses.AdditionalInfo, msg1.Message)
                               });
                }
            }

          
            // przed połączeniem musimy utworzyć ostateczne listy 
            // łączymy dwie listy
            
            var concat = res_sp24.ToList().Union(res_spC.ToList());
            // porządkujemy liste
            var concat_sorted = concat.OrderByDescending(o => o.Created);

            
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;
            IQueryable<A_orders_display> result = null;
            result = concat_sorted.AsQueryable();
            var customerData = result;
            //Sorting  
            
            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Reference.ToUpper().Contains(searchValue.ToUpper()));
            }
            //Paging   
            recordsTotal = customerData.Count();
            //Paging   
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            Response.StatusCode = 200;
            return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        public IActionResult AddOrder()
        {
            string cookieValueFromReq = Request.Cookies["product_id"];
            
            if (cookieValueFromReq != null)
            {
                List<string> itemList = cookieValueFromReq.Split(",").ToList();
                List<Product> productsList = new List<Product>();
                foreach (var singleItem in itemList)
                {
                    productsList.Add(_context.Products.Where(x => x.Id == int.Parse(singleItem)).SingleOrDefault());
                }
                ViewData["busketIT"] = productsList;

            }
            else
            {
                ViewData["busketIT"] = null;
            }

            
            return View();
        }
        static string GetMd5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public IActionResult CreateOrder(string ShopType, string deliveryType, string exampleInputEmail1, string nameInput, string surnameInput, string deliveryName, string deliverySurname, string deliveryCompany, string deliveryNip, string deliveryAddress, string deliveryZip, string deliveryCity, string deliveryCountry, string deliveryNumber, string invoiceName, string invoiceSurname, string invoiceCompany, string invoiceNip, string invoiceAddress, string invoiceZip, string invoiceCity, string invoiceCountry, string invoiceNumber)
        {
            var user = User.Identity.Name;
            
            int noOfRowInserted = _context.Database.ExecuteSqlCommand("insert into cl_orders(order_date, order_creator) values (NOW(),'"+ user +"')");

            // dodajemy zamówienie do sprzegla24.pl
            int id = _contextSp24.Orders_sp24.LastOrDefault().Id_order + 1; // Id nowego zamówienia
            string reference = "SP24-" + id.ToString(); // referencja nowego zamówienia
            string secure_key = GetMd5Hash(Guid.NewGuid().ToString()); // secure key do ps_orders
            string deliveryTypeS = "";
            string module = "";
            switch (deliveryType)
            {
                case "18":
                    {
                        deliveryTypeS = "Płatność u kuriera przy odbiorze";
                        module = "cashondelivery"; 
                        break;
                    }
                case "20":
                    {
                        deliveryTypeS = "Przelew na konto";
                        module = "bankwire";
                        break;
                    }
                case "22":
                    {
                        deliveryTypeS = "Płatność gotówką lub przelewem";
                        module = "cheque";
                        break;
                    }
            }


            // podliczamy koszyk
            List<Product> productsList = new List<Product>();
            string cookieValueFromReq = Request.Cookies["product_id"];

            if (cookieValueFromReq != null)
            {
                List<string> itemList = cookieValueFromReq.Split(",").ToList();
                foreach (var singleItem in itemList)
                {
                    productsList.Add(_context.Products.Where(x => x.Id == int.Parse(singleItem)).SingleOrDefault());
                }
            }

            decimal totalPaid = 0;
            decimal totalPaidTaxIncl = 0;
            decimal totalPaidTaxExcl = 0;
            decimal totalPaidReal = 0;
            decimal totalProducts = 0;
            decimal totalProductsWt = 0;
            decimal totalShipping = 0;
            decimal totalShippingTaxIncl = 0;
            decimal totalShippingTaxExcl = 0;
            decimal CarrierTaxRate = 0;

            // podliczamy koszyk

            int noPsCustomer = _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_customer (id_shop_group, id_shop, id_gender, id_default_group, id_lang, id_risk, company, siret, ape, firstname, lastname, email, passwd, last_passwd_gen, birthday, newsletter, ip_registration_newsletter, newsletter_date_add, optin, website, outstanding_allow_amount, show_public_prices, max_payment_days, secure_key, note, active, is_guest, deleted, date_add, date_upd) VALUES (1, 1, 0, 2, 1, 0, '"+invoiceCompany+"', NULL, NULL, '"+nameInput+"', '"+surnameInput+"', '"+exampleInputEmail1+"', 'AA', NOW(), '0000-00-00', 0, NULL, '0000-00-00 00:00:00', 0, NULL, 0.00, 0, 0, '"+secure_key+"', NULL, 1, 1, 0, NOW(),NOW())");
            int customerLastId = _contextSp24.Customers_sp24.Last().Id_customer; // id nowo-dodanego klienta. 

            int NoPsAddressDelivery = _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_address (id_country, id_state, id_customer, id_manufacturer, id_supplier, id_warehouse, alias, company, lastname, firstname, address1, address2, postcode, city, other, phone, phone_mobile, vat_number, dni, date_add, date_upd, active, deleted) VALUES ('"+deliveryCountry+"', 0, '"+customerLastId+"', 0, 0, 0, 'Mój adres', '"+deliveryCompany+"', '"+deliveryName+"', '"+deliverySurname+"', '"+deliveryAddress+"', '', '"+deliveryZip+"', '"+deliveryCity+"', '', '', '"+deliveryNumber+"', '"+deliveryNip+"', '', NOW(), NOW(), 1, 0) ");
            int addressDeliveryId = _contextSp24.Addresses_sp24.Last().Id_address;

            int NoPsAddressInvoice = _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_address (id_country, id_state, id_customer, id_manufacturer, id_supplier, id_warehouse, alias, company, lastname, firstname, address1, address2, postcode, city, other, phone, phone_mobile, vat_number, dni, date_add, date_upd, active, deleted) VALUES ('" + invoiceCountry + "', 0, '" + customerLastId + "', 0, 0, 0, 'Mój adres rozliczeniowy', '" + invoiceCompany + "', '" + invoiceName + "', '" + invoiceSurname + "', '" + invoiceAddress + "', '', '" + invoiceZip + "', '" + invoiceCity + "', '', '', '" + invoiceNumber + "', '" + invoiceNip + "', '', NOW(), NOW(), 1, 0) ");
            int addressInvoiceId = _contextSp24.Addresses_sp24.Last().Id_address;

            int NoPsCart = _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_cart (id_shop_group, id_shop, id_carrier, delivery_option, id_lang, id_address_delivery, id_address_invoice, id_currency, id_customer, id_guest, secure_key, recyclable, gift, gift_message, mobile_theme, allow_seperated_package, date_add, date_upd) VALUES (1, 1, "+deliveryType+", 'a:1:{i:"+addressDeliveryId+";s:3:\""+deliveryType+",\";}', 1, "+addressDeliveryId+", "+addressInvoiceId+", 1, "+customerLastId+", 9999, '"+secure_key+"', 0, 0, '', 0, 0, NOW(), NOW())");
            int cartId = _contextSp24.Carts_sp24.Last().Id_cart;

            _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_oav_order_ip_log (id_cart, ip) VALUES ('"+cartId+"','200.200.200.200') ");
            _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_oav_ip_info (ip, info, lastdate) VALUES ('200.200.200.200','CLUTCHLIT','1477559381')");

            int NoPsOrders = _contextSp24.Database.ExecuteSqlCommand("INSERT INTO ps_orders (reference, id_shop_group, id_shop, id_carrier, id_lang, id_customer, id_cart, id_currency, id_address_delivery, id_address_invoice, current_state, secure_key, payment, conversion_rate, module, recyclable, gift, gift_message, mobile_theme, shipping_number, total_discounts, total_discounts_tax_incl, total_discounts_tax_excl, total_paid, total_paid_tax_incl, total_paid_tax_excl, total_paid_real, total_products, total_products_wt, total_shipping, total_shipping_tax_incl, total_shipping_tax_excl, carrier_tax_rate, total_wrapping, total_wrapping_tax_incl, total_wrapping_tax_excl, round_mode, round_type, invoice_number, delivery_number, invoice_date, delivery_date, valid, date_add, date_upd, dhl_lp, dhl_shipment_id) VALUES ('"+reference+"', 1, 1, '"+deliveryType+"', 1, '"+customerLastId+"', '"+cartId+"', 1, '"+addressDeliveryId+"', '"+addressInvoiceId+"', '10', '"+secure_key+"', '"+deliveryTypeS+"', 1.00, '"+module+"', 0, 0, '', 0, '', 0.00, 0.00, 0.00, '"+totalPaid+"', '"+totalPaidTaxIncl+"', '"+totalPaidTaxExcl+"', '"+totalPaidReal+"', '"+totalProducts+"', '"+totalProductsWt+"', '"+totalShipping+"', '"+totalShippingTaxIncl+"', '"+totalShippingTaxExcl+"', '"+CarrierTaxRate+ "', '0.00', '0.00', '0.00', 2, 2, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 1, NOW(), NOW(), NULL,NULL) ");
            
            // sprzegla24
            return Json(customerLastId);
        }
    }
}