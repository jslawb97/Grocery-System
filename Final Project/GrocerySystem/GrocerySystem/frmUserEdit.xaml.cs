using DataTransferObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GrocerySystem
{
    /// <summary>
    /// Interaction logic for frmUserEdit.xaml
    /// </summary>
    public partial class frmUserEdit : Window
    {
        UserManager _userManager;
        CustomerManager _customerManager;
        Customer _customer;
        Employee _employee;
        ItemDetailForm _type;
        UserDetailForm _user;

        // Creates the form to edit for a customer
        public frmUserEdit(CustomerManager csMgr, Customer customer, ItemDetailForm type, UserDetailForm user)
        {
            _customerManager = csMgr;
            _customer = customer;
            _type = type;
            _user = user;
            InitializeComponent();
        }

        // Creates the form to edit for a employee
        public frmUserEdit(UserManager usMgr, Employee employee, ItemDetailForm type)
        {
            _userManager = usMgr;
            _employee = employee;
            _type = type;
            _user = UserDetailForm.Employee;
            InitializeComponent();
        }

        // Creates the form to add a customer
        public frmUserEdit(CustomerManager csMgr, UserDetailForm user)
        {
            _customerManager = csMgr;
            _type = ItemDetailForm.Add;
            _user = UserDetailForm.Customer;
            InitializeComponent();
        }

        // Creates the form to add an employee
        public frmUserEdit(UserManager usMgr)
        {
            _userManager = usMgr;
            _type = ItemDetailForm.Add;
            _user = UserDetailForm.Employee;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_user)
            {
                case UserDetailForm.Customer:
                    switch (_type)
                    {
                        case ItemDetailForm.Add:
                            setupCustomerAdd();
                            break;
                        case ItemDetailForm.Edit:
                            setupCustomerEdit();
                            break;
                        case ItemDetailForm.View:
                            setupCustomerView();
                            break;
                        default:
                            break;
                    }
                    break;
                case UserDetailForm.Employee:
                    switch (_type)
                    {
                        case ItemDetailForm.Add:
                            setupEmployeeAdd();
                            break;
                        case ItemDetailForm.Edit:
                            setupEmployeeEdit();
                            break;
                        case ItemDetailForm.View:
                            setupEmployeeView();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            this.txtID.IsEnabled = false;
        }

        private void setupEmployeeAdd()
        {
            this.Title = "Add Employee Record";
            setControls(false);
        }

        private void setupCustomerAdd()
        {
            this.Title = "Add Customer Record";
            setControls(false);
        }

        private void setupEmployeeEdit()
        {
            this.Title = "Edit Employee Record";
            populateEmployeeControls();
            setControls(false);
        }

        private void setupCustomerEdit()
        {
            this.Title = "Edit Customer Record";
            populateCustomerControls();
            setControls(false);
        }

        private void setupEmployeeView()
        {
            this.btnSave.IsEnabled = false;
            this.Title = "Employee Details";
            populateEmployeeControls();
            setControls(true);

        }

        private void setupCustomerView()
        {
            this.btnSave.IsEnabled = false;
            this.Title = "Customer Details";
            populateCustomerControls();
            setControls(true);
        }

        // Fill the employee controls with the selected row
        private void populateEmployeeControls()
        {
            this.txtID.Text = _employee.EmployeeID;
            this.txtFirstName.Text = _employee.FirstName;
            this.txtLastName.Text = _employee.LastName;
            this.txtPhoneNumber.Text = _employee.PhoneNumber;
            this.txtEmail.Text = _employee.Email;
        }

        // Fill the customer controls with the selected row
        private void populateCustomerControls()
        {
            this.txtID.Text = _customer.CustomerID.ToString();
            this.txtFirstName.Text = _customer.FirstName;
            this.txtLastName.Text = _customer.LastName;
            this.txtPhoneNumber.Text = _customer.PhoneNumber;
            this.txtEmail.Text = _customer.Email;
        }

        // Closes the window when clicked
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        // Sets the controls to true or false
        private void setControls(bool readOnly = true)
        {
            this.txtID.IsReadOnly = readOnly;
            this.txtFirstName.IsReadOnly = readOnly;
            this.txtLastName.IsReadOnly = readOnly;
            this.txtPhoneNumber.IsReadOnly = readOnly;
            this.txtEmail.IsReadOnly = readOnly;
        }

        // Decide whether it's a customer or employee in add or edit mode
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer();
            var employee = new Employee();
            switch (_user)
            {
                // If it's a customer
                case UserDetailForm.Customer:
                    switch (_type)
                    {
                        // In add mode
                        case ItemDetailForm.Add:
                            if (captureCustomer(customer)==false)
                            {
                                return;
                            }

                            try
                            {
                                if (_customerManager.SaveNewCustomer(customer))
                                {
                                    this.DialogResult = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                            // In edit mode
                        case ItemDetailForm.Edit:
                            if (captureCustomer(customer)==false)
                            {
                                return;
                            }
                            customer.CustomerID = _customer.CustomerID;
                            var oldCustomer = _customer;
                            try
                            {
                                if (_customerManager.EditCustomer(customer, oldCustomer))
                                {
                                    this.DialogResult = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                // If it's an employee
                case UserDetailForm.Employee:
                    switch (_type)
                    {
                        // In add mode
                        case ItemDetailForm.Add:
                            if (captureEmployee(employee) == false)
                            {
                                return;
                            }

                            try
                            {
                                if (_userManager.SaveNewEmployee(employee))
                                {
                                    this.DialogResult = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                        // In edit mode
                        case ItemDetailForm.Edit:
                            if (captureEmployee(employee) == false)
                            {
                                return;
                            }
                            employee.EmployeeID = _employee.EmployeeID;
                            var oldEmployee = _employee;
                            try
                            {
                                if (_userManager.EditEmployee(employee, oldEmployee))
                                {
                                    this.DialogResult = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        // Checks the inputs for a customer
        private bool captureCustomer(Customer customer)
        {
            if (this.txtFirstName.Text == "")
            {
                MessageBox.Show("You must enter a first name.");
                return false;
            }
            else
            {
                customer.FirstName = txtFirstName.Text;
            }
            if (this.txtLastName.Text == "")
            {
                MessageBox.Show("You must enter a last name.");
                return false;
            }
            else
            {
                customer.LastName = txtLastName.Text;
            }
            if (this.txtPhoneNumber.Text == "")
            {
                MessageBox.Show("You must enter a phone number.");
                return false;
            }
            else
            {
                customer.PhoneNumber = txtPhoneNumber.Text;
            }
            if (this.txtEmail.Text == "")
            {
                MessageBox.Show("You must enter a customer email.");
                return false;
            }
            else
            {
                customer.Email = txtEmail.Text;
            }
            return true;
        }

        // Checks the inputs for an employee
        private bool captureEmployee(Employee employee)
        {
            if (this.txtFirstName.Text == "")
            {
                MessageBox.Show("You must enter a first name.");
                return false;
            }
            else
            {
                employee.FirstName = txtFirstName.Text;
            }
            if (this.txtLastName.Text == "")
            {
                MessageBox.Show("You must enter a last name.");
                return false;
            }
            else
            {
                employee.LastName = txtLastName.Text;
            }
            if (this.txtPhoneNumber.Text == "")
            {
                MessageBox.Show("You must enter a phone number.");
                return false;
            }
            else
            {
                employee.PhoneNumber = txtPhoneNumber.Text;
            }
            if (this.txtEmail.Text == "")
            {
                MessageBox.Show("You must enter an employee email.");
                return false;
            }
            else
            {
                employee.Email = txtEmail.Text;
            }
            return true;
        }
    }
}
