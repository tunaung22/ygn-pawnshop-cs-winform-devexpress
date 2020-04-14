using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ygn.report
{
    public partial class rptExpired : DevExpress.XtraReports.UI.XtraReport
    {
        public rptExpired()
        {
            InitializeComponent();
        }

        DateTime _d;

        public void ShowPreviewDialog(DateTime _dt)
        {
            _d = _dt;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Object voucher_no = GetCurrentColumnValue("voucher_no");
            Object customer_name = GetCurrentColumnValue("customer_name");
            Object customer_nrc = GetCurrentColumnValue("customer_nrc");
            Object customer_address = GetCurrentColumnValue("customer_address");
            Object item_name = GetCurrentColumnValue("item_name");
            Object item_weight = GetCurrentColumnValue("item_weight");            
            Object en_amount = GetCurrentColumnValue("en_amount");
            Object pawn_date = GetCurrentColumnValue("pawn_date");            
            
            this.labelControl_date.Text = string.Format("dd-MMM-yyyy", _d);

            this.labelControl_invoice_number.Text = voucher_no.ToString().Trim();
            this.labelControl_customer_name.Text = customer_name.ToString().Trim();
            this.labelControl_customer_nrc.Text = customer_nrc.ToString().Trim();
            this.labelControl_customer_address.Text = customer_address.ToString().Trim();
            this.labelControl_item_name.Text = item_name.ToString().Trim();
            this.labelControl_item_weight.Text = item_weight.ToString().Trim();
            this.labelControl_en_amount.Text = en_amount.ToString().Trim();
            this.labelControl_pawn_date.Text = pawn_date.ToString().Trim();
            this.labelControl_expire_date.Text = _d.AddMonths(4).ToString("dd-MMM-yyyy");
        }

    }
}
