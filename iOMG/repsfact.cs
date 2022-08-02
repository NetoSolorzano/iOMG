﻿using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class repsfact : Form
    {
        static string nomform = "repsfact";           // nombre del formulario
        string colback = iOMG.Program.colbac;         // color de fondo
        string colpage = iOMG.Program.colpag;         // color de los pageframes
        string colgrid = iOMG.Program.colgri;         // color de las grillas
        //string colfogr = iOMG.Program.colfog;   // color fondo con grillas
        //string colsfon = iOMG.Program.colsbg;   // color fondo seleccion
        //string colsfgr = iOMG.Program.colsfc;   // color seleccion grilla
        string colstrp = iOMG.Program.colstr;         // color del strip
        static string nomtab = "cabfactu";            // 

        #region variables
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        public int totfilgrid, cta;             // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string codfact = "";
        string coddni = "";
        string codruc = "";
        string codmon = "";
        //string tiesta = "";
        string img_btN = "";
        string img_btE = "";
        string img_btP = "";
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string img_imprime = "";
        string img_preview = "";        // imagen del boton preview e imprimir reporte
        string cliente = Program.cliente;    // razon social para los reportes
        string codAnul = "";            // codigo de documento anulado
        string nomAnul = "";            // texto nombre del estado anulado
        string codGene = "";            // codigo documento nuevo generado
        //int pageCount = 1, cuenta = 0;
        #endregion

        libreria lib = new libreria();

        DataTable dtestad = new DataTable();
        DataTable dttaller = new DataTable();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public repsfact()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            // en este form no usamos
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void repsfact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void repsfact_Load(object sender, EventArgs e)
        {
            /*
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            // Set up the delays for the ToolTip.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            */
            dataload("todos");
            jalainfo();
            init();
            toolboton();
            KeyPreview = true;
            tabControl1.Enabled = false;
            //
        }
        private void init()
        {
            tabControl1.BackColor = Color.FromName(iOMG.Program.colgri);
            this.BackColor = Color.FromName(colback);
            toolStrip1.BackColor = Color.FromName(colstrp);
            //
            dgv_facts.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_detfact.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_vtabar.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_notcre.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            //
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_btA);
            //Bt_ver.Image = Image.FromFile(img_btV);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_close.Image = Image.FromFile(img_btq);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            // 
        }
        private void jalainfo()                                     // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped,@clie)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "docsvta");
                micon.Parameters.AddWithValue("@clie","clients");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["campo"].ToString() == "imagenes" && row["formulario"].ToString() == "main")
                    {
                        if (row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                        if (row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                        if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                        if (row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                        if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                        //if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();        // imagen del boton de accion IMPRIMIR
                        if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                        if (row["param"].ToString() == "img_imprime") img_imprime = row["valor"].ToString().Trim();  // imagen del boton IMPRIMIR REPORTE
                        if (row["param"].ToString() == "img_pre") img_preview = row["valor"].ToString().Trim();  // imagen del boton VISTA PRELIMINAR
                    }
                    if (row["formulario"].ToString() == "docsvta")
                    {
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "codAnu") codAnul = row["valor"].ToString().Trim();         // codigo doc anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "generado") codGene = row["valor"].ToString().Trim();        // codigo doc generado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "Anulado") nomAnul = row["valor"].ToString().Trim();         // nombre estado anulado
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") codmon = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "factura") codfact = row["valor"].ToString().Trim();         // tipo de pedido por defecto en almacen
                    }
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "dni") coddni = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ruc") codruc = row["valor"].ToString().Trim();
                    }
                }
                da.Dispose();
                dt.Dispose();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        public void dataload(string quien)                          // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (quien == "todos")
            {
                string filtro = "";
                // ***************** seleccion de la tienda de ventas
                if (int.Parse(Program.nivuser) > 1)
                {
                    filtro = "and idcodice='" + Program.tdauser + "' ";
                }
                string contaller = "select descrizionerid,idcodice,codigo from desc_ven " +
                                       "where numero=1 " + filtro + "order by idcodice";
                MySqlCommand cmd = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmd);
                dataller.Fill(dttaller);
                // PANEL facturacion
                cmb_sede_guias.DataSource = dttaller;
                cmb_sede_guias.DisplayMember = "descrizionerid";
                cmb_sede_guias.ValueMember = "idcodice";
                // PANEL notas de credito
                cmb_sede_plan.DataSource = dttaller;
                cmb_sede_plan.DisplayMember = "descrizionerid"; ;
                cmb_sede_plan.ValueMember = "idcodice";
                // PANEL FACT. DETALLE
                cmb_loc_det.DataSource = dttaller;
                cmb_loc_det.DisplayMember = "descrizionerid";
                cmb_loc_det.ValueMember = "idcodice";
                // panel rep. ventas estilo barranco
                cmb_vtabar.DataSource = dttaller;
                cmb_vtabar.DisplayMember = "descrizionerid";
                cmb_vtabar.ValueMember = "idcodice";
                // panel reg.ventas
                cmb_rvtas_sede.DataSource = dttaller;
                cmb_rvtas_sede.DisplayMember = "descrizionerid";
                cmb_rvtas_sede.ValueMember = "idcodice";

                // ***************** seleccion de estado de servicios
                string conestad = "select descrizionerid,idcodice,codigo from desc_est " +
                                       "where numero=1 order by idcodice";
                cmd = new MySqlCommand(conestad, conn);
                MySqlDataAdapter daestad = new MySqlDataAdapter(cmd);
                daestad.Fill(dtestad);
                // PANEL facturacion
                cmb_estad_guias.DataSource = dtestad;
                cmb_estad_guias.DisplayMember = "descrizionerid";
                cmb_estad_guias.ValueMember = "idcodice";
                // PANEL notas de credito
                cmb_estad_plan.DataSource = dtestad;
                cmb_estad_plan.DisplayMember = "descrizionerid";
                cmb_estad_plan.ValueMember = "idcodice";
                // *************** seleccion de segmento de ventas
                // aca falta ....

            }
            conn.Close();
        }
        private void grilla(string dgv)                             // 
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            int b;
            switch (dgv)
            {
                case "dgv_guias":
                    dgv_facts.Font = tiplg;
                    dgv_facts.DefaultCellStyle.Font = tiplg;
                    dgv_facts.RowTemplate.Height = 15;
                    dgv_facts.AllowUserToAddRows = false;
                    dgv_facts.Width = Parent.Width - 70; // 1015;
                    if (dgv_facts.DataSource == null) dgv_facts.ColumnCount = 11;
                    if (dgv_facts.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_facts.Columns.Count; i++)
                        {
                            dgv_facts.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_facts.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_facts.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_facts.Columns.Count; i++)
                        {
                            int a = dgv_facts.Columns[i].Width;
                            b += a;
                            dgv_facts.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_facts.Columns[i].Width = a;
                        }
                        if (b < dgv_facts.Width) dgv_facts.Width = b - 20;  // b + 60;
                        dgv_facts.ReadOnly = true;
                    }
                    suma_grilla("dgv_facts");
                    break;
                case "dgv_plan":
                    dgv_notcre.Font = tiplg;
                    dgv_notcre.DefaultCellStyle.Font = tiplg;
                    dgv_notcre.RowTemplate.Height = 15;
                    dgv_notcre.AllowUserToAddRows = false;
                    dgv_facts.Width = Parent.Width - 70; // 1015;
                    if (dgv_notcre.DataSource == null) dgv_notcre.ColumnCount = 11;
                    if (dgv_notcre.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_notcre.Columns.Count; i++)
                        {
                            dgv_notcre.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_notcre.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_notcre.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_notcre.Columns.Count; i++)
                        {
                            int a = dgv_notcre.Columns[i].Width;
                            b += a;
                            dgv_notcre.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_notcre.Columns[i].Width = a;
                        }
                        if (b < dgv_notcre.Width) dgv_notcre.Width = b - 20;    // b + 60 ;
                        dgv_notcre.ReadOnly = true;
                    }
                    suma_grilla("dgv_notcre");
                    break;
                case "dgv_detfact":
                    dgv_detfact.Font = tiplg;
                    dgv_detfact.DefaultCellStyle.Font = tiplg;
                    dgv_detfact.RowTemplate.Height = 15;
                    dgv_detfact.AllowUserToAddRows = false;
                    dgv_detfact.Width = Parent.Width - 70; // 1015;
                    break;
                case "dgv_vtabar":
                    dgv_vtabar.Font = tiplg;
                    dgv_vtabar.DefaultCellStyle.Font = tiplg;
                    dgv_vtabar.RowTemplate.Height = 15;
                    dgv_vtabar.AllowUserToAddRows = false;
                    dgv_vtabar.Width = Parent.Width - 70; // 1015;
                    break;
                case "dgv_regvtas":
                    dgv_regvtas.Font = tiplg;
                    dgv_regvtas.DefaultCellStyle.Font = tiplg;
                    dgv_regvtas.RowTemplate.Height = 15;
                    dgv_regvtas.AllowUserToAddRows = false;
                    dgv_regvtas.Width = Parent.Width - 70; // 1015;
                    if (dgv_regvtas.DataSource == null) dgv_regvtas.ColumnCount = 17;
                    if (dgv_regvtas.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_regvtas.Columns.Count; i++)
                        {
                            dgv_regvtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_regvtas.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_regvtas.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_regvtas.Columns.Count; i++)
                        {
                            int a = dgv_regvtas.Columns[i].Width;
                            b += a;
                            dgv_regvtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_regvtas.Columns[i].Width = a;
                        }
                        if (b < dgv_regvtas.Width) dgv_regvtas.Width = b - 20;    // b + 60 ;
                        dgv_regvtas.ReadOnly = true;
                    }
                    break;
            }
        }
        private void bt_guias_Click(object sender, EventArgs e)         // genera reporte facturacion
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_vtas_fact1";
                using (MySqlCommand micon = new MySqlCommand(consulta,conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@loca", (tx_sede_guias.Text != "") ? tx_sede_guias.Text : "");
                    micon.Parameters.AddWithValue("@fecini", dtp_ini_guias.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fin_guias.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@esta", (tx_estad_guias.Text != "") ? tx_estad_guias.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_excl_guias.Checked == true) ? "1" : "0");
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_facts.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_facts.DataSource = dt;
                        grilla("dgv_guias");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_plan_Click(object sender, EventArgs e)          // genera reporte planilla de carga
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_vtas_ncred1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fecini", dtp_fini_plan.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fter_plan.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@loca", (tx_dat_sede_plan.Text != "") ? tx_dat_sede_plan.Text : "");
                    micon.Parameters.AddWithValue("@esta", (tx_dat_estad_plan.Text != "") ? tx_dat_estad_plan.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_exclu_plan.Checked == true)? "1" : "0");
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_notcre.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_notcre.DataSource = dt;
                        grilla("dgv_plan");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_regvtas_Click(object sender, EventArgs e)       // genera reporte registro de ventas
        {
            if (tx_dat_rvtas_Sede.Text == "")
            {
                if (Program.nivuser == "1" || Program.nivuser == "0")
                {
                    // puede proceder
                }
                else
                {
                    MessageBox.Show("Debe seleccionar su local","Error de nivel",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
            }
            string consulta = "rep_vtas_regvtas1";
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                    {
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@fini", dtp_rvtas_fini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fter", dtp_rvtas_fter.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@vsede", tx_dat_rvtas_Sede.Text);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            dgv_regvtas.DataSource = null;
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgv_regvtas.DataSource = dt;
                            grilla("dgv_regvtas");
                        }
                        string resulta = lib.ult_mov(nomform, nomtab, asd);
                        if (resulta != "OK")                                        // actualizamos la tabla usuarios
                        {
                            MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }
        private void bt_det_Click(object sender, EventArgs e)           // facturacion detallada
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_vtas_factDet1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@loca", (tx_dat_locDet.Text != "") ? tx_dat_locDet.Text : "");
                    micon.Parameters.AddWithValue("@fecini", dtp_ini_det.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fin_det.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@segm", (tx_dat_segDet.Text != "") ? tx_dat_segDet.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_seg_det.Checked == true) ? "1" : "0");
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_detfact.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_detfact.DataSource = dt;
                        grilla("dgv_detfact");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btn_vtabar_Click(object sender, EventArgs e)       // Reporte ventas estilo barranco
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_vtas_barran";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@loca", (tx_dat_vtabar.Text != "") ? tx_dat_vtabar.Text : "");
                    micon.Parameters.AddWithValue("@fecini", dtp_ini_vtabar.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fin_vtabar.Value.ToString("yyyy-MM-dd"));
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_vtabar.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_vtabar.DataSource = dt;
                        grilla("dgv_vtabar");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void suma_grilla(string dgv)
        {
            string etiq_anulado = nomAnul;
            int cr = 0, ca = 0; // dgv_facts.Rows.Count;
            double tvv = 0, tva = 0;
            switch (dgv)
            {
                case "dgv_facts":
                    for (int i=0; i < dgv_facts.Rows.Count; i++)
                    {
                        if (dgv_facts.Rows[i].Cells["ESTADO"].Value.ToString() != etiq_anulado)
                        {
                            tvv = tvv + Convert.ToDouble(dgv_facts.Rows[i].Cells["TOTAL_MN"].Value);
                            cr = cr + 1;
                        }
                        else
                        {
                            dgv_facts.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            ca = ca + 1;
                            tva = tva + Convert.ToDouble(dgv_facts.Rows[i].Cells["TOTAL_MN"].Value);
                        }
                    }
                    tx_tfi_f.Text = cr.ToString();
                    tx_totval.Text = tvv.ToString("#0.00");
                    tx_tfi_a.Text = ca.ToString();
                    tx_totv_a.Text = tva.ToString("#0.00");
                    break;
                case "dgv_notcre":
                    for (int i = 0; i < dgv_notcre.Rows.Count; i++)
                    {
                        if (dgv_notcre.Rows[i].Cells["ESTADO"].Value.ToString() != etiq_anulado)
                        {
                            tvv = tvv + Convert.ToDouble(dgv_notcre.Rows[i].Cells["TOTAL_MN"].Value);
                            cr = cr + 1;
                        }
                        else
                        {
                            dgv_notcre.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            ca = ca + 1;
                            tva = tva + Convert.ToDouble(dgv_notcre.Rows[i].Cells["TOTAL_MN"].Value);
                        }
                    }
                    tx_tfi_n.Text = cr.ToString();
                    tx_totval_n.Text = tvv.ToString("#0.00");
                    break;
            }
        }

        #region combos
        private void cmb_sede_plan_SelectionChangeCommitted(object sender, EventArgs e) // ok
        {
            if (cmb_sede_plan.SelectedValue != null) tx_dat_sede_plan.Text = cmb_sede_plan.SelectedValue.ToString();
            else tx_dat_sede_plan.Text = "";
        }
        private void cmb_sede_plan_KeyDown(object sender, KeyEventArgs e)               // ok
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_sede_plan.SelectedIndex = -1;
                tx_dat_sede_plan.Text = "";
            }
        }
        private void cmb_estad_plan_SelectionChangeCommitted(object sender, EventArgs e)    // ok
        {
            if (cmb_estad_plan.SelectedValue != null) tx_dat_estad_plan.Text = cmb_estad_plan.SelectedValue.ToString();
            else tx_dat_estad_plan.Text = "";
        }
        private void cmb_estad_plan_KeyDown(object sender, KeyEventArgs e)                  // ok
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad_plan.SelectedIndex = -1;
                tx_dat_estad_plan.Text = "";
            }
        }
        private void cmb_sede_guias_SelectionChangeCommitted(object sender, EventArgs e)    // ok
        {
            if (cmb_sede_guias.SelectedValue != null) tx_sede_guias.Text = cmb_sede_guias.SelectedValue.ToString();
            else tx_sede_guias.Text = "";
        }
        private void cmb_sede_guias_KeyDown(object sender, KeyEventArgs e)                  // ok
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_sede_guias.SelectedIndex = -1;
                tx_sede_guias.Text = "";
            }
        }
        private void cmb_estad_guias_SelectionChangeCommitted(object sender, EventArgs e)   // ok
        {
            if (cmb_estad_guias.SelectedValue != null) tx_estad_guias.Text = cmb_estad_guias.SelectedValue.ToString();
            else tx_estad_guias.Text = "";
        }
        private void cmb_estad_guias_KeyDown(object sender, KeyEventArgs e)                 // ok
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad_guias.SelectedIndex = -1;
                tx_estad_guias.Text = "";
            }
        }
        private void cmb_loc_det_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_loc_det.SelectedValue != null) tx_dat_locDet.Text = cmb_loc_det.SelectedValue.ToString();
            else tx_dat_locDet.Text = "";
        }
        private void cmb_loc_det_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_loc_det.SelectedIndex = -1;
                tx_dat_locDet.Text = "";
            }
        }
        private void cmb_vtabar_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_vtabar.SelectedValue != null) tx_dat_vtabar.Text = cmb_vtabar.SelectedValue.ToString();
            else tx_dat_vtabar.Text = "";
        }
        private void cmb_vtabar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_vtabar.SelectedIndex = -1;
                tx_dat_vtabar.Text = "";
            }
        }
        private void cmb_rvtas_sede_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_rvtas_sede.SelectedValue != null) tx_dat_rvtas_Sede.Text = cmb_rvtas_sede.SelectedValue.ToString();
            else tx_dat_rvtas_Sede.Text = "";
        }
        private void cmb_rvtas_sede_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_rvtas_sede.SelectedIndex = -1;
                tx_dat_rvtas_Sede.Text = "";
            }
        }
        #endregion

        #region botones de comando
        public void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            Bt_print.Visible = false;
            bt_exc.Visible = false;
            Bt_ini.Visible = false;
            Bt_sig.Visible = false;
            Bt_ret.Visible = false;
            Bt_fin.Visible = false;
            //
            DataTable mdtb = new DataTable();
            const string consbot = "select * from permisos where formulario=@nomform and usuario=@use";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlCommand consulb = new MySqlCommand(consbot, conn);
                    consulb.Parameters.AddWithValue("@nomform", nomform);
                    consulb.Parameters.AddWithValue("@use", asd);
                    MySqlDataAdapter mab = new MySqlDataAdapter(consulb);
                    mab.Fill(mdtb);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return;
                }
                finally { conn.Close(); }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (mdtb.Rows.Count > 0)
            {
                DataRow row = mdtb.Rows[0];
                if (Convert.ToString(row["btn1"]) == "S")               // nuevo ... ok
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")               // editar ... ok
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")               // anular ... ok
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                /*if (Convert.ToString(row["btn4"]) == "S")               // visualizar ... ok
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }*/
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir ... ok
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                /*if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar ... ok
                {
                    this.bt_prev.Visible = true;
                }
                else { this.bt_prev.Visible = false; }*/
                if (Convert.ToString(row["btn8"]) == "S")               // exporta xlsx  .. ok
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")               // salir del form ... ok
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
        }
        private void Bt_add_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "IMPRIMIR";
            tabControl1.Enabled = true;
            //
            cmb_sede_guias.SelectedIndex = -1;
            cmb_estad_guias.SelectedIndex = -1;
            cmb_estad_plan.SelectedIndex = -1;
            cmb_loc_det.SelectedIndex = -1;
            cmb_sede_plan.SelectedIndex = -1;
            cmb_seg_det.SelectedIndex = -1;
            cmb_vtabar.SelectedIndex = -1;
            cmb_rvtas_sede.SelectedIndex = -1;
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            // segun la pestanha activa debe exportar
            string nombre = "";
            if (tabControl1.Enabled == false) return;
            if (tabControl1.SelectedTab == tabfacts && dgv_facts.Rows.Count > 0)
            {
                nombre = "Reportes_facturacion_" + cmb_sede_guias.Text.Trim() +"_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_facts.DataSource;
                    wb.Worksheets.Add(dt, "Ventas");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabnotas && dgv_notcre.Rows.Count > 0)
            {
                nombre = "Reportes_NotasCred_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_notcre.DataSource;
                    wb.Worksheets.Add(dt, "Notas");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabregvtas && dgv_regvtas.Rows.Count > 0)
            {
                nombre = "Registro_Ventas_" + dtp_ini_vtabar.Value.Date.ToString() + "-" + dtp_fin_vtabar.Value.Date.ToString() + "_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_regvtas.DataSource;
                    wb.Worksheets.Add(dt, "RepVtas");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabBarran && dgv_vtabar.Rows.Count > 0)
            {
                nombre = "Registro_de_Ventas_" + dtp_rvtas_fini.Value.Date.ToString() + "-" + dtp_rvtas_fter.Value.Date.ToString() + "_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_regvtas.DataSource;
                    wb.Worksheets.Add(dt, "RegVtas");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
        }
        #endregion

        #region crystal
        private void button2_Click(object sender, EventArgs e)      // resumen de contrato
        {
            setParaCrystal("resumen");
        }
        private void button4_Click(object sender, EventArgs e)      // reporte de ventas
        {
            //if (rb_listado.Checked == true) setParaCrystal("vtasxclte");
            //else setParaCrystal("ventas");
        }

        private void setParaCrystal(string repo)                    // genera el set para el reporte de crystal
        {
            if (repo== "resumen")
            {
                //conClie datos = generareporte();                        // conClie = dataset de impresion de contrato   
                //frmvizcont visualizador = new frmvizcont(datos);        // POR ESO SE CREO ESTE FORM frmvizcont PARA MOSTRAR AHI. ES MEJOR ASI.  
                //visualizador.Show();
            }
            if (repo == "ventas")
            {
                //conClie datos = generarepvtas();
                //frmvizcont visualizador = new frmvizcont(datos);
                //visualizador.Show();
            }
            if (repo == "vtasxclte")
            {
                //conClie datos = generarepvtasxclte();
                //frmvizoper visualizador = new frmvizoper(datos);
                //visualizador.Show();
            }
        }
        private conClie generareporte()
        {
            conClie rescont = new conClie();                                    // dataset
            /*
            conClie.rescont_cabRow rowcabeza = rescont.rescont_cab.Newrescont_cabRow();
            
            rowcabeza.id = "0";
            rowcabeza.contrato = tx_codped.Text;
            rowcabeza.doccli = tx_docu.Text;
            rowcabeza.nomcli = tx_cliente.Text.Trim();
            rowcabeza.estado = tx_estad.Text;
            rowcabeza.fecha = tx_fecha.Text;
            rowcabeza.tienda = tx_tiend.Text;
            rowcabeza.valor = tx_valor.Text;
            rowcabeza.fent = tx_fent.Text;
            rescont.rescont_cab.Addrescont_cabRow(rowcabeza);
            // detalle
            foreach(DataGridViewRow row in dgv_resumen.Rows)
            {
                if (row.Cells["codigo"].Value != null && row.Cells["codigo"].Value.ToString().Trim() != "")
                {
                    conClie.rescont_detRow rowdetalle = rescont.rescont_det.Newrescont_detRow();
                    rowdetalle.id = row.Cells["id"].Value.ToString();
                    rowdetalle.codigo = row.Cells["codigo"].Value.ToString();
                    rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                    rowdetalle.madera = row.Cells["madera"].Value.ToString();
                    rowdetalle.cantC = row.Cells["CanC"].Value.ToString();
                    rowdetalle.sep_id = row.Cells["sep_id"].Value.ToString();
                    rowdetalle.sep_fecha = row.Cells["sep_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.sep_almac = row.Cells["sep_almac"].Value.ToString();
                    rowdetalle.sep_cant = row.Cells["canS"].Value.ToString();
                    rowdetalle.ent_id = row.Cells["ent_id"].Value.ToString();
                    rowdetalle.ent_fecha = row.Cells["ent_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ent_cant = row.Cells["canE"].Value.ToString();
                    rowdetalle.tallerped = row.Cells["tallerped"].Value.ToString();
                    rowdetalle.ped_pedido = row.Cells["codped"].Value.ToString();
                    rowdetalle.ped_fecha = row.Cells["ped_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ped_cant = row.Cells["canP"].Value.ToString();
                    rowdetalle.ing_id = row.Cells["ing_id"].Value.ToString();
                    rowdetalle.ing_fecha = row.Cells["ing_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ing_cant = row.Cells["canI"].Value.ToString();
                    rowdetalle.sal_id = row.Cells["sal_id"].Value.ToString();
                    rowdetalle.sal_fecha = row.Cells["sal_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.sal_cant = row.Cells["canA"].Value.ToString();
                    rescont.rescont_det.Addrescont_detRow(rowdetalle);
                }
            }
            */
            return rescont;
        }
        #endregion

        #region leaves y enter
        private void tabvtas_Enter(object sender, EventArgs e)
        {
            //cmb_vtasloc.Focus();
        }
        private void tabres_Enter(object sender, EventArgs e)
        {
            //cmb_tidoc.Focus();
        }
        #endregion

        #region advancedatagridview
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)                    // ordenamiento
        {
            if (tabControl1.SelectedTab.Name == "tabfacts")
            {
                DataTable dtg = (DataTable)dgv_facts.DataSource;
                dtg.DefaultView.Sort = dgv_facts.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabnotas")
            {
                DataTable dtg = (DataTable)dgv_notcre.DataSource;
                dtg.DefaultView.Sort = dgv_notcre.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabregvtas")
            {
                DataTable dtg = (DataTable)dgv_regvtas.DataSource;
                dtg.DefaultView.Sort = dgv_regvtas.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabBarran")
            {
                DataTable dtg = (DataTable)dgv_vtabar.DataSource;
                dtg.DefaultView.RowFilter = dgv_vtabar.SortString;
            }
        }
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            if (tabControl1.SelectedTab.Name == "tabfacts")
            {
                DataTable dtg = (DataTable)dgv_facts.DataSource;
                dtg.DefaultView.RowFilter = dgv_facts.FilterString;
                suma_grilla("dgv_facts");
            }
            if (tabControl1.SelectedTab.Name == "tabnotas")
            {
                DataTable dtg = (DataTable)dgv_notcre.DataSource;
                dtg.DefaultView.RowFilter = dgv_notcre.FilterString;
                suma_grilla("dgv_notcre");
            }
            if (tabControl1.SelectedTab.Name == "tabregvtas")
            {
                DataTable dtg = (DataTable)dgv_regvtas.DataSource;
                dtg.DefaultView.RowFilter = dgv_regvtas.FilterString;
            }
            if (tabControl1.SelectedTab.Name == "tabBarran")
            {
                DataTable dtg = (DataTable)dgv_vtabar.DataSource;
                dtg.DefaultView.RowFilter = dgv_vtabar.FilterString;
            }
        }
        private void advancedDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)            // no usamos
        {
            //advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)      // no usamos
        {
            /*if(e.ColumnIndex == 1)
            {
                //string codu = "";
                string idr = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabreg;
                limpiar(this);
                limpia_otros();
                limpia_combos();
                tx_idr.Text = idr;
                jalaoc("tx_idr");
            }*/
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // no usamos
        {
            /*if (e.RowIndex > -1 && e.ColumnIndex > 0 
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString())
            {
                string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                string[] noeta = equivinter(advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString());    // retorna la tabla segun el titulo de la columna

                var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                    "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    if(advancedDataGridView1.Columns[e.ColumnIndex].Tag.ToString() == "validaSI")   // la columna se valida?
                    {
                        // valida si el dato ingresado es valido en la columna
                        if (lib.validac(noeta[0], noeta[1], e.FormattedValue.ToString()) == true)
                        {
                            // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                            lib.actuac(nomtab, campo, e.FormattedValue.ToString(),advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                        lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }*/
        }
        #endregion

    }
}
