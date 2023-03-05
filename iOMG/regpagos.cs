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
        public string para5 = "";
        static string nomform = "regpagos";    // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        //static string nomtab = "pagamenti";       // 
        string timodef = "";                        // codigo moneda por defecto
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

        public regpagos(string param1, string param2, string param3, string param4, string param5)
        {
            para1 = param1;              // pago contrato = PAGCON
            para2 = param2;              // contrato
            para3 = param3;              // saldo del contrato
            para4 = param4;              // imp.total contrato
            para5 = param5;              // version del contrato
            InitializeComponent();
        }

        private void regpagos_Load(object sender, EventArgs e)
        {
            tx_cont.Text = para2;
            jalainfo();
            loadgrids();                // datos del grid
            loadcombos();
            tx_dat_mone.Text = timodef;
            cmb_mone.SelectedIndex = cmb_mone.FindString(tx_dat_mone.Text);
            cmb_mone.Enabled = false;
            tx_serie.MaxLength = 4;
            tx_corre.MaxLength = 8;
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

        private void jalainfo()                                     // obtiene parametros del form
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", nomform);
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["campo"].ToString() == "imagenes" && row["formulario"].ToString() == "main")
                    {
                        // no van botones en este form
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") timodef = row["valor"].ToString().Trim();         // moneda por defecto
                        //if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // 
                        //if (row["campo"].ToString() == "detalle2" && row["param"].ToString() == "piedra") letpied = row["valor"].ToString().Trim();       // 
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

        #region combos
        private void loadcombos()
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            // seleccion de forma de pago
            const string confpago = "select descrizionerid,idcodice from desc_mpa " +
                                   "where numero=1";
            MySqlCommand cmd = new MySqlCommand(confpago, conn);
            MySqlDataAdapter dat = new MySqlDataAdapter(cmd);
            DataTable dtta = new DataTable();
            dat.Fill(dtta);
            foreach (DataRow row in dtta.Rows)
            {
                cmb_fpago.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                cmb_fpago.ValueMember = row.ItemArray[1].ToString();
            }
            // seleccion del tipo de documento
            const string contdv = "select descrizionerid,idcodice from desc_tdv " +
                                   "where numero=1";
            MySqlCommand cmdtdv = new MySqlCommand(contdv, conn);
            DataTable dttdv = new DataTable();
            MySqlDataAdapter datdv = new MySqlDataAdapter(cmdtdv);
            datdv.Fill(dttdv);
            foreach (DataRow row in dttdv.Rows)
            {
                cmb_td.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                cmb_td.ValueMember = row.ItemArray[1].ToString();
            }
            // seleccion de moneda
            const string conmon = "select descrizionerid,idcodice from desc_mon " +
                                   "where numero=1";
            MySqlCommand cmdmon = new MySqlCommand(conmon, conn);
            DataTable dtmon = new DataTable();
            MySqlDataAdapter damon = new MySqlDataAdapter(cmdmon);
            damon.Fill(dtmon);
            foreach (DataRow row in dtmon.Rows)
            {
                cmb_mone.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                cmb_mone.ValueMember = row.ItemArray[1].ToString();
            }
        }
        private void cmb_fpago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_fpago.SelectedIndex == -1) tx_dat_fpago.Text = "";
            else tx_dat_fpago.Text = cmb_fpago.Text.ToString().Substring(0, 6).Trim();   //cmb_fpago.SelectedText.ToString().Substring(0, 6);
        }
        private void cmb_td_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_td.SelectedIndex == -1) tx_dat_td.Text = "";
            else tx_dat_td.Text = cmb_td.Text.ToString().Substring(0, 3).Trim();  //cmb_td.SelectedText.ToString().Substring(0, 6).Trim();
        }
        #endregion

        private bool valRegPago(string NumCon, string Saldo, string NumVer)
        {
            bool retorna = false;
            if (true)   // modo "EDITAR" si, otro modo no debe proceder
            {
                if (NumVer == "2")
                {
                   if (double.Parse(Saldo) > 0)
                    {
                        // si version 2 y no tiene saldo -> si permitimos el registro
                        retorna = true;
                    }
                }
                else
                {
                    // versión anterior al 2, si se permite todo
                    retorna = true;
                }
            }
            return retorna;
        }
        private void loadgrids()
        {
            string consulta = "select idpagamenti,fecha,montosol,via,detalle,dv,serie,numero," +
                "valor,acuenta,saldo,moneda,monto,space(1) as marca " +
                "from pagamenti where contrato=@cont";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                mdaDatos.SelectCommand.Parameters.AddWithValue("@cont", para2);
                mdaDatos.Fill(dtDatos);
                dataGridView1.DataSource = dtDatos;
                dataGridView1.ReadOnly = true;
                //
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].HeaderText = "   FECHA";
                dataGridView1.Columns[1].Width = 110;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[2].HeaderText = "    MONTO";
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
                dataGridView1.Columns[3].HeaderText = "    MEDIO";
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[4].HeaderText = "COMENT";
                dataGridView1.Columns[5].HeaderText = "    DOCVTA";
                dataGridView1.Columns[5].Width = 80;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[6].HeaderText = "  SERIE";
                dataGridView1.Columns[6].Width = 70;
                dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[7].HeaderText = " NUMERO";
                dataGridView1.Columns[7].Width = 110;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;       // marca N=nuevo A=actualizado
            }
            calcula();
            conn.Close();
        }
        private void calcula()
        {
            decimal x = 0;
            for (int i=0; i<dataGridView1.Rows.Count -1; i++)
            {
                x = x + decimal.Parse(dataGridView1.Rows[i].Cells["montosol"].Value.ToString());
            }
            tx_total.Text = x.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            calcula();
            if (valRegPago(para2, para3, para5) == false)    // contrato, saldo, version
            {
                MessageBox.Show("No se permite registrar pago porque no" + Environment.NewLine + 
                    "hay saldo en el contrato o no esta el modo correcto", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (tx_total.Text.Trim() != "")
            {
                var aa = MessageBox.Show("Confirma que desea GRABAR?", "Atención confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string conactin = "";
                        string nsald = "";
                        nsald = (decimal.Parse(para4) - decimal.Parse(tx_total.Text)).ToString();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["marca"].Value != null && row.Cells["marca"].Value.ToString().Trim() != "")
                            {
                                if (row.Cells["marca"].Value.ToString() == "A")
                                {
                                    conactin = "update pagamenti set contrato=@cont,fecha=@fec,montosol=@mont,via=@via,detalle=@com,dv=@tdv,serie=@ser,numero=@corr," +
                                        "valor=@val,acuenta=@act,saldo=@sal,moneda=@sol,monto=@mont,usuario=@asd,dia=now() " +
                                        "where idpagamenti=@idp";
                                }
                                if (row.Cells["marca"].Value.ToString() == "N")
                                {
                                    conactin = "insert into pagamenti (contrato,fecha,montosol,via,detalle,dv,serie,numero,valor,acuenta,saldo,moneda,monto,usuario,dia) " +
                                        "values (@cont,@fec,@mont,@via,@com,@tdv,@ser,@corr,@val,@act,@sal,@sol,@mont,@asd,now())";
                                }
                                if (row.Cells["marca"].Value.ToString().Trim() != "")
                                {
                                    MySqlCommand micon = new MySqlCommand(conactin, conn);
                                    if (row.Cells["marca"].Value.ToString() == "A") micon.Parameters.AddWithValue("@idp", row.Cells["idpagamenti"].Value.ToString());
                                    micon.Parameters.AddWithValue("@cont", para2);
                                    micon.Parameters.AddWithValue("@fec", row.Cells["fecha"].Value.ToString().Substring(6,4) + "-" + 
                                        row.Cells["fecha"].Value.ToString().Substring(3, 2) + "-" +
                                        row.Cells["fecha"].Value.ToString().Substring(0, 2));
                                    micon.Parameters.AddWithValue("@mont", row.Cells["montosol"].Value.ToString());
                                    micon.Parameters.AddWithValue("@via", row.Cells["via"].Value.ToString());
                                    micon.Parameters.AddWithValue("@com", row.Cells["detalle"].Value.ToString());
                                    micon.Parameters.AddWithValue("@tdv", row.Cells["dv"].Value.ToString());
                                    micon.Parameters.AddWithValue("@ser", row.Cells["serie"].Value.ToString());
                                    micon.Parameters.AddWithValue("@corr", row.Cells["numero"].Value.ToString());
                                    micon.Parameters.AddWithValue("@val", para3);            // row.Cells[""].Value.ToString()
                                    micon.Parameters.AddWithValue("@act", tx_total.Text);    // row.Cells[""].Value.ToString()
                                    micon.Parameters.AddWithValue("@sal", nsald);    // row.Cells[""].Value.ToString()
                                    micon.Parameters.AddWithValue("@sol", row.Cells["moneda"].Value.ToString());
                                    micon.Parameters.AddWithValue("@asd", asd);
                                    micon.ExecuteNonQuery();
                                }
                            }
                        }
                        MySqlCommand miupd = new MySqlCommand("update contrat set acuenta=@acta,saldo=@sal where contrato=@cont", conn);
                        miupd.Parameters.AddWithValue("@acta", tx_total.Text);
                        miupd.Parameters.AddWithValue("@sal", nsald);
                        miupd.Parameters.AddWithValue("@cont", para2);
                        miupd.ExecuteNonQuery();
                        //
                        ReturnValue0 = tx_total.Text;       // total pagado
                        ReturnValue1 = nsald;    // debe calcular nuevo saldo
                        ReturnValue2 = "";
                        this.Close();
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("No fue posible conectarse al servidor", "Error de conectividad");
                        Application.Exit();
                        return;
                    }
                    //conn.Close();
                }
                else
                {
                    return;
                }
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // idpagamenti,fecha,montosol,via,detalle,dv,serie,numero
            /*      DESHABILITADO HASTA SABER COMO QUEDAMOS .. SI NO HAGO LA FACT. ELECT
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
            */
        }
        private void bt_det_Click(object sender, EventArgs e)
        {
            if(tx_dat_td.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de documento", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_fpago.Focus();
                return;
            }
            if(tx_serie.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione la serie del documento", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_serie.Focus();
                return;
            }
            if(tx_corre.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el número del documento", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_corre.Focus();
                return;
            }
            if(tx_importe.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el importe pagado", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_importe.Focus();
                return;
            }
            if(tx_dat_fpago.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione la forma de pago", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_fpago.Focus();
                return;
            }
            // validamos que no repitamos el comprobante
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[5].Value.ToString().Trim() == tx_dat_td.Text.Trim() &&
                        row.Cells[6].Value.ToString().Trim() == tx_serie.Text.Trim() &&
                        row.Cells[7].Value.ToString().Trim() == tx_corre.Text.Trim())
                    {
                        MessageBox.Show("Esta repitiendo el comprobante", " Error! ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (tx_idr.Text.Trim() == "")
            {
                // idpagamenti,fecha,montosol,via,detalle,dv,serie,numero,valor,acuenta,saldo,moneda,monto,marca
                DataRow rw = dtDatos.NewRow();
                rw["idpagamenti"] = 0;
                rw["fecha"] = dtp_pago.Value.ToString("dd/MM/yyyy");
                rw["montosol"] = tx_importe.Text;
                rw["via"] = tx_dat_fpago.Text;
                rw["detalle"] = tx_comen.Text.Trim();
                rw["dv"] = tx_dat_td.Text;
                rw["serie"] = tx_serie.Text;
                rw["numero"] = tx_corre.Text;
                rw["valor"] = tx_importe.Text;
                rw["acuenta"] = 0;
                rw["saldo"] = 0;
                rw["moneda"] = tx_dat_mone.Text;
                rw["monto"] = tx_importe.Text;
                rw["marca"] = "N";
                dtDatos.Rows.Add(rw);
                /*
                dataGridView1.Rows.Add(0,
                    dtp_pago.Value.ToString("dd/MM/yyyy"),
                    tx_importe.Text,
                    tx_dat_fpago.Text,
                    tx_comen.Text.Trim(),
                    tx_dat_td.Text,
                    tx_serie.Text,
                    tx_corre.Text,
                    tx_importe.Text,
                    0,
                    0,
                    tx_dat_mone.Text,
                    tx_importe.Text,
                    "N");
                */
            }
            /*
            if (tx_idr.Text.Trim() != "")
            {
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["idpagamenti"].Value.ToString() == tx_idr.Text)
                    {
                        row.Cells["fecha"].Value = dtp_pago.Value;
                        row.Cells["montosol"].Value = tx_importe.Text;
                        row.Cells["monto"].Value = tx_importe.Text;
                        row.Cells["via"].Value = tx_dat_fpago.Text;
                        row.Cells["detalle"].Value = tx_comen.Text.Trim();
                        row.Cells["dv"].Value = tx_dat_td.Text;
                        row.Cells["serie"].Value = tx_serie.Text;
                        row.Cells["numero"].Value = tx_corre.Text;
                        row.Cells["marca"].Value = "A";
                        row.Cells["saldo"].Value = 0;
                        row.Cells["moneda"].Value = tx_dat_mone.Text;
                    }
                }
            }
            */
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
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var aa = MessageBox.Show("Confirma que desea borrar el pago?" + Environment.NewLine +
                dataGridView1.Rows[e.Row.Index].Cells["idpagamenti"].Value.ToString(), "Pago borrado!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                if (e.Row.Index > -1)
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string borrame = "delete from pagamenti where idpagamenti=@idp";
                        MySqlCommand micon = new MySqlCommand(borrame, conn);
                        micon.Parameters.AddWithValue("@idp", dataGridView1.Rows[e.Row.Index].Cells["idpagamenti"].Value.ToString());
                        micon.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("No es posible conectarse al servidor" + Environment.NewLine +
                            "No es posible borrar la fila", "Error de conectividad");
                        e.Cancel = true;
                        return;
                    }
                    conn.Close();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void tx_importe_Enter(object sender, EventArgs e)
        {
            decimal x = 0;
            for (int i=0; i<dataGridView1.Rows.Count -1; i++)
            {
                x = x + decimal.Parse(dataGridView1.Rows[i].Cells["montosol"].Value.ToString());
            }
            tx_total.Text = x.ToString();
            //calcula();
        }
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            calcula();
            tx_importe.Focus();
        }
    }
}
