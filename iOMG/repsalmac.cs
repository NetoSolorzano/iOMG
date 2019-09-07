using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class repsalmac : Form
    {
        static string nomform = "repsalmac";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "";         // 
        public int totfilgrid, cta;             // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string tipede = "";
        string tiesta = "";
        string img_btN = "";
        string img_btE = "";
        string img_btP = "";
        string img_btA = "";                                    // anula = bloquea
        string img_btexc = "";                                  // exporta a excel
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string img_imprime = "", img_preview = "";              // imagen del boton preview e imprimir reporte
        string letpied = "";                                    // letra indentificadora de piedra en detalle 2
        string cliente = Program.cliente;                       // razon social para los reportes
        libreria lib = new libreria();
        DataTable dtg = new DataTable();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public repsalmac()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1)
            {
                if (tx_nomclie.Focused == true && rb_listado.Checked == true) 
                {
                    para1 = "anag_cli";
                    para2 = "todos";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            tx_doclie.Text = ayu2.ReturnValue1;
                            tx_idclie.Text = ayu2.ReturnValue0;      // id del cliente
                            tx_nomclie.Text = ayu2.ReturnValue2;
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void repsalmac_Load(object sender, EventArgs e)
        {
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            // Set up the delays for the ToolTip.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            init();
            toolboton();
            dataload("todos");
            KeyPreview = true;
            tabControl1.Enabled = false;
        }
        private void init()
        {
            BackColor = Color.FromName(colback);
            toolStrip1.BackColor = Color.FromName(colstrp);
            tabControl1.BackColor = Color.FromName(iOMG.Program.colgri);

            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_anul.Image = Image.FromFile(img_btA);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            //bt_ingresos.Image = Image.FromFile(img_preview);
            //bt_salidas.Image = Image.FromFile(img_preview);
        }
        private void jalainfo()                                     // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "xxx");
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
                        if (row["param"].ToString() == "img_preview") img_preview = row["valor"].ToString().Trim();  // imagen del boton VISTA PRELIMINAR
                    }
                    if (row["formulario"].ToString() == "xxx")
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "almacen") tipede = row["valor"].ToString().Trim();         // tipo de pedido por defecto en almacen
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // estado del pedido inicial
                        if (row["campo"].ToString() == "detalle2" && row["param"].ToString() == "piedra") letpied = row["valor"].ToString().Trim();         // letra identificadora de Piedra en Detalle2
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
                // seleccion del local de ventas
                const string conlocven = "select descrizionerid,idcodice from desc_ven " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdlocven = new MySqlCommand(conlocven, conn);
                DataTable dtlocven = new DataTable();
                MySqlDataAdapter dalocven = new MySqlDataAdapter(cmdlocven);
                dalocven.Fill(dtlocven);
                foreach (DataRow row in dtlocven.Rows)
                {
                    cmb_vtasloc.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_vtasloc.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del almacen
                const string condest = "select descrizionerid,idcodice from desc_alm " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmddest = new MySqlCommand(condest, conn);
                DataTable dtdest = new DataTable();
                MySqlDataAdapter dadest = new MySqlDataAdapter(cmddest);
                dadest.Fill(dtdest);
                foreach (DataRow row in dtdest.Rows)
                {
                    cmb_destino.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_destino.ValueMember = row.ItemArray[1].ToString();
                    //
                    cmb_tienda.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_tienda.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del capitulo
                const string concap = "select descrizionerid,idcodice from desc_gru " +
                    "where numero=1";
                MySqlCommand cmdcap = new MySqlCommand(concap, conn);
                DataTable dtcap = new DataTable();
                MySqlDataAdapter dacap = new MySqlDataAdapter(cmdcap);
                dacap.Fill(dtcap);
                foreach (DataRow row in dtcap.Rows)
                {
                    cmb_fam.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());
                    cmb_fam.ValueMember = row.ItemArray[1].ToString();
                }
            }
            //
            conn.Close();
        }
        private void grilla()                                       // arma la grilla salidas
        {

        }
        private void grilla_rsv()                                               // arma la grilla de las reservas 
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_reserv.Font = tiplg;
            dgv_reserv.DefaultCellStyle.Font = tiplg;
            dgv_reserv.RowTemplate.Height = 15;
            dgv_reserv.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_reserv.AllowUserToAddRows = false;
        }
        private void grilla_sal()
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_salidas.Font = tiplg;
            dgv_salidas.DefaultCellStyle.Font = tiplg;
            dgv_salidas.RowTemplate.Height = 15;
            dgv_salidas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_salidas.AllowUserToAddRows = false;
        }
        private void grillares(string modo)                                     // arma la grilla del stock
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_resumen.Font = tiplg;
            dgv_resumen.DefaultCellStyle.Font = tiplg;
            dgv_resumen.RowTemplate.Height = 15;
            dgv_resumen.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_resumen.AllowUserToAddRows = false;
            if (dgv_resumen.DataSource == null) dgv_resumen.ColumnCount = 17;
            if (modo == "conval")
            {
                // id
                dgv_resumen.Columns[0].Visible = false;
                // codalm
                dgv_resumen.Columns[1].Visible = true;
                // codig
                dgv_resumen.Columns[2].Visible = true;
                // nombr
                dgv_resumen.Columns[3].Visible = true;
                // medid
                dgv_resumen.Columns[4].Visible = true;
                // precio
                dgv_resumen.Columns[5].Visible = true;
                // capit
                dgv_resumen.Columns[6].Visible = true;
                // model
                dgv_resumen.Columns[7].Visible = true;
                // mader
                dgv_resumen.Columns[8].Visible = true;
                // tipol
                dgv_resumen.Columns[9].Visible = true;
                // deta1
                dgv_resumen.Columns[10].Visible = true;
                // acaba
                dgv_resumen.Columns[11].Visible = true;
                // talle
                dgv_resumen.Columns[12].Visible = true;
                // deta2
                dgv_resumen.Columns[13].Visible = true;
                // deta3
                dgv_resumen.Columns[14].Visible = true;
                // juego
                dgv_resumen.Columns[15].Visible = true;
                // cant
                dgv_resumen.Columns[16].Visible = true;
            }
            if (modo == "sinval")
            {
                // id
                dgv_resumen.Columns[0].Visible = false;
                // codalm
                dgv_resumen.Columns[1].Visible = true;
                // codig
                dgv_resumen.Columns[2].Visible = true;
                // nombr
                dgv_resumen.Columns[3].Visible = true;
                // medid
                dgv_resumen.Columns[4].Visible = true;
                // precio
                dgv_resumen.Columns[5].Visible = false;
                // capit
                dgv_resumen.Columns[6].Visible = true;
                // model
                dgv_resumen.Columns[7].Visible = true;
                // mader
                dgv_resumen.Columns[8].Visible = true;
                // tipol
                dgv_resumen.Columns[9].Visible = true;
                // deta1
                dgv_resumen.Columns[10].Visible = true;
                // acaba
                dgv_resumen.Columns[11].Visible = true;
                // talle
                dgv_resumen.Columns[12].Visible = true;
                // deta2
                dgv_resumen.Columns[13].Visible = true;
                // deta3
                dgv_resumen.Columns[14].Visible = true;
                // juego
                dgv_resumen.Columns[15].Visible = true;
                // cant
                dgv_resumen.Columns[16].Visible = true;
            }
        }
        private void grillavtas()                                               // arma grilla de 
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_vtas.Font = tiplg;
            dgv_vtas.DefaultCellStyle.Font = tiplg;
            dgv_vtas.RowTemplate.Height = 15;
            dgv_vtas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_vtas.AllowUserToAddRows = false;
            if (dgv_vtas.DataSource == null) dgv_vtas.ColumnCount = 7;
        }
        //
        private void button1_Click(object sender, EventArgs e)                  // filtra y muestra las reservas
        {
            string consulta = "lisreserv";                                      // todos los ingresos de pedidos
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);    // solo estado anulado si se selecciona directamente
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_reserv.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@calm", tx_dat_almres.Text);
                    micon.Parameters.AddWithValue("@fini", dtp_resfini.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fina", dtp_resfinal.Value.ToString("yyyy-MM-dd"));
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_reserv.DataSource = dt;
                    dt.Dispose();
                    da.Dispose();
                    grilla_rsv();
                }
                else
                {
                    conn.Close();
                    MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener datos de contratos");
                Application.Exit();
                return;
            }
        }
        private void bt_filtra_sal_Click(object sender, EventArgs e)    // filtra y muestra las salidas (entregas)
        {
            string consulta = "salpedclte";                                 // todos los ingresos de pedidos
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);    // solo estado anulado si se selecciona directamente
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_salidas.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fini", dtp_fini_sal.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fina", dtp_final_sal.Value.ToString("yyyy-MM-dd"));
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_salidas.DataSource = dt;
                    dt.Dispose();
                    da.Dispose();
                    grilla_sal();
                }
                else
                {
                    conn.Close();
                    MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener datos de contratos");
                Application.Exit();
                return;
            }

        }
        private void bt_vtasfiltra_Click(object sender, EventArgs e)    // filtra y muestra ventas
        {
            string consulta = "";
            if (rb_listado.Checked == true && tx_nomclie.Text.Trim() != "")         // reporte de ventas por cliente
            {
                consulta = "repvtasxclt";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_vtas.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@idclte", tx_idclie.Text.Trim());
                        micon.Parameters.AddWithValue("@fecini", dtp_vtasfini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fecfin", dtp_vtasfina.Value.ToString("yyyy-MM-dd"));
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_vtas.DataSource = dt;
                        dt.Dispose();
                        da.Dispose();
                        grillavtas();
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
            else
            {
                consulta = "repventas";                          // CALL repventas('2019-07-01','2019-08-30','listado','');
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_vtas.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@fecini", dtp_vtasfini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fecfin", dtp_vtasfina.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@tienda", tx_dat_vtasloc.Text.Trim());
                        if (rb_listado.Checked == true) micon.Parameters.AddWithValue("@modo", "listado");
                        if (rb_resumen.Checked == true) micon.Parameters.AddWithValue("@modo", "resumen");
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_vtas.DataSource = dt;
                        dt.Dispose();
                        da.Dispose();
                        grillavtas();
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
        }
        private void bt_resumen_Click(object sender, EventArgs e)   // genera resumen de contrato
        {
            {
                string consulta = "rep_stock";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dtg.Clear();
                        dgv_resumen.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@calm", tx_dat_dest.Text);
                        micon.Parameters.AddWithValue("@ccap", cmb_fam.Text.Trim());
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        da.Fill(dtg);
                        dgv_resumen.DataSource = dtg;
                        da.Dispose();
                        if (chk_stkval.Checked == true) grillares("conval");
                        else grillares("sinval");
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
        }

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            //DataTable dtg = (DataTable)dgv_resumen.DataSource;
            dtg.DefaultView.RowFilter = dgv_resumen.FilterString;
        }
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            dtg.DefaultView.Sort = dgv_resumen.SortString;
        }
        private void advancedDataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            dgv_resumen.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = dgv_resumen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && Tx_modo.Text != "NUEVO")
            {
                // aca
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            // aca tampoco 
        }
        private void advancedDataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region combos
        private void cmb_vtasloc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_vtasloc.SelectedValue != null) tx_dat_vtasloc.Text = cmb_vtasloc.SelectedValue.ToString();
            else tx_dat_vtasloc.Text = cmb_vtasloc.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_vtasloc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_vtasloc.SelectedIndex = -1;
                tx_dat_vtasloc.Text = "";
            }
        }
        // 
        private void cmb_destino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_destino.SelectedValue != null) tx_dat_dest.Text = cmb_destino.SelectedValue.ToString();
            else tx_dat_dest.Text = cmb_destino.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_destino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_destino.SelectedIndex = -1;
                tx_dat_dest.Text = "";
            }
        }
        //
        private void cmb_fam_SelectionChangeCommitted(object sender, EventArgs e)       // capitulo familia
        {
            // de momento no hacemos nada
        }
        private void cmb_fam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_fam.SelectedIndex = -1;
                //tx_dat_dest.Text = "";
            }
        }
        //
        private void cmb_tienda_SelectionChangeCommitted(object sender, EventArgs e)    // reservas
        {
            if (cmb_tienda.SelectedValue != null) tx_dat_almres.Text = cmb_tienda.SelectedValue.ToString();
            else tx_dat_almres.Text = cmb_tienda.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_tienda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_tienda.SelectedIndex = -1;
                tx_dat_almres.Text = "";
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
            //cmb_tall_ing.Enabled = false;
            //cmb_estad_ing.Enabled = false;
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
            if (tabControl1.SelectedTab == tabres && dgv_reserv.Rows.Count > 0)
            {
                nombre = "Listado_reservas_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_reserv.DataSource;
                    wb.Worksheets.Add(dt, "Reservas");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabSal && dgv_salidas.Rows.Count > 0)
            {
                nombre = "Listado_salidas_pedidosclientes_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_salidas.DataSource;
                    wb.Worksheets.Add(dt, "Salidas_pedidos");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabstock && dgv_resumen.Rows.Count > 0)
            {
                nombre = "stock_" + cmb_destino.Text.Trim() +"_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_resumen.DataSource;
                    wb.Worksheets.Add(dt, "Resumen");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabvtas && dgv_vtas.Rows.Count > 0)
            {
                nombre = "Reportes_ventas_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_vtas.DataSource;
                    wb.Worksheets.Add(dt, "Ventas");
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
            setParaCrystal("stock");
        }
        private void button4_Click(object sender, EventArgs e)      // reporte de ventas
        {
            if (tx_idclie.Text.Trim() == "") setParaCrystal("ventas");
            if (tx_idclie.Text.Trim() != "" && tx_nomclie.Text.Trim() != "" && rb_listado.Checked == true) setParaCrystal("vtasxclte");
        }
        private void bt_ingresos_Click(object sender, EventArgs e)  // reportes de ingresos de pedidos
        {
            setParaCrystal("reservas");
        }
        private void bt_salidas_Click(object sender, EventArgs e)
        {
            setParaCrystal("salidas");
        }

        private void setParaCrystal(string repo)                    // genera el set para el reporte de crystal
        {
            if (repo== "stock")
            {
                repsalmacen datos = generareporte();                        // repsalmacen = dataset de los reportes de almacen
                frmvizalm visualizador = new frmvizalm(datos);              // FORM frmvizalm PARA MOSTRAR el crystal
                visualizador.Show();
            }
            if (repo == "reservas")     // ME QUEDE ACA 06/09/2019
            {
                //conClie datos = generarepvtasxclte();
                //frmvizcont visualizador = new frmvizcont(datos);
                //visualizador.Show();
            }
            if (repo == "ingresos")
            {
                //pedsclts datos = generarepingresos();
                //frmvizcpeds visualizador = new frmvizcpeds(datos);
                //visualizador.Show();
            }
            if (repo == "salidas")
            {
                //pedsclts datos = generarepsalidas();
                //frmvizcpeds visualizador = new frmvizcpeds(datos);
                //visualizador.Show();
            }
        }
        private conClie generarepvtasxclte()
        {
            conClie repvtas = new conClie();                        // xsd
            conClie.repvtas_cabRow cabrow = repvtas.repvtas_cab.Newrepvtas_cabRow();
            cabrow.id = "0";
            cabrow.fecini = dtp_vtasfini.Value.ToString("dd/MM/yyyy");
            cabrow.fecfin = dtp_vtasfina.Value.ToString("dd/MM/yyyy");
            cabrow.nudoclte = tx_doclie.Text.Trim();
            cabrow.nomclie = tx_nomclie.Text.Trim();
            if (rb_listado.Checked == true) cabrow.modo = "listado";
            repvtas.repvtas_cab.Addrepvtas_cabRow(cabrow);
            // detalle
            foreach (DataGridViewRow row in dgv_vtas.Rows)
            {
                if (rb_listado.Checked == true)                      // 
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";    //String.Format("{0:dd/MM/yyyy}",row.Cells["fecha"].Value.ToString()); //.ToString("dd/MM/yyyy"); ... ninguno de estos funciono
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        detrow.fecha = row.Cells["fecha"].Value.ToString().Substring(0,2) + "/" + row.Cells["fecha"].Value.ToString().Substring(3, 2) + "/" + row.Cells["fecha"].Value.ToString().Substring(6, 4); 
                        detrow.contrato = row.Cells["contratoh"].Value.ToString();
                        detrow.cant = row.Cells["cant"].Value.ToString().Trim();
                        detrow.codigo = row.Cells["item"].Value.ToString();
                        detrow.nombre = row.Cells["nombre"].Value.ToString().Trim();
                        detrow.medidas = row.Cells["medidas"].Value.ToString().Trim();
                        detrow.madera =  row.Cells["madera"].Value.ToString().Trim();
                        detrow.precio = row.Cells["precio"].Value.ToString().Trim();
                        detrow.total = double.Parse(row.Cells["total"].Value.ToString());
                        detrow.estcont = row.Cells["status"].Value.ToString().Trim();
                        if (row.Cells["fesal"].Value.ToString().Trim() == "") detrow.fecsal = row.Cells["fesal"].Value.ToString().Trim(); // salida de almacen con reserva
                        else detrow.fecsal = row.Cells["fesal"].Value.ToString().Substring(8, 2) + "/" + row.Cells["fesal"].Value.ToString().Substring(5, 2) + "/" + row.Cells["fesal"].Value.ToString().Substring(0, 4);
                        if (row.Cells["fecent"].Value.ToString().Trim() == "") detrow.fecent = row.Cells["fecent"].Value.ToString().Trim(); // fecha del pedido
                        else detrow.fecent = row.Cells["fecent"].Value.ToString().Substring(0,10);  //.Substring(8, 2) + "/" + row.Cells["fecent"].Value.ToString().Substring(5, 2) + "/" + row.Cells["fecent"].Value.ToString().Substring(0, 4);
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
            }
            return repvtas;
        }
        private repsalmacen generareporte()                         // reporte stock datos del formulario hacia dataset del reporte en crystal
        {
            repsalmacen rescont = new repsalmacen();                                    // dataset
            repsalmacen.cab_stockRow rowcabeza = rescont.cab_stock.Newcab_stockRow();
            rowcabeza.id = "0";
            rowcabeza.almacen = tx_dat_dest.Text;
            rowcabeza.capitulo = cmb_fam.Text.Trim();
            rowcabeza.fecha = DateTime.Now.Date.ToString();
            rowcabeza.tipologia = "";
            rowcabeza.valorizado = chk_stkval.CheckState.ToString();
            rescont.cab_stock.Addcab_stockRow(rowcabeza);
            // detalle
            foreach(DataGridViewRow row in dgv_resumen.Rows)
            {
                if (row.Cells["codig"].Value != null && row.Cells["codig"].Value.ToString().Trim() != "")
                {
                    repsalmacen.det_stockRow rowdetalle = rescont.det_stock.Newdet_stockRow();
                    rowdetalle.idc = "0";
                    rowdetalle.id = row.Cells["id"].Value.ToString();
                    rowdetalle.almacen = row.Cells["codalm"].Value.ToString();
                    rowdetalle.item = row.Cells["codig"].Value.ToString();
                    rowdetalle.nombre = row.Cells["nombr"].Value.ToString();
                    rowdetalle.medidas = row.Cells["medid"].Value.ToString();
                    rowdetalle.precio = row.Cells["precio"].Value.ToString();
                    rowdetalle.acabado = row.Cells["acaba"].Value.ToString();
                    rowdetalle.deta2 = row.Cells["deta2"].Value.ToString();
                    rowdetalle.madera = row.Cells["mader"].Value.ToString();
                    rowdetalle.cant = row.Cells["cant"].Value.ToString();
                    rescont.det_stock.Adddet_stockRow(rowdetalle);
                }
            }
            return rescont;
        }
        private pedsclts generarepingresos()
        {   // a.idmovim,a.fechain,tipo,a.pedido,a.origen,a.destino,a.cant,a.articulo,nomad,med1,tipoes,madera,cliente,nomitem
            pedsclts pedset = new pedsclts();
            pedsclts.cab_repingRow rowcab = pedset.cab_reping.Newcab_repingRow();
            rowcab.id = "0";
            rowcab.fini = dtp_fini_sal.Value.ToString().Substring(0, 10);
            rowcab.fina = dtp_final_sal.Value.ToString().Substring(0, 10);
            pedset.cab_reping.Addcab_repingRow(rowcab);
            //
            foreach(DataGridViewRow row in dgv_reserv.Rows)
            {
                if (row.Cells["pedido"].Value != null && row.Cells["pedido"].Value.ToString().Trim() != "")
                {
                    pedsclts.det_repingRow rowdet = pedset.det_reping.Newdet_repingRow();
                    rowdet.id = "0";
                    rowdet.fecha = row.Cells["fechain"].Value.ToString().Substring(0, 10);
                    rowdet.tipo = row.Cells["tipo"].Value.ToString();
                    rowdet.pedido = row.Cells["pedido"].Value.ToString();
                    rowdet.origen = row.Cells["origen"].Value.ToString();
                    rowdet.destino = row.Cells["destino"].Value.ToString();
                    rowdet.cant = row.Cells["cant"].Value.ToString();
                    rowdet.articulo = row.Cells["articulo"].Value.ToString();
                    rowdet.nonmad = row.Cells["nomad"].Value.ToString();
                    rowdet.medidas = row.Cells["med1"].Value.ToString();
                    rowdet.tipoes = row.Cells["tipoes"].Value.ToString();
                    rowdet.madera = row.Cells["madera"].Value.ToString();
                    rowdet.cliente = row.Cells["cliente"].Value.ToString();
                    rowdet.nomitem = row.Cells["nomitem"].Value.ToString();
                    pedset.det_reping.Adddet_repingRow(rowdet);
                }
            }
            return pedset;
        }
        private pedsclts generarepsalidas()                         // salidas de pedidos de clientes (entregas)
        {
            //a.fecha,a.tipo,a.pedido,cliente,a.uantes,a.uactual,a.coment,b.item,b.cant,b.medidas,b.madera
            pedsclts pedset = new pedsclts();
            pedsclts.cab_repsalRow rowcab = pedset.cab_repsal.Newcab_repsalRow();
            rowcab.id = "0";
            rowcab.fini = dtp_fini_sal.Value.ToString().Substring(0, 10);
            rowcab.fina = dtp_final_sal.Value.ToString().Substring(0, 10);
            pedset.cab_repsal.Addcab_repsalRow(rowcab);
            //
            foreach(DataGridViewRow row in dgv_salidas.Rows)
            {
                if (row.Cells["tipo"].Value != null && row.Cells["tipo"].Value.ToString().Trim() != "")
                {
                    pedsclts.det_repsalRow rowdet = pedset.det_repsal.Newdet_repsalRow();
                    rowdet.id = "0";
                    rowdet.fecha = row.Cells["fecha"].Value.ToString().Substring(0, 10);
                    rowdet.tipo = row.Cells["tipo"].Value.ToString();
                    rowdet.pedido = row.Cells["pedido"].Value.ToString();
                    rowdet.cliente = row.Cells["cliente"].Value.ToString();
                    rowdet.uantes = row.Cells["uantes"].Value.ToString();
                    rowdet.uactual = row.Cells["uactual"].Value.ToString();
                    rowdet.coment = row.Cells["coment"].Value.ToString();
                    rowdet.item = row.Cells["item"].Value.ToString();
                    rowdet.cant = row.Cells["cant"].Value.ToString();
                    rowdet.medidas = row.Cells["medidas"].Value.ToString();
                    rowdet.madera = row.Cells["madera"].Value.ToString();
                    pedset.det_repsal.Adddet_repsalRow(rowdet);
                }
            }
            return pedset;
        }
        #endregion
    }
}
