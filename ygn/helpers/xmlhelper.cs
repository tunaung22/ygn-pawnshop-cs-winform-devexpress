using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace ygn.helpers
{
    public class xmlhelper
    {
        // last date used #use to retrieve the date of last time used
        public string xml_config_path = "config/";
        public string xml_config_file = "config/form_settings.xml";
        public bool initXmlConfig()
        {
            if (!File.Exists(xml_config_file))
            {
                try
                {
                    XDocument xdoc = new XDocument(
                    new XDeclaration("1.0", "UTF-16", "true"),
                    //new XProcessingInstruction("test", "value"),
                    new XComment("you can put comment here"),



                    new XElement("config",



                        //1st child element of config
                        new XElement("form",
                        new XAttribute("ID", "1"),
                            new XElement("Name", "PawnTransaction"),
                            new XElement("DateEdit", "1-Jan-2013"),
                            new XCData("XML CDATA")),



                        //2nd child element of config
                        new XElement("form",
                        new XAttribute("ID", "2"),
                            new XElement("Name", "PawnReceiveTransaction"),
                            new XElement("DateEdit", "3-Jan-2013"),
                            new XCData("XML CDATA"))
                                ));

                    xdoc.Save(xml_config_file);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public string retrieveRcentValue(string form_name, string child_element_name)
        {
            // read data from xml config file

            return "";
        }

        /*
        if(Properties.Settings.Default.PawnDate.Date != null)
            {                 
                deditDate.EditValue = Properties.Settings.Default.PawnDate;
            }
        */
        



        /*



        #region "Variable Declaration"
        Model.ygndbDataContext db = new Model.ygndbDataContext();
        public Controller.DbHelper dbhelper = new Controller.DbHelper();
        public Controller.StringHelper strhelper = new Controller.StringHelper();
        public Controller.DateTimeHelper dthelper = new Controller.DateTimeHelper();
        public SqlConnection con = null;
        public SqlCommand cmd = null;
        #endregion

        

        
        
        #region "[frmAddCustomVoucherCode.cs]"
        public bool SaveCustomVoucherCode(string _voucherdateid, string _customvouchercode, string _itemcode, byte _vouchertype, DateTime _voucherdate)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    Model.tbl_VoucherCode vc = new Model.tbl_VoucherCode
                    {
                        VoucherDateId = _voucherdateid,
                        CustomVoucherCode = _customvouchercode,
                        ItemTypeCode = _itemcode,
                        VoucherType = _vouchertype,
                        VoucherDate = _voucherdate
                    };
                    db.tbl_VoucherCodes.InsertOnSubmit(vc);
                    db.SubmitChanges();
                    ts.Complete();
                    return true;
                }
                catch
                {
                    ts.Dispose();
                    return false;
                }
            }
        }
        #endregion

#region "[frmPawn]"

        

#endregion


        public bool HasCustomVoucherCodeByMonthYear(DateTime _date)
        {
            var vouchercode = from vc in db.tbl_VoucherCodes
                        where vc.VoucherDate.Value.Month == _date.Month && vc.VoucherDate.Value.Year == _date.Year
                        select vc;
            if (vouchercode.Count() < 3)
            {
                return false;
            }
            return true;
        }
        
         * 
         * 
        */
    }
}
