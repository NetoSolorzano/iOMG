﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class main : Form
    {
        #region conexion a la base de datos
        // own database connection
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";port=" + port + ";uid=" + usua + ";pwd=" + cont + ";database=" + data +
            ";ConnectionLifeTime=" + ctl + ";";
        //string CONN_CLTE = "";
        #endregion

        #region variables publicas
        // datos generales del emisor para fact. electrónica
        string nomclie = "";                                            // nombre comercial emisor
        string rucclie = "";                                            // ruc del emisor
        string dirclie = "";                                            // direccion fiscal del emisor
        string rasclie = "";                                            // razon social emisor
        string tasaigv = "";                                            // tasa IGV
        string ubigeoe = "";                                            // ubigeo del emisor
        string direcem = "";                                            // direccion de emision
        string distemi = "";                                            // distrito
        string provemi = "";                                            // provincia
        string depaemi = "";                                            // departamento
        string urbemis = "";                                            // urbanizacion
        // ticket impresion 
        string leyen1 = "";                                             // leyenda1
        string nuausu = "";                                             // autorizsunat
        string leyen3 = "";                                             // leyenda3
        string desped = "";                                             // despedida
        string despe2 = "";                                             // despedida 2
        string provee = "";                                             // ose o pse
        string Cfactura = "";                                           // documento factura
        string Cboleta = "";                                            // documento boleta
        string iFE = "";                                                // identificador de factura electrónica
        // funcionamiento del formulario
        string nomform = "main";                                        // nombre del formulario
        string asd = Program.vg_user;                                   // usuario logueado
        string img_log1 = "";                                           // ruta y nombre del logo del applicativo
        string img_sol1 = "";                                           // ruta y nombre del logo de solorsoft.com
        string img_sali = "";                                           // imagen para el boton de salir del sistema
        string img_pcon = "";                                           // imagen para el boton de panel de control
        string img_fact = "";                                           // imagen para el boton de facturacion
        string img_vent = "";                                           // imagen para el boton de ventas contratos
        string img_pedi = "";                                           // imagen para el boton de pedidos de fab.
        string img_alma = "";                                           // imagen para el boton de almacen
        string img_maes = "";                                           // imagen para el boton de maestras
        string imgF1 = "";                                              // imagen1 de menu facturacion
        string imgF2 = "";                                              // imagen2 de menu facturacion
        string imgF3 = "";                                              // imagen3 de menu facturacion
        string imgF4 = "";                                              // imagen4 de menu facturacion
        string imgF5 = "";                                              // imagen5 de menu facturacion
        string imgpc1 = "";                                             // imagen1 de menu panel de control
        string imgpc2 = "";                                             // imagen2 de menu panel de control
        string imgpc3 = "";                                             // imagen3 de menu panel de control
        string imgpc4 = "";                                             // imagen4 de menu panel de control
        string imgpc5 = "";                                             // imagen5 de menu panel de control
        string imgma1 = "";                                             // imagen 1 maestras - clientes
        string imgma2 = "";                                             // imagen 2 maestras - artículos
        string imgma3 = "";                                             // imagen 3 maestras - adicionales
        string imgpe1 = "";                                             // imagen 1 pedidos - registro
        string imgpe2 = "";                                             // imagen 2 pedidos - reportes
        string imgvc1 = "";                                             // imagen 1 ventas - contratos
        string imgvp1 = "";                                             // imagen 1 ventas - pedidos
        // botones de accion
        string img_btN = "";                                            // imagen del boton de accion NUEVO
        string img_btE = "";                                            // imagen del boton de accion EDITAR
        string img_btA = "";                                            // imagen del boton de accion ANULAR/BORRAR
        string img_btP = "";                                            // imagen del boton de accion IMPRIMIR
        string img_bti = "";                                            // imagen del boton de accion IR AL INICIO
        string img_bts = "";                                            // imagen del boton de accion SIGUIENTE
        string img_btr = "";                                            // imagen del boton de accion RETROCEDE
        string img_btf = "";                                            // imagen del boton de accion IR AL FINAL
        // varios
        public string nufha = "";                                       // nombre del formulario hijo activo
        #endregion

        public main()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {
            jalainfo();                                         // jalamos los parametros 
            Image logo1 = Image.FromFile(img_log1);
            Image solo1 = Image.FromFile(img_sol1);
            Image salir = Image.FromFile(img_sali);
            Image factu = Image.FromFile(img_fact);
            Image venta = Image.FromFile(img_vent);
            Image pedid = Image.FromFile(img_pedi);
            Image almac = Image.FromFile(img_alma);
            Image maest = Image.FromFile(img_maes);
            Image panel = Image.FromFile(img_pcon);
            pictureBox1.Image = logo1;
            bt_solorsoft.Image = solo1;
            bt_salir.Image = salir;
            bt_facele.Image = factu;
            bt_ventas.Image = venta;
            bt_pedidos.Image = pedid;
            bt_almacen.Image = almac;
            bt_maestras.Image = maest;
            bt_pcontrol.Image = panel;
            // botones de acciones
            Image botnew = Image.FromFile(img_btN);
            Image botedi = Image.FromFile(img_btE);
            Image botanu = Image.FromFile(img_btA);
            Image botimp = Image.FromFile(img_btP);
            //Image bot     // vista preliminar
            Image botini = Image.FromFile(img_bti);     // ir al inicio
            Image botsig = Image.FromFile(img_bts);     // siguiente
            Image botret = Image.FromFile(img_btr);     // retrocede
            Image botfin = Image.FromFile(img_btf);     // ir al final
            //bt_nuevo.Image = botnew;

            cuadre();                                           // formateamos el form principal
            pn_phor.BackColor = Color.Gray;
            pn_pver.BackColor = Color.Gray;
            bt_facele.BackColor = Color.White;
            bt_salir.BackColor = Color.White;
            bt_ventas.BackColor = Color.White;
            bt_pedidos.BackColor = Color.White;
            bt_almacen.BackColor = Color.White;
            bt_maestras.BackColor = Color.White;
            bt_pcontrol.BackColor = Color.White;
            pn_user.BackColor = Color.White;
            pn_menu.BackColor = Color.White;
            //pn_acciones.BackColor = Color.White;
            //
            tx_user.Text = Program.vg_user;                     // código de usuario
            tx_nuser.Text = Program.vg_nuse;                    // nombre de usuario
            tx_empresa.Text = Program.cliente;                 // nombre de la organización
            //
            pn_phor.Controls.Add(pn_menu);
            pn_menu.Width = pn_phor.Width;  // - pn_acciones.Width;
            menuStrip1.Visible = true;
            pn_menu.Controls.Add(menuStrip1);
            menuStrip1.Dock = DockStyle.Top;
        }

        private void jalainfo()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select * from baseconf limit 1";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.CommandTimeout = 300;
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        nomclie = dr.GetString("Cliente");                      // nombre comercial
                        rucclie = dr.GetString("Ruc");
                        dirclie = dr.GetString("direcc").Trim() + " - " + dr.GetString("distrit").Trim();
                        rasclie = dr.GetString("razonsocial");
                        tasaigv = dr.GetString("igv");
                        ubigeoe = dr.GetString("referen1");                     // ubigeo del emisor
                        direcem = dr.GetString("direcc").Trim();
                        distemi = dr.GetString("distrit").Trim();
                        provemi = dr.GetString("provin").Trim();
                        urbemis = dr.GetString("referen2").Trim();              // urbanizacion
                        depaemi = dr.GetString("depart").Trim();          // departamento
                        //
                    }
                    dr.Close();
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    MessageBox.Show("No se ubica empresa", "Error fatal de config.");
                    Application.Exit();
                    return;
                }
                //
                consulta = "select campo,param,valor from enlaces where formulario=@nofo";
                micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", nomform);
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["campo"].ToString() == "leyendas")
                    {
                        if (row["param"].ToString() == "1") leyen1 = row["valor"].ToString();             // leyenda1
                        if (row["param"].ToString() == "2") nuausu = row["valor"].ToString();             // autorizsunat
                        if (row["param"].ToString() == "3") leyen3 = row["valor"].ToString();             // leyenda3
                        if (row["param"].ToString() == "4") desped = row["valor"].ToString();             // despedida
                        if (row["param"].ToString() == "5") despe2 = row["valor"].ToString();             // despedida 2
                        if (row["param"].ToString() == "6") provee = row["valor"].ToString();             // pag. del proveedor
                    }
                    if (row["campo"].ToString() == "docvta")
                    {
                        if (row["param"].ToString() == "factura") Cfactura = row["valor"].ToString();           // documento factura
                        if (row["param"].ToString() == "boleta") Cboleta = row["valor"].ToString();             // documento boleta
                    }
                    if (row["campo"].ToString() == "identificador")
                    {
                        if (row["param"].ToString() == "identif") iFE = row["valor"].ToString().Trim();         // identif. de fact. electrónica
                    }
                    if(row["campo"].ToString() == "imagenes")
                    {
                        if (row["param"].ToString() == "logoPrin") img_log1 = row["valor"].ToString().Trim();   // logo principal
                        if (row["param"].ToString() == "logosolChi") img_sol1 = row["valor"].ToString().Trim(); // logo solorsoft chico
                        if (row["param"].ToString() == "imgsalir") img_sali = row["valor"].ToString().Trim();   // imagen boton salida
                        if (row["param"].ToString() == "imgpcont") img_pcon = row["valor"].ToString().Trim();   // imagen boton panel de control
                        if (row["param"].ToString() == "imgfactu") img_fact = row["valor"].ToString().Trim();   // imagen boton facturacion
                        if (row["param"].ToString() == "imgventa") img_vent = row["valor"].ToString().Trim();   // imagen para el boton de ventas contratos
                        if (row["param"].ToString() == "imgpedid") img_pedi = row["valor"].ToString().Trim();   // imagen para el boton de pedidos fab.
                        if (row["param"].ToString() == "imgalmac") img_alma = row["valor"].ToString().Trim();   // imagen para el boton de almacen
                        if (row["param"].ToString() == "imgmaest") img_maes = row["valor"].ToString().Trim();   // imagen para el boton de maestras
                        if (row["param"].ToString() == "imgF1") imgF1 = row["valor"].ToString().Trim();         // imagen1 del menu de facturacion opcion1
                        if (row["param"].ToString() == "imgF2") imgF2 = row["valor"].ToString().Trim();         // imagen2 del menu de facturacion opcion2
                        if (row["param"].ToString() == "imgF3") imgF3 = row["valor"].ToString().Trim();         // imagen3 del menu de facturacion opcion3
                        if (row["param"].ToString() == "imgF4") imgF4 = row["valor"].ToString().Trim();         // imagen4 del menu de facturacion opcion4
                        if (row["param"].ToString() == "imgF5") imgF5 = row["valor"].ToString().Trim();         // imagen5 del menu de facturacion opcion5
                        if (row["param"].ToString() == "imgpc1") imgpc1 = row["valor"].ToString().Trim();         // imagen1 del menu de facturacion opcion1
                        if (row["param"].ToString() == "imgpc2") imgpc2 = row["valor"].ToString().Trim();         // imagen2 del menu de facturacion opcion2
                        if (row["param"].ToString() == "imgpc3") imgpc3 = row["valor"].ToString().Trim();         // imagen3 del menu de facturacion opcion3
                        if (row["param"].ToString() == "imgpc4") imgpc4 = row["valor"].ToString().Trim();         // imagen4 del menu de facturacion opcion4
                        if (row["param"].ToString() == "imgpc5") imgpc5 = row["valor"].ToString().Trim();         // imagen5 del menu de facturacion opcion4
                        if (row["param"].ToString() == "imgma1") imgma1 = row["valor"].ToString().Trim();         // imagen1 de maestras - clientes
                        if (row["param"].ToString() == "imgma2") imgma2 = row["valor"].ToString().Trim();         // imagen2 de maestras - articulos 
                        if (row["param"].ToString() == "imgma3") imgma3 = row["valor"].ToString().Trim();         // imagen3 de maestras - adicionales 
                        if (row["param"].ToString() == "imgpe1") imgpe1 = row["valor"].ToString().Trim();         // imagen1 de pedidos - registro
                        if (row["param"].ToString() == "imgpe2") imgpe2 = row["valor"].ToString().Trim();         // imagen1 de pedidos - reportes
                        if (row["param"].ToString() == "imgvc1") imgvc1 = row["valor"].ToString().Trim();         // imagen1 de ventas contratos
                        // .. resto de imagenes de ventas
                        if (row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                        if (row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                        if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                        if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        // boton de vista preliminar .... esta por verse su utlidad
                        if (row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                        if (row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                        if (row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                        if (row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
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

        public string[] toolboton(string formu)
        {
            string[] retorno = new string[3];
            retorno[0] = "";
            retorno[1] = "";
            retorno[2] = "";

            DataTable mdtb = new DataTable();
            const string consbot = "select * from permisos where formulario=@nomform and usuario=@use";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlCommand consulb = new MySqlCommand(consbot, conn);
                    consulb.Parameters.AddWithValue("@nomform", formu);
                    consulb.Parameters.AddWithValue("@use", asd);
                    MySqlDataAdapter mab = new MySqlDataAdapter(consulb);
                    mab.Fill(mdtb);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return retorno;
                }
                finally { conn.Close(); }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return retorno;
            }
            if (mdtb.Rows.Count > 0)
            {
                DataRow row = mdtb.Rows[0];

                if (Convert.ToString(row["btn1"]) == "S")
                {
                    retorno[0] = "true";
                }
                else { retorno[0] = "false"; }
                if (Convert.ToString(row["btn2"]) == "S")
                {
                    retorno[1] = "true";
                }
                else { retorno[1] = "false"; }
                if (Convert.ToString(row["btn5"]) == "S")
                {
                    retorno[2] = "true";
                }
                else { retorno[2] = "false"; }
                /*
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")
                {
                    this.Bt_ver.Visible = true;
                }
                else { this.Bt_ver.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
                */
            }
            return retorno;
        }

        private void cuadre()
        {
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Text = Program.tituloF;
            Left = Screen.PrimaryScreen.Bounds.Left;
            Top = Screen.PrimaryScreen.Bounds.Top;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
            //
            bt_facele.Top = pictureBox1.Top + pictureBox1.Height + 2;
            bt_ventas.Top = bt_facele.Top + bt_facele.Height + 2;
            bt_pedidos.Top = bt_ventas.Top + bt_ventas.Height + 2;
            bt_almacen.Top = bt_pedidos.Top + bt_pedidos.Height + 2;
            bt_maestras.Top = bt_almacen.Top + bt_almacen.Height + 2;
            bt_pcontrol.Top = bt_maestras.Top + bt_maestras.Height + 2;
        }

        #region botones_verticales
        private void bt_salir_Click(object sender, EventArgs e)
        {
            var aa = MessageBox.Show("Realmente desea salir del sistema?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(aa == DialogResult.Yes)
            {
                Application.Exit();
                return;
            }
        }
        #endregion

        #region botones_horizontales
        private void bt_solorsoft_Click(object sender, EventArgs e)
        {
            string url = "http://solorsoft.com";
            Process.Start(url);
        }
        #endregion

        #region botones_click   // menus
        private void bt_facele_Click(object sender, EventArgs e)        // facturacion electrónica
        {
            Image img_F1 = Image.FromFile(imgF1);
            Image img_F2 = Image.FromFile(imgF2);
            Image img_F3 = Image.FromFile(imgF3);
            Image img_F4 = Image.FromFile(imgF4);
            Image img_F5 = Image.FromFile(imgF5);
            //
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Libre (Rápida)",img_F1,fac_rapida_Click);           // F1
            menuStrip1.Items.Add("de Contratos",img_F2,fac_clientes_Click);           // F2
            menuStrip1.Items.Add("F. Anticipos",img_F3,fac_anticipos_Click);          // F3
            menuStrip1.Items.Add("Anulaciones",img_F4,fac_anulac_Click);              // F4
            menuStrip1.Items.Add("Reportes",img_F5,fac_reportes_Click);               // F5
            //
            menuStrip1.Visible = true;
        }
        private void fac_rapida_Click(object sender, EventArgs e)       // factura rapida
        {
            facelec ffe1 = new iOMG.facelec();
            ffe1.TopLevel = false;
            ffe1.Parent = this;
            ffe1.Top = pn_phor.Top + pn_phor.Height + 1;
            ffe1.Left = pn_pver.Left + pn_pver.Width + 1;
            pn_centro.Controls.Add(ffe1);
            ffe1.Show();
        }
        //
        private void fac_clientes_Click(object sender, EventArgs e)     // factura de clientes con contrato
        {
            MessageBox.Show("Form de facturas de contratos de clientes");
        }
        private void fac_anticipos_Click(object sender, EventArgs e)    // factura de anticipos
        {
            MessageBox.Show("Form de facturas de anticipos");
        }
        private void fac_anulac_Click(object sender, EventArgs e)       // anulaciones de facturas
        {
            MessageBox.Show("Form de anulaciones de facturas");
        }
        private void fac_reportes_Click(object sender, EventArgs e)     // reportes de facturas
        {
            MessageBox.Show("Form de reportes de facturas");
        }
        //
        private void bt_ventas_Click(object sender, EventArgs e)
        {
            Image img_v_c = Image.FromFile(imgvc1);
            //Image img_v_p = Image.FromFile("");
            //Image img_v_i = Image.FromFile("");
            //Image img_v_s = Image.FromFile("");
            //Image img_v_r = Image.FromFile("");
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Contratos",img_v_c, vc_registro_Click);
            menuStrip1.Items.Add("Pedidos a diseño");
            menuStrip1.Items.Add("Ingresos");
            menuStrip1.Items.Add("Salidas");
            menuStrip1.Items.Add("Reportes");
            menuStrip1.Visible = true;
        }
        private void vc_registro_Click(object sender, EventArgs e)
        {
            contclte fvc = new contclte();
            fvc.TopLevel = false;
            fvc.Parent = this;
            pn_centro.Controls.Add(fvc);
            fvc.Location = new Point((pn_centro.Width - fvc.Width) / 2, (pn_centro.Height - fvc.Height) / 2);
            fvc.Anchor = AnchorStyles.None;
            fvc.Show();
            fvc.BringToFront();
        }
        //
        private void bt_pedidos_Click(object sender, EventArgs e)       // pedidos de fabricación
        {
            Image img_pe1 = Image.FromFile(imgpe1);                     // registro de pedidos
            Image img_pe2 = Image.FromFile(imgpe2);                     // reportes
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Registro", img_pe1, pe_registro_Click);            // usuarios
            menuStrip1.Items.Add("Reportes", img_pe2, pe_reportes_Click);            // definiciones
            menuStrip1.Visible = true;
        }
        private void pe_registro_Click(object sender, EventArgs e)
        {
            Pedsalm fpe = new Pedsalm();
            fpe.TopLevel = false;
            fpe.Parent = this;
            pn_centro.Controls.Add(fpe);
            fpe.Location = new Point((pn_centro.Width - fpe.Width) / 2, (pn_centro.Height - fpe.Height) / 2);
            fpe.Anchor = AnchorStyles.None;
            fpe.Show();
            fpe.BringToFront();
        }
        private void pe_reportes_Click(object sender, EventArgs e)
        {
            // reportes de pedidos de almacen
            repspedalm frp = new repspedalm();
            frp.TopLevel = false;
            frp.Parent = this;
            pn_centro.Controls.Add(frp);
            frp.Location = new Point((pn_centro.Width - frp.Width) / 2, (pn_centro.Height - frp.Height) / 2);
            frp.Anchor = AnchorStyles.None;
            frp.Show();
            frp.BringToFront();
        }
        //
        private void bt_pcontrol_Click(object sender, EventArgs e)
        {
            Image img_pc1 = Image.FromFile(imgpc1);
            Image img_pc2 = Image.FromFile(imgpc2);
            Image img_pc3 = Image.FromFile(imgpc3);
            Image img_pc4 = Image.FromFile(imgpc4);
            Image img_pc5 = Image.FromFile(imgpc5);
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Usuarios", img_pc1, pc_usuarios_Click);                    // usuarios
            menuStrip1.Items.Add("Definiciones", img_pc2, pc_definiciones_Click);            // definiciones
            menuStrip1.Items.Add("Series", img_pc3, pc_series_Click);                        // series de documentos
            menuStrip1.Items.Add("Enlaces", img_pc4, pc_enlaces_Click);                      // enlaces de datos
            menuStrip1.Items.Add("Permisos", img_pc5, pc_permisos_Click);                    // permisos
            menuStrip1.Visible = true;
        }
        //
        private void bt_maestras_Click(object sender, EventArgs e)
        {
            Image img_ma1 = Image.FromFile(imgma1);
            Image img_ma2 = Image.FromFile(imgma2);
            Image img_ma3 = Image.FromFile(imgma3);
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Clientes", img_ma1, ma_clientes_Click);                   // clientes
            menuStrip1.Items.Add("Artículos", img_ma2, ma_articulos_Click);                 // articulos 
            menuStrip1.Items.Add("Adicionales", img_ma3, ma_artadic_Click);                // adicionales para los contratos clientes
            menuStrip1.Visible = true;
        }
        private void ma_clientes_Click(object sender, EventArgs e)
        {
            clients fmc = new clients();
            fmc.TopLevel = false;
            fmc.Parent = this;
            pn_centro.Controls.Add(fmc);
            fmc.Location = new Point((pn_centro.Width - fmc.Width) / 2, (pn_centro.Height - fmc.Height) / 2);
            fmc.Anchor = AnchorStyles.None;
            fmc.Show();
            fmc.BringToFront();
        }
        private void ma_articulos_Click(object sender, EventArgs e)
        {
            //items fma = new items();
            articulos fma = new articulos();
            fma.TopLevel = false;
            fma.Parent = this;
            pn_centro.Controls.Add(fma);
            fma.Location = new Point((pn_centro.Width - fma.Width) / 2, (pn_centro.Height - fma.Height) / 2);
            fma.Anchor = AnchorStyles.None;
            fma.Show();
            fma.BringToFront();
        }
        private void ma_artadic_Click(object sender, EventArgs e)
        {
            adicionals fad = new adicionals();
            fad.TopLevel = false;
            fad.Parent = this;
            pn_centro.Controls.Add(fad);
            fad.Location = new Point((pn_centro.Width - fad.Width) / 2, (pn_centro.Height - fad.Height) / 2);
            fad.Anchor = AnchorStyles.None;
            fad.Show();
            fad.BringToFront();
        }
        //
        private void pc_usuarios_Click(object sender, EventArgs e)
        {
            users fuser = new iOMG.users();
            fuser.TopLevel = false;
            fuser.Parent = this;
            pn_centro.Controls.Add(fuser);
            fuser.Location = new Point((pn_centro.Width - fuser.Width) / 2, (pn_centro.Height - fuser.Height) / 2);
            fuser.Anchor = AnchorStyles.None;
            fuser.Show();
            fuser.BringToFront();
        }
        private void pc_definiciones_Click(object sender, EventArgs e)
        {
            defs fdefs = new defs();
            fdefs.TopLevel = false;
            fdefs.Parent = this;
            pn_centro.Controls.Add(fdefs);
            fdefs.Location = new Point((pn_centro.Width - fdefs.Width) / 2, (pn_centro.Height - fdefs.Height) / 2);
            fdefs.Anchor = AnchorStyles.None;
            fdefs.Show();
            fdefs.BringToFront();
        }
        private void pc_series_Click(object sender, EventArgs e)
        {
            sernum fsn = new sernum();
            fsn.TopLevel = false;
            fsn.Parent = this;
            pn_centro.Controls.Add(fsn);
            fsn.Location = new Point((pn_centro.Width - fsn.Width) / 2, (pn_centro.Height - fsn.Height) / 2);
            fsn.Anchor = AnchorStyles.None;
            fsn.Show();
            fsn.BringToFront();
        }
        private void pc_enlaces_Click(object sender, EventArgs e)
        {
            enlaces fenl = new enlaces();
            fenl.TopLevel = false;
            fenl.Parent = this;
            pn_centro.Controls.Add(fenl);
            fenl.Location = new Point((pn_centro.Width - fenl.Width) / 2, (pn_centro.Height - fenl.Height) / 2);
            fenl.Anchor = AnchorStyles.None;
            fenl.Show();
            fenl.BringToFront();
        }
        private void pc_permisos_Click(object sender, EventArgs e)
        {
            permisos fper = new permisos();
            fper.TopLevel = false;
            fper.Parent = this;
            pn_centro.Controls.Add(fper);
            fper.Location = new Point((pn_centro.Width - fper.Width) / 2, (pn_centro.Height - fper.Height) / 2);
            fper.Anchor = AnchorStyles.None;
            fper.Show();
            fper.BringToFront();
        }
        //
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string url = "http://www.artesanosdonbosco.pe";
            Process.Start(url);
        }
        #endregion

        private void main_Activated(object sender, EventArgs e)
        {
            //bt_nuevo.Enabled = false;
        }
    }
}
