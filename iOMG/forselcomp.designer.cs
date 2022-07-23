namespace iOMG
{
    partial class forselcomp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(forselcomp));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_total = new iOMG.NumericTextBox();
            this.tx_tfil = new iOMG.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.marca = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.docvta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ndoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipdv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serdv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cordv = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.button2.Location = new System.Drawing.Point(501, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 38);
            this.button2.TabIndex = 5;
            this.button2.Text = "GRABA";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.marca,
            this.docvta,
            this.fecha,
            this.ndoc,
            this.cliente,
            this.importe,
            this.tipdv,
            this.serdv,
            this.cordv});
            this.dataGridView1.Location = new System.Drawing.Point(3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 15;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(625, 156);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 418;
            this.label1.Text = "Importe seleccionado";
            // 
            // tx_total
            // 
            this.tx_total.AllowSpace = false;
            this.tx_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_total.Location = new System.Drawing.Point(122, 174);
            this.tx_total.Name = "tx_total";
            this.tx_total.ReadOnly = true;
            this.tx_total.Size = new System.Drawing.Size(82, 21);
            this.tx_total.TabIndex = 417;
            this.tx_total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tx_tfil
            // 
            this.tx_tfil.AllowSpace = false;
            this.tx_tfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_tfil.Location = new System.Drawing.Point(375, 174);
            this.tx_tfil.Name = "tx_tfil";
            this.tx_tfil.ReadOnly = true;
            this.tx_tfil.Size = new System.Drawing.Size(31, 21);
            this.tx_tfil.TabIndex = 419;
            this.tx_tfil.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 420;
            this.label2.Text = "Comprobantes seleccionados";
            // 
            // marca
            // 
            this.marca.HeaderText = "";
            this.marca.Name = "marca";
            this.marca.Width = 20;
            // 
            // docvta
            // 
            this.docvta.HeaderText = "DOC.VTA.";
            this.docvta.Name = "docvta";
            this.docvta.ReadOnly = true;
            this.docvta.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.docvta.Width = 70;
            // 
            // fecha
            // 
            this.fecha.HeaderText = "FECHA";
            this.fecha.Name = "fecha";
            this.fecha.ReadOnly = true;
            this.fecha.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.fecha.Width = 90;
            // 
            // ndoc
            // 
            this.ndoc.HeaderText = "N.DOC.";
            this.ndoc.Name = "ndoc";
            this.ndoc.ReadOnly = true;
            this.ndoc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cliente
            // 
            this.cliente.HeaderText = "CLIENTE";
            this.cliente.Name = "cliente";
            this.cliente.ReadOnly = true;
            this.cliente.Width = 230;
            // 
            // importe
            // 
            this.importe.HeaderText = "IMPORTE";
            this.importe.Name = "importe";
            this.importe.ReadOnly = true;
            // 
            // tipdv
            // 
            this.tipdv.HeaderText = "tipdv";
            this.tipdv.Name = "tipdv";
            this.tipdv.ReadOnly = true;
            this.tipdv.Visible = false;
            // 
            // serdv
            // 
            this.serdv.HeaderText = "serdv";
            this.serdv.Name = "serdv";
            this.serdv.ReadOnly = true;
            this.serdv.Visible = false;
            // 
            // cordv
            // 
            this.cordv.HeaderText = "cordv";
            this.cordv.Name = "cordv";
            this.cordv.ReadOnly = true;
            this.cordv.Visible = false;
            // 
            // forselcomp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 211);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tx_tfil);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tx_total);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "forselcomp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COMPROBANTES SIN CONTRATO";
            this.Load += new System.EventHandler(this.forselcomp_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.forselcomp_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private NumericTextBox tx_total;
        private NumericTextBox tx_tfil;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn marca;
        private System.Windows.Forms.DataGridViewTextBoxColumn docvta;
        private System.Windows.Forms.DataGridViewTextBoxColumn fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn ndoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn importe;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipdv;
        private System.Windows.Forms.DataGridViewTextBoxColumn serdv;
        private System.Windows.Forms.DataGridViewTextBoxColumn cordv;
    }
}
