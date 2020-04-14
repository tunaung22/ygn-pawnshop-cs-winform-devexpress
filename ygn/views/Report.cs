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
    public partial class Report : DevExpress.XtraEditors.XtraForm
    {
        public Report()
        {
            InitializeComponent();
        }

        helpers.dbhelper dbhelper = new helpers.dbhelper();

        public void ShowDialog(string _title, DateTime _dt)
        {
            if (Properties.Settings.Default.datetime_Report.Date != null)
            {
                dateEdit_date.EditValue = Properties.Settings.Default.datetime_Report.Date;
            }
            else
            {
                dateEdit_date.EditValue = _dt;
            }
            this.Text = _title;         
            this.ShowDialog();
        }

        private void DailyReport_Load(object sender, EventArgs e)
        {
            dateEdit_date.EditValue = DateTime.Now;
            updateGridControl((DateTime)dateEdit_date.EditValue, (byte)(radioGroup_type.SelectedIndex + 1), (byte)(radioGroup_voucher_type.SelectedIndex));            
        }

        private void dateEdit_date_EditValueChanged(object sender, EventArgs e)
        {
            updateGridControl((DateTime)dateEdit_date.EditValue, (byte)(radioGroup_type.SelectedIndex + 1), (byte)(radioGroup_voucher_type.SelectedIndex));
        }

        /// <summary>
        /// Update Grid Control
        /// </summary>
        /// <param name="_dt">Current Date Time</param>
        /// <param name="_typ">Transaction Type (pawn, receive, expire)</param>
        /// <param name="_vtype">Voucher Type</param>
        private void updateGridControl(DateTime _dt, byte _typ, byte _vtype)
        {
            DataTable _tbl_pawn_info = null;
            DataTable _tbl_receive_info = null;
            DataTable _tbl_expire_info = null;
            gridControl_list.DataSource = "";
            gridView_list.Columns.Clear();
            switch (_typ)
            {
                case 1:
                    
                    switch (_vtype)
                    {
                        case 0:
                            _tbl_pawn_info = dbhelper.selectPawnTransactionInfoByPawnDateShort(_dt);
                            break;
                        case 1:
                            _tbl_pawn_info = dbhelper.selectPawnTransactionInfoByPawnDateShortByVoucherType(_dt, 1);
                            break;
                        case 2:
                            _tbl_pawn_info = dbhelper.selectPawnTransactionInfoByPawnDateShortByVoucherType(_dt, 2);
                            break;
                        default:
                            break;
                    }                    
                    gridControl_list.DataSource = _tbl_pawn_info;                    
                    break;
                case 2:
                    switch (_vtype)
                    {
                        case 0:
                            _tbl_receive_info = dbhelper.selectPawnReceiveTransactionInfoByReceiveDate(_dt);
                            break;
                        case 1:
                            _tbl_receive_info = dbhelper.selectPawnReceiveTransactionInfoByReceiveDateByVoucherType(_dt, 1);
                            break;
                        case 2:
                            _tbl_receive_info = dbhelper.selectPawnReceiveTransactionInfoByReceiveDateByVoucherType(_dt, 2);
                            break;
                        default:
                            break;
                    }                    
                    gridControl_list.DataSource = _tbl_receive_info;
                    gridView_list.Columns["interest_rate"].Width = 100;
                    gridView_list.Columns["interest_rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    break;
                case 3:
                    switch (_vtype)
                    {
                        case 0:
                            _tbl_expire_info = dbhelper.selectExpriedInvoiceInfoByDate(_dt);
                            break;
                        case 1:
                            _tbl_expire_info = dbhelper.selectExpriedInvoiceInfoByDateByVoucherType(_dt, 1);
                            break;
                        case 2:
                            _tbl_expire_info = dbhelper.selectExpriedInvoiceInfoByDateByVoucherType(_dt, 2);
                            break;
                        default:
                            break;
                    }                    
                    gridControl_list.DataSource = _tbl_expire_info;
                    break;
                default:
                    break;
            }


            initGridProperty(_typ);
            fillSummaryData(_dt);
        }

        private void initGridProperty(byte _typ)
        {
            switch (_typ)
            {
                case 1:
                    gridView_list.Columns[0].Width = 64;
                    gridView_list.Columns["no"].Width = 50;
                    gridView_list.Columns["voucher_no"].Width = 100;
                    gridView_list.Columns["customer_name"].Width = 175;
                    gridView_list.Columns["customer_nrc"].Width = 100;
                    gridView_list.Columns["item_weight"].Width = 150;
                    gridView_list.Columns["en_amount"].Width = 100;            
                    gridView_list.Columns["en_amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    gridView_list.Columns["month_count"].Width = 100;
                    gridView_list.Columns["month_count"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                    gridView_list.Columns["no"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView_list.Columns["en_amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;                    
                    initGridSummary(_typ);

                    break;
                case 2:
                    gridView_list.Columns[0].Width = 64;
                    gridView_list.Columns["no"].Width = 50;
                    gridView_list.Columns["voucher_no"].Width = 100;
                    gridView_list.Columns["customer_name"].Width = 175;
                    gridView_list.Columns["customer_nrc"].Width = 100;
                    gridView_list.Columns["customer_address"].Width = 100;                    
                    gridView_list.Columns["item_weight"].Width = 150;

                    gridView_list.Columns["en_amount"].Width = 100;            
                    gridView_list.Columns["en_amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    gridView_list.Columns["interest_rate"].Width = 75;
                    gridView_list.Columns["interest_rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    gridView_list.Columns["interest_amount"].Width = 75;
                    gridView_list.Columns["interest_amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    gridView_list.Columns["receive_amount"].Width = 100;
                    gridView_list.Columns["receive_amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                    gridView_list.Columns["no"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView_list.Columns["en_amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView_list.Columns["interest_amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;                    
                    gridView_list.Columns["receive_amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;                    
                    initGridSummary(_typ);

                    break;
                case 3:                    
                    gridView_list.Columns["no"].Width = 50;
                    gridView_list.Columns["invoice_number"].Width = 100;
                    gridView_list.Columns["customer_name"].Width = 175;
                    gridView_list.Columns["customer_nrc"].Width = 100;
                    gridView_list.Columns["customer_address"].Width = 100;                    
                    gridView_list.Columns["item_name"].Width = 150;
                    gridView_list.Columns["item_weight"].Width = 50;
                    gridView_list.Columns["en_amount"].Width = 150;
                    gridView_list.Columns["en_amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;                    
                    gridView_list.Columns["pawn_date"].Width = 175;
                    gridView_list.Columns["month_count"].Width = 100;
                    
                    
                    gridView_list.Columns["en_amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;                                        
                    gridView_list.Columns["month_count"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    initGridSummary(_typ);

                    break;
            }

            

            for (int i = 0; i < gridView_list.Columns.Count; i++)
            {
                gridView_list.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }           
        }

        /// <summary>
        /// Setup Grid Control Summary Items By Transaction Type (pawn , receive, expire)
        /// </summary>
        /// <param name="_transactionType">transaction type (byte)</param>
        private void initGridSummary(byte _transactionType)
        {
            gridView_list.OptionsView.ShowFooter = true;

            if(_transactionType == 1)
            {
                gridView_list.Columns["no"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView_list.Columns["en_amount"].SummaryItem.DisplayFormat = "{0:n0}";
            }
            else if(_transactionType == 2)
            {
                gridView_list.Columns["no"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView_list.Columns["en_amount"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView_list.Columns["no"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView_list.Columns["interest_amount"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView_list.Columns["receive_amount"].SummaryItem.DisplayFormat = "{0:n0}";
            }
            else
            {
            }
            //global
            //gridView_list.Columns["en_amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //gridView_list.Columns["en_amount"].FieldName = "en_amount";
            
            
        }


        // where pawndat == date;
        private void fillSummaryData(DateTime _dt)
        {
            int voucher_paper_cost = 10; //get value from db

            spinEdit_total_gold_bought_amount.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType((DateTime)dateEdit_date.EditValue, (byte)1);
            spinEdit_total_gold_sold_amount.EditValue = dbhelper.selectTotalGoldTransactionAmountByDateByType((DateTime)dateEdit_date.EditValue, (byte)2);



            int _total_pawn_amount = dbhelper.selectTotalPawnTransactionAmountByPawnDate(_dt);
            int _total_receive_amount = dbhelper.selectTotalPawnReceiveTransactionAmountByReceiveDate((DateTime)dateEdit_date.EditValue);
            int _gold_bought = Convert.ToInt32(spinEdit_total_gold_bought_amount.EditValue);
            int _gold_sold = Convert.ToInt32(spinEdit_total_gold_sold_amount.EditValue);            
            int _invoice_paper_cost = dbhelper.selectPawnTransactionCountByPawnDate(_dt);
            int _total_invest_money = dbhelper.selectTotalInvestMoneyByDate(_dt);
            int _total_withdraw_money = dbhelper.selectTotalWithdrawMoneyByDate(_dt);

            spinEdit_total_pawn_amount_vtype1.EditValue = dbhelper.selectTotalPawnTransactionAmountByPawnDateByVoucherType(_dt, 1);
            spinEdit_total_pawn_amount_vtype2.EditValue = dbhelper.selectTotalPawnTransactionAmountByPawnDateByVoucherType(_dt, 2);
            spinEdit_total_pawn_amount.EditValue = _total_pawn_amount;
            
            spinEdit_total_receive_amount_vtype1.EditValue = dbhelper.selectTotalPawnReceiveTransactionAmountByReceiveDateByVoucherType((DateTime)dateEdit_date.EditValue, 1);
            spinEdit_total_receive_amount_vtype2.EditValue = dbhelper.selectTotalPawnReceiveTransactionAmountByReceiveDateByVoucherType((DateTime)dateEdit_date.EditValue, 2);
            spinEdit_total_receive_amount.EditValue = _total_receive_amount;

            spinEdit_total_invoice_paper_amount.EditValue = _invoice_paper_cost * voucher_paper_cost;
            spinEdit_total_income.EditValue = (_total_receive_amount + _gold_sold + _invoice_paper_cost);
            spinEdit_total_invested.EditValue = (_total_pawn_amount + _gold_bought);

            spinEdit_total_cash_left.EditValue = Convert.ToInt32(spinEdit_total_income.EditValue) - Convert.ToInt32(spinEdit_total_invested.EditValue);
            if (Convert.ToInt32(spinEdit_total_cash_left.EditValue) < 0)
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.Red;
            }
            else
            {
                spinEdit_total_cash_left.Properties.Appearance.ForeColor = Color.DarkGreen;
            }
        }


        
        

        private void simpleButton_show_report_Click(object sender, EventArgs e)
        {
            switch(radioGroup_type.SelectedIndex + 1)
            {
                case 1:
                    using (report.rptPawn rpt = new report.rptPawn())
                    {
                        rpt.RequestParameters = false;
                        DataTable _tbl = new DataTable("pawn");
                        _tbl = dbhelper.selectPawnTransactionInfoByPawnDate((DateTime)dateEdit_date.EditValue);                        
                        rpt.DataSource = _tbl;
                        rpt.DataMember = _tbl.TableName;
                        rpt.ShowPreviewDialog((DateTime)dateEdit_date.EditValue);
                    }
                    break;
                case 2:
                    using (report.rptPawnReceive rpt = new report.rptPawnReceive())
                    {
                        rpt.RequestParameters = false;
                        DataTable _tbl = new DataTable("receive");
                        _tbl = null;// dbhelper.selectPawnReceiveTransactionInfoByReceiveDate((DateTime)dateEdit_date.EditValue);
                        rpt.DataSource = _tbl;
                        rpt.DataMember = _tbl.TableName;
                        rpt.ShowPreviewDialog((DateTime)dateEdit_date.EditValue);
                    }
                    break;
                case 3:
                    using (report.rptExpired rpt = new report.rptExpired())
                    {
                        rpt.RequestParameters = false;
                        DataTable _tbl = new DataTable();
                        _tbl = dbhelper.selectExpriedInvoiceInfoByDate((DateTime)dateEdit_date.EditValue);
                        rpt.DataSource = _tbl;
                        rpt.DataMember = _tbl.TableName;
                        rpt.ShowPreviewDialog((DateTime)dateEdit_date.EditValue);
                    }
                    break;
                   
                default:
                    break;
            }
            
        }

        private void radioGroup_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            layoutControlGroup_gridControl.Text = radioGroup_type.Properties.Items[radioGroup_type.SelectedIndex].Description.ToString();
            updateGridControl((DateTime)dateEdit_date.EditValue, (byte)(radioGroup_type.SelectedIndex + 1), (byte)(radioGroup_voucher_type.SelectedIndex));
        }

        private void radioGroup_voucher_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            layoutControlGroup_gridControl.Text = radioGroup_type.Properties.Items[radioGroup_type.SelectedIndex].Description.ToString();
            updateGridControl((DateTime)dateEdit_date.EditValue, (byte)(radioGroup_type.SelectedIndex + 1), (byte)(radioGroup_voucher_type.SelectedIndex));
        }

    }
}