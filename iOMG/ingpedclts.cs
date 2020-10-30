using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class ingpedclts : Form
    {
        static string nomform = "ingpedclts";      // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "contrat";
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
        string tipede = "";             // tipo de ingreso
        string tipedc = "";             // tipo de pedido de cliente
        string cliente = Program.cliente;    // razon social para los reportes
        #endregion
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();

        public ingpedclts()
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
            if (keyData == Keys.F1 && Tx_modo.Text == "NUEVO")
            {
                if (tx_pedido.Focused == true)     // pedidos de clientes
                {
                    para1 = "pedidos";
                    para2 = "pend";                                         // que no esten aun recibidos
                    para3 = tipedc;                                         // pedidos de clientes
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            // ayu2.ReturnValue0;
                            //       0,    1,      2,      3,   4,   5,     6,      7,     8,     9,    10,   11,   12,    13,     14,      15,      16   17
                            // codped,origen,destino,cliente,cant,item,nombre,medidas,madera,estado,precio,total,nomad,acabado,nomorig,nomdestin,contrato,
                            // llenado de campos
                            tx_pedido.Text = ayu2.ReturnValueA[0].ToString();
                            tx_dat_ped.Text = ayu2.ReturnValueA[0].ToString();
                            tx_cliente.Text = ayu2.ReturnValueA[3].ToString();
                            tx_dat_orig.Text = ayu2.ReturnValueA[1].ToString();
                            tx_origen.Text = ayu2.ReturnValueA[14].ToString();
                            tx_dat_dest.Text = ayu2.ReturnValueA[2].ToString();
                            tx_dest.Text = ayu2.ReturnValueA[15].ToString();
                            tx_item.Text = ayu2.ReturnValueA[5].ToString();
                            tx_nombre.Text = ayu2.ReturnValueA[6].ToString();
                            tx_medidas.Text = ayu2.ReturnValueA[7].ToString();
                            tx_dat_mad.Text = ayu2.ReturnValueA[8].ToString();
                            tx_nomad.Text = ayu2.ReturnValueA[12].ToString();
                            tx_dat_aca.Text = ayu2.ReturnValueA[9].ToString();
                            tx_acabad.Text = ayu2.ReturnValueA[13].ToString();
                            tx_cant.Text = ayu2.ReturnValueA[4].ToString();
                            tx_dat_cant.Text = ayu2.ReturnValueA[4].ToString();
                            tx_precio.Text = ayu2.ReturnValueA[10].ToString();
                            tx_total.Text = ayu2.ReturnValueA[11].ToString();
                            tx_contrato.Text = ayu2.ReturnValueA[16].ToString();
                            tx_dat_fechp.Text = ayu2.ReturnValueA[17].ToString();
                            // saldo solo jala de ingresos existentes
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ingpedclts_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            this.KeyPreview = true;
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);

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
            tx_comen.MaxLength = 50;
            cmb_tipo.Enabled = false;                       // no se debe mover el tipo de ingreso
            tx_pedido.CharacterCasing = CharacterCasing.Upper;
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
                        if (row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
                        if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                        if (row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                        if (row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                        if (row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                        if (row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                        if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "tipoing" && row["param"].ToString() == "cliente") tipede = row["valor"].ToString().Trim();   // tipo de ingreso
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "cliente") tipedc = row["valor"].ToString().Trim();   // tipo ped cliente
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
            if (quien == "maestra")
            {
                // datos de los contratos date_format(date(a.fecha),'%Y-%m-%d')
                string datgri = "select a.idmovim,date(a.fechain) as fechain,a.tipoes,a.origen,a.destino,a.pedido,trim(cl.razonsocial) as cliente,a.coment," +
                    "a.cant,a.articulo,a.med1,b.descrizionerid as nomad,c.descrizionerid as acabado,a.precio,a.total," +
                    "a.madera,a.estado,d.descrizionerid as nomorig,e.descrizionerid as nomdestin,dp.nombre,pe.contrato,a.saldo,pe.fecha " +
                    "from movim a left join desc_mad b on b.idcodice=a.madera " +
                    "left join desc_est c on c.idcodice=a.estado " +
                    "left join desc_loc d on d.idcodice=a.origen " +
                    "left join desc_alm e on e.idcodice=a.destino " +
                    "left join detaped dp on dp.pedidoh=a.pedido AND dp.item=a.articulo " +
                    "left join pedidos pe on pe.codped=a.pedido and pe.tipoes=@tpe " +
                    "left join contrat co ON co.contrato=pe.contrato " +
                    "left join anag_cli cl on cl.idanagrafica=pe.cliente " +
                    "WHERE co.status <> 'ENTREG' order by idmovim";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                cdg.Parameters.AddWithValue("@tpe", tipedc);                    // codigo pedido cliente
                //cdg.Parameters.AddWithValue("@tip", tipede);                  // "TPE001"
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Dispose();
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion de tipo de contrato
                const string conpedido = "select descrizionerid,idcodice from desc_tmo " +
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
            }
            conn.Close();
        }
        private void grilla()                               // arma la grilla
        {
            // a.idmovim,a.fechain,a.tipoes,a.origen,a.destino,a.pedido,a.cliente,a.coment,
            // a.cant,a.articulo,a.med1,b.descrizionerid as nomad,c.descrizionerid as acabado,a.precio,a.total,
            // a.madera,a.estado,nomorig,nomdestin,nombre,contrato,saldo,fecha    ==> 23 columnas
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            // id 
            advancedDataGridView1.Columns[0].Visible = false;
            advancedDataGridView1.Columns[0].HeaderText = "id";    // titulo de la columna
            // fecha de ingreso
            advancedDataGridView1.Columns[1].Visible = true;
            advancedDataGridView1.Columns[1].HeaderText = "F.Ingreso";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 60;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[1].Tag = "validaNO";
            advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // tipo movimiento
            advancedDataGridView1.Columns[2].Visible = false;            // columna visible o no
            advancedDataGridView1.Columns[2].HeaderText = "Tipo";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 60;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaNO";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // taller origen
            advancedDataGridView1.Columns[3].Visible = true;
            advancedDataGridView1.Columns[3].HeaderText = "Taller";    // titulo de la columna
            advancedDataGridView1.Columns[3].Width = 70;                // ancho
            advancedDataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[3].Tag = "validaNO";
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // almacen destino
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Destino";
            advancedDataGridView1.Columns[4].Width = 70;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaSI";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // pedido de cliente
            advancedDataGridView1.Columns[5].Visible = true;
            advancedDataGridView1.Columns[5].HeaderText = "Pedido";
            advancedDataGridView1.Columns[5].Width = 70;
            advancedDataGridView1.Columns[5].ReadOnly = true;
            advancedDataGridView1.Columns[5].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre cliente
            advancedDataGridView1.Columns[6].Visible = true;
            advancedDataGridView1.Columns[6].HeaderText = "Nombre del cliente";
            advancedDataGridView1.Columns[6].Width = 140;
            advancedDataGridView1.Columns[6].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[6].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // comentarios
            advancedDataGridView1.Columns[7].Visible = false;   // true
            advancedDataGridView1.Columns[7].HeaderText = "Comentarios";
            advancedDataGridView1.Columns[7].Width = 200;
            advancedDataGridView1.Columns[7].ReadOnly = true;
            advancedDataGridView1.Columns[7].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // a.madera,a.estado    ==> 17 columnas
            advancedDataGridView1.Columns[8].Visible = false;
            advancedDataGridView1.Columns[8].HeaderText = "Cant";
            advancedDataGridView1.Columns[8].Width = 40;
            advancedDataGridView1.Columns[8].ReadOnly = true;
            // codigo art
            advancedDataGridView1.Columns[9].Visible = true;    // false
            advancedDataGridView1.Columns[9].HeaderText = "Articulo";
            advancedDataGridView1.Columns[9].Width = 130;
            advancedDataGridView1.Columns[9].ReadOnly = true;
            // medidas
            advancedDataGridView1.Columns[10].Visible = false;
            advancedDataGridView1.Columns[10].HeaderText = "Medidas";
            advancedDataGridView1.Columns[10].Width = 150;
            advancedDataGridView1.Columns[10].ReadOnly = true;
            // nombre madera
            advancedDataGridView1.Columns[11].Visible = false;
            advancedDataGridView1.Columns[11].HeaderText = "Nom.Mad";
            advancedDataGridView1.Columns[11].Width = 60;
            advancedDataGridView1.Columns[11].ReadOnly = true;
            // nombre acabado
            advancedDataGridView1.Columns[12].Visible = false;
            advancedDataGridView1.Columns[12].HeaderText = "Nom.Acab.";
            advancedDataGridView1.Columns[12].Width = 60;
            advancedDataGridView1.Columns[12].ReadOnly = true;
            // precio
            advancedDataGridView1.Columns[13].Visible = false;
            advancedDataGridView1.Columns[13].HeaderText = "Precio";
            advancedDataGridView1.Columns[13].Width = 60;
            advancedDataGridView1.Columns[13].ReadOnly = true;
            // total
            advancedDataGridView1.Columns[14].Visible = false;
            advancedDataGridView1.Columns[14].HeaderText = "Total";
            advancedDataGridView1.Columns[14].Width = 60;
            advancedDataGridView1.Columns[14].ReadOnly = true;
            // madera
            advancedDataGridView1.Columns[15].Visible = false;
            advancedDataGridView1.Columns[15].HeaderText = "Madera";
            advancedDataGridView1.Columns[15].Width = 60;
            advancedDataGridView1.Columns[15].ReadOnly = true;
            // acabado
            advancedDataGridView1.Columns[16].Visible = false;
            advancedDataGridView1.Columns[16].HeaderText = "Acabado";
            advancedDataGridView1.Columns[16].Width = 60;
            advancedDataGridView1.Columns[16].ReadOnly = true;
            // nomorig
            advancedDataGridView1.Columns[17].Visible = false;
            advancedDataGridView1.Columns[17].HeaderText = "nomorig";
            advancedDataGridView1.Columns[17].Width = 60;
            advancedDataGridView1.Columns[17].ReadOnly = true;
            // nomdestin
            advancedDataGridView1.Columns[18].Visible = false;
            advancedDataGridView1.Columns[18].HeaderText = "nomdestin";
            advancedDataGridView1.Columns[18].Width = 60;
            advancedDataGridView1.Columns[18].ReadOnly = true;
            // nombre del articulo 
            advancedDataGridView1.Columns[19].Visible = true;   // false
            advancedDataGridView1.Columns[19].HeaderText = "nombre";
            advancedDataGridView1.Columns[19].Width = 160;
            advancedDataGridView1.Columns[19].ReadOnly = true;
            //
            advancedDataGridView1.Columns[20].Visible = false;  // contrato
            advancedDataGridView1.Columns[21].Visible = false;  // saldo
            advancedDataGridView1.Columns[22].Visible = false;  // fecha del pedido
        }
        private void jalaoc(string campo)                   // jala datos
        {
            if (campo == "tx_idr" && tx_idr.Text != "")
            {
                // a.idmovim,a.fechain,a.tipoes,a.origen,a.destino,a.pedido,a.cliente,a.coment,
                // a.cant,a.articulo,a.med1,b.descrizionerid as nomad,c.descrizionerid as acabado,a.precio,a.total,
                // a.madera,a.estado,nomorig,nomdestin,nombre,contrato,saldo    ==> 22 columnas
                tx_pedido.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["pedido"].Value.ToString();                      // 
                dtp_ingreso.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["fechain"].Value.ToString());
                tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["tipoes"].Value.ToString();                   // tipo ingreso
                tx_cliente.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cliente"].Value.ToString();                    // nombre del cliente
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["origen"].Value.ToString();                    // codigo taller
                tx_origen.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomorig"].Value.ToString();
                tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["destino"].Value.ToString();
                tx_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomdestin"].Value.ToString();
                tx_comen.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["coment"].Value.ToString();
                tx_item.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["articulo"].Value.ToString();
                tx_nombre.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nombre"].Value.ToString();
                tx_medidas.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["med1"].Value.ToString();
                tx_dat_mad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["madera"].Value.ToString();
                tx_nomad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomad"].Value.ToString();
                tx_dat_aca.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["estado"].Value.ToString();
                tx_acabad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["acabado"].Value.ToString();
                tx_cant.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cant"].Value.ToString();
                tx_dat_cant.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cant"].Value.ToString();
                tx_precio.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["precio"].Value.ToString();
                tx_total.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["total"].Value.ToString();
                tx_contrato.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["contrato"].Value.ToString();
                tx_saldo.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["saldo"].Value.ToString();
                tx_dat_fechp.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["fecha"].Value.ToString();
                //
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);        // tipo ingreso
            }
            if (campo == "tx_pedido" && tx_pedido.Text != "")
            {
                int cta = 0;
                foreach (DataRow row in dtg.Rows)
                {
                    if (row["pedido"].ToString().Trim() == tx_pedido.Text.Trim())
                    {
                        tx_rind.Text = cta.ToString();
                        tx_idr.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["idmovim"].Value.ToString();                      // 
                        dtp_ingreso.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["fechain"].Value.ToString());
                        tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["tipoes"].Value.ToString();                   // tipo ingreso
                        tx_cliente.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cliente"].Value.ToString();                    // nombre del cliente
                        tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["origen"].Value.ToString();                    // codigo taller
                        tx_origen.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomorig"].Value.ToString();
                        tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["destino"].Value.ToString();
                        tx_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomdestin"].Value.ToString();
                        tx_comen.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["coment"].Value.ToString();
                        tx_item.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["articulo"].Value.ToString();
                        tx_nombre.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nombre"].Value.ToString();
                        tx_medidas.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["med1"].Value.ToString();
                        tx_dat_mad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["madera"].Value.ToString();
                        tx_nomad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["nomad"].Value.ToString();
                        tx_dat_aca.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["estado"].Value.ToString();
                        tx_acabad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["acabado"].Value.ToString();
                        tx_cant.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cant"].Value.ToString();
                        tx_dat_cant.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["cant"].Value.ToString();
                        tx_precio.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["precio"].Value.ToString();
                        tx_total.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["total"].Value.ToString();
                        tx_contrato.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["contrato"].Value.ToString();
                        tx_saldo.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["saldo"].Value.ToString();
                        tx_dat_fechp.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells["fecha"].Value.ToString();
                        //
                        cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);        // tipo ingreso
                    }
                    cta = cta + 1;
                }
            }
        }
        private bool graba()                                // graba cabecera
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string inserta = "insert into movim (fechain,tipoes,origen,destino,pedido,coment," +
                    "cant,saldo,articulo,med1,precio,total,madera,estado,USER,dia) " +
                    "values (@fepe,@tipe,@orig,@dest,@pedi,@come,@cant,@sald,@arti,@med1,@prec,@tota,@made,@esta,@asd,now())";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@fepe", dtp_ingreso.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@tipe", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@orig", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@dest", tx_dat_dest.Text);
                    micon.Parameters.AddWithValue("@pedi", tx_pedido.Text);
                    micon.Parameters.AddWithValue("@come", tx_comen.Text.Trim());
                    micon.Parameters.AddWithValue("@cant", tx_cant.Text);
                    micon.Parameters.AddWithValue("@sald", tx_cant.Text);
                    micon.Parameters.AddWithValue("@arti", tx_item.Text);
                    micon.Parameters.AddWithValue("@med1", tx_medidas.Text);
                    micon.Parameters.AddWithValue("@prec", tx_precio.Text);
                    micon.Parameters.AddWithValue("@tota", tx_total.Text);
                    micon.Parameters.AddWithValue("@made", tx_dat_mad.Text);
                    micon.Parameters.AddWithValue("@esta", tx_dat_aca.Text);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.ExecuteNonQuery();
                    string lid = "select last_insert_id()";
                    micon = new MySqlCommand(lid, conn);
                    MySqlDataReader rlid = micon.ExecuteReader();
                    if (rlid.Read())
                    {
                        tx_idr.Text = rlid.GetString(0);
                    }
                    rlid.Close();
                    // actualiza saldo del pedido
                    string actua = "update detaped set saldo=saldo-@cant,fingreso=@fing where pedidoh=@ped and item=@ite";
                    micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@fing", dtp_ingreso.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@cant", int.Parse(tx_cant.Text));
                    micon.Parameters.AddWithValue("@ped", tx_pedido.Text);
                    micon.Parameters.AddWithValue("@ite", tx_item.Text.Trim());
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
        private bool edita()                                // actualiza solo comentario y tipo
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string actua = "update movim set " +
                        "tipoes=@tipe,fechain=@fein,coment=@come,USER=@asd,dia=now() " +
                        "where idmovim=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.Parameters.AddWithValue("@tipe", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@fein", dtp_ingreso.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@come", tx_comen.Text.Trim());
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.ExecuteNonQuery();
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
        private bool anula()                                // anula el contrato
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                // despues de pasar las validaciones se BORRA 22/09/2020
                string anu = "delete from movim where idmovim=@idr";
                MySqlCommand micon = new MySqlCommand(anu, conn);
                micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                micon.ExecuteNonQuery();
                anu = "update detaped set saldo=saldo+@can where trim(pedidoh)=@ped and trim(item)=@cod";
                micon = new MySqlCommand(anu, conn);
                micon.Parameters.AddWithValue("@can", int.Parse(tx_cant.Text));
                micon.Parameters.AddWithValue("@ped", tx_pedido.Text.Trim());
                micon.Parameters.AddWithValue("@cod", tx_item.Text.Trim());
                micon.ExecuteNonQuery();
                //
                micon.Dispose();
                retorna = true;
            }
            conn.Close();
            return retorna;
        }
        private bool valexist(string docu)                  // valida existencia del pedido
        {
            bool retorna = true;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string consulta = "select count(a.id) from pedidos a " +
                    "left join detaped b on b.pedidoh=a.codped " +
                    "where trim(a.codped)=@doc and b.saldo>0 and a.status<>'ANULAD'";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@doc", docu.Trim());
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                        else
                        {
                            MessageBox.Show("No existe el pedido ingresado, esta anulado" + Environment.NewLine +
                                "o el pedido no tiene saldo", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            tx_pedido.Text = "";
                            tx_dat_ped.Text = "";
                            retorna = false;
                        }
                    }
                    dr.Close();
                }
            }
            conn.Close();
            return retorna;
        }
        private void jalaped(string pedi)                   // jala y muestra datos del pedido
        {

            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string consulta = "select a.codped,a.origen,a.destino,trim(cl.razonsocial) as cliente," +
                    "b.cant,b.item,b.nombre,b.medidas,b.madera,b.estado,b.precio,b.total," +
                    "ifnull(m.descrizionerid,'') as nomad,ifnull(e.descrizionerid,'') as acabado," +
                    "ifnull(o.descrizionerid,'') as nomorig,ifnull(d.descrizionerid,'') as nomdestin,a.contrato " +
                    "from pedidos a left join detaped b on b.pedidoh=a.codped " +
                    "left join desc_mad m on m.idcodice=b.madera " +
                    "left join desc_est e on e.idcodice=b.estado " +
                    "left join desc_loc o on o.idcodice=a.origen " +
                    "left join desc_alm d on d.idcodice=a.destino " +
                    "left join anag_cli cl on cl.idanagrafica=a.cliente " +
                    "where a.codped=@doc and a.tipoes=@tip";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@doc", pedi);
                micon.Parameters.AddWithValue("@tip", tipedc);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        tx_dat_ped.Text = dr.GetString(0);              // para las validaciones con F1
                        tx_cliente.Text = dr.GetString(3);
                        tx_origen.Text = dr.GetString(14);
                        tx_dat_orig.Text = dr.GetString(1);
                        tx_dest.Text = dr.GetString(15);
                        tx_dat_dest.Text = dr.GetString(2);
                        tx_item.Text = dr.GetString(5);
                        tx_nombre.Text = dr.GetString(6);
                        tx_medidas.Text = dr.GetString(7);
                        tx_nomad.Text = dr.GetString(12);
                        tx_dat_mad.Text = dr.GetString(8);
                        tx_acabad.Text = dr.GetString(13);
                        tx_dat_aca.Text = dr.GetString(9);
                        tx_cant.Text = dr.GetString(4);
                        tx_dat_cant.Text = dr.GetString(4);
                        tx_precio.Text = dr.GetString(10);
                        tx_total.Text = dr.GetString(11);
                        tx_contrato.Text = dr.GetString(16);
                    }
                    dr.Close();
                }
            }
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

        #region autocompletados
        private void autodptos()
        {
            // nada
        }
        private void autoprovi()
        {
            //
        }
        private void autodistr()
        {
            //
        }
        #endregion autocompletados
        #region botones_de_comando_y_permisos  
        private void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            bt_exc.Visible = false;
            bt_prev.Visible = false;
            Bt_print.Visible = false;
            bt_view.Visible = false;
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
                if (Convert.ToString(row["btn3"]) == "S")               // imprimir
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")               // anular
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")               // preview
                {
                    bt_prev.Visible = true;
                }
                else { bt_prev.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")               // salir del form
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
                if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }
                if (Convert.ToString(row["btn8"]) == "S")               // exporta xlsx
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
            }
        }
        private void Bt_add_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            sololee(this);
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            dtp_ingreso.Value = DateTime.Now;
            limpiar(this);
            //
            tx_dat_tiped.Text = tipede;
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_pedido.Enabled = true;
            dtp_ingreso.Enabled = true;
            //cmb_tipo.Enabled = true;
            tx_cant.Enabled = true;
            tx_cant.ReadOnly = false;
            tx_comen.Enabled = true;
            tx_pedido.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            Tx_modo.Text = "EDITAR";
            sololee(this);
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            tx_pedido.Enabled = true;
            tx_pedido.ReadOnly = false;
            tx_comen.ReadOnly = false;
            tx_comen.Enabled = true;
            dtp_ingreso.Enabled = true;
            //cmb_tipo.Enabled = true;
            tx_pedido.Focus();
            //
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            Tx_modo.Text = "ANULAR";
            sololee(this);
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            tx_pedido.Enabled = true;
            tx_pedido.ReadOnly = false;
            tx_pedido.Focus();
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            sololee(this);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;    // Image.FromFile(img_grab);
            limpiar(this);
            tx_pedido.Enabled = true;
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            jalaoc("tx_idr");
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            //setParaCrystal();
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text != "" && tx_rind.Text != "")
            {
                //setParaCrystal();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Ingresos_pedidos_clientes_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                wb.Worksheets.Add(dtg, "Ingresos");
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
        private void limpia_chk()
        {
            //checkBox1.Checked = false;
        }
        private void limpia_otros(TabPage pag)
        {
            //this.checkBox1.Checked = false;
        }
        private void limpia_combos(TabPage pag)
        {
            cmb_tipo.SelectedIndex = -1;
        }
        #endregion limpiadores_modos;
        #region comboboxes
        private void cmb_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tiped.Text = tipede; //cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        #endregion comboboxes
        #region boton_form GRABA EDITA ANULA - agrega detalle
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            // string modos = "NUEVO,EDITAR";
            if (true)   // modos.Contains(Tx_modo.Text)
            {
                if (tx_dat_tiped.Text == "")
                {
                    MessageBox.Show("Seleccione el tipo de ingreso", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_tipo.Focus();
                    return;
                }
                if (tx_cant.Text.Trim() == "" || tx_cant.Text.Trim() == "0")
                {
                    MessageBox.Show("La cantidad debe ser mayor a cero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tx_cant.Focus();
                    return;
                }
                if (tx_pedido.Text.Trim() == "")
                {
                    MessageBox.Show("Ingrese el pedido!","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    tx_pedido.Focus();
                    return;
                }
                if (tx_item.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione correctamente el pedido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_pedido.Focus();
                    return;
                }
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            //
            if (modo == "NUEVO")
            {
                var aa = MessageBox.Show("Confirma que desea crear el ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (graba() == true)
                    {
                        // a.idmovim,a.fechain,a.tipoes,a.origen,a.destino,a.pedido,a.cliente,a.coment,
                        // a.cant,a.articulo,a.med1,b.descrizionerid as nomad,c.descrizionerid as acabado,a.precio,a.total,
                        // a.madera,a.estado,nomorig,nomdestin,nombre
                        DataRow dr = dtg.NewRow();
                        string cid = tx_idr.Text;
                        dr[0] = cid;
                        dr[1] = dtp_ingreso.Value.ToString("dd/MM/yyyy");
                        dr[2] = tx_dat_tiped.Text;
                        dr[3] = tx_dat_orig.Text;
                        dr[4] = tx_dat_dest.Text;
                        dr[5] = tx_pedido.Text;
                        dr[6] = tx_cliente.Text.Trim();
                        dr[7] = tx_comen.Text.Trim();
                        dr[8] = tx_cant.Text;
                        dr[9] = tx_item.Text;
                        dr[10] = tx_medidas.Text;
                        dr[11] = tx_nomad.Text;
                        dr[12] = tx_acabad.Text;
                        dr[13] = tx_precio.Text;
                        dr[14] = tx_total.Text;
                        dr[15] = tx_dat_mad.Text;
                        dr[16] = tx_dat_aca.Text;
                        dr[17] = tx_origen.Text;
                        dr[18] = tx_dest.Text;
                        dr[19] = tx_nombre.Text;
                        dr[22] = tx_dat_fechp.Text;
                        dtg.Rows.Add(dr);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo grabar el ingreso", "Error en crear", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    // vista previa
                    //setParaCrystal();
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                // aca validamos que la cantidad ingresada (editada) no sea mayor a la pedida
                bool pasa = false;
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string busca = "select cant from detaped where pedidoh=@pedi and item=@item";
                        using (MySqlCommand micon = new MySqlCommand(busca, conn))
                        {
                            string cod = tx_item.Text.Trim();
                            micon.Parameters.AddWithValue("@pedi", tx_pedido.Text.Trim());
                            micon.Parameters.AddWithValue("@item", cod);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                int vc = 0;
                                if (dr.Read())
                                {
                                    vc = dr.GetInt32(0);
                                    if (int.Parse(tx_cant.Text) > vc)
                                    {
                                        MessageBox.Show("La cantidad ingresada es mayor al pedido", "Error - corrija",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        tx_cant.Focus();
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
                        MessageBox.Show("No fue posible conectarse al servidor", "Error de conexión");
                        return;
                    }
                }
                if (pasa == false) return;
                var aa = MessageBox.Show("Confirma que desea MODIFICAR el ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                                dtg.Rows[i][1] = dtp_ingreso.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][2] = tx_dat_tiped.Text;
                                dtg.Rows[i][7] = tx_comen.Text.Trim();
                            }
                        }
                    }
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "ANULAR")       // se anula o borra o ambos ??????? quedamos con Gloria que se BORRA 18/09/2020
            {
                // validar que no tenga salida este ingreso
                if (int.Parse(tx_saldo.Text) == 0 || int.Parse(tx_saldo.Text) < int.Parse(tx_cant.Text))
                {
                    MessageBox.Show("El ingreso tiene documento enlazado" + Environment.NewLine +
                        "debe anular primero la salida o salidas", "No puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                var aa = MessageBox.Show("Confirma que desea ANULAR el ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if(anula() != true)
                    {
                        MessageBox.Show("No se realizo la operacion de anular", "Error en anular");
                        return;
                    }
                    // actualizamos el datatable
                    for (int i = 0; i < dtg.Rows.Count; i++)
                    {
                        DataRow row = dtg.Rows[i];
                        if (row[0].ToString() == tx_idr.Text)
                        {
                            row.Delete();
                        }
                    }
                    dtg.AcceptChanges();
                }
            }
            // actualizamos el estado del contrato
            acciones acc = new acciones();
            acc.act_cont(tx_contrato.Text,"");
            if (iserror == "no")
            {
                // debe limpiar los campos y actualizar la grilla
                string zz = tx_dat_tiped.Text; 
                limpiar(this);
                tx_dat_tiped.Text = zz;
                dtp_ingreso.Value = DateTime.Now;
                cmb_tipo.Focus();
            }
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
        private void tx_pedido_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" && tx_pedido.Text != "")
            {
                // SOLO SE PERMITE CON F1
                /*if (valexist(tx_pedido.Text) == true && tx_dat_ped.Text.Trim() != tx_pedido.Text.Trim())
                {
                    // jalamos los datos del pedido y mostramos
                    jalaped(tx_pedido.Text);
                }*/ 
            }
            if (Tx_modo.Text != "NUEVO" && tx_pedido.Text != "" && tx_idr.Text == "")
            {
                jalaoc("tx_pedido");                        // jalamos los datos
            }
        }
        private void tx_cant_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_cant.Text.Trim() != "" && tx_dat_cant.Text.Trim() != "")
                {
                    if(int.Parse(tx_cant.Text) <= 0)
                    {
                        MessageBox.Show("La cantidad debe ser mayor a cero", "Atención - corrija");
                        tx_cant.Focus();
                    }
                    if (int.Parse(tx_cant.Text) > int.Parse(tx_dat_cant.Text))
                    {
                        MessageBox.Show("La cantidad no debe ser mayor a la pedida", "Atención - corrija");
                        tx_cant.Focus();
                    }
                }
            }
        }
        private void dtp_ingreso_Leave(object sender, EventArgs e)
        {
            if (tx_dat_fechp.Text.Trim() == "")
            {
                MessageBox.Show("Primero debe seleccionar el pedido!","Atención",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                dtp_ingreso.Value = DateTime.Now;
                tx_pedido.Focus();
                return;
            }
            else
            {
                if (dtp_ingreso.Value.Date < Convert.ToDateTime(tx_dat_fechp.Text).Date)
                {
                    MessageBox.Show("La fecha de ingreso no puede ser" + Environment.NewLine +
                        "menor a la fecha del pedido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtp_ingreso.Value = DateTime.Now;
                    dtp_ingreso.Focus();
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
                limpiar(this);
                tx_idr.Text = idr;
                tx_rind.Text = rind;
                tx_dat_tiped.Text = tipede;
                jalaoc("tx_idr");
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString() != e.FormattedValue.ToString()
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
                        // id,codped,status,origen,destino,fecha,entrega,coment,tipoes
                        // valida si el dato ingresado es valido en la columna
                        if (e.ColumnIndex == 4)                         // valida almacen destino
                        {
                            if (lib.validac("desc_alm", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para almacen", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 5)           // fecha
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
                        if (e.ColumnIndex == 8)          // 
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
    }
}
