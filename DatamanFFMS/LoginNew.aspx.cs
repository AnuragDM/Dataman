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
    public partial class LoginNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Image1.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolder"].ToString();
            if (!string.IsNullOrEmpty(Convert.ToString(Session["msz"])))
            {
                lbltxt.Visible = true;

                lbltxt.InnerHtml = Convert.ToString(Session["msz"]); ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                Session["msz"] = null;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                
                string name = Request.Form["txtuser"];
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

                    Response.Redirect("/Home.aspx", true);  

                    //if (Settings.Instance.RoleType == "CRMExecutive")
                    //{
                    //    Response.Redirect("/CRMTask.aspx", true);
                    //}
                    //else
                    //{
                    //    Response.Redirect("/Home.aspx", true);                       
                    //}
                }
                else
                {
                    //BootstrapErrorMessage.Visible = true;
                    //lbltxt.Text = "Invalid Username or Password";
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

        protected void lnkHelp_Click(object sender, EventArgs e)
        {
            try
            {
                ShowHelp();
                mpePop.TargetControlID = lnkHelp.ID;
                mpePop.PopupControlID = pnlData.ID;
                mpePop.Show();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                mpePop.Hide();
                gvData.DataSource = null;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        public void ShowHelp()
        {
            try
            {
                DataTable gdt = new DataTable();

                gdt.Columns.Add("UserName");
                gdt.Columns.Add("Password");
                gdt.Columns.Add("Role");

                DataRow dr = gdt.NewRow();
                dr["UserName"] = "SA";
                dr["Password"] = "admin@123";
                dr["Role"] = "ADMIN";

                gdt.Rows.Add(dr);

                dr = gdt.NewRow();
                dr["UserName"] = "L3UP";
                dr["Password"] = "12345678";
                dr["Role"] = "State Head";
                gdt.Rows.Add(dr);

                dr = gdt.NewRow();
                dr["UserName"] = "L2KNP";
                dr["Password"] = "12345678";
                dr["Role"] = "District Head";
                gdt.Rows.Add(dr);

                dr = gdt.NewRow();
                dr["UserName"] = "L1NORTHKNP";
                dr["Password"] = "12345678";
                dr["Role"] = "Area Incharge";
                gdt.Rows.Add(dr);

                dr = gdt.NewRow();
                dr["UserName"] = "dataman";
                dr["password"] = "12345";
                dr["Role"] = "Distributor";
                gdt.Rows.Add(dr);

                gvData.DataSource = gdt;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

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
    }
}