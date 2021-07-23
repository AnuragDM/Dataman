using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Services;

namespace AstralFFMS
{
    public partial class Rpt_ExportLogDBReport : System.Web.UI.Page
    {

        #region " Variable Declaration "

        string roleType = "";
        int cnt = 0;
        string str = "";
        DataTable dt;
        string StateId = string.Empty, CityId = string.Empty, AreaId = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                roleType = Settings.Instance.RoleType;
                string pageName = Path.GetFileName(Request.Path);
               // btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
               //// btnExport.Enabled = true;
               // btnExport.CssClass = "btn btn-primary";
                distitemsalerpt.DataSource = new DataTable();
                distitemsalerpt.DataBind();
                BindSalesPerson();
               
            }


        }



     
        public void  BindSalesPerson()
        {
            DataTable dtDist = new DataTable();
            try
            {
                string distqry = @"select msp.smid,(msp.smname+' - '+ msp.SyncId+' - '+ma.AreaName) as Text  from mastsalesrep msp left join MastArea ma on ma.AreaId=msp.cityid  where  msp.Active=1 and smname<>'.' order by msp.smname";
                dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                lstsales.DataSource = dtDist;
                lstsales.DataValueField = "smid";
                lstsales.DataTextField = "Text";
                lstsales.DataBind();

            }
            catch (Exception)
            {

            }
           
        }

     
       



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getBackupData(string fromdate, string Todate, string SMID)
        {
            //
            DataTable dtdata = new DataTable();

            string str = "", whr = "" ;

            try
            {
                    if (SMID != "")
                        whr = " and db.smid in (" + SMID + ") ";

                    str = "select row_number() over (order by msp.smname ) as SNo,db.date,(msp.smname+' - '+ msp.SyncId+' - '+ma.AreaName) smname,(select 'http://'+compurl from mastenviro)+replace(db.logurl,'~','') logurl,(select 'http://'+compurl from mastenviro)+replace(db.dburl,'~','') dburl,format(db.createddate,'dd/MMM/yyyy') createddate from exportloganddb db left join mastsalesrep msp on msp.smid=db.smid  left join MastArea ma on ma.AreaId=msp.cityid  where  msp.Active=1 and smname<>'.' "+ whr +"  order by msp.smname , db.date";

                    str = "select row_number() over (order by msp.smname ) as SNo,db.date,(msp.smname+' - '+ msp.SyncId+' - '+ma.AreaName) smname,replace(db.logurl,'~','') logurl,replace(db.dburl,'~','') dburl,format(db.createddate,'dd/MMM/yyyy') createddate from exportloganddb db left join mastsalesrep msp on msp.smid=db.smid  left join MastArea ma on ma.AreaId=msp.cityid  where  msp.Active=1 and smname<>'.' " + whr + "  order by msp.smname , db.date";
                
                dtdata = DbConnectionDAL.GetDataTable(CommandType.Text, str);


            }
            catch (Exception ex)
            {

            }
            return JsonConvert.SerializeObject(dtdata);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dtdata = new DataTable();

            string str = "", whr = "";

            try
            {
                if (hidSalesPerson.Value != "")
                    whr = " and db.smid in (" + hidSalesPerson.Value + ") ";

                str = "select format(db.date,'dd/MMM/yyyy HH:mm:sss') date,(msp.smname+' - '+ msp.SyncId+' - '+ma.AreaName) smname,replace(db.logurl,'~','') logurl,replace(db.dburl,'~','') dburl,format(db.createddate,'dd/MMM/yyyy') createddate from exportloganddb db left join mastsalesrep msp on msp.smid=db.smid  left join MastArea ma on ma.AreaId=msp.cityid  where  msp.Active=1 and smname<>'.' and  db.date>='" + txtfmDate.Text + " 00:00' and  db.date<='" + txttodate.Text + " 23:59' " + whr + "  order by msp.smname asc, db.date desc";

                dtdata = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                rptmain.Style.Add("Display","block");
                distitemsalerpt.DataSource = dtdata;
                distitemsalerpt.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCan_Click(object sender, EventArgs e)
        {
            Response.Write("~/Rpt_ExportLogDBReport.aspx");
        }

        protected void distitemsalerpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                try
                {
                    string filePath = Server.MapPath(e.CommandArgument.ToString());
                    // get file object as FileInfo
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                    // -- if the file exists on the server
                    if (file.Exists)
                    {
                        // set appropriate headers
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", ("attachment; filename=" + file.Name.Replace("&", "").Replace(",", "")));
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";

                        Response.TransmitFile(file.FullName);

                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        // if file does not exist
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' This file does not exist.');", true);

                    }


                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }
        }


      

    
    }
}