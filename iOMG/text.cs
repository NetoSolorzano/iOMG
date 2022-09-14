using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace iOMG
{
    public partial class text : Form
    {
        public text()
        {
            InitializeComponent();
        }

        private void text_Load(object sender, EventArgs e)
        {
            //string resultado = "{"xml_pdf":{"IDComprobante":21216,"Codigo":0,"IDRepositorio":"S1Jn36U / POAusmSDhh / onA == ","Firma":"eICsgg4izZWY2CkNqg7HHMfbAOw = ","Mensaje":"03 - B200 - 885"},"cdr":{"IDComprobante":21216,"Codigo":0,"IDRepositorio":"13013658","Firma":"","Mensaje":"La Boleta numero B200-885, ha sido aceptada"}}";

        }
    }
}
