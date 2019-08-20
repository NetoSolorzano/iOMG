﻿using System;
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
                ContratoG _contrato = new ContratoG();
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
        }
    }
}
