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
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
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
        string img_btP = "";
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
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
            panel1.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            advancedDataGridView1.Enabled = false;
            bt_reserva.Enabled = false;
            bt_salida.Enabled = false;
            bt_borra.Enabled = false;
            //
            jaladat();
            advancedDataGridView1.DataSource = dt;
            grilla();
            init();
            cellsum(0);
            cvc();
            rb_estan.Checked = true;
            toolboton();
        }
        private void pan_inicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        #region funciones priopas del form
        private void jalainfo()                                                         // obtiene datos de imagenes
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
        private void jaladat()                                                          // jala almacen 
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
        private void grilla()                                                           // arma la grilla1
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
            advancedDataGridView1.Columns[5].ReadOnly = true;
            advancedDataGridView1.Columns[6].Width = 30;             // modelo
            advancedDataGridView1.Columns[6].ReadOnly = true;
            advancedDataGridView1.Columns[7].Width = 20;             // madera
            advancedDataGridView1.Columns[7].ReadOnly = true;
            advancedDataGridView1.Columns[8].Width = 30;             // tipologia
            advancedDataGridView1.Columns[8].ReadOnly = true;
            advancedDataGridView1.Columns[9].Width = 30;            // detalle 1
            advancedDataGridView1.Columns[9].ReadOnly = true;
            advancedDataGridView1.Columns[10].Width = 20;            // acabado
            advancedDataGridView1.Columns[10].ReadOnly = true;
            advancedDataGridView1.Columns[11].Width = 30;            // taller
            advancedDataGridView1.Columns[11].ReadOnly = true;
            advancedDataGridView1.Columns[12].Width = 40;            // detalle 2
            advancedDataGridView1.Columns[12].ReadOnly = true;
            advancedDataGridView1.Columns[13].Width = 40;           // detalle 3
            advancedDataGridView1.Columns[13].ReadOnly = true;
            advancedDataGridView1.Columns[14].Width = 40;           // juego
            advancedDataGridView1.Columns[14].ReadOnly = true;
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
        private void init()                                                             // inicializa ancho de columnas grilla de filtros
        {
            this.BackColor = Color.FromName(colback);                               // color de fondo
            panel1.BackColor = Color.FromName(colpage);                             // color de los pageframes
            advancedDataGridView1.BackgroundColor = Color.FromName(colgrid);        // color de las grillas
            dataGridView1.BackgroundColor = Color.FromName(colgrid);
            dataGridView2.BackgroundColor = Color.FromName(colgrid);
            toolStrip1.BackColor = Color.FromName(colstrp);                         // color del strip
            //
            jalainfo();
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_btA);
            Bt_close.Image = Image.FromFile(img_btq);
            bt_exc.Image = Image.FromFile(img_btexc);
            bt_prev.Image = Image.FromFile(img_btP);
            Bt_print.Image = Image.FromFile(img_btP);
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
        private void cvc()                                                              // checks de visualizacion de columnas
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
        private void llenagri()                                                         // llena la grilla 1 con datos de la tabla DT
        {
            //   
        }
        private void cellsum(int ind)                                                   // suma la columna especificada
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
        private void filtros(string expres)                                             // filtros de nivel superior
        {
            dv = new DataView(dt);
            dv.RowFilter = expres;
            dt = dv.ToTable();
            //advancedDataGridView1.Columns.Remove("marca");
            advancedDataGridView1.Columns.Remove("chkreserva");
            advancedDataGridView1.Columns.Remove("chksalida");
            advancedDataGridView1.DataSource = dt;
            grilla();
            cellsum(0);
            rb_redu_CheckedChanged(null, null);
            rb_todos_CheckedChanged(null, null);
        }
        private bool vali_alm(string codi)                                                  // valida almacen
        {
            bool retorna = false;
            string DB_CONN_STR0 = DB_CONN_STR;
            MySqlConnection cn0 = new MySqlConnection(DB_CONN_STR0);
            cn0.Open();
            try
            {
                string sqlCmd = "select count(*) from desc_alm where idcodice=@valm";
                MySqlCommand micon = new MySqlCommand(sqlCmd, cn0);
                micon.Parameters.AddWithValue("@valm", codi);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0)
                        {
                            dr.Close();
                            retorna = true;
                        }
                        else dr.Close();
                    }
                }
                else
                {
                    dr.Close();
                    retorna = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en vali alm");
                cn0.Close();
                cn0.Dispose(); // return connection to pool
                Application.Exit();
            }
            cn0.Close();
            cn0.Dispose(); // return connection to pool
            return retorna;
        }
        private bool vali_par(string nomcol, string valcol, string colcap)                      // valida existencia de dato en la maestra
        {                                                   // en consecuencia con la estructura de la maestra
            bool retorna = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                if (nomcol == "capit")  // valida que la letra exista
                {
                    string consulta = "select count(*) from items where capit=@cap";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cap", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                if (nomcol == "model")  // valida que el modelo exista dentro del grupo capital
                {
                    string consulta = "select count(*) from items where capit=@cap and model=@mod";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cap", colcap);
                    micon.Parameters.AddWithValue("@mod", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                if (nomcol == "mader")  // 
                {
                    string consulta = "select count(*) from desc_mad where idcodice=@mad";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@mad", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                if (nomcol == "tipol")
                {
                    string consulta = "select count(*) from items where capit=@cap and tipol=@tip";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cap", colcap);
                    micon.Parameters.AddWithValue("@tip", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                if (nomcol == "deta1")
                {
                    string consulta = "select count(*) from items where capit=@cap and deta1=@det1";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cap", colcap);
                    micon.Parameters.AddWithValue("@det1", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                if (nomcol == "deta2")
                {
                    string consulta = "select count(*) from items where capit=@cap and deta2=@det2";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cap", colcap);
                    micon.Parameters.AddWithValue("@det2", valcol);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                    }
                    dr.Close();
                }
                // hay validacion de deta3 ?????
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
                Application.Exit();
            }
            cn.Close();
            return retorna;
        }
        private void grabacam(int idm, string campo, string valor)                                  // graba el cambio en la tabla almloc y la maestra de items
        {
            string DB_CONN_STR1 = DB_CONN_STR;
            MySqlConnection cn0 = new MySqlConnection(DB_CONN_STR1);
            cn0.Open();
            try
            {
                string sqlCmd = "update almloc set " + campo + "=@val where id=@idm";   // debería deshabilitarse esto porque cualquier cambio en el codigo
                MySqlCommand micon = new MySqlCommand(sqlCmd, cn0);                     // afecta al kardex porque pasa a ser otro producto!
                micon.Parameters.AddWithValue("@val", valor);
                micon.Parameters.AddWithValue("@idm", idm);
                micon.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en grabacam");
                cn0.Close();
                cn0.Dispose(); // return connection to pool
                Application.Exit();
            }
            cn0.Close();
            cn0.Dispose(); // return connection to pool
        }
        private void jalareg(string cap, string mod, string tip, string det1, string aca, int id)       // jala datos de la maestra y actualiza la grilla
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string consulta = "select ifnull(b.nombr,'') as nombr,ifnull(b.medid,'') as medid," +
                    "ifnull(b.umed,'') as umed,ifnull(b.soles2018,0) as soles2018 " +
                    "from items b where b.capit=@cap and b.model=@mod and b.tipol=@tip and b.deta1=@det1 and b.acaba=@aca";  // a.capit,a.model,a.tipol,a.deta1,a.acaba
                MySqlCommand micon = new MySqlCommand(consulta, cn);    // ifnull(b.soles,0) as soles,
                micon.Parameters.AddWithValue("@cap", cap);
                micon.Parameters.AddWithValue("@mod", mod);
                micon.Parameters.AddWithValue("@tip", tip);
                micon.Parameters.AddWithValue("@det1", det1);
                micon.Parameters.AddWithValue("@aca", aca);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            advancedDataGridView1.Rows[id].Cells[dr.GetName(i)].Value = dr.GetString(i);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("No existe el código");
                }
                dr.Close();
            }
            catch (MySqlException ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error en actualizar datos de la grilla");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void grabaitems(string campo, string artic, string valor)               // graba en items el campo 
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string consulta = "update items set " + campo + "=@val where codig=@art";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                micon.Parameters.AddWithValue("@val", valor);
                micon.Parameters.AddWithValue("@art", artic);
                micon.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error en actualizar base de datos");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void busmarcas()                                                        // busca y guarda las marcas de visualizacion vertical
        {
            for (int i = 0; i < dataGridView1.Rows[0].Cells.Count - 2; i++)
            {
                marcas.Add((dataGridView1.Rows[0].Cells[i].Value.ToString() == "True") ? true : false);
            }
        }
        private void restauramar()                                                      // restaura las visualizaciones segun la marca
        {
            for (int i = 0; i <= dataGridView1.Rows[0].Cells.Count - 3; i++)
            {
                if (marcas.ElementAt(i).ToString() == "True")
                {
                    dataGridView1.Rows[0].Cells[i].Value = true;
                    dataGridView1.Columns[i].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[0].Cells[i].Value = false;
                    dataGridView1.Columns[i].Visible = false;
                    dataGridView2.Columns[i].Visible = false;
                    advancedDataGridView1.Columns[i].Visible = false;
                }
            }
        }
        private void marcash(string id, int valo)                                       // graba los checks de marcas horizontales en la tabla
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string consulta = "update almloc set marca=@mar where id=@idr";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                micon.Parameters.AddWithValue("@mar", valo);
                micon.Parameters.AddWithValue("@idr", id);
                micon.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error en actualizar marcas");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void selec()                                                            // pone color de seleccion si esta con check
        {
            for (int i = 0; i < advancedDataGridView1.Rows.Count - 1; i++)
            {
                if (advancedDataGridView1.Rows[i].Cells[advancedDataGridView1.Columns["marca"].Index].Value.ToString() == "True")
                {
                    advancedDataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                }
            }
        }
        private bool quitareserv(string idr, string ida, string contra)
        {
            bool retorna = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string nar = advancedDataGridView1.CurrentRow.Cells["capit"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["model"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["mader"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["tipol"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["deta1"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["acaba"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["deta2"].Value.ToString() +
                    advancedDataGridView1.CurrentRow.Cells["deta3"].Value.ToString();
                string actua = "update reservh a,reservd b set a.status=@vstat,b.almacen='' " +
                    "where a.idreservh=b.reservh and a.idreservh=@ptxres";
                MySqlCommand micon = new MySqlCommand(actua, cn);
                micon.Parameters.AddWithValue("@vstat", "ANULADO");
                micon.Parameters.AddWithValue("@ptxres", idr);
                micon.ExecuteNonQuery();
                //
                actua = "update almloc set reserva='',contrat='' " +
                    "where id=@ida";   // item=@nar and codalm=@ptxalm
                micon = new MySqlCommand(actua, cn);
                micon.Parameters.AddWithValue("@ida", ida);
                micon.ExecuteNonQuery();
                //
                actua = "UPDATE detacon SET saldo=saldo+@can " +
                    "where contratoh=@ptxcon and item=@nar";
                micon = new MySqlCommand(actua, cn);
                micon.Parameters.AddWithValue("@can", 1);
                micon.Parameters.AddWithValue("@ptxcon", contra);
                micon.Parameters.AddWithValue("@nar", nar);
                micon.ExecuteNonQuery();
                retorna = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
            }
            cn.Close();
            //
            return retorna;
        }
        private bool quitasalida(string idr, string ida)
        {
            bool retorna = false;
            // actualiza almloc
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string actua = "update almloc set evento='',almdes='',salida='' where id=@idr";
                MySqlCommand micon = new MySqlCommand(actua, cn);
                micon.Parameters.AddWithValue("@idr", ida);
                micon.ExecuteNonQuery();
                //
                retorna = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en la conexión");
                Application.Exit();
            }
            return retorna;
        }
        #endregion

        #region botones_click
        private void bt_borra_Click(object sender, EventArgs e)
        {
            int udt = 0;
            if (rb_estan.Checked == true)
            {
                udt = 1;
            }
            if (rb_redu.Checked == true) udt = 2;
            if (rb_todos.Checked == true) udt = 3;
            busmarcas();    // visualizacion de columnas
            dt.Rows.Clear();
            dataGridView2.Rows.Clear();
            dt.DefaultView.RowFilter = "";
            advancedDataGridView1.DataSource = null;
            advancedDataGridView1.Rows.Clear();
            //advancedDataGridView1.Columns.Remove("marca");
            advancedDataGridView1.Columns.Remove("chkreserva");
            advancedDataGridView1.Columns.Remove("chksalida");
            jaladat();
            advancedDataGridView1.DataSource = dt;
            grilla();
            init();
            cvc();
            cellsum(0);
            rb_estan.Checked = false;
            rb_redu.Checked = false;
            rb_todos.Checked = false;
            restauramar();
            selec();
            switch (udt)
            {
                case 1:
                    rb_estan.PerformClick();
                    break;
                case 2:
                    rb_redu.PerformClick();
                    break;
                case 3:
                    rb_todos.PerformClick();
                    break;
            }
        }
        private void bt_reserva_Click(object sender, EventArgs e)                       // reserva masiva
        {
            // primero validamos
            int fi = 0;
            for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)
            {
                if (advancedDataGridView1.Rows[i].Cells["marca"].FormattedValue.ToString() == "True")
                {
                    fi = fi + 1;
                }
            }
            if (fi > 1)
            {
                MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                cn.Open();
                try
                {
                    string trunca = "truncate tempo";
                    MySqlCommand micon = new MySqlCommand(trunca, cn);
                    micon.ExecuteNonQuery();
                    //
                    for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)   // le quitamos el -1
                    {
                        if (advancedDataGridView1.Rows[i].Cells["marca"].FormattedValue.ToString() == "True")
                        {
                            string id = advancedDataGridView1.Rows[i].Cells["id"].FormattedValue.ToString();
                            string co = advancedDataGridView1.Rows[i].Cells["capit"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["model"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["mader"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["tipol"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["deta1"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["acaba"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["deta2"].Value.ToString() +
                                advancedDataGridView1.Rows[i].Cells["deta3"].Value.ToString();
                            string no = advancedDataGridView1.Rows[i].Cells["nombr"].FormattedValue.ToString();
                            string ca = "1";
                            string al = advancedDataGridView1.Rows[i].Cells["codalm"].FormattedValue.ToString();
                            //
                            string inserta = "insert into tempo (ida,codigo,nombre,cant,almacen) values (@id,@co,@no,@ca,@al)";
                            micon = new MySqlCommand(inserta, cn);
                            micon.Parameters.AddWithValue("@id", id);
                            micon.Parameters.AddWithValue("@co", co);
                            micon.Parameters.AddWithValue("@no", no);
                            micon.Parameters.AddWithValue("@ca", ca);
                            micon.Parameters.AddWithValue("@al", al);
                            micon.ExecuteNonQuery();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error - no se pudo insertar");
                    Application.Exit();
                    return;
                }
                cn.Close();
                // vamos a llamar a movimas
                movimas resem = new movimas("reserva", "", "");    // modo,array,libre
                var result = resem.ShowDialog();
                if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu
                {
                    if (resem.retorno == true)
                    {
                        MySqlConnection cnx = new MySqlConnection(DB_CONN_STR);
                        cnx.Open();
                        try
                        {
                            string consulta = "select codigo,nombre,cant,almacen,idres,contrat,ida from tempo";
                            MySqlCommand micon = new MySqlCommand(consulta, cnx);
                            MySqlDataAdapter da = new MySqlDataAdapter(micon);      //
                            DataTable dtt = new DataTable();                        //
                            da.Fill(dtt);                                           // datatable del tempo
                            for(int y = 0; y < dtt.Rows.Count; y++)                 // for del tempo
                            {
                                DataRow row = dtt.Rows[y];                          // row del tempo
                                {
                                    // actualizamos el datagridview / datatable y almloc
                                    for (int i = 0; i < dt.Rows.Count; i++)         // for de la grilla
                                    {
                                        DataRow fila = dt.Rows[i];                  // row de la grilla
                                        if (fila[1].ToString() == row[6].ToString())// comparacion de id's
                                        {
                                            dt.Rows[i]["reserva"] = row[4].ToString();
                                            dt.Rows[i]["contrat"] = row[5].ToString();
                                            // actualizamos almloc
                                            string actua = "update almloc set reserva=@res,contrat=@con,marca=0 where id=@idr";
                                            MySqlCommand miact = new MySqlCommand(actua, cnx);
                                            miact.Parameters.AddWithValue("@res", row[4].ToString());
                                            miact.Parameters.AddWithValue("@con", row[5].ToString());
                                            miact.Parameters.AddWithValue("@idr", row[6].ToString());
                                            miact.ExecuteNonQuery();
                                            dt.Rows[i]["marca"] = 0;
                                        }
                                    }
                                }
                            }
                            consulta = "truncate tempo";
                            micon = new MySqlCommand(consulta, cnx);
                            micon.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error de conexión");
                            Application.Exit();
                            return;
                        }
                        cnx.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acción individual");
            }
        }
        private void bt_salida_Click(object sender, EventArgs e)
        {
            // primero validamos
            int fi = 0;
            for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)
            {
                if (advancedDataGridView1.Rows[i].Cells["marca"].FormattedValue.ToString() == "True")
                {
                    fi = fi + 1;
                }
            }
            if (fi > 1)
            {
                MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                cn.Open();
                try
                {
                    string trunca = "truncate tempo";
                    MySqlCommand micon = new MySqlCommand(trunca, cn);
                    micon.ExecuteNonQuery();
                    //
                    for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)
                    {
                        if (advancedDataGridView1.Rows[i].Cells["marca"].FormattedValue.ToString() == "True")
                        {
                            string id = advancedDataGridView1.Rows[i].Cells["id"].FormattedValue.ToString();
                            string co = advancedDataGridView1.Rows[i].Cells["codig"].FormattedValue.ToString();
                            string no = advancedDataGridView1.Rows[i].Cells["nombr"].FormattedValue.ToString();
                            string ca = "1";
                            string al = advancedDataGridView1.Rows[i].Cells["codalm"].FormattedValue.ToString();
                            //
                            try
                            {
                                string inserta = "insert into tempo (ida,codigo,nombre,cant,almacen) values (@id,@co,@no,@ca,@al)";
                                micon = new MySqlCommand(inserta, cn);
                                micon.Parameters.AddWithValue("@id", id);
                                micon.Parameters.AddWithValue("@co", co);
                                micon.Parameters.AddWithValue("@no", no);
                                micon.Parameters.AddWithValue("@ca", ca);
                                micon.Parameters.AddWithValue("@al", al);
                                micon.ExecuteNonQuery();
                            }
                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error - no se pudo insertar");
                                Application.Exit();
                                return;
                            }
                        }
                    }
                    // vamos a llamar a movimas
                    movimas resem = new movimas("salida", "", "");    // modo,array,libre
                    var result = resem.ShowDialog();
                    if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu
                    {
                        if (resem.retorno == true)
                        {
                            try
                            {       //  salida,evento,almdes
                                string consulta = "select codigo,nombre,cant,almacen,idres,evento,almdes,ida from tempo";
                                micon = new MySqlCommand(consulta, cn);    // idres = id de salida
                                MySqlDataReader dr = micon.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        // actualizamos el datagridview
                                        for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)
                                        {
                                            if (advancedDataGridView1.Rows[i].Cells["id"].Value.ToString() == dr.GetString(7))
                                            {
                                                if (dr.GetString(4) == "0" && dr.GetString(6) == "")
                                                {
                                                    advancedDataGridView1.Rows.RemoveAt(i);
                                                }
                                                else
                                                {
                                                    advancedDataGridView1.Rows[i].Cells["salida"].Value = dr.GetString(4);
                                                    advancedDataGridView1.Rows[i].Cells["evento"].Value = dr.GetString(5);
                                                    advancedDataGridView1.Rows[i].Cells["almdes"].Value = dr.GetString(6);
                                                }
                                            }
                                        }
                                    }
                                }
                                dr.Close();
                                for (int i = 0; i < advancedDataGridView1.Rows.Count; i++)
                                {
                                    if (advancedDataGridView1.Rows[i].Cells["marca"].FormattedValue.ToString() == "True")
                                    {
                                        // actualizamos almloc
                                        string actua = "update almloc set salida=@res,evento=@con,almdes=@ald,marca=0 where id=@idr";
                                        MySqlCommand miact = new MySqlCommand(actua, cn);
                                        miact.Parameters.AddWithValue("@res", advancedDataGridView1.Rows[i].Cells["salida"].Value.ToString());
                                        miact.Parameters.AddWithValue("@con", advancedDataGridView1.Rows[i].Cells["evento"].Value.ToString());
                                        miact.Parameters.AddWithValue("@ald", advancedDataGridView1.Rows[i].Cells["almdes"].Value.ToString());
                                        miact.Parameters.AddWithValue("@idr", advancedDataGridView1.Rows[i].Cells["id"].Value.ToString());
                                        miact.ExecuteNonQuery();
                                        advancedDataGridView1.Rows[i].Cells["marca"].Value = 0;
                                    }
                                }
                                consulta = "truncate tempo";
                                micon = new MySqlCommand(consulta, cn);
                                micon.ExecuteNonQuery();
                            }
                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error de conexión");
                                Application.Exit();
                                return;
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión");
                    Application.Exit();
                    return;
                }
                cn.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acción individual");
            }
        }
        private void pan_inicio_Enter(object sender, EventArgs e)                       // llamamos al procedimiento que colorea las filas seleccionadas
        {
            selec();
        }
        private void bt_bmf_Click(object sender, EventArgs e)                           // BORRA LAS MARCAS DE SELECCION DE FILAS
        {
            foreach (DataGridViewRow row in advancedDataGridView1.Rows)
            {
                if (row.Cells["marca"].FormattedValue.ToString() == "True")
                {
                    int mark = 0;
                    row.Cells["marca"].Value = mark;
                    grabacam(int.Parse(row.Cells["id"].Value.ToString()), "marca", mark.ToString());
                }
            }
        }
        private void bt_etiq_Click(object sender, EventArgs e)                          // imprime etiqueta del mueble seleccionado
        {
            if(advancedDataGridView1.Enabled == true && advancedDataGridView1.CurrentRow.Index > 0)
            {
                if (advancedDataGridView1.CurrentRow.Index >= 0)
                {
                    var aa = MessageBox.Show("Impresión de Etiquetas para el artículo" + Environment.NewLine +
                        "Esta listo para la imprimir?", "Rutina de impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        string id_mueble = advancedDataGridView1.CurrentRow.Cells["id"].Value.ToString();
                        string tx_cant = " ";
                        string tx_paq = " ";
                        //
                        string cap = advancedDataGridView1.CurrentRow.Cells["capit"].Value.ToString();
                        string mod = advancedDataGridView1.CurrentRow.Cells["model"].Value.ToString();
                        string mad = advancedDataGridView1.CurrentRow.Cells["mader"].Value.ToString();
                        string tip = advancedDataGridView1.CurrentRow.Cells["tipol"].Value.ToString();
                        string dt1 = advancedDataGridView1.CurrentRow.Cells["deta1"].Value.ToString();
                        string aca = advancedDataGridView1.CurrentRow.Cells["acaba"].Value.ToString();
                        string tal = advancedDataGridView1.CurrentRow.Cells["talle"].Value.ToString();
                        string dt2 = advancedDataGridView1.CurrentRow.Cells["deta2"].Value.ToString();
                        string dt3 = advancedDataGridView1.CurrentRow.Cells["deta3"].Value.ToString();
                        string jgo = advancedDataGridView1.CurrentRow.Cells["juego"].Value.ToString();
                        string nom = advancedDataGridView1.CurrentRow.Cells["nombr"].Value.ToString();
                        string med = advancedDataGridView1.CurrentRow.Cells["medid"].Value.ToString();
                        // llama al form impresor con los valores actuales
                        impresor impetiq = new impresor(cap, mod, mad, tip, dt1, aca, tal,
                            dt2, dt3, jgo, nom, med, tx_cant, tx_paq, int.Parse(id_mueble));
                        impetiq.Show();
                    }
                }
            }
        }
        #endregion

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                //iCount = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                totcolv = 0;
                foreach (DataGridViewColumn dgvGridCol in advancedDataGridView1.Columns)
                {
                    if (dgvGridCol.Visible == true && dgvGridCol.IsDataBound == true)
                    {
                        iTotalWidth += dgvGridCol.Width;
                        totcolv += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string lb_titulo = this.Text;
            try
            {
                //Set the left margin
                int iLeftMargin = e.MarginBounds.Left;
                //Set the top margin
                int iTopMargin = e.MarginBounds.Top;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;
                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in advancedDataGridView1.Columns)
                    {
                        if (GridCol.Visible == true && GridCol.IsDataBound == true)
                        {
                            iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                (double)iTotalWidth * (double)iTotalWidth *
                                ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                            iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                            // Save width and height of headers
                            arrColumnLefts.Add(iLeftMargin);
                            arrColumnWidths.Add(iTmpWidth);
                            iLeftMargin += iTmpWidth;
                        }
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= advancedDataGridView1.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = advancedDataGridView1.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height - 10;       // + 5              ********************************************
                    int iCount = 0;
                    //Check whether the current page settings allows more rows to print
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        Font titulo = new Font("Arial", 7);// para el titulo de columnas y dentro de la grilla
                        Font normal = new Font("Arial", 6);// para el titulo de columnas y dentro de la grilla
                        if (bNewPage)
                        {
                            //Draw Header
                            e.Graphics.DrawString(lb_titulo,
                                new Font(advancedDataGridView1.Font, FontStyle.Bold),
                                Brushes.Black, e.MarginBounds.Left,
                                e.MarginBounds.Top - e.Graphics.MeasureString(lb_titulo,
                                new Font(dataGridView1.Font, FontStyle.Bold),
                                e.MarginBounds.Width).Height - 13);

                            String strDate = DateTime.Now.ToLongDateString() + " " +
                                DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate,
                                new Font(advancedDataGridView1.Font, FontStyle.Bold), Brushes.Black,
                                e.MarginBounds.Left +
                                (e.MarginBounds.Width - e.Graphics.MeasureString(strDate,
                                new Font(advancedDataGridView1.Font, FontStyle.Bold),
                                e.MarginBounds.Width).Width),
                                e.MarginBounds.Top - e.Graphics.MeasureString(lb_titulo,
                                new Font(new Font(advancedDataGridView1.Font, FontStyle.Bold),
                                FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in advancedDataGridView1.Columns)
                            {
                                if (GridCol.Visible == true && GridCol.IsDataBound == true)
                                {
                                    e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                        new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawRectangle(Pens.Black,
                                        new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawString(GridCol.Name.ToString(),
                                        titulo,
                                        new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                        new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);   // HeaderText
                                    iCount++;
                                }
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        //Draw Columns Contents                
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null && Cel.Visible == true)
                            {
                                if (Cel.Value.GetType().ToString() == "System.DateTime")   //Cel.ValueType.ToString() == "System.DateTime"
                                {   // 
                                    e.Graphics.DrawString(Cel.Value.ToString().Substring(0, 10),
                                    normal,
                                    new SolidBrush(Cel.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount],
                                    (float)iTopMargin,
                                    (int)arrColumnWidths[iCount], (float)iCellHeight)
                                    );
                                }
                                else
                                {
                                    e.Graphics.DrawString(Cel.Value.ToString(),
                                    normal,
                                    new SolidBrush(Cel.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount],
                                    (float)iTopMargin,
                                    (int)arrColumnWidths[iCount], (float)iCellHeight),
                                    strFormat);
                                }
                                //Drawing Cells Borders 
                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iCellHeight));
                                iCount++;
                            }
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                    if (iTopMargin <= e.PageBounds.Height)
                    {
                        e.HasMorePages = false;
                    }
                    else
                    {
                        e.HasMorePages = true;
                    }
                }
                //If more lines exist, print another page.
                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
                return;     // lo acabo de poner 08-03-2018 
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            bFirstPage = true;
            bNewPage = true;
            iRow = 0;
        }

        #region radiobuttons - checked changed
        private void rb_estan_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_estan.Checked == true)
            {
                for (int i = 0; i < advancedDataGridView1.Rows[0].Cells.Count; i++)
                {
                    advancedDataGridView1.Columns[i].Visible = false;
                    dataGridView1.Columns[i].Visible = false;
                    dataGridView2.Columns[i].Visible = false;
                }
                advancedDataGridView1.Columns["marca"].Visible = true;
                dataGridView1.Columns["marca"].Visible = true;
                dataGridView2.Columns["marca"].Visible = true;
                advancedDataGridView1.Columns["id"].Visible = true;
                dataGridView1.Columns["id"].Visible = true;
                dataGridView2.Columns["id"].Visible = true;
                advancedDataGridView1.Columns["codalm"].Visible = true;
                dataGridView1.Columns["codalm"].Visible = true;
                dataGridView2.Columns["codalm"].Visible = true;
                advancedDataGridView1.Columns["codig"].Visible = true;
                dataGridView1.Columns["codig"].Visible = true;
                dataGridView2.Columns["codig"].Visible = true;
                advancedDataGridView1.Columns["capit"].Visible = true;
                dataGridView1.Columns["capit"].Visible = true;
                dataGridView2.Columns["capit"].Visible = true;
                advancedDataGridView1.Columns["model"].Visible = true;
                dataGridView1.Columns["model"].Visible = true;
                dataGridView2.Columns["model"].Visible = true;
                advancedDataGridView1.Columns["mader"].Visible = true;
                dataGridView1.Columns["mader"].Visible = true;
                dataGridView2.Columns["mader"].Visible = true;
                advancedDataGridView1.Columns["tipol"].Visible = true;
                dataGridView1.Columns["tipol"].Visible = true;
                dataGridView2.Columns["tipol"].Visible = true;
                advancedDataGridView1.Columns["deta1"].Visible = true;
                dataGridView1.Columns["deta1"].Visible = true;
                dataGridView2.Columns["deta1"].Visible = true;
                advancedDataGridView1.Columns["acaba"].Visible = true;
                dataGridView1.Columns["acaba"].Visible = true;
                dataGridView2.Columns["acaba"].Visible = true;
                advancedDataGridView1.Columns["talle"].Visible = true;
                dataGridView1.Columns["talle"].Visible = true;
                dataGridView2.Columns["talle"].Visible = true;
                advancedDataGridView1.Columns["deta2"].Visible = true;
                dataGridView1.Columns["deta2"].Visible = true;
                dataGridView2.Columns["deta2"].Visible = true;
                advancedDataGridView1.Columns["deta3"].Visible = true;
                dataGridView1.Columns["deta3"].Visible = true;
                dataGridView2.Columns["deta3"].Visible = true;
                advancedDataGridView1.Columns["juego"].Visible = true;
                dataGridView1.Columns["juego"].Visible = true;
                dataGridView2.Columns["juego"].Visible = true;
                advancedDataGridView1.Columns["nombr"].Visible = true;
                dataGridView1.Columns["nombr"].Visible = true;
                dataGridView2.Columns["nombr"].Visible = true;
                advancedDataGridView1.Columns["chkreserva"].Visible = true;
                dataGridView1.Columns["chkreserva"].Visible = true;
                dataGridView2.Columns["chkreserva"].Visible = true;
                advancedDataGridView1.Columns["reserva"].Visible = true;
                dataGridView1.Columns["reserva"].Visible = true;
                dataGridView2.Columns["reserva"].Visible = true;
                advancedDataGridView1.Columns["contrat"].Visible = true;
                dataGridView1.Columns["contrat"].Visible = true;
                dataGridView2.Columns["contrat"].Visible = true;
                advancedDataGridView1.Columns["chksalida"].Visible = true;
                dataGridView1.Columns["chksalida"].Visible = true;
                dataGridView2.Columns["chksalida"].Visible = true;
                advancedDataGridView1.Columns["salida"].Visible = true;
                dataGridView1.Columns["salida"].Visible = true;
                dataGridView2.Columns["salida"].Visible = true;
                advancedDataGridView1.Columns["evento"].Visible = true;
                dataGridView1.Columns["evento"].Visible = true;
                dataGridView2.Columns["evento"].Visible = true;
                advancedDataGridView1.Columns["almdes"].Visible = true;
                dataGridView1.Columns["almdes"].Visible = true;
                dataGridView2.Columns["almdes"].Visible = true;
            }
        }
        private void rb_redu_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_redu.Checked == true)
            {
                for (int i = 0; i < advancedDataGridView1.Rows[0].Cells.Count; i++)
                {
                    advancedDataGridView1.Columns[i].Visible = false;
                    dataGridView1.Columns[i].Visible = false;
                    dataGridView2.Columns[i].Visible = false;
                }
                advancedDataGridView1.Columns["marca"].Visible = true;
                dataGridView1.Columns["marca"].Visible = true;
                dataGridView2.Columns["marca"].Visible = true;
                advancedDataGridView1.Columns["id"].Visible = true;
                dataGridView1.Columns["id"].Visible = true;
                dataGridView2.Columns["id"].Visible = true;
                advancedDataGridView1.Columns["codalm"].Visible = true;
                dataGridView1.Columns["codalm"].Visible = true;
                dataGridView2.Columns["codalm"].Visible = true;
                advancedDataGridView1.Columns["codig"].Visible = true;
                dataGridView1.Columns["codig"].Visible = true;
                dataGridView2.Columns["codig"].Visible = true;
                advancedDataGridView1.Columns["nombr"].Visible = true;
                dataGridView1.Columns["nombr"].Visible = true;
                dataGridView2.Columns["nombr"].Visible = true;
            }
        }
        private void rb_todos_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows[0].Cells.Count; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = true;
                dataGridView1.Columns[i].Visible = true;
                dataGridView2.Columns[i].Visible = true;
                advancedDataGridView1.Columns[i].Visible = true;
            }
        }
        #endregion

        #region botones_de_comando_y_permisos  
        public void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            bt_view.Visible = false;
            Bt_print.Visible = false;
            bt_prev.Visible = false;
            bt_exc.Visible = false;
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
            Tx_modo.Text = "NUEVO";
            //button1.Image = Image.FromFile(img_grab);
            panel1.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView2.Enabled = true;
            advancedDataGridView1.Enabled = true;
            bt_reserva.Enabled = true;
            bt_salida.Enabled = true;
            bt_borra.Enabled = true;
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDITAR";
            panel1.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView2.Enabled = true;
            advancedDataGridView1.Enabled = true;
            bt_reserva.Enabled = true;
            bt_salida.Enabled = true;
            bt_borra.Enabled = true;
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "IMPRIMIR";
            //Open the print preview dialog
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 50;
            pg.Margins.Bottom = 0;
            pg.Margins.Left = 50;
            pg.Margins.Right = 0;
            pg.Landscape = true;
            printDocument1.DefaultPageSettings = pg;

            iRow = 0; // a ver a ver
            PrintPreviewDialog objPPdialog = new PrintPreviewDialog();
            objPPdialog.Document = printDocument1;
            objPPdialog.ShowDialog();
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "ANULAR";
            // no tiene funcion en este form
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "inventario_al_" + DateTime.Now.ToShortDateString() + "_.xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
            "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                DataTable datexc = (DataTable)(advancedDataGridView1.DataSource);
                wb.Worksheets.Add(datexc, "Inventario");
                wb.SaveAs(nombre);
                MessageBox.Show("Archivo generado con exito!");
            }
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

        #region grillas 1, 2 y advanced
        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Value != null)
            {
                string frase = dataGridView2.Columns[e.ColumnIndex].Name.ToString() + " like '" + dataGridView2.CurrentCell.Value.ToString() + "*'";
                filtros(frase);
            }
        }
        private void advancedDataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                valant = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }
        private void advancedDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                valnue = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                int index = advancedDataGridView1.Columns["id"].Index;
                switch (advancedDataGridView1.Columns[e.ColumnIndex].Name)
                {
                    case "marca": //  0
                        int mark = (advancedDataGridView1.Rows[e.RowIndex].Cells[advancedDataGridView1.Columns["marca"].Index].Value.ToString() == "False") ? 0 : 1;
                        grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells[advancedDataGridView1.Columns["id"].Index].Value.ToString()),
                                        advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(), mark.ToString());
                        break;
                    case "codalm": // 2
                        if ((advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "" ||
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != null) &&
                            valant != "" && valant != valnue)
                        {
                            if (vali_alm(valnue) == true)
                            {
                                var aa = MessageBox.Show("Desea MOVER el mueble al almacén ingresado?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (aa == DialogResult.Yes)
                                {
                                    // ejecuta el proceso interno de cambio de almacen
                                    grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()),
                                        advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString(), valnue);
                                }
                                else
                                {
                                    // regresa el valor anterior de la columna almacen
                                    advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Almacén incorrecto: " + valnue,
                                    "Verifique por favor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                            }
                        }
                        else
                        {
                            //MessageBox.Show("es null o vacio o valant es igual al titulo", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        }
                        break;
                    case "fecha":     //  3
                        break;
                    case "capit":
                    case "model":
                    case "mader":
                    case "tipol":
                    case "deta1":
                        // string letrascod = "capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3";
                        // debe validar si lo cambiado existe en la maestra
                        string nomcol = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                        string valcol = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        string nomcap = advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString();
                        if (vali_par(nomcol, valcol, nomcap) == true)   // valida si existe en la maestra el dato cambiado, debe validar toda la estructura ..
                        {   // ,string nomcol, string valcol, string colcap
                            var aaa = MessageBox.Show("Realmente desea cambiar el valor?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (aaa == DialogResult.Yes)
                            {
                                // cambia el codigo en la grilla
                                advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value =
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["model"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["mader"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["tipol"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["deta1"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["acaba"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["talle"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["deta2"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["deta3"].Value.ToString() +
                                    advancedDataGridView1.Rows[e.RowIndex].Cells["juego"].Value.ToString();
                                // graba el nuevo codigo y letra en almloc
                                grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()),
                                        advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString(), valnue);
                                grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()),
                                        "codig", advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value.ToString());
                                // jala nuevos datos de la maestra y actualiza la grilla
                                //jalareg(advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells["model"].Value.ToString(),
                                //    advancedDataGridView1.Rows[e.RowIndex].Cells["tipol"].Value.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells["deta1"].Value.ToString(),
                                //    advancedDataGridView1.Rows[e.RowIndex].Cells["acaba"].Value.ToString(), e.RowIndex);  //Int16.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString())
                                // ya no jala nada desde el 14-03-2018 a solicitud de Lorenzo
                            }
                            else
                            {
                                advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Valor incorrecto: " + valnue,
                                    "Verifique por favor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                        }
                        break;
                    case "acaba":
                    case "talle":
                    case "deta2":
                    case "deta3":
                    case "juego":
                        var a12 = MessageBox.Show("Confirma que desea cambiar el valor de la columna?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (a12 == DialogResult.Yes)
                        {
                            // cambia el codigo en la grilla
                            advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value =
                                advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["model"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["mader"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["tipol"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta1"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["acaba"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["talle"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta2"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta3"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["juego"].Value.ToString();
                            // graba el nuevo codigo y letra en almloc
                            grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()),
                                    advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString(), valnue);
                            grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()),
                                    "codig", advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value.ToString());
                            // jala nuevos datos de la maestra y actualiza la grilla
                            //jalareg(advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value.ToString(), Int16.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()));
                            //jalareg(advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells["model"].Value.ToString(),
                            //        advancedDataGridView1.Rows[e.RowIndex].Cells["tipol"].Value.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells["deta1"].Value.ToString(),
                            //        advancedDataGridView1.Rows[e.RowIndex].Cells["acaba"].Value.ToString(), e.RowIndex);
                            // ya no jala nada desde el 14-03-2018 a solicitud de Lorenzo
                        }
                        break;
                    case "nombr":    // nombre
                        var a13 = MessageBox.Show("Confirma que desea cambiar el nombre del mueble?" + Environment.NewLine +
                            "Este cambio solo es para el stock - inventario", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (a13 == DialogResult.Yes)
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valnue;
                            grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells[index].Value.ToString()),
                                advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(), valnue);
                            //grabaitems("nombr",advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value.ToString(),valnue);
                        }
                        else
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                        }
                        break;
                    case "medid":    // medidas
                        var a14 = MessageBox.Show("Confirma que desea cambiar las medidas?" + Environment.NewLine +
                            "Este cambio solo es para el stock - inventario", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (a14 == DialogResult.Yes)
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valnue;
                            grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells[index].Value.ToString()),
                                advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(), valnue);
                        }
                        else
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                        }
                        break;
                    //case "soles":
                    case "soles2018":
                        var a15 = MessageBox.Show("Confirma que desea cambiar el valor?" + Environment.NewLine +
                            "Este cambio solo es para el stock - inventario", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (a15 == DialogResult.Yes)
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valnue;
                            grabacam(int.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells[index].Value.ToString()),
                                advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString(), valnue);
                        }
                        else
                        {
                            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valant;
                        }
                        break;
                }
            }
        }
        private void advancedDataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView2.ColumnCount > 1)
            {
                dataGridView2.Columns[e.Column.Name].Width = e.Column.Width;
                dataGridView1.Columns[e.Column.Name].Width = e.Column.Width;
            }
        }
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = advancedDataGridView1.FilterString;
            cellsum(7);
        }
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            dt.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (advancedDataGridView1.IsCurrentCellDirty)
            {
                advancedDataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void advancedDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (advancedDataGridView1.Columns[e.ColumnIndex].Name == "codig" && !string.IsNullOrEmpty(valant))
            {
                // se hace en cellendedit
            }
            if (advancedDataGridView1.Columns[e.ColumnIndex].Name == "marca")
            {
                if (advancedDataGridView1.CurrentCell.FormattedValue.ToString() == "True")
                {
                    advancedDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    advancedDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
            if (advancedDataGridView1.Columns[e.ColumnIndex].Name == "chkreserva")
            {
                if (advancedDataGridView1.CurrentCell != null &&
                    advancedDataGridView1.CurrentCell.FormattedValue.ToString() == "True")
                {
                    if (string.IsNullOrWhiteSpace(advancedDataGridView1.Rows[e.RowIndex].Cells["reserva"].Value.ToString()))
                    {
                        var aa = MessageBox.Show("Realmente desea reservar este mueble?", "Confirme por favor",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            string codpar = advancedDataGridView1.Rows[e.RowIndex].Cells["capit"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["model"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["mader"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["tipol"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta1"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["acaba"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta2"].Value.ToString() +
                                advancedDataGridView1.Rows[e.RowIndex].Cells["deta3"].Value.ToString();
                            movim rese = new movim("reserva", 
                                advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString(),
                                codpar,
                                advancedDataGridView1.Rows[e.RowIndex].Cells["codalm"].Value.ToString());    // modo,id_mueble,cod_mueble
                            var result = rese.ShowDialog();
                            if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu .... no sea aaa
                            {
                                if (rese.retorno == false) advancedDataGridView1.CurrentCell.Value = false;
                                else 
                                {
                                    advancedDataGridView1.CurrentCell.Value = true;
                                    advancedDataGridView1.CurrentRow.Cells["reserva"].Value = rese.retval1;
                                    advancedDataGridView1.CurrentRow.Cells["contrat"].Value = rese.retval2;
                                }
                            }
                        }
                        else
                        {
                            advancedDataGridView1.CurrentCell.Value = false;
                        }
                    }
                    else
                    {
                        var aa = MessageBox.Show("Realmente desea QUITAR la reserva de este mueble?", "Confirme por favor",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (aa == DialogResult.Yes)
                        {
                            if (quitareserv(advancedDataGridView1.Rows[e.RowIndex].Cells["reserva"].Value.ToString(),
                                advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString(),
                                advancedDataGridView1.Rows[e.RowIndex].Cells["contrat"].Value.ToString()
                                ) == true)
                            {
                                // borra las marcas en la grilla
                                advancedDataGridView1.CurrentRow.Cells["reserva"].Value = "";
                                advancedDataGridView1.CurrentRow.Cells["contrat"].Value = "";
                                advancedDataGridView1.CurrentRow.Cells["chkreserva"].Value = 0;
                            }
                        }
                        else
                        {
                            advancedDataGridView1.CurrentCell.Value = false;
                        }
                    }
                }
            }
            if (advancedDataGridView1.Columns[e.ColumnIndex].Name == "chksalida")
            {
                if (advancedDataGridView1.CurrentCell != null &&
                    advancedDataGridView1.CurrentCell.FormattedValue.ToString() == "True")
                {
                    if (string.IsNullOrWhiteSpace(advancedDataGridView1.Rows[e.RowIndex].Cells["almdes"].Value.ToString()))
                    {
                        var aa = MessageBox.Show("Realmente desea AUTORIZAR SALIDA a este mueble?", "Confirme por favor",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            movim rese = new movim("salida",
                                advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString(),
                                advancedDataGridView1.Rows[e.RowIndex].Cells["codig"].Value.ToString(),
                                advancedDataGridView1.Rows[e.RowIndex].Cells["codalm"].Value.ToString());    // modo,id_mueble,cod_mueble
                            var result = rese.ShowDialog();
                            if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu .... no sea aaa
                            {
                                if (rese.retorno == false) advancedDataGridView1.CurrentCell.Value = false;
                                else
                                {
                                    advancedDataGridView1.CurrentCell.Value = true;
                                    advancedDataGridView1.CurrentRow.Cells["salida"].Value = rese.retval1;
                                    advancedDataGridView1.CurrentRow.Cells["evento"].Value = rese.retval2;
                                    advancedDataGridView1.CurrentRow.Cells["almdes"].Value = rese.retval3;
                                    if (advancedDataGridView1.CurrentRow.Cells["salida"].Value.ToString() == "0")
                                    {   // debe borrarse la fila del datagrid porque la salida fue por ajuste
                                        advancedDataGridView1.Rows.RemoveAt(e.RowIndex);
                                    }
                                }
                            }
                        }
                        else
                        {
                            advancedDataGridView1.CurrentCell.Value = false;
                        }
                    }
                    else
                    {
                        var aa = MessageBox.Show("Realmente desea BORRAR la autorización de salida?", "Confirme por favor",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (aa == DialogResult.Yes)
                        {
                            if (quitasalida(advancedDataGridView1.Rows[e.RowIndex].Cells["salida"].Value.ToString(),
                                advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()
                                ) == true)
                            {
                                // borra las marcas en la grilla
                                advancedDataGridView1.CurrentRow.Cells["salida"].Value = "";
                                advancedDataGridView1.CurrentRow.Cells["evento"].Value = "";
                                advancedDataGridView1.CurrentRow.Cells["almdes"].Value = "";
                                advancedDataGridView1.CurrentRow.Cells["chksalida"].Value = 0;
                            }
                        }
                        else
                        {
                            advancedDataGridView1.CurrentCell.Value = false;
                        }
                    }
                }
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                if (dataGridView1.CurrentCell.FormattedValue.ToString() == "False")
                {
                    //string noseve = dataGridView1.Columns[dataGridView1.Columns[e.ColumnIndex].Name.ToString()].ToString();
                    string noseve = dataGridView1.Columns[e.ColumnIndex].Name.ToString();
                    dataGridView1.Columns[noseve].Visible = false;
                    dataGridView2.Columns[noseve].Visible = false;
                    advancedDataGridView1.Columns[noseve].Visible = false;
                }
            }
        }
        private void advancedDataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dataGridView2.HorizontalScrollingOffset = e.NewValue;
                dataGridView1.HorizontalScrollingOffset = e.NewValue;
            }
        }
        #endregion
    }
}
