using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class movfismas : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        string para1, para2, para3;
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";ConnectionLifeTime=" + ctl + ";";
        
        public movfismas(string parm1,string parm2,string parm3)    // parm1 = modo = reserva o salida
        {                                                       // parm2 = 
            InitializeComponent();                              // parm3 = 
            lb_titulo.Text = parm1.ToUpper(); // modo del movfismasiento
            para1 = parm1;  // modo
            //para2 = parm2;  // almacen de reserva
            //para3 = parm3;
            if (parm1 == "reserva")
            {
                //
            }
            if (parm1 == "salida")
            {
                panel4.Visible = true;
                panel4.Left = 7;
                panel4.Top = 30;
            }
            this.KeyPreview = true; // habilitando la posibilidad de pasar el tab con el enter
        }
        private void movfismas_Load(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                DataTable dt = new DataTable();
                string consulta = "select codigo,nombre,cant,almacen,ida,idres,contrat,evento,almdes from tempo";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 120;   // codigo mueble
                dataGridView1.Columns[1].Width = 190;   // nombre
                dataGridView1.Columns[2].Width = 20;    // cantid
                dataGridView1.Columns[3].Width = 60;    // almacen
                dataGridView1.Columns[4].Width = 40;    // id alm
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Width = 40;    // id reserva
                dataGridView1.Columns[6].Width = 60;    // contrato
                dataGridView1.Columns[7].Width = 120;   // evento
                dataGridView1.Columns[8].Width = 60;    // almacen destino
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void movfismas_KeyDown(object sender, KeyEventArgs e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            var aa = MessageBox.Show("Confirma que desea grabar la operación?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                if (lb_titulo.Text == "RESERVA")
                {
                    // no va
                }
                if (lb_titulo.Text.ToUpper() == "SALIDA")
                {
                    if (salida() == true)
                    {
                        MessageBox.Show("Todas las salidas se hicieron conforme", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        retorno = true; // true = se efectuo la operacion
                    }
                    else
                    {
                        MessageBox.Show("No se pudo efectuar algunas o todas las salidas", "Verifique", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        retorno = false;
                    }
                }
                this.Close();
            }
        }
        //
        private bool salida()
        {
            bool bien = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                // salidas TOTALES
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {   // codigo,nombre,cant,almacen,ida,idres,contrat,evento,almdes
                    string texto = "insert into salidash " +
                        "(fecha,pedido,reserva,evento,coment,user,dia,llegada,partida,tipomov,contrato) " +
                        "values " +
                        "(@ptxfec,@ptxped,@ptxres,@ptxt03,@ptxcom,@vg_us,now(),@ptxlle,@ptxpar,@ptxtmo,@ptxctr)";
                    MySqlCommand micon = new MySqlCommand(texto, cn);
                    micon.Parameters.AddWithValue("@ptxfec", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@ptxped", "");
                    micon.Parameters.AddWithValue("@ptxres", dataGridView1.Rows[i].Cells[5].Value.ToString());  // reserva
                    micon.Parameters.AddWithValue("@ptxt03", dataGridView1.Rows[i].Cells[7].Value.ToString());  // evento
                    micon.Parameters.AddWithValue("@ptxcom", tx_comsal.Text);
                    micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                    micon.Parameters.AddWithValue("@ptxlle", dataGridView1.Rows[i].Cells[8].Value.ToString());
                    micon.Parameters.AddWithValue("@ptxpar", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    micon.Parameters.AddWithValue("@ptxtmo", (dataGridView1.Rows[i].Cells[6].Value.ToString().Trim()=="")? "1":"2");
                    micon.Parameters.AddWithValue("@ptxctr", dataGridView1.Rows[i].Cells[6].Value.ToString());
                    micon.ExecuteNonQuery();
                    //
                    texto = "select MAX(idsalidash) as idreg from salidash";
                    micon = new MySqlCommand(texto, cn);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        tx_idr.Text = dr.GetString(0);
                    }
                    dr.Close();
                    //
                    texto = "insert into salidasd " +
                        "(salidash,item,cant,user,dia,idalm) " +
                        "values " +
                        "(@v_id,@nar,@can,@vg_us,now(),@ida)";
                    micon = new MySqlCommand(texto, cn);
                    micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                    micon.Parameters.AddWithValue("@nar", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    micon.Parameters.AddWithValue("@can", "1");
                    micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                    micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                    micon.ExecuteNonQuery();
                    // actualiza almloc si salida por movimiento o borra del almloc si es salida por venta de una reserva
                    string accion = "";
                    if (dataGridView1.Rows[i].Cells[6].Value.ToString().Trim() != "")   // si tiene contrato = salida por venta = 2
                    {
                        // graba en vendalm solo salidas por venta, osea con reserva
                        string acc2 = "insert into vendalm (ida,codalm,fechop,tipop,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,reserva,contrat,salida,evento,almdes,medid,idajuste,pedalm) " +
                        "select id,codalm,fechop,tipop,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,reserva,contrat,@v_id,evento,almdes,medid,idajuste,pedalm from almloc where id=@ida";
                        micon = new MySqlCommand(acc2, cn);
                        micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.ExecuteNonQuery();
                        // kardex
                        acc2 = "insert into kardex (codalm,fecha,tipmov,item,cant_s,coment,idalm,USER,dias) " +
                            "select codalm,@fech,'SALIDA',codig,'1',concat('X venta - Reserva:',reserva),@v_id,@asd,now() from almloc where id=@ida";
                        micon = new MySqlCommand(acc2, cn);
                        micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                        micon.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.ExecuteNonQuery();
                        // borra en almloc
                        accion = "delete from almloc where id=@idr";
                    }
                    else
                    {
                        string accX = "insert into kardex (codalm,fecha,tipmov,item,cant_s,coment,idalm,USER,dias) " +
                            "select codalm,@fech,'SALIDA',codig,'1',concat('Movimiento ',@ptxlle),@v_id,@asd,now() from almloc where id=@ida";
                        micon = new MySqlCommand(accX, cn);
                        micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                        micon.Parameters.AddWithValue("@ptxlle", dataGridView1.Rows[i].Cells[8].Value.ToString());
                        micon.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.ExecuteNonQuery();
                        accX = "insert into kardex (codalm,fecha,tipmov,item,cant_i,coment,idalm,USER,dias) " +
                            "select @ptxlle,@fech,'INGRESO',codig,'1',concat('Movimiento ',codalm),@v_id,@asd,now() from almloc where id=@ida";
                        micon = new MySqlCommand(accX, cn);
                        micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                        micon.Parameters.AddWithValue("@ptxlle", dataGridView1.Rows[i].Cells[8].Value.ToString());
                        micon.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.ExecuteNonQuery();
                        //
                        accion = "update almloc set codalm=@ptxlle,fechop=@ptxfec,evento='',almdes='',salida='' " +
                            "where id=@idr";
                    }
                    micon = new MySqlCommand(accion, cn);
                    micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[4].Value.ToString());
                    micon.Parameters.AddWithValue("@ptxlle", dataGridView1.Rows[i].Cells[8].Value.ToString());
                    micon.Parameters.AddWithValue("@ptxfec", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                    micon.ExecuteNonQuery();
                    // falta actualizar el estado del contrato
                    acciones acc = new acciones();
                    acc.act_cont(dataGridView1.Rows[i].Cells[6].Value.ToString(), "RESERVA");
                    // 
                }
                bien = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión");
                Application.Exit();
            }
            return bien;
        }
    }
}
