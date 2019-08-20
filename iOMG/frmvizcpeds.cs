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
                /*
                 * replispedclts _lisped = new replispedclts();
                */
                lispedclts _lisped = new lispedclts();
                _lisped.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _lisped;
            }
            //MessageBox.Show("aca estuve");
        }
    }
}
