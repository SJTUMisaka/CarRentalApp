using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities1 _db;
        public delegate void RefreshData();
        public RefreshData refreshDelegate;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            refreshDelegate = new RefreshData(PopulateGrid);
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.Id
                })
                .ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns["LicensePlateNumber"].HeaderText = "License Plate Number";
            gvVehicleList.Columns["Id"].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addVehicle = new AddEditVehicle();
            addVehicle.MdiParent = this.MdiParent;
            addVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            var carToEdit = _db.TypesOfCars.Find(gvVehicleList.CurrentRow.Cells["Id"].Value);
            var editVehicle = new AddEditVehicle(carToEdit);
            editVehicle.MdiParent = this.MdiParent;
            editVehicle.Show();
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                var carToDelete = _db.TypesOfCars.Find(gvVehicleList.CurrentRow.Cells["Id"].Value);
                _db.TypesOfCars.Remove(carToDelete);
                _db.SaveChanges();
                Application.OpenForms.Cast<Form>().Where(q => q is ManageVehicleListing)
                    .Cast<ManageVehicleListing>().ToList().ForEach(q => q.Invoke(q.refreshDelegate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
