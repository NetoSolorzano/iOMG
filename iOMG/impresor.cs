using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace iOMG
{
    public partial class impresor : Form
    {
        public impresor(string capi, string mode, string made, string tipo, string det1, string acab, string tall, 
            string det2, string det3, string jueg, string nomb, string medi, string cant, string paqu, int idm)
        {
            InitializeComponent();
            tx_dat_cap.Text = capi;
            tx_dat_cap.Text = capi;
            tx_dat_mod.Text = mode;
            tx_dat_mad.Text = made;
            tx_dat_tip.Text = tipo;
            tx_dat_det1.Text = det1;
            tx_dat_aca.Text = acab;
            tx_dat_tal.Text = tall;
            tx_dat_det2.Text = det2;
            tx_dat_det3.Text = det3;
            tx_dat_jgo.Text = jueg;
            tx_nombre.Text = nomb;
            tx_medidas.Text = medi;
            tx_cant.Text = cant;
            tx_paq.Text = paqu;
            tx_idm.Text = idm.ToString();   // ID de almacen del mueble ingresado
        }

        private void impresor_Load(object sender, EventArgs e)
        {
            PrinterSettings ps = new PrinterSettings();
            IEnumerable<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>();
            PaperSize sizeA5 = paperSizes.First<PaperSize>(size => size.Kind == PaperKind.A5);
            ps.DefaultPageSettings.PaperSize = sizeA5;
            ps.DefaultPageSettings.Landscape = true;
            //
            printPreviewDialog1.Document = printDocument1;
            printDocument1.PrinterSettings = ps;
            printPreviewDialog1.ShowDialog();
            this.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // GENERALES
            int pagini = e.PageSettings.PrinterSettings.FromPage;       // num pag inicio
            int pagfin = e.PageSettings.PrinterSettings.ToPage;         // num pag final
            float largo = e.PageBounds.Height - 10.0F;                  // largo de la pagina
            float ancho_pag = e.PageSettings.PaperSize.Width - 40.0F;   // ancho de la pagina
            float mar_sup = 20.0F;                                      // margen superior de la impresion
            float mar_izq = 10.0F;                                      // margen izquierdo de la impresion // 20.0F
            // tipos y tamaños de letra
            Font lt_codigo = new Font("Arial", 80);                      // tipo y tamaño de letra rect_codigo
            Font lt_resto = new Font("Arial", 20);                       // tipo y tamaño de letra rect_resto
            Font lt_madera = new Font("Arial", 120, FontStyle.Bold);      // tipo y tamaño de letra rect_madera
            Font lt_id = new Font("Arial", 30);                          // tipo y tamaño de letra rect_id
            Font lt_juego = new Font("Arial", 30);                       // tipo y tamaño de letra rect_juego
            Font lt_cant = new Font("Arial", 25);                        // tipo y tamaño de letra rect_cant
            Font lt_nombre = new Font("Arial", 30);                      // tipo y tamaño de letra rect_nombre
            // rectangulos
            float anc_rec_cod = 350.0F;                                 // ancho rectangulo codigo
            float alt_rec_cod = 200.0F;                                  // alto rectangulo codigo
            float anc_rec_res = 250.0F;                                 // ancho rectangulo resto
            float alt_rec_res = 200.0F;                                  // alto rectangulo resto
            float anc_rec_mad = 190.0F;                                 // ancho rectangulo madera      // 250.0F 200.0F
            float alt_rec_mad = 200.0F;                                  // alto rectangulo codigo
            float anc_rec_id = 250.0F;                                  // ancho rectangulo ID
            float alt_rec_id = 80.0F;                                   // alto rectangulo ID
            float anc_rec_jgo = 250.0F;                                 // ancho rectangulo juego
            float alt_rec_jgo = 80.0F;                                  // alto rectangulo juego
            float anc_rec_can = 250.0F;                                 // ancho rectangulo cantidad
            float alt_rec_can = 80.0F;                                  // alto rectangulo cantidad
            float anc_rec_nom = (anc_rec_res + anc_rec_mad + anc_rec_cod) - anc_rec_id;// ancho rectangulo nombre
            float alt_rec_nom = alt_rec_id + alt_rec_jgo + alt_rec_can; // alto rectangulo nombre
            // rayas
            Pen blackPen = new Pen(Color.Black, 1);                     // color y grosor de la línea
            Pen blackPg2 = new Pen(Color.Black, 2);                     // color y grosor de la línea
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            //
            RectangleF rect_codigo = new RectangleF(mar_izq, mar_sup, anc_rec_cod, alt_rec_cod);
            RectangleF rect_resto = new RectangleF(mar_izq + anc_rec_cod, mar_sup, anc_rec_res, alt_rec_res);
            RectangleF rect_madera = new RectangleF(mar_izq + anc_rec_cod + anc_rec_res, mar_sup, anc_rec_mad, alt_rec_mad);
            RectangleF rect_id = new RectangleF(mar_izq, mar_sup + alt_rec_cod, anc_rec_id, alt_rec_id);
            RectangleF rec_juego = new RectangleF(mar_izq, mar_sup + alt_rec_cod + alt_rec_id, anc_rec_jgo, alt_rec_jgo);
            RectangleF rec_cant = new RectangleF(mar_izq, mar_sup + alt_rec_cod + alt_rec_id + alt_rec_jgo, anc_rec_can, alt_rec_can);
            RectangleF rec_nom = new RectangleF(mar_izq + anc_rec_id, mar_sup + alt_rec_cod, anc_rec_nom, alt_rec_nom);

            e.Graphics.DrawRectangle(blackPg2, mar_izq, mar_sup, anc_rec_cod, alt_rec_cod);                                 //codig
            e.Graphics.DrawRectangle(blackPen, mar_izq + anc_rec_cod, mar_sup, anc_rec_res, alt_rec_res);                  //REsto
            e.Graphics.DrawRectangle(blackPg2, mar_izq + anc_rec_cod + anc_rec_res, mar_sup, anc_rec_mad, alt_rec_mad);     //MAD 
            e.Graphics.DrawRectangle(blackPen, mar_izq, mar_sup + alt_rec_cod, anc_rec_id, alt_rec_id);                      //ID
            e.Graphics.DrawRectangle(blackPen, mar_izq, mar_sup + alt_rec_cod + alt_rec_id, anc_rec_jgo, alt_rec_jgo);      // juego
            e.Graphics.DrawRectangle(blackPen, mar_izq, mar_sup + alt_rec_cod + alt_rec_id + alt_rec_jgo, anc_rec_can, alt_rec_can); // cantid
            e.Graphics.DrawRectangle(blackPen, mar_izq + anc_rec_id, mar_sup + alt_rec_cod, anc_rec_nom, alt_rec_nom);      // nombre

            e.Graphics.DrawString(tx_dat_cap.Text + " " + tx_dat_mod.Text, lt_codigo, Brushes.Black, rect_codigo, drawFormat);  // codig
            e.Graphics.DrawString(tx_dat_tip.Text + tx_dat_det1.Text + tx_dat_aca.Text + tx_dat_tal.Text + tx_dat_det2.Text + tx_dat_det3.Text,
                lt_resto, Brushes.Black, rect_resto, drawFormat);   // Resto
            e.Graphics.DrawString(tx_dat_mad.Text, lt_madera, Brushes.Black, rect_madera, drawFormat);  // madera
            e.Graphics.DrawString("ID "+tx_idm.Text, lt_id, Brushes.Black, rect_id, drawFormat);  // id almacen
            e.Graphics.DrawString(tx_dat_jgo.Text, lt_juego, Brushes.Black, rec_juego, drawFormat); // juego
            e.Graphics.DrawString(tx_cant.Text.Trim() + " de " + tx_paq.Text.Trim(), lt_cant, Brushes.Black, rec_cant, drawFormat);        // cantidad
            e.Graphics.DrawString(tx_nombre.Text.Trim() + Environment.NewLine + tx_medidas.Text.Trim(), lt_nombre, Brushes.Black, rec_nom, drawFormat);
        }
    }
}
