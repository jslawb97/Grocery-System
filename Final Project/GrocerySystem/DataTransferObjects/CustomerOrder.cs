using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerOrder
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int DepartmentID { get; set; }
        public string Description { get; set; }
        public string PickupDate { get; set; }
        public bool Active { get; set; }
    }
}
