using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CComprobanteDetalle
    {
        public long ID { get; set; }
        public int Item { get; set; }
        public long ComprobanteID { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        //public string Serie { get; set; }
        //public int Correlativo { get; set; }
        public string TipoProductoCodigo { get; set; }
        public string Observacion { get; set; }
        public string DocumentoID { get; set; }
        public string UnidadMedidaCodigo { get; set; }
        public string UnidadMedidaDescripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoCodigoCliente { get; set; }
        public string ProductoCodigoSUNAT { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorVenta { get; set; }
        public decimal PrecioUnitarioItem { get; set; }
        public decimal ValorVentaItem { get; set; }
        public decimal ValorVentaItemXML { get; set; }
        /* PRECIO VENTA */
        public decimal PrecioVenta { get; set; }
        public string PrecioVentaCodigo { get; set; }
        public int CantidadUnidadMedida { get; set; }
        /* DESCUENTO */
        public decimal Descuento { get; set; }
        public decimal DescuentoBase { get; set; }
        public decimal DescuentoMonto { get; set; }
        public string DescuentoPorcentaje { get; set; }
        public int DescuentoIndicador { get; set; }
        public string DescuentoCargoCodigo { get; set; }
        /* OTROS CARGOS (NO HAY DATOS)*/
        public decimal Cargo { get; set; }
        public decimal CargoBase { get; set; }
        public decimal CargoItem { get; set; }
        public decimal CargoPorcentaje { get; set; }
        public string CargoIndicador { get; set; }
        public string CargoCargoCodigo { get; set; }
        public string CargoCodigo { get; set; }
        /* PERCEPCION */
        public decimal Percepcion { get; set; }
        public decimal PercepcionBase { get; set; }
        public decimal PercepcionPorcentaje { get; set; }
        public string PercepcionIndicador { get; set; }
        public string PercepcionCargoCodigo { get; set; }
        /* PERCEPCION REGLAS (SOLO PARA RECUPERAR) */
        public decimal PercepcionCantidadUmbral { get; set; }
        public decimal PercepcionMontoUmbral { get; set; }
        /* IMPUESTOS */
        public decimal MontoTributo { get; set; }
        public decimal ISC { get; set; }
        public decimal ISCBase { get; set; }
        public decimal ISCPorcentaje { get; set; }
        public string TipoSistemaISCCodigo { get; set; }
        public decimal PrecioUnitarioSugerido { get; set; }
        public decimal ICBPER { get; set; }
        public int ICBPECantidad { get; set; }
        public decimal ICBPERSubTotal { get; set; }
        public string TipoAfectacionIGVCodigo { get; set; }
        public decimal IGVBase { get; set; }
        public decimal IGV { get; set; }
        public decimal IGVPorcentaje { get; set; }
        public decimal ImporteTotal { get; set; }
        public int Kit { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Peso { get; set; }
        public decimal PesoTotal { get; set; }
        public int Control { get; set; }
        public int PrecioCodigo { get; set; }
        public bool EsAnticipo { get; set; }
        public decimal ImporteTotalReferencia { get; set; }
        public int CantidadReferencial { get; set; }
        public decimal DescuentoGlobal { get; set; }
        public decimal ValorVentaNeto { get; set; }
        public decimal ValorVentaNetoXML { get; set; }
        public decimal ISCUnitario { get; set; }
        public decimal ISCNeto { get; set; }
        public decimal ICBPERItem { get; set; }
        public decimal DescuentoCargo { get; set; }
        public decimal DescuentoCargoGravado { get; set; }
        public decimal CargoTotal { get; set; }
        public decimal CargoNeto { get; set; }
        public decimal ISCMonto { get; set; }
        public bool PrecioUnitarioRecuperado { get; set; }
        public string UUID { get; set; }
        public bool BANDERA_CONCURRENCIA { get; set; }
        public bool BANDERA_TIPOAFECTACIONIGVAGREGARITEMDETALLE { get; set; }
        public bool BANDERA_DETALLEREEMPLAZADO { get; set; }
        public bool BANDERA_DETALLERECUPERADO { get; set; }
        public bool BANDERA_ITEMDETALLADO { get; set; }
        public int Stock { get; set; }
        public decimal Ganancia { get; set; }
        public decimal IGVNeto { get; set; }

        public List<CProductoCodigoSerie> ListaSeries { get; set; }
        public List<ProductoPrecioDTO> ListaPrecios { get; set; }

        public CComprobanteDetalle()
        {
            ID = 0;
            Item = 0;
            ComprobanteID = 0;
            TipoDocumentoCodigo = "";
            //Serie = "";
            //Correlativo = 0;
            TipoProductoCodigo = "0";
            Observacion = "";
            DocumentoID = "";
            UnidadMedidaCodigo = "";
            UnidadMedidaDescripcion = "";
            Cantidad = 0;
            Descripcion = "";
            ProductoCodigo = "";
            ProductoCodigoCliente = "";
            ProductoCodigoSUNAT = "";
            PrecioUnitario = 0;
            ValorUnitario = 0;
            ValorVenta = 0;
            PrecioUnitarioItem = 0;
            ValorVentaItem = 0;
            PrecioVenta = 0;
            PrecioVentaCodigo = "";
            CantidadUnidadMedida = 1;
            Descuento = 0;
            DescuentoBase = 0;
            DescuentoPorcentaje = "0";
            DescuentoIndicador = 0;
            DescuentoCargoCodigo = "";
            Cargo = 0;
            CargoBase = 0;
            CargoPorcentaje = 0;
            CargoIndicador = "";
            CargoCodigo = "";
            Percepcion = 0;
            PercepcionBase = 0;
            PercepcionPorcentaje = 0;
            PercepcionIndicador = "";
            PercepcionCargoCodigo = "";
            MontoTributo = 0;
            ISC = 0;
            ISCBase = 0;
            ISCPorcentaje = 0;
            TipoSistemaISCCodigo = "";
            ICBPER = 0;
            ICBPECantidad = 0;
            ICBPERSubTotal = 0;
            TipoAfectacionIGVCodigo = "";
            IGVBase = 0;
            IGV = 0;
            IGVPorcentaje = 0;
            ImporteTotal = 0;
            Kit = 0;
            PrecioCompra = 0;
            Peso = 0;
            PesoTotal = 0;
            PrecioCodigo = 0;

            ListaSeries = new List<CProductoCodigoSerie>();
        }
    }
}
