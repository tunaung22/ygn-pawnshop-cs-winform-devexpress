using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ygn.helpers
{
    public class voucher_helper
    {        
        ygn.helpers.dbhelper dbhelper = new helpers.dbhelper();
        ygn.helpers.datetime_helper dthelper = new datetime_helper();

        public string generateNewInvoiceNumber(DateTime _date, byte _itemtype, byte _vouchertype)
        {
            //string _voucherno = "";     // final voucher number to return
            string _customcode = "";    // A            
            string _lastno = "";        // 2013A0001
            string _no = "";            // 0001            
            string _monthYear = "";     // 12/Jan/2013 -to-> Jan-2013
            
            _monthYear= dthelper.MonthYearString(_date.Month, _date.Year);            
            _customcode = dbhelper.selectCustomVoucherCode(_monthYear, _itemtype, _vouchertype);   // A,B


            

            _lastno = dbhelper.selectLastInvoiceNumber(_date, _vouchertype);           // 2013A0001
            int startindex = (4 + Convert.ToInt32(_customcode.Length));     // 4 is for year [2012]
            
            //_no = _lastno.Substring(startindex, 4);      // 0001, 0002, 0003,...
            //_no = (Convert.ToInt32(_voucherno) + 1).ToString();
            // single line
            _no = (Convert.ToInt16(_lastno) + 1).ToString();
            if (_no.Length == 4)
            {
                _no = _no + "";
            }
            else if (_no.Length == 1)
            { _no = "000" + _no; }
            else if (_no.Length == 2)
            { _no = "00" + _no; }
            else if (_no.Length == 3)
            { _no = "0" + _no; }
                        

            return (Convert.ToString(_date.Year) + _customcode + _no);
            
        }

        /// <summary>
        /// check 
        /// </summary>
        /// <param name="_date"></param>
        /// <param name="_itm_type"></param>
        /// <param name="_vtype"></param>
        /// <returns></returns>
        /// 
        /*
        public bool codeIsUsable(DateTime _date, string _itm_type, int _vtype)
        {
            if (dbhelper.voucherCodeExist(_date, _itm_type, _vtype))
            {
                return true;
            }
            return false;
        }
        */


        public int calculateInterestAmount(decimal _interestrate, byte _monthcount, decimal _enamount)
        {
            decimal rt = _interestrate / 100;
            return Convert.ToInt32((rt * _monthcount) * _enamount);
        }


        public string manipulateNumber(string _sn)
        {
            string _no = "";
            if (_sn.Length == 4)
            {
                _no = _sn;
            }
            else if (_sn.Length == 3)
            {
                _no = "0" + _sn;
            }
            else if (_sn.Length == 2)
            {
                _no = "00" + _sn;
            }
            else if (_sn.Length == 1)
            {
                _no = "000" + _sn;
            }            
            return _no;
        }
    }
}
