using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Npgsql;
using System.Data;

namespace HotelKoKoMu_CardRegister.ContextDB
{
    public class BaseDL
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public DataTable SelectDataTable(string sSQL, NpgsqlParameter[] param)
        {
            DataTable dt = new DataTable
            {
                TableName = "data"
            };
            try
            {
                var newCon = new NpgsqlConnection(conStr);
                using (var adapt = new NpgsqlDataAdapter(sSQL, newCon))
                {
                    newCon.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapt.SelectCommand.Parameters.AddRange(param);
                    //cmd.CommandText = "fetch all in \"ref\"";
                    adapt.Fill(dt);
                    newCon.Close();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return dt;
        }

        public string InsertUpdateDeleteData(string sSQL, params NpgsqlParameter[] para)
        {
            try
            {
                var newCon = new NpgsqlConnection(conStr);
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddRange(para);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return "true";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return "false";
            }
        }

    }
}