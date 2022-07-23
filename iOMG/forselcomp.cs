using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace iOMG
{
    public partial class forselcomp : Form
    {
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + " " + ";default command timeout=120" +
        ";ConnectionLifeTime=" + ctl + ";";
        DataTable dt;           // medios de pago
        string _vlocal = "";    // local del usuario
        string _vanul = "";     // estado doc.venta anulado
        int _intfec = 1;        // intervalo de días atras para la consulta de comprobantes
        public forselcomp(string vlocal, string vanul, int intfec)
        {
            _vlocal = vlocal;
            _vanul = vanul;
            _intfec = intfec;
            InitializeComponent();
        }
        private void forselcomp_Load(object sender, EventArgs e)
        {
            jalaoc();
        }
        private void forselcomp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public DataTable ReturnValueT = new DataTable();  

        private void button1_Click(object sender, EventArgs e)
        {
            DataColumn dc0 = new DataColumn("comprob", typeof(String));
            DataColumn dc1 = new DataColumn("tipdv", typeof(String));
            DataColumn dc2 = new DataColumn("serdv", typeof(String));
            DataColumn dc3 = new DataColumn("cordv", typeof(String));
            ReturnValueT.Columns.Add(dc0);
            ReturnValueT.Columns.Add(dc1);
            ReturnValueT.Columns.Add(dc2);
            ReturnValueT.Columns.Add(dc3);

            ReturnValue1 = tx_total.Text;
            int i = 0;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
                {
                    DataRow dr = ReturnValueT.NewRow();
                    dr[0] = row.Cells[1].Value.ToString();
                    dr[1] = row.Cells[6].Value.ToString();
                    dr[2] = row.Cells[7].Value.ToString();
                    dr[3] = row.Cells[8].Value.ToString();
                    ReturnValueT.Rows.InsertAt(dr, i);
                }
                i = i + 1;
            }
            this.Close();
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            {
                var aa = MessageBox.Show("Confirma que desea borrar el pago?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    totalizagrid();
                }
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // check
            {
                
                totalizagrid();
            }
        }
        private void totalizagrid()
        {
            double vb = 0;
            int cnt = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString().Trim() == "True")
                {
                    vb = vb + double.Parse(row.Cells[5].Value.ToString());
                    cnt = cnt + 1;
                }
            }
            tx_total.Text = vb.ToString("#0.00");
            tx_tfil.Text = (cnt).ToString();
        }
        private void jalaoc()
        {
            // llenamos la grilla con: 
            // - docs.vta que no tienen contrato
            // - docs.vta que sean del local del usuario y con fecha no menos a 2 días atras
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consu = "SELECT CONCAT(martdve,serdvta,'-',numdvta) AS comprob,fechope,nudoclt,nombclt,mondvta,totdvta,tipdvta,serdvta,numdvta " +
                        "FROM cabfactu WHERE contrato='' AND estdvta<>@esanu AND locorig=@loca AND fechope >= NOW() - INTERVAL @da DAY";
                    using (MySqlCommand micon = new MySqlCommand(consu, conn))
                    {
                        micon.Parameters.AddWithValue("@esanu", _vanul);     // codigo estado anulado
                        micon.Parameters.AddWithValue("@loca", _vlocal);     // codigo local usuario
                        micon.Parameters.AddWithValue("@da", _intfec);       // intervalo días atras
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.ItemArray[0] != null)
                                {
                                    dataGridView1.Rows.Add(0,row.ItemArray[0].ToString(),row.ItemArray[1].ToString().Substring(0,10),row.ItemArray[2].ToString(),
                                        row.ItemArray[3].ToString(),row.ItemArray[5].ToString(),row.ItemArray[6].ToString(),row.ItemArray[7].ToString(),row.ItemArray[8].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Se perdió conexión con el servidor","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
        }

    }
}
