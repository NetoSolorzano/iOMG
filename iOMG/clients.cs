using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class clients : Form
    {
        static string nomform = "clients"; // nombre del formulario
        string asd = iOMG.Program.vg_user;   // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        static string nomtab = "anagrafiche";   // idcategoria='CLI' -> vista anag_cli
        public int totfilgrid, cta;      // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string img_btN = "";
        string img_btE = "";
        string img_btA = "";            // anula = bloquea
        string img_bti = "";            // imagen boton inicio
        string img_bts = "";            // imagen boton siguiente
        string img_btr = "";            // imagen boton regresa
        string img_btf = "";            // imagen boton final
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string vapadef = "";            // variable pais por defecto para los clientes
        libreria lib = new libreria();
        AutoCompleteStringCollection paises = new AutoCompleteStringCollection();       // autocompletado paises
        AutoCompleteStringCollection departamentos = new AutoCompleteStringCollection();// autocompletado departamentos
        AutoCompleteStringCollection provincias = new AutoCompleteStringCollection();   // autocompletado provincias
        AutoCompleteStringCollection distritos = new AutoCompleteStringCollection();    // autocompletado distritos
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();

        public clients()
        {
            InitializeComponent();
        }
        private void clients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void clients_Load(object sender, EventArgs e)
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
            dataload();
            grilla();
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = true;
            comboBox1.SelectedIndex = -1;
            tabControl1.SelectedTab = tabgrilla;
            advancedDataGridView1.Enabled = false;
            autopais();                                     // autocompleta paises
            autodepa();                                     // autocompleta departamentos
            //autoprov();                                     // autocompleta provincias
            //autodist();                                     // autocompleta distritos
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
            Bt_anul.Image = Image.FromFile(img_btA);
            Bt_close.Image = Image.FromFile(img_btq);
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // autocompletados
            textBox5.AutoCompleteMode = AutoCompleteMode.Suggest;           // paises
            textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;  // paises
            textBox5.AutoCompleteCustomSource = paises;                     // paises
            textBox7.AutoCompleteMode = AutoCompleteMode.Suggest;           // departamentos
            textBox7.AutoCompleteSource = AutoCompleteSource.CustomSource;  // departamentos
            textBox7.AutoCompleteCustomSource = departamentos;              // departamentos
            textBox8.AutoCompleteMode = AutoCompleteMode.Suggest;           // provincias
            textBox8.AutoCompleteSource = AutoCompleteSource.CustomSource;  // provincias
            textBox8.AutoCompleteCustomSource = provincias;                 // provincias
            textBox9.AutoCompleteMode = AutoCompleteMode.Suggest;           // distritos
            textBox9.AutoCompleteSource = AutoCompleteSource.CustomSource;  // distritos
            textBox9.AutoCompleteCustomSource = distritos;                  // distritos
            // longitudes maximas de campos
            textBox5.MaxLength = 3;           // pais
            textBox5.CharacterCasing = CharacterCasing.Upper;
            textBox4.MaxLength = 100;           // nombre
            textBox6.MaxLength = 100;           // direccion
            textBox13.MaxLength = 6;            // ubigeo
            textBox10.MaxLength = 15;           // telef. 1
            textBox11.MaxLength = 15;           // telef. 2
            textBox12.MaxLength = 50;          // correo electr.
        }
        private void grilla()                   // arma la grilla
        {
            // IDAnagrafica,tipdoc,RUC,RazonSocial,concat(trim(Direcc1),' ',trim(Direcc2)),depart,Provincia,Localidad,NumeroTel1,NumeroTel2,EMail,pais,ubigeo,estado
            //            0,     1,  2,          3,                                      4,     5,        6,        7,         8,         9,   10,  11,    12,    13
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
            advancedDataGridView1.Columns[1].HeaderText = "TipoDoc";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 60;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = false;           // lectura o no
            advancedDataGridView1.Columns[1].Tag = "validaSI";
            advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // numero de documento
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Documento";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 80;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = false;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaSI";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre
            advancedDataGridView1.Columns[3].Visible = true;       
            advancedDataGridView1.Columns[3].HeaderText = "Nombre";
            advancedDataGridView1.Columns[3].Width = 150;
            advancedDataGridView1.Columns[3].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[3].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // direccion
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Dirección";
            advancedDataGridView1.Columns[4].Width = 150;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // departamento
            advancedDataGridView1.Columns[5].Visible = true;       
            advancedDataGridView1.Columns[5].HeaderText = "Departamento";
            advancedDataGridView1.Columns[5].Width = 100;
            advancedDataGridView1.Columns[5].ReadOnly = false;
            advancedDataGridView1.Columns[5].Tag = "validaSI";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // provincia
            advancedDataGridView1.Columns[6].Visible = true;       
            advancedDataGridView1.Columns[6].HeaderText = "Provincia";
            advancedDataGridView1.Columns[6].Width = 100;
            advancedDataGridView1.Columns[6].ReadOnly = true;
            advancedDataGridView1.Columns[6].Tag = "validaNO";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // distrito
            advancedDataGridView1.Columns[7].Visible = true;
            advancedDataGridView1.Columns[7].HeaderText = "Distrito";
            advancedDataGridView1.Columns[7].Width = 100;
            advancedDataGridView1.Columns[7].ReadOnly = true;
            advancedDataGridView1.Columns[7].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // telefono 1
            advancedDataGridView1.Columns[8].Visible = true;
            advancedDataGridView1.Columns[8].HeaderText = "Teléfono1";
            advancedDataGridView1.Columns[8].Width = 70;
            advancedDataGridView1.Columns[8].ReadOnly = true;
            advancedDataGridView1.Columns[8].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // telefono 2
            advancedDataGridView1.Columns[9].Visible = true;
            advancedDataGridView1.Columns[9].HeaderText = "Teléfono2";
            advancedDataGridView1.Columns[9].Width = 70;
            advancedDataGridView1.Columns[9].ReadOnly = false;
            advancedDataGridView1.Columns[9].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Correo electrónico
            advancedDataGridView1.Columns[10].Visible = true;
            advancedDataGridView1.Columns[10].HeaderText = "Correo Electrónico";
            advancedDataGridView1.Columns[10].Width = 120;
            advancedDataGridView1.Columns[10].ReadOnly = false;
            advancedDataGridView1.Columns[10].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // pais de procedencia
            advancedDataGridView1.Columns[11].Visible = true;
            advancedDataGridView1.Columns[11].HeaderText = "País Origen";
            advancedDataGridView1.Columns[11].Width = 100;
            advancedDataGridView1.Columns[11].ReadOnly = false;
            advancedDataGridView1.Columns[11].Tag = "validaSI";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // ubigeo
            advancedDataGridView1.Columns[12].Visible = false;
            // estado, bloqueado o no
            advancedDataGridView1.Columns[13].Visible = false;
        }
        private void jalainfo()                 // obtiene datos de imagenes
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
                        if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
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
                    if (row["campo"].ToString() == "pais" && row["param"].ToString() == "default") vapadef = row["valor"].ToString().Trim();         // pais por defecto
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
        public void jalaoc(string campo)        // jala datos id o ????
        {
            // IDAnagrafica,tipdoc,RUC,RazonSocial,concat(trim(Direcc1),' ',trim(Direcc2)),depart,Provincia,Localidad,NumeroTel1,NumeroTel2,EMail,pais,ubigeo,estado
            //            0,     1,  2,          3,                                      4,     5,        6,        7,         8,         9,   10,  11,    12,    13
            if (campo == "tx_idr" && tx_idr.Text.Trim() != "")
            {
                textBox1.Text = "";   // codigo
                textBox2.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();   // tipo de documento
                textBox3.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();   // # documento
                textBox4.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();   // nombre
                textBox5.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[11].Value.ToString();   // pais
                textBox6.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();   // direccion
                textBox7.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString();   // departamento
                textBox8.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString();   // provincia
                textBox9.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[7].Value.ToString();   // distrito
                textBox10.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[8].Value.ToString();   // teléfono 1
                textBox11.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString();   // teléfono 2
                textBox12.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[10].Value.ToString();   // correo electrónico
                textBox13.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[12].Value.ToString();   // ubigeo
                if (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[13].Value.ToString() == "1") checkBox1.Checked = true;
                comboBox1.SelectedValue = textBox2.Text;
            }
        }
        public void dataload()                  // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            tabControl1.SelectedTab = tabreg;
            // datos de los clients
            string datgri = "select IDAnagrafica,tipdoc,RUC,RazonSocial,concat(trim(Direcc1),' ',trim(Direcc2))," +
                "depart,Provincia,Localidad,NumeroTel1,NumeroTel2,EMail,pais,ubigeo,estado from anag_cli";
            MySqlCommand cdg = new MySqlCommand(datgri, conn);
            MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
            dtg.Clear();
            dag.Fill(dtg);
            dag.Dispose();
            //  datos para el combobox de tipo de documento
            comboBox1.Items.Clear();
            string datuse = "select idcodice,descrizionerid,codigo from desc_doc where numero=@bloq";
            MySqlCommand cdu = new MySqlCommand(datuse, conn);
            cdu.Parameters.AddWithValue("@bloq", 1);
            MySqlDataAdapter dacu = new MySqlDataAdapter(cdu);
            dtu.Clear();
            dacu.Fill(dtu);
            comboBox1.DataSource = dtu;
            comboBox1.DisplayMember = "descrizionerid";
            comboBox1.ValueMember = "idcodice";
            //
            dacu.Dispose();
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

        #region autocompletados
        private void autopais()
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                string consulta = "select distinct descrizionerid from desc_pai order by descrizionerid asc";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                try
                {
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            paises.Add(dr["descrizionerid"].ToString());
                        }
                    }
                    dr.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener relación de paises", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        private void autodepa()                 // se jala en el load
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string consulta = "select nombre from ubigeos where depart<>'00' and provin='00' and distri='00'";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                try
                {
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            departamentos.Add(dr["nombre"].ToString());
                        }
                    }
                    dr.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener relación de departamentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void autoprov()                 // se jala despues de ingresado el departamento
        {
            if (textBox13.Text.Trim() != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select nombre from ubigeos where depart=@dep and provin<>'00' and distri='00'";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@dep", textBox13.Text.Substring(0, 2));
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                provincias.Add(dr["nombre"].ToString());
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener relación de provincias", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void autodist()                 // se jala despues de ingresado la provincia
        {
            if (textBox13.Text.Trim() != "" && textBox8.Text.Trim() != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select nombre from ubigeos where depart=@dep and provin=@prov and distri<>'00'";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@dep", textBox13.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@prov", (textBox13.Text.Length > 2)? textBox13.Text.Substring(2, 2):"  ");
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                distritos.Add(dr["nombre"].ToString());
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener relación de distritos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        #endregion autocompletados

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
            comboBox1.SelectedIndex = -1;
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
                    comboBox1.SelectedIndex = -1;
                }
            }
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            //if (textBox1.Text.Trim() == "")
            //{
            //    MessageBox.Show("Ingrese el código", " Error! ");
            //    textBox1.Focus();
            //    return;
            //}
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de documento", " Error! ");
                textBox2.Focus();
                return;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el número de documento", " Error! ");
                textBox3.Focus();
                return;
            }
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre o razón social", " Error! ");
                textBox4.Focus();
                return;
            }
            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre el país de origen", " Error! ");
                textBox5.Focus();
                return;
            }
            if (textBox6.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la dirección", " Error! ");
                textBox6.Focus();
                return;
            }
            if (textBox13.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese ubigeo correcto", " Error! ");
                textBox13.Focus();
                return;
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            if (modo == "NUEVO")
            {
                if (tx_idr.Text.Trim() == "")
                {
                    var aa = MessageBox.Show("Confirma que desea crear al cliente?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        if (graba() == true)
                        {
                            DataRow dtr = dtg.NewRow();
                            // IDAnagrafica,tipdoc,RUC,RazonSocial,concat(trim(Direcc1),' ',trim(Direcc2)),depart,Provincia,Localidad,NumeroTel1,NumeroTel2,EMail,pais,ubigeo,estado
                            dtr["IDAnagrafica"] = textBox1.Text;
                            dtr["tipdoc"] = textBox2.Text;
                            dtr["RUC"] = textBox3.Text;
                            dtr["RazonSocial"] = textBox4.Text.Trim();
                            dtr[4] = textBox6.Text.Trim();
                            dtr["depart"] = textBox7.Text;
                            dtr["Provincia"] = textBox8.Text;
                            dtr["Localidad"] = textBox9.Text;
                            dtr["NumeroTel1"] = textBox10.Text;
                            dtr["NumeroTel2"] = textBox11.Text;
                            dtr["EMail"] = textBox12.Text;
                            dtr["pais"] = textBox5.Text;
                            dtr["ubigeo"] = textBox13.Text;
                            dtr["estado"] = (checkBox1.Checked == true) ? 1 : 0;
                            dtg.Rows.Add(dtr);
                        }
                    }
                    else
                    {
                        textBox1.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Los datos no son nuevos", "Verifique duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                if(textBox7.Text.Trim() == "")
                {
                    textBox7.Focus();
                    return;
                }
                if (textBox8.Text.Trim() == "")
                {
                    textBox8.Focus();
                    return;
                }
                if (textBox9.Text.Trim() == "")
                {
                    textBox9.Focus();
                    return;
                }
                if (textBox13.Text.Length < 6)
                {
                    MessageBox.Show("Falta información de ubigeo o es incorrecta", "Confirme dpto, prov. o distrito");
                    textBox8.Focus();
                    return;
                }
                var aa = MessageBox.Show("Confirma que desea modificar el cliente?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (textBox5.Text.Trim() == "") textBox5.Text = vapadef;
                    edita();
                    //
                    foreach(DataRow row in dtg.Rows)
                    {
                        if (row["idanagrafica"].ToString().Trim() == tx_idr.Text.Trim())
                        {
                            row["tipdoc"] = textBox2.Text;
                            row["RUC"] = textBox3.Text;
                            row["RazonSocial"] = textBox4.Text.Trim();
                            row[4] = textBox6.Text.Trim();
                            row["depart"] = textBox7.Text;
                            row["Provincia"] = textBox8.Text;
                            row["Localidad"] = textBox9.Text;
                            row["NumeroTel1"] = textBox10.Text;
                            row["NumeroTel2"] = textBox11.Text;
                            row["EMail"] = textBox12.Text;
                            row["pais"] = textBox5.Text;
                            row["ubigeo"] = textBox13.Text;
                            row["estado"] = (checkBox1.Checked == true) ? 1 : 0;
                        }
                    }
                }
                else
                {
                    textBox1.Focus();
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
                textBox5.Focus();
                //dataload();
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
                    string inserta = "insert into anagrafiche (" +
                        "tipdoc,RUC,RazonSocial,Direcc1,Direcc2,depart,Provincia,Localidad,NumeroTel1,NumeroTel2,EMail,pais,ubigeo,codigo,estado,idcategoria) " +
                        "values (@tidoc,@nudoc,@raso,@dir1,@dir2,@depa,@prov,@dist,@tel1,@tel2,@mail,@pais,@ubig,@codi,@bloq,@cate)";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@tidoc", textBox2.Text);
                    micon.Parameters.AddWithValue("@nudoc", textBox3.Text);
                    micon.Parameters.AddWithValue("@raso", textBox4.Text);
                    micon.Parameters.AddWithValue("@dir1", textBox6.Text);
                    micon.Parameters.AddWithValue("@dir2", (textBox6.Text.Trim().Length > 50) ? textBox6.Text.Substring(50, (textBox6.Text.Trim().Length - 50)) : "");
                    micon.Parameters.AddWithValue("@depa", textBox7.Text);
                    micon.Parameters.AddWithValue("@prov", textBox8.Text);
                    micon.Parameters.AddWithValue("@dist", textBox9.Text);
                    micon.Parameters.AddWithValue("@tel1", textBox10.Text);
                    micon.Parameters.AddWithValue("@tel2", textBox11.Text);
                    micon.Parameters.AddWithValue("@mail", textBox12.Text);
                    micon.Parameters.AddWithValue("@pais", textBox5.Text);
                    micon.Parameters.AddWithValue("@ubig", textBox13.Text);
                    micon.Parameters.AddWithValue("@codi", textBox1.Text);
                    micon.Parameters.AddWithValue("@bloq", (checkBox1.Checked == true) ? "1" : "0");
                    micon.Parameters.AddWithValue("@cate", "CLI");                  // en la base de datos hay un trigger que actualiza el campo "codigo" con
                    micon.ExecuteNonQuery();                                        // la letra "C" + id del registro, C=cliente
                    //
                    string lectura = "select last_insert_id()";
                    micon = new MySqlCommand(lectura, conn);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox1.Text = dr.GetString(0);
                        retorna = true;
                    }
                    dr.Close();
                }
                catch(MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en insertar cliente");
                    Application.Exit();
                    return retorna;
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
                    string inserta = "update anagrafiche set tipdoc=@tidoc,RUC=@nudoc,RazonSocial=@raso,Direcc1=@dir1," +
                        "Direcc2=@dir2,depart=@depa,Provincia=@prov,Localidad=@dist,NumeroTel1=@tel1,NumeroTel2=@tel2," +
                        "EMail=@mail,pais=@pais,ubigeo=@ubig,estado=@bloq where idanagrafica=@idan";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@tidoc", textBox2.Text);
                    micon.Parameters.AddWithValue("@nudoc", textBox3.Text);
                    micon.Parameters.AddWithValue("@raso", textBox4.Text);
                    micon.Parameters.AddWithValue("@dir1", textBox6.Text);
                    micon.Parameters.AddWithValue("@dir2", (textBox6.Text.Trim().Length > 50) ? textBox6.Text.Substring(50, (textBox6.Text.Trim().Length - 50)) : "");
                    micon.Parameters.AddWithValue("@depa", textBox7.Text);
                    micon.Parameters.AddWithValue("@prov", textBox8.Text);
                    micon.Parameters.AddWithValue("@dist", textBox9.Text);
                    micon.Parameters.AddWithValue("@tel1", textBox10.Text);
                    micon.Parameters.AddWithValue("@tel2", textBox11.Text);
                    micon.Parameters.AddWithValue("@mail", textBox12.Text);
                    micon.Parameters.AddWithValue("@pais", textBox5.Text);
                    micon.Parameters.AddWithValue("@ubig", textBox13.Text);
                    //micon.Parameters.AddWithValue("@codi", textBox1.Text);
                    micon.Parameters.AddWithValue("@bloq", (checkBox1.Checked == true)? "1":"0");
                    micon.Parameters.AddWithValue("@idan", tx_idr.Text);
                    micon.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en modificar el cliente");
                    Application.Exit();
                    return;
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
        private void textBox1_Leave(object sender, EventArgs e)         // codigo cliente
        {
            /*  validamos segun el modo
            */
        }
        private void textBox7_Leave(object sender, EventArgs e)         // departamento, jala provincia
        {
            if(textBox7.Text != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select depart from ubigeos where trim(nombre)=@dep and depart<>'00' and provin='00' and distri='00'";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@dep", textBox7.Text.Trim());
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                textBox13.Text = dr.GetString(0).Trim();
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener codigo de departamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                autoprov();
            }
        }
        private void textBox8_Leave(object sender, EventArgs e)         // provincia de un departamento, jala distrito
        {
            if(textBox8.Text != "" && textBox7.Text.Trim() != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select provin from ubigeos where trim(nombre)=@prov and depart=@dep and provin<>'00' and distri='00'";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@dep", textBox13.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@prov", textBox8.Text.Trim());
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                if (textBox13.Text.Trim().Length == 6) textBox13.Text = textBox13.Text.Substring(0,2) + dr.GetString(0).Trim() + textBox13.Text.Substring(4, 2);
                                if (textBox13.Text.Trim().Length < 6) textBox13.Text = textBox13.Text.Substring(0, 2) + dr.GetString(0).Trim();
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener codigo de provincia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                autodist();
            }
        }
        private void textBox9_Leave(object sender, EventArgs e)
        {
            if(textBox9.Text.Trim() != "" && textBox8.Text.Trim() != "" && textBox7.Text.Trim() != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select distri from ubigeos where trim(nombre)=@dist and depart=@dep and provin=@prov and distri<>'00'";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@dep", textBox13.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@prov", (textBox13.Text.Length > 2)? textBox13.Text.Substring(2, 2):"  ");
                    micon.Parameters.AddWithValue("@dist", textBox9.Text.Trim());
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                if(textBox13.Text.Trim().Length >= 4) textBox13.Text = textBox13.Text.Trim().Substring(0,4) + dr.GetString(0).Trim();
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener codigo de distrito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void textBox12_Leave(object sender, EventArgs e)        // correo electrónico
        {
            if(textBox12.Text != "")
            {
                if(lib.email_bien_escrito(textBox12.Text.Trim()) == false)
                {
                    MessageBox.Show("El formato del correo electrónico esta mal", "Atención - Corrija", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    textBox12.Focus();
                    return;
                }
            }
        }
        private void textBox13_Leave(object sender, EventArgs e)        // ubigeo
        {
            if(textBox13.Text.Trim() != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select d.nombre,b.nombre,c.nombre from ubigeos a " +
                        "left join ubigeos b on concat(b.depart, b.provin)= concat(a.depart, a.provin) and b.distri = '00' " +
                        "left join ubigeos c on concat(c.depart, c.provin, c.distri)= concat(a.depart, a.provin, a.distri) " +
                        "left join (select nombre, depart from ubigeos where depart<>'00' and provin = '00' and distri = '00')d " +
                        "on d.depart = a.depart " +
                        "where concat(a.depart, a.provin, a.distri)=@ubi";
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@ubi", textBox13.Text);
                    try
                    {
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            while (dr.Read())
                            {
                                textBox7.Text = dr.GetString(0).Trim();
                                textBox8.Text = dr.GetString(1).Trim();
                                textBox9.Text = dr.GetString(2).Trim();
                            }
                        }
                        dr.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error en obtener codigo de distrito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor!", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void textBox3_Leave(object sender, EventArgs e)         // número de documento
        {
            if (textBox3.Text.Trim() != "" && tx_mld.Text.Trim() != "")
            {
                if (textBox3.Text.Trim().Length != Int16.Parse(tx_mld.Text))
                {
                    MessageBox.Show("El número de caracteres para" + Environment.NewLine +
                        "su tipo de documento debe ser: " + tx_mld.Text, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    textBox3.Focus();
                    return;
                }
                if (Tx_modo.Text == "NUEVO")    //  || Tx_modo.Text == "EDITAR"
                {
                    foreach (DataRow row in dtg.Rows)   // && row["tipdoc"].ToString() == textBox2.Text.Trim()  && Tx_modo.Text == "NUEVO"
                    {
                        if (row["RUC"].ToString().Trim() == textBox3.Text.Trim())
                        {
                            MessageBox.Show("Ya existe el cliente!", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            textBox3.Focus();
                            return;
                        }
                    }
                }
            }
            if (textBox3.Text.Trim() != "" && tx_mld.Text.Trim() == "")
            {
                comboBox1.Focus();
            }
        }
        private void comboBox1_Leave(object sender, EventArgs e)
        {
            textBox3.Focus();
        }
        #endregion leaves;

        #region botones_de_comando_y_clients  
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
                //if (Convert.ToString(row["btn5"]) == "S")
                //{
                //    this.Bt_print.Visible = true;
                //}
                //else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                //if (Convert.ToString(row["btn4"]) == "S")
                //{
                //    this.Bt_ver.Visible = true;
                //}
                //else { this.Bt_ver.Visible = false; }
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
            textBox1.Focus();
            limpiar(this);
            limpiapag(tabreg);
            limpia_otros();
            limpia_combos();
            textBox1.ReadOnly = true;
            textBox5.Text = vapadef;
            textBox5.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            //string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                //codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
            }
            tabControl1.SelectedTab = tabgrilla;
            escribe(this);
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabreg);
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
            this.Tx_modo.Text = "IMPRIMIR";
            this.button1.Image = Image.FromFile("print48");
            this.textBox1.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)          // pone todos los clients en N
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
        #endregion botones;
        // clients para habilitar los botones de comando
        #endregion botones_de_comando  ;

        #region comboboxes
        // selected index del combobox de usuarios
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                DataRow row = ((DataTable)comboBox1.DataSource).Rows[comboBox1.SelectedIndex];
                textBox2.Text = (string)row["idcodice"];
                tx_mld.Text = (string)row["codigo"];
            }
            else
            {
                textBox2.Text = "";
            }
        }
        #endregion comboboxes

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
                string idr = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_rind.Text = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabreg;
                limpiar(this);
                limpia_otros();
                limpia_combos();
                tx_idr.Text = idr;
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
                        if(e.ColumnIndex == 1)                         // valida tipo de documento
                        {
                            if (lib.validac("desc_doc", "idcodice", e.FormattedValue.ToString()) == true)
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
                        if(e.ColumnIndex == 2)                      // valida # documento no debe existir
                        {
                            if(lib.validac("anag_cli", "ruc", e.FormattedValue.ToString()) == true)
                            {
                                MessageBox.Show("El valor se repite en la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            else
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                        }
                        if (e.ColumnIndex == 5)           // valida dpto ... no se edita en la grilla
                        {
                            if (lib.validaub("ubigeos", "nombre", e.FormattedValue.ToString(), "xx", "00", "00") == true)
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
                        if (e.ColumnIndex == 6)           // valida provincia ... no se edita en la grilla
                        {
                            /*
                            if (lib.validaub("ubigeos", "nombre", e.FormattedValue.ToString(), ) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            */
                        }
                        if (e.ColumnIndex == 7)           // valida distrito ... no se edita en la grilla
                        {
                            /*
                            if (lib.validaub("ubigeos", "nombre", "distri", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                                e.Cancel = true;
                            }
                            */
                        }
                        if(e.ColumnIndex == 11)
                        {
                            if (lib.validac("desc_pai", "idcodice", e.FormattedValue.ToString()) == false)
                            {
                                MessageBox.Show("No existe el código de país ingresado", "Atención - Corrija");
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
        #endregion
    }
}
