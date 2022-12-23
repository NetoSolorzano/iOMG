namespace iOMG
{
    partial class forpagos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(forpagos));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.tx_numOpe = new System.Windows.Forms.TextBox();
            this.cmb_plazo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.it = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nompag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codpag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fpago = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_mas = new System.Windows.Forms.Button();
            this.tx_dat_mp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tx_fpago = new System.Windows.Forms.DateTimePicker();
            this.tx_tfil = new iOMG.NumericTextBox();
            this.tx_total = new iOMG.NumericTextBox();
            this.tx_importe = new iOMG.NumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(757, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = global::iOMG.Properties.Resources.floppy_red;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Location = new System.Drawing.Point(272, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 38);
            this.button2.TabIndex = 6;
            this.button2.Text = "GRABA";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(116, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 13);
            this.label16.TabIndex = 413;
            this.label16.Text = "Nro.OPER";
            // 
            // tx_numOpe
            // 
            this.tx_numOpe.Location = new System.Drawing.Point(113, 24);
            this.tx_numOpe.Name = "tx_numOpe";
            this.tx_numOpe.Size = new System.Drawing.Size(97, 20);
            this.tx_numOpe.TabIndex = 1;
            this.tx_numOpe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmb_plazo
            // 
            this.cmb_plazo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_plazo.DropDownWidth = 100;
            this.cmb_plazo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_plazo.FormattingEnabled = true;
            this.cmb_plazo.Location = new System.Drawing.Point(7, 22);
            this.cmb_plazo.Name = "cmb_plazo";
            this.cmb_plazo.Size = new System.Drawing.Size(98, 21);
            this.cmb_plazo.TabIndex = 0;
            this.cmb_plazo.SelectionChangeCommitted += new System.EventHandler(this.cmb_plazo_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(231, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 414;
            this.label2.Text = "Importe S/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 415;
            this.label3.Text = "Medio de pago";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idc,
            this.it,
            this.nompag,
            this.noper,
            this.importe,
            this.codpag,
            this.fpago});
            this.dataGridView1.Location = new System.Drawing.Point(6, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 15;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(415, 111);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserDeletedRow);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // idc
            // 
            this.idc.HeaderText = "IDC";
            this.idc.Name = "idc";
            this.idc.ReadOnly = true;
            this.idc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.idc.Width = 30;
            // 
            // it
            // 
            this.it.HeaderText = "It";
            this.it.Name = "it";
            this.it.ReadOnly = true;
            this.it.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.it.Width = 20;
            // 
            // nompag
            // 
            this.nompag.HeaderText = "MEDIO";
            this.nompag.Name = "nompag";
            this.nompag.ReadOnly = true;
            this.nompag.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nompag.Width = 90;
            // 
            // noper
            // 
            this.noper.HeaderText = "OPERAC";
            this.noper.Name = "noper";
            this.noper.ReadOnly = true;
            this.noper.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.noper.Width = 80;
            // 
            // importe
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.importe.DefaultCellStyle = dataGridViewCellStyle1;
            this.importe.HeaderText = "Imp.S/";
            this.importe.Name = "importe";
            this.importe.ReadOnly = true;
            this.importe.Width = 90;
            // 
            // codpag
            // 
            this.codpag.HeaderText = "codpag";
            this.codpag.Name = "codpag";
            this.codpag.ReadOnly = true;
            this.codpag.Visible = false;
            // 
            // fpago
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.fpago.DefaultCellStyle = dataGridViewCellStyle2;
            this.fpago.HeaderText = "F_PAGO";
            this.fpago.Name = "fpago";
            this.fpago.ReadOnly = true;
            this.fpago.Width = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 418;
            this.label1.Text = "Importe Total";
            // 
            // bt_mas
            // 
            this.bt_mas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_mas.Location = new System.Drawing.Point(392, 18);
            this.bt_mas.Name = "bt_mas";
            this.bt_mas.Size = new System.Drawing.Size(29, 28);
            this.bt_mas.TabIndex = 4;
            this.bt_mas.Text = "+";
            this.bt_mas.UseVisualStyleBackColor = true;
            this.bt_mas.Click += new System.EventHandler(this.bt_mas_Click);
            // 
            // tx_dat_mp
            // 
            this.tx_dat_mp.Location = new System.Drawing.Point(82, 0);
            this.tx_dat_mp.Name = "tx_dat_mp";
            this.tx_dat_mp.Size = new System.Drawing.Size(40, 20);
            this.tx_dat_mp.TabIndex = 420;
            this.tx_dat_mp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tx_dat_mp.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 422;
            this.label4.Text = "Fecha Pago";
            // 
            // tx_fpago
            // 
            this.tx_fpago.Checked = false;
            this.tx_fpago.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tx_fpago.Location = new System.Drawing.Point(304, 24);
            this.tx_fpago.Name = "tx_fpago";
            this.tx_fpago.Size = new System.Drawing.Size(82, 20);
            this.tx_fpago.TabIndex = 3;
            this.tx_fpago.ValueChanged += new System.EventHandler(this.tx_fpago_ValueChanged);
            // 
            // tx_tfil
            // 
            this.tx_tfil.AllowSpace = false;
            this.tx_tfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_tfil.Location = new System.Drawing.Point(219, 174);
            this.tx_tfil.Name = "tx_tfil";
            this.tx_tfil.ReadOnly = true;
            this.tx_tfil.Size = new System.Drawing.Size(31, 21);
            this.tx_tfil.TabIndex = 419;
            this.tx_tfil.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tx_tfil.Visible = false;
            // 
            // tx_total
            // 
            this.tx_total.AllowSpace = false;
            this.tx_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_total.Location = new System.Drawing.Point(115, 174);
            this.tx_total.Name = "tx_total";
            this.tx_total.ReadOnly = true;
            this.tx_total.Size = new System.Drawing.Size(82, 21);
            this.tx_total.TabIndex = 417;
            this.tx_total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_importe
            // 
            this.tx_importe.AllowSpace = false;
            this.tx_importe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_importe.Location = new System.Drawing.Point(216, 24);
            this.tx_importe.Name = "tx_importe";
            this.tx_importe.Size = new System.Drawing.Size(82, 21);
            this.tx_importe.TabIndex = 2;
            this.tx_importe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // forpagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 209);
            this.Controls.Add(this.tx_fpago);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tx_dat_mp);
            this.Controls.Add(this.tx_tfil);
            this.Controls.Add(this.bt_mas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tx_total);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tx_numOpe);
            this.Controls.Add(this.cmb_plazo);
            this.Controls.Add(this.tx_importe);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "forpagos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MEDIOS DE PAGO";
            this.Load += new System.EventHandler(this.forpagos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.forpagos_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tx_numOpe;
        private System.Windows.Forms.ComboBox cmb_plazo;
        private NumericTextBox tx_importe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private NumericTextBox tx_total;
        private System.Windows.Forms.Button bt_mas;
        private NumericTextBox tx_tfil;
        private System.Windows.Forms.TextBox tx_dat_mp;
        private System.Windows.Forms.DataGridViewTextBoxColumn idc;
        private System.Windows.Forms.DataGridViewTextBoxColumn it;
        private System.Windows.Forms.DataGridViewTextBoxColumn nompag;
        private System.Windows.Forms.DataGridViewTextBoxColumn noper;
        private System.Windows.Forms.DataGridViewTextBoxColumn importe;
        private System.Windows.Forms.DataGridViewTextBoxColumn codpag;
        private System.Windows.Forms.DataGridViewTextBoxColumn fpago;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker tx_fpago;
    }
}
