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
    public partial class pedsclients : Form
    {
        static string nomform = "pedsclients";      // nombre del formulario
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
        DataTable dttaller = new DataTable();   // combo taller de fabric.

        public pedsclients()
        {
            InitializeComponent();
        }
        private void pedsclients_KeyDown(object sender, KeyEventArgs e)
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
                if (tx_cliente.Focused == true)
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
                            tx_cliente.Text = ayu2.ReturnValue2;
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
                            tx_cliente.Text = ayu2.ReturnValue2;
                            //tx_ciudades.Text = ayu2.ReturnValueA[5];
                            cmb_destino.SelectedIndex = cmb_destino.FindString(ayu2.ReturnValueA[6]);
                            tx_dat_dest.Text = ayu2.ReturnValueA[6];
                        }
                    }
                }
                if (tx_d_can.Focused == true || tx_d_codi.Focused == true)
                {
                    if(tx_dat_orig.Text.Trim() == "")
                    {
                        MessageBox.Show("Debe seleccionar el taller!", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cmb_taller.Focus();
                        return false;
                    }
                    para1 = "detacon";
                    para2 = tx_cont.Text;
                    para3 = "";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue1;    // codigo del articulo
                            //ayu2.ReturnValue0;    // id del articulo
                            //ayu3.ReturnValue2;    // nombre del articulo
                            tx_d_can.Text = ayu2.ReturnValueA[7].ToString();
                            if(ayu2.ReturnValue1.Trim().Length != 18)          // si el codigo no tiene taller
                            {
                                tx_d_codi.Text = ayu2.ReturnValue1.Substring(0, 10) + tx_codta.Text.Trim() + ayu2.ReturnValue1.Substring(10, 6);
                            }
                            else
                            {                                               // el codigo tiene taller
                                tx_d_codi.Text = ayu2.ReturnValue1.Substring(0, 10) + tx_codta.Text.Trim() + ayu2.ReturnValue1.Substring(12, 6);
                            }
                            tx_d_iddc.Text = ayu2.ReturnValue0;                 // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment,total,acabado
                            tx_d_nom.Text = ayu2.ReturnValueA[3].ToString();
                            tx_d_med.Text = ayu2.ReturnValueA[4].ToString();
                            tx_d_mad.Text = ayu2.ReturnValueA[5].ToString();
                            tx_d_est.Text = ayu2.ReturnValueA[6].ToString();         // codigo de acabado
                            tx_acab.Text = ayu2.ReturnValueA[10].ToString();          // nombre del acabado
                            tx_d_com.Text = ayu2.ReturnValueA[8].ToString();
                            tx_d_precio.Text = ayu2.ReturnValueA[9].ToString();
                            tx_acab.Text = ayu2.ReturnValueA[10].ToString();
                            tx_saldo.Text = ayu2.ReturnValueA[7].ToString();
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void pedsclients_Load(object sender, EventArgs e)
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
            tabControl1.Enabled = false;
            tx_d_nom.Enabled = false;
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);
            this.tabuser.BackColor = Color.FromName(iOMG.Program.colgri);

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
            tx_status.Visible = false;                  // solo sera visible si tiene estado
            // longitudes maximas de campos
            tx_coment.MaxLength = 240;
            tx_d_com.MaxLength = 100;
            tx_codped.CharacterCasing = CharacterCasing.Upper;
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
            if (campo == "tx_idr" && tx_idr.Text != "" && tx_codped.Text.Trim() == "")
            {   // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,nom_estado,a.origen,a.destino,
                // fecha,entrega,a.coment,a.tipoes,a.status,coddes,destino
                if (Tx_modo.Text != "NUEVO")
                {
                    if(ingped(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString().Trim().ToUpper()) == true)    // pedido ya ingreso
                    {                             // hay que poner todo en readonly menos comentarios y adjuntos
                        MessageBox.Show("El pedido ya ingresó al taller", "No puede editar");
                        sololeepag(tabuser);
                        tx_coment.ReadOnly = false;
                        bt_adj1.Enabled = true;
                        bt_adj2.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("El pedido aun no ingresa al taller", "Si puede editar");
                        escribepag(tabuser);
                    }
                }
                tx_dat_tiped.Text = tipede;
                tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // codigo pedido
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString();   // taller origen
                dtp_pedido.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[8].Value.ToString());   // fecha pedido
                dtp_entreg.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString());    // fecha entrega
                tx_coment.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[10].Value.ToString();    // comentario
                tx_idc.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();        // id cliente
                tx_cliente.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();    // nombre cliente
                tx_cont.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();       // contrato
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();   // cod destino
                //tx_ciudades.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString();   // destino
                cmb_destino.SelectedIndex = cmb_destino.FindString(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString());
                tx_status.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString();     // estado
                tx_adjun1.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[15].Value.ToString();     // adjunto 1
                tx_adjun2.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[16].Value.ToString();     // adjunto 2
                jaladet(tx_codped.Text);
            }
            if (campo == "tx_codped" && tx_codped.Text != "" && tx_idr.Text.Trim() == "")
            {
                if (Tx_modo.Text != "NUEVO")
                {
                    if (ingped(tx_codped.Text.Trim().ToUpper()) == true)    // pedido ya ingreso
                    {                             // hay que poner todo en readonly menos comentarios y adjuntos
                        MessageBox.Show("El pedido ya ingresó al taller", "No puede editar");
                        sololeepag(tabuser);
                        tx_coment.ReadOnly = false;
                        bt_adj1.Enabled = true;
                        bt_adj2.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("El pedido aun no ingresa al taller", "Si puede editar");
                        escribepag(tabuser);
                    }
                }
                int cta = 0;
                foreach (DataRow row in dtg.Rows)
                {
                    if (row["codped"].ToString().Trim() == tx_codped.Text.Trim())
                    {// a.id,a.codped,a.contrato,a.cliente,c.razonsocial,nom_estado,a.origen,a.destino,
                     // fecha,entrega,a.coment,a.tipoes,a.status
                        tx_dat_tiped.Text = tipede;
                        tx_idr.Text = row["id"].ToString();                                 // id del registro
                        tx_rind.Text = cta.ToString();
                        tx_dat_orig.Text = row["origen"].ToString();                        // taller origen
                        dtp_pedido.Value = Convert.ToDateTime(row["fecha"].ToString());     // fecha pedido
                        dtp_entreg.Value = Convert.ToDateTime(row["entrega"].ToString());   // fecha entrega
                        tx_coment.Text = row["coment"].ToString();                          // comentario
                        tx_idc.Text = row["cliente"].ToString();                            // id cliente
                        tx_cliente.Text = row["razonsocial"].ToString();                    // nombre cliente
                        tx_cont.Text = row["contrato"].ToString();
                        cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                        cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                        tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[13].Value.ToString();     // cod destino
                        //tx_ciudades.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString();     // destino
                        cmb_destino.SelectedIndex = cmb_destino.FindString(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString());
                        tx_adjun1.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[15].Value.ToString();     // adjunto 1
                        tx_adjun2.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[16].Value.ToString();     // adjunto 2
                        jaladet(tx_codped.Text);
                    }
                    cta = cta + 1;
                }
            }
        }
        private void jaladet(string pedido)                 // jala el detalle del pedido
        {
            // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo,total,ne,iddetc
            string jalad = "select a.iddetaped,a.cant,a.item,a.nombre,a.medidas,a.madera,d.descrizionerid," +
                "b.descrizionerid,a.coment,a.estado,a.madera,a.piedra,DATE_FORMAT(fingreso,'%d/%m/%Y'),a.saldo,a.total,space(1) as ne,a.iddetc " +
                "from detaped a " +
                "left join desc_est b on b.idcodice=a.estado " +
                "left join desc_mad c on c.idcodice=a.madera " +
                "left join desc_dt2 d on d.idcodice=a.piedra " +
                "where a.pedidoh=@pedi";    // c.descrizionerid
            //try
            {
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
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error en obtener detalle del pedido");
            //    Application.Exit();
            //    return;
            //}
        }
        private void grilla()                               // arma la grilla
        {
            // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,nom_estado,a.origen,a.destino,
            // fecha,entrega,a.coment,a.tipoes,a.status,coddes,destino,a.nomimg1,a.nomimg2
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
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
            // contrato
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Contrato";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 70;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaNO";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // id cliente
            advancedDataGridView1.Columns[3].Visible = true;
            advancedDataGridView1.Columns[3].HeaderText = "ID Clte";
            advancedDataGridView1.Columns[3].Width = 50;
            advancedDataGridView1.Columns[3].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[3].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre cliente
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Nombre Cliente";
            advancedDataGridView1.Columns[4].Width = 200;
            advancedDataGridView1.Columns[4].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // estado (nombre)
            advancedDataGridView1.Columns[5].Visible = true;
            advancedDataGridView1.Columns[5].HeaderText = "Estado";
            advancedDataGridView1.Columns[5].Width = 200;
            advancedDataGridView1.Columns[5].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[5].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Origen - taller
            advancedDataGridView1.Columns[6].Visible = true;
            advancedDataGridView1.Columns[6].HeaderText = "Taller";
            advancedDataGridView1.Columns[6].Width = 80;
            advancedDataGridView1.Columns[6].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[6].Tag = "validaSI";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Destino
            advancedDataGridView1.Columns[7].Visible = true;
            advancedDataGridView1.Columns[7].HeaderText = "Destino";
            advancedDataGridView1.Columns[7].Width = 80;
            advancedDataGridView1.Columns[7].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[7].Tag = "validaSI";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha del pedido
            advancedDataGridView1.Columns[8].Visible = true;
            advancedDataGridView1.Columns[8].HeaderText = "Fecha Ped.";
            advancedDataGridView1.Columns[8].Width = 100;
            advancedDataGridView1.Columns[8].ReadOnly = true;
            advancedDataGridView1.Columns[8].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha de Entrega
            advancedDataGridView1.Columns[9].Visible = true;
            advancedDataGridView1.Columns[9].HeaderText = "Fecha Ent.";
            advancedDataGridView1.Columns[9].Width = 100;
            advancedDataGridView1.Columns[9].ReadOnly = true;
            advancedDataGridView1.Columns[9].Tag = "validaNO";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // comentarios
            advancedDataGridView1.Columns[10].Visible = true;
            advancedDataGridView1.Columns[10].HeaderText = "Comentarios";
            advancedDataGridView1.Columns[10].Width = 250;
            advancedDataGridView1.Columns[10].ReadOnly = false;
            advancedDataGridView1.Columns[10].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // codigo tipo
            advancedDataGridView1.Columns[11].Visible = false;
            // codigo estado
            advancedDataGridView1.Columns[12].Visible = false;
            // codigo destino
            advancedDataGridView1.Columns[13].Visible = false;
            // nombre destino
            advancedDataGridView1.Columns[14].Visible = false;
            // adjunto 1
            advancedDataGridView1.Columns[15].Visible = false;
            advancedDataGridView1.Columns[15].HeaderText = "Ajunto1";
            advancedDataGridView1.Columns[15].Width = 100;
            advancedDataGridView1.Columns[15].ReadOnly = true;
            advancedDataGridView1.Columns[15].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[15].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // adjunto 2
            advancedDataGridView1.Columns[16].Visible = false;
            advancedDataGridView1.Columns[16].HeaderText = "Ajunto2";
            advancedDataGridView1.Columns[16].Width = 100;
            advancedDataGridView1.Columns[16].ReadOnly = true;
            advancedDataGridView1.Columns[16].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        private void grilladet(string modo)                 // grilla detalle de pedido
        {   // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo,total,ne,iddetc
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 17;
            // id 
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho
            dataGridView1.Columns[0].HeaderText = "Id";         // titulo de la columna
            dataGridView1.Columns[0].Name = "iddetaped";
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
            // codigo de estado
            dataGridView1.Columns[9].Visible = false;            // columna visible o no
            dataGridView1.Columns[9].Name = "estado";
            // codigo madera
            dataGridView1.Columns[10].Visible = false;           // 
            // codigo detalle 2 (piedra)
            dataGridView1.Columns[11].Visible = true;
            dataGridView1.Columns[11].ReadOnly = true;           // lectura o no
            // fecha de ingreso del articulo
            dataGridView1.Columns[12].Visible = true;            // columna visible o no
            dataGridView1.Columns[12].HeaderText = "F.Ingreso"; // titulo de la columna
            dataGridView1.Columns[12].Width = 80;                // ancho
            dataGridView1.Columns[12].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[12].Name = "fingreso";
            // saldo
            dataGridView1.Columns[13].Visible = true;            // columna visible o no
            dataGridView1.Columns[13].HeaderText = "Saldo"; // titulo de la columna
            dataGridView1.Columns[13].Width = 60;                // ancho
            dataGridView1.Columns[13].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[13].Name = "saldo";
            // total
            dataGridView1.Columns[14].Visible = false;
            dataGridView1.Columns[14].Name = "total";
            // tipo nuevo o modif
            dataGridView1.Columns[15].Visible = false;
            dataGridView1.Columns[15].Name = "NE";
            // id item detalle de contrato
            dataGridView1.Columns[16].Visible = false;
            dataGridView1.Columns[16].Name = "iddetc";
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
            tabControl1.SelectedTab = tabgrilla;
            if (quien == "maestra")
            {
                // datos de los pedidos
                string datgri = "select a.id,a.codped,a.contrato,a.cliente,c.razonsocial,e.descrizionerid as nomest,a.origen,a.destino," +
                    "date_format(date(a.fecha),'%Y-%m-%d') as fecha,date_format(date(a.entrega),'%Y-%m-%d') as entrega,a.coment," +
                    "a.tipoes,a.status,ifnull(b.tipoes,'') as coddes,ifnull(d.descrizionerid,'') as destino,a.nomimg1,a.nomimg2 " +
                    "from pedidos a " +
                    "left join anag_cli c on c.idanagrafica=a.cliente " +
                    "left join contrat b on b.contrato=a.contrato " +
                    "left join desc_alm d on d.idcodice=b.tipoes " +
                    "left join desc_sta e on e.idcodice=a.status " +
                    "where a.tipoes=@tip";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                cdg.Parameters.AddWithValue("@tip", tipede);                // tipo pedidos catalogo clientes
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Fill(dtu);  // original con la carga
                dag.Dispose();
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion de taller de produccion ... ok
                const string contaller = "select descrizionerid,idcodice,codigo from desc_loc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6) + " - " + row.ItemArray[0].ToString());
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de tipo de pedido ... ok
                const string conpedido = "select descrizionerid,idcodice from desc_tpe " +
                                       "where numero=1 and idcodice in (@dstp)";
                MySqlCommand cmdpedido = new MySqlCommand(conpedido, conn);
                cmdpedido.Parameters.AddWithValue("@dstp", tipede);
                DataTable dtpedido = new DataTable();
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
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
            string adjunC = "";
            string adjunV = "";
            if (tx_adjun1.Text.Trim() != "")
            {
                if(tx_adjun1.Text.Trim().Length > 245)
                {
                    MessageBox.Show("La longitud de la ruta es muy larga", "Error validando Adjunto 1");
                    return retorna;
                }
                adjunC = adjunC + ",nomimg1,imagen1";
                adjunV = adjunV + ",@noma1,@imag1";
            }
            if (tx_adjun2.Text.Trim() != "")
            {
                if (tx_adjun2.Text.Trim().Length > 245)
                {
                    MessageBox.Show("La longitud de la ruta es muy larga", "Error validando Adjunto 2");
                    return retorna;
                }
                adjunC = adjunC + ",nomimg2,imagen2";
                adjunV = adjunV + ",@noma2,@imag2";
            }
            string inserta = "insert into pedidos (codped,contrato,cliente,origen,destino,fecha,entrega,coment,tipoes,status,user,dia" + adjunC + ") " +
                "values (@cped,@cont,@clie,@orig,@dest,@fech,@entr,@come,@tipo,@esta,@asd,now()" + adjunV + ")";
            MySqlCommand micon = new MySqlCommand(inserta, conn);
            micon.Parameters.AddWithValue("@cped", tx_codped.Text);
            micon.Parameters.AddWithValue("@cont", tx_cont.Text);
            micon.Parameters.AddWithValue("@clie", tx_idc.Text);
            micon.Parameters.AddWithValue("@orig", tx_dat_orig.Text);
            micon.Parameters.AddWithValue("@dest", tx_dat_dest.Text);
            micon.Parameters.AddWithValue("@fech", dtp_pedido.Value.ToString("yyyy-MM-dd"));
            micon.Parameters.AddWithValue("@entr", dtp_entreg.Value.ToString("yyyy-MM-dd"));
            micon.Parameters.AddWithValue("@come", tx_coment.Text.Trim());
            micon.Parameters.AddWithValue("@tipo", tx_dat_tiped.Text);
            micon.Parameters.AddWithValue("@esta", "");
            micon.Parameters.AddWithValue("@asd", asd);
            if (tx_adjun1.Text.Trim() != "")
            {
                using (var stream = new FileStream(tx_adjun1.Text.Trim(), FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        byte[] file;
                        file = reader.ReadBytes((int)stream.Length);
                        micon.Parameters.AddWithValue("@noma1", tx_dat_adj1.Text);
                        micon.Parameters.Add("@imag1", MySqlDbType.VarBinary, file.Length).Value = file;
                    }
                }
            }
            if (tx_adjun2.Text.Trim() != "")
            {
                using (var stream = new FileStream(tx_adjun2.Text.Trim(), FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        byte[] file;
                        file = reader.ReadBytes((int)stream.Length);
                        micon.Parameters.AddWithValue("@noma2", tx_dat_adj2.Text);
                        micon.Parameters.Add("@imag2", MySqlDbType.VarBinary, file.Length).Value = file;
                    }
                }
            }
            micon.ExecuteNonQuery();
            string lee = "select last_insert_id()";
            micon = new MySqlCommand(lee, conn);
            MySqlDataReader dr = micon.ExecuteReader();
            if (dr.Read())
            {
                tx_idr.Text = dr.GetString(0);
            }
            dr.Close();
            // detalle
            inserta = "insert into detaped (pedidoh,tipo," +
                "cant,item,nombre,medidas,madera,estado,saldo,piedra,coment,precio,total,iddetc) " +
                "values (@cped,@tipo,@cant,@item,@nomb,@medi,@made,@esta,@sald,@pied,@come,@prec,@tota,@iddc)";   // inserta detalle del pedido
            string actua = "update detacon set saldo=saldo-@can where iddetacon=@idd";       // actualiza saldo en detacon .. contratoh=@cont and 
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {   // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo
                if (row.Cells["item"].Value != null)
                {
                    // en iOMG se tendrá un detalle por cada pedido, los pedidos de CoopV3 se matan ahi.
                    // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo
                    micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@cped", tx_codped.Text);
                    micon.Parameters.AddWithValue("@tipo", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@cant", row.Cells["cant"].Value.ToString());
                    micon.Parameters.AddWithValue("@item", row.Cells["item"].Value.ToString());
                    micon.Parameters.AddWithValue("@nomb", row.Cells["nombre"].Value.ToString());
                    micon.Parameters.AddWithValue("@medi", row.Cells["medidas"].Value.ToString());
                    micon.Parameters.AddWithValue("@made", row.Cells[5].Value.ToString());              // codigo madera
                    micon.Parameters.AddWithValue("@esta", row.Cells[9].Value.ToString());
                    micon.Parameters.AddWithValue("@sald", row.Cells["saldo"].Value.ToString());
                    micon.Parameters.AddWithValue("@pied", row.Cells["piedra"].Value.ToString());
                    micon.Parameters.AddWithValue("@come", row.Cells["coment"].Value.ToString());
                    micon.Parameters.AddWithValue("@prec", row.Cells["total"].Value.ToString());
                    micon.Parameters.AddWithValue("@tota", row.Cells["total"].Value.ToString());
                    micon.Parameters.AddWithValue("@iddc", row.Cells["iddetc"].Value.ToString());
                    micon.ExecuteNonQuery();
                    // actualizacion de saldos en el contrato
                    micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@can", row.Cells["cant"].Value.ToString());
                    micon.Parameters.AddWithValue("@idd", row.Cells["iddetc"].Value.ToString());   // columna id detacon
                    //micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                    micon.ExecuteNonQuery();
                    retorna = true;
                }
            }
            // cambiar el estado del contrato
            //string reto = lib.estcont(tx_cont.Text.Trim());
            acciones reto = new acciones();
            reto.act_cont(tx_cont.Text.Trim(),"");
            //MessageBox.Show("Estado actual del contrato " + tx_cont.Text.Trim() + Environment.NewLine +
            //    reto, "CONTRATO CON NUEVO ESTADO");
            retorna = true;
            //
            conn.Close();
            return retorna;
        }
        private bool edita()                                // modifica pedido de clientes
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string cab1 = "";
                    string cab2 = "";
                    if (tx_adjun1.Text.Trim() != advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomimg1"].Value.ToString().Trim())
                    {
                        cab1 = cab1 + ",nomimg1=@nom1,imagen1=@imag1";
                    }
                    if (tx_adjun2.Text.Trim() != advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomimg2"].Value.ToString().Trim())
                    {
                        cab2 = cab2 + ",nomimg2=@nom2,imagen2=@imag2";
                    }   // OJO, no debe cambiarse el contrato porque habria que recalcular saldos en detacon
                    string actua = "update pedidos set " +
                        "origen=@orig,destino=@dest,fecha=@fech,tipoes=@tipo," + 
                        "coment=@come,entrega=@entr,user=@asd,dia=now()" + cab1 + cab2 + " " +
                        "where id=@idr";    // contrato=@cont,cliente=@clie,
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    //micon.Parameters.AddWithValue("@cont", tx_cont.Text);
                    //micon.Parameters.AddWithValue("@clie", tx_idc.Text);
                    micon.Parameters.AddWithValue("@orig", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@dest", tx_dat_dest.Text);
                    micon.Parameters.AddWithValue("@fech", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@tipo", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.Parameters.AddWithValue("@come", tx_coment.Text);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@entr", dtp_entreg.Value.ToString("yyyy-MM-dd"));
                    if (cab1 != "")
                    {
                        if(tx_adjun1.Text.Trim() != "")
                        {
                            using (var stream = new FileStream(tx_adjun1.Text.Trim(), FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    byte[] file;
                                    file = reader.ReadBytes((int)stream.Length);
                                    micon.Parameters.AddWithValue("@nom1", tx_dat_adj1.Text.Trim());
                                    micon.Parameters.Add("@imag1", MySqlDbType.VarBinary, file.Length).Value = file;
                                }
                            }
                        }
                        else
                        {
                            micon.Parameters.AddWithValue("@nom1", "");
                            micon.Parameters.AddWithValue("@imag1", DBNull.Value);
                        }
                    }
                    if (cab2 != "")
                    {
                        if(tx_adjun2.Text.Trim() != "")
                        {
                            using (var stream = new FileStream(tx_adjun2.Text.Trim(), FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    byte[] file;
                                    file = reader.ReadBytes((int)stream.Length);
                                    micon.Parameters.AddWithValue("@nom2", tx_dat_adj2.Text.Trim());
                                    micon.Parameters.Add("@imag2", MySqlDbType.VarBinary, file.Length).Value = file;
                                }
                            }
                        }
                        else
                        {
                            micon.Parameters.AddWithValue("@nom2", "");
                            micon.Parameters.AddWithValue("@imag2", DBNull.Value);
                        }
                    }
                    micon.ExecuteNonQuery();
                    // detalle
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string insdet = "";
                        if (dataGridView1.Rows[i].Cells[15].Value.ToString() == "A")
                        {
                            insdet = "update detaped set " +
                                "item=@item,cant=@cant,nombre=@nomb,medidas=@medi,madera=@made,estado=@esta,piedra=@det2,coment=@come,fingreso=@fing,saldo=@sald " +
                                "where iddetaped=@idr";
                            //string actuaC = "update detacon set saldo=saldo-@can where iddetacon=@idd";       // problema de actualizar saldo en detacon 
                            // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo,total,ne,iddetc
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[0].Value.ToString());
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[2].Value.ToString());   // row.Cells["item"].Value.ToString()
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[1].Value.ToString());   // row.Cells["cant"].Value.ToString()
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());   // row.Cells["nombre"].Value.ToString()
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());   // row.Cells["medidas"].Value.ToString()
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());   //row.Cells[5].Value.ToString()            // codigo madera
                            micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[9].Value.ToString());   // row.Cells[9].Value.ToString()
                            micon.Parameters.AddWithValue("@det2", dataGridView1.Rows[i].Cells[11].Value.ToString());   // row.Cells["piedra"].Value.ToString()
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[8].Value.ToString());   // row.Cells["coment"].Value.ToString()
                            if (dataGridView1.Rows[i].Cells[12].Value.ToString() != "00/00/0000") micon.Parameters.AddWithValue("@fing", dataGridView1.Rows[i].Cells[12].Value.ToString());
                            else micon.Parameters.AddWithValue("@fing", DBNull.Value);
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[13].Value.ToString());   // row.Cells["saldo"].Value.ToString()
                            micon.ExecuteNonQuery();
                            retorna = true;
                        }
                    }
                    // cambiar el estado del contrato
                    //string reto = lib.estcont(tx_cont.Text.Trim());
                    acciones reto = new acciones();
                    reto.act_cont(tx_cont.Text.Trim(), "");
                    //MessageBox.Show("Estado actual del contrato " + tx_cont.Text.Trim() + Environment.NewLine +
                    //    reto, "CONTRATO CON NUEVO ESTADO"); micon.ExecuteNonQuery();
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
        private bool anula()                                // anula pedido, regresa saldos y actualiza estado del pedido
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                // nombre del estado anulado
                string consulta = "select ifnull(descrizionerid,'') from desc_sta where idcodice=@ca";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@ca", estanu);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.Read())
                {
                    tx_status.Text = dr.GetString(0);
                }
                dr.Close();
                // anular el pedido
                consulta = "update pedidos set status=@sta where id=@idp";
                micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@sta", estanu);
                micon.Parameters.AddWithValue("@idp", tx_idr.Text);
                micon.ExecuteNonQuery();
                // sumar y actualizar saldos de detalle en detacon
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string insdet = "";
                    int vsal=0;
                    if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() != "")
                    {
                        string lectura = "select saldo from detacon where iddetacon=@idd";
                        micon = new MySqlCommand(lectura, conn);
                        micon.Parameters.AddWithValue("@idd", dataGridView1.Rows[i].Cells["iddetc"].Value.ToString());
                        dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            vsal = dr.GetInt16(0) + int.Parse(dataGridView1.Rows[i].Cells["cant"].Value.ToString());
                        }
                        dr.Close();
                        insdet = "update detacon set saldo=@vsal where iddetacon=@idd";
                        micon = new MySqlCommand(insdet, conn);
                        micon.Parameters.AddWithValue("@idd", dataGridView1.Rows[i].Cells["iddetc"].Value.ToString());
                        micon.Parameters.AddWithValue("@vsal", vsal);
                        micon.ExecuteNonQuery();
                    }
                }
                // cambiar el estado del contrato
                //string reto = lib.estcont(tx_cont.Text.Trim());
                acciones reto = new acciones();
                reto.act_cont(tx_cont.Text.Trim(), "");
                //MessageBox.Show("Estado actual del contrato " + tx_cont.Text.Trim() + Environment.NewLine +
                //    reto, "CONTRATO CON NUEVO ESTADO");
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
            string consulta = "select a.id,a.contrato,b.idanagrafica,b.razonsocial,c.descrizionerid,a.tipoes " +
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
                    tx_cliente.Text = dr.GetString(3);
                    //tx_ciudades.Text = dr.GetString(4); //cmb_destino.Text.PadRight(15).Substring(9,15);
                    tx_dat_dest.Text = dr.GetString(5);
                    cmb_destino.SelectedIndex = cmb_destino.FindString(tx_dat_dest.Text);
                    retorna = true;
                }
                else retorna = false;
            }
            dr.Close();
            conn.Close();
            return retorna;
        }
        private string gencodp(string cont)                 // genera codigo de pedido
        {                                       // letra + #cont + '_' + #correlativo ... Ejemplo: C023055_1
            string retorna = "";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return retorna;
            }
            string cnsult = "select count(id) from pedidos where contrato=@cont";
            MySqlCommand micon = new MySqlCommand(cnsult, conn);
            micon.Parameters.AddWithValue("@cont", cont);
            MySqlDataReader dr = micon.ExecuteReader();
            int cant = 0;
            string let = "";
            if (dr.Read())
            {
                cant = dr.GetInt16(0) + 1;
                let = lib.funlet(cant);
            }
            dr.Close();
            conn.Close();
            retorna = letiden + tx_cont.Text.Trim() + let;  // "_" + cant.ToString();
            return retorna;
        }
        private bool ingped(string pedido)                  // retorna true si el pedido ya ingreso
        {
            bool retorna = true;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {   // fechain,tipoes,origen,destino,pedido,cant,articulo,madera,estado
                string busca = "select count(idmovim) from movim where trim(pedido)=@ped";
                MySqlCommand micon = new MySqlCommand(busca, conn); 
                micon.Parameters.AddWithValue("@ped", pedido.Trim().ToUpper());
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetInt16(0) > 0) retorna = true;
                    else retorna = false;
                }
                dr.Close();
            }
            else
            {
                MessageBox.Show("No obtiene respuesta del servidor", "Error de conexión");
            }
            conn.Close();
            return retorna;
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
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            tabControl1.SelectedTab = tabuser;
            escribe(this);
            escribepag(tabuser);
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            dtp_pedido.Value = DateTime.Now;
            dtp_entreg.Value = DateTime.Now;
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            grilladet("NUEVO");
            tabControl1.SelectedTab = tabuser;
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            cmb_tipo.Enabled = false;
            tx_codped.ReadOnly = true;
            tx_cont.ReadOnly = false;
            dtp_fingreso.Checked = false;
            dtp_fingreso.Enabled = false;
            tx_saldo.ReadOnly = false;
            tx_d_can.ReadOnly = false;
            tx_d_nom.ReadOnly = false;
            tx_d_med.ReadOnly = false;
            cmb_taller.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                //tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabuser;
            escribe(this);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            dtp_fingreso.Checked = false;
            jalaoc("tx_idr");
            tx_codped.ReadOnly = false;
            tx_saldo.ReadOnly = false;
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
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                //tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabuser;
            sololeepag(tabuser);
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            dtp_fingreso.Checked = false;
            jalaoc("tx_idr");
            tx_codped.ReadOnly = false;
            tx_codped.Enabled = true;
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            tabControl1.SelectedTab = tabgrilla;
            sololee(this);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;    // Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabuser);
            sololeepag(tabuser);
            tx_codped.Enabled = true;
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            dtp_fingreso.Checked = false;
            jalaoc("tx_idr");
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            if (tx_codped.Text != "")
            {
                //Tx_modo.Text = "IMPRIMIR";
                setParaCrystal();
            }
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_codped.Text != "")
            {
                //Tx_modo.Text = "IMPRIMIR";
                setParaCrystal();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Pedidos_contratos_clientes_" +
                "" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                wb.Worksheets.Add(dtg, "Articulos");
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
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            //--
            //tx_idr.Text = lib.gofirts(nomtab);
            tx_idr_Leave(null, null);
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            //string aca = tx_idr.Text;
            limpia_chk();
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            limpiar(this);
            //--
            //tx_idr.Text = lib.goback(nomtab, aca);
            tx_idr_Leave(null, null);
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            //string aca = tx_idr.Text;
            limpia_chk();
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            limpiar(this);
            //--
            //tx_idr.Text = lib.gonext(nomtab, aca);
            tx_idr_Leave(null, null);
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            limpiar(this);
            limpia_chk();
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            //--
            //tx_idr.Text = lib.golast(nomtab);
            tx_idr_Leave(null, null);
        }
        #endregion botones;
        // configurador de permisos
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
            tx_d_can.Text = "";
            tx_d_codi.Text = "";
            tx_d_com.Text = "";
            tx_d_det2.Text = "";
            tx_acab.Text = "";
            tx_d_est.Text = "";
            tx_d_id.Text = "";
            tx_d_it.Text = "";
            tx_d_mad.Text = "";
            tx_d_med.Text = "";
            tx_d_nom.Text = "";
        }
        private void limpia_chk()
        {
            //checkBox1.Checked = false;
        }
        private void limpia_otros(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            //this.checkBox1.Checked = false;
        }
        private void limpia_combos(TabPage pag)
        {
            //tabControl1.SelectedTab = pag;
            cmb_taller.SelectedIndex = -1;
        }
        private void limpia_panel(Panel pan)
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
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            //string cod2d = "";
            foreach (DataRow row in dttaller.Rows)
            {
                if (row["idcodice"].ToString().Trim() == tx_dat_orig.Text.Trim())
                {
                    //cod2d = row["codigo"].ToString();
                    tx_codta.Text = row["codigo"].ToString();
                }
            }
            if (Tx_modo.Text != "NUEVO" && dataGridView1.Rows.Count > 0)
            {
                // dataGridView1.Columns[2].Name = "item";
                //DataTable dt = (DataTable)dataGridView1.DataSource;
                foreach(DataGridViewRow row in dataGridView1.Rows)   // DataRow row in dt.Rows
                {
                    //row[2] = row[2].ToString().Substring(0, 10) + tx_codta.Text.Trim() + row[2].ToString().Substring(12, 6);
                    if (row.Cells[2].Value != null && row.Cells[2].Value.ToString() != "")
                        row.Cells[2].Value = row.Cells[2].Value.ToString().Substring(0, 10) + tx_codta.Text.Trim() + row.Cells[2].Value.ToString().Substring(12, 6);
                }
            }
        }
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tiped.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_destino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_destino.SelectedValue != null) tx_dat_dest.Text = cmb_destino.SelectedValue.ToString();
            else tx_dat_dest.Text = cmb_destino.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        #endregion comboboxes
        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text.Trim() != "" && tx_codped.Text.Trim() == "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_codped_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_codped.Text != "" && tx_idr.Text.Trim() == "")
            {
                tx_cont.ReadOnly = true;
                tx_codped.ReadOnly = true;
                jalaoc("tx_codped");
            }
        }
        private void tx_d_can_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                tx_saldo.Text = tx_d_can.Text;
                if (tx_d_codi.Text.Trim() != "")
                {
                    tx_d_codi_Leave(null, null);
                }
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
        private void tx_cont_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_cont.Text.Trim() != "")  //  && tx_cliente.Text.Trim() == ""
                {
                    if (buscont(tx_cont.Text) == false)
                    {
                        MessageBox.Show("No existe el contrato o se encuentra ANULADO", "Error");
                        tx_cont.Text = "";
                        tx_cliente.Text = "";
                    }
                }
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
            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && Tx_modo.Text != "NUEVO")
            {
                //string codu = "";
                string idr, rind = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rind = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabuser;
                limpiar(this);
                limpiapag(tabuser);
                limpia_otros(tabuser);
                limpia_combos(tabuser);
                tx_idr.Text = idr;
                tx_rind.Text = rind;
                tx_dat_tiped.Text = tipede;
                tx_cont.ReadOnly = true;
                tx_codped.ReadOnly = true;
                jalaoc("tx_idr");
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString()
                && Tx_modo.Text == "EDITAR")
            {
                string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                string[] noeta = equivinter(advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString());    // retorna la tabla segun el titulo de la columna

                var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                    "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    if (advancedDataGridView1.Columns[e.ColumnIndex].Tag.ToString() == "validaSI")   // la columna se valida?
                    {
                        // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,nom_estado,a.origen,a.destino,fecha,entrega,a.coment,a.tipoes,a.status
                        // nada se edita directamente en la grilla, solo comentarios
                        // valida si el dato ingresado es valido en la columna
                        if (e.ColumnIndex == 5)           // fecha pedido
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 6)           // fecha entrega
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 7)          // comentario
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 8)          // tipo pedido
                        {
                            // no se valida
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
            else
            {
                //if(Tx_modo.Text == "NUEVO" || Tx_modo.Text == "VISUALIZAR") e.Cancel = true;
            }
        }
        private void advancedDataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion
        #region datagridview1 - grilla detalle de pedido
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                if (Tx_modo.Text == "EDITAR")
                {
                    dtp_fingreso.Enabled = true;
                    tx_saldo.Enabled = true;
                    tx_d_can.Enabled = true;
                    tx_d_can.ReadOnly = false;
                }
                else
                {
                    dtp_fingreso.Enabled = false;
                    tx_saldo.Enabled = false;
                    tx_d_can.Enabled = false;
                }
                tx_d_nom.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();    //
                tx_d_med.Text = dataGridView1.Rows[e.RowIndex].Cells["medidas"].Value.ToString();   //
                tx_d_can.Text = dataGridView1.Rows[e.RowIndex].Cells["cant"].Value.ToString();      //
                tx_d_id.Text = dataGridView1.Rows[e.RowIndex].Cells["iddetaped"].Value.ToString();  //
                tx_d_codi.Text = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString();     //
                tx_d_com.Text = dataGridView1.Rows[e.RowIndex].Cells["coment"].Value.ToString();    //
                tx_d_mad.Text = dataGridView1.Rows[e.RowIndex].Cells["madera"].Value.ToString();    //
                tx_saldo.Text = dataGridView1.Rows[e.RowIndex].Cells["saldo"].Value.ToString();     // saldo
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
                                tx_d_mad.Text, tx_d_det2.Text, tx_acab.Text, tx_d_com.Text, tx_d_est.Text,
                            tx_dat_mad.Text, "", "", tx_saldo.Text, tx_d_precio.Text, "N", tx_d_iddc.Text);
                }
            }
            if (Tx_modo.Text == "EDITAR")   // SOLO SE PERMITE EDITAR COMENTARIO DE ITEM 01/10/2020
            {
                if (tx_d_id.Text.Trim() != "")  // iddetaped,cant,item,nombre,medidas,madera,piedra,descrizionerid,coment,estado,madera,piedra,fingreso,saldo,total,ne,iddetc
                {
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;    // cant editada > cant grilla? -> saldo=saldo+(dif cant edit - cant grilla)
                    //int dif = int.Parse(tx_d_can.Text) - int.Parse(obj.Cells[1].Value.ToString());
                    obj.Cells[8].Value = tx_d_com.Text;
                    //obj.Cells[1].Value = tx_d_can.Text;
                    //obj.Cells[13].Value = (int.Parse(tx_d_can.Text) + dif).ToString();
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
        private void button1_Click(object sender, EventArgs e)      // graba pedido cabecera y detalle
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (dataGridView1.Rows.Count < 2)
                {
                    MessageBox.Show("Debe ingresar los artículos a pedir", "Atención - complete", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_d_can.Focus();
                    return;
                }
                if (tx_dat_orig.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione el taller", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_taller.Focus();
                    return;
                }
                if (tx_cliente.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione al cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_cliente.Focus();
                    return;
                }
                if (tx_cont.Text.Trim() == "")
                {
                    MessageBox.Show("Ingrese el contrato", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_cont.Focus();
                    return;
                }
                tx_codped.Text = gencodp(tx_cont.Text);     // genramos el codigo del pedido
                if(graba() == true)
                {
                    // agregamos el nuevo registro a la grilla
                    DataRow dr = dtg.NewRow();
                    // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,space(6) as nomest,a.origen,a.destino,
                    // fecha,entrega,a.coment,a.tipoes,a.status
                    string cid = tx_idr.Text;       // sería bueno que fuera el id real  
                    dr[0] = cid;
                    dr[1] = tx_codped.Text;
                    dr[2] = tx_cont.Text;
                    dr[3] = tx_idc.Text;
                    dr[4] = tx_cliente.Text.Trim();
                    dr[5] = "";
                    dr[6] = tx_dat_orig.Text;
                    dr[7] = tx_dat_dest.Text;
                    dr[8] = dtp_pedido.Value.ToString("yyy-MM-dd");
                    dr[9] = dtp_entreg.Value.ToString("yyy-MM-dd");
                    dr[10] = tx_coment.Text;
                    dr[11] = tx_dat_tiped.Text;
                    dr[12] = "";
                    dtg.Rows.Add(dr);
                    // vista previa
                    setParaCrystal();
                }
            }
            if (Tx_modo.Text == "EDITAR")
            {
                if (dataGridView1.Rows.Count < 2)
                {
                    MessageBox.Show("Debe ingresar los artículos a pedir", "Atención - complete", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_d_can.Focus();
                    return;
                }
                if (tx_dat_orig.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione el taller", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_taller.Focus();
                    return;
                }
                if (tx_cliente.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione al cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_cliente.Focus();
                    return;
                }
                if (tx_cont.Text.Trim() == "")
                {
                    MessageBox.Show("Ingrese el contrato", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_cont.Focus();
                    return;
                }
                var aa = MessageBox.Show("Confirma que desea modificar?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (edita() == true)
                    {
                        // actualizamos el datatable
                        for (int i = 0; i < dtg.Rows.Count; i++)
                        {
                            DataRow row = dtg.Rows[i];
                            if (row[0].ToString() == tx_idr.Text)
                            {
                                // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,space(6) as nomest,a.origen,a.destino,
                                // fecha,entrega,a.coment,a.tipoes,a.status
                                dtg.Rows[i][3] = tx_idc.Text;
                                dtg.Rows[i][4] = tx_cliente.Text.Trim();
                                dtg.Rows[i][5] = "";
                                dtg.Rows[i][6] = tx_dat_orig.Text;
                                dtg.Rows[i][7] = tx_dat_dest.Text;
                                dtg.Rows[i][8] = dtp_pedido.Value.ToString("yyy-MM-dd");
                                dtg.Rows[i][9] = dtp_entreg.Value.ToString("yyy-MM-dd");
                                dtg.Rows[i][10] = tx_coment.Text;
                                dtg.Rows[i][11] = tx_dat_tiped.Text;
                                dtg.Rows[i][12] = "";
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar!", "Error");
                        return;
                    }
                }
            }
            if (Tx_modo.Text == "ANULAR")
            {
                if (tx_status.Text != nomanu)
                {
                    // aca falta validar si el pedido fue atendido aunque sea en parte
                    // si ingreso todo o parte del pedido YA NO SE PUEDE ANULAR, solo cerrar
                    int tot1 = 0, tot2 = 0;
                    for (int i=0; i < dataGridView1.Rows.Count -1; i++)
                    {
                        tot1 = tot1 + ((dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() == "") ? 0 : int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));   // cant
                        tot2 = tot2 + ((dataGridView1.Rows[i].Cells[13].Value.ToString().Trim() == "") ? 0 : int.Parse(dataGridView1.Rows[i].Cells[13].Value.ToString()));  // saldo
                    }
                    if (tot1 != tot2)
                    {
                        MessageBox.Show("El pedido no se puede anular porque registra ingreso" + Environment.NewLine +
                            "solo se puede editar o cerrar", "No es posible continuar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    var xx = MessageBox.Show("Confirma que desea ANULAR el presente pedido?", "Atención - Confirme",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (xx == DialogResult.Yes)
                    {
                        if (anula() == true)
                        {
                            // actualizamos el datatable
                            for (int i = 0; i < dtg.Rows.Count; i++)
                            {
                                DataRow row = dtg.Rows[i];
                                if (row[0].ToString() == tx_idr.Text)
                                {
                                    // a.id,a.codped,a.contrato,a.cliente,c.razonsocial,nomest,a.origen,a.destino,
                                    // fecha,entrega,a.coment,a.tipoes,a.status
                                    dtg.Rows[i][5] = tx_status.Text;
                                    dtg.Rows[i][12] = estanu;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ya se encuentra anulado", "Verifique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            // limpiamos todo
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            limpia_panel(panel1);
            limpiapag(tabuser);
        }
        private void button2_Click(object sender, EventArgs e)      // documentos adjuntos 1
        {
            if(Tx_modo.Text == "NUEVO")
            {
                if (tx_adjun1.Text.Trim() != "")
                {
                    MessageBox.Show("Debe borrar espacio para djuntos", "Borre un adjunto", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                else
                {
                    OpenFileDialog ofd1 = new OpenFileDialog();
                    ofd1.Title = "Seleccione el documento a Adjuntar";
                    ofd1.Multiselect = false;
                    var aa = ofd1.ShowDialog();
                    if(aa != DialogResult.Cancel)
                    {
                        tx_adjun1.Text = ofd1.FileName;
                        tx_dat_adj1.Text = ofd1.SafeFileName;
                    }
                }
            }
            if(Tx_modo.Text == "EDITAR")
            {
                if (tx_adjun1.Text.Trim() != "")
                {
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        using (var sqlQuery = new MySqlCommand(@"SELECT imagen1 FROM pedidos WHERE id = @IDR", conn))
                        {
                            sqlQuery.Parameters.AddWithValue("@IDR", tx_idr.Text);
                            using (var sqlQueryResult = sqlQuery.ExecuteReader())
                                if (sqlQueryResult != null)
                                {
                                    sqlQueryResult.Read();
                                    var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                                    sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                                    FolderBrowserDialog ruta = new FolderBrowserDialog();
                                    var aa = ruta.ShowDialog();
                                    if(aa != DialogResult.Cancel)
                                    {
                                        string chivo = ruta.SelectedPath.ToString() + "\\" + tx_adjun1.Text.Trim();
                                        using (var fs = new FileStream(chivo, FileMode.Create, FileAccess.Write))
                                            fs.Write(blob, 0, blob.Length);
                                        MessageBox.Show("Archivo grabado con éxito", "Confirmación de escritura", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                        }
                    }
                }
                else
                {
                    OpenFileDialog ofd1 = new OpenFileDialog();
                    ofd1.Title = "Seleccione el documento a Adjuntar";
                    ofd1.Multiselect = false;
                    var aa = ofd1.ShowDialog();
                    if (aa != DialogResult.Cancel)
                    {
                        tx_adjun1.Text = ofd1.FileName;
                        tx_dat_adj1.Text = ofd1.SafeFileName;
                    }
                }
            }
        }
        private void bt_adj2_Click(object sender, EventArgs e)      // documentos adjuntos 2
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_adjun2.Text.Trim() != "")
                {
                    MessageBox.Show("Debe borrar espacio para djuntos", "Borre un adjunto", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                else
                {
                    OpenFileDialog ofd2 = new OpenFileDialog();
                    ofd2.Title = "Seleccione el documento a Adjuntar";
                    ofd2.Multiselect = false;
                    var aa =ofd2.ShowDialog();
                    if(aa != DialogResult.Cancel)
                    {
                        tx_adjun2.Text = ofd2.FileName;
                        tx_dat_adj2.Text = ofd2.SafeFileName;
                    }
                }
            }
            if (Tx_modo.Text == "EDITAR")
            {
                if (tx_adjun2.Text.Trim() != "")
                {
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        using (var sqlQuery = new MySqlCommand(@"SELECT imagen2 FROM pedidos WHERE id = @IDR", conn))
                        {
                            sqlQuery.Parameters.AddWithValue("@IDR", tx_idr.Text);
                            using (var sqlQueryResult = sqlQuery.ExecuteReader())
                                if (sqlQueryResult != null)
                                {
                                    sqlQueryResult.Read();
                                    var b2ob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                                    sqlQueryResult.GetBytes(0, 0, b2ob, 0, b2ob.Length);
                                    FolderBrowserDialog ruta = new FolderBrowserDialog();
                                    ruta.Description = "Lugar donde se grabará el archivo";
                                    var aa = ruta.ShowDialog();
                                    if (aa != DialogResult.Cancel)
                                    {
                                        string chivo = ruta.SelectedPath.ToString() + "\\" + tx_adjun2.Text.Trim();
                                        using (var fs = new FileStream(chivo, FileMode.Create, FileAccess.Write))
                                            fs.Write(b2ob, 0, b2ob.Length);
                                        MessageBox.Show("Archivo grabado con éxito", "Confirmación de escritura", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                        }
                    }
                }
                else
                {
                    OpenFileDialog ofd2 = new OpenFileDialog();
                    ofd2.Title = "Seleccione el documento a Adjuntar";
                    ofd2.Multiselect = false;
                    var aa = ofd2.ShowDialog();
                    if (aa != DialogResult.Cancel)
                    {
                        tx_adjun2.Text = ofd2.FileName;
                        tx_dat_adj2.Text = ofd2.SafeFileName;
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)      // borra adjunto 1
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR") tx_adjun1.Text = "";
        }
        private void button3_Click_1(object sender, EventArgs e)    // borra adjunto 2
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR") tx_adjun2.Text = "";
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
            int can = 0;
            decimal tot = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["item"].Value != null)
                {
                    pedsclts.deta_pedcltRow rowdetalle = reppedido.deta_pedclt.Newdeta_pedcltRow();
                    rowdetalle.id = "0";
                    rowdetalle.cant = row.Cells["cant"].Value.ToString();
                    rowdetalle.item = row.Cells["item"].Value.ToString();
                    rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                    rowdetalle.medidas = row.Cells["medidas"].Value.ToString();
                    rowdetalle.acabado = row.Cells[7].Value.ToString();
                    rowdetalle.coment = row.Cells["coment"].Value.ToString();
                    rowdetalle.detalle2 = "";   // row.Cells["??"].Value.ToString();
                    rowdetalle.precio = row.Cells["total"].Value.ToString();   //tx_d_precio.Text;
                    rowdetalle.madera = lib.descorta(row.Cells["madera"].Value.ToString(),"mad");
                    reppedido.deta_pedclt.Adddeta_pedcltRow(rowdetalle);
                    //
                    can += int.Parse(row.Cells["cant"].Value.ToString());
                    tot += decimal.Parse(row.Cells["total"].Value.ToString());
                }
            }
            pedsclts.cabeza_pedcltRow rowcabeza = reppedido.cabeza_pedclt.Newcabeza_pedcltRow();
            rowcabeza.codped = tx_codped.Text;
            rowcabeza.fecha = dtp_pedido.Value.ToString("dd/MM/yyyy");
            rowcabeza.id = "0";
            rowcabeza.origen = cmb_taller.Text;
            rowcabeza.razonsocial = tx_cliente.Text;
            rowcabeza.cliente = tx_idc.Text;
            rowcabeza.coment = tx_coment.Text;
            rowcabeza.contrato = tx_cont.Text;
            rowcabeza.entrega = dtp_entreg.Value.ToString("dd/MM/yyyy");
            rowcabeza.ciudad_des = cmb_destino.Text.PadRight(15).Substring(0, 15); //tx_ciudades.Text;
            rowcabeza.status = (tx_status.Text == nomanu) ? tx_status.Text : "";
            rowcabeza.taller = (cmb_taller.Text.Trim().Length < 7) ? cmb_taller.Text : cmb_taller.Text.Substring(9, cmb_taller.Text.Trim().Length - 9);
            rowcabeza.cant = can.ToString();
            rowcabeza.total = tot.ToString();
            reppedido.cabeza_pedclt.Addcabeza_pedcltRow(rowcabeza);
            //
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
    }
}
