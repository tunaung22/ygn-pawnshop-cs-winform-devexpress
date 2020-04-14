using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace ygn.helpers
{
    class datetime_helper
    {
        public string conStr = "";//Properties.Settings.Default.YGNDBConnectionString;        
        public string TodayDate = string.Empty;
        public string MonthYear = string.Empty;

        
        
        public string MonthYearString(int _month, int _year)
        {
            var a = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(_month);
            return (String.Format("{0}-{1}", System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(_month), _year));
        }

        /// <summary>
        /// Calculate Expire Date (add 4 months of Pawn Date)
        /// </summary>
        /// <param name="_date">Pawn Date</param>
        /// <param name="_itmtype">0=add 4 months, 1=add 3 months</param>
        /// <returns>string</returns>
        public string calculateExpireDate(DateTime _date, int _itmtype)
        {
            return _date.AddMonths(4).ToString("dd-MMM-yyyy");
         /*
            if (_itmtype == 0)
                return _date.AddMonths(4).ToString("dd-MMM-yyyy");
            //{ return string.Format("dd/MMM/yyyy", _date.AddMonths(4)); } //= deditDate.DateTime.AddMonths(4)); }
            else
                return _date.AddMonths(3).ToString("dd-MMM-yyyy");
            //{ return string.Format("dd/MMM/yyyy", _date.AddMonths(3)); } // = deditDate.DateTime.AddMonths(3)); }
          */
        }




        /// <summary>
        /// get number of months between two date
        /// </summary>
        /// <param name="fdate">from date</param>
        /// <param name="tdate">current date</param>
        /// <returns>fdate - tdate</returns>
        public int monthDifferent(DateTime fdate, DateTime tdate)
        {            
            return System.Math.Abs((fdate.Month - tdate.Month) + 12 * (fdate.Year - tdate.Year)) + 1;            
        }

        public int monthDiff(DateTime recDate, DateTime pDate)
        {
            int returnvalue = 0;

            if (pDate.Date == recDate.Date)
            {
                returnvalue = 1;
            }
            else
            {
                returnvalue = Math.Abs((pDate.Month - recDate.Month) + 12 * (pDate.Year - recDate.Year));

                if (recDate.Day > pDate.Day)
                {
                    returnvalue++;
                }
            }
            
            return returnvalue;
        }

        public int monthDifferentAdvanced(DateTime recDate, DateTime pDate, DateTime expDate)
        {
            return monthDiff(recDate, pDate);

            /*
            int mc = 0;
            DateTime p = pDate;
            DateTime e = expDate;
            DateTime r = recDate;

            mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;

            e = p.AddMonths(4);
            if (r.Date <= e.Date)
            {
                mc--;
                if (r.Date < e.Date)
                {
                    if (r.Day > p.Day)
                    {
                        mc++;
                    }
                    else if (r.Day <= p.Day)
                    {
                        
                    }
                }
            }
            else if(r.Date > e.Date)
            {
                if (r.Day > e.Day)
                {
                    //mc++;
                }
                else if(r.Day <= e.Day)
                {
                    mc--;
                }
            }

            if (mc <= 0)
            {
                mc = 1;
            }
            
            return mc;
            */
        }




        public int monthDifferentForPawnReceive(DateTime recDate, DateTime pDate, DateTime expDate)
        {
            return monthDifferentAdvanced(recDate, pDate, expDate);


            /*
            int mc = 0;

            if (expDate.Month > recDate.Month)
            {
                mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year));                
            }
            
            else
            {
                mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;
                if (recDate.Day <= expDate.Day)
                {
                    mc = mc - 1;
                }
            }

            if (expDate.Month > recDate.Month && recDate.Day > expDate.Day)
            {
                mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;                
            }
            if (expDate.Month > recDate.Month && recDate.Day == pDate.Day)
            {
                mc--;
            }

            if (mc <= 0)
            {
                mc = 1;
            }
             
             * 
             * 
             * 
            */
 

            /*
            if (expDate.Month > recDate.Month)
            {
                mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;
            }
            else if(expDate.Date <= recDate.Date)
            {
                if (recDate.Day > expDate.Day)
                {
                    mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;
                }
                else
                {
                    mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year));
                }
            }
            */

            /*
            if (recDate.Date <= expDate.Date)
            {
                mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year));
            }
            else
            {
                if (pDate.Day <= expDate.Day && pDate.Date < recDate.Date)
                {
                    mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year));
                    if(recDate.Day > expDate.Day)
                    {
                        mc = System.Math.Abs((recDate.Month - pDate.Month) + 12 * (recDate.Year - pDate.Year)) + 1;
                    }
                }
                
            }
            */
            

        }











        public bool CheckServerAvailablity()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetTodayDate()
        {
            TodayDate = DateTime.Now.ToString();
            if (!string.IsNullOrWhiteSpace(TodayDate))
            {
                return TodayDate;
            }
            else
            {
                return "Null";
            }            
        }

        public string GetThisMonthYear()
        {
            MonthYear = String.Format("{0}-{1}", System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month), Convert.ToString(DateTime.Now.Year));
            if (!string.IsNullOrWhiteSpace(MonthYear))
            {
                return MonthYear;
            }
            else
            {
                return "Null";
            }        
        }


        
        // Get Last ID
        
    }
}
