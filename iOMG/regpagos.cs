using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class regpagos : Form
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

        public regpagos(string param1, string param2, string param3, string param4)
        {
            para1 = param1;              // 
            para2 = param2;              //
            para3 = param3;              //
            para4 = param4;              // 
            InitializeComponent();
        }

        private void regpagos_Load(object sender, EventArgs e)
        {
            loadgrids();    // datos del grid
        }

        private void regpagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string ReturnValue0 { get; set; }
        public string ReturnValue2 { get; set; }

        private void loadgrids()
        {
            string consulta = "select idpagamenti,fecha,montosol,via,detalle,dv,serie,numero,space(1) as marca " +
                    "from pagamenti where contrato=@cont";
            // 
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "FECHA";
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "MONTO";
            dataGridView1.Columns[2].Width = 70;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "MEDIO";
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "COMENT";
            dataGridView1.Columns[4].Width = 170;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].HeaderText = "DOCVTA";
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].HeaderText = "SERIE";
            dataGridView1.Columns[6].Width = 50;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].HeaderText = "NUMERO";
            dataGridView1.Columns[7].Width = 70;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].Visible = false;       // marca N=nuevo A=actualizado
            //
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                mdaDatos.SelectCommand.Parameters.AddWithValue("@cont", para2);
                mdaDatos.Fill(dtDatos);
                int li = 0;   // contador de las lineas a llenar el datagrid
                for (li = 0; li < dtDatos.Rows.Count; li++) // 
                {
                    DataRow row = dtDatos.Rows[li];
                    dataGridView1.Rows.Add(
                                        row.ItemArray[0].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[3].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[5].ToString(),
                                        row.ItemArray[6].ToString(),
                                        row.ItemArray[7].ToString()
                                        );
                }
            }
            calcula();
        }
        private void calcula()
        {
            decimal toti = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                toti = toti + decimal.Parse(row.Cells["montosol"].Value.ToString());
            }
            tx_total.Text = toti.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var aa = MessageBox.Show("Confirma que desea GRABAR?", "Atención confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["marca"].Value.ToString() == "A")
                    {
                        row.Cells["idpagamenti"].Value;
                        row.Cells["fecha"].Value = dtp_pago.Value;
                        row.Cells["montosol"].Value = tx_importe.Text;
                        row.Cells["via"].Value = tx_dat_fpago.Text;
                        row.Cells["detalle"].Value = tx_comen.Text.Trim();
                        row.Cells["dv"].Value = tx_dat_td.Text;
                        row.Cells["serie"].Value = tx_serie.Text;
                        row.Cells["numero"].Value = tx_corre.Text;
                        
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // idpagamenti,fecha,montosol,via,detalle,dv,serie,numero
            tx_idr.Text = dataGridView1.CurrentRow.Cells["idpagamenti"].Value.ToString();
            dtp_pago.Value = DateTime.Parse(dataGridView1.CurrentRow.Cells["fecha"].Value.ToString());
            tx_importe.Text = dataGridView1.CurrentRow.Cells["montosol"].Value.ToString();
            tx_dat_fpago.Text = dataGridView1.CurrentRow.Cells["via"].Value.ToString();
            cmb_fpago.SelectedIndex = cmb_fpago.FindString(tx_dat_fpago.Text);
            tx_dat_td.Text = dataGridView1.CurrentRow.Cells["dv"].Value.ToString();
            cmb_td.SelectedIndex = cmb_td.FindString(tx_dat_td.Text);
            tx_serie.Text = dataGridView1.CurrentRow.Cells["serie"].Value.ToString();
            tx_corre.Text = dataGridView1.CurrentRow.Cells["numero"].Value.ToString();
            tx_comen.Text = dataGridView1.CurrentRow.Cells["detalle"].Value.ToString();
        }

        private void bt_det_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text.Trim() == "")
            {
                // idpagamenti,fecha,montosol,via,detalle,dv,serie,numero,marca
                dataGridView1.Rows.Add(0,
                    dtp_pago.Value.ToString("yyyy-MM-dd"),
                    tx_importe.Text,
                    tx_dat_fpago.Text,
                    tx_comen.Text.Trim(),
                    tx_dat_td.Text,
                    tx_serie.Text,
                    tx_corre.Text,
                    "N");
            }
            if (tx_idr.Text.Trim() != "")
            {
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["idpagamenti"].Value.ToString() == tx_idr.Text)
                    {
                        row.Cells["fecha"].Value = dtp_pago.Value;
                        row.Cells["montosol"].Value = tx_importe.Text;
                        row.Cells["via"].Value = tx_dat_fpago.Text;
                        row.Cells["detalle"].Value = tx_comen.Text.Trim();
                        row.Cells["dv"].Value = tx_dat_td.Text;
                        row.Cells["serie"].Value = tx_serie.Text;
                        row.Cells["numero"].Value = tx_corre.Text;
                        row.Cells["marca"].Value = "A";
                    }
                }
            }
            calcula();
            // limpiamos
            tx_idr.Text = "";
            dtp_pago.Value = DateTime.Now;
            tx_importe.Text = "";
            tx_dat_fpago.Text = "";
            cmb_fpago.SelectedIndex = -1;
            tx_dat_td.Text = "";
            cmb_td.SelectedIndex = -1;
            tx_serie.Text = "";
            tx_corre.Text = "";
            tx_comen.Text = "";
        }
    }
}
