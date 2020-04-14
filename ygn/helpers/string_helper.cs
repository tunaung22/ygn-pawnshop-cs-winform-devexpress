using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ygn.helpers
{
    public class string_helper
    {

        public string convertMyanmarToEnglishNumberString(string _str)
        {
            while (!string.IsNullOrWhiteSpace(_str))
            {
                //unicode number : 4160=0 ---> 4169=9
                string ascNum = null;
                char[] numArr = _str.ToCharArray();
                foreach (char numChar in numArr)
                {
                    System.Globalization.UnicodeCategory uniCat = Char.GetUnicodeCategory(numChar);
                    int charCode = (int)Convert.ToUInt32(numChar);

                    if (charCode == 4125)
                        ascNum += "0";
                    else
                    {
                        if (uniCat == System.Globalization.UnicodeCategory.DecimalDigitNumber)
                        {
                            if (charCode >= 4160 && charCode <= 4169)
                            {
                                int i = charCode - 4160;
                                ascNum += i.ToString();
                            }
                        }

                    }
                }
                //txtEn_str.EditValue = ascNum;
                return ascNum;
            }
            return "";
        }
    }
}
