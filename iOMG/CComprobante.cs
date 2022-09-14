using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CComprobante
    {
        public long ID { get; set; }
        public long IdRepositorio { get; set; }
        public bool AplicaContingencia { get; set; }
        public bool AplicaAnticipo { get; set; }
        public bool AplicaOtroSistema { get; set; }

        public bool EsBorrador { get; set; }
        public string DocumentoID { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaEmision { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaRegistro { get; set; }
        public string TipoOperacionCodigo { get; set; }
        public string TipoNotaCreditoCodigo { get; set; }
        public string TipoNotaDebitoCodigo { get; set; }
        public string TipoGuiaRemisionCodigo { get; set; }
        public string TipoDocumentoCodigoModificado { get; set; }
        public string SerieModificado { get; set; }
        public string CorrelativoModificado { get; set; }
        public string CondicionEstado { get; set; }
        /*** LEYENDA ANALIZAR LEYENDA ***/
        public string ImporteTotalTexto { get; set; }
        public string TransferenciaDescripcion { get; set; }
        public string PercepcionDescripcion { get; set; }
        public string DetraccionDescripcion { get; set; }
        public string AmazoniaBienesDescripcion { get; set; }
        public string AmazoniaServiciosDescripcion { get; set; }
        public string AmazoniaContratosDescripcion { get; set; }
        public string CodigoInternoDescripcion { get; set; }
        public string EmisorItineranteDescripcion { get; set; }
        public string AgenciaPaqueteDescripcion { get; set; }
        /*** OBSERVACION CAMBIO ***/
        public string Observacion { get; set; }
        public string MonedaCodigo { get; set; }
        public string TipoCambio { get; set; }
        /*** ORDEN Y GUIA ***/
        public bool DesdeOrden { get; set; }
        public string GuiaNumero { get; set; }
        /*** SERVICIO PUBLICO  (NO ENVIAR)***/
        public string ServicioPublicoNumero { get; set; }
        public string ServicioPublicoTipo { get; set; }
        public string ServicioPublicoCodigo { get; set; }
        public string ServicioPublicoCodigoTarifa { get; set; }
        /*** DOCUMENTO REFERENCIA (NOTA CREDITO - GUiA)***/
        public string ReferenciaNumeroDocumento { get; set; }
        public string ReferenciaTipoDocumento { get; set; }
        /*** EMISOR ***/
        public string EmisorDocumento { get; set; }
        public string EmisorRuc { get; set; }
        public string EmisorNombreComercial { get; set; }
        public string EmisorRazonSocial { get; set; }
        public string EmisorProvincia { get; set; }
        public string EmisorDepartamento { get; set; }
        public string EmisorDistrito { get; set; }
        public string EmisorUbigeo { get; set; }
        public string EmisorCodigoPais { get; set; }
        public string EmisorDireccion { get; set; }
        public string EmisorAdicionales { get; set; }
        /*** CLIENTE ***/
        public string ClienteTipoDocIdentidadCodigo { get; set; }
        public string ClienteNumeroDocIdentidad { get; set; }
        public string ClienteNombreRazonSocial { get; set; }
        public string ClienteDireccion { get; set; }
        public string CorreoElectronicoPrincipal { get; set; }
        public string ClienteContacto { get; set; }
        public string ClienteTelefono { get; set; }
        /*** DETRACCION ***/
        public decimal Detraccion { get; set; }
        public string DetraccionCuenta { get; set; }
        public string DetraccionTipoOperacion { get; set; }
        public decimal DetraccionPorcentaje { get; set; }
        public string BienServicioCodigo { get; set; }
        /*** FORMA Y CONDICICION DE PAGO ***/
        public string CondicionPago { get; set; }
        public decimal CreditoTotal { get; set; }
        /*** PERCEPCION ***/
        public decimal Percepcion { get; set; }
        public string PercepcionRegimen { get; set; }
        public decimal PercepcionFactor { get; set; }
        public decimal PercepcionTotal { get; set; }
        public decimal PercepcionBaseImponible { get; set; }
        public decimal Retencion { get; set; }
        public decimal MontoRetencion { get; set; }
        public decimal DescuentoGlobal { get; set; }
        /*** DESCUENTOS GLOBAL ***/
        public int DescuentoIndicador { get; set; }
        public decimal DescuentoGlobalMonto { get; set; }
        public decimal DescuentoGlobalMontoBase { get; set; }
        public decimal DescuentoGlobalPorcentaje { get; set; }
        public decimal DescuentoGlobalValor { get; set; }
        public string DescuentoGlobalIndicadorDescuento { get; set; }
        public string DescuentoGlobalCodigoMotivo { get; set; }
        /*** DESCUENTOS GLOBAL NO GRAVADO ***/
        public decimal DescuentoGlobalNGMonto { get; set; }
        public decimal DescuentoGlobalNGMontoBase { get; set; }
        public int DescuentoGlobalNGIndicadorDescuento { get; set; }
        public string DescuentoGlobalNGCodigoMotivo { get; set; }
        public decimal DescuentoNGMonto { get; set; }
        public decimal AnticiposGravado { get; set; }
        public decimal AnticiposExonerado { get; set; }
        public decimal AnticiposInafecto { get; set; }
        /*** DESCUENTO ANTICIPO ***/
        public decimal AnticipoMonto { get; set; }
        public decimal AnticipoMontoBase { get; set; }
        public string AnticipoCodigo { get; set; }
        /*** CARGO GLOBAL ***/
        public decimal CargoGlobalMonto { get; set; }
        public decimal CargoGlobalMontoBase { get; set; }
        public decimal CargoGlobalPorcentaje { get; set; }
        public string CargoGlobalIndicadorCargos { get; set; }
        public string CargoGlobalCodigoMotivo { get; set; }
        /*** IMPUESTOS ***/
        public decimal ImpuestoTotal { get; set; }
        public decimal ISC { get; set; }
        public decimal ISCBase { get; set; }
        public decimal IGV { get; set; }
        public decimal IGVPorcentaje { get; set; }
        public decimal Gravado { get; set; }
        public decimal ICBPER { get; set; }
        public decimal Exonerada { get; set; }
        public decimal Inafecto { get; set; }
        public decimal Exportacion { get; set; }

        public decimal ExoneradaXML { get; set; }
        public decimal InafectoXML { get; set; }
        public decimal ExportacionXML { get; set; }

        public decimal Gratuito { get; set; }
        public decimal GratuitoGravado { get; set; }
        /*** TOTALES MONETARIO ***/
        public decimal TotalImporteVenta { get; set; }
        public decimal TotalPrecioVenta { get; set; }
        public decimal TotalValorVenta { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalOtrosCargos { get; set; }
        public decimal TotalAnticipos { get; set; }
        public decimal TotalPago { get; set; }
        public decimal PendientePago { get; set; }
        public bool EnviarCorreo { get; set; }
        public bool TienePDF { get; set; }
        public bool TieneXML { get; set; }
        public bool TieneCDR { get; set; }
        public int FormatoPDF { get; set; }
        public string VistaDocumento { get; set; }
        public string SunatRespuesta { get; set; }
        public string SunatMensaje { get; set; }
        public int Sucursal { get; set; }
        public string Usuario { get; set; }
        public string Vendedor { get; set; }
        public string VendedorNombre { get; set; }
        public string CanalVenta { get; set; }
        /* PARA FACTURA */

        public string AlojamientoTipoDocIdentidadCodigo { get; set; }
        public string AlojamientoNumeroDocIdentidad { get; set; }
        public string AlojamientoNombreRazonSocial { get; set; }
        public string AlojamientoPaisDocEmisor { get; set; }

        public string PaisResidencia { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaIngresoPais { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaIngresoEstablecimiento { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaSalidaEstablecimiento { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaConsumo { get; set; }
        public int DiasPermanencia { get; set; }
        /* GUIA REMISION */
        public decimal Peso { get; set; }
        public decimal PesoTotal { get; set; }
        public int Bultos { get; set; }
        public string MotivoTrasladoDescripcion { get; set; }
        public string MotivoTrasladoCodigo { get; set; }
        public string ModalidadTrasladoCodigo { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaTraslado { get; set; }
        public string TransportistaNumeroDocIdentidad { get; set; }
        public string TransportistaNombreRazonSocial { get; set; }
        public string TransportistaTipoDocIdentidadCodigo { get; set; }
        public string PlacaVehiculo { get; set; }
        public string ConductorNumeroDocIdentidad { get; set; }
        public string ConductorTipoDocIdentidadCodigo { get; set; }
        public string UbigeoLlegada { get; set; }
        public string DireccionLlegada { get; set; }
        public string NOMBRE_UBIGEOLLEGADA { get; set; }
        public string UbigeoPartida { get; set; }
        public string DireccionPartida { get; set; }
        public string NOMBRE_UBIGEOPARTIDA { get; set; }
        /* OTROS DATOS */
        public bool Anticipo { get; set; }
        public bool EstadoContingencia { get; set; }
        public int Baja { get; set; }
        public int Leyenda { get; set; }
        public int LeyendaBoleta { get; set; }
        public int LeyendaFactura { get; set; }
        public string MotivoBaja { get; set; }
        public int OrigenSistema { get; set; }
        public int TipoPrecio { get; set; }
        public string Ubigeo { get; set; }
        public string Firma { get; set; }
        public string sIdentificadorBaja { get; set; }
        public string sIdentificadorResumen { get; set; }
        public int Estado { get; set; }
        public int DocAdicionalCodigo { get; set; }
        public string DocAdicionalDetalle { get; set; }
        public bool PagosMultiples { get; set; }
        public bool EsPrico { get; set; }
        /* agragados finales */
        public bool AplicaStockNegativo { get; set; }
        public bool ModificacionDePrecio { get; set; }
        public decimal DescuentoGlobalNGPorcentaje { get; set; }
        public int CantidadDecimales { get; set; }
        public bool AgentePercepcion { get; set; }
        public bool PermisoProductoSerie { get; set; }
        public int SituacionPagoCodigo { get; set; }
        public string OrdenNumero { get; set; }
        public int ClienteTipoSunat { get; set; }
        public int CondicionComercialIndicador { get; set; }
        public int TotalCuotas { get; set; }
        public string UUID { get; set; }
        public bool BANDERA_CONCURRENCIA { get; set; }
        public bool BANDERA_DIRECCIONPARTIDAEDICION { get; set; }
        public bool BANDERA_GANANCIAVERIFICADA { get; set; }
        public bool BANDERA_ERRORESGANANCIA { get; set; }
        public int CONTADOR_BUSCAPRODUCTO { get; set; }
        public int CONTADOR_CLICKEMITIR { get; set; }
        public bool EstadoOtroSistema { get; set; }
        public int ClasePrecioCodigo { get; set; }
        public int TipoBusquedaProductoCodigo { get; set; }
        public int OperacionNoGravada { get; set; }
        public decimal ImpuestoVarios { get; set; }
        public decimal TotalImporteVentaCelular { get; set; }
        public decimal TotalImporteVentaReferencia { get; set; }
        public int RetencionPorcentaje { get; set; }
        public decimal TotalRetencion { get; set; }
        public int PermitirCuotas { get; set; }


        /* LISTAS */
        public List<CComprobanteDetalle> ListaDetalles { get; set; }
        public List<CAnticipo> ListaAnticipos { get; set; }
        public List<CCreditoCuota> ListaCuotas { get; set; }
        public List<CGuiasComprobante> ListaGuias { get; set; }
        public List<CMovimientoCuenta> ListaMovimientos { get; set; }
        public List<CDocumentoRelacionado> ListaDocumentosRelacionados { get; set; }
        public List<CCondicionComercial> ListaCondicionesComerciales { get; set; }

        public CComprobante()
        {
            ID = 0;
            EsBorrador = false;
            IdRepositorio = 0;
            DocumentoID = "";
            TipoDocumentoCodigo = "";
            Serie = "0000";
            Correlativo = 0;
            FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy");
            TipoOperacionCodigo = "";
            TipoNotaCreditoCodigo = "";
            TipoNotaDebitoCodigo = "";
            TipoGuiaRemisionCodigo = "";
            TipoDocumentoCodigoModificado = "";
            SerieModificado = "";
            CorrelativoModificado = "";
            CondicionEstado = "";
            ImporteTotalTexto = "";
            TransferenciaDescripcion = "";
            PercepcionDescripcion = "";
            DetraccionDescripcion = "";
            AmazoniaBienesDescripcion = "";
            AmazoniaServiciosDescripcion = "";
            AmazoniaContratosDescripcion = "";
            CodigoInternoDescripcion = "";
            EmisorItineranteDescripcion = "";
            AgenciaPaqueteDescripcion = "";
            Observacion = "";
            MonedaCodigo = "PEN";
            TipoCambio = "0.00";
            DesdeOrden = false;
            GuiaNumero = "";
            ServicioPublicoNumero = "";
            ServicioPublicoTipo = "";
            ServicioPublicoCodigo = "";
            ServicioPublicoCodigoTarifa = "";
            ReferenciaNumeroDocumento = "";
            ReferenciaTipoDocumento = "";
            EmisorDocumento = "";
            EmisorRuc = "";
            EmisorNombreComercial = "";
            EmisorRazonSocial = "";
            EmisorProvincia = "";
            EmisorDepartamento = "";
            EmisorDistrito = "";
            EmisorUbigeo = "";
            EmisorCodigoPais = "";
            EmisorDireccion = "";
            ClienteTipoDocIdentidadCodigo = "";
            ClienteNumeroDocIdentidad = "";
            ClienteNombreRazonSocial = "";
            ClienteDireccion = "";
            Detraccion = 0;
            DetraccionCuenta = "";
            DetraccionTipoOperacion = "";
            DetraccionPorcentaje = 0;
            BienServicioCodigo = "";
            CondicionPago = "";
            CreditoTotal = 0;
            Percepcion = 0;
            PercepcionRegimen = "";
            PercepcionFactor = 0;
            PercepcionTotal = 0;
            PercepcionBaseImponible = 0;
            Retencion = 0;
            DescuentoGlobal = 0;
            DescuentoGlobalMonto = 0;
            DescuentoGlobalMontoBase = 0;
            DescuentoGlobalPorcentaje = 0;
            DescuentoGlobalValor = 0;
            DescuentoGlobalIndicadorDescuento = "";
            DescuentoGlobalCodigoMotivo = "";
            DescuentoGlobalNGMonto = 0;
            DescuentoGlobalNGMontoBase = 0;
            DescuentoGlobalNGIndicadorDescuento = 0;
            DescuentoGlobalNGCodigoMotivo = "";
            DescuentoNGMonto = 0;
            AnticiposGravado = 0;
            AnticiposExonerado = 0;
            AnticiposInafecto = 0;
            AnticipoMonto = 0;
            AnticipoMontoBase = 0;
            AnticipoCodigo = "";
            CargoGlobalMonto = 0;
            CargoGlobalMontoBase = 0;
            CargoGlobalPorcentaje = 0;
            CargoGlobalIndicadorCargos = "";
            CargoGlobalCodigoMotivo = "";
            ImpuestoTotal = 0;
            ISC = 0;
            ISCBase = 0;
            ICBPER = 0;
            Exonerada = 0;
            Inafecto = 0;
            Exportacion = 0;
            ExoneradaXML = 0;
            InafectoXML = 0;
            ExportacionXML = 0;
            Gratuito = 0;
            GratuitoGravado = 0;
            TotalImporteVenta = 0;
            TotalPrecioVenta = 0;
            TotalValorVenta = 0;
            TotalDescuentos = 0;
            TotalOtrosCargos = 0;
            TotalAnticipos = 0;
            TotalPago = 0;
            PendientePago = 0;
            EnviarCorreo = false;
            TienePDF = false;
            TieneXML = false;
            TieneCDR = false;
            FormatoPDF = 0;
            SunatRespuesta = "1";
            SunatMensaje = "";
            Sucursal = 0;
            Usuario = "";
            Vendedor = "";
            CanalVenta = "";
            AlojamientoTipoDocIdentidadCodigo = "";
            AlojamientoNumeroDocIdentidad = "";
            AlojamientoNombreRazonSocial = "";
            AlojamientoPaisDocEmisor = "";
            PaisResidencia = "";
            DiasPermanencia = 0;
            Peso = 0;
            Bultos = 0;
            MotivoTrasladoDescripcion = "";
            MotivoTrasladoCodigo = "";
            ModalidadTrasladoCodigo = "";
            TransportistaNumeroDocIdentidad = "";
            TransportistaTipoDocIdentidadCodigo = "";
            UbigeoLlegada = "";
            DireccionLlegada = "";
            NOMBRE_UBIGEOLLEGADA = "";
            UbigeoPartida = "";
            DireccionPartida = "";
            NOMBRE_UBIGEOPARTIDA = "";
            Anticipo = false;
            EstadoContingencia = false;
            Baja = 0;
            Leyenda = 0;
            LeyendaBoleta = 0;
            LeyendaFactura = 0;
            MotivoBaja = "";
            OrigenSistema = 0;
            TipoPrecio = 0;
            Ubigeo = "";
            Firma = "";
            sIdentificadorBaja = "";
            sIdentificadorResumen = "";
            Estado = 6;
            DocAdicionalCodigo = 0;
            DocAdicionalDetalle = "";
            PagosMultiples = false;
            EsPrico = false;
            ListaDetalles = null;
            ListaAnticipos = null;
            ListaCuotas = null;
            ListaGuias = null;
            ListaMovimientos = null;
            ListaDocumentosRelacionados = null;
        }
    }
}
