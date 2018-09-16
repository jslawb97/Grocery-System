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
    public class UserAccessor
    {
        // Verify an employees username and password
        public static int VerifyUsernameAndPassword(string username, string passwordHash)
        {
            var result = 0; // return the number of rows found

            // Get the database connection
            var conn = DBConnection.GetDBConnection();

            // Get the stored procedure name
            var cmdText = @"sp_authenticate_user";

            // Use the connection and command
            var cmd = new SqlCommand(cmdText, conn);

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // set the parameter values
            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        // Retrieve the Employee by username (email)
        public static Employee RetrieveEmployeeByUsername(string username)
        {
            Employee employee = null;

            // connection first
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_employee_by_email";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // parameeter values
            cmd.Parameters["@Email"].Value = username;

            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    // create a new employee object
                    employee = new Employee()
                    {
                        EmployeeID = reader.GetString(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        Email = reader.GetString(4),
                        Active = reader.GetBoolean(5)
                    };
                    if (employee.Active != true)
                    {
                        throw new ApplicationException("Not an active employee.");
                    }
                }
                else
                {
                    throw new ApplicationException("Employee record not found!");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }


            return employee;
        }

        // Retrieve the Employee Roles by ID
        public static List<Title> RetrieveRolesByEmployeeID(string employeeID)
        {
            List<Title> titles = new List<Title>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // cmdText 
            var cmdText = @"sp_retrieve_employee_titles";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@EmployeeID", SqlDbType.NVarChar, 20);

            // paramter values
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // check for results
                if (reader.HasRows)
                {
                    // Multiple Rows?
                    while (reader.Read())
                    {
                        // create role object
                        var title = new Title()
                        {
                            TitleID = reader.GetString(0)
                        };
                        // add the tole to the list
                        titles.Add(title);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return titles;
        }

        // Add a new employee record (Does not work because of the EmployeeID issue)
        public static string InsertEmployee(Employee employee)
        {
            string newId = null;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_employee";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // New parameters
            cmd.Parameters.AddWithValue("@EmployeeID", "Employee008");
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", employee.Email);

            try
            {
                conn.Open();
                newId = (string)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }

            return newId;
        }

        // Deactivate an employee by calling a stored procedure
        public static int DeactivateEmployee(string employeeID)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_deactivate_employee";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

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

        // Update an employee record by calling the stored procedure
        public static int UpdateEmployee(Employee employee, Employee oldEmployee)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_employee";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Set the new parameters
            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", employee.Email);

            // Set the old parameters
            cmd.Parameters.AddWithValue("@OldFirstName", oldEmployee.FirstName);
            cmd.Parameters.AddWithValue("@OldLastName", oldEmployee.LastName);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", oldEmployee.PhoneNumber);
            cmd.Parameters.AddWithValue("@OldEmail", oldEmployee.Email);

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

        // Update a password with HashSha
        public static int UpdatePasswordHash(string employeeID,
            string oldPasswordHash, string newPasswordHash)
        {
            int result = 0;

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_update_passwordHash";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@EmployeeID", SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            // parameter values
            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            // try-catch
            try
            {
                // open the connection
                conn.Open();

                // execute the command
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    throw new ApplicationException("Password update failed.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        // Retrieve all employees that are active
        public static List<Employee> RetrieveEmployeesByActive(bool active = true)
        {
            var employeeList = new List<Employee>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_employee_by_active";

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
                        var emplo = new Employee()
                        {
                            EmployeeID = reader.GetString(0),
                            PhoneNumber = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Email = reader.GetString(4),
                            Active = reader.GetBoolean(5)
                        };
                        employeeList.Add(emplo);
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
            return employeeList;
        }
    }
}
