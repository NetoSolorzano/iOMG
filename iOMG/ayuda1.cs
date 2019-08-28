using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace iOMG
{
    public partial class ayuda1 : Form
    {
        //static string nomform = "frmayud01"; // nombre del formulario
        public string para1 = "";
        public string para2 = "";
        public string para3 = "";
        // Se crea un DataTable que almacenará los datos desde donde se cargaran los datos al DataGridView
        DataTable dtDatos = new DataTable();
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";ConnectionLifeTime=" + ctl + ";";

        public ayuda1(string param1,string param2,string param3)
        {
            para1 = param1;
            para2 = param2;
            para3 = param3;
            InitializeComponent();
        }
        private void ayuda1_Load(object sender, EventArgs e)
        {
            loadgrids();    // datos del grid
        }
        private void ayuda1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string ReturnValue0 { get; set; }
        public string ReturnValue2 { get; set; }
        public string ReturnValue3 { get; set; }
        public string ReturnValue4 { get; set; }

        public void loadgrids()
        {
            // DATOS DE LA GRILLA
            string consulta = "";
            if (para1 == "detaped" && para2 != "" && para3 == "")   // detalle de pedido, codpedido, nulo
            {
                //consulta = "select trim(item),trim(nombre),cant,saldo from detaped where pedidoh=@para2";
                consulta = "select trim(a.item),trim(a.nombre),a.cant,a.saldo,a.medidas,b.soles2018,b.medid,b.umed " +
                    "from detaped a left join items b on " +
                    "substr(a.item,1,4)=substr(b.codig,1,4) and substr(a.item,6,4)=substr(b.codig,6,4) and substr(a.item,13,6)=substr(b.codig,13,6) " +
                    "where a.pedidoh=@para2";
                // left(a.item,18)=left(b.codig,18) 
                // Acomodamos la grilla
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.ColumnCount = 6;
                this.dataGridView1.Columns[0].Name = " Código";
                this.dataGridView1.Columns[0].Width = 130;
                this.dataGridView1.Columns[1].Name = " Nombre";
                this.dataGridView1.Columns[1].Width = 210;
                this.dataGridView1.Columns[2].Name = "Can";
                this.dataGridView1.Columns[2].Width = 30;
                this.dataGridView1.Columns[3].Name = "Sald";
                this.dataGridView1.Columns[3].Width = 30;
                dataGridView1.Columns[4].Name = "medidas";
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Name = "precio";
                dataGridView1.Columns[5].Visible = false;
            }
            if (para1 == "items" && para2 == "parcial" && para3 == "")
            {
                consulta = "select trim(codig),trim(nombr),concat(capit,model,mader,tipol,deta1,acaba,deta2) " +
                    "from items group by capit,model,mader,tipol,deta1,acaba,deta2";
                // Acomodamos la grilla
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.ColumnCount = 3;
                this.dataGridView1.Columns[0].Name = " Codigo";
                this.dataGridView1.Columns[0].Width = 130;
                this.dataGridView1.Columns[1].Name = " Nombre";
                this.dataGridView1.Columns[1].Width = 180;
                this.dataGridView1.Columns[2].Name = " Parcial";
                this.dataGridView1.Columns[2].Width = 80;
            }
            // Se crea un MySqlAdapter para obtener los datos de la base
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, cn);
                if (para1 == "detaped")
                {
                    if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                }
                if (para1 == "items")
                {
                    //if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                    //if (para3 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para3", para3);
                }
                if (para1 == "yyy" || para1 == "instital" || para1 == "certificados")
                {
                    if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                }
                if (para1 == "zzzz" && para2 != "")
                {
                    mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                }
                mdaDatos.Fill(dtDatos);
                int li = 0;   // contador de las lineas a llenar el datagrid
                for (li = 0; li < dtDatos.Rows.Count; li++) // 
                {
                    DataRow row = dtDatos.Rows[li];
                    string cols4 = "detaped";
                    string cols5 = "qqq";           // columna 3 fecha, 5 columnas
                    string colst = "items";         // 3 columnas sn fecha
                    if (colst.Contains(para1))
                    {
                        dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                            row.ItemArray[1].ToString(),
                                            row.ItemArray[2].ToString());
                    }
                    if (cols4.Contains(para1))
                    {
                        dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                            row.ItemArray[1].ToString(),
                                            row.ItemArray[2].ToString(),
                                            row.ItemArray[3].ToString(),
                                            row.ItemArray[4].ToString(),
                                            row.ItemArray[5].ToString());
                        if (Int16.Parse(dataGridView1.CurrentRow.Cells["sald"].Value.ToString()) <= 0)
                        {
                            dataGridView1.CurrentRow.ReadOnly = true;
                            //dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = 
                        }

                    }
                    if (cols5.Contains(para1))
                    {
                        dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                            row.ItemArray[1].ToString(),
                                            row.ItemArray[2].ToString());
                    }
                }
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReturnValue1 = tx_codigo.Text;
            this.ReturnValue0 = tx_id.Text;
            this.ReturnValue2 = tx_nombre.Text;
            if (para1 == "detaped")
            {
                ReturnValue3 = dataGridView1.CurrentRow.Cells[4].Value.ToString();  // medidas
                ReturnValue4 = dataGridView1.CurrentRow.Cells[5].Value.ToString();  // precio
            }
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                this.tx_codigo.Text = cellva;
                this.tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                iOMG.Program.retorna1 = cellva;
                this.tx_codigo.Focus();
                //this.Close();
        }

        private void tx_codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                this.ReturnValue1 = tx_codigo.Text;
                this.ReturnValue0 = tx_id.Text;
                this.ReturnValue2 = tx_nombre.Text;
                if (para1 == "detaped")
                {
                    ReturnValue3 = dataGridView1.CurrentRow.Cells[4].Value.ToString();  // medidas
                    ReturnValue4 = dataGridView1.CurrentRow.Cells[5].Value.ToString();  // precio
                }
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
                    string cols4 = "detaped";
                    string cols5 = "qqq";           // columna 3 fecha  
                    string colst = "items";         // 3 columnas sn fecha
                    if (row.ItemArray[1].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))    // campo nombre, socios
                    {
                        if (colst.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString());
                        }
                        if (cols4.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString());
                            if (Int16.Parse(dataGridView1.CurrentRow.Cells["sald"].Value.ToString()) <= 0)
                            {
                                dataGridView1.CurrentRow.ReadOnly = true;
                                //dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = 
                            }

                        }
                        if (cols5.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString());
                        }
                    }
                }
            }
            else loadgrids();
        }
    }
}
