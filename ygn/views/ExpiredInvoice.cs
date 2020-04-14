using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ygn.views
{
    public partial class ExpiredInvoice : DevExpress.XtraEditors.XtraForm
    {
        public ExpiredInvoice()
        {
            InitializeComponent();
        }

        helpers.dbhelper dbhelper = new helpers.dbhelper();

        public void ShowDialog(string _title, DateTime _dt)
        {
            this.Text = _title;
            dateEdit_date.EditValue = _dt;
            this.ShowDialog();
        }


        private void ExpiredInvoice_Load(object sender, EventArgs e)
        {
            
        }

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {
            updateGridControl((DateTime)dateEdit_date.EditValue);
        }

        private void updateGridControl(DateTime _dt)
        {
            gridControl_list.DataSource = dbhelper.selectExpriedInvoiceInfoByDate(_dt);
        }

    }
}