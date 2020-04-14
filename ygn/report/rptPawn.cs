using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ygn.report
{
    public partial class rptPawn : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPawn()
        {
            InitializeComponent();
        }

        DateTime _pawnDate;

        public void ShowPreviewDialog(DateTime _pdate)
        {
            _pawnDate = _pdate;
            this.ShowPreviewDialog();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Object voucher_no = GetCurrentColumnValue("voucher_no");
            Object customer_name = GetCurrentColumnValue("customer_name");
            Object customer_nrc = GetCurrentColumnValue("customer_nrc");
            Object customer_address = GetCurrentColumnValue("customer_address");
            Object item_name = GetCurrentColumnValue("item_name");
            Object item_weight = GetCurrentColumnValue("item_weight");
            Object pawn_date = GetCurrentColumnValue("pawn_date");
            Object en_amount = GetCurrentColumnValue("en_amount");

            this.labelControl_date.Text = string.Format("dd-MMM-yyyy", _pawnDate);

            this.labelControl_voucher_no.Text = voucher_no.ToString().Trim();
            this.labelControl_customer_name.Text = customer_name.ToString().Trim();
            this.labelControl_customer_nrc.Text = customer_nrc.ToString().Trim();
            this.labelControl_customer_address.Text = customer_address.ToString().Trim();
            this.labelControl_item_name.Text = item_name.ToString().Trim();
            this.labelControl_item_weight.Text = item_weight.ToString().Trim();
            this.labelControl_en_amount.Text = en_amount.ToString().Trim();
        }

    }
}
