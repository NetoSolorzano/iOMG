using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace iOMG
{
    public partial class notcred : Form
    {
        static string nomform = "notcred";      // nombre del formulario
        string asd = iOMG.Program.vg_user;          // usuario conectado al sistema
        string colback = iOMG.Program.colbac;       // color de fondo
        string colpage = iOMG.Program.colpag;       // color de los pageframes
        string colgrid = iOMG.Program.colgri;       // color de las grillas
        string colstrp = iOMG.Program.colstr;       // color del strip
        static string nomtab = "cabnotascd";
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
        string letiden = "";            // glosa de anticipos
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
        string MonTodas = "";           // variable para determinar si van todas las monedas o solo soles, S=soles, T=todas
        string v_igv = Program.v_igv;   // porentaje en numero del igv
        string codCanc = "";            // codigo estado cancelado
        string lps = "";                // listado de productos que tienen stock
        string v_impTK = "";            // nombre de la impresora de tickets facturacion electrónica
        string otro = "";               // ruta y nombre del png código QR
        string logoclt = "";            // 
        string codfact = "";            // codigo de tipo de documento Factura
        string codbole = "";            // codigo de tipo de documento boleta
        string vtc_dni = "";            // codigo tipo documento dni
        string vtc_ruc = "";            // codigo tipo documento ruc
        string tipdo = "";              // CODIGO SUNAT tipo de documento de venta
        string tipoDocEmi = "";         // CODIGO SUNAT tipo de documento RUC/DNI
        string leydet1 = "";            // leyenda de detraccion
        string leydet2 = "";            // leyenda de la cuenta
        string restexto = "xxx";        // texto resolucion sunat autorizando prov. fact electronica
        string autoriz = "";            // resolucion de autorizacion sunat
        string despe2 = "";             // texto despedida en la impresion
        string valdirec = "";           // valor limite maximo para tener boletas sin direccion
        string tpcontad = "";           // codigo tipo de pago contado efectivo
        string estman = "";             // estados que se pueden seleccionar manualmente
        int indant = -1;                // indice anterior al cambio en el combobox de estado
        string v_liav = "";             // letra o caracter inicial indicativo de articulos varios vta directa sin stock
        string v_cnprd = "";            // Se puede cambiar nombres de items de prods. catalogo? S=si, N=no
        string itemSer = "";            // items (capit) de comprobantes de servicios
        string cliente = Program.cliente;    // razon social para los reportes
        string v_tnotanu = "";          // tipo de nota de credito por Anulacion
        string v_tnotdsc = "";          // tipo de nota de credito por Descuento Global
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
        DataTable dttaller = new DataTable();   // combo local de ventas
        DataTable dtdoc = new DataTable();      // combo tipo doc cliente
        DataTable dtpedido = new DataTable();   // tipos documento de venta
        DataTable dtmon = new DataTable();      // monedas
        DataTable dtnota = new DataTable();     // tipos de notas de credito

        public notcred()
        {
            InitializeComponent();
        }
        private void notcred_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1)
            {
                if (tx_ndc.Focused == true && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
                {
                    //
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void notcred_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilladet("NUEVO");
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            //Bt_print.Enabled = false;
            bt_prev.Enabled = false;
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
            tx_coment.MaxLength = 240;
            tx_numdvta.CharacterCasing = CharacterCasing.Upper;
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
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@clie,@dvta,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@clie", "clients");
                micon.Parameters.AddWithValue("@dvta", "docsvta");
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
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento")
                        {
                            if (row["param"].ToString() == "ruc") vtc_ruc = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "dni") vtc_dni = row["valor"].ToString().Trim();
                        }
                    }
                    if (row["formulario"].ToString() == "docsvta")
                    {
                        //if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "clientes") tipede = row["valor"].ToString().Trim();         // 
                        //if (row["campo"].ToString() == "anticipos" && row["param"].ToString() == "glosa") letiden = row["valor"].ToString().Trim();         // glosa de anticipos
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "codAnu") estanu = row["valor"].ToString().Trim();         // codigo estado anulado
                        //if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "Anulado") nomanu = row["valor"].ToString().Trim();        // nombre estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "cancelado") codCanc = row["valor"].ToString().Trim();     // codigo estado cancelado
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") MonDeft = row["valor"].ToString().Trim();          // moneda por defecto
                        //if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "todas") MonTodas = row["valor"].ToString().Trim();          // moneda por defecto
                        //if (row["campo"].ToString() == "items" && row["param"].ToString() == "stock") lps = row["valor"].ToString().Trim();                 // tipos de muebles que se hacen contrato
                        //if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "nomImTK") v_impTK = row["valor"].ToString().Trim();       // nombre de la impresora de tickets
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "factura") codfact = row["valor"].ToString().Trim();       // codigo tipo doc factura
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "boleta") codbole = row["valor"].ToString().Trim();       // codigo tipo doc boleta
                        //if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "leyen1") leydet1 = row["valor"].ToString().Trim();           // 
                        //if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "leyen2") leydet2 = row["valor"].ToString().Trim();           // 
                        //if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "restex") restexto = row["valor"].ToString().Trim();       // texto resolucion sunat autorizando prov. fact electronica
                        //if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "resAut") autoriz = row["valor"].ToString().Trim();        //
                        //if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "desped") despe2 = row["valor"].ToString().Trim();         // 
                        //if (row["campo"].ToString() == "documento" && row["param"].ToString() == "valdirec") valdirec = row["valor"].ToString().Trim();     // monto limite para obligar a tener direcion en boleta
                        //if (row["campo"].ToString() == "documento" && row["param"].ToString() == "codefect") tpcontad = row["valor"].ToString().Trim();     // codigo tipo de documento efectivo contado
                        //if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ciavss") v_liav = row["valor"].ToString().Trim();         // letra o caracter inicial indicativo de articulos varios vta directa sin stock
                        //if (row["campo"].ToString() == "documento" && row["param"].ToString() == "camnomb") v_cnprd = row["valor"].ToString().Trim();       // Se puede cambiar nombres de items de prods. catalogo? S=si, N=no
                        //if (row["campo"].ToString() == "servicios" && row["param"].ToString() == "items") itemSer = row["valor"].ToString().Trim();       // Items para comprobantes de servicios
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "ctipnot" && row["param"].ToString() == "anulacion") v_tnotanu = row["valor"].ToString().Trim();         // tipo nota anulacion
                        if (row["campo"].ToString() == "ctipnot" && row["param"].ToString() == "dsctoGlob") v_tnotdsc = row["valor"].ToString().Trim();         // tipo nota descuento
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
            string parte = "";
            string jala = "SELECT id,fechope,martnot,tipnota,sernota,numnota,tipdvta,serdvta,numdvta,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt," +
                "ubigclt,corrclt,teleclt,locorig,dirorig,ubiorig,obsnota,mondvta,tcadvta,subtota,igvtota,porcigv,totnota,totdvta,saldvta,subtMN," +
                "igvtMN,totdvMN,codMN,estnota,frase01,impreso,canfidt,tipncred,vendedor,contrato " +
                "FROM cabnotascd where ";
            if (campo == "tx_idr" && tx_idr.Text != "" && tx_numdvta.Text.Trim() == "")
            {
                if (Tx_modo.Text != "NUEVO")
                {
                    parte = "id=@idr";
                }
            }
            if (campo == "tx_corre")
            {
                if (Tx_modo.Text != "NUEVO")
                {
                    parte = "sernota=@sdv and numnota=@ndv";
                }
            }
            jala = jala + parte;
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (MySqlCommand micon = new MySqlCommand(jala, conn))
                    {
                        if (parte == "id=@idr") micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                        else
                        {
                            //micon.Parameters.AddWithValue("@tdv", tx_dat_tipnot.Text);
                            micon.Parameters.AddWithValue("@sdv", tx_sernot.Text);
                            micon.Parameters.AddWithValue("@ndv", tx_numnot.Text);
                        }
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            /*
                martnot,tipncred,ubigclt,frase01,impreso,
                tcadvta,porcigv,totdvta,saldvta,subtMN,igvtMN,totdvMN,codMN,
                             */

                            tx_idr.Text = dr.GetString("id");
                            tx_dat_tipnot.Text = dr.GetString("tipnota");
                            tx_sernot.Text = dr.GetString("sernota");
                            tx_numnot.Text = dr.GetString("numnota");
                            dtp_pedido.Value = dr.GetDateTime("fechope");
                            tx_dat_tipdoc.Text = dr.GetString("tipdvta");
                            tx_serdvta.Text = dr.GetString("serdvta");
                            tx_numdvta.Text = dr.GetString("numdvta");
                            tx_dat_tdoc.Text = dr.GetString("tidoclt");
                            tx_ndc.Text = dr.GetString("nudoclt");
                            tx_nombre.Text = dr.GetString("nombclt");
                            tx_direc.Text = dr.GetString("direclt");
                            tx_dpto.Text = dr.GetString("dptoclt");
                            tx_prov.Text = dr.GetString("provclt");
                            tx_dist.Text = dr.GetString("distclt");
                            tx_mail.Text = dr.GetString("corrclt");
                            tx_telef1.Text = dr.GetString("teleclt");
                            tx_dat_orig.Text = dr.GetString("locorig");
                            tx_dir_pe.Text = dr.GetString("dirorig");
                            tx_coment.Text = dr.GetString("obsnota");
                            tx_tfil.Text = dr.GetString("canfidt");
                            tx_dat_mone.Text = dr.GetString("mondvta");
                            tx_nomVen.Text = dr.GetString("vendedor");
                            tx_contrat.Text = dr.GetString("contrato");
                            tx_valor.Text = dr.GetString("totdvta");
                            tx_bruto.Text = (double.Parse(dr.GetString("totdvta")) / (1 + double.Parse(v_igv) / 100)).ToString("#0.00");
                            tx_igv.Text = (double.Parse(dr.GetString("totdvta")) / (double.Parse(v_igv) / 100)).ToString("#0.00");

                            tx_bruNot.Text = dr.GetString("subtota");
                            tx_igvNot.Text = dr.GetString("igvtota");
                            tx_valNot.Text = dr.GetString("totnota");

                            tx_dat_estad.Text = dr.GetString("estnota");
                            //tx_nomVen.Text = dr.GetString("vendedor");
                            //tx_id_rapifac.Text = dr.GetString("idpse_ose");
                        }
                        dr.Dispose();
                        if (tx_idr.Text != "")
                        {
                            cmb_taller.SelectedItem = tx_dat_orig.Text;     // local de ventas
                           
                            string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                            DataRow[] row = dtdoc.Select(axs);
                            cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();     // tipo doc cliente
                            
                            axs = string.Format("idcodice='{0}'", tx_dat_tipnot.Text);
                            row = dtnota.Select(axs);
                            cmb_tiponot.SelectedItem = row[0].ItemArray[0].ToString();
                            tx_dat_codnot.Text = row[0].ItemArray[3].ToString();

                            axs = string.Format("idcodice='{0}'", tx_dat_tipdoc.Text);
                            row = dtpedido.Select(axs);
                            cmb_tipo.SelectedItem = row[0].ItemArray[1].ToString();

                            // nombre de estado
                            tx_status.Text = tx_dat_estad.Text;
                            // moneda
                            cmb_mon.SelectedItem = tx_dat_mone.Text;
                            cmb_mon_SelectionChangeCommitted(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Documento no encontrado!","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            tx_numdvta.Text = "";
                            tx_numdvta.Focus();
                            return;
                        }
                    }
                }
            }
            jaladet(tx_idr.Text);
        }
        private void jaladet(string idr)                    // jala el detalle 
        {
            string jalad = "SELECT filadet,cantbul,codprod,descpro,unimedp,madera,acabado,medidas,codmad,detpied,codMN,estadoser " + 
                "FROM detnotcred where idc=@idr";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                using (MySqlCommand micon = new MySqlCommand(jalad, conn))
                {
                    micon.Parameters.AddWithValue("@idr", idr);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    da.Dispose();
                }
                grilladet("edita");     // obtiene contenido de grilla con DT
            }
            conn.Close();
        }
        private void grilladet(string modo)                 // grilla detalle
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 12;
            // it       
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho                
            dataGridView1.Columns[0].HeaderText = "It";         // titulo de la columna
            dataGridView1.Columns[0].Name = "it";
            dataGridView1.Columns[0].ReadOnly = true;
            // cantbul
            dataGridView1.Columns[1].Visible = true;            // columna visible o no
            dataGridView1.Columns[1].HeaderText = "Cant";    // titulo de la columna
            dataGridView1.Columns[1].Width = 20;                // ancho
            dataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].Name = "cant";
            // codprod  
            dataGridView1.Columns[2].Visible = false;            // columna visible o no
            dataGridView1.Columns[2].HeaderText = "Artículo";    // titulo de la columna
            dataGridView1.Columns[2].Width = 70;                // ancho
            dataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Name = "item";
            // descpro     
            dataGridView1.Columns[3].Visible = true;            // columna visible o no
            dataGridView1.Columns[3].HeaderText = "descpro";    // titulo de la columna
            dataGridView1.Columns[3].Width = 400;                // ancho
            dataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[3].Name = "descpro";
            // unimedp  
            dataGridView1.Columns[4].Visible = true;            // columna visible o no
            dataGridView1.Columns[4].HeaderText = "unimedp";    // titulo de la columna
            dataGridView1.Columns[4].Width = 100;                // ancho
            dataGridView1.Columns[4].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].Name = "unimedp";
            // madera
            dataGridView1.Columns[5].Visible = true;            // columna visible o no     
            dataGridView1.Columns[5].HeaderText = "Madera";    // titulo de la columna
            dataGridView1.Columns[5].Width = 60;                // ancho
            dataGridView1.Columns[5].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].Name = "madera";
            // acabado   
            dataGridView1.Columns[6].Visible = false;            // columna visible o no
            dataGridView1.Columns[6].HeaderText = "acabado";    // titulo de la columna
            dataGridView1.Columns[6].Width = 70;                // ancho
            dataGridView1.Columns[6].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].Name = "acabado";
            // medidas 
            dataGridView1.Columns[7].Visible = false;            // columna visible o no
            dataGridView1.Columns[7].HeaderText = "medidas";    // titulo de la columna
            dataGridView1.Columns[7].Width = 70;                // ancho
            dataGridView1.Columns[7].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].Name = "medidas";
            // codmad        
            dataGridView1.Columns[8].Visible = true;            // columna visible o no
            dataGridView1.Columns[8].HeaderText = "codmad"; // titulo de la columna
            dataGridView1.Columns[8].Width = 60;                // ancho
            dataGridView1.Columns[8].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].Name = "codmad";
            // detpied 
            dataGridView1.Columns[9].Visible = true;
            dataGridView1.Columns[9].HeaderText = "detpied"; // titulo de la columna
            dataGridView1.Columns[9].Width = 60;                // ancho
            dataGridView1.Columns[9].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[9].Name = "detpied";
            // codMN
            dataGridView1.Columns[10].Visible = true;
            dataGridView1.Columns[10].HeaderText = "codMN"; // titulo de la columna
            dataGridView1.Columns[10].Width = 60;                // ancho
            dataGridView1.Columns[10].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[10].Name = "codMN";
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
                const string contaller = "select a.descrizionerid,a.idcodice,a.codigo,b.serie,b.dir_pe,b.ubigeo,a.sunat from desc_ven a " +
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
                // seleccion del tipo de nota de credito
                const string connota = "select descrizionerid,idcodice,sunat,codigo from desc_tnc " +
                                       "where numero=1";            // filtramos solo los documentos de venta
                MySqlCommand cmdnota = new MySqlCommand(connota, conn);
                MySqlDataAdapter danota = new MySqlDataAdapter(cmdnota);
                danota.Fill(dtnota);
                foreach (DataRow row in dtnota.Rows)
                {
                    cmb_tiponot.Items.Add(row.ItemArray[0].ToString());
                    cmb_tiponot.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de tipo de doc. venta ... ok
                const string conpedido = "select descrizionerid,idcodice,sunat from desc_tdv " +
                                       "where numero=1 and codigo='DV'";            // filtramos solo los documentos de venta
                MySqlCommand cmdpedido = new MySqlCommand(conpedido, conn);
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[1].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del tipo documento cliente
                const string condoc = "select descrizionerid,idcodice,codigo,sunat from desc_doc " +
                                       "where numero=1";
                MySqlCommand cmddoc = new MySqlCommand(condoc, conn);
                MySqlDataAdapter dadoc = new MySqlDataAdapter(cmddoc);
                dadoc.Fill(dtdoc);
                foreach (DataRow row in dtdoc.Rows)
                {
                    cmb_tdoc.Items.Add(row.ItemArray[0].ToString());
                    //cmb_tdoc.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de moneda
                const string conmon = "select descrizionerid,idcodice,codigo from desc_mon where numero=1";
                using (MySqlCommand my = new MySqlCommand(conmon, conn))
                {
                    using (MySqlDataAdapter dafp = new MySqlDataAdapter(my))
                    {
                        dafp.Fill(dtmon);
                        foreach (DataRow row in dtmon.Rows)
                        {
                            cmb_mon.Items.Add(row.ItemArray[1].ToString());
                        }
                    }
                }
            }
            conn.Close();
        }
        private void limpia_ini()                           // limpia e inicializa datos antes y despues de leer y grabar registro
        {
            string modo = Tx_modo.Text;
            limpiar(this);
            limpia_chk();
            limpia_combos();
            limpia_otros();
            limpia_panel(pan_cli);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_mon.Enabled = false;
            Tx_modo.Text = modo;
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
                tx_sernot.ReadOnly = true;
                tx_numnot.ReadOnly = true;
                tx_dat_mone.Text = MonDeft;
                cmb_mon_SelectionChangeCommitted(null, null);
                cmb_mon.SelectedItem = tx_dat_mone.Text;
                if (MonTodas == "S") cmb_mon.Enabled = false;
                else cmb_mon.Enabled = true;
            }
        }
        private void modonota()                             // campos de valores según el tipo de nota de credito
        {
            if (tx_dat_tipnot.Text == v_tnotanu)        // tipo de nota ANULACION
            {
                tx_bruNot.Text = tx_bruto.Text;
                tx_igvNot.Text = tx_igv.Text;
                tx_valNot.Text = tx_valor.Text;

                tx_bruNot.ReadOnly = true;
                tx_igvNot.ReadOnly = true;
                tx_valNot.ReadOnly = true;
                tx_coment.Focus();
            }
            if (tx_dat_tipnot.Text == v_tnotdsc)        // tipo de nota por descuento
            {
                tx_bruNot.ReadOnly = true;
                tx_igvNot.ReadOnly = true;
                tx_valNot.ReadOnly = false;
                tx_valNot.Focus();
            }
        }
        private void valnumero(string tipo, string serie, string corre)
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string vb = "select * from cabnotascd where tipnota=@tno and sernota=@sno and numnota=@nno";
                    using (MySqlCommand micon = new MySqlCommand(vb, conn))
                    {
                        micon.Parameters.AddWithValue("@tno", tipo);
                        micon.Parameters.AddWithValue("@sno", serie);
                        micon.Parameters.AddWithValue("@nno", corre);
                        using (MySqlDataReader dr = micon.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                if (dr.GetString(0) != null && dr.GetString(0) != "")
                                {
                                    MessageBox.Show("Error, la nota ya existe!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    tx_numnot.Text = "";
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Se perdió acceso al servidor!","Error de conexión",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
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
            sololeepan(pan_cli);
            cmb_taller.Enabled = false;
            limpia_ini();
            button1.Image = Image.FromFile(img_grab);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            grilladet("NUEVO");
            tx_nomVen.Text = Program.vg_nuse;
            tx_numnot.Enabled = true;
            tx_numnot.ReadOnly = false;
            cmb_tiponot.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            sololee(this);
            sololeepan(pan_cli);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpia_ini();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.Enabled = true;
            tx_serdvta.Enabled = true;
            tx_serdvta.ReadOnly = false;
            tx_numdvta.Enabled = true;
            tx_numdvta.ReadOnly = false;
            //  solo se modifica comentarios
            tx_coment.Enabled = true;
            tx_coment.ReadOnly = false;
            //
            tx_coment.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            //       no se pueden anular notas
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            sololee(this);
            sololeepan(pan_cli);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;
            limpia_ini();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            tx_sernot.Enabled = true;
            tx_sernot.ReadOnly = false;
            tx_numnot.Enabled = true;
            tx_numnot.ReadOnly = false;
            tx_sernot.Focus();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            // no haty
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_numdvta.Text != "")
            {
                
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            //
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
        private void sololeepan(Panel pan)
        {
            foreach (Control oControls in pan.Controls)
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
            tx_bruNot.ReadOnly = true;
            tx_igvNot.ReadOnly = true;
            tx_valNot.ReadOnly = true;
        }
        private void escribepan(Panel pan)
        {
            foreach (Control oControls in pan.Controls)
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
        private void limpia_chk()
        {
            //checkBox1.Checked = false;
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
            cmb_tiponot.SelectedIndex = -1;
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
            if (cmb_tipo.SelectedIndex > -1)
            {
                string axs = string.Format("idcodice='{0}'", cmb_tipo.Text);
                DataRow[] row = dtpedido.Select(axs);
                tx_dat_tipdoc.Text = row[0].ItemArray[1].ToString();
                tx_dat_tipdoc_s.Text = row[0].ItemArray[2].ToString();
            }
            else
            {
                tx_dat_tipdoc.Text = "";
                tx_dat_tipdoc_s.Text = "";
            }
        }
        private void cmb_tdoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tdoc.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_tdoc.Text);
                DataRow[] row = dtdoc.Select(axs);
                tx_dat_tdoc.Text = row[0].ItemArray[1].ToString();
                tx_dat_tdoc_s.Text = row[0].ItemArray[3].ToString();
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
                tx_dir_ubigpe.Text = row[0].ItemArray[5].ToString();
                tx_sernot.Text = row[0].ItemArray[3].ToString();
                tx_codSuc.Text = row[0].ItemArray[6].ToString();          // codigo de sucursal fact. elect
            }
        }
        private void cmb_mon_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (true) // cmb_mon.SelectedIndex > -1
            {
                string axs = "";
                if (cmb_mon.Text == "") axs = string.Format("idcodice='{0}'", tx_dat_mone.Text);
                else axs = string.Format("idcodice='{0}'", cmb_mon.Text);
                DataRow[] row = dtmon.Select(axs);
                tx_dat_mone.Text = row[0].ItemArray[1].ToString();
                tx_dat_mon_s.Text = row[0].ItemArray[2].ToString();
            }
        }
        private void cmb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {
            string axs = string.Format("idcodice='{0}'", cmb_mon.Text);
            DataRow[] row = dtmon.Select(axs);
            tx_dat_mone.Text = row[0].ItemArray[1].ToString();
            tx_dat_mon_s.Text = row[0].ItemArray[2].ToString();
        }
        private void cmb_tiponot_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tx_bruNot.Text = "";
            tx_igvNot.Text = "";
            tx_valNot.Text = "";
            modonota();
            string axs = string.Format("descrizionerid='{0}'", cmb_tiponot.Text);
            DataRow[] row = dtnota.Select(axs);
            tx_dat_tipnot.Text = row[0].ItemArray[1].ToString();
            tx_dat_codnot.Text = row[0].ItemArray[3].ToString();
        }
        #endregion comboboxes

        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text.Trim() != "" && tx_numnot.Text.Trim() == "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
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
            }
        }
        internal void tx_corre_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")
            {
                if (tx_sernot.Text == "")
                {
                    tx_sernot.Focus();
                    return;
                }
                if (tx_numnot.Text != "")
                {
                    jalaoc("tx_corre");
                }
            }
            else
            {
                valnumero(tx_dat_tipnot.Text,tx_sernot.Text,tx_numnot.Text);
            }
        }
        private void tx_valNot_Leave(object sender, EventArgs e)            // nota de cred por descuento
        {
            if (Tx_modo.Text == "NUEVO" && tx_dat_tipnot.Text == v_tnotdsc && tx_valNot.Text != "")
            {
                double vbru = double.Parse(tx_valNot.Text) / (1 + double.Parse(v_igv) / 100);  // 1.18;
                double vigv = double.Parse(tx_valNot.Text) - vbru;
                tx_igvNot.Text = vigv.ToString("#0.00");
                tx_bruNot.Text = vbru.ToString("#0.00");
            }
        }

        #endregion leaves;

        #region radio_buttons

        #endregion

        #region datagridview1 - grilla detalle del doc.venta

        #endregion

        #region botones de grabar y agregar
        private void button2_Click(object sender, EventArgs e)      // jala datos del comprobante para la nota
        {
            if (Tx_modo.Text != "NUEVO")
            {
                return;
            }
            // validamos datos del comprobante
            if (tx_dat_tipdoc.Text == "")
            {
                MessageBox.Show("Seleccione el tipo de comprobante","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                cmb_tipo.Focus();
                return;
            }
            if (tx_serdvta.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la serie del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_serdvta.Focus();
                return;
            }
            if (tx_numdvta.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el número del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_numdvta.Focus();
                return;
            }
            // llamamos al procedimiento que jala toooooodos los datos
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string jala = "select id,fechope,martdve,tipdvta,serdvta,numdvta,ticltgr,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt,ubigclt,corrclt,teleclt,telemsg," +
                        "locorig,dirorig,ubiorig,obsdvta,canfidt,canbudt,mondvta,tcadvta,subtota,igvtota,porcigv,round(totdvta,2) as totdvta,totpags,saldvta,estdvta,frase01," +
                        "tipoclt,m1clien,tippago,impreso,codMN,subtMN,igvtMN,totdvMN,pagauto,tipdcob,idcaja,plazocred,porcendscto,valordscto," +
                        "referen1,ubipdest,conPago,contrato,vendedor,muebles,idpse_ose,contrato " +
                        "from cabfactu where tipdvta=@tdv and serdvta=@sdv and numdvta=@ndv";
                    using (MySqlCommand micon = new MySqlCommand(jala, conn))
                    {
                        micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                        micon.Parameters.AddWithValue("@sdv", tx_serdvta.Text);
                        micon.Parameters.AddWithValue("@ndv", tx_numdvta.Text);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString("id");
                            //dtp_pedido.Value = dr.GetDateTime("fechope");
                            tx_dat_tipdoc.Text = dr.GetString("tipdvta");
                            tx_dat_tdoc.Text = dr.GetString("tidoclt");
                            tx_ndc.Text = dr.GetString("nudoclt");
                            tx_nombre.Text = dr.GetString("nombclt");
                            tx_direc.Text = dr.GetString("direclt");
                            tx_dpto.Text = dr.GetString("dptoclt");
                            tx_prov.Text = dr.GetString("provclt");
                            tx_dist.Text = dr.GetString("distclt");
                            tx_mail.Text = dr.GetString("corrclt");
                            tx_telef1.Text = dr.GetString("teleclt");
                            tx_telef2.Text = dr.GetString("telemsg");
                            tx_dat_orig.Text = dr.GetString("locorig");
                            tx_dir_pe.Text = dr.GetString("dirorig");
                            //tx_coment.Text = dr.GetString("obsdvta"); // 
                            tx_tfil.Text = dr.GetString("canfidt");
                            tx_dat_mone.Text = dr.GetString("mondvta");
                            tx_bruto.Text = dr.GetString("subtota");
                            tx_igv.Text = dr.GetString("igvtota");
                            tx_valor.Text = dr.GetString("totdvta");
                            tx_dat_estad.Text = dr.GetString("estdvta");
                            tx_nomVen.Text = dr.GetString("vendedor");
                            tx_id_rapifac.Text = dr.GetString("idpse_ose");
                            tx_contrat.Text = dr.GetString("contrato");
                        }
                        dr.Dispose();
                        if (tx_idr.Text != "")
                        {
                            cmb_taller.SelectedItem = tx_dat_orig.Text;     // local de ventas
                            // tipo doc cliente
                            string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                            DataRow[] row = dtdoc.Select(axs);
                            cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();
                            // nombre de estado
                            //tx_status.Text = tx_dat_estad.Text;
                            // moneda
                            cmb_mon.SelectedItem = tx_dat_mone.Text;
                            cmb_mon_SelectionChangeCommitted(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Documento no encontrado!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tx_numdvta.Text = "";
                            tx_numdvta.Focus();
                            return;
                        }
                    }
                    string jalad = "SELECT filadet,cantbul,codprod,descpro,medidas,codmad,madera,acabado,precio,totalMN,space(1),dscto,totSinDscto " +
                    "FROM detfactu where tipdocvta=@tdv and serdvta=@sdv and numdvta=@ndv";
                    using (MySqlCommand micon = new MySqlCommand(jalad, conn))
                    {
                        micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                        micon.Parameters.AddWithValue("@sdv", tx_serdvta.Text);
                        micon.Parameters.AddWithValue("@ndv", tx_numdvta.Text);
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        dataGridView1.DataSource = dt;
                        da.Dispose();
                    }
                    grilladet("edita");
                }
                else
                {
                    MessageBox.Show("Error en conexión a la base de datos","No se puede continuar",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                // según tipo de nota vemos que hacemos
                modonota();
            }
        }
        private void button1_Click(object sender, EventArgs e)      // graba, anula
        {
            // validaciones generales
            if (tx_dat_tipnot.Text == "")
            {
                MessageBox.Show("Seleccione el tipo de nota de crédito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_tiponot.Focus();
                return;
            }
            if (tx_dat_tipdoc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de documento de venta","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                cmb_tipo.Focus();
                return;
            }
            if (tx_dat_tdoc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de documento del cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_tdoc.Focus();
                return;
            }
            if (tx_serdvta.Text == "")
            {
                MessageBox.Show("Ingrese la serie del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_serdvta.Focus();
                return;
            }
            if (tx_numdvta.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el número del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_numdvta.Focus();
                return;
            }
            if (tx_valNot.Text == "")
            {
                MessageBox.Show("Ingrese el valor de la nota", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_valNot.Focus();
                return;
            }
            
            if (Tx_modo.Text == "NUEVO")
            {
                // validaciones 

                var aa = MessageBox.Show(" Confirma que desea CREAR " + Environment.NewLine +
                    "la Nota de Crédito?","Confirme por favor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (graba() == true)
                    {
                        //Bt_print.PerformClick();
                    }
                }
                else return;
            }
            limpia_ini();
            cmb_tiponot.Focus();
        }
        private bool graba()                                // graba cabecera de la nota
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                // no se hace conexion a Rapifac 28/09/2022, en ADM hacen las notas y luego avisan a Barranco para su registro en el sistema y archivos
                //tx_numnot.Text = lib.Right(DateTime.Now.Millisecond.ToString(), 8);
                string inserta = "insert into cabnotascd (" +
                    "fechope,martnot,tipnota,sernota,numnota,tipdvta,serdvta,numdvta,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt," +
                    "ubigclt,corrclt,teleclt,locorig,dirorig,ubiorig,obsnota,mondvta,tcadvta,subtota,igvtota,porcigv,totnota,totdvta,saldvta," +
                    "subtMN,igvtMN,totdvMN,codMN,estnota,frase01,impreso,canfidt,tipncred,vendedor,idpse_ose,contrato," +
                    "verApp,userc,fechc,diriplan4,diripwan4,netbname) values (" +
                    "@fechop,@mtdvta,@ctnota,@sernot,@numnot,@tcdvta,@serdvta,@numdvta,@tdcrem,@ndcrem,@nomrem,@dircre,@dptocl,@provcl,@distcl," +
                    "@ubicre,@mailcl,@telec1,@ldcpgr,@didegr,@ubdegr,@obsprg,@monppr,@tcoper,@subpgr,@igvpgr,@porcigv,@totpgr,@totdva,@saldvta," +
                    "@subMN,@igvMN,@totMN,@codMN,@estpgr,@frase1,@impSN,@canfil,@tinocr,@vende,@idpse,@cont," +
                    "@verApp,@asd,now(),@iplan,@ipwan,@nbnam)";
                using (MySqlCommand micon = new MySqlCommand(inserta, conn))
                {
                    micon.Parameters.AddWithValue("@fechop", dtp_pedido.Text.Substring(6, 4) + "-" + dtp_pedido.Text.Substring(3, 2) + "-" + dtp_pedido.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@mtdvta", "");     // cmb_tipo.Text.Substring(0, 1)
                    micon.Parameters.AddWithValue("@ctnota", tx_dat_tipnot.Text);
                    micon.Parameters.AddWithValue("@sernot", tx_sernot.Text);
                    micon.Parameters.AddWithValue("@numnot", tx_numnot.Text);
                    micon.Parameters.AddWithValue("@tcdvta", tx_dat_tipdoc.Text);
                    micon.Parameters.AddWithValue("@serdvta", tx_serdvta.Text);
                    micon.Parameters.AddWithValue("@numdvta", tx_numdvta.Text);
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
                    micon.Parameters.AddWithValue("@ldcpgr", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@didegr", tx_dir_pe.Text);                   // direccion local de ventas
                    micon.Parameters.AddWithValue("@ubdegr", "");                               // ubigeo origen
                    micon.Parameters.AddWithValue("@obsprg", tx_coment.Text);
                    micon.Parameters.AddWithValue("@monppr", tx_dat_mone.Text);
                    micon.Parameters.AddWithValue("@tcoper", "0");                              // TIPO DE CAMBIO
                    micon.Parameters.AddWithValue("@subpgr", tx_bruNot.Text);                     // sub total
                    micon.Parameters.AddWithValue("@igvpgr", tx_igvNot.Text);                      // igv
                    micon.Parameters.AddWithValue("@porcigv", v_igv);                           // porcentaje en numeros de IGV
                    micon.Parameters.AddWithValue("@totpgr", tx_valNot.Text);                    // total inc. igv
                    micon.Parameters.AddWithValue("@totdva", tx_valor.Text);                    // total doc.venta
                    micon.Parameters.AddWithValue("@saldvta", 0);                               // SALDO DEL DOC.VTA.
                    micon.Parameters.AddWithValue("@subMN", tx_bruNot.Text);      // subtMN
                    micon.Parameters.AddWithValue("@igvMN", tx_igvNot.Text);        // igvtMN
                    micon.Parameters.AddWithValue("@totMN", tx_valNot.Text);      // fletMN
                    micon.Parameters.AddWithValue("@codMN", MonDeft);                           // codigo moneda local
                    micon.Parameters.AddWithValue("@estpgr", "");                          // estado de la nota
                    micon.Parameters.AddWithValue("@frase1", "");                               // no hay nada que poner
                    micon.Parameters.AddWithValue("@impSN", "N");                               // impreso? S, N ==> no se imprimen las notas 23/08/2022
                    micon.Parameters.AddWithValue("@canfil", tx_tfil.Text);                     // cantidad de filas de detalle
                    micon.Parameters.AddWithValue("@tinocr", tx_dat_codnot.Text);               // tipo de cliente credito o contado => TODOS SON CONTADO=1
                    micon.Parameters.AddWithValue("@vende", tx_nomVen.Text);
                    micon.Parameters.AddWithValue("@idpse", tx_id_rapifac.Text);
                    micon.Parameters.AddWithValue("@cont",tx_contrat.Text);                     //
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
                    if (row.Cells[0].Value != null)
                    {
                        {
                            string inserd2 = "update detnotcred set " +
                                "cantbul=@bult,codprod=@citem,unimedp=@unim,descpro=@desc,pesogro=@peso,medidas=@medid,madera=@mader," +
                                "acabado=@acaba,codmad=@codm,detpied=@detp,codMN=@cmnn " +
                                "where tipnota=@tnot and sernota=@snot and numnota=@nnot and filadet=@fila";
                            using (MySqlCommand micon = new MySqlCommand(inserd2, conn))
                            {
                                micon.CommandTimeout = 60;
                                // me quede ac'a
                                micon.Parameters.AddWithValue("@tnot", tx_dat_tipnot.Text);
                                micon.Parameters.AddWithValue("@snot", tx_sernot.Text);
                                micon.Parameters.AddWithValue("@nnot", tx_numnot.Text);
                                micon.Parameters.AddWithValue("@fila", fila);
                                micon.Parameters.AddWithValue("@bult", row.Cells[1].Value.ToString());
                                micon.Parameters.AddWithValue("@citem", row.Cells[2].Value.ToString());
                                micon.Parameters.AddWithValue("@unim", "");
                                micon.Parameters.AddWithValue("@desc", row.Cells[3].Value.ToString());
                                micon.Parameters.AddWithValue("@peso", "0");
                                micon.Parameters.AddWithValue("@medid", row.Cells[4].Value.ToString());
                                micon.Parameters.AddWithValue("@mader", "");    // la madera verdadera debe seleccionarse en el contrato
                                micon.Parameters.AddWithValue("@acaba", row.Cells[7].Value.ToString());
                                micon.Parameters.AddWithValue("@codm", row.Cells[5].Value.ToString());
                                micon.Parameters.AddWithValue("@detp", row.Cells[6].Value.ToString());
                                micon.Parameters.AddWithValue("@cmnn", MonDeft);
                                //micon.Parameters.AddWithValue("@pret", decimal.Parse(row.Cells[8].Value.ToString()));
                                //micon.Parameters.AddWithValue("@tgrmn", decimal.Parse(row.Cells[9].Value.ToString()));
                                //micon.Parameters.AddWithValue("@pagaut", "S");
                                //micon.Parameters.AddWithValue("@esta", codCanc);        // todos los comprob. nacen cancelados
                                //micon.Parameters.AddWithValue("@vesta", (row.Cells[11].Value == null || row.Cells[11].Value == DBNull.Value) ? 0 : decimal.Parse(row.Cells[11].Value.ToString()));
                                micon.ExecuteNonQuery();
                                fila += 1;
                                //
                                retorna = true;         // no hubo errores!
                            }
                        }
                    }
                }
                // medios de pago
                {
                    string inpag = "insert into adifactpag (idc,tdvta,sdvta,ndvta,it,medio,operac,importe,codpag,fpago) values (" +
                        "@idc,@tdv,@sdv,@ndv,@it,@med,@ope,@imp,@cpa,@fpa)";
                    using (MySqlCommand micon = new MySqlCommand(inpag, conn))
                    {
                        decimal xx = decimal.Negate(decimal.Parse(tx_valNot.Text));
                        micon.Parameters.AddWithValue("@idc", 0);
                        micon.Parameters.AddWithValue("@tdv", tx_dat_codnot.Text);
                        micon.Parameters.AddWithValue("@sdv", tx_sernot.Text);
                        micon.Parameters.AddWithValue("@ndv", tx_numnot.Text);
                        micon.Parameters.AddWithValue("@it", (1).ToString());
                        micon.Parameters.AddWithValue("@med", "");
                        micon.Parameters.AddWithValue("@ope", "");
                        micon.Parameters.AddWithValue("@imp", xx.ToString());
                        micon.Parameters.AddWithValue("@cpa", "");
                        micon.Parameters.AddWithValue("@fpa", dtp_pedido.Text.Substring(6, 4) + "-" + dtp_pedido.Text.Substring(3, 2) + "-" + dtp_pedido.Text.Substring(0, 2));
                        micon.ExecuteNonQuery();
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
        #endregion

        #region Fact-Electrónica RAPIFAC
        // no hay
        #endregion

        #region impresion

        #endregion

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

    }

}
