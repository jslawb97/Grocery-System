using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;

namespace LogicLayer
{
    public class DepartmentManager
    {
        // Retrieve Department List
        public List<Department> RetrieveDepartmentList(bool active = true)
        {
            List<Department> departmentList = null;

            try
            {
                departmentList = DepartmentAccessor.RetrieveDepartmentByActive(active);
            }
            catch (Exception)
            {
                throw;
            }

            return departmentList;
        }

        // Add New Department
        public bool CreateNewDepartment(Department department)
        {
            var result = false;

            if (department.Name == null || department.Description == null)
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != DepartmentAccessor.CreateDepartment(department));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Edit Department
        public bool EditDepartment(Department department, Department oldDepartment)
        {
            var result = false;

            if (department.Name == null || department.DepartmentID == null)
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != DepartmentAccessor.EditDepartment(department, oldDepartment));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Deactivate Department
        public bool DeactivateDepartment(int id)
        {
            bool result = false;

            try
            {
                result = (1 == DepartmentAccessor.DeactivateDepartment(id));
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
