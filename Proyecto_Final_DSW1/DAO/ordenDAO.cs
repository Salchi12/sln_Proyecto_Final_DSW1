using Proyecto_Final_DSW1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_Final_DSW1.DAO
{
    public class ordenDAO
    {
        public string Transaccion(OrdenPedido reg, List<Item> canasta)
        {
            string mensaje = "";
            ConexionDAO cn = new ConexionDAO();
            cn.getcn.Open();
            SqlTransaction tr = cn.getcn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                //1.ejecutar el procedure usp_agregar_orden y recuperar @n output  
                SqlCommand cmd = new SqlCommand("usp_agregar_orden", cn.getcn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cliente", reg.cliente);
                cmd.Parameters.AddWithValue("@email", reg.email);
                cmd.Parameters.AddWithValue("@fono", reg.fono);
                cmd.Parameters.Add("@n", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int n = (int)cmd.Parameters["@n"].Value;

                //2.ejecutar el proc usp_agregar_orden_detalle para lista canasta
                foreach (Item it in canasta)
                {
                    cmd = new SqlCommand("usp_agregar_orden_detalle", cn.getcn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@norden", n);
                    cmd.Parameters.AddWithValue("@idproducto", it.codigo);
                    cmd.Parameters.AddWithValue("@precio", it.precio);
                    cmd.Parameters.AddWithValue("@cantidad", it.cantidad);
                    cmd.ExecuteNonQuery();
                }

                //3.ejecutar la actualizacion de tb_productos descontando a UnidadesEnExistencia
                foreach (Item it in canasta)
                {
                    cmd = new SqlCommand("usp_actualiza_unidades", cn.getcn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", it.codigo);
                    cmd.Parameters.AddWithValue("@cantidad", it.cantidad);
                    cmd.ExecuteNonQuery();
                }

                //4.si todo esta OK
                tr.Commit();
                mensaje = string.Format("Se ha generado la orden {0}", n);
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message; tr.Rollback();
            }
            finally { cn.getcn.Close(); }
            return mensaje;
        }
    }
}