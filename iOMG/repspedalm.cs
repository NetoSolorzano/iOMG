using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class repspedalm : Form
    {
        static string nomform = "repspedalm";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "pedidos";         // 
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
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public repspedalm()
        {
            InitializeComponent();
        }

        private void repspedalm_Load(object sender, EventArgs e)
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
            //limpiar(this);
            dataload("todos");
            //grilla();
            KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = true;
            tabControl1.Enabled = false;
            // desactivamos los botones sin uso
            Bt_add.Enabled = false;
            Bt_edit.Enabled = false;
            Bt_anul.Enabled = false;
            // invisibilizamos los botones de desplazamiento
            Bt_ini.Visible = false;
            Bt_sig.Visible = false;
            Bt_ret.Visible = false;
            Bt_fin.Visible = false;
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
        }
        private void jalainfo()                 // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "pedidos");
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
                        //if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                    }
                    if (row["formulario"].ToString() == "pedidos")
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "almacen") tipede = row["valor"].ToString().Trim();         // tipo de pedido por defecto en almacen
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // estado del pedido inicial
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
        public void dataload(string quien)                  // jala datos para los combos y la grilla
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
                // seleccion de taller de produccion ... ok
                const string contaller = "select descrizionerid,idcodice,codigo from desc_loc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                DataTable dttaller = new DataTable();
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6) + " - " + row.ItemArray[0].ToString());
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del almacen de destino ... 
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
                }
                // seleccion del estado
                const string conestado = "select descrizionerid,idcodice from desc_stp " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdestado = new MySqlCommand(conestado, conn);
                DataTable dtestado = new DataTable();
                MySqlDataAdapter daestado = new MySqlDataAdapter(cmdestado);
                daestado.Fill(dtestado);
                foreach (DataRow row in dtestado.Rows)
                {
                    cmb_estado.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_estado.ValueMember = row.ItemArray[1].ToString();
                }
            }
            //
            conn.Close();
        }
        private void grilla()                   // arma la grilla
        {
            // a.fecha,a.codped,b.descrizione,c.descrizione,a.destino,a.entrega," +
            //d.item,d.nombre,d.madera,d.piedra,d.medidas,d.cant,d.saldo,a.status,a.origen
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_pedidos.Font = tiplg;
            dgv_pedidos.DefaultCellStyle.Font = tiplg;
            dgv_pedidos.RowTemplate.Height = 15;
            dgv_pedidos.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_pedidos.AllowUserToAddRows = false;
            if (dgv_pedidos.DataSource == null) dgv_pedidos.ColumnCount = 15;
            //dgv_ped.DataSource = dtg;
            // Fecha pedido
            dgv_pedidos.Columns[0].Visible = true;
            dgv_pedidos.Columns[0].HeaderText = "Fecha";    // titulo de la columna
            dgv_pedidos.Columns[0].Width = 70;                // ancho
            dgv_pedidos.Columns[0].ReadOnly = true;           // lectura o no
            dgv_pedidos.Columns[0].Tag = "validaNO";
            //dgv_pedidos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Pedido
            dgv_pedidos.Columns[1].Visible = true;            // columna visible o no
            dgv_pedidos.Columns[1].HeaderText = "Pedido";    // titulo de la columna
            dgv_pedidos.Columns[1].Width = 60;                // ancho
            dgv_pedidos.Columns[1].ReadOnly = true;           // lectura o no
            dgv_pedidos.Columns[1].Tag = "validaNO";
            //dgv_pedidos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Estado
            dgv_pedidos.Columns[2].Visible = true;
            dgv_pedidos.Columns[2].HeaderText = "Estado";    // titulo de la columna
            dgv_pedidos.Columns[2].Width = 80;                // ancho
            dgv_pedidos.Columns[2].ReadOnly = true;           // lectura o no
            dgv_pedidos.Columns[2].Tag = "validaNO";
            //dgv_pedidos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // taller
            dgv_pedidos.Columns[3].Visible = true;
            dgv_pedidos.Columns[3].HeaderText = "Taller";
            dgv_pedidos.Columns[3].Width = 100;
            dgv_pedidos.Columns[3].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_pedidos.Columns[3].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            //dgv_pedidos.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Destino
            dgv_pedidos.Columns[4].Visible = true;
            dgv_pedidos.Columns[4].HeaderText = "Destino";
            dgv_pedidos.Columns[4].Width = 70;
            dgv_pedidos.Columns[4].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_pedidos.Columns[4].Tag = "validaNO";          // las celdas de esta columna se validan
            //dgv_pedidos.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fech entrega
            dgv_pedidos.Columns[5].Visible = true;
            dgv_pedidos.Columns[5].HeaderText = "Fecha Ent";
            dgv_pedidos.Columns[5].Width = 70;
            dgv_pedidos.Columns[5].ReadOnly = true;
            dgv_pedidos.Columns[5].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            //dgv_pedidos.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // items
            dgv_pedidos.Columns[6].Visible = true;
            dgv_pedidos.Columns[6].HeaderText = "Código";
            dgv_pedidos.Columns[6].Width = 100;
            dgv_pedidos.Columns[6].ReadOnly = true;
            dgv_pedidos.Columns[6].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Nombre
            dgv_pedidos.Columns[7].Visible = true;
            dgv_pedidos.Columns[7].HeaderText = "Nombre del artículo";
            dgv_pedidos.Columns[7].Width = 200;
            dgv_pedidos.Columns[7].ReadOnly = true;
            dgv_pedidos.Columns[7].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // madera
            dgv_pedidos.Columns[8].Visible = true;
            dgv_pedidos.Columns[8].HeaderText = "Madera";
            dgv_pedidos.Columns[8].Width = 30;
            dgv_pedidos.Columns[8].ReadOnly = true;
            dgv_pedidos.Columns[8].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // detalle 2
            dgv_pedidos.Columns[9].Visible = true;
            dgv_pedidos.Columns[9].HeaderText = "Det.2";
            dgv_pedidos.Columns[9].Width = 60;
            dgv_pedidos.Columns[9].ReadOnly = true;
            dgv_pedidos.Columns[9].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // medidas
            dgv_pedidos.Columns[10].Visible = true;
            dgv_pedidos.Columns[10].HeaderText = "Medidas";
            dgv_pedidos.Columns[10].Width = 100;
            dgv_pedidos.Columns[10].ReadOnly = true;
            dgv_pedidos.Columns[10].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Cant
            dgv_pedidos.Columns[11].Visible = true;
            dgv_pedidos.Columns[11].HeaderText = "Cant";
            dgv_pedidos.Columns[11].Width = 50;
            dgv_pedidos.Columns[11].ReadOnly = true;
            dgv_pedidos.Columns[11].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Saldo
            dgv_pedidos.Columns[12].Visible = true;
            dgv_pedidos.Columns[12].HeaderText = "Saldo";
            dgv_pedidos.Columns[12].Width = 50;
            dgv_pedidos.Columns[12].ReadOnly = true;
            dgv_pedidos.Columns[12].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // resto de campos
            dgv_pedidos.Columns[13].Visible = false;
            dgv_pedidos.Columns[14].Visible = false;
        }

        #region combos
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_estado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estado.SelectedValue != null) tx_dat_estad.Text = cmb_estado.SelectedValue.ToString();
            else tx_dat_estad.Text = cmb_estado.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_destino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_destino.SelectedValue != null) tx_dat_dest.Text = cmb_destino.SelectedValue.ToString();
            else tx_dat_dest.Text = cmb_destino.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        // 
        private void cmb_estado_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                cmb_estado.SelectedIndex = -1;
                tx_dat_estad.Text = "";
            }
        }

        #endregion

        #region botones de comando
        public void toolboton()
        {
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
                if (Convert.ToString(row["btn1"]) == "S")
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                //if (Convert.ToString(row["btn5"]) == "S")
                //{
                //    this.Bt_print.Visible = true;
                //}
                //else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                //if (Convert.ToString(row["btn4"]) == "S")
                //{
                //    this.Bt_ver.Visible = true;
                //}
                //else { this.Bt_ver.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
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
            /*
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            */
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Reporte_Pedidos_almacen_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                DataTable dt = (DataTable)dgv_pedidos.DataSource;
                wb.Worksheets.Add(dt, "Reporte_Pedidos");
                wb.SaveAs(nombre);
                MessageBox.Show("Archivo generado con exito!");
                this.Close();
            }
         }
        #endregion

        private void button1_Click(object sender, EventArgs e)      // filtra y muestra la info
        {
            // id,codped,tipoes,origen,destino,fecha,entrega,coment
            string parte = "where a.tipoes=@tip and a.fecha between @fec1 and @fec2";
            string parte0 = "";
            if(tx_dat_orig.Text != "")          // taller
            {
                parte0 = " and  a.origen=@tal";
            }
            string consulta = "select a.fecha,a.codped,b.descrizione,c.descrizione,a.destino,a.entrega," +
                "d.item,d.nombre,d.madera,d.piedra,d.medidas,d.cant,d.saldo,a.status,a.origen " +
                "from pedidos a left join detaped d on d.pedidoh=a.codped " +
                "left join desc_stp b on b.idcodice=a.status " +
                "left join desc_loc c on c.idcodice=a.origen " +
                parte + parte0; // d.coment, a.coment,
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if(conn.State == ConnectionState.Open)
                {
                    dgv_pedidos.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@tip", tipede);
                    micon.Parameters.AddWithValue("@fec1", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fec2", dtp_entreg.Value.ToString("yyyy-MM-dd"));
                    if(parte0 != "") micon.Parameters.AddWithValue("@tal", tx_dat_orig.Text);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_pedidos.DataSource = dt;
                    dt.Dispose();
                    da.Dispose();
                    grilla();
                }
                else
                {
                    conn.Close();
                    MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                conn.Close();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener datos");
                Application.Exit();
                return;
            }
        }

    }
}
