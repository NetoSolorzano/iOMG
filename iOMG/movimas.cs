using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class movimas : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        string para1, para2, para3;
        libreria lib = new libreria();
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";ConnectionLifeTime=" + ctl + ";";
        
        public movimas(string parm1,string parm2,string parm3)    // parm1 = modo = reserva o salida
        {                                                       // parm2 = 
            InitializeComponent();                              // parm3 = 
            lb_titulo.Text = parm1.ToUpper(); // modo del movimasiento
            para1 = parm1;  // modo
            //para2 = parm2;  // almacen de reserva
            //para3 = parm3;
            if (parm1 == "reserva")
            {
                panel3.Visible = true;
                panel3.Left = 7;
                panel3.Top = 30;
                panel4.Visible = false;
            }
            if (parm1 == "salida")
            {
                panel4.Visible = true;
                panel4.Left = 7;
                panel4.Top = 30;
                panel3.Visible = false;
                rb_mov.Checked = true;
                combos();
            }
            this.KeyPreview = true; // habilitando la posibilidad de pasar el tab con el enter
        }
        private void movimas_Load(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                DataTable dt = new DataTable();
                string consulta = "select codigo,nombre,cant,almacen,ida from tempo";
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
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
            cn.Close();
            //
            combos();
        }
        private void movimas_KeyDown(object sender, KeyEventArgs e)
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
                    {
                        MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                        cn.Open();
                        try
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                            {
                                // graba la reserva en la maestra de reservas
                                string texto = "insert into reservh (fecha,contrato,evento,coment,user,dia,almacen) " +
                                    "values (@ptxfec,@ptxcon,@ptxt03,@ptxcom,@vg_us,now(),@ptxalm)";
                                MySqlCommand micon = new MySqlCommand(texto, cn);
                                micon.Parameters.AddWithValue("@ptxfec", DateTime.Now.ToString("yyyy-MM-dd"));
                                micon.Parameters.AddWithValue("@ptxcon", tx_contra.Text);
                                micon.Parameters.AddWithValue("@ptxt03", tx_evento.Text);
                                micon.Parameters.AddWithValue("@ptxcom", tx_comres.Text);
                                micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                                micon.Parameters.AddWithValue("@ptxalm", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                micon.ExecuteNonQuery();
                                //
                                texto="select last_insert_id() as idreservh";
                                micon = new MySqlCommand(texto, cn);
                                MySqlDataReader dr = micon.ExecuteReader();
                                if(dr.Read()){
                                    tx_idr.Text = dr.GetString(0);
                                }
                                dr.Close();
                                // y el detalle de la reserva
                                texto = "insert into reservd (reservh,item,cant,user,dia,almacen,idalm) " +
                                    "values (@ptxidr,@ptxite,@ptxcan,@asd,now(),@ptxalm,@ida)";
                                micon = new MySqlCommand(texto, cn);
                                micon.Parameters.AddWithValue("@ptxidr", tx_idr.Text);
                                micon.Parameters.AddWithValue("@ptxite", dataGridView1.Rows[i].Cells[0].Value.ToString());
                                micon.Parameters.AddWithValue("@ptxcan", "1");
                                micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                                micon.Parameters.AddWithValue("@ptxalm", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                                micon.ExecuteNonQuery();
                                // actualiza saldo en detalle del contrato
                                texto = "UPDATE detacon SET saldo=saldo-@can " +
                                    "where contratoh=@ptxcon and item=@ptxi";
                                micon = new MySqlCommand(texto, cn);
                                micon.Parameters.AddWithValue("@ptxcon", tx_contra.Text);
                                micon.Parameters.AddWithValue("@ptxi", dataGridView1.Rows[i].Cells[0].Value.ToString());
                                micon.Parameters.AddWithValue("@can", 1);
                                micon.ExecuteNonQuery();
                                // actualizamos el temporal
                                texto = "update tempo set idres=@idr,contrat=@cont where ida=@ida";
                                micon = new MySqlCommand(texto, cn);
                                micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                                micon.Parameters.AddWithValue("@cont", tx_contra.Text);
                                micon.Parameters.AddWithValue("@ida", dataGridView1.Rows[i].Cells[4].Value.ToString());
                                micon.ExecuteNonQuery();
                            }
                            // algo hará en estado de contratos
                            string reto = lib.estcont(tx_contra.Text.Trim());
                            //acciones acc = new acciones();
                            //acc.act_cont(tx_contra.Text, "RESERVA");
                            //
                            // en el form llamante deben estar las instrucciones para escribir en la grilla el id reserva y contrato
                            retorno = true; // true = se efectuo la operacion
                        }
                        catch(MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error de conexión");
                            Application.Exit();
                            return;
                        }
                    }
                }
                if (lb_titulo.Text.ToUpper() == "SALIDA")
                {
                    if (salida() == true)
                    {
                        MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                        cn.Open();
                        try
                        {
                            // actualizamos el temporal
                            string texto = "";
                            if(rb_mov.Checked == true) texto = "update tempo set evento=@cont,almdes=@almd";
                            if (rb_ajuste.Checked == true) texto = "update tempo set idres=0,evento=@cont,almdes=@almd";
                            MySqlCommand micon = new MySqlCommand(texto, cn);
                            micon.Parameters.AddWithValue("@cont", tx_evento.Text);
                            micon.Parameters.AddWithValue("@almd", tx_dat_dest.Text);
                            micon.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error en conexión");
                            Application.Exit();
                        }
                        retorno = true; // true = se efectuo la operacion
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
                // si es tipo de salida por movimiento
                if (rb_mov.Checked == true)
                {
                    // debe retornar el evento y almacen de destino
                    bien = true;
                }
                // salida por ajuste
                if (rb_ajuste.Checked == true)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string texto = "insert into salidash " +
                            "(fecha,pedido,reserva,evento,coment,user,dia,llegada,partida,tipomov,contrato) " +
                            "values " +
                            "(@ptxfec,@ptxped,@ptxcon,@ptxt03,@ptxcom,@vg_us,now(),@ptxlle,@ptxpar,@ptxtmo,@ptxctr)";
                        MySqlCommand micon = new MySqlCommand(texto, cn);
                        micon.Parameters.AddWithValue("@ptxfec", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@ptxped", "");
                        micon.Parameters.AddWithValue("@ptxcon", "");
                        micon.Parameters.AddWithValue("@ptxt03", tx_evento.Text);
                        micon.Parameters.AddWithValue("@ptxcom", tx_comsal.Text);
                        micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                        micon.Parameters.AddWithValue("@ptxlle", "");
                        micon.Parameters.AddWithValue("@ptxpar", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        micon.Parameters.AddWithValue("@ptxtmo", "1");
                        micon.Parameters.AddWithValue("@ptxctr", "");
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
                            "(salidash,item,cant,user,dia) " +
                            "values " +
                            "(@v_id,@nar,@can,@vg_us,now())";
                        micon = new MySqlCommand(texto, cn);
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.Parameters.AddWithValue("@nar", dataGridView1.Rows[i].Cells[0].Value.ToString());
                        micon.Parameters.AddWithValue("@can", "1");
                        micon.Parameters.AddWithValue("@vg_us", "Lorenzo");
                        micon.ExecuteNonQuery();
                        // borra en almloc
                        string borra = "delete from almloc where id=@idr";
                        micon = new MySqlCommand(borra, cn);
                        micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.ExecuteNonQuery();
                    }
                    bien = true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión");
                Application.Exit();
            }
            return bien;
        }
        // RESERVAS **********************
        private void tx_contra_Leave(object sender, EventArgs e)    // ACA SE VALIDA QUE LOS MUEBLES SELECCIONADOS ESTEN 
        {                                                           // EN EL GRUPO DE MUEBLES DEL CONTRATO
            if (tx_contra.Text == "")
            {
                button1.Focus();
                return;
            }
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                DataTable dt2 = new DataTable();
                string consulta = "select a.fecha,a.tipoes,a.coment,a.status,b.RazonSocial,trim(c.item),trim(c.nombre),c.cant,c.saldo " +
                    "from contrat a " +
                    "left join anag_cli b on b.idanagrafica=a.cliente " +
                    "left join detacon c on c.contratoh=a.contrato " +
                    "where a.contrato=@cont and a.status in ('PENDIE','LLEPAR','ENTPAR','PEDPAR')";
                try
                {
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cont", tx_contra.Text);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    da.Fill(dt2);
                    if (dt2.Rows.Count < 1)
                    {
                        cn.Close();
                        MessageBox.Show("No existe el contrato ingresado", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        tx_contra.Text = "";
                        tx_contra.Focus();
                        return;
                    }
                    else
                    {
                        tx_fecon.Text = dt2.Rows[0].ItemArray[0].ToString().Substring(0,10);
                        tx_tienda.Text = dt2.Rows[0].ItemArray[1].ToString();
                        tx_comres.Text = dt2.Rows[0].ItemArray[2].ToString();
                        tx_cliente.Text = dt2.Rows[0].ItemArray[4].ToString();
                        tx_status.Text = dt2.Rows[0].ItemArray[3].ToString();
                        for (int s = 0; s < dataGridView1.Rows.Count - 1; s++)  // muebles seleccionados
                        {
                            string sino = "no";
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                DataRow row = dt2.Rows[i];                      // muebles en el contrato
                                if (dataGridView1.Rows[s].Cells[0].Value.ToString() == row[5].ToString())
                                {                                  // valida si los muebles seleccionados estan en el contrato
                                    sino = "si";
                                    if (row[8].ToString() == "0")
                                    {
                                        MessageBox.Show("El mueble " + dataGridView1.Rows[s].Cells[0].Value.ToString() + Environment.NewLine +
                                            "No tiene saldo en el contrato", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        sino = "no";
                                    }
                                    else
                                    {
                                        
                                        row[8] = (int.Parse(row[8].ToString()) - 1).ToString();
                                    }
                                    break;
                                }
                            }
                            if (sino == "no")
                            {
                                MessageBox.Show("El contrato NO CONTIENE el mueble seleccionado o no tiene saldo" + Environment.NewLine +
                                dataGridView1.Rows[s].Cells[0].Value.ToString(), "Atención Revise", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                bt_close_Click(null, null);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, "No se puede ejecutar la consulta");
                    Application.Exit();
                    return;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se puede conectar con el servidor");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void combos()
        {
            this.panel4.Focus();
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                // seleccion de los almacenes de destino
                this.cmb_dest.Items.Clear();
                tx_dat_dest.Text = "";
                ComboItem citem_dest = new ComboItem();
                const string condest = "select descrizionerid,idcodice from desc_alm " +
                    "where numero=1";
                MySqlCommand cmd2 = new MySqlCommand(condest, cn);
                DataTable dt2 = new DataTable();
                MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
                da2.Fill(dt2);
                foreach (DataRow row in dt2.Rows)
                {
                    citem_dest.Text = row.ItemArray[0].ToString();
                    citem_dest.Value = row.ItemArray[1].ToString();
                    this.cmb_dest.Items.Add(citem_dest);
                    this.cmb_dest.ValueMember = citem_dest.Value.ToString();
                }
                cn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se puede conectar al servidor");
                Application.Exit();
                return;
            }
        }
        private void cmb_dest_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                //int aq = Int16.Parse(this.cmb_dest.SelectedIndex.ToString());
                string consulta = "select idcodice from desc_alm where descrizionerid=@des and numero=1";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                micon.Parameters.AddWithValue("@des", cmb_dest.Text.ToString());
                MySqlDataReader midr = micon.ExecuteReader();
                if (midr.Read())
                {
                    this.tx_dat_dest.Text = midr["idcodice"].ToString();
                }
                midr.Close();
                cn.Close();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se pudo conectar con el servidor");
                Application.Exit();
                return;
            }
        }

        private void rb_mov_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_mov.Checked == true)
            {
                tx_dat_dest.Text = "";
                cmb_dest.Enabled = true;
                tx_evento.Enabled = true;
            }
        }
        private void rb_ajuste_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ajuste.Checked == true)
            {
                tx_dat_dest.Text = "";
                cmb_dest.SelectedIndex = -1;
                cmb_dest.Enabled = false;
                tx_evento.Text = "";
                tx_evento.Enabled = false;
            }
        }
    }
}
