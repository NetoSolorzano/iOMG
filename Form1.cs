using System;
using System.Windows.Forms;

namespace iOMG
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //float ancho_form, alto_form;
        public Form1()
        {
            InitializeComponent();
            bt_min.Visible = false;     // para esta versión casi final
            bt_max.Visible = false;     // a pedido a Lorenzo le puse marco a la ventana
            bt_close.Visible = false;   // y estos botones ya quedan sin uso porque windows los tiene
            this.Text = "GESTION DE ALMACEN - SOLORSOFT";
            // Set an icon using code
            //System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
            //this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            System.Drawing.Icon ico = new System.Drawing.Icon("iOMG.ico");
            this.Icon = ico;

            pan_lateral.Height = bt_ini.Height;
            pan_lateral.Top = bt_ini.Top;
            if (iOMG.Program.m70 == "M0")
            {
                pan_inicio1.BringToFront();
                bt_ini.Tag = "1";
            }
            else
            {
                if (iOMG.Program.m70 == "M70")
                {
                    pan_fisico1.BringToFront();
                    bt_op2.Tag = "1";
                    bt_ini.Enabled = false;
                    bt_op2.Enabled = false;
                }
                else
                {
                    bt_ini.Visible = false;
                    bt_op1.Visible = false;
                    bt_op2.Visible = false;
                }
            }
            button4.Visible = false;
            button5.Visible = false;
            bt_excel.Visible = false;
            bt_vta.Visible = true;
        }

        private void bt_ini_Click(object sender, EventArgs e)       // panel gestion de almacen
        {
            pan_lateral.Height = bt_ini.Height;
            pan_lateral.Top = bt_ini.Top;
            pan_inicio1.BringToFront();
            bt_op1.Tag = "0";
            bt_op2.Tag = "0";
            bt_ini.Tag = "1";
            bt_vta.Tag = "0";
        }

        private void bt_op2_Click(object sender, EventArgs e)       // panel maestra de items
        {
            pan_lateral.Height = bt_op2.Height;
            pan_lateral.Top = bt_op2.Top;
            pan_op21.BringToFront();
            pan_op21.Top = pan_inicio1.Top;
            bt_op2.Tag = "1";
            bt_op1.Tag = "0";
            bt_ini.Tag = "0";
            bt_vta.Tag = "0";
        }

        private void bt_op1_Click(object sender, EventArgs e)       // panel movimientos fisicos
        {
            pan_lateral.Height = bt_op1.Height;
            pan_lateral.Top = bt_op1.Top;
            pan_fisico1.BringToFront();
            pan_fisico1.Top = pan_inicio1.Top;
            bt_op1.Tag = "1";
            bt_op2.Tag = "0";
            bt_ini.Tag = "0";
            bt_vta.Tag = "0";
        }

        private void bt_vta_Click(object sender, EventArgs e)
        {
            pan_lateral.Height = bt_vta.Height;
            pan_lateral.Top = bt_vta.Top;
            pan_op11.BringToFront();
            pan_op11.Top = pan_inicio1.Top;
            bt_vta.Tag = "1";
            bt_op1.Tag = "0";
            bt_op2.Tag = "0";
            bt_ini.Tag = "0";
        }

        private void bt_sale_Click(object sender, EventArgs e)
        {
            pan_lateral.Height = bt_salir.Height;
            pan_lateral.Top = bt_salir.Top;
            bt_salir.PerformClick();
        }

        private void bt_salir_Click(object sender, EventArgs e)
        {
            pan_lateral.Height = bt_salir.Height;
            pan_lateral.Top = bt_salir.Top;
            var aaa = MessageBox.Show("Realmente desea salir del programa?", "Confirme por favor",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aaa == DialogResult.Yes)
            {
                Application.Exit();
                return;
            }
        }

        private void bt_min_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void bt_max_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
            }

        }
        private void bt_face_Click(object sender, EventArgs e)
        {
            // linkedin 
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/lucio-sol%C3%B3rzano-659b8416/");
        }
        private void bt_web_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.solorsoft.com");
        }
        private void bt_control_Click(object sender, EventArgs e)
        {
            //impstock imps = new impstock();
            //imps.Show();
        }
        private void bt_excel_Click(object sender, EventArgs e)
        {
            if (bt_ini.Tag == "1")
            {
                // no funca ... pasamos la export a un boton en el propio form pan_inicio
            }
            if (bt_op1.Tag == "1")
            {
                //MessageBox.Show(pan_op11.Name.ToString());
            }
            if (bt_op2.Tag == "1")
            {
                //MessageBox.Show(pan_op21.Name.ToString());
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pan_op11_Load(object sender, EventArgs e)
        {

        }

    }
}
