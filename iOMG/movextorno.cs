using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class movextorno : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        string para1;
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";ConnectionLifeTime=" + ctl + ";";
        
        public movextorno(string parm1)
        {
            InitializeComponent();
            lb_titulo.Text = parm1.ToUpper();
            para1 = parm1;  // titulo del form
            this.KeyPreview = true; // habilitando la posibilidad de pasar el tab con el enter
        }
        private void movextorno_Load(object sender, EventArgs e)
        {
            tx_idr.Text = "";
            tx_idr.ReadOnly = false;
            tx_salida.Text = "";
            tx_salida.ReadOnly = true;
            tx_contrato.Text = "";
            tx_contrato.ReadOnly = true;
        }
        private void movextorno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void bt_close_Click(object sender, EventArgs e)
        {
            retorno = false;    // false = no se hizo nada
            this.Close();
        }

        private void tx_idr_Leave(object sender, EventArgs e)
        {
            // jalamos los datos del id
            if (tx_idr.Text.Trim() != "")
            {
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    dataGridView1.DataSource = null;
                    dataGridView1.ReadOnly = true;
                    string consulta = "select * from vendalm where ida = @idr";
                    using (MySqlCommand micon = new MySqlCommand(consulta,conn))
                    {
                        micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dataGridView1.DataSource = dt;
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            }
                            tx_salida.Text = dataGridView1.Rows[0].Cells["salida"].Value.ToString();
                            tx_contrato.Text = dataGridView1.Rows[0].Cells["contrat"].Value.ToString();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tx_salida.Text == "")
            {
                MessageBox.Show("El id no tiene salida por ventas", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_idr.Focus();
                return;
            }
            var aa = MessageBox.Show("Confirma que desea grabar la operación?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                if (salidaR() == true) retorno = true; // true = se efectuo la operacion
                else retorno = false;
            }
            this.Close();
        }
        //
        private bool salidaR()
        {
            bool bien = false;
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                // insertamos en almloc
                string inserta = "insert into almloc (" +
                    "id,codalm,fechop,tipop,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,marca,reserva,contrat,salida,evento,almdes,medid,soles2018,idajuste,pedalm) " +
                    "select ida,codalm,fechop,tipop,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,marca,reserva,contrat,salida,evento,almdes,medid,soles2018,idajuste,pedalm " +
                    "from vendalm where ida=@ida";
                MySqlCommand micon = new MySqlCommand(inserta, conn);
                micon.Parameters.AddWithValue("@ida", tx_idr.Text);
                micon.ExecuteNonQuery();
                // anulamos la salidah
                string anula = "update salidash set status='ANULADO' where idsalidash=@ids";
                micon = new MySqlCommand(anula, conn);
                micon.Parameters.AddWithValue("@ids", tx_salida.Text);
                micon.ExecuteNonQuery();
                // restamos en salidasd
                string resta = "update salidasd set cant=cant-@can where salidash=@ids";
                micon = new MySqlCommand(resta, conn);
                micon.Parameters.AddWithValue("@ids", tx_salida.Text);
                micon.Parameters.AddWithValue("@can", 1);
                micon.ExecuteNonQuery();
                // falta actualizar el estado del contrato
                acciones acc = new acciones();
                acc.act_cont(tx_contrato.Text, "RESERVA");
                // borramos de vendalm
                string borra = "delete from vendalm where ida=@idm";
                micon = new MySqlCommand(borra, conn);
                micon.Parameters.AddWithValue("@idm", tx_idr.Text);
                micon.ExecuteNonQuery();
                // kardex
                string accX = "insert into kardex (codalm,fecha,tipmov,item,cant_i,coment,idalm,USER,dias) " +
                            "values (@ptxlle,@fech,'INGRESO',@codi,'1',concat('Extorno vta. salida ',@nsal),@ida,@asd,now())";
                micon = new MySqlCommand(accX, conn);
                micon.Parameters.AddWithValue("@ptxlle", dataGridView1.Rows[0].Cells["codalm"].Value.ToString());
                micon.Parameters.AddWithValue("@fech", DateTime.Now.ToString("yyyy-MM-dd"));
                micon.Parameters.AddWithValue("@codi", dataGridView1.Rows[0].Cells["codig"].Value.ToString());
                micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[0].Cells["ida"].Value.ToString());
                micon.Parameters.AddWithValue("@nsal", tx_salida.Text);
                micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                micon.ExecuteNonQuery();
                //
                bien = true;
            }
            return bien;
        }
    }
}
