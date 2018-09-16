using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;

namespace LogicLayer
{
    public class CustomerManager
    {
        // Retrieve Customer List
        public List<Customer> RetrieveCustomerList(bool active = true)
        {
            List<Customer> customerList = null;

            try
            {
                customerList = CustomerAccessor.RetrieveCustomersByActive(active);
            }
            catch (Exception)
            {
                throw;
            }

            return customerList;
        }

        // Add New Customer
        public bool SaveNewCustomer(Customer customer)
        {
            var result = false;

            if (customer.Email == "" || customer.FirstName == "" || customer.LastName == "" || customer.PhoneNumber == "")
            {
                throw new ApplicationException("You must fill out all the fields.");
            }
            try
            {
                result = (0 != CustomerAccessor.InsertCustomer(customer));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Update Customer Profile
        public bool EditCustomer(Customer customer, Customer oldCustomer)
        {
            var result = false;

            if (customer.Email == "" || customer.FirstName == "" || customer.LastName == "" || customer.PhoneNumber == "")
            {
                throw new ApplicationException("You must fill out all the fields.");
            }
            try
            {
                result = (0 != CustomerAccessor.UpdateCustomer(customer, oldCustomer));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Deactivate Customer
        public bool DeactivateCustomer(int id)
        {
            bool result = false;

            try
            {
                result = (1 == CustomerAccessor.DeactivateCustomer(id));
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }
}
