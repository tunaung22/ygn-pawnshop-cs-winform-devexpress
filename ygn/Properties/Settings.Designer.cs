﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ygn.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ygndb.sdf;Password=R3s!dent;Persist Security Info=True")]
        public string ygndbconn {
            get {
                return ((string)(this["ygndbconn"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_PawnTransaction {
            get {
                return ((global::System.DateTime)(this["datetime_PawnTransaction"]));
            }
            set {
                this["datetime_PawnTransaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_PawnReceive {
            get {
                return ((global::System.DateTime)(this["datetime_PawnReceive"]));
            }
            set {
                this["datetime_PawnReceive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_GoldTransaction {
            get {
                return ((global::System.DateTime)(this["datetime_GoldTransaction"]));
            }
            set {
                this["datetime_GoldTransaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_CashTransaction {
            get {
                return ((global::System.DateTime)(this["datetime_CashTransaction"]));
            }
            set {
                this["datetime_CashTransaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_CloseBalance {
            get {
                return ((global::System.DateTime)(this["datetime_CloseBalance"]));
            }
            set {
                this["datetime_CloseBalance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_Report {
            get {
                return ((global::System.DateTime)(this["datetime_Report"]));
            }
            set {
                this["datetime_Report"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_VoucherCode {
            get {
                return ((global::System.DateTime)(this["datetime_VoucherCode"]));
            }
            set {
                this["datetime_VoucherCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_cash_invest {
            get {
                return ((global::System.DateTime)(this["datetime_cash_invest"]));
            }
            set {
                this["datetime_cash_invest"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_cash_withdraw {
            get {
                return ((global::System.DateTime)(this["datetime_cash_withdraw"]));
            }
            set {
                this["datetime_cash_withdraw"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_gold_buy {
            get {
                return ((global::System.DateTime)(this["datetime_gold_buy"]));
            }
            set {
                this["datetime_gold_buy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime datetime_gold_sell {
            get {
                return ((global::System.DateTime)(this["datetime_gold_sell"]));
            }
            set {
                this["datetime_gold_sell"] = value;
            }
        }
    }
}
