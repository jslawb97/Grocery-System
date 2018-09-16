using LogicLayer;
using DataTransferObjects;
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
    /// Interaction logic for frmItemEdit.xaml
    /// </summary>
    public partial class frmItemEdit : Window
    {
        private ProductManager _itemManager;
        private Products _product;
        private ItemDetailForm _type;

        // View or edit mode
        public frmItemEdit(ProductManager itMgr, Products products, ItemDetailForm type)
        {
            _itemManager = itMgr;
            _product = products;
            _type = type;
            InitializeComponent();
        }

        // Add mode
        public frmItemEdit(ProductManager itMgr)
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
            this.txtUPC.IsEnabled = false;
        }

        private void setupViewForm()
        {
            this.Title = "Item View Mode";
            btnSave.Visibility = System.Windows.Visibility.Hidden;
            setControls(readOnly: true);
            populateControls();

        }

        private void setupEditForm()
        {
            this.Title = "Item Edit Mode";

            setControls(readOnly: false);
            populateControls();
        }

        private void setupAddForm()
        {
            this.Title = "Add New Product";
        }

        // Set the controls to either true or false
        private void setControls(bool readOnly = true)
        {
            this.txtUPC.IsReadOnly = readOnly;
            this.txtDepartment.IsReadOnly = readOnly;
            this.txtManufacturer.IsReadOnly = readOnly;
            this.txtName.IsReadOnly = readOnly;
            this.txtOnHand.IsReadOnly = readOnly;
            this.txtCost.IsReadOnly = readOnly;
        }

        // Fill the controls with the selected row
        private void populateControls()
        {
            try
            {
                this.txtUPC.Text = _product.UPC.ToString();
                this.txtDepartment.Text = _product.DepartmentID.ToString();
                this.txtName.Text = _product.Name;
                this.txtManufacturer.Text = _product.Manufacturer;
                this.txtOnHand.Text = _product.OnHand.ToString();
                this.txtCost.Text = _product.Cost.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // Save changes or new product
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var product = new Products();
            switch (_type)
            {
                case ItemDetailForm.Add:
                    if (captureProduct(product)==false)
                    {
                        return;
                    }

                    try
                    {
                        if (_itemManager.SaveNewProduct(product))
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
                    if (captureProduct(product) == false)
                    {
                        return;
                    }
                    product.UPC = _product.UPC;
                    var oldProduct = _product;

                    try
                    {
                        if (_itemManager.EditProduct(product, oldProduct))
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

        // Make sure all text boxes are filled out
        private bool captureProduct(Products product)
        {
            if (this.txtName.Text == "")
            {
                MessageBox.Show("You must enter a product name.");
                return false;
            }
            else
            {
                product.Name = txtName.Text;
            }
            if (this.txtManufacturer.Text == "")
            {
                MessageBox.Show("You must enter a manufacturer.");
                return false;
            }
            else
            {
                product.Manufacturer = txtManufacturer.Text;
            }
            int Department;
            if (!int.TryParse(this.txtDepartment.Text, out Department))
            {
                MessageBox.Show("You must enter a department ID.");
                return false;
            }
            else
            {
                product.DepartmentID = Department;
            }
            int OnHand;
            if (!int.TryParse(this.txtOnHand.Text, out OnHand))
            {
                MessageBox.Show("You must enter how much we have.");
                return false;
            }
            else
            {
                product.OnHand = OnHand;
            }
            return true;
            /*int Cost;
            if (!int.TryParse(this.txtCost.Text, out Cost))
            {
                MessageBox.Show("You must enter the price.");
                return false;
            }
            else
            {
                product.Cost = Cost;
            }
            return true;*/
            
        }
    }
}
