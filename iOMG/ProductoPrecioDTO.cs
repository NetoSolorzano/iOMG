using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class ProductoPrecioDTO
    {
        public int PrecioId { get; set; }
        public int PrecioConfiguracion { get; set; }
        public string ProductoCod { get; set; }
        public string CodigoUnidadMedida { get; set; }
        public string DescripcionUnidadMedida { get; set; }
        public int CantidadUnidadMedida { get; set; }
        public string MonedaCodigo { get; set; }
        public string SucursalId { get; set; }
        public decimal Margenganancia { get; set; }
        public decimal MargenPorcentaje { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Sugerido { get; set; }
        public decimal OtrosCargosPorcentaje { get; set; }
        public decimal CantidadAplicable { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaActualizacion { get; set; }
        public int Estado { get; set; }
        public decimal ISCPorcentaje { get; set; }
        public decimal ISCCalculado { get; set; }
        public Array Extension { get; set; }                // sera asi???
        public string UUID { get; set; }

        public ProductoPrecioDTO()
        {
            PrecioId = -2;
            PrecioConfiguracion = 1;
            ProductoCod = "";
            CodigoUnidadMedida = "NIU";
            DescripcionUnidadMedida = "UNIDAD";
            CantidadUnidadMedida = 1;
            MonedaCodigo = "PEN";
            SucursalId = "0";
            Margenganancia = 0;
            MargenPorcentaje = 0;
            PrecioVenta = 50;
            Sugerido = 0;
            OtrosCargosPorcentaje = 0;
            CantidadAplicable = 0;
            FechaIngreso = "08/09/2022";
            FechaActualizacion = "08/09/2022";
            Estado = 1;
            ISCPorcentaje = 0;
            ISCCalculado = 0;
            //Extension = { };
            UUID = "";
        }

    }
}
