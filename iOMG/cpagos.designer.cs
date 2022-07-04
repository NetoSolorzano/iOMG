namespace iOMG
{
    partial class cpagos
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabgrilla = new System.Windows.Forms.TabPage();
            this.advancedDataGridView1 = new ADGV.AdvancedDataGridView();
            this.tabuser = new System.Windows.Forms.TabPage();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tx_rind = new System.Windows.Forms.TextBox();
            this.tx_idr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_add = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_close = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_anul = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_view = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_prev = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_exc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Tx_modo = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_ini = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_sig = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_ret = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.Bt_fin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabgrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).BeginInit();
            this.tabuser.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.groupBox25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabgrilla);
            this.tabControl1.Controls.Add(this.tabuser);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(982, 515);
            this.tabControl1.TabIndex = 161;
            // 
            // tabgrilla
            // 
            this.tabgrilla.Controls.Add(this.advancedDataGridView1);
            this.tabgrilla.Location = new System.Drawing.Point(4, 22);
            this.tabgrilla.Name = "tabgrilla";
            this.tabgrilla.Padding = new System.Windows.Forms.Padding(3);
            this.tabgrilla.Size = new System.Drawing.Size(974, 489);
            this.tabgrilla.TabIndex = 0;
            this.tabgrilla.Text = "cpagos";
            this.tabgrilla.UseVisualStyleBackColor = true;
            this.tabgrilla.Enter += new System.EventHandler(this.tabgrilla_Enter);
            // 
            // advancedDataGridView1
            // 
            this.advancedDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedDataGridView1.AutoGenerateContextFilters = true;
            this.advancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advancedDataGridView1.DateWithTime = false;
            this.advancedDataGridView1.Location = new System.Drawing.Point(3, 6);
            this.advancedDataGridView1.Name = "advancedDataGridView1";
            this.advancedDataGridView1.Size = new System.Drawing.Size(969, 477);
            this.advancedDataGridView1.TabIndex = 0;
            this.advancedDataGridView1.TimeFilter = false;
            this.advancedDataGridView1.SortStringChanged += new System.EventHandler(this.advancedDataGridView1_SortStringChanged);
            this.advancedDataGridView1.FilterStringChanged += new System.EventHandler(this.advancedDataGridView1_FilterStringChanged);
            this.advancedDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.advancedDataGridView1_CellDoubleClick);
            this.advancedDataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.advancedDataGridView1_CellEnter_1);
            this.advancedDataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.advancedDataGridView1_CellValidating);
            this.advancedDataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.advancedDataGridView1_UserDeletingRow);
            // 
            // tabuser
            // 
            this.tabuser.Controls.Add(this.groupBox27);
            this.tabuser.Controls.Add(this.groupBox25);
            this.tabuser.Controls.Add(this.dataGridView1);
            this.tabuser.Controls.Add(this.label5);
            this.tabuser.Controls.Add(this.label4);
            this.tabuser.Controls.Add(this.tx_rind);
            this.tabuser.Controls.Add(this.tx_idr);
            this.tabuser.Controls.Add(this.button1);
            this.tabuser.Location = new System.Drawing.Point(4, 22);
            this.tabuser.Name = "tabuser";
            this.tabuser.Padding = new System.Windows.Forms.Padding(3);
            this.tabuser.Size = new System.Drawing.Size(974, 489);
            this.tabuser.TabIndex = 1;
            this.tabuser.Text = "Registro";
            this.tabuser.UseVisualStyleBackColor = true;
            this.tabuser.Enter += new System.EventHandler(this.tabuser_Enter);
            // 
            // groupBox27
            // 
            this.groupBox27.BackColor = System.Drawing.Color.DimGray;
            this.groupBox27.Controls.Add(this.groupBox28);
            this.groupBox27.Location = new System.Drawing.Point(4, 73);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(74, 1);
            this.groupBox27.TabIndex = 338;
            this.groupBox27.TabStop = false;
            // 
            // groupBox28
            // 
            this.groupBox28.Location = new System.Drawing.Point(0, -26);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(164, 1);
            this.groupBox28.TabIndex = 113;
            this.groupBox28.TabStop = false;
            // 
            // groupBox25
            // 
            this.groupBox25.BackColor = System.Drawing.Color.DimGray;
            this.groupBox25.Controls.Add(this.groupBox26);
            this.groupBox25.Location = new System.Drawing.Point(4, 50);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(74, 1);
            this.groupBox25.TabIndex = 337;
            this.groupBox25.TabStop = false;
            // 
            // groupBox26
            // 
            this.groupBox26.Location = new System.Drawing.Point(0, -26);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(164, 1);
            this.groupBox26.TabIndex = 113;
            this.groupBox26.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(969, 179);
            this.dataGridView1.TabIndex = 273;
//            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
//            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 287;
            this.label5.Text = "Id fila";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 286;
            this.label4.Text = "Id int.";
            // 
            // tx_rind
            // 
            this.tx_rind.Location = new System.Drawing.Point(42, 54);
            this.tx_rind.Name = "tx_rind";
            this.tx_rind.ReadOnly = true;
            this.tx_rind.Size = new System.Drawing.Size(36, 20);
            this.tx_rind.TabIndex = 275;
            // 
            // tx_idr
            // 
            this.tx_idr.Location = new System.Drawing.Point(42, 31);
            this.tx_idr.Name = "tx_idr";
            this.tx_idr.ReadOnly = true;
            this.tx_idr.Size = new System.Drawing.Size(36, 20);
            this.tx_idr.TabIndex = 274;
            this.tx_idr.Leave += new System.EventHandler(this.tx_idr_Leave);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(882, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 50);
            this.button1.TabIndex = 22;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.Bt_add,
            this.toolStripSeparator5,
            this.Bt_close,
            this.toolStripSeparator7,
            this.toolStripSeparator6,
            this.Bt_edit,
            this.toolStripSeparator15,
            this.Bt_anul,
            this.toolStripSeparator13,
            this.bt_view,
            this.toolStripSeparator14,
            this.Bt_print,
            this.toolStripSeparator12,
            this.bt_prev,
            this.toolStripSeparator1,
            this.bt_exc,
            this.toolStripSeparator4,
            this.Tx_modo,
            this.toolStripSeparator3,
            this.Bt_ini,
            this.toolStripSeparator8,
            this.Bt_sig,
            this.toolStripSeparator9,
            this.Bt_ret,
            this.toolStripSeparator10,
            this.Bt_fin,
            this.toolStripSeparator11});
            this.toolStrip1.Location = new System.Drawing.Point(0, 521);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(986, 35);
            this.toolStrip1.TabIndex = 162;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.BackColor = System.Drawing.Color.Black;
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_add
            // 
            this.Bt_add.AutoSize = false;
            this.Bt_add.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_add.Name = "Bt_add";
            this.Bt_add.Size = new System.Drawing.Size(32, 32);
            this.Bt_add.Text = "Bt_close";
            this.Bt_add.ToolTipText = "Nuevo ";
            this.Bt_add.Click += new System.EventHandler(this.Bt_add_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.AutoSize = false;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_close
            // 
            this.Bt_close.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Bt_close.AutoSize = false;
            this.Bt_close.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_close.Name = "Bt_close";
            this.Bt_close.Size = new System.Drawing.Size(32, 32);
            this.Bt_close.ToolTipText = "Salir del formulario";
            this.Bt_close.Click += new System.EventHandler(this.Bt_close_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator7.AutoSize = false;
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(3, 45);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.AutoSize = false;
            this.toolStripSeparator6.BackColor = System.Drawing.Color.Black;
            this.toolStripSeparator6.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_edit
            // 
            this.Bt_edit.AutoSize = false;
            this.Bt_edit.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_edit.Name = "Bt_edit";
            this.Bt_edit.Size = new System.Drawing.Size(32, 32);
            this.Bt_edit.ToolTipText = "Editar ";
            this.Bt_edit.Click += new System.EventHandler(this.Bt_edit_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.AutoSize = false;
            this.toolStripSeparator15.BackColor = System.Drawing.Color.Black;
            this.toolStripSeparator15.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_anul
            // 
            this.Bt_anul.AutoSize = false;
            this.Bt_anul.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_anul.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_anul.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_anul.Name = "Bt_anul";
            this.Bt_anul.Size = new System.Drawing.Size(32, 32);
            this.Bt_anul.Text = "Bt_close";
            this.Bt_anul.ToolTipText = "Anulación";
            this.Bt_anul.Click += new System.EventHandler(this.Bt_anul_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.AutoSize = false;
            this.toolStripSeparator13.BackColor = System.Drawing.Color.Black;
            this.toolStripSeparator13.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(3, 45);
            // 
            // bt_view
            // 
            this.bt_view.AutoSize = false;
            this.bt_view.BackColor = System.Drawing.SystemColors.Control;
            this.bt_view.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_view.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_view.Name = "bt_view";
            this.bt_view.Size = new System.Drawing.Size(32, 32);
            this.bt_view.Text = "Bt_close";
            this.bt_view.ToolTipText = "Solo ver";
            this.bt_view.Click += new System.EventHandler(this.bt_view_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.AutoSize = false;
            this.toolStripSeparator14.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_print
            // 
            this.Bt_print.AutoSize = false;
            this.Bt_print.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_print.Name = "Bt_print";
            this.Bt_print.Size = new System.Drawing.Size(32, 32);
            this.Bt_print.ToolTipText = "Imprimir";
            this.Bt_print.Click += new System.EventHandler(this.Bt_print_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.AutoSize = false;
            this.toolStripSeparator12.BackColor = System.Drawing.Color.Black;
            this.toolStripSeparator12.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(3, 45);
            // 
            // bt_prev
            // 
            this.bt_prev.AutoSize = false;
            this.bt_prev.BackColor = System.Drawing.SystemColors.Control;
            this.bt_prev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_prev.Name = "bt_prev";
            this.bt_prev.Size = new System.Drawing.Size(32, 32);
            this.bt_prev.ToolTipText = "Pre-visualizar";
            this.bt_prev.Click += new System.EventHandler(this.bt_prev_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(3, 45);
            // 
            // bt_exc
            // 
            this.bt_exc.AutoSize = false;
            this.bt_exc.BackColor = System.Drawing.SystemColors.Control;
            this.bt_exc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_exc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_exc.Name = "bt_exc";
            this.bt_exc.Size = new System.Drawing.Size(32, 32);
            this.bt_exc.Text = "Bt_close";
            this.bt_exc.ToolTipText = "Exportar";
            this.bt_exc.Click += new System.EventHandler(this.bt_exc_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AutoSize = false;
            this.toolStripSeparator4.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(3, 45);
            // 
            // Tx_modo
            // 
            this.Tx_modo.Name = "Tx_modo";
            this.Tx_modo.ReadOnly = true;
            this.Tx_modo.Size = new System.Drawing.Size(100, 35);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(3, 45);
            // 
            // Bt_ini
            // 
            this.Bt_ini.AutoSize = false;
            this.Bt_ini.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_ini.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_ini.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_ini.Name = "Bt_ini";
            this.Bt_ini.Size = new System.Drawing.Size(32, 32);
            this.Bt_ini.Text = "Bt_close";
            this.Bt_ini.ToolTipText = "Ir al inicio";
            this.Bt_ini.Visible = false;
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.AutoSize = false;
            this.toolStripSeparator8.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(3, 45);
            this.toolStripSeparator8.Visible = false;
            // 
            // Bt_sig
            // 
            this.Bt_sig.AutoSize = false;
            this.Bt_sig.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_sig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_sig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_sig.Name = "Bt_sig";
            this.Bt_sig.Size = new System.Drawing.Size(32, 32);
            this.Bt_sig.Text = "Bt_close";
            this.Bt_sig.ToolTipText = "Siguiente";
            this.Bt_sig.Visible = false;
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.AutoSize = false;
            this.toolStripSeparator9.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(3, 45);
            this.toolStripSeparator9.Visible = false;
            // 
            // Bt_ret
            // 
            this.Bt_ret.AutoSize = false;
            this.Bt_ret.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_ret.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_ret.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_ret.Name = "Bt_ret";
            this.Bt_ret.Size = new System.Drawing.Size(32, 32);
            this.Bt_ret.Text = "Bt_close";
            this.Bt_ret.ToolTipText = "Regresar";
            this.Bt_ret.Visible = false;
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.AutoSize = false;
            this.toolStripSeparator10.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(3, 45);
            this.toolStripSeparator10.Visible = false;
            // 
            // Bt_fin
            // 
            this.Bt_fin.AutoSize = false;
            this.Bt_fin.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_fin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bt_fin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bt_fin.Name = "Bt_fin";
            this.Bt_fin.Size = new System.Drawing.Size(32, 32);
            this.Bt_fin.Text = "Bt_close";
            this.Bt_fin.ToolTipText = "Ir al final";
            this.Bt_fin.Visible = false;
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.AutoSize = false;
            this.toolStripSeparator11.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(3, 45);
            // 
            // cpagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 556);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "cpagos";
            this.Text = "Control de Pagos";
            this.Load += new System.EventHandler(this.cpagos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.users_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabgrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).EndInit();
            this.tabuser.ResumeLayout(false);
            this.tabuser.PerformLayout();
            this.groupBox27.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabgrilla;
        private ADGV.AdvancedDataGridView advancedDataGridView1;
        private System.Windows.Forms.TabPage tabuser;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tx_rind;
        private System.Windows.Forms.TextBox tx_idr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton Bt_close;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton Bt_edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripButton Bt_anul;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton bt_view;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripButton Bt_print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton bt_prev;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_exc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripTextBox Tx_modo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton Bt_ini;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton Bt_sig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton Bt_ret;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton Bt_fin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.ToolStripButton Bt_add;
    }
}