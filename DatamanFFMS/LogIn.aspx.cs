using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AstralFFMS;
using DAL;
using BusinessLayer;
using System.Web.Configuration;

namespace AstralFFMS
{
    public partial class LogIn : System.Web.UI.Page
    {
        // BusinessLayer.UserManagement.Login obj = new BusinessLayer.UserManagement.Login();        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //    BootstrapErrorMessage.InnerHtml = "";

            }
            BootstrapErrorMessage.Visible = false;
            Image1.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolder"].ToString();
            //Image1.Height = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("RequiredWidth"));
            //Image1.Width = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("RequiredWidth"));
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (IsvalidUser(txtUserID.Text, txtPassword.Text))
                {
                    Session["user_name"] = txtUserID.Text;
                    if (Settings.Instance.RoleType == "CRMExecutive")
                    {
                        Response.Redirect("/CRMTask.aspx", true);
                    }
                    else
                    {
                        Response.Redirect("/Home.aspx", true);
                    }
                }
                else
                {
                     BootstrapErrorMessage.Visible = true;
                    //errormsglabel.Text = "Invalid Username or Password";
                }
            }
            else
            {
                //string ErrorMsg = "Invalid user or password!";
                //BootstrapErrorMessage.InnerHtml += "<a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>" +
                //            "<ul>" + ErrorMsg + "</ul>";
                //BootstrapErrorMessage.Attributes["class"] = "alert alert-danger";
            }


        }
   //     private bool IsvalidUser(string userName, string password)
   //     {
   //         bool IsFlag = false;

   ////         string loginquery = @"select * from MastLogin where Name='"+ userName +"' and Pwd='"+ password +"'";

   //         DbParameter[] dbParam = new DbParameter[2];
   //         dbParam[0] = new DbParameter("@userID", DbParameter.DbType.VarChar, 100, userName);
   //         dbParam[1] = new DbParameter("@pwd", DbParameter.DbType.VarChar, 20, password);

   //         DataTable logindt = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_ChkLogin", dbParam);// DbConnectionDAL.GetDataTable(CommandType.Text, loginquery);
   //         if (logindt.Rows.Count>0)
   //         {
   //             if (!Convert.ToBoolean(logindt.Rows[0]["Active"]))
   //             {
   //                 errormsglabel.Text = "This User has been disabled.Please contact your administrator.";
   //                 IsFlag = false;
   //             }
   //             else
   //             {   
   //                 //Ankita - 03/may/2016- (For Optimization)
   //                 Settings.Instance.UserID = Convert.ToString(logindt.Rows[0]["Id"]);
   //                 Settings.Instance.UserName = Convert.ToString(logindt.Rows[0]["Name"]);
   //                 Settings.Instance.RoleID = Convert.ToString(logindt.Rows[0]["RoleId"]);
   //                 Settings.Instance.DesigID = Convert.ToString(logindt.Rows[0]["DesigID"]);
   //                 Settings.Instance.RoleType = Convert.ToString(logindt.Rows[0]["RoleType"]);
   //                 Settings.Instance.EmpName = Convert.ToString(logindt.Rows[0]["empname"]);

   //                 string salesrepquery = @"select SMId,SMName,lvl from MastSalesRep where UserId=" + logindt.Rows[0]["Id"];
   //                 DataTable salesrepdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery);
   //                 //         MastSalesRep LogI = context.MastSalesReps.FirstOrDefault(u => u.UserId == Convert.ToInt32(query.FirstOrDefault().Id));
   //                 if (salesrepdt.Rows.Count > 0)
   //                 {
   //                     //var query1 = from s in context.MastSalesReps
   //                     //             where s.UserId == Convert.ToInt32(query.FirstOrDefault().Id)
   //                     //             select s;
   //                     Settings.Instance.SMID = salesrepdt.Rows[0]["SMId"].ToString();
   //                     Settings.Instance.SmLogInName = salesrepdt.Rows[0]["SMName"].ToString();
   //                     Settings.Instance.SalesPersonLevel = salesrepdt.Rows[0]["lvl"].ToString();
   //                 }


   //                 string strDistID = @"SELECT PartyId,PartyName FROM MastParty WHERE UserId=" + Convert.ToInt32(Settings.Instance.UserID) + "";
   //                 DataTable dtDistID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistID);

   //                 if (dtDistID != null && dtDistID.Rows.Count > 0)
   //                 {
   //                     Settings.Instance.DistributorID = Convert.ToString(dtDistID.Rows[0]["PartyId"]);
   //                     Settings.Instance.LoggedDistName = dtDistID.Rows[0]["PartyName"].ToString();
   //                 }
   //                 IsFlag = true;
   //             }
   //         }
   //         else
   //         {
   //             errormsglabel.Text = "Invalid User Name or Password.";
   //             IsFlag = false;
   //         }
   //         return IsFlag;
   //     }

        private bool IsvalidUser(string userName, string password)
        {
            bool IsFlag = false;

            //         string loginquery = @"select * from MastLogin where Name='"+ userName +"' and Pwd='"+ password +"'";

            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@userID", DbParameter.DbType.VarChar, 100, userName);
            dbParam[1] = new DbParameter("@pwd", DbParameter.DbType.VarChar, 20, password);

            DataTable logindt = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_ChkLogin", dbParam);// DbConnectionDAL.GetDataTable(CommandType.Text, loginquery);
            if (logindt.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(logindt.Rows[0]["Active"]))
                {
                    errormsglabel.Text = "This User has been disabled.Please contact your administrator.";
                    IsFlag = false;
                }
                else
                {
                    if (Settings.Instance.UserID.ToString() != "0")
                    {
                        if (Settings.Instance.UserID.ToString() != Convert.ToString(logindt.Rows[0]["Id"]))
                        {
                            IsFlag = false;
                            //errormsglabel.Text = "User Already Logged In!!";
                            errormsglabel.Text = "Another or Same User Already logged in Same Browser....!";
                            
                        }
                        else
                        {
                            IsFlag = false;                           
                            errormsglabel.Text = "Same User Already logged in Same Browser....!";
                        }
                    }

                    else
                    {
                        //Ankita - 03/may/2016- (For Optimization)
                        Settings.Instance.UserID = Convert.ToString(logindt.Rows[0]["Id"]);
                        Settings.Instance.UserName = Convert.ToString(logindt.Rows[0]["Name"]);
                        Settings.Instance.RoleID = Convert.ToString(logindt.Rows[0]["RoleId"]);
                        Settings.Instance.DesigID = Convert.ToString(logindt.Rows[0]["DesigID"]);
                        Settings.Instance.RoleType = Convert.ToString(logindt.Rows[0]["RoleType"]);
                        Settings.Instance.EmpName = Convert.ToString(logindt.Rows[0]["empname"]);

                        string salesrepquery = @"select SMId,SMName,lvl from MastSalesRep where UserId=" + logindt.Rows[0]["Id"];
                        DataTable salesrepdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery);
                        //         MastSalesRep LogI = context.MastSalesReps.FirstOrDefault(u => u.UserId == Convert.ToInt32(query.FirstOrDefault().Id));
                        if (salesrepdt.Rows.Count > 0)
                        {
                            //var query1 = from s in context.MastSalesReps
                            //             where s.UserId == Convert.ToInt32(query.FirstOrDefault().Id)
                            //             select s;
                            Settings.Instance.SMID = salesrepdt.Rows[0]["SMId"].ToString();
                            Settings.Instance.SmLogInName = salesrepdt.Rows[0]["SMName"].ToString();
                            Settings.Instance.SalesPersonLevel = salesrepdt.Rows[0]["lvl"].ToString();
                        }
                        string strDistID = @"SELECT PartyId,PartyName FROM MastParty WHERE UserId=" + Convert.ToInt32(Settings.Instance.UserID) + "";
                        DataTable dtDistID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistID);

                        if (dtDistID != null && dtDistID.Rows.Count > 0)
                        {
                            Settings.Instance.DistributorID = Convert.ToString(dtDistID.Rows[0]["PartyId"]);
                            Settings.Instance.LoggedDistName = dtDistID.Rows[0]["PartyName"].ToString();
                        }
                        string query = "select * from mastenviro  ";
                  //      int orderEntry = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, query));
                        DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text,query);
                        if (Convert.ToInt32(dtenviro.Rows[0]["orderenrty"]) != 0)
                        {
                            Settings.Instance.OrderEntryType = dtenviro.Rows[0]["orderenrty"].ToString();
                            Settings.Instance.AreaWiseDistributor = dtenviro.Rows[0]["areawisedistributor"].ToString();
                        }
                        IsFlag = true;
                    }
                }
            }
            else
            {
                errormsglabel.Text = "Invalid User Name or Password.";
                IsFlag = false;
            }
            return IsFlag;
        }
    }
}