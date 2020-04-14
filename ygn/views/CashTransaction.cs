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
    public partial class CashTransaction : DevExpress.XtraEditors.XtraForm
    {
        public CashTransaction()
        {
            InitializeComponent();
        }

        public enum form_type { WithdrawMoney, AddInvestMoney };
        public form_type frm_typ;
        helpers.dbhelper dbhelper = new helpers.dbhelper();
        byte _t = 0;

        public void ShowDialog(form_type _typ, DateTime _dtime)
        {
            switch (_typ)
            {
                case form_type.AddInvestMoney:
                    if (Properties.Settings.Default.datetime_cash_invest == null)
                    {
                        dateEdit_date.EditValue = DateTime.Now;
                    }
                    else
                    {
                        dateEdit_date.EditValue = (DateTime)Properties.Settings.Default.datetime_cash_invest;
                    }
                    
                    this.Text = "စိုက်ငွေ";
                    this.layoutControlItem_total.Text = "စိုက်ငွေပေါင်း";
                    _t = 1;
                    break;
                case form_type.WithdrawMoney:
                    if (Properties.Settings.Default.datetime_cash_withdraw == null)
                    {
                        dateEdit_date.EditValue = DateTime.Now;
                    }
                    else
                    {
                        dateEdit_date.EditValue = (DateTime)Properties.Settings.Default.datetime_cash_withdraw;
                    }
                    
                    this.layoutControlItem_total.Text = "သိမ်းငွေပေါင်း";
                    _t = 2;
                    this.Text = "ငွေသိမ်း";
                    break;
            }
            this.ShowDialog();
        }

        private void CashTransaction_Load(object sender, EventArgs e)
        {
            // Get last used date value
            /*
            if (Properties.Settings.Default.datetime_CashTransaction != null)
            {
                dateEdit_date.EditValue = Properties.Settings.Default.datetime_CashTransaction;
            }
            else
            {
                dateEdit_date.EditValue = DateTime.Now;
            }
            */
            dateEdit_date.EditValue = DateTime.Now;
            setButtonState("default");
            setControlState(false);
            updateGridControl((DateTime)dateEdit_date.EditValue, _t);
            //dateEdit_date.EditValue = DateTime.Now;
        }

        

        private void simpleButton_new_Click(object sender, EventArgs e)
        {
            setButtonState("add");
            setControlState(true);
            spinEdit_money.Focus();
        }

        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(memoEdit_description.EditValue.ToString()))
            { memoEdit_description.EditValue = "-"; }

            if(Convert.ToInt32(spinEdit_money.EditValue) != 0)
            {
                if(dbhelper.insertCashTransaction((DateTime)dateEdit_date.EditValue, _t, Convert.ToDecimal(spinEdit_money.EditValue), (string)memoEdit_description.EditValue.ToString()))
                {
                    updateGridControl((DateTime)dateEdit_date.EditValue, _t);
                    setButtonState("default");
                    setControlState(false);
                    saveMySetting();
                }
                else
                {
                    alertControl.Show(this, "ERROR", "INSERT CashTransaction failed.");
                }
            }
            else
            {
                alertControl.Show(this, "Invalid Value", "Enter Transaction Amount.");
            }   
        }

        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            setButtonState("default");
            setControlState(false);
            clearControlData();
            
        }

        private void simpleButton_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {
            updateGridControl((DateTime)dateEdit_date.EditValue, _t);
        }

        private void updateGridControl(DateTime _dt, byte _typ)
        {
            gridControl_list.DataSource = null;
            
            gridControl_list.DataSource = dbhelper.selectCashTransactionInfoByDateByType(_dt, _typ);

            if (gridControl_list.DataSource != null)
            {
                spinEdit_total_money.EditValue = dbhelper.selectTotalCashTransactionAmountByDateByType(_dt, _typ);
            }
        }


        private void setControlState(bool p)
        {
            spinEdit_money.Enabled = p;
            memoEdit_description.Enabled = p;
        }
        private void setButtonState(string _str)
        {
            switch (_str)
            {
                case "default":
                    simpleButton_new.Enabled = true;
                    simpleButton_ok.Enabled = false;
                    simpleButton_cancel.Enabled = false;
                    simpleButton_exit.Enabled = true;
                    break;
                case "add":
                    simpleButton_new.Enabled = false;
                    simpleButton_ok.Enabled = true;
                    simpleButton_cancel.Enabled = true;
                    simpleButton_exit.Enabled = true;
                    break;
                default:
                    break;
            }

        }
        private void clearControlData()
        {
            spinEdit_money.EditValue = 0;
            memoEdit_description.ResetText();
        }

        private void saveMySetting()
        {
            if (frm_typ == form_type.AddInvestMoney)
            {
                Properties.Settings.Default.datetime_cash_invest = (DateTime)dateEdit_date.EditValue;
            }
            else if (frm_typ == form_type.WithdrawMoney)
            {
                Properties.Settings.Default.datetime_cash_withdraw = (DateTime)dateEdit_date.EditValue;
            }
            
            Properties.Settings.Default.Save();
        }
   
    }
}