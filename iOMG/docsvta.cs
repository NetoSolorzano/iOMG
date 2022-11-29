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
    public partial class docsvta : Form
    {
        static string nomform = "docsvta";      // nombre del formulario
        string asd = iOMG.Program.vg_user;          // usuario conectado al sistema
        string colback = iOMG.Program.colbac;       // color de fondo
        string colpage = iOMG.Program.colpag;       // color de los pageframes
        string colgrid = iOMG.Program.colgri;       // color de las grillas
        string colstrp = iOMG.Program.colstr;       // color del strip
        static string nomtab = "cabfactu";
        libreria lib = new libreria();
        NumLetra nle = new NumLetra();

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
        string cliente = Program.cliente;   // razon social para los reportes
        string usuaRuc = "";                // ruc/usuarios del cliente
        string usuaDni = "";                // dni/usuario del cliente
        string clave = "";                  // clave del dni/usuario
        string id_clte = "";                // identificador clave
        string rut_pdf = "";                // ruta web del pdf en servidor Rapifac
        string rut_xml = "";                // ruta para descargar el xml de rapifac
        string cod_umed = "";               // codigo unidad de medida
        string nom_umed = "";               // nombre unidad de medida
        string cod_user = "";               // codigo unidad de SERVICIO
        string nom_user = "";               // nombre unidad de SERVICIO
        string mailPrin = "";               // correo electrónico principal
        string webdni = "";                 // direccion web de pag. busqueda dni
        string vSNdsctoD = "";              // S ó N, permite o no descuento en detalle
        string vSNdsctoC = "";              // S ó N, permite o no descuento GLOBAL
        double DetServLim = 0;              // Límite para Detracción en Servicios
        #endregion

        List<docsAnticip> _docsAnticip = new List<docsAnticip>();

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
        DataTable dttaller = new DataTable();   // combo local de ventas
        DataTable dtdoc = new DataTable();      // combo tipo doc cliente
        DataTable dtfp = new DataTable();       // combo para tipo de pago
        DataTable dtpedido = new DataTable();   // tipos documento de venta
        DataTable dtmon = new DataTable();      // monedas
        DataTable dtadpd = new DataTable();     // tabla para el autocompletado de dpto, provin y distrito
        DataTable dtdetS = new DataTable();     // combo de porcentajes de detraccion
        AutoCompleteStringCollection adptos = new AutoCompleteStringCollection();
        AutoCompleteStringCollection aprovi = new AutoCompleteStringCollection();
        AutoCompleteStringCollection adistr = new AutoCompleteStringCollection();
        string vpago = "";                      // pago anticipo o cancelatorio
        string[,] dtpagos = new string[10, 7];  // 10 filas, 6 columnas para los medios de pago por comprobante

        public docsvta()
        {
            InitializeComponent();
            ini_pagos();
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
            if (keyData == Keys.F1)
            {
                if (tx_ndc.Focused == true && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
                {
                    para1 = "anag_cli";   // maestra clientes
                    para2 = "todos";   // 
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            tx_dat_tdoc.Text = ayu2.ReturnValueA[1];
                            tx_ndc.Text = ayu2.ReturnValueA[2];
                            tx_nombre.Text = ayu2.ReturnValueA[3];
                            tx_idc.Text = ayu2.ReturnValueA[0];
                            //
                            string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                            DataRow[] row = dtdoc.Select(axs);
                            cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();
                            //
                            valDocClte_Leave(null, null);
                        }
                    }
                }
                if (tx_cont.Focused == true && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))        // solo debe mostrar contratos con SALDO
                {
                    para1 = "contrat";
                    para2 = "";
                    para3 = "saldo";
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
                if (tx_d_nom.Focused == true || tx_d_codi.Focused == true && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
                {
                    para1 = "items";
                    para2 = "todos";
                    if (rb_tserv.Checked == true)
                    {
                        para1 = "items_adic";
                        para3 = "";
                        //para4 = itemSer;    // este parametro  no sirve porque itemsadic son solo Z
                    }
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            tx_d_codi.Text = ayu2.ReturnValue1.ToString();
                            tx_d_nom.Text = ayu2.ReturnValue2.ToString();
                            tx_d_id.Text = ayu2.ReturnValue0.ToString();

                            if (ayu2.ReturnValueA != null)
                            {
                                tx_d_precio.Text = ayu2.ReturnValueA[3];
                                tx_d_preSinDscto.Text = ayu2.ReturnValueA[3];
                                tx_d_med.Text = ayu2.ReturnValueA[2];
                            }
                        }
                    }
                }
                if (tx_impMedios.Focused == true)
                {
                    forpagos pagos = new forpagos(dtfp, tpcontad, dtpagos, (Tx_modo.Text == "NUEVO")? false : true);
                    var resu = pagos.ShowDialog();
                    if (resu == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(pagos.ReturnValue1))
                        {
                            tx_impMedios.Text = pagos.ReturnValue1.ToString();
                            for (int i = 0; i < 9; i++)
                            {
                                dtpagos[i, 0] = pagos.ReturnValue[i, 0];
                                dtpagos[i, 1] = pagos.ReturnValue[i, 1];
                                dtpagos[i, 2] = pagos.ReturnValue[i, 2];
                                dtpagos[i, 3] = pagos.ReturnValue[i, 3];
                                dtpagos[i, 4] = pagos.ReturnValue[i, 4];
                                dtpagos[i, 5] = pagos.ReturnValue[i, 5];
                                dtpagos[i, 6] = pagos.ReturnValue[i, 6];
                            }
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
            grilladet("NUEVO");
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            //Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            tx_d_nom.Enabled = false;
            autodptos();
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
            tx_coment.MaxLength = 100;                  // 27-10-2022 se imprime este comentario y va al xml-pdf
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
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@clie,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@clie", "clients");
                micon.Parameters.AddWithValue("@ped", nomform);
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "main")
                    {
                        if (row["campo"].ToString() == "imagenes")
                        {
                            if (row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                            if (row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                            if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                            if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                            if (row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                            if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                            //if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                            if (row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                            if (row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                            if (row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                            if (row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                            if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                            if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                            if (row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                            if (row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
                        }
                        if (row["campo"].ToString() == "rapifac")
                        {
                            if (row["param"].ToString() == "rucCliente") usuaRuc = row["valor"].ToString().Trim();          // ruc/usuarios del cliente
                            if (row["param"].ToString() == "dniCliente") usuaDni = row["valor"].ToString().Trim();          // dni/usuario del cliente
                            if (row["param"].ToString() == "clave") clave = row["valor"].ToString().Trim();                 // clave del dni/usuario
                            if (row["param"].ToString() == "id_clte") id_clte = row["valor"].ToString().Trim();             // identificador clave
                            if (row["param"].ToString() == "ruta_pdf") rut_pdf = row["valor"].ToString().Trim();            // ruta web del pdf en Rapifac
                            if (row["param"].ToString() == "ruta_xml") rut_xml = row["valor"].ToString().Trim();            // ruta web para descargar el xml en Rapifac
                            if (row["param"].ToString() == "cod_uMed") cod_umed = row["valor"].ToString().Trim();           // codigo unidad de medida
                            if (row["param"].ToString() == "nom_uMed") nom_umed = row["valor"].ToString().Trim();           // nombre unidad de medida
                            if (row["param"].ToString() == "cod_uSer") cod_user = row["valor"].ToString().Trim();           // codigo unidad de SERVICIO
                            if (row["param"].ToString() == "nom_uSer") nom_user = row["valor"].ToString().Trim();           // nombre unidad de SERVICIO
                            if (row["param"].ToString() == "mail_prin1") mailPrin = row["valor"].ToString().Trim();           // correo electronico principal
                            if (row["param"].ToString() == "web_dni") webdni = row["valor"].ToString().Trim();              // pag web para busqueda de dni
                        }
                    }
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento")
                        {
                            if (row["param"].ToString() == "ruc") vtc_ruc = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "dni") vtc_dni = row["valor"].ToString().Trim();
                        }
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "clientes") tipede = row["valor"].ToString().Trim();         // 
                        if (row["campo"].ToString() == "anticipos" && row["param"].ToString() == "glosa") letiden = row["valor"].ToString().Trim();         // glosa de anticipos
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "codAnu") estanu = row["valor"].ToString().Trim();         // codigo estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "Anulado") nomanu = row["valor"].ToString().Trim();        // nombre estado anulado
                        if (row["campo"].ToString() == "tx_status" && row["param"].ToString() == "cancelado") codCanc = row["valor"].ToString().Trim();     // codigo estado cancelado
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") MonDeft = row["valor"].ToString().Trim();          // moneda por defecto
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "todas") MonTodas = row["valor"].ToString().Trim();          // moneda por defecto
                        if (row["campo"].ToString() == "items" && row["param"].ToString() == "stock") lps = row["valor"].ToString().Trim();                 // tipos de muebles que se hacen contrato
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "nomImTK") v_impTK = row["valor"].ToString().Trim();       // nombre de la impresora de tickets
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "factura") codfact = row["valor"].ToString().Trim();       // codigo tipo doc factura
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "boleta") codbole = row["valor"].ToString().Trim();       // codigo tipo doc boleta
                        if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "leyen1") leydet1 = row["valor"].ToString().Trim();           // 
                        if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "leyen2") leydet2 = row["valor"].ToString().Trim();           // 
                        if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "serv_limit") DetServLim = double.Parse(row["valor"].ToString()); // Monto limite para Detraccion en servicios
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "restex") restexto = row["valor"].ToString().Trim();       // texto resolucion sunat autorizando prov. fact electronica
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "resAut") autoriz = row["valor"].ToString().Trim();        //
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "desped") despe2 = row["valor"].ToString().Trim();         // 
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "valdirec") valdirec = row["valor"].ToString().Trim();     // monto limite para obligar a tener direcion en boleta
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "codefect") tpcontad = row["valor"].ToString().Trim();     // codigo tipo de documento efectivo contado
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ciavss") v_liav = row["valor"].ToString().Trim();         // letra o caracter inicial indicativo de articulos varios vta directa sin stock
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "camnomb") v_cnprd = row["valor"].ToString().Trim();       // Se puede cambiar nombres de items de prods. catalogo? S=si, N=no
                        if (row["campo"].ToString() == "servicios" && row["param"].ToString() == "items") itemSer = row["valor"].ToString().Trim();         // Items para comprobantes de servicios
                        if (row["campo"].ToString() == "descuento" && row["param"].ToString() == "detalle") vSNdsctoD = row["valor"].ToString().Trim();     // acepta descuento en detalle S/N
                        if (row["campo"].ToString() == "descuento" && row["param"].ToString() == "global") vSNdsctoC = row["valor"].ToString().Trim();     // acepta descuento GLOBAL

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
            string jala = "select id,fechope,martdve,tipdvta,serdvta,numdvta,ticltgr,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt,ubigclt,corrclt,teleclt,telemsg," +
            "locorig,dirorig,ubiorig,obsdvta,canfidt,canbudt,mondvta,tcadvta,subtota,igvtota,porcigv,round(totdvta,2) as totdvta,totpags,saldvta,estdvta,frase01," +
            "tipoclt,m1clien,tippago,impreso,codMN,subtMN,igvtMN,totdvMN,pagauto,tipdcob,idcaja,plazocred,porcendscto,valordscto," +
            "referen1,ubipdest,conPago,contrato,vendedor,muebles,idpse_ose,pdfpse_ose,tipoComp from cabfactu where ";
            string parte = "";
            if (campo == "tx_idr" && tx_idr.Text != "" && tx_corre.Text.Trim() == "")
            {
                if (Tx_modo.Text != "NUEVO")
                {
                    parte = "id=@idr";
                }
            }
            if (campo == "tx_corre" && tx_corre.Text != "")
            {
                if (Tx_modo.Text != "NUEVO")
                {
                    parte = "tipdvta=@tdv and serdvta=@sdv and numdvta=@ndv";
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
                            micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                            micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                            micon.Parameters.AddWithValue("@ndv", tx_corre.Text);
                        }
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString("id");
                            dtp_pedido.Value = dr.GetDateTime("fechope");
                            tx_dat_tipdoc.Text = dr.GetString("tipdvta");
                            tx_serie.Text = dr.GetString("serdvta");
                            tx_corre.Text = dr.GetString("numdvta");
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
                            tx_coment.Text = dr.GetString("obsdvta");
                            tx_tfil.Text = dr.GetString("canfidt");
                            tx_dat_mone.Text = dr.GetString("mondvta");
                            tx_bruto.Text = dr.GetString("subtota");
                            tx_igv.Text = dr.GetString("igvtota");
                            tx_valor.Text = dr.GetString("totdvta");
                            tx_impMedios.Text = dr.GetString("totdvta");    // los pagos son iguales al valor del comprobante, sino no deja grabar
                            tx_dat_estad.Text = dr.GetString("estdvta");
                            tx_dat_plazo.Text = dr.GetString("tippago");
                            tx_numOpe.Text = dr.GetString("referen1");
                            tx_cont.Text = dr.GetString("contrato");
                            tx_nomVen.Text = dr.GetString("vendedor");
                            tx_prdsCont.Text = dr.GetString("muebles");
                            rb_contado.Checked = (dr.GetString("conPago") == "1") ? true : false;
                            tx_tipComp.Text = dr.GetString("tipdcob");
                            tx_id_rapifac.Text = dr.GetString("idpse_ose");
                            tx_pdf_rapifac.Text = dr.GetString("pdfpse_ose");
                            tx_desGlob.Text = dr.GetString("valordscto");
                            tx_subtot.Text = (dr.GetDouble("subtota") + dr.GetDouble("valordscto")).ToString("#0.00");
                            tx_valor.Text = dr.GetDouble("totdvta").ToString("#0.00");
                            tx_bruto.Text = dr.GetDouble("subtota").ToString("#0.00");    // (ntoti / 1.18).ToString("#0.00");
                            tx_igv.Text = dr.GetDouble("igvtota").ToString("#0.00");
                            if (dr.GetString("tipoComp") == "B") rb_tbienes.Checked = true;
                            else
                            {
                                rb_tserv.Checked = true;
                                if (tx_dat_cDet.Text != "")
                                {
                                    tx_dat_cDet.Text = dr.GetString("tippago");
                                    string ax = string.Format("idcodice='{0}'", tx_dat_cDet.Text);
                                    DataRow[] data = dtdetS.Select(ax);
                                    cmb_detrac.SelectedItem = data[0].ItemArray[1].ToString();
                                }
                            }
                        }
                        if (tx_tipComp.Text == "A" || tx_tipComp.Text == "C")
                        {
                            rb_antic.Checked = true;
                        }
                        else rb_bienes.Checked = true;

                        dr.Dispose();
                        if (tx_idr.Text != "")
                        {
                            cmb_taller.SelectedItem = tx_dat_orig.Text;     // local de ventas  
                            cmb_taller_SelectionChangeCommitted(null,null);                        //tx_codSuc.Text = row[0].ItemArray[6].ToString();
                            // tipo doc cliente
                            string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                            DataRow[] row = dtdoc.Select(axs);
                            cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();
                            // boton contado
                            rb_contado.PerformClick();
                            // nombre de estado
                            tx_status.Text = tx_dat_estad.Text;
                            // medio de pago
                            if (tx_dat_plazo.Text.Trim() != "")
                            {
                                axs = string.Format("idcodice='{0}'", tx_dat_plazo.Text);
                                row = dtfp.Select(axs);
                                cmb_plazo.SelectedItem = (row.Length > 0)? row[0].ItemArray[0].ToString() : "";
                            }
                            // moneda
                            cmb_mon.SelectedItem = tx_dat_mone.Text;
                            cmb_mon_SelectionChangeCommitted(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Documento no encontrado!","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            tx_corre.Text = "";
                            tx_corre.Focus();
                            return;
                        }
                    }
                    using (MySqlCommand micon = new MySqlCommand("select * from adifactpag where tdvta=@tdv and sdvta=@sdv and ndvta=@ndv", conn))
                    {
                        micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                        micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                        micon.Parameters.AddWithValue("@ndv", tx_corre.Text);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            DataTable kll = new DataTable();
                            da.Fill(kll);
                            int i = 0;
                            foreach (DataRow row in kll.Rows)
                            {
                                dtpagos[i, 0] = row[1].ToString();
                                dtpagos[i, 1] = (i + 1).ToString();
                                dtpagos[i, 2] = row[6].ToString();
                                dtpagos[i, 3] = row[7].ToString();
                                dtpagos[i, 4] = row[8].ToString();
                                dtpagos[i, 5] = row[9].ToString();
                                dtpagos[i, 6] = row[10].ToString().Substring(0, 10);
                                i = i + 1;
                            }
                            kll.Dispose();
                        }
                    }
                }
            }
            jaladet(tx_idr.Text);
            cosas_pagamenti();                              // llenamos lista de pagos si es cancelacion
            // suma_grilla();
        }
        private void jaladet(string idr)                    // jala el detalle 
        {
            string jalad = "";
            if (rb_antic.Checked == true)
            {
                jalad = "SELECT filadet,cantbul,codprod,descpro,medidas,codmad,madera,acabado,precio,totalMN,if(cantbul>0,'A',space(1)),dscto,totSinDscto " +
                    "FROM detfactu where idc=@idr";
            }
            else
            {
                jalad = "SELECT filadet,cantbul,codprod,descpro,medidas,codmad,madera,acabado,precio,totalMN,space(1),dscto,totSinDscto " +
                    "FROM detfactu where idc=@idr";
            }
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
        private void grilladet(string modo)                 // grilla detalle del doc. venta
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 13;
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
            dataGridView1.Columns[2].Visible = false;            //  columna visible o no
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
            dataGridView1.Columns[6].Visible = false;            //   columna visible o no
            dataGridView1.Columns[6].HeaderText = "Deta2";    // titulo de la columna
            dataGridView1.Columns[6].Width = 70;                // ancho
            dataGridView1.Columns[6].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].Name = "piedra";
            // acabado 
            dataGridView1.Columns[7].Visible = false;            //   columna visible o no
            dataGridView1.Columns[7].HeaderText = "Acabado";    // titulo de la columna
            dataGridView1.Columns[7].Width = 70;                // ancho
            dataGridView1.Columns[7].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].Name = "descrizionerid";
            // precio
            dataGridView1.Columns[8].Visible = true;            // columna visible o no
            dataGridView1.Columns[8].HeaderText = "Prec.Ind."; // titulo de la columna
            dataGridView1.Columns[8].Width = 60;                // ancho
            dataGridView1.Columns[8].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].Name = "precio";
            // total
            dataGridView1.Columns[9].Visible = true;
            dataGridView1.Columns[9].HeaderText = "Total"; // titulo de la columna
            dataGridView1.Columns[9].Width = 60;                // ancho
            dataGridView1.Columns[9].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[9].Name = "total";
            // tipo Normal o Anticipo
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[10].Name = "NA";
            // valor descuento unitario
            dataGridView1.Columns[11].Visible = true;
            dataGridView1.Columns[11].HeaderText = "Dsct.Tot"; // titulo de la columna
            dataGridView1.Columns[11].Width = 60;                // ancho
            dataGridView1.Columns[11].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[11].Name = "DSCTO";
            // valor unitario sin descuento
            dataGridView1.Columns[12].Visible = true;
            dataGridView1.Columns[12].HeaderText = "Val.Unit"; // titulo de la columna
            dataGridView1.Columns[12].Width = 60;                // ancho
            dataGridView1.Columns[12].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[12].Name = "VAL_SIN_DSCTO";
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
            // autocompletados de departamento, provincia y distrito
            string consulta = "SELECT depart,provin,distri,nombre FROM ubigeos";
            MySqlCommand micon = new MySqlCommand(consulta, conn);
            try
            {
                MySqlDataAdapter daa = new MySqlDataAdapter(micon);
                daa.Fill(dtadpd);
                daa.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener nombres de dptos,provin y distritos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion de local de ventas ... ok
                const string contaller = "select a.descrizionerid,a.idcodice,a.codigo,b.serie,b.dir_pe,b.ubigeo,a.sunat,b.format from desc_ven a " +
                    "left JOIN (select serie,sede,dir_pe,ubigeo,format from series WHERE tipdoc IN ('FT','BV')) b on b.sede=a.idcodice " +
                    "where a.numero=1 AND a.codigo<>'' order by a.idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6).Trim());
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
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
                // seleccion de moneda
                const string conmon = "select descrizionerid,idcodice,codigo,descrizione from desc_mon where numero=1";
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
                // seleccion de los porcentajes de detraccion
                const string condet = "select descrizionerid,idcodice,codigo,descrizione,sunat from desc_sde where numero=1";
                using (MySqlCommand my = new MySqlCommand(condet, conn))
                {
                    using (MySqlDataAdapter dafp = new MySqlDataAdapter(my))
                    {
                        dafp.Fill(dtdetS);
                        foreach (DataRow row in dtdetS.Rows)
                        {
                            cmb_detrac.Items.Add(row.ItemArray[0].ToString());
                        }
                    }
                }
            }
            conn.Close();
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
            limpia_panel(panel1);
            limpia_panel(panel3);
            limpia_panel(pan_tipo);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            Tx_modo.Text = modo;
            if (modo != "NUEVO")
            {
                tx_dat_orig.Text = "";
                tx_nomVen.ReadOnly = true;
                tx_status.Text = "";
                tx_dat_estad.Text = "";
                dtp_pedido.Enabled = false;
                tx_idr.Text = "";
                tx_impMedios.Enabled = true;
                tx_impMedios.ReadOnly = true;
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
                tx_dat_mone.Text = MonDeft;
                cmb_mon_SelectionChangeCommitted(null, null);
                cmb_mon.SelectedItem = tx_dat_mone.Text;
                if (MonTodas == "S") cmb_mon.Enabled = false;
                else cmb_mon.Enabled = true;
                if (vSNdsctoD == "S")
                {
                    tx_ImpDsctoD.ReadOnly = false;
                    tx_ImpDsctoD.Text = "0.00";
                }
                else
                {
                    tx_ImpDsctoD.ReadOnly = true;
                    tx_ImpDsctoD.Text = "0.00";
                }
                tx_desGlob.Text = "0";
                if (vSNdsctoC == "S") tx_desGlob.Enabled = true;
                else tx_desGlob.Enabled = false;
            }
            lb_totDet.Text = lb_totDet.Tag + " " + cmb_mon.Text;
            ini_pagos();
            _docsAnticip.Clear();
        }
        private double jala_cont(string conti)                // jala datos del contrato
        {
            double retorna = 0;
            try
            {
                DataTable dt = new DataTable();
                //dataGridView1.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    string continua = "N";
                    string conpag = "SELECT concat('ANTICIPO Cont.',contrato,'  ',dv,serie,'-',numero) AS deta,moneda,monto,montosol from pagamenti where contrato=@cont";
                    string consin = "select a.saldo,a.status from contrat a where a.contrato=@cont";
                    string consulta = "SELECT a.contratoh,a.item,a.nombre,a.cant,a.medidas,de.descrizione,a.codref,a.piedra,a.precio,a.total,c.cliente," +
                        "ac.tipdoc,ac.RUC,ac.RazonSocial,ac.Direcc1,ac.Direcc2,ac.localidad,ac.Provincia,ac.depart,ac.NumeroTel1,ac.NumeroTel2,ac.EMail,c.valor,a.totdscto " +
                        "FROM detacon a " +
                        "LEFT JOIN desc_est de ON de.IDCodice = a.estado " +
                        "LEFT JOIN contrat c ON c.contrato = a.contratoh " +
                        "LEFT JOIN anag_cli ac ON ac.IDAnagrafica = c.cliente " +
                        "WHERE a.contratoh = @cont";
                    using (MySqlCommand micon = new MySqlCommand(consin, conn))
                    {
                        micon.Parameters.AddWithValue("@cont", conti);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            if (dr.GetDouble(0) <= 0)
                            {
                                MessageBox.Show("El contrato no tiene saldo!" + Environment.NewLine +
                                    "No se puede generar comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                tx_cont.Text = "";
                                tx_cont.Focus();
                            }
                            else
                            {
                                if (dr.GetString(1) == "ANULAD")
                                {
                                    MessageBox.Show("El contrato esta ANULADO!" + Environment.NewLine +
                                        "No se puede generar comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    tx_cont.Text = "";
                                    tx_cont.Focus();
                                }
                                else
                                {
                                    var asd = MessageBox.Show("El saldo del contrato es: " + dr.GetString(0) + Environment.NewLine +
                                        "Desea registrar un pago cancelatorio?","confirme por favor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                                    if (asd == DialogResult.Yes)    // cancela el contrato
                                    {
                                        // ponemos todos los anticipos linea por linea, con su numero de factura
                                        // luego todas las filas del detalle
                                        // EN LA SECCION DE TOTALES
                                        // Sub total   = valor total de la venta (sin igv)    monto que falta pagar sin igv
                                        // Anticipos   = sumatoria de totales anticipos (inc igv)
                                        // Valor Venta = Sub total - anticipos
                                        // Igv         = 18% 
                                        // Importe Tot = valor venta + igv
                                        vpago = "cancelacion";
                                        retorna = double.Parse(dr.GetString(0));
                                    }
                                    else
                                    {                               // hace un pago a cuenta
                                        vpago = "anticipo";
                                    }
                                    continua = "S";
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se tienen datos del contrato!","Error en contrato",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            tx_cont.Text = "";
                            tx_cont.Focus();
                            return retorna;
                        }
                        dr.Dispose();
                    }
                    if (continua == "S")
                    {
                        int cnt = 1;
                        double valCont, valAnti = 0;
                        if (vpago == "cancelacion")
                        {
                            dataGridView1.Rows.Clear();
                            using (MySqlCommand micon = new MySqlCommand(conpag, conn))
                            {
                                micon.Parameters.AddWithValue("@cont", conti);
                                using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                                {
                                    DataTable pdt = new DataTable();
                                    da.Fill(pdt);
                                    foreach (DataRow data in pdt.Rows)  //  deta,moneda,monto,montosol
                                    {
                                        dataGridView1.Rows.Add(cnt, "0", "", data.ItemArray[0].ToString(),
                                            "", "", "", "",data.ItemArray[2].ToString(), data.ItemArray[3].ToString());
                                        cnt += 1;
                                        //toti = toti + double.Parse(data.ItemArray[9].ToString());
                                        valAnti = valAnti + double.Parse(data.ItemArray[3].ToString());
                                    }
                                }
                            }
                        }
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@cont", conti);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                da.Fill(dt);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.Rows.Clear();

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
                            valCont = double.Parse(dt.Rows[0].ItemArray[22].ToString());
                            // detalle
                            grilladet(Tx_modo.Text);
                            double toti = 0;
                            foreach (DataRow data in dt.Rows)
                            {
                                double Dscto = double.Parse(data.ItemArray[23].ToString());
                                double totSinD = double.Parse(data.ItemArray[9].ToString());

                                dataGridView1.Rows.Add(cnt, data.ItemArray[3].ToString(), data.ItemArray[1].ToString(), data.ItemArray[2].ToString(),
                                    data.ItemArray[4].ToString(), data.ItemArray[6].ToString(), data.ItemArray[7].ToString(), data.ItemArray[5].ToString(),
                                    (double.Parse(data.ItemArray[8].ToString()) - Dscto / double.Parse(data.ItemArray[3].ToString())).ToString("#0.00"),
                                    (double.Parse(data.ItemArray[9].ToString()) - Dscto).ToString("#0.00"),"",
                                    Dscto, totSinD);
                                cnt += 1;
                                toti = toti + (double.Parse(data.ItemArray[9].ToString()) - double.Parse(data.ItemArray[23].ToString()));
                            }
                            tx_valor.Text = toti.ToString("#0.00");
                            tx_bruto.Text = (toti / 1.18).ToString("#0.00");
                            tx_igv.Text = (toti - (toti / 1.18)).ToString("#0.00");
                            //
                            if (rb_antic.Checked == true && vpago != "cancelacion")
                            {
                                toti = 0;
                                //tx_d_antic.Text = tx_d_antic.Text + " " + tx_cont.Text;
                                tx_valor.Text = toti.ToString("#0.00");
                                tx_bruto.Text = (toti / 1.18).ToString("#0.00");
                                tx_igv.Text = (toti - (toti / 1.18)).ToString("#0.00");
                                //tx_coment.Text = "*** Comprobante por antipo ***" + tx_coment.Text.Trim();
                            }
                            if (rb_antic.Checked == true && vpago == "cancelacion")
                            {
                                toti = valCont - valAnti;
                                tx_valor.Text = toti.ToString("#0.00");
                                tx_bruto.Text = (toti / 1.18).ToString("#0.00");
                                tx_igv.Text = (toti - (toti / 1.18)).ToString("#0.00");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No existe el contrato!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tx_cont.Text = "";
                            return retorna;
                        }
                        dt.Dispose();
                    }
                    suma_grilla();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en ejecución de código", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return retorna;
            }
            return retorna;
        }
        private bool valProdCont()                          // busca productos de stock, grandes que puedan tener contrato
        {
            bool retorna = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[2].Value.ToString() != "")
                {
                    if (lps.Contains(row.Cells[2].Value.ToString().Substring(0, 1)))
                    {
                        retorna = true;
                    }
                }
            }
            return retorna;
        }
        private void ini_pagos()                            // inicializa la matris de pagos
        {
            for (int i = 0; i < 9; i++)
            {
                dtpagos[i, 0] = "0";
                dtpagos[i, 1] = i.ToString();
                dtpagos[i, 2] = "";
                dtpagos[i, 3] = "";
                dtpagos[i, 4] = "";
                dtpagos[i, 5] = "";
                dtpagos[i, 6] = "";
            }
        }
        private void ini_deta()                             // limpia el detalle y totales
        {
            limpia_panel(panel1);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            tx_subtot.Text = "";
            tx_valor.Text = "";
            tx_bruto.Text = "";
            tx_igv.Text = "";
            tx_tfil.Text = "";
        }
        private void cosas_pagamenti()                      // llena lista de pagos si el comprobante es de cancelacion
        {
            if (tx_tipComp.Text == "C")
            {
                _docsAnticip.Clear();

                string consu = "SELECT a.dv,a.serie,a.numero,a.montosol,ifnull(b.mondvta,'') AS mondvta,ifnull(b.tcadvta,1) AS tcadvta,ifnull(b.subtota,0) AS subtota," +
                "ifnull(b.igvtota, 0) AS igvtota, ifnull(b.totdvta, 0) AS totdvta, ifnull(b.idpse_ose, 0) AS idpse_ose, ifnull(b.pdfpse_ose, '') AS pdfpse_ose," +
                "c.sunat,CONCAT(b.martdve,a.serie) AS serRapi,d.codigo AS codMon,e.sunat AS codDoc,b.nudoclt AS docClie,DATE_FORMAT(b.fechope,'%d/%m/%Y') AS fechope " +
                "FROM pagamenti a LEFT JOIN cabfactu b ON b.tipdvta = a.dv AND b.serdvta = a.serie AND b.numdvta = a.numero " +
                "LEFT JOIN desc_tdv c ON c.IDCodice=b.tipdvta LEFT JOIN desc_mon d ON d.IDCodice=b.mondvta LEFT JOIN desc_doc e ON e.IDCodice=b.tidoclt " +
                "WHERE a.contrato = @cont";   //  AND saldo>0
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (MySqlCommand micon = new MySqlCommand(consu, conn))
                        {
                            micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (dr.GetString(0).Substring(0, 1) + dr.GetString(1) + "-" + dr.GetString(2) != cmb_tipo.Text.Substring(0, 1) + tx_serie.Text + "-" + tx_corre.Text)
                                    {
                                        docsAnticip item = new docsAnticip();
                                        item.comprob = dr.GetString(0).Substring(0, 1) + dr.GetString(1) + "-" + dr.GetString(2);
                                        item.valor = dr.GetString(3);
                                        item.bruto = dr.GetString("subtota");
                                        item.descrip = "ANTICIPO DE CONTRATO " + tx_cont.Text + " - " +
                                            dr.GetString(0).Substring(0, 1) + dr.GetString(1) + "-" + dr.GetString(2);              // la consulta debe obtener estos datos
                                        item.IdCompRapifac = dr.GetInt32("idpse_ose");       // este campo debe crearse en la tabla cabfactu, junto al identificador del pdf
                                        item.igv = dr.GetString("igvtota");                  // 
                                        item.tipDocCod = dr.GetString("sunat");             // codigo sunat para el tipo de comprobante
                                        item.serieRap = dr.GetString("serRapi");            // serie completa del comprobante, ej: F200
                                        item.corRap = dr.GetInt16("numero");               // correlativo del comprobante
                                        item.codMon = dr.GetString("codMon");
                                        item.tDocClte = dr.GetString("codDoc");             // codigo sunat del tipo doc del cliente
                                        item.nroDocC = dr.GetString("docClie");             // ruc dni del cliente
                                        item.fecEmi = dr.GetString("fechope");              // fecha emision del comprobante
                                        /*
                                            AnticiposId = "0",
                                            TipoAfectacionIGVCodigo = ""
                                        */
                                        _docsAnticip.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se completo los datos para" + Environment.NewLine +
                            "la impresión del comprobante de cancelación", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
        }
        private void suma_grilla()
        {
            int tbul = 0;
            double tval = 0;
            double.TryParse(tx_desGlob.Text, out double tdes);
            if (rb_bienes.Checked == true)
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    int a = 0;          // cantidad de bultos
                    double b = 0;       // valor total de la fila incluyendo dscto inc igv
                    int.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out a);
                    double.TryParse(dataGridView1.Rows[i].Cells[9].Value.ToString(), out b);
                    tbul = tbul + a;
                    tval = tval + b;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[10].Value.ToString() == "A")
                    {
                        int a = 0;          // cantidad de bultos
                        double b = 0;       // valor total de la fila incluyendo dscto inc igv
                        int.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out a);
                        double.TryParse(dataGridView1.Rows[i].Cells[9].Value.ToString(), out b);
                        tbul = tbul + a;
                        tval = tval + b;
                    }
                }
            }
            tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
            tx_totcant.Text = tbul.ToString();
            tx_subtot.Text = (tval).ToString("#0.00");
            tx_valor.Text = (tval - tdes).ToString("#0.00");
            double tigv = double.Parse(tx_valor.Text) - (double.Parse(tx_valor.Text) / (1 + (double.Parse(v_igv) / 100)));
            tx_bruto.Text = (double.Parse(tx_valor.Text) - tigv).ToString("#0.00");    // (ntoti / 1.18).ToString("#0.00");
            tx_igv.Text = (tigv).ToString("#0.00");
        }
        private void recalDet()                             // CALCULA valores de precios detalle
        {
            int cant = 0;
            int.TryParse(tx_d_can.Text, out cant);
            double preSin = 0;
            double.TryParse(tx_d_preSinDscto.Text, out preSin);
            double desc = 0;
            double.TryParse(tx_ImpDsctoD.Text, out desc);
            tx_d_ptot.Text = (cant * preSin).ToString("#0.00");
            tx_d_precio.Text = (Math.Round(double.Parse(tx_d_ptot.Text) - desc,6)).ToString("#0.00");
        }

        #region autocompletados
        private void autodptos()
        {
            DataRow[] result = dtadpd.Select("provin='00' AND distri='00'");
            foreach (DataRow row in result)
            {
                adptos.Add(row["nombre"].ToString());
            }
            tx_dpto.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_dpto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_dpto.AutoCompleteCustomSource = adptos;
        }
        private void autoprovi()
        {
            aprovi.Clear();
            DataRow[] result = dtadpd.Select("distri='00' AND depart='" + tx_dat_dpto.Text + "'");  // provin<>'00' AND 
            foreach (DataRow row in result)
            {
                aprovi.Add(row["nombre"].ToString());
            }
            tx_prov.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_prov.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_prov.AutoCompleteCustomSource = aprovi;
        }
        private void autodistr()
        {
            adistr.Clear();
            DataRow[] result;
            if (tx_dat_dpto.Text == "07")   // callao
            {
                result = dtadpd.Select("provin='01' AND depart='" + tx_dat_dpto.Text + "'");  // AND distri='00' 
            }
            else
            {
                result = dtadpd.Select("provin='" + tx_dat_provin.Text + "' AND depart='" + tx_dat_dpto.Text + "'");  // AND distri='00' 
            }
            foreach (DataRow row in result)
            {
                adistr.Add(row["nombre"].ToString());
            }
            tx_dist.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_dist.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_dist.AutoCompleteCustomSource = adistr;
        }
        private void tx_dpto_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_dpto.Text = "";
            if (tx_dpto.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                DataRow[] result = dtadpd.Select("provin='00' AND distri='00' AND nombre='" + tx_dpto.Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    tx_dat_dpto.Text = row["depart"].ToString();
                }
                if (tx_dat_dpto.Text == "")
                {
                    MessageBox.Show("No existe el departamento escrito", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_dpto.Text = "";
                    tx_dpto.Focus();
                    return;
                }
                autoprovi();
            }
        }
        private void tx_prov_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_provin.Text = "";
            if (tx_prov.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                DataRow[] result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND distri='00' AND nombre='" + tx_prov.Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    if (tx_dat_dpto.Text == "07") tx_dat_provin.Text = "01";
                    else tx_dat_provin.Text = row["provin"].ToString();
                }
                if (tx_dat_provin.Text == "")
                {
                    MessageBox.Show("No existe la provincia escrita", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_prov.Text = "";
                    tx_prov.Focus();
                    return;
                }
                autodistr();
            }
        }
        private void tx_dist_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_distri.Text = "";
            DataRow[] result;
            if (tx_dist.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                if (tx_dat_dpto.Text == "07")
                {
                    result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND provin='01' AND nombre='" + tx_dist.Text.Trim() + "'");
                }
                else
                {
                    result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND provin='" + tx_dat_provin.Text.Trim() + "' AND nombre='" + tx_dist.Text.Trim() + "'");
                }
                foreach (DataRow row in result)
                {
                    tx_dat_distri.Text = row["distri"].ToString();
                }
                if (tx_dat_distri.Text == "")
                {
                    MessageBox.Show("No existe el distrito escrito", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_dist.Text = "";
                    tx_dist.Focus();
                    return;
                }
            }
        }

        #endregion autocompletados

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
            escribepan(panel1);
            escribepan(panel2);
            escribepan(panel3);
            escribepan(pan_cli);
            escribepan(pan_tipo);
            cmb_taller.Enabled = false;
            limpia_ini();
            button1.Image = Image.FromFile(img_grab);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            grilladet("NUEVO");
            rb_bienes.Checked = true;
            rb_bienes.PerformClick();         // rb_contado_Click(null, null);
            rb_contado.Checked = true;
            tx_d_med.ReadOnly = true;
            rb_tbienes.Checked = true;
            tx_d_can.ReadOnly = false;
            bt_prev.Enabled = false;
            cmb_tipo.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            sololee(this);
            sololeepan(panel1);
            sololeepan(panel2);
            sololeepan(panel3);
            sololeepan(pan_cli);
            sololeepan(pan_tipo);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpia_ini();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            //cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            //jalaoc("tx_idr");
            cmb_tipo.Enabled = true;
            tx_serie.Enabled = true;
            tx_serie.ReadOnly = false;
            tx_corre.Enabled = true;
            tx_corre.ReadOnly = false;
            //  solo se modifica comentarios
            tx_d_can.ReadOnly = true;
            tx_d_nom.ReadOnly = true;
            tx_d_med.ReadOnly = true;
            tx_coment.Enabled = true;
            tx_coment.ReadOnly = false;
            //
            tx_coment.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            sololee(this);
            sololeepan(panel1);
            sololeepan(panel2);
            sololeepan(panel3);
            sololeepan(pan_cli);
            sololeepan(pan_tipo);
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            limpia_ini();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.Enabled = true;
            tx_serie.Enabled = true;
            tx_serie.ReadOnly = false;
            tx_corre.Enabled = true;
            tx_corre.ReadOnly = false;
            //  solo se modifica comentarios
            tx_d_can.ReadOnly = true;
            tx_d_nom.ReadOnly = true;
            tx_d_med.ReadOnly = true;
            tx_coment.Enabled = true;
            tx_coment.ReadOnly = false;
            bt_prev.Enabled = true;
            cmb_tipo.Focus();
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            sololee(this);
            sololeepan(panel1);
            sololeepan(panel2);
            sololeepan(panel3);
            sololeepan(pan_cli);
            sololeepan(pan_tipo);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;
            limpia_ini();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.Enabled = true;
            tx_serie.Enabled = true;
            tx_serie.ReadOnly = false;
            tx_corre.Enabled = true;
            tx_corre.ReadOnly = false;
            tx_impMedios.ReadOnly = false;
            tx_impMedios.Enabled = true;
            bt_prev.Enabled = true;
            cmb_tipo.Focus();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            if (tx_serie.Text.Trim() != "" && tx_corre.Text.Trim() != "")
            {
                {
                    var aa = MessageBox.Show("Desea imprimir el comprobante?", "Confirme por favor",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        if (true)            // vi_formato == "TK"
                        {
                            if (imprimeTK() == true) updateprint("S");
                            else
                            {
                                MessageBox.Show("Error al imprimir el comprobante" + Environment.NewLine +
                                    "verifique el dispositivo de impresión", "Atención - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_corre.Text != "" && Tx_modo.Text != "")
            {
                string rutaT = rut_pdf + tx_pdf_rapifac.Text;
                string rutXml = rut_xml + tx_pdf_rapifac.Text;
                System.Diagnostics.Process.Start(rutXml);
                System.Diagnostics.Process.Start(rutaT);
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {

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
            limpia_panel(pan_tipo);
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
            limpia_panel(pan_tipo);
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
            limpia_panel(pan_tipo);
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
            limpia_panel(pan_tipo);
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
            rb_tbienes.Checked = false;
            rb_tserv.Checked = false;
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
            cmb_detrac.SelectedIndex = -1;
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
            /*if (cmb_tipo.SelectedValue != null)
            {
                tx_dat_tipdoc.Text = cmb_tipo.SelectedValue.ToString();
            }
            else
            {
                tx_dat_tipdoc.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            } */
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
        private void cmb_tdoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_tdoc.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_tdoc.Text);
                DataRow[] row = dtdoc.Select(axs);
                tx_dat_tdoc.Text = row[0].ItemArray[1].ToString();
                tx_dat_tdoc_s.Text = row[0].ItemArray[3].ToString();
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
                tx_dir_ubigpe.Text = row[0].ItemArray[5].ToString();
                tx_serie.Text = row[0].ItemArray[3].ToString();
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
                tx_dat_monNom.Text = row[0].ItemArray[3].ToString();
            }
        }
        private void cmb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {
            string axs = string.Format("idcodice='{0}'", cmb_mon.Text);
            DataRow[] row = dtmon.Select(axs);
            tx_dat_mone.Text = row[0].ItemArray[1].ToString();
            tx_dat_mon_s.Text = row[0].ItemArray[2].ToString();
            tx_dat_monNom.Text = row[0].ItemArray[3].ToString();
        }
        private void cmb_detrac_SelectedIndexChanged(object sender, EventArgs e)
        {
            string axs = string.Format("descrizionerid='{0}'", cmb_detrac.Text);
            DataRow[] row = dtdetS.Select(axs);
            tx_dat_cDet.Text = row[0].ItemArray[1].ToString();  // codigo interno detraccion
            tx_dat_pDet.Text = row[0].ItemArray[2].ToString();  // porcentaje detraccion
            tx_dat_sDet.Text = row[0].ItemArray[4].ToString();  // codigo sunat 
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
            tx_d_nom.ReadOnly = true;
            tx_d_med.ReadOnly = true;
            tx_d_mad.ReadOnly = false;
            tx_d_precio.ReadOnly = true;        // por defecto 27/10/2022
            tx_d_ptot.ReadOnly = true;
            if (Tx_modo.Text == "NUEVO") 
            {
                if (tx_d_codi.Text.Trim() != "")
                {
                    if (tx_d_codi.Text.Substring(0, 1) == v_liav || tx_d_codi.Text.Substring(0, 1) == itemSer)   // articulos varios que no tienen stock
                    {
                        tx_d_nom.ReadOnly = false;
                        tx_d_med.ReadOnly = true;
                        tx_d_mad.ReadOnly = true;
                        tx_d_preSinDscto.ReadOnly = false;
                    }
                    else
                    {
                        if (tx_d_codi.Text.Substring(1, 3) == "000")
                        {
                            tx_d_nom.ReadOnly = false;
                            tx_d_med.ReadOnly = false;
                            tx_d_mad.ReadOnly = false;
                            tx_d_preSinDscto.ReadOnly = false;
                        }
                        else
                        {
                            tx_d_preSinDscto.ReadOnly = true;
                            if (v_cnprd == "S") // ser permite cambiar nombres para efecto del comprobante? S=si | N=no
                            {
                                tx_d_nom.ReadOnly = false;
                                tx_d_med.ReadOnly = false;
                                tx_d_mad.ReadOnly = false;
                            }
                        }
                    }
                }
            }
        }
        private void tx_cont_Leave(object sender, EventArgs e)                // valida contrato y jala los datos
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (rb_antic.Checked == true && tx_cont.Text.Trim() != "")
                {
                    // mostramos una ventana alertando del saldo del contrato y preguntando si se desea cancelar todo
                    double aja = jala_cont(tx_cont.Text);    // segun pague todo o parcial hacemos algo 
                    if (vpago == "cancelacion")
                    {
                        double ntoti = aja;
                        tx_d_antic.Visible = false;
                        tx_d_valAntic.Visible = false;
                        tx_coment.Text = "*** Comprobante de Cancelación ***";
                        tx_tipComp.Text = "C";
                        tx_d_antic.Text = "CANCELACION ";
                        tx_d_valAntic.Text = ntoti.ToString("#0.00");
                        //
                        if (ntoti > 0)
                        {
                            dataGridView1.Rows.Insert(0, dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_antic.Text, tx_d_med.Text,
                                        tx_d_mad.Text, tx_dat_mad.Text, "", string.Format("{0:#0.00}", ntoti.ToString("#0.00")), ntoti.ToString("#0.00"), "A");
                            tx_subtot.Text = (ntoti).ToString("#0.00");
                            tx_valor.Text = (ntoti - double.Parse(tx_desGlob.Text)).ToString("#0.00");
                            tx_bruto.Text = (double.Parse(tx_valor.Text) / 1.18).ToString("#0.00");
                            tx_igv.Text = (double.Parse(tx_valor.Text) - (double.Parse(tx_valor.Text) / 1.18)).ToString("#0.00");
                        }
                        tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
                        tx_desGlob.ReadOnly = false;
                    }
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
                if (rb_tbienes.Checked == false && rb_tserv.Checked == false) rb_tbienes.Checked = true;
                if (rb_bienes.Checked == false && rb_antic.Checked == false) rb_bienes.Checked = true; 
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
                                if (biene[1] == "00000000")     // cliente no identificado
                                {
                                    System.Diagnostics.Process.Start(webdni);    // "https://eldni.com/pe/buscar-por-dni"
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
        internal void tx_corre_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_corre.Text != "")
            {
                string td = tx_dat_tipdoc.Text;
                string sd = tx_serie.Text;
                string nd = tx_corre.Text;
                limpia_ini();
                tx_dat_tipdoc.Text = td;
                string axs = string.Format("idcodice='{0}'", tx_dat_tipdoc.Text);
                DataRow[] row = dtpedido.Select(axs);
                cmb_tipo.SelectedItem = row[0].ItemArray[1].ToString();
                tx_serie.Text = sd;
                tx_corre.Text = nd;
                tx_dat_tipdoc_s.Text = row[0].ItemArray[2].ToString();
                jalaoc("tx_corre");
            }
        }
        private void tx_d_can_Leave(object sender, EventArgs e)
        {
            recalDet();
        }
        private void tx_d_preSinDscto_Leave(object sender, EventArgs e)
        {
            recalDet();
        }
        private void tx_ImpDsctoD_Leave(object sender, EventArgs e)
        {
            if (tx_ImpDsctoD.Text.Trim() == "") tx_ImpDsctoD.Text = "0";
            recalDet();
        }
        private void tx_desGlob_Leave(object sender, EventArgs e)
        {
            if (tx_desGlob.Text.Trim() == "") tx_desGlob.Text = "0";
            double aa = double.Parse(tx_subtot.Text);
            double bb = double.Parse(tx_desGlob.Text);
            double cc = aa - bb;
            double ti = cc - cc / (1 + (double.Parse(v_igv) / 100));

            tx_valor.Text = (cc).ToString("#0.00");
            tx_bruto.Text = (cc - ti).ToString("#0.00");
            tx_igv.Text = (ti).ToString("#0.00");

            if (rb_tserv.Checked == true && double.Parse(tx_valor.Text) > DetServLim)
            {
                panel4.Visible = true;
            }
            else panel4.Visible = false;
        }

        #endregion leaves;

        #region radio_buttons
        private void rb_bienes_Click(object sender, EventArgs e)
        {
            if (rb_bienes.Checked == true)
            {
                tx_tipComp.Text = "B";
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
                tx_d_preSinDscto.Visible = true;
                tx_d_ptot.Visible = true;
                tx_ImpDsctoD.Visible = true;
                //
                lb_cont.Visible = false;
                tx_cont.Visible = false;
                //
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells[10].Value != null && item.Cells[10].Value.ToString() == "A") dataGridView1.Rows.RemoveAt(item.Index);
                }
                double ntoti = 0;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells[8].Value != null) ntoti = ntoti + double.Parse(item.Cells[8].Value.ToString());
                }
                tx_subtot.Text = "";
                tx_valor.Text = ""; // (ntoti).ToString("#0.00");
                tx_bruto.Text = ""; // ((ntoti) / 1.18).ToString("#0.00");
                tx_igv.Text = "";   // ((double.Parse(tx_valor.Text)) - ((double.Parse(tx_valor.Text)) / 1.18)).ToString("#0.00");
                suma_grilla();
                // DESCUENTO GLOBAL
                // si es vta. directa -> se habilita el dscto global
                tx_desGlob.ReadOnly = false;
            }
        }
        private void rb_antic_Click(object sender, EventArgs e)
        {
            if (rb_antic.Checked == true)
            {
                tx_tipComp.Text = "A";
                // ocultamos objetos del panel1, menos el boton 
                tx_d_it.Visible = false;
                tx_d_can.Visible = false;
                tx_d_codi.Visible = false;
                tx_d_nom.Visible = false;
                tx_d_med.Visible = false;
                tx_d_mad.Visible = false;
                tx_d_precio.Visible = false;
                tx_d_preSinDscto.Visible = false;
                tx_d_ptot.Visible = false;
                tx_ImpDsctoD.Visible = false;
                //
                tx_d_antic.Left = 3;    // 28
                tx_d_antic.Top = 5;
                tx_d_antic.Width = 727;
                tx_d_antic.Height = 45; // 40
                tx_d_antic.Multiline = true;
                tx_d_antic.Visible = true;
                tx_d_antic.Text = letiden;
                //
                tx_d_valAntic.Left = 728;
                tx_d_valAntic.Top = 5;
                tx_d_valAntic.Height = 45;  // 40
                tx_d_valAntic.Multiline = true;
                tx_d_valAntic.Visible = true;
                //
                // DESCUENTO GLOBAL
                tx_desGlob.Text = "0";
                tx_desGlob.ReadOnly = true;
                if (Tx_modo.Text == "NUEVO")
                {
                    tx_subtot.Text = "";
                    tx_valor.Text = "";
                    tx_bruto.Text = "";
                    tx_igv.Text = "";
                    //
                    tx_coment.Text = "*** Comprobante por antipo ***";
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
        private void rb_tbienes_Click(object sender, EventArgs e)
        {
            ini_deta();
            panel4.Visible = false;
            tx_dat_pDet.Text = "";
            tx_dat_cDet.Text = "";
        }
        private void rb_tserv_Click(object sender, EventArgs e)
        {
            ini_deta();
            panel4.Visible = false;
            tx_dat_pDet.Text = "";
            tx_dat_cDet.Text = "";
        }
        #endregion

        #region advancedatagridview

        #endregion

        #region datagridview1 - grilla detalle del doc.venta
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /* NO VAN EDICIONES EN FACTURACION, SE BORRA LA LINEA Y SE VUELVE A AGREGAR OTRA .... 28/11/2022
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
            */
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
                    suma_grilla();
                }
            }
        }
        #endregion

        #region botones de grabar y agregar
        private void bt_det_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                double ntoti = 0;
                double ncant = 0;
                double.TryParse(tx_valor.Text, out double tv);

                if (rb_antic.Checked == true && tx_d_valAntic.Visible == true)
                {
                    if (tx_d_valAntic.Text != "")   //  && tx_cont.Text.Trim() == ""
                    {
                        double.TryParse(tx_d_valAntic.Text, out ntoti);
                        if (ntoti > 0)
                        {
                            dataGridView1.Rows.Insert(0, dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_antic.Text, tx_d_med.Text,
                                        tx_d_mad.Text, tx_dat_mad.Text, "", string.Format("{0:#0.00}", ntoti.ToString("#0.00")), ntoti.ToString("#0.00"), "A", "0", "0");

                        }
                        tx_d_valAntic.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Ingrese el valor del anticipo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_valAntic.Focus();
                        //rb_antic.Focus();
                        return;
                    }
                    // DESCUENTO GLOBAL
                    // si es vta. directa -> se habilita el dscto global
                    // si es cancelación -> se habilita el dscto global
                    // si anticipo NO se permite dscto global
                    tx_desGlob.ReadOnly = false;
                    if (tx_cont.Text.Trim() == "")
                    {
                        tx_desGlob.ReadOnly = true;
                        tx_desGlob.Text = "0";
                    }
                }
                if (rb_bienes.Checked == true)
                {
                    // validaciones
                    if (tx_d_can.Text.Trim() == "")
                    {
                        MessageBox.Show("Ingrese la cantidad", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_can.Focus();
                        return;
                    }
                    if (tx_d_codi.Text.Trim() == "")
                    {
                        MessageBox.Show("Seleccione un artículo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_codi.Focus();
                        return;
                    }
                    ncant = double.Parse(tx_d_can.Text);
                    ntoti = double.Parse(tx_d_precio.Text) / ncant;                         // precio total de la fila incluyendo descuento
                    double vdscto = double.Parse(tx_ImpDsctoD.Text);
                    if (ntoti > 0)
                    {
                        if (tx_d_codi.Text.Substring(0, 1) == v_liav)  // articulos varios
                        {
                            _ = dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                                        tx_d_mad.Text, tx_dat_mad.Text, "", string.Format("{0:#0.00}", (ntoti).ToString("#0.00")), (ntoti * ncant).ToString("#0.00"), "N", vdscto.ToString(), tx_d_preSinDscto.Text); // (ntoti+vdscto).ToString()
                        }
                        else
                        {
                            _ = dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                                        tx_d_mad.Text, tx_dat_mad.Text, "", string.Format("{0:#0.00}", ntoti.ToString("#0.00")), (ntoti * ncant).ToString("#0.00"), "N", vdscto.ToString(), tx_d_preSinDscto.Text);  // (ntoti + vdscto).ToString()
                        }

                        limpia_panel(panel1);
                    }
                    else
                    {
                        MessageBox.Show("Ingrese el precio de venta", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_d_precio.Focus();
                        return;
                    }
                    // DESCUENTO GLOBAL
                    // si es vta. directa -> se habilita el dscto global
                    tx_desGlob.ReadOnly = false;
                }
                tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
                suma_grilla();
                if (rb_tserv.Checked == true && double.Parse(tx_valor.Text) > DetServLim)
                {
                    panel4.Visible = true;
                }
                else panel4.Visible = false;
            }
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
                MessageBox.Show("Seleccione el tipo de documento del cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_tdoc.Focus();
                return;
            }
            if (tx_ndc.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el documento del cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_ndc.Focus();
                return;
            }
            //if (conex_Rapifac() == "") return;   //
            if (Tx_modo.Text == "NUEVO")
            {
                // validaciones 
                if (dataGridView1.Rows.Count < 2)
                {
                    MessageBox.Show("Ingrese al menos un producto", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_d_can.Focus();
                    return;
                }
                if (tx_impMedios.Text != tx_valor.Text)
                {
                    MessageBox.Show("El importe en medios de pago debe" + Environment.NewLine +
                        "ser igual al valor del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_impMedios.Focus();
                    return;
                }
                if (tx_direc.Text.Trim().Length < 8 && tx_dat_tipdoc.Text == codfact)
                {
                    MessageBox.Show("Es obligatorio registrar la dirección", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_direc.Focus();
                    return;
                }
                if (tx_direc.Text.Trim().Length < 8 && tx_dat_tipdoc.Text == codbole && double.Parse(tx_valor.Text) > double.Parse(valdirec))
                {
                    MessageBox.Show("Es obligatorio registrar la dirección", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_direc.Focus();
                    return;
                }
                if (rb_antic.Checked == true && dataGridView1.Rows.Count <= 2)
                {
                    MessageBox.Show("Debe ingresar el detalle de la venta" + Environment.NewLine + 
                        "los anticipos deben tener un detalle previo","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    rb_bienes.Focus();
                    return;
                }

                // verificamos si el comprobante tiene items "grandes" que podrían tener contrato ... estos se deben grabar el pago en la tabla pagamenti
                if (valProdCont() == true) tx_prdsCont.Text = "S";
                else tx_prdsCont.Text = "N";

                // validaciones de servicios y detraccion
                if (rb_tserv.Checked == true && double.Parse(tx_valor.Text) > DetServLim)
                {
                    if (tx_dat_cDet.Text == "")
                    {
                        MessageBox.Show("Debe seleccionar un porcentaje" + Environment.NewLine +
                        "de Detracción para el servicio", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_detrac.Focus();
                        return;
                    }
                }

                var aa = MessageBox.Show(" Confirma que desea CREAR " + Environment.NewLine +
                    "el comprobante?","Confirme por favor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    cosas_pagamenti();
                    string resultado = "";
                    // armado de las clases para Rapifac
                    if (rb_bienes.Checked == true)      // venta directa
                    {
                        resultado = rapifac_vtaDirecta(); // conex_Rapifac();
                    }
                    if (rb_antic.Checked == true)       // anticipo o cancelacion
                    {
                        if (tx_cont.Text != "" && tx_tipComp.Text == "C")   // cancelacion
                        {
                            resultado = rapifac_cancelacion();
                        }
                        else
                        {
                            // anticipo
                            resultado = rapifac_anticipo();    // conex_Rapifac();
                        }
                    }
                    if (resultado == "")
                    {
                        MessageBox.Show("Lo sentimos, en este momento no se tiene conexión" + Environment.NewLine +
                            "con el proveedor OSE/PSE. Confirme que tenga internet." + Environment.NewLine +
                            "No se puede grabar el comprobante.",
                            "Falla en Facturación Electrónica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        string cabeza = resultado;  // JsonConvert.SerializeObject(resultado);
                        string token = conex_token();
                        if (token != "")
                        {
                            // datos variables para la emisión
                            string host = "http://wsventas-exp.rapifac.com/v0/comprobantes/series?sucursal=" + tx_codSuc.Text.Trim();

                            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
                            httpWebRequest.ContentType = "application/json";         // , text/javascript, */*; q=0.01"
                            httpWebRequest.Method = "GET";
                            httpWebRequest.Headers.Add("Authorization", "bearer " + token);

                            string serComp = "";                                    // obtiene el correlativo para la sede y serie
                            string numComp = "";

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                                JArray v = JArray.Parse(result);
                                var items = v.Where(x => x["TipoDocumento"].ToString() == tx_dat_tipdoc_s.Text).ToList();
                                serComp = items[0].SelectToken("Serie").ToString();
                                numComp = items[0].SelectToken("Correlativo").ToString();
                                tx_serie.Text = serComp;
                                //tx_corre.Text = numComp;
                            }
                            // emite el comprobante
                            host = "http://wsventas-exp.rapifac.com/v0/comprobantes?IncluirCDR=1";
                            httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Headers.Add("Authorization", "bearer " + token);
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                streamWriter.Write(cabeza);
                                // escribimos el json del comprobantes para efectos de prueba
                                System.IO.File.WriteAllText(@"c:\temp\" + tx_serie.Text + "-" + numComp + ".json", cabeza);
                            }

                            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                try
                                {
                                    var result = streamReader.ReadToEnd();
                                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
                                    string[] resA = myDeserializedClass.xml_pdf.Mensaje.Split('-');
                                    tx_serie.Text = resA[1].Substring(1, (resA[1].Length - 1));
                                    tx_corre.Text = resA[2];
                                    tx_id_rapifac.Text = myDeserializedClass.xml_pdf.IDComprobante.ToString();
                                    tx_pdf_rapifac.Text = myDeserializedClass.xml_pdf.IDRepositorio;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message,"Error en emitir comprobante",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lo sentimos, ocurrió un ERROR con el OSE/PSE" + Environment.NewLine +
                            "no se pudo generar el comprobante electrónico",
                            "Error en Facturación Electrónica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    // despues de terminado todo en rapifac, grabamos en nuestra base de datos
                    if (graba() == true)
                    {
                        // vemos el local y documento para saber si mostramos el pdf o imprimimos el TK
                        if (true)
                        {
                            string axs = string.Format("idcodice='{0}'", tx_dat_orig.Text);
                            DataRow[] row = dttaller.Select(axs);
                            if (row[0].ItemArray[7] == null || row[0].ItemArray[7].ToString().Trim() == "") Bt_print.PerformClick();
                            else
                            {
                                if (row[0].ItemArray[7].ToString() == "TK") Bt_print.PerformClick();
                                else
                                {
                                    if (row[0].ItemArray[7].ToString() == "A4")
                                    {
                                        string rutaT = rut_pdf + tx_pdf_rapifac.Text;  // tx_id_rapifac.Text;
                                        System.Diagnostics.Process.Start(rutaT);
                                    }
                                    else { Bt_print.PerformClick(); }
                                }
                            }
                        }
                        //
                        if (tx_prdsCont.Text == "S" && tx_cont.Text.Trim() == "")
                        {
                            aa = MessageBox.Show("Desea generar contrato relacionado al" + Environment.NewLine +
                                "presente comprobante?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (aa == DialogResult.Yes)
                            {
                                contratos ncont = new contratos();
                                ncont.Show(this);
                                ncont.Bt_add.PerformClick();
                                ncont.jalaDatFact("T", (tx_dat_tipdoc.Text == codfact) ? "F" : "B", tx_serie.Text, tx_corre.Text,"1");
                                ncont.tx_mc.Text = (tx_dat_tipdoc.Text == codfact) ? "F" : "B";
                                ncont.tx_serie.Text = tx_serie.Text;
                                ncont.tx_corre.Text = tx_corre.Text;
                                string xxx = (tx_dat_tipdoc.Text == codfact) ? "F" : "B";
                                ncont._comprobantes.Add(xxx + "-" + tx_serie.Text + "-" + tx_corre.Text);
                                
                                // APLICACION DEL DSCTO GLOBAL EN EL CONTRATO ... AUN NO VA 29/11/2022
                                //double ccc = 0;
                                //double.TryParse(ncont.tx_dscto.Text, out ccc);
                                //ncont.tx_dscto.Text = (ccc + double.Parse(tx_desGlob.Text)).ToString();
                            }
                        }
                    }
                }
                else return;
            }
            if (Tx_modo.Text == "ANULAR")
            {
                // validaciones antes de anular
                if (dtp_pedido.Value.Date != DateTime.Now.Date)     // Rapifac solo permite anular comprobantes del día   10/08/2022
                {
                    MessageBox.Show("No se permite anular fuera del día","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                var aa = MessageBox.Show(" Confirma que desea ANULAR " + Environment.NewLine +
                    "el comprobante?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    // validaciones de conex con Rapifac
                    if (anula_Rapifac() == false)
                    {
                        MessageBox.Show("Lo sentimos, en este momento no se tiene conexión" + Environment.NewLine +
                            "con el proveedor OSE/PSE. Confirme que tenga internet." + Environment.NewLine +
                            "No se puede anular el comprobante.",
                            "Falla en Facturación Electrónica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // despues de terminado todo en rapifac, grabamos en nuestra base de datos
                    if (anula() == false)
                    {
                        MessageBox.Show("No se pudo anular comprobante internamente","Error en integrador",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                else return;
            }
            if (Tx_modo.Text == "EDITAR")
            {
                // NO HABILATAMOS ESTA OPCION, NO HAY NADA QUE EDITAR
            }
            limpia_ini();
            tx_serie.Focus();
        }
        private bool graba()                                // graba cabecera del comprobante
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
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
                    "referen1,ubipdest,conPago,contrato,vendedor,muebles,idpse_ose,pdfpse_ose,tipoComp," +
                    "verApp,userc,fechc,diriplan4,diripwan4,netbname) values (" +
                    "@fechop,@mtdvta,@ctdvta,@serdv,@numdv,@tcdvta,@tdcrem,@ndcrem,@nomrem,@dircre,@dptocl,@provcl,@distcl,@ubicre,@mailcl,@telec1,@telec2," +
                    "@ldcpgr,@didegr,@ubdegr,@obsprg,@canfil,@totcpr,@monppr,@tcoper,@subpgr,@igvpgr,@porcigv,@totpgr,@pagpgr,@salxpa,@estpgr,@frase1," +
                    "@ticlre,@m1clte,@tipacc,@impSN,@codMN,@subMN,@igvMN,@totMN,@pagaut,@tipdco,@idcaj,@plazc,@pordesc,@valdesc," +
                    "@refer,@updest,@conpag,@cont,@vende,@mueb,@idpse,@pdfpse,@tipcom," +
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
                    micon.Parameters.AddWithValue("@tipacc", tx_dat_cDet.Text);                  // codigo tipo detraccion 
                    micon.Parameters.AddWithValue("@impSN", "S");                               // impreso? S, N
                    micon.Parameters.AddWithValue("@codMN", MonDeft);                  // codigo moneda local
                    micon.Parameters.AddWithValue("@subMN", subtMN);
                    micon.Parameters.AddWithValue("@igvMN", igvtMN);
                    micon.Parameters.AddWithValue("@totMN", fletMN);
                    micon.Parameters.AddWithValue("@pagaut", "S");                  // todos los comprobantes nacen pagados
                    micon.Parameters.AddWithValue("@tipdco", tx_tipComp.Text);    // (rb_antic.Checked == true)? "A" : "B"
                    micon.Parameters.AddWithValue("@idcaj", "0");                   // aca no manejamos caja
                    micon.Parameters.AddWithValue("@plazc", "");                    // aca no hay plazo  de credito...todo es contado
                    micon.Parameters.AddWithValue("@pordesc", "0");                 // los precios ya tienen descuento incluido, el operador pone precio
                    micon.Parameters.AddWithValue("@valdesc", tx_desGlob.Text);                 // a partir de reunion con isacco y pedro en argentina, si van descuentos en detalle
                    micon.Parameters.AddWithValue("@refer", "");    // tx_dat_plazo.Text
                    micon.Parameters.AddWithValue("@updest", "");
                    micon.Parameters.AddWithValue("@conpag", "1");                  // todos son contado
                    micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                    micon.Parameters.AddWithValue("@vende", tx_nomVen.Text);
                    micon.Parameters.AddWithValue("@mueb", tx_prdsCont.Text);
                    micon.Parameters.AddWithValue("@idpse", tx_id_rapifac.Text);
                    micon.Parameters.AddWithValue("@pdfpse", tx_pdf_rapifac.Text);
                    micon.Parameters.AddWithValue("@tipcom", (rb_bienes.Checked == true)? "B" : "S");
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
                        if (row.Cells[2].Value != null && row.Cells[2].Value.ToString() == "")      // anticipo
                        {
                            string inserd2 = "update detfactu set " +
                                "contrato=@cont,descpro=@desc,codMN=@cmnn,precio=@pret,totalMN=@tgrmn,pagauto=@pagaut,estadoser=@esta,dscto=@dscto " +
                                "where tipdocvta=@tdv and serdvta=@sdv and numdvta=@cdv and filadet=@fila";
                            using (MySqlCommand micon = new MySqlCommand(inserd2, conn))
                            {
                                micon.CommandTimeout = 60;
                                micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                                micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                                micon.Parameters.AddWithValue("@cdv", tx_corre.Text);
                                micon.Parameters.AddWithValue("@fila", fila);
                                micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                                micon.Parameters.AddWithValue("@desc", row.Cells[3].Value.ToString());
                                micon.Parameters.AddWithValue("@cmnn", MonDeft);
                                micon.Parameters.AddWithValue("@pret", decimal.Parse(row.Cells[8].Value.ToString()));
                                micon.Parameters.AddWithValue("@tgrmn", decimal.Parse(row.Cells[9].Value.ToString()));
                                micon.Parameters.AddWithValue("@pagaut", "S");
                                micon.Parameters.AddWithValue("@esta", codCanc);        // todos los comprob. nacen cancelados
                                micon.Parameters.AddWithValue("@dscto", 0); // decimal.Parse(row.Cells[11].Value.ToString())
                                micon.ExecuteNonQuery();
                                fila += 1;
                                //
                                retorna = true;         // no hubo errores!
                            }
                        }
                        else
                        {
                            string inserd2 = "update detfactu set " +
                                "contrato=@cont,cantbul=@bult,codprod=@citem,unimedp=@unim,descpro=@desc,pesogro=@peso,medidas=@medid,madera=@mader,acabado=@acaba," +
                                "codmad=@codm,detpied=@detp,codMN=@cmnn,precio=@pret,totalMN=@tgrmn,pagauto=@pagaut,estadoser=@esta,dscto=@vesta,totSinDscto=totalMN+dscto " +
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
                                micon.Parameters.AddWithValue("@mader", "");    // la madera verdadera debe seleccionarse en el contrato
                                micon.Parameters.AddWithValue("@acaba", row.Cells[7].Value.ToString());
                                micon.Parameters.AddWithValue("@codm", row.Cells[5].Value.ToString());
                                micon.Parameters.AddWithValue("@detp", row.Cells[6].Value.ToString());
                                micon.Parameters.AddWithValue("@cmnn", MonDeft);
                                micon.Parameters.AddWithValue("@pret", decimal.Parse(row.Cells[8].Value.ToString()));
                                micon.Parameters.AddWithValue("@tgrmn", decimal.Parse(row.Cells[9].Value.ToString()));
                                micon.Parameters.AddWithValue("@pagaut", "S");
                                micon.Parameters.AddWithValue("@esta", codCanc);        // todos los comprob. nacen cancelados
                                micon.Parameters.AddWithValue("@vesta", decimal.Parse(row.Cells[11].Value.ToString()));  // (row.Cells[11].Value == null || row.Cells[11].Value == DBNull.Value) ? 0 : decimal.Parse(row.Cells[11].Value.ToString())
                                micon.ExecuteNonQuery();
                                fila += 1;
                                //
                                retorna = true;         // no hubo errores!
                            }
                        }
                    }
                }
                // medios de pago
                for (int i=0; i < 9; i++)
                {
                    if (dtpagos[i, 0] != null && dtpagos[i, 2].ToString() != "")
                    {
                        string inpag = "insert into adifactpag (idc,tdvta,sdvta,ndvta,it,medio,operac,importe,codpag,fpago) values (" +
                            "@idc,@tdv,@sdv,@ndv,@it,@med,@ope,@imp,@cpa,@fpa)";
                        using (MySqlCommand micon = new MySqlCommand(inpag, conn))
                        {
                            micon.Parameters.AddWithValue("@idc", 0);
                            micon.Parameters.AddWithValue("@tdv", tx_dat_tipdoc.Text);
                            micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                            micon.Parameters.AddWithValue("@ndv", tx_corre.Text);
                            micon.Parameters.AddWithValue("@it", (i + 1).ToString());
                            micon.Parameters.AddWithValue("@med", dtpagos[i, 2].ToString());
                            micon.Parameters.AddWithValue("@ope", dtpagos[i, 3].ToString());
                            micon.Parameters.AddWithValue("@imp", dtpagos[i, 4].ToString());
                            micon.Parameters.AddWithValue("@cpa", dtpagos[i, 5].ToString());
                            micon.Parameters.AddWithValue("@fpa", dtpagos[i, 6].ToString().Substring(6, 4) + "-" + dtpagos[i, 6].ToString().Substring(3, 2) + "-" + dtpagos[i, 6].ToString().Substring(0, 2));    // dd/mm/aaaa
                            micon.ExecuteNonQuery();
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
            if (conn.State == ConnectionState.Open)
            {
                string canul = "update cabfactu set estdvta=@estser,obsdvta=@obse,usera=@asd,fecha=now()," +
                            "verApp=@veap,diriplan4=@dil4,diripwan4=@diw4,netbname=@nbnp,estintreg=@eiar " +
                            "where id=@idr";
                using (MySqlCommand micon = new MySqlCommand(canul, conn))
                {
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.Parameters.AddWithValue("@estser", estanu);
                    micon.Parameters.AddWithValue("@obse", tx_coment.Text);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@dil4", lib.iplan());
                    micon.Parameters.AddWithValue("@diw4", "");   // Program.vg_ipwan
                    micon.Parameters.AddWithValue("@nbnp", Environment.MachineName);
                    micon.Parameters.AddWithValue("@veap", "");     // verapp
                    micon.Parameters.AddWithValue("@eiar", "A0");  // codigo anulacion interna en DB A0
                    micon.ExecuteNonQuery();
                }
                retorna = true;
            }
            else
            {
                MessageBox.Show("Se perdió conexión al servidor", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
            return retorna;
        }
        #endregion

        #region Fact-Electrónica RAPIFAC
        private string conex_token()                                                // obtenemos el token de rapifac
        {
            string retorna = "";
            string host = "http://wsoauth-exp.rapifac.com/oauth2/token";

            // create a request
            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.Method = "POST"; 
            string postData = "grant_type=password&username=" + usuaDni + usuaRuc + "&password=" + clave + "&client_id=" + id_clte;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(postData);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = bytes.Length;
            Stream newStream = httpWebRequest.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var masticado = JObject.Parse(result);
                    retorna = masticado["access_token"].ToString();
                    if (retorna == null)
                    {
                        retorna = recon_rapifac(masticado["refresh_token"].ToString(), host, id_clte);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return retorna;
        }
        private string recon_rapifac(string fresco,string host,string id_clte)      // si el token expiro, pedimos otro
        {
            string retorna = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.Method = "POST";
            string postData = "grant_type=refresh_token&refresh_token=" + fresco + "client_id = " + id_clte;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(postData);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = bytes.Length;
            Stream newStream = httpWebRequest.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var masticado = JObject.Parse(result);
                retorna = masticado["access_token"].ToString();
                if (retorna == null)
                {
                    recon_rapifac(masticado["refresh_token"].ToString(), host, id_clte);
                }
            }
            return retorna;
        }
        private string rapifac_vtaDirecta()
        {
            string retorna = "";
            int cta_ron = 1;
            decimal v_totDscto = 0;
            decimal v_preToti = 0;
            decimal v_dgloSin = decimal.Parse(tx_desGlob.Text) / (1 + (decimal.Parse(v_igv) / 100));
            decimal v_dgporc = Math.Round(decimal.Parse(tx_desGlob.Text) * 100 / decimal.Parse(tx_subtot.Text), 2);
            List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();
            foreach (DataGridViewRow ron in dataGridView1.Rows)
            {
                List<ProductoPrecioDTO> ccc = new List<ProductoPrecioDTO>();
                {
                    if (ron.Cells[1].Value != null)
                    {
                        ProductoPrecioDTO dlp = new ProductoPrecioDTO
                        {
                            PrecioId = cta_ron,
                            PrecioConfiguracion = 1,
                            ProductoCod = ron.Cells[2].Value.ToString(),
                            CodigoUnidadMedida = (rb_tbienes.Checked == true) ? cod_umed : cod_user,              // 
                            DescripcionUnidadMedida = (rb_tbienes.Checked == true) ? nom_umed : nom_user,         // 
                            CantidadUnidadMedida = int.Parse(ron.Cells[1].Value.ToString()),
                            MonedaCodigo = tx_dat_mon_s.Text,       // "PEN",
                            SucursalId = tx_codSuc.Text,
                            Margenganancia = 0,
                            MargenPorcentaje = 0,
                            PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),
                            Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),
                            OtrosCargosPorcentaje = 0,
                            CantidadAplicable = 0,
                            FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            Estado = 1,
                            ISCPorcentaje = 0,
                            ISCCalculado = 0,
                            //Extension = { };
                            UUID = ""
                        };
                        ccc.Add(dlp);
                    }
                }
                if (ron.Cells[1].Value != null)
                {
                    int v_cant = int.Parse(ron.Cells[1].Value.ToString());                    // cantidad
                    decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                        (decimal.Parse(ron.Cells[9].Value.ToString()) /
                        ((decimal.Parse(v_igv) / 100) + 1));
                    v_preToti = decimal.Parse(ron.Cells[12].Value.ToString()) * v_cant;         // 
                    decimal v_dsctofila = 0, v_dsctofsin = 0;
                    decimal v_dsctobase = 0;
                    decimal v_dsctoNume = 0;
                    string v_dsctoLetr = "0.00";                                                // % dscto en letras
                    decimal v_preUmdes = decimal.Parse(ron.Cells[12].Value.ToString());           // precio individual con descuento
                    decimal v_valorUnit = v_preUmdes / ((decimal.Parse(v_igv) / 100) + 1);
                    decimal v_valTotal = v_valorUnit * v_cant;
                    decimal v_dsctoGlob = decimal.Parse(tx_desGlob.Text) / (dataGridView1.Rows.Count - 1) / v_cant;
                    if (ron.Cells[11].Value.ToString() != "" && ron.Cells[11].Value.ToString() != "0" && ron.Cells[11].Value.ToString() != "0.00")
                    {
                        v_dsctofila = decimal.Parse(ron.Cells[11].Value.ToString());        // descuento total fila
                        v_dsctofsin = v_dsctofila / ((decimal.Parse(v_igv) / 100) + 1);     // descuento fila sin igv
                        v_dsctobase = v_valTotal + v_dsctofsin;                             // valor total fila sin igv + dscto total fila
                        v_dsctoNume = v_dsctofila / v_cant; // (v_dsctofila * 100) / v_dsctobase;                    // descuento en numero
                        v_dsctoLetr = Math.Round(100 - (((v_preUmdes - v_dsctoNume) * v_cant) * 100 / v_preToti), 2).ToString();  // v_dsctoNume.ToString();
                        v_totDscto = v_totDscto + v_dsctofila;
                    }
                    CComprobanteDetalle det = new CComprobanteDetalle
                    {
                        ID = 0,
                        ComprobanteID = 0,
                        Item = cta_ron,
                        TipoProductoCodigo = "",
                        ProductoCodigo = ron.Cells[2].Value.ToString(),   // "Prod00005",
                        ProductoCodigoSUNAT = "",                       // "56101532",
                        TipoSistemaISCCodigo = "00",
                        UnidadMedidaCodigo = (rb_tbienes.Checked == true) ? cod_umed : cod_user,                   // "NIU",
                        PrecioUnitarioSugerido = 0,

                        PrecioUnitarioNeto = v_preUmdes - v_dsctoNume - v_dsctoGlob,       // v_preToti / v_cant - decimal.Parse(ron.Cells[11].Value.ToString()),
                        PrecioUnitarioItem = decimal.Parse(ron.Cells[8].Value.ToString()),       // 118,
                        CantidadUnidadMedida = v_cant,
                        DescuentoGlobal = v_dgloSin,
                        Descuento = v_dsctofsin,    //  decimal.Parse(ron.Cells[11].Value.ToString()),
                        ValorUnitario = v_valorUnit,
                        ValorVentaItem = v_valorUnit * v_cant,
                        ValorVentaItemXML = v_valorUnit * v_cant,
                        ValorVentaNeto = v_dsctobase - v_dsctofsin,   // v_valorUnit * v_cant,  
                        ValorVentaNetoXML = 0,
                        IGV = v_valIgvTot,
                        DescuentoBase = (v_dsctofsin <= 0) ? 0 : v_valTotal,    // v_dsctobase,    
                        PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                        MontoTributo = v_valIgvTot,

                        PrecioCodigo = cta_ron,
                        PrecioUnitario = v_preUmdes,    // decimal.Parse(ron.Cells[8].Value.ToString()),
                        Peso = 0,
                        DescuentoMonto = v_dsctoNume,
                        DescuentoPorcentaje = v_dsctoLetr,
                        TipoAfectacionIGVCodigo = "10",                     // Sunat Catalogo 7 - Venta grabada operación onerosa
                        ValorVenta = v_valTotal,
                        Ganancia = 0,
                        IGVNeto = v_valIgvTot,
                        ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                        PesoTotal = 0,
                        Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                        PrecioVentaCodigo = "01",

                        ICBPER = 0,
                        CargoIndicador = "0",
                        CargoCargoCodigo = "",
                        DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                        DescuentoCargoCodigo = "00",
                        PercepcionCantidadUmbral = 0,
                        PercepcionMontoUmbral = 0,
                        PercepcionPorcentaje = 0,
                        Control = 0,
                        PrecioCompra = 0,
                        EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                        ImporteTotalReferencia = 0,                 // este es el valor referencial 
                        Kit = 1,
                        CantidadReferencial = 1,
                        Cargo = 0,
                        ISCUnitario = 0,
                        ISCNeto = 0,
                        ISC = 0,
                        ICBPERItem = 0,
                        ICBPERSubTotal = 0,
                        DescuentoCargo = 0,
                        DescuentoCargoGravado = 0,
                        CargoItem = 0,
                        CargoTotal = 0,
                        CargoNeto = 0,
                        ISCPorcentaje = 0,
                        ISCMonto = 0,
                        CargoPorcentaje = 0,
                        //Extension = { },
                        ListaSeries = new List<CProductoCodigoSerie>(),
                        //ListaPrecios = new List<ProductoPrecioDTO>(),
                        ListaPrecios = ccc,
                        PrecioUnitarioRecuperado = false,
                        UUID = "",
                        BANDERA_CONCURRENCIA = false,
                        BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                        BANDERA_DETALLEREEMPLAZADO = false,
                        BANDERA_DETALLERECUPERADO = false,
                        BANDERA_ITEMDETALLADO = true,
                        Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                        Observacion = (rb_antic.Checked == true && tx_tipComp.Text == "C") ? tx_tipComp.Text : (rb_antic.Checked == true && tx_tipComp.Text != "C") ? "Anticipo" : "",
                        Stock = 0
                    };                      // detalles
                    aaa.Add(det);
                    cta_ron += 1;
                }
            }
            List<CMovimientoCuenta> bbb = new List<CMovimientoCuenta>();
            for (int i = 0; i < 9; i++)
            {
                if (dtpagos[i, 2] != null && dtpagos[i, 2] != "")
                {
                    CMovimientoCuenta cta = new CMovimientoCuenta
                    {
                        TipoDocumentoCodigo = tx_dat_tdoc_s.Text,
                        Serie = tx_serie.Text,
                        Correlativo = 0,                // int.Parse(tx_corre.Text),
                        Condicion = dtpagos[i, 2],
                        TipoCuentaCodigo = 1,
                        CuentaNumero = dtpagos[i, 5],
                        CuentaNombre = dtpagos[i, 2],
                        Usuario = Program.vg_nuse,
                        MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",                                   // de momento todo es soles
                        SucursalId = int.Parse(tx_codSuc.Text),
                        TipoDocIdentidadCodigo = "",
                        NumeroDocIdentidad = "",
                        Observacion = "",
                        //Extension = { },
                        //CuentaValor = "1-30303030",
                        Pago = decimal.Parse(dtpagos[i, 4]),
                        Vuelto = 0,
                        FechaPago = dtpagos[i, 6],   // dtp_pedido.Value.Date.ToString("dd/MM/yyyy")
                        NumeroOperacion = dtpagos[i, 3],
                        FechaVencimiento = dtpagos[i, 6],    // dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        PlazoDias = 1
                    };                      // movimientos
                    bbb.Add(cta);
                }
                /*
                dtpagos[i, 0] = id 
                dtpagos[i, 1] = contador
                dtpagos[i, 2] = nombre del medio de pago
                dtpagos[i, 3] = #operacion
                dtpagos[i, 4] = importe pagado
                dtpagos[i, 5] = codigo medio
                dtpagos[i, 6] = fecha de pago
                */
            }
            CComprobante obj = new CComprobante
            {
                ID = 0,
                IdRepositorio = 0,
                AplicaContingencia = false,
                AplicaAnticipo = false,
                AplicaOtroSistema = false,
                Usuario = Program.vg_nuse,
                AplicaStockNegativo = false,
                ModificacionDePrecio = false,
                Sucursal = int.Parse(tx_codSuc.Text),
                IGVPorcentaje = decimal.Parse(Program.v_igv),
                DescuentoGlobalMonto = v_dgloSin,
                DescuentoGlobalIndicadorDescuento = "0",
                DescuentoGlobalCodigoMotivo = "00",
                DescuentoGlobalNGPorcentaje = 0,
                DescuentoGlobalNGIndicadorDescuento = 0,
                DescuentoGlobalNGCodigoMotivo = "00",
                CargoGlobalPorcentaje = 0,
                CargoGlobalIndicadorCargos = "0",
                CargoGlobalCodigoMotivo = "0",
                CantidadDecimales = 2,
                AgentePercepcion = false,
                PermisoProductoSerie = false,
                EnviarCorreo = false,
                OrigenSistema = 0,
                TipoGuiaRemisionCodigo = "",
                TransportistaTipoDocIdentidadCodigo = "",
                CanalVenta = "2",
                Vendedor = usuaDni,                                 // acá debería ir el dni del usuario que hace el comprob
                VendedorNombre = tx_nomVen.Text,
                CondicionEstado = "",
                CondicionPago = (rb_contado.Checked == true) ? "Contado" : "Credito",
                SituacionPagoCodigo = 2,
                DescuentoIndicador = 0,
                Ubigeo = tx_dir_ubigpe.Text,
                AnticipoMonto = 0,
                ClienteTipoDocIdentidadCodigo = tx_dat_tdoc_s.Text,
                ClienteNumeroDocIdentidad = tx_ndc.Text,
                ClienteContacto = "",
                ClienteTelefono = tx_telef1.Text,
                OrdenNumero = "",
                GuiaNumero = "",
                ReferenciaNumeroDocumento = "",
                ReferenciaTipoDocumento = "",
                DiasPermanencia = 0,
                FechaConsumo = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                MotivoTrasladoDescripcion = "",
                TransportistaNumeroDocIdentidad = "",
                ClienteTipoSunat = 1,
                VistaDocumento = "",
                CondicionComercialIndicador = 0,
                //Extension = {}
                ListaDetalles = aaa,
                ExoneradaXML = 0,
                InafectoXML = 0,
                ExportacionXML = 0,
                ImporteTotalTexto = nle.Convertir(tx_valor.Text, true) + tx_dat_monNom.Text,
                Percepcion = 0,
                PercepcionBaseImponible = 0,
                Retencion = 0,
                DescuentoGlobalMontoBase = 0,
                DescuentoGlobalNGMonto = 0,
                DescuentoGlobalNGMontoBase = 0,
                DescuentoNGMonto = 0,
                AnticiposGravado = 0,
                AnticiposExonerado = 0,
                AnticiposInafecto = 0,
                CargoGlobalMonto = 0,
                CargoGlobalMontoBase = 0,
                ISCBase = 0,
                GratuitoGravado = 0,
                TotalPrecioVenta = decimal.Parse(tx_valor.Text),
                TotalValorVenta = decimal.Parse(tx_bruto.Text),
                Peso = 0,
                PercepcionRegimen = "",
                PercepcionFactor = 0,
                ListaMovimientos = bbb,
                PagosMultiples = false,                             // que significa esto?
                CreditoTotal = 0,
                ListaGuias = { },
                ListaCuotas = { },
                TotalCuotas = 0,
                ListaAnticipos = { },
                ListaDocumentosRelacionados = { },
                ListaCondicionesComerciales = { },
                UUID = "",
                DescuentoGlobalPorcentaje = v_dgporc,
                DescuentoGlobalValor = decimal.Parse(tx_desGlob.Text),
                CorreoElectronicoPrincipal = mailPrin,
                Exonerada = 0,
                Inafecto = 0,
                Exportacion = 0,
                OperacionNoGravada = 0,
                Gratuito = 0,
                TotalDescuentos = (v_totDscto / (1 + decimal.Parse(v_igv) / 100)) + v_dgloSin,
                DescuentoGlobal = v_dgloSin,
                TotalAnticipos = 0,
                BANDERA_CONCURRENCIA = false,
                BANDERA_DIRECCIONPARTIDAEDICION = false,
                BANDERA_GANANCIAVERIFICADA = true,
                BANDERA_ERRORESGANANCIA = false,
                NOMBRE_UBIGEOLLEGADA = "",
                NOMBRE_UBIGEOPARTIDA = "",
                CONTADOR_BUSCAPRODUCTO = 0,
                CONTADOR_CLICKEMITIR = 1,
                EstadoContingencia = false,
                Anticipo = (rb_antic.Checked == true && tx_d_valAntic.Text != "") ? true : false,
                EstadoOtroSistema = false,
                ClasePrecioCodigo = 1,
                TipoPrecio = 0,
                FormatoPDF = 0,
                TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                Serie = tx_serie.Text,       // cmb_tipo.Text.Substring(0,1) + tx_serie.Text
                Correlativo = 0,             // int.Parse(tx_corre.Text),
                MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",
                FechaEmision = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TipoDocumentoCodigoModificado = tx_dat_tipdoc_s.Text,
                SerieModificado = "",
                CorrelativoModificado = "",
                TipoNotaCreditoCodigo = "01",
                TipoNotaDebitoCodigo = "01",
                TipoOperacionCodigo = "0101",
                TipoCambio = "1.00",
                MotivoTrasladoCodigo = "01",
                ClienteNombreRazonSocial = tx_nombre.Text.Trim(),
                ClienteDireccion = tx_direc.Text.Trim() + "-" + tx_dist.Text + "-" + tx_prov.Text + "-" + tx_dpto.Text,
                UbigeoPartida = "",
                DireccionPartida = "",
                UbigeoLlegada = "",
                DireccionLlegada = "",
                TipoBusquedaProductoCodigo = 0,
                ISC = 0,
                IGV = decimal.Parse(tx_igv.Text),
                ICBPER = 0,
                ImpuestoTotal = decimal.Parse(tx_igv.Text),
                ImpuestoVarios = 0,
                TotalOtrosCargos = 0,
                TotalImporteVenta = decimal.Parse(tx_valor.Text),
                TotalImporteVentaCelular = decimal.Parse(tx_valor.Text),
                TotalImporteVentaReferencia = 0,
                PercepcionTotal = 0,
                TotalPago = decimal.Parse(tx_impMedios.Text),
                PesoTotal = 0,
                Bultos = int.Parse(tx_totcant.Text),
                Leyenda = (tx_dat_pDet.Text.Trim() == "") ? "0" : "1",      // codigo leyenda 1 = leyenda de detraccion
                BienServicioCodigo = (tx_dat_pDet.Text.Trim() == "") ? "001" : tx_dat_sDet.Text,        // codigo del bien o servicio sujeto a detraccion
                DetraccionTipoOperacion = "01",
                Detraccion = (tx_dat_pDet.Text.Trim() == "") ? 0 : (decimal.Parse(tx_valor.Text) * decimal.Parse(tx_dat_pDet.Text) / 100),
                DetraccionPorcentaje = (tx_dat_pDet.Text.Trim() == "") ? 0 : decimal.Parse(tx_dat_pDet.Text),
                DetraccionCuenta = Program.ctadetra,
                RetencionPorcentaje = 0,
                DocAdicionalCodigo = 0,
                DocAdicionalDetalle = "",
                TotalRetencion = 0,
                MontoRetencion = 0,
                PendientePago = 0,
                PermitirCuotas = 0,                             // que significa ?
                AlojamientoPaisDocEmisor = "AF",                // esto ?
                PaisResidencia = "AF",
                Gravado = decimal.Parse(tx_bruto.Text),
                Observacion = tx_coment.Text.Trim(),
                FechaIngresoPais = "01/01/1900",
                FechaIngresoEstablecimiento = "01/01/1900",
                FechaSalidaEstablecimiento = "01/01/1900",
                ModalidadTrasladoCodigo = "01",
                ConductorTipoDocIdentidadCodigo = "1",
                FechaTraslado = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TransportistaNombreRazonSocial = "",
                PlacaVehiculo = "",
                ConductorNumeroDocIdentidad = "",
                AlojamientoNumeroDocIdentidad = "",
                AlojamientoNombreRazonSocial = "",
                AlojamientoTipoDocIdentidadCodigo = "1"
            };
            retorna = JsonConvert.SerializeObject(obj);

            return retorna;
        }
        private string rapifac_cancelacion()                                        // genera comprobante de cancelacion para rapifac
        {
            string retorna = "";
            decimal v_totDscto = 0;
            decimal v_preToti = 0;
            decimal v_dgloSin = decimal.Parse(tx_desGlob.Text) / (1 + (decimal.Parse(v_igv) / 100));
            decimal v_dgporc = Math.Round(decimal.Parse(tx_desGlob.Text) * 100 / decimal.Parse(tx_subtot.Text), 2);
            List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();        // detalle cpmpleto
            List<ProductoPrecioDTO> ccc = new List<ProductoPrecioDTO>();            // lista de precios
            List<CAnticipo> ddd = new List<CAnticipo>();                            // lista de anticipos
            List<CDocumentoRelacionado> eee = new List<CDocumentoRelacionado>();    // lista de documentos relacionados 
            int cta_ron = 1;
            foreach (DataGridViewRow ron in dataGridView1.Rows)
            {
                if (ron.Cells[1].Value != null)
                {
                    if (ron.Cells[1].Value.ToString().Trim() != "" && ron.Cells[1].Value.ToString().Trim() != "0")  // buscamos que sea artículo con cantidad
                    {
                        // *********** PRECIO 
                        ProductoPrecioDTO dlp = new ProductoPrecioDTO
                        {
                            PrecioId = cta_ron,        // 99
                            PrecioConfiguracion = 1,
                            ProductoCod = ron.Cells[2].Value.ToString(),
                            CodigoUnidadMedida = cod_umed,          // "NIU",
                            DescripcionUnidadMedida = nom_umed,     //  "UNIDAD",
                            CantidadUnidadMedida = int.Parse(ron.Cells[1].Value.ToString()),
                            MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",
                            SucursalId = tx_codSuc.Text,
                            Margenganancia = 0,
                            MargenPorcentaje = 0,
                            PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),
                            Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),
                            OtrosCargosPorcentaje = 0,
                            CantidadAplicable = 0,
                            FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            Estado = 1,
                            ISCPorcentaje = 0,
                            ISCCalculado = 0,
                            //Extension = { };
                            UUID = ""
                        };
                        ccc.Add(dlp);
                        // ********** DETALLE FILA
                        int v_cant = int.Parse(ron.Cells[1].Value.ToString());                    // cantidad
                        decimal v_valorNeto = decimal.Parse(ron.Cells[8].Value.ToString()) /      // valor unit (precio unit sin IGV)
                            ((decimal.Parse(v_igv) / 100) + 1);
                        decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                            (decimal.Parse(ron.Cells[9].Value.ToString()) /
                            ((decimal.Parse(v_igv) / 100) + 1));
                        //decimal v_valTotal = decimal.Parse(ron.Cells[9].Value.ToString()) /       // valor total fila sin igv
                        //    ((decimal.Parse(v_igv) / 100) + 1);
                        v_preToti = decimal.Parse(ron.Cells[12].Value.ToString()) * v_cant;
                        decimal v_dsctofila = 0, v_dsctofsin = 0;
                        decimal v_dsctobase = 0;
                        decimal v_dsctoNume = 0;
                        string v_dsctoLetr = "0.00";                                                // % dscto en letras
                        decimal v_preUmdes = decimal.Parse(ron.Cells[12].Value.ToString());           // precio individual con descuento
                        decimal v_valorUnit = v_preUmdes / ((decimal.Parse(v_igv) / 100) + 1);
                        decimal v_valTotal = v_valorUnit * v_cant;
                        int sss = ((dataGridView1.Rows.Count - 1) - (_docsAnticip.Count) == 0) ? 1 : ((dataGridView1.Rows.Count - 1) - (_docsAnticip.Count));
                        decimal v_dsctoGlob = decimal.Parse(tx_desGlob.Text) / sss / v_cant;
                        if (ron.Cells[11].Value.ToString() != "" && ron.Cells[11].Value.ToString() != "0" && ron.Cells[11].Value.ToString() != "0.00")
                        {
                            v_dsctofila = decimal.Parse(ron.Cells[11].Value.ToString());        // descuento total fila
                            v_dsctofsin = v_dsctofila / ((decimal.Parse(v_igv) / 100) + 1);     // descuento fila sin igv
                            v_dsctobase = v_valTotal + v_dsctofsin;                             // valor total fila sin igv + dscto total fila
                            v_dsctoNume = v_dsctofila / v_cant; // (v_dsctofila * 100) / v_dsctobase;                    // descuento en numero
                            v_dsctoLetr = Math.Round(100 - (((v_preUmdes - v_dsctoNume) * v_cant) * 100 / v_preToti), 2).ToString();  // v_dsctoNume.ToString();
                            v_totDscto = v_totDscto + v_dsctofila;
                        }

                        CComprobanteDetalle det = new CComprobanteDetalle       // aca falta incluir descuento 
                        {
                            ID = 0,
                            ComprobanteID = 0,
                            Item = cta_ron,
                            TipoProductoCodigo = "",
                            ProductoCodigo = ron.Cells[2].Value.ToString(),   // "Prod00005",
                            ProductoCodigoSUNAT = "",                       // "56101532",
                            TipoSistemaISCCodigo = "00",
                            UnidadMedidaCodigo = cod_umed,          // "NIU",
                            PrecioUnitarioSugerido = 0,

                            PrecioUnitarioNeto = v_preUmdes - v_dsctoNume - v_dsctoGlob,
                            PrecioUnitarioItem = decimal.Parse(ron.Cells[8].Value.ToString()),       // 118,
                            CantidadUnidadMedida = v_cant,
                            DescuentoGlobal = 0,
                            Descuento = v_dsctofsin,
                            ValorUnitario = v_valorUnit,
                            ValorVentaItem = v_valorUnit * v_cant,
                            ValorVentaItemXML = v_valorUnit * v_cant,
                            ValorVentaNeto = v_valorNeto,   // v_dsctobase - v_dsctofsin,     // v_valorUnit * v_cant,
                            ValorVentaNetoXML = 0,
                            IGV = v_valIgvTot,
                            DescuentoBase = v_valTotal,
                            PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                            MontoTributo = v_valIgvTot,

                            PrecioCodigo = cta_ron,
                            PrecioUnitario = v_preUmdes,    //decimal.Parse(ron.Cells[8].Value.ToString()),
                            Peso = 0,
                            DescuentoMonto = v_dsctoNume,
                            DescuentoPorcentaje = v_dsctoLetr,
                            TipoAfectacionIGVCodigo = "10",                     // esto deberia ser variable
                            ValorVenta = v_valTotal,
                            Ganancia = 0,
                            IGVNeto = v_valIgvTot,
                            ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                            PesoTotal = 0,
                            Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                            PrecioVentaCodigo = "01",

                            ICBPER = 0,
                            CargoIndicador = "0",
                            CargoCargoCodigo = "",
                            DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                            DescuentoCargoCodigo = "00",
                            PercepcionCantidadUmbral = 0,
                            PercepcionMontoUmbral = 0,
                            PercepcionPorcentaje = 0,
                            Control = 0,
                            PrecioCompra = 0,
                            EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                            ImporteTotalReferencia = 0,                 // este es el valor referencial 
                            Kit = 1,
                            CantidadReferencial = 1,
                            Cargo = 0,
                            ISCUnitario = 0,
                            ISCNeto = 0,
                            ISC = 0,
                            ICBPERItem = 0,
                            ICBPERSubTotal = 0,
                            DescuentoCargo = 0,
                            DescuentoCargoGravado = 0,
                            CargoItem = 0,
                            CargoTotal = 0,
                            CargoNeto = 0,
                            ISCPorcentaje = 0,
                            ISCMonto = 0,
                            CargoPorcentaje = 0,
                            //Extension = { },
                            ListaSeries = new List<CProductoCodigoSerie>(),
                            ListaPrecios = ccc,
                            PrecioUnitarioRecuperado = false,
                            UUID = "",
                            BANDERA_CONCURRENCIA = false,
                            BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                            BANDERA_DETALLEREEMPLAZADO = false,
                            BANDERA_DETALLERECUPERADO = false,
                            BANDERA_ITEMDETALLADO = true,
                            Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                            //Observacion = (rb_antic.Checked == true && tx_tipComp.Text == "C") ? tx_tipComp.Text : (rb_antic.Checked == true && tx_tipComp.Text != "C") ? "Anticipo" : "",
                            Observacion = "",
                            Stock = 0
                        };                      // detalles
                        aaa.Add(det);
                        cta_ron += 1;
                    }
                }
            }
            // terminado con los items ahora vamos por los anticipos
            decimal totAnt = 0, subtotAnt = 0;
            for (int i = 0; i <= _docsAnticip.Count - 1; i++)
            {
                int v_cant = 1;                                                     // cantidad
                decimal v_valorUnit = decimal.Parse(_docsAnticip[i].bruto) * -1;         // valor unit (precio unit sin IGV) 
                decimal v_valIgvTot = decimal.Parse(_docsAnticip[i].igv) * -1;           // igv total de la fila
                decimal v_valTotal = decimal.Parse(_docsAnticip[i].valor) * -1;          // valor total fila sin igv
                CComprobanteDetalle det = new CComprobanteDetalle
                {
                    ID = 0,
                    ComprobanteID = _docsAnticip[i].IdCompRapifac,
                    Item = cta_ron,
                    TipoProductoCodigo = "",
                    ProductoCodigo = _docsAnticip[i].descrip,
                    ProductoCodigoSUNAT = "",
                    TipoSistemaISCCodigo = "00",
                    UnidadMedidaCodigo = cod_umed, // "NIU",
                    PrecioUnitarioSugerido = 0,
                    PrecioUnitarioItem = decimal.Parse(_docsAnticip[i].valor) * -1,       // 118,
                    PrecioUnitarioNeto = v_valTotal,
                    PrecioVentaCodigo = "01",
                    ICBPER = 0,
                    CargoIndicador = "0",
                    CargoCargoCodigo = "",
                    DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                    DescuentoCargoCodigo = "00",
                    PercepcionCantidadUmbral = 0,
                    PercepcionMontoUmbral = 0,
                    PercepcionPorcentaje = 0,
                    Control = 0,
                    PrecioCompra = 0,
                    EsAnticipo = true,
                    ImporteTotalReferencia = 0,                 // este es el valor referencial 
                    CantidadUnidadMedida = v_cant,
                    Kit = 1,
                    CantidadReferencial = 1,
                    Cargo = 0,
                    DescuentoGlobal = 0,
                    Descuento = 0,
                    ValorUnitario = v_valorUnit,
                    ValorVentaItem = v_valorUnit * v_cant,
                    ValorVentaItemXML = v_valorUnit * v_cant,
                    ValorVentaNeto = v_valorUnit * v_cant,
                    ValorVentaNetoXML = 0,
                    ISCUnitario = 0,
                    ISCNeto = 0,
                    ISC = 0,
                    IGV = v_valIgvTot,
                    ICBPERItem = 0,
                    ICBPERSubTotal = 0,
                    DescuentoBase = 0,
                    DescuentoCargo = 0,
                    DescuentoCargoGravado = 0,
                    CargoItem = 0,
                    CargoTotal = 0,
                    CargoNeto = 0,
                    PrecioVenta = decimal.Parse(_docsAnticip[i].valor) * -1,
                    MontoTributo = v_valIgvTot,
                    ISCPorcentaje = 0,
                    ISCMonto = 0,
                    CargoPorcentaje = 0,
                    //Extension = { },
                    ListaSeries = new List<CProductoCodigoSerie>(),
                    //ListaPrecios = new List<ProductoPrecioDTO>(),
                    ListaPrecios = ccc,
                    PrecioUnitarioRecuperado = true,
                    UUID = "",
                    BANDERA_CONCURRENCIA = false,
                    BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                    BANDERA_DETALLEREEMPLAZADO = false,
                    BANDERA_DETALLERECUPERADO = true,
                    BANDERA_ITEMDETALLADO = true,
                    Descripcion = _docsAnticip[i].descrip,
                    Observacion = "",
                    Stock = 0,
                    Cantidad = 1,
                    PrecioCodigo = 0,
                    PrecioUnitario = decimal.Parse(_docsAnticip[i].valor) * -1,
                    Peso = 0,
                    DescuentoMonto = 0,
                    DescuentoPorcentaje = "0.00",
                    TipoAfectacionIGVCodigo = "10",                     // esto deberia ser variable
                    ValorVenta = v_valorUnit,
                    Ganancia = v_valTotal,
                    IGVNeto = v_valIgvTot,
                    ImporteTotal = decimal.Parse(_docsAnticip[i].valor) * -1,
                    PesoTotal = 0
                };                      // detalles
                aaa.Add(det);
                CAnticipo ant = new CAnticipo
                {
                    ComprobanteID = _docsAnticip[i].IdCompRapifac,
                    TipoDocumentoCodigo = _docsAnticip[i].tipDocCod,
                    Serie = _docsAnticip[i].serieRap,
                    Correlativo = _docsAnticip[i].corRap,
                    MonedaCodigo = _docsAnticip[i].codMon,
                    AnticiposId = "1",
                    Anticipo = decimal.Parse(_docsAnticip[i].valor),
                    AnticipoBase = decimal.Parse(_docsAnticip[i].bruto),
                    AnticipoIGV = decimal.Parse(_docsAnticip[i].igv),
                    TipoDocIdentidadCodigo = _docsAnticip[i].tDocClte,
                    NumeroDocIdentidad = _docsAnticip[i].nroDocC,
                    TipoAfectacionIGVCodigo = "10"
                };
                ddd.Add(ant);
                CDocumentoRelacionado rel = new CDocumentoRelacionado
                {
                    TipoDocumentoCodigo = _docsAnticip[i].tipDocCod,
                    Serie = _docsAnticip[i].serieRap,
                    Correlativo = _docsAnticip[i].corRap,
                    FechaEmision = _docsAnticip[i].fecEmi,
                    Moneda = _docsAnticip[i].codMon,
                    Importe = decimal.Parse(_docsAnticip[i].valor)
                    //Extension = { }
                };
                eee.Add(rel);
                totAnt = totAnt + decimal.Parse(_docsAnticip[i].valor);
                subtotAnt = subtotAnt + decimal.Parse(_docsAnticip[i].bruto);
                cta_ron += 1;
            }
            // terminado los anticipos, vamos por los pagos
            List<CMovimientoCuenta> bbb = new List<CMovimientoCuenta>();
            for (int i = 0; i < 9; i++)
            {
                if (dtpagos[i, 2] != null && dtpagos[i, 2] != "")
                {
                    CMovimientoCuenta cta = new CMovimientoCuenta
                    {
                        TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                        Serie = cmb_tipo.Text.Substring(0, 1) + tx_serie.Text,
                        Correlativo = 0,                // int.Parse(tx_corre.Text),
                        Condicion = dtpagos[i, 2],
                        CondicionComprobante = "Contado",  //dtpagos[i, ], este dato de condicion no tenemos en la matris
                        TipoCuentaCodigo = 1,
                        CuentaNumero = dtpagos[i, 5],
                        CuentaNombre = dtpagos[i, 2],
                        Usuario = usuaDni,             // Program.vg_nuse,
                        MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",                                   // de momento todo es soles
                        SucursalId = int.Parse(tx_codSuc.Text),
                        TipoDocIdentidadCodigo = "",
                        NumeroDocIdentidad = "",
                        Observacion = "",
                        //Extension = { },
                        //CuentaValor = "1-30303030",
                        Pago = decimal.Parse(dtpagos[i, 4]),
                        Vuelto = 0,
                        FechaPago = dtpagos[i, 6],   // dtp_pedido.Value.Date.ToString("dd/MM/yyyy")
                        NumeroOperacion = dtpagos[i, 3],
                        FechaVencimiento = dtpagos[i, 6],    // dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        PlazoDias = 1
                    };                      // movimientos
                    bbb.Add(cta);
                }
                /*
                dtpagos[i, 0] = id 
                dtpagos[i, 1] = contador
                dtpagos[i, 2] = nombre del medio de pago
                dtpagos[i, 3] = #operacion
                dtpagos[i, 4] = importe pagado
                dtpagos[i, 5] = codigo medio
                dtpagos[i, 6] = fecha de pago
                */
            }
            // terminado con los movimientos de pagos por el comprobante entero
            CComprobante obj = new CComprobante
            {
                ID = 0,
                IdRepositorio = 0,
                AplicaContingencia = false,
                AplicaAnticipo = true,
                AplicaOtroSistema = false,
                Usuario = Program.vg_nuse,
                AplicaStockNegativo = false,
                ModificacionDePrecio = false,
                Sucursal = int.Parse(tx_codSuc.Text),
                IGVPorcentaje = decimal.Parse(Program.v_igv),
                DescuentoGlobalMonto = v_dgloSin,
                DescuentoGlobalIndicadorDescuento = "0",
                DescuentoGlobalCodigoMotivo = "00",
                DescuentoGlobalNGPorcentaje = 0,
                DescuentoGlobalNGIndicadorDescuento = 0,
                DescuentoGlobalNGCodigoMotivo = "00",
                CargoGlobalPorcentaje = 0,
                CargoGlobalIndicadorCargos = "0",
                CargoGlobalCodigoMotivo = "0",
                CantidadDecimales = 2,
                AgentePercepcion = false,
                PermisoProductoSerie = false,
                EnviarCorreo = false,
                OrigenSistema = 0,
                TipoGuiaRemisionCodigo = "",
                TransportistaTipoDocIdentidadCodigo = "",
                CanalVenta = "2",
                Vendedor = usuaDni,                                 // acá debería ir el dni del usuario que hace el comprob
                VendedorNombre = tx_nomVen.Text,
                CondicionEstado = "",
                CondicionPago = (rb_contado.Checked == true) ? "Contado" : "Credito",
                SituacionPagoCodigo = 2,
                DescuentoIndicador = 0,
                Ubigeo = tx_dir_ubigpe.Text,
                AnticipoMonto = totAnt,                                   // suma de anticipos, antes de la cancelacion
                ClienteTipoDocIdentidadCodigo = tx_dat_tdoc_s.Text,
                ClienteNumeroDocIdentidad = tx_ndc.Text,
                ClienteContacto = "",
                ClienteTelefono = tx_telef1.Text,
                OrdenNumero = "",
                GuiaNumero = "",
                ReferenciaNumeroDocumento = "",
                ReferenciaTipoDocumento = "",
                DiasPermanencia = 0,
                FechaConsumo = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                MotivoTrasladoDescripcion = "",
                TransportistaNumeroDocIdentidad = "",
                ClienteTipoSunat = 1,
                VistaDocumento = "",
                CondicionComercialIndicador = 0,
                //Extension = {}
                ListaDetalles = aaa,
                ExoneradaXML = 0,
                InafectoXML = 0,
                ExportacionXML = 0,
                ImporteTotalTexto = nle.Convertir(tx_valor.Text, true) + tx_dat_monNom.Text,
                Percepcion = 0,
                PercepcionBaseImponible = 0,
                Retencion = 0,
                DescuentoGlobalMontoBase = decimal.Parse(tx_bruto.Text),
                DescuentoGlobalNGMonto = 0,
                DescuentoGlobalNGMontoBase = 0,
                DescuentoNGMonto = 0,
                AnticiposGravado = subtotAnt,
                AnticiposExonerado = 0,
                AnticiposInafecto = 0,
                CargoGlobalMonto = 0,
                CargoGlobalMontoBase = decimal.Parse(tx_bruto.Text),
                ISCBase = 0,
                GratuitoGravado = 0,
                TotalPrecioVenta = decimal.Parse(tx_valor.Text),
                TotalValorVenta = decimal.Parse(tx_bruto.Text),
                Peso = 0,
                PercepcionRegimen = "",
                PercepcionFactor = 0,
                ListaMovimientos = bbb,
                PagosMultiples = false,                             // que significa esto?
                CreditoTotal = 0,
                ListaGuias = { },
                ListaCuotas = { },
                TotalCuotas = 0,
                ListaAnticipos = ddd,
                ListaDocumentosRelacionados = eee,
                ListaCondicionesComerciales = { },
                UUID = "",
                DescuentoGlobalPorcentaje = v_dgporc,
                DescuentoGlobalValor = decimal.Parse(tx_desGlob.Text),
                CorreoElectronicoPrincipal = mailPrin,
                Exonerada = 0,
                Inafecto = 0,
                Exportacion = 0,
                OperacionNoGravada = 0,
                Gratuito = 0,
                TotalDescuentos = (v_totDscto / (1 + decimal.Parse(v_igv) / 100)) + v_dgloSin,      // TotalDescuentos = 0,
                DescuentoGlobal = v_dgloSin,
                TotalAnticipos = subtotAnt,
                BANDERA_CONCURRENCIA = false,
                BANDERA_DIRECCIONPARTIDAEDICION = false,
                BANDERA_GANANCIAVERIFICADA = true,
                BANDERA_ERRORESGANANCIA = false,
                NOMBRE_UBIGEOLLEGADA = "",
                NOMBRE_UBIGEOPARTIDA = "",
                CONTADOR_BUSCAPRODUCTO = 0,
                CONTADOR_CLICKEMITIR = 1,
                EstadoContingencia = false,
                Anticipo = false,           // este es el ultimo pago, es una cancelacion, no es anticipo
                EstadoOtroSistema = false,
                ClasePrecioCodigo = 1,
                TipoPrecio = 0,
                FormatoPDF = 0,
                TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                Serie = cmb_tipo.Text.Substring(0, 1) + tx_serie.Text,       // cmb_tipo.Text.Substring(0,1) + tx_serie.Text
                Correlativo = 0,             // int.Parse(tx_corre.Text),
                MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",
                FechaEmision = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TipoDocumentoCodigoModificado = tx_dat_tipdoc_s.Text,
                SerieModificado = "",
                CorrelativoModificado = "",
                TipoNotaCreditoCodigo = "01",
                TipoNotaDebitoCodigo = "01",
                TipoOperacionCodigo = "0101",                               // todo esto debe estar en variables
                TipoCambio = "1.00",
                MotivoTrasladoCodigo = "01",
                ClienteNombreRazonSocial = tx_nombre.Text.Trim(),
                ClienteDireccion = tx_direc.Text.Trim() + "-" + tx_dist.Text + "-" + tx_prov.Text + "-" + tx_dpto.Text,
                UbigeoPartida = "",    // tx_dir_ubigpe.Text
                DireccionPartida = "",
                UbigeoLlegada = "",
                DireccionLlegada = "",
                TipoBusquedaProductoCodigo = 0,
                ISC = 0,
                IGV = decimal.Parse(tx_igv.Text),
                ICBPER = 0,
                ImpuestoTotal = decimal.Parse(tx_igv.Text),
                ImpuestoVarios = 0,
                TotalOtrosCargos = 0,
                TotalImporteVenta = decimal.Parse(tx_valor.Text),
                TotalImporteVentaCelular = decimal.Parse(tx_valor.Text),
                TotalImporteVentaReferencia = 0,
                PercepcionTotal = 0,
                TotalPago = decimal.Parse(tx_impMedios.Text),
                PesoTotal = 0,
                Bultos = int.Parse(tx_totcant.Text),
                Leyenda = (rb_tbienes.Checked == true)? "0" : (tx_dat_cDet.Text != "") ? "1" : "0",
                BienServicioCodigo = (rb_tbienes.Checked == true) ? "037" : (tx_dat_cDet.Text != "") ? tx_dat_sDet.Text : "001",                     // Demas servicios gravados con el IGV (Tipo de Bien o Servicio)
                DetraccionTipoOperacion = "01",
                Detraccion = (tx_dat_pDet.Text.Trim() == "") ? 0 : (decimal.Parse(tx_valor.Text) * decimal.Parse(tx_dat_pDet.Text) / 100),
                DetraccionPorcentaje = (tx_dat_pDet.Text.Trim() == "") ? 0 : decimal.Parse(tx_dat_pDet.Text),
                DetraccionCuenta = Program.ctadetra,
                RetencionPorcentaje = 0,
                DocAdicionalCodigo = 0,
                DocAdicionalDetalle = "",
                TotalRetencion = 0,
                MontoRetencion = 0,
                PendientePago = 0,
                PermitirCuotas = 0,                             // que significa ?
                AlojamientoPaisDocEmisor = "AF",                // esto ?
                PaisResidencia = "AF",
                Gravado = decimal.Parse(tx_bruto.Text),
                //Observacion = (rb_antic.Checked == true && tx_d_valAntic.Text != "" && tx_tipComp.Text != "C") ? " * * * ANTICIPO * * * " : (rb_antic.Checked == true && tx_tipComp.Text == "C") ? "CANCELACIÓN DE CONTRATO " + tx_cont.Text : "",
                Observacion = tx_coment.Text,
                FechaIngresoPais = "01/01/1900",
                FechaIngresoEstablecimiento = "01/01/1900",
                FechaSalidaEstablecimiento = "01/01/1900",
                ModalidadTrasladoCodigo = "01",
                ConductorTipoDocIdentidadCodigo = "1",
                FechaTraslado = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TransportistaNombreRazonSocial = "",
                PlacaVehiculo = "",
                ConductorNumeroDocIdentidad = "",
                AlojamientoNumeroDocIdentidad = "",
                AlojamientoNombreRazonSocial = "",
                AlojamientoTipoDocIdentidadCodigo = "1"
            };
            retorna = JsonConvert.SerializeObject(obj);

            return retorna;
        }
        private string rapifac_anticipo()
        {
            string retorna = "";
            int cta_ron = 1;
            List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();
            foreach (DataGridViewRow ron in dataGridView1.Rows)
            {
                List<ProductoPrecioDTO> ccc = new List<ProductoPrecioDTO>();
                if (ron.Cells[1].Value != null && ron.Cells[10].Value != null && ron.Cells[10].Value.ToString() == "A")
                {
                    ProductoPrecioDTO dlp = new ProductoPrecioDTO
                    {
                        PrecioId = 99,
                        PrecioConfiguracion = 1,
                        ProductoCod = "",
                        CodigoUnidadMedida = (rb_tbienes.Checked == true)? cod_umed : cod_user,          //  "NIU",
                        DescripcionUnidadMedida = (rb_tbienes.Checked == true) ? nom_umed : nom_user,     // "UNIDAD",
                        CantidadUnidadMedida = 1,
                        MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",
                        SucursalId = tx_codSuc.Text,
                        Margenganancia = 0,
                        MargenPorcentaje = 0,
                        PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),         // ron.Cells[9].Value.ToString()
                        Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),            // ron.Cells[9].Value.ToString()
                        OtrosCargosPorcentaje = 0,
                        CantidadAplicable = 0,
                        FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        Estado = 1,
                        ISCPorcentaje = 0,
                        ISCCalculado = 0,
                        //Extension = { };
                        UUID = ""
                    };
                    ccc.Add(dlp);
                }
                if (ron.Cells[1].Value != null && ron.Cells[10].Value != null && ron.Cells[10].Value.ToString() == "A")
                {
                    int v_cant = 1;                    // cantidad
                    decimal v_valorUnit = decimal.Parse(ron.Cells[9].Value.ToString()) /      // valor unit (precio unit sin IGV)
                        ((decimal.Parse(v_igv) / 100) + 1);
                    decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                        (decimal.Parse(ron.Cells[9].Value.ToString()) /
                        ((decimal.Parse(v_igv) / 100) + 1));
                    decimal v_valTotal = decimal.Parse(ron.Cells[9].Value.ToString()) /       // valor total fila sin igv
                        ((decimal.Parse(v_igv) / 100) + 1);
                    CComprobanteDetalle det = new CComprobanteDetalle
                    {
                        ID = 0,
                        ComprobanteID = 0,
                        Item = cta_ron,
                        TipoProductoCodigo = "",
                        ProductoCodigo = "",   // "Prod00005",
                        ProductoCodigoSUNAT = "",                       // "56101532",
                        TipoSistemaISCCodigo = "00",
                        UnidadMedidaCodigo = cod_umed,      // "NIU",
                        PrecioUnitarioSugerido = 0,
                        PrecioUnitarioItem = decimal.Parse(ron.Cells[9].Value.ToString()),       // 118,
                        PrecioUnitarioNeto = decimal.Parse(ron.Cells[9].Value.ToString()),       // decimal.Parse(ron.Cells[12].Value.ToString()),
                        PrecioVentaCodigo = "01",
                        ICBPER = 0,
                        CargoIndicador = "0",
                        CargoCargoCodigo = "",
                        DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                        DescuentoCargoCodigo = "00",
                        PercepcionCantidadUmbral = 0,
                        PercepcionMontoUmbral = 0,
                        PercepcionPorcentaje = 0,
                        Control = 0,
                        PrecioCompra = 0,
                        EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                        ImporteTotalReferencia = 0,                 // este es el valor referencial 
                        CantidadUnidadMedida = v_cant,
                        Kit = 1,
                        CantidadReferencial = 1,
                        Cargo = 0,
                        DescuentoGlobal = 0,
                        Descuento = 0,
                        ValorUnitario = v_valorUnit,
                        ValorVentaItem = v_valorUnit * v_cant,
                        ValorVentaItemXML = v_valorUnit * v_cant,
                        ValorVentaNeto = v_valorUnit * v_cant,
                        ValorVentaNetoXML = 0,
                        ISCUnitario = 0,
                        ISCNeto = 0,
                        ISC = 0,
                        IGV = v_valIgvTot,
                        ICBPERItem = 0,
                        ICBPERSubTotal = 0,
                        DescuentoBase = 0,
                        DescuentoCargo = 0,
                        DescuentoCargoGravado = 0,
                        CargoItem = 0,
                        CargoTotal = 0,
                        CargoNeto = 0,
                        PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                        MontoTributo = v_valIgvTot,
                        ISCPorcentaje = 0,
                        ISCMonto = 0,
                        CargoPorcentaje = 0,
                        //Extension = { },
                        ListaSeries = new List<CProductoCodigoSerie>(),
                        //ListaPrecios = new List<ProductoPrecioDTO>(),
                        ListaPrecios = ccc,
                        PrecioUnitarioRecuperado = false,
                        UUID = "",
                        BANDERA_CONCURRENCIA = false,
                        BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                        BANDERA_DETALLEREEMPLAZADO = false,
                        BANDERA_DETALLERECUPERADO = false,
                        BANDERA_ITEMDETALLADO = true,
                        Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                        Observacion = "",
                        Stock = 0,
                        Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                        PrecioCodigo = 0,
                        PrecioUnitario = decimal.Parse(ron.Cells[9].Value.ToString()),
                        Peso = 0,
                        DescuentoMonto = 0,
                        DescuentoPorcentaje = "0.00",
                        TipoAfectacionIGVCodigo = "10",                     // esto deberia ser variable
                        ValorVenta = v_valTotal,
                        Ganancia = 0,
                        IGVNeto = v_valIgvTot,
                        ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                        PesoTotal = 0
                    };                      // detalles
                    aaa.Add(det);
                    cta_ron += 1;
                }
            }
            List<CMovimientoCuenta> bbb = new List<CMovimientoCuenta>();
            for (int i = 0; i < 9; i++)
            {
                if (dtpagos[i, 2] != null && dtpagos[i, 2] != "")
                {
                    CMovimientoCuenta cta = new CMovimientoCuenta
                    {
                        TipoDocumentoCodigo = tx_dat_tdoc_s.Text,
                        Serie = tx_serie.Text,
                        Correlativo = 0,                // int.Parse(tx_corre.Text),
                        Condicion = dtpagos[i, 2],
                        TipoCuentaCodigo = 1,
                        CuentaNumero = dtpagos[i, 5],
                        CuentaNombre = dtpagos[i, 2],
                        Usuario = Program.vg_nuse,
                        MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",                                   // de momento todo es soles
                        SucursalId = int.Parse(tx_codSuc.Text),
                        TipoDocIdentidadCodigo = "",
                        NumeroDocIdentidad = "",
                        Observacion = "",
                        //Extension = { },
                        //CuentaValor = "1-30303030",
                        Pago = decimal.Parse(dtpagos[i, 4]),
                        Vuelto = 0,
                        FechaPago = dtpagos[i, 6],   // dtp_pedido.Value.Date.ToString("dd/MM/yyyy")
                        NumeroOperacion = dtpagos[i, 3],
                        FechaVencimiento = dtpagos[i, 6],    // dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        PlazoDias = 1
                    };                      // movimientos
                    bbb.Add(cta);
                }
                /*
                dtpagos[i, 0] = id 
                dtpagos[i, 1] = contador
                dtpagos[i, 2] = nombre del medio de pago
                dtpagos[i, 3] = #operacion
                dtpagos[i, 4] = importe pagado
                dtpagos[i, 5] = codigo medio
                dtpagos[i, 6] = fecha de pago
                */
            }
            CComprobante obj = new CComprobante
            {
                ID = 0,
                IdRepositorio = 0,
                AplicaContingencia = false,
                AplicaAnticipo = false,
                AplicaOtroSistema = false,
                Usuario = Program.vg_nuse,
                AplicaStockNegativo = false,
                ModificacionDePrecio = false,
                Sucursal = int.Parse(tx_codSuc.Text),
                IGVPorcentaje = decimal.Parse(Program.v_igv),
                DescuentoGlobalMonto = 0,
                DescuentoGlobalIndicadorDescuento = "0",
                DescuentoGlobalCodigoMotivo = "00",
                DescuentoGlobalNGPorcentaje = 0,
                DescuentoGlobalNGIndicadorDescuento = 0,
                DescuentoGlobalNGCodigoMotivo = "00",
                CargoGlobalPorcentaje = 0,
                CargoGlobalIndicadorCargos = "0",
                CargoGlobalCodigoMotivo = "0",
                CantidadDecimales = 2,
                AgentePercepcion = false,
                PermisoProductoSerie = false,
                EnviarCorreo = false,
                OrigenSistema = 0,
                TipoGuiaRemisionCodigo = "",
                TransportistaTipoDocIdentidadCodigo = "",
                CanalVenta = "2",
                Vendedor = usuaDni,                                 // acá debería ir el dni del usuario que hace el comprob
                VendedorNombre = tx_nomVen.Text,
                CondicionEstado = "",
                CondicionPago = (rb_contado.Checked == true) ? "Contado" : "Credito",
                SituacionPagoCodigo = 2,
                DescuentoIndicador = 0,
                Ubigeo = tx_dir_ubigpe.Text,
                AnticipoMonto = 0,
                ClienteTipoDocIdentidadCodigo = tx_dat_tdoc_s.Text,
                ClienteNumeroDocIdentidad = tx_ndc.Text,
                ClienteContacto = "",
                ClienteTelefono = tx_telef1.Text,
                OrdenNumero = "",
                GuiaNumero = "",
                ReferenciaNumeroDocumento = "",
                ReferenciaTipoDocumento = "",
                DiasPermanencia = 0,
                FechaConsumo = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                MotivoTrasladoDescripcion = "",
                TransportistaNumeroDocIdentidad = "",
                ClienteTipoSunat = 1,
                VistaDocumento = "",
                CondicionComercialIndicador = 0,
                //Extension = {}
                ListaDetalles = aaa,
                ExoneradaXML = 0,
                InafectoXML = 0,
                ExportacionXML = 0,
                ImporteTotalTexto = nle.Convertir(tx_valor.Text, true) + tx_dat_monNom.Text,
                Percepcion = 0,
                PercepcionBaseImponible = 0,
                Retencion = 0,
                DescuentoGlobalMontoBase = 0,
                DescuentoGlobalNGMonto = 0,
                DescuentoGlobalNGMontoBase = 0,
                DescuentoNGMonto = 0,
                AnticiposGravado = 0,
                AnticiposExonerado = 0,
                AnticiposInafecto = 0,
                CargoGlobalMonto = 0,
                CargoGlobalMontoBase = 0,
                ISCBase = 0,
                GratuitoGravado = 0,
                TotalPrecioVenta = decimal.Parse(tx_valor.Text),
                TotalValorVenta = decimal.Parse(tx_bruto.Text),
                Peso = 0,
                PercepcionRegimen = "",
                PercepcionFactor = 0,
                ListaMovimientos = bbb,
                PagosMultiples = false,                             // que significa esto?
                CreditoTotal = 0,
                ListaGuias = { },
                ListaCuotas = { },
                TotalCuotas = 0,
                ListaAnticipos = { },
                ListaDocumentosRelacionados = { },
                ListaCondicionesComerciales = { },
                UUID = "",
                DescuentoGlobalPorcentaje = 0,
                DescuentoGlobalValor = 0,
                CorreoElectronicoPrincipal = "no-send@rapifac.com",         // no debería ser correo de artesanos
                Exonerada = 0,
                Inafecto = 0,
                Exportacion = 0,
                OperacionNoGravada = 0,
                Gratuito = 0,
                TotalDescuentos = 0,
                DescuentoGlobal = 0,
                TotalAnticipos = 0,
                BANDERA_CONCURRENCIA = false,
                BANDERA_DIRECCIONPARTIDAEDICION = false,
                BANDERA_GANANCIAVERIFICADA = true,
                BANDERA_ERRORESGANANCIA = false,
                NOMBRE_UBIGEOLLEGADA = "",
                NOMBRE_UBIGEOPARTIDA = "",
                CONTADOR_BUSCAPRODUCTO = 0,
                CONTADOR_CLICKEMITIR = 1,
                EstadoContingencia = false,
                Anticipo = (rb_antic.Checked == true && tx_d_valAntic.Text != "") ? true : false,
                EstadoOtroSistema = false,
                ClasePrecioCodigo = 1,
                TipoPrecio = 0,
                FormatoPDF = 0,
                TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                Serie = tx_serie.Text,       // cmb_tipo.Text.Substring(0,1) + tx_serie.Text
                Correlativo = 0,             // int.Parse(tx_corre.Text),
                MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",
                FechaEmision = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TipoDocumentoCodigoModificado = tx_dat_tipdoc_s.Text,
                SerieModificado = "",
                CorrelativoModificado = "",
                TipoNotaCreditoCodigo = "01",
                TipoNotaDebitoCodigo = "01",
                TipoOperacionCodigo = "0101",                               // todo esto debe estar en variables
                TipoCambio = "1.00",
                MotivoTrasladoCodigo = "01",
                ClienteNombreRazonSocial = tx_nombre.Text.Trim(),
                ClienteDireccion = tx_direc.Text.Trim() + "-" + tx_dist.Text + "-" + tx_prov.Text + "-" + tx_dpto.Text,
                UbigeoPartida = "",    // tx_dir_ubigpe.Text
                DireccionPartida = "",
                UbigeoLlegada = "",
                DireccionLlegada = "",
                TipoBusquedaProductoCodigo = 0,
                ISC = 0,
                IGV = decimal.Parse(tx_igv.Text),
                ICBPER = 0,
                ImpuestoTotal = decimal.Parse(tx_igv.Text),
                ImpuestoVarios = 0,
                TotalOtrosCargos = 0,
                TotalImporteVenta = decimal.Parse(tx_valor.Text),
                TotalImporteVentaCelular = decimal.Parse(tx_valor.Text),
                TotalImporteVentaReferencia = 0,
                PercepcionTotal = 0,
                TotalPago = decimal.Parse(tx_impMedios.Text),
                PesoTotal = 0,
                Bultos = int.Parse(tx_totcant.Text),

                BienServicioCodigo = (tx_dat_pDet.Text.Trim() == "") ? "001" : tx_dat_sDet.Text,        // codigo del bien o servicio sujeto a detraccion
                DetraccionTipoOperacion = "01",
                Detraccion = (tx_dat_pDet.Text.Trim() == "") ? 0 : (decimal.Parse(tx_valor.Text) * decimal.Parse(tx_dat_pDet.Text) / 100),
                DetraccionPorcentaje = (tx_dat_pDet.Text.Trim() == "") ? 0 : decimal.Parse(tx_dat_pDet.Text),
                DetraccionCuenta = Program.ctadetra,

                Leyenda = (rb_tbienes.Checked == true) ? "0" : (tx_dat_cDet.Text != "") ? "1" : "0",
                RetencionPorcentaje = 0,
                DocAdicionalCodigo = 0,
                DocAdicionalDetalle = "",
                TotalRetencion = 0,
                MontoRetencion = 0,
                PendientePago = 0,
                PermitirCuotas = 0,                             // que significa ?
                AlojamientoPaisDocEmisor = "AF",                // esto ?
                PaisResidencia = "AF",
                Gravado = decimal.Parse(tx_bruto.Text),
                //Observacion = (rb_antic.Checked == true && tx_d_valAntic.Text != "") ? " * * * ANTICIPO * * * " : (rb_antic.Checked == true && tx_tipComp.Text == "C") ? "CANCELACIÓN DE CONTRATO " + tx_cont.Text : "",
                Observacion = tx_coment.Text.Trim(),
                FechaIngresoPais = "01/01/1900",
                FechaIngresoEstablecimiento = "01/01/1900",
                FechaSalidaEstablecimiento = "01/01/1900",
                ModalidadTrasladoCodigo = "01",
                ConductorTipoDocIdentidadCodigo = "1",
                FechaTraslado = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TransportistaNombreRazonSocial = "",
                PlacaVehiculo = "",
                ConductorNumeroDocIdentidad = "",
                AlojamientoNumeroDocIdentidad = "",
                AlojamientoNombreRazonSocial = "",
                AlojamientoTipoDocIdentidadCodigo = "1"
            };
            retorna = JsonConvert.SerializeObject(obj);

            return retorna;
        }
        private string conex_Rapifac()                                                // obtemos la serie y correlativo, actualizmos el correlativo a usar
        {
            string retorna = "";
            string token = conex_token();
            if (token != "")
            {
                // datos variables para la emisión
                string host = "http://wsventas-exp.rapifac.com/v0/comprobantes/series?sucursal=" + tx_codSuc.Text.Trim();

                //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
                httpWebRequest.ContentType = "application/json";         // , text/javascript, */*; q=0.01"
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", "bearer " + token);

                string serComp = "";                                    // obtiene el correlativo para la sede y serie
                string numComp = "";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JArray v = JArray.Parse(result);
                    var items = v.Where(x => x["TipoDocumento"].ToString() == tx_dat_tipdoc_s.Text).ToList();
                    serComp = items[0].SelectToken("Serie").ToString();
                    numComp = items[0].SelectToken("Correlativo").ToString();
                    tx_serie.Text = serComp;
                    //tx_corre.Text = numComp;
                }
                // emite el comprobante
                /*host = "http://wsventas-exp.rapifac.com/v0/comprobantes"; */
                host = "http://wsventas-exp.rapifac.com/v0/comprobantes?IncluirCDR=1";
                httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
                httpWebRequest.ContentType = "application/json";        // , text/javascript, */*; q=0.01
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "bearer " + token);

                int cta_ron = 1;
                List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();
                foreach (DataGridViewRow ron in dataGridView1.Rows) 
                {
                    List<ProductoPrecioDTO> ccc = new List<ProductoPrecioDTO>();
                    if ((rb_antic.Checked == true))     //  && tx_d_valAntic.Text != ""
                    {
                        if (ron.Cells[1].Value != null && ron.Cells[10].Value != null && ron.Cells[10].Value.ToString() == "A")
                        {
                            ProductoPrecioDTO dlp = new ProductoPrecioDTO
                            {
                                PrecioId = 99,
                                PrecioConfiguracion = 1,
                                ProductoCod = "",
                                CodigoUnidadMedida = (rb_tbienes.Checked == true) ? cod_umed : cod_user,
                                DescripcionUnidadMedida = (rb_tbienes.Checked == true) ? nom_umed : nom_user,
                                CantidadUnidadMedida = 1,
                                MonedaCodigo = "PEN",
                                SucursalId = tx_codSuc.Text,
                                Margenganancia = 0,
                                MargenPorcentaje = 0,
                                PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),         // ron.Cells[9].Value.ToString()
                                Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),            // ron.Cells[9].Value.ToString()
                                OtrosCargosPorcentaje = 0,
                                CantidadAplicable = 0,
                                FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                                FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                                Estado = 1,
                                ISCPorcentaje = 0,
                                ISCCalculado = 0,
                                //Extension = { };
                                UUID = ""
                            };
                            ccc.Add(dlp);
                        }
                    }
                    else
                    {
                        if (ron.Cells[1].Value != null)
                        {
                            ProductoPrecioDTO dlp = new ProductoPrecioDTO
                            {
                                PrecioId = 99,
                                PrecioConfiguracion = 1,
                                ProductoCod = ron.Cells[2].Value.ToString(),
                                CodigoUnidadMedida = (rb_tbienes.Checked == true) ? cod_umed : cod_user,
                                DescripcionUnidadMedida = (rb_tbienes.Checked == true) ? nom_umed : nom_user,
                                CantidadUnidadMedida = int.Parse(ron.Cells[1].Value.ToString()),
                                MonedaCodigo = "PEN",
                                SucursalId = tx_codSuc.Text,
                                Margenganancia = 0,
                                MargenPorcentaje = 0,
                                PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),
                                Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),
                                OtrosCargosPorcentaje = 0,
                                CantidadAplicable = 0,
                                FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                                FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                                Estado = 1,
                                ISCPorcentaje = 0,
                                ISCCalculado = 0,
                                //Extension = { };
                                UUID = ""
                            };
                            ccc.Add(dlp);
                        }
                    }
                    if (rb_antic.Checked == true)       //  && tx_d_valAntic.Text != ""
                    {
                        if (ron.Cells[1].Value != null && ron.Cells[10].Value != null && ron.Cells[10].Value.ToString() == "A")
                        {
                            int v_cant = 1;                    // cantidad
                            decimal v_valorUnit = decimal.Parse(ron.Cells[9].Value.ToString()) /      // valor unit (precio unit sin IGV)
                                ((decimal.Parse(v_igv) / 100) + 1);
                            decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                                (decimal.Parse(ron.Cells[9].Value.ToString()) /
                                ((decimal.Parse(v_igv) / 100) + 1));
                            decimal v_valTotal = decimal.Parse(ron.Cells[9].Value.ToString()) /       // valor total fila sin igv
                                ((decimal.Parse(v_igv) / 100) + 1);
                            CComprobanteDetalle det = new CComprobanteDetalle
                            {
                                ID = 0,
                                ComprobanteID = 0,
                                Item = cta_ron,
                                TipoProductoCodigo = "",
                                ProductoCodigo = "",   // "Prod00005",
                                ProductoCodigoSUNAT = "",                       // "56101532",
                                TipoSistemaISCCodigo = "00",
                                UnidadMedidaCodigo = "NIU",
                                PrecioUnitarioSugerido = 0,
                                PrecioUnitarioItem = decimal.Parse(ron.Cells[9].Value.ToString()),       // 118,
                                PrecioVentaCodigo = "01",
                                ICBPER = 0,
                                CargoIndicador = "0",
                                CargoCargoCodigo = "",
                                DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                                DescuentoCargoCodigo = "00",
                                PercepcionCantidadUmbral = 0,
                                PercepcionMontoUmbral = 0,
                                PercepcionPorcentaje = 0,
                                Control = 0,
                                PrecioCompra = 0,
                                EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                                ImporteTotalReferencia = 0,                 // este es el valor referencial 
                                CantidadUnidadMedida = v_cant,
                                Kit = 1,
                                CantidadReferencial = 1,
                                Cargo = 0,
                                DescuentoGlobal = 0,
                                Descuento = 0,
                                ValorUnitario = v_valorUnit,
                                ValorVentaItem = v_valorUnit * v_cant,
                                ValorVentaItemXML = v_valorUnit * v_cant,
                                ValorVentaNeto = v_valorUnit * v_cant,
                                ValorVentaNetoXML = 0,
                                ISCUnitario = 0,
                                ISCNeto = 0,
                                ISC = 0,
                                IGV = v_valIgvTot,
                                ICBPERItem = 0,
                                ICBPERSubTotal = 0,
                                DescuentoBase = 0,
                                DescuentoCargo = 0,
                                DescuentoCargoGravado = 0,
                                CargoItem = 0,
                                CargoTotal = 0,
                                CargoNeto = 0,
                                PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                                MontoTributo = v_valIgvTot,
                                ISCPorcentaje = 0,
                                ISCMonto = 0,
                                CargoPorcentaje = 0,
                                //Extension = { },
                                ListaSeries = new List<CProductoCodigoSerie>(),
                                //ListaPrecios = new List<ProductoPrecioDTO>(),
                                ListaPrecios = ccc,
                                PrecioUnitarioRecuperado = false,
                                UUID = "",
                                BANDERA_CONCURRENCIA = false,
                                BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                                BANDERA_DETALLEREEMPLAZADO = false,
                                BANDERA_DETALLERECUPERADO = false,
                                BANDERA_ITEMDETALLADO = true,
                                Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                                Observacion = "",
                                Stock = 0,
                                Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                                PrecioCodigo = 0,
                                PrecioUnitario = decimal.Parse(ron.Cells[9].Value.ToString()),
                                Peso = 0,
                                DescuentoMonto = 0,
                                DescuentoPorcentaje = "0.00",
                                TipoAfectacionIGVCodigo = "10",                     // esto deberia ser variable
                                ValorVenta = v_valTotal,
                                Ganancia = 0,
                                IGVNeto = v_valIgvTot,
                                ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                                PesoTotal = 0
                            };                      // detalles
                            aaa.Add(det);
                            cta_ron += 1;
                        }
                    }
                    else
                    {
                        if (ron.Cells[1].Value != null)
                        {
                            int v_cant = int.Parse(ron.Cells[1].Value.ToString());                    // cantidad
                            decimal v_valorUnit = decimal.Parse(ron.Cells[8].Value.ToString()) /      // valor unit (precio unit sin IGV)
                                ((decimal.Parse(v_igv) / 100) + 1);
                            decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                                (decimal.Parse(ron.Cells[9].Value.ToString()) /
                                ((decimal.Parse(v_igv) / 100) + 1));
                            decimal v_valTotal = decimal.Parse(ron.Cells[9].Value.ToString()) /       // valor total fila sin igv
                                ((decimal.Parse(v_igv) / 100) + 1);
                            CComprobanteDetalle det = new CComprobanteDetalle
                            {
                                ID = 0,
                                ComprobanteID = 0,
                                Item = cta_ron,
                                TipoProductoCodigo = "",
                                ProductoCodigo = ron.Cells[2].Value.ToString(),   // "Prod00005",
                                ProductoCodigoSUNAT = "",                       // "56101532",
                                TipoSistemaISCCodigo = "00",
                                UnidadMedidaCodigo = "NIU",
                                PrecioUnitarioSugerido = 0,
                                PrecioUnitarioItem = decimal.Parse(ron.Cells[8].Value.ToString()),       // 118,
                                PrecioVentaCodigo = "01",
                                ICBPER = 0,
                                CargoIndicador = "0",
                                CargoCargoCodigo = "",
                                DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                                DescuentoCargoCodigo = "00",
                                PercepcionCantidadUmbral = 0,
                                PercepcionMontoUmbral = 0,
                                PercepcionPorcentaje = 0,
                                Control = 0,
                                PrecioCompra = 0,
                                EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                                ImporteTotalReferencia = 0,                 // este es el valor referencial 
                                CantidadUnidadMedida = v_cant,
                                Kit = 1,
                                CantidadReferencial = 1,
                                Cargo = 0,
                                DescuentoGlobal = 0,
                                Descuento = 0,
                                ValorUnitario = v_valorUnit,
                                ValorVentaItem = v_valorUnit * v_cant,
                                ValorVentaItemXML = v_valorUnit * v_cant,
                                ValorVentaNeto = v_valorUnit * v_cant,
                                ValorVentaNetoXML = 0,
                                ISCUnitario = 0,
                                ISCNeto = 0,
                                ISC = 0,
                                IGV = v_valIgvTot,
                                ICBPERItem = 0,
                                ICBPERSubTotal = 0,
                                DescuentoBase = 0,
                                DescuentoCargo = 0,
                                DescuentoCargoGravado = 0,
                                CargoItem = 0,
                                CargoTotal = 0,
                                CargoNeto = 0,
                                PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                                MontoTributo = v_valIgvTot,
                                ISCPorcentaje = 0,
                                ISCMonto = 0,
                                CargoPorcentaje = 0,
                                //Extension = { },
                                ListaSeries = new List<CProductoCodigoSerie>(),
                                //ListaPrecios = new List<ProductoPrecioDTO>(),
                                ListaPrecios = ccc,
                                PrecioUnitarioRecuperado = false,
                                UUID = "",
                                BANDERA_CONCURRENCIA = false,
                                BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                                BANDERA_DETALLEREEMPLAZADO = false,
                                BANDERA_DETALLERECUPERADO = false,
                                BANDERA_ITEMDETALLADO = true,
                                Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                                Observacion = (rb_antic.Checked == true && tx_tipComp.Text == "C") ? tx_tipComp.Text : (rb_antic.Checked == true && tx_tipComp.Text != "C") ? "Anticipo" : "",
                                Stock = 0,
                                Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                                PrecioCodigo = 0,
                                PrecioUnitario = decimal.Parse(ron.Cells[8].Value.ToString()),
                                Peso = 0,
                                DescuentoMonto = 0,
                                DescuentoPorcentaje = "0.00",
                                TipoAfectacionIGVCodigo = "10",                     // esto deberia ser variable
                                ValorVenta = v_valTotal,
                                Ganancia = 0,
                                IGVNeto = v_valIgvTot,
                                ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                                PesoTotal = 0
                            };                      // detalles
                            aaa.Add(det);
                            cta_ron += 1;
                        }
                    }
                }

                List<CMovimientoCuenta> bbb = new List<CMovimientoCuenta>();
                for (int i = 0; i < 9; i++)
                {
                    if (dtpagos[i, 2] != null && dtpagos[i, 2] != "")
                    {
                        CMovimientoCuenta cta = new CMovimientoCuenta
                        {
                            TipoDocumentoCodigo = tx_dat_tdoc_s.Text,
                            Serie = tx_serie.Text,
                            Correlativo = 0,                // int.Parse(tx_corre.Text),
                            Condicion = dtpagos[i, 2],
                            TipoCuentaCodigo = 1,
                            CuentaNumero = "",
                            Usuario = Program.vg_nuse,
                            MonedaCodigo = "PEN",                                   // de momento todo es soles
                            SucursalId = int.Parse(tx_codSuc.Text),
                            TipoDocIdentidadCodigo = "",
                            NumeroDocIdentidad = "",
                            Observacion = "",
                            //Extension = { },
                            //CuentaValor = "1-30303030",
                            Pago = decimal.Parse(dtpagos[i, 4]),
                            Vuelto = 0,
                            FechaPago = dtpagos[i, 6],   // dtp_pedido.Value.Date.ToString("dd/MM/yyyy")
                            NumeroOperacion = dtpagos[i, 3],
                            FechaVencimiento = dtpagos[i, 6],    // dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            PlazoDias = 1
                        };                      // movimientos
                        bbb.Add(cta);
                    }
                    /*
                    dtpagos[i, 0] = id 
                    dtpagos[i, 1] = contador
                    dtpagos[i, 2] = nombre del medio de pago
                    dtpagos[i, 3] = #operacion
                    dtpagos[i, 4] = importe pagado
                    dtpagos[i, 5] = codigo medio
                    dtpagos[i, 6] = fecha de pago
                    */
                }

                CComprobante obj = new CComprobante
                {
                    ID = 0,
                    IdRepositorio = 0,
                    AplicaContingencia = false,
                    AplicaAnticipo = false,
                    AplicaOtroSistema = false,
                    Usuario = Program.vg_nuse,
                    AplicaStockNegativo = false,
                    ModificacionDePrecio = false,
                    Sucursal = int.Parse(tx_codSuc.Text),
                    IGVPorcentaje = decimal.Parse(Program.v_igv),
                    DescuentoGlobalMonto = 0,
                    DescuentoGlobalIndicadorDescuento = "0",
                    DescuentoGlobalCodigoMotivo = "00",
                    DescuentoGlobalNGPorcentaje = 0,
                    DescuentoGlobalNGIndicadorDescuento = 0,
                    DescuentoGlobalNGCodigoMotivo = "00",
                    CargoGlobalPorcentaje = 0,
                    DetraccionTipoOperacion = "01",                                     // esto debería estar en variable
                    CargoGlobalIndicadorCargos = "0",
                    CargoGlobalCodigoMotivo = "0",
                    CantidadDecimales = 2,
                    AgentePercepcion = false,
                    PermisoProductoSerie = false,
                    EnviarCorreo = false,
                    OrigenSistema = 0,
                    TipoGuiaRemisionCodigo = "",
                    TransportistaTipoDocIdentidadCodigo = "",
                    CanalVenta = "2",
                    Vendedor = usuaDni,                                 // acá debería ir el dni del usuario que hace el comprob
                    VendedorNombre = tx_nomVen.Text,
                    CondicionEstado = "",
                    CondicionPago = (rb_contado.Checked == true)? "Contado" : "Credito",
                    SituacionPagoCodigo = 2,
                    DescuentoIndicador = 0,
                    Ubigeo = tx_dir_ubigpe.Text,
                    AnticipoMonto = 0,
                    ClienteTipoDocIdentidadCodigo = tx_dat_tdoc_s.Text,
                    ClienteNumeroDocIdentidad = tx_ndc.Text,
                    ClienteContacto = "",
                    ClienteTelefono = tx_telef1.Text,
                    OrdenNumero = "",
                    GuiaNumero = "",
                    ReferenciaNumeroDocumento = "",
                    ReferenciaTipoDocumento = "",
                    DiasPermanencia = 0,
                    FechaConsumo = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                    MotivoTrasladoDescripcion = "",
                    TransportistaNumeroDocIdentidad = "",
                    ClienteTipoSunat = 1,
                    VistaDocumento = "",
                    CondicionComercialIndicador = 0,
                    //Extension = {}
                    ListaDetalles = aaa,
                    ExoneradaXML = 0,
                    InafectoXML = 0,
                    ExportacionXML = 0,
                    ImporteTotalTexto = nle.Convertir(tx_valor.Text, true) + tx_dat_monNom.Text, 
                    Detraccion = 0,
                    Percepcion = 0,
                    PercepcionBaseImponible = 0,
                    Retencion = 0,
                    DescuentoGlobalMontoBase = 0,
                    DescuentoGlobalNGMonto = 0,
                    DescuentoGlobalNGMontoBase = 0,
                    DescuentoNGMonto = 0,
                    AnticiposGravado = 0,
                    AnticiposExonerado = 0,
                    AnticiposInafecto = 0,
                    CargoGlobalMonto = 0,
                    CargoGlobalMontoBase = 0,
                    ISCBase = 0,
                    GratuitoGravado = 0,
                    TotalPrecioVenta = decimal.Parse(tx_valor.Text),
                    TotalValorVenta = decimal.Parse(tx_bruto.Text),
                    Peso = 0,
                    PercepcionRegimen = "",
                    PercepcionFactor = 0,
                    ListaMovimientos = bbb,
                    PagosMultiples = false,                             // que significa esto?
                    CreditoTotal = 0,
                    ListaGuias = { },
                    ListaCuotas = { },
                    TotalCuotas = 0,
                    ListaAnticipos = { },
                    ListaDocumentosRelacionados = { },
                    ListaCondicionesComerciales = { },
                    UUID = "",
                    DescuentoGlobalPorcentaje = 0,
                    DescuentoGlobalValor = 0,
                    CorreoElectronicoPrincipal = "no-send@rapifac.com",         // no debería ser correo de artesanos
                    Exonerada = 0,
                    Inafecto = 0,
                    Exportacion = 0,
                    OperacionNoGravada = 0,
                    Gratuito = 0,
                    TotalDescuentos = 0,
                    DescuentoGlobal = 0,
                    TotalAnticipos = 0,
                    BANDERA_CONCURRENCIA = false,
                    BANDERA_DIRECCIONPARTIDAEDICION = false,
                    BANDERA_GANANCIAVERIFICADA = true,
                    BANDERA_ERRORESGANANCIA = false,
                    NOMBRE_UBIGEOLLEGADA = "",
                    NOMBRE_UBIGEOPARTIDA = "",
                    CONTADOR_BUSCAPRODUCTO = 0,
                    CONTADOR_CLICKEMITIR = 1,
                    EstadoContingencia = false,
                    Anticipo = (rb_antic.Checked == true && tx_d_valAntic.Text != "") ? true : false,
                    EstadoOtroSistema = false,
                    ClasePrecioCodigo = 1,
                    TipoPrecio = 0,
                    FormatoPDF = 0,
                    TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                    Serie = tx_serie.Text,       // cmb_tipo.Text.Substring(0,1) + tx_serie.Text
                    Correlativo = 0,             // int.Parse(tx_corre.Text),
                    MonedaCodigo = "PEN",
                    FechaEmision = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                    TipoDocumentoCodigoModificado = tx_dat_tipdoc_s.Text,
                    SerieModificado = "",
                    CorrelativoModificado = "",
                    TipoNotaCreditoCodigo = "01",
                    TipoNotaDebitoCodigo = "01",
                    TipoOperacionCodigo = "0101",                               // todo esto debe estar en variables
                    TipoCambio = "1.00",
                    MotivoTrasladoCodigo = "01",
                    ClienteNombreRazonSocial = tx_nombre.Text.Trim(),
                    ClienteDireccion = tx_direc.Text.Trim() + "-" + tx_dist.Text + "-" + tx_prov.Text + "-" + tx_dpto.Text,
                    UbigeoPartida = "",    // tx_dir_ubigpe.Text
                    DireccionPartida = "",
                    UbigeoLlegada = "",
                    DireccionLlegada = "",
                    TipoBusquedaProductoCodigo = 0,
                    ISC = 0,
                    IGV = decimal.Parse(tx_igv.Text),
                    ICBPER = 0,
                    ImpuestoTotal = decimal.Parse(tx_igv.Text),
                    ImpuestoVarios = 0,
                    TotalOtrosCargos = 0,
                    TotalImporteVenta = decimal.Parse(tx_valor.Text),
                    TotalImporteVentaCelular = decimal.Parse(tx_valor.Text),
                    TotalImporteVentaReferencia = 0,
                    PercepcionTotal = 0,
                    TotalPago = decimal.Parse(tx_impMedios.Text),
                    PesoTotal = 0,
                    Bultos = int.Parse(tx_totcant.Text),
                    Leyenda = (rb_tbienes.Checked == true) ? "0" : (tx_dat_cDet.Text != "") ? "1" : "0",
                    BienServicioCodigo = "001",                     // de donde sale esto? ... desde aca
                    DetraccionPorcentaje = 0,
                    RetencionPorcentaje = 0,
                    DetraccionCuenta = "",
                    DocAdicionalCodigo = 0,
                    DocAdicionalDetalle = "",
                    TotalRetencion = 0,
                    MontoRetencion = 0,
                    PendientePago = 0,
                    PermitirCuotas = 0,                             // que significa ?
                    AlojamientoPaisDocEmisor = "AF",                // esto ?
                    PaisResidencia = "AF",
                    Gravado = decimal.Parse(tx_bruto.Text),
                    Observacion = (rb_antic.Checked == true && tx_d_valAntic.Text != "") ? " * * * ANTICIPO * * * " : (rb_antic.Checked == true && tx_tipComp.Text == "C") ? "CANCELACIÓN DE CONTRATO " + tx_cont.Text : "",
                    //FechaIngresoPais = "22/02/2022",
                    //FechaIngresoEstablecimiento = "22/02/2022",
                    //FechaSalidaEstablecimiento = "22/02/2022",
                    ModalidadTrasladoCodigo = "01",
                    ConductorTipoDocIdentidadCodigo = "1",
                    FechaTraslado = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                    TransportistaNombreRazonSocial = "",
                    PlacaVehiculo = "",
                    ConductorNumeroDocIdentidad = "",
                    AlojamientoNumeroDocIdentidad = "",
                    AlojamientoNombreRazonSocial = "",
                    AlojamientoTipoDocIdentidadCodigo = "1"
                };

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string cabeza = JsonConvert.SerializeObject(obj);
                    streamWriter.Write(cabeza);
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    retorna = result.ToString();
                }
            }
            return retorna;
        }
        private bool anula_Rapifac()                                                // damos de baja el comprobante
        {
            bool retorna = false;
            int cta_ron = 1;
            List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();
            foreach (DataGridViewRow ron in dataGridView1.Rows)
            {
                List<ProductoPrecioDTO> ccc = new List<ProductoPrecioDTO>();
                {
                    if (ron.Cells[1].Value != null)
                    {
                        ProductoPrecioDTO dlp = new ProductoPrecioDTO
                        {
                            PrecioId = 99,
                            PrecioConfiguracion = 1,
                            ProductoCod = ron.Cells[2].Value.ToString(),
                            CodigoUnidadMedida = (rb_tbienes.Checked == true) ? cod_umed : cod_user,              // "NIU",
                            DescripcionUnidadMedida = (rb_tbienes.Checked == true) ? nom_umed : nom_user,         // "UNIDAD",
                            CantidadUnidadMedida = int.Parse(ron.Cells[1].Value.ToString()),
                            MonedaCodigo = tx_dat_mon_s.Text,       // "PEN",
                            SucursalId = tx_codSuc.Text,
                            Margenganancia = 0,
                            MargenPorcentaje = 0,
                            PrecioVenta = decimal.Parse(ron.Cells[8].Value.ToString()),
                            Sugerido = decimal.Parse(ron.Cells[8].Value.ToString()),
                            OtrosCargosPorcentaje = 0,
                            CantidadAplicable = 0,
                            FechaIngreso = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            FechaActualizacion = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                            Estado = 1,
                            ISCPorcentaje = 0,
                            ISCCalculado = 0,
                            //Extension = { };
                            UUID = ""
                        };
                        ccc.Add(dlp);
                    }
                }
                if (ron.Cells[1].Value != null)
                {
                    int v_cant = int.Parse(ron.Cells[1].Value.ToString());                    // cantidad
                    decimal v_valorUnit = decimal.Parse(ron.Cells[8].Value.ToString()) /      // valor unit (precio unit sin IGV)
                        ((decimal.Parse(v_igv) / 100) + 1);
                    decimal v_valIgvTot = decimal.Parse(ron.Cells[9].Value.ToString()) -      // igv total de la fila
                        (decimal.Parse(ron.Cells[9].Value.ToString()) /
                        ((decimal.Parse(v_igv) / 100) + 1));
                    decimal v_valTotal = decimal.Parse(ron.Cells[9].Value.ToString()) /       // valor total fila sin igv
                        ((decimal.Parse(v_igv) / 100) + 1);
                    CComprobanteDetalle det = new CComprobanteDetalle
                    {
                        ID = 0,
                        ComprobanteID = 0,
                        Item = cta_ron,
                        TipoProductoCodigo = "",
                        ProductoCodigo = ron.Cells[2].Value.ToString(),   // "Prod00005",
                        ProductoCodigoSUNAT = "",                       // "56101532",
                        TipoSistemaISCCodigo = "00",
                        UnidadMedidaCodigo = cod_umed,                   // "NIU",
                        PrecioUnitarioSugerido = 0,
                        PrecioUnitarioItem = decimal.Parse(ron.Cells[8].Value.ToString()),       // 118,
                        PrecioVentaCodigo = "01",
                        ICBPER = 0,
                        CargoIndicador = "0",
                        CargoCargoCodigo = "",
                        DescuentoIndicador = 0,                         // no reflejamos descuentos en el comprobante
                        DescuentoCargoCodigo = "00",
                        PercepcionCantidadUmbral = 0,
                        PercepcionMontoUmbral = 0,
                        PercepcionPorcentaje = 0,
                        Control = 0,
                        PrecioCompra = 0,
                        EsAnticipo = false,                         // SI ES ANTICIPO IGUAL ES FALSE
                        ImporteTotalReferencia = 0,                 // este es el valor referencial 
                        CantidadUnidadMedida = v_cant,
                        Kit = 1,
                        CantidadReferencial = 1,
                        Cargo = 0,
                        DescuentoGlobal = 0,
                        Descuento = 0,
                        ValorUnitario = v_valorUnit,
                        ValorVentaItem = v_valorUnit * v_cant,
                        ValorVentaItemXML = v_valorUnit * v_cant,
                        ValorVentaNeto = v_valorUnit * v_cant,
                        ValorVentaNetoXML = 0,
                        ISCUnitario = 0,
                        ISCNeto = 0,
                        ISC = 0,
                        IGV = v_valIgvTot,
                        ICBPERItem = 0,
                        ICBPERSubTotal = 0,
                        DescuentoBase = 0,
                        DescuentoCargo = 0,
                        DescuentoCargoGravado = 0,
                        CargoItem = 0,
                        CargoTotal = 0,
                        CargoNeto = 0,
                        PrecioVenta = decimal.Parse(ron.Cells[9].Value.ToString()),
                        MontoTributo = v_valIgvTot,
                        ISCPorcentaje = 0,
                        ISCMonto = 0,
                        CargoPorcentaje = 0,
                        //Extension = { },
                        ListaSeries = new List<CProductoCodigoSerie>(),
                        //ListaPrecios = new List<ProductoPrecioDTO>(),
                        ListaPrecios = ccc,
                        PrecioUnitarioRecuperado = false,
                        UUID = "",
                        BANDERA_CONCURRENCIA = false,
                        BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE = false,
                        BANDERA_DETALLEREEMPLAZADO = false,
                        BANDERA_DETALLERECUPERADO = false,
                        BANDERA_ITEMDETALLADO = true,
                        Descripcion = ron.Cells[3].Value.ToString(),            // "00 PRODUCTO GRAVADO",
                        Observacion = (rb_antic.Checked == true && tx_tipComp.Text == "C") ? tx_tipComp.Text : (rb_antic.Checked == true && tx_tipComp.Text != "C") ? "Anticipo" : "",
                        Stock = 0,
                        Cantidad = (rb_antic.Checked == true) ? 1 : int.Parse(ron.Cells[1].Value.ToString()),       //  && tx_d_valAntic.Text != ""
                        PrecioCodigo = 0,
                        PrecioUnitario = decimal.Parse(ron.Cells[8].Value.ToString()),
                        Peso = 0,
                        DescuentoMonto = 0,
                        DescuentoPorcentaje = "0.00",
                        TipoAfectacionIGVCodigo = "10",                     // Sunat Catalogo 7 - Venta grabada operación onerosa
                        ValorVenta = v_valTotal,
                        Ganancia = 0,
                        IGVNeto = v_valIgvTot,
                        ImporteTotal = decimal.Parse(ron.Cells[9].Value.ToString()),
                        PesoTotal = 0
                    };                      // detalles
                    aaa.Add(det);
                    cta_ron += 1;
                }
            }
            List<CMovimientoCuenta> bbb = new List<CMovimientoCuenta>();
            for (int i = 0; i < 9; i++)
            {
                if (dtpagos[i, 2] != null && dtpagos[i, 2] != "")
                {
                    CMovimientoCuenta cta = new CMovimientoCuenta
                    {
                        TipoDocumentoCodigo = tx_dat_tdoc_s.Text,
                        Serie = tx_serie.Text,
                        Correlativo = 0,                // int.Parse(tx_corre.Text),
                        Condicion = dtpagos[i, 2],
                        TipoCuentaCodigo = 1,
                        CuentaNumero = "",
                        Usuario = Program.vg_nuse,
                        MonedaCodigo = tx_dat_mon_s.Text,     // "PEN",                                   // de momento todo es soles
                        SucursalId = int.Parse(tx_codSuc.Text),
                        TipoDocIdentidadCodigo = "",
                        NumeroDocIdentidad = "",
                        Observacion = "",
                        //Extension = { },
                        //CuentaValor = "1-30303030",
                        Pago = decimal.Parse(dtpagos[i, 4]),
                        Vuelto = 0,
                        FechaPago = dtpagos[i, 6],   // dtp_pedido.Value.Date.ToString("dd/MM/yyyy")
                        NumeroOperacion = dtpagos[i, 3],
                        FechaVencimiento = dtpagos[i, 6],    // dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                        PlazoDias = 1
                    };                      // movimientos
                    bbb.Add(cta);
                }
                /*
                dtpagos[i, 0] = id 
                dtpagos[i, 1] = contador
                dtpagos[i, 2] = nombre del medio de pago
                dtpagos[i, 3] = #operacion
                dtpagos[i, 4] = importe pagado
                dtpagos[i, 5] = codigo medio
                dtpagos[i, 6] = fecha de pago
                */
            }
            CComprobante obj_anu = new CComprobante
            {
                ID = int.Parse(tx_id_rapifac.Text),
                DocumentoID = cmb_tipo.Text.Substring(0, 1) + tx_serie.Text + tx_corre.Text,
                TipoDocumentoCodigo = tx_dat_tipdoc_s.Text,
                Serie = cmb_tipo.Text.Substring(0, 1) + tx_serie.Text,
                Correlativo = int.Parse(tx_corre.Text),
                FechaEmision = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                FechaRegistro = dtp_pedido.Value.Date.ToString("dd/MM/yyyy"),
                TipoNotaCreditoCodigo = "01",
                TipoNotaDebitoCodigo = "01",
                TipoOperacionCodigo = "0101",
                TipoDocumentoCodigoModificado = tx_dat_tipdoc_s.Text,
                SerieModificado = "",
                CorrelativoModificado = "",
                CondicionEstado = "",
                ImporteTotalTexto = nle.Convertir(tx_valor.Text, true) + tx_dat_monNom.Text,
                Observacion = "",
                MonedaCodigo = tx_dat_mon_s.Text,
                TipoCambio = "1.00",
                GuiaNumero = "",
                ReferenciaNumeroDocumento = "",
                ReferenciaTipoDocumento = "",
                ClienteTipoDocIdentidadCodigo = tx_dat_tdoc_s.Text,
                ClienteNumeroDocIdentidad = tx_ndc.Text,
                ClienteNombreRazonSocial = tx_nombre.Text.Trim(),
                ClienteDireccion = tx_direc.Text.Trim() + "-" + tx_dist.Text + "-" + tx_prov.Text + "-" + tx_dpto.Text,
                CorreoElectronicoPrincipal = null,
                ClienteContacto = "",
                ClienteTelefono = "",
                Detraccion = 0,
                DetraccionCuenta = "",
                DetraccionTipoOperacion = "",
                DetraccionPorcentaje = 0,
                BienServicioCodigo = "1",
                CondicionPago = (rb_contado.Checked == true) ? "Contado" : "Credito",
                CreditoTotal = 0,
                Percepcion = 0,
                PercepcionRegimen = "",
                PercepcionFactor = 0,
                PercepcionTotal = 0,
                PercepcionBaseImponible = 0,
                Retencion = 0,
                MontoRetencion = 0,
                DescuentoGlobal = 0,
                DescuentoIndicador = 0,
                DescuentoGlobalMonto = 0,
                DescuentoGlobalMontoBase = 0,
                DescuentoGlobalPorcentaje = 0,
                DescuentoGlobalValor = 0,
                DescuentoGlobalIndicadorDescuento = "0",
                DescuentoGlobalCodigoMotivo = "00",
                DescuentoGlobalNGPorcentaje = 0,
                DescuentoGlobalNGIndicadorDescuento = 0,
                DescuentoGlobalNGCodigoMotivo = "00",
                DescuentoGlobalNGMonto = 0,
                DescuentoGlobalNGMontoBase = 0,
                DescuentoNGMonto = 0,
                AnticiposGravado = 0,
                AnticiposExonerado = 0,
                AnticiposInafecto = 0,
                CargoGlobalMonto = 0,
                CargoGlobalMontoBase = 0,
                CargoGlobalPorcentaje = 0,
                CargoGlobalIndicadorCargos = "0",
                CargoGlobalCodigoMotivo = "0",
                ImpuestoTotal = decimal.Parse(tx_igv.Text),
                ISC = 0,
                ISCBase = 0,
                IGV = decimal.Parse(tx_igv.Text),
                IGVPorcentaje = decimal.Parse(Program.v_igv),
                Gravado = decimal.Parse(tx_bruto.Text),
                ICBPER = 0,
                Exonerada = 0,
                Inafecto = 0,
                Exportacion = 0,
                ExoneradaXML = 0,
                InafectoXML = 0,
                ExportacionXML = 0,
                Gratuito = 0,
                GratuitoGravado = 0,
                TotalImporteVenta = decimal.Parse(tx_valor.Text),
                TotalImporteVentaCelular = decimal.Parse(tx_valor.Text),
                TotalImporteVentaReferencia = 0,
                TotalPrecioVenta = decimal.Parse(tx_valor.Text),
                TotalValorVenta = decimal.Parse(tx_bruto.Text),
                TotalDescuentos = 0,
                TotalOtrosCargos = 0,
                TotalAnticipos = 0,
                TotalPago = decimal.Parse(tx_impMedios.Text),
                PendientePago = 0,
                EnviarCorreo = false,
                FormatoPDF = 0,
                IdRepositorio = 0,                                  // que va aca?
                VistaDocumento = "",
                Sucursal = int.Parse(tx_codSuc.Text),
                Usuario = Program.vg_nuse,
                Vendedor = usuaDni,                                 // acá debería ir el dni del usuario que hace el comprob
                VendedorNombre = tx_nomVen.Text,
                CanalVenta = "2",
                AlojamientoPaisDocEmisor = "",
                AlojamientoNumeroDocIdentidad = "",
                AlojamientoNombreRazonSocial = "",
                AlojamientoTipoDocIdentidadCodigo = "",
                PaisResidencia = "",
                FechaConsumo = null,
                DiasPermanencia = 0,
                Peso = 0,
                PesoTotal = 0,
                Bultos = int.Parse(tx_totcant.Text),
                MotivoTrasladoDescripcion = "",
                MotivoTrasladoCodigo = "01",
                ModalidadTrasladoCodigo = "",
                FechaTraslado = null,
                TransportistaNumeroDocIdentidad = "",
                TransportistaNombreRazonSocial = "",
                TransportistaTipoDocIdentidadCodigo = "",
                ConductorTipoDocIdentidadCodigo = "",
                PlacaVehiculo = "",
                ConductorNumeroDocIdentidad = "",
                UbigeoLlegada = "",
                DireccionLlegada = "",
                NOMBRE_UBIGEOLLEGADA = "",
                UbigeoPartida = "",
                DireccionPartida = "",
                NOMBRE_UBIGEOPARTIDA = "",
                Anticipo = false,
                EstadoContingencia = false,
                Baja = 0,
                Leyenda = (rb_tbienes.Checked == true) ? "0" : (tx_dat_cDet.Text != "") ? "1" : "0",
                MotivoBaja = "Anulación",
                OrigenSistema = 0,
                TipoPrecio = 0,
                Ubigeo = tx_dir_ubigpe.Text,
                Estado = 0,
                DocAdicionalCodigo = 0,
                DocAdicionalDetalle = "",
                PagosMultiples = false,
                ListaDetalles = aaa,
                ListaAnticipos = { },
                ListaCuotas = { },
                ListaGuias = { },
                ListaMovimientos = bbb,
                ListaDocumentosRelacionados = { },
                ListaCondicionesComerciales = { }
            };
            string cabeza = JsonConvert.SerializeObject(obj_anu);

            string token = conex_token();
            // ANULA el comprobante
            string host = "http://wsventas-exp.rapifac.com/v0/comprobantes/anular";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", "bearer " + token);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(cabeza);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //MessageBox.Show(result.ToString());
                retorna = (bool.Parse(result.ToString()) == true) ? true : false;
            }

            return retorna;
        }
        private bool Jala_oc_Rapifac()                                              // jala comprobante de rapifac
        {
            bool retorna = false;
            /*      // NO NECESITAMOS ESTO ... 10/08/2022
            string token = conex_token();
            // obtiene el comprobante desde rapifac
            string host = "http://wsventas-exp.rapifac.com/v0/comprobantes";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.ContentType = "application/json";        // , text/javascript, 
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "bearer " + token);
            // .... para jalar es necesario el ID y ademas serie y numero?
            // no se podría jalar solo con la serie y numero ??
            */
            return retorna;
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

        #region impresion
        private bool imprimeTK()
        {
            bool retorna = false;
            {
                printDocument1.PrinterSettings.PrinterName = v_impTK;
                printDocument1.Print();
                retorna = true;
            }
            return retorna;
        }
        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            imprime_TK(sender, e);
            if (File.Exists(@otro))
            {
                //File.Delete(@"C:\test.txt");
                File.Delete(@otro);
            }
        }
        private void imprime_TK(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            {
                // DATOS PARA EL TICKET
                //string nomclie = "";
                string rasclie = Program.cliente;
                string rucclie = Program.ruc;
                string dirclie = Program.direClte + " - " + Program.disProDpto;
                string dirloc = tx_dir_pe.Text;
                // TIPOS DE LETRA PARA EL DOCUMENTO FORMATO TICKET
                Font lt_gra = new Font("Arial", 11);                // grande
                Font lt_tit = new Font("Lucida Console", 10);       // mediano
                Font lt_med = new Font("Arial", 9);                // normal textos
                Font lt_peq = new Font("Arial", 8);                 // pequeño
                Font lt_det = new Font("Arial", 7);                // letras mas pequeñas del DETALLE
                float anchTik = 7.8F;                               // ancho del TK en centimetros
                int coli = 5;                                      // columna inicial
                float posi = 20;                                    // posicion x,y inicial
                int alfi = 15;                                      // alto de cada fila
                float ancho = 360.0F;                                // ancho de la impresion
                int copias = 1;                                     // cantidad de copias del ticket
                float lt;
                for (int i = 1; i <= copias; i++)
                {
                    PointF puntoF = new PointF(coli, posi);
                    // imprimimos el logo o el nombre comercial del emisor
                    if (logoclt != "")
                    {
                        Image photo = Image.FromFile(logoclt);
                        SizeF cuadLogo = new SizeF(lib.CentimeterToPixel(anchTik) - 20.0F, alfi * 6);
                        RectangleF reclogo = new RectangleF(puntoF, cuadLogo);
                        e.Graphics.DrawImage(photo, reclogo);
                    }
                    else
                    {
                        lt = (ancho - e.Graphics.MeasureString(rasclie, lt_gra).Width) / 2;
                        puntoF = new PointF(lt, posi);
                        e.Graphics.DrawString(rasclie, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // razon social
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                    }
                    e.Graphics.DrawString("Dom.Fiscal", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // direccion emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    SizeF cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    RectangleF recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(dirclie, lt_med, Brushes.Black, recdom, StringFormat.GenericTypographic);     // direccion emisor
                    posi = posi + alfi * 2;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Sucursal", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // direccion emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(dirloc, lt_med, Brushes.Black, recdom, StringFormat.GenericTypographic);     // direccion punto de venta
                    posi = posi + alfi * 2;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("RUC ", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // ruc de emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(rucclie, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // ruc de emisor
                    string tipdo = cmb_tipo.Text;                                  // tipo de documento
                    string serie = cmb_tipo.Text.Substring(0, 1).ToUpper() + lib.Right(tx_serie.Text, 3);                    // serie electrónica
                    string corre = tx_corre.Text;                                // numero del documento electrónico
                    string titdoc = "";
                    if (tx_dat_tipdoc.Text != codfact) titdoc = "Boleta de Venta Electrónica";
                    if (tx_dat_tipdoc.Text == codfact) titdoc = "Factura Electrónica";
                    posi = posi + alfi + 8;
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(titdoc, lt_gra).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(titdoc, lt_gra, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // tipo de documento
                    posi = posi + alfi + 8;
                    string titnum = serie + " - " + corre;
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(titnum, lt_gra).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(titnum, lt_gra, Brushes.Black, puntoF, StringFormat.GenericTypographic);   // serie y numero
                    posi = posi + alfi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("F. Emisión", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic); // fecha y hora emision
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(dtp_pedido.Value.ToString("dd/MM/yyyy"), lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic); // fecha y hora emision
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Cliente", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // DNI/RUC cliente
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    if (tx_nombre.Text.Trim().Length > 39) cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    else cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 1);
                    recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(tx_nombre.Text.Trim(), lt_peq, Brushes.Black, recdom, StringFormat.GenericTypographic);                  // DNI/RUC cliente
                    if (tx_nombre.Text.Trim().Length > 39) posi = posi + alfi + alfi;
                    else posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    if (tx_dat_tipdoc.Text == codfact)
                    {
                        e.Graphics.DrawString("RUC", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // nombre del cliente
                    }
                    else
                    {
                        if (tx_dat_tdoc.Text == vtc_dni)
                        {
                            e.Graphics.DrawString("DNI", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // nombre del cliente
                        }
                        else
                        {
                            e.Graphics.DrawString("OTROS", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // nombre del cliente
                        }
                    }
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(tx_ndc.Text, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // ruc/dni del cliente
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Dirección", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);  // direccion
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    string dipa = tx_direc.Text.Trim() + Environment.NewLine + tx_dist.Text.Trim() + " - " + tx_prov.Text.Trim() + " - " + tx_dpto.Text.Trim();
                    RectangleF recdir;
                    if (dipa.Length < 30)
                    {
                        cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi );
                        recdir = new RectangleF(puntoF, cuad);
                        e.Graphics.DrawString(tx_direc.Text.Trim() + 
                            tx_dist.Text.Trim() + " - " + tx_prov.Text.Trim() + " - " + tx_dpto.Text.Trim(),
                            lt_peq, Brushes.Black, recdir, StringFormat.GenericTypographic);  // direccion
                        posi = posi + alfi;
                    }
                    else
                    {
                        if (dipa.Length < 60)
                        {
                            cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                            posi = posi + alfi + alfi;
                        }
                        else
                        {
                            cuad = new SizeF(lib.CentimeterToPixel(anchTik) - (coli + 70), alfi * 3);
                            posi = posi + alfi + alfi + alfi;
                        }
                        recdir = new RectangleF(puntoF, cuad);
                        e.Graphics.DrawString(tx_direc.Text.Trim() + Environment.NewLine +
                            tx_dist.Text.Trim() + " - " + tx_prov.Text.Trim() + " - " + tx_dpto.Text.Trim(),
                            lt_peq, Brushes.Black, recdir, StringFormat.GenericTypographic);  // direccion
                    }
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString(" ", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    // **************** detalle del documento ****************//
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("---------------------------------------------------------------------------", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    StringFormat alder = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                    SizeF siz = new SizeF(70, 15);
                    RectangleF recto = new RectangleF(puntoF, siz);
                    //puntoF = new PointF(coli, posi);
                    //e.Graphics.DrawString("Descripción", lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    //posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Descripción                            Cantidad       Precio         Importe", lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("---------------------------------------------------------------------------", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    double valcont = 0;             // total importes de items para casos de cancelacion 
                    double desglob = 0;
                    double.TryParse(tx_desGlob.Text, out desglob);
                    if (tx_tipComp.Text == "B") // ventas directas
                    {
                        for (int l = 0; l < dataGridView1.Rows.Count - 1; l++)
                        {
                            string nomprod = dataGridView1.Rows[l].Cells[3].Value.ToString().Trim() + " " + dataGridView1.Rows[l].Cells[5].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(dataGridView1.Rows[l].Cells[0].Value.ToString()))
                            {
                                puntoF = new PointF(coli, posi);
                                var kk = e.Graphics.MeasureString(nomprod, lt_det);
                                if (kk.Width > lib.CentimeterToPixel(anchTik))
                                {
                                    siz = new SizeF(lib.CentimeterToPixel(anchTik) - 10.0F, 30);
                                    recto = new RectangleF(puntoF, siz);
                                    e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, recto, StringFormat.GenericTypographic);
                                    posi = posi + alfi * 2;
                                }
                                else
                                {
                                    e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                    posi = posi + alfi;
                                }
                                puntoF = new PointF(coli + 150, posi);
                                e.Graphics.DrawString(dataGridView1.Rows[l].Cells[1].Value.ToString(), lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                puntoF = new PointF(coli + 180.0F, posi);
                                e.Graphics.DrawString(dataGridView1.Rows[l].Cells[8].Value.ToString(), lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                puntoF = new PointF(coli + 199, posi);// coli + 190
                                siz = new SizeF(70, 30);
                                recto = new RectangleF(puntoF, siz);
                                e.Graphics.DrawString(dataGridView1.Rows[l].Cells[9].Value.ToString(), lt_det, Brushes.Black, recto, alder);
                                posi = posi + alfi;
                            }
                        }
                    }
                    if (tx_tipComp.Text == "A") //  && vpago != "cancelacion" // anticipo
                    {
                        puntoF = new PointF(coli, posi);
                        var kk = e.Graphics.MeasureString(dataGridView1.Rows[0].Cells[3].Value.ToString(), lt_det);
                        if (kk.Width > lib.CentimeterToPixel(anchTik))
                        {
                            siz = new SizeF(lib.CentimeterToPixel(anchTik) - 10.0F, 30);
                            recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString(dataGridView1.Rows[0].Cells[3].Value.ToString(), lt_det, Brushes.Black, recto, StringFormat.GenericTypographic); e.Graphics.DrawString(dataGridView1.Rows[0].Cells[3].Value.ToString(), lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                            puntoF = new PointF(coli + 199, posi);
                            siz = new SizeF(70, 30);
                            recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString(dataGridView1.Rows[0].Cells[9].Value.ToString(), lt_det, Brushes.Black, recto, alder);
                            posi = posi + alfi * 2;
                        }
                        else
                        {
                            e.Graphics.DrawString(dataGridView1.Rows[0].Cells[3].Value.ToString(), lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            puntoF = new PointF(coli + 199, posi);
                            siz = new SizeF(70, 30);
                            recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString(dataGridView1.Rows[0].Cells[9].Value.ToString(), lt_det, Brushes.Black, recto, alder);
                            posi = posi + alfi;
                        }
                        for (int l = 1; l < dataGridView1.Rows.Count - 1; l++)
                        {
                            string nomprod = dataGridView1.Rows[l].Cells[1].Value.ToString() + "  " + dataGridView1.Rows[l].Cells[3].Value.ToString().Trim() + " " + dataGridView1.Rows[l].Cells[5].Value.ToString().Trim();
                            puntoF = new PointF(coli, posi);
                            //e.Graphics.DrawString(dataGridView1.Rows[l].Cells[1].Value.ToString(), lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            //puntoF = new PointF(coli + 30.0F, posi);
                            kk = e.Graphics.MeasureString(nomprod, lt_det);
                            if (kk.Width > lib.CentimeterToPixel(anchTik))
                            {
                                siz = new SizeF(lib.CentimeterToPixel(anchTik) - 10.0F, 30);
                                recto = new RectangleF(puntoF, siz);
                                e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, recto, StringFormat.GenericTypographic);
                                posi = posi + alfi * 2;
                            }
                            else
                            {
                                e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                posi = posi + alfi;
                            }
                        }
                    }
                    if (tx_tipComp.Text == "C") //  && vpago == "cancelacion"
                    {
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("CANCELACION DE CONTRATO " + tx_cont.Text, lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        posi = posi + alfi;
                        for (int l = 0; l < dataGridView1.Rows.Count - 1; l++)
                        {
                            string nomprod = dataGridView1.Rows[l].Cells[3].Value.ToString().Trim() + " " + dataGridView1.Rows[l].Cells[5].Value.ToString().Trim();
                            if (dataGridView1.Rows[l].Cells[1].Value.ToString() != "0" && dataGridView1.Rows[l].Cells[1].Value.ToString().Trim() != "")
                            {
                                double ventu = double.Parse(dataGridView1.Rows[l].Cells[9].Value.ToString());   // - double.Parse(dataGridView1.Rows[l].Cells[11].Value.ToString());

                                puntoF = new PointF(coli, posi);
                                var kk = e.Graphics.MeasureString(nomprod, lt_det);
                                if (kk.Width > lib.CentimeterToPixel(anchTik))
                                {
                                    siz = new SizeF(lib.CentimeterToPixel(anchTik) - 10.0F, 30);
                                    recto = new RectangleF(puntoF, siz);
                                    e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, recto, StringFormat.GenericTypographic);
                                    //puntoF = new PointF(coli + 199, posi);
                                    //siz = new SizeF(70, 30);
                                    //recto = new RectangleF(puntoF, siz);
                                    //e.Graphics.DrawString(ventu.ToString("#0.00"), lt_det, Brushes.Black, recto, alder);
                                    posi = posi + alfi * 2;
                                }
                                else
                                {
                                    e.Graphics.DrawString(nomprod, lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                    posi = posi + alfi;
                                }
                                puntoF = new PointF(coli + 150, posi);
                                e.Graphics.DrawString(dataGridView1.Rows[l].Cells[1].Value.ToString(), lt_det, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                puntoF = new PointF(coli + 199, posi);
                                siz = new SizeF(70, 30);
                                recto = new RectangleF(puntoF, siz);
                                e.Graphics.DrawString(ventu.ToString("#0.00"), lt_det, Brushes.Black, recto, alder);

                                valcont = valcont + ventu;
                                posi = posi + alfi;
                            }
                            else
                            {
                                // no imprimes nada porque son las filas de anticipos
                            }
                        }
                    }
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("---------------------------------------------------------------------------", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    // pie del documento
                    if (tx_tipComp.Text == "C") //  && vpago == "cancelacion"
                    {
                        double valant = 0;
                        siz = new SizeF(70, 15);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("TOTAL VENTA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        RectangleF recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(valcont.ToString("#0.00"), lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        // LISTA DE ANTICIPOS CON SIMBOLO NEGATIVO
                        for (int l = 0; l < _docsAnticip.Count; l++)  // la primera fila es la cancelacion    ACA ESTA EL ASUNTO ...  
                        {                                                       // solo ponemos los anticipos, PARA ESTO TENDREMOS UNA LISTA CON LOS COMPROBRANTES Y SUS VALORES
                            if (true)   // dataGridView1.Rows[l].Cells[1].Value.ToString() == "0"
                            {
                                double venga = double.Parse(_docsAnticip[l].valor);  // dataGridView1.Rows[l].Cells[9].Value.ToString()
                                puntoF = new PointF(coli, posi);    // dataGridView1.Rows[l].Cells[3].Value.ToString()
                                e.Graphics.DrawString("Anticipo " + (l + 1).ToString() + ": " + _docsAnticip[l].comprob, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                puntoF = new PointF(coli + 199, posi);
                                siz = new SizeF(70, 30);
                                recto = new RectangleF(puntoF, siz);
                                e.Graphics.DrawString(venga.ToString("#0.00") + "- ", lt_peq, Brushes.Black, recto, alder);
                                posi = posi + alfi;
                                valant = valant + venga;
                            }
                        }
                        //
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("SUB TOTAL VENTA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString((valcont - valant).ToString("#0.00"), lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        // A PARTIR DE ACA LOS VALORES DEL PAGO ACTUAL
                        if (tx_desGlob.Text.Trim() != "")
                        {
                            puntoF = new PointF(coli, posi);
                            e.Graphics.DrawString("DESCUENTO GLOBAL", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            puntoF = new PointF(coli + 199, posi);
                            recst = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString((desglob).ToString("#0.00"), lt_peq, Brushes.Black, recst, alder);
                        }
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. GRAVADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_bruto.Text, lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IGV", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        RectangleF recgv = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_igv.Text, lt_peq, Brushes.Black, recgv, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IMPORTE TOTAL " + cmb_mon.Text, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        siz = new SizeF(70, 15);
                        recto = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_valor.Text, lt_peq, Brushes.Black, recto, alder);
                    }
                    else
                    {
                        siz = new SizeF(70, 15);
                        posi = posi + alfi;
                        if (tx_desGlob.Text.Trim() != "")
                        {
                            puntoF = new PointF(coli, posi);
                            e.Graphics.DrawString("DESCUENTO GLOBAL", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            puntoF = new PointF(coli + 199, posi);
                            RectangleF recsD = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString((desglob).ToString("#0.00"), lt_peq, Brushes.Black, recsD, alder);
                            posi = posi + alfi;
                        }
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. GRAVADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        RectangleF recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_bruto.Text, lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IGV", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        RectangleF recgv = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_igv.Text, lt_peq, Brushes.Black, recgv, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IMPORTE TOTAL " + cmb_mon.Text, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 199, posi);
                        siz = new SizeF(70, 15);
                        recto = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_valor.Text, lt_peq, Brushes.Black, recto, alder);
                    }
                    if (tx_coment.Text.Trim() != "")
                    {
                        posi = posi + alfi * 2;
                        puntoF = new PointF(coli, posi);
                        string Vobs = "Observ.: " + tx_coment.Text.Trim();
                        if (Vobs.Length <= 30) siz = new SizeF(lib.CentimeterToPixel(anchTik), alfi);
                        else siz = new SizeF(lib.CentimeterToPixel(anchTik), alfi * 2);
                        recto = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(Vobs, lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                        if (Vobs.Length <= 30) posi = posi + alfi;
                        else posi = posi + alfi + alfi;
                    }
                    else
                    {
                        posi = posi + alfi * 2;
                    }
                    puntoF = new PointF(coli, posi);
                    NumLetra nl = new NumLetra();
                    string monlet = "SON: " + nl.Convertir(tx_valor.Text.ToString(), true);
                    if (monlet.Length <= 30) siz = new SizeF(lib.CentimeterToPixel(anchTik), alfi);
                    else siz = new SizeF(lib.CentimeterToPixel(anchTik), alfi * 2);
                    recto = new RectangleF(puntoF, siz);
                    e.Graphics.DrawString(monlet, lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                    if (monlet.Length <= 30) posi = posi + alfi;
                    else posi = posi + alfi + alfi;
                    if (rb_contado.Checked == true)
                    {
                        for (int x=0; x < 9; x++)
                        {
                            if (dtpagos[x, 2] != null && dtpagos[x, 2].ToString().Trim() != "")
                            {
                                puntoF = new PointF(coli, posi);
                                e.Graphics.DrawString(dtpagos[x, 2].ToString() + " " + cmb_mon.Text + " " + dtpagos[x, 4].ToString() + " #.Op. " + dtpagos[x, 3].ToString() + " " + dtpagos[x, 6].ToString(), lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                posi = posi + alfi;
                            }
                        }
                    }
                    else
                    {
                        if (rb_credito.Checked == true)
                        {
                            // no hay ventas al credito 18/07/2022
                        }
                    }
                    /*
                    if (tx_dat_tipdoc.Text == codfact)
                    {
                        if (double.Parse(tx_valor.Text) > double.Parse(Program.valdetra))   // de momento no vamos 20/07/2022
                        {
                            posi = posi + alfi * 1.5F;
                            siz = new SizeF(lib.CentimeterToPixel(anchTik), 15 * 4);
                            puntoF = new PointF(coli, posi);
                            recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString(leydet1.Trim() + " " + leydet2 + " " + Program.ctadetra.Trim(), lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                            posi = posi + alfi * 3;
                        }
                        else
                        {
                            posi = posi + alfi;
                        }
                    }
                    */
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    string repre = "Representación impresa de la";
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(repre, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(repre, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    string previo = "";
                    if (tx_dat_tipdoc.Text != codfact) previo = "boleta de venta electrónica";
                    if (tx_dat_tipdoc.Text == codfact) previo = "factura electrónica";
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(previo, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(previo, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    //posi = posi + alfi;
                    string separ = "|";
                    string codigo = rucclie + separ + tipdo + separ +
                        serie + separ + tx_corre.Text + separ +
                        tx_igv.Text + separ + tx_valor.Text + separ +
                        dtp_pedido.Value.Year.ToString() + "-" + dtp_pedido.Value.Month.ToString() + "-" + dtp_pedido.Value.Day.ToString() + separ + tipoDocEmi + separ +
                        tx_ndc.Text + separ;
                    //
                    var rnd = Path.GetRandomFileName();
                    otro = Path.GetFileNameWithoutExtension(rnd);
                    otro = otro + ".png";
                    //
                    var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    var qrCode = qrEncoder.Encode(codigo);
                    var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                    using (var stream = new FileStream(otro, FileMode.Create))
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    Bitmap png = new Bitmap(otro);
                    posi = posi + alfi + 7;
                    lt = (lib.CentimeterToPixel(anchTik) - lib.CentimeterToPixel(2)) / 2;
                    puntoF = new PointF(lt, posi);
                    SizeF cuadro = new SizeF(lib.CentimeterToPixel(2), lib.CentimeterToPixel(2));    // 5x5 cm
                    RectangleF rec = new RectangleF(puntoF, cuadro);
                    e.Graphics.DrawImage(png, rec);
                    png.Dispose();
                    // leyenda 2
                    posi = posi + lib.CentimeterToPixel(2);
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(restexto, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(restexto, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    lt = (lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(autoriz, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(autoriz, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    // centrado en rectangulo   *********************
                    StringFormat sf = new StringFormat();       //  *
                    sf.Alignment = StringAlignment.Center;      //  *
                    posi = posi + alfi + 5;
                    SizeF leyen = new SizeF(lib.CentimeterToPixel(anchTik) - 20, alfi * 3);
                    puntoF = new PointF(coli, posi);
                    leyen = new SizeF(lib.CentimeterToPixel(anchTik) - 20, alfi * 2);
                    RectangleF recley5 = new RectangleF(puntoF, leyen);
                    e.Graphics.DrawString("Integrador - Rapifac", lt_med, Brushes.Black, recley5, sf);
                    posi = posi + alfi * 3;
                    string locyus = cmb_taller.Text + " - " + tx_nomVen.Text;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString(locyus, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // tienda y vendedor
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Imp. " + DateTime.Now, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi + alfi;
                    puntoF = new PointF((lib.CentimeterToPixel(anchTik) - e.Graphics.MeasureString(despe2, lt_med).Width) / 2, posi);
                    e.Graphics.DrawString(despe2, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi + alfi;
                    //puntoF = new PointF(coli, posi);
                    //e.Graphics.DrawString(".", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                }
            }
        }
        private void updateprint(string sn)  // actualiza el campo impreso de la GR = S
        {   // S=si impreso || N=no impreso
            /*
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "update cabfactu set impreso=@sn where id=@idr";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.Parameters.AddWithValue("@sn", sn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
                }
            }
            */
        }

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
    public class docsAnticip                                                // comprobantes de anticipo
    {
        public string comprob { get; set; }                 // comprobante completo
        public string valor { get; set; }                   // ImporteTotal - rapifac
        public string bruto { get; set; }                   // ValorVentaItem - rapifac
        public string igv { get; set; }                     // IGV - rapifac
        public string descrip { get; set; }                 // Descripcion - rapifac
        public int IdCompRapifac { get; set; }              // ComprobanteId - rapifac
        public string tipDocCod { get; set; }               // tipoDocumentoCodigo - rapifac
        public string serieRap { get; set; }                // serie - rapifac, "F200"
        public int corRap { get; set; }                     // correlativo - rapifac
        public string codMon { get; set; }                  // monedaCodigo - rapifac
        public string tDocClte { get; set; }                // tipoDocIdentidadCodigo - rapifac
        public string nroDocC { get; set; }                 // numeroDocIdentidad - rapifac
        public string fecEmi { get; set; }                  // fecha de emision del comprobante - rapifac
    };
    public class Cdr
    {
        public int IDComprobante { get; set; }
        public int Codigo { get; set; }
        public string IDRepositorio { get; set; }
        public string Firma { get; set; }
        public string Mensaje { get; set; }
    }
    public class Root
    {
        public XmlPdf xml_pdf { get; set; }
        public Cdr cdr { get; set; }
    }
    public class XmlPdf
    {
        public int IDComprobante { get; set; }
        public int Codigo { get; set; }
        public string IDRepositorio { get; set; }
        public string Firma { get; set; }
        public string Mensaje { get; set; }
    }

}
