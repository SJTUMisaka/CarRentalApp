using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private Login _login;
        private int _userId;
        public MainWindow(Login login, int userId)
        {
            InitializeComponent();
            _login = login;
            _userId = userId;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord
            {
                MdiParent = this
            };
            addRentalRecord.Show();
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var managementVehicleListing = new ManageVehicleListing
            {
                MdiParent = this
            };
            managementVehicleListing.Show();
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Application.OpenForms.Cast<Form>().Any(q => q is ManageRentalRecord))
            {
                var manageRentalRecord = new ManageRentalRecord
                {
                    MdiParent = this
                };
                manageRentalRecord.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            var _db = new CarRentalEntities1();
            var userRole = _db.UserRoles.FirstOrDefault(q => q.UserId == _userId);
            var role = _db.Roles.FirstOrDefault(q => q.Id == userRole.RoleId);
            if (!role.ManageVehicle)
                manageVehicleListingToolStripMenuItem.Visible = false;
            if (!role.ManageRecord)
                manageRentalRecordsToolStripMenuItem.Visible = false;
        }
    }
}
