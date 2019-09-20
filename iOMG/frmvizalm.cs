using System.Windows.Forms;

namespace iOMG
{
    public partial class frmvizalm : Form
    {
        repsalmacen _datosreporte;

        private frmvizalm()
        {
            InitializeComponent();
        }

        public frmvizalm(repsalmacen datos) : this()
        {
            _datosreporte = datos;
        }

        private void crystalReportViewer1_Load(object sender, System.EventArgs e)
        {
            // veremos donde va
        }

        private void frmvizalm_Load(object sender, System.EventArgs e)
        {
            // reporte de stock
            if (_datosreporte.cab_stock.Rows.Count > 0)
            {
                if (_datosreporte.cab_stock.Rows[0]["valorizado"].ToString() == "Checked")
                {
                    rep_stock _resumen = new rep_stock();                       // stock valorizado
                    _resumen.SetDataSource(_datosreporte);
                    crystalReportViewer1.ReportSource = _resumen;
                }
                else
                {
                    rep_stock_sinvalor _resumen = new rep_stock_sinvalor();     // stock sin valorizar
                    _resumen.SetDataSource(_datosreporte);
                    crystalReportViewer1.ReportSource = _resumen;
                }
            }
            // reportes de reservas
            if (_datosreporte.cab_lisReservas.Rows.Count > 0)
            {
                list_reservas _resumen = new list_reservas();
                _resumen.SetDataSource(_datosreporte);
                crystalReportViewer1.ReportSource = _resumen;
            }
            // reporte de kardex
            if (_datosreporte.cab_kardex.Rows.Count > 0)
            {
                rep_kardex _resumen = new rep_kardex();
                _resumen.SetDataSource(_datosreporte);
                crystalReportViewer1.ReportSource = _resumen;
            }
            // reporte de salidas de almacen
            if (_datosreporte.cab_salidas.Rows.Count > 0)
            {
                rep_alm_salidas _resumen = new rep_alm_salidas();
                _resumen.SetDataSource(_datosreporte);
                crystalReportViewer1.ReportSource = _resumen;
            }
        }
    }
}
