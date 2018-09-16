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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LogicLayer;
using DataTransferObjects;

namespace GrocerySystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserManager _userManager = new UserManager();
        private ProductManager _itemManager = new ProductManager();
        private CustomerManager _customerManager = new CustomerManager();
        private List<Department> _departmentList;
        private List<Products> _productsList;
        private List<Customer> _customerList;
        private List<CustomerOrder> _ordersList;
        private List<Employee> _employeeList;
        private Products _products;

        private User _user = null;

        private const int MIN_PASSWORD_LENGTH = 5;  // business rule
        private const int MIN_USERNAME_LENGTH = 8;  // forced by naming rules
        private const int MAX_USERNAME_LENGTH = 100;// forced by db field length        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null) // this means someone is logged in, so log out
            {
                logout();
                return;
            }

            // accept the input
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            // check for missing or invalid data
            if (username.Length < MIN_USERNAME_LENGTH ||
                username.Length > MAX_USERNAME_LENGTH)
            {
                MessageBox.Show("Invalid Username", "Login Failed.",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                clearLogin();

                return;
            }
            if (password.Length <= MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show("Invalid Password", "Login Failed.",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                clearLogin();
                return;
            }

            // before checking for the user token, we need to use a try block
            try
            {
                _user = _userManager.AuthenticateUser(username, password);

                if (_user.Titles.Count == 0)
                {
                    // check for unauthorized user
                    _user = null;

                    MessageBox.Show("You have not been assigned a title. \nPlease talk to your manager.",
                        "Unauthorized Employee", MessageBoxButton.OK,
                        MessageBoxImage.Stop);

                    clearLogin();

                    return;
                }
                // user is now logged in
                var message = "Welcome " + _user.Employee.FirstName +
                    ", You are signed in as ";
                foreach (var r in _user.Titles)
                {
                    message += r.TitleID + ".";
                }

                pictureBox1.Visibility = Visibility.Hidden;
                showUserTabs();
                statusMain.Items[0] = message;

                clearLogin();
                txtPassword.Visibility = Visibility.Hidden;
                txtUsername.Visibility = Visibility.Hidden;
                lblPassword.Visibility = Visibility.Hidden;
                lblUsername.Visibility = Visibility.Hidden;

                // Change the login button to logout
                this.btnLogin.IsDefault = false;
                btnLogin.Content = "Log Out";

                // check for expired password
                if (_user.PasswordMustBeChanged)
                {
                    changePassword();
                }
            }
            catch (Exception ex) // nowhere to throw an exception at the presentation layer
            {
                string message = ex.Message;

                if (ex.InnerException != null)
                {
                    message += "\n\n" + ex.InnerException.Message;
                }

                MessageBox.Show(message, "Login Failed!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                clearLogin();
                return;
            }
        }

        private void logout()
        {
            _user = null;

            // reenable the login controls
            txtPassword.Visibility = Visibility.Visible;
            txtUsername.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            lblUsername.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            clearLogin();
            statusMain.Items[0] = "Please Log Into Your Account.";

            pictureBox1.Visibility = Visibility.Visible;

            hideAllTabs();
        }

        private void changePassword()
        {
            // we need to capture a new password from the user.
            var passwordChangeWindow = new frmUpdatePassword(_userManager, _user);
            var result = passwordChangeWindow.ShowDialog();

            if (result == true)
            {
                if (_user.Titles[0].TitleID == "New User")
                {
                    // Logout
                    logout();
                    MessageBox.Show("Please log back in with your new password.");
                    return;
                }
                else // Does not need to log back in
                {
                    MessageBox.Show("Password Updated.");
                }
            }
            else
            {
                logout();
                MessageBox.Show("Password change cancelled. You have been logged out.");
            }
        }

        private void clearLogin()
        {
            this.btnLogin.IsDefault = true;
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtUsername.Focus();
        }

        private void hideAllTabs()
        {
            foreach (var tab in tabsetMain.Items)
            {
                // collapse all the tabs
                ((TabItem)tab).Visibility = Visibility.Collapsed;
                // hide the tabset completely
                tabsetMain.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
            this.btnLogin.IsDefault = true;
            
            hideAllTabs();

            refreshProductList();
        }

        private void refreshProductList(bool active = true)
        {
            try
            {
                _departmentList = _itemManager.RetrieveDepartmentList(active);
                _customerList = _customerManager.RetrieveCustomerList(active);
                _ordersList = _itemManager.RetrieveOrdersList(active);
                _employeeList = _userManager.RetrieveEmployeeList(active);
                
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Data Retrieval Error!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnMeatDpt_Click(object sender, RoutedEventArgs e)
        {
            int department = 100000;
            selectDepartmentItems(department);
        }

        private void selectDepartmentItems(int department)
        {
            try
            {
                _productsList = _itemManager.RetrieveProductsListByDepartment(department);
                grInventory.ItemsSource = _productsList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Data Retrieval Error!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        // Show product details
        private void btnItemDetails_Click(object sender, RoutedEventArgs e)
        {
            Products product = null;
            Products prdt = null;
            if (this.grInventory.SelectedItems.Count > 0)
            {
                product = (Products)this.grInventory.SelectedItem;
                try
                {
                    prdt = _itemManager.RetrieveProductDetail(product);
                    var detailForm = new frmItemEdit(_itemManager, prdt, ItemDetailForm.View);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grInventory.ItemsSource = _productsList;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select a product.");
            }
        }
    
        // Show department table
        private void grDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            grDepartment.ItemsSource = _departmentList;
        }

        // Does nothing
        private void tabInventory_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        // show all products
        private void btnAllDpt_Click(object sender, RoutedEventArgs e)
        {
            bool active = true;
            try
            {
                _productsList = _itemManager.RetrieveProductsList(active);
                grInventory.ItemsSource = _productsList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Data Retrieval Error!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        // Show only produce products
        private void btnProduceDpt_Click(object sender, RoutedEventArgs e)
        {
            int department = 100001;
            selectDepartmentItems(department);
        }

        // Show only dairy products
        private void btnDairyDpt_Click(object sender, RoutedEventArgs e)
        {
            int department = 100002;
            selectDepartmentItems(department);
        }

        // Show only frozen products
        private void btnFrozenDpt_Click(object sender, RoutedEventArgs e)
        {
            int department = 100003;
            selectDepartmentItems(department);
        }

        // Show customer info table
        private void tabCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            grCustomers.ItemsSource = _customerList;
        }

        // Show customerorder table
        private void tabCustomerOrder_GotFocus(object sender, RoutedEventArgs e)
        {
            grCustomerOrders.ItemsSource = _ordersList;
        }

        // Show employee table
        private void tabEmployeeInfo_GotFocus(object sender, RoutedEventArgs e)
        {
            grEmployeeInfo.ItemsSource = _employeeList;
        }

        // Edit an item
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            Products product = null;
            Products prdt = null;
            if (this.grInventory.SelectedItems.Count > 0)
            {
                product = (Products)this.grInventory.SelectedItem;
                try
                {
                    prdt = _itemManager.RetrieveProductDetail(product);
                    var detailForm = new frmItemEdit(_itemManager, prdt, ItemDetailForm.Edit);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grInventory.ItemsSource = _productsList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Error.");
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select a product.");
            }
        }

        // Add a new item
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var detailForm = new frmItemEdit(_itemManager);
            var result = detailForm.ShowDialog();
            if (result == true)
            {
                refreshProductList();
            }
        }

        // Discontinue an item
        private void btnDiscontinueItem_Click(object sender, RoutedEventArgs e)
        {
            if (grInventory.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product.");
                return;
            }
            else
            {
                var result = MessageBox.Show("Are you sure you want to discountinue " + ((Products)(grInventory.SelectedItem)).Name + "?", "Deactivate Equipment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _itemManager.DeactivateProduct((Products)grInventory.SelectedItem);
                    refreshProductList();
                }
            }
        }

        // Show only dry grocery items
        private void btnGroceryDpt_Click(object sender, RoutedEventArgs e)
        {
            int department = 100004;
            selectDepartmentItems(department);
        }

        // Show only HBC items
        private void btnHBC_Click(object sender, RoutedEventArgs e)
        {
            int department = 100005;
            selectDepartmentItems(department);
        }

        // Show customer details
        private void btnCustomerDetails_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            Customer cstmr = null;
            if (this.grCustomers.SelectedItems.Count > 0)
            {
                customer = (Customer)this.grCustomers.SelectedItem;
                try
                {
                    var detailForm = new frmUserEdit(_customerManager, customer, ItemDetailForm.View, UserDetailForm.Customer);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grCustomers.ItemsSource = _customerList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select a customer.");
            }
        }

        // Show employee details
        private void btnEmployeeDetails_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = null;
            if (this.grEmployeeInfo.SelectedItems.Count > 0)
            {
                employee = (Employee)this.grEmployeeInfo.SelectedItem;
                try
                {
                    var detailForm = new frmUserEdit(_userManager, employee, ItemDetailForm.View);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grEmployeeInfo.ItemsSource = _employeeList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        // Update a customer
        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (this.grCustomers.SelectedItems.Count > 0)
            {
                customer = (Customer)this.grCustomers.SelectedItem;
                try
                {
                    var detailForm = new frmUserEdit(_customerManager, customer, ItemDetailForm.Edit, UserDetailForm.Customer);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grCustomers.ItemsSource = _customerList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select a customer.");
            }
        }

        // Add a customer
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var detailForm = new frmUserEdit(_customerManager, UserDetailForm.Customer);
            var result = detailForm.ShowDialog();
            if (result == true)
            {
                refreshProductList();
            }
        }

        // Deactivate a customer
        private void btnDeactivateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (grCustomers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }
            else
            {
                var result = MessageBox.Show("Are you sure you want to deactivate " + ((Customer)(grCustomers.SelectedItem)).FirstName + "?", "Deactivate Equipment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _customerManager.DeactivateCustomer((Customer)grCustomers.SelectedItem);
                    refreshProductList();
                }
            }
        }

        // Update an employee
        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = null;
            if (this.grEmployeeInfo.SelectedItems.Count > 0)
            {
                employee = (Employee)this.grEmployeeInfo.SelectedItem;
                try
                {
                    var detailForm = new frmUserEdit(_userManager, employee, ItemDetailForm.Edit);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grEmployeeInfo.ItemsSource = _employeeList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        // Add a new employee
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var detailForm = new frmUserEdit(_userManager);
            var result = detailForm.ShowDialog();
            if (result == true)
            {
                refreshProductList();
            }
        }

        // Deactivate an employee
        private void btnDeactivateEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (grEmployeeInfo.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an employee.");
                return;
            }
            else
            {
                var result = MessageBox.Show("Are you sure you want to deactivate " + ((Employee)(grEmployeeInfo.SelectedItem)).FirstName + "?", "Deactivate Equipment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _userManager.DeactivateEmployee((Employee)grEmployeeInfo.SelectedItem);
                    refreshProductList();
                }
            }
        }

        // Show tabs based on the employee title
        private void showUserTabs()
        {
            foreach (var t in _user.Titles)
            {
                switch (t.TitleID)
                {
                    case "Manager":
                        foreach (var tab in tabsetMain.Items)
                        {
                            ((TabItem)tab).Visibility = Visibility.Visible;
                        }
                        tabInventory.IsSelected = true;
                        btnEditCustomer.IsEnabled = true;
                        btnDeactivateCustomer.IsEnabled = true;
                        btnDiscontinueItem.IsEnabled = true;
                        btnDeactivateEmployee.IsEnabled = true;
                        btnAddEmployee.IsEnabled = true;
                        btnEditEmployee.IsEnabled = true;
                        btnEditCustomer.IsEnabled = true;
                        btnAddCustomer.IsEnabled = true;
                        break;
                    case "Supervisor":
                        foreach (var tab in tabsetMain.Items)
                        {
                            ((TabItem)tab).Visibility = Visibility.Visible;
                        }
                        btnEditCustomer.IsEnabled = false;
                        btnDeactivateCustomer.IsEnabled = false;
                        btnDiscontinueItem.IsEnabled = false;
                        btnDeactivateEmployee.IsEnabled = false;
                        btnAddEmployee.IsEnabled = false;
                        btnEditEmployee.IsEnabled = false;
                        tabInventory.IsSelected = true;
                        break;
                    case "Carryout":
                        tabInventory.Visibility = Visibility.Visible;
                        tabCustomer.Visibility = Visibility.Visible;
                        tabCustomerOrder.Visibility = Visibility.Visible;
                        btnDiscontinueItem.IsEnabled = false;
                        btnEditCustomer.IsEnabled = false;
                        btnAddCustomer.IsEnabled = false;
                        btnDeactivateCustomer.IsEnabled = false;
                        break;
                    case "Checkout":
                        tabInventory.Visibility = Visibility.Visible;
                        tabCustomer.Visibility = Visibility.Visible;
                        tabCustomerOrder.Visibility = Visibility.Visible;
                        btnDiscontinueItem.IsEnabled = false;
                        btnEditCustomer.IsEnabled = false;
                        btnAddCustomer.IsEnabled = false;
                        btnDeactivateCustomer.IsEnabled = false;
                        break;
                }
            }
            tabsetMain.Visibility = Visibility.Visible;
        }

        // Order details
        private void btnOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder customerOrder = null;
            if (this.grCustomerOrders.SelectedItems.Count > 0)
            {
                customerOrder = (CustomerOrder)this.grCustomerOrders.SelectedItem;
                try
                {
                    var detailForm = new frmOrderEdit(_itemManager, customerOrder, ItemDetailForm.View);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grCustomerOrders.ItemsSource = _ordersList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        // Edit an order
        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder customerOrder= null;
            if (this.grCustomerOrders.SelectedItems.Count > 0)
            {
                customerOrder = (CustomerOrder)this.grCustomerOrders.SelectedItem;
                try
                {
                    var detailForm = new frmOrderEdit(_itemManager, customerOrder, ItemDetailForm.Edit);
                    var result = detailForm.ShowDialog();
                    if (result == true)
                    {
                        refreshProductList();
                        grCustomerOrders.ItemsSource = _ordersList;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        // Add a new order
        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var detailForm = new frmOrderEdit(_itemManager);
            var result = detailForm.ShowDialog();
            if (result == true)
            {
                refreshProductList();
            }
        }

        // Deactivate an order
        private void btnDeactivateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (grCustomerOrders.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an order.");
                return;
            }
            else
            {
                var result = MessageBox.Show("Are you sure you want to mark order #" + ((CustomerOrder)(grCustomerOrders.SelectedItem)).OrderID + " as complete?", "Deactivate Equipment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _itemManager.DeactivateOrder((CustomerOrder)grCustomerOrders.SelectedItem);
                    refreshProductList();
                }
            }
        }
    }
}
