using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class articulos : Form
    {
        static string nomform = "articulos";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "items";         // idcategoria='CLI' -> vista anag_cli
        public int totfilgrid, cta;             // variables para impresion
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
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();    // dtg primario, original con la carga del form

        public articulos()
        {
            InitializeComponent();
        }
        private void articulos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void articulos_Load(object sender, EventArgs e)
        {
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            // Set up the delays for the ToolTip.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            grilla2();
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = true;
            tabControl1.SelectedTab = tabgrilla;
            advancedDataGridView1.Enabled = false;
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);
            this.tabreg.BackColor = Color.FromName(iOMG.Program.colgri);

            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_anul.Image = Image.FromFile(img_btA);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // longitudes maximas de campos
            tx_nombre.MaxLength = 90;           // nombre
            tx_medidas.MaxLength = 45;           // direccion
        }
        private void grilla()                               // arma la grilla
        {
            // id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles2018
            Font tiplg = new Font("Arial",7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            // id 
            advancedDataGridView1.Columns[0].Visible = false;
            // tipo de documento
            advancedDataGridView1.Columns[1].Visible = true;            // columna visible o no
            advancedDataGridView1.Columns[1].HeaderText = "Código";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 120;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[1].Tag = "validaNO";
            advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // familia
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Capitulo";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 50;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = false;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaSI";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // modelo
            advancedDataGridView1.Columns[3].Visible = true;       
            advancedDataGridView1.Columns[3].HeaderText = "Modelo";
            advancedDataGridView1.Columns[3].Width = 50;
            advancedDataGridView1.Columns[3].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[3].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // madera
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Madera";
            advancedDataGridView1.Columns[4].Width = 50;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaSI";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // tipologia
            advancedDataGridView1.Columns[5].Visible = true;       
            advancedDataGridView1.Columns[5].HeaderText = "Topología";
            advancedDataGridView1.Columns[5].Width = 50;
            advancedDataGridView1.Columns[5].ReadOnly = false;
            advancedDataGridView1.Columns[5].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // detalle 1
            advancedDataGridView1.Columns[6].Visible = true;       
            advancedDataGridView1.Columns[6].HeaderText = "Det.1";
            advancedDataGridView1.Columns[6].Width = 50;
            advancedDataGridView1.Columns[6].ReadOnly = true;
            advancedDataGridView1.Columns[6].Tag = "validaSI";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // acabado
            advancedDataGridView1.Columns[7].Visible = true;
            advancedDataGridView1.Columns[7].HeaderText = "Acabado";
            advancedDataGridView1.Columns[7].Width = 50;
            advancedDataGridView1.Columns[7].ReadOnly = true;
            advancedDataGridView1.Columns[7].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // taller
            advancedDataGridView1.Columns[8].Visible = true;
            advancedDataGridView1.Columns[8].HeaderText = "Taller";
            advancedDataGridView1.Columns[8].Width = 50;
            advancedDataGridView1.Columns[8].ReadOnly = true;
            advancedDataGridView1.Columns[8].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // detalle 2
            advancedDataGridView1.Columns[9].Visible = true;
            advancedDataGridView1.Columns[9].HeaderText = "Det.2";
            advancedDataGridView1.Columns[9].Width = 50;
            advancedDataGridView1.Columns[9].ReadOnly = false;
            advancedDataGridView1.Columns[9].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // detalle 3
            advancedDataGridView1.Columns[10].Visible = true;
            advancedDataGridView1.Columns[10].HeaderText = "Det.3";
            advancedDataGridView1.Columns[10].Width = 50;
            advancedDataGridView1.Columns[10].ReadOnly = false;
            advancedDataGridView1.Columns[10].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre
            advancedDataGridView1.Columns[11].Visible = true;
            advancedDataGridView1.Columns[11].HeaderText = "Nombre";
            advancedDataGridView1.Columns[11].Width = 160;
            advancedDataGridView1.Columns[11].ReadOnly = false;
            advancedDataGridView1.Columns[11].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // medida
            advancedDataGridView1.Columns[12].Visible = true;
            advancedDataGridView1.Columns[12].HeaderText = "Medida";
            advancedDataGridView1.Columns[12].Width = 100;
            advancedDataGridView1.Columns[12].ReadOnly = false;
            advancedDataGridView1.Columns[12].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // unidad media
            advancedDataGridView1.Columns[13].Visible = true;
            advancedDataGridView1.Columns[13].HeaderText = "U.Med.";
            advancedDataGridView1.Columns[13].Width = 160;
            advancedDataGridView1.Columns[13].ReadOnly = false;
            advancedDataGridView1.Columns[13].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // soles 2018
            advancedDataGridView1.Columns[14].Visible = true;
            advancedDataGridView1.Columns[14].HeaderText = "Precio";
            advancedDataGridView1.Columns[14].Width = 160;
            advancedDataGridView1.Columns[14].ReadOnly = false;
            advancedDataGridView1.Columns[14].Tag = "validaNO";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        private void grilla2()                              // grilla de filtros de nivel superior
        {
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ColumnCount = (advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.ColumnCount;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.Rows.Add();
            for (int i = 0; i < ((advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.Columns.Count); i++)
            {
                dataGridView2.Columns[i].Width = advancedDataGridView1.Columns[i].Width;
                dataGridView2.Columns[i].Name = advancedDataGridView1.Columns[i].Name;
                //
                if (i == 0)
                {
                    dataGridView2.Columns[i].Visible = false;
                }
            }
            dataGridView2.Columns["id"].ReadOnly = true;
        }
        private void jalainfo()                             // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select campo,param,valor from enlaces where formulario=@nofo";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");   // nomform
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["campo"].ToString() == "imagenes")
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
        public void jalaoc(string campo)                    // jala datos id o ????
        {
            if (campo == "tx_idr")  //  && tx_idr.Text != ""
            {// id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles2018
                //tx_idr.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();     // codigo
                tx_dat_cap.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();   // capitulo
                tx_dat_mod.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();   // modelo
                tx_dat_mad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();   // madera
                tx_dat_tip.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString();   // tipologia
                tx_dat_det1.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString();   // detalle 1
                tx_dat_aca.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();   // acabado
                tx_dat_tal.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[8].Value.ToString();   // taller
                tx_dat_det2.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString();   // detalle 2
                tx_dat_det3.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[10].Value.ToString();   // detalle 3
                tx_nombre.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[11].Value.ToString();   // nombre
                tx_medidas.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[12].Value.ToString();   // medida
                tx_umed.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[13].Value.ToString();      // u.medida
                tx_precio.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString();    // precio
                cmb_cap.SelectedValue = tx_dat_cap.Text;
                cmb_mod.SelectedValue = tx_dat_mod.Text;
                cmb_mad.SelectedValue = tx_dat_mad.Text;
                cmb_tip.SelectedValue = tx_dat_tip.Text;
                cmb_det1.SelectedValue = tx_dat_det1.Text;
                cmb_det2.SelectedValue = tx_dat_det2.Text;
                cmb_det3.SelectedValue = tx_dat_det3.Text;
                cmb_aca.SelectedValue = tx_dat_aca.Text;
                cmb_tal.SelectedValue = tx_dat_tal.Text;
            }
        }
        public void dataload(string quien)                  // jala datos para los combos y la grilla
        {   // "todos"=comboscodigo, "capit"=codigo familia, "maestra"=items de la grilla
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            tabControl1.SelectedTab = tabreg;
            if (quien == "maestra")
            {
                // datos de los articulos
                string datgri = "select id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles2018 " +
                    "from items";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Fill(dtu);  // original con la carga
                dag.Dispose();
            }
            //  datos para el combobox de tipo de documento
            if (quien == "capit")
            {
                cmb_tip.Items.Clear();
                //tx_dat_tip.Text = "";
                const string contip = "select b.descrizione,a.tipol from items a " +
                    "left join desc_tip b on b.idcodice=a.tipol " +
                    "where a.capit=@des group by a.tipol";
                MySqlCommand cmdtip = new MySqlCommand(contip, conn);
                cmdtip.Parameters.AddWithValue("@des", tx_dat_cap.Text.Trim());
                DataTable dttip = new DataTable();
                MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                datip.Fill(dttip);
                foreach (DataRow row in dttip.Rows)
                {
                    cmb_tip.Items.Add(row.ItemArray[1].ToString());
                    cmb_tip.ValueMember = row.ItemArray[1].ToString();
                }
            }
            if (quien == "todos")
            {
                // seleccion de capitulo
                cmb_cap.Items.Clear();
                tx_dat_cap.Text = "";
                const string concap = "select descrizionerid,idcodice from desc_gru " +
                    "where numero=1";
                MySqlCommand cmdcap = new MySqlCommand(concap, conn);
                DataTable dtcap = new DataTable();
                MySqlDataAdapter dacap = new MySqlDataAdapter(cmdcap);
                dacap.Fill(dtcap);
                foreach (DataRow row in dtcap.Rows)
                {
                    this.cmb_cap.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());  // citem_cap
                    this.cmb_cap.ValueMember = row.ItemArray[1].ToString(); //citem_cap.Value.ToString();
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
                tx_dat_mad.Text = "";
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
                // seleccion de detalle1
                this.cmb_det1.Items.Clear();
                tx_dat_det1.Text = "";
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
                this.cmb_aca.Items.Clear();
                tx_dat_aca.Text = "";
                const string conaca = "select descrizionerid,idcodice from desc_est " +
                    "where numero=1";
                MySqlCommand cmdaca = new MySqlCommand(conaca, conn);
                DataTable dtaca = new DataTable();
                MySqlDataAdapter daaca = new MySqlDataAdapter(cmdaca);
                daaca.Fill(dtaca);
                foreach (DataRow row in dtaca.Rows)
                {
                    this.cmb_aca.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_aca
                    this.cmb_aca.ValueMember = row.ItemArray[1].ToString(); //citem_aca.Value.ToString();
                }
                // seleccion de taller
                this.cmb_tal.Items.Clear();
                tx_dat_tal.Text = "";
                const string contal = "select descrizionerid,codigo from desc_loc " +
                    "where numero=1";
                MySqlCommand cmdtal = new MySqlCommand(contal, conn);
                DataTable dttal = new DataTable();
                MySqlDataAdapter datal = new MySqlDataAdapter(cmdtal);
                datal.Fill(dttal);
                foreach (DataRow row in dttal.Rows)
                {
                    this.cmb_tal.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_tal
                    this.cmb_tal.ValueMember = row.ItemArray[1].ToString(); // citem_tal.Value.ToString();
                }
                // seleccion de detalle 2 (tallado, marqueteado, etc)
                this.cmb_det2.Items.Clear();
                tx_dat_det2.Text = "";
                const string condt2 = "select descrizione,idcodice from desc_dt2 " +
                    "where numero=1";
                MySqlCommand cmddt2 = new MySqlCommand(condt2, conn);
                DataTable dtdt2 = new DataTable();
                MySqlDataAdapter dadt2 = new MySqlDataAdapter(cmddt2);
                dadt2.Fill(dtdt2);
                foreach (DataRow row in dtdt2.Rows)
                {
                    this.cmb_det2.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt2
                    this.cmb_det2.ValueMember = row.ItemArray[1].ToString();     //citem_dt2.Value.ToString();
                }
                // seleccion de detalle 3
                cmb_det3.Items.Clear();
                tx_dat_det3.Text = "";
                const string condt3 = "select descrizione,idcodice from desc_dt3 where numero=1";
                MySqlCommand cmddt3 = new MySqlCommand(condt3, conn);
                DataTable dtdt3 = new DataTable();
                MySqlDataAdapter dadt3 = new MySqlDataAdapter(cmddt3);
                dadt3.Fill(dtdt3);
                foreach (DataRow row in dtdt3.Rows)
                {
                    this.cmb_det3.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt3
                    this.cmb_det3.ValueMember = row.ItemArray[1].ToString();    //citem_dt3.Value.ToString();
                }
            }
            //
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
        private Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void filtros(string expres)                 // filtros de nivel superior datagridview2
        {
            DataView dv = new DataView(dtg);
            dv.RowFilter = expres;
            dtg = dv.ToTable();
            advancedDataGridView1.DataSource = dtg;
            grilla();
            //cellsum(0);
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
        public void limpia_chk()    
        {
            checkBox1.Checked = false;
        }
        public void limpia_otros()
        {
            //this.checkBox1.Checked = false;
        }
        public void limpia_combos()
        {
            cmb_aca.SelectedIndex = -1;
            cmb_cap.SelectedIndex = -1;
            cmb_det1.SelectedIndex = -1;
            cmb_det2.SelectedIndex = -1;
            cmb_det3.SelectedIndex = -1;
            cmb_mad.SelectedIndex = -1;
            cmb_mod.SelectedIndex = -1;
            cmb_tal.SelectedIndex = -1;
            cmb_tip.SelectedIndex = -1;
        }
        public void limpiapag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if(oControls is CheckBox)
                {
                    checkBox1.Checked = false;
                }
                if(oControls is ComboBox)
                {
                    cmb_aca.SelectedIndex = -1;
                    cmb_cap.SelectedIndex = -1;
                    cmb_det1.SelectedIndex = -1;
                    cmb_det2.SelectedIndex = -1;
                    cmb_det3.SelectedIndex = -1;
                    cmb_mad.SelectedIndex = -1;
                    cmb_mod.SelectedIndex = -1;
                    cmb_tal.SelectedIndex = -1;
                    cmb_tip.SelectedIndex = -1;
                }
            }
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            if (tx_dat_cap.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el capítulo", " Error! ");
                cmb_cap.Focus();
                return;
            }
            if (tx_dat_mod.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el modelo", " Error! ");
                cmb_mod.Focus();
                return;
            }
            if (tx_dat_mad.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el tipo de madera", " Error! ");
                cmb_mad.Focus();
                return;
            }
            if (tx_dat_tip.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el tipo de artículo", " Error! ");
                cmb_tip.Focus();
                return;
            }
            if (tx_dat_det1.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el detalle 1", " Error! ");
                cmb_det1.Focus();
                return;
            }
            if (tx_dat_aca.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la dirección", " Error! ");
                cmb_aca.Focus();
                return;
            }
            if (tx_dat_tal.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese ubigeo correcto", " Error! ");
                cmb_tal.Focus();
                return;
            }
            if(tx_dat_det2.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el detalle 2", " Error! ");
                cmb_det2.Focus();
                return;
            }
            if (tx_dat_det3.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el detalle 3", " Error! ");
                cmb_det3.Focus();
                return;
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            if (modo == "NUEVO")
            {
                var aa = MessageBox.Show("Confirma que desea crear el artículo?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(aa == DialogResult.Yes)
                {
                    if(graba() == true)
                    {
                        // insertamos en el datatable
                        DataRow dr = dtg.NewRow();
                        // id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles2018
                        string codi = tx_dat_cap.Text.Trim() + tx_dat_mod.Text.Trim() + tx_dat_mad.Text.Trim() +
                            tx_dat_tip.Text.Trim() + tx_dat_det1.Text.Trim() + tx_dat_aca.Text.Trim() +
                            tx_dat_tal.Text.Trim() + tx_dat_det2.Text.Trim() + tx_dat_det3.Text.Trim() + "N000";
                        dr[1] = codi;
                        dr[2] = tx_dat_cap.Text.Trim();
                        dr[3] = tx_dat_mod.Text.Trim();
                        dr[4] = tx_dat_mad.Text.Trim();
                        dr[5] = tx_dat_tip.Text.Trim();
                        dr[6] = tx_dat_det1.Text.Trim();
                        dr[7] = tx_dat_aca.Text.Trim();
                        dr[8] = tx_dat_tal.Text.Trim();
                        dr[9] = tx_dat_det2.Text.Trim();
                        dr[10] = tx_dat_det3.Text.Trim();
                        //micon.Parameters.AddWithValue("@jgo", "N000");
                        dr[11] = tx_nombre.Text.Trim();
                        dr[12] = tx_medidas.Text.Trim();
                        dr[13] = "C.U.";
                        dr[14] = tx_umed.Text;
                        dtg.Rows.Add(dr);
                        dtu.Rows.Add(dr);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo grabar el artículo", "Error en crear", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    cmb_cap.Focus();
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                var aa = MessageBox.Show("Confirma que desea modificar el artículo?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    edita();
                }
                else
                {
                    cmb_cap.Focus();
                    return;
                }
            }
            if (modo == "ANULAR")       // opción para borrar
            { 
                // 

            }
            if (iserror == "no")
            {
                // debe limpiar los campos y actualizar la grilla
                limpiar(this);
                limpiapag(tabreg);
                limpia_otros();
                cmb_cap.Focus();
            }
        }
        private bool graba()
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                try
                {
                    string codi = tx_dat_cap.Text.Trim() + tx_dat_mod.Text.Trim() + tx_dat_mad.Text.Trim() +
                    tx_dat_tip.Text.Trim() + tx_dat_det1.Text.Trim() + tx_dat_aca.Text.Trim() +
                    tx_dat_tal.Text.Trim() + tx_dat_det2.Text.Trim() + tx_dat_det3.Text.Trim() + "N000";
                    string inserta = "insert into items (" +
                        "codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,medid,umed,soles2018) values (" +
                        "@codi,@capi,@mode,@made,@tipo,@det1,@acab,@tall,@det2,@det3,@jgo,@nomb,@medi,@umed,@prec)";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@codi", codi);
                    micon.Parameters.AddWithValue("@capi", tx_dat_cap.Text.Trim());
                    micon.Parameters.AddWithValue("@mode", tx_dat_mod.Text.Trim());
                    micon.Parameters.AddWithValue("@made", tx_dat_mad.Text.Trim());
                    micon.Parameters.AddWithValue("@tipo", tx_dat_tip.Text.Trim());
                    micon.Parameters.AddWithValue("@det1", tx_dat_det1.Text.Trim());
                    micon.Parameters.AddWithValue("@acab", tx_dat_aca.Text.Trim());
                    micon.Parameters.AddWithValue("@tall", tx_dat_tal.Text.Trim());
                    micon.Parameters.AddWithValue("@det2", tx_dat_det2.Text.Trim());
                    micon.Parameters.AddWithValue("@det3", tx_dat_det3.Text.Trim());
                    micon.Parameters.AddWithValue("@jgo", "N000");
                    micon.Parameters.AddWithValue("@nomb", tx_nombre.Text.Trim());
                    micon.Parameters.AddWithValue("@medi", tx_medidas.Text.Trim());
                    micon.Parameters.AddWithValue("@umed", "C.U.");
                    micon.Parameters.AddWithValue("@prec", tx_umed.Text);
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
        private void edita()
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    string codi = tx_dat_cap.Text.Trim() + tx_dat_mod.Text.Trim() + tx_dat_mad.Text.Trim() +
                    tx_dat_tip.Text.Trim() + tx_dat_det1.Text.Trim() + tx_dat_aca.Text.Trim() +
                    tx_dat_tal.Text.Trim() + tx_dat_det2.Text.Trim() + tx_dat_det3.Text.Trim() + "N000";
                    string actua = "update items set " +
                        "codig=@codi,capit=@capi,model=@mode,mader=@made,tipol=@tipo,deta1=@det1,acaba=@acab,talle=@tall," +
                        "deta2=@det2,deta3=@det3,juego=@jgo,nombr=@nomb,medid=@medi,umed=@umed,soles2018=@prec " +
                        "where id=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@codi", codi);
                    micon.Parameters.AddWithValue("@capi", tx_dat_cap.Text.Trim());
                    micon.Parameters.AddWithValue("@mode", tx_dat_mod.Text.Trim());
                    micon.Parameters.AddWithValue("@made", tx_dat_mad.Text.Trim());
                    micon.Parameters.AddWithValue("@tipo", tx_dat_tip.Text.Trim());
                    micon.Parameters.AddWithValue("@det1", tx_dat_det1.Text.Trim());
                    micon.Parameters.AddWithValue("@acab", tx_dat_aca.Text.Trim());
                    micon.Parameters.AddWithValue("@tall", tx_dat_tal.Text.Trim());
                    micon.Parameters.AddWithValue("@det2", tx_dat_det2.Text.Trim());
                    micon.Parameters.AddWithValue("@det3", tx_dat_det3.Text.Trim());
                    micon.Parameters.AddWithValue("@jgo", "N000");
                    micon.Parameters.AddWithValue("@nomb", tx_nombre.Text.Trim());
                    micon.Parameters.AddWithValue("@medi", tx_medidas.Text.Trim());
                    micon.Parameters.AddWithValue("@umed", "C.U.");
                    micon.Parameters.AddWithValue("@prec", tx_umed.Text);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
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
                return;
            }
            conn.Close();
        }
        #endregion boton_form;

        #region leaves
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text != "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        private void tx_rind_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_rind.Text != "")
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        #endregion leaves;

        #region botones_de_comando_y_articulos  
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
                if (Convert.ToString(row["btn1"]) == "S")
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")
                {
                    bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
        }
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            tabControl1.SelectedTab = tabreg;
            escribe(this);
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            cmb_cap.Focus();
            limpiar(this);
            limpia_otros();
            limpia_combos();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabgrilla;
            escribe(this);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            limpia_otros();
            limpia_combos();
            jalaoc("tx_idr");
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            sololee(this);
            Tx_modo.Text = "IMPRIMIR";
            button1.Image = Image.FromFile("print48");
            cmb_cap.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)          // pone todos los articulos en N
        {
            advancedDataGridView1.Enabled = true;
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabreg;
            escribe(this);
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            limpia_otros();
            limpia_combos();
            jalaoc("tx_idr");
        }
        private void bt_exc_Click(object sender, EventArgs e)           // exporta a excel
        {
            // me quede aca!
        }
        private void Bt_first_Click(object sender, EventArgs e)
        {
            limpiar(this);
            limpia_chk();
            limpia_combos();
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[0].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            int aca = int.Parse(tx_rind.Text) - 1;
            limpia_chk();
            limpia_combos();
            limpiar(this);
            int fila = advancedDataGridView1.CurrentCell.RowIndex;
            int nfil = fila - 1;
            if (nfil < 0)
            {
                nfil = nfil + 1;
            }
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[nfil].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            int aca = int.Parse(tx_rind.Text) + 1;
            limpia_chk();
            limpia_combos();
            limpiar(this);
            int fila = advancedDataGridView1.CurrentCell.RowIndex;
            int nfil = fila + 1;
            if(nfil > advancedDataGridView1.Rows.Count - 2)
            {
                nfil = nfil - 1;
            }
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[nfil].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = advancedDataGridView1.CurrentCell.RowIndex.ToString();
            tx_rind_Leave(null, null);
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            int ultimo = advancedDataGridView1.Rows.Count-2;
            limpiar(this);
            limpia_chk();
            limpia_combos();
            advancedDataGridView1.CurrentCell = advancedDataGridView1.Rows[ultimo].Cells[1];
            advancedDataGridView1.CurrentCell.Selected = true;
            tx_rind.Text = ultimo.ToString();//advancedDataGridView1.Rows[ultimo].Cells[0].Value.ToString();
            tx_rind_Leave(null, null);
        }

        private void tabreg_Enter(object sender, EventArgs e)
        {
            bt_exc.Enabled = false;
            Bt_print.Enabled = true;
        }
        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            bt_exc.Enabled = true;
            Bt_print.Enabled = false;
        }
        #endregion botones;
        // articulos para habilitar los botones de comando
        #endregion botones_de_comando  ;

        #region comboboxes
        private void cmb_cap_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_cap.Text = cmb_cap.SelectedItem.ToString().Substring(0, 1);
            dataload("capit");
        }
        private void cmb_mod_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_mod.Text = cmb_mod.SelectedItem.ToString().Substring(0, 3);
        }
        private void cmb_mad_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_mad.Text = cmb_mad.SelectedItem.ToString().Substring(0, 1);
        }
        private void cmb_tip_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_tip.Text = cmb_tip.SelectedItem.ToString().Substring(0, 2);
        }
        private void cmb_det1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det1.Text = cmb_det1.SelectedItem.ToString().Substring(0, 2);
        }
        private void cmb_aca_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_aca.Text = cmb_aca.SelectedItem.ToString().Substring(0, 1);
        }
        private void cmb_tal_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_tal.Text = cmb_tal.SelectedItem.ToString().Substring(0, 2);
        }
        private void cmb_det2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det2.Text = cmb_det2.SelectedItem.ToString().Substring(0, 3);
        }
        private void cmb_det3_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det3.Text = cmb_det3.SelectedItem.ToString().Substring(0, 3);
        }
        #endregion comboboxes

        #region datagridview2
        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Value != null)
            {
                string frase = dataGridView2.Columns[e.ColumnIndex].Name.ToString() + " like '" + dataGridView2.CurrentCell.Value.ToString() + "*'";
                filtros(frase);
            }
            if(dataGridView2.CurrentCell.Value == null || dataGridView2.CurrentCell.Value.ToString().Trim() == "")
            {
                if(true == true)    // no hay otros filtros
                {
                    advancedDataGridView1.DataSource = dtu;
                }
            }
        }
        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                advancedDataGridView1.HorizontalScrollingOffset = e.NewValue;
            }
        }
        #endregion

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString;
        }
        private void advancedDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)            // almacena valor previo al ingresar a la celda
        {
            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                //string codu = "";
                string idr,rind = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rind = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabreg;
                limpiar(this);
                limpiapag(tabreg);
                limpia_otros();
                limpia_combos();
                tx_idr.Text = idr;
                tx_rind.Text = rind;
                jalaoc("tx_idr");
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0 
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString())
            {
                string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                string[] noeta = equivinter(advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString());    // retorna la tabla segun el titulo de la columna

                var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                    "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    if(advancedDataGridView1.Columns[e.ColumnIndex].Tag.ToString() == "validaSI")   // la columna se valida?
                    {
                        // valida si el dato ingresado es valido en la columna
                        if (e.ColumnIndex == 2)                         // valida familia o capitulo
                        {
                            if (lib.validac("desc_gru", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if(e.ColumnIndex == 3)                      // valida modelo
                        {
                            /*
                            if(lib.validac("desc_mod", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                MessageBox.Show("El valor no es valido en la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            */
                            if(e.FormattedValue.ToString().Length != 3)
                            {
                                MessageBox.Show("El valor debe tener 3 dígitos", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 4)           // valida madera
                        {
                            if (lib.validac("desc_mad", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 5)           // valida tipologia
                        {
                            if (lib.validac("desc_tip", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 6)           // valida detalle 1
                        {
                            if (lib.validac("desc_dt1", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        if(e.ColumnIndex == 7)          // valida acabado
                        {
                            if (lib.validac("desc_est", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        // id,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,nombr,medid,umed,soles,soles2018
                        if(e.ColumnIndex == 8)          // valida detalle 2
                        {
                            if (lib.validac("desc_dt2", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        if(e.ColumnIndex == 9)          // valida detalle 3
                        {
                            if (lib.validac("desc_dt3", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
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
        }
        private void advancedDataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dataGridView2.HorizontalScrollingOffset = e.NewValue;
            }
        }
        private void advancedDataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView2.ColumnCount > 1 && advancedDataGridView1.Rows.Count > 1)
            {
                dataGridView2.Columns[e.Column.Index].Width = e.Column.Width;
            }
        }
        #endregion
    }
}
