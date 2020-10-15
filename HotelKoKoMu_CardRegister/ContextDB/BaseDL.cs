using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Npgsql;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using HotelKoKoMu_CardRegister.Models;

namespace HotelKoKoMu_CardRegister.ContextDB
{
    public class BaseDL
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
       
        public async Task<DataTable> SelectDataTable(string sSQL, NpgsqlParameter[] param)
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
                    NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        param = ChangeToDBNull(param);
                        adapt.SelectCommand.Parameters.AddRange(param);
                    }
                    newCon.Open();
                    await Task.Run(() => adapt.Fill(dt));                    
                    newCon.Close();
                }
            }
            catch(NpgsqlException ex)
            {
                string msg = ex.ErrorCode + ex.Message;
            }
            return dt;
        }
        public async Task<ReturnMessageInfo> InsertUpdateDeleteData(string sSQL, params NpgsqlParameter[] para)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
            try
            {
                var newCon = new NpgsqlConnection(conStr);
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon)
                {
                    CommandType = CommandType.Text
                };               
                para = ChangeToDBNull(para);
                cmd.Parameters.AddRange(para);
                cmd.Connection.Open();
                await cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
                msgInfo.Status = "Success";
                msgInfo.FailureReason = "";
                msgInfo.ErrorDescription = "";
                return msgInfo;
            }
            catch (NpgsqlException ex)
            {
                if(ex.ErrorCode==-2147467259)
                {
                    msgInfo.Status = "Error";
                    msgInfo.FailureReason = "1003";
                    msgInfo.ErrorDescription = "Database connection error.";
                }
                else
                {
                    msgInfo.Status = "Error";
                    msgInfo.FailureReason = "1004";
                    msgInfo.ErrorDescription = "Database error.";
                }
                return msgInfo;
            }
        }

        private NpgsqlParameter[] ChangeToDBNull(NpgsqlParameter[] para)
        {
            foreach (var p in para)
            {
                if (p.Value == null || string.IsNullOrWhiteSpace(p.Value.ToString()))
                {
                    p.Value = DBNull.Value;
                    p.NpgsqlValue = DBNull.Value;
                }
                else
                {
                    p.Value = p.Value;
                    p.NpgsqlValue = p.Value;
                }
            }

            return para;
        }
        public async Task<Tuple<string, string>> SelectJson(string sSQL, params NpgsqlParameter[] param)
        {
            string msg = string.Empty;
            DataTable dt = new DataTable
            {
                TableName = "data"
            };
            try
            {
                var newCon = new NpgsqlConnection(conStr);
                using (var adapt = new NpgsqlDataAdapter(sSQL, newCon))
                {                                      
                    NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon);
                    cmd.CommandType = CommandType.Text;
                    if (param != null)
                    {
                        param = ChangeToDBNull(param);
                        adapt.SelectCommand.Parameters.AddRange(param);
                    }
                    newCon.Open();
                    await Task.Run(() => adapt.Fill(dt));                    
                    msg = "Success";
                    newCon.Close();

                }
            }
            catch (NpgsqlException ex)
            {
                msg = ex.ErrorCode + "/" + ex.Message;
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return new Tuple<string, string>(DataTableToJSONWithJSONNet(dt), msg);
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }

        public DataTable SelectDataTable_Info(string sSQL, NpgsqlParameter[] param)
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
                    NpgsqlCommand cmd = new NpgsqlCommand(sSQL, newCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        param = ChangeToDBNull(param);
                        adapt.SelectCommand.Parameters.AddRange(param);
                    }
                    newCon.Open();
                    adapt.Fill(dt);
                    newCon.Close();
                }
            }
            catch (NpgsqlException ex)
            {
                string msg =ex.ErrorCode+"/"+ ex.Message;
            }
            return dt;
        }       
    }
}