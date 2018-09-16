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
    public static class ProductAccessor
    {
        // Retrieve all active products
        public static List<Products> RetrieveProductsByActive (bool active = true)
        {
            var productsList = new List<Products>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_products_by_active";

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
                        var prdcts = new Products()
                        {
                            UPC = reader.GetInt32(0),
                            DepartmentID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Manufacturer = reader.GetString(3),
                            OnHand = reader.GetInt32(4),
                            Cost = reader.GetDecimal(5),
                            Active = reader.GetBoolean(6)
                        };
                        productsList.Add(prdcts);
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
            return productsList;
        }

        // Retrieve products by id
        public static Products RetrieveProductsByID(int uPC)
        {
            Products products = null;

            // connection
            var conn = DBConnection.GetDBConnection();

            // cmdText
            var cmdText = @"sp_select_products_by_ID";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@UPC", uPC);

            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // check for rows
                if (reader.HasRows)
                {
                    reader.Read();

                    products = new Products()
                    {
                        UPC = reader.GetInt32(0),
                        DepartmentID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Manufacturer = reader.GetString(3),
                        OnHand = reader.GetInt32(4),
                        Cost = reader.GetDecimal(5),
                        Active = reader.GetBoolean(6)
                    };
                }
                else
                {
                    throw new ApplicationException("No data found.");
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
            return products;
        }

        // Deactivate a selected product
        public static int DeactivateProduct(int uPC)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_deactivate_product";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UPC", uPC);

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

        // Add a new product
        public static int AddNewProduct(Products product)
        {
            int newID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_product";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DepartmentID", product.DepartmentID);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
            cmd.Parameters.AddWithValue("@OnHand", product.OnHand);
            cmd.Parameters.AddWithValue("@Cost", product.Cost);

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

        // Update a product
        public static int UpdateProduct(Products product, Products oldProduct)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_product";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // New parameters
            cmd.Parameters.AddWithValue("@UPC", product.UPC);
            cmd.Parameters.AddWithValue("@DepartmentID", product.DepartmentID);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
            cmd.Parameters.AddWithValue("@OnHand", product.OnHand);

            // Old parameters
            cmd.Parameters.AddWithValue("@OldDepartmentID", oldProduct.DepartmentID);
            cmd.Parameters.AddWithValue("@OldName", oldProduct.Name);
            cmd.Parameters.AddWithValue("@OldManufacturer", oldProduct.Manufacturer);
            cmd.Parameters.AddWithValue("@OldOnHand", oldProduct.OnHand);

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

        // Retrieve products by department
        public static List<Products> RetrieveProductsByDepartment(int department)
        {
            var productsList = new List<Products>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_products_by_department";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@DepartmentID", department);

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
                        var prdcts = new Products()
                        {
                            UPC = reader.GetInt32(0),
                            DepartmentID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Manufacturer = reader.GetString(3),
                            OnHand = reader.GetInt32(4),
                            Cost = reader.GetDecimal(5),
                            Active = reader.GetBoolean(6)
                        };
                        productsList.Add(prdcts);
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
            return productsList;
        }

        
    }
}
