using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;

namespace ygn.views
{
    public partial class PawnTransaction : DevExpress.XtraEditors.XtraForm
    {
        public PawnTransaction()
        {
            InitializeComponent();
            
        }

        helpers.xmlhelper xmlhelper = new helpers.xmlhelper();
        helpers.dbhelper dbhelper = new helpers.dbhelper();
        helpers.voucher_helper vhelper = new helpers.voucher_helper();
        helpers.datetime_helper dthelper = new helpers.datetime_helper();
        helpers.string_helper strhelper = new helpers.string_helper();
        Dictionary<string, object> _dic = new Dictionary<string, object>();

        public enum form_state { non, add, edit };
        public form_state frm_sts;
        private string _vno;

        public void ShowDialog(string _title, form_state _sts, string _vno, DateTime _dt)
        {
            if (Properties.Settings.Default.datetime_PawnTransaction.Date != null)
            {
                dateEdit_date.EditValue = Properties.Settings.Default.datetime_PawnTransaction;
            }
            else
            {
                dateEdit_date.EditValue = _dt;
            }
            this.Text = _title;            
            frm_sts = _sts;
            if (!string.IsNullOrWhiteSpace(_vno))
            { this._vno = _vno; }
            else
            { this._vno = ""; }
            this.ShowDialog();
        }

        private void PawnTransaction_Load(object sender, EventArgs e)
        {
            dateEdit_date.EditValue = DateTime.Now;
            if (frm_sts == form_state.add)
            {
                setControlState(false);
                setButtonState("default");
            }
            else if (frm_sts == form_state.edit)
            {                
                setButtonState("default");
                setControlState(false);
            }

            clearControlData();
            updateGridControl();
        }

        private void setControlState(bool p)
        {
            //dateEdit_date.Enabled = p;
            radioGroup_item_type.Enabled = p;
            radioGroup_voucher_type.Enabled = p;
            textEdit_invoice_number.Enabled = p;
            spinEdit_invoice_number_sn.Enabled = p;
            textEdit_customer_name.Enabled = p;
            textEdit_customer_nrc.Enabled = p;
            textEdit_customer_address.Enabled = p;
            textEdit_item_name.Enabled = p;
            textEdit_item_weight.Enabled = p;
            textEdit_mm_amount.Enabled = p;
            textEdit_mm_amount_text.Enabled = p;
            textEdit_description.Enabled = p;            
        }
        private void setButtonState(string _str)
        {
            switch (_str)
            {
                case "default":
                    simpleButton_new_invoice.Enabled = true;
                    simpleButton_ok.Enabled = false;
                    simpleButton_cancel.Enabled = false;
                    simpleButton_close.Enabled = true;
                    break;
                case "add":
                    simpleButton_new_invoice.Enabled = false;
                    simpleButton_ok.Enabled = true;
                    simpleButton_cancel.Enabled = true;
                    simpleButton_close.Enabled = true;
                    break;
                case "edit":
                    simpleButton_new_invoice.Enabled = false;
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
            //dateEdit_date.EditValue = DateTime.Now;
            textEdit_expire_date.ResetText();
            textEdit_invoice_number.ResetText();
            spinEdit_invoice_number_sn.ResetText();
            textEdit_customer_name.ResetText();
            textEdit_customer_nrc.ResetText();
            textEdit_customer_address.ResetText();
            textEdit_item_name.ResetText();
            textEdit_item_weight.ResetText();
            textEdit_en_amount.ResetText();
            textEdit_mm_amount.ResetText();
            textEdit_mm_amount_text.ResetText();
            textEdit_description.ResetText();
        }

        private void simpleButton_new_invoice_Click(object sender, EventArgs e)
        {
            setButtonState("add");
            clearControlData();
            generateInvoiceNumber();
            displayExpireDate();
            textEdit_customer_name.Focus();
        }

        

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {            
            updateGridControl();
            displayExpireDate();
        }

        private void displayExpireDate()
        {            
            string.Format("dd-MMM-yyyy", textEdit_expire_date.EditValue = dthelper.calculateExpireDate(Convert.ToDateTime(dateEdit_date.EditValue), radioGroup_item_type.SelectedIndex));
        }


        private void generateInvoiceNumber()
        {
            // check code is usable
            if (dbhelper.voucherCodeExist((DateTime)dateEdit_date.EditValue, radioGroup_item_type.SelectedIndex + 1, radioGroup_voucher_type.SelectedIndex + 1))                
            {
                string _invno = vhelper.generateNewInvoiceNumber(Convert.ToDateTime(dateEdit_date.EditValue), Convert.ToByte(radioGroup_item_type.SelectedIndex + 1), Convert.ToByte(radioGroup_voucher_type.SelectedIndex + 1));
                textEdit_invoice_number.EditValue = _invno.Substring(0, _invno.Trim().Length - 4);
                spinEdit_invoice_number_sn.EditValue = _invno.Substring((_invno.Trim().Length - 4), 4);
                //string.Format("dd-MMM-yyyy", textEdit_expire_date.EditValue = dthelper.calculateExpireDate(Convert.ToDateTime(dateEdit_date.EditValue), radioGroup_item_type.SelectedIndex));
                setControlState(true);
            }
            else
            {
                alertControl.Show(this, "No Voucher Code", "ယခုလအတွက် ပြေစာကုဒ် မထည့်ရသေးပါ။");

                if (DialogResult.Yes == XtraMessageBox.Show("ယခုလအတွက်ပြေစာ ကုဒ်မထည့်ရသေးပါ။ ကုဒ်အသစ်ထည့်ရန်. YES ကိုရွေးပါ။", "Add New Voucher Code", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    using (views.VoucherCode vouchercode = new VoucherCode())
                    {
                        vouchercode.ShowInTaskbar = false;
                        vouchercode.ShowDialog("ပြေစာကုဒ်", dateEdit_date.EditValue.ToString());
                    }                    
                }
                simpleButton_cancel.PerformClick();
            }

            
        }
        /// <summary>
        /// Manipulate number with 0
        /// </summary>        
        /// <param name="_sn"></param>
        /// <returns></returns>
        

        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            //check invalid input
            if (checkNullValue())
            {
                if (frm_sts == form_state.add)
                {
                    if (dbhelper.checkInvoiceNumberExist(textEdit_invoice_number.EditValue.ToString(), spinEdit_invoice_number_sn.EditValue.ToString()))
                    {
                        string _invoiceNumber = textEdit_invoice_number.EditValue.ToString().Trim() + vhelper.manipulateNumber(spinEdit_invoice_number_sn.EditValue.ToString().Trim());
                        if (dbhelper.insertPawnTransaction(_invoiceNumber, Convert.ToByte(radioGroup_voucher_type.Properties.Items[radioGroup_voucher_type.SelectedIndex].Value.ToString()), textEdit_customer_name.EditValue.ToString(), textEdit_customer_nrc.EditValue.ToString(), textEdit_customer_address.EditValue.ToString(), Convert.ToByte(radioGroup_item_type.SelectedIndex + 1), textEdit_item_name.EditValue.ToString(), textEdit_item_weight.EditValue.ToString(), Convert.ToDecimal(textEdit_en_amount.EditValue), textEdit_mm_amount.EditValue.ToString(), textEdit_mm_amount_text.EditValue.ToString(), textEdit_description.EditValue.ToString(), Convert.ToDateTime(dateEdit_date.EditValue), DateTime.Now, "admin", false, 0))
                        {
                            //
                            setButtonState("default");
                            setControlState(false);
                            updateGridControl();
                            saveMySetting();
                        }
                        else
                            MessageBox.Show("Insert pawn_transaction has failed.");
                    }
                    else // voucher number already exist
                    {
                        alertControl.Show(this, "Invoice Number already Exist", "This voucher number already in use.");
                    }
                }
                else if(frm_sts == form_state.edit)
                {
                    string _invoiceNumber = textEdit_invoice_number.EditValue.ToString().Trim() + vhelper.manipulateNumber(spinEdit_invoice_number_sn.EditValue.ToString().Trim());
                    if (dbhelper.updatePawnTransaction(_invoiceNumber, Convert.ToByte(radioGroup_voucher_type.Properties.Items[radioGroup_voucher_type.SelectedIndex].Value.ToString()), textEdit_customer_name.EditValue.ToString(), textEdit_customer_nrc.EditValue.ToString(), textEdit_customer_address.EditValue.ToString(), Convert.ToByte(radioGroup_item_type.SelectedIndex + 1), textEdit_item_name.EditValue.ToString(), textEdit_item_weight.EditValue.ToString(), Convert.ToDecimal(textEdit_en_amount.EditValue), textEdit_mm_amount.EditValue.ToString(), textEdit_mm_amount_text.EditValue.ToString(), textEdit_description.EditValue.ToString(), Convert.ToDateTime(dateEdit_date.EditValue), DateTime.Now, "admin", false, 0))
                    {
                        //
                        setButtonState("default");
                        setControlState(false);
                        updateGridControl();
                        frm_sts = form_state.add;
                    }
                    else
                        MessageBox.Show("Insert pawn_transaction has failed.");
                }                
            }
            else
            {
                alertControl.Show(this, "Invalid", "Invalid input.");
                
            }
        }

        private bool checkNullValue()
        {
            while ( 
                (!string.IsNullOrWhiteSpace(textEdit_invoice_number.EditValue.ToString())) &&                 
                //(!string.IsNullOrWhiteSpace(textEdit_customer_name.EditValue.ToString())) && 
                //(!string.IsNullOrWhiteSpace(textEdit_customer_nrc.EditValue.ToString())) &&  
                //(!string.IsNullOrWhiteSpace(textEdit_customer_address.EditValue.ToString())) && 
                (!string.IsNullOrEmpty(textEdit_en_amount.EditValue.ToString())) && 
                //(!string.IsNullOrWhiteSpace(textEdit_description.EditValue.ToString())) && 
                (!string.IsNullOrWhiteSpace(textEdit_item_weight.EditValue.ToString()))
                )
            {
                return true;
            }
            return false;
        }

        private void updateGridControl()
        {
            if (radioGroup_grid_filter.SelectedIndex == 0)
            {
                gridControl_pawn.DataSource = null;
                gridControl_pawn.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShort((DateTime)dateEdit_date.EditValue);
            }
            else if(radioGroup_grid_filter.SelectedIndex ==1)
            {
                gridControl_pawn.DataSource = null;
                gridControl_pawn.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShortByVoucherType1((DateTime)dateEdit_date.EditValue, 1);
            }
            else
            {
                gridControl_pawn.DataSource = null;
                gridControl_pawn.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShortByVoucherType2((DateTime)dateEdit_date.EditValue, 2);
            }
            
            
            //gridControl_list.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShort((DateTime)dateEdit_date.EditValue);
            /*
            gridControl_pawn.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShort((DateTime)dateEdit_date.EditValue);
            
            gridView_list.Columns["no"].Width = 50;
            gridView_list.Columns["voucher_no"].Width = 100;
            gridView_list.Columns["customer_name"].Width = 100;
            gridView_list.Columns["customer_nrc"].Width = 75;
            gridView_list.Columns["item_name"].Width = 150;
            gridView_list.Columns["item_weight"].Width = 150;
            gridView_list.Columns["en_amount"].Width = 100;
            gridView_list.Columns["month_count"].Visible = false;

            //gridView_list.Columns["en_amount"].DisplayFormat.FormatString = "d0";
            */
            
        }

        private void textEdit_mm_amount_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textEdit_mm_amount.EditValue.ToString()))
            {
                string _str = strhelper.convertMyanmarToEnglishNumberString(textEdit_mm_amount.EditValue.ToString());
                if (!string.IsNullOrWhiteSpace(_str))
                {
                    textEdit_en_amount.EditValue = _str;
                }
                else
                {
                    textEdit_mm_amount.ResetText();
                    textEdit_en_amount.ResetText();
                }
            }
        }


        private void PawnTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                simpleButton_new_invoice_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F3)
            {
                simpleButton_ok_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F4)
            {
                simpleButton_cancel_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                simpleButton_close_Click(sender, e);
            }
        }

        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            setButtonState("default");
            setControlState(false);
            clearControlData();
            //frm_sts = form_state.non;
        }

        private void simpleButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioGroup_voucher_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            generateInvoiceNumber();
        }

        private void radioGroup_item_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup_item_type.SelectedIndex == 1)
            {
                radioGroup_voucher_type.Enabled = false;
            }
            else
            {
                radioGroup_voucher_type.Enabled = true;
            }
        }

        private void gridView_list_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {            
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   
                popupMenu.ShowPopup(MousePosition);
            }
        }
        private void gridView_pawn_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                popupMenu.ShowPopup(MousePosition);
            }
        }

        private void barButtonItem_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // change state
            frm_sts = form_state.edit;
            // fill old data
            //fillPawnTransactionData(gridView_list.GetFocusedDataRow()["voucher_no"].ToString());
            fillPawnTransactionData(gridView_pawn.GetFocusedDataRow()["voucher_no"].ToString());
            displayExpireDate();
            // lock
            setControlState(true);
            setButtonState("edit");
        }

        private void barButtonItem_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string _invno = gridView_list.GetFocusedDataRow()["voucher_no"].ToString();
            string _invno = gridView_pawn.GetFocusedDataRow()["voucher_no"].ToString();
            if (dbhelper.checkVoucherNotExpiredAndReceivedByInvoiceNumberByPawnDate(_invno, (DateTime)dateEdit_date.EditValue))
            {
                if (DialogResult.Yes == XtraMessageBox.Show("ပြေစာအမှတ် " + _invno + " အားဖျက်မည်။", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (dbhelper.deletePawnTransactionByInvoiceNumber(_invno))
                    {
                        setButtonState("default");
                        setControlState(false);
                        updateGridControl();
                    }
                    else
                    {
                        alertControl.Show(this, "Failed", "Failed to delete invovice.");
                    }
                    frm_sts = form_state.non;
                }
            }
            else
            {
                XtraMessageBox.Show("Received or Expired", "​​ေရွးပြီးသား ​ေပြစာနှင့် ​ေပါင်ဆံုး​ေပြစာများအား ဖျက်လို့မရပါ။", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fillPawnTransactionData(string _invno)
        {
            Dictionary<string, Object> _dic = dbhelper.getAllPawnInfoByInvoiceNumber(_invno);
            
            radioGroup_voucher_type.SelectedIndex = ((byte)_dic["voucher_type"] - 1);
            radioGroup_item_type.SelectedIndex = ((byte)_dic["item_type"] - 1);

            dateEdit_date.EditValue = _dic["pawn_date"];
            textEdit_invoice_number.EditValue = _invno.Substring(0, _invno.Length - 4);
            spinEdit_invoice_number_sn.EditValue = _invno.Substring(_invno.Length - 4, 4);
            textEdit_customer_name.EditValue = (string)_dic["customer_name"];
            textEdit_customer_nrc.EditValue = (string)_dic["customer_nrc"];
            textEdit_customer_address.EditValue = (string)_dic["customer_address"];
            textEdit_item_name.EditValue = (string)_dic["item_name"];
            textEdit_item_weight.EditValue = (string)_dic["item_weight"];
            textEdit_description.EditValue = (string)_dic["description"];
            textEdit_en_amount.EditValue = (decimal)_dic["en_amount"];
            textEdit_mm_amount.EditValue = (string)_dic["mm_amount"];
            textEdit_mm_amount_text.EditValue = (string)_dic["mm_amount_text"];         
        }

        private void gridControl_pawn_Click(object sender, EventArgs e)
        {

        }



        private void saveMySetting()
        {
            Properties.Settings.Default.datetime_PawnTransaction = (DateTime)dateEdit_date.EditValue;
            Properties.Settings.Default.Save();
        }

        private void radioGroup_grid_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridControl();
        }
        
        

    }
}