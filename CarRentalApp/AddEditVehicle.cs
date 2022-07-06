using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode = false;
        private int Id;
        public AddEditVehicle()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
        }

        public AddEditVehicle(TypesOfCars carToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            PopulateFields(carToEdit);
            isEditMode = true;
        }

        private void PopulateFields(TypesOfCars car)
        {
            txtMake.Text = car.Make;
            txtModel.Text = car.Model;
            txtVIN.Text = car.VIN;
            txtYear.Text = car.Year.ToString();
            txtLPN.Text = car.LicensePlateNumber;
            Id = car.Id;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                var _db = new CarRentalEntities1();
                if (isEditMode)
                {
                    var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == Id);
                    car.Make = txtMake.Text;
                    car.Model = txtModel.Text;
                    car.VIN = txtVIN.Text;
                    car.LicensePlateNumber = txtLPN.Text;
                    if (!string.IsNullOrEmpty(txtYear.Text))
                    {
                        car.Year = int.Parse(txtYear.Text);
                    }
                }
                else
                {
                    var car = new TypesOfCars()
                    {
                        Make = txtMake.Text,
                        Model = txtModel.Text,
                        VIN = txtVIN.Text,
                        LicensePlateNumber = txtLPN.Text
                    };
                    if (!string.IsNullOrEmpty(txtYear.Text))
                    {
                        car.Year = int.Parse(txtYear.Text);
                    }
                    _db.TypesOfCars.Add(car);
                }
                _db.SaveChanges();
                Application.OpenForms.Cast<Form>().Where(q => q is ManageVehicleListing)
                    .Cast<ManageVehicleListing>().ToList().ForEach(q => q.Invoke(q.refreshDelegate));
                Application.OpenForms.Cast<Form>().Where(q => q is ManageRentalRecord).Cast<ManageRentalRecord>()
                    .ToList().ForEach(q => q.Invoke(q.refreshDelegate));
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
