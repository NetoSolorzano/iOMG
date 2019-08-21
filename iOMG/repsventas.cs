using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class repsventas : Form
    {
        static string nomform = "repsventas";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "contrat";         // 
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
        string img_imprime = "", img_preview = "";        // imagen del boton preview e imprimir reporte
        string letpied = "";            // letra indentificadora de piedra en detalle 2
        string cliente = Program.cliente;    // razon social para los reportes
        int pageCount = 1, cuenta = 0;
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public repsventas()
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
        private void repsventas_Load(object sender, EventArgs e)
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
            tabControl1.Enabled = false;
            //
            tx_codped.CharacterCasing = CharacterCasing.Upper;
            tx_codped.TextAlign = HorizontalAlignment.Center;
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
            bt_imprime.Image = Image.FromFile(img_imprime);
            bt_preview.Image = Image.FromFile(img_preview);
            bt_imp_ing.Image = Image.FromFile(img_imprime);
            bt_preview_ing.Image = Image.FromFile(img_preview);
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
                    //
                    cmb_tall_ing.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6) + " - " + row.ItemArray[0].ToString());
                    cmb_tall_ing.ValueMember = row.ItemArray[1].ToString();
                    //
                    cmb_pedtaller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6) + " - " + row.ItemArray[0].ToString());
                    cmb_pedtaller.ValueMember = row.ItemArray[1].ToString();
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
                    //
                    cmb_dest_ing.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_dest_ing.ValueMember = row.ItemArray[1].ToString();
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
                    //
                    cmb_estad_ing.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_estad_ing.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del estado del contrato
                const string conestcont = "select descrizionerid,idcodice from desc_sta " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdestcont = new MySqlCommand(conestcont, conn);
                DataTable dtestcont = new DataTable();
                MySqlDataAdapter daestcont = new MySqlDataAdapter(cmdestcont);
                daestcont.Fill(dtestcont);
                foreach (DataRow row in dtestcont.Rows)
                {
                    cmb_conestado.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_conestado.ValueMember = row.ItemArray[1].ToString();
                }
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
            }
            //
            conn.Close();
        }
        private void grilla()                                       // arma la grilla salidas
        {

        }
        private void grillacont()                                   // arma grilla contratos
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_contratos.Font = tiplg;
            dgv_contratos.DefaultCellStyle.Font = tiplg;
            dgv_contratos.RowTemplate.Height = 15;
            dgv_contratos.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_contratos.AllowUserToAddRows = false;
            if (dgv_contratos.DataSource == null) dgv_contratos.ColumnCount = 8;
        }
        private void grillapeds()                                   // arma grilla de pedidos
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_pedidos.Font = tiplg;
            dgv_pedidos.DefaultCellStyle.Font = tiplg;
            dgv_pedidos.RowTemplate.Height = 15;
            dgv_pedidos.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_pedidos.AllowUserToAddRows = false;
            if (dgv_pedidos.DataSource == null) dgv_pedidos.ColumnCount = 16;
        }
        private void grilla_ing()                                   // arma la grilla ingresos
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_salidas.Font = tiplg;
            dgv_salidas.DefaultCellStyle.Font = tiplg;
            dgv_salidas.RowTemplate.Height = 15;
            dgv_salidas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_salidas.AllowUserToAddRows = false;
            if (dgv_salidas.DataSource == null) dgv_salidas.ColumnCount = 8;
            // Fecha ingreso real
            dgv_salidas.Columns[0].Visible = true;
            dgv_salidas.Columns[0].HeaderText = "Fecha";    // titulo de la columna
            dgv_salidas.Columns[0].Width = 70;                // ancho
            dgv_salidas.Columns[0].ReadOnly = false;           // lectura o no
            dgv_salidas.Columns[0].Tag = "validaNO";
            //dgv_pedidos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // almacen ingresado
            dgv_salidas.Columns[1].Visible = true;            // columna visible o no
            dgv_salidas.Columns[1].HeaderText = "Almacén";    // titulo de la columna
            dgv_salidas.Columns[1].Width = 60;                // ancho
            dgv_salidas.Columns[1].ReadOnly = true;           // lectura o no
            dgv_salidas.Columns[1].Tag = "validaSI";
            //dgv_pedidos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // codigo pedido
            dgv_salidas.Columns[2].Visible = true;
            dgv_salidas.Columns[2].HeaderText = "Pedido";    // titulo de la columna
            dgv_salidas.Columns[2].Width = 80;                // ancho
            dgv_salidas.Columns[2].ReadOnly = true;           // lectura o no
            dgv_salidas.Columns[2].Tag = "validaNO";
            //dgv_pedidos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // codigo articulo
            dgv_salidas.Columns[3].Visible = true;
            dgv_salidas.Columns[3].HeaderText = "Artículo";
            dgv_salidas.Columns[3].Width = 150;
            dgv_salidas.Columns[3].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_salidas.Columns[3].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            //dgv_pedidos.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Nombre
            dgv_salidas.Columns[4].Visible = true;
            dgv_salidas.Columns[4].HeaderText = "Nombre";
            dgv_salidas.Columns[4].Width = 200;
            dgv_salidas.Columns[4].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_salidas.Columns[4].Tag = "validaNO";          // las celdas de esta columna se validan
            //dgv_pedidos.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Cant
            dgv_salidas.Columns[5].Visible = true;
            dgv_salidas.Columns[5].HeaderText = "Cant";
            dgv_salidas.Columns[5].Width = 50;
            dgv_salidas.Columns[5].ReadOnly = true;
            dgv_salidas.Columns[5].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            //dgv_pedidos.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // madera
            dgv_salidas.Columns[6].Visible = true;
            dgv_salidas.Columns[6].HeaderText = "Madera";
            dgv_salidas.Columns[6].Width = 70;
            dgv_salidas.Columns[6].ReadOnly = true;
            dgv_salidas.Columns[6].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // comentario
            dgv_salidas.Columns[7].Visible = true;
            dgv_salidas.Columns[7].HeaderText = "Comentario";
            dgv_salidas.Columns[7].Width = 200;
            dgv_salidas.Columns[7].ReadOnly = true;
            dgv_salidas.Columns[7].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_pedidos.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // resto
            dgv_salidas.Columns[8].Visible = false;
            dgv_salidas.Columns[9].Visible = false;
        }
        private void grillares()                                    // arma la grilla del resumen de contrato
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_resumen.Font = tiplg;
            dgv_resumen.DefaultCellStyle.Font = tiplg;
            dgv_resumen.RowTemplate.Height = 15;
            dgv_resumen.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_resumen.AllowUserToAddRows = false;
            if (dgv_resumen.DataSource == null) dgv_resumen.ColumnCount = 22;
            // id,codigo,nombre,madera,CanC,sep_id,sep_fecha,sep_almac,canS,ent_id,ent_fecha,canE,
            // codped,ped_fecha,canP,ing_id,ing_fecha,canI,sal_id,sal_fecha,canA
            // id
            dgv_resumen.Columns[0].Visible = false;
            // codigo
            dgv_resumen.Columns[1].Visible = true;
            dgv_resumen.Columns[1].HeaderText = "Código";    // titulo de la columna
            dgv_resumen.Columns[1].Width = 140;                // ancho
            dgv_resumen.Columns[1].ReadOnly = true;           // lectura o no
            dgv_resumen.Columns[1].Tag = "validaNO";
            dgv_resumen.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre
            dgv_resumen.Columns[2].Visible = true;            // columna visible o no
            dgv_resumen.Columns[2].HeaderText = "Nombre";    // titulo de la columna
            dgv_resumen.Columns[2].Width = 200;                // ancho
            dgv_resumen.Columns[2].ReadOnly = true;           // lectura o no
            dgv_resumen.Columns[2].Tag = "validaNO";
            dgv_resumen.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // madera
            dgv_resumen.Columns[3].Visible = true;
            dgv_resumen.Columns[3].HeaderText = "Madera";    // titulo de la columna
            dgv_resumen.Columns[3].Width = 60;                // ancho
            dgv_resumen.Columns[3].ReadOnly = true;           // lectura o no
            dgv_resumen.Columns[3].Tag = "validaNO";
            //dgv_resumen.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // cantidad contrato
            dgv_resumen.Columns[4].Visible = true;
            dgv_resumen.Columns[4].HeaderText = "Cant";
            dgv_resumen.Columns[4].Width = 50;
            dgv_resumen.Columns[4].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_resumen.Columns[4].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            //dgv_resumen.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // SEPARACION - Id
            dgv_resumen.Columns[5].Visible = true;
            dgv_resumen.Columns[5].HeaderText = "RESERVA Id";
            dgv_resumen.Columns[5].Width = 50;
            dgv_resumen.Columns[5].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            dgv_resumen.Columns[5].Tag = "validaNO";          // las celdas de esta columna se validan
            //dgv_resumen.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // SEPARACION - Fecha
            dgv_resumen.Columns[6].Visible = true;
            dgv_resumen.Columns[6].HeaderText = "Fecha";
            dgv_resumen.Columns[6].Width = 70;
            dgv_resumen.Columns[6].ReadOnly = true;
            dgv_resumen.Columns[6].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            //dgv_resumen.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // SEPARACION - Almacen
            dgv_resumen.Columns[7].Visible = true;
            dgv_resumen.Columns[7].HeaderText = "Almacen";
            dgv_resumen.Columns[7].Width = 70;
            dgv_resumen.Columns[7].ReadOnly = true;
            dgv_resumen.Columns[7].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // SEPARACION - Cant
            dgv_resumen.Columns[8].Visible = true;
            dgv_resumen.Columns[8].HeaderText = "Cant";
            dgv_resumen.Columns[8].Width = 50;
            dgv_resumen.Columns[8].ReadOnly = true;
            dgv_resumen.Columns[8].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // ENTREGAS - Id
            dgv_resumen.Columns[9].Visible = true;
            dgv_resumen.Columns[9].HeaderText = "ENTREGA Id";
            dgv_resumen.Columns[9].Width = 50;
            dgv_resumen.Columns[9].ReadOnly = true;
            dgv_resumen.Columns[9].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // ENTREGAS - Fecha
            dgv_resumen.Columns[10].Visible = true;
            dgv_resumen.Columns[10].HeaderText = "Fecha";
            dgv_resumen.Columns[10].Width = 70;
            dgv_resumen.Columns[10].ReadOnly = true;
            dgv_resumen.Columns[10].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // ENTREGAS - Cant
            dgv_resumen.Columns[11].Visible = true;
            dgv_resumen.Columns[11].HeaderText = "Cant";
            dgv_resumen.Columns[11].Width = 50;
            dgv_resumen.Columns[11].ReadOnly = true;
            dgv_resumen.Columns[11].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // PEDIDOS - Pedido
            dgv_resumen.Columns[12].Visible = true;
            dgv_resumen.Columns[12].HeaderText = "PEDIDOS Pedido";
            dgv_resumen.Columns[12].Width = 70;
            dgv_resumen.Columns[12].ReadOnly = true;
            dgv_resumen.Columns[12].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // PEDIDOS - Fecha
            dgv_resumen.Columns[13].Visible = true;
            dgv_resumen.Columns[13].HeaderText = "Fecha";
            dgv_resumen.Columns[13].Width = 70;
            dgv_resumen.Columns[13].ReadOnly = true;
            dgv_resumen.Columns[13].Tag = "validaNO";          // las celdas de esta columna SI se validan
            //dgv_resumen.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // PEDIDOS - Cant
            dgv_resumen.Columns[14].Visible = true;
            dgv_resumen.Columns[14].HeaderText = "Cant";
            dgv_resumen.Columns[14].Width = 50;
            dgv_resumen.Columns[14].ReadOnly = true;
            dgv_resumen.Columns[14].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // INGRESOS - Id4
            dgv_resumen.Columns[15].Visible = true;
            dgv_resumen.Columns[15].HeaderText = "INGRESO Id";
            dgv_resumen.Columns[15].Width = 50;
            dgv_resumen.Columns[15].ReadOnly = true;
            dgv_resumen.Columns[15].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // INGRESOS - Fecha
            dgv_resumen.Columns[16].Visible = true;
            dgv_resumen.Columns[16].HeaderText = "Fecha";
            dgv_resumen.Columns[16].Width = 70;
            dgv_resumen.Columns[16].ReadOnly = true;
            dgv_resumen.Columns[16].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // INGRESOS - Cant
            dgv_resumen.Columns[17].Visible = true;
            dgv_resumen.Columns[17].HeaderText = "Cant";
            dgv_resumen.Columns[17].Width = 50;
            dgv_resumen.Columns[17].ReadOnly = true;
            dgv_resumen.Columns[17].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // ENTREGAS - Id
            dgv_resumen.Columns[18].Visible = true;
            dgv_resumen.Columns[18].HeaderText = "SALIDAS Id";
            dgv_resumen.Columns[18].Width = 50;
            dgv_resumen.Columns[18].ReadOnly = true;
            dgv_resumen.Columns[18].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // ENTREGAS - Fecha
            dgv_resumen.Columns[19].Visible = true;
            dgv_resumen.Columns[19].HeaderText = "Fecha";
            dgv_resumen.Columns[19].Width = 70;
            dgv_resumen.Columns[19].ReadOnly = true;
            dgv_resumen.Columns[19].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // ENTREGAS - Cant
            dgv_resumen.Columns[20].Visible = true;
            dgv_resumen.Columns[20].HeaderText = "Cant";
            dgv_resumen.Columns[20].Width = 50;
            dgv_resumen.Columns[20].ReadOnly = true;
            dgv_resumen.Columns[20].Tag = "validaNO";          // las celdas de esta columna SI se validan
            // PEDIDOS - taller
            dgv_resumen.Columns[21].Visible = true;
            dgv_resumen.Columns[21].HeaderText = "Taller";
            dgv_resumen.Columns[21].Width = 50;
            dgv_resumen.Columns[21].ReadOnly = true;
            dgv_resumen.Columns[21].Tag = "validaNO";          // las celdas de esta columna SI se validan
            dgv_resumen.Columns[21].DisplayIndex = 12;
        }
        private void grillavtas()                                   // arma grilla de ventas
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_vtas.Font = tiplg;
            dgv_vtas.DefaultCellStyle.Font = tiplg;
            dgv_vtas.RowTemplate.Height = 15;
            dgv_vtas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_vtas.AllowUserToAddRows = false;
            if (dgv_vtas.DataSource == null) dgv_vtas.ColumnCount = 7;
        }
        private void button1_Click(object sender, EventArgs e)          // filtra y muestra la info - 
        {
            /*
            string parte = "where a.tipoes=@tip and a.fecha between @fec1 and @fec2";
            string parte0 = "", parte1 = "", parte2 = "";
            if (tx_dat_orig.Text != "")          // taller
            {
                parte0 = " and a.origen=@tal";
            }
            if (tx_dat_dest.Text != "")
            {
                parte1 = " and a.destino=@des";
            }
            if (tx_dat_estad.Text != "")
            {
                parte2 = " and a.status=@sta";
            }
            string consulta = "";
            if (chk_resu.Checked == true)
            {
                consulta = "select a.fecha,a.codped,b.descrizione,c.descrizione,a.destino,a.entrega," +
                    "space(1) as item,space(1) as nombre,space(1) as madera, '' as piedra,'' as medidas,sum(d.cant) as cant,sum(d.saldo) as saldo," +
                    "space(1) as acabado,a.status,trim(a.origen),'' as estado,'' as cmadera,'' as cpiedra,d.fingreso,a.coment " +
                    "from pedidos a left join detaped d on d.pedidoh=a.codped " +
                    "left join desc_stp b on b.idcodice=a.status " +
                    "left join desc_loc c on trim(c.idcodice)=trim(a.origen) " +
                    "left join desc_est e on e.idcodice=d.estado " +
                    parte + parte0 + parte1 + parte2 + " group by a.codped order by a.fecha,a.origen,a.codped";
            }
            else
            {
                consulta = "select a.fecha,a.codped,b.descrizione,c.descrizione,a.destino,a.entrega," +
                    "d.item,d.nombre,f.descrizionerid,g.descrizionerid,d.medidas,d.cant,d.saldo,e.descrizionerid," +
                    "a.status,trim(a.origen),d.estado,d.madera,d.piedra,d.fingreso,'' as coment " +
                    "from pedidos a left join detaped d on d.pedidoh=a.codped " +
                    "left join desc_stp b on b.idcodice=a.status " +
                    "left join desc_loc c on trim(c.idcodice)=trim(a.origen) " +
                    "left join desc_est e on e.idcodice=d.estado " +
                    "left join desc_mad f on f.idcodice=d.madera " +
                    "left join desc_dt2 g on g.idcodice=d.piedra " +
                    parte + parte0 + parte1 + parte2 + " order by a.fecha,a.origen,a.codped"; // d.coment, a.coment,
            }
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_pedidos.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@tip", tipede);
                    micon.Parameters.AddWithValue("@fec1", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fec2", dtp_entreg.Value.ToString("yyyy-MM-dd"));
                    if (parte0 != "") micon.Parameters.AddWithValue("@tal", tx_dat_orig.Text);
                    if (parte1 != "") micon.Parameters.AddWithValue("@des", tx_dat_dest.Text);
                    if (parte2 != "") micon.Parameters.AddWithValue("@sta", tx_dat_estad.Text);
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
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener datos");
                Application.Exit();
                return;
            }
            */
        }
        private void bt_filtra_ing_Click(object sender, EventArgs e)    // filtra y muestra info - 
        {
            /*
            string parte = "where a.tipmov = 'INGRES' and a.fecha between @fec1 and @fec2";
            string parte0 = "", parte1 = "", parte2 = "";
            if (tx_dat_taling.Text != "")
            {
                parte0 = " and a.origen=@tal";
            }
            if (tx_dat_desing.Text != "")
            {
                parte1 = " and a.almad=@des";
            }
            if (tx_dat_esting.Text != "")
            {
                parte2 = " and b.status=@sta";
            }
            string consulta = "SELECT a.fecha,b.descrizionerid,a.docum,a.item,i.nombr,a.cant,a.madera,a.coment,a.almad,'' " +
                "FROM movalm a " +
                "LEFT JOIN items i ON concat(i.capit, left(i.model, 3), i.mader, i.tipol, left(i.deta1, 2), i.acaba, i.talle, i.deta2) = " +
                "concat(SUBSTRING(a.item, 1, 1), SUBSTRING(a.item, 2, 3), 'X', SUBSTRING(a.item, 6, 2), SUBSTRING(a.item, 8, 2), SUBSTRING(a.item, 10, 1), 'XX', SUBSTRING(a.item, 13, 3)) " +
                "LEFT JOIN desc_alm b ON b.IDCodice = a.almad " +
                parte + parte0 + parte1 + parte2 + " order by a.fecha,a.docum";
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_ingresos.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@fec1", dtp_fini_ing.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fec2", dtp_final_ing.Value.ToString("yyyy-MM-dd"));
                    if (parte0 != "") micon.Parameters.AddWithValue("@tal", tx_dat_taling.Text);
                    if (parte1 != "") micon.Parameters.AddWithValue("@des", tx_dat_desing.Text);
                    if (parte2 != "") micon.Parameters.AddWithValue("@sta", tx_dat_esting.Text);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dti = new DataTable();
                    da.Fill(dti);
                    dgv_ingresos.DataSource = dti;
                    dti.Dispose();
                    da.Dispose();
                    grilla_ing();
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
            */
        }
        private void bt_confiltra_Click(object sender, EventArgs e)     // flltra y muestra contratos
        {
            string consulta = "repliscont";
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_contratos.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fini", dtp_confini.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fina", dtp_confina.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@estado", tx_dat_conestado.Text.Trim());
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_contratos.DataSource = dt;
                    dt.Dispose();
                    da.Dispose();
                    grillacont();
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
        private void button5_Click(object sender, EventArgs e)          // filtra y muestra pedidos de clientes
        {
            string consulta = "lispedclt";
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dgv_pedidos.DataSource = null;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fecini", dtp_pedfini.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_pedfina.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@taller", tx_dat_pedtaller.Text.Trim());
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_pedidos.DataSource = dt;
                    dt.Dispose();
                    da.Dispose();
                    grillapeds();
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
        private void tx_codped_Leave(object sender, EventArgs e)    // valida existencia de contrato
        {
            if(tx_codped.Text != "")
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string consu = "select a.id,a.fecha,a.tipoes,a.cliente,a.valor,a.status,b.ruc,b.razonsocial,a.entrega " +
                            "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                            "where a.contrato=@ped";
                        MySqlCommand micon = new MySqlCommand(consu, conn);
                        micon.Parameters.AddWithValue("@ped", tx_codped.Text);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            if(dr[0] == null)
                            {
                                MessageBox.Show("No existe el contrato!", "Atención verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                tx_codped.Text = "";
                                tx_cliente.Text = "";
                                tx_docu.Text = "";
                                tx_fecha.Text = "";
                                tx_valor.Text = "";
                                tx_tiend.Text = "";
                                tx_estad.Text = "";
                                tx_fent.Text = "";
                                tx_codped.Focus();
                                dr.Close();
                                conn.Close();
                                return;
                            }
                            else
                            {
                                tx_cliente.Text = dr.GetString(7);
                                tx_docu.Text = dr.GetString(6);
                                tx_fecha.Text = dr.GetString(1).Substring(0,10);
                                tx_valor.Text = dr.GetString(4);
                                tx_tiend.Text = dr.GetString(2);
                                tx_estad.Text = dr.GetString(5);
                                tx_fent.Text = dr.GetString(8).Substring(0,10);
                                dr.Close();
                            }
                        }
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
        }
        private void bt_resumen_Click(object sender, EventArgs e)   // genera resumen de contrato
        {
            if(tx_codped.Text != "")
            {
                string consulta = "res_cont";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_resumen.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@cont", tx_codped.Text);
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_resumen.DataSource = dt;
                        dt.Dispose();
                        da.Dispose();
                        grillares();
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
        private void cmb_tall_ing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tall_ing.SelectedValue != null) tx_dat_taling.Text = cmb_tall_ing.SelectedValue.ToString();
            else tx_dat_taling.Text = cmb_tall_ing.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_estad_ing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estad_ing.SelectedValue != null) tx_dat_esting.Text = cmb_estad_ing.SelectedValue.ToString();
            else tx_dat_esting.Text = cmb_estad_ing.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_dest_ing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_dest_ing.SelectedValue != null) tx_dat_desing.Text = cmb_dest_ing.SelectedValue.ToString();
            else tx_dat_desing.Text = cmb_dest_ing.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_pedtaller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_pedtaller.SelectedValue != null) tx_dat_pedtaller.Text = cmb_pedtaller.SelectedValue.ToString();
            else tx_dat_pedtaller.Text = cmb_pedtaller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_conestado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_conestado.SelectedValue != null) tx_dat_conestado.Text = cmb_conestado.SelectedValue.ToString();
            else tx_dat_conestado.Text = cmb_conestado.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_vtasloc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_vtasloc.SelectedValue != null) tx_dat_vtasloc.Text = cmb_vtasloc.SelectedValue.ToString();
            else tx_dat_vtasloc.Text = cmb_vtasloc.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
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
        private void cmb_taller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_taller.SelectedIndex = -1;
                tx_dat_orig.Text = "";
            }
        }
        private void cmb_destino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_destino.SelectedIndex = -1;
                tx_dat_dest.Text = "";
            }
        }
        private void cmb_tall_ing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_tall_ing.SelectedIndex = -1;
                tx_dat_taling.Text = "";
            }
        }
        private void cmb_estad_ing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad_ing.SelectedIndex = -1;
                tx_dat_esting.Text = "";
            }
        }
        private void cmb_dest_ing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_dest_ing.SelectedIndex = -1;
                tx_dat_desing.Text = "";
            }
        }
        private void cmb_pedtaller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_pedtaller.SelectedIndex = -1;
                tx_dat_pedtaller.Text = "";
            }
        }
        private void cmb_conestado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_conestado.SelectedIndex = -1;
                tx_dat_conestado.Text = "";
            }
        }
        private void cmb_vtasloc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_vtasloc.SelectedIndex = -1;
                tx_dat_vtasloc.Text = "";
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
            cmb_tall_ing.Enabled = false;
            cmb_estad_ing.Enabled = false;
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            // segun la pestanha activa debe exportar
            string nombre = "";
            if (tabControl1.SelectedTab == tabres)
            {
                nombre = "Resumen_Contrato_" + tx_codped.Text.Trim() + "_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_ingresos.DataSource;
                    wb.Worksheets.Add(dt, "Resumen_Contrato");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if(tabControl1.SelectedTab == tabIng)
            {
                nombre = "Reporte_Ingresos_almacen_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_salidas.DataSource;
                    wb.Worksheets.Add(dt, "Reporte_Ingresos");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
        }
        #endregion

        #region impresion
        // pedidos
        private void bt_imprime_Click(object sender, EventArgs e)   // imprime el reporte
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.Document = printDocument1;
            printDlg.AllowSomePages = true;
            printDlg.AllowSelection = true;
            //
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            if (printDlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
        }
        private void bt_preview_Click(object sender, EventArgs e)
        {
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if(tabControl1.SelectedTab == tabPed)
            {
                // +++++++++++++++++++ VARIABLES DE POSICIONAMIENTO GENERAL ++++++++++++++++++ //
                float pix = 50.0F;      // punto inicial X
                float piy = 30.0F;      // punto inicial Y
                float alfi = 10.0F;     // alto de cada fila
                float alin = 45.0F;     // alto inicial
                float posi = 160.0F;     // posición de impresión
                float coli = 30.0F;     // columna mas a la izquierda
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ //
                if(chk_resu.Checked == false)
                {
                    imprime(pix, piy, cliente, coli, alin, posi, alfi, e);
                }
                else
                {
                    impresum(pix, piy, cliente, coli, alin, posi, alfi, e);
                }

            }
            if(tabControl1.SelectedTab == tabIng)
            {
                // +++++++++++++++++++ VARIABLES DE POSICIONAMIENTO GENERAL ++++++++++++++++++ //
                float pix = 50.0F;      // punto inicial X
                float piy = 30.0F;      // punto inicial Y
                float alfi = 10.0F;     // alto de cada fila
                float alin = 45.0F;     // alto inicial
                float posi = 160.0F;     // posición de impresión
                float coli = 30.0F;     // columna mas a la izquierda
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ //
                impri_ing(pix, piy, cliente, coli, alin, posi, alfi, e);
            }
        }
        private void impresum(float pix, float piy, string cliente, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // columnas del reporte
            float col0 = coli;              // Fecha
            float col1 = coli + 70.0F;      // Llegada
            float col2 = coli + 150.0F;     // Pedido
            float col3 = coli + 210.0F;     // Estado
            float col4 = coli + 300.0F;     // taller
            float col5 = coli + 400.0F;     // destino - almacen
            float col6 = coli + 500.0F;     // cant
            float col7 = coli + 550.0F;     // saldo
            float col8 = coli + 600.0F;     // ult fecha ingreso
            float col9 = coli + 680.0F;     // comentario
            //
            //float col6 = coli + 700.0F;     // Madera
            //float col7 = coli + 760.0F;     // Detalle2
            //float co12 = coli + 1060.0F;    // fecha ingreso
            //
            float posit = impcabres(piy, coli, alin, posi, alfi, e,
                col0, col1, col2, col3, col4, col5, col6, col7, col8, col9);    // , col6, col7, co10, co11, co12
            posi = posit;
            SizeF espnom = new SizeF(250.0F, alfi);         // recuadro para el nombre y comentario
            Font lt_tit = new Font("Arial", 7);
            Font lt_quie = new Font("Arial", 8, FontStyle.Bold);
            PointF ptoimp;
            Pen blackPen = new Pen(Color.Black, 1);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;
            // leemos las columnas del data table
            //string quiebre = "";
            for (int fila = cuenta; fila < dgv_ingresos.Rows.Count; fila++)
            {
                /*
                if (dgv_pedidos.Rows[fila].Cells[15].Value.ToString() != quiebre)
                {
                    quiebre = dgv_pedidos.Rows[fila].Cells[15].Value.ToString();
                    ptoimp = new PointF(col0, posi);
                    e.Graphics.DrawString(quiebre + " - " + dgv_pedidos.Rows[fila].Cells[3].Value.ToString(), lt_quie, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                    posi = posi + alfi + 5;             // avance de fila
                }
                */
                string data0 = (fila + 1).ToString("###");
                string dataI = dgv_ingresos.Rows[fila].Cells[0].Value.ToString().Substring(0, 10);    // Fecha
                string data1 = dgv_ingresos.Rows[fila].Cells[5].Value.ToString().Substring(0, 10);    // Llegada
                string data2 = dgv_ingresos.Rows[fila].Cells[1].Value.ToString();    // Pedido
                string data3 = dgv_ingresos.Rows[fila].Cells[2].Value.ToString();    // Estado
                string data4 = dgv_ingresos.Rows[fila].Cells[3].Value.ToString();    // taller
                string data5 = dgv_ingresos.Rows[fila].Cells[4].Value.ToString();    // destino
                string data6 = dgv_ingresos.Rows[fila].Cells[11].Value.ToString();   // cant
                string data7 = dgv_ingresos.Rows[fila].Cells[12].Value.ToString();    // saldo
                string data8 = dgv_ingresos.Rows[fila].Cells[19].Value.ToString().PadRight(10).Substring(0, 10);    // ult fecha ingresa
                string data9 = dgv_ingresos.Rows[fila].Cells[20].Value.ToString();    // comentarios

                /*
                string data6 = dgv_pedidos.Rows[fila].Cells[8].Value.ToString();    // Madera
                string data7 = "";
                if (data4.Substring(12, 1) == letpied) data7 = dgv_pedidos.Rows[fila].Cells[9].Value.ToString();    // Detalle 2
                string data12 = dgv_pedidos.Rows[fila].Cells[19].Value.ToString().PadRight(10).Substring(0, 10);    // fecha de ingreso
                */
                ptoimp = new PointF(col0, posi);
                e.Graphics.DrawString(dataI, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col1, posi);
                e.Graphics.DrawString(data1, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col2, posi);
                e.Graphics.DrawString(data2, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col3, posi);
                RectangleF recn = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data3, lt_tit, Brushes.Black, recn, sf);
                ptoimp = new PointF(col4, posi);
                RectangleF recco = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data4, lt_tit, Brushes.Black, ptoimp, sf);
                ptoimp = new PointF(col5, posi);
                Size siznom = new Size(200, 15);
                RectangleF recnom = new RectangleF(ptoimp, siznom);
                e.Graphics.DrawString(data5, lt_tit, Brushes.Black, recnom, StringFormat.GenericTypographic);
                ptoimp = new PointF(col6, posi);
                e.Graphics.DrawString(data6, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col7, posi);
                e.Graphics.DrawString(data7, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col8, posi);
                e.Graphics.DrawString(data8, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col9, posi);
                e.Graphics.DrawString(data9, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                /*
                e.Graphics.DrawString(data6, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col7, posi);
                e.Graphics.DrawString(data7, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co10 + 10.0F, posi);
                e.Graphics.DrawString(data10, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co11 + 10.0F, posi);
                e.Graphics.DrawString(data11, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co12, posi);
                e.Graphics.DrawString(data12, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                */
                //
                posi = posi + alfi + 5;             // avance de fila
                e.Graphics.DrawLine(blackPen, coli - 1, posi, e.PageSettings.Bounds.Width - 20.0F, posi);
                posi = posi + alfi - 5;             // avance de fila
                cuenta = cuenta + 1;
                if (posi >= e.PageBounds.Height - 20.0F)
                {
                    pageCount = pageCount + 1;
                    e.HasMorePages = true;
                    return;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            posi = posi + alfi * 2;             // avance de fila
            cuenta = 0;
        }
        private void imprime(float pix, float piy, string cliente, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // columnas del reporte
            float col0 = coli;              // Fecha
            float col1 = coli + 70.0F;      // Llegada
            float col2 = coli + 150.0F;     // Pedido
            float col3 = coli + 210.0F;     // Estado
            float col4 = coli + 310.0F;     // Articulo
            float col5 = coli + 480.0F;     // Nombre
            float col6 = coli + 700.0F;     // Madera
            float col7 = coli + 760.0F;     // Detalle2
            float col8 = coli + 820.0F;     // Acabado
            float col9 = coli + 920.0F;     // medidas
            float co10 = coli + 1000.0F;    // cant
            float co11 = coli + 1030.0F;    // saldo
            float co12 = coli + 1060.0F;    // fecha ingreso
            //
            float posit = impcab2(piy, coli, alin, posi, alfi, e,
                col0, col1, col2, col3, col4, col5, col6, col7, col8, col9, co10, co11, co12);
            posi = posit;
            SizeF espnom = new SizeF(250.0F, alfi);         // recuadro para el nombre y comentario
            Font lt_tit = new Font("Arial", 7);
            Font lt_quie = new Font("Arial", 8, FontStyle.Bold);
            PointF ptoimp;
            Pen blackPen = new Pen(Color.Black, 1);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;
            // leemos las columnas del data table
            string quiebre = "";
            for (int fila = cuenta; fila < dgv_ingresos.Rows.Count; fila++)
            {
                if(dgv_ingresos.Rows[fila].Cells[15].Value.ToString() != quiebre)
                {
                    quiebre = dgv_ingresos.Rows[fila].Cells[15].Value.ToString();
                    ptoimp = new PointF(col0, posi);
                    e.Graphics.DrawString(quiebre + " - " + dgv_ingresos.Rows[fila].Cells[3].Value.ToString(), lt_quie, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                    posi = posi + alfi + 5;             // avance de fila
                }
                string data0 = (fila + 1).ToString("###");                          
                string dataI = dgv_ingresos.Rows[fila].Cells[0].Value.ToString().Substring(0, 10);    // Fecha
                string data1 = dgv_ingresos.Rows[fila].Cells[5].Value.ToString().Substring(0, 10);    // Llegada
                string data2 = dgv_ingresos.Rows[fila].Cells[1].Value.ToString();    // Pedido
                string data3 = dgv_ingresos.Rows[fila].Cells[2].Value.ToString();    // Estado
                string data4 = dgv_ingresos.Rows[fila].Cells[6].Value.ToString();    // Articulo
                string data5 = dgv_ingresos.Rows[fila].Cells[7].Value.ToString();    // Nombre
                string data6 = dgv_ingresos.Rows[fila].Cells[8].Value.ToString();    // Madera
                string data7 = "";
                if (data4.Substring(12,1) == letpied) data7 = dgv_ingresos.Rows[fila].Cells[9].Value.ToString();    // Detalle 2
                string data8 = dgv_ingresos.Rows[fila].Cells[13].Value.ToString();    // acabado
                string data9 = dgv_ingresos.Rows[fila].Cells[10].Value.ToString();    // medidas
                string data10 = dgv_ingresos.Rows[fila].Cells[11].Value.ToString();   // cant
                string data11 = dgv_ingresos.Rows[fila].Cells[12].Value.ToString();    // saldo
                string data12 = dgv_ingresos.Rows[fila].Cells[19].Value.ToString().PadRight(10).Substring(0, 10);    // fecha de ingreso
                //
                ptoimp = new PointF(col0, posi);
                e.Graphics.DrawString(dataI, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col1, posi);
                e.Graphics.DrawString(data1, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col2, posi);
                e.Graphics.DrawString(data2, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col3, posi);
                RectangleF recn = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data3, lt_tit, Brushes.Black, recn, sf);
                ptoimp = new PointF(col4, posi);
                RectangleF recco = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data4, lt_tit, Brushes.Black, ptoimp, sf);
                ptoimp = new PointF(col5, posi);
                Size siznom = new Size(200, 15);
                RectangleF recnom = new RectangleF(ptoimp, siznom);
                e.Graphics.DrawString(data5, lt_tit, Brushes.Black, recnom, StringFormat.GenericTypographic);
                ptoimp = new PointF(col6, posi);
                e.Graphics.DrawString(data6, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col7, posi);
                e.Graphics.DrawString(data7, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col8, posi);
                e.Graphics.DrawString(data8, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col9, posi);
                e.Graphics.DrawString(data9, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co10 + 10.0F, posi);
                e.Graphics.DrawString(data10, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co11 + 10.0F, posi);
                e.Graphics.DrawString(data11, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(co12, posi);
                e.Graphics.DrawString(data12, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                //
                posi = posi + alfi + 5;             // avance de fila
                e.Graphics.DrawLine(blackPen, coli - 1, posi, e.PageSettings.Bounds.Width - 20.0F, posi);
                posi = posi + alfi - 5;             // avance de fila
                cuenta = cuenta + 1;
                if (posi >= e.PageBounds.Height - 20.0F)
                {
                    pageCount = pageCount + 1;
                    e.HasMorePages = true;
                    return;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            posi = posi + alfi * 2;             // avance de fila
            cuenta = 0;
        }
        private float impcab2(float piy, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e,
            float col0, float col1, float col2, float col3, float col4, float col5, float col6, float col7, float col8, float col9, float co10, float co11, float co12)
        {
            float ancho_pag = printDocument1.DefaultPageSettings.Bounds.Width;  // ancho de la pag.
            float colm = coli + 280.0F;                                 // columna media
            float cold = coli + 530.0F;                                 // columna derecha
            Font lt_cliente = new Font("Arial", 15, FontStyle.Bold);
            Font lt_pag = new Font("Arial", 9);
            Font lt_fec = new Font("Arial", 7, FontStyle.Bold);
            Font lt_tit = new Font("Arial", 11);                        // tipo de letra del titulo
            Pen grueso = new Pen(Color.Black, 2);                       // linea gruesa
            Pen delgado = new Pen(Color.Black, 1);                      // linea delgada
            StringFormat sf = new StringFormat();                       // formato centrado
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            // logo
            e.Graphics.DrawImage(Image.FromFile("recursos/logo_artesanos_omg_peru.jpeg"), 30, 20, 200, 150);
            // pagina y fecha
            SizeF anctit = new SizeF();
            anctit = e.Graphics.MeasureString(cliente, lt_cliente);
            PointF ptocli = new PointF((ancho_pag - anctit.Width) / 2, piy);
            e.Graphics.DrawString(cliente, lt_cliente, Brushes.Black, ptocli, StringFormat.GenericTypographic);
            // pintamos contador de pág.
            PointF ptopag = new PointF(ancho_pag - 80.0F, piy);
            string pag = "Pág. " + pageCount.ToString();
            e.Graphics.DrawString(pag, lt_pag, Brushes.Black, ptopag, StringFormat.GenericTypographic);
            // pintamos la fecha
            PointF ptofec = new PointF(ancho_pag - 80.0F, piy + 15.0F);
            string fecha = DateTime.Today.ToShortDateString();
            e.Graphics.DrawString(fecha, lt_fec, Brushes.Black, ptofec, StringFormat.GenericTypographic);
            // titulo y filtros
            SizeF anctyf = new SizeF();
            anctyf = e.Graphics.MeasureString(this.Text, lt_cliente);
            PointF ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 30.0F);
            e.Graphics.DrawString(this.Text, lt_cliente, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            string ddd = "Del " + dtp_pedido.Value.ToString("dd/MM/yyyy") + " Al " + dtp_entreg.Value.ToString("dd/MM/yyyy");
            anctyf = e.Graphics.MeasureString(ddd, lt_tit);
            ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 60.0F);
            e.Graphics.DrawString(ddd,lt_tit, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            // titulo de las columnas
            //a.fecha,a.codped,b.descrizione,c.descrizione,a.destino,a.entrega,
            //d.item,d.nombre,d.madera,d.piedra,d.medidas,d.cant,d.saldo,a.status,a.origen
            posi = posi + alfi;
            PointF ptoimp = new PointF(col0, posi);
            e.Graphics.DrawString("Fecha", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col1, posi);
            e.Graphics.DrawString("Llegada", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString("Pedido", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col3, posi);
            e.Graphics.DrawString("Estado", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col4, posi);
            e.Graphics.DrawString("Articulo", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col5, posi);
            e.Graphics.DrawString("Nombre", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col6, posi);
            e.Graphics.DrawString("Mad.", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col7, posi);
            e.Graphics.DrawString("Det.2", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col8, posi);
            e.Graphics.DrawString("Acabado", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col9, posi);
            e.Graphics.DrawString("Medidas", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(co10, posi);
            e.Graphics.DrawString("Cant", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(co11, posi);
            e.Graphics.DrawString("Saldo", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(co12, posi);
            e.Graphics.DrawString("F.Ingreso", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi + 7.0F;             // avance de fila
            e.Graphics.DrawLine(delgado, coli, posi, ancho_pag - 20.0F, posi);
            posi = posi + 2;             // avance de fila
            //
            return posi;
        }
        private float impcabres(float piy, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e,
            float col0, float col1, float col2, float col3, float col4, float col5, float col6, float col7, float col8, float col9)
        {
            float ancho_pag = printDocument1.DefaultPageSettings.Bounds.Width;  // ancho de la pag.
            float colm = coli + 280.0F;                                 // columna media
            float cold = coli + 530.0F;                                 // columna derecha
            Font lt_cliente = new Font("Arial", 15, FontStyle.Bold);
            Font lt_pag = new Font("Arial", 9);
            Font lt_fec = new Font("Arial", 7, FontStyle.Bold);
            Font lt_tit = new Font("Arial", 11);                        // tipo de letra del titulo
            Pen grueso = new Pen(Color.Black, 2);                       // linea gruesa
            Pen delgado = new Pen(Color.Black, 1);                      // linea delgada
            StringFormat sf = new StringFormat();                       // formato centrado
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            // logo
            e.Graphics.DrawImage(Image.FromFile("recursos/logo_artesanos_omg_peru.jpeg"), 30, 20, 200, 150);
            // pagina y fecha
            SizeF anctit = new SizeF();
            anctit = e.Graphics.MeasureString(cliente, lt_cliente);
            PointF ptocli = new PointF((ancho_pag - anctit.Width) / 2, piy);
            e.Graphics.DrawString(cliente, lt_cliente, Brushes.Black, ptocli, StringFormat.GenericTypographic);
            // pintamos contador de pág.
            PointF ptopag = new PointF(ancho_pag - 80.0F, piy);
            string pag = "Pág. " + pageCount.ToString();
            e.Graphics.DrawString(pag, lt_pag, Brushes.Black, ptopag, StringFormat.GenericTypographic);
            // pintamos la fecha
            PointF ptofec = new PointF(ancho_pag - 80.0F, piy + 15.0F);
            string fecha = DateTime.Today.ToShortDateString();
            e.Graphics.DrawString(fecha, lt_fec, Brushes.Black, ptofec, StringFormat.GenericTypographic);
            // titulo y filtros
            SizeF anctyf = new SizeF();
            anctyf = e.Graphics.MeasureString(this.Text, lt_cliente);
            PointF ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 30.0F);
            e.Graphics.DrawString(this.Text, lt_cliente, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            string ddd = "Del " + dtp_pedido.Value.ToString("dd/MM/yyyy") + " Al " + dtp_entreg.Value.ToString("dd/MM/yyyy");
            anctyf = e.Graphics.MeasureString(ddd, lt_tit);
            ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 60.0F);
            e.Graphics.DrawString(ddd, lt_tit, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            // titulo de las columnas
            posi = posi + alfi;
            PointF ptoimp = new PointF(col0, posi);
            e.Graphics.DrawString("Fecha", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col1, posi);
            e.Graphics.DrawString("Llegada", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString("Pedido", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col3, posi);
            e.Graphics.DrawString("Estado", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col4, posi);
            e.Graphics.DrawString("Taller", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col5, posi);
            e.Graphics.DrawString("", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col6, posi);
            e.Graphics.DrawString("Destino", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col7, posi);
            e.Graphics.DrawString("Saldo", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col8, posi);
            e.Graphics.DrawString("F.Ingreso", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col9, posi);
            e.Graphics.DrawString("Comentarios", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi + 7.0F;             // avance de fila
            e.Graphics.DrawLine(delgado, coli, posi, ancho_pag - 20.0F, posi);
            posi = posi + 2;             // avance de fila
            //
            return posi;
        }
        // ingresos
        private void bt_imp_ing_Click(object sender, EventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();

            printDlg.Document = printDocument1; //printDoc;
            printDlg.AllowSomePages = true;
            printDlg.AllowSelection = true;
            //
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            //
            if (printDlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
        }
        private void bt_preview_ing_Click(object sender, EventArgs e)
        {
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private void impri_ing(float pix, float piy, string cliente, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // a.fecha,c.descrizionerid,a.docum,a.item,'nombreArt',a.cant,madera,a.coment,a.almad,b.status
            // columnas del reporte
            float col0 = coli;              // it contador filas
            float col1 = coli + 50.0F;      // Fecha
            float col2 = coli + 150.0F;     // almacen
            float col3 = coli + 250.0F;     // pedido
            float col4 = coli + 310.0F;     // Articulo
            float col5 = coli + 480.0F;     // Nombre
            float col6 = coli + 700.0F;     // cant
            float col7 = coli + 750.0F;     // madera
            float col8 = coli + 800.0F;     // coment
            //
            float posit = impcab_ing(piy, coli, alin, posi, alfi, e,
                col0, col1, col2, col3, col4, col5, col6, col7, col8);
            posi = posit;
            SizeF espnom = new SizeF(250.0F, alfi);         // recuadro para el nombre y comentario
            Font lt_tit = new Font("Arial", 7);
            Font lt_quie = new Font("Arial", 8, FontStyle.Bold);
            PointF ptoimp;
            Pen blackPen = new Pen(Color.Black, 1);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;
            // leemos las columnas del data table
            for (int fila = cuenta; fila < dgv_salidas.Rows.Count; fila++)
            {
                string data0 = (fila + 1).ToString("###");                                             // contador
                string dataI = dgv_salidas.Rows[fila].Cells[0].Value.ToString().Substring(0, 10);     // Fecha
                string data1 = dgv_salidas.Rows[fila].Cells[1].Value.ToString().Substring(0, 10);     // almacen
                string data2 = dgv_salidas.Rows[fila].Cells[2].Value.ToString();                      // Pedido
                string data3 = dgv_salidas.Rows[fila].Cells[3].Value.ToString();                      // articulo
                string data4 = dgv_salidas.Rows[fila].Cells[4].Value.ToString();                      // nombre
                string data5 = dgv_salidas.Rows[fila].Cells[5].Value.ToString();                      // cant
                string data6 = dgv_salidas.Rows[fila].Cells[6].Value.ToString();                      // madera
                string data7 = dgv_salidas.Rows[fila].Cells[7].Value.ToString();                      // coment
                // a.fecha,c.descrizionerid,a.docum,a.item,'nombreArt',a.cant,madera,a.coment,a.almad,b.status
                ptoimp = new PointF(col0, posi);
                e.Graphics.DrawString(data0, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col1, posi);
                e.Graphics.DrawString(dataI, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col2, posi);
                e.Graphics.DrawString(data1, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col3, posi);
                RectangleF recn = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data2, lt_tit, Brushes.Black, recn, sf);
                ptoimp = new PointF(col4, posi);
                RectangleF recco = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data3, lt_tit, Brushes.Black, ptoimp, sf);
                ptoimp = new PointF(col5, posi);
                Size siznom = new Size(200, 15);
                RectangleF recnom = new RectangleF(ptoimp, siznom);
                e.Graphics.DrawString(data4, lt_tit, Brushes.Black, recnom, StringFormat.GenericTypographic);
                ptoimp = new PointF(col6, posi);
                e.Graphics.DrawString(data5, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col7, posi);
                e.Graphics.DrawString(data6, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col8, posi);
                e.Graphics.DrawString(data7, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                //
                posi = posi + alfi + 5;             // avance de fila
                e.Graphics.DrawLine(blackPen, coli - 1, posi, e.PageSettings.Bounds.Width - 20.0F, posi);
                posi = posi + alfi - 5;             // avance de fila
                cuenta = cuenta + 1;
                if (posi >= e.PageBounds.Height - 20.0F)
                {
                    pageCount = pageCount + 1;
                    e.HasMorePages = true;
                    return;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            posi = posi + alfi * 2;             // avance de fila
            cuenta = 0;
        }
        private float impcab_ing(float piy, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e,
            float col0, float col1, float col2, float col3, float col4, float col5, float col6, float col7, float col8)
        {
            float ancho_pag = printDocument1.DefaultPageSettings.Bounds.Width;  // ancho de la pag.
            float colm = coli + 280.0F;                                 // columna media
            float cold = coli + 530.0F;                                 // columna derecha
            Font lt_cliente = new Font("Arial", 15, FontStyle.Bold);
            Font lt_pag = new Font("Arial", 9);
            Font lt_fec = new Font("Arial", 7, FontStyle.Bold);
            Font lt_tit = new Font("Arial", 11);                        // tipo de letra del titulo
            Pen grueso = new Pen(Color.Black, 2);                       // linea gruesa
            Pen delgado = new Pen(Color.Black, 1);                      // linea delgada
            StringFormat sf = new StringFormat();                       // formato centrado
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            // logo
            e.Graphics.DrawImage(Image.FromFile("recursos/logo_artesanos_omg_peru.jpeg"), 30, 20, 200, 150);
            // pagina y fecha
            SizeF anctit = new SizeF();
            anctit = e.Graphics.MeasureString(cliente, lt_cliente);
            PointF ptocli = new PointF((ancho_pag - anctit.Width) / 2, piy);
            e.Graphics.DrawString(cliente, lt_cliente, Brushes.Black, ptocli, StringFormat.GenericTypographic);
            // pintamos contador de pág.
            PointF ptopag = new PointF(ancho_pag - 80.0F, piy);
            string pag = "Pág. " + pageCount.ToString();
            e.Graphics.DrawString(pag, lt_pag, Brushes.Black, ptopag, StringFormat.GenericTypographic);
            // pintamos la fecha
            PointF ptofec = new PointF(ancho_pag - 80.0F, piy + 15.0F);
            string fecha = DateTime.Today.ToShortDateString();
            e.Graphics.DrawString(fecha, lt_fec, Brushes.Black, ptofec, StringFormat.GenericTypographic);
            // titulo y filtros
            SizeF anctyf = new SizeF();
            anctyf = e.Graphics.MeasureString("INGRESOS DE ALMACEN", lt_cliente);
            PointF ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 30.0F);
            e.Graphics.DrawString("INGRESOS DE ALMACEN", lt_cliente, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            string ddd = "Del " + dtp_fini_ing.Value.ToString("dd/MM/yyyy") + " Al " + dtp_final_ing.Value.ToString("dd/MM/yyyy");
            anctyf = e.Graphics.MeasureString(ddd, lt_tit);
            ptotit = new PointF((ancho_pag - anctyf.Width) / 2, piy + 60.0F);
            e.Graphics.DrawString(ddd, lt_tit, Brushes.Black, ptotit, StringFormat.GenericTypographic);
            // titulo de las columnas
            // a.fecha,c.descrizionerid,a.docum,a.item,'nombreArt',a.cant,'saldo',a.coment,a.almad,b.status
            posi = posi + alfi;
            PointF ptoimp = new PointF(col0, posi);
            e.Graphics.DrawString("It", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col1, posi);
            e.Graphics.DrawString("F.Ingreso", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString("Almacén", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col3, posi);
            e.Graphics.DrawString("Pedido", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col4, posi);
            e.Graphics.DrawString("Articulo", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col5, posi);
            e.Graphics.DrawString("Nombre", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col6, posi);
            e.Graphics.DrawString("Cant", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col7, posi);
            e.Graphics.DrawString("Madera", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col8, posi);
            e.Graphics.DrawString("Comentario", lt_fec, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi + 7.0F;             // avance de fila
            e.Graphics.DrawLine(delgado, coli, posi, ancho_pag - 20.0F, posi);
            posi = posi + 2;             // avance de fila
            //
            return posi;
        }
        // salidas
        #endregion

        #region crystal
        private void bt_pedidos_Click(object sender, EventArgs e)
        {
            setParaCrystal("pedidos");
        }
        private void button2_Click(object sender, EventArgs e)      // resumen de contrato
        {
            setParaCrystal("resumen");
        }
        private void button3_Click(object sender, EventArgs e)      // listado de contratos
        {
            setParaCrystal("contratos");
        }
        private void button4_Click(object sender, EventArgs e)      // reporte de ventas
        {
            setParaCrystal("ventas");
        }
        private void setParaCrystal(string repo)                    // genera el set para el reporte de crystal
        {
            if (repo== "resumen")
            {
                conClie datos = generareporte();                        // conClie = dataset de impresion de contrato   
                frmvizcont visualizador = new frmvizcont(datos);        // POR ESO SE CREO ESTE FORM frmvizcont PARA MOSTRAR AHI. ES MEJOR ASI.  
                visualizador.Show();
            }
            if (repo == "pedidos")
            {
                pedsclts datos = generarepedidos();
                frmvizcpeds visualizador = new frmvizcpeds(datos);
                visualizador.Show();
            }
            if (repo == "contratos")
            {
                conClie datos = generaliscont();
                frmvizcont visualizador = new frmvizcont(datos);
                visualizador.Show();
            }
            if (repo == "ventas")
            {
                conClie datos = generarepvtas();
                frmvizcont visualizador = new frmvizcont(datos);
                visualizador.Show();
            }
        }
        private conClie generarepvtas()
        {
            conClie repvtas = new conClie();                        // xsd
            conClie.repvtas_cabRow cabrow = repvtas.repvtas_cab.Newrepvtas_cabRow();
            cabrow.id = "0";
            cabrow.fecini = dtp_vtasfini.Value.ToString("dd/MM/yyyy");
            cabrow.fecfin = dtp_vtasfina.Value.ToString("dd/MM/yyyy");
            cabrow.tienda = tx_dat_vtasloc.Text.Trim();
            if (rb_listado.Checked == true) cabrow.modo = "listado";
            if (rb_resumen.Checked == true) cabrow.modo = "resumen";
            repvtas.repvtas_cab.Addrepvtas_cabRow(cabrow);
            // detalle
            foreach(DataGridViewRow row in dgv_vtas.Rows)
            {
                if (rb_resumen.Checked == true)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        detrow.codigo = row.Cells["item"].Value.ToString();
                        detrow.nombre = row.Cells["nombre"].Value.ToString().Trim();
                        detrow.cant = row.Cells["cant"].Value.ToString().Trim();
                        detrow.madera = row.Cells["madera"].Value.ToString().Trim();
                        detrow.medidas = row.Cells["medidas"].Value.ToString().Trim();
                        detrow.total = double.Parse(row.Cells["total"].Value.ToString());
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
                if (rb_listado.Checked == true)                      // tienda,b.fecha,a.contratoh,cliente,a.cant,a.item,a.nombre,a.medidas,a.madera,a.precio,a.total,PEDIDO,STOCK
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        detrow.fecha = row.Cells["fecha"].Value.ToString();
                        detrow.contrato = row.Cells["contratoh"].Value.ToString();
                        detrow.cliente = row.Cells["cliente"].Value.ToString();
                        detrow.cant = row.Cells["cant"].Value.ToString().Trim();
                        detrow.codigo = row.Cells["item"].Value.ToString();
                        detrow.nombre = row.Cells["nombre"].Value.ToString().Trim();
                        detrow.medidas = row.Cells["medidas"].Value.ToString().Trim();
                        detrow.madera = row.Cells["madera"].Value.ToString().Trim();
                        detrow.precio = row.Cells["precio"].Value.ToString().Trim();
                        detrow.total = double.Parse(row.Cells["total"].Value.ToString());
                        detrow.pedido = row.Cells["PEDIDO"].Value.ToString().Trim();
                        detrow.stock = row.Cells["STOCK"].Value.ToString().Trim();
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
            }
            return repvtas;
        }
        private conClie generareporte()                             // procedimiento para meter los datos del formulario hacia las tablas del dataset del reporte en crystal
        {
            conClie rescont = new conClie();                                    // dataset
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
            return rescont;
        }
        private pedsclts generarepedidos()
        {
            pedsclts pedset = new pedsclts();
            pedsclts.cab_lispedidosRow rowcab = pedset.cab_lispedidos.Newcab_lispedidosRow();
            rowcab.id = "0";
            rowcab.fecfin = dtp_pedfini.Value.ToString("dd/MM/yyyy");
            rowcab.fecini = dtp_pedfina.Value.ToString("dd/MM/yyyy");
            rowcab.taller = cmb_pedtaller.Text.Trim();
            pedset.cab_lispedidos.Addcab_lispedidosRow(rowcab);
            // 
            foreach (DataGridViewRow row in dgv_pedidos.Rows)
            {
                if (row.Cells["codped"].Value != null && row.Cells["codped"].Value.ToString().Trim() != "")
                {
                    pedsclts.det_lispedidosRow rowdet = pedset.det_lispedidos.Newdet_lispedidosRow();
                    rowdet.id = "0";
                    rowdet.lugar = row.Cells["destino"].Value.ToString();
                    rowdet.pedido = row.Cells["codped"].Value.ToString();
                    rowdet.cliente = row.Cells["cliente"].Value.ToString();
                    rowdet.codigo = row.Cells["item"].Value.ToString();
                    rowdet.nombre = row.Cells["nombre"].Value.ToString().Trim() + " - " + row.Cells["coment"].Value.ToString().Trim();
                    rowdet.coment = "";
                    rowdet.medidas = row.Cells["medidas"].Value.ToString();
                    rowdet.madera = row.Cells["madera"].Value.ToString();
                    rowdet.cant = row.Cells["cant"].Value.ToString();
                    rowdet.taller = row.Cells["taller"].Value.ToString();
                    rowdet.estado = row.Cells["estado"].Value.ToString();
                    rowdet.fecped = row.Cells["fecped"].Value.ToString().Substring(0,10);
                    rowdet.fecing = row.Cells["fecing"].Value.ToString().PadRight(10).Substring(0, 10);
                    rowdet.fecsal = row.Cells["fecent"].Value.ToString().PadRight(10).Substring(0, 10);
                    rowdet.fececon = row.Cells["feencon"].Value.ToString().PadRight(10).Substring(0, 10);
                    pedset.det_lispedidos.Adddet_lispedidosRow(rowdet);
                }
            }
            return pedset;
        }
        private conClie generaliscont()                             // procedimiento para generar los datos del listado de contratos en el dataset
        {
            conClie liscont = new conClie();
            conClie.liscont_cabRow rowcabeza = liscont.liscont_cab.Newliscont_cabRow();
            rowcabeza.id = "0";
            rowcabeza.fechini = dtp_confini.Value.ToString("yyyy-MM-dd");
            rowcabeza.fechfin = dtp_confina.Value.ToString("yyyy-MM-dd");
            rowcabeza.estado = tx_dat_conestado.Text.Trim();
            liscont.liscont_cab.Addliscont_cabRow(rowcabeza);
            //
            foreach(DataGridViewRow row in dgv_contratos.Rows)
            {
                if (row.Cells["fecha"].Value != null && row.Cells["fecha"].Value.ToString().Trim() != "")
                {
                    conClie.liscont_detRow rowdetalle = liscont.liscont_det.Newliscont_detRow();
                    rowdetalle.id = "0";
                    rowdetalle.fecha = row.Cells[0].Value.ToString();
                    rowdetalle.tienda = row.Cells[1].Value.ToString();
                    rowdetalle.contrato = row.Cells[2].Value.ToString();
                    rowdetalle.cliente = row.Cells[3].Value.ToString();
                    rowdetalle.coment = row.Cells[4].Value.ToString();
                    rowdetalle.fentrega = row.Cells[5].Value.ToString();
                    rowdetalle.fenreal = row.Cells[6].Value.ToString();
                    rowdetalle.estado = row.Cells[7].Value.ToString();
                    liscont.liscont_det.Addliscont_detRow(rowdetalle);
                }
            }
            return liscont;
        }
        #endregion
    }
}
