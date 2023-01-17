using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CMovimientoCuenta
    {
        public int IdMovimiento { get; set; }
        public int TipoCuentaCodigo { get; set; }
        public string CuentaNumero { get; set; }
        public string CuentaNombre { get; set; }
        public int TipoIngresoEgreso { get; set; }
        public decimal MontoPagar { get; set; }
        public string MonedaCodigo { get; set; }
        public string Observacion { get; set; }
        public string NumeroOperacion { get; set; }
        public string TipoDocIdentidadCodigo { get; set; }
        public string NumeroDocIdentidad { get; set; }
        public string Origen { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        public string Usuario { get; set; }
        public int IdOrigen { get; set; }
        public int Estado { get; set; }
        public string FechaPago { get; set; }
        public int SucursalId { get; set; }
        public decimal Pago { get; set; }
        public decimal Pagado { get; set; }
        public decimal Saldo { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaVencimiento { get; set; }
        public int PlazoDias { get; set; }
        public string CondicionComprobante { get; set; }
        public string Condicion { get; set; }
        public decimal Vuelto { get; set; }
        public string CondicionPagoModificado { get; set; }
        public string CuentaNumeroModificado { get; set; }
        public string CuentaTipoModificado { get; set; }
        public string CuentaValor { get; set; }

        public CMovimientoCuenta()
        {
            IdMovimiento = 0;
            TipoCuentaCodigo = 0;
            CuentaNumero = "";
            CuentaNombre = "";
            TipoIngresoEgreso = 0;
            MontoPagar = 0;
            MonedaCodigo = "";
            Observacion = "";
            NumeroOperacion = "";
            TipoDocIdentidadCodigo = "";
            NumeroDocIdentidad = "";
            Origen = "";
            TipoDocumentoCodigo = "";
            Serie = "";
            Correlativo = 0;
            Usuario = "";
            IdOrigen = 0;
            Estado = 1;
            FechaPago = "";
            SucursalId = 0;
            Pago = 0;
            Pagado = 0;
            Saldo = 0;
            FechaVencimiento = "";      //DateTime.Today;
            PlazoDias = 0;
            CondicionComprobante = "";
            Condicion = "";
            Vuelto = 0;
            CondicionPagoModificado = "";
            CuentaNumeroModificado = "";
            CuentaTipoModificado = "";
            CuentaValor = "";
        }
        public CMovimientoCuenta(int pTipoCuentaCodigo, string pCuentaNumero, string pCuentaNombre, int pTipoIngresoEgreso, decimal pMontoPagar, string pMonedaCodigo, string pObservacion,
            string pNumeroOperacion, string pTipoDocIdentidadCodigo, string pNumeroDocIdentidad, string pOrigen, string pTipoDocumentoCodigo, string pSerie,
            int pCorrelativo, string pUsuario, int pIdOrigen, int pEstado, string pFechaPago, int pSucursalId,
            decimal pTotal, decimal pPagado, decimal pSaldo, string pFechaVencimiento, int pPlazoDias, string pCondicion, decimal pVuelto,
            string pCondicionPagoModicado, string pCuentaNumeroMoficado, string pCuentaTipoModificado)
        {
            TipoCuentaCodigo = pTipoCuentaCodigo;
            CuentaNumero = pCuentaNumero;
            CuentaNombre = pCuentaNombre;
            TipoIngresoEgreso = pTipoIngresoEgreso;
            MontoPagar = pMontoPagar;
            MonedaCodigo = pMonedaCodigo;
            Observacion = pObservacion;
            NumeroOperacion = pNumeroOperacion;
            TipoDocIdentidadCodigo = pTipoDocIdentidadCodigo;
            NumeroDocIdentidad = pNumeroDocIdentidad;
            Origen = pOrigen;
            TipoDocumentoCodigo = pTipoDocumentoCodigo;
            Serie = pSerie;
            Correlativo = pCorrelativo;
            Usuario = pUsuario;
            IdOrigen = pIdOrigen;
            Estado = pEstado;
            FechaPago = pFechaPago;
            SucursalId = pSucursalId;
            Pago = pTotal;
            Pagado = pPagado;
            Saldo = pSaldo;
            FechaVencimiento = pFechaVencimiento;
            PlazoDias = pPlazoDias;
            Condicion = pCondicion;
            Vuelto = pVuelto;
            CondicionPagoModificado = pCondicionPagoModicado;
            CuentaNumeroModificado = pCuentaNumeroMoficado;
            CuentaTipoModificado = pCuentaTipoModificado;
            // aca no falta CuentaValor ????
        }
    }
}
