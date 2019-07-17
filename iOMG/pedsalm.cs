using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace iOMG
{
    public partial class Pedsalm : Form
    {
        static string nomform = "pedidos";      // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "pedidos";
        public int totfilgrid, cta, cuenta, pageCount;      // variables para impresion
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
        string tiesta = "";             // estado inicial por defecto del pedido
        string escambio = "";           // estados de pedido que admiten modif el pedido
        string estpend = "";            // estado de pedido con articulos pendientes de recibir
        string estcomp = "";            // estado de pedido con articulos recibidos en su totalidad
        string estenv = "";             // estado de pedido enviado a producción
        string estanu = "";             // estado de pedido anulado
        string estcer = "";             // estado de pedido cerrado tal como esta, ya no se atiende
        string canovald2 = "";          // captitulos donde no se valida det2
        string conovald2 = "";          // valor por defecto al no validar det2
        string letpied = "";            // letra identificadora de piedra en detalle2
        string estman = "";             // estados que se pueden seleccionar manualmente
        int indant = -1;                // indice anterior al cambio en el combobox de estado
        //string cn_adm = "";     // codigo nivel usuario admin
        //string cn_sup = "";     // codigo nivel usuario superusuario
        //string cn_est = "";     // codigo nivel usuario estandar
        //string cn_mir = "";     // codigo nivel usuario solo mira
        string cliente = Program.cliente;    // razon social para los reportes
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();    // dtg primario, original con la carga del 
        DataTable dttaller = new DataTable();   // combo taller de fabric.

        public Pedsalm()
        {
            InitializeComponent();
        }
        private void users_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1 && Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
            {
                if(cmb_fam.Focused == true || cmb_mod.Focused == true || cmb_mad.Focused == true || cmb_tip.Focused == true ||
                    cmb_det1.Focused == true || cmb_aca.Focused == true || cmb_tal.Focused == true ||
                    cmb_det2.Focused == true || cmb_det3.Focused == true)
                {
                    para1 = "items";
                    para2 = "todos";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue1;
                            //ayu2.ReturnValue0;
                            //ayu3.ReturnValue2;
                            cmb_fam.SelectedIndex = cmb_fam.FindString(ayu2.ReturnValue1.Substring(0, 1));
                            cmb_mod.SelectedIndex = cmb_mod.FindString(ayu2.ReturnValue1.Substring(1, 3));
                            cmb_mad.SelectedIndex = cmb_mad.FindString(ayu2.ReturnValue1.Substring(4, 1));
                            cmb_mad_SelectionChangeCommitted(null, null);
                            cmb_tip.SelectedIndex = cmb_tip.FindString(ayu2.ReturnValue1.Substring(5, 2));
                            cmb_det1.SelectedIndex = cmb_det1.FindString(ayu2.ReturnValue1.Substring(7, 2));
                            cmb_det1_SelectionChangeCommitted(null, null);
                            cmb_aca.SelectedIndex = cmb_aca.FindString(ayu2.ReturnValue1.Substring(9, 1));
                            cmb_aca_SelectionChangeCommitted(null, null);
                            if(tx_dat_orig.Text == "") cmb_tal.SelectedIndex = cmb_tal.FindString(ayu2.ReturnValue1.Substring(10, 2));
                            cmb_det2.SelectedIndex = cmb_det2.FindString(ayu2.ReturnValue1.Substring(12, 3));
                            cmb_det2_SelectionChangeCommitted(null, null);
                            cmb_det3.SelectedIndex = cmb_det3.FindString(ayu2.ReturnValue1.Substring(15, 3));
                            armani();
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void repspedidos_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            //grilla2();
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = false;            // LA ANULACION O CIERRE DE PEDIDO SE HACE CON "EDIT", BOTON PARA PRE-VISUALIZAR
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            //Bt_add_Click(null, null);
            //tabControl1.SelectedTab = tabgrilla;
            //advancedDataGridView1.Enabled = false;
            tabControl1.Enabled = false;
            cmb_tipo.Enabled = false;
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
            // longitudes maximas de campos
            tx_coment.MaxLength = 90;           // nombre
            tx_codped.CharacterCasing = CharacterCasing.Upper;
        }
        private void grilla()                   // arma la grilla
        {
            // a.id,a.codped,b.descrizionerid,a.origen,a.destino,fecha,entrega,a.coment,a.tipoes,a.status
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
            // tipo de pedido ==> situacion del pedido, status
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Sit.Ped";    // titulo de la columna
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
            // codigo tipo
            advancedDataGridView1.Columns[8].Visible = false;
            // codigo estado
            advancedDataGridView1.Columns[9].Visible = false;
        }
        private void jalainfo()                 // obtiene datos de imagenes
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
                        //if (row["param"].ToString() == "img_imprime") img_imprime = row["valor"].ToString().Trim();  // imagen del boton IMPRIMIR REPORTE
                    }
                    if (row["formulario"].ToString() == "pedidos")
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "almacen") tipede = row["valor"].ToString().Trim();         // tipo de pedido por defecto en almacen
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // estado del pedido inicial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "pendiente") estpend = row["valor"].ToString().Trim();         // estado del pedido con llegada parcial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "recibido") estcomp = row["valor"].ToString().Trim();         // estado del pedido con llegada total
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cambio") escambio = row["valor"].ToString().Trim();         // estado del pedido que admiten modificar el pedido
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "enviado") estenv = row["valor"].ToString().Trim();         // estado del pedido enviado a producción
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "anulado") estanu = row["valor"].ToString().Trim();         // estado del pedido anulado
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cerrado") estcer = row["valor"].ToString().Trim();         // estado del pedido cerrado asi como esta
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "manual") estman = row["valor"].ToString().Trim();         // estados que se pueden seleccionar manualmente
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "nodet2") canovald2 = row["valor"].ToString().Trim();         // captitulos donde no se valida det2
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "valdet2") conovald2 = row["valor"].ToString().Trim();         // valor por defecto al no validar det2
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
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
        public void jalaoc(string campo)        // jala datos de usuarios por id o nom_user
        {
            if (campo == "tx_idr" && tx_idr.Text != "")
            {   // id,codped,tipoes,origen,destino,fecha,entrega,coment
                // tx_idr.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // 
                tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // codigo pedido
                //tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();  // tipo pedido
                tx_dat_tiped.Text = tipede;
                tx_dat_estad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString();  // estado del pedido
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();   // taller origen
                tx_dat_dest.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();   // destino
                dtp_pedido.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString());   // fecha pedido
                dtp_entreg.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString());    // fecha entrega
                tx_coment.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();     // comentario
                //cmb_cap.SelectedValue = tx_dat_tiped.Text;
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                cmb_destino.SelectedIndex = cmb_destino.FindString(tx_dat_dest.Text);
                cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                //cmb_tip.SelectedValue = tx_dat_tip.Text;
                jaladet(tx_codped.Text);
            }
            if(campo == "tx_codped" && tx_codped.Text != "")
            {
                int cta = 0;
                foreach (DataRow row in dtg.Rows)
                {
                    if (row["codped"].ToString().Trim() == tx_codped.Text.Trim())
                    {
                        //id,codped,tipoes,origen,destino,fecha,entrega,coment
                        tx_dat_tiped.Text = tipede;
                        tx_idr.Text = row["id"].ToString();            // id del registro
                        tx_rind.Text = cta.ToString();
                        //tx_dat_tiped.Text = row["tipoes"].ToString();  // tipo pedido
                        tx_dat_estad.Text = row["status"].ToString();   // estado del pedido
                        tx_dat_orig.Text = row["origen"].ToString();   // taller origen
                        tx_dat_dest.Text = row["destino"].ToString();   // destino
                        dtp_pedido.Value = Convert.ToDateTime(row["fecha"].ToString());   // fecha pedido
                        dtp_entreg.Value = Convert.ToDateTime(row["entrega"].ToString());    // fecha entrega
                        tx_coment.Text = row["coment"].ToString();     // comentario
                        cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                        cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                        cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                        cmb_destino.SelectedIndex = cmb_destino.FindString(tx_dat_dest.Text);
                        jaladet(tx_codped.Text);
                    }
                    cta = cta + 1;
                }
            }
        }
        private void jaladet(string pedido)                 // jala el detalle del pedido
        {
            // id,cant,item,nombre,medidas,madera,detalle2,acabado,comentario,estado,.....
            string jalad = "select a.iddetaped,a.cant,a.item,a.nombre,a.medidas,c.descrizionerid,d.descrizionerid," +
                "b.descrizionerid,a.coment,a.estado,a.madera,a.piedra,DATE_FORMAT(fingreso,'%d/%m/%Y'),a.saldo " +
                "from detaped a " +
                "left join desc_est b on b.idcodice=a.estado " +
                "left join desc_mad c on c.idcodice=a.madera " +
                "left join desc_dt2 d on d.idcodice=a.piedra " +
                "where a.pedidoh=@pedi";
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener detalle del pedido");
                Application.Exit();
                return;
            }
        }
        private void grilladet(string modo)                 // grilla detalle de pedido
        {   
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 14;
            // id 
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho
            dataGridView1.Columns[0].HeaderText = "Id";         // titulo de la columna
            dataGridView1.Columns[0].Name = "iddetaped";
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
            // codigo madera
            dataGridView1.Columns[10].Visible = false;
            // codigo detalle 2
            dataGridView1.Columns[11].Visible = true;   // false
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
        }
        public void dataload(string quien)                  // jala datos para los combos y la grilla
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
                string datgri = "select a.id,a.codped,b.descrizionerid,a.origen,a.destino,date_format(date(a.fecha),'%Y-%m-%d') as fecha," +
                    "date_format(date(a.entrega),'%Y-%m-%d') as entrega,a.coment,a.tipoes,a.status " +
                    "from pedidos a left join desc_stp b on b.idcodice=a.status where a.tipoes=@tip";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                cdg.Parameters.AddWithValue("@tip", tipede);                // "TPE001"
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
                    "where numero=1 order by idcodice";
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
        string[] equivinter(string titulo)        // equivalencia entre titulo de columna y tabla 
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
        private void armani()                               // arma el codigo y busca en la maestra
        {
            string fam = "", mod = "", mad = "", tip = "", de1 = "", aca = "", tal = "", de2 = "", de3 = "";
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
            if (fam != "" && mod != "" && tip != "" && de1 != "" && aca != "" && de2 != "" && de3 != "")
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        if (conn.State == ConnectionState.Open)
                        {
                            //string busca = "select id,nombr,medid,umed,soles2018 from items where codig=@cod";
                            string busca = "select id,nombr,medid,umed,soles2018,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3 " +
                                "from items where capit=@fam and model=@mod and tipol=@tip and deta1=@dt1"; // and deta3=@dt3
                            MySqlCommand micon = new MySqlCommand(busca, conn);
                            //micon.Parameters.AddWithValue("@cod", codbs);
                            micon.Parameters.AddWithValue("@fam", fam);
                            micon.Parameters.AddWithValue("@mod", mod);
                            micon.Parameters.AddWithValue("@tip", tip);
                            micon.Parameters.AddWithValue("@dt1", de1);
                            //micon.Parameters.AddWithValue("@dt3", de3);
                            //MySqlDataReader dr = micon.ExecuteReader();
                            MySqlDataAdapter da = new MySqlDataAdapter(micon);
                            DataTable dtm = new DataTable();
                            da.Fill(dtm);
                            if (dtm.Rows.Count == 0)
                            {
                                MessageBox.Show("No existe en la base de datos!0", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                tx_d_nom.Text = "";
                                tx_d_med.Text = "";
                                tx_d_mad.Text = "";
                                tx_d_det2.Text = "";
                                tx_d_est.Text = "";
                                return;
                            }
                            string gol = "";
                            for (int i = 0; i < dtm.Rows.Count; i++)
                            {
                                DataRow fila = dtm.Rows[i];
                                if (fila["mader"].ToString() == mad && fila["acaba"].ToString() == aca &&
                                    fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3)
                                {
                                    tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                    tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                    gol = "1";
                                    break;
                                }
                            }
                            if (gol == "")
                            {
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    DataRow fila = dtm.Rows[i]; // fila["mader"].ToString() == "X" && 
                                    if (fila["acaba"].ToString() == aca &&
                                    fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                        tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                        gol = "1";
                                        break;
                                    }   // (fila["mader"].ToString() == "X" || ... fila["mader"].ToString() == mad && 
                                    if (fila["acaba"].ToString() == aca &&
                                    fila["deta2"].ToString().Substring(0, 1) == letpied && fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                        tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                        gol = "1";
                                        break;
                                    }
                                    /*
                                    if (fila["mader"].ToString() != "X" && fila["acaba"].ToString() != "X" &&
                                    fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                        tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                        gol = "1";
                                        break;
                                    }
                                    if (fila["mader"].ToString() == "X" && fila["acaba"].ToString() == "X" &&
                                    fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                        tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                        gol = "1";
                                        break;
                                    }
                                    */
                                }
                            }
                            if(gol == "")
                            {
                                MessageBox.Show("No existe en la base de datos!1", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                tx_d_nom.Text = "";
                                tx_d_med.Text = "";
                                tx_d_mad.Text = "";
                                tx_d_det2.Text = "";
                                tx_d_est.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se puede conectar a la base de datos", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error interno en codigo");
                    Application.Exit();
                    return;
                }
            }
        }
        private bool graba()                                // graba cabecera y detalle
        {
            bool retorna = false;
            string ncp = "";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string lee = "SELECT right(codped,length(codped)-2) FROM pedidos WHERE tipoes=@tpe ORDER BY id DESC LIMIT 1";
                    MySqlCommand comlee = new MySqlCommand(lee, conn);
                    comlee.Parameters.AddWithValue("@tpe", tipede);
                    MySqlDataReader dr = comlee.ExecuteReader();
                    if (dr.Read())
                    {
                        int np = dr.GetInt16(0) + 1;
                        ncp = "AL" + (np.ToString());
                        tx_codped.Text = ncp;
                    }
                    dr.Close();
                }
                catch(MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener # pedido");
                    Application.Exit();
                }
                try
                {
                    string inserta = "insert into pedidos (" +
                        "fecha,tipoes,origen,destino,coment,USER,dia,codped,status,entrega) values (" +
                        "@fepe,@tipe,@tall,@dest,@come,@asd,now(),@cope,@esta,@entr)";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@fepe", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@tipe", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@tall", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@dest", tx_dat_dest.Text);
                    micon.Parameters.AddWithValue("@come", tx_coment.Text);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                    micon.Parameters.AddWithValue("@esta", tx_dat_estad.Text);
                    micon.Parameters.AddWithValue("@entr", dtp_entreg.Value.ToString("yyyy-MM-dd"));
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
                    for (int i=0; i<dataGridView1.Rows.Count - 1; i++)
                    {
                        // a.iddetaped,a.cant,a.item,a.nombre,a.medidas,c.descrizionerid,d.descrizionerid,b.descrizionerid,a.coment,
                        // a.estado,a.madera,a.piedra,a.fingreso,a.saldo
                        string tfingreso = "", tfing = "";
                        if (dtp_fingreso.Checked == true)
                        {
                            tfingreso = ",fingreso";
                            tfing = ",@fing";
                        }
                        string insdet = "insert into detaped (" +
                            "pedidoh,tipo,item,cant,nombre,medidas,madera,estado,piedra,coment,saldo" + tfingreso + ") values (" +
                            "@cope,@tipe,@item,@cant,@nomb,@medi,@made,@esta,@det2,@come,@sald" + tfing + ")";
                        micon = new MySqlCommand(insdet, conn);
                        micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                        micon.Parameters.AddWithValue("@tipe", tx_dat_tiped.Text);
                        micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[2].Value.ToString());
                        micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[10].Value.ToString());   // 
                        micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[9].Value.ToString());   // 
                        micon.Parameters.AddWithValue("@det2", dataGridView1.Rows[i].Cells[11].Value.ToString());   // 
                        micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[8].Value.ToString());
                        if (dtp_fingreso.Checked == true) micon.Parameters.AddWithValue("@fing", dataGridView1.Rows[i].Cells[12].Value.ToString());
                        micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[13].Value.ToString());
                        micon.ExecuteNonQuery();
                    }
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
        private bool edita()                                // actualiza cabecera y detalle
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string actua = "update pedidos set " +
                        "fecha=@fepe,origen=@tall,destino=@dest,coment=@come,user=@asd,dia=now(),status=@esta,entrega=@entr " +
                        "where id=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.Parameters.AddWithValue("@fepe", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@tall", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@dest", tx_dat_dest.Text);
                    micon.Parameters.AddWithValue("@come", tx_coment.Text);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@esta", tx_dat_estad.Text);
                    micon.Parameters.AddWithValue("@entr", dtp_entreg.Value.ToString("yyyy-MM-dd"));
                    micon.ExecuteNonQuery();
                    // detalle
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string insdet = "";
                        if (Int16.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()) < 100) // las filas deben ser < 100
                        {
                            string tfingreso = "", tfing = "";
                            if (dtp_fingreso.Checked == true)
                            {
                                tfingreso = ",fingreso";
                                tfing = ",@fing";
                            }
                            insdet = "insert into detaped (" +
                                "pedidoh,tipo,item,cant,nombre,medidas,madera,estado,piedra,coment,saldo" + tfingreso + ") values (" +
                                "@cope,@tipe,@item,@cant,@nomb,@medi,@made,@esta,@det2,@come,@sald" + tfing + ")";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                            micon.Parameters.AddWithValue("@tipe", tx_dat_tiped.Text);
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[10].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[9].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@det2", dataGridView1.Rows[i].Cells[11].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            if (dtp_fingreso.Checked == true) micon.Parameters.AddWithValue("@fing", dataGridView1.Rows[i].Cells[12].Value.ToString());
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[13].Value.ToString());
                            micon.ExecuteNonQuery();
                        }
                        else
                        {
                            insdet = "update detaped set " +
                                "item=@item,cant=@cant,nombre=@nomb,medidas=@medi,madera=@made,estado=@esta,piedra=@det2,coment=@come,fingreso=@fing,saldo=@sald " +
                                "where iddetaped=@idr";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[0].Value.ToString());
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[10].Value.ToString());
                            micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[9].Value.ToString());
                            micon.Parameters.AddWithValue("@det2", dataGridView1.Rows[i].Cells[11].Value.ToString());
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            if (dataGridView1.Rows[i].Cells[12].Value.ToString() == "")
                            {
                                micon.Parameters.AddWithValue("@fing", DBNull.Value);
                            }
                            else
                            {
                                micon.Parameters.AddWithValue("@fing", Convert.ToDateTime(dataGridView1.Rows[i].Cells[12].Value.ToString()));
                            }
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[13].Value.ToString());
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
        public void sololeepag(TabPage pag)
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
        public void escribepag(TabPage pag)
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
        public void limpiapag(TabPage pag)
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
            tx_d_est.Text = "";
            tx_d_id.Text = "";
            tx_d_it.Text = "";
            tx_d_mad.Text = "";
            tx_d_med.Text = "";
            tx_d_nom.Text = "";
        }
        public void limpia_chk()
        {
            //checkBox1.Checked = false;
        }
        public void limpia_otros(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            //this.checkBox1.Checked = false;
        }
        public void limpia_combos(TabPage pag)
        {
            //tabControl1.SelectedTab = pag;
            cmb_taller.SelectedIndex = -1;
            cmb_destino.SelectedIndex = -1;
            cmb_estado.SelectedIndex = -1;
            cmb_fam.SelectedIndex = -1;
            cmb_mod.SelectedIndex = -1;
            cmb_mad.SelectedIndex = -1;
            cmb_tip.SelectedIndex = -1;
            cmb_det1.SelectedIndex = -1;
            cmb_aca.SelectedIndex = -1;
            cmb_tal.SelectedIndex = -1;
            cmb_det2.SelectedIndex = -1;
            cmb_det3.SelectedIndex = -1;
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA - agrega detalle
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            if (tx_dat_tiped.Text == "")
            {
                MessageBox.Show("Seleccione el tipo de pedido", "Atención - verifique",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                cmb_tipo.Focus();
                return;
            }
            if (tx_dat_estad.Text == "")
            {
                MessageBox.Show("Seleccione el estado del pedido", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_estado.Focus();
                return;
            }
            if(tx_dat_orig.Text == "")
            {
                MessageBox.Show("Seleccione el taller", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_taller.Focus();
                return;
            }
            if(tx_dat_dest.Text == "")
            {
                MessageBox.Show("Seleccione el destino", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_destino.Focus();
                return;
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            if (modo == "NUEVO")
            {
                var aa = MessageBox.Show("Confirma que desea crear el pedido?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (graba() == true)
                    {
                        // insertamos en el datatable
                        DataRow dr = dtg.NewRow();
                        // a.id,a.codped,b.descrizionerid,a.origen,a.destino,fecha,entrega,a.coment,a.tipoes,a.status
                        string cid = tx_idr.Text;   //"0";   // sería bueno que fuera el id real  
                        dr[0] = cid;
                        dr[1] = tx_codped.Text; 
                        dr[2] = cmb_estado.SelectedItem.ToString().Substring(9, 6); // tx_dat_estad.Text;
                        dr[3] = tx_dat_orig.Text;
                        dr[4] = tx_dat_dest.Text;
                        dr[5] = dtp_pedido.Value.ToString("yyy-MM-dd");
                        dr[6] = dtp_entreg.Value.ToString("yyy-MM-dd");
                        dr[7] = tx_coment.Text;
                        dr[8] = tx_dat_tiped.Text;
                        dr[9] = tx_dat_estad.Text;

                        dtg.Rows.Add(dr);
                        //dtu.Rows.Add(dr); da error porque la fila ya pertenece a otra tabla
                    }
                    else
                    {
                        MessageBox.Show("No se pudo grabar el pedido", "Error en crear", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    // vista previa
                    //pageCount = 1;
                    //printDocument1.DefaultPageSettings.Landscape = true;
                    //printPreviewDialog1.Document = printDocument1;
                    //printPreviewDialog1.ShowDialog();
                    // impresion sin vista previa, solicitud de gloria 03/07/2019
                    PrintDialog printDlg = new PrintDialog();
                    printDlg.Document = printDocument1;
                    printDlg.AllowSomePages = true;
                    printDlg.AllowSelection = true;
                    pageCount = 1;
                    printDocument1.DefaultPageSettings.Landscape = true;
                    if (printDlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                var aa = MessageBox.Show("Confirma que desea MODIFICAR el pedido?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if(tx_dat_estad.Text == estanu || tx_dat_estad.Text == estcer)    // si estado actual es anulado o cerrado
                    {
                        // se cierra o anula, no se calcula el estado
                    }
                    else
                    {
                        // calcula el estado del pedido segun el saldo de muebles
                        int vasa = 0;   // saldo
                        int vaca = 0;   // cantidad
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            vasa = vasa + Int16.Parse(dataGridView1.Rows[i].Cells[13].Value.ToString());
                            vaca = vaca + Int16.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        }
                        if (vasa > 0 && vasa < vaca) tx_dat_estad.Text = estpend;    // atendido parcial
                        if (vasa > 0 && vasa >= vaca) tx_dat_estad.Text = estenv;   // sin atender
                        if (vasa <= 0) tx_dat_estad.Text = estcomp;                  // atendido total
                        cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                    }
                    //
                    if (edita() == true)
                    {
                        // actualizamos el datatable
                        for (int i = 0; i < dtg.Rows.Count; i++)
                        {
                            DataRow row = dtg.Rows[i];
                            if (row[0].ToString() == tx_idr.Text)
                            {
                                // a.id,a.codped,b.descrizionerid,a.origen,a.destino,fecha,entrega,a.coment,a.tipoes,a.status
                                dtg.Rows[i][2] = cmb_estado.SelectedItem.ToString().Substring(9, 6);    // tx_dat_estad.Text;
                                dtg.Rows[i][3] = tx_dat_orig.Text;
                                dtg.Rows[i][4] = tx_dat_dest.Text;
                                dtg.Rows[i][5] = dtp_pedido.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][6] = dtp_entreg.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][7] = tx_coment.Text;
                                dtg.Rows[i][8] = tx_dat_tiped.Text;
                                dtg.Rows[i][9] = tx_dat_estad.Text;
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
            if (modo == "ANULAR")       // opción para borrar
            {
                // en modo edicion se anula o cierra
            }
            if (iserror == "no")
            {
                // debe limpiar los campos y actualizar la grilla
                limpiar(this);
                limpiapag(tabuser);
                limpia_otros(tabuser);
                limpia_combos(tabuser);
                dtp_entreg.Value = DateTime.Now;
                dtp_pedido.Value = DateTime.Now;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                cmb_tipo.Focus();
            }
        }
        private void bt_det_Click(object sender, EventArgs e)
        {
            if(tx_d_nom.Text == "")
            {
                MessageBox.Show("El código no existe en la maestra", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if(tx_d_can.Text == "")
            {
                MessageBox.Show("Falta la cantidad de muebles", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_can.Focus();
                return;
            }
            if (tx_d_mad.Text.Trim() == "")
            {
                MessageBox.Show("Confirme el tipo de madera", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mad.Focus();
                return;
            }
            if(tx_d_det2.Text.Trim() == "")
            {
                MessageBox.Show("Confirme el detalle 2", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det2.Focus();
                return;
            }
            if(tx_d_est.Text.Trim() == "")
            {
                MessageBox.Show("Confirme el acabado", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_aca.Focus();
                return;
            }
            if(cmb_det3.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 3", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det3.Focus();
                return;
            }
            if (cmb_det2.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 2", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det2.Focus();
                return;
            }
            if (cmb_tal.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el taller", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_tal.Focus();
                return;
            }
            if (cmb_aca.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el acabado", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_aca.Focus();
                return;
            }
            if (cmb_det1.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 1", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det1.Focus();
                return;
            }
            if (cmb_tip.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la tipología", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_tip.Focus();
                return;
            }
            if (cmb_mad.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la madera", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mad.Focus();
                return;
            }
            if (cmb_mod.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el modelo", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mod.Focus();
                return;
            }
            if (cmb_fam.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el capitulo o familia", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_fam.Focus();
                return;
            }
            // valida que no existan X en madera y acabado, y no exista XX en taller
            if (cmb_mad.SelectedItem.ToString().Substring(0,1) == "X")
            {
                MessageBox.Show("Seleccione un tipo de madera valido", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mad.Focus();
                return;
            }
            if (cmb_aca.SelectedItem.ToString().Substring(0, 1) == "X")
            {
                MessageBox.Show("Seleccione el acabado correcto", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_aca.Focus();
                return;
            }
            if (cmb_tal.SelectedItem.ToString().Substring(0, 2) == "XX")
            {
                MessageBox.Show("Seleccione un taller!", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_tal.Focus();
                return;
            }
            // fin de las validaciones de X
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_d_id.Text.Trim() != "")    //  dataGridView1.Rows.Count > 1
                {
                    //a.iddetaped,a.cant,a.item,a.nombre,a.medidas,c.descrizionerid,d.descrizionerid,
                    //b.descrizionerid,a.coment,a.estado,a.madera,a.piedra,a.fingreso,a.saldo
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                    obj.Cells[1].Value = tx_d_can.Text;
                    obj.Cells[2].Value = tx_d_codi.Text;
                    obj.Cells[3].Value = tx_d_nom.Text;
                    obj.Cells[4].Value = tx_d_med.Text;
                    obj.Cells[5].Value = tx_d_mad.Text;
                    obj.Cells[6].Value = tx_d_det2.Text;
                    obj.Cells[7].Value = tx_d_est.Text;
                    obj.Cells[8].Value = tx_d_com.Text;
                    obj.Cells[9].Value = cmb_aca.Tag.ToString();
                    obj.Cells[10].Value = cmb_mad.Tag.ToString();
                    obj.Cells[11].Value = cmb_det2.Tag.ToString();
                    if (dtp_fingreso.Checked == true) obj.Cells[12].Value = dtp_fingreso.Value.ToShortDateString();  // tx_fingreso.Text;
                    obj.Cells[13].Value = tx_saldo.Text;
                }
                else
                {
                    if (dataGridView1.Rows.Count < 100)
                    {
                        dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                             tx_d_mad.Text, tx_d_det2.Text, tx_d_est.Text, tx_d_com.Text, cmb_aca.Tag.ToString(),
                            cmb_mad.SelectedItem.ToString().Substring(0, 1), cmb_det2.SelectedItem.ToString().Substring(0, 3), "", tx_saldo.Text);
                    }
                    else
                    {
                        MessageBox.Show("Límite de filas por pedido alcanzado", "No se puede insertar mas filas",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            if (Tx_modo.Text == "EDITAR")
            {
                if (!escambio.Contains(tx_dat_estad.Text))
                {
                    MessageBox.Show("El estado actual del pedido no permite modificar el detalle",
                        "No puede continuar",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }
                if(tx_d_id.Text.Trim() != "")    //  dataGridView1.Rows.Count > 1
                {
                    //a.iddetaped,a.cant,a.item,a.nombre,a.medidas,c.descrizionerid,d.descrizionerid,
                    //b.descrizionerid,a.coment,a.estado,a.madera,a.piedra,a.fingreso,a.saldo
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                    obj.Cells[1].Value = tx_d_can.Text;
                    obj.Cells[2].Value = tx_d_codi.Text;
                    obj.Cells[3].Value = tx_d_nom.Text;
                    obj.Cells[4].Value = tx_d_med.Text;
                    obj.Cells[5].Value = tx_d_mad.Text;
                    obj.Cells[6].Value = tx_d_det2.Text;
                    obj.Cells[7].Value = tx_d_est.Text;
                    obj.Cells[8].Value = tx_d_com.Text;
                    obj.Cells[9].Value = cmb_aca.Tag.ToString();
                    obj.Cells[10].Value = cmb_mad.Tag.ToString();
                    obj.Cells[11].Value = cmb_det2.Text.Substring(0, 3);  // cmb_det2.Tag.ToString();
                    if (dtp_fingreso.Checked == true) obj.Cells[12].Value = dtp_fingreso.Value.ToShortDateString();  // tx_fingreso.Text;
                    obj.Cells[13].Value = tx_saldo.Text;
                }
                else
                {
                    DataTable dtg = (DataTable)dataGridView1.DataSource;
                    dtg.Rows.Add(dataGridView1.Rows.Count, tx_d_can.Text, tx_d_codi.Text, tx_d_nom.Text, tx_d_med.Text,
                        tx_d_mad.Text, tx_d_det2.Text, tx_d_est.Text, tx_d_com.Text, cmb_aca.Tag.ToString(),
                        cmb_mad.SelectedItem.ToString().Substring(0, 1), cmb_det2.SelectedItem.ToString().Substring(0, 3), "", tx_saldo.Text);
                }
            }
            tx_d_nom.Text = "";
            tx_d_can.Text = "";
            tx_d_com.Text = "";
            tx_d_med.Text = "";
            tx_d_mad.Text = "";
            tx_d_det2.Text = "";
            tx_d_est.Text = "";
            tx_d_id.Text = "";
            tx_d_codi.Text = "";
            //tx_fingreso.Text = "";
            tx_saldo.Text = "";
            //limpia_combos(tabuser);
            cmb_fam.SelectedIndex = -1;
            cmb_mod.SelectedIndex = -1;
            cmb_mad.SelectedIndex = -1;
            cmb_tip.SelectedIndex = -1;
            cmb_det1.SelectedIndex = -1;
            cmb_aca.SelectedIndex = -1;
            //cmb_tal.SelectedIndex = -1;
            cmb_det2.SelectedIndex = -1;
            cmb_det3.SelectedIndex = -1;
        }
        #endregion boton_form;

        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")    //  && tx_idr.Text != ""
            {
                //string aca = tx_idr.Text;
                //limpia_chk();
                //limpia_combos();
                //limpiar(this);
                //tx_idr.Text = aca;
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {

        }
        private void tx_rind_Leave(object sender, EventArgs e)
        {

        }
        private void tx_codped_Leave(object sender, EventArgs e)
        {
            if(Tx_modo.Text != "NUEVO" && tx_codped.Text != "")
            {
                jalaoc("tx_codped");
            }
        }
        private void tx_d_can_Leave(object sender, EventArgs e)
        {
            tx_saldo.Text = tx_d_can.Text;
        }
        #endregion leaves;

        #region botones_de_comando_y_permisos  
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
                    consulb.Parameters.AddWithValue("@nomform", "pedsalm");
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
                if (Convert.ToString(row["btn3"]) == "S")               // anular
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")               // visualizar
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
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
            cmb_estado.SelectedIndex = cmb_estado.FindString(tiesta);
            tx_dat_estad.Text = tiesta;
            cmb_estado.Enabled = false;
            tx_codped.ReadOnly = true;
            dtp_fingreso.Checked = false;
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
            cmb_estado.Enabled = true;
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            // nada que hacer
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
            PrintDialog printDlg = new PrintDialog();
            printDlg.Document = printDocument1;
            printDlg.AllowSomePages = true;
            printDlg.AllowSelection = true;
            //
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            //
            if (printDlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text != "" && tx_rind.Text != "")
            {
                Tx_modo.Text = "IMPRIMIR";
                pageCount = 1;
                printDocument1.DefaultPageSettings.Landscape = true;
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Pedidos_almacen_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
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
        // permisos para habilitar los botones de comando
        /*private void permisos()
        {
            string consulta = "select formulario,nivel,coment,btn1,btn2,btn3,btn4,btn5,btn6 from setupform";
            DataTable dt = new DataTable();
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(consulta, conn);
                    da.Fill(dt);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión a setupform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
            string bot1 = "N";
            string bot2 = "N";
            string bot3 = "N";
            string bot4 = "N";
            string bot5 = "N";
            string bot6 = "S";
            string com = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow fil = dt.Rows[i];
                if (fil[1].ToString() == "0")
                { // usuarios de sistemas, acceso total a todo

                }
                if (fil[1].ToString() == "1")
                {   // usuario directivo, acceso de usuario avanzado

                }
                if (fil[1].ToString() == "2")
                {   // usuario secretarias, usuario normal

                }
                com = fil[2].ToString();    // comentario - descripcion del form
            }
            conn.Close();
        }*/
        // configurador de permisos
        #endregion botones_de_comando_y_permisos  ;

        #region comboboxes
        private void cmb_estado_Click(object sender, EventArgs e)
        {
            indant = cmb_estado.SelectedIndex;
            //MessageBox.Show(indant.ToString());
        }
        private void cmb_estado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // en edicion solo debe permitir CERRAR, ANULAR o (GRABADO PARA PODER EDITAR EL DETALLE)
            if (!estman.Contains(tx_dat_estad.Text))
            {
                MessageBox.Show("No puede realizar este cambio de estado", "Atención - corrija", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tx_dat_estad.Text = "";
                cmb_estado.SelectedIndex = indant;
                cmb_estado.Focus();
            }
            else
            {
                if (cmb_estado.SelectedValue != null) tx_dat_estad.Text = cmb_estado.SelectedValue.ToString();
                else tx_dat_estad.Text = cmb_estado.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            }
        }
        private void cmb_destino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_destino.SelectedValue != null) tx_dat_dest.Text = cmb_destino.SelectedValue.ToString();
            else tx_dat_dest.Text = cmb_destino.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            if(Tx_modo.Text == "NUEVO")
            {
                string cod2d = "";
                foreach (DataRow row in dttaller.Rows)
                {
                    if (row["idcodice"].ToString().Trim() == tx_dat_orig.Text.Trim())
                    {
                        cod2d = row["codigo"].ToString();
                    }
                }
                cmb_tal.Tag = cod2d;
                cmb_tal.SelectedIndex = cmb_tal.FindString(cmb_tal.Tag.ToString());
            }
        }
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tiped.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
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
            tx_d_mad.Text = cmb_mad.SelectedItem.ToString().Substring(4, cmb_mad.SelectedItem.ToString().Length - 4).Trim();
            //tx_d_mad.Text = cmb_mad.SelectedItem.ToString().Substring(0, 1);
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
            tx_d_est.Text = cmb_aca.SelectedItem.ToString().Substring(5, cmb_aca.SelectedItem.ToString().Length - 5).Trim();
            cmb_aca.Tag = cmb_aca.SelectedItem.ToString().Substring(0, 1);
            armani();
        }
        private void cmb_tal_SelectedIndexChanged(object sender, EventArgs e)           // taller
        {
            armani();
        }
        private void cmb_det2_SelectionChangeCommitted(object sender, EventArgs e)      // detalle 2
        {
            if (cmb_det2.SelectedIndex == -1) tx_d_det2.Text = "";
            else tx_d_det2.Text = cmb_det2.SelectedItem.ToString().Substring(6, cmb_det2.SelectedItem.ToString().Length - 6).Trim();
            armani();
        }
        private void cmb_det3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            armani();
        }
        #endregion comboboxes

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
                        // id,codped,status,origen,destino,fecha,entrega,coment,tipoes
                        // valida si el dato ingresado es valido en la columna
                        if (e.ColumnIndex == 2)                         // valida estado del pedido
                        {
                            if (lib.validac("desc_stp", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para el estado", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 3)                         // valida taller de origen
                        {
                            if(lib.validac("desc_loc", "idcodice", e.FormattedValue.ToString().Trim()) == false)
                            {
                                MessageBox.Show("El valor no es valido para el taller", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
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
                if(Tx_modo.Text == "EDITAR")
                {
                    dtp_fingreso.Enabled = true;
                    tx_saldo.Enabled = true;
                }
                else
                {
                    dtp_fingreso.Enabled = false;
                    tx_saldo.Enabled = false;
                }
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
                if (Tx_modo.Text == "EDITAR")
                {
                    string cod2d = "";
                    foreach (DataRow row in dttaller.Rows)
                    {
                        if (row["idcodice"].ToString().Trim() == tx_dat_orig.Text.Trim())
                        {
                            cod2d = row["codigo"].ToString();
                        }
                    }
                    //cmb_tal.Tag = cod2d;
                    //cmb_tal.SelectedIndex = cmb_tal.FindString(cmb_tal.Tag.ToString());
                    tal = cod2d;
                }
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
                cmb_det2_SelectionChangeCommitted(null, null);
                cmb_det3.Tag = de3;
                cmb_det3.SelectedIndex = cmb_det3.FindString(cmb_det3.Tag.ToString());
                cmb_det3_SelectionChangeCommitted(null, null);
                if(dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value != null)
                {   
                    if (dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString() != "")         // f. ingreso
                    {   // tx_fingreso.Text = dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString().Substring(0, 10)
                        if(dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString().Substring(0, 10) == "00/00/0000")
                        {
                            dtp_fingreso.Value = DateTime.Now;
                            dtp_fingreso.Checked = false;
                        }
                        else
                        {
                            dtp_fingreso.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString());
                        }
                    }
                    else dtp_fingreso.Checked = false;  // tx_fingreso.Text = ""
                }
                tx_saldo.Text = dataGridView1.Rows[e.RowIndex].Cells["saldo"].Value.ToString();              // saldo
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // si es edicion, si es el usuario autorizado y el pedido es reciente => borra la(s) filas de detalle
            // busca en la base de datos y lo borra, debe actualizar estado del pedido y saldos
            if (Tx_modo.Text == "EDITAR")    // y el usuario esta autorizado
            {
                var aa = MessageBox.Show("seleccionó una fila para borrar" + Environment.NewLine + 
                    "se actualizarán los datos", "Confirma?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    //MessageBox.Show(dataGridView1.Rows[e.Row.Index].Cells[0].Value.ToString(),"los perros ladran");
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string borra = "delete from detaped where iddetaped=@idp";
                        MySqlCommand mion = new MySqlCommand(borra, conn);
                        mion.Parameters.AddWithValue("@idp", dataGridView1.Rows[e.Row.Index].Cells[0].Value.ToString());
                        mion.ExecuteNonQuery();
                        // estado del pedido
                        string pedido = "";
                        string compa = "select ifnull(sum(cant),0), ifnull(sum(saldo),0) from detaped where pedidoh=@ped";
                        mion = new MySqlCommand(compa, conn);
                        mion.Parameters.AddWithValue("@ped", tx_codped.Text);
                        MySqlDataReader dr = mion.ExecuteReader();
                        if (dr.Read())
                        {
                            if (dr.GetInt16(1) <= 0) pedido = estcomp;   // pedido recibo todo
                            if (dr.GetInt16(1) > 0 && dr.GetInt16(0) > dr.GetInt16(1)) pedido = estpend;    // "in-parcial";
                            if (dr.GetInt16(1) == dr.GetInt16(0)) pedido = estenv; // enviado a producción
                        }
                        dr.Close();
                        string actua = "update pedidos set status=@est where tipoes='TPE001' and codped=@ped";
                        mion = new MySqlCommand(actua, conn);
                        mion.Parameters.AddWithValue("@ped", tx_codped.Text);
                        mion.Parameters.AddWithValue("@est", pedido);
                        mion.ExecuteNonQuery();
                        conn.Close();
                        // actualizar el estado en el form y en la grilla
                        tx_dat_estad.Text = pedido;
                        cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                        for (int i = 0; i < dtg.Rows.Count; i++)
                        {
                            DataRow row = dtg.Rows[i];
                            if (row[0].ToString() == tx_idr.Text)
                            {
                                // a.id,a.codped,b.descrizionerid,a.origen,a.destino,fecha,entrega,a.coment,a.tipoes,a.status
                                dtg.Rows[i][2] = cmb_estado.SelectedItem.ToString().Substring(9, 6);    // tx_dat_estad.Text;
                                dtg.Rows[i][3] = tx_dat_orig.Text;
                                dtg.Rows[i][4] = tx_dat_dest.Text;
                                dtg.Rows[i][5] = dtp_pedido.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][6] = dtp_entreg.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][7] = tx_coment.Text;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No fue posible conectarse al servidor", "Error de conectividad");
                    }
                }
            }
        }
        #endregion

        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            bt_exc.Enabled = true;
        }
        private void tabuser_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = true;
            bt_prev.Enabled = true;
            bt_exc.Enabled = false;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // +++++++++++++++++++ VARIABLES DE POSICIONAMIENTO GENERAL ++++++++++++++++++ //
            float pix = 50.0F;      // punto inicial X
            float piy = 30.0F;      // punto inicial Y
            float alfi = 13.0F;     // alto de cada fila
            float alin = 45.0F;     // alto inicial
            float posi = 80.0F;     // posición de impresión
            float coli = 30.0F;     // columna mas a la izquierda
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ //
            imprime(pix, piy, cliente, coli, alin, posi, alfi, e);
        }
        private void imprime(float pix, float piy, string cliente, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // columnas del reporte
            float col0 = coli;              // It
            float col1 = coli + 40.0F;      // Cant
            float col2 = coli + 70.0F;      // Codigo
            float col3 = coli + 230.0F;     // Nombre del articulo
            float col4 = coli + 500.0F;     // Comentario
            float col5 = coli + 780.0F;     // Detalle2
            float col6 = coli + 860.0F;     // Madera
            float col7 = coli + 910.0F;     // Medidas
            float col8 = coli + 1010.0F;    // Acabado
            //
            float posit = impcab2(piy, coli, alin, posi, alfi, e,
                col0, col1, col2, col3, col4, col5, col6, col7, col8);
            posi = posit;
            SizeF espnom = new SizeF(250.0F, alfi);         // recuadro para el nombre y comentario
            Font lt_tit = new Font("Arial", 7);
            PointF ptoimp;
            Pen blackPen = new Pen(Color.Black, 2);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;
            // leemos las columnas del data table
            for (int fila = cuenta; fila < dataGridView1.Rows.Count - 1; fila++)
            {
                //id,cant,articulo,nombre,medidas,madera,deta2,acab,coment,cod.acab,cod.made,cod.det2,feching,saldo
                string data0 = (fila + 1).ToString("###");    // IT
                string data1 = dataGridView1.Rows[fila].Cells[1].Value.ToString();    // cant
                string data2 = dataGridView1.Rows[fila].Cells[2].Value.ToString();    // item
                string data3 = dataGridView1.Rows[fila].Cells[3].Value.ToString();    // nombre
                string data4 = dataGridView1.Rows[fila].Cells[8].Value.ToString();    // coment
                string data5 = "";
                if (dataGridView1.Rows[fila].Cells[11].Value.ToString().Substring(0, 1) == letpied)
                {
                    data5 = dataGridView1.Rows[fila].Cells[6].Value.ToString(); //.PadRight(6).Substring(0, 6); // detalle 2
                }
                string data6 = dataGridView1.Rows[fila].Cells[5].Value.ToString();    // madera
                string data7 = dataGridView1.Rows[fila].Cells[4].Value.ToString();    // medidas
                string data8 = dataGridView1.Rows[fila].Cells[7].Value.ToString();    // acabado
                //
                ptoimp = new PointF(col0, posi);
                e.Graphics.DrawString(data0, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col1, posi);
                e.Graphics.DrawString(data1, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col2, posi);
                e.Graphics.DrawString(data2, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col3, posi);
                RectangleF recn = new RectangleF(ptoimp,espnom);
                e.Graphics.DrawString(data3, lt_tit, Brushes.Black, recn, sf);
                ptoimp = new PointF(col4, posi);
                RectangleF recco = new RectangleF(ptoimp, espnom);
                e.Graphics.DrawString(data4, lt_tit, Brushes.Black, ptoimp, sf);
                ptoimp = new PointF(col5, posi);
                e.Graphics.DrawString(data5, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col6, posi);
                e.Graphics.DrawString(data6, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col7, posi);
                e.Graphics.DrawString(data7, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(col8, posi);
                e.Graphics.DrawString(data8, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
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
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString("OBSERVACIONES", lt_tit, Brushes.Black, ptoimp);
            posi = posi + alfi;             // avance de fila
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString(tx_coment.Text, lt_tit, Brushes.Black, ptoimp);
            cuenta = 0;
        }
        private float impcab2(float piy, float coli, float alin, float posi, float alfi, System.Drawing.Printing.PrintPageEventArgs e,
            float col0, float col1, float col2, float col3, float col4, float col5, float col6, float col7, float col8)
        {
            float ancho_pag = printDocument1.DefaultPageSettings.Bounds.Width;  // ancho de la pag.
            float colm = coli + 280.0F;                                 // columna media
            float cold = coli + 530.0F;                                 // columna derecha
            Font lt_cliente = new Font("Arial", 15, FontStyle.Bold);
            Font lt_pag = new Font("Arial", 9);
            Font lt_fec = new Font("Arial", 9);
            Font lt_tit = new Font("Arial", 9);                        // tipo de letra del titulo
            Pen grueso = new Pen(Color.Black, 2);                       // linea gruesa
            Pen delgado = new Pen(Color.Black, 1);                      // linea delgada
            StringFormat sf = new StringFormat();                       // formato centrado
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            // logo
            e.Graphics.DrawImage(Image.FromFile("recursos/logo_artesanos_omg_peru.jpeg"), 30, 20,200,150);
            //
            SizeF anctit = new SizeF();
            anctit = e.Graphics.MeasureString(cliente, lt_cliente);
            PointF ptocli = new PointF((ancho_pag - anctit.Width)/2, piy);
            e.Graphics.DrawString(cliente, lt_cliente, Brushes.Black, ptocli, StringFormat.GenericTypographic);
            // pintamos contador de pág.
            PointF ptopag = new PointF(ancho_pag - 80.0F, piy);
            string pag = "Pág. " + pageCount.ToString();
            e.Graphics.DrawString(pag, lt_pag, Brushes.Black, ptopag, StringFormat.GenericTypographic);
            // pintamos la fecha
            PointF ptofec = new PointF(ancho_pag - 80.0F, piy + 15.0F);
            string fecha = DateTime.Today.ToShortDateString();
            e.Graphics.DrawString(fecha, lt_fec, Brushes.Black, ptofec, StringFormat.GenericTypographic);
            // almacen destino y numero de pedido, fecha y entrega programada
            posi = posi + alfi;
            SizeF sizrec = new SizeF(200, piy);
            PointF ptodir = new PointF(coli + 300, posi);
            e.Graphics.DrawString("PEDIDO INTERNO", lt_pag, Brushes.Black, ptodir, StringFormat.GenericTypographic);
            ptodir = new PointF(coli + 500, posi);
            e.Graphics.DrawString("FECHA DEL PEDIDO", lt_pag, Brushes.Black, ptodir, StringFormat.GenericTypographic);
            ptodir = new PointF(coli + 700, posi);
            e.Graphics.DrawString("INGRESO A ALMACEN", lt_pag, Brushes.Black, ptodir, StringFormat.GenericTypographic);
            ptodir = new PointF(coli + 300, posi + 15.0F);
            RectangleF recped = new RectangleF(ptodir, sizrec);
            e.Graphics.DrawRectangle(grueso, Rectangle.Round(recped));
            e.Graphics.DrawString(cmb_destino.Text.Substring(0,6) + "   " + tx_codped.Text, lt_tit, Brushes.Black, recped, sf);
            ptodir = new PointF(coli + 500, posi + 15.0F);
            RectangleF recfep = new RectangleF(ptodir, sizrec);
            e.Graphics.DrawRectangle(grueso, Rectangle.Round(recfep));
            e.Graphics.DrawString(dtp_pedido.Value.ToShortDateString(), lt_tit, Brushes.Black, recfep, sf);
            ptodir = new PointF(coli + 700, posi + 15.0F);
            RectangleF recent = new RectangleF(ptodir, sizrec);
            e.Graphics.DrawRectangle(grueso, Rectangle.Round(recent));
            e.Graphics.DrawString(dtp_entreg.Value.ToShortDateString(), lt_tit, Brushes.Black, recent, sf);
            posi = posi + alfi * 6;
            // pintamos el recuadro de la familia productora        
            SizeF reclargo = new SizeF(ancho_pag - 50.0F, piy);
            ptodir = new PointF(coli, posi);
            RectangleF recfam = new RectangleF(ptodir,reclargo);
            e.Graphics.DrawRectangle(delgado, Rectangle.Round(recfam));
            e.Graphics.DrawString("FAMILIA PRODUCTORA " + cmb_taller.Text, lt_tit, Brushes.Black, recfam, sf);
            //
            colm = coli + 280.0F;
            cold = colm + 280.0F;
            posi = posi + alfi * 3;                                         // avance de fila
            //Pen blackPen = new Pen(Color.Black, 2);                              // color y grosor de la línea separadora
            //e.Graphics.DrawLine(blackPen, coli - 1, posi, cold + 160.0F, posi);
            //posi = posi + alfi;                                         // avance de fila
            // titulo de las columnas
            PointF ptoimp = new PointF(col0, posi);
            e.Graphics.DrawString("It", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col1, posi);
            e.Graphics.DrawString("Cant", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col2, posi);
            e.Graphics.DrawString("Código", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col3, posi);
            e.Graphics.DrawString("Nombre", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col4, posi);
            e.Graphics.DrawString("Comentario", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col5, posi);
            e.Graphics.DrawString("Deta2", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col6, posi);
            e.Graphics.DrawString("Mad.", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col7, posi);
            e.Graphics.DrawString("Medidas", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(col8, posi);
            e.Graphics.DrawString("Acabado", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi + 7.0F;             // avance de fila
            e.Graphics.DrawLine(delgado, coli, posi, ancho_pag - 20.0F, posi);
            posi = posi + 2;             // avance de fila
            //
            return posi;
        }
    }
}
