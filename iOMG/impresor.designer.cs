namespace iOMG
{
    partial class impresor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(impresor));
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tx_dat_jgo = new System.Windows.Forms.TextBox();
            this.tx_paq = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tx_dat_det2 = new System.Windows.Forms.TextBox();
            this.tx_dat_tal = new System.Windows.Forms.TextBox();
            this.tx_dat_aca = new System.Windows.Forms.TextBox();
            this.tx_dat_det1 = new System.Windows.Forms.TextBox();
            this.tx_dat_tip = new System.Windows.Forms.TextBox();
            this.tx_dat_mad = new System.Windows.Forms.TextBox();
            this.tx_dat_mod = new System.Windows.Forms.TextBox();
            this.tx_dat_cap = new System.Windows.Forms.TextBox();
            this.tx_cant = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tx_medidas = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tx_nombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_idm = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tx_dat_det3 = new System.Windows.Forms.TextBox();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.tx_acabado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // tx_dat_jgo
            // 
            this.tx_dat_jgo.Location = new System.Drawing.Point(358, 89);
            this.tx_dat_jgo.Name = "tx_dat_jgo";
            this.tx_dat_jgo.ReadOnly = true;
            this.tx_dat_jgo.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_jgo.TabIndex = 67;
            this.tx_dat_jgo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_paq
            // 
            this.tx_paq.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_paq.Location = new System.Drawing.Point(362, 118);
            this.tx_paq.Name = "tx_paq";
            this.tx_paq.ReadOnly = true;
            this.tx_paq.Size = new System.Drawing.Size(35, 26);
            this.tx_paq.TabIndex = 53;
            this.tx_paq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 26);
            this.label5.TabIndex = 66;
            this.label5.Text = "Paquetes\r\npor Mueble\r\n";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(-51, 121);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 65;
            this.label18.Text = "Código";
            // 
            // tx_dat_det2
            // 
            this.tx_dat_det2.Location = new System.Drawing.Point(314, 62);
            this.tx_dat_det2.Name = "tx_dat_det2";
            this.tx_dat_det2.ReadOnly = true;
            this.tx_dat_det2.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_det2.TabIndex = 64;
            this.tx_dat_det2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_tal
            // 
            this.tx_dat_tal.Location = new System.Drawing.Point(270, 62);
            this.tx_dat_tal.Name = "tx_dat_tal";
            this.tx_dat_tal.ReadOnly = true;
            this.tx_dat_tal.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_tal.TabIndex = 63;
            this.tx_dat_tal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_aca
            // 
            this.tx_dat_aca.Location = new System.Drawing.Point(226, 62);
            this.tx_dat_aca.Name = "tx_dat_aca";
            this.tx_dat_aca.ReadOnly = true;
            this.tx_dat_aca.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_aca.TabIndex = 62;
            this.tx_dat_aca.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_det1
            // 
            this.tx_dat_det1.Location = new System.Drawing.Point(182, 62);
            this.tx_dat_det1.Name = "tx_dat_det1";
            this.tx_dat_det1.ReadOnly = true;
            this.tx_dat_det1.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_det1.TabIndex = 61;
            this.tx_dat_det1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_tip
            // 
            this.tx_dat_tip.Location = new System.Drawing.Point(138, 62);
            this.tx_dat_tip.Name = "tx_dat_tip";
            this.tx_dat_tip.ReadOnly = true;
            this.tx_dat_tip.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_tip.TabIndex = 60;
            this.tx_dat_tip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_mad
            // 
            this.tx_dat_mad.Location = new System.Drawing.Point(94, 62);
            this.tx_dat_mad.Name = "tx_dat_mad";
            this.tx_dat_mad.ReadOnly = true;
            this.tx_dat_mad.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_mad.TabIndex = 59;
            this.tx_dat_mad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_mod
            // 
            this.tx_dat_mod.Location = new System.Drawing.Point(50, 62);
            this.tx_dat_mod.Name = "tx_dat_mod";
            this.tx_dat_mod.ReadOnly = true;
            this.tx_dat_mod.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_mod.TabIndex = 58;
            this.tx_dat_mod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_dat_cap
            // 
            this.tx_dat_cap.Location = new System.Drawing.Point(6, 62);
            this.tx_dat_cap.Name = "tx_dat_cap";
            this.tx_dat_cap.ReadOnly = true;
            this.tx_dat_cap.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_cap.TabIndex = 57;
            this.tx_dat_cap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_cant
            // 
            this.tx_cant.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_cant.Location = new System.Drawing.Point(240, 118);
            this.tx_cant.Name = "tx_cant";
            this.tx_cant.ReadOnly = true;
            this.tx_cant.Size = new System.Drawing.Size(34, 26);
            this.tx_cant.TabIndex = 52;
            this.tx_cant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 26);
            this.label3.TabIndex = 56;
            this.label3.Text = "Cantidad\r\nde Muebles\r\n";
            // 
            // tx_medidas
            // 
            this.tx_medidas.Location = new System.Drawing.Point(6, 116);
            this.tx_medidas.Name = "tx_medidas";
            this.tx_medidas.ReadOnly = true;
            this.tx_medidas.Size = new System.Drawing.Size(147, 20);
            this.tx_medidas.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-51, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Medidas";
            // 
            // tx_nombre
            // 
            this.tx_nombre.Location = new System.Drawing.Point(6, 89);
            this.tx_nombre.Name = "tx_nombre";
            this.tx_nombre.ReadOnly = true;
            this.tx_nombre.Size = new System.Drawing.Size(346, 20);
            this.tx_nombre.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-51, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Nombre";
            // 
            // tx_idm
            // 
            this.tx_idm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_idm.Location = new System.Drawing.Point(67, 149);
            this.tx_idm.Name = "tx_idm";
            this.tx_idm.ReadOnly = true;
            this.tx_idm.Size = new System.Drawing.Size(34, 26);
            this.tx_idm.TabIndex = 68;
            this.tx_idm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 69;
            this.label4.Text = "ID Mueble";
            // 
            // tx_dat_det3
            // 
            this.tx_dat_det3.Location = new System.Drawing.Point(358, 63);
            this.tx_dat_det3.Name = "tx_dat_det3";
            this.tx_dat_det3.ReadOnly = true;
            this.tx_dat_det3.Size = new System.Drawing.Size(39, 20);
            this.tx_dat_det3.TabIndex = 70;
            this.tx_dat_det3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // tx_acabado
            // 
            this.tx_acabado.Location = new System.Drawing.Point(138, 155);
            this.tx_acabado.Name = "tx_acabado";
            this.tx_acabado.ReadOnly = true;
            this.tx_acabado.Size = new System.Drawing.Size(73, 20);
            this.tx_acabado.TabIndex = 71;
            // 
            // impresor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 219);
            this.Controls.Add(this.tx_acabado);
            this.Controls.Add(this.tx_dat_det3);
            this.Controls.Add(this.tx_idm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tx_dat_jgo);
            this.Controls.Add(this.tx_paq);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tx_dat_det2);
            this.Controls.Add(this.tx_dat_tal);
            this.Controls.Add(this.tx_dat_aca);
            this.Controls.Add(this.tx_dat_det1);
            this.Controls.Add(this.tx_dat_tip);
            this.Controls.Add(this.tx_dat_mad);
            this.Controls.Add(this.tx_dat_mod);
            this.Controls.Add(this.tx_dat_cap);
            this.Controls.Add(this.tx_cant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tx_medidas);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tx_nombre);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "impresor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IMPRESOR";
            this.Load += new System.EventHandler(this.impresor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TextBox tx_dat_jgo;
        private System.Windows.Forms.TextBox tx_paq;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tx_dat_det2;
        private System.Windows.Forms.TextBox tx_dat_tal;
        private System.Windows.Forms.TextBox tx_dat_aca;
        private System.Windows.Forms.TextBox tx_dat_det1;
        private System.Windows.Forms.TextBox tx_dat_tip;
        private System.Windows.Forms.TextBox tx_dat_mad;
        private System.Windows.Forms.TextBox tx_dat_mod;
        private System.Windows.Forms.TextBox tx_dat_cap;
        private System.Windows.Forms.TextBox tx_cant;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tx_medidas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tx_nombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_idm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tx_dat_det3;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.TextBox tx_acabado;
    }
}