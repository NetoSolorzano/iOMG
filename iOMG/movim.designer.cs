namespace iOMG
{
    partial class movim
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lb_titulo = new System.Windows.Forms.Label();
            this.bt_close = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tx_d_codi = new System.Windows.Forms.TextBox();
            this.tx_idr = new System.Windows.Forms.TextBox();
            this.tx_comres = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tx_status = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tx_tienda = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_fecon = new System.Windows.Forms.TextBox();
            this.lb_fecon = new System.Windows.Forms.Label();
            this.tx_cliente = new System.Windows.Forms.TextBox();
            this.lb_cliente = new System.Windows.Forms.Label();
            this.tx_contra = new System.Windows.Forms.TextBox();
            this.lb_contra = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tx_dat_dest = new System.Windows.Forms.TextBox();
            this.cmb_dest = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_fsal = new System.Windows.Forms.DateTimePicker();
            this.rb_ajuste = new System.Windows.Forms.RadioButton();
            this.rb_mov = new System.Windows.Forms.RadioButton();
            this.tx_comsal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tx_evento = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(2, 381);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 31);
            this.panel1.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(532, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 29);
            this.button1.TabIndex = 16;
            this.button1.Text = "Graba";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.Controls.Add(this.lb_titulo);
            this.panel2.Controls.Add(this.bt_close);
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 26);
            this.panel2.TabIndex = 16;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            // 
            // lb_titulo
            // 
            this.lb_titulo.AutoSize = true;
            this.lb_titulo.Location = new System.Drawing.Point(280, 7);
            this.lb_titulo.Name = "lb_titulo";
            this.lb_titulo.Size = new System.Drawing.Size(73, 13);
            this.lb_titulo.TabIndex = 15;
            this.lb_titulo.Text = "Titulo del form";
            // 
            // bt_close
            // 
            this.bt_close.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bt_close.FlatAppearance.BorderSize = 0;
            this.bt_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_close.ForeColor = System.Drawing.Color.White;
            this.bt_close.Image = global::iOMG.Properties.Resources.close_square;
            this.bt_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bt_close.Location = new System.Drawing.Point(599, 4);
            this.bt_close.Name = "bt_close";
            this.bt_close.Size = new System.Drawing.Size(23, 18);
            this.bt_close.TabIndex = 14;
            this.bt_close.UseVisualStyleBackColor = true;
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.tx_d_codi);
            this.panel3.Controls.Add(this.tx_idr);
            this.panel3.Controls.Add(this.tx_comres);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.tx_status);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.tx_tienda);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.tx_fecon);
            this.panel3.Controls.Add(this.lb_fecon);
            this.panel3.Controls.Add(this.tx_cliente);
            this.panel3.Controls.Add(this.lb_cliente);
            this.panel3.Controls.Add(this.tx_contra);
            this.panel3.Controls.Add(this.lb_contra);
            this.panel3.Location = new System.Drawing.Point(2, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(626, 100);
            this.panel3.TabIndex = 18;
            // 
            // tx_d_codi
            // 
            this.tx_d_codi.Location = new System.Drawing.Point(277, 5);
            this.tx_d_codi.Name = "tx_d_codi";
            this.tx_d_codi.ReadOnly = true;
            this.tx_d_codi.Size = new System.Drawing.Size(149, 20);
            this.tx_d_codi.TabIndex = 16;
            this.tx_d_codi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_idr
            // 
            this.tx_idr.Location = new System.Drawing.Point(212, 28);
            this.tx_idr.Name = "tx_idr";
            this.tx_idr.Size = new System.Drawing.Size(36, 20);
            this.tx_idr.TabIndex = 15;
            this.tx_idr.Visible = false;
            // 
            // tx_comres
            // 
            this.tx_comres.Location = new System.Drawing.Point(78, 75);
            this.tx_comres.Name = "tx_comres";
            this.tx_comres.Size = new System.Drawing.Size(534, 20);
            this.tx_comres.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Comentario";
            // 
            // tx_status
            // 
            this.tx_status.Location = new System.Drawing.Point(522, 28);
            this.tx_status.Name = "tx_status";
            this.tx_status.ReadOnly = true;
            this.tx_status.Size = new System.Drawing.Size(90, 20);
            this.tx_status.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(439, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Estado";
            // 
            // tx_tienda
            // 
            this.tx_tienda.Location = new System.Drawing.Point(78, 28);
            this.tx_tienda.Name = "tx_tienda";
            this.tx_tienda.ReadOnly = true;
            this.tx_tienda.Size = new System.Drawing.Size(100, 20);
            this.tx_tienda.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Tienda";
            // 
            // tx_fecon
            // 
            this.tx_fecon.Location = new System.Drawing.Point(522, 5);
            this.tx_fecon.Name = "tx_fecon";
            this.tx_fecon.ReadOnly = true;
            this.tx_fecon.Size = new System.Drawing.Size(90, 20);
            this.tx_fecon.TabIndex = 5;
            // 
            // lb_fecon
            // 
            this.lb_fecon.AutoSize = true;
            this.lb_fecon.Location = new System.Drawing.Point(439, 9);
            this.lb_fecon.Name = "lb_fecon";
            this.lb_fecon.Size = new System.Drawing.Size(80, 13);
            this.lb_fecon.TabIndex = 4;
            this.lb_fecon.Text = "Fecha Contrato";
            // 
            // tx_cliente
            // 
            this.tx_cliente.Location = new System.Drawing.Point(78, 51);
            this.tx_cliente.Name = "tx_cliente";
            this.tx_cliente.ReadOnly = true;
            this.tx_cliente.Size = new System.Drawing.Size(534, 20);
            this.tx_cliente.TabIndex = 3;
            // 
            // lb_cliente
            // 
            this.lb_cliente.AutoSize = true;
            this.lb_cliente.Location = new System.Drawing.Point(8, 55);
            this.lb_cliente.Name = "lb_cliente";
            this.lb_cliente.Size = new System.Drawing.Size(39, 13);
            this.lb_cliente.TabIndex = 2;
            this.lb_cliente.Text = "Cliente";
            // 
            // tx_contra
            // 
            this.tx_contra.Location = new System.Drawing.Point(78, 5);
            this.tx_contra.Name = "tx_contra";
            this.tx_contra.Size = new System.Drawing.Size(100, 20);
            this.tx_contra.TabIndex = 0;
            this.tx_contra.Leave += new System.EventHandler(this.tx_contra_Leave);
            // 
            // lb_contra
            // 
            this.lb_contra.AutoSize = true;
            this.lb_contra.Location = new System.Drawing.Point(8, 9);
            this.lb_contra.Name = "lb_contra";
            this.lb_contra.Size = new System.Drawing.Size(67, 13);
            this.lb_contra.TabIndex = 0;
            this.lb_contra.Text = "CONTRATO";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 132);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(625, 248);
            this.dataGridView1.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel4.Controls.Add(this.tx_dat_dest);
            this.panel4.Controls.Add(this.cmb_dest);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.dtp_fsal);
            this.panel4.Controls.Add(this.rb_ajuste);
            this.panel4.Controls.Add(this.rb_mov);
            this.panel4.Controls.Add(this.tx_comsal);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.tx_evento);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(2, 148);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(626, 100);
            this.panel4.TabIndex = 21;
            // 
            // tx_dat_dest
            // 
            this.tx_dat_dest.Location = new System.Drawing.Point(398, 29);
            this.tx_dat_dest.Name = "tx_dat_dest";
            this.tx_dat_dest.Size = new System.Drawing.Size(36, 20);
            this.tx_dat_dest.TabIndex = 14;
            this.tx_dat_dest.Visible = false;
            // 
            // cmb_dest
            // 
            this.cmb_dest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_dest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_dest.FormattingEnabled = true;
            this.cmb_dest.Location = new System.Drawing.Point(514, 28);
            this.cmb_dest.Name = "cmb_dest";
            this.cmb_dest.Size = new System.Drawing.Size(99, 21);
            this.cmb_dest.TabIndex = 13;
            this.cmb_dest.SelectedIndexChanged += new System.EventHandler(this.cmb_dest_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(440, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Alm. Destino";
            // 
            // dtp_fsal
            // 
            this.dtp_fsal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_fsal.Location = new System.Drawing.Point(514, 4);
            this.dtp_fsal.Name = "dtp_fsal";
            this.dtp_fsal.Size = new System.Drawing.Size(99, 20);
            this.dtp_fsal.TabIndex = 11;
            // 
            // rb_ajuste
            // 
            this.rb_ajuste.AutoSize = true;
            this.rb_ajuste.Location = new System.Drawing.Point(98, 23);
            this.rb_ajuste.Name = "rb_ajuste";
            this.rb_ajuste.Size = new System.Drawing.Size(54, 17);
            this.rb_ajuste.TabIndex = 1;
            this.rb_ajuste.TabStop = true;
            this.rb_ajuste.Text = "Ajuste";
            this.rb_ajuste.UseVisualStyleBackColor = true;
            this.rb_ajuste.CheckedChanged += new System.EventHandler(this.rb_ajuste_CheckedChanged);
            // 
            // rb_mov
            // 
            this.rb_mov.AutoSize = true;
            this.rb_mov.Location = new System.Drawing.Point(98, 4);
            this.rb_mov.Name = "rb_mov";
            this.rb_mov.Size = new System.Drawing.Size(79, 17);
            this.rb_mov.TabIndex = 0;
            this.rb_mov.TabStop = true;
            this.rb_mov.Text = "Movimiento";
            this.rb_mov.UseVisualStyleBackColor = true;
            this.rb_mov.CheckedChanged += new System.EventHandler(this.rb_mov_CheckedChanged);
            // 
            // tx_comsal
            // 
            this.tx_comsal.Location = new System.Drawing.Point(78, 75);
            this.tx_comsal.Name = "tx_comsal";
            this.tx_comsal.Size = new System.Drawing.Size(535, 20);
            this.tx_comsal.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Comentario";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(440, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Fecha Salida";
            // 
            // tx_evento
            // 
            this.tx_evento.Location = new System.Drawing.Point(78, 52);
            this.tx_evento.Name = "tx_evento";
            this.tx_evento.Size = new System.Drawing.Size(535, 20);
            this.tx_evento.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Evento";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "TIPO SALIDA";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(195, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Item contratado";
            // 
            // movim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(627, 411);
            this.ControlBox = false;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "movim";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.movim_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button bt_close;
        private System.Windows.Forms.Label lb_titulo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tx_fecon;
        private System.Windows.Forms.Label lb_fecon;
        private System.Windows.Forms.TextBox tx_cliente;
        private System.Windows.Forms.Label lb_cliente;
        private System.Windows.Forms.TextBox tx_contra;
        private System.Windows.Forms.Label lb_contra;
        private System.Windows.Forms.TextBox tx_status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tx_tienda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_comres;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox tx_comsal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tx_evento;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rb_ajuste;
        private System.Windows.Forms.RadioButton rb_mov;
        private System.Windows.Forms.DateTimePicker dtp_fsal;
        private System.Windows.Forms.ComboBox cmb_dest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tx_dat_dest;
        private System.Windows.Forms.TextBox tx_idr;
        private System.Windows.Forms.TextBox tx_d_codi;
        private System.Windows.Forms.Label label6;
    }
}