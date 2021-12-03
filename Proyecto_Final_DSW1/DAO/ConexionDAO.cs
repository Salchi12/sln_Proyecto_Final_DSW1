using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_Final_DSW1.DAO
{
    public class ConexionDAO
    {
        SqlConnection cn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        public SqlConnection getcn
        {
            get { return cn; }
        }
    }
}