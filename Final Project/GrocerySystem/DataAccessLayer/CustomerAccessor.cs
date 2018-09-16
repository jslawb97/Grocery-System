using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;

namespace DataAccessLayer
{
    public static class CustomerAccessor
    {
        // Retrieve all active customers
        public static List<Customer> RetrieveCustomersByActive(bool active = true)
        {
            var customerList = new List<Customer>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_customers_by_active";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@Active", active);

            try
            {
                // open connection
                conn.Open();
                // execute command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var cstmr = new Customer()
                        {
                            CustomerID = reader.GetInt32(0),
                            PhoneNumber = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Email = reader.GetString(4),
                            Active = reader.GetBoolean(5)
                        };
                        customerList.Add(cstmr);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }
            return customerList;
        }

        // Deactivate a customer
        public static int DeactivateCustomer(int customerID)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_deactivate_customer";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rows;
        }

        // Update a customer record
        public static int UpdateCustomer(Customer customer, Customer oldCustomer)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_customer";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", customer.Email);

            cmd.Parameters.AddWithValue("@OldFirstName", oldCustomer.FirstName);
            cmd.Parameters.AddWithValue("@OldLastName", oldCustomer.LastName);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", oldCustomer.PhoneNumber);
            cmd.Parameters.AddWithValue("@OldEmail", oldCustomer.Email);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            return rows;
        }

        // Add a new customer
        public static int InsertCustomer(Customer customer)
        {
            int newId = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_customer";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", customer.Email);

            try
            {
                conn.Open();
                decimal id = (decimal)cmd.ExecuteScalar();
                newId = (int)id;
            }
            catch (Exception)
            {
                throw;
            }

            return newId;
        }
    }
}
