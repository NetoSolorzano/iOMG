namespace iOMG
{
    partial class pan_inicio
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lb_titulo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_todos = new System.Windows.Forms.RadioButton();
            this.rb_redu = new System.Windows.Forms.RadioButton();
            this.rb_estan = new System.Windows.Forms.RadioButton();
            this.lb_col = new System.Windows.Forms.Label();
            this.bt_borra = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tx_tarti = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.advancedDataGridView1 = new ADGV.AdvancedDataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt_reserva = new System.Windows.Forms.Button();
            this.bt_salida = new System.Windows.Forms.Button();
            this.tx_totprec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bt_etiq = new System.Windows.Forms.Button();
            this.bt_print = new System.Windows.Forms.Button();
            this.bt_expex = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.bt_bmf = new System.Windows.Forms.Button();
            this.pan_ico = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_titulo
            // 
            this.lb_titulo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_titulo.AutoSize = true;
            this.lb_titulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_titulo.Location = new System.Drawing.Point(376, -3);
            this.lb_titulo.Name = "lb_titulo";
            this.lb_titulo.Size = new System.Drawing.Size(266, 25);
            this.lb_titulo.TabIndex = 0;
            this.lb_titulo.Text = "GESTION DE ALMACEN";
            this.lb_titulo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rb_todos);
            this.panel1.Controls.Add(this.rb_redu);
            this.panel1.Controls.Add(this.rb_estan);
            this.panel1.Controls.Add(this.lb_col);
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 25);
            this.panel1.TabIndex = 15;
            // 
            // rb_todos
            // 
            this.rb_todos.AutoSize = true;
            this.rb_todos.Location = new System.Drawing.Point(283, 3);
            this.rb_todos.Name = "rb_todos";
            this.rb_todos.Size = new System.Drawing.Size(55, 17);
            this.rb_todos.TabIndex = 4;
            this.rb_todos.TabStop = true;
            this.rb_todos.Text = "Todos";
            this.rb_todos.UseVisualStyleBackColor = true;
            this.rb_todos.CheckedChanged += new System.EventHandler(this.rb_todos_CheckedChanged);
            // 
            // rb_redu
            // 
            this.rb_redu.AutoSize = true;
            this.rb_redu.Location = new System.Drawing.Point(183, 3);
            this.rb_redu.Name = "rb_redu";
            this.rb_redu.Size = new System.Drawing.Size(71, 17);
            this.rb_redu.TabIndex = 2;
            this.rb_redu.TabStop = true;
            this.rb_redu.Text = "Reducido";
            this.rb_redu.UseVisualStyleBackColor = true;
            this.rb_redu.CheckedChanged += new System.EventHandler(this.rb_redu_CheckedChanged);
            // 
            // rb_estan
            // 
            this.rb_estan.AutoSize = true;
            this.rb_estan.Location = new System.Drawing.Point(87, 3);
            this.rb_estan.Name = "rb_estan";
            this.rb_estan.Size = new System.Drawing.Size(67, 17);
            this.rb_estan.TabIndex = 1;
            this.rb_estan.TabStop = true;
            this.rb_estan.Text = "Estandar";
            this.rb_estan.UseVisualStyleBackColor = true;
            this.rb_estan.CheckedChanged += new System.EventHandler(this.rb_estan_CheckedChanged);
            // 
            // lb_col
            // 
            this.lb_col.AutoSize = true;
            this.lb_col.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_col.Location = new System.Drawing.Point(3, 4);
            this.lb_col.Name = "lb_col";
            this.lb_col.Size = new System.Drawing.Size(61, 13);
            this.lb_col.TabIndex = 0;
            this.lb_col.Text = "Columnas";
            // 
            // bt_borra
            // 
            this.bt_borra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_borra.Location = new System.Drawing.Point(598, 25);
            this.bt_borra.Name = "bt_borra";
            this.bt_borra.Size = new System.Drawing.Size(95, 25);
            this.bt_borra.TabIndex = 14;
            this.bt_borra.Text = "REINICIA TODO";
            this.bt_borra.UseVisualStyleBackColor = true;
            this.bt_borra.Click += new System.EventHandler(this.bt_borra_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(1, 75);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView2.Size = new System.Drawing.Size(998, 26);
            this.dataGridView2.TabIndex = 13;
            this.dataGridView2.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellLeave);
            // 
            // tx_tarti
            // 
            this.tx_tarti.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tx_tarti.Location = new System.Drawing.Point(445, 484);
            this.tx_tarti.Name = "tx_tarti";
            this.tx_tarti.ReadOnly = true;
            this.tx_tarti.Size = new System.Drawing.Size(42, 20);
            this.tx_tarti.TabIndex = 12;
            this.tx_tarti.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(353, 488);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total artículos";
            // 
            // advancedDataGridView1
            // 
            this.advancedDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedDataGridView1.AutoGenerateContextFilters = true;
            this.advancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advancedDataGridView1.DateWithTime = false;
            this.advancedDataGridView1.Location = new System.Drawing.Point(1, 101);
            this.advancedDataGridView1.Name = "advancedDataGridView1";
            this.advancedDataGridView1.Size = new System.Drawing.Size(998, 377);
            this.advancedDataGridView1.TabIndex = 16;
            this.advancedDataGridView1.TimeFilter = false;
            this.advancedDataGridView1.SortStringChanged += new System.EventHandler(this.advancedDataGridView1_SortStringChanged);
            this.advancedDataGridView1.FilterStringChanged += new System.EventHandler(this.advancedDataGridView1_FilterStringChanged);
            this.advancedDataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.advancedDataGridView1_CellBeginEdit);
            this.advancedDataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.advancedDataGridView1_CellEndEdit);
            this.advancedDataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.advancedDataGridView1_CellValueChanged);
            this.advancedDataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.advancedDataGridView1_ColumnWidthChanged);
            this.advancedDataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.advancedDataGridView1_CurrentCellDirtyStateChanged);
            this.advancedDataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.advancedDataGridView1_Scroll);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.Size = new System.Drawing.Size(998, 24);
            this.dataGridView1.TabIndex = 17;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // bt_reserva
            // 
            this.bt_reserva.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_reserva.Location = new System.Drawing.Point(798, 25);
            this.bt_reserva.Name = "bt_reserva";
            this.bt_reserva.Size = new System.Drawing.Size(95, 25);
            this.bt_reserva.TabIndex = 18;
            this.bt_reserva.Text = "Reserva masiva";
            this.bt_reserva.UseVisualStyleBackColor = true;
            this.bt_reserva.Click += new System.EventHandler(this.bt_reserva_Click);
            // 
            // bt_salida
            // 
            this.bt_salida.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_salida.Location = new System.Drawing.Point(902, 25);
            this.bt_salida.Name = "bt_salida";
            this.bt_salida.Size = new System.Drawing.Size(95, 25);
            this.bt_salida.TabIndex = 19;
            this.bt_salida.Text = "Salida masiva";
            this.bt_salida.UseVisualStyleBackColor = true;
            this.bt_salida.Click += new System.EventHandler(this.bt_salida_Click);
            // 
            // tx_totprec
            // 
            this.tx_totprec.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tx_totprec.Location = new System.Drawing.Point(608, 484);
            this.tx_totprec.Name = "tx_totprec";
            this.tx_totprec.ReadOnly = true;
            this.tx_totprec.Size = new System.Drawing.Size(85, 20);
            this.tx_totprec.TabIndex = 23;
            this.tx_totprec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(529, 488);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Total Precio";
            // 
            // bt_etiq
            // 
            this.bt_etiq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_etiq.Location = new System.Drawing.Point(957, 478);
            this.bt_etiq.Name = "bt_etiq";
            this.bt_etiq.Size = new System.Drawing.Size(32, 31);
            this.bt_etiq.TabIndex = 27;
            this.toolTip1.SetToolTip(this.bt_etiq, "Imprime Etiqueta");
            this.bt_etiq.UseVisualStyleBackColor = true;
            this.bt_etiq.Click += new System.EventHandler(this.bt_etiq_Click);
            // 
            // bt_print
            // 
            this.bt_print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_print.Location = new System.Drawing.Point(48, 478);
            this.bt_print.Name = "bt_print";
            this.bt_print.Size = new System.Drawing.Size(32, 31);
            this.bt_print.TabIndex = 25;
            this.toolTip1.SetToolTip(this.bt_print, "Imprime lo visible");
            this.bt_print.UseVisualStyleBackColor = true;
            this.bt_print.Click += new System.EventHandler(this.bt_print_Click);
            // 
            // bt_expex
            // 
            this.bt_expex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_expex.Location = new System.Drawing.Point(10, 478);
            this.bt_expex.Name = "bt_expex";
            this.bt_expex.Size = new System.Drawing.Size(32, 31);
            this.bt_expex.TabIndex = 24;
            this.toolTip1.SetToolTip(this.bt_expex, "Exporta a Excel");
            this.bt_expex.UseVisualStyleBackColor = true;
            this.bt_expex.Click += new System.EventHandler(this.bt_expex_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // bt_bmf
            // 
            this.bt_bmf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_bmf.Location = new System.Drawing.Point(376, 25);
            this.bt_bmf.Name = "bt_bmf";
            this.bt_bmf.Size = new System.Drawing.Size(115, 25);
            this.bt_bmf.TabIndex = 26;
            this.bt_bmf.Text = "Borra marcas de fila";
            this.bt_bmf.UseVisualStyleBackColor = true;
            this.bt_bmf.Click += new System.EventHandler(this.bt_bmf_Click);
            // 
            // pan_ico
            // 
            this.pan_ico.Location = new System.Drawing.Point(729, 2);
            this.pan_ico.Name = "pan_ico";
            this.pan_ico.Size = new System.Drawing.Size(53, 47);
            this.pan_ico.TabIndex = 44;
            this.pan_ico.Visible = false;
            // 
            // pan_inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pan_ico);
            this.Controls.Add(this.bt_etiq);
            this.Controls.Add(this.bt_bmf);
            this.Controls.Add(this.bt_print);
            this.Controls.Add(this.bt_expex);
            this.Controls.Add(this.tx_totprec);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bt_salida);
            this.Controls.Add(this.bt_reserva);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.advancedDataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bt_borra);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.tx_tarti);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_titulo);
            this.Name = "pan_inicio";
            this.Size = new System.Drawing.Size(1000, 508);
            this.Load += new System.EventHandler(this.pan_inicio_Load);
            this.Enter += new System.EventHandler(this.pan_inicio_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pan_inicio_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_titulo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rb_todos;
        private System.Windows.Forms.RadioButton rb_redu;
        private System.Windows.Forms.RadioButton rb_estan;
        private System.Windows.Forms.Label lb_col;
        private System.Windows.Forms.Button bt_borra;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TextBox tx_tarti;
        private System.Windows.Forms.Label label2;
        private ADGV.AdvancedDataGridView advancedDataGridView1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bt_reserva;
        private System.Windows.Forms.Button bt_salida;
        private System.Windows.Forms.TextBox tx_totprec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bt_expex;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bt_print;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button bt_bmf;
        private System.Windows.Forms.Button bt_etiq;
        private System.Windows.Forms.Panel pan_ico;
    }
}
