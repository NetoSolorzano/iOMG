using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CProductoCodigoSerie
    {
        public int Item { get; set; }
        public int CodigoProductoSerie { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoSerie { get; set; }
        public int SucursalId { get; set; }
        public int Estado { get; set; }
        public int IngresoId { get; set; }
        public int OrigenIngreso { get; set; }
        public string VentaTipoComprobante { get; set; }
        public string VentaSerie { get; set; }
        public int VentaCorrelativo { get; set; }
        public int EstadoVendido { get; set; }

        public CProductoCodigoSerie()
        {
            Item = 0;
            CodigoProductoSerie = 0;
            CodigoProducto = string.Empty;
            CodigoSerie = string.Empty;
            SucursalId = 0;
            Estado = 0;
            IngresoId = 0;
            OrigenIngreso = 0;
            VentaTipoComprobante = string.Empty;
            VentaSerie = string.Empty;
            VentaCorrelativo = 0;
            EstadoVendido = 0;
        }

        public CProductoCodigoSerie(int pItem, int pCodigoProductoSerie, string pCodigoProducto, string pCodigoSerie, int pSucursalId, int pEstado,
            int pIngresoId, int pOrigenIngreso, string pVentaTipoComprobante, string pVentaSerie, int pVentaCorrelativo, int pEstadoVendido)
        {
            Item = pItem;
            CodigoProductoSerie = pCodigoProductoSerie;
            CodigoProducto = pCodigoProducto;
            CodigoSerie = pCodigoSerie;
            SucursalId = pSucursalId;
            Estado = pEstado;
            IngresoId = pIngresoId;
            OrigenIngreso = pOrigenIngreso;
            VentaTipoComprobante = pVentaTipoComprobante;
            VentaSerie = pVentaSerie;
            VentaCorrelativo = pVentaCorrelativo;
            EstadoVendido = pEstadoVendido;
        }

    }
}
