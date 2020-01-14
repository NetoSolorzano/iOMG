using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace iOMG
{
    public partial class movenmas : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        string para1;       // , para2, para3
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";ConnectionLifeTime=" + ctl + ";";
        
        public movenmas(string parm1,string parm2,string parm3)    // parm1 = modo = reserva o salida
        {                                                       // parm2 = 
            InitializeComponent();                              // parm3 = 
            lb_titulo.Text = parm1.ToUpper(); // modo del movenmasiento
            para1 = parm1;  // modo
            //para2 = parm2;  // almacen de reserva
            //para3 = parm3;
            if (parm1 == "reserva")
            {
                //panel3.Visible = true;
                //panel3.Left = 7;
                //panel3.Top = 30;
                //panel4.Visible = false;
            }
            if (parm1 == "salida")
            {
                //panel4.Visible = true;
                //panel4.Left = 7;
                //panel4.Top = 30;
                //panel3.Visible = false;
                //rb_mov.Checked = true;
            }
            this.KeyPreview = true; // habilitando la posibilidad de pasar el tab con el enter
        }
        private void movenmas_Load(object sender, EventArgs e)
        {
            combos("todos");
            panel3.Enabled = false;
            tx_idped.Enabled = false;
            tx_codm.Enabled = false;
            rb_no.Checked = true;
            rb_mov.Checked = true;

            cmb_aca.DropDownWidth = 150;
            cmb_cap.DropDownWidth = 150;
            cmb_det1.DropDownWidth = 200;
            cmb_det2.DropDownWidth = 200;
            cmb_det3.DropDownWidth = 200;
            cmb_mad.DropDownWidth = 150;
            cmb_mod.DropDownWidth = 150;
            cmb_tal.DropDownWidth = 150;
            cmb_tip.DropDownWidth = 150;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            if (keyData == Keys.F1 && tx_codm.Focused == true)
            {
                para1 = "items";
                para2 = "parcial";
                para3 = "";
                ayuda1 ayu1 = new ayuda1(para1, para2, para3);
                var result = ayu1.ShowDialog();     //ayu1.Show();
                if (result == DialogResult.Cancel)  // deberia ser OK, pero que chuuu
                {
                    tx_codm.Text = ayu1.ReturnValue0;
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void movenmas_KeyDown(object sender, KeyEventArgs e)
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
            if (tx_medidas.Text == "")
            {
                MessageBox.Show("Ingrese las medidas del mueble", "Atención - Corrija", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_medidas.Focus();
                return;
            }
            if (tx_cant.Text != "")
            {
                if (Int16.Parse(tx_cant.Text) <= 0)
                {
                    MessageBox.Show("Ingrese un valor mayor a cero", "Error en cantidad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tx_cant.Text = "0";
                    tx_cant.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor mayor a cero", "Error en cantidad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tx_cant.Text = "0";
                tx_cant.Focus();
                return;
            }
            if (tx_paq.Text != "")
            {
                if (Int16.Parse(tx_paq.Text) <= 0)
                {
                    MessageBox.Show("Ingrese un valor mayor a cero", "Error en paquetes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tx_paq.Text = "0";
                    tx_paq.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor mayor a cero", "Error en paquetes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tx_paq.Text = "0";
                tx_paq.Focus();
                return;
            }
            if (tx_dat_cap.Text == "")
            {
                MessageBox.Show("Seleccione el Capitulo", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_cap.Focus();
                return;
            }
            if (tx_dat_mod.Text == "")
            {
                MessageBox.Show("Seleccione el Modelo", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_mod.Focus();
                return;
            }
            if (tx_dat_mad.Text == "")
            {
                MessageBox.Show("Seleccione la Madera", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_mad.Focus();
                return;
            }
            if (tx_dat_tip.Text == "")
            {
                MessageBox.Show("Seleccione la Tipología", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_tip.Focus();
                return;
            }
            if (tx_dat_det1.Text == "")
            {
                MessageBox.Show("Seleccione el Detalle 1", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_det1.Focus();
                return;
            }
            if (tx_dat_aca.Text == "")
            {
                MessageBox.Show("Seleccione el Acabado", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_aca.Focus();
                return;
            }
            if (tx_dat_tal.Text == "")
            {
                MessageBox.Show("Seleccione el Taller", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_tal.Focus();
                return;
            }
            if (tx_dat_det2.Text == "")
            {
                MessageBox.Show("Seleccione el Detalle 2", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_det2.Focus();
                return;
            }
            if (tx_dat_det3.Text == "")
            {
                MessageBox.Show("Seleccione el Detalle 3", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_det3.Focus();
                return;
            }
            if (tx_dat_jgo.Text == "")
            {
                MessageBox.Show("Seleccione si tiene juego", "Error en Código", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rb_no.Focus();
                return;
            }
            //
            if (tx_nombre.Text == "")
            {
                MessageBox.Show("Ingrese el nombre del mueble", "Nombre para inventario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tx_nombre.Focus();
                return;
            }
            if (tx_medidas.Text == "")
            {
                MessageBox.Show("Ingrese las medidas del mueble", "Faltan las medidas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tx_medidas.Focus();
                return;
            }
            //
            var aa = MessageBox.Show("Confirma que desea grabar la operación?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                if (lb_titulo.Text == "xyZ")
                {
                    {
                        //
                    }
                }
                if (lb_titulo.Text.ToUpper() == "ENTRADA")
                {
                    if (entreda() == true)
                    {
                        retorno = true; // true = se efectuo la operacion
                    }
                }
                this.Close();
            }
        }
        //
        private bool entreda()
        {
            bool bien = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                if (rb_mov.Checked == true || rb_ajuste.Checked == true)
                {
                    string inserta = "";
                    string codi = "";
                    string id_mueble = "";
                    // grabamos en almloc una vez por cada mueble
                    for (int i = 1; i <= Int16.Parse(tx_cant.Text); i++)
                    {   // ingresa por defecto al almacen del usuario, osea argentina
                        codi = tx_dat_cap.Text.Trim() + tx_dat_mod.Text.Trim() + tx_dat_mad.Text.Trim() +
                            tx_dat_tip.Text.Trim() + tx_dat_det1.Text.Trim() + tx_dat_aca.Text.Trim() + 
                            tx_dat_tal.Text.Trim() + tx_dat_det2.Text.Trim() + tx_dat_det3.Text.Trim() + tx_dat_jgo.Text.Trim();
                        inserta = "insert into almloc (" +
                            "codalm,fechop,tipop,codig,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,juego,nombr,medid,soles2018) values (" +
                            "@coda,@fech,@tope,@codi,@capi,@mode,@made,@tipo,@det1,@acab,@tall,@det2,@det3,@jgo,@nomb,@medi,@pre)";
                        MySqlCommand micon = new MySqlCommand(inserta, cn);
                        micon.Parameters.AddWithValue("@coda", iOMG.Program.almuser);
                        micon.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@tope","INGRES");
                        micon.Parameters.AddWithValue("@codi", codi);
                        micon.Parameters.AddWithValue("@capi", tx_dat_cap.Text.Trim());
                        micon.Parameters.AddWithValue("@mode", tx_dat_mod.Text.Trim());
                        micon.Parameters.AddWithValue("@made", tx_dat_mad.Text.Trim());
                        micon.Parameters.AddWithValue("@tipo", tx_dat_tip.Text.Trim());
                        micon.Parameters.AddWithValue("@det1", tx_dat_det1.Text.Trim());
                        micon.Parameters.AddWithValue("@acab", tx_dat_aca.Text.Trim());
                        micon.Parameters.AddWithValue("@tall", tx_dat_tal.Text.Trim());
                        micon.Parameters.AddWithValue("@det2", tx_dat_det2.Text.Trim());
                        micon.Parameters.AddWithValue("@det3", tx_dat_det3.Text.Trim());
                        micon.Parameters.AddWithValue("@jgo", tx_dat_jgo.Text.Trim());
                        micon.Parameters.AddWithValue("@nomb", tx_nombre.Text.Trim());
                        micon.Parameters.AddWithValue("@medi", tx_medidas.Text.Trim());
                        micon.Parameters.AddWithValue("@pre", tx_precio.Text.Trim());
                        micon.ExecuteNonQuery();
                        // id de la operacion
                        inserta = "select last_insert_id()";
                        micon = new MySqlCommand(inserta, cn);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            id_mueble = dr.GetString(0); // ultimo ID ingresado
                        }
                        dr.Close();
                        // kardex
                        inserta = "insert into kardex (codalm,fecha,tipmov,item,cant_i,coment,idalm,USER,dias) " +
                            "values (@coda,@fech,@tope,@codi,@cant,@come,@ida,@asd,now())";
                        micon = new MySqlCommand(inserta, cn);
                        micon.Parameters.AddWithValue("@coda", iOMG.Program.almuser);
                        micon.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@tope", "INGRES");
                        micon.Parameters.AddWithValue("@codi", codi);
                        micon.Parameters.AddWithValue("@cant", "1");
                        micon.Parameters.AddWithValue("@come", tx_comsal.Text.Trim());
                        micon.Parameters.AddWithValue("@ida", id_mueble);
                        micon.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                        micon.ExecuteNonQuery();
                        //
                    }
                    // grabamos en ingresos y actualizamos el saldo del mueble en detaped
                    inserta = "insert into movalm (tipmov,fecha,docum,item,cant,madera,user,dia,almad,cntpaq,coment) " +
                        "values (@tope,@fech,@docu,@codi,@cant,@made,@asd,now(),@coda,@cpaq,@come)";
                    MySqlCommand micin = new MySqlCommand(inserta,cn);
                    micin.Parameters.AddWithValue("@tope","INGRES");
                    micin.Parameters.AddWithValue("@fech", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                    micin.Parameters.AddWithValue("@docu", tx_idped.Text.Trim());
                    micin.Parameters.AddWithValue("@codi", codi);
                    micin.Parameters.AddWithValue("@cant", tx_cant.Text.Trim());
                    micin.Parameters.AddWithValue("@made", tx_dat_mad.Text.Trim());
                    micin.Parameters.AddWithValue("@asd", iOMG.Program.vg_user);
                    micin.Parameters.AddWithValue("@coda", iOMG.Program.almuser);
                    micin.Parameters.AddWithValue("@cpaq", Int16.Parse(tx_paq.Text.Trim()));
                    micin.Parameters.AddWithValue("@come", tx_comsal.Text.Trim());
                    micin.ExecuteNonQuery();
                    if (rb_mov.Checked == true)
                    {
                        string mueble = tx_dat_cap.Text.Trim() + tx_dat_mod.Text.Trim() + tx_dat_mad.Text.Trim() + tx_dat_tip.Text.Trim() +
                            tx_dat_det1.Text.Trim() + tx_dat_aca.Text.Trim() + tx_dat_tal.Text.Trim() + tx_dat_det2.Text.Trim() + tx_dat_det3.Text.Trim();  // +
                            //tx_dat_jgo.Text.Trim();   // el juego no va en el codigo porque los pedidos de fab. se hacen sin juego
                        string actua = "update detaped set saldo=if(@cant>saldo,0,saldo-@cant),fingreso=@fing where pedidoh=@docu and left(item,18)=@codm";
                        micin = new MySqlCommand(actua, cn);
                        micin.Parameters.AddWithValue("@cant", Int16.Parse(tx_cant.Text.Trim()));
                        micin.Parameters.AddWithValue("@fing", dtp_fsal.Value);   // dtp_fsal.Value.ToShortDateString()
                        micin.Parameters.AddWithValue("@docu", tx_idped.Text.Trim());
                        micin.Parameters.AddWithValue("@codm", mueble);
                        micin.ExecuteNonQuery();
                    }
                    // una vez grabado todo .. se debe imprimir las etiquetas una por cada paquete
                    var aa = MessageBox.Show("Impresión de Etiquetas para el artículo" + Environment.NewLine +
                    "Esta listo para la imprimir?", "Rutina de impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        for (int i = (int.Parse(id_mueble) - int.Parse(tx_cant.Text)) + 1; i <= int.Parse(id_mueble); i++ )
                        {   // int i = int.Parse(id_mueble); i <= (int.Parse(id_mueble) - int.Parse(tx_cant.Text)) + 1; i--
                            for (int y = 1; y <= Int16.Parse(tx_paq.Text); y++)
                            {
                                // llama al form impresor con los valores actuales
                                impresor impetiq = new impresor(tx_dat_cap.Text, tx_dat_mod.Text, tx_dat_mad.Text, tx_dat_tip.Text, tx_dat_det1.Text, tx_dat_aca.Text, tx_dat_tal.Text,
                                    tx_dat_det2.Text, tx_dat_det3.Text, tx_dat_jgo.Text, tx_nombre.Text.Trim(), tx_medidas.Text.Trim(), y.ToString(), tx_paq.Text, i);
                                impetiq.Show();
                            }
                        }
                    }
                    bien = true;
                }
                if (rb_mov.Checked == true)
                {
                    // actualizamos el estado del pedido
                    string proced = "estpedalm";
                    MySqlCommand conproc = new MySqlCommand(proced, cn);
                    conproc.CommandType = CommandType.StoredProcedure;
                    conproc.Parameters.AddWithValue("@pedido", tx_idped.Text.Trim());
                    conproc.Parameters.AddWithValue("@enuevo", "");
                    conproc.Parameters["@enuevo"].Direction = ParameterDirection.Output;
                    // conproc.Parameters.AddWithValue("@enuevo", MySqlDbType.String).Direction = ParameterDirection.Output;
                    conproc.ExecuteNonQuery();
                }
                cn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión");
                Application.Exit();
            }
            return bien;
        }
        #region combos y selected index
        private void combos(string quien)
        {
            this.panel4.Focus();
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                if (quien == "todos")
                {
                    // seleccion de capitulo
                    cmb_cap.Items.Clear();
                    tx_dat_cap.Text = "";
                    const string concap = "select descrizionerid,idcodice from desc_gru " +
                        "where numero=1";
                    MySqlCommand cmdcap = new MySqlCommand(concap, cn);
                    DataTable dtcap = new DataTable();
                    MySqlDataAdapter dacap = new MySqlDataAdapter(cmdcap);
                    dacap.Fill(dtcap);
                    foreach (DataRow row in dtcap.Rows)
                    {
                        this.cmb_cap.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());  // citem_cap
                        this.cmb_cap.ValueMember = row.ItemArray[1].ToString(); //citem_cap.Value.ToString();
                    }
                    // seleccion de modelo
                    const string conmod = "select descrizionerid,idcodice from desc_mod " +
                                           "where numero=1 order by idcodice";
                    MySqlCommand cmdmod = new MySqlCommand(conmod, cn);
                    DataTable dtmod = new DataTable();
                    MySqlDataAdapter damod = new MySqlDataAdapter(cmdmod);
                    damod.Fill(dtmod);
                    foreach (DataRow row in dtmod.Rows)
                    {
                        cmb_mod.Items.Add(row.ItemArray[0].ToString());
                        cmb_mod.ValueMember = row.ItemArray[0].ToString();
                    }
                    // seleccion de madera
                    cmb_mad.Items.Clear();
                    tx_dat_mad.Text = "";
                    const string conmad = "select descrizionerid,idcodice from desc_mad " +
                        "where numero=1";
                    MySqlCommand cmdmad = new MySqlCommand(conmad, cn);
                    DataTable dtmad = new DataTable();
                    MySqlDataAdapter damad = new MySqlDataAdapter(cmdmad);
                    damad.Fill(dtmad);
                    foreach (DataRow row in dtmad.Rows)
                    {
                        this.cmb_mad.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());   // citem_mad
                        this.cmb_mad.ValueMember = row.ItemArray[1].ToString(); //citem_mad.Value.ToString();
                    }
                    // seleccion de tipologia
                    cmb_tip.Items.Clear();
                    tx_dat_tip.Text = "";
                    const string contip = "select b.descrizione,a.tipol from items a " +
                        "left join desc_tip b on b.idcodice=a.tipol " +
                        "where a.capit=@des group by a.tipol"; 
                    MySqlCommand cmdtip = new MySqlCommand(contip, cn);
                    cmdtip.Parameters.AddWithValue("@des", tx_dat_cap.Text.Trim());
                    DataTable dttip = new DataTable();
                    MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                    datip.Fill(dttip);
                    foreach (DataRow row in dttip.Rows)
                    {
                        cmb_tip.Items.Add(row.ItemArray[1].ToString());
                        cmb_tip.ValueMember = row.ItemArray[1].ToString();
                    }
                    // seleccion de detalle1
                    this.cmb_det1.Items.Clear();
                    tx_dat_det1.Text = "";
                    //ComboItem citem_dt1 = new ComboItem();
                    const string condt1 = "select descrizionerid,idcodice from desc_dt1 " +
                        "where numero=1";
                    MySqlCommand cmddt1 = new MySqlCommand(condt1, cn);
                    DataTable dtdt1 = new DataTable();
                    MySqlDataAdapter dadt1 = new MySqlDataAdapter(cmddt1);
                    dadt1.Fill(dtdt1);
                    foreach (DataRow row in dtdt1.Rows)
                    {
                        //citem_dt1.Text = row.ItemArray[0].ToString();
                        //citem_dt1.Value = row.ItemArray[1].ToString();
                        this.cmb_det1.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt1
                        this.cmb_det1.ValueMember = row.ItemArray[1].ToString();    // citem_dt1.Value.ToString();
                    }
                    // seleccion de acabado (pulido, lacado, etc)
                    this.cmb_aca.Items.Clear();
                    tx_dat_aca.Text = "";
                    //ComboItem citem_aca = new ComboItem();
                    const string conaca = "select descrizionerid,idcodice from desc_est " +
                        "where numero=1";
                    MySqlCommand cmdaca = new MySqlCommand(conaca, cn);
                    DataTable dtaca = new DataTable();
                    MySqlDataAdapter daaca = new MySqlDataAdapter(cmdaca);
                    daaca.Fill(dtaca);
                    foreach (DataRow row in dtaca.Rows)
                    {
                        //citem_aca.Text = row.ItemArray[0].ToString();
                        //citem_aca.Value = row.ItemArray[1].ToString();
                        this.cmb_aca.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_aca
                        this.cmb_aca.ValueMember = row.ItemArray[1].ToString(); //citem_aca.Value.ToString();
                    }
                    // seleccion de taller
                    this.cmb_tal.Items.Clear();
                    tx_dat_tal.Text = "";
                    //ComboItem citem_tal = new ComboItem();
                    const string contal = "select descrizionerid,trim(codigo) from desc_loc " +
                        "where numero=1";
                    MySqlCommand cmdtal = new MySqlCommand(contal, cn);
                    DataTable dttal = new DataTable();
                    MySqlDataAdapter datal = new MySqlDataAdapter(cmdtal);
                    datal.Fill(dttal);
                    foreach (DataRow row in dttal.Rows)
                    {
                        //citem_tal.Text = row.ItemArray[0].ToString();
                        //citem_tal.Value = row.ItemArray[1].ToString();
                        this.cmb_tal.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_tal
                        this.cmb_tal.ValueMember = row.ItemArray[1].ToString(); // citem_tal.Value.ToString();
                    }
                    // seleccion de detalle 2 (tallado, marqueteado, etc)
                    this.cmb_det2.Items.Clear();
                    tx_dat_det2.Text = "";
                    //ComboItem citem_dt2 = new ComboItem();
                    const string condt2 = "select descrizione,idcodice from desc_dt2 " +
                        "where numero=1";
                    MySqlCommand cmddt2 = new MySqlCommand(condt2, cn);
                    DataTable dtdt2 = new DataTable();
                    MySqlDataAdapter dadt2 = new MySqlDataAdapter(cmddt2);
                    dadt2.Fill(dtdt2);
                    foreach (DataRow row in dtdt2.Rows)
                    {
                        //citem_dt2.Text = row.ItemArray[0].ToString();
                        //citem_dt2.Value = row.ItemArray[1].ToString();
                        this.cmb_det2.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt2
                        this.cmb_det2.ValueMember = row.ItemArray[1].ToString();     //citem_dt2.Value.ToString();
                    }
                    // seleccion de detalle 3
                    cmb_det3.Items.Clear();
                    tx_dat_det3.Text = "";
                    //ComboItem citem_dt3 = new ComboItem();
                    const string condt3 = "select descrizione,idcodice from desc_dt3 where numero=1";
                    MySqlCommand cmddt3 = new MySqlCommand(condt3, cn);
                    DataTable dtdt3 = new DataTable();
                    MySqlDataAdapter dadt3 = new MySqlDataAdapter(cmddt3);
                    dadt3.Fill(dtdt3);
                    foreach (DataRow row in dtdt3.Rows)
                    {
                        //citem_dt3.Text = row.ItemArray[0].ToString();
                        //citem_dt3.Value = row.ItemArray[1].ToString();
                        this.cmb_det3.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt3
                        this.cmb_det3.ValueMember = row.ItemArray[1].ToString();    //citem_dt3.Value.ToString();
                    }
                }
                if (quien == "capit")
                {
                    // seleccion de tipologia
                    cmb_tip.Items.Clear();
                    const string contip = "select b.descrizione,a.tipol from items a " +
                        "left join desc_tip b on b.idcodice=a.tipol " +
                        "where a.capit=@des group by a.tipol";
                    MySqlCommand cmdtip = new MySqlCommand(contip, cn);
                    cmdtip.Parameters.AddWithValue("@des", tx_dat_cap.Text.Trim());
                    DataTable dttip = new DataTable();
                    MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                    datip.Fill(dttip);
                    foreach (DataRow row in dttip.Rows)
                    {
                        cmb_tip.Items.Add(row.ItemArray[1].ToString());
                        cmb_tip.ValueMember = row.ItemArray[1].ToString();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se puede conectar al servidor");
                Application.Exit();
                return;
            }
            cn.Close();
        }

        private void cmb_cap_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_cap.Text = cmb_cap.SelectedItem.ToString().Substring(0,1);
            combos("capit");
        }
        private void cmb_mod_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_mod.Text = cmb_mod.SelectedItem.ToString();
        }
        private void cmb_mad_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_mad.Text = cmb_mad.SelectedItem.ToString().Substring(0, 1);
        }
        private void cmb_tip_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_tip.Text = cmb_tip.SelectedItem.ToString();
        }
        private void cmb_det1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det1.Text = cmb_det1.SelectedItem.ToString().Substring(0, 2);
        }
        private void cmb_aca_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_aca.Text = cmb_aca.SelectedItem.ToString().Substring(0, 1);
        }
        private void cmb_tal_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_tal.Text = cmb_tal.SelectedItem.ToString().Substring(0, 2);
        }
        private void cmb_det2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det2.Text = cmb_det2.SelectedItem.ToString().Substring(0, 3);
        }
        private void cmb_det3_SelectedIndexChanged(object sender, EventArgs e)
        {
            tx_dat_det3.Text = cmb_det3.SelectedItem.ToString().Substring(0, 3);
        }
        #endregion combos
        private void nombre_mueble()
        {
            tx_nombre.Text = cmb_cap.Text.ToString().Trim() + " " + cmb_mod.Text.ToString().Trim() + " " +
                cmb_mad.Text.ToString().Trim() + " " + cmb_tip.Text.ToString().Trim() + " " +
                cmb_det1.Text.ToString().Trim() + " " + cmb_aca.Text.ToString().Trim();
        }
        private void rb_mov_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_mov.Checked == true)
            {
                panel3.Enabled = true;
                tx_idped.Enabled = true;
                tx_codm.Enabled = false;
                //
                lb_idped.Visible = true;
                tx_idped.Visible = true;
                tx_idped.Focus();
            }
        }
        private void rb_ajuste_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ajuste.Checked == true)
            {
                panel3.Enabled = true;
                tx_idped.Enabled = true;
                tx_codm.Enabled = true;
                //
                lb_idped.Visible = false;
                tx_idped.Visible = false;
                //
                lb_codm.Visible = true;
                tx_codm.Visible = true;
                tx_codm.Focus();
                //
                tx_cant.Text = "1";
                tx_paq.Text = "1";
            }
        }

        private void tx_idped_Validating(object sender, CancelEventArgs e)
        {
            if (tx_idped.Text != "")
            {
                MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                cn.Open();
                try
                {
                    string consulta = "select sum(b.saldo) as saldo from pedidos a " +
                        "left join detaped b on b.pedidoh=a.codped " +
                        "where a.tipoes='TPE001' and a.codped=@cp";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cp", tx_idped.Text.Trim());
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.IsDBNull(0))
                        {
                            dr.Close();
                            cn.Close();
                            MessageBox.Show("No existe el código del pedido", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            tx_idped.Text = "";
                            tx_idped.Focus();
                            return;
                        }
                        if (dr.GetInt16(0) == 0)
                        {
                            MessageBox.Show("El código de pedido ingresado NO tiene saldo", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dr.Close();
                            cn.Close();
                            tx_idped.Text = "";
                            tx_idped.Focus();
                            return;
                        }
                        if (dr.GetInt16(0) > 0)
                        {
                            dr.Close();
                            cn.Close();
                            ayuda1 ayu = new ayuda1("detaped", tx_idped.Text, "");
                            //ayu.Show();
                            if (ayu.ShowDialog() == DialogResult.Cancel && !string.IsNullOrEmpty(ayu.ReturnValue0))
                            {
                                //tx_codm.Text = ayu.ReturnValue0;
                                tx_dat_cap.Text = ayu.ReturnValue0.Substring(0, 1);
                                tx_dat_mod.Text = ayu.ReturnValue0.Substring(1, 3);
                                tx_dat_mad.Text = ayu.ReturnValue0.Substring(4, 1);
                                tx_dat_tip.Text = ayu.ReturnValue0.Substring(5, 2);
                                tx_dat_det1.Text = ayu.ReturnValue0.Substring(7, 2);
                                tx_dat_aca.Text = ayu.ReturnValue0.Substring(9, 1);
                                tx_dat_tal.Text = ayu.ReturnValue0.Substring(10, 2);
                                tx_dat_det2.Text = ayu.ReturnValue0.Substring(12, 3);
                                tx_dat_det3.Text = ayu.ReturnValue0.Substring(15, 3);
                                //tx_dat_jgo.Text = ayu.ReturnValue0.Substring(18, 4);
                                tx_nombre.Text = ayu.ReturnValue2;
                                tx_medidas.Text = ayu.ReturnValue3;
                                tx_precio.Text = ayu.ReturnValue4;
                                //
                                cmb_cap.SelectedIndex = cmb_cap.FindString(tx_dat_cap.Text);
                                cmb_mod.SelectedIndex = cmb_mod.FindString(tx_dat_mod.Text);
                                cmb_mad.SelectedIndex = cmb_mad.FindString(tx_dat_mad.Text);
                                cmb_tip.SelectedIndex = cmb_tip.FindString(tx_dat_tip.Text);
                                cmb_det1.SelectedIndex = cmb_det1.FindString(tx_dat_det1.Text);
                                cmb_aca.SelectedIndex = cmb_aca.FindString(tx_dat_aca.Text);
                                cmb_tal.SelectedIndex = cmb_tal.FindString(tx_dat_tal.Text);
                                cmb_det2.SelectedIndex = cmb_det2.FindString(tx_dat_det2.Text);
                                cmb_det3.SelectedIndex = cmb_det3.FindString(tx_dat_det3.Text);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión");
                    Application.Exit();
                    return;
                }
                cn.Close();
            }
        }
        private void tx_codm_Validating(object sender, CancelEventArgs e)   // cod. de referencia desde la maestra
        {
            if (tx_codm.Text != "")
            {
                MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                cn.Open();
                try
                {
                    string consulta = "select it.capit,it.model,it.mader,it.tipol,it.deta1,it.acaba,it.talle,it.deta2,it.deta3,it.juego," +
                        "it.nombr,it.medid,ifnull(a.cnt,-1),ifnull(b.cnt,-1),ifnull(c.cnt,-1),ifnull(d.cnt,-1),ifnull(e.cnt,-1)," +
                        "ifnull(f.cnt,-1),ifnull(g.cnt,-1),ifnull(h.cnt,-1),ifnull(i.cnt,-1),it.soles2018 " +
                        "from items it " +
                        "left join desc_gru a on a.idcodice=it.capit " +
                        "left join desc_mod b on b.idcodice=it.model " +
                        "left join desc_mad c on trim(c.idcodice)=trim(it.mader) " +
                        "left join desc_tip d on d.idcodice=it.tipol " +
                        "left join desc_dt1 e on e.idcodice=it.deta1 " +
                        "left join desc_est f on f.idcodice=it.acaba " +
                        "left join desc_loc g on g.codigo=it.talle " +
                        "left join desc_dt2 h on h.idcodice=it.deta2 " +
                        "left join desc_dt3 i on i.idcodice=it.deta3 " +
                        "where codig=@cm";
                    MySqlCommand micon = new MySqlCommand(consulta, cn);
                    micon.Parameters.AddWithValue("@cm", tx_codm.Text);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        tx_dat_cap.Text = dr.GetString(0);
                        tx_dat_mod.Text = dr.GetString(1);
                        tx_dat_mad.Text = dr.GetString(2);
                        tx_dat_tip.Text = dr.GetString(3);
                        tx_dat_det1.Text = dr.GetString(4);
                        tx_dat_aca.Text = dr.GetString(5);
                        tx_dat_tal.Text = dr.GetString(6);
                        tx_dat_det2.Text = dr.GetString(7);
                        tx_dat_det3.Text = dr.GetString(8);
                        tx_nombre.Text = dr.GetString(10);
                        tx_medidas.Text = dr.GetString(11);
                        tx_precio.Text = dr.GetString(21);
                        //
                        combos("capit");
                        cmb_cap.SelectedIndex = cmb_cap.FindString(tx_dat_cap.Text);    //dr.GetInt16(12);
                        cmb_mod.SelectedIndex = cmb_mod.FindString(tx_dat_mod.Text);
                        cmb_mad.SelectedIndex = cmb_mad.FindString(tx_dat_mad.Text);    // dr.GetInt16(14);
                        cmb_tip.SelectedIndex = cmb_tip.FindString(tx_dat_tip.Text);    // dr.GetInt16(15);
                        cmb_det1.SelectedIndex = cmb_det1.FindString(tx_dat_det1.Text); // dr.GetInt16(16);
                        cmb_aca.SelectedIndex = cmb_aca.FindString(tx_dat_aca.Text);    // dr.GetInt16(17);
                        cmb_tal.SelectedIndex = cmb_tal.FindString(tx_dat_tal.Text);    // dr.GetInt16(18);
                        cmb_det2.SelectedIndex = cmb_det2.FindString(tx_dat_det2.Text); // dr.GetInt16(19);
                        cmb_det3.SelectedIndex = cmb_det3.FindString(tx_dat_det3.Text); // dr.GetInt16(20);
                    }
                    dr.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión");
                    Application.Exit();
                    return;
                }
                //combos();
                cn.Close();
            }
        }

        private void rb_si_CheckedChanged(object sender, EventArgs e)   // usuario dice mueble es parte de un juego
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            try
            {
                cn.Open();  // obtenemos el numero de juego que no existe entre el primer y ultimo registro
                string consulta = "select concat('J',right(concat('00',cast(gap_starts_at as char(3))),3)) from (" +
                    "SELECT distinct (cast(replace(t1.juego,'J',0) as unsigned) + 1) gap_starts_at " +
                    "FROM almloc t1 " +
                    "WHERE NOT EXISTS (SELECT cast(replace(t2.juego,'J',0) as unsigned) " +
	                "FROM almloc t2 WHERE cast(replace(t2.juego,'J',0) as unsigned) = cast(replace(t1.juego,'J',0) as unsigned) + 1) " +
                    "HAVING gap_starts_at IS NOT NULL limit 1)z";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        tx_dat_jgo.Text = dr.GetString(0);
                    }
                }
                dr.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void rb_no_CheckedChanged(object sender, EventArgs e)   // usuario dice mueble NO tiene juego
        {
            tx_dat_jgo.Text = "N000";
        }
    }
}
