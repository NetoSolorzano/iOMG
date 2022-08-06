using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class _rapifac
    {
        public string ID;                                   // Id del comprobante, se genera despues de la emision
        public string IdRepositorio;                        // Id de repositorio, se genera despues de la emision
        public bool AplicaContingencia;                   // Señala si se trata de un comprobante de contingencia
        public bool AplicaAnticipo;                       // Señala si el comprobante es un anticipo
        public bool AplicaOtroSistema;                    // Señala si el comprobante modifica comprobante de otro sistema (nota cred
        public string Usuario;                              // DNI del usuario que realiza la emision
        public bool AplicaStockNegativo;                  // No aplica a integracion, enviar true
        public bool ModificacionDePrecio;                 // No aplica a integracion, enviar true
        public string Sucursal;                             // Id de la sucursal en la que se realizara la emision
        public string IGVPorcentaje;                        // Porcentaje de IGV
        public string DescuentoGlobalMonto;                 // Monto de descuento global
        public string DescuentoGlobalIndicadorDescuento;    // TODO
        public string DescuentoGlobalCodigoMotivo;          // TODO
        public string DescuentoGlobalNGPorcentaje;           // Porcentaje de descuento global no gravado
        public string DescuentoGlobalNGIndicadorDescuento;   // TODO
        public string DescuentoGlobalNGCodigoMotivo;         // TODO
        public string CargoGlobalPorcentaje;                 // Porcentaje de cargo global
        public string DetraccionTipoOperacion;               // Tipo de operacion con detracción
        public string CargoGlobalIndicadorCargos;            // TODO
        public string CargoGlobalCodigoMotivo;               // TODO
        public int CantidadDecimales;                     // Cantidad de decimales con los que se muestra los montos
        public bool AgentePercepcion;                      // Indica si el emisor es agente de percepción
        public bool PermisoProductoSerie;                  // TODO
        public bool PagosMultiples;                        // Indica si se realizan pagos multiples ????? PREGUNTAR COMO ES ESTE ASUNTO
        public bool EnviarCorreo;                          // Indica si se enviara correo despues de la emision, no aplica para
        public int OrigenSistema;                         // Indica el origen de la emision para integracion el valor sera 7
        public string TipoGuiaRemisionCodigo;                //
        public string ModalidadTrasladoCodigo;               // PORQUE ES 02 ??? PREGUNTAR
        public string TransportistaTipoDocIdentidadCodigo;   //
        public string ConductorTipoDocIdentidadCodigo;       //
        public int CanalVenta;                            // PREGUNTAR QUE ES ESTA VAINA
        public string Vendedor;                              // Vendedor que realiza la emision
        public string CondicionEstado;                      //
        public string CondicionPago;                        // PREGUNTAR "Contado" O PUEDE SER CONTADO, AL CONTADO, ETC
        public int SituacionPagoCodigo;                     // PORQUE ES 2 ???? 
        public int DescuentoIndicador;                      // PORQUE ES 1 ????
        public string Ubigeo;                               // Ubigeo de la direccion de la que emite el comprobante
        public string Anticipo;                             //
        public string TipoCambio;                           //
        public string ClienteTipoDocIdentidadCodigo;        //
        public string ClienteNumeroDocIdentidad;            //
        public string OrdenNumero;                          //
        public string GuiaNumero;                           //
        public string ReferenciaNumeroDocumento;            //
        public string ReferenciaTipoDocumento;              //
        public string DocAdicionalDetalle;                  //
        public int DiasPermanencia;                      //
        public string FechaConsumo;                         //
        public string MotivoTrasladoDescripcion;            //
        public string FechaTraslado;                        //
        public string TransportistaNumeroDocIdentidad;      //
        public string TransportistaNombreRazonSocial;       //
        public string PlacaVehiculo;                        //
        public string ConductorNumeroDocIdentidad;          // 
    }

    public class detalle_rapifac
    {
        public int ID;
        public int ComprobanteID;
        public int Item;
        public string TipoProductoCodigo;
        public string ProductoCodigo;
        public string ProductoCodigoSUNAT;
        public string TipoSistemaISCCodigo;
        public string UnidadMedidaCodigo;
        public double PrecioUnitarioSugerido;
        public double PrecioUnitarioItem;
        public string PrecioVentaCodigo;
        public int ICBPER;
        public int CargoIndicador;
        public bool DescuentoIndicador;
        public string DescuentoCargoCodigo;
        public double PercepcionCantidadUmbral;
        public double PercepcionMontoUmbral;
        public double PercepcionPorcentaje;
        public string Control;
        public double PrecioCompra;
        public double Cargo;
        public double DescuentoGlobal;
        public double Descuento;
        public double ValorUnitario;
        public double ValorVenta;
        public double ValorVentaItem;
        public double ValorVentaItemXML;
        public double ValorVentaNeto;
        public double ValorVentaNetoXML;
        public double ISCUnitario;
        public double ISCNeto;
        public double ISC;
        public double IGV;
        public double ICBPERItem;
        public double ICBPERSubTotal;
        public double DescuentoBase;
        public double DescuentoCargo;
        public double DescuentoCargoGravado;
        public double CargoItem;
        public double CargoTotal;
        public double CargoNeto;
        public double PrecioVenta;
        public double MontoTributo;
        public double ISCPorcentaje;
        public double ISCMonto;
        public double CargoPorcentaje;
        public string Extension;
        public string Descripcion;
        public string Observacion;
        public int Cantidad;
        public int PrecioCodigo;
        public double PrecioUnitario;
        public double Peso;
        public double DescuentoMonto;
        public double DescuentoPorcentaje;
        public string TipoAfectacionIGVCodigo;
        public double IGVNeto;
        public double ImporteTotal;
        public double PesoTotal;
    }
}
