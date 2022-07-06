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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode = false;
        private int id;
        private readonly CarRentalEntities1 _db;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            lblTitle.Text = "Add Rental Record";
            PopularFields();
        }

        public AddEditRentalRecord(CarRentalRecord record)
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            lblTitle.Text = "Edit Rental Record";
            isEditMode = true;
            PopularFields(record);
        }

        private void PopularFields(CarRentalRecord record = null)
        {
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Id = q.Id,
                    Name = q.Make + " " + q.Model
                })
                .ToList();
            cbCarType.DisplayMember = "Name";
            cbCarType.ValueMember = "Id";
            cbCarType.DataSource = cars;
            if (isEditMode)
            {
                txtCustomerName.Text = record.CustomerName;
                if (record.DateRented != null)
                    dtRented.Value = (DateTime)record.DateRented;
                if (record.DateReturned != null)
                    dtReturned.Value = (DateTime)record.DateReturned;
                txtCost.Text = record.Cost.ToString();
                cbCarType.SelectedIndex = cbCarType.FindStringExact(record.TypesOfCars.Make + " " + record.TypesOfCars.Model);
                id = record.Id;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var customerName = txtCustomerName.Text;
                var dateRent = dtRented.Value;
                var dateReturn = dtReturned.Value;
                var carTypeId = cbCarType.SelectedValue as int?;
                Decimal.TryParse(txtCost.Text, out var cost);

                MessageBox.Show($"Thanks {customerName} for renting {cbCarType.Text}, rented at {dateRent.ToLongDateString()}" +
                    $", returned at {dateReturn.ToLongDateString()}, cost{cost}");
                CarRentalRecord record;
                if (isEditMode)
                    record = _db.CarRentalRecord.FirstOrDefault(q => q.Id == id);
                else
                    record = new CarRentalRecord();
                record.CustomerName = customerName;
                record.DateRented = dateRent;
                record.DateReturned = dateReturn;
                record.TypeOfCarId = carTypeId;
                record.Cost = cost;
                if (!isEditMode)
                    _db.CarRentalRecord.Add(record);
                _db.SaveChanges();
                Application.OpenForms.Cast<Form>().Where(q => q is ManageRentalRecord).Cast<ManageRentalRecord>()
                    .ToList().ForEach(q => q.Invoke(q.refreshDelegate));
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
