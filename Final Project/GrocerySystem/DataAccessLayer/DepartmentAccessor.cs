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
    public static class DepartmentAccessor
    {
        // Retrieve all active departments
        public static List<Department> RetrieveDepartmentByActive(bool active = true)
        {
            var departmentList = new List<Department>();
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_select_department_by_active";
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
                        var dpt = new Department()
                        {
                            DepartmentID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };
                        departmentList.Add(dpt);
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
            return departmentList;
        }

        public static int EditDepartment(Department department, Department oldDepartment)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_department";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // New parameters
            cmd.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);
            cmd.Parameters.AddWithValue("@Name", department.Name);
            cmd.Parameters.AddWithValue("@Description", department.Description);

            // Old parameters
            cmd.Parameters.AddWithValue("@OldName", oldDepartment.Name);
            cmd.Parameters.AddWithValue("@OldDescription", oldDepartment.Description);


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

        public static int CreateDepartment(Department department)
        {
            int newID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_department";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", department.Name);
            cmd.Parameters.AddWithValue("@Description", department.Description);

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

        public static int DeactivateDepartment(int id)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_deactive_department";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentID", id);

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
    }
}
