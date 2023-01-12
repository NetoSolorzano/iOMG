namespace iOMG
{
    partial class forpcred
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(forpcred));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.tx_num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_mas = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tx_fpago = new System.Windows.Forms.DateTimePicker();
            this.tx_tfil = new iOMG.NumericTextBox();
            this.tx_total = new iOMG.NumericTextBox();
            this.tx_importe = new iOMG.NumericTextBox();
            this.idc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cuota = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fpago = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.button2.Location = new System.Drawing.Point(235, 166);
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
            this.label16.Location = new System.Drawing.Point(22, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 413;
            this.label16.Text = "Cuota";
            // 
            // tx_num
            // 
            this.tx_num.Location = new System.Drawing.Point(19, 24);
            this.tx_num.Name = "tx_num";
            this.tx_num.ReadOnly = true;
            this.tx_num.Size = new System.Drawing.Size(39, 20);
            this.tx_num.TabIndex = 1;
            this.tx_num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(99, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 414;
            this.label2.Text = "Importe S/";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idc,
            this.cuota,
            this.importe,
            this.fpago});
            this.dataGridView1.Location = new System.Drawing.Point(6, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 15;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(328, 111);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserDeletedRow);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 418;
            this.label1.Text = "Importe Total";
            // 
            // bt_mas
            // 
            this.bt_mas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_mas.Location = new System.Drawing.Point(293, 12);
            this.bt_mas.Name = "bt_mas";
            this.bt_mas.Size = new System.Drawing.Size(37, 34);
            this.bt_mas.TabIndex = 4;
            this.bt_mas.Text = "+";
            this.bt_mas.UseVisualStyleBackColor = true;
            this.bt_mas.Click += new System.EventHandler(this.bt_mas_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 422;
            this.label4.Text = "Fecha Pago";
            // 
            // tx_fpago
            // 
            this.tx_fpago.Checked = false;
            this.tx_fpago.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tx_fpago.Location = new System.Drawing.Point(189, 24);
            this.tx_fpago.Name = "tx_fpago";
            this.tx_fpago.Size = new System.Drawing.Size(82, 20);
            this.tx_fpago.TabIndex = 3;
            this.tx_fpago.ValueChanged += new System.EventHandler(this.tx_fpago_ValueChanged);
            // 
            // tx_tfil
            // 
            this.tx_tfil.AllowSpace = false;
            this.tx_tfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_tfil.Location = new System.Drawing.Point(182, 174);
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
            this.tx_total.Location = new System.Drawing.Point(78, 174);
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
            this.tx_importe.Location = new System.Drawing.Point(83, 24);
            this.tx_importe.Name = "tx_importe";
            this.tx_importe.Size = new System.Drawing.Size(82, 21);
            this.tx_importe.TabIndex = 2;
            this.tx_importe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // idc
            // 
            this.idc.HeaderText = "IDC";
            this.idc.Name = "idc";
            this.idc.ReadOnly = true;
            this.idc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.idc.Width = 40;
            // 
            // cuota
            // 
            this.cuota.HeaderText = "Cuota";
            this.cuota.Name = "cuota";
            this.cuota.ReadOnly = true;
            this.cuota.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cuota.Width = 50;
            // 
            // importe
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.importe.DefaultCellStyle = dataGridViewCellStyle1;
            this.importe.HeaderText = "Imp.S/";
            this.importe.Name = "importe";
            this.importe.ReadOnly = true;
            // 
            // fpago
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.fpago.DefaultCellStyle = dataGridViewCellStyle2;
            this.fpago.FillWeight = 120F;
            this.fpago.HeaderText = "F_PAGO";
            this.fpago.Name = "fpago";
            this.fpago.ReadOnly = true;
            this.fpago.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.fpago.Width = 120;
            // 
            // forpcred
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 209);
            this.Controls.Add(this.tx_fpago);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tx_tfil);
            this.Controls.Add(this.bt_mas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tx_total);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tx_num);
            this.Controls.Add(this.tx_importe);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "forpcred";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CREDITOS Y FECHAS";
            this.Load += new System.EventHandler(this.forpcred_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.forpcred_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tx_num;
        private NumericTextBox tx_importe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private NumericTextBox tx_total;
        private System.Windows.Forms.Button bt_mas;
        private NumericTextBox tx_tfil;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker tx_fpago;
        private System.Windows.Forms.DataGridViewTextBoxColumn idc;
        private System.Windows.Forms.DataGridViewTextBoxColumn cuota;
        private System.Windows.Forms.DataGridViewTextBoxColumn importe;
        private System.Windows.Forms.DataGridViewTextBoxColumn fpago;
    }
}
