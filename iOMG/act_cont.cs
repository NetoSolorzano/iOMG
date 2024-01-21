//******************* actualizacion del estado de los contratos ***********************
//PROCEDURE act_cont
//PARAMETERS vpcont,formu && contrato
//LOCAL texto,asd,cantit,cantrs
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;
namespace iOMG
{
    class acciones
    {
        // conexion a la base de datos
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + " " + ";default command timeout=120" +
        ";ConnectionLifeTime=" + ctl + ";";
        // 18-09-2020, REUNION GLORIA, NESTOR. LOS CODIGOS Z NO INFLUYEN EN EL ESTADO DE UN CONTRATO
        // 20/01/2024, si el contrato esta anulado, no debe actualizar el estado
        public void act_cont(string numcon,string tipo)
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            while (true)
            {
                try
                {
                    cn.Open();
                    if (cn.State == System.Data.ConnectionState.Open) break;
                }
                catch
                {
                    var aa = MessageBox.Show("La conexión al servidor esta tardando demasiado" + Environment.NewLine +
                        "Desea seguir esperando?", "Problema de conectividad", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
            }
            try
            {
                int cantit = 0;
                int cantne = 0;
                //int cantes = 0;
                int caning = 0;
                int cantrs = 0;
                int canped = 0;
                int canten = 0;
                int cansal = 0;
                //******************************************** calculamos la cantidad de articulos del contrato vpcont='012601'
                string texto="select ifnull(SUM(a.cant),0) as cant,a.contratoh " +
                "from detacon a " +
                "left join items b on UPPER(TRIM(a.item))=UPPER(TRIM(b.codig)) " +
                "where TRIM(a.contratoh)=@vpcont and left(a.item,1)<>'Z'";  // where TRIM(a.contratoh)=@vpcont
                MySqlCommand micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                MySqlDataReader dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    cantit = dr.GetInt16(0); // cantidad de articulos del contrato
                }
                dr.Close();
                //*************************** calculamos la cantidad de articulos que tienen pedido  ***  cambio 21-09-2020
                //texto="select ifnull(sum(a.cant),0) as cant from pedidos a " +
                //    "left join contrat c on c.contrato=a.contrato " +
                //    "where c.contrato=@vpcont";
                texto = "SELECT ifnull(sum(d.cant),0) from pedidos p " +
                "LEFT JOIN detaped d ON d.pedidoh = p.codped " +
                "where p.contrato = @vpcont AND left(d.item,1)<> 'Z' AND p.status<>'ANULAD'";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    canped = dr.GetInt16(0); // cantidad de articulos pedidos
                }
                dr.Close();
                //******************************* calculamos la cantidad de articulos reservados de ese contrato vpcont='012601'
                texto="select ifnull(SUM(a.cant),0) as cant from reservd a " +
                    "left join reservh b on a.reservh=b.idreservh " +
                    "where TRIM(b.contrato)=@vpcont and b.status not in('SALIDAS','ANULADO') AND left(a.item,1)<>'Z'";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    cantrs = dr.GetInt16(0); // cantidad de articulos reservados del contrato
                }
                dr.Close();
                //************************* calculamos la cantidad de articulos ingresados de los pedidos ********
                texto = "select ifnull(SUM(a.cant),0) as cant from movim a " +
                    "left join pedidos b on a.pedido=b.codped AND b.status<>'ANULAD' " +
                    "where TRIM(b.contrato)=@vpcont and left(a.articulo,1)<>'Z'";
                micon = new MySqlCommand(texto, cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if (dr.Read())
                {
                    caning = dr.GetInt16(0);    // cantidad articulos ingresados
                }
                dr.Close();
                //****************************** calculamos la cantidad de articulos ya entregados de ese contrato
                // salidas por ventas en clientes movimientos
                // salidas por ventas en almacen a partir de una reserva  vpcont='012601'
                //texto= "select ifnull(sum(a.cant),0) as cant from movim a " +
                //    "left join pedidos b on a.pedido=b.codped AND b.status<>'ANULAD' " +
                //    "where TRIM(b.contrato)=@vpcont and a.fventa is not null and left(a.articulo,1)<>'Z'";
                texto = "SELECT ifnull(sum(a.cant),0) as cant from detam a " +
                    "LEFT JOIN pedidos b ON a.pedido = b.codped AND b.status <> 'ANULAD' " +
                    "where TRIM(b.contrato)= @vpcont and left(a.item,1)<>'Z'";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    canten = dr.GetInt16(0);    // cantidad articulos entregados
                }
                dr.Close();
                texto="select ifnull(SUM(a.cant),0) as cant from salidasd a " +
                    "left join salidash b on a.salidash=b.idsalidash " +
                    "where b.tipomov=2 and TRIM(b.contrato)=@vpcont and b.status<>'ANULADO' and left(a.item,1)<>'Z'";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    cansal = dr.GetInt16(0);     // cantidad de articulos salidos del contrato
                }
                dr.Close();
                //cantes = canten + cansal;    // cantidad de articulos ya entregados del contrato
                //********************************** calculamos la cantidad de articulos no entregados del contrato  vpcont='012601'
                texto="select ifnull(sum(a.cant),0) as cant from movim a " +
                    "left join pedidos b on a.pedido=b.codped AND b.status<>'ANULAD' " +
                    "where TRIM(b.contrato)=@vpcont and a.fventa is null and left(a.articulo,1)<>'Z'";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                dr = micon.ExecuteReader();
                if(dr.Read())
                {
                    cantne = dr.GetInt16(0);    // cantidad de articulos no entregagos
                }
                dr.Close();
                //
                // sumas y restas y actualizaciones
                /*
                MessageBox.Show("cantit: "+cantit.ToString() + Environment.NewLine +
                    "cantne: "+cantne.ToString() + Environment.NewLine +
                    "cantes: " + cantes.ToString() + Environment.NewLine +
                    "cantrs: " + cantrs.ToString() + Environment.NewLine +
                    "canped: " + canped.ToString() + Environment.NewLine +
                    "canten: " + canten.ToString() + Environment.NewLine +
                    "cansal: " + cansal.ToString());*/
                texto="update contrat set status='------' where TRIM(contrato)=@vpcont";
                if(canped + cantrs == cantit && caning == 0) texto="update contrat set status='PORLLE', codped=' ' where TRIM(contrato)=@vpcont";
                //if(cantne + cantrs < cantit && cantne + cantrs != 0 && canten + cansal == 0 && cantne != 0) texto="update contrat set status='LLEPAR', codped=' ' where TRIM(contrato)=@vpcont";
                if (cantne + cantrs == cantit && caning > 0 && caning < canped) texto = "update contrat set status='LLEPAR', codped=' ' where TRIM(contrato)=@vpcont";
                if (cantne + cantrs == cantit && canped == caning && cansal + canten == 0) texto="update contrat set status='PORENT',codped=' ' where TRIM(contrato)=@vpcont";
                if (cansal + canten > 0 && canten + cansal < cantrs + caning) texto="update contrat set status='ENTPAR', codped='parcia' where TRIM(contrato)=@vpcont";
                if (canped + cantrs + cansal < cantit) texto="update contrat set status='PEDPAR', codped=' ' where TRIM(contrato)=@vpcont";
                if (canped + cantrs == 0) texto="update contrat set status='PENDIE', codped=' ' where TRIM(contrato)=@vpcont";
                if (canten + cansal == cantit) texto="update contrat set status='ENTREG', codped='total' where TRIM(contrato)=@vpcont";
                micon = new MySqlCommand(texto,cn);
                micon.Parameters.AddWithValue("@vpcont", numcon);
                micon.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message,"Error");
                Application.Exit();
            }
        }
    }
}
