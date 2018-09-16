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
    /// Interaction logic for frmOrderEdit.xaml
    /// </summary>
    public partial class frmOrderEdit : Window
    {
        private ProductManager _itemManager;
        private CustomerOrder _order;
        private ItemDetailForm _type;

        // Edit or view mode
        public frmOrderEdit(ProductManager itMgr, CustomerOrder order, ItemDetailForm type)
        {
            _itemManager = itMgr;
            _order = order;
            _type = type;
            InitializeComponent();
        }

        // Add mode
        public frmOrderEdit(ProductManager itMgr)
        {
            _itemManager = itMgr;
            _type = ItemDetailForm.Add;
            InitializeComponent();
        }

        // Close the window
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        // Decide what to set the window for (view, edit, add)
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_type)  // how should we set up the form?
            {
                case ItemDetailForm.Add:
                    setupAddForm();
                    break;
                case ItemDetailForm.Edit:
                    setupEditForm();
                    break;
                case ItemDetailForm.View:
                    setupViewForm();
                    break;
                default:
                    break;
            }
            this.txtID.IsEnabled = false;
        }

        private void setupViewForm()
        {
            this.Title = "Order View Mode";
            btnSave.Visibility = System.Windows.Visibility.Hidden;
            setControls(readOnly: true);
            populateControls();
        }

        private void setupEditForm()
        {
            this.Title = "Order Edit Mode";

            setControls(readOnly: false);
            populateControls();
        }

        private void setupAddForm()
        {
            this.Title = "Add New Order";
        }

        // Fill the controls with data
        private void populateControls()
        {
            try
            {
                this.txtID.Text = _order.OrderID.ToString();
                this.txtCustomer.Text = _order.CustomerID.ToString();
                this.txtDepartment.Text = _order.DepartmentID.ToString();
                this.txtDescription.Text = _order.Description;
                this.txtPickup.Text = _order.PickupDate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Set IsReadOnly to the controls
        private void setControls(bool readOnly = true)
        {
            this.txtID.IsReadOnly = readOnly;
            this.txtDescription.IsReadOnly = readOnly;
            this.txtDepartment.IsReadOnly = readOnly;
            this.txtCustomer.IsReadOnly = readOnly;
            this.txtPickup.IsReadOnly = readOnly;
        }

        // Save the edit/add
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var order = new CustomerOrder();
            switch (_type)
            {
                case ItemDetailForm.Add:
                    if (captureOrder(order) == false)
                    {
                        return;
                    }

                    try
                    {
                        if (_itemManager.SaveNewOrder(order))
                        {
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case ItemDetailForm.Edit:
                    if (captureOrder(order) == false)
                    {
                        return;
                    }
                    order.OrderID = _order.OrderID;
                    var oldOrder = _order;

                    try
                    {
                        if (_itemManager.EditOrder(order, oldOrder))
                        {
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case ItemDetailForm.View:
                    break;
                default:
                    break;
            }
        }

        // Make sure everything is filled out
        private bool captureOrder(CustomerOrder order)
        {
            int ID;
            if (!int.TryParse(this.txtCustomer.Text, out ID))
            {
                MessageBox.Show("You must enter a CustomerID.");
                return false;
            }
            else
            {
                order.CustomerID = ID;

            }
            if (!int.TryParse(this.txtDepartment.Text, out ID))
            {
                MessageBox.Show("You must enter a DepartmentID.");
                return false;
            }
            else
            {
                order.DepartmentID = ID;

            }
            if (this.txtDescription.Text == "")
            {
                MessageBox.Show("You must enter a description.");
                return false;
            }
            else
            {
                order.Description = txtDescription.Text;
            }
            if (this.txtPickup.Text == "")
            {
                MessageBox.Show("You must enter a pickup date.");
                return false;
            }
            else
            {
                order.PickupDate = txtPickup.Text;
            }
            return true;
        }
    }
}
