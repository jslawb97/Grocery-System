using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;

namespace LogicLayer
{
    public class CustomerOrderManager
    {
        // Retrieve Orders
        public List<CustomerOrder> RetrieveOrdersList(bool active = true)
        {
            List<CustomerOrder> ordersList = null;

            try
            {
                ordersList = CustomerOrderAccessor.RetrieveOrdersByActive(active);
            }
            catch (Exception)
            {
                throw;
            }

            return ordersList;
        }

        // Retrieve Customer Orders
        public List<CustomerOrder> RetrieveOrdersByCustomerID(int customerID)
        {
            List<CustomerOrder> orders = null;

            try
            {
                orders = CustomerOrderAccessor.RetrieveOrdersByCustomerID(customerID);

            }
            catch (Exception)
            {
                throw;
            }

            return orders;
        }

        public List<CustomerOrder> RetrieveCompletedOrdersList()
        {
            List<CustomerOrder> orders = null;

            try
            {
                orders = CustomerOrderAccessor.RetrieveCompletedOrders();

            }
            catch (Exception)
            {
                throw;
            }

            return orders;
        }

        // Add New Order
        public bool SaveNewOrder(CustomerOrder order)
        {
            var result = false;

            if (order.CustomerID == null || order.DepartmentID == null || order.Description == "" || order.PickupDate == "")
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != CustomerOrderAccessor.AddNewOrder(order));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Edit Order
        public bool EditOrder(CustomerOrder order, CustomerOrder oldOrder)
        {
            var result = false;

            if (order.CustomerID == null || order.DepartmentID == null || order.Description == "" || order.PickupDate == "")
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != CustomerOrderAccessor.UpdateOrder(order, oldOrder));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Deactivate Order
        public bool DeactivateOrder(int id)
        {
            bool result = false;

            try
            {
                result = (1 == CustomerOrderAccessor.DeactivateOrder(id));
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        // Deactivate Order
        public bool DeleteOrder(int id)
        {
            bool result = false;

            try
            {
                result = (1 == CustomerOrderAccessor.DeleteOrder(id));
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
