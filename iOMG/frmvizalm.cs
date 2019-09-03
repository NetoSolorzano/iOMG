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
                rep_stock _resumen = new rep_stock();           // reporte crystal
                _resumen.SetDataSource(_datosreporte);
                crystalReportViewer1.ReportSource = _resumen;
            }

        }
    }
}
