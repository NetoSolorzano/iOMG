using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;     // ok
using MySql.Data.MySqlClient;   // ok
using System.Configuration;     // ok
using ClosedXML.Excel;          // ok
using System.Collections;       // ok

namespace iOMG
{
    public partial class almgestion : Form
    {
        string valant = "";
        string valnue = "";
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        List<bool> marcas = new List<bool>();
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        #region variables
        string asd = iOMG.Program.vg_user;   // usuario conectado al sistema
        // para la impresion
        StringFormat strFormat;                                 // Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();             // Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();            // Used to save column widths
        int iCellHeight = 0;                                    // Used to get/set the datagridview cell height
        int iTotalWidth = 0;                                    //
        int iRow = 0;                                           // Used as counter
        bool bFirstPage = false;                                // Used to check whether we are printing first page
        bool bNewPage = false;                                  // Used to check whether we are printing a new page
        int iHeaderHeight = 0;                                  // Used for the header height
        int totcolv = 0;                                        // total columnas visibles
        string nomform = "almgestion";                          //
        string img_btN = "";
        string img_btE = "";
        string img_btA = "";
        string img_bti = "";
        string img_bts = "";
        string img_btr = "";
        string img_btf = "";
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string vcprecio = "";                                   // nombre del campo precio de la tabla "alm2018"
        #endregion

        public almgestion()
        {
            InitializeComponent();
        }

        private void almgestion_Load(object sender, EventArgs e)
        {
            jaladat();
            advancedDataGridView1.DataSource = dt;
            grilla();
            init();
            cellsum(0);
            cvc();
            rb_estan.Checked = true;
        }
        private void pan_inicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void jalainfo()                     // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in (@nofo,@noga)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");   // nomform
                micon.Parameters.AddWithValue("@noga", nomform);   // 
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "main" && row["campo"].ToString() == "imagenes")
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
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "precio" && row["param"].ToString() == "campo") vcprecio = row["valor"].ToString().Trim();         // campo de precio actual
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
        private void jaladat()                      // jala almacen 
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string sqlCmd = "select distinct a.marca,a.id,a.codalm,a.fechop,a.codig,a.capit,a.model,a.mader,a.tipol,a.deta1,a.acaba,a.talle,a.deta2,a.deta3,a.juego," +
                    "ifnull(a.nombr,'') as nombr,ifnull(a.medid,'') as medid,a.reserva,a.contrat,a.salida,a.evento,a.almdes," +
                    "ifnull(b.umed,'') as umed,ifnull(a.soles2018,0) as soles2018 " +
                    "from almloc a " +
                    "left join (select * from items group by capit,model,tipol,deta1,acaba) b " +
                    "on b.capit=a.capit and b.model=a.model and b.tipol=a.tipol and b.deta1=a.deta1 and b.acaba=a.acaba ";  // ifnull(a.soles,0) as soles,
                MySqlCommand micon = new MySqlCommand(sqlCmd, cn);
                micon.CommandTimeout = 300;
                MySqlDataAdapter adr = new MySqlDataAdapter(micon);
                adr.SelectCommand.CommandType = CommandType.Text;
                adr.Fill(dt); //opens and closes the DB connection automatically !! (fetches from pool)
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en jaladat");
                cn.Dispose(); // return connection to pool
                cn.Close();
                Application.Exit();
            }
            cn.Close();
        }
        private void grilla()                       // arma la grilla1
        {
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn checkColum2 = new DataGridViewCheckBoxColumn();
            //DataGridViewCheckBoxColumn checkmarca = new DataGridViewCheckBoxColumn();
            advancedDataGridView1.AllowUserToAddRows = false;
            //
            advancedDataGridView1.Columns[0].Width = 30;            // marca
            advancedDataGridView1.Columns[1].Width = 40;            // id
            advancedDataGridView1.Columns[1].ReadOnly = true;
            advancedDataGridView1.Columns[2].Width = 60;            // almacen
            advancedDataGridView1.Columns[2].ReadOnly = false;
            advancedDataGridView1.Columns[3].Width = 70;            // fecha
            advancedDataGridView1.Columns[3].ReadOnly = true;
            advancedDataGridView1.Columns[4].Width = 154;            // código
            advancedDataGridView1.Columns[4].ReadOnly = true;
            advancedDataGridView1.Columns[5].Width = 20;             // capital
            advancedDataGridView1.Columns[5].ReadOnly = false;
            advancedDataGridView1.Columns[6].Width = 30;             // modelo
            advancedDataGridView1.Columns[6].ReadOnly = false;
            advancedDataGridView1.Columns[7].Width = 20;             // madera
            advancedDataGridView1.Columns[7].ReadOnly = false;
            advancedDataGridView1.Columns[8].Width = 30;             // tipologia
            advancedDataGridView1.Columns[8].ReadOnly = false;
            advancedDataGridView1.Columns[9].Width = 30;            // detalle 1
            advancedDataGridView1.Columns[9].ReadOnly = false;
            advancedDataGridView1.Columns[10].Width = 20;            // acabado
            advancedDataGridView1.Columns[10].ReadOnly = false;
            advancedDataGridView1.Columns[11].Width = 30;            // taller
            advancedDataGridView1.Columns[11].ReadOnly = false;
            advancedDataGridView1.Columns[12].Width = 40;            // detalle 2
            advancedDataGridView1.Columns[12].ReadOnly = false;
            advancedDataGridView1.Columns[13].Width = 40;           // detalle 3
            advancedDataGridView1.Columns[13].ReadOnly = false;
            advancedDataGridView1.Columns[14].Width = 40;           // juego
            advancedDataGridView1.Columns[14].ReadOnly = false;
            advancedDataGridView1.Columns[15].Width = 190;          // nombre
            advancedDataGridView1.Columns[15].ReadOnly = false;
            advancedDataGridView1.Columns[16].Width = 70;            // medidas
            advancedDataGridView1.Columns[16].ReadOnly = false;
            // columnas vista reducida false
            checkColumn.Name = "chkreserva";
            checkColumn.HeaderText = "";
            checkColumn.Width = 30;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10;
            advancedDataGridView1.Columns.Insert(17, checkColumn);
            //
            advancedDataGridView1.Columns[18].Width = 30;           // id reserva
            advancedDataGridView1.Columns[18].ReadOnly = true;
            advancedDataGridView1.Columns[19].Width = 70;           // contrato
            advancedDataGridView1.Columns[19].ReadOnly = true;
            //
            checkColum2.Name = "chksalida";
            checkColum2.HeaderText = "";
            checkColum2.Width = 30;
            checkColum2.ReadOnly = false;
            checkColum2.FillWeight = 10;
            advancedDataGridView1.Columns.Insert(20, checkColum2);
            //
            advancedDataGridView1.Columns[21].Width = 30;           // id salida
            advancedDataGridView1.Columns[21].ReadOnly = true;
            advancedDataGridView1.Columns[22].Width = 70;           // evento
            advancedDataGridView1.Columns[22].ReadOnly = true;
            advancedDataGridView1.Columns[23].Width = 70;           // almacen destino
            advancedDataGridView1.Columns[23].ReadOnly = true;
            advancedDataGridView1.Columns[24].Width = 50;           // unidad de medida
            advancedDataGridView1.Columns[24].ReadOnly = false;
            advancedDataGridView1.Columns[25].Width = 70;           // precio soles desde el 2018
            advancedDataGridView1.Columns[25].ReadOnly = false;
            advancedDataGridView1.Columns[25].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void init()                         // inicializa ancho de columnas grilla de filtros
        {
            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_btA);
            Bt_close.Image = Image.FromFile(img_btq);
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            //
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.ColumnCount = (advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.ColumnCount;
            dataGridView1.ColumnCount = 0;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView2.Rows.Add();
            for (int i = 0; i < ((advancedDataGridView1.Rows.Count > 0) ? advancedDataGridView1.Rows[0].Cells.Count : advancedDataGridView1.Columns.Count); i++)
            {
                dataGridView2.Columns[i].Width = advancedDataGridView1.Columns[i].Width;
                dataGridView2.Columns[i].Name = advancedDataGridView1.Columns[i].Name;
                //
                DataGridViewCheckBoxColumn checkver = new DataGridViewCheckBoxColumn();
                checkver.Name = advancedDataGridView1.Columns[i].Name;    //"vc"+i.ToString();
                checkver.HeaderText = "";
                checkver.Width = advancedDataGridView1.Columns[i].Width;
                checkver.ReadOnly = false;
                checkver.FillWeight = 10;
                dataGridView1.Columns.Insert(i, checkver);
            }
            dataGridView1.Rows.Add();
            dataGridView2.Columns["id"].ReadOnly = true;
        }
        private void cvc()                          // checks de visualizacion de columnas
        {
            for (int i = 0; i <= advancedDataGridView1.Rows[0].Cells.Count - 1; i++)  // dataGridView1 -2
            {
                if (advancedDataGridView1.Columns[i].Visible == true)
                {
                    dataGridView1.Rows[0].Cells[i].Value = true;
                }
                else
                {
                    dataGridView1.Rows[0].Cells[i].Value = false;
                }
            }
        }
        private void llenagri()                     // llena la grilla 1 con datos de la tabla DT
        {
            //   
        }
        private void cellsum(int ind)               // suma la columna especificada
        {
            tx_tarti.Text = (advancedDataGridView1.Rows.Count).ToString();
            decimal b = 0;
            string qw = vcprecio;    //"soles2018";
            foreach (DataGridViewRow r in advancedDataGridView1.Rows)
            {
                if (r.Cells[qw].Value != null && r.Cells[qw].Value != DBNull.Value) b += Convert.ToDecimal(r.Cells[qw].Value);  // total precio con igv
            }
            tx_totprec.Text = b.ToString("###,###,##0.00");
        }


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

        private void Bt_add_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "IMPRIMIR";
            button1.Image = Image.FromFile("print48");
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
        }
        private void Bt_first_Click(object sender, EventArgs e)
        {

        }
        private void Bt_back_Click(object sender, EventArgs e)
        {

        }
        private void Bt_next_Click(object sender, EventArgs e)
        {

        }
        private void Bt_last_Click(object sender, EventArgs e)
        {

        }
        #endregion botones_de_comando  ;
    }
}
