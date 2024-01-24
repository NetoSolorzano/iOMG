using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using CrystalDecisions.Shared;
using PaperSize = CrystalDecisions.Shared.PaperSize;
using System.Collections.Generic;

namespace iOMG
{
    public partial class contratos : Form
    {
        static string nomform = "contratos";      // nombre del formulario
        string asd = iOMG.Program.vg_user;      // usuario conectado al sistema
        string colback = iOMG.Program.colbac;   // color de fondo
        string colpage = iOMG.Program.colpag;   // color de los pageframes
        string colgrid = iOMG.Program.colgri;   // color de las grillas
        string colstrp = iOMG.Program.colstr;   // color del strip
        bool conSol = iOMG.Program.vg_conSol;   // usa conector solorsoft ?
        static string nomtab = "contrat";
        decimal desCab = 0;                     // descuento global
        #region variables 
        public int totfilgrid, cta, cuenta, pageCount;      // variables para impresion sin crystal, con crystal ya no se usan
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string img_btN = "";
        string img_btE = "";
        string img_btP = "";
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
        string img_bti = "";            // imagen boton inicio
        string img_bts = "";            // imagen boton siguiente
        string img_btr = "";            // imagen boton regresa
        string img_btf = "";            // imagen boton final
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string img_pre = "";            // imagen del boton vista preliminar
        string img_ver = "";            // imagen del boton visualizacion (solo ver)
        string tipede = "";             // tipo de pedido por defecto
        string tiesta = "";             // estado inicial por defecto del contrato
        string tiesan = "";             // estado anulado / codigo
        string escambio = "";           // estados del contrato que admiten modificacion
        string cnojal = "";             // estados de contratos que no se jalan a la grilla
        string canovald2 = "";          // captitulos donde no se valida det2
        string conovald2 = "";          // valor por defecto al no validar det2
        string tdc = "";                // tipo de documento para contratos
        string sdc = "";                // local de contratos (vacio = todos los locales)
        string raz = "";                // razon social del contrato (vacio si es un solo contador para todos)
        string letpied = "";            // letra identificadora de Piedra en detalle 2 = R
        int vfdmax = 0;                 // limite de filas de detalle maximo por contrato
        string tncont = "";             // tipo de numeracion: AUTOMATICA o MANUAL
        string letgru = "";                 // letra identificado en campo "CAPIT" para ADICIONAL, Z=adicionales, _=artículos chicos
        string talldef = "";                // taller por defecto en los contratos
        string madedef = "";                // maderas para adicionales
        string dets1 = "";                  // detalles1 para adicionales
        string dets2 = "";                  // detalles2 para adicionales
        string dets3 = "";                  // detalles3 para adicionales
        string acadef = "";                 // acabados para adicionales
        string vpaisdef = "";               // pais por defecto para los clientes y proveedores
        string docDni = "";                 // codigo documento dni
        string docRuc = "";                 // codigo documento RUC
        string cliente = Program.cliente;   // razon social para los reportes
        string impDef = "";                 // nombre de la impresora por defecto
        string docBol = "";                 // codigo boletas
        string docFac = "";                 // codigo facturas
        int intfec = 1;                     // intervalo de días para busqueda de comprobantes sin contrato
        string vtasc = "";                  // tipo de articulo (capitulo) que no se hace contrato
        string vapm = "";                   // sedes donde el pago se puede registrar manualmente                22/10/2022
        string vupb = "";                   // usuarios que pueden quitar items virgenes de contratos sin importar su estado
        string v_monLoc = "";               // codigo moneda local
        string limdet = "";                 // monto limite para el pago de detracciones en servicios
        string vucecs = "";                 // usuarios que pueden cambiar el estado de contratos de servicios
        string venora = "";                 // estados de contratos que el sistema NO DEBE re actualizar 21/01/2024
        internal List<string> _comprobantes = new List<string>();        // comprobantes del contrato
        #endregion

        libreria lib = new libreria();
        acciones acc = new acciones();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        //string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";default command timeout=120";
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + " " + ";default command timeout=120" +
        ";ConnectionLifeTime=" + ctl + ";";

        DataTable dtg = new DataTable();
        DataTable dtadpd = new DataTable();     // tabla para el autocompletado de dpto, provin y distrito
        DataTable dttaller = new DataTable();   // combo taller de fabric.
        DataTable dtdest = new DataTable();     // tipos de documentos de clientes
        AutoCompleteStringCollection adptos = new AutoCompleteStringCollection();
        AutoCompleteStringCollection aprovi = new AutoCompleteStringCollection();
        AutoCompleteStringCollection adistr = new AutoCompleteStringCollection();

        public contratos()
        {
            InitializeComponent();
        }
        private void users_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            string para5 = "";
            if (keyData == Keys.F1) //  && Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"
            {
                if (cmb_fam.Focused == true || cmb_mod.Focused == true || cmb_mad.Focused == true || cmb_tip.Focused == true ||
                    cmb_det1.Focused == true || cmb_aca.Focused == true || cmb_tal.Focused == true ||
                    cmb_det2.Focused == true || cmb_det3.Focused == true)
                {
                    para1 = "items";
                    para2 = "todos";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            cmb_fam.SelectedIndex = cmb_fam.FindString(ayu2.ReturnValue1.Substring(0, 1));
                            cmb_mod.SelectedIndex = cmb_mod.FindString(ayu2.ReturnValue1.Substring(1, 3));
                            cmb_mad.SelectedIndex = cmb_mad.FindString(ayu2.ReturnValue1.Substring(4, 1));
                            cmb_mad_SelectionChangeCommitted(null, null);
                            cmb_tip.SelectedIndex = cmb_tip.FindString(ayu2.ReturnValue1.Substring(5, 2));
                            cmb_det1.SelectedIndex = cmb_det1.FindString(ayu2.ReturnValue1.Substring(7, 2));
                            cmb_det1_SelectionChangeCommitted(null, null);
                            cmb_aca.SelectedIndex = cmb_aca.FindString(ayu2.ReturnValue1.Substring(9, 1));
                            cmb_aca_SelectionChangeCommitted(null, null);
                            //if (tx_dat_orig.Text == "") cmb_tal.SelectedIndex = cmb_tal.FindString(ayu2.ReturnValue1.Substring(10, 2));
                            cmb_tal.SelectedIndex = cmb_tal.FindString(ayu2.ReturnValue1.Substring(10, 2));
                            cmb_det2.SelectedIndex = cmb_det2.FindString(ayu2.ReturnValue1.Substring(12, 3));
                            cmb_det2_SelectionChangeCommitted(null, null);
                            cmb_det3.SelectedIndex = cmb_det3.FindString(ayu2.ReturnValue1.Substring(15, 3));
                            tx_d_codal1.Text = ayu2.ReturnValueA[0];    // x ver
                            armani();
                        }
                    }
                }
                if (tx_a_codig.Focused == true)     // adicionales
                {
                    para1 = "items_adic";
                    para2 = "todos";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            // ayu2.ReturnValue0;
                            tx_a_codig.Text = ayu2.ReturnValue1;
                            tx_a_nombre.Text = ayu2.ReturnValue2;
                            tx_a_pd.Text = ayu2.ReturnValueA[4];
                        }
                    }
                }
                if (tx_acta.Focused == true && tx_codped.Text.Trim() != "" && 
                    "NUEVO,EDITAR".Contains(Tx_modo.Text)) 
                {
                    //  && vapm.Contains(tx_dat_orig.Text)
                    // tx_verCont.Text.Trim() == "" <-- esto se agrego el 14/02/23 porque solo los cont anteriores al 2.0 se ingresan los pagos manualmente
                    // "NUEVO,EDITAR".Contains(Tx_modo.Text) && tx_verCont.Text.Trim() == "") cambiado 02/03/23 para validar en regpagos
                    para1 = "PAGCON";
                    para2 = tx_codped.Text.Trim();
                    para3 = tx_saldo.Text.Trim();
                    para4 = tx_valor.Text.Trim();
                    para5 = tx_verCont.Text.Trim();
                    regpagos pagos = new regpagos(para1, para2, para3, para4, para5);
                    DialogResult result = pagos.ShowDialog();
                    if(result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(pagos.ReturnValue0))
                        {
                            tx_acta.Text = pagos.ReturnValue0;  // nuevo a cuenta
                            tx_saldo.Text = pagos.ReturnValue1; // nuevo saldo
                            // actualizamos la grilla
                            if(Tx_modo.Text == "EDITAR")
                            {
                                Int16 fdt = Int16.Parse(tx_rind.Text.ToString());
                                DataRow row = dtg.Rows[fdt];
                                dtg.Rows[fdt][12] = tx_acta.Text;
                                dtg.Rows[fdt][13] = tx_saldo.Text;
                            }
                        }
                    }
                }
                if (tx_corre.Focused == true || tx_serie.Focused == true || tx_mc.Focused == true)
                {
                    if (tx_dat_orig.Text == "")
                    {
                        MessageBox.Show("Seleccione un lugar de ventas!","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else
                    {
                        // aca va la llamada a la ventana donde seleccionamos los comprobantes
                        forselcomp pagos = new forselcomp(tx_dat_orig.Text, tiesan, intfec);
                        var resu = pagos.ShowDialog();
                        if (resu == DialogResult.Cancel)
                        {
                            if (!string.IsNullOrEmpty(pagos.ReturnValue1))
                            {
                                int i = 1;
                                tx_acta.Text = "";
                                foreach (DataRow row in pagos.ReturnValueT.Rows)
                                {
                                    jalaDatFact("T", row[1].ToString().Substring(0, 1), row[2].ToString(), row[3].ToString(), i.ToString());
                                    i = i + 1;
                                    _comprobantes.Add(row[1].ToString().Substring(0, 1) + "-" + row[2].ToString() + "-" + row[3].ToString());
                                    calculos();     // recien lo pongo, verificar cuando jalamos bienes
                                    tx_d_prec.ReadOnly = true;      // 16/01/2024 no se debe cambiar precio si viene de un comprobante
                                    tx_d_total.ReadOnly = true;     // 16/01/2024
                                    tx_vDc.Text = "S";              // 17/01/2024
                                }
                            }
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void contratos_Load(object sender, EventArgs e)
        {
            init();
            toolboton();
            limpiar(this);
            sololee(this);
            dataload("maestra");
            dataload("todos");
            grilla();
            this.KeyPreview = true;
            Bt_add.Enabled = true;
            Bt_anul.Enabled = true;     // borra si no tiene enlaces, anula si ya tiene relacionados
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            tabControl1.Enabled = false;
            //cmb_tipo.Enabled = false;
            tx_d_nom.Enabled = false;
        }

        #region Fact_Elec
        internal void jalaDatFact(string modo,string FB, string serF, string corF, string kk)   // modo: T=jala todos, C=jala cliente, D=jala detalle
        {
            string excluye = "";
            decimal valexc = 0;
            /* 16/02/2023, no se debe exluir nada porque sino los totales no cuadran cuando hay anticipos y cancelaciones
            var xx = MessageBox.Show("Excluye los artículos del capítulo: " + vtasc + Environment.NewLine +
                "en el detalle del contrato?","Confirme por favor", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (xx == DialogResult.Yes) excluye = " and left(a.codprod,1) <> '" + vtasc + "'";
            */
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string idc = "";
                    if (modo == "T")    //  || modo == "T"
                    {
                        string accion = "select a.id,b.idanagrafica,a.totdvMN,a.tidoclt,a.nudoclt,a.nombclt,a.direclt,a.dptoclt,a.provclt,a.distclt,a.corrclt,a.teleclt,a.telemsg," +
                            "a.conPago,a.pagauto,a.valordscto,a.tipocomp " +
                            "from cabfactu a LEFT JOIN anag_cli b ON b.tipdoc=a.tidoclt AND b.RUC=a.nudoclt where a.martdve=@mar and a.serdvta=@ser and a.numdvta=@num";
                        using (MySqlCommand micon = new MySqlCommand(accion, conn))
                        {
                            micon.Parameters.AddWithValue("@mar", FB);
                            micon.Parameters.AddWithValue("@ser", serF);
                            micon.Parameters.AddWithValue("@num", corF);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    idc = dr.GetString("id");
                                    tx_idcli.Text = dr.GetString("idanagrafica");      // id del cliente
                                                                                       // jala datos del cliente
                                    tx_ndc.Text = dr.GetString("nudoclt");
                                    tx_nombre.Text = dr.GetString("nombclt");
                                    tx_direc.Text = dr.GetString("direclt");
                                    tx_dpto.Text = dr.GetString("dptoclt");
                                    tx_prov.Text = dr.GetString("provclt");
                                    tx_dist.Text = dr.GetString("distclt");
                                    tx_mail.Text = dr.GetString("corrclt");
                                    tx_telef1.Text = dr.GetString("teleclt");
                                    tx_telef2.Text = dr.GetString("telemsg");
                                    //
                                    tx_dat_tdoc.Text = dr.GetString("tidoclt");
                                    foreach (DataRow row in dtdest.Rows)
                                    {
                                        if (row["idcodice"].ToString() == tx_dat_tdoc.Text)
                                        {
                                            cmb_tdoc.SelectedItem = row["descrizionerid"].ToString();
                                        }
                                    }
                                    double ff = 0;
                                    double.TryParse(tx_acta.Text, out ff);
                                    //if (dr.GetString("conPago") == "1") tx_acta.Text = (ff + dr.GetDouble("totdvMN")).ToString("#0.00");     // pago a cuenta
                                    //else tx_acta.Text = ff.ToString("#0.00");
                                    tx_acta.Text = (ff + dr.GetDouble("totdvMN")).ToString("#0.00");     // 14/02/2023, todos los comprobantes se asumen el valor como contado
                                    tx_desCab.Text = dr.GetString("valordscto");
                                    desCab = decimal.Parse(tx_desCab.Text);
                                    //
                                    if ((rb_bienes.Checked == true && dr.GetString("tipocomp") != "B") || (rb_servi.Checked == true && dr.GetString("tipocomp") != "S"))
                                    {
                                        MessageBox.Show("El comprobante seleccionado es de un tipo distinto al contrato", "Tipo de contrato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    if (modo == "T")    //  || modo == "T"
                    {
                        if (excluye != "")
                        {
                            string conexc = "select sum(totalMN),pagauto from detfactu where idc=@idc and codprod<>'' and left(codprod,1) = '" + vtasc + "'";
                            using (MySqlCommand conexcl = new MySqlCommand(conexc, conn))
                            {
                                conexcl.Parameters.AddWithValue("@idc", idc);
                                using (MySqlDataReader dr = conexcl.ExecuteReader())
                                {
                                    if (dr.Read())
                                    {
                                        if (dr.HasRows == true && dr[0] != DBNull.Value)
                                        {
                                            if (dr.GetString("pagauto") == "S") valexc = dr.GetDecimal(0);
                                            else valexc = 0;
                                        }
                                    }
                                }
                            }
                        }
                        decimal ff = 0;
                        decimal.TryParse(tx_acta.Text, out ff);

                        decimal nueActa = ff - valexc;
                        tx_acta.Text = nueActa.ToString("#0.00");     // pago a cuenta - menos valor excluido
                        /*
                        string jadet = "select 0 as 'iddetacon',a.codprod,a.cantbul,a.descpro,a.medidas,a.madera,a.precio,a.totalMN,0 as 'saldo',space(1) AS 'pedido'," +
                            "space(1) as 'codref',space(1) as 'coment',a.detpied,space(1) as 'codpie',space(1) as na,space(1) as 'tda_item'," +
                            "ifnull(if(i.soles2018*a.cantbul=0,a.precio*a.cantbul,i.soles2018*a.cantbul),a.precio*a.cantbul) as totCat,a.cantbul*a.dscto,a.dscto " +
                            "from detfactu a LEFT JOIN items i ON i.codig=a.codprod where a.idc=@idc and a.codprod<>''" + excluye;
                        */
                        string jadet = "select 0 as 'iddetacon',a.codprod,a.cantbul,a.descpro,a.medidas,a.madera,a.precio,a.totalMN,0 as 'saldo',space(1) AS 'pedido'," +
                            "space(1) as 'codref',space(1) as 'coment',a.detpied,space(1) as 'codpie',space(1) as na,space(1) as 'tda_item'," +
                            "ifnull(if(i.soles2018*a.cantbul=0,a.totSinDscto,i.soles2018*a.cantbul),a.precio*a.cantbul) as totCat,a.dscto,if(a.codprod<>'',left(a.codprod,15),'') as alterno1 " + //a.cantbul*a.dscto,a.dscto
                            "from detfactu a LEFT JOIN items i ON i.codig=a.codprod where a.idc=@idc and a.codprod<>''" + excluye;
                        // 23/02/2023
                        using (MySqlCommand midet = new MySqlCommand(jadet, conn))
                        {
                            midet.Parameters.AddWithValue("@idc", idc); //
                            MySqlDataAdapter da = new MySqlDataAdapter(midet);
                            if (kk == "1")  // dataGridView1.Rows.Count <= 2
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                dataGridView1.DataSource = null;
                                dataGridView1.Rows.Clear();
                                dataGridView1.Columns.Clear();
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                // dataAdapter.Update((DataTable)bindingSource1.DataSource);
                                //DataRow dtr = dt.NewRow();
                                DataTable dd = new DataTable();
                                da.Fill(dd);
                                foreach (DataRow rw in dd.Rows)
                                {
                                    DataRow dtr = ((DataTable)dataGridView1.DataSource).NewRow();
                                    dtr[0] = rw.ItemArray[0].ToString();
                                    dtr[1] = rw.ItemArray[1].ToString();
                                    dtr[2] = rw.ItemArray[2].ToString();
                                    dtr[3] = rw.ItemArray[3].ToString();
                                    dtr[4] = rw.ItemArray[4].ToString();
                                    dtr[5] = rw.ItemArray[5].ToString();
                                    dtr[6] = rw.ItemArray[6].ToString();
                                    dtr[7] = rw.ItemArray[7].ToString();
                                    dtr[8] = rw.ItemArray[8].ToString();
                                    dtr[9] = rw.ItemArray[9].ToString();
                                    dtr[10] = rw.ItemArray[10].ToString();
                                    dtr[11] = rw.ItemArray[11].ToString();
                                    dtr[12] = rw.ItemArray[12].ToString();
                                    dtr[13] = rw.ItemArray[13].ToString();
                                    dtr[14] = rw.ItemArray[14].ToString();
                                    dtr[15] = rw.ItemArray[15].ToString();
                                    dtr[16] = rw.ItemArray[16].ToString();
                                    //dtr[17] = rw.ItemArray[17].ToString();
                                    try
                                    {
                                        ((DataTable)dataGridView1.DataSource).Rows.Add(dtr);
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                }
                            }
                            grilladet("edita");     // obtiene contenido de grilla con DT
                            //dt.Dispose();
                            da.Dispose();
                        }
                    }
                    conn.Close();
                    //
                    int v_ifm = 0;
                    decimal val = 0, dscto = 0;

                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[7].Value != null) val = val + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[16].Value != null && dataGridView1.Rows[i].Cells[16].Value.ToString().Trim() != "")
                        {
                            //decimal xxx = decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString()) / decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            decimal xxx = decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString());
                            dscto = dscto + xxx;
                        }
                        // buscamos que la madera este seleccionada
                        if (dataGridView1.Rows[i].Cells[5].Value != null)
                        {
                            if (dataGridView1.Rows[i].Cells[5].Value.ToString().Trim() == "" &&
                                dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 1) != "_") v_ifm += 1;
                        }
                    }
                    tx_dscto.Text = (dscto - val).ToString("#0.00");
                    tx_coment.Focus();
                    //
                }
                else
                {
                    MessageBox.Show("No se pudo abrir conexión con el servidor", "Error de comunicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        #endregion

        #region resto del mundo
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            this.toolStrip1.BackColor = Color.FromName(colstrp);
            this.advancedDataGridView1.BackgroundColor = Color.FromName(iOMG.Program.colgri);
            this.tabuser.BackColor = Color.FromName(iOMG.Program.colgri);

            jalainfo();
            //autodptos();                              // porque solo dptos y el resto?
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_anul);
            bt_view.Image = Image.FromFile(img_ver);
            Bt_print.Image = Image.FromFile(img_btP);
            bt_prev.Image = Image.FromFile(img_pre);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
            //
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // longitudes maximas de campos
            tx_ndc.MaxLength = 12;
            tx_nombre.MaxLength = 100;
            tx_direc.MaxLength = 60;
            tx_dpto.MaxLength = 45;
            tx_prov.MaxLength = 20;
            tx_dist.MaxLength = 20;
            tx_mail.MaxLength = 50;
            tx_telef1.MaxLength = 15;
            tx_telef2.MaxLength = 15;
            tx_coment.MaxLength = 240;           // nombre
            tx_d_com.MaxLength = 80;
            tx_dirent.MaxLength = 90;
            tx_codped.CharacterCasing = CharacterCasing.Upper;
            tx_piso.MaxLength = 2;
            tx_dirRef.MaxLength = 90;           // referencia de la dirección
            tx_contac.MaxLength = 90;           // persona de contacto
            tx_telcont.MaxLength = 25;          // telefono de contacto
            tx_motivD.MaxLength = 100;
            tx_d_nom.MaxLength = 100;
            tx_a_nombre.MaxLength = 100;
            tx_d_med.MaxLength = 20;
            tx_a_medid.MaxLength = 20;
            tx_a_comen.MaxLength = 80;
            tx_d_tda.MaxLength = 6;
        }
        private void jalainfo()                                                 // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped,@adi,@cli,@dvt)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "contratos");
                micon.Parameters.AddWithValue("@adi", "adicionals");
                micon.Parameters.AddWithValue("@cli", "clients");
                micon.Parameters.AddWithValue("@dvt", "docsvta");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "main")
                    {
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_pre") img_pre = row["valor"].ToString().Trim();         // imagen del boton vista preliminar
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_ver") img_ver = row["valor"].ToString().Trim();         // imagen del boton visualización
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["campo"].ToString() == "imagenes" && row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                        if (row["campo"].ToString() == "pais" && row["param"].ToString() == "default") vpaisdef = row["valor"].ToString().Trim();             // pais por defecto para los clientes
                    }
                    if (row["formulario"].ToString() == "contratos")
                    {
                        if (row["campo"].ToString() == "tipocon" && row["param"].ToString() == "normal") tipede = row["valor"].ToString().Trim();               // tipo de contrato x defecto "normal"
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();               // estado del contrato inicial
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "cambio") escambio = row["valor"].ToString().Trim();              // estado del contrato que admiten modificar el pedido
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "nodet2") canovald2 = row["valor"].ToString().Trim();            // captitulos donde no se valida det2
                        if (row["campo"].ToString() == "validac" && row["param"].ToString() == "valdet2") conovald2 = row["valor"].ToString().Trim();           // valor por defecto al no validar det2
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "tipdoc") tdc = row["valor"].ToString().Trim();                 // tipo de documento para contratos
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "local") sdc = row["valor"].ToString().Trim();                  // local del contrato, vacio todos los locales
                        if (row["campo"].ToString() == "contrato" && row["param"].ToString() == "rsocial") raz = row["valor"].ToString().Trim();                // tipo de documento para contratos
                        if (row["campo"].ToString() == "detalle2" && row["param"].ToString() == "piedra") letpied = row["valor"].ToString().Trim();             // letra identificadora de Piedra en Detalle2
                        if (row["campo"].ToString() == "grilladet" && row["param"].ToString() == "limite") vfdmax = int.Parse(row["valor"].ToString().Trim());  // cantidad de filas de detalle maximo del cont estandar
                        if (row["campo"].ToString() == "numeracion" && row["param"].ToString() == "modo") tncont = row["valor"].ToString().Trim();              // tipo de numeracion de los contratos: MANUAL o AUTOMA 
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "codAnu") tiesan = row["valor"].ToString().Trim();                // codigo de estado anulado
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "nogrilla") cnojal = row["valor"].ToString().Trim();              // estados de contratos que no se jalan a la grilla
                        if (row["campo"].ToString() == "impresora" && row["param"].ToString() == "default") impDef = row["valor"].ToString().Trim();            // nombre de la impresora por defecto
                        if (row["campo"].ToString() == "tipoart" && row["param"].ToString() == "sinContrat") vtasc = row["valor"].ToString().Trim();            // capitulo de articulos chicos
                        if (row["campo"].ToString() == "sedespag" && row["param"].ToString() == "manual") vapm = row["valor"].ToString().Trim();                // sedes donde se acepta registro de pagos manuales 
                        if (row["campo"].ToString() == "permisos_u" && row["param"].ToString() == "borra_i") vupb = row["valor"].ToString().Trim();             // usuarios que pueden quitar items virghenes de contratos
                        if (row["campo"].ToString() == "permisos_u" && row["param"].ToString() == "cambia_e") vucecs = row["valor"].ToString().Trim();          // usuarios que pueden cambiar el estado de contrato de servicios
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "noreact") venora = row["valor"].ToString().Trim();               // estados de contratos que el sistema NO DEBE re actualizar 21/01/2024
                    }
                    if (row["formulario"].ToString() == "adicionals")
                    {
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "capitulo") letgru = row["valor"].ToString().Trim();    // capitulo
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "talleres") talldef = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "maderas") madedef = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle1") dets1 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle2") dets2 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "detalle3") dets3 = row["valor"].ToString().Trim();    // 
                        if (row["campo"].ToString() == "identificador" && row["param"].ToString() == "acabados") acadef = row["valor"].ToString().Trim();    // 
                    }
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "dni") docDni = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ruc") docRuc = row["valor"].ToString().Trim();
                    }
                    if (row["formulario"].ToString() == "docsvta")
                    {
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "boleta") docBol = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "factura") docFac = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "intervalo" && row["param"].ToString() == "diasFec") intfec = int.Parse(row["valor"].ToString());
                        if (row["campo"].ToString() == "detrac" && row["param"].ToString() == "serv_limit") limdet = row["valor"].ToString().Trim();
                    }
                }
                da.Dispose();
                dt.Dispose();
                // autocompletados de departamento, provincia y distrito
                consulta = "SELECT depart,provin,distri,nombre FROM ubigeos";
                micon = new MySqlCommand(consulta, conn);
                try
                {
                    MySqlDataAdapter daa = new MySqlDataAdapter(micon);
                    daa.Fill(dtadpd);
                    daa.Dispose();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener nombres de dptos,provin y distritos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();

                return;
            }
        }
        private void dataload(string quien)                                     // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            tabControl1.SelectedTab = tabgrilla;
            if (quien == "maestra")
            {
                // datos de los contratos date_format(date(a.fecha),'%Y-%m-%d')
                string datgri = "select a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,date_format(date(a.fecha),'%Y-%m-%d') as fecha,a.cliente,ifnull(b.razonsocial,'') as razonsocial,a.coment," +
                    "date_format(date(a.entrega),'%Y-%m-%d') as entrega,a.dentrega,a.valor,a.acuenta,a.saldo,a.dscto,a.clte_recoje,a.seresma,a.pisoent,a.ascensor," +
                    "a.pcontacto,a.dreferen,a.telcont,a.totsad,a.motivDes,a.articulo,a.marca0,dsctocab " +
                    "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                    "where not find_in_set(a.status,@tea)";   // where a.status not in (@tea)
                MySqlCommand cdg = new MySqlCommand(datgri, conn);
                //cdg.Parameters.AddWithValue("@tip", tipede);                // "TPE001"
                cdg.Parameters.AddWithValue("@tea", cnojal);          // estados de contratos que no se jalan a la grilla
                MySqlDataAdapter dag = new MySqlDataAdapter(cdg);
                dtg.Clear();
                dag.Fill(dtg);
                dag.Dispose();
            }
            //  datos para el combobox de tipo de documento
            if (quien == "todos")
            {
                // seleccion de tipo de contrato
                const string conpedido = "select descrizionerid,idcodice from desc_tco " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdpedido = new MySqlCommand(conpedido, conn);
                DataTable dtpedido = new DataTable();
                MySqlDataAdapter dapedido = new MySqlDataAdapter(cmdpedido);
                dapedido.Fill(dtpedido);
                foreach (DataRow row in dtpedido.Rows)
                {
                    cmb_tipo.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_tipo.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del local de ventas 
                const string contaller = "select descrizionerid,idcodice from desc_alm " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdtaller = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmdtaller);
                dataller.Fill(dttaller);
                foreach (DataRow row in dttaller.Rows)
                {
                    cmb_taller.Items.Add(row.ItemArray[1].ToString().PadRight(6).Substring(0, 6) + " - " + row.ItemArray[0].ToString());
                    cmb_taller.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de estado del contrato
                cmb_estado.Items.Clear();
                const string conestado = "select descrizionerid,idcodice from desc_sta " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdestado = new MySqlCommand(conestado, conn);
                DataTable dtestado = new DataTable();
                MySqlDataAdapter daestado = new MySqlDataAdapter(cmdestado);
                daestado.Fill(dtestado);
                foreach (DataRow row in dtestado.Rows)
                {
                    cmb_estado.Items.Add(row.ItemArray[1].ToString() + " - " + row.ItemArray[0].ToString());
                    cmb_estado.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion del tipo documento cliente
                cmb_tdoc.Items.Clear();
                const string condest = "select descrizionerid,idcodice,codigo from desc_doc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmddest = new MySqlCommand(condest, conn);
                MySqlDataAdapter dadest = new MySqlDataAdapter(cmddest);
                dadest.Fill(dtdest);
                foreach (DataRow row in dtdest.Rows)
                {
                    cmb_tdoc.Items.Add(row.ItemArray[0].ToString());    //  + " - " + row.ItemArray[1].ToString()
                    cmb_tdoc.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de familia de art
                cmb_fam.Items.Clear();
                //tx_dat_fam.Text = "";
                cmb_fam.Tag = "";
                const string concap = "select descrizionerid,idcodice from desc_gru " +
                    "where numero=1";
                MySqlCommand cmdcap = new MySqlCommand(concap, conn);
                DataTable dtcap = new DataTable();
                MySqlDataAdapter dacap = new MySqlDataAdapter(cmdcap);
                dacap.Fill(dtcap);
                foreach (DataRow row in dtcap.Rows)
                {
                    cmb_fam.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());
                    cmb_fam.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de modelo
                const string conmod = "select descrizionerid,idcodice from desc_mod " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmdmod = new MySqlCommand(conmod, conn);
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
                //tx_dat_mad.Text = "";
                cmb_mad.Tag = "";
                const string conmad = "select descrizionerid,idcodice from desc_mad " +
                    "where numero=1";
                MySqlCommand cmdmad = new MySqlCommand(conmad, conn);
                DataTable dtmad = new DataTable();
                MySqlDataAdapter damad = new MySqlDataAdapter(cmdmad);
                damad.Fill(dtmad);
                foreach (DataRow row in dtmad.Rows)
                {
                    this.cmb_mad.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());   // citem_mad
                    this.cmb_mad.ValueMember = row.ItemArray[1].ToString(); //citem_mad.Value.ToString();
                }
                // seleccion del tipo de mueble
                cmb_tip.Items.Clear();
                cmb_tip.Tag = "";
                const string contip = "select descrizionerid,idcodice from desc_tip " +
                    "where numero=1 order by descrizionerid,idcodice";
                MySqlCommand cmdtip = new MySqlCommand(contip, conn);
                DataTable dttip = new DataTable();
                MySqlDataAdapter datip = new MySqlDataAdapter(cmdtip);
                datip.Fill(dttip);
                foreach (DataRow row in dttip.Rows)
                {
                    this.cmb_tip.Items.Add(row.ItemArray[1].ToString().Trim() + "  -  " + row.ItemArray[0].ToString());
                    this.cmb_tip.ValueMember = row.ItemArray[1].ToString();
                }
                // seleccion de detalle1
                cmb_det1.Items.Clear();
                //tx_dat_det1.Text = "";
                cmb_det1.Tag = "";
                const string condt1 = "select descrizionerid,idcodice from desc_dt1 " +
                    "where numero=1";
                MySqlCommand cmddt1 = new MySqlCommand(condt1, conn);
                DataTable dtdt1 = new DataTable();
                MySqlDataAdapter dadt1 = new MySqlDataAdapter(cmddt1);
                dadt1.Fill(dtdt1);
                foreach (DataRow row in dtdt1.Rows)
                {
                    this.cmb_det1.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt1
                    this.cmb_det1.ValueMember = row.ItemArray[1].ToString();    // citem_dt1.Value.ToString();
                }
                // seleccion de acabado (pulido, lacado, etc)
                cmb_aca.Items.Clear();
                //tx_dat_aca.Text = "";
                cmb_aca.Tag = "";
                const string conaca = "select descrizionerid,idcodice from desc_est " +
                    "where numero=1";
                MySqlCommand cmdaca = new MySqlCommand(conaca, conn);
                DataTable dtaca = new DataTable();
                MySqlDataAdapter daaca = new MySqlDataAdapter(cmdaca);
                daaca.Fill(dtaca);
                foreach (DataRow row in dtaca.Rows)
                {
                    cmb_aca.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_aca
                    cmb_aca.ValueMember = row.ItemArray[1].ToString(); //citem_aca.Value.ToString();
                }
                // seleccion de taller
                cmb_tal.Items.Clear();
                //tx_dat_tal.Text = "";
                cmb_tal.Tag = "";
                const string contal = "select descrizionerid,codigo from desc_loc " +
                    "where idcodice='XX'";
                MySqlCommand cmdtal = new MySqlCommand(contal, conn);
                DataTable dttal = new DataTable();
                MySqlDataAdapter datal = new MySqlDataAdapter(cmdtal);
                datal.Fill(dttal);
                foreach (DataRow row in dttal.Rows)
                {
                    cmb_tal.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());   // citem_tal
                    cmb_tal.ValueMember = row.ItemArray[1].ToString(); // citem_tal.Value.ToString();
                }
                // seleccion de detalle 2 (tallado, marqueteado, etc)
                cmb_det2.Items.Clear();
                //tx_dat_det2.Text = "";
                cmb_det2.Tag = "";
                const string condt2 = "select descrizione,idcodice from desc_dt2 " +
                    "where numero=1 order by idcodice";
                MySqlCommand cmddt2 = new MySqlCommand(condt2, conn);
                DataTable dtdt2 = new DataTable();
                MySqlDataAdapter dadt2 = new MySqlDataAdapter(cmddt2);
                dadt2.Fill(dtdt2);
                foreach (DataRow row in dtdt2.Rows)
                {
                    cmb_det2.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt2
                    cmb_det2.ValueMember = row.ItemArray[1].ToString();     //citem_dt2.Value.ToString();
                }
                // seleccion de detalle 3
                cmb_det3.Items.Clear();
                //tx_dat_det3.Text = "";
                cmb_det3.Tag = "";
                const string condt3 = "select descrizione,idcodice from desc_dt3 where numero=1";
                MySqlCommand cmddt3 = new MySqlCommand(condt3, conn);
                DataTable dtdt3 = new DataTable();
                MySqlDataAdapter dadt3 = new MySqlDataAdapter(cmddt3);
                dadt3.Fill(dtdt3);
                foreach (DataRow row in dtdt3.Rows)
                {
                    cmb_det3.Items.Add(row.ItemArray[1].ToString() + "  -  " + row.ItemArray[0].ToString());  // citem_dt3
                    cmb_det3.ValueMember = row.ItemArray[1].ToString();    //citem_dt3.Value.ToString();
                }
                // seleccion de moneda nacional -> por defecto moneda local es la primera fila
                MySqlCommand mimon = new MySqlCommand("SELECT * from desc_mon order by id limit 1", conn);
                MySqlDataReader drmon = mimon.ExecuteReader();
                if (drmon.Read())
                {
                    v_monLoc = drmon.GetString(1);
                }
            }
            conn.Close();
        }
        private void grilla()                                                   // arma la grilla
        {
            // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
            // a.valor,a.acuenta,a.saldo,a.dscto,a.clte_recoje,a.seresma,a.pisoent,a.ascensor,a.pcontacto,a.dreferen,a.telcont,a.totsad
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            //advancedDataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            advancedDataGridView1.DataSource = dtg;
            // id 
            advancedDataGridView1.Columns[0].Visible = false;
            advancedDataGridView1.Columns[0].HeaderText = "id";    // titulo de la columna
            // tipo contrato
            advancedDataGridView1.Columns[1].Visible = false;
            // codigo de contrato
            advancedDataGridView1.Columns[2].Visible = true;            // columna visible o no
            advancedDataGridView1.Columns[2].HeaderText = "Contrato";    // titulo de la columna
            advancedDataGridView1.Columns[2].Width = 70;                // ancho
            advancedDataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[2].Tag = "validaNO";
            advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // estado del contrato
            advancedDataGridView1.Columns[3].Visible = true;
            advancedDataGridView1.Columns[3].HeaderText = "Estado";    // titulo de la columna
            advancedDataGridView1.Columns[3].Width = 70;                // ancho
            advancedDataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            advancedDataGridView1.Columns[3].Tag = "validaNO";
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Local venta
            advancedDataGridView1.Columns[4].Visible = true;
            advancedDataGridView1.Columns[4].HeaderText = "Local Vta.";
            advancedDataGridView1.Columns[4].Width = 80;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaSI";          // las celdas de esta columna se SI se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha del contrato
            advancedDataGridView1.Columns[5].Visible = true;
            advancedDataGridView1.Columns[5].HeaderText = "Fecha";
            advancedDataGridView1.Columns[5].Width = 70;
            advancedDataGridView1.Columns[5].ReadOnly = true;
            advancedDataGridView1.Columns[5].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // id cliente
            advancedDataGridView1.Columns[6].Visible = true;
            advancedDataGridView1.Columns[6].HeaderText = "Cliente";
            advancedDataGridView1.Columns[6].Width = 80;
            advancedDataGridView1.Columns[6].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[6].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // nombre cliente
            advancedDataGridView1.Columns[7].Visible = true;
            advancedDataGridView1.Columns[7].HeaderText = "Nombre del cliente";
            advancedDataGridView1.Columns[7].Width = 200;
            advancedDataGridView1.Columns[7].ReadOnly = true;
            advancedDataGridView1.Columns[7].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // comentarios
            advancedDataGridView1.Columns[8].Visible = true;
            advancedDataGridView1.Columns[8].HeaderText = "Comentarios";
            advancedDataGridView1.Columns[8].Width = 250;
            advancedDataGridView1.Columns[8].ReadOnly = false;
            advancedDataGridView1.Columns[8].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            advancedDataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Fecha de Entrega
            advancedDataGridView1.Columns[9].Visible = true;
            advancedDataGridView1.Columns[9].HeaderText = "Fecha Ent";
            advancedDataGridView1.Columns[9].Width = 70;
            advancedDataGridView1.Columns[9].ReadOnly = false;
            advancedDataGridView1.Columns[9].Tag = "validaNO";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // dir entrega
            advancedDataGridView1.Columns[10].Visible = true;
            advancedDataGridView1.Columns[10].HeaderText = "Dir.Entrega";
            advancedDataGridView1.Columns[10].Width = 150;
            advancedDataGridView1.Columns[10].ReadOnly = false;
            advancedDataGridView1.Columns[10].Tag = "validaNO";          // las celdas de esta columna SI se validan
            advancedDataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // valor
            advancedDataGridView1.Columns[11].Visible = false;
            // a cuenta
            advancedDataGridView1.Columns[12].Visible = false;
            // saldo
            advancedDataGridView1.Columns[13].Visible = false;
            // descuento %
            advancedDataGridView1.Columns[14].Visible = false;
            // cliente recoje
            advancedDataGridView1.Columns[15].Visible = false;
            // servicio espacial de maniobra
            advancedDataGridView1.Columns[16].Visible = false;
            // piso de entrega
            advancedDataGridView1.Columns[17].Visible = false;
            // ascensor
            advancedDataGridView1.Columns[18].Visible = false;
            // persona de contacto para la dirección/instalación
            advancedDataGridView1.Columns[19].Visible = false;
            // referencia de la dirección
            advancedDataGridView1.Columns[20].Visible = false;
            // telefono del contacto .. telcont
            advancedDataGridView1.Columns[21].Visible = false;
            // total servicios adicionales
            advancedDataGridView1.Columns[22].Visible = false;
        }
        private void grilladet(string modo)                                     // grilla detalle
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.Font = tiplg;
            dataGridView1.DefaultCellStyle.Font = tiplg;
            dataGridView1.RowTemplate.Height = 15;
            dataGridView1.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            if (modo == "NUEVO") dataGridView1.ColumnCount = 20;
            // id 
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].Width = 30;                // ancho
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].HeaderText = "Id";         // titulo de la columna
            dataGridView1.Columns[0].Name = "iddetacon";
            // item
            dataGridView1.Columns[1].Visible = true;            // columna visible o no
            dataGridView1.Columns[1].HeaderText = "Item";    // titulo de la columna
            dataGridView1.Columns[1].Width = 100;                // ancho
            dataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].Name = "item";
            // cant
            dataGridView1.Columns[2].Visible = true;            // columna visible o no
            dataGridView1.Columns[2].HeaderText = "Cant";    // titulo de la columna
            dataGridView1.Columns[2].Width = 30;                // ancho
            dataGridView1.Columns[2].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Name = "cant";
            // nombre
            dataGridView1.Columns[3].Visible = true;            // columna visible o no
            dataGridView1.Columns[3].HeaderText = "Nombre";    // titulo de la columna
            dataGridView1.Columns[3].Width = 200;                // ancho
            dataGridView1.Columns[3].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[3].Name = "nombre";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // medidas  a.medidas,a.madera,a.precio,a.totalMN,saldo,
            dataGridView1.Columns[4].Visible = true;            // columna visible o no
            dataGridView1.Columns[4].HeaderText = "Medidas";    // titulo de la columna
            dataGridView1.Columns[4].Width = 100;                // ancho
            dataGridView1.Columns[4].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].Name = "medidas";
            // madera
            dataGridView1.Columns[5].Visible = true;            // columna visible o no
            dataGridView1.Columns[5].HeaderText = "Madera";    // titulo de la columna
            dataGridView1.Columns[5].Width = 60;                // ancho
            dataGridView1.Columns[5].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].Name = "madera";
            // precio
            dataGridView1.Columns[6].Visible = true;            // columna visible o no
            dataGridView1.Columns[6].HeaderText = "Precio";    // titulo de la columna
            dataGridView1.Columns[6].Width = 70;                // ancho
            dataGridView1.Columns[6].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[6].Name = "precio";
            // total
            dataGridView1.Columns[7].Visible = true;            // columna visible o no
            dataGridView1.Columns[7].HeaderText = "Total";    // titulo de la columna
            dataGridView1.Columns[7].Width = 70;                // ancho
            dataGridView1.Columns[7].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[7].Name = "total";
            // saldo
            dataGridView1.Columns[8].Visible = true;            // columna visible o no
            dataGridView1.Columns[8].HeaderText = "Saldo"; // titulo de la columna
            dataGridView1.Columns[8].Width = 70;                // ancho
            dataGridView1.Columns[8].ReadOnly = true;           // lectura o no
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].Name = "saldo";
            // pedido   pedido,codref,coment,a.detpied,codpie,na,tda_item,
            dataGridView1.Columns[9].Visible = true;            // columna visible o no
            dataGridView1.Columns[9].HeaderText = "Pedido";      // titulo de la columna
            dataGridView1.Columns[9].Width = 60;                 // ancho
            dataGridView1.Columns[9].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[9].Name = "pedido";
            // codref = codigo de madera
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[10].HeaderText = "Codref";      // titulo de la columna
            dataGridView1.Columns[10].Width = 60;                 // ancho
            dataGridView1.Columns[10].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[10].Name = "codref";
            // coment
            dataGridView1.Columns[11].Visible = true;
            dataGridView1.Columns[11].HeaderText = "Comentario";      // titulo de la columna
            dataGridView1.Columns[11].Width = 160;                 // ancho
            dataGridView1.Columns[11].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[11].Name = "coment";
            // piedra 
            dataGridView1.Columns[12].Visible = true;
            dataGridView1.Columns[12].HeaderText = "Piedra";      // titulo de la columna
            dataGridView1.Columns[12].Width = 60;                 // ancho
            dataGridView1.Columns[12].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[12].Name = "Piedra";
            // codigo piedra  - EN EL CASO DE CONTRATOS DE SERVICIOS % DETRACCION DEL ITEM
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[13].HeaderText = "CodPie";      // titulo de la columna
            dataGridView1.Columns[13].Width = 60;                 // ancho
            dataGridView1.Columns[13].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[13].Name = "CodPie";
            // nab (nuevo o actualiza o borra)
            dataGridView1.Columns[14].Visible = false;
            // tda del item
            dataGridView1.Columns[15].Visible = true;
            dataGridView1.Columns[15].HeaderText = "Tienda";      // titulo de la columna
            dataGridView1.Columns[15].Width = 60;                 // ancho
            dataGridView1.Columns[15].ReadOnly = true;            // lectura o no
            dataGridView1.Columns[15].Name = "tda_item";
            // total 
            if (int.Parse(dataGridView1.Columns.Count.ToString()) > 16)
            {
                dataGridView1.Columns[16].Visible = false;
                dataGridView1.Columns[16].HeaderText = "totCat";      // titulo de la columna
                dataGridView1.Columns[16].Width = 60;                 // ancho
                dataGridView1.Columns[16].ReadOnly = true;            // lectura o no
                dataGridView1.Columns[16].Name = "totCat";
                // total dscto de la fila
                dataGridView1.Columns[17].Visible = true;
                dataGridView1.Columns[17].HeaderText = "totDscto";      // titulo de la columna
                dataGridView1.Columns[17].Width = 60;                 // ancho
                dataGridView1.Columns[17].ReadOnly = true;            // lectura o no
                dataGridView1.Columns[17].Name = "totDscto";
                // tipo de contrato, B=bienes, S=servicios .......... esto es cabecera, no detalle!
                //dataGridView1.Columns[18].Visible = false;
                //dataGridView1.Columns[18].HeaderText = "BoS";      // titulo de la columna
                //dataGridView1.Columns[18].Width = 30;                 // ancho
                //dataGridView1.Columns[18].ReadOnly = true;            // lectura o no
                //dataGridView1.Columns[18].Name = "BoS";
                dataGridView1.Columns[18].Visible = false;
                dataGridView1.Columns[18].HeaderText = "rapifac";      // titulo de la columna
                dataGridView1.Columns[18].Width = 60;                 // ancho
                dataGridView1.Columns[18].ReadOnly = true;            // lectura o no
                dataGridView1.Columns[18].Name = "alterno1";
            }
        }
        private bool bus_detalle_detrac()                                       // busca codigos detraccion diferentes en el detalle
        {
            bool retorna = true;
            string codi = "";
            if (rb_servi.Checked == true)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[13].Value != null && row.Cells[13].Value.ToString() != "")
                    {
                        if (codi != "")
                        {
                            if (row.Cells[13].Value.ToString() != codi)
                            {
                                retorna = false;     // osea si hay codigo de detrac diferentes
                            }
                        }
                        codi = row.Cells[13].Value.ToString();
                    }
                }
            }
            return retorna;
        }
        private void armani()                                                   // arma el codigo y busca en la maestra
        {
            string fam = "", mod = "", mad = "", tip = "", de1 = "", aca = "", tal = "", de2 = "", de3 = "";
            if (cmb_fam.SelectedItem != null) fam = cmb_fam.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_mod.SelectedItem != null) mod = cmb_mod.SelectedItem.ToString().Substring(0, 3);    // 3
            if (cmb_mad.SelectedItem != null) mad = cmb_mad.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_tip.SelectedItem != null) tip = cmb_tip.SelectedItem.ToString().Substring(0, 2);    // 2
            if (cmb_det1.SelectedItem != null) de1 = cmb_det1.SelectedItem.ToString().Substring(0, 2);  // 2
            if (cmb_aca.SelectedItem != null) aca = cmb_aca.SelectedItem.ToString().Substring(0, 1);    // 1
            if (cmb_tal.SelectedItem != null) tal = cmb_tal.SelectedItem.ToString().Substring(0, 2);    // 2
            if (cmb_det2.SelectedItem != null) de2 = cmb_det2.SelectedItem.ToString().Substring(0, 3);  // 3
            if (cmb_det3.SelectedItem != null) de3 = cmb_det3.SelectedItem.ToString().Substring(0, 3);  // 3 _____ total 18
            tx_d_codi.Text = fam + mod + mad + tip + de1 + aca + tal + de2 + de3;
            if (tx_vDc.Text == "")      // solo aplica si hacemos contrato desde la nada  17/01/2024
            {
                if (fam != "" && mod != "" && tip != "" && de1 != "" && aca != "" && de2 != "" && de3 != "")
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                        {
                            conn.Open();
                            if (conn.State == ConnectionState.Open)
                            {
                                string busca = "select id,nombr,medid,umed,soles2018,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,alterno1 " +
                                    "from items where capit=@fam and model=@mod and tipol=@tip and deta1=@dt1 and deta2=@dt2"; // se agrega detalle2 24/11/20 Gloria
                                MySqlCommand micon = new MySqlCommand(busca, conn);
                                //micon.Parameters.AddWithValue("@cod", codbs);
                                micon.Parameters.AddWithValue("@fam", fam);
                                micon.Parameters.AddWithValue("@mod", mod);
                                micon.Parameters.AddWithValue("@tip", tip);
                                micon.Parameters.AddWithValue("@dt1", de1);
                                micon.Parameters.AddWithValue("@dt2", de2); // se agrega 24/11/20 solicitado por Gloria
                                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                                DataTable dtm = new DataTable();
                                da.Fill(dtm);
                                if (dtm.Rows.Count == 0)
                                {
                                    /*
                                    var aaa = MessageBox.Show("No existe en la base de items" + Environment.NewLine +
                                        "Busca en el stock?", "Atención - confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aaa == DialogResult.Yes)
                                    {
                                        if (busstock(tx_d_codi.Text) == false)
                                        {
                                            MessageBox.Show("No existe en el stock", "Error en códido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            tx_d_nom.Text = "";
                                            tx_d_med.Text = "";
                                            tx_d_mad.Text = "";
                                            tx_d_det2.Text = "";
                                            tx_d_est.Text = "";
                                            return;
                                        }
                                        else
                                        {
                                            dtm.Dispose();
                                            conn.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        tx_d_nom.Text = "";
                                        tx_d_med.Text = "";
                                        tx_d_mad.Text = "";
                                        tx_d_det2.Text = "";
                                        tx_d_est.Text = "";
                                        dtm.Dispose();
                                        conn.Close();
                                        return;
                                    }
                                    */
                                    MessageBox.Show("No existe en la base de items", " Atención ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dtm.Dispose();
                                    conn.Close();
                                    return;
                                }
                                string gol = "";
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    DataRow fila = dtm.Rows[i];
                                    if (fila["mader"].ToString() == mad && fila["acaba"].ToString() == aca &&
                                        fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                        tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                                                                     //if (Tx_modo.Text == "NUEVO") tx_d_prec.Text = fila["soles2018"].ToString();  // precio
                                        tx_d_prec.Text = fila["soles2018"].ToString();  // 02-11-2020 reunion con Gloria, debe jalar precio en nuevo y edicion
                                        tx_d_dat_prec.Text = fila["soles2018"].ToString();  // precio original sin descuento
                                        tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                        gol = "1";
                                        tx_d_can.Focus();
                                        break;
                                    }
                                }
                                if (gol == "")
                                {
                                    for (int i = 0; i < dtm.Rows.Count; i++)
                                    {
                                        DataRow fila = dtm.Rows[i];
                                        if (mad != "X" &&
                                        fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3) // fila["acaba"].ToString() == aca &&
                                        {
                                            tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                            tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                                                                         //if (tx_d_id.Text.Trim() == "") tx_d_prec.Text = fila["soles2018"].ToString();  // Tx_modo.Text == "NUEVO"
                                            tx_d_prec.Text = fila["soles2018"].ToString();  // 02-11-2020 reunion con Gloria, debe jalar precio en nuevo y edicion
                                            tx_d_dat_prec.Text = fila["soles2018"].ToString();  // precio original sin descuento
                                            tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                            gol = "1";
                                            tx_d_can.Focus();
                                            break;
                                        }
                                        if (mad != "X" &&
                                        fila["deta2"].ToString().Substring(0, 1) == letpied && fila["deta3"].ToString() == de3) // fila["acaba"].ToString() == aca &&
                                        {
                                            tx_d_nom.Text = fila["nombr"].ToString();    // dr.GetString(1);
                                            tx_d_med.Text = fila["medid"].ToString();    // dr.GetString(2);
                                                                                         //if (tx_d_id.Text.Trim() == "") tx_d_prec.Text = fila["soles2018"].ToString();  // Tx_modo.Text == "NUEVO"
                                            tx_d_prec.Text = fila["soles2018"].ToString();  // 02-11-2020 reunion con Gloria, debe jalar precio en nuevo y edicion
                                            tx_d_dat_prec.Text = fila["soles2018"].ToString();  // precio original sin descuento
                                            tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                            gol = "1";
                                            tx_d_can.Focus();
                                            break;
                                        }
                                    }
                                }
                                if (gol == "")
                                {
                                    /*
                                    var aa = MessageBox.Show("No existe en la base de datos de items!" + Environment.NewLine + 
                                        "Busca en el stock?", "Atención - Confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aa == DialogResult.Yes)     // buscamos si existe en el stock
                                    {
                                        if (busstock(tx_d_codi.Text) == false)
                                        {
                                            MessageBox.Show("No existe en el stock", "Error en códido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            tx_d_nom.Text = "";
                                            tx_d_med.Text = "";
                                            tx_d_mad.Text = "";
                                            tx_d_det2.Text = "";
                                            tx_d_est.Text = "";
                                            conn.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        tx_d_nom.Text = "";
                                        tx_d_med.Text = "";
                                        tx_d_mad.Text = "";
                                        tx_d_det2.Text = "";
                                        tx_d_est.Text = "";
                                        conn.Close();
                                        return;
                                    }*/
                                    MessageBox.Show("No existe en la base de items", " Atención ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dtm.Dispose();
                                    conn.Close();
                                    return;
                                }
                                dtm.Dispose();
                                conn.Close();
                            }
                            else
                            {
                                MessageBox.Show("No se puede conectar a la base de datos", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                conn.Close();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error interno en codigo");
                        Application.Exit();
                        return;
                    }
                }
            }
            if (tx_vDc.Text == "S")
            {
                if (fam != "" && mod != "" && tip != "" && de1 != "" && aca != "" && de2 != "" && de3 != "")
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                        {
                            conn.Open();
                            if (conn.State == ConnectionState.Open)
                            {
                                string busca = "select id,nombr,medid,umed,soles2018,capit,model,mader,tipol,deta1,acaba,talle,deta2,deta3,alterno1 " +
                                    "from items where capit=@fam and model=@mod and tipol=@tip and deta1=@dt1 and deta2=@dt2"; // se agrega detalle2 24/11/20 Gloria
                                MySqlCommand micon = new MySqlCommand(busca, conn);
                                //micon.Parameters.AddWithValue("@cod", codbs);
                                micon.Parameters.AddWithValue("@fam", fam);
                                micon.Parameters.AddWithValue("@mod", mod);
                                micon.Parameters.AddWithValue("@tip", tip);
                                micon.Parameters.AddWithValue("@dt1", de1);
                                micon.Parameters.AddWithValue("@dt2", de2); // se agrega 24/11/20 solicitado por Gloria
                                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                                DataTable dtm = new DataTable();
                                da.Fill(dtm);
                                if (dtm.Rows.Count == 0)
                                {
                                    MessageBox.Show("No existe en la base de items", " Atención ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dtm.Dispose();
                                    conn.Close();
                                    return;
                                }
                                string gol = "";
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    DataRow fila = dtm.Rows[i];
                                    if (fila["mader"].ToString() == mad && fila["acaba"].ToString() == aca &&
                                        fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3)
                                    {
                                        tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                        gol = "1";
                                        tx_d_can.Focus();
                                        break;
                                    }
                                }
                                if (gol == "")
                                {
                                    for (int i = 0; i < dtm.Rows.Count; i++)
                                    {
                                        DataRow fila = dtm.Rows[i];
                                        if (mad != "X" &&
                                        fila["deta2"].ToString() == de2 && fila["deta3"].ToString() == de3) // fila["acaba"].ToString() == aca &&
                                        {
                                            tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                            gol = "1";
                                            tx_d_can.Focus();
                                            break;
                                        }
                                        if (mad != "X" &&
                                        fila["deta2"].ToString().Substring(0, 1) == letpied && fila["deta3"].ToString() == de3) // fila["acaba"].ToString() == aca &&
                                        {
                                            tx_d_codal1.Text = fila["alterno1"].ToString();     // codigo alterno para rapifac
                                            gol = "1";
                                            tx_d_can.Focus();
                                            break;
                                        }
                                    }
                                }
                                if (gol == "")
                                {
                                    MessageBox.Show("No existe en la base de items", " Atención ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dtm.Dispose();
                                    conn.Close();
                                    return;
                                }
                                dtm.Dispose();
                                conn.Close();
                            }
                            else
                            {
                                MessageBox.Show("No se puede conectar a la base de datos", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                conn.Close();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error interno en codigo");
                        Application.Exit();
                        return;
                    }
                }
            }
        }
        private bool busstock(string codigo)                                    // busca codigo en stock y retorna true o false
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return retorna;
            }
            string busca = "select a.nombr,a.medid,a.mader,m.descrizionerid,d2.descrizionerid,e.descrizionerid from almloc a " +
                "left join desc_mad m on trim(m.idcodice)=trim(a.mader) " +
                "left join desc_dt2 d2 on trim(d2.idcodice)=trim(a.deta2) " +
                "left join desc_est e on trim(e.idcodice)=trim(a.acaba) " +
                "where left(insert(a.codig,11,2,'XX'),18)=@cc";
            MySqlCommand micon = new MySqlCommand(busca, conn);
            micon.Parameters.AddWithValue("@cc", codigo);
            MySqlDataReader dr = micon.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    tx_d_nom.Text = dr.GetString(0);
                    tx_d_med.Text = dr.GetString(1);
                    tx_d_mad.Text = dr.GetString(2);
                    tx_dat_mad.Text = dr.GetString(3);
                    tx_d_det2.Text = dr.GetString(4);
                    tx_d_est.Text = dr.GetString(5);
                    retorna = true;
                    tx_d_can.Focus();
                }
            }
            dr.Close();
            conn.Close();
            return retorna;
        }
        private void jalaoc(string campo)                                       // jala datos del contrato
        {
            if (campo == "tx_idr" && tx_rind.Text != "") // tx_idr.Text
            {
                // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                // a.valor,a.acuenta,a.saldo,a.dscto 
                tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();     // contrato
                tx_dat_tiped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString();  // tipo contrato
                tx_dat_orig.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[4].Value.ToString();   // local venta
                dtp_pedido.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString());
                tx_dat_estad.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[3].Value.ToString();  // estado
                tx_idcli.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString();      // id del cliente
                tx_coment.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[8].Value.ToString();     // comentario
                tx_dirent.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[10].Value.ToString();     // direc. de entrega
                if (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString().Trim() == "") dtp_entreg.Checked = false;
                else dtp_entreg.Value = Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString());    // fecha entrega
                tx_valor.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[11].Value.ToString();     // valor del contrato
                //tx_dscto.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString();     // descuento final
                // tx_desCab.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[14].Value.ToString();     // descuento final
                tx_desCab.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[26].Value.ToString();        // 22/02/2023
                desCab = decimal.Parse(tx_desCab.Text);
                //tx_bruto.Text = (decimal.Parse(tx_valor.Text) + decimal.Parse(tx_dscto.Text)).ToString("0.00");     // total bruto
                tx_bruto.Text = (decimal.Parse(tx_valor.Text) + decimal.Parse(tx_desCab.Text)).ToString("0.00");     // total bruto
                tx_acta.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[12].Value.ToString();     // pago a cuenta
                tx_saldo.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[13].Value.ToString();     // saldo actual del contrato
                chk_lugent.Checked = (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[15].Value.ToString() == "1") ? true:false ;
                chk_serema.Checked = (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[16].Value.ToString() == "1") ? true : false;
                chk_ascensor.Checked = (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[18].Value.ToString() == "1") ? true : false;  // ascensor
                tx_piso.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[17].Value.ToString();     // piso de la instalac.
                tx_contac.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[19].Value.ToString();     // persona de contacto
                tx_dirRef.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[20].Value.ToString();     // referencia de direccion
                tx_telcont.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[21].Value.ToString();    // telefono del contact
                tx_totesp.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[22].Value.ToString();    // total servicios adicionales
                tx_motivD.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[23].Value.ToString();    // motivo de descuento
                tx_conMadre.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[24].Value.ToString();    // tipo de contrato Bienes o Servicios
                tx_verCont.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[25].Value.ToString();    // versión del contrato, anterior=(vacio), 2.0 nuevo
                jaladatclt(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[6].Value.ToString());          // jala datos del cliente
                //
                if (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString() == "1") rb_bienes.PerformClick();
                else
                {
                    if (advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[1].Value.ToString() == "9") rb_servi.PerformClick();
                }
                cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);        // tipo de contrato
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);     // local de venta
                cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);    // estado
                jaladet(tx_codped.Text);
            }
            if (campo == "tx_codped" && tx_codped.Text != "")
            {
                int cta = 0;
                foreach (DataRow row in dtg.Rows)
                {
                    if (row["contrato"].ToString().Trim() == tx_codped.Text.Trim())
                    {
                        // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                        // a.valor,a.acuenta,a.saldo,a.dscto, 
                        tx_dat_tiped.Text = row["tipocon"].ToString();                      // tipo contrato
                        tx_idr.Text = row["id"].ToString();                                 // id del registro
                        tx_rind.Text = cta.ToString();
                        tx_dat_estad.Text = row["status"].ToString();                       // estado
                        tx_dat_orig.Text = row["tipoes"].ToString();                        // local venta
                        dtp_pedido.Value = Convert.ToDateTime(row["fecha"].ToString());     // fecha 
                        tx_idcli.Text = row["cliente"].ToString();                          // id del cliente
                        jaladatclt(row["cliente"].ToString());                              // jala datos del cliente
                        //dtp_entreg.Value = Convert.ToDateTime(row["entrega"].ToString());   // fecha entrega
                        //if (advancedDataGridView1.Rows[cta].Cells[9].Value.ToString().Trim() == "") dtp_entreg.Checked = false;
                        //else dtp_entreg.Value = Convert.ToDateTime(advancedDataGridView1.Rows[cta].Cells[9].Value.ToString());    // fecha entrega
                        if (row[9].ToString().Trim() == "") dtp_entreg.Checked = false;
                        else dtp_entreg.Value = Convert.ToDateTime(row[9].ToString());
                        tx_coment.Text = row["coment"].ToString();                          // comentario
                        tx_dirent.Text = row["dentrega"].ToString();                        // direc de entrega
                        tx_valor.Text = row["valor"].ToString();                            // valor del contrato
                        //tx_dscto.Text = row["dscto"].ToString();                            // descuento final
                        //tx_desCab.Text = row["dscto"].ToString();     // descuento final
                        tx_desCab.Text = row["dsctocab"].ToString();     // 22/02/2023
                        desCab = decimal.Parse(tx_desCab.Text);
                        tx_acta.Text = row["acuenta"].ToString();                           // pago a cuenta
                        tx_saldo.Text = row["saldo"].ToString();                            // saldo actual del contrato
                        chk_lugent.Checked = (row["clte_recoje"].ToString() == "1")? true:false;
                        chk_serema.Checked = (row["seresma"].ToString() == "1")? true:false;
                        chk_ascensor.Checked = (row["ascensor"].ToString() == "1")? true:false;
                        tx_piso.Text = row["pisoent"].ToString();                           // piso donde se lleva el mueble
                        tx_contac.Text = row["pcontacto"].ToString();                       // persona de contacto
                        tx_dirRef.Text = row["dreferen"].ToString();                        // referencia de direccion
                        tx_telcont.Text = row["telcont"].ToString();                        // telefono del contacto de instal
                        tx_totesp.Text = string.Format("{0:#0.00}", row["totsad"].ToString());                          // total servicios adicionales
                        tx_motivD.Text = row["motivDes"].ToString();                        // motivo de descuento
                        tx_conMadre.Text = row["articulo"].ToString();                      // tipo de contrato Bienes o Servicios
                        tx_verCont.Text = row["marca0"].ToString();                         // 
                        cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);
                        cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                        cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);
                        if (row["tipocon"].ToString() == "1") rb_bienes.PerformClick();
                        if (row["tipocon"].ToString() == "9") rb_servi.PerformClick();
                        //
                        jaladet(tx_codped.Text);
                    }
                    cta = cta + 1;
                }
                tx_acta_Leave(null, null);
            }
            if (Tx_modo.Text == "EDITAR")   // si permite modificacion se habilitan los campos
            {
                if (escambio.Contains(tx_dat_estad.Text))
                {
                    escribepag(tabuser);
                }
                else
                {
                    MessageBox.Show("No se permite modificar totalmente el contrato", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sololeepag(tabuser);
                    tx_acta.Enabled = true;
                    tx_acta.ReadOnly = true;
                    dtp_entreg.Enabled = true;
                    tx_coment.Enabled = true;
                    tx_coment.ReadOnly = false;
                }
            }
            calculos();
        }
        private void jaladatclt(string id)                                      // jala datos del cliente
        {
            Int32 vi = -1;
            string consulta = "select ifnull(razonsocial,''),ifnull(direcc1,''),ifnull(direcc2,''),ifnull(localidad,''),ifnull(provincia,'')," +
                "ifnull(depart,''),ifnull(tipdoc,''),ifnull(ruc,''),ifnull(numerotel1,''),ifnull(numerotel2,''),ifnull(email,''),ifnull(desc_doc.cnt,'') " +
                "from anag_cli left join desc_doc on desc_doc.idcodice=anag_cli.tipdoc " +
                "where idanagrafica=@idc";
            //try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@idc", id);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        tx_dat_tdoc.Text = dr.GetString(6);
                        tx_dat_tdoc.Tag = dr.GetString(6);                                      // todos los tag sirven para comparar si el text fue cambiado
                        tx_ndc.Text = dr.GetString(7);
                        tx_ndc.Tag = dr.GetString(7);                                           // si el tag no coincide con el text se graba en la tabla
                        tx_nombre.Text = dr.GetString(0);
                        tx_nombre.Tag = dr.GetString(0);                                        // despues de grabar en la tabla actualiza el tag con el nuevo text
                        tx_direc.Text = dr.GetString(1).Trim() + " " + dr.GetString(2).Trim();
                        tx_direc.Tag = dr.GetString(1).Trim() + " " + dr.GetString(2).Trim();
                        tx_dist.Text = dr.GetString(3);
                        tx_dist.Tag = dr.GetString(3);
                        tx_prov.Text = dr.GetString(4);
                        tx_prov.Tag = dr.GetString(4);
                        tx_dpto.Text = dr.GetString(5);
                        tx_dpto.Tag = dr.GetString(5);
                        tx_telef1.Text = dr.GetString(8);
                        tx_telef1.Tag = dr.GetString(8);
                        tx_telef2.Text = dr.GetString(9);
                        tx_telef2.Tag = dr.GetString(9);
                        tx_mail.Text = dr.GetString(10);
                        tx_mail.Tag = dr.GetString(10);
                        if (dr.GetString(11).Trim() != "") vi = Int32.Parse(dr.GetString(11));
                    }
                    dr.Close();
                    cmb_tdoc.SelectedIndex = vi;    //cmb_tdoc.FindString(tx_dat_tdoc.Text);
                }
                conn.Close();
            }
        }
        private void jaladet(string pedido)                                     // jala el detalle del contrato
        {
            /*string jalad = "SELECT a.iddetacon,a.item,a.cant,a.nombre,a.medidas,a.madera,a.precio,a.total,a.saldo,a.pedido,c.descrizionerid as codref,a.coment," +
                "ifnull(b.descrizionerid,'') as piedra,ifnull(b.idcodice,'') as codpie,space(1) as na,tda_item,space(1) as totCat,totdscto " +
                "FROM detacon a " +
                "left join desc_dt2 b on b.idcodice=a.piedra " +
                "left join desc_mad c on c.idcodice=a.madera " +
                "WHERE a.contratoh = @cont ";
            */
            string jalad = "SELECT a.iddetacon,a.item,a.cant,a.nombre,a.medidas,a.madera,a.precio,a.total,a.saldo,a.pedido,c.descrizionerid as codref,a.coment," +
                "ifnull(b.descrizionerid,'') as piedra,ifnull(b.idcodice,if(left(a.item,1)=@letg,it.detporc,'')) as codpie,space(1) as na,tda_item," +
                "space(1) as totCat,a.totdscto,a.alterno1 " +
                "FROM detacon a " +
                "left join desc_dt2 b on b.idcodice=a.piedra " +
                "left join desc_mad c on c.idcodice=a.madera " +
                "left join items_adic it on it.codig=a.item " +
                "WHERE a.contratoh = @cont ";
            //try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand micon = new MySqlCommand(jalad, conn);
                    micon.Parameters.AddWithValue("@cont", pedido);
                    micon.Parameters.AddWithValue("@letg", letgru);
                    MySqlDataAdapter da = new MySqlDataAdapter(micon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    grilladet("edita");     // obtiene contenido de grilla con DT
                    dt.Dispose();
                    da.Dispose();
                }
                conn.Close();
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error en obtener detalle del contrato");
            //    Application.Exit();
            //    return;
            //}
        }
        private bool jalacont(string ctrato)                                    // jala contrato desde la base de datos
        {
            bool retorna = false;
            string jalac = "select a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,date_format(date(a.fecha),'%Y-%m-%d') as fecha,a.cliente,ifnull(b.razonsocial,'') as razonsocial,a.coment," +
                    "date_format(date(a.entrega),'%Y-%m-%d') as entrega,a.dentrega,a.valor,a.acuenta,a.saldo,a.dscto,a.clte_recoje,a.seresma,a.pisoent,a.ascensor," +
                    "a.pcontacto,a.dreferen,a.telcont,a.totsad " +
                    "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                    "where a.contrato=@cont";
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand micon = new MySqlCommand(jalac, conn);
                    micon.Parameters.AddWithValue("@cont", ctrato);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            //tx_codped.Text = advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[2].Value.ToString();     // contrato
                            tx_idr.Text = dr.GetString(0);
                            tx_dat_tiped.Text = dr.GetString(1);  // tipo contrato
                            tx_dat_orig.Text = dr.GetString(4);   // local venta
                            dtp_pedido.Value = dr.GetDateTime(5); // Convert.ToDateTime(advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[5].Value.ToString());
                            tx_dat_estad.Text = dr.GetString(3);  // estado
                            tx_idcli.Text = dr.GetString(6);      // id del cliente
                            tx_coment.Text = dr.GetString(8);     // comentario
                            tx_dirent.Text = dr.GetString(10);     // direc. de entrega
                            // advancedDataGridView1.Rows[int.Parse(tx_rind.Text)].Cells[9].Value.ToString().Trim() == ""
                            if (dr.GetDateTime(9) == null) dtp_entreg.Checked = false;
                            else dtp_entreg.Value = dr.GetDateTime(9);    // fecha entrega
                            tx_valor.Text = dr.GetString(11);     // valor del contrato
                            //tx_dscto.Text = dr.GetString(14);     // descuento final
                            tx_desCab.Text = dr.GetString(14);     // descuento final
                            tx_bruto.Text = (decimal.Parse(tx_valor.Text) + decimal.Parse(tx_dscto.Text)).ToString("0.00");     // total bruto
                            tx_acta.Text = dr.GetString(12);     // pago a cuenta
                            tx_saldo.Text = dr.GetString(13);     // saldo actual del contrato
                            chk_lugent.Checked = (dr.GetString(15) == "1") ? true : false;
                            chk_serema.Checked = (dr.GetString(16) == "1") ? true : false;
                            chk_ascensor.Checked = (dr.GetString(18) == "1") ? true : false;  // ascensor
                            tx_piso.Text = dr.GetString(17);     // piso de la instalac.
                            tx_contac.Text = dr.GetString(19);     // persona de contacto
                            tx_dirRef.Text = dr.GetString(20);     // referencia de direccion
                            tx_telcont.Text = dr.GetString(21);    // telefono del contact
                            tx_totesp.Text = dr.GetString(22);    // total servicios adicionales
                            jaladatclt(dr.GetString(6));          // jala datos del cliente
                            //                                                                                                    //
                            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tx_dat_tiped.Text);        // tipo de contrato
                            cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);     // local de venta
                            cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);    // estado
                        }
                        dr.Close();
                        retorna = true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en obtener detalle del contrato");
                Application.Exit();
                return retorna;
            }
            return retorna;
        }
        private bool correNum(MySqlConnection conn)                             // avanza la numeracion de contratos
        {
            bool retorna = true;
            if (tncont == "AUTOMA")  // modo automatico y el campo vacio
            {
                if(rb_bienes.Checked == true)
                {
                    string vamos = "select CONCAT(serie,right(CONCAT('00000',CAST(actual AS SIGNED) + 1),5)) from series " +
                        "WHERE tipdoc=@tdo AND sede=@sed"; 
                    MySqlCommand covam = new MySqlCommand(vamos, conn);
                    covam.Parameters.AddWithValue("@tdo", tdc);
                    covam.Parameters.AddWithValue("@sed", tx_dat_orig.Text);
                    MySqlDataReader dr = covam.ExecuteReader();
                    if (dr.Read())
                    {
                        tx_codped.Text = dr.GetString(0);
                    }
                    dr.Dispose();
                    covam.Dispose();
                    //
                    using (MySqlCommand micon = new MySqlCommand("update series set actual=@act where tipdoc=@tdo AND sede=@sed and vermodi<>@bs AND id>0", conn))
                    {
                        micon.Parameters.AddWithValue("@act", tx_codped.Text);   // tx_codped.Text.Substring(2, tx_codped.Text.Trim().Length - 2)
                        micon.Parameters.AddWithValue("@tdo", tdc);
                        micon.Parameters.AddWithValue("@sed", tx_dat_orig.Text);
                        micon.Parameters.AddWithValue("@bs", "S");  // diferente de servicio
                        micon.ExecuteNonQuery();
                    }
                }
                else
                {
                    string vamS = "";
                    if (tx_conMadre.Text == "")
                    {
                        vamS = "select CONCAT(TRIM(serie),right(CONCAT('000000',CAST(right(actual, (LENGTH(actual) - 1)) AS SIGNED) + 1),6)) from series " +
                            "WHERE tipdoc=@tdo AND sede=@sed and vermodi=@bs";
                    }
                    else
                    {
                        vamS = "select CONCAT(TRIM(serie),@conta,@conMad) from series " +
                            "WHERE tipdoc=@tdo AND sede=@sed and vermodi=@bs"; 
                    }
                    using (MySqlCommand covam = new MySqlCommand(vamS, conn))
                    {
                        covam.Parameters.AddWithValue("@conta", tx_conS.Text);
                        covam.Parameters.AddWithValue("@conMad", tx_conMadre.Text);
                        covam.Parameters.AddWithValue("@tdo", tdc);
                        covam.Parameters.AddWithValue("@sed", tx_dat_orig.Text);
                        covam.Parameters.AddWithValue("@bs", "S");                      // SERVICIOS
                        using (MySqlDataReader dr = covam.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                tx_codped.Text = dr.GetString(0);
                            }
                        }
                    }
                    if (tx_conMadre.Text == "")         // solo se actualiza el numero si es contrato de servicio sin contrado madre de bienes
                    {
                        using (MySqlCommand micon = new MySqlCommand("update series set actual=@act where tipdoc=@tdo AND sede=@sed and vermodi=@bs AND id>0", conn))
                        {
                            micon.Parameters.AddWithValue("@act", tx_codped.Text);   // tx_codped.Text.Substring(2, tx_codped.Text.Trim().Length - 2)
                            micon.Parameters.AddWithValue("@tdo", tdc);
                            micon.Parameters.AddWithValue("@sed", tx_dat_orig.Text);
                            micon.Parameters.AddWithValue("@bs", "S");
                            micon.ExecuteNonQuery();
                        }
                    }
                }
                if (tx_codped.Text.Trim() == "")
                {
                    MessageBox.Show("Falta configurar numeración automática", "Tabla series");
                    retorna = false;
                }
                else
                {
                    //
                }
            }
            return retorna;
        }
        private bool graba()                                                    // graba cabecera y detalle
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    if (correNum(conn) == false)
                    {
                        return retorna;
                    }
                    //
                    string inserta = "insert into contrat (fecha,tipoes,coment,cliente,entrega,contrato,STATUS,articulo,valor,acuenta,saldo,dscto," +
                        "dentrega,tipocon,USER,dia,clte_recoje,seresma,pisoent,ascensor,pcontacto,dreferen,telcont,totsad,motivDes,marca0,dsctocab) " +
                        "values (@fepe,@tall,@come,@idcl,@entr,@cope,@esta,@arti,@valo,@acta,@sald,@dsct,@dent,@tipe,@asd,now(),@cltr,@ceem," +
                        "@pise,@asce,@pecon,@drefe,@tecont,@totadi,@motdes,@mar0,@descab)";
                    MySqlCommand micon = new MySqlCommand(inserta, conn);
                    micon.Parameters.AddWithValue("@fepe", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@tall", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@come", tx_coment.Text);
                    micon.Parameters.AddWithValue("@idcl", tx_idcli.Text);
                    micon.Parameters.AddWithValue("@entr", (dtp_entreg.Checked == true) ? dtp_entreg.Value.ToString("yyyy-MM-dd") : null);
                    micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                    micon.Parameters.AddWithValue("@esta", tx_dat_estad.Text);
                    micon.Parameters.AddWithValue("@arti", tx_conMadre.Text); 
                    micon.Parameters.AddWithValue("@valo", tx_valor.Text);
                    micon.Parameters.AddWithValue("@acta", tx_acta.Text);
                    micon.Parameters.AddWithValue("@sald", tx_saldo.Text);
                    micon.Parameters.AddWithValue("@dsct", tx_dscto.Text);
                    micon.Parameters.AddWithValue("@dent", tx_dirent.Text);
                    micon.Parameters.AddWithValue("@tipe", (rb_servi.Checked == true)? 9 : 1);  // tx_dat_tiped.Text
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@cltr", (chk_lugent.Checked.ToString() == "True") ? "1" : "0");
                    micon.Parameters.AddWithValue("@ceem", (chk_serema.Checked.ToString() == "True") ? "1" : "0");
                    micon.Parameters.AddWithValue("@pise", (tx_piso.Text.Trim() == "")? "0": tx_piso.Text);
                    micon.Parameters.AddWithValue("@asce", (chk_ascensor.Checked.ToString() == "True") ? "1" : "0");
                    micon.Parameters.AddWithValue("@pecon", tx_contac.Text);
                    micon.Parameters.AddWithValue("@drefe", tx_dirRef.Text);
                    micon.Parameters.AddWithValue("@tecont", tx_telcont.Text);
                    micon.Parameters.AddWithValue("@totadi", (string.IsNullOrEmpty(tx_totesp.Text)) ? "0.00":tx_totesp.Text);
                    micon.Parameters.AddWithValue("@motdes", tx_motivD.Text);
                    micon.Parameters.AddWithValue("@mar0", "2");    // marca de version nueva integrador 2.0
                    micon.Parameters.AddWithValue("@descab", (tx_desCab.Text.Trim() == "")? 0 : decimal.Parse(tx_desCab.Text));
                    micon.ExecuteNonQuery();
                    string lid = "select last_insert_id()";
                    micon = new MySqlCommand(lid, conn);
                    MySqlDataReader rlid = micon.ExecuteReader();
                    if (rlid.Read())
                    {
                        tx_idr.Text = rlid.GetString(0);
                    }
                    rlid.Close();
                    foreach (string item in _comprobantes)
                    {
                        string[] partes = item.Split('-');
                        string actg = "update pagamenti a LEFT JOIN cabfactu b ON b.tipdvta=a.dv AND b.serdvta=a.serie AND b.numdvta=a.numero " +
                            "set a.contrato=@con,a.valor=@calc,a.acuenta=@acta,a.saldo=@sald,b.contrato=@con " +
                            "where a.dv=@dv and a.serie=@ser and a.numero=@num and a.idpagamenti>0";    // and a.fecha>=@fcon"; comentado 24/02/2023
                        // 15/02/2023, se agrega condicion de fecha del pago para evitar errores de digitacion de fechas en los pagos de datos migrados al 2.0
                        using (MySqlCommand mic = new MySqlCommand(actg, conn))
                        {
                            mic.Parameters.AddWithValue("@con", tx_codped.Text);
                            mic.Parameters.AddWithValue("@calc", tx_valor.Text);
                            mic.Parameters.AddWithValue("@acta", tx_acta.Text);
                            mic.Parameters.AddWithValue("@sald", tx_saldo.Text);
                            mic.Parameters.AddWithValue("@dv", (partes[0].Substring(0, 1) == "F") ? docFac : docBol);   // (tx_mc.Text == "F") ? docFac : docBol
                            mic.Parameters.AddWithValue("@ser", partes[1]);      // tx_serie.Text
                            mic.Parameters.AddWithValue("@num", partes[2]);      // tx_corre.Text
                            mic.Parameters.AddWithValue("@fcon", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                            mic.ExecuteNonQuery();
                        }
                    }
                    // detalle 
                    //dataGridView1.Sort(dataGridView1.Columns[1], System.ComponentModel.ListSortDirection.Ascending);  // ya no va 24/11/20 Gloria
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {   // iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,'na'
                        string acabado = dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(9, 1);
                        decimal vtotd = 0;
                        if (dataGridView1.Rows[i].Cells[17].Value != null)
                        {
                            //vtotd = decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString()) / decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            decimal.TryParse(dataGridView1.Rows[i].Cells[17].Value.ToString(), out vtotd);
                        }
                        decimal totdscto = vtotd;    // decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString());
                        string insdet = "insert into detacon (" +
                            "contratoh,tipo,item,cant,nombre,medidas,madera,precio,total,saldo,codref,coment,piedra,estado,tda_item,totdscto,alterno1) values (" +
                            "@cope,@tipe,@item,@cant,@nomb,@medi,@made,@prec,@tota,@sald,@cref,@come,@det2,@esta,@tdai,@tdscto,@crapif)";
                        micon = new MySqlCommand(insdet, conn);
                        micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                        micon.Parameters.AddWithValue("@tipe", tx_dat_orig.Text);       // tx_dat_tiped.Text
                        micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[2].Value.ToString());
                        micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());
                        micon.Parameters.AddWithValue("@prec", dataGridView1.Rows[i].Cells[6].Value.ToString());
                        micon.Parameters.AddWithValue("@tota", dataGridView1.Rows[i].Cells[7].Value.ToString());
                        micon.Parameters.AddWithValue("@esta", acabado); // dataGridView1.Rows[i].Cells[].Value.ToString()
                        micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[8].Value.ToString());
                        micon.Parameters.AddWithValue("@cref", dataGridView1.Rows[i].Cells[10].Value.ToString());
                        micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[11].Value.ToString());
                        micon.Parameters.AddWithValue("@det2", dataGridView1.Rows[i].Cells[13].Value.ToString());
                        micon.Parameters.AddWithValue("@tdai", dataGridView1.Rows[i].Cells[15].Value.ToString());
                        micon.Parameters.AddWithValue("@tdscto", totdscto);
                        micon.Parameters.AddWithValue("@crapif", dataGridView1.Rows[i].Cells[18].Value.ToString()); 
                        micon.ExecuteNonQuery();
                    }
                    /* if (tncont == "AUTOMA")
                    {
                        micon = new MySqlCommand("update series set actual=@act where tipdoc=@tdo AND sede=@sed AND id>0", conn);    //  AND rsocial=@raz
                        micon.Parameters.AddWithValue("@act", tx_codped.Text.Substring(2,tx_codped.Text.Trim().Length - 2));
                        micon.Parameters.AddWithValue("@tdo", tdc);
                        micon.Parameters.AddWithValue("@sed", tx_dat_orig.Text);
                        //micon.Parameters.AddWithValue("@raz", raz);
                        micon.ExecuteNonQuery();
                    }
                    */
                    retorna = true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en insertar contrato o detalle");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return retorna;
            }
            conn.Close();
            return retorna;
        }
        private bool edita()                                                    // actualiza cabecera y detalle
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    double tvb = 0;     // valor total de items borrados en edicion
                    // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                    // a.valor,a.acuenta,a.saldo,a.dscto
                    string actua = "update contrat set " +
                        "tipocon=@tco,tipoes=@loc,fecha=@fec,cliente=@clt,coment=@com,entrega=@ent,dentrega=@den,valor=@val," +
                        "acuenta=@acta,saldo=@sal,dscto=@dscto,clte_recoje=@cltr,seresma=@ceem,pisoent=@pise,ascensor=@asce," +
                        "pcontacto=@pecon,dreferen=@drefe,telcont=@tecont,totsad=@totadi,status=@stat,motivDes=@motdes " +
                        "where id=@idr";
                    MySqlCommand micon = new MySqlCommand(actua, conn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.Parameters.AddWithValue("@tco", tx_dat_tiped.Text);
                    micon.Parameters.AddWithValue("@loc", tx_dat_orig.Text);
                    micon.Parameters.AddWithValue("@fec", dtp_pedido.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@clt", tx_idcli.Text);
                    micon.Parameters.AddWithValue("@com", tx_coment.Text);
                    micon.Parameters.AddWithValue("@ent", dtp_entreg.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@den", tx_dirent.Text);
                    micon.Parameters.AddWithValue("@val", tx_valor.Text);
                    micon.Parameters.AddWithValue("@acta", tx_acta.Text);
                    micon.Parameters.AddWithValue("@sal", tx_saldo.Text);
                    micon.Parameters.AddWithValue("@dscto", tx_desDet.Text);      // tx_dscto.Text
                    micon.Parameters.AddWithValue("@cltr", (chk_lugent.Checked.ToString() == "True") ? "1":"0");
                    micon.Parameters.AddWithValue("@ceem", (chk_serema.Checked.ToString() == "True") ? "1" : "0");
                    micon.Parameters.AddWithValue("@pise", (tx_piso.Text.Trim() == "") ? "0" : tx_piso.Text);
                    micon.Parameters.AddWithValue("@asce", (chk_ascensor.Checked.ToString() == "True") ? "1" : "0");
                    micon.Parameters.AddWithValue("@pecon", tx_contac.Text);
                    micon.Parameters.AddWithValue("@drefe", tx_dirRef.Text);
                    micon.Parameters.AddWithValue("@tecont", tx_telcont.Text);
                    micon.Parameters.AddWithValue("@totadi", (string.IsNullOrEmpty(tx_totesp.Text)) ? "0.00" : tx_totesp.Text);
                    micon.Parameters.AddWithValue("@stat", tx_dat_estad.Text);
                    micon.Parameters.AddWithValue("@motdes", tx_motivD.Text);
                    micon.ExecuteNonQuery();
                    // detalle
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string insdet = "";
                        if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "N")   // nueva fila de detalle o actualizacion
                        {
                            insdet = "insert into detacon (" +
                                "contratoh,tipo,item,cant,nombre,medidas,madera,precio,total,saldo,coment,piedra,tda_item) values (" +
                                "@cope,@tipe,@item,@cant,@nomb,@medi,@made,@prec,@tota,@sald,@come,@pied,@tdai)";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@cope", tx_codped.Text);
                            micon.Parameters.AddWithValue("@tipe", tx_dat_orig.Text);  //  tx_dat_tiped.Text 20/09/2020
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@prec", dataGridView1.Rows[i].Cells[6].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@tota", dataGridView1.Rows[i].Cells[7].Value.ToString());
                            //micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[].Value.ToString()); // acabado debe ser
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            //micon.Parameters.AddWithValue("@cref", dataGridView1.Rows[i].Cells[10].Value.ToString());
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[11].Value.ToString());
                            micon.Parameters.AddWithValue("@pied", dataGridView1.Rows[i].Cells[13].Value.ToString());
                            micon.Parameters.AddWithValue("@tdai", dataGridView1.Rows[i].Cells[15].Value.ToString());   // tienda item
                            micon.ExecuteNonQuery();
                        }
                        if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "A")
                        {   // iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,space(1) as na
                            insdet = "update detacon set tipo=@tipe,item=@item,cant=@cant," +
                                "nombre=@nomb,medidas=@medi,madera=@made,precio=@prec,total=@tota,saldo=@sald,coment=@come,piedra=@pied," +
                                "tda_item=@tdai " +
                                "where iddetacon=@idr";
                            micon = new MySqlCommand(insdet, conn);
                            micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[0].Value.ToString());
                            micon.Parameters.AddWithValue("@tipe", tx_dat_orig.Text);   // tx_dat_tiped.Text
                            micon.Parameters.AddWithValue("@item", dataGridView1.Rows[i].Cells[1].Value.ToString());
                            micon.Parameters.AddWithValue("@cant", dataGridView1.Rows[i].Cells[2].Value.ToString());
                            micon.Parameters.AddWithValue("@nomb", dataGridView1.Rows[i].Cells[3].Value.ToString());
                            micon.Parameters.AddWithValue("@medi", dataGridView1.Rows[i].Cells[4].Value.ToString());
                            micon.Parameters.AddWithValue("@made", dataGridView1.Rows[i].Cells[5].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@prec", dataGridView1.Rows[i].Cells[6].Value.ToString());   // 
                            micon.Parameters.AddWithValue("@tota", dataGridView1.Rows[i].Cells[7].Value.ToString());
                            //micon.Parameters.AddWithValue("@esta", dataGridView1.Rows[i].Cells[].Value.ToString()); // acabado debe ser
                            micon.Parameters.AddWithValue("@sald", dataGridView1.Rows[i].Cells[8].Value.ToString());
                            //micon.Parameters.AddWithValue("@cref", dataGridView1.Rows[i].Cells[10].Value.ToString());
                            micon.Parameters.AddWithValue("@come", dataGridView1.Rows[i].Cells[11].Value.ToString());
                            micon.Parameters.AddWithValue("@pied", dataGridView1.Rows[i].Cells[13].Value.ToString());
                            micon.Parameters.AddWithValue("@tdai", dataGridView1.Rows[i].Cells[15].Value.ToString());   // tienda item
                            micon.ExecuteNonQuery();
                        }
                        if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "B")
                        {
                            tvb = tvb + (double.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString()) - double.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString()));

                            string borra = "delete from detacon where iddetacon=@idp";
                            MySqlCommand mion = new MySqlCommand(borra, conn);
                            mion.Parameters.AddWithValue("@idp", dataGridView1.Rows[i].Cells[0].Value.ToString());
                            mion.ExecuteNonQuery();
                            mion.Dispose();
                        }
                    }
                    if (!venora.Contains(tx_dat_estad.Text))
                    {
                        // actualizamos el estado del contrato
                        acciones acc = new acciones();          // ahora se usa éste actualizador 21/09/2020
                        acc.act_cont(tx_codped.Text, "");
                    }
                    micon = new MySqlCommand("select status from contrat where contrato=@cont", conn);
                    micon.Parameters.AddWithValue("@cont", tx_codped.Text);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.Read())
                    {
                        tx_dat_estad.Text = dr.GetString(0);
                    }
                    dr.Dispose();
                    // agregamos linea en pagamenti con 
                    if (tvb > 0)    // si se borraron filas
                    {
                        string paga = "insert into pagamenti (contrato,valor,acuenta,saldo,fecha,moneda,monto,montosol,detalle,usuario,dia) " +
                            "values (@cont,@valc,@acta,@sald,@fech,@mone,@mont,@mons,@deta,@usua,now())";
                        micon = new MySqlCommand(paga, conn);
                        micon.Parameters.AddWithValue("@cont", tx_codped.Text);
                        micon.Parameters.AddWithValue("@valc", tx_valor.Text);
                        micon.Parameters.AddWithValue("@acta", tx_acta.Text);
                        micon.Parameters.AddWithValue("@sald", tx_saldo.Text);
                        micon.Parameters.AddWithValue("@fech", dtp_pedido.Value.ToString("yyyy-MM-dd"));    // acá no debería ser la fecha de la modif?
                        micon.Parameters.AddWithValue("@mone", v_monLoc);
                        micon.Parameters.AddWithValue("@mont", tvb.ToString());
                        micon.Parameters.AddWithValue("@mons", tvb.ToString());
                        micon.Parameters.AddWithValue("@deta", "Modificación de contrato y saldo");
                        micon.Parameters.AddWithValue("@usua", asd);
                        micon.ExecuteNonQuery();
                    }
                    micon.Dispose();
                    retorna = true;

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en edicion");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return retorna;
            }
            conn.Close();
            return retorna;
        }
        private bool anula()                                                    // anula el contrato
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string anu = "update contrat set status=@sta,user=@asd,dia=now() " +
                    "where id=@idr";
                MySqlCommand micon = new MySqlCommand(anu, conn);
                micon.Parameters.AddWithValue("@sta", tiesan);
                micon.Parameters.AddWithValue("@asd", asd);
                micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                micon.ExecuteNonQuery();
                retorna = true;
            }
            conn.Close();
            return retorna;
        }
        private bool borra_fila(string fila)                                       // elimina de la grilla un item detalle
        {
            bool retorna = true;

            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string borra = "delete from detacon where iddetacon=@idp";
                MySqlCommand mion = new MySqlCommand(borra, conn);
                mion.Parameters.AddWithValue("@idp", fila);
                mion.ExecuteNonQuery();
                // estado del contrato
                /*string compa = "act_cont";
                mion = new MySqlCommand(compa, conn);
                mion.CommandType = CommandType.StoredProcedure;
                mion.CommandTimeout = 300;
                mion.Parameters.AddWithValue("@cont", tx_codped.Text);
                MySqlParameter estad = new MySqlParameter("@estad","");
                estad.Direction = ParameterDirection.Output;
                mion.Parameters.Add(estad);
                mion.ExecuteNonQuery();
                string newestad = mion.Parameters["@estad"].Value.ToString();
                tx_dat_estad.Text = newestad;
                cmb_estado.SelectedIndex = cmb_estado.FindString(tx_dat_estad.Text);
                for (int i = 0; i < dtg.Rows.Count; i++)
                {
                    DataRow row = dtg.Rows[i];
                    if (row[0].ToString() == tx_idr.Text)
                    {
                        dtg.Rows[i][3] = cmb_estado.SelectedItem.ToString().Substring(9, 6);
                    }
                }
                */
                retorna = true;
                conn.Close();
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor", "Error de conectividad");
            }
            conn.Close();

            return retorna;
        }
        private void actuacli()                                                 // actualiza datos del cliente
        {
            string parte = "";
            if (tx_nombre.Text != tx_nombre.Tag.ToString())
            {
                parte = parte + "razonsocial='" + tx_nombre.Text.Trim() + "'";
            }
            if (tx_direc.Text != tx_direc.Tag.ToString())
            {
                if (tx_direc.Text.Trim().Length > 100)
                {
                    if (parte == "")
                    {
                        parte = parte + "direcc1='" + tx_direc.Text.Trim().Substring(0, 99) + "',direcc2='" + tx_direc.Text.Trim().Substring(100, tx_direc.Text.Trim().Length - 100) + "'";
                    }
                    else parte = parte + ",direcc1='" + tx_direc.Text.Trim() + "',direcc2='" + tx_direc.Text.Trim().Substring(100, tx_direc.Text.Trim().Length - 100) + "'";
                }
                else
                {
                    if (parte == "") parte = parte + "direcc1='" + tx_direc.Text.Trim() + "',direcc2=''";
                    else parte = parte + ",direcc1='" + tx_direc.Text.Trim() + "',direcc2=''";
                }
            }
            if (tx_dist.Text != tx_dist.Tag.ToString())
            {
                if (parte == "") parte = parte + "localidad='" + tx_dist.Text.Trim() + "'";
                else parte = parte + ",localidad='" + tx_dist.Text.Trim() + "'";
            }
            if (tx_prov.Text != tx_prov.Tag.ToString())
            {
                if (parte == "") parte = parte + "provincia='" + tx_prov.Text.Trim() + "'";
                else parte = parte + ",provincia='" + tx_prov.Text.Trim() + "'";
            }
            if (tx_dpto.Text != tx_dpto.Tag.ToString())
            {
                if (parte == "") parte = parte + "depart='" + tx_dpto.Text.Trim() + "'";
                else parte = parte + ",depart='" + tx_dpto.Text.Trim() + "'";
            }
            if (parte == "") parte = parte + "email='" + tx_mail.Text.Trim() + "'";
            else parte = parte + ",email='" + tx_mail.Text.Trim() + "'";
            if (parte == "") parte = parte + "numerotel1='" + tx_telef1.Text.Trim() + "'";
            else parte = parte + ",numerotel1='" + tx_telef1.Text.Trim() + "'";
            if (parte == "") parte = parte + "numerotel2='" + tx_telef2.Text.Trim() + "'";
            else parte = parte + ",numerotel2='" + tx_telef2.Text.Trim() + "'";
            if (tx_dat_dpto.Text.Trim().Length == 2 && tx_dat_provin.Text.Trim().Length == 2 && tx_dat_distri.Text.Trim().Length == 2)
            {
                if (parte == "") parte = parte + "ubigeo='" + tx_dat_dpto.Text.Trim() + tx_dat_provin.Text.Trim() + tx_dat_distri.Text.Trim() + "'";
                else parte = parte + ",ubigeo='" + tx_dat_dpto.Text.Trim() + tx_dat_provin.Text.Trim() + tx_dat_distri.Text.Trim() + "'";
            }
            string actua = "";
            if (tx_idcli.Text.Trim() != "")
            {
                actua = "update anagrafiche set " + parte + " where idanagrafica=@idc";
            }
            else
            {
                actua = "insert into anagrafiche (razonsocial,direcc1,localidad,provincia,depart,email,numerotel1,numerotel2,ubigeo,pais,tipdoc,ruc,idcategoria) values (" +
                    "'" + tx_nombre.Text.Trim() + "'," + 
                    "'" + tx_direc.Text.Trim() + "'," +
                    "'" + tx_dist.Text.Trim() + "'," +
                    "'" + tx_prov.Text.Trim() + "'," +
                    "'" + tx_dpto.Text.Trim() + "'," +
                    "'" + tx_mail.Text.Trim() + "'," +
                    "'" + tx_telef1.Text.Trim() + "'," +
                    "'" + tx_telef2.Text.Trim() + "'," +
                    "'" + tx_dat_dpto.Text.Trim() + tx_dat_provin.Text.Trim() + tx_dat_distri.Text.Trim() + "'," +
                    "'" + vpaisdef + "'," +
                    "'" + tx_dat_tdoc.Text.Trim() + "'," +
                    "'" + tx_ndc.Text.Trim() + "'," +
                    "'CLI')";
            }
            if (parte != "")
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    if (tx_idcli.Text.Trim() != "")
                    {
                        MySqlCommand micon = new MySqlCommand(actua, conn);
                        micon.Parameters.AddWithValue("@idc", tx_idcli.Text);
                        micon.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand micon = new MySqlCommand(actua, conn);
                        micon.ExecuteNonQuery();
                        //
                        string jala = "select last_insert_id()";
                        micon = new MySqlCommand(jala, conn);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            tx_idcli.Text = dr.GetString(0);
                        }
                        dr.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor", "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                conn.Close();
            }
        }
        private void calculos()                                                 // calculos de total, y saldo
        {
            int v_ifm = 0;
            decimal val = 0, dsto = 0, acta = 0, espe = 0;  //sald = 0
            //decimal.TryParse(tx_desCab.Text, out desCab);
            //desCab = decimal.Parse(tx_desCab.Text);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[14].Value.ToString() != "B")    // no totaliza las filas marcadas para borrar
                {
                    //val = val + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    // val = val + decimal.Parse(dataGridView1.Rows[i].Cells[16].Value.ToString());    // 23/02/2022
                    //if (dataGridView1.Rows[i].Cells[16].Value.ToString().Trim() == "") val = val + 0;
                    //else val = val + decimal.Parse(dataGridView1.Rows[i].Cells[16].Value.ToString());   // 24/02/2022
                    /*  24/02/2023           */
                    decimal vddes = 0;
                    if (dataGridView1.Rows[i].Cells[17].Value != null) decimal.TryParse(dataGridView1.Rows[i].Cells[17].Value.ToString(), out vddes);
                    //dsto = dsto + (vddes / decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()));    // decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString());
                    dsto = dsto + vddes;

                    if (Tx_modo.Text == "NUEVO")
                    {
                        val = val + decimal.Parse(dataGridView1.Rows[i].Cells[16].Value.ToString());
                    }
                    else
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(1, 3) == "000") val = val + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString()) + vddes;
                        else val = val + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    }
                    // buscamos los codigos adicionales para acumularlos y guardarlo en el campo 
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 1) == letgru)
                    {
                        espe = espe + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    }
                    else
                    {
                        // buscamos que la madera este seleccionada
                        if (dataGridView1.Rows[i].Cells[5].Value.ToString().Trim() == "" &&
                            dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 1) != "_") v_ifm += 1;
                    }
                }
            }
            tx_totesp.Text = espe.ToString("0.00");
            tx_bruto.Text = val.ToString("0.00");
            //if (tx_dscto.Text.Trim() != "") dsto = decimal.Parse(tx_dscto.Text);
            if (Tx_modo.Text == "NUEVO")
            {
                //tx_dscto.Text = dsto.ToString("0.00");
                tx_desDet.Text = dsto.ToString("0.00");
            }
            else
            {
                //tx_dscto.Text = Math.Abs((desCab)).ToString("0.00");
                tx_desDet.Text = dsto.ToString("0.00");
            }
            if (tx_desCab.Text.Trim() == "") tx_desCab.Text = "0";
            tx_dscto.Text = (dsto + decimal.Parse(tx_desCab.Text)).ToString("#0.00");
            if (tx_acta.Text.Trim() != "") acta = decimal.Parse(tx_acta.Text);
            tx_valor.Text = (decimal.Parse(tx_bruto.Text) - decimal.Parse(tx_dscto.Text)).ToString("0.00");
            tx_saldo.Text = (decimal.Parse(tx_valor.Text) - acta).ToString("0.00");
            if (tx_dscto.Text.Trim() == "") tx_dscto.Text = "0.00";
            if (tx_acta.Text.Trim() == "") tx_acta.Text = "0.00";
            //if (tx_totesp.Text.Trim() == "") tx_totesp.Text = "0.00";
            tx_cifm.Text = v_ifm.ToString();
            motivD();
        }
        private void calculos_ex()                                                 // cambiado 24/02/2023 .. ya no se usa
        {
            int v_ifm = 0;
            decimal val = 0, dsto = 0, acta = 0, espe = 0;  //sald = 0
            //decimal.TryParse(tx_desCab.Text, out desCab);
            //desCab = decimal.Parse(tx_desCab.Text);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[14].Value.ToString() != "B")    // no totaliza las filas marcadas para borrar
                {
                    //val = val + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    val = val + decimal.Parse(dataGridView1.Rows[i].Cells[16].Value.ToString());    // 23/02/2022
                    decimal vddes = 0;
                    if (dataGridView1.Rows[i].Cells[17].Value != null) decimal.TryParse(dataGridView1.Rows[i].Cells[17].Value.ToString(), out vddes);
                    dsto = dsto + vddes;    // decimal.Parse(dataGridView1.Rows[i].Cells[17].Value.ToString());
                    // buscamos los codigos adicionales para acumularlos y guardarlo en el campo 
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 1) == letgru)
                    {
                        espe = espe + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    }
                    else
                    {
                        // buscamos que la madera este seleccionada
                        if (dataGridView1.Rows[i].Cells[5].Value.ToString().Trim() == "" &&
                            dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 1) != "_") v_ifm += 1;
                    }
                }
            }
            tx_totesp.Text = espe.ToString("0.00");
            tx_bruto.Text = val.ToString("0.00");
            //if (tx_dscto.Text.Trim() != "") dsto = decimal.Parse(tx_dscto.Text);
            if (Tx_modo.Text == "NUEVO")
            {
                //tx_dscto.Text = dsto.ToString("0.00");
                tx_desDet.Text = dsto.ToString("0.00");
            }
            else
            {
                //tx_dscto.Text = Math.Abs((desCab)).ToString("0.00");
                tx_desDet.Text = dsto.ToString("0.00");
            }
            if (tx_desCab.Text.Trim() == "") tx_desCab.Text = "0";
            tx_dscto.Text = (dsto + decimal.Parse(tx_desCab.Text)).ToString("#0.00");
            if (tx_acta.Text.Trim() != "") acta = decimal.Parse(tx_acta.Text);
            tx_valor.Text = (decimal.Parse(tx_bruto.Text) - decimal.Parse(tx_dscto.Text)).ToString("0.00");
            tx_saldo.Text = (decimal.Parse(tx_valor.Text) - acta).ToString("0.00");
            if (tx_dscto.Text.Trim() == "") tx_dscto.Text = "0.00";
            if (tx_acta.Text.Trim() == "") tx_acta.Text = "0.00";
            //if (tx_totesp.Text.Trim() == "") tx_totesp.Text = "0.00";
            tx_cifm.Text = v_ifm.ToString();
            motivD();
        }
        private bool valexist(String docu)                                      // valida existencia de documento
        {
            bool retorna = true;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                string consulta = "select count(*) from contrat where contrato=@doc";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@doc", docu);
                MySqlDataReader dr = micon.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        if (dr.GetInt16(0) > 0) retorna = true;
                        else retorna = false;
                    }
                    dr.Close();
                }
            }
            conn.Close();
            return retorna;
        }
        string[] equivinter(string titulo)                                      // equivalencia entre titulo de columna y tabla 
        {
            string[] retorna = new string[2];
            switch (titulo)
            {
                case "NIVEL":
                    retorna[0] = "desc_niv";
                    retorna[1] = "codigo";
                    break;
                case "???":
                    retorna[0] = "";
                    retorna[1] = "";
                    break;
                case "????":
                    retorna[0] = "";
                    retorna[1] = "";
                    break;
                case "LOCAL":
                    retorna[0] = "desc_alm";
                    retorna[1] = "idcodice";
                    break;
                case "TIENDA":
                    retorna[0] = "desc_ven";
                    retorna[1] = "idcodice";
                    break;
                case "SEDE":
                    retorna[0] = "desc_loc";
                    retorna[1] = "idcodice";
                    break;
                case "RUC":
                    retorna[0] = "desc_raz";
                    retorna[1] = "idcodice";
                    break;
            }
            return retorna;
        }
        private void tabuser_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = true;
            bt_prev.Enabled = true;
            bt_exc.Enabled = false;
            if (Tx_modo.Text != "NUEVO" && Tx_modo.Text != "EDITAR")
            {
                pan_cli.Enabled = false;
                chk_cliente.Checked = false;
                label13.ForeColor = Color.White;
                chk_cliente.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label19.ForeColor = Color.White;
                label18.ForeColor = Color.White;
                label17.ForeColor = Color.White;
                label15.ForeColor = Color.White;
                label16.ForeColor = Color.White;
            }
            else
            {
                motivD();
            }
        }
        private void motivD()                                                   // habilita o no el campo tx_motivD si hay descuento
        {
            if ("NUEVO,EDITAR".Contains(Tx_modo.Text) == true)
            {
                if (tx_dscto.Text.Trim() != "")     // habilitamos la escritura si el contrato tiene descuento
                {
                    if (double.Parse(tx_dscto.Text) > 0) tx_motivD.ReadOnly = false;
                    else
                    {
                        tx_motivD.Text = "";
                        tx_motivD.ReadOnly = true;
                    }
                }
                else
                {
                    tx_motivD.Text = "";
                    tx_motivD.ReadOnly = true;
                }
            }
        }
        private void tabgrilla_Enter(object sender, EventArgs e)
        {
            Bt_anul.Enabled = false;
            Bt_print.Enabled = false;
            bt_prev.Enabled = false;
            bt_exc.Enabled = true;
        }
        private void tx_d_nom_Enter(object sender, EventArgs e)
        {
            tx_d_nom.ReadOnly = true;
            if(cmb_mod.Text == "000" && ("NUEVO,EDITAR").Contains(Tx_modo.Text))
            {
                tx_d_nom.ReadOnly = false;
            }
        }
        private void cmb_tip_Enter(object sender, EventArgs e)
        {
            /*if (("NUEVO,EDITAR").Contains(Tx_modo.Text) && cmb_mod.Text == "000")
            {
                cmb_tip.Enabled = false;
            }
            else cmb_tip.Enabled = true; */
        }
        private void cmb_det1_Enter(object sender, EventArgs e)
        {
            /*if (("NUEVO,EDITAR").Contains(Tx_modo.Text) && cmb_mod.Text == "000") cmb_det1.Enabled = false;
            else cmb_det1.Enabled = true; */
        }
        private void cmb_aca_Enter(object sender, EventArgs e)
        {
            /*if (("NUEVO,EDITAR").Contains(Tx_modo.Text) && cmb_mod.Text == "000") cmb_aca.Enabled = false;
            else cmb_aca.Enabled = true;*/
        }
        private void cmb_det2_Enter(object sender, EventArgs e)
        {
            /*if (("NUEVO,EDITAR").Contains(Tx_modo.Text) && cmb_mod.Text == "000") cmb_det2.Enabled = false;
            else cmb_det2.Enabled = true;*/
        }
        private void cmb_det3_Enter(object sender, EventArgs e)
        {
            /*if (("NUEVO,EDITAR").Contains(Tx_modo.Text) && cmb_mod.Text == "000") cmb_det3.Enabled = false;
            else cmb_det3.Enabled = true;*/
        }
        #endregion

        #region autocompletados
        private void autodptos()
        {
            DataRow[] result = dtadpd.Select("provin='00' AND distri='00'");
            foreach (DataRow row in result)
            {
                adptos.Add(row["nombre"].ToString());
            }
            tx_dpto.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_dpto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_dpto.AutoCompleteCustomSource = adptos;
        }
        private void autoprovi()
        {
            aprovi.Clear();
            DataRow[] result = dtadpd.Select("distri='00' AND depart='" + tx_dat_dpto.Text + "'");  // provin<>'00' AND 
            foreach (DataRow row in result)
            {
                aprovi.Add(row["nombre"].ToString());
            }
            tx_prov.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_prov.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_prov.AutoCompleteCustomSource = aprovi;
        }
        private void autodistr()
        {
            adistr.Clear();
            DataRow[] result;
            if (tx_dat_dpto.Text == "07")   // callao
            {
                result = dtadpd.Select("provin='01' AND depart='" + tx_dat_dpto.Text + "'");  // AND distri='00' 
            }
            else
            {
                result = dtadpd.Select("provin='" + tx_dat_provin.Text + "' AND depart='" + tx_dat_dpto.Text + "'");  // AND distri='00' 
            }
            foreach (DataRow row in result)
            {
                adistr.Add(row["nombre"].ToString());
            }
            tx_dist.AutoCompleteMode = AutoCompleteMode.Suggest;
            tx_dist.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tx_dist.AutoCompleteCustomSource = adistr;
        }
        #endregion autocompletados

        #region botones_de_comando_y_permisos  
        private void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            bt_view.Visible = false;
            Bt_print.Visible = false;
            bt_exc.Visible = false;
            bt_prev.Visible = false;
            //
            DataTable mdtb = new DataTable();
            const string consbot = "select * from permisos where formulario=@nomform and usuario=@use";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlCommand consulb = new MySqlCommand(consbot, conn);
                    consulb.Parameters.AddWithValue("@nomform", nomform);
                    consulb.Parameters.AddWithValue("@use", asd);
                    MySqlDataAdapter mab = new MySqlDataAdapter(consulb);
                    mab.Fill(mdtb);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return;
                }
                finally { conn.Close(); }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (mdtb.Rows.Count > 0)
            {
                DataRow row = mdtb.Rows[0];
                if (Convert.ToString(row["btn1"]) == "S")               // nuevo
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")               // editar
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")               // ANULAR
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")               // VISUALIZAR
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir
                {
                    this.Bt_print.Visible = true;
                }
                else { Bt_print.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")               // salir del form
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
                if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar
                {
                    bt_prev.Visible = true;
                }
                else { this.bt_prev.Visible = false; }
                if (Convert.ToString(row["btn8"]) == "S")               // exporta xlsx
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
            }
        }
        private void Bt_add_Click(object sender, EventArgs e)
        {
            desCab = 0;                                 // inicializamos descuento global
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            tabControl1.SelectedTab = tabuser;
            escribe(this);
            escribepag(tabuser);
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            dtp_pedido.Value = DateTime.Now;
            dtp_entreg.Checked = true;
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            //grilladet("NUEVO");
            tabControl1.SelectedTab = tabuser;
            //
            pan_cli.Enabled = true;
            cmb_tdoc.Enabled = true;
            tx_ndc.Enabled = true;
            tx_nombre.Enabled = false;
            tx_direc.Enabled = false;
            tx_dist.Enabled = false;
            tx_prov.Enabled = false;
            tx_dpto.Enabled = false;
            tx_mail.Enabled = false;
            tx_telef1.Enabled = false;
            tx_telef2.Enabled = false;
            tx_valor.ReadOnly = true;
            //
            tx_dat_tiped.Text = tipede;
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_estad.Text = tiesta;
            cmb_estado.SelectedIndex = cmb_estado.FindString(tiesta);
            cmb_estado.Enabled = false;
            tx_idr.ReadOnly = true;
            tx_rind.ReadOnly = true;
            tx_valor.ReadOnly = true;
            tx_saldo.ReadOnly = true;
            tx_d_saldo.ReadOnly = true;
            tx_a_codig.ReadOnly = true;
            tx_a_salcan.ReadOnly = true;
            cmb_tipo.Enabled = false;
            if (vapm.Contains(tx_dat_orig.Text)) tx_acta.ReadOnly = false;
            else tx_acta.ReadOnly = true;
            /*
            if (tncont == "AUTOMA")
            {
                tx_codped.ReadOnly = true;
                if (tx_dat_orig.Text != "") correNum();
            }
            else tx_codped.ReadOnly = false;
            */
            tx_codped.ReadOnly = true;
            if (int.Parse(iOMG.Program.nivuser) < 2) cmb_taller.Focus();
            else
            {
                tx_dat_orig.Text = iOMG.Program.tdauser;
                cmb_taller.SelectedIndex = cmb_taller.FindString(tx_dat_orig.Text);     // local de venta
                cmb_taller_SelectionChangeCommitted(null,null);
                cmb_taller.Enabled = false;
                cmb_tdoc.Focus();
            }
            rb_bienes.PerformClick();        // rb_bienes.Checked = true;           // por defecto, bienes
            // 
            tx_mc.Visible = true;
            tx_serie.Visible = true;
            tx_corre.Visible = true;
            //
            _comprobantes.Clear();
            motivD();
            grilladet("NUEVO");
            color_filas();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            desCab = 0;                                 // inicializamos descuento global
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            tabControl1.SelectedTab = tabuser;
            Tx_modo.Text = "EDITAR";
            sololee(this);
            sololeepag(tabuser);
            button1.Image = Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            tx_codped.Focus();
            tabControl1.SelectedTab = tabuser;
            //
            pan_cli.Enabled = true;
            chk_cliente.Enabled = true;
            cmb_tdoc.Enabled = false;
            tx_ndc.Enabled = false;
            tx_nombre.Enabled = false;
            tx_direc.Enabled = false;
            tx_dist.Enabled = false;
            tx_prov.Enabled = false;
            tx_dpto.Enabled = false;
            tx_mail.Enabled = false;
            tx_telef1.Enabled = false;
            tx_telef2.Enabled = false;
            //
            cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            tx_dat_tiped.Text = tipede;
            tx_codped.Enabled = true;
            tx_codped.ReadOnly = false;
            tx_acta.Enabled = true;
            tx_coment.Enabled = true;
            tx_saldo.ReadOnly = true;
            tx_acta.ReadOnly = true;
            tx_a_codig.ReadOnly = true;
            tx_a_salcan.ReadOnly = true;
            tx_codped.Focus();
            // 
            tx_mc.Visible = false;
            tx_serie.Visible = false;
            tx_corre.Visible = false;
            rb_bienes.PerformClick();            // por defecto, bienes
            //
            _comprobantes.Clear();
            color_filas();
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            desCab = 0;                                 // inicializamos descuento global
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            tabControl1.SelectedTab = tabuser;
            Tx_modo.Text = "ANULAR";
            sololee(this);
            sololeepag(tabuser);
            button1.Image = Image.FromFile(img_anul);
            limpiar(this);
            limpiapag(tabuser);
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            tx_codped.Enabled = true;
            tx_codped.ReadOnly = false;
            tx_codped.Focus();
            tabControl1.SelectedTab = tabuser;
            //
            pan_cli.Enabled = false;
            chk_cliente.Enabled = false;
            //
            tx_codped.Enabled = true;
            tx_codped.Focus();
            // 
            tx_mc.Visible = false;
            tx_serie.Visible = false;
            tx_corre.Visible = false;
            //
            _comprobantes.Clear();
            color_filas();
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            desCab = 0;                                 // inicializamos descuento global
            tabControl1.Enabled = true;
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            /*
            string codu = "";
            string idr = "";
            if (advancedDataGridView1.CurrentRow.Index > -1)
            {
                codu = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            */
            tabControl1.SelectedTab = tabgrilla;
            sololee(this);
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = null;    // Image.FromFile(img_grab);
            limpiar(this);
            limpiapag(tabuser);
            sololeepag(tabuser);
            tx_codped.Enabled = true;
            limpia_otros(tabuser);
            limpia_combos(tabuser);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            //cmb_tipo.SelectedIndex = cmb_tipo.FindString(tipede);
            //tx_dat_tiped.Text = tipede;
            //jalaoc("tx_idr");
            tx_codped.Enabled = true;
            tx_codped.ReadOnly = false;
            tx_codped.Focus();
            tabControl1.SelectedTab = tabuser;
            //
            pan_cli.Enabled = false;
            label13.ForeColor = Color.White;
            chk_cliente.ForeColor = Color.White;
            label14.ForeColor = Color.White;
            label19.ForeColor = Color.White;
            label18.ForeColor = Color.White;
            label17.ForeColor = Color.White;
            label15.ForeColor = Color.White;
            label16.ForeColor = Color.White;
            chk_cliente.Enabled = false;
            //
            tx_codped.Enabled = true;
            tx_codped.Focus();
            // 
            tx_mc.Visible = false;
            tx_serie.Visible = false;
            tx_corre.Visible = false;
            //
            _comprobantes.Clear();
            color_filas();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            //setParaCrystal();
            if (impDef == "")
            {
                PrinterSettings setPrintD = new PrinterSettings();
                impDef = setPrintD.PrinterName;
            }
            PrintReport(Application.StartupPath + "\\ContratoI.rpt", impDef, 1);  // "CutePDFWriter" 
            //PrintReport(Application.StartupPath + "\\resumen_termYcond.rpt", impDef, 2);  // ya no va 14/02/2023
        }
        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (tx_idr.Text != "" || tx_rind.Text != "")    // &&
            {
                setParaCrystal();
            }
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            string nombre = "";
            nombre = "Contratos_clientes_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                var wb = new XLWorkbook();
                wb.Worksheets.Add(dtg, "Contratos");
                wb.SaveAs(nombre);
                MessageBox.Show("Archivo generado con exito!");
                this.Close();
            }
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //
        #endregion botones_de_comando_y_permisos  ;

        #region limpiadores_modos
        private void sololee(Form lfrm)
        {
            foreach (Control oControls in lfrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = false;
                }
            }
        }
        private void sololeepag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = false;
                }
            }
            foreach (Control oControls in tabcodigo.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = false;
                }
            }
            foreach (Control oControls in tabadicion.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = false;
                }
            }
        }
        private void escribe(Form efrm)
        {
            foreach (Control oControls in efrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
            }
            //
            cmb_tip.Enabled = false;
            cmb_det1.Enabled = false;
            cmb_aca.Enabled = false;
            cmb_det2.Enabled = false;
            cmb_det3.Enabled = false;
            cmb_tal.Enabled = false;
            //
            tx_acta.ReadOnly = true;
            tx_saldo.ReadOnly = true;
            tx_valor.ReadOnly = true;
            tx_bruto.ReadOnly = true;
            //
            tx_d_it.ReadOnly = true;
            tx_d_id.ReadOnly = true;
            tx_d_saldo.ReadOnly = true;
            //
            tx_a_id.ReadOnly = true;
            tx_a_codig.ReadOnly = true;
            tx_a_total.ReadOnly = true;
            //
            tx_idr.ReadOnly = true;
            tx_rind.ReadOnly = true;
            tx_verCont.ReadOnly = true;
        }
        private void escribepag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = true;
                }
            }
            foreach (Control oControls in tabcodigo.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = true;
                }
            }
            foreach (Control oControls in tabadicion.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                    TextBox tb = oControls as TextBox;
                    tb.ReadOnly = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = true;
                }
            }
            //
            cmb_tip.Enabled = false;
            cmb_det1.Enabled = false;
            cmb_aca.Enabled = false;
            cmb_det2.Enabled = false;
            cmb_det3.Enabled = false;
            cmb_tal.Enabled = false;
            cmb_mod.Enabled = false;
            // 
            tx_acta.ReadOnly = true;
            tx_saldo.ReadOnly = true;
            tx_valor.ReadOnly = true;
            tx_bruto.ReadOnly = true;
            //
            tx_d_it.ReadOnly = true;
            tx_d_id.ReadOnly = true;
            tx_d_saldo.ReadOnly = true;
            //
            tx_a_id.ReadOnly = true;
            tx_a_codig.ReadOnly = true;
            tx_a_total.ReadOnly = true;
            //
            tx_idr.ReadOnly = true;
            tx_rind.ReadOnly = true;
            tx_verCont.ReadOnly = true;
        }
        private static void limpiar(Form ofrm)
        {
            foreach (Control oControls in ofrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        private void limpiapag(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
            foreach (Control oControls in pan_cli.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
            foreach (Control oControls in tabcodigo.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
            foreach (Control oControls in tabadicion.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
            foreach (Control oControls in pan_tcon.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        private void limpia_chk()
        {
            chk_ascensor.Checked = false;
            chk_lugent.Checked = false;
            chk_serema.Checked = false;
        }
        private void limpia_otros(TabPage pag)
        {
            tabControl1.SelectedTab = pag;
            //this.checkBox1.Checked = false;
        }
        private void limpia_combos(TabPage pag)
        {
            //tabControl1.SelectedTab = pag;
            cmb_tipo.SelectedIndex = -1;
            cmb_taller.SelectedIndex = -1;
            cmb_estado.SelectedIndex = -1;
            cmb_tdoc.SelectedIndex = -1;
            cmb_fam.SelectedIndex = -1;
            cmb_mod.SelectedIndex = -1;
            cmb_mad.SelectedIndex = -1;
            cmb_tip.SelectedIndex = -1;
            cmb_det1.SelectedIndex = -1;
            cmb_aca.SelectedIndex = -1;
            cmb_tal.SelectedIndex = -1;
            cmb_det2.SelectedIndex = -1;
            cmb_det3.SelectedIndex = -1;
        }
        #endregion limpiadores_modos;

        #region comboboxes
        private void cmb_estado_Enter(object sender, EventArgs e)
        {
            cmb_estado.Tag = cmb_estado.SelectedIndex;
        }
        private void cmb_estado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (rb_bienes.Checked == true)
            {
                if (cmb_estado.SelectedValue != null) tx_dat_estad.Text = cmb_estado.SelectedValue.ToString();
                else tx_dat_estad.Text = cmb_estado.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
                var aaa = MessageBox.Show("Confirma que desea cambiar el estado del contrato?" + Environment.NewLine +
                    "no es una acción recomendada, el estado cambia de forma automática", "Alerta de procedimiento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (aaa == DialogResult.No)
                {
                    cmb_estado.SelectedIndex = int.Parse(cmb_estado.Tag.ToString());
                }
            }
            if (rb_servi.Checked == true)
            {
                if (!vucecs.Contains(asd))
                {
                    MessageBox.Show("No esta autorizado a realizar este cambio","Error en permisos",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    cmb_estado.SelectedIndex = int.Parse(cmb_estado.Tag.ToString());
                }
            }
        }
        private void cmb_taller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_taller.SelectedValue != null) tx_dat_orig.Text = cmb_taller.SelectedValue.ToString();
            else tx_dat_orig.Text = cmb_taller.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
            //if (tncont == "AUTOMA" && tx_dat_orig.Text != "") correNum();
        }
        private void cmb_tdoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_tdoc.SelectedIndex == -1) tx_dat_tdoc.Text = "";
            else
            {
                foreach (DataRow row in dtdest.Rows)
                {
                    if (row["descrizionerid"].ToString() == cmb_tdoc.Text)   // tx_dat_tdoc.Text
                    {
                        tx_dat_tdoc.Text = row["idcodice"].ToString();
                    }
                }
            }
        }
        private void cmb_cap_SelectionChangeCommitted(object sender, EventArgs e)       // tipo contrato 1=normal o 2=especial
        {
            if (cmb_tipo.SelectedValue != null) tx_dat_tiped.Text = cmb_tipo.SelectedValue.ToString().Substring(0,1);
            else tx_dat_tiped.Text = cmb_tipo.SelectedItem.ToString().PadRight(6).Substring(0, 1).Trim();
        }
        private void cmb_fam_SelectionChangeCommitted(object sender, EventArgs e)       // capitulo familia
        {
            armani();
        }
        private void cmb_mod_SelectionChangeCommitted(object sender, EventArgs e)       // modelo
        {
            armani();
        }
        private void cmb_mad_SelectionChangeCommitted(object sender, EventArgs e)       // madera
        {
            if (cmb_mad.SelectedItem != null)
            {
                tx_d_mad.Text = cmb_mad.SelectedItem.ToString().Substring(0, 1);
                tx_dat_mad.Text = cmb_mad.SelectedItem.ToString().Substring(4, cmb_mad.SelectedItem.ToString().Length - 4).Trim();
                armani();
            }
        }
        private void cmb_tip_SelectedIndexChanged(object sender, EventArgs e)           // tipologia
        {
            armani();
        }
        private void cmb_det1_SelectionChangeCommitted(object sender, EventArgs e)      // detalle1
        {
            armani();
        }
        private void cmb_aca_SelectionChangeCommitted(object sender, EventArgs e)       // acabado
        {
            tx_d_est.Text = cmb_aca.SelectedItem.ToString().Substring(5, cmb_aca.SelectedItem.ToString().Length - 5).Trim();
            cmb_aca.Tag = cmb_aca.SelectedItem.ToString().Substring(0, 1);
            armani();
        }
        private void cmb_tal_SelectedIndexChanged(object sender, EventArgs e)           // taller
        {
            armani();
        }
        private void cmb_det2_SelectionChangeCommitted(object sender, EventArgs e)      // detalle 2
        {
            if (cmb_det2.SelectedIndex == -1) tx_d_det2.Text = "";
            else tx_d_det2.Text = cmb_det2.SelectedItem.ToString().Substring(6, cmb_det2.SelectedItem.ToString().Length - 6).Trim();
            armani();
        }
        private void cmb_det3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            armani();
        }
        #endregion comboboxes

        #region boton_form GRABA EDITA ANULA - agrega detalle
        private void button1_Click(object sender, EventArgs e)
        {
            // validamos que los campos no esten vacíos
            string modos = "NUEVO,EDITAR";
            if (modos.Contains(Tx_modo.Text))
            {
                motivD();       // ojeamos el asunto del dscto y su comentario 27/02/2023
                calculos();     // verificamos los totales, maderas y demas 27/02/2023
                if (tx_motivD.ReadOnly == false && tx_motivD.Text.Trim() == "")
                {
                    MessageBox.Show("Debe ingresar el motivo del descuento","Atención, es ogligatorio",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    tx_motivD.Focus();
                    return;
                }
                if (tx_dat_tiped.Text == "")
                {
                    MessageBox.Show("Seleccione el tipo de contrato", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_tipo.Focus();
                    return;
                }
                if (tx_dat_estad.Text == "")
                {
                    MessageBox.Show("Seleccione el estado del contrato", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_estado.Focus();
                    return;
                }
                if (tx_dat_orig.Text == "")
                {
                    MessageBox.Show("Seleccione el local de ventas", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_taller.Focus();
                    return;
                }
                if (tx_ndc.Text == "")
                {
                    MessageBox.Show("Falta el cliente", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_ndc.Focus();
                    return;
                }
                if (tx_nombre.Text == "")
                {
                    MessageBox.Show("Falta el nombre del cliente", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_nombre.Focus();
                    return;
                }
                if (dataGridView1.Rows.Count < 2)
                {
                    MessageBox.Show("Falta el detalle del contrato", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    cmb_fam.Focus();
                    return;
                }
                if (tx_mail.Text.Trim() != "" && lib.email_bien_escrito(tx_mail.Text.Trim()) == false)
                {
                    MessageBox.Show("Debe arreglar el correo electrónico" + Environment.NewLine +
                        "porque no cumple con el formato", "Atención verifique", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    tx_mail.Focus();
                    return;
                }
                /*  NUm contratos automáticos al momento de grabar
                if (tx_codped.Text.Trim() == "")    // tncont == "MANUAL" && 01/11/2020
                {
                    MessageBox.Show("Ingrese el identificador del contrato", "Atención - verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_codped.Focus();
                    return;
                }
                */
                // falta validar que tengan madera y demas cada fila
                if (tx_cifm.Text.Trim() != "0")
                {
                    MessageBox.Show("Falta detallar madera", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView1.Focus();
                    return;
                }
                if (decimal.Parse(tx_saldo.Text.ToString()) < 0)
                {
                    MessageBox.Show("El saldo es negativo, el pago debe ser inferior o igual al valor del contrato","Atención",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    tx_acta.Focus();
                    return;
                }
                if (double.Parse(tx_valor.Text) >= double.Parse(limdet) && bus_detalle_detrac() == false)      // buscamos codigos diferentes en el detalle del contrato de servicios
                {
                    MessageBox.Show("El contrato tiene detracción y códigos diferentes","Atención",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    dataGridView1.Focus();
                    return;
                }
                if (MessageBox.Show("Confirma que la FECHA DE ENTREGA" + Environment.NewLine +
                    "es la correcta?","Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    dtp_entreg.Focus();
                    return;
                }
                if (tx_telef1.Text.Trim() == "" && tx_telef2.Text.Trim() == "")
                {
                    MessageBox.Show(" Por favor, ingrese al menos " + Environment.NewLine +
                        " un número de teléfono ","Atención",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    return;
                }
            }
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            string asd = iOMG.Program.vg_user;
            string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            //
            actuacli();                 // actualiza datos del cliente
            //
            if (modo == "NUEVO")
            {
                var aa = MessageBox.Show("Confirma que desea crear el contrato?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (valexist(tx_codped.Text.Trim()) == true)
                    {
                        // true = documento existe
                        // false = documento no existe
                        MessageBox.Show("El identificador de contrato YA existe!", "Por favor corrija", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tx_codped.Focus();
                        return;
                    }
                    if (graba() == true)
                    {
                        // insertamos en el datatable
                        DataRow dr = dtg.NewRow();
                        // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                        // a.valor,a.acuenta,a.saldo,a.dscto,a.clte_recoje,a.seresma,a.pisoent,a.ascensor,a.pcontacto,a.dreferen,telcont,totsad
                        string cid = tx_idr.Text;
                        dr[0] = cid;
                        dr[1] = tx_dat_tiped.Text;
                        dr[2] = tx_codped.Text;
                        dr[3] = cmb_estado.SelectedItem.ToString().Substring(9, 6);
                        dr[4] = tx_dat_orig.Text;
                        dr[5] = dtp_pedido.Value.ToString("yyy-MM-dd");
                        dr[6] = tx_idcli.Text;                                          // *
                        dr[7] = tx_nombre.Text;                                         // *
                        dr[8] = tx_coment.Text;
                        dr[9] = dtp_entreg.Value.ToString("yyy-MM-dd");
                        dr[10] = tx_dirent.Text;
                        dr[11] = tx_valor.Text;
                        dr[12] = tx_acta.Text;
                        dr[13] = tx_saldo.Text;
                        dr[14] = tx_dscto.Text;
                        dr[15] = (chk_lugent.Checked.ToString() == "True") ? "1" : "0";    // cliente recoje en tienda
                        dr[16] = (chk_serema.Checked.ToString() == "true") ? "1" : "0";
                        dr[17] = (tx_piso.Text.Trim().Length == 0) ? "0" : tx_piso.Text;
                        dr[18] = (chk_ascensor.Checked.ToString() == "true") ? "1" : "0";
                        dr[19] = tx_contac.Text;
                        dr[20] = tx_dirRef.Text;
                        dr[21] = tx_telcont.Text;
                        dr[22] = tx_totesp.Text;
                        dr[23] = tx_motivD.Text;        // motivDes
                        dr[24] = "";                    // articulo --> contrato madre de bienes si este contrato es servicios
                        dr[25] = "2";                   // marca0
                        dr[26] = (tx_desCab.Text.Trim() == "") ? "0" : tx_desCab.Text;    // dsctocab
                        dtg.Rows.Add(dr);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo grabar el contrato", "Error en crear", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    // vista previa   setParaCrystal();
                    Bt_print.PerformClick();
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                var aa = MessageBox.Show("Confirma que desea MODIFICAR el contrato?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (edita() == true)
                    {
                        // actualizamos el datatable
                        for (int i = 0; i < dtg.Rows.Count; i++)
                        {
                            DataRow row = dtg.Rows[i];
                            if (row[0].ToString() == tx_idr.Text)
                            {
                                // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                                // a.valor,a.acuenta,a.saldo,a.dscto,a.pcontacto,a.dreferen
                                dtg.Rows[i][3] = tx_dat_estad.Text; // cmb_estado.SelectedText.ToString();
                                dtg.Rows[i][4] = tx_dat_orig.Text;  // cmb_taller.SelectedText.ToString();
                                dtg.Rows[i][5] = dtp_pedido.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][6] = tx_idcli.Text;
                                dtg.Rows[i][7] = tx_nombre.Text;
                                dtg.Rows[i][8] = tx_coment.Text;
                                dtg.Rows[i][9] = dtp_entreg.Value.ToString("yyyy-MM-dd");
                                dtg.Rows[i][10] = tx_dirent.Text;
                                dtg.Rows[i][11] = tx_valor.Text;
                                dtg.Rows[i][12] = tx_acta.Text;
                                dtg.Rows[i][13] = tx_saldo.Text;
                                dtg.Rows[i][14] = tx_dscto.Text;
                                dtg.Rows[i][15] = (chk_lugent.Checked.ToString() == "True") ? "1" : "0";
                                dtg.Rows[i][16] = (chk_serema.Checked.ToString() == "True") ? "1" : "0";
                                dtg.Rows[i][17] = tx_piso.Text;
                                dtg.Rows[i][18] = (chk_ascensor.Checked.ToString() == "True") ? "1" : "0";
                                dtg.Rows[i][19] = tx_contac.Text;
                                dtg.Rows[i][20] = tx_dirRef.Text;
                                dtg.Rows[i][21] = tx_telcont.Text;
                                dtg.Rows[i][22] = tx_totesp.Text;
                                dtg.Rows[i][23] = tx_motivD.Text;        // motivDes
                                dtg.Rows[i][24] = "";                    // articulo --> contrato madre de bienes si este contrato es servicios
                                //dtg.Rows[i][25] = "";                   // marca0
                                //dr[26] = (tx_desCab.Text.Trim() == "") ? "0" : tx_desCab.Text;    // dsctocab
                            }
                        }
                        // el estado es anulado??
                        if (tx_dat_estad.Text == tiesan)
                        {
                            // actualizamos el datatable
                            for (int i = 0; i < dtg.Rows.Count; i++)
                            {
                                DataRow row = dtg.Rows[i];
                                if (row[0].ToString() == tx_idr.Text)
                                {
                                    row.Delete();
                                }
                            }
                            dtg.AcceptChanges();
                        }
                    }
                }
                else
                {
                    cmb_tipo.Focus();
                    return;
                }
            }
            if (modo == "ANULAR")       // opción para borrar o anular, NO ESTA HABILITADO, SE USA EDICION
            {
                // si el contrato no tiene movimientos o enlaces se puede borrar
                // si tiene mov. enlaces, etc. solo se puede anular
                var aa = MessageBox.Show("Confirma que desea ANULAR el contrato?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    if (tiesta == tx_dat_estad.Text)
                    {
                        if(anula() != true)
                        {
                            MessageBox.Show("No se realizo la operacion de anular", "Error en anular");
                            return;
                        }
                    }
                    else
                    {
                        var aaa = MessageBox.Show("El estado del contrato no permite anular" + Environment.NewLine +
                            "Confirma que desea ANULAR de todas formas?", "Atención - Confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(aaa == DialogResult.Yes)
                        {
                            if (anula() != true)
                            {
                                MessageBox.Show("No se realizo la operacion de anular", "Error en anular");
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    // actualizamos el datatable
                    for (int i = 0; i < dtg.Rows.Count; i++)
                    {
                        DataRow row = dtg.Rows[i];
                        if (row[0].ToString() == tx_idr.Text)
                        {
                            // a.id,a.tipocon,a.contrato,a.STATUS,a.tipoes,a.fecha,a.cliente,b.razonsocial,a.coment,a.entrega,a.dentrega,
                            // a.valor,a.acuenta,a.saldo,a.dscto
                            //dtg.Rows[i][3] = tiesan; // cmb_estado.SelectedText.ToString();
                            row.Delete();
                        }
                    }
                    dtg.AcceptChanges();
                }
            }
            if (iserror == "no")
            {
                // debe limpiar los campos y actualizar la grilla
                desCab = 0;                                 // inicializamos descuento global
                limpiar(this);
                limpiapag(tabuser);
                limpia_otros(tabuser);
                limpia_combos(tabuser);
                limpia_chk();
                dtp_entreg.Value = DateTime.Now;
                dtp_pedido.Value = DateTime.Now;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                cmb_tipo.Focus();
            }
        }
        private void bt_det_Click(object sender, EventArgs e)
        {
            if (tx_d_nom.Text == "")
            {
                MessageBox.Show("El código no existe en la maestra", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (tx_d_can.Text == "")
            {
                MessageBox.Show("Falta la cantidad de muebles", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_can.Focus();
                return;
            }
            if (tx_d_saldo.Text.Trim() == "")
            {
                MessageBox.Show("Falta el saldo de muebles", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_saldo.Focus();
                return;
            }
            if (cmb_det3.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 3", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det3.Focus();
                return;
            }
            if (cmb_det2.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 2", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det2.Focus();
                return;
            }
            if (cmb_tal.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el taller", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_tal.Focus();
                return;
            }
            if (tx_d_est.Text.Trim() == "")    // cmb_aca.SelectedIndex == -1
            {
                MessageBox.Show("Seleccione el acabado", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_aca.Focus();
                return;
            }
            if (cmb_det1.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el detalle 1", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_det1.Focus();
                return;
            }
            if (cmb_tip.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la tipología", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_tip.Focus();
                return;
            }
            if (tx_d_mad.Text.Trim() == "")   //cmb_mad.SelectedIndex == -1
            {
                MessageBox.Show("Seleccione la madera", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mad.Focus();
                return;
            }
            if (cmb_mod.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el modelo", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mod.Focus();
                return;
            }
            if (cmb_fam.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el capitulo o familia", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_fam.Focus();
                return;
            }
            // valida que no existan X en madera y acabado, y no exista XX en taller
            if (cmb_mad.SelectedItem.ToString().Substring(0, 1) == "X" && cmb_fam.Text.Substring(0,1) != vtasc) // articulos chicos no van madera
            {
                MessageBox.Show("Seleccione un tipo de madera valido", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_mad.Focus();
                return;
            }
            if (cmb_aca.SelectedItem.ToString().Substring(0, 1) == "X")
            {
                MessageBox.Show("Seleccione el acabado correcto", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cmb_aca.Focus();
                return;
            }
            if (tx_d_prec.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el precio", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_prec.Focus();
                return;
            }
            else
            {
                if(decimal.Parse(tx_d_prec.Text) < 1)
                {
                    MessageBox.Show("Ingrese un precio mayor a cero", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_d_prec.Focus();
                    return;
                }
            }
            if (tx_d_total.Text.Trim() == "")
            {
                MessageBox.Show("Falta calcular el total", "Faltan datos!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_d_can.Focus();
                return;
            }
            if (tx_d_id.Text.Trim() == "")      //  .. NUEVO O EDICION, NO SE PERMITE REPETIR CODIGOS
            {              // validamos que el codigo no se repita en la grilla, REUNIÓN 10-10-2020 CON GLORIA
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (tx_d_codi.Text == dataGridView1.Rows[i].Cells[1].Value.ToString())
                    {
                        MessageBox.Show("Esta repitiendo el código del artículo","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        cmb_fam.Focus();
                        return;
                    }
                }
            }
            // fin de las validaciones de X
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_d_id.Text.Trim() != "")
                {
                    // a.iddetacon,a.item,a.cant,a.nombre,a.medidas,a.madera,a.precio,a.total,a.saldo,a.pedido,a.codref,a.coment,
                    //piedra,codpie,na
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                    obj.Cells[1].Value = tx_d_codi.Text;
                    obj.Cells[2].Value = tx_d_can.Text;
                    obj.Cells[3].Value = tx_d_nom.Text;
                    obj.Cells[4].Value = tx_d_med.Text;
                    obj.Cells[5].Value = tx_d_mad.Text;     //codigo madera
                    obj.Cells[6].Value = tx_d_prec.Text;
                    obj.Cells[7].Value = tx_d_total.Text;
                    obj.Cells[8].Value = tx_d_can.Text;
                    obj.Cells[9].Value = "";
                    obj.Cells[10].Value = tx_dat_mad.Text;     // nombre madera
                    obj.Cells[11].Value = tx_d_com.Text;
                    obj.Cells[12].Value = tx_d_det2.Text;
                    obj.Cells[13].Value = cmb_det2.Text.ToString().Substring(0, 3).Trim();     // sera?
                    obj.Cells[14].Value = "N";
                    obj.Cells[15].Value = tx_d_tda.Text;  // tx_dat_orig.Text;
                    //obj.Cells[16].Value = ;
                    //obj.Cells[17].Value = ;
                    obj.Cells[18].Value = tx_d_codal1.Text;
                }
                else
                {
                    if (dataGridView1.Rows.Count < vfdmax && tipede == tx_dat_tiped.Text.Trim())
                    {
                        int vca = int.Parse(tx_d_can.Text);                     // cantidad items
                        decimal vpo = decimal.Parse(tx_d_prec.Text);            // Si es mueble a diseño no tiene precio de catalogo, cojemos el precio digitado
                        if (decimal.Parse(tx_d_dat_prec.Text) > 0) vpo = decimal.Parse(tx_d_dat_prec.Text);        // precio original de catologo  28/02/2023
                        decimal vpts = vpo * vca;                               // precio total sin descuento
                        decimal vpc = decimal.Parse(tx_d_prec.Text);            // precio incluyendo descuento
                        decimal vptc = vpc * vca;                               // precio total con descuento
                        decimal vtd = vpts - vptc;                              // descuento total de la fila

                        //dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_codi.Text, tx_d_can.Text, tx_d_nom.Text, tx_d_med.Text,
                        //     tx_d_mad.Text, tx_d_prec.Text, tx_d_total.Text, tx_d_can.Text, "", tx_dat_mad.Text, tx_d_com.Text, tx_d_det2.Text, 
                        //     cmb_det2.Text.ToString().Substring(0, 3).Trim(), "N",tx_d_tda.Text,vpts.ToString("#0.00"),vtd.ToString("#0.00"));
                        // 05/01/2023 -> si el item es articulo chico o a diseño (sin precio definido) el precio es el que pone el operador
                        if (cmb_mod.Text != "000")
                        {
                            dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_codi.Text, tx_d_can.Text, tx_d_nom.Text, tx_d_med.Text,
                                 tx_d_mad.Text, tx_d_dat_prec.Text, vpts.ToString("#0.00"), tx_d_can.Text, "", tx_dat_mad.Text, tx_d_com.Text, tx_d_det2.Text,
                                 cmb_det2.Text.ToString().Substring(0, 3).Trim(), "N", tx_d_tda.Text, vpts.ToString("#0.00"), vtd.ToString("#0.00"), tx_d_codal1.Text);
                        }
                        else
                        {
                            dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_d_codi.Text, tx_d_can.Text, tx_d_nom.Text, tx_d_med.Text,
                                 tx_d_mad.Text, tx_d_prec.Text, vpts.ToString("#0.00"), tx_d_can.Text, "", tx_dat_mad.Text, tx_d_com.Text, tx_d_det2.Text,
                                 cmb_det2.Text.ToString().Substring(0, 3).Trim(), "N", tx_d_tda.Text, vpts.ToString("#0.00"), vtd.ToString("#0.00"), tx_d_codal1.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Límite de filas por contrato alcanzado", "No se puede insertar mas filas",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            if (Tx_modo.Text == "EDITAR")           
            {
                if (!escambio.Contains(tx_dat_estad.Text))
                {
                    if (decimal.Parse(tx_saldo.Text) <= 0)
                    {
                        MessageBox.Show("No esta permitido agregar items, el SALDO en S/ es cero",
                                    "No puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        if (vupb.Contains(asd) == true)
                        {
                            string sn = "N";
                            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[14].Value.ToString() == "B")
                                {
                                    sn = "S";
                                }
                            }
                            if (sn == "N")
                            {
                                MessageBox.Show("No esta permitido agregar items, el estado no lo permite",
                                    "No puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El estado actual del contrato no permite modificar el detalle",
                                "No puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
                if (tx_d_id.Text.Trim() != "")    //  dataGridView1.Rows.Count > 1
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (tx_d_codi.Text == dataGridView1.Rows[i].Cells[1].Value.ToString() && 
                            tx_d_id.Text.Trim() != dataGridView1.Rows[i].Cells[0].Value.ToString().Trim())
                        {
                            MessageBox.Show("No se puede repetir el código!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cmb_fam.Focus();
                            return;
                        }
                    }
                    //a.iddetacon,a.item,a.cant,a.nombre,a.medidas,a.madera,a.precio,a.total,a.saldo,a.pedido,a.codref,a.coment,
                    //piedra,codpie,na,tda_item
                    //DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                    DataGridViewRow obj = (DataGridViewRow)dataGridView1.Rows[int.Parse(tx_d_it.Text)];
                    obj.Cells[1].Value = tx_d_codi.Text;
                    obj.Cells[2].Value = tx_d_can.Text;
                    obj.Cells[3].Value = tx_d_nom.Text;
                    obj.Cells[4].Value = tx_d_med.Text;
                    obj.Cells[5].Value = tx_d_mad.Text;
                    obj.Cells[6].Value = tx_d_prec.Text;
                    obj.Cells[7].Value = tx_d_total.Text;
                    obj.Cells[8].Value = tx_d_saldo.Text;
                    obj.Cells[9].Value = "";
                    obj.Cells[10].Value = tx_dat_mad.Text;
                    obj.Cells[11].Value = tx_d_com.Text;
                    obj.Cells[12].Value = tx_d_det2.Text;
                    obj.Cells[13].Value = cmb_det2.Text.ToString().Substring(0, 3).Trim();
                    obj.Cells[14].Value = "A";  // registro actualizado
                    obj.Cells[15].Value = tx_d_tda.Text;
                    obj.Cells[18].Value = tx_d_codal1.Text;
                }
                else
                {
                    DataTable dtg = (DataTable)dataGridView1.DataSource;
                    DataRow tr = dtg.NewRow();
                    tr["iddetacon"] = "0";  // dataGridView1.Rows.Count;
                    tr["item"] = tx_d_codi.Text;
                    tr["cant"] = tx_d_can.Text;
                    tr["nombre"] = tx_d_nom.Text;
                    tr["medidas"] = tx_d_med.Text;
                    tr["madera"] = tx_d_mad.Text;
                    tr["precio"] = tx_d_prec.Text;
                    tr["total"] = tx_d_total.Text;
                    tr["saldo"] = tx_d_saldo.Text;
                    tr["pedido"] = "";
                    tr["codref"] = tx_dat_mad.Text;
                    tr["coment"] = tx_d_com.Text;
                    tr["piedra"] = tx_d_det2.Text;
                    tr["codpie"] = cmb_det2.Text.ToString().Substring(0, 3).Trim();
                    tr["na"] = "N";
                    tr["tda_item"] = tx_d_tda.Text;
                    tr["alterno1"] = tx_d_codal1.Text;
                    dtg.Rows.Add(tr);
                }
            }
            tx_d_nom.Text = "";
            tx_d_can.Text = "";
            tx_d_com.Text = "";
            tx_d_med.Text = "";
            tx_d_mad.Text = "";
            tx_d_det2.Text = "";
            tx_d_est.Text = "";
            tx_d_id.Text = "";
            tx_d_codi.Text = "";
            tx_d_prec.Text = "";
            tx_d_total.Text = "";
            tx_d_tda.Text = "";
            tx_d_codal1.Text = "";
            //tx_saldo.Text = "";
            //limpia_combos(tabuser);
            cmb_fam.SelectedIndex = -1;
            cmb_mod.SelectedIndex = -1;
            cmb_mad.SelectedIndex = -1;
            cmb_tip.SelectedIndex = -1;
            cmb_det1.SelectedIndex = -1;
            cmb_aca.SelectedIndex = -1;
            //cmb_tal.SelectedIndex = -1;
            cmb_det2.SelectedIndex = -1;
            cmb_det3.SelectedIndex = -1;
            //
            calculos();
            motivD();
        }        // detalle articulos 
        private void button2_Click(object sender, EventArgs e)          // detalle adicionales
        {
            if (tx_a_nombre.Text == "")
            {
                MessageBox.Show("El código/nombre no existe en la maestra", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_a_nombre.Focus();
                return;
            }
            if (tx_a_cant.Text == "")
            {
                MessageBox.Show("Falta la cantidad", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_a_cant.Focus();
                return;
            }
            if (tx_a_total.Text == "")
            {
                MessageBox.Show("Falta el precio", "Atención - Verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_a_precio.Focus();
                return;
            }
            // validamos que el codigo no se repita en la grilla, REUNIÓN 09-03-2021 CON juan carlos, nestor y christian
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (tx_a_codig.Text == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    MessageBox.Show("Esta repitiendo el código adicional del artículo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_a_cant.Focus();
                    return;
                }
            }
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_a_id.Text.Trim() != "")
                {
                    for (int i=0;i<dataGridView1.Rows.Count - 1 ; i++)
                    {
                        if (tx_a_id.Text.Trim() == dataGridView1.Rows[i].Cells[0].Value.ToString())
                        {
                            DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                            obj.Cells[1].Value = tx_a_codig.Text;   // obj.Cells[1].Value = 
                            obj.Cells[2].Value = tx_a_cant.Text;
                            obj.Cells[3].Value = tx_a_nombre.Text;
                            obj.Cells[4].Value = tx_a_medid.Text;
                            obj.Cells[5].Value = "";
                            obj.Cells[6].Value = Math.Round(decimal.Parse(tx_a_precio.Text), 2).ToString("0.00");
                            obj.Cells[7].Value = Math.Round(decimal.Parse(tx_a_total.Text), 2).ToString("0.00");
                            obj.Cells[8].Value = tx_a_cant.Text;
                            obj.Cells[9].Value = "";
                            obj.Cells[10].Value = "";
                            obj.Cells[11].Value = tx_a_comen.Text;
                            obj.Cells[12].Value = "";
                            obj.Cells[13].Value = tx_a_pd.Text;   // en servicios es % detraccion
                            obj.Cells[14].Value = "N";
                            obj.Cells[14].Value = "";   // no tiene tienda 
                        }
                    }
                }
                else
                {
                    if (dataGridView1.Rows.Count < vfdmax && tipede == tx_dat_tiped.Text.Trim())
                    {
                        DataTable ndtg = (DataTable)dataGridView1.DataSource;
                        if (ndtg != null)
                        {
                            ndtg.Rows.Add(dataGridView1.Rows.Count, tx_a_codig.Text, tx_a_cant.Text, tx_a_nombre.Text, tx_a_medid.Text,
                                 "", tx_a_precio.Text, tx_a_total.Text, tx_a_cant.Text, "", "", tx_a_comen.Text, "", tx_a_pd.Text, "N", "");
                        }
                        else
                        {
                            dataGridView1.Rows.Add(dataGridView1.Rows.Count, tx_a_codig.Text, tx_a_cant.Text, tx_a_nombre.Text, tx_a_medid.Text,
                                 "", tx_a_precio.Text, tx_a_total.Text, tx_a_cant.Text, "", "", tx_a_comen.Text, "", tx_a_pd.Text, "N", "");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Límite de filas por contrato alcanzado", "No se puede insertar mas filas",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            if (Tx_modo.Text == "EDITAR")
            {
                if (!escambio.Contains(tx_dat_estad.Text))
                {
                    MessageBox.Show("El estado actual del contrato no permite modificar el detalle",
                        "No puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (tx_a_id.Text.Trim() != "")
                {
                    DataTable dtg = (DataTable)dataGridView1.DataSource;
                    foreach(DataRow row in dtg.Rows)
                    {
                        if (tx_a_id.Text.Trim() == row[0].ToString())    // row.Cells[0].Value.ToString()
                        {
                            // iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,'na'
                            //DataGridViewRow obj = (DataGridViewRow)dataGridView1.CurrentRow;
                            row[1] = tx_a_codig.Text;   // obj.Cells[1].Value = 
                            row[2] = tx_a_cant.Text;
                            row[3] = tx_a_nombre.Text;
                            row[4] = tx_a_medid.Text;
                            row[5] = "";
                            row[6] = tx_a_precio.Text;
                            row[7] = tx_a_total.Text;
                            row[8] = tx_a_salcan.Text;
                            row[9] = "";
                            row[10] = "";
                            row[11] = tx_a_comen.Text;
                            row[12] = "";
                            row[13] = tx_a_pd.Text;
                            row[14] = "A";  // registro actualizado
                            row[15] = "";   // adicionales no tienen tienda
                        }
                    }
                }
                else
                {
                    DataTable dtg = (DataTable)dataGridView1.DataSource;
                    DataRow tr = dtg.NewRow();
                    tr["iddetacon"] = dataGridView1.Rows.Count.ToString();  // "0";
                    tr["item"] = tx_a_codig.Text;
                    tr["cant"] = tx_a_cant.Text;
                    tr["nombre"] = tx_a_nombre.Text;
                    tr["medidas"] = tx_a_medid.Text;
                    tr["madera"] = "";
                    tr["precio"] = tx_a_precio.Text;
                    tr["total"] = tx_a_total.Text;
                    tr["saldo"] = tx_a_salcan.Text;
                    tr["pedido"] = "";
                    tr["codref"] = "";
                    tr["coment"] = tx_a_comen.Text;
                    tr["piedra"] = tx_a_pd.Text;
                    tr["na"] = "N";
                    tr["tda_item"] = "";
                    dtg.Rows.Add(tr);
                }
            }
            tx_a_id.Text = "";
            tx_a_cant.Text = "";
            tx_a_codig.Text = "";
            tx_a_nombre.Text = "";
            tx_a_medid.Text = "";
            tx_a_precio.Text = "";
            tx_a_total.Text = "";
            calculos();
        }
        #endregion boton_form;

        #region leaves and enter
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")    //  && tx_idr.Text != ""
            {
                jalaoc("tx_idr");               // jalamos los datos del registro
            }
        }
        internal void tx_codped_Leave(object sender, EventArgs e)       // cambie de private a internal el 28/07/2022
        {
            if (Tx_modo.Text != "NUEVO")
            {
                if (tx_codped.Text != "")   //  && tx_idr.Text == ""
                {
                    string tcp = tx_codped.Text;
                    limpiapag(tabuser);
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    tx_codped.Text = tcp;
                    //
                    jalaoc("tx_codped");                        // jalamos los datos
                    if (tx_idr.Text == "")
                    {
                        // jalamos desde la base de datos, debe ser un contrato entregado o anulado
                        if (jalacont(tx_codped.Text) == false)
                        {
                            //MessageBox.Show("Error en obtener datos del contrato", "Error de conexión");
                            MessageBox.Show("Error en obtener datos del contrato", "No existe el contrato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tx_codped.Text = "";
                            tx_codped.Focus();
                            return;
                        }
                        else
                        {
                            jaladet(tx_codped.Text);
                            // verificar que jale los codigos adicionales
                        }
                    }
                    if (escambio.Contains(tx_dat_estad.Text) && Tx_modo.Text == "EDITAR")   // si permite modificacion se habilitan los campos
                    {
                        escribepag(tabuser);
                        tx_a_codig.ReadOnly = true;
                        tx_a_total.ReadOnly = true;
                    }
                }
                /*else
                {
                    limpiapag(tabuser);
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }*/
            }
        }
        private void tx_d_can_Leave(object sender, EventArgs e)
        {
            if (tx_d_can.Text != "" && tx_d_prec.Text != "")
            {
                tx_d_total.Text = (Decimal.Parse(tx_d_can.Text) * Decimal.Parse(tx_d_prec.Text)).ToString("0.00");
            }
            //if (Tx_modo.Text == "NUEVO") tx_d_saldo.Text = tx_d_can.Text;                                 ya no va desde la 
            //if (Tx_modo.Text == "EDITAR" && tx_d_id.Text.Trim() == "") tx_d_saldo.Text = tx_d_can.Text;   reunión del 09/10/2020
            tx_d_saldo.Text = tx_d_can.Text;    // no se modifica el saldo desde el 09/10/2020
        }
        private void tx_a_can_Leave(object sender, EventArgs e)
        {
            if (tx_a_cant.Text != "" && tx_a_precio.Text != "")
            {
                tx_a_total.Text = (Decimal.Parse(tx_a_cant.Text) * Decimal.Parse(tx_a_precio.Text)).ToString("0.00");
            }
            if (Tx_modo.Text == "NUEVO") tx_a_salcan.Text = tx_a_cant.Text;
            if (Tx_modo.Text == "EDITAR" && tx_a_id.Text.Trim() == "") tx_a_salcan.Text = tx_a_cant.Text;
        }
        private void tx_d_prec_Leave(object sender, EventArgs e)
        {
            if (tx_d_can.Text != "" && tx_d_prec.Text != "")
            {
                tx_d_total.Text = (Decimal.Parse(tx_d_can.Text) * Decimal.Parse(tx_d_prec.Text)).ToString("0.00");
            }
            if (Tx_modo.Text == "NUEVO") tx_d_saldo.Text = tx_d_can.Text;
            if (Tx_modo.Text == "EDITAR" && tx_d_id.Text.Trim() == "") tx_d_saldo.Text = tx_d_can.Text;
        }
        private void tx_a_precio_Leave(object sender, EventArgs e)
        {
            if (tx_a_cant.Text != "" && tx_a_precio.Text != "")
            {
                tx_a_total.Text = (Decimal.Parse(tx_a_cant.Text) * Decimal.Parse(tx_a_precio.Text)).ToString("0.00");
            }
            if (Tx_modo.Text == "NUEVO") tx_a_salcan.Text = tx_a_cant.Text;
            if (Tx_modo.Text == "EDITAR" && tx_a_id.Text.Trim() == "") tx_a_salcan.Text = tx_a_cant.Text;
        }
        private void tx_ndc_Leave(object sender, EventArgs e)       // en modo nuevo permite jalar la info del ruc o dni o c.extranjeria
        {
            if (tx_ndc.Text != "")                       // digitos por cada tipo de documento
            {
                foreach (DataRow row in dtdest.Rows)
                {
                    if (row["idcodice"].ToString() == tx_dat_tdoc.Text) //descrizionerid
                    {
                        if (row["codigo"].ToString() != tx_ndc.Text.Trim().Length.ToString())
                        {
                            MessageBox.Show("La longitud del documento debe ser " + row["codigo"].ToString(), "Atención - debe corregir", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            tx_ndc.Text = "";
                            tx_ndc.Focus();
                            return;
                        }
                    }
                }
            }
            else
            {
                if (Tx_modo.Text == "NUEVO") cmb_tdoc.Focus();
            }
        }
        private void tx_ndc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR") && tx_ndc.Text != "")
            {
                if (tx_dat_tdoc.Text == "") cmb_tdoc.Focus();
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string jala = "select ifnull(razonsocial,''),ifnull(direcc1,''),ifnull(direcc2,''),ifnull(localidad,''),ifnull(provincia,'')," +
                        "ifnull(depart,''),ifnull(numerotel1,''),ifnull(numerotel2,''),ifnull(email,''),desc_doc.cnt,idanagrafica " +
                        "from anag_cli left join desc_doc on desc_doc.idcodice=anag_cli.tipdoc " +
                        "where tipdoc=@tdo and ruc=@ndo";
                    MySqlCommand micon = new MySqlCommand(jala, conn);
                    micon.Parameters.AddWithValue("@tdo", tx_dat_tdoc.Text);    // idcodice del documento
                    micon.Parameters.AddWithValue("@ndo", tx_ndc.Text);
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            tx_nombre.Text = dr.GetString(0).Trim();
                            tx_direc.Text = dr.GetString(1).Trim() + " " + dr.GetString(2).Trim();
                            tx_dist.Text = dr.GetString(3).Trim();
                            tx_prov.Text = dr.GetString(4).Trim();
                            tx_dpto.Text = dr.GetString(5).Trim();
                            tx_mail.Text = dr.GetString(8).Trim();
                            tx_telef1.Text = dr.GetString(6).Trim();
                            tx_telef2.Text = dr.GetString(7).Trim();
                            tx_idcli.Text = dr.GetString(10).Trim();
                        }
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        conn.Close();
                        var aaa = MessageBox.Show("No existe el cliente" + Environment.NewLine +
                            "desea crealo?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aaa == DialogResult.Yes)
                        {
                            chk_cliente.Checked = true;
                            // llamamos a conectorSolorsoft si esta habilitado
                            if(conSol == true)
                            {
                                if (tx_dat_tdoc.Text == docDni)
                                {
                                    string[] rl = lib.conectorSolorsoft("DNI", tx_ndc.Text);
                                    tx_nombre.Text = rl[0];
                                }
                                if (tx_dat_tdoc.Text == docRuc)
                                {
                                    string[] rl = lib.conectorSolorsoft("RUC", tx_ndc.Text);
                                    tx_nombre.Text = rl[0];
                                    tx_direc.Text = rl[2];
                                    tx_dpto.Text = rl[3];
                                    tx_prov.Text = rl[4];
                                    tx_dist.Text = rl[5];
                                }
                            }
                            else
                            {
                                chk_cliente.Checked = true;
                                tx_nombre.Focus();
                            }
                        }
                        else
                        {
                            tx_ndc.Text = "";
                            tx_ndc.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se puede conectar al servidor", "Error de conectidad");
                    Application.Exit();
                    return;
                }
                conn.Close();
            }
        }
        private void tx_dpto_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_dpto.Text = "";
            if (tx_dpto.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                DataRow[] result = dtadpd.Select("provin='00' AND distri='00' AND nombre='" + tx_dpto.Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    tx_dat_dpto.Text = row["depart"].ToString();
                }
                if (tx_dat_dpto.Text == "")
                {
                    MessageBox.Show("No existe el departamento escrito", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_dpto.Text = "";
                    tx_dpto.Focus();
                    return;
                }
                autoprovi();
            }
        }
        private void tx_prov_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_provin.Text = "";
            if (tx_prov.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                DataRow[] result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND distri='00' AND nombre='" + tx_prov.Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    if (tx_dat_dpto.Text == "07") tx_dat_provin.Text = "01";
                    else tx_dat_provin.Text = row["provin"].ToString();
                }
                if (tx_dat_provin.Text == "")
                {
                    MessageBox.Show("No existe la provincia escrita", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_prov.Text = "";
                    tx_prov.Focus();
                    return;
                }
                autodistr();
            }
        }
        private void tx_dist_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tx_dat_distri.Text = "";
            DataRow[] result;
            if (tx_dist.Text.Trim() != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                if (tx_dat_dpto.Text == "07")
                {
                    result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND provin='01' AND nombre='" + tx_dist.Text.Trim() + "'");
                }
                else
                {
                    result = dtadpd.Select("depart='" + tx_dat_dpto.Text + "' AND provin='" + tx_dat_provin.Text.Trim() + "' AND nombre='" + tx_dist.Text.Trim() + "'");
                }
                foreach (DataRow row in result)
                {
                    tx_dat_distri.Text = row["distri"].ToString();
                }
                if (tx_dat_distri.Text == "")
                {
                    MessageBox.Show("No existe el distrito escrito", "Por favor revise", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_dist.Text = "";
                    tx_dist.Focus();
                    return;
                }
            }
        }
        private void tx_mail_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lib.email_bien_escrito(tx_mail.Text.Trim()) == false && tx_mail.Text.Trim() != "")
            {
                MessageBox.Show("El correo no tiene el formato correcto", "Atención verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tx_mail.Text = "";
                return;
            }
        }
        private void tx_dscto_Leave(object sender, EventArgs e)
        {
            calculos();
        }
        private void tx_acta_Leave(object sender, EventArgs e)
        {
            calculos();
        }
        private void tx_valor_Enter(object sender, EventArgs e)
        {
            // por las huev....
        }
        private void chk_cliente_CheckStateChanged(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
            {
                if (chk_cliente.Checked == true)
                {
                    cmb_tdoc.Enabled = true;
                    tx_ndc.Enabled = true;
                    tx_nombre.Enabled = true;
                    tx_direc.Enabled = true;
                    tx_dist.Enabled = true;
                    tx_prov.Enabled = true;
                    tx_dpto.Enabled = true;
                    tx_mail.Enabled = true;
                    tx_telef1.Enabled = true;
                    tx_telef2.Enabled = true;
                    //
                    tx_ndc.ReadOnly = false;
                    tx_nombre.ReadOnly = false;
                    tx_direc.ReadOnly = false;
                    tx_dist.ReadOnly = false;
                    tx_prov.ReadOnly = false;
                    tx_dpto.ReadOnly = false;
                    tx_mail.ReadOnly = false;
                    tx_telef1.ReadOnly = false;
                    tx_telef2.ReadOnly = false;
                }
                else
                {
                    tx_nombre.ReadOnly = true;
                    tx_direc.ReadOnly = true;
                    tx_dist.ReadOnly = true;
                    tx_prov.ReadOnly = true;
                    tx_dpto.ReadOnly = true;
                    tx_mail.ReadOnly = true;
                    tx_telef1.ReadOnly = true;
                    tx_telef2.ReadOnly = true;
                }
            }
        }
        private void chk_lugent_CheckStateChanged(object sender, EventArgs e)
        {
            if(chk_lugent.CheckState == CheckState.Checked)
            {
                tx_dirent.Tag = tx_dirent.Text;
                tx_dirent.Text = "";
                tx_dirent.ReadOnly = true;
            }
            if (chk_lugent.CheckState == CheckState.Unchecked)
            {
                tx_dirent.Text = tx_dirent.Tag.ToString();
                tx_dirent.ReadOnly = false;
            }
        }
        private void dtp_entreg_ValueChanged(object sender, EventArgs e)
        {
            if (dtp_entreg.Checked == true && (Tx_modo.Text == "NUEVO") || Tx_modo.Text == "EDITAR")
            {
                if (dtp_pedido.Value.Date > dtp_entreg.Value.Date)
                {
                    MessageBox.Show("La fecha de entrega debe ser mayor" + Environment.NewLine +
                        "a la fecha del contrato", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtp_entreg.Value = dtp_pedido.Value;
                }
            }
        }
        private void tx_corre_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                /*
                if (tx_mc.Text == "")
                {
                    tx_mc.Focus();
                    return;
                } 
                if (tx_serie.Text == "")
                {
                    tx_serie.Focus();
                    return;
                }
                COMENTAMOS ESTA PARTE DE LA LLAMADA A jalaDatFact porque esta duplicando valor a cuenta 10/08/2022 */ 
                //if (tx_mc.Text != "" && tx_serie.Text != "" && tx_corre.Text != "") jalaDatFact("T", tx_mc.Text, tx_serie.Text, tx_corre.Text, "1");
                //tx_mc.Text = ""; tx_serie.Text = ""; tx_corre.Text = "";      // 10/08/2022 no esta funcando esto .. esta duplicando el a cuenta
                tx_coment.Focus();
            }
        }
        private void tx_conMadre_Leave(object sender, EventArgs e)              // contrato de bienes madre para servicios
        {
            // validamos que el contrato sea de bienes y exista
            // contrat.articulo = (blanco) ambos tipos, S=servicio, B=bienes
            if (tx_conMadre.Text != "" && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")) 
            {
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string consu = "select a.contrato,a.articulo,a.fecha,a.tipoes,a.status,a.tipocon," +
                            "b.razonsocial,b.ruc,b.tipdoc,ifnull(c.cnt,-1)," +
                            "(SELECT COUNT(id) FROM contrat WHERE right(contrato,6)=@cont) AS cta " +
                            "from contrat a left join anag_cli b on b.idanagrafica=a.cliente left join desc_doc c on c.idcodice=b.tipdoc " +
                            "where right(a.contrato,6)=@cont";
                        using (MySqlCommand micon = new MySqlCommand(consu, conn))
                        {
                            micon.Parameters.AddWithValue("@cont", lib.Right(tx_conMadre.Text.Trim(), 6));
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    if (dr.HasRows)
                                    {
                                        if (dr.GetString(1) == "S")
                                        {
                                            MessageBox.Show("El contrato existe pero no es del tipo Bienes", "verifique", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            tx_conMadre.Text = "";
                                            return;
                                        }
                                        else
                                        {
                                            tx_conS.Text = dr.GetString(10);
                                            tx_dat_tdoc.Text = dr.GetString(8);
                                            tx_ndc.Text = dr.GetString(7);
                                            int vi = dr.GetInt16(9);
                                            cmb_tdoc.SelectedIndex = vi;
                                            tx_ndc_Leave(null, null);
                                            tx_ndc_Validating(null, null);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Contrato NO existe", "verifique", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        tx_conMadre.Text = "";
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ocurrió un error no controlado" + Environment.NewLine +
                                        "Notifique al soporte técnico", "Error interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    tx_conMadre.Text = "";
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se tiene conexión con el servidor","Error de comunicación",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        tx_conMadre.Text = "";
                        return;
                    }
                }
            }
        }
        private void telefonos_Leave(object sender, EventArgs e)                // validaciones de telefono 9 digitos o nada1
        {
            if ((Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR"))
            {
                NumericTextBox ntb = (NumericTextBox)sender;
                if (ntb.Text.Trim() != "" && ntb.Text.Length != 9)
                {
                    MessageBox.Show("El teléfono debe ser de 9 dígitos", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ntb.Focus();
                    return;
                }
            }
        }
        #endregion leaves;

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString; // original
        }
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            dtg.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && Tx_modo.Text != "NUEVO")
            {
                //string codu = "";
                string idr, rind = "";
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rind = advancedDataGridView1.CurrentRow.Index.ToString();
                tabControl1.SelectedTab = tabuser;
                limpiar(this);
                limpiapag(tabuser);
                limpia_otros(tabuser);
                limpia_combos(tabuser);
                limpia_chk();
                //escribepag(tabuser);
                //sololeepag(tabuser);
                tx_idr.Text = idr;
                tx_rind.Text = rind;
                tx_dat_tiped.Text = tipede;
                jalaoc("tx_idr");
                tx_coment.Focus();
            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString()
                && Tx_modo.Text == "EDITAR")
            {
                string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                string[] noeta = equivinter(advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString());    // retorna la tabla segun el titulo de la columna

                var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                    "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    if (advancedDataGridView1.Columns[e.ColumnIndex].Tag.ToString() == "validaSI")   // la columna se valida?
                    {
                        // id,codped,status,origen,destino,fecha,entrega,coment,tipoes
                        // valida si el dato ingresado es valido en la columna
                        if (e.ColumnIndex == 4)                         // valida almacen destino
                        {
                            if (lib.validac("desc_alm", "idcodice", e.FormattedValue.ToString()) == true)
                            {
                                // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                                lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            }
                            else
                            {
                                MessageBox.Show("El valor no es válido para almacen", "Atención - Corrija");
                                e.Cancel = true;
                            }
                        }
                        if (e.ColumnIndex == 5)           // fecha
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 6)           // fecha entrega
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 7)          // comentario
                        {
                            // no se valida
                        }
                        if (e.ColumnIndex == 8)          // 
                        {
                            // no se valida
                        }
                    }
                    else
                    {
                        // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                        lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                //if(Tx_modo.Text == "NUEVO" || Tx_modo.Text == "VISUALIZAR") e.Cancel = true;
            }
        }
        private void advancedDataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void color_filas()
        {
            foreach (DataGridViewRow row in advancedDataGridView1.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    if (row.Cells[3].Value.ToString() == "PENDIE")
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                    if (row.Cells[3].Value.ToString() == "PORLLE")
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    if (row.Cells[3].Value.ToString() == "LLEPAR")
                    {
                        row.DefaultCellStyle.BackColor = Color.YellowGreen;
                    }
                    if (row.Cells[3].Value.ToString() == "PORENT")
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                    if (row.Cells[3].Value.ToString() == "ENTPAR")
                    {
                        row.DefaultCellStyle.BackColor = Color.Peru;
                    }
                    if (row.Cells[3].Value.ToString() == "PEDPAR")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }
        }
        #endregion

        #region datagridview1 - grilla detalle del contrato
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                if (Tx_modo.Text == "EDITAR")
                {
                    tx_saldo.Enabled = true;
                }
                else
                {
                    tx_saldo.Enabled = false;
                }
                if (dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(0,1) == letgru) // Z = adicionales, servicios
                {
                    tabControl2.SelectedTab = tabadicion;
                    tx_a_it.Text = dataGridView1.Rows[e.RowIndex].Index.ToString();
                    tx_a_id.Text = dataGridView1.Rows[e.RowIndex].Cells["iddetacon"].Value.ToString();
                    tx_a_cant.Text = dataGridView1.Rows[e.RowIndex].Cells["cant"].Value.ToString();
                    tx_a_codig.Text = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString();
                    tx_a_nombre.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                    tx_a_medid.Text = dataGridView1.Rows[e.RowIndex].Cells["medidas"].Value.ToString();
                    tx_a_precio.Text = dataGridView1.Rows[e.RowIndex].Cells["precio"].Value.ToString();
                    tx_a_total.Text = dataGridView1.Rows[e.RowIndex].Cells["total"].Value.ToString();
                    tx_a_salcan.Text = dataGridView1.Rows[e.RowIndex].Cells["saldo"].Value.ToString();
                    tx_a_comen.Text = dataGridView1.Rows[e.RowIndex].Cells["coment"].Value.ToString();
                }
                else
                {
                    tabControl2.SelectedTab = tabcodigo;                                                    // articulos chicos y de catalogo
                    tx_d_it.Text = dataGridView1.Rows[e.RowIndex].Index.ToString();
                    tx_d_nom.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                    tx_d_med.Text = dataGridView1.Rows[e.RowIndex].Cells["medidas"].Value.ToString();
                    tx_d_can.Text = dataGridView1.Rows[e.RowIndex].Cells["cant"].Value.ToString();
                    tx_d_id.Text = dataGridView1.Rows[e.RowIndex].Cells["iddetacon"].Value.ToString();
                    tx_d_codi.Text = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString();
                    tx_d_prec.Text = dataGridView1.Rows[e.RowIndex].Cells["precio"].Value.ToString();
                    tx_d_total.Text = dataGridView1.Rows[e.RowIndex].Cells["total"].Value.ToString();
                    tx_d_saldo.Text = dataGridView1.Rows[e.RowIndex].Cells["saldo"].Value.ToString();
                    tx_d_com.Text = dataGridView1.Rows[e.RowIndex].Cells["coment"].Value.ToString();
                    tx_d_det2.Text = dataGridView1.Rows[e.RowIndex].Cells["piedra"].Value.ToString();
                    string fam = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(0, 1);
                    string mod = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(1, 3);
                    string mad = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(4, 1);
                    string tip = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(5, 2);
                    string de1 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(7, 2);
                    string aca = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(9, 1);
                    string tal = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(10, 2);
                    string de2 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(12, 3);
                    string de3 = dataGridView1.Rows[e.RowIndex].Cells["item"].Value.ToString().Substring(15, 3);
                    //
                    cmb_aca.Enabled = true;
                    cmb_det2.Enabled = true;
                    //
                    cmb_fam.Tag = fam;
                    cmb_fam.SelectedIndex = cmb_fam.FindString(cmb_fam.Tag.ToString());
                    cmb_mod.Tag = mod;
                    cmb_mod.SelectedIndex = cmb_mod.FindString(cmb_mod.Tag.ToString());
                    cmb_mad.Tag = mad;
                    cmb_mad.SelectedIndex = cmb_mad.FindString(cmb_mad.Tag.ToString());
                    cmb_mad_SelectionChangeCommitted(null, null);
                    cmb_tip.Tag = tip;
                    cmb_tip.SelectedIndex = cmb_tip.FindString(cmb_tip.Tag.ToString());
                    cmb_det1.Tag = de1;
                    cmb_det1.SelectedIndex = cmb_det1.FindString(cmb_det1.Tag.ToString());
                    cmb_det1_SelectionChangeCommitted(null, null);
                    cmb_aca.Tag = aca;
                    cmb_aca.SelectedIndex = cmb_aca.FindString(cmb_aca.Tag.ToString());
                    cmb_aca_SelectionChangeCommitted(null, null);
                    cmb_tal.Tag = tal;
                    cmb_tal.SelectedIndex = 0;  //cmb_tal.FindString(cmb_tal.Tag.ToString());
                    cmb_det2.Tag = de2;
                    cmb_det2.SelectedIndex = cmb_det2.FindString(cmb_det2.Tag.ToString());
                    cmb_det2_SelectionChangeCommitted(null, null);
                    cmb_det3.Tag = de3;
                    cmb_det3.SelectedIndex = cmb_det3.FindString(cmb_det3.Tag.ToString());
                    cmb_det3_SelectionChangeCommitted(null, null);
                    //tx_saldo.Text = dataGridView1.Rows[e.RowIndex].Cells["saldo"].Value.ToString();              // saldo
                    tx_d_tda.Text = dataGridView1.Rows[e.RowIndex].Cells["tda_item"].Value.ToString();
                    //
                    cmb_aca.Enabled = false;
                    cmb_det2.Enabled = false;
                    //
                }
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) 
        {
            // si es edicion, si es el usuario autorizado y el contrato es reciente => borra la(s) filas de detalle
            // busca en la base de datos y lo borra, debe actualizar estado del contrato y saldos
            string modos = "EDITAR,NUEVO";
            if (modos.Contains(Tx_modo.Text) == true)
            {
                if (decimal.Parse(tx_saldo.Text) <= 0)
                {
                    MessageBox.Show("seleccionó una fila para borrar" + Environment.NewLine +
                    "pero no se puede proceder porque el saldo S/ es cero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                    return;
                }
                if (_comprobantes.Count > 0)
                {
                    MessageBox.Show("seleccionó una fila para borrar" + Environment.NewLine +
                    "pero biene de una FT o BV, no se permite borrar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                    return;
                }
                var aa = MessageBox.Show("seleccionó una fila para borrar" + Environment.NewLine +
                    "se actualizarán los datos", "Confirma?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (Tx_modo.Text == "NUEVO")
                    {
                        e.Cancel = false;
                        //calculos();
                    }
                    else
                    {   // modo edicion contrato = PENDIE y usuario con permiso
                        if (Tx_modo.Text == "EDITAR")
                        {
                            DataGridViewRow rdg = e.Row;
                            if (tx_dat_estad.Text == tiesta)
                            {
                                //if (borra_fila(rdg.Cells[0].Value.ToString()) == true)
                                {
                                    rdg.DefaultCellStyle.BackColor = Color.Red;
                                    rdg.Cells[14].Value = "B";
                                    e.Cancel = true;   // false
                                    calculos();
                                }
                            }
                            else
                            {
                                // si es usuario autorizado y el producto no tiene reserva,pedido o salida
                                if (vupb.Contains(asd) == true && (rdg.Cells[2].Value.ToString() == rdg.Cells[8].Value.ToString()))
                                {
                                    //if (borra_fila(rdg.Cells[0].Value.ToString()) == true)
                                    {
                                        rdg.DefaultCellStyle.BackColor = Color.Red;
                                        rdg.Cells[14].Value = "B";
                                        e.Cancel = true;   // false
                                        calculos();        // cálculo de saldos y actualizacion del estado si queda en cero
                                    }
                                }
                                else
                                {
                                    if (vupb.Contains(asd) == true && vtasc.Contains(rdg.Cells[1].Value.ToString().Substring(0, 1)) == true)
                                    {
                                        rdg.DefaultCellStyle.BackColor = Color.Red;
                                        rdg.Cells[14].Value = "B";
                                        e.Cancel = true;
                                        calculos();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No es posible proceder por falta de permisos" + Environment.NewLine +
                                            "o porque el artículo tiene relación con otros procesos", "No se grabó la operación");
                                        e.Cancel = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            calculos();
        }
        #endregion

        #region radiobuttons
        private void rb_servi_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabadicion;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            //dataGridView1.Columns.Clear();
            calculos();
            tx_conMadre.Visible = true;
            tx_conMadre.Focus();
        }
        private void rb_bienes_Click(object sender, EventArgs e)
        {
            tx_conMadre.Visible = false;
            tx_conMadre.Text = "";
            tabControl2.SelectedTab = tabcodigo;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            //dataGridView1.Columns.Clear();
            calculos();
        }
        private void tabcodigo_Enter(object sender, EventArgs e)
        {
            if (rb_bienes.Checked != true)
            {
                tabControl2.SelectedTab = tabadicion;
            }
        }
        private void tabadicion_Enter(object sender, EventArgs e)
        {
            if (rb_servi.Checked != true)
            {
                tabControl2.SelectedTab = tabcodigo;
            }
        }
        #endregion

        #region crystal
        private void setParaCrystal()                   // genera el set para el reporte de crystal
        {
            conClie datos = generareporte();                        // conClie = dataset de impresion de contrato   
            frmvizcont visualizador = new frmvizcont(datos);        // POR ESO SE CREO ESTE FORM frmvizcont PARA MOSTRAR AHI. ES MEJOR ASI.  
            visualizador.Show();
        }
        private conClie generareporte()                 // procedimiento para meter los datos del formulario hacia las tablas del dataset del reporte en crystal
        {
            conClie repcontrato = new conClie();                                    // dataset

            conClie.cabeceraRow rowcabeza = repcontrato.cabecera.NewcabeceraRow();  // llenamos la tabla cabecera del dataset
            rowcabeza.contrato = tx_codped.Text;
            rowcabeza.fecha = dtp_pedido.Value.ToString("dd/MM/yyyy");
            rowcabeza.id = "0";
            string nnnn = cmb_taller.Text.Trim();     //PadRight(22).Substring(8, 15)
            rowcabeza.localVen = nnnn;
            rowcabeza.nomClie = tx_nombre.Text.Trim();
            rowcabeza.numDoc = tx_ndc.Text.Trim();
            if (cmb_tdoc.SelectedIndex == -1) rowcabeza.tipDoc = "";
            else rowcabeza.tipDoc = cmb_tdoc.SelectedItem.ToString();     //.SelectedText;
            rowcabeza.tipoCont = tx_dat_tiped.Text; // cmb_tipo.SelectedText;
            rowcabeza.direc = tx_direc.Text.Trim();
            rowcabeza.distrit = tx_dist.Text.Trim();
            rowcabeza.provin = tx_prov.Text.Trim();
            rowcabeza.depart = tx_dpto.Text.Trim();
            rowcabeza.email = tx_mail.Text.Trim();
            rowcabeza.telef1 = tx_telef1.Text.Trim();
            rowcabeza.telef2 = tx_telef2.Text.Trim();
            rowcabeza.valTot = tx_valor.Text;
            rowcabeza.valDscto = tx_dscto.Text;
            rowcabeza.valActa = tx_acta.Text;
            rowcabeza.valSaldo = tx_saldo.Text;
            rowcabeza.coment = tx_coment.Text.Trim();
            rowcabeza.dirEntreg = tx_dirent.Text.Trim();
            rowcabeza.fechEnt = dtp_entreg.Value.ToString("dd/MM/yyyy");
            rowcabeza.usuario = asd;
            rowcabeza.clte_r = (chk_lugent.Checked.ToString()=="True")? "1":"0";
            rowcabeza.serespman = (chk_serema.Checked.ToString() == "True") ? "1" : "0";
            rowcabeza.piso = tx_piso.Text;
            rowcabeza.ascensor = (chk_ascensor.Checked.ToString() == "True") ? "1" : "0";
            rowcabeza.contac = tx_contac.Text;
            rowcabeza.drefer = tx_dirRef.Text;
            rowcabeza.telcont = tx_telcont.Text;
            rowcabeza.totadic = tx_totesp.Text;
            rowcabeza.totbrut = tx_bruto.Text;
            repcontrato.cabecera.AddcabeceraRow(rowcabeza);
            //MessageBox.Show(chk_lugent.Checked.ToString(), "Valor lugent");
            if (rb_bienes.Checked == true)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)  //
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "" && row.Cells["item"].Value.ToString().Substring(0, 1) != letgru) // "Z"
                    {
                        conClie.detalleRow rowdetalle = repcontrato.detalle.NewdetalleRow();
                        rowdetalle.id = "0";    // row.Cells["iddetacon"].Value.ToString();
                        rowdetalle.cant = row.Cells["cant"].Value.ToString();
                        rowdetalle.codigo = row.Cells["item"].Value.ToString();
                        rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                        rowdetalle.medidas = row.Cells["medidas"].Value.ToString();
                        rowdetalle.madera = row.Cells["codref"].Value.ToString();     // madera
                        rowdetalle.det2 = row.Cells["piedra"].Value.ToString();
                        rowdetalle.acabado = "";    // row.Cells[""].Value.ToString();
                        rowdetalle.precio = row.Cells["precio"].Value.ToString();
                        rowdetalle.total = row.Cells["total"].Value.ToString();
                        rowdetalle.coment = row.Cells["coment"].Value.ToString();
                        rowdetalle.tienda = row.Cells["tda_item"].Value.ToString();    // 
                        repcontrato.detalle.AdddetalleRow(rowdetalle);
                        //iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,piedra,na
                    }
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "" && row.Cells["item"].Value.ToString().Substring(0, 1) == letgru) // "Z"
                    {
                        conClie.detalleRow rowdetalle = repcontrato.detalle.NewdetalleRow();
                        rowdetalle.id = "0";
                        rowdetalle.cant = "";
                        rowdetalle.codigo = "";
                        rowdetalle.nombre = "";
                        rowdetalle.medidas = "";
                        rowdetalle.madera = "";
                        rowdetalle.det2 = "";
                        rowdetalle.acabado = "";
                        rowdetalle.precio = "";
                        rowdetalle.total = "";
                        rowdetalle.coment = "";
                        rowdetalle.tienda = "";
                        repcontrato.detalle.AdddetalleRow(rowdetalle);
                        //
                        rowdetalle = repcontrato.detalle.NewdetalleRow();
                        rowdetalle.id = "0";
                        rowdetalle.cant = "";
                        rowdetalle.codigo = "";
                        rowdetalle.nombre = "-- ADICIONALES --";
                        rowdetalle.medidas = "";
                        rowdetalle.madera = "";
                        rowdetalle.det2 = "";
                        rowdetalle.acabado = "";
                        rowdetalle.precio = "";
                        rowdetalle.total = "";
                        rowdetalle.coment = "";
                        rowdetalle.tienda = "";
                        repcontrato.detalle.AdddetalleRow(rowdetalle);
                        break;
                    }
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Substring(0, 1) == letgru)    // "Z"
                    {
                        conClie.detalleRow rowdetalle = repcontrato.detalle.NewdetalleRow();
                        rowdetalle.id = "0";    // row.Cells["iddetacon"].Value.ToString();
                        rowdetalle.cant = row.Cells["cant"].Value.ToString();
                        rowdetalle.codigo = row.Cells["item"].Value.ToString();
                        rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                        rowdetalle.medidas = row.Cells["medidas"].Value.ToString();
                        rowdetalle.madera = row.Cells["madera"].Value.ToString();
                        rowdetalle.det2 = row.Cells["piedra"].Value.ToString();
                        rowdetalle.acabado = "";    // row.Cells[""].Value.ToString();
                        rowdetalle.precio = row.Cells["precio"].Value.ToString();
                        rowdetalle.total = row.Cells["total"].Value.ToString();
                        rowdetalle.coment = row.Cells["coment"].Value.ToString();
                        rowdetalle.tienda = row.Cells["tda_item"].Value.ToString();
                        repcontrato.detalle.AdddetalleRow(rowdetalle);
                        //iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,piedra,na
                    }
                }
            }
            if (rb_servi.Checked == true)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Substring(0, 1) == letgru)    // "Z"
                    {
                        conClie.detalleRow rowdetalle = repcontrato.detalle.NewdetalleRow();
                        rowdetalle.id = "0";    // row.Cells["iddetacon"].Value.ToString();
                        rowdetalle.cant = row.Cells["cant"].Value.ToString();
                        rowdetalle.codigo = row.Cells["item"].Value.ToString();
                        rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                        rowdetalle.medidas = row.Cells["medidas"].Value.ToString();
                        rowdetalle.madera = row.Cells["madera"].Value.ToString();
                        rowdetalle.det2 = row.Cells["piedra"].Value.ToString();
                        rowdetalle.acabado = "";    // row.Cells[""].Value.ToString();
                        rowdetalle.precio = row.Cells["precio"].Value.ToString();
                        rowdetalle.total = row.Cells["total"].Value.ToString();
                        rowdetalle.coment = row.Cells["coment"].Value.ToString();
                        rowdetalle.tienda = row.Cells["tda_item"].Value.ToString();
                        repcontrato.detalle.AdddetalleRow(rowdetalle);
                        //iddetacon,item,cant,nombre,medidas,madera,precio,total,saldo,pedido,codref,coment,piedra,na
                    }
                }
            }
            // pagos del contrato
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                //string cpag = "select idpagamenti,fecha,moneda,montosol,dv,serie,numero,via,saldo,detalle from pagamenti where contrato=@cont";
                string cpag = "select a.idpagamenti,a.fecha,a.moneda,a.montosol,a.dv,a.serie,a.numero,a.via,a.saldo,a.detalle," +
                    "ifnull(p.medio,''),ifnull(p.operac,''),ifnull(p.importe,''),ifnull(p.codpag,''),ifnull(p.fpago,'') " +
                    "from pagamenti a left join adifactpag p ON p.tdvta = a.dv AND p.sdvta = a.serie AND p.ndvta = a.numero " +
                    "where a.contrato=@cont";
                MySqlCommand micon = new MySqlCommand(cpag, conn);
                micon.Parameters.AddWithValue("@cont", tx_codped.Text.Trim());
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach(DataRow row in dt.Rows)
                {
                    conClie.pagoscontRow pagoscont = repcontrato.pagoscont.NewpagoscontRow();
                    pagoscont.id = row.ItemArray[0].ToString();
                    pagoscont.fecha = row.ItemArray[1].ToString().Substring(0,10);
                    pagoscont.moneda = row.ItemArray[2].ToString();
                    pagoscont.montosol = row.ItemArray[3].ToString();
                    pagoscont.dv = row.ItemArray[4].ToString();
                    pagoscont.serie = row.ItemArray[5].ToString();
                    pagoscont.numero = row.ItemArray[6].ToString();
                    pagoscont.tipoPago = (row.ItemArray[7].ToString().Trim() == "") ? row.ItemArray[10].ToString() : row.ItemArray[7].ToString();
                    pagoscont.saldo = row.ItemArray[8].ToString();
                    pagoscont.detalle = row.ItemArray[9].ToString();
                    repcontrato.pagoscont.AddpagoscontRow(pagoscont);
                }
                da.Dispose();
                dt.Dispose();
            }
            else
            {
                MessageBox.Show("Se perdió conexión con el servidor", "Error de conectividad");
            }
            conn.Close();
            //
            return repcontrato;
        }
        public void PrintReport(string reportPath, string PrinterName, int cual)    // cual => 1=contrato, 2=condiciones
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc =
                                new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rptDoc.Load(reportPath);
            //_contrato.SetDataSource(_datosReporte);
            if (cual == 1)
            {
                conClie datos = generareporte();
                rptDoc.SetDataSource(datos);
            }
            rptDoc.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
            rptDoc.PrintOptions.PaperSize = PaperSize.PaperA4;
            if (PrinterName != "") rptDoc.PrintOptions.PrinterName = PrinterName;
            rptDoc.PrintToPrinter(1, false, 0, 0);
        }
        #endregion crystal
    }
}
