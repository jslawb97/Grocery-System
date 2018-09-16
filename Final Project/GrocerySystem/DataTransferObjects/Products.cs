using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Products
    {
        public int UPC { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int OnHand { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }

    }
}
