using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;


namespace iOMG
{
    public partial class pedsclients : Form
    {
        static string nomform = "pedsclients";      // nombre del formulario
        string asd = iOMG.Program.vg_user;          // usuario conectado al sistema
        string colback = iOMG.Program.colbac;       // color de fondo
        string colpage = iOMG.Program.colpag;       // color de los pageframes
        string colgrid = iOMG.Program.colgri;       // color de las grillas
        string colstrp = iOMG.Program.colstr;       // color del strip
        static string nomtab = "pedidos";
        //public int totfilgrid, cta, cuenta, pageCount;      // variables para impresion
        //public string perAg = "";             // permisos agregar
        //public string perMo = "";             // permisos modificar
        //public string perAn = "";             // permisos anular
        //public string perIm = "";             // permisos imprimir
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
        string tipede = "";             // tipo de pedido de clientes por defecto
        string tiesta = "";             // estado inicial por defecto del pedido de clientes
        string escambio = "";           // estados de pedido de clientes que admiten modificar el pedido
        string estpend = "";            // estado de pedido de clientes con articulos pendientes de recibir
        string estcomp = "";            // estado de pedido de clientes con articulos recibidos en su totalidad
        string estenv = "";             // estado de pedido de clientes enviado a producción
        string estanu = "";             // estado de pedido de clientes anulado
        string estcer = "";             // estado de pedido de clientes cerrado tal como esta, ya no se atiende
        //string canovald2 = "";          // captitulos donde no se valida det2
        //string conovald2 = "";          // valor por defecto al no validar det2
        //string letpied = "";            // letra identificadora de piedra en detalle2
        string estman = "";             // estados que se pueden seleccionar manualmente
        int indant = -1;                // indice anterior al cambio en el combobox de estado
        //string cn_adm = "";               // codigo nivel usuario admin
        //string cn_sup = "";               // codigo nivel usuario superusuario
        //string cn_est = "";               // codigo nivel usuario estandar
        //string cn_mir = "";               // codigo nivel usuario solo mira
        string cliente = Program.cliente;    // razon social para los reportes
        libreria lib = new libreria();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        DataTable dtg = new DataTable();
        DataTable dtu = new DataTable();    // dtg primario, original con la carga del 
        DataTable dttaller = new DataTable();   // combo taller de fabric.

        public pedsclients()
        {
            InitializeComponent();
        }
        private void pedsclients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1 && Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
            {
                if (tx_cliente.Focused == true)
                {
                    para1 = "??";
                    para2 = "??";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue1;    // tipdoc y numero
                            //ayu2.ReturnValue0;    // id del cliente
                            //ayu3.ReturnValue2;    // nombre
                            tx_cliente.Text = ayu2.ReturnValue2;
                        }
                    }
                }
                if (tx_cont.Focused == true)
                {
                    para1 = "??";
                    para2 = "??";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))
                        {
                            //ayu2.ReturnValue1;    // numero cont
                            //ayu2.ReturnValue0;    // id del contrato
                            //ayu3.ReturnValue2;    // nombre del cliente
                            tx_cont.Text = ayu2.ReturnValue1;
                            tx_cliente.Text = ayu2.ReturnValue2;
                        }
                    }
                }
                if (cmb_fam.Focused == true || cmb_mod.Focused == true || cmb_mad.Focused == true || cmb_tip.Focused == true ||
                    cmb_det1.Focused == true || cmb_aca.Focused == true || cmb_tal.Focused == true ||
                    cmb_det2.Focused == true || cmb_det3.Focused == true)
                {
                    para1 = "contratos";
                    para2 = "detalle";
                    para3 = "contrato";
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))       // ME QUEDE ACA .... CODIGOS ESTANDARES Y ADICIONALES ?? COMO SE MANEJARAN ACA?
                        {
                            //ayu2.ReturnValue1;    // codigo del articulo
                            //ayu2.ReturnValue0;    // id del articulo en el contrato
                            //ayu3.ReturnValue2;    // saldo por pedir
                            cmb_fam.SelectedIndex = cmb_fam.FindString(ayu2.ReturnValue1.Substring(0, 1));
                            cmb_mod.SelectedIndex = cmb_mod.FindString(ayu2.ReturnValue1.Substring(1, 3));
                            cmb_mad.SelectedIndex = cmb_mad.FindString(ayu2.ReturnValue1.Substring(4, 1));
                            //cmb_mad_SelectionChangeCommitted(null, null);
                            cmb_tip.SelectedIndex = cmb_tip.FindString(ayu2.ReturnValue1.Substring(5, 2));
                            cmb_det1.SelectedIndex = cmb_det1.FindString(ayu2.ReturnValue1.Substring(7, 2));
                            //cmb_det1_SelectionChangeCommitted(null, null);
                            cmb_aca.SelectedIndex = cmb_aca.FindString(ayu2.ReturnValue1.Substring(9, 1));
                            //cmb_aca_SelectionChangeCommitted(null, null);
                            if (tx_dat_orig.Text == "") cmb_tal.SelectedIndex = cmb_tal.FindString(ayu2.ReturnValue1.Substring(10, 2));
                            cmb_det2.SelectedIndex = cmb_det2.FindString(ayu2.ReturnValue1.Substring(12, 3));
                            //cmb_det2_SelectionChangeCommitted(null, null);
                            cmb_det3.SelectedIndex = cmb_det3.FindString(ayu2.ReturnValue1.Substring(15, 3));
                            armani();
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }

}
