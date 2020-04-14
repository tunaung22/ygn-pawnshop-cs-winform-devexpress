using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Transactions;
using System.Collections;

namespace ygn.helpers
{
    public class dbhelper
    {   
        ygn.models.Ygndb db = new models.Ygndb(ygn.Properties.Settings.Default.ygndbconn);
        Dictionary<string, object> _dic = new Dictionary<string, object>();
        helpers.datetime_helper dthelper = new datetime_helper();
        

        DataTable _tbl = null;

        /// <summary>
        /// Get custom voucher code 
        /// </summary>
        /// <param name="_monthyear">Jan-2013, Feb-2013, ...</param>
        /// <param name="_itemtypecode">1, 2</param>
        /// <param name="_vouchertype">1, 2</param>
        /// <returns>A, AA, B, ...</returns>
        public string selectCustomVoucherCode(string _monthyear, int _itemtypecode, int _vouchertype)
        {
            var query = from c in db.Voucher_codes
                        where c.Voucher_code_id.Equals(_monthyear) && c.Item_type.Equals(_itemtypecode) && c.Voucher_type.Equals(_vouchertype)
                        select c.Custom_voucher_code;
            if (query.Count() > 0)
            {
                foreach (var c in query)
                {
                    return c.ToString();
                }
            }
            return "";            
        }
        /// <summary>
        /// Check custom voucher code already exist or not
        /// </summary>
        /// <param name="_date"></param>
        /// <param name="_itm_type">1 to 2. 1=gold 2=others</param>
        /// <param name="_voucher_type">1 to 2. 1=voucher#1 2=voucher#2</param>
        /// <returns>True=already exist. False=not exist.</returns>
        public bool voucherCodeExist(DateTime _date, int _itm_type, int _voucher_type)
        {   
            var query = from v in db.Voucher_codes
                        where v.Voucher_code_id.Equals(dthelper.MonthYearString(_date.Month, _date.Year)) && v.Item_type == _itm_type && v.Voucher_type == _voucher_type
                        select v.Voucher_code_id;
            if (query.Count() > 0)
            { return true; }
            else
            { return false; }
        }
        /// <summary>
        /// Get Latest Invoice Number
        /// </summary>
        /// <param name="_date"></param>
        /// <param name="_voucher_type"></param>
        /// <returns></returns>
        public string selectLastInvoiceNumber(DateTime _date, int _voucher_type)
        {
            // get 0001,0002
            //string tmp = _monthYear.Replace('-', '/');
            string num = "";
            var query = from n in db.Pawn_transactions
                        where (n.Pawn_date.Year == _date.Year) && (n.Pawn_date.Month == _date.Month) && (n.Voucher_type == _voucher_type)                        
                        select n.Voucher_no;
            if (query.Count() > 0)
            {
                num = query.Max();
                //foreach (var v in query)
                //{
                //    num = v.ToString();
                //}
            }
            else
            {
                return "0000";
            }
            return num.Substring( (num.Count() - 4), 4);
        }
        /// <summary>
        /// Select all voucher code
        /// </summary>
        /// <returns>DatTable(voucher_code_id, custom_voucher_code, item_type, voucher_type)</returns>
        public DataTable selectAllVoucherCode()
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("​လ-ခုနှစ်");
            _tbl.Columns.Add("ဘောက်ချာကုဒ်");            
            _tbl.Columns.Add("ပစ္စည်းအမျိုးအစား");
            _tbl.Columns.Add("ဘောက်ချာအမျိုးအစား");
            var query = from d in db.Voucher_codes
                        orderby d.Voucher_code_id
                        select new { d.Voucher_code_id, d.Custom_voucher_code, d.Item_type, d.Voucher_type };
            if (query.Count() > 0)
            {                
                foreach (var q in query)
                {
                    string _ityp = "";
                    string _vtyp = "";
                    
                    if(q.Item_type == 1)
                    { _ityp = "ရွှေထည်"; }
                    else if(q.Item_type == 2)
                    { _ityp = "အထွေထွေ"; }
                    if(q.Voucher_type == 1)
                    { _vtyp = "တစ်လုံး"; }
                    else if(q.Voucher_type == 2)
                    { _vtyp = "နှစ်လုံး"; }
                    _tbl.Rows.Add(q.Voucher_code_id, q.Custom_voucher_code, _ityp, _vtyp);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        /// <summary>
        /// Check Invoice Number already exist or not
        /// </summary>
        /// <param name="_invno">2013AA0001, 2013A0001, ...</param>
        /// <returns>True=already exist. False=not exist.</returns>
        public bool checkInvoiceNumberExist(string _invno, string _invnosn)
        {
            string _no = "";
            if (_invnosn.Length == 4)
            {
                _no = _no + "";
            }
            else if (_invnosn.Length == 3)
            {
                _no = "0" + _invnosn;
            }
            else if (_invnosn.Length == 2)
            {
                _no = "00" + _invnosn;
            }
            else if (_invnosn.Length == 1)
            {
                _no = "000" + _invnosn;
            }
            string _invoiceNumber = _invno + _no;
            

            var query = from inv in db.Pawn_transactions
                        where inv.Voucher_no.Equals(_invoiceNumber)
                        select inv.Voucher_no;
            if (query.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }            
        }
        /*
        #region wasteful
        public DataTable selectPawnTransaction()
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("နေ့စွဲ");            
            _tbl.Columns.Add("​ပြေစာအမှတ်");
            _tbl.Columns.Add("နာမည်");
            _tbl.Columns.Add("မှတ်ပုံတင်");
            _tbl.Columns.Add("လိပ်စာ");
            _tbl.Columns.Add("ပစ္စည်း");
            _tbl.Columns.Add("ပစ္စည်း အမည်");
            _tbl.Columns.Add("အလေးချိန်");
            _tbl.Columns.Add("တန်ဖိုး");
            
            var query = from pwn in db.Pawn_transactions
                        select pwn;
            if (query.Count() > 0)
            {
                foreach (var p in query)
                {
                    
                    _tbl.Rows.Add(   p.Pawn_date,
                                    p.Voucher_no, 
                                    p.Customer_name,
                                    p.Customer_nrc,
                                    p.Customer_address,
                                    p.Item_type,
                                    p.Item_name, 
                                    p.Item_weight, 
                                    p.En_amount);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        #endregion
        */
        
        /*
        public DataTable selectDailyPawnInfo(DateTime _dt)
        {
            _tbl = new DataTable();            
            _tbl.Columns.Add("no");
            _tbl.Columns["no"].Caption = "စဉ်";            
            _tbl.Columns["no"].AutoIncrement = true;
            //tbl.Columns.Add("pawn_date");
            //tbl.Columns["pawn_date"].Caption = "နေ့စွဲ";                        
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns["voucher_no"].Caption = "​ပြေစာအမှတ်";            
            _tbl.Columns.Add("customer_name");
            _tbl.Columns["customer_name"].Caption = "အမည်";            
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns.Add("customer_address");
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns.Add("item_code");
            _tbl.Columns["item_code"].Caption = "ပစ္စည်း";
            _tbl.Columns.Add("item_name");
            _tbl.Columns["item_name"].Caption = "ပစ္စည်း အမည်";
            _tbl.Columns.Add("item_weight");
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns.Add("en_amount");
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး";
            
            var query = from pwn in db.Pawn_transactions
                        where pwn.Pawn_date.Day == _dt.Day && pwn.Pawn_date.Month == _dt.Month && pwn.Pawn_date.Year == _dt.Year
                        select pwn;
            if (query.Count() > 0)
            {
                foreach (var p in query)
                {
                    int c = 0;
                    c += 1;

                    _tbl.Rows.Add(   c,
                                    //p.Pawn_date,
                                    p.Voucher_no,
                                    p.Customer_name,
                                    p.Customer_nrc,
                                    p.Customer_address,
                                    p.Item_type, 
                                    p.Item_name, 
                                    p.Item_weight, 
                                    p.En_amount);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        */
        /// <summary>
        /// Get last closing balance value
        /// </summary>
        /// <returns>Int32</returns>
        public int selectLastClosingBalance()
        {
            int _lastid = 0;
            var tmp = from c in db.Cash_balances                      
                      select c.Balance_id;
            if (tmp.Count() > 0)
            {
                _lastid = (from cb in db.Cash_balances
                            select cb.Balance_id).Max();
                if (_lastid > 0)
                {                    
                    var query = from c in db.Cash_balances
                                where c.Balance_id == _lastid
                                select c.Closing_balance;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            return Convert.ToInt32(v);
                        }
                    }
                }                
            }
            return 0;            
        }
        /// <summary>
        /// Get last closing balance value filter by date
        /// </summary>
        /// <returns>Int32</returns>
        public int selectLastClosingBalanceByDate(DateTime _dt)
        {
            var query = from c in db.Cash_balances
                      where c.Entry_date.Day == _dt.Day && c.Entry_date.Month == _dt.Month && c.Entry_date.Year == _dt.Year
                      select c.Opening_balance;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    return Convert.ToInt32(v);
                }
            }
            return 0;
        }

        public bool alreadyClosed(DateTime _dt)
        {
            return true;
        }

        /*
        public int selectTotalCashTransactionByDateByType(DateTime _dt, byte _typ)
        {
            int _lastid = 0;
            var tmp = from c in db.Cash_balances
                      select c.Balance_id;
            if (tmp.Count() > 0)
            {
                _lastid = (from cb in db.Cash_balances
                       select cb.Balance_id).Max();
                if (_lastid > 0)
                {
                    var query = from c in db.Cash_balances
                                where c.Balance_id == _lastid && c.Entry_date.Day == _dt.Day && c.Entry_date.Month == _dt.Month && c.Entry_date.Year == _dt.Year
                                select c.Withdraw_money;
                    if (query.Count() > 0)
                    {
                        return Convert.ToInt32(query.Sum());
                    }
                }
            }
            return 0;
        }
        */

#region Filter by Date
        /// <summary>
        /// Select PawnTransaction info by Pawn Date
        /// </summary>
        /// <param name="_dt">Pawn Date</param>        
        /// <returns>DatTable(no, voucher_no, customer_name, customer_nrc, customer_address, item_type, item_name, item_weight, en_amount)</returns>
        public DataTable selectPawnTransactionInfoByPawnDate(DateTime _dt)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["no"].AutoIncrement = true;
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns["voucher_no"].Caption = "​ပြေစာအမှတ်";
            _tbl.Columns.Add("customer_name");
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns.Add("customer_address");
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns.Add("item_code");
            _tbl.Columns["item_code"].Caption = "ပစ္စည်း";
            _tbl.Columns.Add("item_name");
            _tbl.Columns["item_name"].Caption = "ပစ္စည်း အမည်";
            _tbl.Columns.Add("item_weight");
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns.Add("en_amount");
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး";

            var query = from pwn in db.Pawn_transactions
                        where pwn.Pawn_date.Day == _dt.Day && pwn.Pawn_date.Month == _dt.Month && pwn.Pawn_date.Year == _dt.Year
                        select pwn;
            if (query.Count() > 0)
            {
                int c = 0;
                foreach (var p in query)
                {
                    c += 1;
                    _tbl.Rows.Add(c, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Customer_address, p.Item_type, p.Item_name, p.Item_weight, p.En_amount);
                }
            }
            return _tbl;
        }
        /// <summary>
        /// Select PawnTransaction by Date by VoucherType
        /// </summary>
        /// <remarks>
        /// CashBalance's GridControl,
        /// </remarks>
        /// <param name="_dt"></param>
        /// <returns>DataTable(no, voucher_no, customer_name, customer_nrc, item_name,item_weight, en_amount, month_count)</returns>
        public DataTable selectPawnTransactionInfoByPawnDateShort(DateTime _dt)
        {
            
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("month_count");

            
            _tbl.Columns[0].Caption = "စဉ်";
            _tbl.Columns[1].Caption = "ပြေစာအမှတ်";
            _tbl.Columns[2].Caption = "အမည်";
            _tbl.Columns[3].Caption = "မှတ်ပုံတင်";
            _tbl.Columns[4].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns[5].Caption = "အလေးချိန်";
            
            _tbl.Columns[6].Caption = "တန်ဖိုး";
            

            _tbl.Columns[7].Caption = "လအရေအတွက်";
            
            
            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year
                        select p;//select new { p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, p.Month_count };
            if (query.Count() > 0)
            {
                int i = 0;
                foreach (var p in query)
                {
                    i++;
                    _tbl.Rows.Add(i, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, dthelper.monthDifferent(p.Pawn_date, _dt));
                    
                }
            }
            return _tbl;
        }



        public DataTable selectPawnTransactionInfoByPawnDateShortByVoucherType1(DateTime _dt, byte _vtype)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("month_count");

            _tbl.Columns[0].Caption = "စဉ်";
            _tbl.Columns[1].Caption = "ပြေစာအမှတ်";
            _tbl.Columns[2].Caption = "အမည်";
            _tbl.Columns[3].Caption = "မှတ်ပုံတင်";
            _tbl.Columns[4].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns[5].Caption = "အလေးချိန်";
            _tbl.Columns[6].Caption = "တန်ဖိုး";
            _tbl.Columns[7].Caption = "လအရေအတွက်";

            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year && p.Voucher_type == _vtype
                        select p;//select new { p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, p.Month_count };
            if (query.Count() > 0)
            {
                int i = 0;
                foreach (var p in query)
                {
                    i++;
                    _tbl.Rows.Add(i, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, dthelper.monthDifferent(p.Pawn_date, _dt));
                }
            }
            return _tbl; 
        }

        public DataTable selectPawnTransactionInfoByPawnDateShortByVoucherType2(DateTime _dt, byte _vtype)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("month_count");

            _tbl.Columns[0].Caption = "စဉ်";
            _tbl.Columns[1].Caption = "ပြေစာအမှတ်";
            _tbl.Columns[2].Caption = "အမည်";
            _tbl.Columns[3].Caption = "မှတ်ပုံတင်";
            _tbl.Columns[4].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns[5].Caption = "အလေးချိန်";
            _tbl.Columns[6].Caption = "တန်ဖိုး";
            _tbl.Columns[7].Caption = "လအရေအတွက်";

            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year && p.Voucher_type == _vtype
                        select p;//select new { p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, p.Month_count };
            if (query.Count() > 0)
            {
                int i = 0;
                foreach (var p in query)
                {
                    i++;
                    _tbl.Rows.Add(i, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, dthelper.monthDifferent(p.Pawn_date, _dt));
                }
            }
            return _tbl;
        }



















        /// <summary>
        /// Select PawnTransaction by Date Filterd By Voucher Type
        /// </summary>
        /// <remarks>
        /// CashBalance's GridControl,
        /// </remarks>
        /// <param name="_dt"></param>
        /// <returns>DataTable(no, voucher_no, customer_name, customer_nrc, item_name,item_weight, en_amount, month_count)</returns>
        public DataTable selectPawnTransactionInfoByPawnDateShortByVoucherType(DateTime _dt, byte vtype)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("month_count");

            _tbl.Columns[0].Caption = "စဉ်";
            _tbl.Columns[1].Caption = "ပြေစာအမှတ်";
            _tbl.Columns[2].Caption = "အမည်";
            _tbl.Columns[3].Caption = "မှတ်ပုံတင်";
            _tbl.Columns[4].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns[5].Caption = "အလေးချိန်";
            _tbl.Columns[6].Caption = "တန်ဖိုး";
            _tbl.Columns[7].Caption = "လအရေအတွက်";

            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year && p.Voucher_type == vtype
                        select p;//select new { p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, p.Month_count };
            if (query.Count() > 0)
            {
                int i = 0;
                foreach (var p in query)
                {
                    i++;
                    _tbl.Rows.Add(i, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Item_name, p.Item_weight, p.En_amount, dthelper.monthDifferent(p.Pawn_date, _dt));
                }
            }
            return _tbl;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Select PawnReceiveTransactionInfoByReceiveDate
        /// </summary>
        /// <param name="_dt">Receive Date</param>
        /// <returns>DataTable(no, voucher_no, customer_name, customer_nrc, customer_address, item_type, item_name, item_weight, en_amount, is_received)</returns>
        /*public DataTable selectPawnReceiveTransactionInfoByReceiveDate(DateTime _dt)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["no"].AutoIncrement = true;
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns["voucher_no"].Caption = "​ပြေစာအမှတ်";
            _tbl.Columns.Add("customer_name");
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns.Add("customer_address");
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns.Add("item_code");
            _tbl.Columns["item_code"].Caption = "ပစ္စည်း";
            _tbl.Columns.Add("item_name");
            _tbl.Columns["item_name"].Caption = "ပစ္စည်း အမည်";
            _tbl.Columns.Add("item_weight");
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns.Add("en_amount");
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns.Add("is_received");

            var query = from pwn in db.Pawn_transactions
                        where pwn.Is_received == true && pwn.Receive_date.Day == _dt.Day && pwn.Receive_date.Month == _dt.Month && pwn.Receive_date.Year == _dt.Year
                        select pwn;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var p in query)
                {
                    _i++;
                    _tbl.Rows.Add(_i, p.Voucher_no, p.Customer_name, p.Customer_nrc, p.Customer_address, p.Item_type, p.Item_name, p.Item_weight, p.En_amount, p.Is_received);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        */
        
        
        
        /// <summary>
        /// Select Pawn Receive Transaction Info By Receive Date
        /// </summary>
        /// <param name="_dt">Pawn Receive Date</param>
        /// <returns></returns>
        public DataTable selectPawnReceiveTransactionInfoByReceiveDate(DateTime _rdate)
        {
            
            _tbl = new DataTable("receive_info");            
            
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("voucher_no");            
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("customer_address");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("interest_rate");
            _tbl.Columns.Add("interest_amount");
            _tbl.Columns.Add("receive_amount");
            

            
            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["voucher_no"].Caption = "ပြေစာအမှတ်";            
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns["item_name"].Caption = "ပစ္စည်း";
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး (အပေါင်)";
            _tbl.Columns["interest_rate"].Caption = "အတိုးနှုန်း";            
            _tbl.Columns["interest_amount"].Caption = "အတိုးတန်ဖိုး";
            _tbl.Columns["receive_amount"].Caption = "အပေါင်ရွေးတန်ဖိုး";            
            
            var query = from p in db.Pawn_transactions
                        where p.Receive_date.Day == _rdate.Day && p.Receive_date.Month == _rdate.Month && p.Receive_date.Year == _rdate.Year && p.Is_received == true
                        select p;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var p in query)
                {
                    _i++;
                    int _intst = 0;
                    DateTime expdate = Convert.ToDateTime(dthelper.calculateExpireDate(p.Pawn_date, 0));
                    byte mcount = Convert.ToByte(dthelper.monthDifferentForPawnReceive(_rdate, p.Pawn_date, expdate));

                    _intst = calculateInterestAmount(Convert.ToDecimal(p.Interest_rate), mcount, p.En_amount);
                    // Convert.ToByte(dthelper.monthDifferent((DateTime)p.Pawn_date, _dt)), p.En_amount);

                    //dthelper.monthDifferentForPawnReceive();


                    _tbl.Rows.Add(  _i, 
                                    p.Voucher_no, 
                                    p.Customer_name, 
                                    p.Customer_nrc, 
                                    p.Customer_address, 
                                    p.Item_name, 
                                    p.Item_weight, 
                                    p.En_amount, 
                                    (p.Interest_rate + " % "), 
                                    _intst, 
                                    Convert.ToInt32(_intst + p.En_amount)
                                  );
                    
                }
            }
            return _tbl;
        }

        /// <summary>
        /// Select Pawn Receive Transaction Info By Receive Date
        /// </summary>
        /// <param name="_rdate">Pawn Receive Date</param>
        /// <returns></returns>
        public DataTable selectPawnReceiveTransactionInfoByReceiveDateByVoucherType(DateTime _rdate, byte vtype)
        {
            _tbl = new DataTable("receive_info");
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("voucher_no");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("customer_address");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("interest_rate");
            _tbl.Columns.Add("interest_amount");
            _tbl.Columns.Add("receive_amount");

            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["voucher_no"].Caption = "ပြေစာအမှတ်";
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns["item_name"].Caption = "ပစ္စည်း";
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး (အပေါင်)";
            _tbl.Columns["interest_rate"].Caption = "အတိုးနှုန်း";
            _tbl.Columns["interest_amount"].Caption = "အတိုးတန်ဖိုး";
            _tbl.Columns["receive_amount"].Caption = "အပေါင်ရွေးတန်ဖိုး";

            var query = from p in db.Pawn_transactions
                        where p.Receive_date.Day == _rdate.Day && p.Receive_date.Month == _rdate.Month && p.Receive_date.Year == _rdate.Year && p.Is_received == true && p.Voucher_type == vtype
                        select p;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var p in query)
                {
                    _i++;
                    int _intst = 0;
                    DateTime expdate = Convert.ToDateTime(dthelper.calculateExpireDate(p.Pawn_date, 0));
                    byte mcount = Convert.ToByte(dthelper.monthDifferentForPawnReceive(_rdate, p.Pawn_date, expdate));
                    _intst = calculateInterestAmount(Convert.ToDecimal(p.Interest_rate), mcount, p.En_amount);
                    _tbl.Rows.Add(_i,
                                    p.Voucher_no,
                                    p.Customer_name,
                                    p.Customer_nrc,
                                    p.Customer_address,
                                    p.Item_name,
                                    p.Item_weight,
                                    p.En_amount,
                                    (p.Interest_rate + " % "),
                                    _intst,
                                    Convert.ToInt32(_intst + p.En_amount));
                }
            }
            return _tbl;
        }







        /// <summary>
        /// Select Gold Transaction Info By Date
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns>DataTable(transaction_amount, description, string)</returns>
        public DataTable selectGoldTransactionByDate(DateTime _dt)
        {
            _tbl = new DataTable();            
            _tbl.Columns.Add("transaction_amount");
            _tbl.Columns.Add("description");
            _tbl.Columns.Add("transaction_type");
            
            _tbl.Columns["transaction_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["description"].Caption = "မှတ်ချက်";
            _tbl.Columns["transaction_type"].Caption = "type";

            var query = from gt in db.Gold_transactions
                        where gt.Transaction_date.Value.Day == _dt.Day && gt.Transaction_date.Value.Month == _dt.Month && gt.Transaction_date.Value.Year == _dt.Year
                        select new { gt.Transaction_type, gt.Transaction_amount, gt.Description };
            if (query.Count() > 0)
            {
                //int _no = 0;
                foreach (var v in query)
                {
                    //_no++;
                    string _str = null;
                    if (v.Transaction_type == 1)
                    { _str = "အဝယ်"; }
                    else
                    { _str = "အရောင်း"; }
                    _tbl.Rows.Add(v.Transaction_amount, v.Description, _str);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        /// <summary>
        /// Select Cash Transaction Info By Transaction Date By Transacion Type
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_typ">1 or 2. 1->investmoney. 2->withdraw money.</param>
        /// <returns>DataTable(transaction_amount, description)</returns>
        public DataTable selectCashTransactionInfoByDateByType(DateTime _dt, byte _typ)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("transaction_amount");
            _tbl.Columns.Add("description");
            _tbl.Columns["transaction_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["description"].Caption = "မှတ်ချက်";
            var query = from ct in db.Cash_transactions
                        where ct.Transaction_type == _typ && ct.Transaction_date.Value.Day == _dt.Day && ct.Transaction_date.Value.Month == _dt.Month && ct.Transaction_date.Value.Year == _dt.Year
                        select ct;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    _tbl.Rows.Add(v.Transaction_amount, v.Description);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }


        public DataTable selectBalances()
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("balance_id");
            _tbl.Columns.Add("entry_date");
            _tbl.Columns.Add("opening_balance");
            _tbl.Columns.Add("closing_balance");
            _tbl.Columns.Add("modified_on");
            var query = from b in db.Cash_balances
                        select b;
            foreach (var v in query)
            {
                _tbl.Rows.Add(v.Balance_id, v.Entry_date, v.Opening_balance, v.Closing_balance, v.Modified_on);
            }
            return _tbl;
        }


        /// <summary>
        /// Select Cash Balance Info By Month
        /// </summary>
        /// <param name="_dt">Day of the Month</param>
        /// <returns>DataTable(no, entry_date, opening_balance, closing_balance)</returns>
        public DataTable selectCashBalanceInfoByMonth(DateTime _dt)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("entry_date");            
            _tbl.Columns.Add("opening_balance");
            _tbl.Columns.Add("closing_balance");
            _tbl.Columns.Add("modified_on");
            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["entry_date"].Caption = "နေ့စွဲ";
            _tbl.Columns["opening_balance"].Caption = "အဖွင့်စာရင်း";
            _tbl.Columns["closing_balance"].Caption = "အပိတ်စာရင်း";
            _tbl.Columns["modified_on"].Caption = "စာရင်းထည့်သည့်ရက်";
            var query = from cb in db.Cash_balances
                        where cb.Entry_date.Month == _dt.Month
                        orderby cb.Entry_date descending
                        select cb;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var v in query)
                {
                    _i++;
                    _tbl.Rows.Add(_i, v.Entry_date, v.Opening_balance, v.Closing_balance, v.Modified_on);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        
        
        /// <summary>
        /// Select Expired Invoice Info By Date
        /// </summary>
        /// <param name="_dt">Current Date to calculate expire date. (PawnDate - CurrentDate)</param>
        /// <returns>DataTable(no, voucher_no, customer_name, customer_nrc, customer_address, item_name, item_weight, en_amount, pawn_date, month_count)</returns>
        public DataTable selectExpriedInvoiceInfoByDate(DateTime _dt)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("invoice_number");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("customer_address");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("pawn_date");
            _tbl.Columns.Add("month_count");

            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["invoice_number"].Caption = "ပြေစာအမှတ်";
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns["item_name"].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["pawn_date"].Caption = "ပေါင်သည့်နေ့";
            _tbl.Columns["month_count"].Caption = "လအရေအတွက်";

            var query = from pt in db.Pawn_transactions
                        where pt.Is_received == false
                        select pt;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var v in query)
                {                    
                    DateTime _pd = v.Pawn_date;
                    DateTime _expd = Convert.ToDateTime(dthelper.calculateExpireDate(_pd, 0));
                    
                    // calculate-month-count                    
                    int _months = 0;
                    DateTime _pdate = v.Pawn_date;
                    int _monthcount = dthelper.monthDifferent(_pdate, _dt);
                    
                    if (_monthcount == 0 || _monthcount == 1)
                    { _months = 1; }
                    else
                    { _months = _monthcount; }

                    if (_dt >= _expd)
                    {
                        _i++;                        
                        _tbl.Rows.Add(_i, v.Voucher_no, v.Customer_name, v.Customer_nrc, v.Customer_address, v.Item_name, v.Item_weight, v.En_amount, v.Pawn_date, _months);
                    }
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }

        /// <summary>
        /// Select Expired Invoice Info By Date
        /// </summary>
        /// <param name="_dt">Current Date to calculate expire date. (PawnDate - CurrentDate)</param>
        /// <returns>DataTable(no, voucher_no, customer_name, customer_nrc, customer_address, item_name, item_weight, en_amount, pawn_date, month_count)</returns>
        public DataTable selectExpriedInvoiceInfoByDateByVoucherType(DateTime _dt, byte vtype)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("no");
            _tbl.Columns.Add("invoice_number");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("customer_address");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            _tbl.Columns.Add("pawn_date");
            _tbl.Columns.Add("month_count");

            _tbl.Columns["no"].Caption = "စဉ်";
            _tbl.Columns["invoice_number"].Caption = "ပြေစာအမှတ်";
            _tbl.Columns["customer_name"].Caption = "အမည်";
            _tbl.Columns["customer_nrc"].Caption = "မှတ်ပုံတင်";
            _tbl.Columns["customer_address"].Caption = "လိပ်စာ";
            _tbl.Columns["item_name"].Caption = "ပစ္စည်းအမည်";
            _tbl.Columns["item_weight"].Caption = "အလေးချိန်";
            _tbl.Columns["en_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["pawn_date"].Caption = "ပေါင်သည့်နေ့";
            _tbl.Columns["month_count"].Caption = "လအရေအတွက်";

            var query = from pt in db.Pawn_transactions
                        where pt.Is_received == false && pt.Voucher_type == vtype
                        select pt;
            if (query.Count() > 0)
            {
                int _i = 0;
                foreach (var v in query)
                {
                    DateTime _pd = v.Pawn_date;
                    DateTime _expd = Convert.ToDateTime(dthelper.calculateExpireDate(_pd, 0));

                    // calculate-month-count                    
                    int _months = 0;
                    DateTime _pdate = v.Pawn_date;
                    int _monthcount = dthelper.monthDifferent(_pdate, _dt);

                    if (_monthcount == 0)
                    { _months = 1; }
                    else
                    { _months = _monthcount; }

                    if (_dt >= _expd)
                    {
                        _i++;

                        _tbl.Rows.Add(_i, v.Voucher_no, v.Customer_name, v.Customer_nrc, v.Customer_address, v.Item_name, v.Item_weight, v.En_amount, v.Pawn_date, _months);
                    }
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }






        /// <summary>
        /// Select Invoice Info By Date (For GridLookupEdit - Pawn Receive Transaction)
        /// </summary>
        /// <param name="_dt">Receive Date</param>
        /// <returns>DataTable(voucher_no, customer_name, item_name, item_weight, bool, bool)</returns>
        public DataTable selectInvoiceInfo(DateTime _dt)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("invoice_number");
            _tbl.Columns.Add("voucher_code");
            _tbl.Columns.Add("voucher_number");
            _tbl.Columns.Add("customer_name");            
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");            
            _tbl.Columns.Add("is_received");
            _tbl.Columns.Add("is_expired");
            _tbl.Columns.Add("pawn_date");
            _tbl.Columns.Add("receive_date");

            var query = from pt in db.Pawn_transactions
                        select pt;
            if (query.Count() > 0)
            {
                //int _i = 0;
                foreach (var v in query)
                {
                    //_i++;
                    DateTime _pd = v.Pawn_date;
                    DateTime _expd = Convert.ToDateTime(dthelper.calculateExpireDate(_pd, 0));
                    // calculate-month-count
                    int _months = 0;
                    DateTime _pdate = v.Pawn_date;
                    int _monthcount = dthelper.monthDifferent(_pdate, _dt);
                    if (_monthcount == 0)
                    { _months = 1; }
                    else
                    { _months = _monthcount-1 ; }

                    string vc = v.Voucher_no.Substring(4,v.Voucher_no.Length-8);
                    string vn = v.Voucher_no.Substring(4+vc.Count(), 4);

                    
                    if (v.Is_received == true) {
                        _tbl.Rows.Add(v.Voucher_no, vc, vn, v.Customer_name, v.Item_name, v.Item_weight, true, false, v.Pawn_date.ToString("dd-MMM-yyyy"), v.Receive_date.ToString("dd-MMM-yyyy"));
                    }
                    else if (_dt <=_expd) {
                        _tbl.Rows.Add(v.Voucher_no, vc, vn, v.Customer_name, v.Item_name, v.Item_weight, false, false, v.Pawn_date.ToString("dd-MMM-yyyy"), "မရွေးသေး");
                    }
                    else if (_dt >_expd)
                    {
                        _tbl.Rows.Add(v.Voucher_no, vc, vn, v.Customer_name, v.Item_name, v.Item_weight, false, true, v.Pawn_date.ToString("dd-MMM-yyyy"), "မရွေးသေး");
                    }
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
        
        #region Select Total
        /// <summary>
        /// Select Total Pawn Transaction Amount By Pawn Date
        /// </summary>
        /// <param name="_dt">Pawn Date</param>
        /// <returns>integer</returns>
        public int selectTotalPawnTransactionAmountByPawnDate(DateTime _dt)
        {
            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year
                        select p.En_amount;                        
            if (query.Count() > 0)
            {
                return (int)query.Sum();                
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Select Total Pawn Transaction Amount By Pawn Date By Voucher Type
        /// </summary>
        /// <param name="_dt">Pawn Date</param>
        /// <returns>integer</returns>
        public int selectTotalPawnTransactionAmountByPawnDateByVoucherType(DateTime _dt, byte _vtype)
        {
            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year && p.Voucher_type == _vtype
                        select p.En_amount;
            if (query.Count() > 0)
            {
                return (int)query.Sum();
            }
            else
            {
                return 0;
            }
        }



        /// <summary>
        /// Select total Pawn Receive Transaction Amount By Receive Date. (Interest Amount are also included)
        /// </summary>
        /// <param name="_dt">Receive Date</param>
        /// <returns>integer</returns>
        public int selectTotalPawnReceiveTransactionAmountByReceiveDate(DateTime _dt)
        {            
            var query = from p in db.Pawn_transactions
                        where p.Is_received == true && p.Receive_date.Day == _dt.Day && p.Receive_date.Month == _dt.Month && p.Receive_date.Year == _dt.Year
                        select new { p.En_amount, p.Interest_rate, p.Pawn_date, p.Receive_date };
            if (query.Count() > 0)
            {
                int _sum = 0;
                foreach (var p in query)
                {
                    //byte _monthCount = Convert.ToByte(dthelper.monthDifferent(p.Pawn_date, p.Receive_date));
                    DateTime expdate = Convert.ToDateTime(dthelper.calculateExpireDate(p.Pawn_date, 0));
                    byte _monthCount = Convert.ToByte(dthelper.monthDifferentForPawnReceive(p.Receive_date, p.Pawn_date, expdate));
                    _sum = _sum + (Convert.ToInt32(p.En_amount) + calculateInterestAmount(Convert.ToDecimal(p.Interest_rate), _monthCount, p.En_amount));
                }
                return _sum;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Select total Pawn Receive Transaction Amount By Receive Date By Voucher Type. (Interest Amount are also included)
        /// </summary>
        /// <param name="_dt">Receive Date</param>
        /// <returns>integer</returns>
        public int selectTotalPawnReceiveTransactionAmountByReceiveDateByVoucherType(DateTime _dt, byte _vtype)
        {
            var query = from p in db.Pawn_transactions
                        where p.Is_received == true && p.Receive_date.Day == _dt.Day && p.Receive_date.Month == _dt.Month && p.Receive_date.Year == _dt.Year && p.Voucher_type == _vtype
                        select new { p.En_amount, p.Interest_rate, p.Pawn_date, p.Receive_date };
            if (query.Count() > 0)
            {
                int _sum = 0;
                foreach (var p in query)
                {
                    byte _monthCount = Convert.ToByte(dthelper.monthDifferent(p.Pawn_date, p.Receive_date));
                    _sum = _sum + (Convert.ToInt32(p.En_amount) + calculateInterestAmount(Convert.ToDecimal(p.Interest_rate), _monthCount, p.En_amount));
                }
                return _sum;
            }
            else
            {
                return 0;
            }
        }


        public int selectTotalInvestMoneyByDate(DateTime _dt)
        {/*
            var query = from v in db.Cash_transactions
                        where v.Transaction_date == _dt
                        select v;
            //if (query.Count() > 0)
            //{
                foreach (var v in query)
                {
                    //return v.Transaction_amount;
                    
                }
            //}
          */
            return 0;
        }

        public int selectTotalWithdrawMoneyByDate(DateTime _dt)
        {
            return 0;
        }


        /// <summary>
        /// Select Pawn Transaction Count By Pawn Date
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>        
        public int selectPawnTransactionCountByPawnDate(DateTime _dt)
        {
            var query = from p in db.Pawn_transactions
                        where p.Pawn_date.Day == _dt.Day && p.Pawn_date.Month == _dt.Month && p.Pawn_date.Year == _dt.Year
                        select p.Voucher_no;
            if (query.Count() > 0)
            {
                return query.Count();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Select Total Gold Transaction Amount By Date By Type
        /// </summary>
        /// <param name="_dt">Transaction Date</param>
        /// <param name="_typ">1, 2</param>
        /// <returns>integer</returns>

        public int selectTotalGoldTransactionAmountByDateByType(DateTime _dt, byte _typ)
        {
            var query = from gt in db.Gold_transactions
                        where gt.Transaction_type == _typ && gt.Transaction_date.Value.Day == _dt.Day && gt.Transaction_date.Value.Month == _dt.Month && gt.Transaction_date.Value.Year == _dt.Year
                        select gt.Transaction_amount;
            if (query.Count() > 0)
            {
                return (int)query.Sum();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Select Total Cash Transaction Amount By Date By Type
        /// </summary>
        /// <param name="_dt">Transaction Date</param>
        /// <param name="_typ">1, 2</param>
        /// <returns>integer</returns>
        public int selectTotalCashTransactionAmountByDateByType(DateTime _dt, byte _typ)
        {
            var query = from ct in db.Cash_transactions
                        where ct.Transaction_type == _typ && ct.Transaction_date.Value.Day == _dt.Day && ct.Transaction_date.Value.Month == _dt.Month && ct.Transaction_date.Value.Year == _dt.Year
                        select ct.Transaction_amount;
            if (query.Count() > 0)
            {
                return (int)query.Sum();
            }
            else
            {
                return 0;
            }
        }
        #endregion
        
        /// <summary>
        /// Select Opening Balance By Date
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        /*
        public int selectOpeningBalanceByDate(DateTime _dt)
        {
            var query = from b in db.Cash_balances
                        where b.Entry_date.Day == _dt.Day && b.Entry_date.Month == _dt.Month && b.Entry_date.Year == _dt.Year
                        select b.Opening_balance;
            if (query.Count() > 0)
            {
                return Convert.ToInt32(query.ToString());
            }
            else
            {
                return 0;
            }
        }
        */

        

#endregion

#region Util
        /// <summary>
        /// Calculate Interest Amount by Interest Rate
        /// </summary>
        /// <param name="_interestrate">interest rate float value</param>
        /// <param name="_monthcount">month count</param>
        /// <param name="_enamount">pawn amount</param>
        /// <returns></returns>
        private int calculateInterestAmount(decimal _interestrate, byte _monthcount, decimal _enamount)
        {
            decimal rt;
            rt = Convert.ToDecimal(_interestrate) / 100;
            return Convert.ToInt32(rt * _monthcount * _enamount);
        }
#endregion
  
        public DataTable selectReceiveInfo()
        {
            _tbl.Clear();
            _tbl = new DataTable();
            _tbl.Columns.Add("pawn_date");
            _tbl.Columns.Add("voucher_no​");
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("customer_nrc");
            _tbl.Columns.Add("customer_address");            
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("en_amount");
            
            _tbl.Columns[0].Caption = "နေ့စွဲ";
            _tbl.Columns[1].Caption = "ပြေစာအမှတ်";
            _tbl.Columns[2].Caption = "နာမည်";
            _tbl.Columns[3].Caption = "မှတ်ပုံတင်";
            _tbl.Columns[4].Caption = "လိပ်စာ";
            _tbl.Columns[5].Caption = "ပစ္စည်း အမည်";
            _tbl.Columns[6].Caption = "အလေးချိန်";
            _tbl.Columns[7].Caption = "တန်ဖိုး";

            var query = from pwn in db.Pawn_transactions
                        where pwn.Is_received == true
                        select new { pwn.Pawn_date, pwn.Voucher_no, pwn.Customer_name, pwn.Customer_nrc, pwn.Customer_address, pwn.Item_name, pwn.Item_weight, pwn.En_amount };
            if (query.Count() > 0)
            {
                foreach (var p in query)
                {
                    _tbl.Rows.Add(  p.Pawn_date,
                                    p.Voucher_no,
                                    p.Customer_name,
                                    p.Customer_nrc,
                                    p.Customer_address,                                    
                                    p.Item_name,
                                    p.Item_weight,
                                    p.En_amount);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }


        public Dictionary<string, object> getAllPawnInfoByInvoiceNumber(string _invno)
        {
            _dic.Clear();
            var query = from p in db.Pawn_transactions
                        where p.Voucher_no.Equals(_invno)
                        select p;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {   
                    _dic.Add("pawn_date", v.Pawn_date);
                    _dic.Add("voucher_type", (byte)v.Voucher_type);
                    _dic.Add("customer_name", (string)v.Customer_name);
                    _dic.Add("customer_nrc", (string)v.Customer_nrc);
                    _dic.Add("customer_address", (string)v.Customer_address);
                    _dic.Add("item_type", (byte)v.Item_type);
                    _dic.Add("item_name", (string)v.Item_name);
                    _dic.Add("item_weight", (string)v.Item_weight);                    
                    _dic.Add("interest_rate", (byte)v.Interest_rate);
                    _dic.Add("en_amount", v.En_amount);
                    _dic.Add("mm_amount", (string)v.Mm_amount);
                    _dic.Add("mm_amount_text", (string)v.Mm_amount_text);
                    _dic.Add("description", (string)v.Description);
                }
            }
            return _dic;
        }
        
        public Dictionary<string, object> getAllPawnReceiveInfoByInvoiceNumber(string _invno)
        {
            _dic.Clear();
            var query = from p in db.Pawn_transactions
                        where p.Voucher_no.Equals(_invno)
                        select p;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    _dic.Add("pawn_date", v.Pawn_date);
                    _dic.Add("rec_date", v.Receive_date);
                    _dic.Add("voucher_type", (byte)v.Voucher_type);
                    _dic.Add("customer_name", (string)v.Customer_name);
                    _dic.Add("customer_nrc", (string)v.Customer_nrc);
                    _dic.Add("customer_address", (string)v.Customer_address);
                    _dic.Add("item_type", (byte)v.Item_type);
                    _dic.Add("item_name", (string)v.Item_name);
                    _dic.Add("item_weight", (string)v.Item_weight);
                    _dic.Add("interest_rate", (byte)v.Interest_rate);
                    _dic.Add("en_amount", v.En_amount);
                    _dic.Add("mm_amount", (string)v.Mm_amount);
                    _dic.Add("mm_amount_text", (string)v.Mm_amount_text);
                    _dic.Add("description", (string)v.Description);
                }
            }
            return _dic;
        }
        

        public Dictionary<string, object> getPawnInfoByInvoiceNumber(string _invno, DateTime _rdate)
        {
            _dic.Clear();
            var query = from p in db.Pawn_transactions
                            where p.Voucher_no.Equals(_invno) //&& p.Is_expired == false
                            select p;
            if (query.Count() > 0)
            {
                foreach (var pwn in query)
                {
                    //calculate expire date
                    bool _invoice_expired;
                    DateTime _pdate = pwn.Pawn_date;
                    
                    DateTime _expDate = Convert.ToDateTime(dthelper.calculateExpireDate(_pdate, 0));
                    if (_rdate.Date <= _expDate.Date)
                    {
                        // ok
                        _invoice_expired = false;
                    }
                    else
                    {
                        // expired
                        _invoice_expired = true;
                    }
                    


                    _dic.Add("is_received", pwn.Is_received);
                    _dic.Add("is_expired", _invoice_expired);
                    _dic.Add("pawn_date", pwn.Pawn_date);                        
                    _dic.Add("customer_name", (string)pwn.Customer_name);
                    _dic.Add("customer_nrc", (string)pwn.Customer_nrc);
                    _dic.Add("customer_address", (string)pwn.Customer_address);
                    _dic.Add("item_name", (string)pwn.Item_name);
                    _dic.Add("item_weight", (string)pwn.Item_weight);
                    _dic.Add("description", (string)pwn.Description);
                    _dic.Add("interest_rate", (byte)pwn.Interest_rate);
                    _dic.Add("en_amount", pwn.En_amount);
                    _dic.Add("month_count", (byte)dthelper.monthDifferent(pwn.Pawn_date, _rdate));

                    _dic.Add("receive_date", pwn.Receive_date);
                }
            }
            return _dic;
        }
            
        public DataTable selectInvoiceNumber()
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("invoice_number");            
            _tbl.Columns.Add("customer_name");
            _tbl.Columns.Add("item_name");
            _tbl.Columns.Add("item_weight");
            _tbl.Columns.Add("is_expired");
            _tbl.Columns.Add("is_received");
            var query = from p in db.Pawn_transactions                        
                        select new { p.Voucher_no, p.Customer_name, p.Item_name, p.Item_weight, p.Is_received };
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    _tbl.Rows.Add((string)v.Voucher_no, v.Customer_name, v.Item_name, v.Item_weight, v.Is_received);
                }
            }
            return _tbl;
        }

#region save changes transactions
        /// <summary>
        /// Insert Pawn Transaction
        /// </summary>
        /// <param name="_voucher_no"></param>
        /// <param name="_voucher_type"></param>
        /// <param name="_customer_name"></param>
        /// <param name="_customer_nrc"></param>
        /// <param name="_customer_address"></param>
        /// <param name="_item_type"></param>
        /// <param name="_item_name"></param>
        /// <param name="_item_weight"></param>
        /// <param name="_en_amount"></param>
        /// <param name="_mm_amount"></param>
        /// <param name="_mm_amount_text"></param>
        /// <param name="_description"></param>
        /// <param name="_pawn_date"></param>
        /// <param name="_receive_date"></param>
        /// <param name="_user_name"></param>
        /// <param name="_is_received"></param>
        /// <param name="_interest_rate"></param>
        /// <returns>bool</returns>
        public bool insertPawnTransaction(string _voucher_no, Byte _voucher_type, string _customer_name, string _customer_nrc, string _customer_address, byte _item_type, string _item_name, string _item_weight, decimal _en_amount, string _mm_amount, string _mm_amount_text, string _description, DateTime _pawn_date, DateTime _receive_date, string _user_name, bool _is_received, decimal _interest_rate)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    models.Pawn_transaction pwn = new models.Pawn_transaction
                    {
                        Voucher_no = _voucher_no,
                        Voucher_type = _voucher_type,
                        Customer_name = _customer_name,
                        Customer_nrc = _customer_nrc,
                        Customer_address = _customer_address,
                        Item_type = _item_type,
                        Item_name = _item_name,
                        Item_weight = _item_weight,
                        En_amount = _en_amount,
                        Mm_amount = _mm_amount,
                        Mm_amount_text = _mm_amount_text,
                        Description = _description,
                        Pawn_date = _pawn_date,
                        Receive_date = _receive_date,
                        User_name = _user_name,
                        Is_received = _is_received,                        
                        Interest_rate = Convert.ToDouble(_interest_rate),
                        Modified_on = DateTime.Now
                    };
                    db.Pawn_transactions.InsertOnSubmit(pwn);
                    db.SubmitChanges();
                    ts.Complete();
                    Properties.Settings.Default.datetime_PawnTransaction = (DateTime)_pawn_date;
                    return true;
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        /// <summary>
        /// Update Pawn Transaction
        /// </summary>
        /// <param name="_voucher_no"></param>
        /// <param name="_voucher_type"></param>
        /// <param name="_customer_name"></param>
        /// <param name="_customer_nrc"></param>
        /// <param name="_customer_address"></param>
        /// <param name="_item_type"></param>
        /// <param name="_item_name"></param>
        /// <param name="_item_weight"></param>
        /// <param name="_en_amount"></param>
        /// <param name="_mm_amount"></param>
        /// <param name="_mm_amount_text"></param>
        /// <param name="_description"></param>
        /// <param name="_pawn_date"></param>
        /// <param name="_receive_date"></param>
        /// <param name="_user_name"></param>
        /// <param name="_is_received"></param>
        /// <param name="_interest_rate"></param>
        /// <returns></returns>
        public bool updatePawnTransaction(string _voucher_no, Byte _voucher_type, string _customer_name, string _customer_nrc, string _customer_address, byte _item_type, string _item_name, string _item_weight, decimal _en_amount, string _mm_amount, string _mm_amount_text, string _description, DateTime _pawn_date, DateTime _receive_date, string _user_name, bool _is_received, decimal _interest_rate)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from p in db.Pawn_transactions
                                where p.Voucher_no == _voucher_no
                                select p;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            v.Voucher_no = _voucher_no;
                            v.Voucher_no = _voucher_no;
                            v.Voucher_type = _voucher_type;
                            v.Customer_name = _customer_name;
                            v.Customer_nrc = _customer_nrc;
                            v.Customer_address = _customer_address;
                            v.Item_type = _item_type;
                            v.Item_name = _item_name;
                            v.Item_weight = _item_weight;
                            v.En_amount = _en_amount;
                            v.Mm_amount = _mm_amount;
                            v.Mm_amount_text = _mm_amount_text;
                            v.Description = _description;
                            v.Pawn_date = _pawn_date;
                            v.Receive_date = _receive_date;
                            v.User_name = _user_name;
                            v.Is_received = _is_received;
                            v.Interest_rate = Convert.ToDouble(_interest_rate);
                            v.Modified_on = DateTime.Now;
                        }
                        db.SubmitChanges();
                        ts.Complete();
                        Properties.Settings.Default.datetime_PawnTransaction = (DateTime)_pawn_date;
                        return true;
                    }
                    else
                    {
                        ts.Dispose();
                        return false;
                    }                    
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }
        }


        public bool updatePawnReceiveTransaction(string _voucherNo, DateTime _recDate, decimal _interestRate)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from p in db.Pawn_transactions
                                where p.Voucher_no == _voucherNo
                                select p;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            v.Receive_date = _recDate;
                            v.Interest_rate = Convert.ToDouble(_interestRate);
                        }
                        db.SubmitChanges();
                        ts.Complete();
                        Properties.Settings.Default.datetime_PawnTransaction = (DateTime)_recDate;
                        return true;
                    }
                    else
                    {
                        ts.Dispose();
                        return false;
                    }
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        public bool undoPawnReceiveTransaction(string _voucherNo)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from p in db.Pawn_transactions
                                where p.Voucher_no == _voucherNo
                                select p;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            v.Is_received = false;
                        }
                        db.SubmitChanges();
                        ts.Complete();
                        return true;
                    }
                    else
                    {
                        ts.Dispose();
                        return true;
                    }
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }


        }



        /// <summary>
        /// Pawn Receive Transaction
        /// </summary>
        /// <param name="_invno"></param>
        /// <param name="_rec_date"></param>
        /// <param name="_interest_rate"></param>        
        /// <returns></returns>
        public bool insertPawnReceiveTransaction(string _invno, DateTime _rec_date, decimal _interest_rate)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from p in db.Pawn_transactions
                              where p.Voucher_no.Equals(_invno)
                              select p;
                    foreach (var p in query)
                    {
                        p.Receive_date = (DateTime)_rec_date;
                        p.Is_received = true;
                        p.Interest_rate = (float)_interest_rate;
                        p.Modified_on = DateTime.Now;
                    }
                    db.SubmitChanges();
                    ts.Complete();
                    Properties.Settings.Default.datetime_PawnReceive = (DateTime)_rec_date;
                    return true;
                }
                catch
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        /// <summary>
        /// Insert Custom Voucher Code
        /// </summary>
        /// <param name="vc_id">Jan-2013,Feb-2013, ...</param>
        /// <param name="cv_code">A, AB, BB, D, E, ...</param>
        /// <param name="itm_typ">1, 2</param>
        /// <param name="v_type">1, 2</param>        
        /// <returns>True=Success, False=Failed</returns>
        public bool insertVoucherCode(string vc_id, string cv_code, byte itm_typ, byte v_type)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    models.Voucher_code vc = new models.Voucher_code
                    {
                        Voucher_code_id = vc_id,
                        Custom_voucher_code = cv_code,
                        Item_type = itm_typ,
                        Voucher_type = v_type,
                        Modified_on = DateTime.Now
                    };
                    db.Voucher_codes.InsertOnSubmit(vc);
                    db.SubmitChanges();
                    ts.Complete();          
                    return true;
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        /// <summary>
        /// Insert Gold Transaction
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_transactionType"></param>
        /// <param name="_transactionAmount"></param>
        /// <param name="_description"></param>
        /// <returns></returns>
        public bool insertGoldTransaction(DateTime _dt, byte _transactionType, decimal _transactionAmount, string _description)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    models.Gold_transaction gtran = new models.Gold_transaction
                    {
                        Transaction_date = _dt,
                        Transaction_type = (byte)_transactionType,
                        Transaction_amount = _transactionAmount,
                        Description = _description,
                        Modified_on = DateTime.Now
                    };
                    db.Gold_transactions.InsertOnSubmit(gtran);
                    db.SubmitChanges();                    
                    ts.Complete();
                    Properties.Settings.Default.datetime_GoldTransaction = (DateTime)_dt;
                    return true;
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        /// <summary>
        /// Insert Cash TRansaction
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_transactionType"></param>
        /// <param name="_transactionAmount"></param>
        /// <param name="_description"></param>
        /// <returns></returns>
        public bool insertCashTransaction(DateTime _dt, byte _transactionType, decimal _transactionAmount, string _description)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    models.Cash_transaction ct = new models.Cash_transaction
                    {
                        Transaction_date = _dt,
                        Transaction_type = _transactionType,
                        Transaction_amount = _transactionAmount,
                        Description = _description,
                        Modified_on = DateTime.Now
                    };
                    db.Cash_transactions.InsertOnSubmit(ct);
                    db.SubmitChanges();
                    ts.Complete();
                    Properties.Settings.Default.datetime_CashTransaction = (DateTime)_dt;
                    return true;
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }

        }
        /// <summary>
        /// insert or update CashBalance Table
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_opening_balance">last closing balance</param>
        /// <param name="_closing_balance">closing balance (total_cash_left)</param>
        /// <returns></returns>
        public bool insertCashBalance(DateTime _dt, decimal _opening_balance, decimal _closing_balance)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from cb in db.Cash_balances
                                where cb.Entry_date.Day == _dt.Day && cb.Entry_date.Month == _dt.Month && cb.Entry_date.Year == _dt.Year
                                select cb;
                    if (query.Count() > 0)
                    {
                        foreach(var v in query)
                        {
                            //update
                            var query_update = from cb_update in db.Cash_balances
                                               where cb_update.Balance_id == v.Balance_id
                                               select cb_update;                            
                            foreach(models.Cash_balance cb_column in query_update)
                            {
                                cb_column.Opening_balance = _opening_balance;
                                cb_column.Closing_balance = _closing_balance;
                                cb_column.Modified_on = DateTime.Now;
                            }
                            db.SubmitChanges();
                        }
                        
                    }
                    else
                    {
                        //insert
                        models.Cash_balance cb_insert = new models.Cash_balance
                        {
                            Entry_date = _dt,
                            Opening_balance = _opening_balance,
                            Closing_balance = _closing_balance,
                            Modified_on = DateTime.Now
                        };
                        db.Cash_balances.InsertOnSubmit(cb_insert);
                        db.SubmitChanges();
                    }
                    ts.Complete();
                    Properties.Settings.Default.datetime_CashTransaction = (DateTime)_dt;
                    return true;
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }            
        }

#endregion



        public bool checkVoucherNotExpiredAndReceivedByInvoiceNumberByPawnDate(string _invno, DateTime _dt)
        {
            var query = from p in db.Pawn_transactions
                        where p.Is_received == false
                        select p;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    DateTime _xpdt = Convert.ToDateTime(dthelper.calculateExpireDate(v.Pawn_date, 0));
                    if (_dt < _xpdt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool deletePawnTransactionByInvoiceNumber(string _invno)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from p in db.Pawn_transactions
                                where p.Voucher_no == _invno
                                select p;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            db.Pawn_transactions.DeleteOnSubmit(v);
                        }
                        db.SubmitChanges();
                        ts.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    ts.Dispose();
                    return false;
                }
            }            
        }

        public DataTable selectGoldBuyByDate(DateTime dateTime)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("srno");
            _tbl.Columns.Add("transaction_amount");
            _tbl.Columns.Add("description");
            //_tbl.Columns.Add("transaction_type");

            _tbl.Columns["srno"].Caption = "စဉ်";
            _tbl.Columns["transaction_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["description"].Caption = "မှတ်ချက်";
            //_tbl.Columns["transaction_type"].Caption = "type";

            var query = from gt in db.Gold_transactions
                        where gt.Transaction_date.Value.Day == dateTime.Day && gt.Transaction_date.Value.Month == dateTime.Month && gt.Transaction_date.Value.Year == dateTime.Year && gt.Transaction_type == 1
                        select new { gt.Transaction_amount, gt.Description };
            if (query.Count() > 0)
            {
                int _no = 0;
                foreach (var v in query)
                {
                    _no++;
                    _tbl.Rows.Add(_no, v.Transaction_amount, v.Description);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }

        public DataTable selectGoldSellByDate(DateTime dateTime)
        {
            _tbl = new DataTable();
            _tbl.Columns.Add("srno");
            _tbl.Columns.Add("transaction_amount");
            _tbl.Columns.Add("description");
            //_tbl.Columns.Add("transaction_type");

            _tbl.Columns["srno"].Caption = "စဉ်";
            _tbl.Columns["transaction_amount"].Caption = "တန်ဖိုး";
            _tbl.Columns["description"].Caption = "မှတ်ချက်";
            //_tbl.Columns["transaction_type"].Caption = "type";

            var query = from gt in db.Gold_transactions
                        where gt.Transaction_date.Value.Day == dateTime.Day && gt.Transaction_date.Value.Month == dateTime.Month && gt.Transaction_date.Value.Year == dateTime.Year && gt.Transaction_type == 2
                        select new { gt.Transaction_amount, gt.Description };
            if (query.Count() > 0)
            {
                int _no = 0;
                foreach (var v in query)
                {
                    _no++;
                    _tbl.Rows.Add(_no, v.Transaction_amount, v.Description);
                }
            }
            else
            {
                _tbl = null;
            }
            return _tbl;
        }
    }
}