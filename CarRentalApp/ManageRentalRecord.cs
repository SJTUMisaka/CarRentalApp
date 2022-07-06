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
    public partial class ManageRentalRecord : Form
    {
        public delegate void RefreshData();
        public RefreshData refreshDelegate;
        private readonly CarRentalEntities1 _db;
        public ManageRentalRecord()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            refreshDelegate = new RefreshData(PopulateGrid);
        }

        private void ManageRentalRecord_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            var record = _db.CarRentalRecord
                .Select(q => new
                {
                    q.CustomerName,
                    q.DateRented,
                    q.DateReturned,
                    q.Cost,
                    q.Id,
                    Car = q.TypesOfCars.Make + " " + q.TypesOfCars.Model
                })
                .ToList();
            gvRecordList.DataSource = record;
            gvRecordList.Columns["Id"].Visible = false;
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            var recordToDelete = _db.CarRentalRecord.Find(gvRecordList.CurrentRow.Cells["Id"].Value);
            var dialogResult = MessageBox.Show($"Do you really want to delete rental record of " +
                $"{recordToDelete.CustomerName} when {recordToDelete.DateRented}?", "Delete", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                _db.CarRentalRecord.Remove(recordToDelete);
                _db.SaveChanges();
                Application.OpenForms.Cast<Form>().Where(q => q is ManageRentalRecord).Cast<ManageRentalRecord>()
                    .ToList().ForEach(q => q.Invoke(q.refreshDelegate));
            }
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            var recordToEdit = _db.CarRentalRecord.Find(gvRecordList.CurrentRow.Cells["Id"].Value);
            var editRecord = new AddEditRentalRecord(recordToEdit);
            editRecord.MdiParent = this.MdiParent;
            editRecord.Show();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord
            {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();
        }
    }
}
