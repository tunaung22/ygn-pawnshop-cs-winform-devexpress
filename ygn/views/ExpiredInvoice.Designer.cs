namespace ygn.views
{
    partial class ExpiredInvoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.dateEdit_date = new DevExpress.XtraEditors.DateEdit();
            this.gridControl_list = new DevExpress.XtraGrid.GridControl();
            this.gridView_list = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_invoice_number = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_customer_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_customer_nrc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_customer_address = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_item_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_item_weight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_en_amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_pawn_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_month_count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_date.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_date.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_list)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_list)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.dateEdit_date);
            this.layoutControl1.Controls.Add(this.gridControl_list);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(886, 444);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // dateEdit_date
            // 
            this.dateEdit_date.EditValue = null;
            this.dateEdit_date.Location = new System.Drawing.Point(111, 14);
            this.dateEdit_date.Name = "dateEdit_date";
            this.dateEdit_date.Properties.Appearance.Font = new System.Drawing.Font("Myanmar3", 10F);
            this.dateEdit_date.Properties.Appearance.Options.UseFont = true;
            this.dateEdit_date.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_date.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.dateEdit_date.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_date.Properties.EditFormat.FormatString = "dd-MMM-yyyy";
            this.dateEdit_date.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_date.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dateEdit_date.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_date.Size = new System.Drawing.Size(145, 27);
            this.dateEdit_date.StyleController = this.layoutControl1;
            this.dateEdit_date.TabIndex = 5;
            this.dateEdit_date.EditValueChanged += new System.EventHandler(this.dateEdit_date_EditValueChanged);
            // 
            // gridControl_list
            // 
            this.gridControl_list.Location = new System.Drawing.Point(12, 63);
            this.gridControl_list.MainView = this.gridView_list;
            this.gridControl_list.Name = "gridControl_list";
            this.gridControl_list.Size = new System.Drawing.Size(862, 369);
            this.gridControl_list.TabIndex = 4;
            this.gridControl_list.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_list});
            // 
            // gridView_list
            // 
            this.gridView_list.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_no,
            this.gridColumn_invoice_number,
            this.gridColumn_customer_name,
            this.gridColumn_customer_nrc,
            this.gridColumn_customer_address,
            this.gridColumn_item_name,
            this.gridColumn_item_weight,
            this.gridColumn_en_amount,
            this.gridColumn_pawn_date,
            this.gridColumn_month_count});
            this.gridView_list.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView_list.GridControl = this.gridControl_list;
            this.gridView_list.Name = "gridView_list";
            this.gridView_list.OptionsBehavior.Editable = false;
            this.gridView_list.OptionsSelection.EnableAppearanceFocusedCell = false;
            // 
            // gridColumn_no
            // 
            this.gridColumn_no.Caption = "စဉ်";
            this.gridColumn_no.FieldName = "no";
            this.gridColumn_no.Name = "gridColumn_no";
            this.gridColumn_no.Visible = true;
            this.gridColumn_no.VisibleIndex = 0;
            // 
            // gridColumn_invoice_number
            // 
            this.gridColumn_invoice_number.Caption = "ပြေစာအမှတ်";
            this.gridColumn_invoice_number.FieldName = "invoice_number";
            this.gridColumn_invoice_number.Name = "gridColumn_invoice_number";
            this.gridColumn_invoice_number.Visible = true;
            this.gridColumn_invoice_number.VisibleIndex = 1;
            // 
            // gridColumn_customer_name
            // 
            this.gridColumn_customer_name.Caption = "အမည်";
            this.gridColumn_customer_name.FieldName = "customer_name";
            this.gridColumn_customer_name.Name = "gridColumn_customer_name";
            this.gridColumn_customer_name.Visible = true;
            this.gridColumn_customer_name.VisibleIndex = 2;
            // 
            // gridColumn_customer_nrc
            // 
            this.gridColumn_customer_nrc.Caption = "မှတ်ပုံတင်";
            this.gridColumn_customer_nrc.FieldName = "customer_nrc";
            this.gridColumn_customer_nrc.Name = "gridColumn_customer_nrc";
            this.gridColumn_customer_nrc.Visible = true;
            this.gridColumn_customer_nrc.VisibleIndex = 3;
            // 
            // gridColumn_customer_address
            // 
            this.gridColumn_customer_address.Caption = "လိပ်စာ";
            this.gridColumn_customer_address.FieldName = "customer_address";
            this.gridColumn_customer_address.Name = "gridColumn_customer_address";
            this.gridColumn_customer_address.Visible = true;
            this.gridColumn_customer_address.VisibleIndex = 4;
            // 
            // gridColumn_item_name
            // 
            this.gridColumn_item_name.Caption = "ပစ္စည်းအမည်";
            this.gridColumn_item_name.FieldName = "item_name";
            this.gridColumn_item_name.Name = "gridColumn_item_name";
            this.gridColumn_item_name.Visible = true;
            this.gridColumn_item_name.VisibleIndex = 5;
            // 
            // gridColumn_item_weight
            // 
            this.gridColumn_item_weight.Caption = "အလေးချိန်";
            this.gridColumn_item_weight.FieldName = "item_weight";
            this.gridColumn_item_weight.Name = "gridColumn_item_weight";
            this.gridColumn_item_weight.Visible = true;
            this.gridColumn_item_weight.VisibleIndex = 6;
            // 
            // gridColumn_en_amount
            // 
            this.gridColumn_en_amount.Caption = "တန်ဖိုး";
            this.gridColumn_en_amount.FieldName = "en_amount";
            this.gridColumn_en_amount.Name = "gridColumn_en_amount";
            this.gridColumn_en_amount.Visible = true;
            this.gridColumn_en_amount.VisibleIndex = 7;
            // 
            // gridColumn_pawn_date
            // 
            this.gridColumn_pawn_date.Caption = "ပေါင်သည့်နေ့";
            this.gridColumn_pawn_date.FieldName = "pawn_date";
            this.gridColumn_pawn_date.Name = "gridColumn_pawn_date";
            this.gridColumn_pawn_date.Visible = true;
            this.gridColumn_pawn_date.VisibleIndex = 9;
            // 
            // gridColumn_month_count
            // 
            this.gridColumn_month_count.Caption = "လအရေအတွက်";
            this.gridColumn_month_count.FieldName = "month_count";
            this.gridColumn_month_count.Name = "gridColumn_month_count";
            this.gridColumn_month_count.Visible = true;
            this.gridColumn_month_count.VisibleIndex = 8;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(886, 444);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl_list;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 35);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(866, 389);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(93, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Myanmar3", 10F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.dateEdit_date;
            this.layoutControlItem2.CustomizationFormText = "နေ့စွဲ";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(250, 35);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(250, 35);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(250, 35);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlItem2.Text = "နေ့စွဲ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(93, 20);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(250, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(616, 35);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ExpiredInvoice
            // 
            this.Appearance.Font = new System.Drawing.Font("Myanmar3", 10F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 444);
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExpiredInvoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expired Invoice";
            this.Load += new System.EventHandler(this.ExpiredInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_date.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_date.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_list)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_list)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gridControl_list;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_list;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_no;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_invoice_number;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_customer_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_customer_nrc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_customer_address;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_item_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_item_weight;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_en_amount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_pawn_date;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_month_count;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DateEdit dateEdit_date;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}