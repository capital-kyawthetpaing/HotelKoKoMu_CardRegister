using Npgsql;
using NpgsqlTypes;
using System;
using System.Configuration;
using System.Data;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace eRegistrationCardSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        //protected void Application_PostAuthorizeRequest()
        //{
        //    System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    string cardInfo = Session["CardInfo"].ToString();
        //   if (cardInfo != null)
        //    {
        //        try
        //        {
        //            NpgsqlParameter[] para = new NpgsqlParameter[3];
        //            para[0] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardInfo.Split('_')[4] };
        //            para[1] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardInfo.Split('_')[3] };
        //            para[2] = new NpgsqlParameter("@logindate", NpgsqlDbType.Timestamp) { Value = DBNull.Value };
        //            var newCon = new NpgsqlConnection(conStr);
        //            NpgsqlCommand cmd = new NpgsqlCommand("update mst_hotelmachine set logindate=@logindate where hotel_code=@hotelcode and machineno=@machineno", newCon)
        //            {
        //                CommandType = CommandType.Text
        //            };
        //            cmd.Parameters.AddRange(para);
        //            cmd.Connection.Open();
        //            cmd.ExecuteNonQuery();
        //            cmd.Connection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            string msg = ex.Message;
        //        }
        //    }
        //}
    }
}
