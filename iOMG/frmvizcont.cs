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
            //MessageBox.Show(_datosReporte.cabecera.Rows[0]["tipoCont"].ToString(),"que dice aca");
            if (_datosReporte.cabecera.Rows[0]["tipoCont"].ToString().Trim() == "1")
            {
                ContratoG _contrato = new ContratoG();
                _contrato.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _contrato;
            }
            if (_datosReporte.cabecera.Rows[0]["tipoCont"].ToString().Trim() == "2")
            {
                ContratoE _contrato = new ContratoE();
                _contrato.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _contrato;
            }
        }
    }
}
