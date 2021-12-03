using Proyecto_Final_DSW1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_Final_DSW1.DAO
{
    public class productoDAO
    {
        public IEnumerable<Libro> listado()
        {
            ConexionDAO cn = new ConexionDAO();
            List<Libro> temporal = new List<Libro>();
            using (cn.getcn)
            {
                SqlCommand cmd = new SqlCommand(
                "select IdProducto,NombreProducto,PrecioUnidad,UnidadesEnExistencia,IdCategoria from tb_productos", cn.getcn);

                cn.getcn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Libro()
                    {
                        idlibro = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        precio = dr.GetDecimal(2),
                        stock = dr.GetInt16(3),
                        idcategoria = dr.GetInt32(4)
                    });
                }
                dr.Close(); cn.getcn.Close();
            }
            return temporal;
        }
        public IEnumerable<Libro> filtro(int categoria)
        {
            return listado().Where(p => p.idcategoria.Equals(categoria));

        }
        public Libro Buscar(int id)
        {
            return listado().Where(p => p.idlibro == id).FirstOrDefault();
        }
    }
}