using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace iOMG
{
    public partial class forpcred : Form
    {
        DataTable dt;           // plazos, monto y fecha
        bool SoloLee = false;   // si no es modo "nuevo" debe ser solo lectura
        string _feComp = "";    // fecha del comprobante

        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();

        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + " " + ";default command timeout=120" +
            ";ConnectionLifeTime=" + ctl + ";";

        public forpcred(string[,] idavuelta, bool sololee, string fechComp)
        {
            InitializeComponent();
            if (idavuelta[0, 2] != null && idavuelta[0, 2].ToString() != "")    // 
            {
                for (int i=0; i<9; i++)
                {
                    if (idavuelta[i, 2] != null)
                    {
                        if (idavuelta[i, 2].ToString() != "")
                        {
                            dataGridView1.Rows.Add(0, i + 1, idavuelta[i, 2].ToString(), idavuelta[i, 3].ToString());
                        }
                    }
                }
            }
            if (sololee == true) SoloLee = true;
            _feComp = fechComp;
        }
        private void forpcred_Load(object sender, EventArgs e)
        {
            jalainfo();
            //foreach (DataRow row in dt.Rows)
            {
                totalizagrid();
            }
            if (SoloLee == true)
            {
                bt_mas.Enabled = false;
                button1.Enabled = false;
                dataGridView1.Enabled = false;
            }
        }
        private void forpcred_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
        private void jalainfo()                                                 // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "forpcred");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "forpcred")
                    {
                        //if (row["campo"].ToString() == "efec_bancariz" && row["param"].ToString() == "limite") vlimban = double.Parse(row["valor"].ToString().Trim()); // valor limite para banzarizar efectivos
                        //if (row["campo"].ToString() == "efec_bancariz" && row["param"].ToString() == "glosa1") vglosa1 = row["valor"].ToString().Trim();               // glosa del limite bancarizacion
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
        public string ReturnValue1 { get; set; }
        public string ReturnValue2 { get; set; }

        public string[,] ReturnValue = new string[10, 4];

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnValue1 = tx_tfil.Text;
            ReturnValue2 = tx_total.Text;
            int i = 0;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    ReturnValue[i, 0] = "0";
                    ReturnValue[i, 1] = (i + 1).ToString();
                    ReturnValue[i, 2] = row.Cells[2].Value.ToString();
                    ReturnValue[i, 3] = row.Cells[3].Value.ToString().Substring(0, 10);
                }
                i = i + 1;
            }
            totalizagrid();
            this.Close();
        }
        private void bt_mas_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 9)
            {
                MessageBox.Show("No se puede ingresar mas cuotas", "Limite de cuotas excedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // la fecha de pago por defecto es la fecha del día
            if (tx_fpago.Value.Date <= DateTime.Parse(_feComp).Date)
            {
                MessageBox.Show("La fecha debe ser mayor al del comprobante", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_fpago.Focus();
                return;
            }
            double vi = 0;
            double.TryParse(tx_importe.Text, out vi);
            if (tx_importe.Text.Trim() == "" || vi <= 0)
            {
                MessageBox.Show("El importe debe ser > 0", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_importe.Focus();
                return;
            }

            dataGridView1.Rows.Add(0,dataGridView1.Rows.Count-1,tx_importe.Text,tx_fpago.Text);
            totalizagrid();
            //
            tx_num.Text = "";
            tx_importe.Text = "";
            dataGridView1.Focus();
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            {
                var aa = MessageBox.Show("Confirma que desea borrar la cuota?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    // ...
                }
            }
        }
        private void totalizagrid()
        {
            int i = 0;
            double vb = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().Trim() != "")
                {
                    row.Cells[1].Value = (i + 1).ToString();
                    vb = vb + double.Parse(row.Cells[2].Value.ToString());
                    i += 1;
                }
            }
            tx_total.Text = vb.ToString("#0.00");
            tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
        }
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            totalizagrid();
        }
        private void tx_fpago_Leave(object sender, EventArgs e)
        {
            bt_mas.Focus();
        }
        private void tx_fpago_ValueChanged(object sender, EventArgs e)
        {
            if (tx_fpago.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("La fecha del pago no puede" + Environment.NewLine +
                    "ser menor a la fecha actual!", "Error de fecha", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tx_fpago.Value = DateTime.Now;
            }
        }
    }
}
