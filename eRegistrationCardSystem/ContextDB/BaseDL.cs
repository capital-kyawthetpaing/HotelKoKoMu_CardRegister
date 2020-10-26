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
using eRegistrationCardSystem.Models;
using NLog;
using NLog.Config;

namespace eRegistrationCardSystem.ContextDB
{
    public class BaseDL
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
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
                    Logger.WithProperty("path", HttpContext.Current.Request.Path).Info("Success");
                }
            }
            catch(NpgsqlException ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
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
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Info("Success");
                msgInfo.Status = "Success";
                msgInfo.FailureReason = "";
                msgInfo.ErrorDescription = "";
                return msgInfo;
            }
            catch (PostgresException ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
                string msg = ex.Message;
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1005";
                msgInfo.ErrorDescription = "Database error.";
                return msgInfo;
            }
            catch (Exception ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
                string msg = ex.Message;
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1004";
                msgInfo.ErrorDescription = "Database connection error.";
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
        public async Task<Tuple<string, ReturnMessageInfo>> SelectJson(string sSQL, params NpgsqlParameter[] param)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
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
                    newCon.Close();
                    Logger.WithProperty("path", HttpContext.Current.Request.Path).Info("Success");
                    msgInfo.Status = "Success";
                    msgInfo.FailureReason = "";
                    msgInfo.ErrorDescription = "";
                }
            }
            catch (PostgresException ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
                string msg = ex.Message;
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1005";
                msgInfo.ErrorDescription = "Database error.";                
            }
            catch (Exception ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
                string msg = ex.Message;
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1004";
                msgInfo.ErrorDescription = "Database connection error.";
            }
            return new Tuple<string, ReturnMessageInfo>(DataTableToJSONWithJSONNet(dt), msgInfo);
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
                    Logger.WithProperty("path", HttpContext.Current.Request.Path).Info("Success");
                }
            }
            catch (NpgsqlException ex)
            {
                Logger.WithProperty("path", HttpContext.Current.Request.Path).Error(ex.Message);
                string msg =ex.ErrorCode+"/"+ ex.Message;
            }
            return dt;
        }       
    }
}