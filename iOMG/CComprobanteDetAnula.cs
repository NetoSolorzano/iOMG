using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CComprobanteDetAnula
    {
        public long ID { get; set; }
        public int Item { get; set; }
        public int ComprobanteID { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string Serie  { get; set; }
        public int Correlativo  { get; set; }
        public string UnidadMedidaCodigo  { get; set; }
        public int Cantidad  { get; set; }
        public string ProductoCodigo  { get; set; }
        public string ProductoCodigoCliente  { get; set; }
    }
}
