using System;
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
        string tiesta = "";             // estado inicial por defecto del pedido de clientes
        string escambio = "";           // estados de pedido de clientes que admiten modificar el pedido
        string estpend = "";            // estado de pedido de clientes con articulos pendientes de recibir
        string estcomp = "";            // estado de pedido de clientes con articulos recibidos en su totalidad
        string estenv = "";             // estado de pedido de clientes enviado a producción
        string estanu = "";             // estado de pedido de clientes anulado
        string estcer = "";             // estado de pedido de clientes cerrado tal como esta, ya no se atiende
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
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();    // dtg primario, original con la carga del 
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
            if (keyData == Keys.F1 && Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
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
                    para2 = tx_idc.Text;
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue0;    // id del contrato
                            tx_cont.Text = ayu2.ReturnValue1;
                            tx_cliente.Text = ayu2.ReturnValue2;
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

                            tx_d_can.Text = ayu2.ReturnValueA[2].ToString();
                            tx_d_codi.Text = ayu2.ReturnValue1.Substring(0,10) + tx_codta.Text.Trim() + ayu2.ReturnValue1.Substring(12, 6);
                            tx_d_nom.Text = ayu2.ReturnValueA[3].ToString();
                            tx_d_med.Text = ayu2.ReturnValueA[4].ToString();
                            tx_d_mad.Text = ayu2.ReturnValueA[5].ToString();
                            //tx_d_est.Text = ayu2.ReturnValueA[5].ToString();
                            tx_d_com.Text = ayu2.ReturnValueA[6].ToString();
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
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = false;
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
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
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // estado del pedido inicial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "pendiente") estpend = row["valor"].ToString().Trim();         // estado del pedido con llegada parcial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "recibido") estcomp = row["valor"].ToString().Trim();         // estado del pedido con llegada total
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cambio") escambio = row["valor"].ToString().Trim();         // estado del pedido que admiten modificar el pedido
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "enviado") estenv = row["valor"].ToString().Trim();         // estado del pedido enviado a producción
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "anulado") estanu = row["valor"].ToString().Trim();         // estado del pedido anulado
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cerrado") estcer = row["valor"].ToString().Trim();         // estado del pedido cerrado asi como esta
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "manual") estman = row["valor"].ToString().Trim();         // estados que se pueden seleccionar manualmente
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
        private void jalaoc(string campo)                   // jala datos de usuarios por id o nom_user
        {
            if (campo == "tx_idr" && tx_idr.Text != "")
            {   // id,codped,tipoes,origen,destino,fecha,entrega,coment
                // tx_idr.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // 
                tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // codigo pedido
                //tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();  // tipo pedido
                tx_dat_tiped.Text = tipede;
                tx_dat_estad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString();  // estado del pedido
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();   // taller origen
                dtp_pedido.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString());   // fecha pedido
                dtp_entreg.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString());    // fecha entrega
                tx_coment.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();     // comentario
                //cmb_cap.SelectedValue = tx_dat_tiped.Text;
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                //cmb_tip.SelectedValue = tx_dat_tip.Text;
                jaladet(tx_codped.Text);
            }
            if (campo == "tx_codped" && tx_codped.Text != "")
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
                        dtp_pedido.Value = Convert.ToDateTime(row["fecha"].ToString());   // fecha pedido
                        dtp_entreg.Value = Convert.ToDateTime(row["entrega"].ToString());    // fecha entrega
                        tx_coment.Text = row["coment"].ToString();     // comentario
                        cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                        cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                        cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
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
                "from ?? a " +
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
        private void grilla()                               // arma la grilla
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
        private void dataload(string quien)                  // jala datos para los combos y la grilla
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
                cdg.Parameters.AddWithValue("@tip", tipede);                // tipo pedidos catalogo clientes
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
            }
            conn.Close();
        }
        // graba
        // edita
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
            string consulta = "select a.id,a.contrato,b.idanagrafica,b.razonsocial " +
                "from contrat a left join anag_cli b on b.idanagrafica=a.cliente where a.contrato=@cont";
            MySqlCommand micon = new MySqlCommand(consulta, conn);
            micon.Parameters.AddWithValue("@cont", cont);
            MySqlDataReader dr = micon.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt16(0) > 0)
                {
                    tx_idc.Text = dr.GetString(2);
                    tx_cliente.Text = dr.GetString(3);
                    retorna = true;
                }
                else retorna = false;
            }
            dr.Close();
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
            cmb_tipo.Enabled = false;
            cmb_estado.SelectedIndex = cmb_estado.FindString(tiesta);
            cmb_estado.Enabled = false;
            tx_dat_estad.Text = tiesta;
            cmb_estado.Enabled = false;
            tx_codped.ReadOnly = true;
            dtp_fingreso.Checked = false;
            dtp_fingreso.Enabled = false;
            tx_saldo.ReadOnly = true;
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
            /*
            PrintDialog printDlg = new PrintDialog();
            printDlg.Document = printDocument1;
            printDlg.AllowSomePages = true;
            printDlg.AllowSelection = true;
            //
            pageCount = 1;
            printDocument1.DefaultPageSettings.Landscape = true;
            //
            if (printDlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
            */
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text != "" && tx_rind.Text != "")
            {
                Tx_modo.Text = "IMPRIMIR";
                /*
                pageCount = 1;
                printDocument1.DefaultPageSettings.Landscape = true;
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
                */
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
            cmb_estado.SelectedIndex = -1;
        }
        #endregion limpiadores_modos;
        #region comboboxes
        private void cmb_estado_Click(object sender, EventArgs e)
        {
            indant = cmb_estado.SelectedIndex;
        }
        private void cmb_estado_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            if (Tx_modo.Text == "NUEVO")
            {
                string cod2d = "";
                foreach (DataRow row in dttaller.Rows)
                {
                    if (row["idcodice"].ToString().Trim() == tx_dat_orig.Text.Trim())
                    {
                        cod2d = row["codigo"].ToString();
                        tx_codta.Text = row["codigo"].ToString();
                    }
                }
            }
        }
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString();
            else tx_dat_tiped.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        #endregion comboboxes
        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")    //  && tx_idr.Text != ""
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_codped_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_codped.Text != "")
            {
                jalaoc("tx_codped");
            }
        }
        private void tx_d_can_Leave(object sender, EventArgs e)
        {
            tx_saldo.Text = tx_d_can.Text;
            if (tx_d_codi.Text.Trim() != "")
            {
                tx_d_codi_Leave(null, null);
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
                if (tx_cont.Text.Trim() != "" && tx_cliente.Text.Trim() == "")
                {
                    if (buscont(tx_cont.Text) == false)
                    {
                        MessageBox.Show("No existe el contrato", "Error");
                        tx_cont.Text = "";
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
                            if (lib.validac("desc_loc", "idcodice", e.FormattedValue.ToString().Trim()) == false)
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
                if (Tx_modo.Text == "EDITAR")
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

                if (dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value != null)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString() != "")         // f. ingreso
                    {   // tx_fingreso.Text = dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString().Substring(0, 10)
                        if (dataGridView1.Rows[e.RowIndex].Cells["fingreso"].Value.ToString().Substring(0, 10) == "00/00/0000")
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
    }
}
