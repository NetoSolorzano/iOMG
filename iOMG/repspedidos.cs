using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace iOMG
{
    public partial class repspedidos : Form
    {
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();

        public repspedidos()
        {
            InitializeComponent();
        }

        private void repspedidos_Load(object sender, EventArgs e)
        {
            dataload("maestra");        // revisar  
            grilla();
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
            if (quien == "maestra")
            {
                // datos de los pedidos
                string datgri = "select id,codped,tipoes,origen,destino,date_format(date(fecha),'%Y-%m-%d') as fecha,date_format(date(entrega),'%Y-%m-%d') as entrega,coment " +
                    "from pedidos where tipoes=@tip";
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                cdg.Parameters.AddWithValue("@tip", "TPE001");
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Dispose();
            }
            //
            conn.Close();
        }
        private void grilla()                               // arma la advancedatagrid
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            /*
            // id 
            advancedDataGridView1.Columns[0].Visible = false;
            // codigo de pedido
            advancedDataGridView1.Columns[1].Visible = true;            // columna visible o no
            advancedDataGridView1.Columns[1].HeaderText = "Pedido";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 70;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[1].Tag = "validaNO";
            advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // tipo de pedido
            advancedDataGridView1.Columns[2].Visible = true;
            advancedDataGridView1.Columns[2].HeaderText = "Tipo Ped";    // titulo de la columna
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
            */
        }

        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            dtg.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString;
        }
    }
}
