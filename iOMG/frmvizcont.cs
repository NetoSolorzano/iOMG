using System;
using System.Data;
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
            if (_datosReporte.cabecera.Rows.Count > 0 && _datosReporte.cabecera.Rows[0]["tipoCont"].ToString().Trim() == "1")
            {
                ContratoI _contrato = new ContratoI();  // antes ContratoG (23/03/2020)
                _contrato.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _contrato;
            }
            if (_datosReporte.cabecera.Rows.Count > 0 && _datosReporte.cabecera.Rows[0]["tipoCont"].ToString().Trim() == "2")
            {
                ContratoE _contrato = new ContratoE();
                _contrato.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _contrato;
            }
            if (_datosReporte.rescont_cab.Rows.Count > 0)
            {
                res_cont1 _resumen = new res_cont1();
                _resumen.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _resumen;
            }
            if (_datosReporte.liscont_cab.Rows.Count > 0)
            {
                liscontratos _listado = new liscontratos();
                _listado.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = _listado;
            }
            if (_datosReporte.repvtas_cab.Rows.Count > 0)
            {
                DataRow row = _datosReporte.Tables["repvtas_cab"].Rows[0];
                if (row["modo"].ToString() == "resumen")
                {
                    repvtas_resumen _ventas = new repvtas_resumen();
                    _ventas.SetDataSource(_datosReporte);
                    crystalReportViewer1.ReportSource = _ventas;
                }
                if (row["modo"].ToString() == "listado" && row["nudoclte"].ToString() == "")
                {
                    repvtas_listado _ventas = new repvtas_listado();
                    _ventas.SetDataSource(_datosReporte);
                    crystalReportViewer1.ReportSource = _ventas;
                }
                if (row["modo"].ToString() == "listado" && row["nudoclte"].ToString() != "")
                {
                    repvtas_xclte _ventas = new repvtas_xclte();
                    _ventas.SetDataSource(_datosReporte);
                    crystalReportViewer1.ReportSource = _ventas;
                }
            }
        }
    }
}
