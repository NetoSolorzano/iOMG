using System;
using System.Windows.Forms;
using System.Data;

namespace iOMG
{
    public partial class forpagos : Form
    {
        DataTable dt;           // medios de pago
        string _mpefec = "";    // variable tipo de pago efectivo
        DataTable contenido;
        public forpagos(DataTable dtmp, string mpefec, DataTable idavuelta)
        {
            dt = dtmp;
            _mpefec = mpefec;
            contenido = idavuelta;
            InitializeComponent();
        }
        private void forpagos_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = contenido;
            foreach (DataRow row in dt.Rows)
            {
                cmb_plazo.Items.Add(row.ItemArray[0].ToString());
            }
            foreach (DataRow rc in contenido.Rows)
            {
                double tv = 0;
                dataGridView1.Rows.Add(0, dataGridView1.Rows.Count - 1, rc.ItemArray[2].ToString(), rc.ItemArray[3].ToString(), rc.ItemArray[4].ToString(), rc.ItemArray[5].ToString());
                tv = tv + double.Parse(rc.ItemArray[4].ToString());
                tx_total.Text = (tv).ToString("#0.00");
                tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
            }
            //return contenido;
        }
        private void forpagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public DataTable ReturnTable { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnValue1 = tx_importe.Text;

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    DataRow dr = contenido.NewRow();
                    dr[0] = "0";
                    dr[1] = "1";
                    dr[2] = row.Cells[2].Value.ToString();
                    dr[3] = row.Cells[3].Value.ToString();
                    dr[4] = row.Cells[4].Value.ToString();
                    dr[5] = row.Cells[5].Value.ToString();
                    contenido.Rows.Add(dr);
                }
            }

            ReturnTable = contenido;
            this.Close();
        }
        private void bt_mas_Click(object sender, EventArgs e)
        {
            if (tx_dat_mp.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el medio de pago","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                cmb_plazo.Focus();
                return;
            }
            if (tx_numOpe.Text.Trim().Length < 4 && tx_dat_mp.Text != _mpefec)
            {
                MessageBox.Show("Ingrese número de operación", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_numOpe.Focus();
                return;
            }
            double vi = 0;
            double.TryParse(tx_importe.Text, out vi);
            if (tx_importe.Text.Trim() == "" || vi <= 0)
            {
                MessageBox.Show("El importe debe ser > 0", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_importe.Focus();
                return;
            }
            dataGridView1.Rows.Add(0,dataGridView1.Rows.Count-1,cmb_plazo.Text,tx_numOpe.Text,tx_importe.Text,tx_dat_mp.Text);
            //contenido.Rows.Add(0, dataGridView1.Rows.Count - 1, cmb_plazo.Text, tx_numOpe.Text, tx_importe.Text, tx_dat_mp.Text);
            double tv = 0;
            double.TryParse(tx_total.Text, out tv);
            tx_total.Text = (tv + vi).ToString("#0.00");
            tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            {
                var aa = MessageBox.Show("Confirma que desea borrar el pago?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    double vb = double.Parse(dataGridView1.Rows[e.Row.Index].Cells["importe"].Value.ToString());

                    double tv = 0;
                    double.TryParse(tx_total.Text, out tv);

                    tx_total.Text = (tv - vb).ToString("#0.00");

                    tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
                }
            }
        }
        private void cmb_plazo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_plazo.SelectedIndex > -1)
            {
                string axs = string.Format("descrizionerid='{0}'", cmb_plazo.Text);
                DataRow[] row = dt.Select(axs);
                tx_dat_mp.Text = row[0].ItemArray[1].ToString();
            }
        }
    }
}
