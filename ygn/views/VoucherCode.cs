using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;





namespace ygn.views
{
    public partial class VoucherCode : DevExpress.XtraEditors.XtraForm
    {
        ygn.helpers.dbhelper dhelper = new ygn.helpers.dbhelper();

        public VoucherCode()
        {
            InitializeComponent();
        }

        public void ShowDialog(string _title, String _dt)
        {
            this.Text = _title;
            if (_dt != null)
            {
                dateEdit_date.EditValue = Convert.ToDateTime(_dt);
            }
            else
            {
                if (Properties.Settings.Default.datetime_VoucherCode != null)
                {
                    dateEdit_date.EditValue = Properties.Settings.Default.datetime_VoucherCode;
                }
                else
                {
                    dateEdit_date.EditValue = DateTime.Now;
                }
            }
            
            
            this.ShowDialog();
        }

        private void VoucherCode_Load(object sender, EventArgs e)
        {
            updateGridControl();
            textEdit_voucher_code.Focus();
        }

        private void updateGridControl()
        {   
            gridControl_list.DataSource = dhelper.selectAllVoucherCode();            
        }        

        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            /*  - validate
             *  - check duplicate
             *  - insert
             *  
             * format
             * yyyy-MM-dd HH:mmK
            */

            if (textEdit_voucher_code.EditValue != null)
            {
                if (!String.IsNullOrWhiteSpace(textEdit_voucher_code.EditValue.ToString()))
                {
                    if (!dhelper.voucherCodeExist((DateTime)dateEdit_date.EditValue, radioGroup_item_type.SelectedIndex + 1, radioGroup_voucher_type.SelectedIndex + 1))
                    {
                        if (dhelper.insertVoucherCode(dateEdit_date.Text, textEdit_voucher_code.EditValue.ToString(), Convert.ToByte(radioGroup_item_type.SelectedIndex + 1), Convert.ToByte(radioGroup_voucher_type.SelectedIndex + 1)))
                        {
                            updateGridControl();
                            textEdit_voucher_code.ResetText();
                            saveMySetting();
                        }
                        else
                        {
                            alertControl.Show(this, "ERROR", "INSERT VoucherCode Failed !");                            
                        }
                    }
                    else
                    {
                        alertControl.Show(this, "Already Exist", "လအတွက် ကုဒ်ထည့်ပြီး ဖြစ်သည်။");                        
                    }
                }
                else
                {
                    alertControl.Show(this, "Voucher Code Required", "ကုဒ်အမှတ် ထဲ့ရန်လိုသည်။");                    
                }
            }
            else
            {
                alertControl.Show(this, "Voucher Code Required", "ကုဒ်အမှတ် ထဲ့ရန်လိုသည်။");                
            }
            textEdit_voucher_code.Focus();
        }

        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            textEdit_voucher_code.ResetText();
            textEdit_voucher_code.Focus();
        }

        private void simpleButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void saveMySetting()
        {
            Properties.Settings.Default.datetime_VoucherCode = (DateTime)dateEdit_date.EditValue;
            Properties.Settings.Default.Save();
        }
    }
}