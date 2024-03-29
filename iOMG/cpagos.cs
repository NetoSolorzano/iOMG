﻿using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using CrystalDecisions.Shared;
using PaperSize = CrystalDecisions.Shared.PaperSize;

namespace iOMG
{
    public partial class cpagos : Form
    {
        static string nomform = "cpagos";      // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        bool conSol = iOMG.Program.vg_conSol;   // usa conector solorsoft ?
        static string nomtab = "pagamenti";

        #region variables 
        public int totfilgrid, cta, cuenta, pageCount;      // variables para impresion sin crystal, con crystal ya no se usan
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
        string img_pre = "";            // imagen del boton vista preliminar
        string img_ver = "";            // imagen del boton visualizacion (solo ver)
        string tipede = "";             // tipo de pedido por defecto
        string tiesta = "";             // estado inicial por defecto del contrato
        string tiesan = "";             // estado anulado / codigo
        string escambio = "";           // estados del contrato que admiten modificacion
        string cnojal = "";             // estados de contratos que no se jalan a la grilla
        string canovald2 = "";          // captitulos donde no se valida det2
        string conovald2 = "";          // valor por defecto al no validar det2
        string tdc = "";                // tipo de documento para contratos
        string sdc = "";                // local de contratos (vacio = todos los locales)
        string raz = "";                // razon social del contrato (vacio si es un solo contador para todos)
        string letpied = "";            // letra identificadora de Piedra en detalle 2 = R
        int vfdmax = 0;                 // limite de filas de detalle maximo por contrato
        string tncont = "";             // tipo de numeracion: AUTOMATICA o MANUAL
        string letgru = "";                 // letra identificado en campo "CAPIT" para ADICIONAL
        string talldef = "";                // taller por defecto en los contratos
        string madedef = "";                // maderas para adicionales
        string dets1 = "";                  // detalles1 para adicionales
        string dets2 = "";                  // detalles2 para adicionales
        string dets3 = "";                  // detalles3 para adicionales
        string acadef = "";                 // acabados para adicionales
        string vpaisdef = "";               // pais por defecto para los clientes y proveedores
        string docDni = "";             // codigo documento dni
        string docRuc = "";             // codigo documento RUC
        string cliente = Program.cliente;    // razon social para los reportes
        string impDef = "";                 // nombre de la impresora por defecto
        #endregion

        libreria lib = new libreria();
        acciones acc = new acciones();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";default command timeout=120";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + " " + ";default command timeout=120" +
        ";ConnectionLifeTime=" + ctl + ";";

        DataTable dtg = new DataTable();
        DataTable dtadpd = new DataTable();     // tabla para el autocompletado de dpto, provin y distrito
        DataTable dttaller = new DataTable();   // combo taller de fabric.
        DataTable dtdest = new DataTable();     // tipos de documentos de clientes
        AutoCompleteStringCollection adptos = new AutoCompleteStringCollection();
        AutoCompleteStringCollection aprovi = new AutoCompleteStringCollection();
        AutoCompleteStringCollection adistr = new AutoCompleteStringCollection();

        public cpagos()
        {
            InitializeComponent();
        }
        private void users_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1 && tx_ndc.Focused == true) //  && Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"
            {
                para1 = "anag_cli";   // maestra clientes
                para2 = "todos";   // 
                ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                var result = ayu2.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                    {
                        //tx_nombre.Text = ayu2.ReturnValueA[3];
                        tx_dat_tdoc.Text = ayu2.ReturnValueA[1];

                        string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                        DataRow[] row = dtdest.Select(axs);
                        cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();

                        tx_ndc.Text = ayu2.ReturnValueA[2];
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void cpagos_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            this.KeyPreview = true;
            tabControl1.Enabled = false;
        }

        #region resto del mundo
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);
            this.tabuser.BackColor = Color.FromName(iOMG.Program.colgri);

            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_anul);
            bt_view.Image = Image.FromFile(img_ver);
            Bt_print.Image = Image.FromFile(img_btP);
            bt_prev.Image = Image.FromFile(img_pre);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            //
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // longitudes maximas de campos
            
        }
        private void jalainfo()                                                 // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped,@adi,@cli)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "cpagos");
                micon.Parameters.AddWithValue("@adi", "adicionals");
                micon.Parameters.AddWithValue("@cli", "clients");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "main")
                    {
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                        if (row["campo"].ToString() == "pais" && row["param"].ToString() == "default") vpaisdef = row["valor"].ToString().Trim();             // pais por defecto para los clientes
                    }
                    if (row["formulario"].ToString() == "cpagos")
                    {
                        if (row["campo"].ToString() == "tipocon" && row["param"].ToString() == "normal") tipede = row["valor"].ToString().Trim();               // tipo de contrato x defecto "normal"
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();               // estado del contrato inicial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cambio") escambio = row["valor"].ToString().Trim();              // estado del contrato que admiten modificar el pedido
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "nodet2") canovald2 = row["valor"].ToString().Trim();            // captitulos donde no se valida det2
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "valdet2") conovald2 = row["valor"].ToString().Trim();           // valor por defecto al no validar det2
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "tipdoc") tdc = row["valor"].ToString().Trim();                 // tipo de documento para cpagos
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "local") sdc = row["valor"].ToString().Trim();                  // local del contrato, vacio todos los locales
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "rsocial") raz = row["valor"].ToString().Trim();                // tipo de documento para cpagos
                        if (row["campo"].ToString() == "detalle2" && row["param"].ToString() == "piedra") letpied = row["valor"].ToString().Trim();             // letra identificadora de Piedra en Detalle2
                        if (row["campo"].ToString() == "grilladet" && row["param"].ToString() == "limite") vfdmax = int.Parse(row["valor"].ToString().Trim());  // cantidad de filas de detalle maximo del cont estandar
                        if (row["campo"].ToString() == "numeracion" && row["param"].ToString() == "modo") tncont = row["valor"].ToString().Trim();              // tipo de numeracion de los cpagos: MANUAL o AUTOMA 
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "codAnu") tiesan = row["valor"].ToString().Trim();                // codigo de estado anulado
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "nogrilla") cnojal = row["valor"].ToString().Trim();              // estados de cpagos que no se jalan a la grilla
                        if (row["campo"].ToString() == "impresora" && row["param"].ToString() == "default") impDef = row["valor"].ToString().Trim();            // nombre de la impresora por defecto
                    }
                    if (row["formulario"].ToString() == "adicionals")
                    {
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "capitulo") letgru = row["valor"].ToString().Trim();    // capitulo
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "talleres") talldef = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "maderas") madedef = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle1") dets1 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle2") dets2 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle3") dets3 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "acabados") acadef = row["valor"].ToString().Trim();    // 
                    }
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "dni") docDni = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ruc") docRuc = row["valor"].ToString().Trim();
                    }
                }
                da.Dispose();
                dt.Dispose();
                // autocompletados de departamento, provincia y distrito
                consulta = "SELECT depart,provin,distri,nombre FROM ubigeos";
                micon = new MySqlCommand(consulta, conn);
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
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();

                return;
            }
        }
        private void dataload(string quien)                                     // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            tabControl1.SelectedTab = tabgrilla;
            if (quien == "maestra")
            {
                // datos de los contratos date_format(date(a.fecha),'%Y-%m-%d')
                string datgri = "cont_pagos";
                using (MySqlCommand cdg = new MySqlCommand(datgri, conn))
                {
                    cdg.CommandType = CommandType.StoredProcedure;
                    MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                    dtg.Clear();
                    dag.Fill(dtg);
                    dag.Dispose();
                }
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion del tipo documento cliente
                cmb_tdoc.Items.Clear();
                const string condest = "select descrizionerid,idcodice,codigo from desc_doc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmddest = new MySqlCommand(condest, conn);
                MySqlDataAdapter dadest = new MySqlDataAdapter(cmddest);
                dadest.Fill(dtdest);
                foreach (DataRow row in dtdest.Rows)
                {
                    cmb_tdoc.Items.Add(row.ItemArray[0].ToString());
                    cmb_tdoc.ValueMember = row.ItemArray[1].ToString();
                }
            }
            conn.Close();
        }
        private void grilla()                                                   // arma la grilla
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            //
            if (advancedDataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < advancedDataGridView1.Columns.Count; i++)
                {
                    advancedDataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    _ = decimal.TryParse(advancedDataGridView1.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                    if (vd != 0) advancedDataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                int b = 0;
                for (int i = 3; i < advancedDataGridView1.Columns.Count; i++)
                {
                    int a = advancedDataGridView1.Columns[i].Width;
                    b += a;
                    advancedDataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;    // DataGridViewAutoSizeColumnMode.None;
                    advancedDataGridView1.Columns[i].Width = a;
                    if (i + 1 == advancedDataGridView1.Columns.Count) advancedDataGridView1.Columns[i].Visible = false; // ultima columna invisible
                }
                //if (b < advancedDataGridView1.Width) advancedDataGridView1.Width = b + 60;   // b - 20;
            }
            //
            advancedDataGridView1.ReadOnly = true;
        }
        private void jalaoc(string campo)                                       // jala datos del contrato
        {
            if (campo == "codigo")
            {
                string datclt = "SELECT idanagrafica,razonsocial,direcc1,direcc2,localidad,provincia,depart,numerotel1,numerotel2,email " +
                    "FROM anag_cli WHERE tipdoc=@doc and ruc=@num";
                string datgri = "cont_pagos_clte";
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    using (MySqlCommand cdg = new MySqlCommand(datclt, conn))
                    {
                        cdg.Parameters.AddWithValue("@doc", tx_idr.Text);
                        cdg.Parameters.AddWithValue("@num", tx_rind.Text);
                        MySqlDataReader dr = cdg.ExecuteReader();
                        if (dr.Read())
                        {
                            if (dr.GetInt16(0) > 0)
                            {
                                tx_ndc.Text = tx_rind.Text;
                                tx_nombre.Text = dr.GetString(1);
                                tx_nombre.ReadOnly = true;
                                tx_direc.Text = dr.GetString(2);
                                tx_direc.ReadOnly = true;
                                tx_dpto.Text = dr.GetString(6);
                                tx_dpto.ReadOnly = true;
                                tx_prov.Text = dr.GetString(5);
                                tx_prov.ReadOnly = true;
                                tx_dist.Text = dr.GetString(4);
                                tx_dist.ReadOnly = true;
                                tx_mail.Text = dr.GetString(9);
                                tx_mail.ReadOnly = true;
                                tx_telef1.Text = dr.GetString(7);
                                tx_telef1.ReadOnly = true;
                                tx_telef2.Text = dr.GetString(8);
                                tx_telef2.ReadOnly = true;
                                tx_dat_tdoc.Text = tx_idr.Text;
                                //
                                string axs = string.Format("idcodice='{0}'", tx_dat_tdoc.Text);
                                DataRow[] row = dtdest.Select(axs);
                                cmb_tdoc.SelectedItem = row[0].ItemArray[0].ToString();
                            }
                        }
                        dr.Dispose();
                    }
                    using (MySqlCommand cdg = new MySqlCommand(datgri, conn))
                    {
                        cdg.Parameters.AddWithValue("@v_doc", tx_idr.Text);
                        cdg.Parameters.AddWithValue("@v_num", tx_rind.Text.Trim());
                        cdg.CommandType = CommandType.StoredProcedure;
                        MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                        DataTable dtz = new DataTable();
                        dag.Fill(dtz);
                        dag.Dispose();
                        dataGridView1.DataSource = dtz;
                    }
                    grilladet();
                }
            }
        }
        private void grilladet()                                                // jala datos del cliente
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            //dataGridView1.DataSource = dtg;
            //
            if (dataGridView1.Rows.Count > 1)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    _ = decimal.TryParse(dataGridView1.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                    if (vd != 0) dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                int b = 0;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    int a = dataGridView1.Columns[i].Width;
                    b += a;
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridView1.Columns[i].Width = a;
                    if (i + 1 == dataGridView1.Columns.Count) dataGridView1.Columns[i].Visible = true; // Visible = false  ultima columna invisible
                }
                if (b < dataGridView1.Width) dataGridView1.Width = b - 20;  // b + 60;
            }
            //
            dataGridView1.ReadOnly = true;
            suma_grilla(dataGridView1.Name);
        }
        private void jaladet(int idc)                                           // jala datos del grid principal
        {
            //ID,FPAGO,DOC_VENTA,CONT,DOC,NUM_DOC,CLIENTE,ESTADO,VAL_CONT,SALDO,MON,VAL_SOLES,MED_PAGO,N_OPER,TDC
            string tdoc, ndoc = "";
            tdoc = advancedDataGridView1.Rows[idc].Cells[4].Value.ToString();
            ndoc = advancedDataGridView1.Rows[idc].Cells[2].Value.ToString();
            //tabControl1.SelectedTab = tabuser;
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            limpia_chk();
            //escribepag(tabuser);
            //sololeepag(tabuser);
            tx_idr.Text = tdoc;
            tx_rind.Text = ndoc;
            jalaoc("codigo");
        }
        private bool graba()                                                    // graba cabecera y detalle
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string inserta = " ";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.ExecuteNonQuery();
                    string lid = "select last_insert_id()";
                    micon = new MySqlCommand(lid, conn);
                    MySqlDataReader rlid = micon.ExecuteReader();
                    if (rlid.Read())
                    {
                        tx_idr.Text = rlid.GetString(0);
                    }
                    rlid.Close();
                    // detalle 
                    retorna = true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en insertar contrato o detalle");
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
        private bool edita()                                                    // actualiza cabecera y detalle
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                    // a.valor,a.acuenta,a.saldo,a.dscto
                    string actua = "update ....";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
                    // detalle
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string insdet = "";
                        if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "N")   // nueva fila de detalle o actualizacion
                        {
                            insdet = "insert into detacon ...";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@prec", dataGridView1.Rows[i].Cells[6].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@tota", dataGridView1.Rows[i].Cells[7].Value.ToString());
                            //micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[].Value.ToString()); // acabado debe ser
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            //micon.Parameters.AddWithValue("@cref", dataGridView1.Rows[i].Cells[10].Value.ToString());
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[11].Value.ToString());
                            micon.Parameters.AddWithValue("@pied", dataGridView1.Rows[i].Cells[13].Value.ToString());
                            micon.Parameters.AddWithValue("@tdai", dataGridView1.Rows[i].Cells[15].Value.ToString());   // tienda item
                            micon.ExecuteNonQuery();
                        }
                        if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "A")
                        {   // iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,space(1) as na
                            insdet = "update detacon set tipo=@tipe,item=@item,cant=@cant," +
                                "nombre=@nomb,medidas=@medi,madera=@made,precio=@prec,total=@tota,saldo=@sald,coment=@come,piedra=@pied," +
                                "tda_item=@tdai " +
                                "where iddetacon=@idr";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[0].Value.ToString());
                            //micon.Parameters.AddWithValue("@tipe", tx_dat_orig.Text);   // tx_dat_tiped.Text
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@prec", dataGridView1.Rows[i].Cells[6].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@tota", dataGridView1.Rows[i].Cells[7].Value.ToString());
                            //micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[].Value.ToString()); // acabado debe ser
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            //micon.Parameters.AddWithValue("@cref", dataGridView1.Rows[i].Cells[10].Value.ToString());
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[11].Value.ToString());
                            micon.Parameters.AddWithValue("@pied", dataGridView1.Rows[i].Cells[13].Value.ToString());
                            micon.Parameters.AddWithValue("@tdai", dataGridView1.Rows[i].Cells[15].Value.ToString());   // tienda item
                            micon.ExecuteNonQuery();
                        }
                    }
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
        private void tabuser_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = true;
            bt_prev.Enabled = true;
            bt_exc.Enabled = false;
            if (Tx_modo.Text != "NUEVO" && Tx_modo.Text != "EDITAR")
            {

            }
        }
        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            bt_exc.Enabled = true;
        }
        private void suma_grilla(string dgv)
        {
            int cr = 0, ca = 0;
            double tvv = 0, tva = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["ESTADO"].Value != null && dataGridView1.Rows[i].Cells["ESTADO"].Value.ToString() == "ANULAD")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    ca = ca + 1;
                    tva = tva + Convert.ToDouble(dataGridView1.Rows[i].Cells["???"].Value);
                }
                else
                {
                    cr = cr + 1;
                    if (dataGridView1.Rows[i].Cells["SALDO"].Value != null && 
                        dataGridView1.Rows[i].Cells["SALDO"].Value.ToString().Trim() != "") tvv = tvv + Convert.ToDouble(dataGridView1.Rows[i].Cells["SALDO"].Value.ToString());
                }
            }
            tx_totSaldo.Text = tvv.ToString("#0.00");
            //tx_tfi_f.Text = cr.ToString();
            
        }
        #endregion

        #region botones_de_comando_y_permisos  
        private void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            bt_view.Visible = false;
            Bt_print.Visible = false;
            bt_exc.Visible = false;
            bt_prev.Visible = false;
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
                if (Convert.ToString(row["btn1"]) == "S")               // nuevo
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")               // editar
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")               // ANULAR
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")               // VISUALIZAR
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir
                {
                    this.Bt_print.Visible = true;
                }
                else { Bt_print.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")               // salir del form
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
                if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar
                {
                    bt_prev.Visible = true;
                }
                else { this.bt_prev.Visible = false; }
                if (Convert.ToString(row["btn8"]) == "S")               // exporta xlsx
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
            }
        }
        private void Bt_add_Click(object sender, EventArgs e)
        {
            //
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            //
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            //
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            sololee(this);
            Tx_modo.Text = "VISUALIZAR";
            //button1.Image = null;    // Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabuser);
            sololeepag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            limpia_panel(pan_cli);
            cmb_tdoc.SelectedIndex = -1;
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            //setParaCrystal();
            if (impDef == "")
            {
                PrinterSettings setPrintD = new PrinterSettings();
                impDef = setPrintD.PrinterName;
            }
            //PrintReport(Application.StartupPath + "\\ContratoI.rpt", impDef, 1);  // "CutePDFWriter" 
            //PrintReport(Application.StartupPath + "\\resumen_termYcond.rpt", impDef, 2);  // \\terminosYcondiciones.rpt  "CutePDFWriter"
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text != "" || tx_rind.Text != "")    // &&
            {
                //setParaCrystal();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Cont_Pagos_clientes_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                wb.Worksheets.Add(dtg, "cpagos");
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
        #endregion botones_de_comando_y_permisos  ;

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
                if (oControls is CheckBox)
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
                if (oControls is CheckBox)
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
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
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
            //
            tx_idr.ReadOnly = true;
            tx_rind.ReadOnly = true;
        }
        private void escribepag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
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
                if (oControls is CheckBox)
                {
                    oControls.Enabled = true;
                }
            }
            tx_idr.ReadOnly = true;
            tx_rind.ReadOnly = true;
        }
        private static void limpiar(Form ofrm)
        {
            foreach (Control oControls in ofrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        private void limpiapag(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        private void limpia_chk()
        {
            
        }
        private void limpia_otros(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            //this.checkBox1.Checked = false;
        }
        private void limpia_combos(TabPage pag)
        {
            //tabControl1.SelectedTab = pag;
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
        private void cmb_tdoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tdoc.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_tdoc.Text);
                DataRow[] row = dtdest.Select(axs);
                tx_dat_tdoc.Text = row[0].ItemArray[1].ToString();
            }
        }
        #endregion comboboxes

        #region boton_form GRABA EDITA ANULA - agrega detalle
        private void button1_Click(object sender, EventArgs e)
        {

        }
        #endregion boton_form;

        #region leaves and enter
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")    //  && tx_idr.Text != ""
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_ndc_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")
            {
                tx_idr.Text = tx_dat_tdoc.Text;
                tx_rind.Text = tx_ndc.Text;
                jalaoc("codigo");               // jalamos los datos del registro
            }
        }
        #endregion leaves;

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString; // original
        }
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            dtg.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            //jaladet(e.RowIndex);
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")
            {
                jaladet(e.RowIndex);
            }
        }
        #endregion

        #region datagridview
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // nada
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Tx_modo.Text != "")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "CONT")
                    {
                        contratos ncont = new contratos();
                        ncont.Show(this);
                        ncont.bt_view.PerformClick();
                        ncont.tx_codped.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        ncont.tx_codped_Leave(null, null);
                    }
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "DOC_VENTA")
                    {
                        String[] partes = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split('-');
                        docsvta ndv = new docsvta();
                        ndv.Show(this);
                        ndv.bt_view.PerformClick();
                        ndv.tx_dat_tipdoc.Text = partes[0];
                        ndv.cmb_tipo.SelectedItem = partes[0];
                        ndv.tx_serie.Text = partes[1];
                        ndv.tx_corre.Text = partes[2];
                        ndv.tx_corre_Leave(null, null);
                    }
                }
            }
        }

        #endregion
    }
}
