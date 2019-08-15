namespace iOMG
{
    partial class regpagos
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt_det = new System.Windows.Forms.Button();
            this.tx_total = new iOMG.NumericTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tx_idr = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tx_cont = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_pago = new System.Windows.Forms.DateTimePicker();
            this.tx_importe = new iOMG.NumericTextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_fpago = new System.Windows.Forms.ComboBox();
            this.tx_dat_fpago = new System.Windows.Forms.TextBox();
            this.cmb_td = new System.Windows.Forms.ComboBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tx_serie = new iOMG.NumericTextBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tx_corre = new iOMG.NumericTextBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.tx_comen = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tx_dat_td = new System.Windows.Forms.TextBox();
            this.cmb_mone = new System.Windows.Forms.ComboBox();
            this.tx_dat_mone = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 65);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(797, 179);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // bt_det
            // 
            this.bt_det.Location = new System.Drawing.Point(731, 11);
            this.bt_det.Name = "bt_det";
            this.bt_det.Size = new System.Drawing.Size(58, 41);
            this.bt_det.TabIndex = 8;
            this.bt_det.Text = "Agrega  Actualiza";
            this.bt_det.UseVisualStyleBackColor = true;
            this.bt_det.Click += new System.EventHandler(this.bt_det_Click);
            // 
            // tx_total
            // 
            this.tx_total.AllowSpace = false;
            this.tx_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_total.Location = new System.Drawing.Point(631, 269);
            this.tx_total.Name = "tx_total";
            this.tx_total.ReadOnly = true;
            this.tx_total.Size = new System.Drawing.Size(78, 20);
            this.tx_total.TabIndex = 10;
            this.tx_total.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Image = global::iOMG.Properties.Resources.floppy_red;
            this.button1.Location = new System.Drawing.Point(726, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 50);
            this.button1.TabIndex = 11;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tx_idr
            // 
            this.tx_idr.Location = new System.Drawing.Point(30, 8);
            this.tx_idr.Name = "tx_idr";
            this.tx_idr.ReadOnly = true;
            this.tx_idr.Size = new System.Drawing.Size(37, 20);
            this.tx_idr.TabIndex = 336;
            this.tx_idr.Tag = "Id";
            this.tx_idr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DimGray;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(71, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 1);
            this.groupBox1.TabIndex = 339;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(0, -26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(164, 1);
            this.groupBox2.TabIndex = 113;
            this.groupBox2.TabStop = false;
            // 
            // tx_cont
            // 
            this.tx_cont.Location = new System.Drawing.Point(146, 8);
            this.tx_cont.Name = "tx_cont";
            this.tx_cont.ReadOnly = true;
            this.tx_cont.Size = new System.Drawing.Size(81, 20);
            this.tx_cont.TabIndex = 0;
            this.tx_cont.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(72, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 338;
            this.label5.Text = "Num.Contrato";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.DimGray;
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(6, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(61, 1);
            this.groupBox3.TabIndex = 341;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(0, -26);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(164, 1);
            this.groupBox4.TabIndex = 113;
            this.groupBox4.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 340;
            this.label1.Text = "ID ";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.DimGray;
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(230, 27);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(156, 1);
            this.groupBox5.TabIndex = 344;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(0, -26);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(164, 1);
            this.groupBox6.TabIndex = 113;
            this.groupBox6.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 343;
            this.label2.Text = "Fecha pago";
            // 
            // dtp_pago
            // 
            this.dtp_pago.Checked = false;
            this.dtp_pago.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_pago.Location = new System.Drawing.Point(303, 8);
            this.dtp_pago.Name = "dtp_pago";
            this.dtp_pago.Size = new System.Drawing.Size(95, 20);
            this.dtp_pago.TabIndex = 1;
            // 
            // tx_importe
            // 
            this.tx_importe.AllowSpace = false;
            this.tx_importe.Location = new System.Drawing.Point(466, 8);
            this.tx_importe.Name = "tx_importe";
            this.tx_importe.Size = new System.Drawing.Size(87, 20);
            this.tx_importe.TabIndex = 2;
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.DimGray;
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Location = new System.Drawing.Point(402, 27);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(151, 1);
            this.groupBox7.TabIndex = 348;
            this.groupBox7.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Location = new System.Drawing.Point(0, -26);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(164, 1);
            this.groupBox8.TabIndex = 113;
            this.groupBox8.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 347;
            this.label3.Text = "Importe S/";
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.DimGray;
            this.groupBox9.Controls.Add(this.groupBox10);
            this.groupBox9.Location = new System.Drawing.Point(558, 288);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(151, 1);
            this.groupBox9.TabIndex = 350;
            this.groupBox9.TabStop = false;
            // 
            // groupBox10
            // 
            this.groupBox10.Location = new System.Drawing.Point(0, -26);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(164, 1);
            this.groupBox10.TabIndex = 113;
            this.groupBox10.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(560, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 349;
            this.label4.Text = "TOTAL  S/";
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.DimGray;
            this.groupBox11.Controls.Add(this.groupBox12);
            this.groupBox11.Location = new System.Drawing.Point(557, 26);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(161, 1);
            this.groupBox11.TabIndex = 352;
            this.groupBox11.TabStop = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Location = new System.Drawing.Point(0, -26);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(164, 1);
            this.groupBox12.TabIndex = 113;
            this.groupBox12.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(560, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 351;
            this.label6.Text = "Forma Pago";
            // 
            // cmb_fpago
            // 
            this.cmb_fpago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_fpago.FormattingEnabled = true;
            this.cmb_fpago.Location = new System.Drawing.Point(628, 6);
            this.cmb_fpago.Name = "cmb_fpago";
            this.cmb_fpago.Size = new System.Drawing.Size(90, 21);
            this.cmb_fpago.TabIndex = 3;
            // 
            // tx_dat_fpago
            // 
            this.tx_dat_fpago.Location = new System.Drawing.Point(604, 0);
            this.tx_dat_fpago.Name = "tx_dat_fpago";
            this.tx_dat_fpago.ReadOnly = true;
            this.tx_dat_fpago.Size = new System.Drawing.Size(18, 20);
            this.tx_dat_fpago.TabIndex = 354;
            this.tx_dat_fpago.Tag = "Id";
            this.tx_dat_fpago.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tx_dat_fpago.Visible = false;
            // 
            // cmb_td
            // 
            this.cmb_td.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_td.FormattingEnabled = true;
            this.cmb_td.Location = new System.Drawing.Point(40, 35);
            this.cmb_td.Name = "cmb_td";
            this.cmb_td.Size = new System.Drawing.Size(42, 21);
            this.cmb_td.TabIndex = 4;
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.DimGray;
            this.groupBox13.Controls.Add(this.groupBox14);
            this.groupBox13.Location = new System.Drawing.Point(4, 55);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(78, 1);
            this.groupBox13.TabIndex = 356;
            this.groupBox13.TabStop = false;
            // 
            // groupBox14
            // 
            this.groupBox14.Location = new System.Drawing.Point(0, -26);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(164, 1);
            this.groupBox14.TabIndex = 113;
            this.groupBox14.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 355;
            this.label7.Text = "Tipo";
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.Color.DimGray;
            this.groupBox15.Controls.Add(this.groupBox16);
            this.groupBox15.Location = new System.Drawing.Point(85, 55);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(74, 1);
            this.groupBox15.TabIndex = 360;
            this.groupBox15.TabStop = false;
            // 
            // groupBox16
            // 
            this.groupBox16.Location = new System.Drawing.Point(0, -26);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(164, 1);
            this.groupBox16.TabIndex = 113;
            this.groupBox16.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(88, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 359;
            this.label8.Text = "Serie";
            // 
            // tx_serie
            // 
            this.tx_serie.AllowSpace = false;
            this.tx_serie.Location = new System.Drawing.Point(122, 36);
            this.tx_serie.Name = "tx_serie";
            this.tx_serie.Size = new System.Drawing.Size(37, 20);
            this.tx_serie.TabIndex = 5;
            // 
            // groupBox17
            // 
            this.groupBox17.BackColor = System.Drawing.Color.DimGray;
            this.groupBox17.Controls.Add(this.groupBox18);
            this.groupBox17.Location = new System.Drawing.Point(163, 55);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(108, 1);
            this.groupBox17.TabIndex = 363;
            this.groupBox17.TabStop = false;
            // 
            // groupBox18
            // 
            this.groupBox18.Location = new System.Drawing.Point(0, -26);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(164, 1);
            this.groupBox18.TabIndex = 113;
            this.groupBox18.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(167, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 362;
            this.label9.Text = "Nro.";
            // 
            // tx_corre
            // 
            this.tx_corre.AllowSpace = false;
            this.tx_corre.Location = new System.Drawing.Point(198, 36);
            this.tx_corre.Name = "tx_corre";
            this.tx_corre.Size = new System.Drawing.Size(73, 20);
            this.tx_corre.TabIndex = 6;
            // 
            // groupBox19
            // 
            this.groupBox19.BackColor = System.Drawing.Color.DimGray;
            this.groupBox19.Controls.Add(this.groupBox20);
            this.groupBox19.Location = new System.Drawing.Point(277, 55);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(441, 1);
            this.groupBox19.TabIndex = 366;
            this.groupBox19.TabStop = false;
            // 
            // groupBox20
            // 
            this.groupBox20.Location = new System.Drawing.Point(0, -26);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(164, 1);
            this.groupBox20.TabIndex = 113;
            this.groupBox20.TabStop = false;
            // 
            // tx_comen
            // 
            this.tx_comen.Location = new System.Drawing.Point(342, 36);
            this.tx_comen.Name = "tx_comen";
            this.tx_comen.Size = new System.Drawing.Size(376, 20);
            this.tx_comen.TabIndex = 7;
            this.tx_comen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tx_comen.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(278, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 365;
            this.label10.Text = "Comentario";
            // 
            // tx_dat_td
            // 
            this.tx_dat_td.Location = new System.Drawing.Point(85, 29);
            this.tx_dat_td.Name = "tx_dat_td";
            this.tx_dat_td.ReadOnly = true;
            this.tx_dat_td.Size = new System.Drawing.Size(18, 20);
            this.tx_dat_td.TabIndex = 367;
            this.tx_dat_td.Tag = "Id";
            this.tx_dat_td.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tx_dat_td.Visible = false;
            // 
            // cmb_mone
            // 
            this.cmb_mone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_mone.FormattingEnabled = true;
            this.cmb_mone.Location = new System.Drawing.Point(25, 265);
            this.cmb_mone.Name = "cmb_mone";
            this.cmb_mone.Size = new System.Drawing.Size(42, 21);
            this.cmb_mone.TabIndex = 368;
            // 
            // tx_dat_mone
            // 
            this.tx_dat_mone.Location = new System.Drawing.Point(75, 266);
            this.tx_dat_mone.Name = "tx_dat_mone";
            this.tx_dat_mone.ReadOnly = true;
            this.tx_dat_mone.Size = new System.Drawing.Size(55, 20);
            this.tx_dat_mone.TabIndex = 369;
            this.tx_dat_mone.Tag = "Id";
            this.tx_dat_mone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // regpagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 315);
            this.Controls.Add(this.tx_dat_mone);
            this.Controls.Add(this.cmb_mone);
            this.Controls.Add(this.tx_dat_td);
            this.Controls.Add(this.groupBox19);
            this.Controls.Add(this.tx_comen);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox17);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tx_corre);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tx_serie);
            this.Controls.Add(this.cmb_td);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tx_dat_fpago);
            this.Controls.Add(this.cmb_fpago);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tx_importe);
            this.Controls.Add(this.dtp_pago);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tx_cont);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tx_idr);
            this.Controls.Add(this.tx_total);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_det);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "regpagos";
            this.Text = "Registro de Pagos";
            this.Load += new System.EventHandler(this.regpagos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.regpagos_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bt_det;
        private NumericTextBox tx_total;
        private System.Windows.Forms.TextBox tx_idr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tx_cont;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp_pago;
        private NumericTextBox tx_importe;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_fpago;
        private System.Windows.Forms.TextBox tx_dat_fpago;
        private System.Windows.Forms.ComboBox cmb_td;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Label label8;
        private NumericTextBox tx_serie;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label label9;
        private NumericTextBox tx_corre;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.TextBox tx_comen;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tx_dat_td;
        private System.Windows.Forms.ComboBox cmb_mone;
        private System.Windows.Forms.TextBox tx_dat_mone;
    }
}