using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ygn.report
{
    public partial class rptPawnReceive : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPawnReceive()
        {
            InitializeComponent();
        }

        DateTime _receiveDate;
        int _total_rec = 0;
        int _total_int = 0;
        

        public void ShowPreviewDialog(DateTime _dt)
        {
            _receiveDate = _dt;
            this.ShowPreviewDialog();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            object no = GetCurrentColumnValue("no");
            object voucher_no = GetCurrentColumnValue("voucher_no");
            object customer_name = GetCurrentColumnValue("customer_name");
            object customer_nrc = GetCurrentColumnValue("customer_nrc");
            object customer_address = GetCurrentColumnValue("customer_address");
            object item_name = GetCurrentColumnValue("item_name");
            object item_weight = GetCurrentColumnValue("item_weight");            
            object en_amount = GetCurrentColumnValue("en_amount");
            object interest_rate = GetCurrentColumnValue("interest_rate");
            object interest_amount = GetCurrentColumnValue("interest_amount");
            object receive_amount = GetCurrentColumnValue("receive_amount");


            this.labelControl_date.Text = string.Format("dd-MMM-yyyy", _receiveDate);

            this.labelControl_no.Text = no.ToString();            
            this.labelControl_customer_name.Text = customer_name.ToString().Trim();
            this.labelControl_customer_nrc.Text = customer_nrc.ToString().Trim();
            this.labelControl_customer_address.Text = customer_address.ToString().Trim();
            this.labelControl_item_name.Text = item_name.ToString().Trim();
            this.labelControl_item_weight.Text = item_weight.ToString().Trim();
            this.labelControl_en_amount.Text = en_amount.ToString().Trim();
            this.labelControl_interest_rate.Text = interest_amount.ToString().Trim();
            this.labelControl_receive_amount.Text = receive_amount.ToString().Trim();

            _total_rec = _total_rec + (int)receive_amount;
            this.labelControl_total_receive_amount.Text = _total_rec.ToString().Trim();
            _total_int = _total_int + (int)interest_amount;
            this.labelControl_total_interest_amount.Text = _total_int.ToString().Trim();            
        }

        private void Detail_AfterPrint(object sender, EventArgs e)
        {
            
        }


    }
}
