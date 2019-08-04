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
            pedsclte _pedidoc = new pedsclte();
            _pedidoc.SetDataSource(_datosReporte);
            crystalReportViewer1.ReportSource = _pedidoc;
            //MessageBox.Show("aca estuve");
        }
    }
}
