using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class ManuModelView
    {
        public List<Manufacturer> manu { get; set; }
        public IEnumerable<Model> mod { get; set; }
        public IEnumerable<Product> prod { get; set; }
        public IEnumerable<PassengerCar> pc { get; set; }


        public ManuModelView()
        {
            this.manu = new List<Manufacturer>();
            this.mod = new List<Model>();
            this.prod = new List<Product>();
            this.pc = new List<PassengerCar>();
        }
    }
}
