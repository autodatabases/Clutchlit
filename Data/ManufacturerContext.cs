using Clutchlit.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Data
{
    public class ManufacturerContext
    {
        public string ConnectionString { get; set; }

        public ManufacturerContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
         
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public List<Manufacturer> GetAllManufacturers()
        {
            List<Manufacturer> list = new List<Manufacturer>();

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cl_manufacturers", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Manufacturer manu = new Manufacturer(Convert.ToInt32(reader["manufacturer_id"]),Convert.ToInt32(reader["manufacturer_tecdoc_id"]),reader["manufacturer_name"].ToString());
                        list.Add(manu);
                    }
                }
            }
            
            return list;
        }

        public List<Model> GetAllModels(int id)
        {
            return new List<Model>();
        }

        public List<PassengerCar> GetAllEngines(int id)
        {
            List<PassengerCar> list = new List<PassengerCar>();
            
            return list;
        }
    }
}
