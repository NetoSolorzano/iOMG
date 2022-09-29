using System;
using System.Windows.Forms;
using System.Data;

namespace iOMG
{
    public partial class forpagos : Form
    {
        DataTable dt;           // medios de pago
        string _mpefec = "";    // variable tipo de pago efectivo
        bool SoloLee = false;
        public forpagos(DataTable dtmp, string mpefec, string[,] idavuelta, bool sololee)
        {
            dt = dtmp;
            _mpefec = mpefec;
            InitializeComponent();
            if (idavuelta[0, 2] != null && idavuelta[0, 2].ToString() != "")
            {
                for (int i=0; i<9; i++)
                {
                    if (idavuelta[i, 2] != null)
                    {
                        if (idavuelta[i, 2].ToString() != "")
                        {
                            dataGridView1.Rows.Add(0, i + 1, idavuelta[i, 2].ToString(), idavuelta[i, 3].ToString(), idavuelta[i, 4].ToString(), idavuelta[i, 5].ToString(), idavuelta[i, 6].ToString());
                        }
                    }
                }
            }
            if (sololee == true) SoloLee = true;
        }
        private void forpagos_Load(object sender, EventArgs e)
        {
            foreach (DataRow row in dt.Rows)
            {
                cmb_plazo.Items.Add(row.ItemArray[0].ToString());
                totalizagrid();
            }
            if (SoloLee == true)
            {
                bt_mas.Enabled = false;
                button1.Enabled = false;
                dataGridView1.Enabled = false;
            }
        }
        private void forpagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string[,] ReturnValue = new string[10, 7];

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnValue1 = tx_total.Text;
            int i = 0;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    ReturnValue[i, 0] = "0";
                    ReturnValue[i, 1] = (i + 1).ToString();
                    ReturnValue[i, 2] = row.Cells[2].Value.ToString();
                    ReturnValue[i, 3] = row.Cells[3].Value.ToString();
                    ReturnValue[i, 4] = row.Cells[4].Value.ToString();
                    ReturnValue[i, 5] = row.Cells[5].Value.ToString();
                    ReturnValue[i, 6] = row.Cells[6].Value.ToString().Substring(0,10);
                }
                i = i + 1;
            }
            totalizagrid();
            this.Close();
        }
        private void bt_mas_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 9)
            {
                MessageBox.Show("No se puede ingresar mas medios de pago", "Limite de medios excedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
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
            // la fecha de pago por defecto es la fecha del día
            double vi = 0;
            double.TryParse(tx_importe.Text, out vi);
            if (tx_importe.Text.Trim() == "" || vi <= 0)
            {
                MessageBox.Show("El importe debe ser > 0", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_importe.Focus();
                return;
            }
            dataGridView1.Rows.Add(0,dataGridView1.Rows.Count-1,cmb_plazo.Text,tx_numOpe.Text,tx_importe.Text,tx_dat_mp.Text,tx_fpago.Text);
            totalizagrid();
            //
            cmb_plazo.SelectedIndex = -1;
            tx_numOpe.Text = "";
            tx_importe.Text = "";
            dataGridView1.Focus();
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

                    /* double vb = double.Parse(dataGridView1.Rows[e.Row.Index].Cells["importe"].Value.ToString());
                    double tv = 0;
                    double.TryParse(tx_total.Text, out tv);
                    tx_total.Text = (tv - vb).ToString("#0.00");
                    tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString(); */
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
                //
                tx_fpago.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }
        }
        private void totalizagrid()
        {
            int i = 0;
            double vb = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().Trim() != "")
                {
                    row.Cells[1].Value = (i + 1).ToString();
                    vb = vb + double.Parse(row.Cells[4].Value.ToString());
                    i += 1;
                }
            }
            tx_total.Text = vb.ToString("#0.00");
            tx_tfil.Text = (dataGridView1.Rows.Count - 1).ToString();
        }
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            totalizagrid();
        }

        private void tx_fpago_Leave(object sender, EventArgs e)
        {
            bt_mas.Focus();
        }
    }
}
