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
            if (_datosreporte.cab_stock.Rows.Count > 0)
            {
                if (_datosreporte.cab_stock.Rows[0][""].ToString() == "Checked")
                {
                    rep_stock _resumen = new rep_stock();           // stock valorizado
                    _resumen.SetDataSource(_datosreporte);
                    crystalReportViewer1.ReportSource = _resumen;
                }
                else
                {
                    rep_stock_sinvalor _resumen = new rep_stock_sinvalor();           // stock sin valorizar
                    _resumen.SetDataSource(_datosreporte);
                    crystalReportViewer1.ReportSource = _resumen;
                }
            }

        }
    }
}
