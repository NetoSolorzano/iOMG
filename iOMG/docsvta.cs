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
        static string nomtab = "????";
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
        string MonDeft = "";            // moneda por defecto para los comprobantes
        string v_igv = Program.v_igv;   // porentaje en numero del igv
        string codCanc = "";            // codigo estado cancelado
        string lps = "";                // listado de productos que tienen stock

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
        DataTable dtfp = new DataTable();       // combo para tipo de pago
        DataTable dtpedido = new DataTable();   // tipos documento de venta

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
                    para1 = "items";
                    para2 = "todos";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            tx_d_codi.Text = ayu2.ReturnValue1.ToString();
                            tx_d_nom.Text = ayu2.ReturnValue2.ToString();
                            tx_d_id.Text = ayu2.ReturnValue0.ToString();
                            tx_d_precio.Text = ayu2.ReturnValueA[3];
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
            tx_nomVen.MaxLength = 20;                   // este campo debe grabarse e imprimirse en el ticket
            tx_nombre.MaxLength = 100;                  // ancho del campo razonsocial de la tabla anagrafiche
            tx_direc.MaxLength = 100;                   // ancho del campo direc1 de la tabla anagrafiche
            tx_dpto.MaxLength = 45;                     // ancho del campo depart de la tabla anagrafiche
            tx_prov.MaxLength = 20;                     // ancho del campo provincia de la tabla anagrafiche
            tx_dist.MaxLength = 20;                     // ancho del campo localidad de la tabla anagrafiche
            tx_mail.MaxLength = 50;                     // ancho del campo email de la tabla anagrafiche
            tx_telef1.MaxLength = 15;                   // ancho del campo numeroTel1 de la tabla anagrafiche
            tx_telef2.MaxLength = 15;                   // ancho del campo numerotel2 de la tabla anagrafiche
            tx_numOpe.MaxLength = 25;                   // este campo debe grabarse en todos lados .. referen1 tabla cabfactu
            tx_coment.MaxLength = 240;
            tx_corre.CharacterCasing = CharacterCasing.Upper;
            //
            tx_d_nom.MaxLength = 90;                    // ancho del campo nombr de la maestra de items
            tx_d_antic.Text = letiden;
            tx_d_antic.MaxLength = 90;                  // 
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
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "clientes") tipede = row["valor"].ToString().Trim();         // 
                        if (row["campo"].ToString() == "anticipos" && row["param"].ToString() == "glosa") letiden = row["valor"].ToString().Trim();         // glosa de anticipos
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "codAnu") estanu = row["valor"].ToString().Trim();         // codigo estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "Anulado") nomanu = row["valor"].ToString().Trim();        // nombre estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "cancelado") codCanc = row["valor"].ToString().Trim();        // codigo estado cancelado
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") MonDeft = row["valor"].ToString().Trim();          // moneda por defecto
                        if (row["campo"].ToString() == "items" && row["param"].ToString() == "stock") lps = row["valor"].ToString().Trim();                 // tipos de muebles que se hacen contrato
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
            if (modo == "NUEVO") dataGridView1.ColumnCount = 10;
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
            dataGridView1.Columns[2].Visible = false;            // columna visible o no
            dataGridView1.Columns[2].HeaderText = "Artículo";    // titulo de la columna
            dataGridView1.Columns[2].Width = 70;                // ancho
            dataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Name = "item";
            // nombre del articulo
            dataGridView1.Columns[3].Visible = true;            // columna visible o no
            dataGridView1.Columns[3].HeaderText = "Nombre";    // titulo de la columna
            dataGridView1.Columns[3].Width = 400;                // ancho
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
            dataGridView1.Columns[8].Visible = true;            // columna visible o no
            dataGridView1.Columns[8].HeaderText = "Precio"; // titulo de la columna
            dataGridView1.Columns[8].Width = 60;                // ancho
            dataGridView1.Columns[8].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].Name = "precio";
            // total
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[9].Name = "total";
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
                const string contaller = "select a.descrizionerid,a.idcodice,a.codigo,b.serie,b.dir_pe,b.ubigeo from desc_ven a " +
                    "left JOIN (select serie,sede,dir_pe,ubigeo from series WHERE tipdoc IN ('FT','BV')) b on b.sede=a.idcodice " +
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
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[0].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del tipo documento cliente
                const string condoc = "select descrizionerid,idcodice,codigo from desc_doc " +
                                       "where numero=1";
                MySqlCommand cmddoc = new MySqlCommand(condoc, conn);
                MySqlDataAdapter dadoc = new MySqlDataAdapter(cmddoc);
                dadoc.Fill(dtdoc);
                foreach (DataRow row in dtdoc.Rows)
                {
                    cmb_tdoc.Items.Add(row.ItemArray[0].ToString());
                    //cmb_tdoc.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de forma de pago
                const string confpa = "select descrizionerid,idcodice from desc_mpa where numero=1";
                using (MySqlCommand my = new MySqlCommand(confpa, conn))
                {
                    using (MySqlDataAdapter dafp = new MySqlDataAdapter(my))
                    {
                        dafp.Fill(dtfp);
                        foreach (DataRow row in dtfp.Rows)
                        {
                            cmb_plazo.Items.Add(row.ItemArray[0].ToString());
                        }
                    }
                }
            }
            conn.Close();
        }
        private bool graba()                                // graba cabecera del comprobante
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                // conexion a rapifact para leer el correlativo del comprobante
                //
                tx_corre.Text = "99004144";
                //

                //if (tx_tipcam.Text == "") tx_tipcam.Text = "0";
                decimal fletMN = 0;
                decimal subtMN = 0;
                decimal igvtMN = 0;
                if (tx_dat_mone.Text != MonDeft)
                {
                    // falta ver tema de dolares .... 29/09/2022
                }
                else
                {
                    fletMN = Math.Round(decimal.Parse(tx_valor.Text), 3);
                    subtMN = Math.Round(decimal.Parse(tx_bruto.Text), 3);
                    igvtMN = Math.Round(decimal.Parse(tx_igv.Text), 3);
                }
                // ACTUALIZAMOS LOS DTOS DEL CLIENTE (anagrafiche) CADA VEZ QUE SE GRABA UN REGISTRO .. triger
                //
                string inserta = "insert into cabfactu (" +
                    "fechope,martdve,tipdvta,serdvta,numdvta,ticltgr,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt,ubigclt,corrclt,teleclt,telemsg," +
                    "locorig,dirorig,ubiorig,obsdvta,canfidt,canbudt,mondvta,tcadvta,subtota,igvtota,porcigv,totdvta,totpags,saldvta,estdvta,frase01," +
                    "tipoclt,m1clien,tippago,impreso,codMN,subtMN,igvtMN,totdvMN,pagauto,tipdcob,idcaja,plazocred,porcendscto,valordscto," +
                    "referen1,ubipdest,conPago,contrato,vendedor,muebles," +
                    "verApp,userc,fechc,diriplan4,diripwan4,netbname) values (" +
                    "@fechop,@mtdvta,@ctdvta,@serdv,@numdv,@tcdvta,@tdcrem,@ndcrem,@nomrem,@dircre,@dptocl,@provcl,@distcl,@ubicre,@mailcl,@telec1,@telec2," +
                    "@ldcpgr,@didegr,@ubdegr,@obsprg,@canfil,@totcpr,@monppr,@tcoper,@subpgr,@igvpgr,@porcigv,@totpgr,@pagpgr,@salxpa,@estpgr,@frase1," +
                    "@ticlre,@m1clte,@tipacc,@impSN,@codMN,@subMN,@igvMN,@totMN,@pagaut,@tipdco,@idcaj,@plazc,@pordesc,@valdesc," +
                    "@refer,@updest,@conpag,@cont,@vende,@mueb," +
                    "@verApp,@asd,now(),@iplan,@ipwan,@nbnam)";
                using (MySqlCommand micon = new MySqlCommand(inserta, conn))
                {
                    micon.Parameters.AddWithValue("@fechop", dtp_pedido.Text.Substring(6, 4) + "-" + dtp_pedido.Text.Substring(3, 2) + "-" + dtp_pedido.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@mtdvta", cmb_tipo.Text.Substring(0, 1));
                    micon.Parameters.AddWithValue("@ctdvta", tx_dat_tipdoc.Text);
                    micon.Parameters.AddWithValue("@serdv", tx_serie.Text);
                    micon.Parameters.AddWithValue("@numdv", tx_corre.Text);
                    micon.Parameters.AddWithValue("@tcdvta", (tx_cont.Text.Trim() == "") ? "2" : "1");  // comprob. sin contrato=2 | con contrato=1
                    micon.Parameters.AddWithValue("@tdcrem", tx_dat_tdoc.Text);
                    micon.Parameters.AddWithValue("@ndcrem", tx_ndc.Text);
                    micon.Parameters.AddWithValue("@nomrem", tx_nombre.Text);
                    micon.Parameters.AddWithValue("@dircre", tx_direc.Text);
                    micon.Parameters.AddWithValue("@dptocl", tx_dpto.Text);
                    micon.Parameters.AddWithValue("@provcl", tx_prov.Text);
                    micon.Parameters.AddWithValue("@distcl", tx_dist.Text);
                    micon.Parameters.AddWithValue("@ubicre", "");
                    micon.Parameters.AddWithValue("@mailcl", tx_mail.Text);
                    micon.Parameters.AddWithValue("@telec1", tx_telef1.Text);
                    micon.Parameters.AddWithValue("@telec2", tx_telef2.Text);
                    micon.Parameters.AddWithValue("@ldcpgr", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@didegr", tx_dir_pe.Text);                   // direccion local de ventas
                    micon.Parameters.AddWithValue("@ubdegr", "");                               // ubigeo origen
                    micon.Parameters.AddWithValue("@obsprg", tx_coment.Text);
                    micon.Parameters.AddWithValue("@canfil", tx_tfil.Text);                     // cantidad de filas de detalle
                    micon.Parameters.AddWithValue("@totcpr", "0");                              // total bultos
                    micon.Parameters.AddWithValue("@monppr", tx_dat_mone.Text);
                    micon.Parameters.AddWithValue("@tcoper", "0");                              // TIPO DE CAMBIO
                    micon.Parameters.AddWithValue("@subpgr", tx_bruto.Text);                     // sub total
                    micon.Parameters.AddWithValue("@igvpgr", tx_igv.Text);                      // igv
                    micon.Parameters.AddWithValue("@porcigv", v_igv);                           // porcentaje en numeros de IGV
                    micon.Parameters.AddWithValue("@totpgr", tx_valor.Text);                    // total inc. igv
                    micon.Parameters.AddWithValue("@pagpgr", "0");      // todos los comprobantes se emiten contado
                    micon.Parameters.AddWithValue("@salxpa", "0");      // y con la plata en mano ... asi que los comprobantes nacen cancelados
                    micon.Parameters.AddWithValue("@estpgr", codCanc);                          // estado del comprobante
                    micon.Parameters.AddWithValue("@frase1", "");                               // no hay nada que poner 19/11/2020
                    micon.Parameters.AddWithValue("@ticlre", "1");                              // tipo de cliente credito o contado => TODOS SON CONTADO=1
                    micon.Parameters.AddWithValue("@m1clte", "");
                    micon.Parameters.AddWithValue("@tipacc", tx_dat_plazo.Text);                   // pago del documento x defecto si nace la fact pagada
                    micon.Parameters.AddWithValue("@impSN", "S");                               // impreso? S, N
                    micon.Parameters.AddWithValue("@codMN", MonDeft);               // codigo moneda local
                    micon.Parameters.AddWithValue("@subMN", subtMN);
                    micon.Parameters.AddWithValue("@igvMN", igvtMN);
                    micon.Parameters.AddWithValue("@totMN", fletMN);
                    micon.Parameters.AddWithValue("@pagaut", "S");                  // todos los comprobantes nacen pagados
                    micon.Parameters.AddWithValue("@tipdco", "");
                    micon.Parameters.AddWithValue("@idcaj", "0");                   // aca no manejamos caja
                    micon.Parameters.AddWithValue("@plazc", "");                    // aca no hay plazo  de credito...todo es contado
                    micon.Parameters.AddWithValue("@pordesc", "0");                 // los precios ya tienen descuento incluido, el operador pone precio
                    micon.Parameters.AddWithValue("@valdesc", "0");                 // los precios ya tienen descuento incluido, el operador pone precio
                    micon.Parameters.AddWithValue("@refer", tx_numOpe.Text);
                    micon.Parameters.AddWithValue("@updest", "");
                    micon.Parameters.AddWithValue("@conpag", "1");                  // todos son contado
                    micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                    micon.Parameters.AddWithValue("@vende", tx_nomVen.Text);
                    micon.Parameters.AddWithValue("@mueb", tx_prdsCont.Text);
                    micon.Parameters.AddWithValue("@verApp", "");
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@iplan", lib.iplan());
                    micon.Parameters.AddWithValue("@ipwan", "");
                    micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                    micon.ExecuteNonQuery();
                }
                // detalle
                int fila = 1;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() != "")
                    {
                        string inserd2 = "update detfactu set " +
                            "contrato=@cont,cantbul=@bult,codprod=@citem,unimedp=@unim,descpro=@desc,pesogro=@peso,medidas=@medid,madera=@mader,acabado=@acaba," +
                            "codmad=@codm,detpied=@detp,codMN=@cmnn,precio=@pret,totalMN=@tgrmn,pagauto=@pagaut,estadoser=@esta " +
                            "where tipdocvta=@tdv and serdvta=@sdv and numdvta=@cdv and filadet=@fila";
                        using (MySqlCommand micon = new MySqlCommand(inserd2, conn))
                        {
                            micon.CommandTimeout = 60;
                            micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                            micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                            micon.Parameters.AddWithValue("@cdv", tx_corre.Text);
                            micon.Parameters.AddWithValue("@fila", fila);
                            micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                            micon.Parameters.AddWithValue("@bult", row.Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@citem", row.Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@unim", "");
                            micon.Parameters.AddWithValue("@desc", row.Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@peso", "0");
                            micon.Parameters.AddWithValue("@medid", row.Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@mader", row.Cells[5].Value.ToString().PadRight(2).Substring(0,1));
                            micon.Parameters.AddWithValue("@acaba", row.Cells[7].Value.ToString());
                            micon.Parameters.AddWithValue("@codm", row.Cells[5].Value.ToString());
                            micon.Parameters.AddWithValue("@detp", row.Cells[6].Value.ToString());
                            micon.Parameters.AddWithValue("@cmnn", MonDeft);
                            micon.Parameters.AddWithValue("@pret", decimal.Parse(row.Cells[8].Value.ToString()));
                            micon.Parameters.AddWithValue("@tgrmn", decimal.Parse(row.Cells[9].Value.ToString()));
                            micon.Parameters.AddWithValue("@pagaut", "S");
                            micon.Parameters.AddWithValue("@esta", codCanc);        // todos los comprob. nacen cancelados
                            micon.ExecuteNonQuery();
                            fila += 1;
                            //
                            retorna = true;         // no hubo errores!
                        }
                    }
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
        private bool busclte(string doc, string num)        // busqueda de cliente
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
            string consulta = "SELECT idanagrafica,razonsocial,direcc1,direcc2,localidad,provincia,depart,numerotel1,numerotel2,email " +
                "FROM anag_cli WHERE tipdoc=@doc and ruc=@num";
            MySqlCommand micon = new MySqlCommand(consulta, conn);
            micon.Parameters.AddWithValue("@doc", doc);
            micon.Parameters.AddWithValue("@num", num);
            MySqlDataReader dr = micon.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt16(0) > 0)
                {
                    tx_idc.Text = dr.GetString(0);
                    tx_nombre.Text = dr.GetString(1);
                    tx_direc.Text = dr.GetString(2);
                    tx_dpto.Text = dr.GetString(6);
                    tx_prov.Text = dr.GetString(5);
                    tx_dist.Text = dr.GetString(4);
                    tx_mail.Text = dr.GetString(9);
                    tx_telef1.Text = dr.GetString(7);
                    tx_telef2.Text = dr.GetString(8);
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
            limpia_panel(pan_cli);
            limpia_panel(panel2);
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
                cmb_taller_SelectionChangeCommitted(null,null);
                tx_nomVen.Text = asd; // Program.vg_nuse;
                tx_nomVen.ReadOnly = false;
                dtp_pedido.Value = DateTime.Now;
                tx_serie.ReadOnly = true;
                tx_corre.ReadOnly = true;
            }
        }
        private void jala_cont(string conti)                // jala datos del contrato
        {
            try
            {
                DataTable dt = new DataTable();
                dataGridView1.Rows.Clear();
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
                        micon.Parameters.AddWithValue("@cont", conti);
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
                    cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();
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
                    int cnt = 1;
                    double toti = 0;
                    foreach (DataRow data in dt.Rows)
                    {
                        dataGridView1.Rows.Add(cnt, data.ItemArray[3].ToString(), data.ItemArray[1].ToString(), data.ItemArray[2].ToString(),
                            data.ItemArray[4].ToString(), data.ItemArray[6].ToString(), data.ItemArray[7].ToString(), data.ItemArray[5].ToString(),
                            data.ItemArray[8].ToString(), data.ItemArray[9].ToString());
                        cnt += 1;
                        toti = toti + double.Parse(data.ItemArray[9].ToString());
                    }
                    tx_valor.Text = toti.ToString("#0.00");
                    tx_bruto.Text = (toti / 1.18).ToString("#0.00");
                    tx_igv.Text = (toti - (toti / 1.18)).ToString("#0.00");
                    //
                    if (rb_antic.Checked == true)
                    {
                        toti = 0;
                        tx_d_antic.Text = tx_d_antic.Text + " " + tx_cont.Text;
                        tx_valor.Text = toti.ToString("#0.00");
                        tx_bruto.Text = (toti / 1.18).ToString("#0.00");
                        tx_igv.Text = (toti - (toti / 1.18)).ToString("#0.00");
                        tx_coment.Text = "*** Comprobante por antipo ***" + tx_coment.Text.Trim();
                    }
                }
                else
                {
                    MessageBox.Show("No existe el contrato!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_cont.Text = "";
                    return;
                }
                dt.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en ejecución de código", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private bool valProdCont()                          // busca productos de stock, grandes que puedan tener contrato
        {
            bool retorna = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (lps.Contains(row.Cells[2].Value.ToString().Substring(0, 1)))
                    {
                        retorna = true;
                    }
                }
            }
            return retorna;
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
            cmb_taller.Enabled = false;
            limpia_ini();
            button1.Image = Image.FromFile(img_grab);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            grilladet("NUEVO");
            rb_bienes.Checked = true;
            rb_bienes.PerformClick();         // rb_contado_Click(null, null);
            rb_contado.Checked = true;
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
            tx_bruto.ReadOnly = true;
            tx_igv.ReadOnly = true;
            tx_valor.ReadOnly = true;
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
            if (cmb_tdoc.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_tdoc.Text);
                DataRow[] row = dtdoc.Select(axs);
                tx_dat_tdoc.Text = row[0].ItemArray[1].ToString();
            }
        }
        private void cmb_plazo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_plazo.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_plazo.Text);
                DataRow[] row = dtfp.Select(axs);
                tx_dat_plazo.Text = row[0].ItemArray[1].ToString();
            }
        }
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (true)   // cmb_taller.SelectedIndex > -1
            {
                string axs = string.Format("idcodice='{0}'", tx_dat_orig.Text);
                DataRow[] row = dttaller.Select(axs);
                cmb_taller.SelectedItem = row[0].ItemArray[1].ToString();
                tx_dir_pe.Text = row[0].ItemArray[4].ToString();
                tx_serie.Text = row[0].ItemArray[3].ToString();
            }
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
        private void tx_cont_Leave(object sender, EventArgs e)                // valida contrato y jala los datos
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (rb_antic.Checked == true && tx_cont.Text.Trim() == "")
                {
                    //MessageBox.Show("Si es anticipo, debe seleccionar un contrarto","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    //rb_antic.Checked = false;
                    return;
                }
                if (tx_cont.Text.Trim() != "")
                {
                    jala_cont(tx_cont.Text);
                    //if (rb_antic.Checked == true) tx_d_valAntic.Focus();
                }
            }
        }
        private void valDocClte_Leave(object sender, EventArgs e)             // validamos el documento del cliente
        {
            // según que tipo de comprobante vamos a generar .. actuamos
            if (Tx_modo.Text == "NUEVO" && tx_ndc.Text.Trim() != "")
            {
                // validaciones básicas por tipo de documento 
                string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                DataRow[] row = dtdoc.Select(axs);
                if (row[0].ItemArray[2].ToString() != tx_ndc.Text.Trim().Length.ToString())
                {
                    MessageBox.Show("Cantidad de dígitos incorrecto","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    tx_ndc.Focus();
                    return;
                }
                if (rb_bienes.Checked == true)  // pago total vta directa o anticipo de vta con contrato
                {
                    // Aca no hay F1  ... acá vamos defrente con el número
                    if (tx_ndc.Text.Trim() != "" && tx_dat_tdoc.Text != "")
                    {
                        // primero buscamos en la base de clientes del sistema
                        if (busclte(tx_dat_tdoc.Text,tx_ndc.Text) == false)
                        {
                            // si no hay Y SI DOCUMENTO ES RUC O DNI, vamos al conector a buscarlo por ahí
                            string[] biene = lib.conectorSolorsoft(cmb_tdoc.Text.ToUpper().Trim(), tx_ndc.Text);
                            if (biene[0] == "")
                            {
                                var aa = MessageBox.Show(" No encontramos el documento en ningún registro. " + Environment.NewLine +
                                    " Deberá ingresarlo manualmente si esta seguro(a) " + Environment.NewLine + 
                                    " de la validez del número y documento. " + Environment.NewLine +
                                    "" + Environment.NewLine +
                                    "Confirma que desea ingresarlo manualmente?","Atención",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (aa == DialogResult.No)
                                {
                                    tx_ndc.Text = "";
                                    tx_ndc.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                tx_nombre.Text = biene[0];   // razon social
                                //biene[1];                    // ubigeo
                                tx_direc.Text = biene[2];    // direccion
                                tx_dpto.Text = biene[3];     // departamento
                                tx_prov.Text = biene[4];     // provincia
                                tx_dist.Text = biene[5];     // distrito
                                //biene[6]                      // estado del contrib.
                                //biene[7]
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un tipo de documento y" + Environment.NewLine +
                            "escriba el número correspondiente","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        cmb_tdoc.Focus();
                        return;
                    }
                }
                if (rb_antic.Checked == true)   // 2do pago parcial o cancelatorio de un contrato
                {
                    if (tx_ndc.Text.Trim() != "" && tx_dat_tdoc.Text != "")
                    {
                        if (busclte(tx_dat_tdoc.Text, tx_ndc.Text) == false)
                        {
                            var aaa = MessageBox.Show("No encontramos el documento en la B.D." + Environment.NewLine +
                                "Confirma que desea generar el comprobante?","Atención",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                            if (aaa == DialogResult.Yes)
                            {
                                string[] biene = lib.conectorSolorsoft(cmb_tdoc.Text.ToUpper().Trim(), tx_ndc.Text);
                                if (biene[0] == "")
                                {
                                    var aa = MessageBox.Show(" No encontramos los datos en ningún registro. " + Environment.NewLine +
                                        " Deberá ingresarlo manualmente si esta seguro(a) " + Environment.NewLine +
                                        " de la validez del número y documento. " + Environment.NewLine +
                                        "" + Environment.NewLine +
                                        "Confirma que desea ingresarlo manualmente?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aa == DialogResult.No)
                                    {
                                        tx_ndc.Text = "";
                                        tx_ndc.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    tx_nombre.Text = biene[0];   // razon social
                                    //biene[1];                    // ubigeo
                                    tx_direc.Text = biene[2];    // direccion
                                    tx_dpto.Text = biene[3];     // departamento
                                    tx_prov.Text = biene[4];     // provincia
                                    tx_dist.Text = biene[5];     // distrito
                                }
                            }
                            else
                            {
                                tx_ndc.Text = "";
                                tx_ndc.Focus();
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un tipo de documento", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_tdoc.Focus();
                        return;
                    }
                }
            }
        }
        private void tx_d_valAntic_Leave(object sender, EventArgs e)
        {

        }
        #endregion leaves;

        #region radio_buttons
        private void rb_bienes_Click(object sender, EventArgs e)
        {
            if (rb_bienes.Checked == true)
            {
                // ocultamos objetos del panel1, menos el boton
                tx_d_antic.Visible = false;
                tx_d_valAntic.Visible = false;
                //
                tx_d_it.Visible = true;
                tx_d_can.Visible = true;
                tx_d_codi.Visible = true;
                tx_d_nom.Visible = true;
                tx_d_med.Visible = true;
                tx_d_mad.Visible = true;
                tx_d_precio.Visible = true;
                //
                lb_cont.Visible = false;
                tx_cont.Visible = false;
            }
        }
        private void rb_antic_Click(object sender, EventArgs e)
        {
            if (rb_antic.Checked == true)
            {
                // ocultamos objetos del panel1, menos el boton 
                tx_d_it.Visible = false;
                tx_d_can.Visible = false;
                tx_d_codi.Visible = false;
                tx_d_nom.Visible = false;
                tx_d_med.Visible = false;
                tx_d_mad.Visible = false;
                tx_d_precio.Visible = false;
                //
                tx_d_antic.Left = 28;
                tx_d_antic.Top = 5;
                tx_d_antic.Width = 700;
                tx_d_antic.Height = 40;
                tx_d_antic.Multiline = true;
                tx_d_antic.Visible = true;
                //
                tx_d_valAntic.Left = 728;
                tx_d_valAntic.Top = 5;
                tx_d_valAntic.Height = 40;
                tx_d_valAntic.Multiline = true;
                tx_d_valAntic.Visible = true;
                //
                if (Tx_modo.Text == "NUEVO")
                {
                    lb_cont.Visible = true;
                    tx_cont.Visible = true;
                    tx_cont.Focus();
                }
            }
        }
        private void rb_contado_Click(object sender, EventArgs e)
        {
            if (rb_contado.Checked == true)
            {
                if (Tx_modo.Text == "NUEVO")
                {
                    tx_cuotas.ReadOnly = true;
                    cmb_plazo.Enabled = false;
                }
            }
        }
        private void rb_credito_Click(object sender, EventArgs e)
        {
            if (rb_credito.Checked == true)
            {
                if (Tx_modo.Text == "NUEVO")
                {
                    tx_cuotas.ReadOnly = false;
                    cmb_plazo.Enabled = true;
                }
            }
        }
        #endregion

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
            else
            {
                var aa = MessageBox.Show("Confirma que desea borrar el artículo?","Atención",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    int i = e.Row.Index;
                    double vb = double.Parse(dataGridView1.Rows[i].Cells["total"].Value.ToString());

                    double tv = 0;
                    double.TryParse(tx_valor.Text, out tv);

                    tx_valor.Text = (tv - vb).ToString("#0.00");
                    tx_bruto.Text = ((tv - vb) / 1.18).ToString("#0.00");
                    tx_igv.Text = ((tv - vb) - ((tv - vb) / 1.18)).ToString("#0.00");

                    tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
                }
            }
        }
        #endregion

        #region botones de grabar y agregar
        private void bt_det_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" && rb_antic.Checked == true)
            {
                if (tx_d_valAntic.Text != "")
                {
                    double ntoti = 0;
                    double.TryParse(tx_d_valAntic.Text, out ntoti);
                    if (ntoti > 0)
                    {
                        tx_valor.Text = ntoti.ToString("#0.00");
                        tx_bruto.Text = (ntoti / 1.18).ToString("#0.00");
                        tx_igv.Text = (ntoti - (ntoti / 1.18)).ToString("#0.00");
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese el valor del anticipo","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    rb_antic.Focus();
                    return;
                }
            }
            if (Tx_modo.Text == "NUEVO" && rb_bienes.Checked == true)
            {
                if (tx_d_precio.Text != "")
                {
                    double tv = 0;
                    double.TryParse(tx_valor.Text, out tv);

                    double ntoti = double.Parse(tx_d_precio.Text);
                    if (ntoti > 0)
                    {
                        _ = dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                                    tx_d_mad.Text, tx_dat_mad.Text, "", string.Format("{0:#0.00}", ntoti.ToString("#0.00")), ntoti.ToString("#0.00"), "N");

                        tx_valor.Text = (ntoti + tv).ToString("#0.00");
                        tx_bruto.Text = ((ntoti + tv) / 1.18).ToString("#0.00");
                        tx_igv.Text = ((double.Parse(tx_valor.Text)) - ((double.Parse(tx_valor.Text)) / 1.18)).ToString("#0.00");

                        limpia_panel(panel1);
                    }
                    else
                    {
                        MessageBox.Show("El precio debe ser mayor a cero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_precio.Focus();
                        return;
                    }
                }
                else 
                {
                    MessageBox.Show("Ingrese el precio de venta", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_d_precio.Focus();
                    return;
                }

            }
            tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
        }
        private void button1_Click(object sender, EventArgs e)      // graba, anula
        {
            // validaciones generales
            if (tx_dat_tipdoc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de documento","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                cmb_tipo.Focus();
                return;
            }
            if (tx_dat_tdoc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione un cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_tdoc.Focus();
                return;
            }
            if (tx_ndc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione un cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_ndc.Focus();
                return;
            }
            if (dataGridView1.Rows.Count < 2)
            {
                MessageBox.Show("Ingrese al menos un producto", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_d_can.Focus();
                return;
            }
            if (tx_dat_plazo.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de pago", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_plazo.Focus();
                return;
            }

            if (Tx_modo.Text == "NUEVO")
            {
                // validaciones antes de grabar nuevo
                // verificamos si el comprobante tiene items "grandes" que podrían tener contrato ... estos se deben grabar el pago en la tabla pagamenti
                if (valProdCont() == true) tx_prdsCont.Text = "S";
                else tx_prdsCont.Text = "N";

                var aa = MessageBox.Show(" Confirma que desea CREAR " + Environment.NewLine +
                    "el comprobante?","Confirme por favor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    graba();
                    if (tx_prdsCont.Text == "S")
                    {
                        aa = MessageBox.Show("Desea generar contrato relacionado al" + Environment.NewLine +
                            "presente comprobante?","Confirme por favor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            contratos ncont = new contratos();
                            ncont.Show();
                        }
                    }
                }
                else return;
            }
            if (Tx_modo.Text == "ANULAR")
            {
                // validaciones antes de anular

                var aa = MessageBox.Show(" Confirma que desea ANULAR " + Environment.NewLine +
                    "el comprobante?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    anula();
                }
                else return;
            }
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
