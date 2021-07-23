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
using System.Drawing;

namespace AstralFFMS
{
    public partial class Loginn : System.Web.UI.Page
    {
        BAL.ProjectMaster.ProjectMasterBAL dp = new BAL.ProjectMaster.ProjectMasterBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Isreset() == "Y")
            {
                lnkReset.Visible = true;
                //btnlogindemo.Visible = true;
            }
            else
            {
                lnkReset.Visible = false;
               // btnlogindemo.Visible = false;
            }
        }

        private void ClearHeart_PushTable()
        {
            string sql = "delete from [PushNotification] where cast (created_date as date)<cast (getdate() as date)";
            DbConnectionDAL.ExecuteQuery(sql);
            sql = "delete  from [PushHeartChk] where cast (webdatetime as date)<cast (getdate() as date) ";
            DbConnectionDAL.ExecuteQuery(sql);

        }
        private void ClearNotification()
        {
            string sql = "delete from Transnotification where cast (VDate as date) < DATEADD(day,-10,cast (getdate() as date))";
            DbConnectionDAL.ExecuteQuery(sql);           

        }
        private string Isreset()
        {
            string str = "SELECT resetdata FROM mastenviro";
            string Reset = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            if (Reset != "Y")
            {
                Reset = "N";

            }
            return Reset;
        }
        protected void lnkReset_Click(object sender, EventArgs e)
        {
            int retval = 0;
            DbParameter[] dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_ReSetCRMData", dbParam);
            retval = Convert.ToInt32(dbParam[0].Value);
            if (retval == 1)
            { ShowAlert("Data Reset Successfully!"); }
            else
            { ShowAlert("There are some errors while Data Reset!"); }

        }     

        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }


        private bool IsvalidUser(string userName, string password)
        {
            ClearNotification();
            ClearHeart_PushTable();           
            bool IsFlag = false;

            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@userID", DbParameter.DbType.VarChar, 100, userName);
            dbParam[1] = new DbParameter("@pwd", DbParameter.DbType.VarChar, 20, password);

            DataTable logindt = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_ChkLogin", dbParam);// DbConnectionDAL.GetDataTable(CommandType.Text, loginquery);
            if (logindt.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(logindt.Rows[0]["Active"]))
                {
                    lbltxt.Visible = true;
                    lbltxt.InnerHtml = "This User has been disabled.Please contact your administrator."; ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);

                    IsFlag = false;
                }

                else
                {
                    if (Settings.Instance.UserID.ToString() != "0")
                    {
                        if (Settings.Instance.UserID.ToString() != Convert.ToString(logindt.Rows[0]["Id"]))
                        {
                            IsFlag = false;
                            //lbltxt.Text = "User Already Logged In!!";
                            lbltxt.Visible = true;
                            lbltxt.InnerHtml = "Another or Same User Already logged in Same Browser....!";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        }
                        else
                        {
                            IsFlag = false; lbltxt.Visible = true;
                            lbltxt.InnerHtml = "Same User Already logged in Same Browser....!"; ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        }
                    }

                    else
                    {
                        //Ankita - 03/may/2016- (For Optimization)
                        string salesrepquery1 = @"select userid from usermast where id=" + logindt.Rows[0]["Id"];
                        string Track_userid = DbConnectionDAL.GetStringScalarVal(salesrepquery1);

                        Settings.Instance.UserID = Convert.ToString(logindt.Rows[0]["Id"]);
                        Settings.Instance.UserName = Convert.ToString(logindt.Rows[0]["Name"]);
                        Settings.Instance.RoleID = Convert.ToString(logindt.Rows[0]["RoleId"]);
                        Settings.Instance.DesigID = Convert.ToString(logindt.Rows[0]["DesigID"]);
                        Settings.Instance.RoleType = Convert.ToString(logindt.Rows[0]["RoleType"]);
                        Settings.Instance.EmpName = Convert.ToString(logindt.Rows[0]["empname"]);
                        Settings.Instance.Track_userid = Track_userid;

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
                        DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, query);
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
                lbltxt.Visible = true;
                lbltxt.InnerHtml = "Invalid User Name or Password."; ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                IsFlag = false;
            }
            return IsFlag;
        }


        protected void btnsub_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string name = Request.Form["username"];
                string pwd = Request.Form["txtpwd"];
                if (IsvalidUser(name, pwd))
                {
                    string password = DbConnectionDAL.GetStringScalarVal("select Pwd from MastLogin where Name = '" + name + "'");
                    if (password == "12345")
                    {
                        Session["user_name"] = name;
                        Response.Redirect("/changepassword.aspx", true);
                    }

                    Session["user_name"] = name;
                    Session["ShowGreeting"] = "Yes";
                    //string str = "SELECT DATEDIFF(DAY, max(vdate) , GETDATE()) FROM TransVisit where smid=9";
                    //int days = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                    //if (days != 0)
                    //{
                    //    int retsave = dp.UpdateCRMData(days);
                    //}
                    if (Isreset() == "Y")
                    {
                        string str = "SELECT DATEDIFF(DAY, max(vdate) , GETDATE()) FROM TransVisit where smid=9";
                        int days = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                        if (days != 0)
                        {
                             int retsave = dp.UpdateCRMData(days);
                        }
                    }
                    Response.Redirect("/Home.aspx", true);
                }

            }
        }
    }
}