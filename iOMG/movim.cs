using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class movim : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        public string retval1;  // id reserva que retorna al form llamante / id de salida NO HAY PORQUE AUN NO SALE, ESTO ES AUTORIZ.
        public string retval2;  // contrato que retorna al form llamante   / evento de salida autorizado
        public string retval3;  // en reservas no se usa este campo        / almacen hacia donde llegara el mueble
        string para1, para2, para3, para4;
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + 
            ";ConnectionLifeTime=" + ctl + ";default command timeout=120";

        public movim(string parm1,string parm2,string parm3,string parm4)    // parm1 = modo = reserva o salida
        {                                                       // parm2 = id del mueble
            InitializeComponent();                              // parm3 = codigo del mueble
            lb_titulo.Text = parm1.ToUpper(); // modo del movimiento
            para1 = parm1;  // modo
            para2 = parm2;  // id almacen del mueble
            para3 = parm3;  // codig mueble
            para4 = parm4;  // almacen de donde se reserva
            if (parm1 == "reserva")
            {
                panel3.Visible = true;
                panel3.Left = 0;
                panel3.Top = 30;
                panel4.Visible = false;
            }
            if (parm1 == "salida")
            {
                panel4.Visible = true;
                panel4.Left = 0;
                panel4.Top = 30;
                panel3.Visible = false;
                rb_mov.Checked = true;
                combos();
            }
        }
        private void movim_Load(object sender, EventArgs e)
        {
            combos();
            tx_contra.Focus();
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
                if (lb_titulo.Text.ToLower() == "reserva")
                {
                    if (reserva() == true)
                    {
                        // retornamos los datos de id reserva y contrato
                        retval1 = tx_idr.Text;
                        retval2 = tx_contra.Text;
                        retorno = true; // true = se efectuo la operacion
                    }
                }
                if (lb_titulo.Text == "SALIDA")
                {
                    if (tx_dat_dest.Text == "" && rb_mov.Checked == true)
                    {
                        MessageBox.Show("Seleccione el almacen de destino", "Atención", MessageBoxButtons.OK);
                        cmb_dest.Focus();
                        return;
                    }
                    if (salida() == true)
                    {
                        // retornamos el evento y almacen destino ...SIEMPRE Y CUANDO SEA SALIDA POR MOVIMIENTO
                        // si es salida por AJUSTE el id=0
                        retval1 = (rb_ajuste.Checked == true) ? "0" : "";
                        retval2 = tx_evento.Text;
                        retval3 = tx_dat_dest.Text;
                        retorno = true; // true = se efectuo la operacion
                    }
                }
                this.Close();
            }
        }
        //
        private bool reserva()
        {
            bool bien = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                // inserta la reserva en maestra de reservas   
                string texto = "insert into reservh (fecha,contrato,evento,coment,user,dia,almacen) " +
                    "values (@ptxfec,@ptxcon,@ptxt03,@ptxcom,@vg_us,now(),@ptxalm)";
                MySqlCommand micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@ptxfec", DateTime.Now.ToString("yyyy-MM-dd"));
                micon.Parameters.AddWithValue("@ptxcon", tx_contra.Text);
                micon.Parameters.AddWithValue("@ptxt03", tx_evento.Text);
                micon.Parameters.AddWithValue("@ptxcom", tx_comres.Text);
                micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                micon.Parameters.AddWithValue("@ptxalm", para4);    // almacen
                micon.ExecuteNonQuery();
                //
                texto = "select last_insert_id() as idreservh";
                micon = new MySqlCommand(texto, cn);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.Read())
                {
                    tx_idr.Text = dr.GetString(0);
                }
                dr.Close();
                // y el detalle de la reserva
                texto = "insert into reservd (reservh,item,cant,user,dia,almacen,idalm,itemCont) " +
                    "values (@ptxidr,@ptxite,@ptxcan,@asd,now(),@ptxalm,@ida,@itcon)";
                micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@ptxidr", tx_idr.Text);
                micon.Parameters.AddWithValue("@ptxite", para3); // codigo del mueble
                micon.Parameters.AddWithValue("@ptxcan", "1");
                micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                micon.Parameters.AddWithValue("@ptxalm", para4);
                micon.Parameters.AddWithValue("@ida", para2);
                micon.Parameters.AddWithValue("@itcon", tx_d_codi.Text);
                micon.ExecuteNonQuery();
                // actualiza saldo en detalle del contrato
                texto = "UPDATE detacon SET saldo=saldo-@can " +
                    "where contratoh=@ptxcon and item=@ptxi";
                micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@ptxcon", tx_contra.Text);
                micon.Parameters.AddWithValue("@ptxi", tx_d_codi.Text);   // para3
                micon.Parameters.AddWithValue("@can", 1);
                micon.ExecuteNonQuery();
                // algo hará en estado de contratos
                acciones acc = new acciones();              // revisar si usamos esto
                acc.act_cont(tx_contra.Text, "RESERVA");    // o el actualizador de estado en la libreria 09/09/2019
                // actualizamos el temporal
                texto = "update tempo set idres=@idr,contrat=@cont where ida=@ida";
                micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                micon.Parameters.AddWithValue("@cont", tx_contra.Text);
                micon.Parameters.AddWithValue("@ida", para2);
                micon.ExecuteNonQuery();
                // actualizamos almloc
                texto = "update almloc set reserva=@res,contrat=@con,marca=0 where id=@ida";
                micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@res", tx_idr.Text);
                micon.Parameters.AddWithValue("@con", tx_contra.Text);
                micon.Parameters.AddWithValue("@ida", para2);
                micon.ExecuteNonQuery();
                //advancedDataGridView1.Rows[i].Cells["marca"].Value = 0;
                bien = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión");
                Application.Exit();
            }
            cn.Close();
            return bien;
        }
        private bool salida()
        {
            bool bien = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {

                if (rb_mov.Checked == true)
                {
                    // actualizamos la tabla almloc
                    string actua = "update almloc set evento=@even,almdes=@alm where id=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, cn);
                    micon.Parameters.AddWithValue("@even", tx_evento.Text);
                    micon.Parameters.AddWithValue("@alm", tx_dat_dest.Text);
                    micon.Parameters.AddWithValue("@idr", para2);
                    micon.ExecuteNonQuery();
                    //
                    bien = true;
                }
                else
                {   // salida por ajuste 
                    // graba la salida en cabecera y detalle
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
                    micon.Parameters.AddWithValue("@ptxpar", para4);
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
                    micon.Parameters.AddWithValue("@nar", para3);
                    micon.Parameters.AddWithValue("@can", "1");
                    micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                    micon.ExecuteNonQuery();
                    // borra en almloc
                    string borra = "delete from almloc where id=@idr";
                    micon = new MySqlCommand(borra, cn);
                    micon.Parameters.AddWithValue("@idr", para2);
                    micon.ExecuteNonQuery();
                    // kardex
                    string acc2 = "insert into kardex (codalm,fecha,tipmov,item,cant_s,coment,idalm,USER,dias) " +
                        "values (@ptxpar,@ptxfec,'SALIDA',@nar,@can,'Ajuste',@idr,@vg_us,now())";
                    micon = new MySqlCommand(acc2, cn);
                    micon.Parameters.AddWithValue("@ptxpar", para4);
                    micon.Parameters.AddWithValue("@ptxfec", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@nar", para3);
                    micon.Parameters.AddWithValue("@can", "1");
                    micon.Parameters.AddWithValue("@idr", para2);
                    micon.Parameters.AddWithValue("@vg_us", iOMG.Program.vg_user);
                    micon.ExecuteNonQuery();
                    //
                    bien = true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión al servidor");
                Application.Exit();
            }
            cn.Close();
            return bien;
        }
        // RESERVAS **********************
        private void tx_contra_Leave(object sender, EventArgs e)
        {
            if (tx_contra.Text == "")
            {
                button1.Focus();
                return;
            }
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                DataTable dt = new DataTable();
                string consulta = "select a.fecha,a.tipoes,a.coment,a.status,b.RazonSocial,trim(c.item),c.cant,trim(c.nombre),c.coment as comitem,ifnull(x.cant,0) " +
                    "from contrat a " +
                    "left join anag_cli b on b.idanagrafica = a.cliente " +
                    "left join detacon c on c.contratoh = a.contrato AND c.saldo>0 " +
                    "LEFT JOIN (SELECT a.item, sum(a.cant) AS cant FROM reservd a LEFT JOIN reservh b ON a.reservh = b.idreservh " +
                        "WHERE b.contrato = @cont AND b.status <> 'ANULADO' GROUP BY a.item) x on x.item = concat(left(c.item, 10), SUBSTRING(c.item, 13, 6)) " +
                    "where a.contrato = @cont and a.status <> 'ENTREG'";
                try
                {
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cont", tx_contra.Text);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    da.Fill(dt);
                    if (dt.Rows.Count < 1)
                    {
                        cn.Close();
                        MessageBox.Show("No existe el contrato ingresado o esta entregado", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        tx_contra.Text = "";
                        tx_contra.Focus();
                        return;
                    }
                    else
                    {
                        tx_fecon.Text = dt.Rows[0].ItemArray[0].ToString().Substring(0,10);
                        tx_tienda.Text = dt.Rows[0].ItemArray[1].ToString();
                        tx_comres.Text = dt.Rows[0].ItemArray[2].ToString();
                        tx_cliente.Text = dt.Rows[0].ItemArray[4].ToString();
                        tx_status.Text = dt.Rows[0].ItemArray[3].ToString();
                        dataGridView1.ColumnCount = 4;
                        dataGridView1.Columns[0].Width = 160;
                        dataGridView1.Columns[0].HeaderText = dt.Columns[5].Caption;
                        dataGridView1.Columns[1].Width = 30;
                        dataGridView1.Columns[1].HeaderText = dt.Columns[6].Caption;
                        dataGridView1.Columns[2].Width = 230;
                        dataGridView1.Columns[2].HeaderText = dt.Columns[7].Caption;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[3].HeaderText = dt.Columns[8].Caption;
                        string sino = "no";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            dataGridView1.Rows.Add(row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString());
                            string parte1 = "";     // item del contrato
                            if (row[5].ToString().Trim().Length == 18)
                            {
                                parte1 = row[5].ToString().Trim().Substring(0, 10) + row[5].ToString().Trim().Substring(12, 6);   // item del contrato
                            }
                            if (row[5].ToString().Trim().Length == 16)
                            {
                                parte1 = row[5].ToString().Trim();   // item del contrato
                            }
                            string parte2 = para3.Trim();               // item del almloc
                            if (parte1 == parte2)
                            {
                                sino = "si";    // aca debemos validar por columnas
                                tx_comres.Text = row[8].ToString();
                                tx_d_codi.Text = row[5].ToString();
                            }
                            else
                            {
                                if (parte1 != "")
                                {
                                    if (parte1.Substring(1, 3) == "000")     // vemos si el item del contrato es A DISEÑO
                                    {
                                        if (parte1.Substring(0, 1) == parte2.Substring(0, 1) 
                                            && parte1.Substring(4, 1) == parte2.Substring(4, 1) 
                                            && parte1.Substring(5, 2) == parte2.Substring(5, 2))
                                        {
                                            // en este caso, el item del contrato es a diseño y el capitulo y madera son iguales
                                            // 09/04/2021 ... agregando la tipologia (5,2)
                                            sino = "si";
                                            tx_comres.Text = row[8].ToString();
                                            tx_d_codi.Text = row[5].ToString();
                                        }
                                    }
                                }
                            }
                            if (row[9] != null) // comparamos la cant. reservada
                            {
                                if (row[5].ToString().Trim() != "" && para3.Substring(0,10) + "XX" + para3.Substring(10,6) == row[5].ToString())
                                {
                                    if (int.Parse(row[9].ToString()) >= int.Parse(row[6].ToString()))
                                    {
                                        MessageBox.Show("El mueble seleccionado ya esta reservado totalmente", "Atención Revise", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        bt_close_Click(null, null);
                                    }
                                }
                            }
                        }
                        if (sino == "no")
                        {
                            MessageBox.Show("Este contrato NO CONTIENE el mueble seleccionado", "Atención Revise", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            bt_close_Click(null, null);
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
        private void rb_mov_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_mov.Checked == true)
            {
                cmb_dest.Enabled = true;
                tx_evento.Enabled = true;
            }
        }

    }
}
