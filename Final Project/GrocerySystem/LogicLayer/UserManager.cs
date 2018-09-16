using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class UserManager
    {
        // Authenticate the User
        public User AuthenticateUser(string username, string password)
        {
            User user = null;

            // hash the password
            var passwordHash = HashSha256(password);

            try
            {
                // validate the username and password
                var validationResult = UserAccessor.VerifyUsernameAndPassword(username, passwordHash);

                if (validationResult == 1) // user is validated
                {
                    // get the employee
                    var employee = UserAccessor.RetrieveEmployeeByUsername(username);

                    // get the employee's roles
                    var titles = UserAccessor.RetrieveRolesByEmployeeID(employee.EmployeeID);



                    bool passwordMustBeChanged = false;

                    if (password == "newuser")
                    {
                        passwordMustBeChanged = true;
                        titles.Clear(); // clear the user's roles
                        titles.Add(new Title() { TitleID = "New User" });
                    }

                    // create the user token
                    user = new User(employee, titles, passwordMustBeChanged);

                }
                else // user was not validated
                {
                    throw new ApplicationException("Login failed. Bad username (email address) or password");
                }
            }
            catch (ApplicationException) 
            {
                throw;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("There was a problem connecting to the server.", ex);
            }

            return user;
        }

        // SHA 256 Function
        private string HashSha256(string source)
        {
            var result = "";

            // create a byte array
            byte[] data;

            // create a .NET Hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            var s = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString();

            return result;
        }

        // Update Password
        public User UpdatePassword(User user, string oldPassword, string newPassword)
        {
            User newUser = null;
            int rowsAffected = 0;

            string oldPasswordHash = HashSha256(oldPassword);
            string newPasswordHash = HashSha256(newPassword);

            try
            {
                rowsAffected = UserAccessor.UpdatePasswordHash(user.Employee.EmployeeID,
                    oldPasswordHash, newPasswordHash);
                if (rowsAffected == 1)
                {
                    if (user.Titles[0].TitleID == "New User")
                    {
                        // get the roles and create a new user
                        var titles = UserAccessor.RetrieveRolesByEmployeeID(user.Employee.EmployeeID);
                        newUser = new User(user.Employee, titles);
                    }
                    else
                    {
                        newUser = user;
                    }
                }
                else
                {
                    throw new ApplicationException("Update returned 0 rows affected.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Password change failed.", ex);
            }

            return newUser;
        }

        // Retrieve Employee List
        public List<Employee> RetrieveEmployeeList(bool active = true)
        {
            List<Employee> employeeList = null;

            try
            {
                employeeList = UserAccessor.RetrieveEmployeesByActive(active);
            }
            catch (Exception)
            {
                throw;
            }

            return employeeList;
        }

        // Add Employee (Not Working)
        public bool SaveNewEmployee(Employee employee)
        {
            var result = false;

            if (employee.Email == "" || employee.FirstName == "" || employee.LastName == "" || employee.PhoneNumber == "")
            {
                throw new ApplicationException("You must fill out all the fields.");
            }
            try
            {
                result = (null != UserAccessor.InsertEmployee(employee));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Edit Employee
        public bool EditEmployee(Employee employee, Employee oldEmployee)
        {
            var result = false;

            if (employee.Email == "" || employee.FirstName == "" || employee.LastName == "" || employee.PhoneNumber == "")
            {
                throw new ApplicationException("You must fill out all the fields.");
            }
            try
            {
                result = (0 != UserAccessor.UpdateEmployee(employee, oldEmployee));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Deactivate Employee
        public bool DeactivateEmployee(string id)
        {
            bool result = false;

            try
            {
                result = (1 == UserAccessor.DeactivateEmployee(id));
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }
}
