using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class docsvta : Form
    {
        static string nomform = "docsvta";      // nombre del formulario
        string asd = iOMG.Program.vg_user;          // usuario conectado al sistema
        string colback = iOMG.Program.colbac;       // color de fondo
        string colpage = iOMG.Program.colpag;       // color de los pageframes
        string colgrid = iOMG.Program.colgri;       // color de las grillas
        string colstrp = iOMG.Program.colstr;       // color del strip
        static string nomtab = "pedidos";
        libreria lib = new libreria();
       
        #region variables
        //public string perAg = "";             // permisos agregar
        //public string perMo = "";             // permisos modificar
        //public string perAn = "";             // permisos anular
        //public string perIm = "";             // permisos imprimir
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
        string img_pre = "";            // imagen del boton vista preliminar
        string img_ver = "";            // imagen del boton visualizacion (solo ver)
        string tipede = "";             // tipo de pedido de clientes por defecto
        string letiden = "";            // letra inicial identificadora de pedidos de clientes
        string tiesta = "";             // estado inicial por defecto del pedido de clientes
        string escambio = "";           // estados de pedido de clientes que admiten modificar el pedido
        string estpend = "";            // estado de pedido de clientes con articulos pendientes de recibir
        string estcomp = "";            // estado de pedido de clientes con articulos recibidos en su totalidad
        string estenv = "";             // estado de pedido de clientes enviado a producción
        string estanu = "";             // estado de pedido de clientes anulado
        string nomanu = "";             // nombre estado anulado
        string estcer = "";             // estado de pedido de clientes cerrado tal como esta, ya no se atiende
        string codVar = "";             // 4 caracteres de inicio que permiten varios items por pedido
        //string canovald2 = "";          // captitulos donde no se valida det2
        //string conovald2 = "";          // valor por defecto al no validar det2
        //string letpied = "";            // letra identificadora de piedra en detalle2
        string estman = "";             // estados que se pueden seleccionar manualmente
        int indant = -1;                // indice anterior al cambio en el combobox de estado
        //string cn_adm = "";               // codigo nivel usuario admin
        //string cn_sup = "";               // codigo nivel usuario superusuario
        //string cn_est = "";               // codigo nivel usuario estandar
        //string cn_mir = "";               // codigo nivel usuario solo mira
        string cliente = Program.cliente;    // razon social para los reportes
        #endregion

        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";" +
            "ConnectionLifeTime=" + ctl + ";default command timeout=120";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();        // dtg primario, original con la carga del 
        DataTable dttaller = new DataTable();   // combo local de ventas
        DataTable dtdoc = new DataTable();      // combo tipo doc cliente

        public docsvta()
        {
            InitializeComponent();
        }
        private void docsvta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1 && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                if (tx_ndc.Focused == true)
                {
                    para1 = "anag_cli";   // maestra clientes
                    para2 = "todos";   // 
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue1;    // tipdoc y numero
                            //ayu2.ReturnValue0;    // id del cliente
                            //ayu3.ReturnValue2;    // nombre
                            tx_nombre.Text = ayu2.ReturnValue2;
                            tx_idc.Text = ayu2.ReturnValue0;
                        }
                    }
                }
                if (tx_cont.Focused == true)
                {
                    para1 = "contrat";
                    para2 = "";    // tx_idc.Text
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue0;    // id del contrato
                            tx_idc.Text = ayu2.ReturnValue0;
                            tx_cont.Text = ayu2.ReturnValue1;
                        }
                    }
                }
                if (tx_d_nom.Focused == true || tx_d_codi.Focused == true)
                {
                    para1 = "detacon";
                    para2 = tx_cont.Text;
                    para3 = "";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            tx_d_can.Text = ayu2.ReturnValueA[7].ToString();
                            tx_d_nom.Text = ayu2.ReturnValueA[3].ToString();
                            tx_d_med.Text = ayu2.ReturnValueA[4].ToString();
                            tx_d_mad.Text = ayu2.ReturnValueA[5].ToString();
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void docsvta_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            grilladet("NUEVO");
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            tx_d_nom.Enabled = false;
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);

            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);     // oki
            Bt_edit.Image = Image.FromFile(img_btE);    // oki
            Bt_anul.Image = Image.FromFile(img_anul);   // oki
            bt_view.Image = Image.FromFile(img_ver);    // oki
            Bt_print.Image = Image.FromFile(img_btP);   // oki
            bt_prev.Image = Image.FromFile(img_pre);    // oki
            bt_exc.Image = Image.FromFile(img_btexc);   // oki
            Bt_close.Image = Image.FromFile(img_btq);   // oki
            //
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            //
            tx_status.Visible = true;                  // solo sera visible si tiene estado
            // longitudes maximas de campos
            tx_coment.MaxLength = 240;
            tx_corre.CharacterCasing = CharacterCasing.Upper;
            //
            this.milinea1.BackColor = Color.White;
            this.milinea1.ForeColor = Color.White;
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
                        if (row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
                        //if (row["param"].ToString() == "img_imprime") img_imprime = row["valor"].ToString().Trim();  // imagen del boton IMPRIMIR REPORTE
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "clientes") tipede = row["valor"].ToString().Trim();         // tipo de pedido de clientes
                        if (row["campo"].ToString() == "indentif" && row["param"].ToString() == "letra") letiden = row["valor"].ToString().Trim();         // letra identif para codigo de pedido
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "codAnu") estanu = row["valor"].ToString().Trim();         // codigo estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "Anulado") nomanu = row["valor"].ToString().Trim();         // nombre estado anulado
                        if (row["campo"].ToString() == "articulos" && row["param"].ToString() == "varios") codVar = row["valor"].ToString().Trim();         // codigo que permite varios items x pedido
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
        private void jalaoc(string campo)                   // jala datos 
        {
            if (campo == "tx_idr" && tx_idr.Text != "" && tx_corre.Text.Trim() == "")
            {
                if (Tx_modo.Text != "NUEVO")
                {

                }
                jaladet(tx_corre.Text);
            }
            if (campo == "tx_codped" && tx_corre.Text != "" && tx_idr.Text.Trim() == "")
            {
                if (Tx_modo.Text != "NUEVO")
                {

                }
                int cta = 0;
                foreach (DataRow row in dtg.Rows)
                {
                    if (row["codped"].ToString().Trim() == tx_corre.Text.Trim())
                    {
                        // ...
                        jaladet(tx_corre.Text);
                    }
                    cta = cta + 1;
                }
            }
        }
        private void jaladet(string pedido)                 // jala el detalle 
        {
            string jalad = "";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
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
        private void grilla()                               // arma la grilla
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            // 
        }
        private void grilladet(string modo)                 // grilla detalle del doc. venta
        {   // a.contratoh,a.item,a.nombre,a.cant,a.medidas,de.descrizione,a.codref,a.piedra,a.precio,a.total
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 9;
            // it
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho
            dataGridView1.Columns[0].HeaderText = "It";         // titulo de la columna
            dataGridView1.Columns[0].Name = "it";
            dataGridView1.Columns[0].ReadOnly = true;
            // cant
            dataGridView1.Columns[1].Visible = true;            // columna visible o no
            dataGridView1.Columns[1].HeaderText = "Cant";    // titulo de la columna
            dataGridView1.Columns[1].Width = 20;                // ancho
            dataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].Name = "cant";
            // articulo
            dataGridView1.Columns[2].Visible = true;            // columna visible o no
            dataGridView1.Columns[2].HeaderText = "Artículo";    // titulo de la columna
            dataGridView1.Columns[2].Width = 70;                // ancho
            dataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Name = "item";
            // nombre del articulo
            dataGridView1.Columns[3].Visible = true;            // columna visible o no
            dataGridView1.Columns[3].HeaderText = "Nombre";    // titulo de la columna
            dataGridView1.Columns[3].Width = 200;                // ancho
            dataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[3].Name = "nombre";
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
            // piedra
            dataGridView1.Columns[6].Visible = true;            // columna visible o no
            dataGridView1.Columns[6].HeaderText = "Deta2";    // titulo de la columna
            dataGridView1.Columns[6].Width = 70;                // ancho
            dataGridView1.Columns[6].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].Name = "piedra";
            // acabado 
            dataGridView1.Columns[7].Visible = true;            // columna visible o no
            dataGridView1.Columns[7].HeaderText = "Acabado";    // titulo de la columna
            dataGridView1.Columns[7].Width = 70;                // ancho
            dataGridView1.Columns[7].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].Name = "descrizionerid";
            // precio
            dataGridView1.Columns[13].Visible = true;            // columna visible o no
            dataGridView1.Columns[13].HeaderText = "Precio"; // titulo de la columna
            dataGridView1.Columns[13].Width = 60;                // ancho
            dataGridView1.Columns[13].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[13].Name = "precio";
            // total
            dataGridView1.Columns[14].Visible = false;
            dataGridView1.Columns[14].Name = "total";
            // tipo nuevo o modif
            //dataGridView1.Columns[15].Visible = false;
            //dataGridView1.Columns[15].Name = "NE";
        }
        private void dataload(string quien)                 // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion de local de ventas ... ok
                const string contaller = "select a.descrizionerid,a.idcodice,a.codigo,b.serie from desc_ven a " +
                    "left JOIN (select serie,sede from series WHERE tipdoc IN ('FT','BV')) b on b.sede=a.idcodice " +
                    "where a.numero=1 AND a.codigo<>'' order by a.idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6));
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de tipo de doc. venta ... ok
                const string conpedido = "select descrizionerid,idcodice from desc_tdv " +
                                       "where numero=1";
                MySqlCommand cmdpedido = new MySqlCommand(conpedido, conn);
                DataTable dtpedido = new DataTable();
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del tipo documento cliente
                const string condoc = "select descrizionerid,idcodice from desc_doc " +
                                       "where numero=1";
                MySqlCommand cmddoc = new MySqlCommand(condoc, conn);
                MySqlDataAdapter dadoc = new MySqlDataAdapter(cmddoc);
                dadoc.Fill(dtdoc);
                foreach (DataRow row in dtdoc.Rows)
                {
                    cmb_tdoc.Items.Add(row.ItemArray[0].ToString());
                    cmb_tdoc.ValueMember = row.ItemArray[1].ToString();
                }
            }
            conn.Close();
        }
        private bool graba()                                // graba cabecera del pedido de clientes
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return retorna;
            }
            retorna = true;
            //
            conn.Close();
            return retorna;
        }
        private bool edita()                                // modifica 
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    retorna = true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en edicion");
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
        private bool anula()                                // anula 
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                retorna = true;
            }
            else
            {
                MessageBox.Show("Se perdió conexión al servidor", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
            return retorna;
        }
        private bool buscont(string cont)                   // busqueda de contrato
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return retorna;
            }
            string consulta = "select a.id,a.contrato,b.idanagrafica,b.razonsocial,c.descrizionerid,a.tipoes,a.fecha " +
                "from contrat a " +
                "left join anag_cli b on b.idanagrafica=a.cliente " +
                "left join desc_alm c on c.idcodice=a.tipoes " +
                "where a.contrato=@cont and a.status<>'ANULAD'";
            MySqlCommand micon = new MySqlCommand(consulta, conn);
            micon.Parameters.AddWithValue("@cont", cont);
            MySqlDataReader dr = micon.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt16(0) > 0)
                {
                    tx_idc.Text = dr.GetString(2);
                    tx_nombre.Text = dr.GetString(3);
                    // aca va el resto ...
                    retorna = true;
                }
                else retorna = false;
            }
            dr.Close();
            conn.Close();
            return retorna;
        }
        private void limpia_ini()                           // limpia e inicializa datos antes y despues de leer y grabar registro
        {
            string modo = Tx_modo.Text;
            limpiar(this);
            limpia_chk();
            limpia_combos();
            limpia_otros();
            Tx_modo.Text = modo;
            //cmb_taller.Enabled = false;
            if (modo != "NUEVO")
            {
                tx_dat_orig.Text = "";
                tx_nomVen.ReadOnly = true;
                tx_status.Text = "";
                tx_dat_estad.Text = "";
                dtp_pedido.Enabled = false;
                tx_idr.Text = "";
            }
            else
            {
                tx_dat_orig.Text = Program.tdauser;
                string axs = string.Format("idcodice='{0}'",tx_dat_orig.Text);
                DataRow[] row = dttaller.Select(axs);
                cmb_taller.SelectedItem = row[0].ItemArray[1].ToString();
                tx_nomVen.Text = Program.vg_nuse;
                dtp_pedido.Value = DateTime.Now;
                tx_serie.ReadOnly = true;
                tx_corre.ReadOnly = true;
                tx_serie.Text = row[0].ItemArray[3].ToString();
            }
        }

        #region botones_de_comando_y_permisos  
        private void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            bt_view.Visible = false;
            Bt_print.Visible = false;
            bt_prev.Visible = false;
            bt_exc.Visible = false;
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
                if (Convert.ToString(row["btn4"]) == "S")               // visualizar ... ok
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir ... ok
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar ... ok
                {
                    this.bt_prev.Visible = true;
                }
                else { this.bt_prev.Visible = false; }
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
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            escribe(this);
            limpia_ini();
            button1.Image = Image.FromFile(img_grab);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            grilladet("NUEVO");
            rb_bienes.Checked = true;
            cmb_tipo.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            escribe(this);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            jalaoc("tx_idr");
            tx_corre.ReadOnly = false;
            //  solo se modifica comentarios
            tx_d_can.ReadOnly = true;
            tx_d_nom.ReadOnly = true;
            tx_d_med.ReadOnly = true;
            tx_coment.Enabled = true;
            tx_coment.ReadOnly = false;
            //
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            jalaoc("tx_idr");
            tx_corre.ReadOnly = false;
            tx_corre.Enabled = true;
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            sololee(this);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;
            limpiar(this);
            tx_corre.Enabled = true;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            jalaoc("tx_idr");
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            if (tx_corre.Text != "")
            {
                setParaCrystal();
            }
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_corre.Text != "")
            {
                setParaCrystal();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "xxx" +
                "" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                wb.Worksheets.Add(dtg, "xxx");
                wb.SaveAs(nombre);
                MessageBox.Show("Archivo generado con exito!");
                this.Close();
            }
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //
        private void Bt_first_Click(object sender, EventArgs e)
        {
            limpiar(this);
            limpia_chk();
            limpia_otros();
            limpia_combos();
            limpia_panel(panel1);
            limpia_panel(panel2);
            limpia_panel(panel3);
            limpia_panel(pan_cli);
            tx_idr_Leave(null, null);
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            string aca = tx_idr.Text;
            limpiar(this);
            limpia_chk();
            limpia_otros();
            limpia_combos();
            limpia_panel(panel1);
            limpia_panel(panel2);
            limpia_panel(panel3);
            limpia_panel(pan_cli);
            tx_idr.Text = lib.goback(nomtab, aca);
            tx_idr_Leave(null, null);
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            string aca = tx_idr.Text;
            limpiar(this);
            limpia_chk();
            limpia_otros();
            limpia_combos();
            limpia_panel(panel1);
            limpia_panel(panel2);
            limpia_panel(panel3);
            limpia_panel(pan_cli);
            tx_idr.Text = lib.gonext(nomtab, aca);
            tx_idr_Leave(null, null);
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            limpiar(this);
            limpia_chk();
            limpia_otros();
            limpia_combos();
            limpia_panel(panel1);
            limpia_panel(panel2);
            limpia_panel(panel3);
            limpia_panel(pan_cli);
            tx_idr.Text = lib.golast(nomtab);
            tx_idr_Leave(null, null);
        }
        #endregion botones;
        // configurador de permisos
        #endregion botones_de_comando_y_permisos

        #region limpiadores_modos
        private void sololee(Form lfrm)
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
        private void sololeepag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
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
            //
            foreach (Control oControls in panel1.Controls)
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
        private void escribe(Form efrm)
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
            tx_status.Enabled = false;
            dtp_pedido.Enabled = false;
        }
        private void escribepag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
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
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
            }
            //
            foreach (Control oControls in panel1.Controls)
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
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
            }
        }
        private static void limpiar(Form ofrm)          // limpia todos los cuadros de texto
        {
            foreach (Control oControls in ofrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if (oControls is Panel)
                {
                    foreach (Control oPan in oControls.Controls)
                    {
                        if (oPan is TextBox)
                        {
                            oControls.Text = "";
                        }
                    }
                }
            }
        }
        private void limpiapag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
            tx_d_can.Text = "";
            tx_d_codi.Text = "";
            tx_d_id.Text = "";
            tx_d_it.Text = "";
            tx_d_mad.Text = "";
            tx_d_med.Text = "";
            tx_d_nom.Text = "";
        }
        private void limpia_chk()
        {
            //checkBox1.Checked = false;
            rb_antic.Checked = false;
            rb_bienes.Checked = false;
            rb_contado.Checked = false;
            rb_credito.Checked = false;
        }
        private void limpia_otros()
        {
            //this.checkBox1.Checked = false;
        }
        private void limpia_combos()
        {
            cmb_taller.SelectedIndex = -1;
            cmb_tipo.SelectedIndex = -1;
            cmb_tdoc.SelectedIndex = -1;
            cmb_plazo.SelectedIndex = -1;
        }
        private void limpia_panel(Panel pan)            // limpia los cuadros de texto solo del panel pasado como parametro
        {
            foreach (Control oControls in pan.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        #endregion limpiadores_modos;

        #region comboboxes
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tipdoc.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tipdoc.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_tdoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tdoc.SelectedValue != null) tx_dat_tdoc.Text = cmb_tdoc.SelectedValue.ToString();
            else tx_dat_tdoc.Text = cmb_tdoc.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        #endregion comboboxes

        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text.Trim() != "" && tx_corre.Text.Trim() == "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_d_codi_Leave(object sender, EventArgs e)
        {
            if (tx_d_codi.Text.Trim().Length != 18)
            {
                MessageBox.Show("La longitud del código no es correcto " +
                    Environment.NewLine + tx_d_codi.Text.Trim().Length.ToString(), "Error en validación", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                tx_d_codi.Text = "";
                return;
            }
            if (tx_d_codi.Text.Substring(10,2) == "XX")
            {
                MessageBox.Show("El taller no es el correcto!", "Error en validación", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                tx_d_codi.Text = "";
                return;
            }
        }
        private void tx_cont_Leave(object sender, EventArgs e)          // valida contrato y jala los datos
        {
            if (Tx_modo.Text == "NUEVO" && tx_cont.Text.Trim() != "")
            {
                try
                {
                    DataTable dt = new DataTable();
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        string consulta = "SELECT a.contratoh,a.item,a.nombre,a.cant,a.medidas,de.descrizione,a.codref,a.piedra,a.precio,a.total,c.cliente," +
                            "ac.tipdoc,ac.RUC,ac.RazonSocial,ac.Direcc1,ac.Direcc2,ac.localidad,ac.Provincia,ac.depart,ac.NumeroTel1,ac.NumeroTel2,ac.EMail " +
                            "FROM detacon a " +
                            "LEFT JOIN desc_est de ON de.IDCodice = a.estado " +
                            "LEFT JOIN contrat c ON c.contrato = a.contratoh " +
                            "LEFT JOIN anag_cli ac ON ac.IDAnagrafica = c.cliente " +
                            "WHERE a.contratoh = @cont";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                da.Fill(dt);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        tx_idc.Text = dt.Rows[0].ItemArray[10].ToString();
                        tx_dat_tdoc.Text = dt.Rows[0].ItemArray[11].ToString();
                        string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                        DataRow[] row = dtdoc.Select(axs);
                        cmb_tdoc.SelectedItem = row[0].ItemArray[1].ToString();
                        tx_ndc.Text = dt.Rows[0].ItemArray[12].ToString();
                        tx_nombre.Text = dt.Rows[0].ItemArray[13].ToString();
                        tx_direc.Text = dt.Rows[0].ItemArray[14].ToString() + " " + dt.Rows[0].ItemArray[15].ToString();
                        tx_dpto.Text = dt.Rows[0].ItemArray[18].ToString();
                        tx_prov.Text = dt.Rows[0].ItemArray[17].ToString();
                        tx_dist.Text = dt.Rows[0].ItemArray[16].ToString();
                        tx_mail.Text = dt.Rows[0].ItemArray[21].ToString();
                        tx_telef1.Text = dt.Rows[0].ItemArray[19].ToString();
                        tx_telef2.Text = dt.Rows[0].ItemArray[20].ToString();
                        // detalle
                        grilladet(Tx_modo.Text);
                        foreach (DataRow data in dt.Rows)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("No existe el contrato!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_cont.Text = "";
                        return;
                    }
                }
                catch(MySqlException ex)
                {
                    MessageBox.Show(ex.Message,"Error en ejecución de código",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
        }
        #endregion leaves;

        #region advancedatagridview

        #endregion

        #region datagridview1 - grilla detalle del doc.venta
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                if (Tx_modo.Text == "EDITAR")
                {
                    tx_d_can.Enabled = true;
                    tx_d_can.ReadOnly = false;
                }
                else
                {
                    tx_d_can.Enabled = false;
                }
                tx_d_nom.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();    //
                tx_d_med.Text = dataGridView1.Rows[e.RowIndex].Cells["medidas"].Value.ToString();   //
                tx_d_can.Text = dataGridView1.Rows[e.RowIndex].Cells["cant"].Value.ToString();      //
                tx_d_id.Text = dataGridView1.Rows[e.RowIndex].Cells["iddetaped"].Value.ToString();  //
                tx_d_codi.Text = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString();     //
                tx_d_mad.Text = dataGridView1.Rows[e.RowIndex].Cells["madera"].Value.ToString();    //
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // si es cualquier modo no nuevo, no deja borrar
            if (Tx_modo.Text != "NUEVO")    // y el usuario esta autorizado
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region botones de grabar y agregar
        private void bt_det_Click(object sender, EventArgs e)
        {
            // validaciones
            if(tx_d_can.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la cantidad", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_can.Focus();
                return;
            }
            if (int.Parse(tx_d_can.Text) <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_can.Focus();
                return;
            }
            if (tx_d_codi.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el código del artículo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_codi.Focus();
                return;
            }
            if (tx_d_id.Text.Trim() == "")  // validamos que el codigo no se repita en la grilla
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (tx_d_codi.Text == dataGridView1.Rows[i].Cells[2].Value.ToString())
                    {
                        MessageBox.Show("Esta repitiendo el código del artículo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_can.Focus();
                        return;
                    }
                }
            }
            if (Tx_modo.Text == "NUEVO")
            {
                // POR DEFECTO, SOLO SE PERMITE UN ITEM POR PEDIDO 18/09/2020 a menos que sea silla kandinski
                // validamos que solo sea un 1 item en detalle a menos que variable [codVar] sea para varios
                if (dataGridView1.Rows.Count != 1)
                {
                    if (tx_d_codi.Text.Substring(0, 4) != codVar || (tx_d_codi.Text.Substring(0, 4) != dataGridView1.Rows[0].Cells[2].Value.ToString().Substring(0, 4)))
                    {
                        MessageBox.Show("No se permite mas items", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                // validamos cant item  validar que la cantidad no sea > cantidad del contrato
                bool pasa = false;
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string busca = "select cant,saldo from detacon where contratoh=@cont and item=@item";
                        using (MySqlCommand micon = new MySqlCommand(busca, conn))
                        {
                            string cod = tx_d_codi.Text.Substring(0, 10) + "XX" + tx_d_codi.Text.Substring(12, 6);
                            micon.Parameters.AddWithValue("@cont", tx_cont.Text.Trim());
                            micon.Parameters.AddWithValue("@item", cod);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                int vs = 0;
                                if (dr.Read())
                                {
                                    vs = dr.GetInt32(1);
                                    if (int.Parse(tx_d_can.Text) > vs)
                                    {
                                        MessageBox.Show("La cantidad pedida es mayor al saldo del contrato!", "Error - corrija",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tx_d_can.Focus();
                                        pasa = false;
                                    }
                                    else
                                    {
                                        pasa = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede validar con el contrato", "Imposible conectarse al servidor",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                if (pasa == true)
                {
                    dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                                tx_d_mad.Text, tx_dat_mad.Text, "", "", tx_d_precio.Text, "N");
                }
            }
            if (Tx_modo.Text == "EDITAR")   // SOLO SE PERMITE EDITAR COMENTARIO DE ITEM 01/10/2020
            {
                if (tx_d_id.Text.Trim() != "")  // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo,total,ne,iddetc
                {
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;    // cant editada > cant grilla? -> saldo=saldo+(dif cant edit - cant grilla)

                    obj.Cells[15].Value = "A";  // registro actualizado
                }
                else
                {
                    MessageBox.Show("No es posible agregar en este modo", "Modo Edición");
                }
                //dtp_fingreso.Checked = false;
                //dtp_fingreso.Value = DateTime.Now;
                limpia_panel(panel1);               // limpia panel1
            }
        }
        private void button1_Click(object sender, EventArgs e)      // graba 
        {

        }
        #endregion

        #region crystal
        private void setParaCrystal()               // genera el set para el reporte de crystal
        {
            pedsclts datos = generareporte();            // pedsclts = dataset de impresion del pedido
            frmvizcpeds visualizador = new frmvizcpeds(datos);      // POR ESO SE CREO ESTE FORM frmvizpeds PARA MOSTRAR AHI. ES MEJOR ASI.  
            visualizador.Show();
        }
        private pedsclts generareporte()             // procedimiento para meter los datos del formulario hacia las tablas del dataset del reporte en crystal
        {
            pedsclts reppedido = new pedsclts();                                    // dataset

            return reppedido;
        }
        #endregion crystal

        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            bt_prev.Enabled = false;
            Bt_print.Enabled = false;
        }
        private void tabuser_Enter(object sender, EventArgs e)
        {
            bt_prev.Enabled = true;
            Bt_print.Enabled = true;
        }
        private void tx_status_TextChanged(object sender, EventArgs e)      // se pone visible si tiene dato
        {
            if (e.ToString().Trim() == "") tx_status.Visible = false;
            else tx_status.Visible = true;
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
