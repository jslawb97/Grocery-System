using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class User
    {
        public Employee Employee { get; private set; }
        public List<Title> Titles { get; private set; }

        public bool PasswordMustBeChanged { get; private set; }
        public User(Employee employee, List<Title> titles, bool passwordMustBeChanged = false)
        {
            this.Employee = employee;
            this.Titles = titles;
            this.PasswordMustBeChanged = passwordMustBeChanged;
        }
    }
}
