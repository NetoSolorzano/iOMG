﻿namespace iOMG
{
    partial class ayuda1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ayuda1));
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tx_codigo = new System.Windows.Forms.TextBox();
            this.tx_id = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_buscar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tx_nombre = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(359, 458);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(438, 416);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            // 
            // tx_codigo
            // 
            this.tx_codigo.Location = new System.Drawing.Point(150, 462);
            this.tx_codigo.Name = "tx_codigo";
            this.tx_codigo.Size = new System.Drawing.Size(33, 20);
            this.tx_codigo.TabIndex = 2;
            this.tx_codigo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tx_codigo_KeyPress);
            // 
            // tx_id
            // 
            this.tx_id.Location = new System.Drawing.Point(189, 462);
            this.tx_id.Name = "tx_id";
            this.tx_id.Size = new System.Drawing.Size(49, 20);
            this.tx_id.TabIndex = 3;
            this.tx_id.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 465);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Información seleccionada ";
            // 
            // tx_buscar
            // 
            this.tx_buscar.Location = new System.Drawing.Point(106, 5);
            this.tx_buscar.Name = "tx_buscar";
            this.tx_buscar.Size = new System.Drawing.Size(148, 20);
            this.tx_buscar.TabIndex = 1;
            this.tx_buscar.Leave += new System.EventHandler(this.tx_buscar_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nombre a buscar";
            // 
            // tx_nombre
            // 
            this.tx_nombre.Location = new System.Drawing.Point(244, 462);
            this.tx_nombre.Name = "tx_nombre";
            this.tx_nombre.Size = new System.Drawing.Size(92, 20);
            this.tx_nombre.TabIndex = 7;
            this.tx_nombre.Visible = false;
            // 
            // ayuda1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 497);
            this.Controls.Add(this.tx_nombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tx_buscar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tx_id);
            this.Controls.Add(this.tx_codigo);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ayuda1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AYUDA";
            this.Load += new System.EventHandler(this.ayuda1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tx_codigo;
        private System.Windows.Forms.TextBox tx_id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_buscar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tx_nombre;
    }
}
