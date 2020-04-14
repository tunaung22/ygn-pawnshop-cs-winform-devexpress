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

namespace ygn
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void simpleButton_pawn_Click(object sender, EventArgs e)
        {
            using (views.PawnTransaction pawn_tran = new views.PawnTransaction())
            {
                pawn_tran.ShowInTaskbar = false;
                pawn_tran.ShowDialog("အပေါင် လက်ခံ", views.PawnTransaction.form_state.add, null, DateTime.Now);
            }
        }
        private void simpleButton_receive_Click(object sender, EventArgs e)
    {
        using (views.PawnReceive pawn_rec = new views.PawnReceive())
        {
            pawn_rec.ShowInTaskbar = false;
            pawn_rec.ShowDialog();
        }
    }
        private void simpleButton_voucher_code_Click(object sender, EventArgs e)
    {   
        using (views.VoucherCode voucher_code = new views.VoucherCode())
        {
            voucher_code.ShowInTaskbar = false;
            voucher_code.ShowDialog("ပြေစာ ကုဒ်အမှတ်", null);
        }
    }

        private void simpleButton_close_balance_Click(object sender, EventArgs e)
    {
        using (views.CloseBalance closeBal = new views.CloseBalance())
        {
            closeBal.ShowInTaskbar = false;
            closeBal.ShowDialog("စာရင်းပိတ်", DateTime.Now);
        }
    }

        private void simpleButton_gold_transaction_Click(object sender, EventArgs e)
    {
        using (views.GoldTransaction goldTran = new views.GoldTransaction())
        {
            goldTran.ShowInTaskbar = false;
            goldTran.ShowDialog("ရွှေရောင်း", views.GoldTransaction.form_type.goldSell, DateTime.Now);
        }
    }
        private void simpleButton_gold_buy_Click(object sender, EventArgs e)
        {
            using (views.GoldTransaction goldBuy = new views.GoldTransaction())
            {
                goldBuy.ShowInTaskbar = false;
                goldBuy.ShowDialog("ရွှေဝယ်", views.GoldTransaction.form_type.goldBuy, DateTime.Now);
            }

        }



        private void simpleButton_withdraw_money_Click(object sender, EventArgs e)
    {
        using (views.CashTransaction cashTran = new views.CashTransaction())
        {
            cashTran.ShowInTaskbar = false;
            cashTran.ShowDialog(views.CashTransaction.form_type.WithdrawMoney, DateTime.Now);
        }
    }
        private void simpleButton_invest_money_Click(object sender, EventArgs e)
    {
        using (views.CashTransaction cashTran = new views.CashTransaction())
        {
            cashTran.ShowInTaskbar = false;
            cashTran.ShowDialog(views.CashTransaction.form_type.AddInvestMoney, DateTime.Now);
        }
    }

        private void simpleButton_expired_invoice_Click(object sender, EventArgs e)
    {
        using (views.ExpiredInvoice expInv = new views.ExpiredInvoice())
        {
            expInv.ShowInTaskbar = false;
            expInv.ShowDialog("အပေါင်ဆုံး စာရင်း", DateTime.Now);
        }
    }
        private void simpleButton_report_Click(object sender, EventArgs e)
    {
        using (views.Report rptForm = new views.Report())
        {
            rptForm.ShowInTaskbar = false;
            rptForm.ShowDialog("စာရင်းချုပ်", DateTime.Now);
        }
    }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        

        
    }
}