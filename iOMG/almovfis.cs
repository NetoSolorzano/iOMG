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
    public partial class almovfis : Form
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
        string nomform = "almovfis";                          //
        string img_btN = "";
        string img_btE = "";
        string img_btA = "";
        string img_btP = "";
        string img_btexc = "";
        string img_pre = "";
        string img_ver = "";
        string img_bti = "";
        string img_bts = "";
        string img_btr = "";
        string img_btf = "";
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string vcprecio = "";                                   // nombre del campo precio de la tabla "alm2018"
        #endregion

        public almovfis()
        {
            InitializeComponent();
        }
        private void almovfis_Load(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            advancedDataGridView1.Enabled = false;
            bt_bmf.Enabled = false;
            bt_reserva.Enabled = false;
            bt_salida.Enabled = false;
            bt_borra.Enabled = false;
            bt_extornaS.Enabled = false;
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
        private void almovfis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        #region funcionas propias del form
        private void jalainfo()                                             // obtiene datos de imagenes
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
                        if (row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
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
        private void jaladat()                                              // jala los datos, 
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string sqlCmd = "select distinct a.marca,a.id,a.codalm,a.fechop,a.codig,a.capit,a.model,a.mader,a.tipol,a.deta1,a.acaba,a.talle,a.deta2,a.deta3,a.juego," +
                    "ifnull(a.nombr,'') as nombr,ifnull(a.medid,'') as medid,a.reserva,a.contrat,a.salida,a.evento,a.almdes," +
                    "ifnull(b.umed,'') as umed,ifnull(b.soles2018,0) as soles2018,a.idajuste,a.pedalm " +
                    "from almloc a " +
                    "left join (select * from items group by capit,model,tipol,deta1,acaba,deta2) b " +
                    "on b.capit=a.capit and b.model=a.model and b.tipol=a.tipol and b.deta1=a.deta1 and b.acaba=a.acaba and b.deta2=a.deta2 " +
                    "where (a.reserva<>'' or a.almdes<>'')";
                MySqlCommand micon = new MySqlCommand(sqlCmd, cn);
                micon.CommandTimeout = 300;
                micon.Parameters.AddWithValue("@alm", iOMG.Program.almuser);
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
        private void grilla()                                               // arma la grilla1
        {
            advancedDataGridView1.AllowUserToAddRows = false;
            //
            advancedDataGridView1.Columns[0].Width = 30;            // marca
            advancedDataGridView1.Columns[1].Width = 40;            // id
            advancedDataGridView1.Columns[1].ReadOnly = true;
            advancedDataGridView1.Columns[2].Width = 70;            // almacen
            advancedDataGridView1.Columns[2].ReadOnly = true;
            advancedDataGridView1.Columns[3].Width = 80;            // fecha
            advancedDataGridView1.Columns[3].ReadOnly = true;
            advancedDataGridView1.Columns[4].Width = 130;            // código
            advancedDataGridView1.Columns[4].ReadOnly = true;
            advancedDataGridView1.Columns[5].Width = 30;             // capital
            advancedDataGridView1.Columns[5].ReadOnly = true;
            advancedDataGridView1.Columns[6].Width = 40;             // modelo
            advancedDataGridView1.Columns[6].ReadOnly = true;
            advancedDataGridView1.Columns[7].Width = 30;             // madera
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
            advancedDataGridView1.Columns[15].Width = 297;          // nombre
            advancedDataGridView1.Columns[15].ReadOnly = true;
            advancedDataGridView1.Columns[16].Width = 70;            // medidas
            advancedDataGridView1.Columns[16].ReadOnly = true;
            //
            advancedDataGridView1.Columns[17].Width = 30;           // id reserva
            advancedDataGridView1.Columns[17].ReadOnly = true;
            advancedDataGridView1.Columns[18].Width = 70;           // contrato
            advancedDataGridView1.Columns[18].ReadOnly = true;
            //
            advancedDataGridView1.Columns[19].Width = 30;           // id salida
            advancedDataGridView1.Columns[19].ReadOnly = true;
            advancedDataGridView1.Columns[20].Width = 70;           // evento
            advancedDataGridView1.Columns[20].ReadOnly = true;
            advancedDataGridView1.Columns[21].Width = 70;           // almacen destino
            advancedDataGridView1.Columns[21].ReadOnly = true;
            advancedDataGridView1.Columns[22].Width = 50;           // unidad de medida
            advancedDataGridView1.Columns[22].ReadOnly = true;
            advancedDataGridView1.Columns[23].Width = 70;           // precio soles hasta el 2017
            advancedDataGridView1.Columns[23].ReadOnly = true;
            advancedDataGridView1.Columns[23].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            advancedDataGridView1.Columns[24].Width = 30;           // id ajuste ingreso
            advancedDataGridView1.Columns[24].ReadOnly = true;
            advancedDataGridView1.Columns[25].Width = 70;           // cod pedido almacen
            advancedDataGridView1.Columns[25].ReadOnly = true;
        }
        private void init()                                                 // inicializa ancho de columnas grilla de filtros
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
            Bt_print.Image = Image.FromFile(img_btP);
            bt_prev.Image = Image.FromFile(img_pre);
            bt_view.Image = Image.FromFile(img_ver);
            bt_exc.Image = Image.FromFile(img_btexc);
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
            dataGridView2.Columns["id"].ReadOnly = true;
            dataGridView1.Rows.Add();
        }
        private void cvc()                                                  // checks de visualizacion de columnas
        {
            int totcolv = 1;
            if (advancedDataGridView1.Rows.Count > 0) totcolv = advancedDataGridView1.Rows[0].Cells.Count;
            else totcolv = advancedDataGridView1.Columns.Count;
            for (int i = 0; i <= totcolv - 1; i++)  // dataGridView1 -2
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
        private void cellsum(int ind)                                       // suma la columna especificada
        {
            tx_tarti.Text = (advancedDataGridView1.Rows.Count).ToString();
            decimal b = 0;
            string qw = vcprecio;   // "soles2018";
            foreach (DataGridViewRow r in advancedDataGridView1.Rows)
            {
                if (r.Cells[qw].Value != null && r.Cells[qw].Value != DBNull.Value) b += Convert.ToDecimal(r.Cells[qw].Value);  // total precio con igv
            }
            tx_totprec.Text = b.ToString("###,###,##0.00");
        }
        private void filtros(string expres)                                 // filtros de nivel superior
        {
            dv = new DataView(dt);
            dv.RowFilter = expres;
            dt = dv.ToTable();
            advancedDataGridView1.DataSource = dt;
            grilla();
            cellsum(0);
            rb_redu_CheckedChanged(null, null);
            rb_todos_CheckedChanged(null, null);
        }
        private void jalareg(string cap, string mod, string tip, string det1, string aca, int id)   // jala datos de la maestra y actualiza la grilla
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                string consulta = "select ifnull(b.nombr,'') as nombr,ifnull(b.medid,'') as medid," +
                    "ifnull(b.umed,'') as umed,ifnull(b.soles2018,0) as soles2018 " +
                    "from items b where b.capit=@cap and b.model=@mod and b.tipol=@tip and b.deta1=@det1 and b.acaba=@aca";
                MySqlCommand micon = new MySqlCommand(consulta, cn);

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
        private void busmarcas()                                            // busca y guarda las marcas de visualizacion vertical
        {
            for (int i = 0; i < dataGridView1.Rows[0].Cells.Count - 2; i++)
            {
                marcas.Add((dataGridView1.Rows[0].Cells[i].Value.ToString() == "True") ? true : false);
            }
        }
        private void restauramar()                                          // restaura las visualizaciones segun la marca
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
        private void selec()                                                // pone color de seleccion si esta con check
        {
            for (int i = 0; i < advancedDataGridView1.Rows.Count - 1; i++)
            {
                if (advancedDataGridView1.Rows[i].Cells[advancedDataGridView1.Columns["marca"].Index].Value.ToString() == "True")
                {
                    advancedDataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                }
            }
        }
        private bool quitareserv(string idr, string ida, string contra)     // esto habria que modificar cuando el mueble sale del almacen
        {                       //id reserva,id almacen,contrato
            bool retorna = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
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
                micon.Parameters.AddWithValue("@nar", advancedDataGridView1.CurrentRow.Cells["codig"].Value.ToString());
                micon.ExecuteNonQuery();
                //
                //acciones acx = new acciones();
                //acx.act_cont(contra,"RESERVA");
                //
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
        private bool quitasalida(string idr, string ida)                    // igual .. cambia cuando los muebles sales del almacen
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

        private void bt_print_Click(object sender, EventArgs e)             // creo que esto si queda
        {
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

        #region grillas 1,2 y advanced
        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Value != null)
            {
                string frase = dataGridView2.Columns[e.ColumnIndex].Name.ToString() + " like '" + dataGridView2.CurrentCell.Value.ToString() + "*'";
                filtros(frase);
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

        #region botones click
        private void bt_borra_Click(object sender, EventArgs e)
        {
            busmarcas();    // visualizacion de columnas
            dt.Rows.Clear();
            dataGridView2.Rows.Clear();
            dt.DefaultView.RowFilter = "";
            advancedDataGridView1.DataSource = null;
            advancedDataGridView1.Rows.Clear();
            jaladat();
            advancedDataGridView1.DataSource = dt;
            grilla();
            init();
            cvc();
            cellsum(0);
            rb_estan.Checked = false;
            rb_estan.PerformClick();
            restauramar();
            selec();
        }
        private void bt_etiq_Click(object sender, EventArgs e)
        {
            if (advancedDataGridView1.CurrentRow.Index >= 0 && advancedDataGridView1.Enabled == true)
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
        private void bt_bmf_Click(object sender, EventArgs e)       // BORRA LAS MARCAS DE SELECCION DE FILAS
        {
            foreach (DataGridViewRow row in advancedDataGridView1.Rows)
            {
                if (row.Cells["marca"].FormattedValue.ToString() == "True")
                {
                    int mark = 0;
                    row.Cells["marca"].Value = mark;
                }
            }
        }
        private void bt_salida_Click(object sender, EventArgs e)        // salida fisica del almacen
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
            if (fi > 0)
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
                            string al = advancedDataGridView1.Rows[i].Cells["codalm"].FormattedValue.ToString();    // alm origen
                            string ev = advancedDataGridView1.Rows[i].Cells["evento"].FormattedValue.ToString();
                            string ad = advancedDataGridView1.Rows[i].Cells["almdes"].FormattedValue.ToString();    // alm destino
                            string ct = advancedDataGridView1.Rows[i].Cells["contrat"].FormattedValue.ToString();
                            string re = advancedDataGridView1.Rows[i].Cells["reserva"].FormattedValue.ToString();
                            // reserva,a.contrat,a.salida,a.evento,a.almdes
                            try
                            {
                                string inserta = "insert into tempo (ida,codigo,nombre,cant,almacen,evento,almdes,contrat,idres) " +
                                    "values (@id,@co,@no,@ca,@al,@ev,@ad,@ct,@re)";
                                micon = new MySqlCommand(inserta, cn);
                                micon.Parameters.AddWithValue("@id", id);
                                micon.Parameters.AddWithValue("@co", co);
                                micon.Parameters.AddWithValue("@no", no);
                                micon.Parameters.AddWithValue("@ca", ca);
                                micon.Parameters.AddWithValue("@al", al);
                                micon.Parameters.AddWithValue("@ev", ev);
                                micon.Parameters.AddWithValue("@ad", ad);
                                micon.Parameters.AddWithValue("@ct", ct);
                                micon.Parameters.AddWithValue("@re", (re.Trim() == "") ? 0 : Int16.Parse(re));
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
                    // vamos a llamar a movfismas
                    movfismas resem = new movfismas("salida", "", "");    // modo,array,libre
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
                                                advancedDataGridView1.Rows.RemoveAt(i);
                                            }
                                        }
                                    }
                                }
                                dr.Close();
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
                        else
                        {
                            MessageBox.Show("No se proceso la información", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void bt_reserva_Click(object sender, EventArgs e)       // ahora es el boton de ingreso de merca
        {
            // entradas fisicas al almacen
            movenmas resem = new movenmas("entrada", "", "");    // modo,array,libre
            var result = resem.ShowDialog();
            if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu
            {
                if (resem.retorno == true)
                {
                    MessageBox.Show("Los muebles se ingresaron con exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se logró ingresar todos o algunos muebles", "Verifique por favor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void bt_extornaS_Click(object sender, EventArgs e)      // extorna mueble salido por venta
        {
            movextorno rexto = new movextorno("EXTORNO DE SALIDA POR VENTA");
            var result = rexto.ShowDialog();
            if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu
            {
                if (rexto.retorno == true)
                {
                    MessageBox.Show("El mueble se re-ingresó con exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se logró re-ingresar el mueble", "Verifique por favor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void almovfis_Enter(object sender, EventArgs e)
        {
            selec();
        }
        #endregion

        #region radio buttons
        private void rb_estan_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_estan.Checked == true)
            {
                totcolv = 0;
                if (advancedDataGridView1.Rows.Count > 0) totcolv = advancedDataGridView1.Rows[0].Cells.Count;
                else totcolv = advancedDataGridView1.Columns.Count;
                for (int i = 0; i < totcolv; i++)
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
                advancedDataGridView1.Columns["medid"].Visible = true;
                dataGridView1.Columns["medid"].Visible = true;
                dataGridView2.Columns["medid"].Visible = true;
                //
                advancedDataGridView1.Columns["reserva"].Visible = true;
                dataGridView1.Columns["reserva"].Visible = true;
                dataGridView2.Columns["reserva"].Visible = true;
                advancedDataGridView1.Columns["contrat"].Visible = true;
                dataGridView1.Columns["contrat"].Visible = true;
                dataGridView2.Columns["contrat"].Visible = true;
                advancedDataGridView1.Columns["salida"].Visible = true;
                dataGridView1.Columns["salida"].Visible = true;
                dataGridView2.Columns["salida"].Visible = true;
                advancedDataGridView1.Columns["evento"].Visible = true;
                dataGridView1.Columns["evento"].Visible = true;
                dataGridView2.Columns["evento"].Visible = true;
                advancedDataGridView1.Columns["almdes"].Visible = true;
                dataGridView1.Columns["almdes"].Visible = true;
                dataGridView2.Columns["almdes"].Visible = true;
                // 
                advancedDataGridView1.Columns["idajuste"].Visible = true;
                dataGridView1.Columns["idajuste"].Visible = true;
                dataGridView2.Columns["idajuste"].Visible = true;
                advancedDataGridView1.Columns["pedalm"].Visible = true;
                dataGridView1.Columns["pedalm"].Visible = true;
                dataGridView2.Columns["pedalm"].Visible = true;
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
            panel1.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView2.Enabled = true;
            advancedDataGridView1.Enabled = true;
            bt_reserva.Enabled = true;
            bt_salida.Enabled = true;
            bt_borra.Enabled = true;
            bt_bmf.Enabled = true;
            bt_extornaS.Enabled = true;
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
            bt_bmf.Enabled = true;
            bt_extornaS.Enabled = true;
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
            string nombre = "Mov_fisicos.xlsx";
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
    }
}
