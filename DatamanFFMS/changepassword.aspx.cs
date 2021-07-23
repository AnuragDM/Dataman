using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using System.Web.Configuration;

namespace AstralFFMS
{
    public partial class changepassword : System.Web.UI.Page
    {
     //    OpeartionDataContext context = new OpeartionDataContext();
        int msg = 0;
        int success = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["user_name"] == null)
            //{
            //    Response.Redirect("/LogIn.aspx", true);
            //}
            BootstrapErrorMessage.Visible = false;
            BootstrapSuccessMessage.Visible = false;
            Image1.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolder"].ToString();
            Label1.Visible = false;
            LinkButton2.Visible = true;
            if (Request.QueryString["UserId"] != null)
            {
                LinkButton2.Visible = false;
            }
        }

        protected void btnchangePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["user_name"] != null)
                {
                    if (IsvalidUser(Session["user_name"].ToString()))
                    {
                        string newpwd = txtNewPassword.Text.Trim();
                        int val = UpdateNewPwd(Session["user_name"].ToString(), newpwd);
                        if (val == 1)
                        {
                            BootstrapSuccessMessage.Visible = true;
                            successmsglabel.Text = "Password changed successfully";
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "5;url=Loginn.aspx";
                            this.Page.Controls.Add(meta);
                            Label1.Visible = true;
                            Label1.Text = "You will now be redirected in 5 seconds";
                            //       Response.Redirect("~/LogIn.aspx", true);
                            Session.Abandon();
                            Session.Clear();
                            Session.RemoveAll();
                        }
                        
                    }
                    LinkButton2.Visible = true;
                }
                if (Request.QueryString["UserId"] != null)
                {
                    int success = UpdatePwd(Request.QueryString["UserId"]);
                    if (success == 1)
                    {
                        BootstrapSuccessMessage.Visible = true;
                        successmsglabel.Text = "Password changed successfully";
                        Response.Redirect("~/Loginn.aspx", true);
                    }
                    LinkButton2.Visible = false;
                }
            }
        }

        private int UpdatePwd(string p)
        {
            string updateLogin = @"update MastLogin set Pwd='" + txtNewPassword.Text + "' where Id=" + Convert.ToInt32(p) + "";

            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLogin);
            success=1;
        //    MastLogin MRC = context.MastLogins.Where(x => x.Id == Convert.ToInt32(p)).Single<MastLogin>();
       //     if (MRC != null)
       //     {
       //         MRC.Pwd = txtNewPassword.Text;
        //        context.SubmitChanges();
       //         success = 1;
      //      }
     //       else
     //       {
      //          success = 0;
      //      }
            return success;
        }
        private int UpdateNewPwd(string p, string newpwd)
        {
            if (newpwd != "")
            {
                string updateLogin1 = @"update MastLogin set Pwd='" + newpwd + "' where Name='" + p + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLogin1);
                msg = 1;
                //MastLogin MRC = context.MastLogins.Where(x => x.Name == p).Single<MastLogin>();
                //if (MRC != null)
                //{
                //    MRC.Pwd = newpwd;
                //    context.SubmitChanges();
                //    msg = 1;
                //}
                //else
                //{
                //    msg = 0;
                //}

            }
            return msg;
        }
        private bool IsvalidUser(string userName)
        {
            //Ankita - 18/may/2016- (For Optimization)
            //string checkLogin = @"select * from MastLogin where Name='" + userName + "'";
            string checkLogin = @"select count(*) from MastLogin where Name='" + userName + "'";
              
            //DataTable checklogindt = DbConnectionDAL.GetDataTable(CommandType.Text, checkLogin);
            int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, checkLogin));
            //if (checklogindt.Rows.Count > 0)
            if (cnt > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}