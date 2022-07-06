using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var _db = new CarRentalEntities1();
                var username = txtUserName.Text.Trim();
                var password = txtPW.Text;
                using (var myEncrypt = SHA256.Create())
                {
                    byte[] hashValue = myEncrypt.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var stringBuilder = new StringBuilder();
                    hashValue.ToList().ForEach(q => stringBuilder.Append(q.ToString("x2")));
                    var hashed_password = stringBuilder.ToString();
                    var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password);
                    if (user == null)
                    {
                        MessageBox.Show("Either your username or password is wrong.");
                        txtPW.Text = "";
                    }
                    else
                    {
                        var mainWindow = new MainWindow(this, user.id);
                        mainWindow.Show();
                        Hide();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured. Please try again.");
            }
        }
    }
}
