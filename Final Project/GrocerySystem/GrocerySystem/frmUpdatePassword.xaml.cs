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
using DataTransferObjects;
using LogicLayer;

namespace GrocerySystem
{
    /// <summary>
    /// Interaction logic for frmUpdatePassword.xaml
    /// </summary>
    public partial class frmUpdatePassword : Window
    {
        private UserManager _userManager;
        private User _user;

        public frmUpdatePassword(UserManager userManager, User user)
        {
            _userManager = userManager;
            _user = user;

            InitializeComponent();
        }



        private void clearPasswordBoxes()
        {
            pwdNewPassword.Password = "";
            pwdRetypePassword.Password = "";

            if (_user.Titles[0].TitleID == "New User")
            {
                pwdNewPassword.Focus();
            }
            else // existing user
            {
                pwdOldPassword.Password = "";
                pwdOldPassword.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnSubmit.IsDefault = true;
            if (_user.Titles[0].TitleID == "New User")
            {
                this.tblkMessage.Text = "Please enter a new password.";
                this.pwdOldPassword.Password = "newuser";
                this.pwdOldPassword.IsEnabled = false;
            }
            clearPasswordBoxes();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // storage
            var oldPassword = pwdOldPassword.Password;
            var newPassword = pwdNewPassword.Password;
            var retypePassword = pwdRetypePassword.Password;

            // is old password missing?
            if (oldPassword == "")
            {
                MessageBox.Show("You must supply your current password.");
                clearPasswordBoxes();
                return;
            }

            // is new password missing or too short?
            if (newPassword.Length < 6)
            {
                MessageBox.Show("The new password is invalid. Try again.");
                clearPasswordBoxes();
                return;
            }

            // make sure the new password and retype password match
            if (newPassword != retypePassword)
            {
                MessageBox.Show("Your new password and retyped password don't match.");
                clearPasswordBoxes();
                return;
            }

            // update the password
            try
            {
                _user = _userManager.UpdatePassword(_user, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;

                MessageBox.Show(message, "Update Failed!");
            }
            // if the dialog completed sucessfully, indicate that
            this.DialogResult = true;
        }
    }
}
