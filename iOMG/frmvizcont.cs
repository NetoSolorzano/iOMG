using System;
using System.Windows.Forms;

namespace iOMG
{
    public partial class frmvizcont : Form
    {
        conClie _datosReporte;

        private frmvizcont()
        {
            InitializeComponent();
        }

        public frmvizcont(conClie datos): this()
        {
            _datosReporte = datos;
        }

        private void frmvizcont_Load(object sender, EventArgs e)
        {
            ContratoG _contrato = new ContratoG();
            _contrato.SetDataSource(_datosReporte);
            crystalReportViewer1.ReportSource = _contrato;
        }
    }
}
