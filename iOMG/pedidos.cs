﻿using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class pedidos : Form
    {
        static string nomform = "pedidos";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "pedidos";         // idcategoria='CLI' -> vista anag_cli
        public int totfilgrid, cta;             // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string img_btN = "";
        string img_btE = "";
        string img_btP = "";
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
        string img_bti = "";            // imagen boton inicio
        string img_bts = "";            // imagen boton siguiente
        string img_btr = "";            // imagen boton regresa
        string img_btf = "";            // imagen boton final
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string tipede = "";             // tipo de pedido por defecto
        string tiesta = "";             // estado inicial por defecto del pedido
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();    // dtg primario, original con la carga del form

        public pedidos()
        {
            InitializeComponent();
        }
        private void pedidos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void pedidos_Load(object sender, EventArgs e)
        {
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            // Set up the delays for the ToolTip.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            //dataload("maestra");        // revisar  
            //grilla();
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");        // revisar  
            dataload("todos");          // revisar
            //grilla();
            advancedDataGridView1.DataSource = dtg;
            grilla2();
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = true;
            tabControl1.SelectedTab = tabgrilla;
            //advancedDataGridView1.Enabled = false;
            //tabControl1.Enabled = false;
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);
            this.tabreg.BackColor = Color.FromName(iOMG.Program.colgri);

            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_anul.Image = Image.FromFile(img_btA);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // longitudes maximas de campos
            tx_coment.MaxLength = 90;           // nombre
            tx_fechope.MaxLength = 45;           // direccion
        }
        private void grilla()                               // arma la advancedatagrid
        {
            // id,codped,tipoes,origen,destino,fecha,entrega,coment
            Font tiplg = new Font("Arial",7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            // id 
            advancedDataGridView1.Columns[0].Visible = false;
            // codigo de pedido
            advancedDataGridView1.Columns[1].Visible = true;            // columna visible o no
            advancedDataGridView1.Columns[1].HeaderText = "Pedido";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 70;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[1].Tag = "validaNO";
            advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // tipo de pedido
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Tipo Ped";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 70;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaNO";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Origen - taller
            advancedDataGridView1.Columns[3].Visible = true;       
            advancedDataGridView1.Columns[3].HeaderText = "Taller";
            advancedDataGridView1.Columns[3].Width = 80;
            advancedDataGridView1.Columns[3].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[3].Tag = "validaSI";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Destino
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Destino";
            advancedDataGridView1.Columns[4].Width = 80;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaSI";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha del pedido
            advancedDataGridView1.Columns[5].Visible = true;       
            advancedDataGridView1.Columns[5].HeaderText = "Fecha Ped.";
            advancedDataGridView1.Columns[5].Width = 100;
            advancedDataGridView1.Columns[5].ReadOnly = false;
            advancedDataGridView1.Columns[5].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha de Entrega
            advancedDataGridView1.Columns[6].Visible = true;       
            advancedDataGridView1.Columns[6].HeaderText = "Fecha Ent.";
            advancedDataGridView1.Columns[6].Width = 100;
            advancedDataGridView1.Columns[6].ReadOnly = false;
            advancedDataGridView1.Columns[6].Tag = "validaNO";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // comentarios
            advancedDataGridView1.Columns[7].Visible = true;
            advancedDataGridView1.Columns[7].HeaderText = "Comentarios";
            advancedDataGridView1.Columns[7].Width = 250;
            advancedDataGridView1.Columns[7].ReadOnly = false;
            advancedDataGridView1.Columns[7].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        private void grilla2()                              // grilla de filtros de nivel superior
        {
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ColumnCount = (advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.ColumnCount;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.Rows.Add();
            for (int i = 0; i < ((advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.Columns.Count); i++)
            {
                dataGridView2.Columns[i].Width = advancedDataGridView1.Columns[i].Width;
                dataGridView2.Columns[i].Name = advancedDataGridView1.Columns[i].Name;
                //
                if (i == 0)
                {
                    dataGridView2.Columns[i].Visible = false;
                }
            }
            dataGridView2.Columns["id"].ReadOnly = true;
        }
        private void grilladet(string modo)                            // grilla detalle de pedido
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if(modo=="nuevo") dataGridView1.ColumnCount = 10;
            // id 
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho
            dataGridView1.Columns[0].HeaderText = "Id";         // titulo de la columna
            dataGridView1.Columns[0].Name = "iddetaped";
            // cant
            dataGridView1.Columns[1].Visible = true;            // columna visible o no
            dataGridView1.Columns[1].HeaderText = "Cant";    // titulo de la columna
            dataGridView1.Columns[1].Width = 30;                // ancho
            dataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].Name = "cant";
            // articulo
            dataGridView1.Columns[2].Visible = true;            // columna visible o no
            dataGridView1.Columns[2].HeaderText = "Artículo";    // titulo de la columna
            dataGridView1.Columns[2].Width = 100;                // ancho
            dataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Name = "item";
            // nombre del articulo
            dataGridView1.Columns[3].Visible = true;            // columna visible o no
            dataGridView1.Columns[3].HeaderText = "Nombre";    // titulo de la columna
            dataGridView1.Columns[3].Width = 200;                // ancho
            dataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[3].Name = "nombre";
            //dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // medidas 
            dataGridView1.Columns[4].Visible = true;            // columna visible o no
            dataGridView1.Columns[4].HeaderText = "Medidas";    // titulo de la columna
            dataGridView1.Columns[4].Width = 100;                // ancho
            dataGridView1.Columns[4].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].Name = "medidas";
            // madera
            dataGridView1.Columns[5].Visible = true;            // columna visible o no
            dataGridView1.Columns[5].HeaderText = "Madera";    // titulo de la columna
            dataGridView1.Columns[5].Width = 60;                // ancho
            dataGridView1.Columns[5].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].Name = "madera";
            // detalle2
            dataGridView1.Columns[6].Visible = true;            // columna visible o no
            dataGridView1.Columns[6].HeaderText = "Deta2";    // titulo de la columna
            dataGridView1.Columns[6].Width = 70;                // ancho
            dataGridView1.Columns[6].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].Name = "piedra";
            // acabado - descrizionerid
            dataGridView1.Columns[7].Visible = true;            // columna visible o no
            dataGridView1.Columns[7].HeaderText = "Acabado";    // titulo de la columna
            dataGridView1.Columns[7].Width = 70;                // ancho
            dataGridView1.Columns[7].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].Name = "descrizionerid";
            // comentario
            dataGridView1.Columns[8].Visible = true;            // columna visible o no
            dataGridView1.Columns[8].HeaderText = "Comentario"; // titulo de la columna
            dataGridView1.Columns[8].Width = 150;                // ancho
            dataGridView1.Columns[8].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[8].Name = "coment";
            // codigo de acabado - idcodice
            dataGridView1.Columns[9].Visible = false;            // columna visible o no
            dataGridView1.Columns[9].HeaderText = "Codest"; // titulo de la columna
            dataGridView1.Columns[9].Width = 50;                // ancho
            dataGridView1.Columns[9].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[9].Name = "estado";
        }
        private void jalainfo()                             // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", nomform);
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
                        // boton de vista preliminar .... esta por verse su utlidad
                        if (row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                        if (row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                        if (row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                        if (row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                        if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                    }
                    if(row["formulario"].ToString() == "pedidos")
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
        public void jalaoc(string campo)                    // jala datos id o ????
        {
            if (campo == "tx_idr")  //  && tx_idr.Text != ""
            {   // id,codped,tipoes,origen,destino,fecha,entrega,coment
                // tx_idr.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // 
                tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // codigo pedido
                tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();  // tipo pedido
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();   // taller origen
                tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();   // destino
                tx_fechope.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString();    // fecha pedido
                tx_fentreg.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString();    // fecha entrega
                tx_coment.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();     // comentario
                //cmb_cap.SelectedValue = tx_dat_tiped.Text;
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                cmb_destino.SelectedIndex = cmb_destino.FindString(tx_dat_dest.Text);
                //cmb_tip.SelectedValue = tx_dat_tip.Text;
                jaladet(tx_codped.Text);
            }
        }
        private void jaladet(string pedido)                 // jala el detalle del pedido
        {
            // id,cant,item,nombre,medidas,madera,detalle2,acabado,comentario,estado
            string jalad = "select a.iddetaped,a.cant,a.item,a.nombre,a.medidas,a.madera,a.piedra,b.descrizionerid,a.coment,a.estado " +
                "from detaped a left join desc_est b on b.idcodice=a.estado " +
                "where a.pedidoh=@pedi";
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if(conn.State == ConnectionState.Open)
                {
                    MySqlCommand micon = new MySqlCommand(jalad, conn);
                    micon.Parameters.AddWithValue("@pedi", pedido);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    grilladet("edita");     // obtiene contenido de grilla con DT
                    dt.Dispose();
                    da.Dispose();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en código");
                Application.Exit();
                return;
            }
        }
        public void dataload(string quien)                  // jala datos para los combos y la grilla
        {   // "todos"=comboscodigo, "capit"=codigo familia, "maestra"=items de la grilla
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            tabControl1.SelectedTab = tabreg;
            if (quien == "maestra")
            {
                // datos de los pedidos
                string datgri = "select id,codped,tipoes,origen,destino,date_format(date(fecha),'%Y-%m-%d') as fecha,date_format(date(entrega),'%Y-%m-%d') as entrega,coment " +
                    "from pedidos where tipoes=@tip";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                cdg.Parameters.AddWithValue("@tip", "TPE001");
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Fill(dtu);  // original con la carga
                dag.Dispose();
            }
            //  datos para el combobox de tipo de documento
            if (quien == "capit")
            {
                cmb_estado.Items.Clear();
                //tx_dat_tip.Text = "";
                const string contip = "select b.descrizione,a.tipol from items a " +
                    "left join desc_tip b on b.idcodice=a.tipol " +
                    "where a.capit=@des group by a.tipol";
                MySqlCommand cmdtip = new MySqlCommand(contip, conn);
                cmdtip.Parameters.AddWithValue("@des", "tx_dat_cap.Text.Trim()");       // revisar
                DataTable dttip = new DataTable();
                MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                datip.Fill(dttip);
                foreach (DataRow row in dttip.Rows)
                {
                    cmb_estado.Items.Add(row.ItemArray[1].ToString());
                    cmb_estado.ValueMember = row.ItemArray[1].ToString();
                }
            }
            if (quien == "todos")
            {
                // seleccion de taller de produccion ... ok
                const string contaller = "select descrizionerid,idcodice from desc_loc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                DataTable dttaller = new DataTable();
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0,6) + " - " + row.ItemArray[0].ToString());
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del almacen de destino ... ok
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
                // seleccion de tipo de pedido ... ok
                const string conpedido = "select descrizionerid,idcodice from desc_tpe " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdpedido = new MySqlCommand(conpedido, conn);
                DataTable dtpedido = new DataTable();
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de estado del pedido
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
                //
                // seleccion de familia de art
                cmb_fam.Items.Clear();
                //tx_dat_fam.Text = "";
                cmb_fam.Tag = "";
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
                // seleccion de modelo
                const string conmod = "select descrizionerid,idcodice from desc_mod " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdmod = new MySqlCommand(conmod, conn);
                DataTable dtmod = new DataTable();
                MySqlDataAdapter damod = new MySqlDataAdapter(cmdmod);
                damod.Fill(dtmod);
                foreach (DataRow row in dtmod.Rows)
                {
                    cmb_mod.Items.Add(row.ItemArray[0].ToString());
                    cmb_mod.ValueMember = row.ItemArray[0].ToString();
                }
                // seleccion de madera
                cmb_mad.Items.Clear();
                //tx_dat_mad.Text = "";
                cmb_mad.Tag = "";
                const string conmad = "select descrizionerid,idcodice from desc_mad " +
                    "where numero=1";
                MySqlCommand cmdmad = new MySqlCommand(conmad, conn);
                DataTable dtmad = new DataTable();
                MySqlDataAdapter damad = new MySqlDataAdapter(cmdmad);
                damad.Fill(dtmad);
                foreach (DataRow row in dtmad.Rows)
                {
                    this.cmb_mad.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());   // citem_mad
                    this.cmb_mad.ValueMember = row.ItemArray[1].ToString(); //citem_mad.Value.ToString();
                }
                // seleccion del tipo de mueble
                cmb_tip.Items.Clear();
                cmb_tip.Tag = "";
                const string contip = "select descrizionerid,idcodice from desc_tip " +
                    "where numero=1";
                MySqlCommand cmdtip = new MySqlCommand(contip, conn);
                DataTable dttip = new DataTable();
                MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                datip.Fill(dttip);
                foreach (DataRow row in dttip.Rows)
                {
                    this.cmb_tip.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());
                    this.cmb_tip.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de detalle1
                cmb_det1.Items.Clear();
                //tx_dat_det1.Text = "";
                cmb_det1.Tag = "";
                const string condt1 = "select descrizionerid,idcodice from desc_dt1 " +
                    "where numero=1";
                MySqlCommand cmddt1 = new MySqlCommand(condt1, conn);
                DataTable dtdt1 = new DataTable();
                MySqlDataAdapter dadt1 = new MySqlDataAdapter(cmddt1);
                dadt1.Fill(dtdt1);
                foreach (DataRow row in dtdt1.Rows)
                {
                    this.cmb_det1.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt1
                    this.cmb_det1.ValueMember = row.ItemArray[1].ToString();    // citem_dt1.Value.ToString();
                }
                // seleccion de acabado (pulido, lacado, etc)
                cmb_aca.Items.Clear();
                //tx_dat_aca.Text = "";
                cmb_aca.Tag = "";
                const string conaca = "select descrizionerid,idcodice from desc_est " +
                    "where numero=1";
                MySqlCommand cmdaca = new MySqlCommand(conaca, conn);
                DataTable dtaca = new DataTable();
                MySqlDataAdapter daaca = new MySqlDataAdapter(cmdaca);
                daaca.Fill(dtaca);
                foreach (DataRow row in dtaca.Rows)
                {
                    cmb_aca.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_aca
                    cmb_aca.ValueMember = row.ItemArray[1].ToString(); //citem_aca.Value.ToString();
                }
                // seleccion de taller
                cmb_tal.Items.Clear();
                //tx_dat_tal.Text = "";
                cmb_tal.Tag = "";
                const string contal = "select descrizionerid,codigo from desc_loc " +
                    "where numero=1";
                MySqlCommand cmdtal = new MySqlCommand(contal, conn);
                DataTable dttal = new DataTable();
                MySqlDataAdapter datal = new MySqlDataAdapter(cmdtal);
                datal.Fill(dttal);
                foreach (DataRow row in dttal.Rows)
                {
                    cmb_tal.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_tal
                    cmb_tal.ValueMember = row.ItemArray[1].ToString(); // citem_tal.Value.ToString();
                }
                // seleccion de detalle 2 (tallado, marqueteado, etc)
                cmb_det2.Items.Clear();
                //tx_dat_det2.Text = "";
                cmb_det2.Tag = "";
                const string condt2 = "select descrizione,idcodice from desc_dt2 " +
                    "where numero=1";
                MySqlCommand cmddt2 = new MySqlCommand(condt2, conn);
                DataTable dtdt2 = new DataTable();
                MySqlDataAdapter dadt2 = new MySqlDataAdapter(cmddt2);
                dadt2.Fill(dtdt2);
                foreach (DataRow row in dtdt2.Rows)
                {
                    cmb_det2.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt2
                    cmb_det2.ValueMember = row.ItemArray[1].ToString();     //citem_dt2.Value.ToString();
                }
                // seleccion de detalle 3
                cmb_det3.Items.Clear();
                //tx_dat_det3.Text = "";
                cmb_det3.Tag = "";
                const string condt3 = "select descrizione,idcodice from desc_dt3 where numero=1";
                MySqlCommand cmddt3 = new MySqlCommand(condt3, conn);
                DataTable dtdt3 = new DataTable();
                MySqlDataAdapter dadt3 = new MySqlDataAdapter(cmddt3);
                dadt3.Fill(dtdt3);
                foreach (DataRow row in dtdt3.Rows)
                {
                    cmb_det3.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt3
                    cmb_det3.ValueMember = row.ItemArray[1].ToString();    //citem_dt3.Value.ToString();
                }
            }
            //
            conn.Close();
        }
        string[] equivinter(string titulo)                  // equivalencia entre titulo de columna y tabla 
        {
            string[] retorna = new string[2];
            switch (titulo)
            {
                case "NIVEL":
                    retorna[0] = "desc_niv";
                    retorna[1] = "codigo";
                    break;
                case "???":
                    retorna[0] = "";
                    retorna[1] = "";
                    break;
                case "????":
                    retorna[0] = "";
                    retorna[1] = "";
                    break;
                case "LOCAL":
                    retorna[0] = "desc_alm";
                    retorna[1] = "idcodice";
                    break;
                case "TIENDA":
                    retorna[0] = "desc_ven";
                    retorna[1] = "idcodice";
                    break;
                case "SEDE":
                    retorna[0] = "desc_loc";
                    retorna[1] = "idcodice";
                    break;
                case "RUC":
                    retorna[0] = "desc_raz";
                    retorna[1] = "idcodice";
                    break;
            }
            return retorna;
        }
        private Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void filtros(string expres)                 // filtros de nivel superior datagridview2
        {
            DataView dv = new DataView(dtg);
            dv.RowFilter = expres;
            dtg = dv.ToTable();
            advancedDataGridView1.DataSource = dtg;
            grilla();
            //cellsum(0);
        }
        private void armani()                               // arma el codigo y busca en la maestra
        {
            string fam="", mod="", mad="", tip="", de1 = "", aca = "", tal = "", de2 = "", de3 = "";
            if (cmb_fam.SelectedItem != null) fam = cmb_fam.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_mod.SelectedItem != null) mod = cmb_mod.SelectedItem.ToString().Substring(0, 3);    // 3
            if (cmb_mad.SelectedItem != null) mad = cmb_mad.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_tip.SelectedItem != null) tip = cmb_tip.SelectedItem.ToString().Substring(0, 2);    // 2
            if (cmb_det1.SelectedItem != null) de1 = cmb_det1.SelectedItem.ToString().Substring(0, 2);  // 2
            if (cmb_aca.SelectedItem != null) aca = cmb_aca.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_tal.SelectedItem != null) tal = cmb_tal.SelectedItem.ToString().Substring(0, 2);    // 2
            if (cmb_det2.SelectedItem != null) de2 = cmb_det2.SelectedItem.ToString().Substring(0, 3);  // 3
            if (cmb_det3.SelectedItem != null) de3 = cmb_det3.SelectedItem.ToString().Substring(0, 3);  // 3 _____ total 18
            tx_d_codi.Text = fam + mod + mad + tip + de1 + aca + tal + de2 + de3;
            if(fam != "" && mod != "" && tip != "" && de1 != "" && aca != "" && de2 != "" && de3 != "")
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        if(conn.State == ConnectionState.Open)
                        {
                            string codbs = fam + mod + "X" + tip + de1 + aca + "XX" + de2 + de3 + "N000";
                            string busca = "select id,nombr,medid,umed,soles2018 from items where codig=@cod";
                            MySqlCommand micon = new MySqlCommand(busca, conn);
                            micon.Parameters.AddWithValue("@cod", codbs);
                            MySqlDataReader dr = micon.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                MessageBox.Show("No existe en la base de datos!", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            if (dr.Read())
                            {
                                tx_d_nom.Text = dr.GetString(1);
                                tx_d_med.Text = dr.GetString(2);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se puede conectar a la base de datos", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error interno en codigo");
                    Application.Exit();
                    return;
                }
            }
        }
        private void bt_det_Click(object sender, EventArgs e)   // agrega a la grilla de detalle el articulo
        {
            dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text, tx_d_mad.Text,
                tx_d_det2.Text, tx_d_est.Text, tx_d_com.Text, cmb_aca.Tag.ToString());

        }

        #region limpiadores_modos
        public void sololee(Form lfrm)
        {
            foreach (Control oControls in lfrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
            }
        }
        public void escribe(Form efrm)
        {
            foreach (Control oControls in efrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
            }
        }
        public static void limpiar(Form ofrm)
        {
            foreach (Control oControls in ofrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        public void limpia_otros()
        {
            //this.checkBox1.Checked = false;
        }
        public void limpia_combos()
        {
            cmb_tipo.SelectedIndex = -1;
            cmb_destino.SelectedIndex = -1;
            cmb_taller.SelectedIndex = -1;
            cmb_estado.SelectedIndex = -1;
        }
        public void limpiapag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if(oControls is CheckBox)
                {
                    //checkBox1.Checked = false;
                }
                if(oControls is ComboBox)
                {
                    cmb_tipo.SelectedIndex = -1;
                    cmb_destino.SelectedIndex = -1;
                    cmb_taller.SelectedIndex = -1;
                    cmb_estado.SelectedIndex = -1;
                }
            }
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            if ("" == "")
            {
                MessageBox.Show("Ingrese el detalle 3", " Error! ");
                //cmb_det3.Focus();
                return;
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            if (modo == "NUEVO")
            {
                var aa = MessageBox.Show("Confirma que desea crear el artículo?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(aa == DialogResult.Yes)
                {
                    if(graba() == true)
                    {
                        // insertamos en el datatable
                        DataRow dr = dtg.NewRow();
                        // id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles2018
                        string codi = "N000";
                        dr[1] = codi;
                        //micon.Parameters.AddWithValue("@jgo", "N000");
                        dr[11] = tx_coment.Text.Trim();
                        dr[12] = tx_fechope.Text.Trim();
                        dr[13] = "C.U.";
                        dr[14] = tx_codped.Text;
                        dtg.Rows.Add(dr);
                        dtu.Rows.Add(dr);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo grabar el artículo", "Error en crear", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                var aa = MessageBox.Show("Confirma que desea modificar el artículo?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    edita();
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "ANULAR")       // opción para borrar
            { 
                // 

            }
            if (iserror == "no")
            {
                // debe limpiar los campos y actualizar la grilla
                limpiar(this);
                limpiapag(tabreg);
                limpia_otros();
                cmb_tipo.Focus();
            }
        }
        private bool graba()
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                try
                {
                    string codi = "N000";
                    string inserta = "insert into xxx (" +
                        "codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,medid,umed,soles2018) values (" +
                        "@codi,@capi,@mode,@made,@tipo,@det1,@acab,@tall,@det2,@det3,@jgo,@nomb,@medi,@umed,@prec)";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@codi", codi);
                    micon.Parameters.AddWithValue("@jgo", "N000");
                    micon.Parameters.AddWithValue("@nomb", tx_coment.Text.Trim());
                    micon.Parameters.AddWithValue("@medi", tx_fechope.Text.Trim());
                    micon.Parameters.AddWithValue("@umed", "C.U.");
                    micon.Parameters.AddWithValue("@prec", tx_codped.Text);
                    micon.ExecuteNonQuery();
                    retorna = true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en conexión");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return retorna;
            }
            conn.Close();
            return retorna;
        }
        private void edita()
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string codi = "N000";
                    string actua = "update xxx set " +
                        "codig=@codi,capit=@capi,model=@mode,mader=@made,tipol=@tipo,deta1=@det1,acaba=@acab,talle=@tall," +
                        "deta2=@det2,deta3=@det3,juego=@jgo,nombr=@nomb,medid=@medi,umed=@umed,soles2018=@prec " +
                        "where id=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@codi", codi);
                    micon.Parameters.AddWithValue("@jgo", "N000");
                    micon.Parameters.AddWithValue("@nomb", tx_coment.Text.Trim());
                    micon.Parameters.AddWithValue("@medi", tx_fechope.Text.Trim());
                    micon.Parameters.AddWithValue("@umed", "C.U.");
                    micon.Parameters.AddWithValue("@prec", tx_codped.Text);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en conexión");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return;
            }
            conn.Close();
        }
        #endregion boton_form;

        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text != "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_rind_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_rind.Text != "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        #endregion leaves;

        #region botones_de_comando_y_pedidos  
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
                if (Convert.ToString(row["btn5"]) == "S")
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")
                {
                    bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
        }
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            //advancedDataGridView1.Enabled = true;
            tabControl1.Enabled = true;
            tabControl1.SelectedTab = tabreg;
            escribe(this);
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            cmb_tipo.Focus();
            limpiar(this);
            limpia_otros();
            limpia_combos();
            limpiapag(tabreg);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            grilladet("nuevo");
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            cmb_tipo.Enabled = false;
            cmb_estado.SelectedIndex = cmb_estado.FindString(tiesta);
            cmb_estado.Enabled = false;
            tx_codped.ReadOnly = true;  // al grabar debe generar su codificacion
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabgrilla;
            escribe(this);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            limpia_otros();
            limpia_combos();
            jalaoc("tx_idr");
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            sololee(this);
            Tx_modo.Text = "IMPRIMIR";
            button1.Image = Image.FromFile("print48");
            cmb_tipo.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)          // pone todos los pedidos en N
        {
            advancedDataGridView1.Enabled = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabreg;
            escribe(this);
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            limpia_otros();
            limpia_combos();
            jalaoc("tx_idr");
        }
        private void bt_exc_Click(object sender, EventArgs e)           // exporta a excel
        {
            // me quede aca!
        }
        private void Bt_first_Click(object sender, EventArgs e)
        {
            limpiar(this);
            limpia_combos();
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[0].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            int aca = int.Parse(tx_rind.Text) - 1;
            limpia_combos();
            limpiar(this);
            int fila = advancedDataGridView1.CurrentCell.RowIndex;
            int nfil = fila - 1;
            if (nfil < 0)
            {
                nfil = nfil + 1;
            }
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[nfil].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            int aca = int.Parse(tx_rind.Text) + 1;
            limpia_combos();
            limpiar(this);
            int fila = advancedDataGridView1.CurrentCell.RowIndex;
            int nfil = fila + 1;
            if(nfil > advancedDataGridView1.Rows.Count - 2)
            {
                nfil = nfil - 1;
            }
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[nfil].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            int ultimo = advancedDataGridView1.Rows.Count-2;
            limpiar(this);
            limpia_combos();
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[ultimo].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = ultimo.ToString();//advancedDataGridView1.Rows[ultimo].Cells[0].Value.ToString();
            tx_rind_Leave(null, null);
        }

        private void tabreg_Enter(object sender, EventArgs e)
        {
            bt_exc.Enabled = false;
            Bt_print.Enabled = true;
        }
        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            bt_exc.Enabled = true;
            Bt_print.Enabled = false;
        }
        #endregion botones;
        // pedidos para habilitar los botones de comando
        #endregion botones_de_comando  ;

        #region comboboxes
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)   // tipo de pedido
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tiped.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)   // taller de origen
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_destino_SelectionChangeCommitted(object sender, EventArgs e)   // almacen destino
        {
            if (cmb_destino.SelectedValue != null) tx_dat_dest.Text = cmb_destino.SelectedValue.ToString();
            else tx_dat_dest.Text = cmb_destino.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_estado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estado.SelectedValue != null) tx_dat_estad.Text = cmb_estado.SelectedValue.ToString();
            else tx_dat_estad.Text = cmb_estado.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        //
        private void cmb_fam_SelectionChangeCommitted(object sender, EventArgs e)       // capitulo familia
        {
            armani();
        }
        private void cmb_mod_SelectionChangeCommitted(object sender, EventArgs e)       // modelo
        {
            armani();
        }
        private void cmb_mad_SelectionChangeCommitted(object sender, EventArgs e)       // madera
        {
            tx_d_mad.Text = cmb_mad.SelectedItem.ToString().Substring(4, cmb_mad.SelectedItem.ToString().Length-4).Trim();
            armani();
        }
        private void cmb_tip_SelectedIndexChanged(object sender, EventArgs e)           // tipologia
        {
            armani();
        }
        private void cmb_det1_SelectionChangeCommitted(object sender, EventArgs e)      // detalle1
        {
            armani();
        }
        private void cmb_aca_SelectionChangeCommitted(object sender, EventArgs e)       // acabado
        {
            tx_d_est.Text = cmb_aca.SelectedItem.ToString().Substring(5, cmb_aca.SelectedItem.ToString().Length-5).Trim();
            armani();
        }
        private void cmb_tal_SelectedIndexChanged(object sender, EventArgs e)           // taller
        {
            armani();
        }
        private void cmb_det2_SelectionChangeCommitted(object sender, EventArgs e)      // detalle 2
        {
            tx_d_det2.Text = cmb_det2.SelectedItem.ToString().Substring(6, cmb_det2.SelectedItem.ToString().Length - 6).Trim();
            armani();
        }
        private void cmb_det3_SelectedIndexChanged(object sender, EventArgs e)          // detalle3
        {
            armani();
        }
        
        #endregion comboboxes

        #region datagridview2
        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Value != null)
            {
                string frase = dataGridView2.Columns[e.ColumnIndex].Name.ToString() + " like '" + dataGridView2.CurrentCell.Value.ToString() + "*'";
                filtros(frase);
            }
            if(dataGridView2.CurrentCell.Value == null || dataGridView2.CurrentCell.Value.ToString().Trim() == "")
            {
                if(true == true)    // no hay otros filtros
                {
                    advancedDataGridView1.DataSource = dtu;
                }
            }
        }
        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                advancedDataGridView1.HorizontalScrollingOffset = e.NewValue;
            }
        }
        #endregion

        #region datagridview1 - grilla detalle de pedido
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                tx_d_nom.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                tx_d_med.Text = dataGridView1.Rows[e.RowIndex].Cells["medidas"].Value.ToString();
                tx_d_can.Text = dataGridView1.Rows[e.RowIndex].Cells["cant"].Value.ToString();
                tx_d_id.Text = dataGridView1.Rows[e.RowIndex].Cells["iddetaped"].Value.ToString();
                tx_d_codi.Text = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString();
                tx_d_com.Text = dataGridView1.Rows[e.RowIndex].Cells["coment"].Value.ToString();

                string fam = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(0, 1);
                string mod = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(1, 3);
                string mad = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(4, 1);
                string tip = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(5, 2);
                string de1 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(7, 2);
                string aca = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(9, 1);
                string tal = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(10, 2);
                string de2 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(12, 3);
                string de3 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(15, 3);

                cmb_fam.Tag = fam;
                cmb_fam.SelectedIndex = cmb_fam.FindString(cmb_fam.Tag.ToString());
                cmb_mod.Tag = mod;
                cmb_mod.SelectedIndex = cmb_mod.FindString(cmb_mod.Tag.ToString());
                cmb_mad.Tag = mad;
                cmb_mad.SelectedIndex = cmb_mad.FindString(cmb_mad.Tag.ToString());
                cmb_mad_SelectionChangeCommitted(null, null);
                cmb_tip.Tag = tip;
                cmb_tip.SelectedIndex = cmb_tip.FindString(cmb_tip.Tag.ToString());
                cmb_det1.Tag = de1;
                cmb_det1.SelectedIndex = cmb_det1.FindString(cmb_det1.Tag.ToString());
                cmb_det1_SelectionChangeCommitted(null, null);
                cmb_aca.Tag = aca;
                cmb_aca.SelectedIndex = cmb_aca.FindString(cmb_aca.Tag.ToString());
                cmb_aca_SelectionChangeCommitted(null, null);
                cmb_tal.Tag = tal;
                cmb_tal.SelectedIndex = cmb_tal.FindString(cmb_tal.Tag.ToString());
                cmb_det2.Tag = de2;
                cmb_det2.SelectedIndex = cmb_det2.FindString(cmb_det2.Tag.ToString());
                //cmb_det2.SelectionChangeCommitted(null, null);
                cmb_det3.Tag = de3;
                cmb_det3.SelectedIndex = cmb_det3.FindString(cmb_det3.Tag.ToString());
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var aa = MessageBox.Show("una fila para borrar", "Confirma?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // si es edicion, si es el usuario autorizado y el pedido es reciente => borra la(s) filas de detalle
                // busca en la base de datos y lo borra, debe actualizar estado del pedido y saldos

            }
        }

        #endregion

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString;
        }
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            dtg.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)            // almacena valor previo al ingresar a la celda
        {
            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                //string codu = "";
                string idr,rind = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rind = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabreg;
                limpiar(this);
                limpiapag(tabreg);
                limpia_otros();
                limpia_combos();
                tx_idr.Text = idr;
                tx_rind.Text = rind;
                jalaoc("tx_idr");
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0 
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
                        if (e.ColumnIndex == 2)                         // valida familia o capitulo
                        {
                            if (lib.validac("desc_gru", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if(e.ColumnIndex == 3)                      // valida modelo
                        {
                            /*
                            if(lib.validac("desc_mod", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                MessageBox.Show("El valor no es valido en la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            */
                            if(e.FormattedValue.ToString().Length != 3)
                            {
                                MessageBox.Show("El valor debe tener 3 dígitos", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 4)           // valida madera
                        {
                            if (lib.validac("desc_mad", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 5)           // valida tipologia
                        {
                            if (lib.validac("desc_tip", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 6)           // valida detalle 1
                        {
                            if (lib.validac("desc_dt1", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        if(e.ColumnIndex == 7)          // valida acabado
                        {
                            if (lib.validac("desc_est", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        // id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles,soles2018
                        if(e.ColumnIndex == 8)          // valida detalle 2
                        {
                            if (lib.validac("desc_dt2", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        if(e.ColumnIndex == 9)          // valida detalle 3
                        {
                            if (lib.validac("desc_dt3", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
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
            }
        }
        private void advancedDataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dataGridView2.HorizontalScrollingOffset = e.NewValue;
            }
        }
        private void advancedDataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView2.ColumnCount > 1 && advancedDataGridView1.Rows.Count > 1)
            {
                dataGridView2.Columns[e.Column.Index].Width = e.Column.Width;
            }
        }
        #endregion
    }
}
