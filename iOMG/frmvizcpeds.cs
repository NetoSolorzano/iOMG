using System;
using System.Windows.Forms;

namespace iOMG
{
    public partial class frmvizcpeds : Form
    {
        pedsclts _datosReporte;             // dataset

        public frmvizcpeds()
        {
            InitializeComponent();
        }

        public frmvizcpeds(pedsclts datos) : this()
        {
            _datosReporte = datos;
        }

        private void crystalReportViewer_Load(object sender, EventArgs e)
        {
            //
        }

        private void frmvizcpeds_Load(object sender, EventArgs e)
        {
            if (_datosReporte.cabeza_pedclt.Rows.Count > 0)
            {
                pedsclte _pedidoc = new pedsclte();
                _pedidoc.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _pedidoc;
            }
            if (_datosReporte.det_lispedidos.Rows.Count > 0)
            {
                lispedclts _lisped = new lispedclts();
                _lisped.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _lisped;
            }
            if (_datosReporte.cab_reping.Rows.Count > 0)
            {
                repingresos _ingre = new repingresos();
                _ingre.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _ingre;
            }
            if (_datosReporte.cab_repsal.Rows.Count > 0)
            {
                repsalidas _salidas = new repsalidas();
                _salidas.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _salidas;
            }
            //MessageBox.Show("aca estuve");
        }
    }
}
