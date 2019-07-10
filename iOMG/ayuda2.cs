using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class ayuda2 : Form
    {
        public string para1 = "";
        public string para2 = "";
        public string para3 = "";
        public string para4 = "";
        libreria lnp = new libreria();
        // Se crea un DataTable que almacenará los datos desde donde se cargaran los datos al DataGridView
        DataTable dtDatos = new DataTable();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public ayuda2(string param1,string param2,string param3,string param4)
        {
            para1 = param1;              // 
            para2 = param2;              //
            para3 = param3;              //
            para4 = param4;              // 
            InitializeComponent();
        }
        private void ayuda2_Load(object sender, EventArgs e)
        {
            loadgrids();    // datos del grid
            this.Text = this.Text + " - " + para4 + " - " + para1 + " - " + para2;
        }
        private void ayuda2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string ReturnValue0 { get; set; }
        public string ReturnValue2 { get; set; }

        public void loadgrids()
        {
            // DATOS DE LA GRILLA
            string consulta = "";
            if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")    // articulos de la maestra
            {
                consulta = "select codig,nombr,medid,soles2018 " +
                    "from items";
                // Acomodamos la grilla 891
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 4;
                dataGridView1.Columns[0].Name = "CODIGO";
                dataGridView1.Columns[0].Width = 170;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = "NOMBRE";
                dataGridView1.Columns[1].Width = 490;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = "MEDIDAS";
                dataGridView1.Columns[2].Width = 90;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = "PRECIO";
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[3].ReadOnly = true;
            }
            if (para1 != "" && para2 != "" && para3 == "caja" && para4 == "cobranzas")   // ejemplo libre
            {
                consulta = "select a.id,concat(a.sercob,a.corcob),b.descrizionerid,a.fechope," +
                    "h.descrizionerid,a.numcli,left(d.nombre,29),concat(ifnull(g.sergr,''),ifnull(g.corgr,'')) as corgr,e.descrizionerid as mon,a.observ," +
                    "' ',' ',' ',' ',a.pagado,a.saldo,a.valortc,g.tipdv,g.serdv,g.cordv,f.sernot,f.cornot,f.valnot,f.pagado," +
                    "ifnull(c.descrizionerid,' ') as tdo,round(g.totnot,2),a.totdoc,a.doccli " +
                    "from macobran a " +
                    "left join desc_sit b on b.idcodice=a.status " +
                    "left join anag_cli d on d.docu=a.doccli and d.ruc=a.numcli " +
                    "left join desc_mon e on e.idcodice=a.moneda " +
                    "left join detacobnot f on f.sercob=a.sercob and f.corcob=a.corcob " +
                    "left join mactacte g on g.sernot=f.sernot and g.cornot=f.cornot " +
                    "left join desc_tdo c on c.IDCodice=g.tipdv " +
                    "left join desc_doc h on h.idcodice=a.doccli " +
                    "where a.local=@para1 and a.fechope=@para2 " +
                    "order by a.fechope,a.docvta,a.servta,a.corvta";
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.ColumnCount = 11;
                this.dataGridView1.Columns[0].Name = " IT ";
                this.dataGridView1.Columns[0].Width = 35;
                this.dataGridView1.Columns[0].ReadOnly = true;
                this.dataGridView1.Columns[1].Name = " Cobranza";
                this.dataGridView1.Columns[1].Width = 70;
                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].Name = " Estado";
                this.dataGridView1.Columns[2].Width = 70;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[3].Name = " Fecha";
                this.dataGridView1.Columns[3].Width = 80;
                this.dataGridView1.Columns[3].ReadOnly = true;
                this.dataGridView1.Columns[4].Name = " Doc";   // Doccli
                this.dataGridView1.Columns[4].Width = 40;
                this.dataGridView1.Columns[4].ReadOnly = true;
                this.dataGridView1.Columns[5].Name = " Num.";
                this.dataGridView1.Columns[5].Width = 80;
                this.dataGridView1.Columns[5].ReadOnly = true;
                this.dataGridView1.Columns[6].Name = " Cliente";
                this.dataGridView1.Columns[6].Width = 280;
                this.dataGridView1.Columns[6].ReadOnly = true;
                this.dataGridView1.Columns[7].Name = "G.R.";
                this.dataGridView1.Columns[7].Width = 80;
                this.dataGridView1.Columns[7].ReadOnly = true;
                this.dataGridView1.Columns[8].Name = "Mon";
                this.dataGridView1.Columns[8].Width = 30;
                this.dataGridView1.Columns[8].ReadOnly = true;
                this.dataGridView1.Columns[9].Name = "Pagado";
                this.dataGridView1.Columns[9].Width = 60;
                this.dataGridView1.Columns[9].ReadOnly = true;
                this.dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns[10].Name = "Saldo";
                this.dataGridView1.Columns[10].Width = 60;
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            // Se crea un MySqlAdapter para obtener los datos de la base
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para3 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@ser", para1);
                        if (para4 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@cor", int.Parse(para2));
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            // (li + 1).ToString(),
                            dataGridView1.Rows.Add(
                                                row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString()
                                                );
                        }
                    }
                    if (para1 != "" && para2 != "" && para3 == "caja" && para4 == "cobranzas")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para1 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para1", para1);
                        if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add((li + 1).ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString().Substring(0,10),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[14].ToString(),
                                                row.ItemArray[15].ToString()
                                                );
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en consulta de datos");
                    Application.Exit();
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnValue0 = tx_id.Text;
            ReturnValue1 = tx_codigo.Text;
            ReturnValue2 = tx_nombre.Text;
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
            //{
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                this.tx_id.Text = "";
                iOMG.Program.retorna1 = cellva;
                tx_codigo.Focus();
            //}
        }

        private void tx_codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                //if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
                //{
                    ReturnValue1 = tx_codigo.Text;
                    ReturnValue0 = tx_id.Text;
                    ReturnValue2 = tx_nombre.Text;
                //}
                this.Close();
            }
        }

        private void tx_buscar_Leave(object sender, EventArgs e)
        {
            if (tx_buscar.Text != "")
            {
                dataGridView1.Rows.Clear();
                int li = 0;   // contador de las lineas a llenar el datagrid
                for (li = 0; li < dtDatos.Rows.Count; li++) // 
                {
                    DataRow row = dtDatos.Rows[li];
                    string cols4 = "items,xxx";         // 4 columnas
                    string cols5 = "stocks,qqq";        // 5 columnas, 3ra fecha
                    string colst = "socios";            // 5 columnas sn fecha
                    if (row.ItemArray[1].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))    // campo nombre
                    {
                        if (colst.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString());
                        }
                        if (cols4.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString());
                        }
                        if (cols5.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                string.Format("{0:dd/MM/yyyy}", row.ItemArray[3]),
                                                row.ItemArray[4].ToString());
                        }
                    }
                }
            }
            else loadgrids();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            tx_codigo.Text = cellva;
            this.tx_id.Text = "";
            iOMG.Program.retorna1 = cellva;
            tx_codigo.Focus();
        }
    }
}
