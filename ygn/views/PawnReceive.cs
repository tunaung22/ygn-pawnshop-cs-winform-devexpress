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
    public partial class PawnReceive : DevExpress.XtraEditors.XtraForm
    {
        public PawnReceive()
        {
            InitializeComponent();
        }

        helpers.dbhelper dbhelper = new helpers.dbhelper();
        helpers.voucher_helper vhelper = new helpers.voucher_helper();
        helpers.datetime_helper dthelper = new helpers.datetime_helper();        
        Dictionary<string, object> _dic = new Dictionary<string, object>();

        public enum form_state { add, edit, non };
        public form_state frm_st;
        private string _vno;

        public void ShowDialog(string _title, form_state _sts, string _vno, DateTime _dt)
        {
            if (Properties.Settings.Default.datetime_PawnReceive.Date != null)
            {
                dateEdit_date.EditValue = Properties.Settings.Default.datetime_PawnReceive;
            }
            else
            {
                dateEdit_date.EditValue = _dt;
            }
            this.Text = _title;
            frm_st = _sts;
            if (string.IsNullOrWhiteSpace(_vno))
            { this._vno = _vno; }
            else
            { this._vno = ""; }
            this.ShowDialog();
        }

        private void PawnReceive_Load(object sender, EventArgs e)
        {
            if (frm_st == form_state.add)
            {                
                setButtonState("default");
            }
            else if (frm_st == form_state.edit)
            {
                setButtonState("add");                
            }
            //simpleButton_new_receive.Enabled = false;
            setControlState(false);            
            clearControlData();
            updateGridControl();
            updateGridLookupEdit_invoice_number();
        }


        private void simpleButton_new_receive_Click(object sender, EventArgs e)
        {
            frm_st = form_state.add;
            setButtonState("add");
            if (gridLookUpEdit_invoice_number.EditValue != null)
            {
                setControlState(true);
            }
        }

        private void updateGridControl()
        {
            if (radioGroup_grid_filter.SelectedIndex == 0)
            {
                gridControl_list.DataSource = null;
                gridControl_list.DataSource = dbhelper.selectPawnReceiveTransactionInfoByReceiveDate((DateTime)dateEdit_date.EditValue);                


            }
            else
            {
                gridControl_list.DataSource = null;
                gridControl_list.DataSource = dbhelper.selectPawnReceiveTransactionInfoByReceiveDateByVoucherType((DateTime)dateEdit_date.EditValue, (byte)radioGroup_grid_filter.SelectedIndex);
            }
            
        }

        
        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {            
            updateGridLookupEdit_invoice_number();
            updateGridControl();
            gridLookUpEdit_invoice_number.EditValue = null;
            quietClean();
            setButtonState("default");
        }

        private void updateGridLookupEdit_invoice_number()
        {
            //gridLookUpEdit_invoice_number.Properties.DataSource = dbhelper.selectInvoiceNumber();
            gridLookUpEdit_invoice_number.Properties.DataSource = dbhelper.selectInvoiceInfo((DateTime)dateEdit_date.EditValue);
            
            gridLookUpEdit_invoice_number.Properties.ValueMember = "invoice_number";
            gridLookUpEdit_invoice_number.Properties.DisplayMember = "invoice_number";
        }

        private void gridLookUpEdit_invoice_number_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(gridLookUpEdit_invoice_number.Text.ToString()))
            {
                setButtonState("default");

                fillPawnInfo(gridLookUpEdit_invoice_number.EditValue.ToString());
            }
        }

        private void quietClean()
        {
            //clean
            textEdit_pawn_date.ResetText();
            textEdit_expire_date.ResetText();
            textEdit_customer_name.ResetText();
            textEdit_customer_nrc.ResetText();
            textEdit_customer_address.ResetText();
            textEdit_item_name.ResetText();
            textEdit_item_weight.ResetText();
            textEdit_description.ResetText();
            textEdit_en_amount.EditValue = 0;
            spinEdit_month_count.EditValue = 0;
            spinEdit_interest_rate.EditValue = 3;
            textEdit_interest_amount.EditValue = 0;
            textEdit_total_receive_amount.EditValue = 0;
        }
        private void fillPawnInfo(string _invno)
        {
            if (_invno != null)
            {
                quietClean();
                _dic.Clear();
                _dic = dbhelper.getPawnInfoByInvoiceNumber(_invno, (DateTime)dateEdit_date.EditValue);
                if (_dic.Count > 0)
                {   
                    string.Format("dd-MMM-yyyy", textEdit_pawn_date.EditValue = ((DateTime)_dic["pawn_date"]).ToString("dd-MMM-yyyy"));
                    textEdit_customer_name.EditValue = (string)_dic["customer_name"];
                    textEdit_customer_nrc.EditValue = (string)_dic["customer_nrc"];
                    textEdit_customer_address.EditValue = (string)_dic["customer_address"];
                    textEdit_item_name.EditValue = (string)_dic["item_name"];
                    textEdit_item_weight.EditValue = (string)_dic["item_weight"];
                    textEdit_description.EditValue = (string)_dic["description"];
                    textEdit_en_amount.EditValue = (decimal)_dic["en_amount"];
                    spinEdit_interest_rate.EditValue = 3.0;
                    //if (Convert.ToDateTime(textEdit_pawn_date.EditValue.ToString()).Month == dateEdit_date.DateTime.Month)
                    //{ spinEdit_interest_rate.EditValue = 3.0; }
                    //else
                    //{ spinEdit_interest_rate.EditValue = 3.0; }

                    // calculate-expire-date
                    string.Format("dd-MMM-yyyy", textEdit_expire_date.EditValue = dthelper.calculateExpireDate(Convert.ToDateTime(textEdit_pawn_date.EditValue.ToString()), 0));
                    
                    // calculate-month-count
                    //pawndate - recdate
                    
                    DateTime _pdate =  (DateTime)_dic["pawn_date"];
                    //DateTime _edate = Convert.ToDateTime(textEdit_expire_date.EditValue);
                    DateTime _rdate = (DateTime)dateEdit_date.EditValue;
                    
                    
                    int _monthcount = dthelper.monthDifferentForPawnReceive(_rdate, _pdate, Convert.ToDateTime(textEdit_expire_date.EditValue));
                    



                    if (_monthcount == 0 || _monthcount == 1)
                    { spinEdit_month_count.EditValue = 1; }
                    else
                    { spinEdit_month_count.EditValue = _monthcount; }
                    calculateInterestAmount();
                    simpleButton_new_receive.Enabled = true;



                    
                    if ((bool)_dic["is_received"] == true)
                    {
                        alertControl.Show(this, "Received", "ပြေစာအမှတ် " + _invno.ToString() + " မှာ ရွေးပြီးသားဖြစ်သည်။");                        
                        calculateInterestAmount();
                        simpleButton_new_receive.Enabled = false;
                    }
                    /*
                    else if ((bool)_dic["is_expired"] == true)
                    {                        
                        alertControl.Show(this, "Expired", "ပြေစာအမှတ် " + _invno.ToString() + " မှာ ပေါင်ဆုံးပြီးသားဖြစ်သည်။");
                        calculateInterestAmount();
                        simpleButton_new_receive.Enabled = true;                        
                    }
                    else
                    {
                        calculateInterestAmount();
                        simpleButton_new_receive.Enabled = true;
                    }
                    */
                }             
            }
            else
            {
                MessageBox.Show(":( !!!!");
            }
        }

        private void calculateInterestAmount()
        {
            #region calculate-interest-amount
            //  ((rate/100)*capital)*monthCount
            decimal rt = spinEdit_interest_rate.Value / 100;
            int mnth = Convert.ToInt32(spinEdit_month_count.EditValue);
            int amt = Convert.ToInt32(textEdit_en_amount.EditValue);
            textEdit_interest_amount.EditValue = Convert.ToInt32((rt * mnth) * amt);
            textEdit_total_receive_amount.EditValue = Convert.ToInt32(textEdit_interest_amount.EditValue) + Convert.ToInt32(textEdit_en_amount.EditValue);
            #endregion
        }

        private void spinEdit_interest_rate_EditValueChanged(object sender, EventArgs e)
        {
            #region calculate-interest-amount
            //  ((rate/100)*capital)*monthCount
            int intamount = vhelper.calculateInterestAmount(Convert.ToDecimal(spinEdit_interest_rate.EditValue), Convert.ToByte(spinEdit_month_count.EditValue), Convert.ToDecimal(textEdit_en_amount.EditValue));
            textEdit_interest_amount.EditValue = intamount;
            textEdit_total_receive_amount.EditValue = intamount + Convert.ToInt32(textEdit_en_amount.EditValue);
            #endregion
        }
        
        

        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            //validate
            if (string.IsNullOrWhiteSpace(textEdit_total_receive_amount.EditValue.ToString()) || gridLookUpEdit_invoice_number.EditValue == null)
            {
                alertControl.Show(this, "Invalid input", "input data is invalid or not provided.");
            }


            if (frm_st == form_state.add)
            {
                if (dbhelper.insertPawnReceiveTransaction(gridLookUpEdit_invoice_number.EditValue.ToString(), (DateTime)dateEdit_date.EditValue, Convert.ToDecimal(spinEdit_interest_rate.EditValue.ToString())))
                {
                    setButtonState("default");
                    setControlState(false);
                    updateGridControl();
                    updateGridLookupEdit_invoice_number();
                    saveMySetting();
                }

                else
                {
                    XtraMessageBox.Show("ERRor", "unaccepatable error. contact your developer :/ ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (frm_st == form_state.edit)
            {
                if ( dbhelper.updatePawnReceiveTransaction(gridLookUpEdit_invoice_number.EditValue.ToString(), (DateTime)dateEdit_date.EditValue, Convert.ToDecimal(spinEdit_interest_rate.EditValue)))
                {
                    //
                    setButtonState("default");
                    setControlState(false);
                    updateGridControl();
                    updateGridLookupEdit_invoice_number();
                    frm_st = form_state.add;
                }
                else
                {
                    XtraMessageBox.Show("ERRor", "unaccepatable error. contact your developer :/ ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
            
        }

        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            setButtonState("default");
            setControlState(false);
            clearControlData();
        }

        private void simpleButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setControlState(bool p)
        {
            //dateEdit_date.Enabled = p;            
            //gridLookUpEdit_invoice_number.Enabled = p;            
            //textEdit_pawn_date.Enabled = p;
            //textEdit_expire_date.Enabled = p;
            //textEdit_customer_name.Enabled = p;
            //textEdit_customer_nrc.Enabled = p;
            //textEdit_customer_address.Enabled = p;
            //textEdit_item_name.Enabled = p;
            //textEdit_item_weight.Enabled = p;
            //textEdit_description.Enabled = p;
            //textEdit_en_amount.Enabled = p;
            //textEdit_month_count.Enabled = p;
            spinEdit_interest_rate.Enabled = p;
            //textEdit_interest_amount.Enabled = p;
            textEdit_total_receive_amount.Enabled = p;
        }

        private void setButtonState(string _str)
        {
            switch (_str)
            {
                case "default":
                    simpleButton_new_receive.Enabled = false;
                    simpleButton_ok.Enabled = false;
                    simpleButton_cancel.Enabled = false;
                    simpleButton_close.Enabled = true;
                    break;
                case "add":
                    simpleButton_new_receive.Enabled = false;
                    simpleButton_ok.Enabled = true;
                    simpleButton_cancel.Enabled = true;
                    simpleButton_close.Enabled = true;
                    break;
                case "edit":
                    simpleButton_new_receive.Enabled = false;
                    simpleButton_ok.Enabled = true;
                    simpleButton_cancel.Enabled = true;
                    simpleButton_close.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void clearControlData()
        {
            dateEdit_date.EditValue = DateTime.Now;
            gridLookUpEdit_invoice_number.EditValue = null;
            textEdit_pawn_date.ResetText();
            textEdit_expire_date.ResetText();
            textEdit_customer_name.ResetText();
            textEdit_customer_nrc.ResetText();
            textEdit_customer_address.ResetText();
            textEdit_item_name.ResetText();
            textEdit_item_weight.ResetText();
            textEdit_description.ResetText();
            textEdit_en_amount.EditValue = 0;
            spinEdit_month_count.EditValue = 0;
            spinEdit_interest_rate.EditValue = 3;
            textEdit_interest_amount.EditValue = 0;
            textEdit_total_receive_amount.EditValue = 0;
        }


        private void saveMySetting()
        {
            Properties.Settings.Default.datetime_PawnReceive = (DateTime)dateEdit_date.EditValue;
            Properties.Settings.Default.Save();
        }

        private void radioGroup_grid_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridControl();
        }

        private void gridView_list_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                popupMenu.ShowPopup(MousePosition);
            }
        }

        private void barButtonItem_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridLookUpEdit_invoice_number.EditValue = gridView_list.GetFocusedDataRow()["voucher_no"].ToString();
            /*
            // fill old data            
            fillPawnTransactionData(gridView_list.GetFocusedDataRow()["voucher_no"].ToString());
            // change state
            */
            frm_st = form_state.edit;            
            // lock
            setControlState(true);
            setButtonState("edit");
        }

        private void barButtonItem_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string _invno = gridView_list.GetFocusedDataRow()["voucher_no"].ToString();
            //if (dbhelper.checkVoucherNotExpiredAndReceivedByInvoiceNumberByPawnDate(_invno, (DateTime)dateEdit_date.EditValue))
            //{
                if (DialogResult.Yes == XtraMessageBox.Show("ပြေစာအမှတ် " + _invno + " အား မရွေးရန် ပြင်မည်", "Confirm Undo Pawn Receive", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (dbhelper.undoPawnReceiveTransaction(_invno))
                    {
                        setButtonState("default");
                        setControlState(false);
                        updateGridControl();
                    }
                    else
                    {
                        alertControl.Show(this, "Failed", "Failed to undo invoice.");
                    }
                    frm_st = form_state.non;
                }
            //}
            //else
            //{
            //    XtraMessageBox.Show("Received or Expired", "​​ေရွးပြီးသား ​ေပြစာနှင့် ​ေပါင်ဆံုး​ေပြစာများအား ဖျက်လို့မရပါ။", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            
        }


        private void fillPawnTransactionData(string _voucherNo)
        {
            Dictionary<string, Object> _dic = dbhelper.getAllPawnReceiveInfoByInvoiceNumber(_voucherNo);
            
            gridLookUpEdit_invoice_number.EditValue = _voucherNo;
            DateTime _pdate = (DateTime)_dic["pawn_date"];            
            string.Format("dd-MMM-yyyy", textEdit_pawn_date.EditValue, _pdate);
            //string.Format("dd-MMM-yyyy", textEdit_pawn_date.EditValue = _dic["pawn_date"], 0);
            string.Format("dd-MMM-yyyy", textEdit_expire_date.EditValue = dthelper.calculateExpireDate(Convert.ToDateTime(textEdit_pawn_date.EditValue.ToString()), 0));

            textEdit_customer_name.EditValue = (string)_dic["customer_name"];
            textEdit_customer_nrc.EditValue = (string)_dic["customer_nrc"];
            textEdit_customer_address.EditValue = (string)_dic["customer_address"];
            textEdit_item_name.EditValue = (string)_dic["item_name"];
            textEdit_item_weight.EditValue = (string)_dic["item_weight"];
            textEdit_description.EditValue = (string)_dic["description"];
            textEdit_en_amount.EditValue = (decimal)_dic["en_amount"];
            int _monthcount = dthelper.monthDifferentForPawnReceive((DateTime)dateEdit_date.EditValue, Convert.ToDateTime(textEdit_pawn_date.EditValue), Convert.ToDateTime(textEdit_expire_date.EditValue));            
            spinEdit_month_count.EditValue = _monthcount;
            spinEdit_interest_rate.EditValue = _dic["interest_rate"];
        }

        private void spinEdit_month_count_EditValueChanged(object sender, EventArgs e)
        {
            #region calculate-interest-amount
            //  ((rate/100)*capital)*monthCount
            int intamount = vhelper.calculateInterestAmount(Convert.ToDecimal(spinEdit_interest_rate.EditValue), Convert.ToByte(spinEdit_month_count.EditValue), Convert.ToDecimal(textEdit_en_amount.EditValue));
            textEdit_interest_amount.EditValue = intamount;
            textEdit_total_receive_amount.EditValue = intamount + Convert.ToInt32(textEdit_en_amount.EditValue);
            #endregion
        }

    }
}