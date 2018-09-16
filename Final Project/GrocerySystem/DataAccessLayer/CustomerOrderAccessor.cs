using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class CustomerOrderAccessor
    {
        public static List<CustomerOrder> RetrieveOrdersByCustomerID(int customerID)
        {
            List<CustomerOrder> orders = null;
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_select_orders_by_customer";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var customerOrder = new CustomerOrder()
                        {
                            OrderID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DepartmentID = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            PickupDate = reader.GetString(3),
                            Active = reader.GetBoolean(4)
                        };
                        orders.Add(customerOrder);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was a problem retrieving your data", ex);
            }
            finally
            {
                conn.Close();
            }
            return orders;
        }

        public static List<CustomerOrder> RetrieveCompletedOrders(bool active = false)
        {
            var ordersList = new List<CustomerOrder>();
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_select_deactived_orders";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", active);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var ordr = new CustomerOrder()
                        {
                            OrderID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DepartmentID = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            PickupDate = reader.GetString(4),
                            Active = reader.GetBoolean(5)
                        };
                        ordersList.Add(ordr);
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
            return ordersList;
        }

        public static int DeleteOrder(int id)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_delete_customer_order";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", id);

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

        // Add a new order
        public static int AddNewOrder(CustomerOrder order)
        {
            int newID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_customer_order";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            cmd.Parameters.AddWithValue("@DepartmentID", order.DepartmentID);
            cmd.Parameters.AddWithValue("@Description", order.Description);
            cmd.Parameters.AddWithValue("@PickupDate", order.PickupDate);

            try
            {
                conn.Open();
                decimal id = (decimal)cmd.ExecuteScalar();
                newID = (int)id;
            }
            catch (Exception)
            {
                throw;
            }

            return newID;
        }

        // Deactivate an order
        public static int DeactivateOrder(int orderID)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_deactivate_customer_order";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", orderID);

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

        // Update an order
        public static int UpdateOrder(CustomerOrder order, CustomerOrder oldOrder)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_customer_order";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // New parameters
            cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            cmd.Parameters.AddWithValue("@DepartmentID", order.DepartmentID);
            cmd.Parameters.AddWithValue("@Description", order.Description);
            cmd.Parameters.AddWithValue("@PickupDate", order.PickupDate);

            // Old parameters
            cmd.Parameters.AddWithValue("@OldCustomerID", oldOrder.CustomerID);
            cmd.Parameters.AddWithValue("@OldDepartmentID", oldOrder.DepartmentID);
            cmd.Parameters.AddWithValue("@OldDescription", oldOrder.Description);
            cmd.Parameters.AddWithValue("@OldPickupDate", oldOrder.PickupDate);

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

        // Retrieve all active orders
        public static List<CustomerOrder> RetrieveOrdersByActive(bool active = true)
        {
            var ordersList = new List<CustomerOrder>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_orders_by_active";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@Active", active);

            // try-catch
            try
            {
                // open connection
                conn.Open();
                // execute command
                var reader = cmd.ExecuteReader();

                // rows ?
                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var ordr = new CustomerOrder()
                        {
                            OrderID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DepartmentID = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            PickupDate = reader.GetString(4),
                            Active = reader.GetBoolean(5)
                        };
                        ordersList.Add(ordr);
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
            return ordersList;
        }
    }
}
