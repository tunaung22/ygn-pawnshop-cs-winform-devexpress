using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Data.SqlServerCe;

namespace ygn
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // enable skin
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Seven");

            // change font
            DevExpress.Utils.AppearanceObject.DefaultFont = new Font("Myanmar3", 10);

            Application.Run(new MainForm());

            // check db
            /*
            if (initdb())
            {
                Application.Run(new MainForm());
            }
            else
            {
                Application.Exit();
            }
            */
        }
        
        static bool initdb()
        {
            // get path
            var dirName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var fileName = System.IO.Path.Combine(dirName, "ygndb.sdf");

            // check exist
            string conStr = @"Data Source = " + fileName;

            // create db
            SqlCeEngine engine = new SqlCeEngine(conStr);
            engine.CreateDatabase();

            // create tables
            using (SqlCeConnection con = new SqlCeConnection(conStr))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"CREATE TABLE voucher_code(voucher_code_id          NVARCHAR(50) PRIMARY KEY,
                                                                                       custom_voucher_code      NVARCHAR(50) PRIMARY KEY,
                                                                                       item_type_code			NVARCHAR(50),
                                                                                       voucher_type			    TINYINT(1),
                                                                                       modified_on				DATETIME(8)
                                                                                        )", con))
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return true;
        }


        //static initdb()
        //{
            /*
            if (!Directory.Exists("db"))
                Directory.CreateDirectory("db");

            if (!Directory.Exists("config"))
                Directory.CreateDirectory("config");


            ygn.helpers.xmlhelper xmlhelper = new helpers.xmlhelper();
            if (xmlhelper.initXmlConfig())
                Console.WriteLine("new xml config file is created");
            else
                Console.WriteLine("using existing xml config file");

            ygn.helpers.dbhelper db = new helpers.dbhelper();
            if (db.initSQLite())
                Console.WriteLine("new db created");
            else
                Console.WriteLine("using existing db");            
             */
        //}
    }
}