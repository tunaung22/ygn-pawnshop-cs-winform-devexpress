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
    public partial class GoldTransaction : DevExpress.XtraEditors.XtraForm
    {
        public GoldTransaction()
        {
            InitializeComponent();
        }

        public enum form_type { goldBuy, goldSell };
        public form_type frm_typ;

        public void ShowDialog(string _title, form_type frm_typ,  DateTime _dt)
        {
            switch (frm_typ)
            {
                case form_type.goldBuy:
                    if (Properties.Settings.Default.datetime_gold_buy == null)
                    {
                        dateEdit_date.EditValue = DateTime.Now;
                    }
                    else
                    {
                        dateEdit_date.EditValue = Properties.Settings.Default.datetime_gold_buy;
                    }

                    radioGroup_transaction_type.SelectedIndex = 0;
                    break;

                case form_type.goldSell:
                    if (Properties.Settings.Default.datetime_gold_sell == null)
                    {
                        dateEdit_date.EditValue = DateTime.Now;
                    }
                    else
                    {
                        dateEdit_date.EditValue = Properties.Settings.Default.datetime_gold_sell;
                    }

                    radioGroup_transaction_type.SelectedIndex = 1;
                    break;

            }
            this.Text = _title;
            this.ShowDialog();
        }

        helpers.dbhelper dbhelper = new helpers.dbhelper();

        private void GoldTransaction_Load(object sender, EventArgs e)
        {
            setButtonState("default");
            setControlState(false);
            dateEdit_date.EditValue = DateTime.Now;
            //updateGridControl();
        }

        private void simpleButton_new_Click(object sender, EventArgs e)
        {
            setButtonState("add");
            setControlState(true);
            clearControlData();
        }

        private void simpleButton_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(memoEdit_description.EditValue.ToString()))
            {
                memoEdit_description.EditValue = "-";
            }
            if (dbhelper.insertGoldTransaction((DateTime)dateEdit_date.EditValue, Convert.ToByte(radioGroup_transaction_type.Properties.Items[radioGroup_transaction_type.SelectedIndex].Value), Convert.ToDecimal(spinEdit_transaction_amount.EditValue), (string)memoEdit_description.EditValue.ToString()))
            {
                alertControl.AppearanceCaption.ForeColor = Color.DarkGreen;
                alertControl.AppearanceText.ForeColor = Color.DarkGreen;
                alertControl.Show(this, "Success", "Saved Successful !");
                setControlState(false);
                setButtonState("default");
                updateGridControl();
                saveMySetting();
            }
            else
            {
                alertControl.AppearanceCaption.ForeColor = Color.Red;
                alertControl.AppearanceText.ForeColor = Color.Red;
                alertControl.Show(this, "Error", "Failed to insert data !");
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



        private void updateGridControl()
        {
            spinEdit_total_bought.EditValue = 0;
            spinEdit_total_sold.EditValue = 0;

            
            

            if (radioGroup_transaction_type.SelectedIndex == 0)
            {
                gridControl_gold_transaction.DataSource = dbhelper.selectGoldBuyByDate((DateTime)dateEdit_date.EditValue);
                spinEdit_total_bought.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType((DateTime)dateEdit_date.EditValue, (byte)1);
                layoutControlItem_total_bought.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_total_sold.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else if (radioGroup_transaction_type.SelectedIndex == 1)
            {
                gridControl_gold_transaction.DataSource = dbhelper.selectGoldSellByDate((DateTime)dateEdit_date.EditValue);
                spinEdit_total_sold.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType((DateTime)dateEdit_date.EditValue, (byte)2);
                layoutControlItem_total_bought.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_total_sold.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }



            //gridControl_gold_transaction.DataSource = dbhelper.selectGoldTransactionByDate((DateTime)dateEdit_date.EditValue);
            
            /*
            if (gridView_gold_transaction.DataSource != null)
            {
                gridView_gold_transaction.Columns["transaction_type"].Group();
                gridView_gold_transaction.ExpandAllGroups();

                spinEdit_total_bought.EditValue = 0;
                spinEdit_total_sold.EditValue = 0;
            }
            */

        }

        private void setButtonState(string _sts)
        {
            switch (_sts)
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
            }
        }
        private void setControlState(bool p)
        {
            //dateEdit_date.Enabled = p;
            //radioGroup_transaction_type.Enabled = p;
            spinEdit_transaction_amount.Enabled = p;
            memoEdit_description.Enabled = p;
        }
        private void clearControlData()
        {
            //dateEdit_date.EditValue = DateTime.Now;
            //radioGroup_transaction_type.SelectedIndex = 0;
            spinEdit_transaction_amount.EditValue = 0;
            memoEdit_description.ResetText();
        }

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {
            updateGridControl();
        }

        private void saveMySetting()
        {
            if (frm_typ == form_type.goldBuy)
            {
                Properties.Settings.Default.datetime_gold_buy= (DateTime)dateEdit_date.EditValue;
            }
            else if (frm_typ == form_type.goldSell)
            {
                Properties.Settings.Default.datetime_gold_sell = (DateTime)dateEdit_date.EditValue;
            }
            
            Properties.Settings.Default.Save();
        }

        private void radioGroup_transaction_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


    }
}