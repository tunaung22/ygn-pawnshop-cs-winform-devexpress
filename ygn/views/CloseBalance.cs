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
    public partial class CloseBalance : DevExpress.XtraEditors.XtraForm
    {
        public CloseBalance()
        {
            InitializeComponent();
        }

        helpers.dbhelper dbhelper = new helpers.dbhelper();
        int _tempOpeningBalance = 0;
        bool _tempOBactive = false;

        public void ShowDialog(string _title, DateTime _dt)
        {
            if (Properties.Settings.Default.datetime_CloseBalance.Date != null)
            {
                dateEdit_date.EditValue = Properties.Settings.Default.datetime_CloseBalance.Date;
            }
            else
            {
                dateEdit_date.EditValue = _dt;
            }
            this.Text = _title;
            this.ShowDialog();
        }

        private void CloseBalance_Load(object sender, EventArgs e)
        {
            setButtonState("default");
            setControlState(false);
            dateEdit_date.EditValue = (Properties.Settings.Default.datetime_CloseBalance).AddDays(1);
            updateGridControl(DateTime.Now, 1);
        }

        private void simpleButton_new_Click(object sender, EventArgs e)
        {
            setButtonState("add");
            setControlState(true);
            //clearControlData();
            //dateEdit_date.EditValue = DateTime.Now;
            int _opening_balance = dbhelper.selectLastClosingBalance();
            if (_opening_balance <= 0)
            {
                _tempOpeningBalance = 0;
            }
            else
            {
                _tempOpeningBalance = _opening_balance;
            }
            spinEdit_opening_balance.EditValue = _tempOpeningBalance;
            _tempOBactive = true;
        }



        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            if(spinEdit_total_cash_left.EditValue != null && spinEdit_total_cash_left.EditValue != null)
            {
                if (dbhelper.insertCashBalance((DateTime)dateEdit_date.EditValue, Convert.ToDecimal(spinEdit_opening_balance.EditValue), Convert.ToDecimal(spinEdit_total_cash_left.EditValue)))
                {
                    updateGridControl((DateTime)dateEdit_date.EditValue, (byte)radioGroup_filter.Properties.Items[radioGroup_filter.SelectedIndex].Value);
                    setButtonState("lock");
                    setControlState(false);
                    simpleButton_ok.Enabled = false;
                    saveMySetting();
                }
                else
                {
                    alertControl.Show(this, "ERROR", "Error insert or updating CashBalance.");
                }
            }    
        }
        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            _tempOBactive = false;
            setButtonState("default");
            setControlState(false);
            //clearControlData();
        }
        private void simpleButton_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {
            _tempOBactive = false;
            fillSummaryData((DateTime)dateEdit_date.EditValue);
            if (radioGroup_filter.SelectedIndex > 0)
            { updateGridControl((DateTime)dateEdit_date.EditValue, (byte)radioGroup_filter.Properties.Items[radioGroup_filter.SelectedIndex].Value); }
        }
        private void spinEdit_invest_money_EditValueChanged(object sender, EventArgs e)
        {
            if (spinEdit_additional_invest_money.EditValue == null)
            {
                spinEdit_additional_invest_money.EditValue = 0;

            }
            calculateSummary();
            /*
            int _in = Convert.ToInt32(spinEdit_additional_invest_money.EditValue);
            int _out = Convert.ToInt32(spinEdit_additional_withdraw_money.EditValue);
            int _left = Convert.ToInt32(spinEdit_total_income_amount.EditValue) - Convert.ToInt32(spinEdit_total_invested_amount.EditValue);
            spinEdit_total_cash_left.EditValue = (_left - _out) + _in;
            */
            if (Convert.ToInt32(spinEdit_total_cash_left.EditValue) < 0)
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.Red;
            }
            else
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.DarkGreen;
            }
        }
        private void spinEdit_withdraw_money_EditValueChanged(object sender, EventArgs e)
        {
            if (spinEdit_additional_withdraw_money.EditValue == null)
            {
                spinEdit_additional_withdraw_money.EditValue = 0;
            }
            calculateSummary();
            /*
            int _in = Convert.ToInt32(spinEdit_additional_invest_money.EditValue);
            int _out = Convert.ToInt32(spinEdit_additional_withdraw_money.EditValue);
            int _left = Convert.ToInt32(spinEdit_total_income_amount.EditValue) - Convert.ToInt32(spinEdit_total_invested_amount.EditValue);
            spinEdit_total_cash_left.EditValue = (_left - _out) + _in;
            */
            if (Convert.ToInt32(spinEdit_total_cash_left.EditValue) < 0)
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.Red;
            }
            else
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.DarkGreen;
            }
        }

        private void simpleButton_add_invest_money_Click(object sender, EventArgs e)
        {
            using (views.CashTransaction cashTran = new views.CashTransaction())
            {
                cashTran.ShowInTaskbar = false;
                cashTran.ShowDialog(views.CashTransaction.form_type.AddInvestMoney, (DateTime)dateEdit_date.EditValue);
            }
            fillSummaryData((DateTime)dateEdit_date.EditValue);
            calculateSummary();
        }

        private void simpleButton_withdraw_money_Click(object sender, EventArgs e)
        {
            using (views.CashTransaction cashTran = new views.CashTransaction())
            {
                cashTran.ShowInTaskbar = false;
                cashTran.ShowDialog(views.CashTransaction.form_type.WithdrawMoney, (DateTime)dateEdit_date.EditValue);
            }
            fillSummaryData((DateTime)dateEdit_date.EditValue);            
            
        }
        
        /// <summary>
        /// This function refresh the data show on the gridControl_list.
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_typ">1,2,3,4</param>
        private void updateGridControl(DateTime _dt, byte _typ)
        {
            gridControl_list.DataSource = "";
            gridView_list.Columns.Clear();
            switch (_typ)
            {
                case 1:
                    gridControl_list.DataSource = dbhelper.selectCashBalanceInfoByMonth(_dt);
                    break;
                case 2:                    
                    gridControl_list.DataSource = dbhelper.selectPawnTransactionInfoByPawnDateShort(_dt);
                    break;
                case 3:
                    gridControl_list.DataSource = null;//dbhelper.selectPawnReceiveTransactionInfoByReceiveDate(_dt);
                    break;
                case 4:
                    gridControl_list.DataSource = dbhelper.selectExpriedInvoiceInfoByDate(_dt);
                    break;
                default:
                    break;
            }         
        }

        

        private void saveMySetting()
        {
            Properties.Settings.Default.datetime_CloseBalance = (DateTime)dateEdit_date.EditValue;
            Properties.Settings.Default.Save();
        }

        private void fillSummaryData(DateTime _dt)
        {
            clearControlData();

            int _opening_balance = dbhelper.selectLastClosingBalanceByDate(_dt);
            if (_opening_balance <= 0)
            {
                _tempOpeningBalance = 0;
                setButtonState("default");
            }
            else
            {
                _tempOpeningBalance = _opening_balance;
                setButtonState("lock");
            }
            spinEdit_opening_balance.EditValue = _tempOpeningBalance;


            if (_tempOBactive)
            {
                int _ob = dbhelper.selectLastClosingBalance();
                spinEdit_opening_balance.EditValue = _ob;
                setButtonState("add");
            }
            else
            {
                //setButtonState("default");
            }
             

            spinEdit_additional_invest_money.EditValue = dbhelper.selectTotalCashTransactionAmountByDateByType(_dt, (byte)1);
            spinEdit_total_receive_amount.EditValue = dbhelper.selectTotalPawnReceiveTransactionAmountByReceiveDate((DateTime)dateEdit_date.EditValue);
            spinEdit_total_invoice_paper_amount.EditValue = (dbhelper.selectPawnTransactionCountByPawnDate(_dt)) * 10;
            spinEdit_total_gold_sold_amount.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType(_dt, (byte)2);

            spinEdit_total_pawn_amount.EditValue = dbhelper.selectTotalPawnTransactionAmountByPawnDate(_dt);
            spinEdit_additional_withdraw_money.EditValue = dbhelper.selectTotalCashTransactionAmountByDateByType(_dt, (byte)2);
            spinEdit_total_gold_bought_amount.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType(_dt, (byte)1);

            calculateSummary();

            if (Convert.ToInt32(spinEdit_total_cash_left.EditValue) < 0)
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.Red;
            }
            else
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.DarkGreen;
            }
        }

        private void calculateSummary()
        {
            int total_in = Convert.ToInt32(spinEdit_opening_balance.EditValue) + Convert.ToInt32(spinEdit_additional_invest_money.EditValue) + Convert.ToInt32(spinEdit_total_receive_amount.EditValue) + Convert.ToInt32(spinEdit_total_invoice_paper_amount.EditValue) + Convert.ToInt32(spinEdit_total_gold_sold_amount.EditValue);
            int total_out = Convert.ToInt32(spinEdit_total_pawn_amount.EditValue) + Convert.ToInt32(spinEdit_additional_withdraw_money.EditValue) + Convert.ToInt32(spinEdit_total_gold_bought_amount.EditValue);

            spinEdit_total_income_amount.EditValue = total_in;
            spinEdit_total_invested_amount.EditValue = total_out;

            spinEdit_total_cash_left.EditValue = total_in - total_out;
        }


        private void setControlState(bool p)
        {
            //dateEdit_date.Enabled = p;
            spinEdit_opening_balance.Enabled = p;
            spinEdit_total_pawn_amount.Enabled = p;
            spinEdit_total_gold_bought_amount.Enabled = p;
            spinEdit_total_receive_amount.Enabled = p;
            spinEdit_total_gold_sold_amount.Enabled = p;
            spinEdit_total_invoice_paper_amount.Enabled = p;
            spinEdit_total_invested_amount.Enabled = p;
            spinEdit_total_income_amount.Enabled = p;
            spinEdit_additional_invest_money.Enabled = p;
            spinEdit_additional_withdraw_money.Enabled = p;
            spinEdit_total_cash_left.Enabled = p;
            simpleButton_add_invest_money.Enabled = p;
            simpleButton_withdraw_money.Enabled = p;
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
                case "lock":
                    simpleButton_new.Enabled = false;
                    simpleButton_ok.Enabled = false;
                    simpleButton_cancel.Enabled = false;
                    simpleButton_exit.Enabled = true;
                    break;
                default:
                    break;
            }

        }
        private void clearControlData()
        {
            //dateEdit_date.EditValue = DateTime.Now;
            spinEdit_opening_balance.EditValue = 0;
            spinEdit_total_pawn_amount.EditValue = 0;
            spinEdit_total_gold_bought_amount.EditValue = 0;

            spinEdit_total_receive_amount.EditValue = 0;
            spinEdit_total_gold_sold_amount.EditValue = 0;
            spinEdit_total_invoice_paper_amount.EditValue = 0;

            spinEdit_total_invested_amount.EditValue = 0;
            spinEdit_total_income_amount.EditValue = 0;

            spinEdit_additional_invest_money.EditValue = 0;
            spinEdit_additional_withdraw_money.EditValue = 0;

            spinEdit_total_cash_left.EditValue = 0;
        }

        private void radioGroup_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillSummaryData((DateTime)dateEdit_date.EditValue);
            updateGridControl((DateTime)dateEdit_date.EditValue, (byte)radioGroup_filter.Properties.Items[radioGroup_filter.SelectedIndex].Value);
        }

        private void spinEdit_opening_balance_EditValueChanged(object sender, EventArgs e)
        {
            calculateSummary();
        }

    }
}