using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CGuiasComprobante
    {
        public string Serie { get; set; }
        public string Correlativo { get; set; }
        public int TipoGuia { get; set; }
        public string SerieCorrelativo { get; set; }

        public CGuiasComprobante()
        {
            Serie = Correlativo = SerieCorrelativo = "";
            TipoGuia = 0;
        }
    }
}
