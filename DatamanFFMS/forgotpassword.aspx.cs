using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Web.UI.HtmlControls;
using System.Data;
using DAL;
using BusinessLayer;
using System.Web.Configuration;

namespace AstralFFMS
{
    public partial class forgotpassword : System.Web.UI.Page
    {
        int UId = 0;
        string UName = string.Empty;
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
        }

        protected void btnchangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string defaultmailId = "", defaultpassword = "";
                string qry = "select DefaultEmailID,DefaultPassword from [MastEnviro]";

                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            //  DataTable dt = DataAccessLayer.DAL.getFromDataTable(qry);
                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["DefaultEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["DefaultPassword"].ToString();
                }
                if (Page.IsValid)
                {
                    string emailId = txtEmailId.Text;                    
                    if(IsvalidUser(txtLoginId.Text,emailId))                   
                    //if (IsvalidUser(txtEmailId.Text))
                    {
                        string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                        DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='ForgetPassword'";
                            DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                            string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='ForgetPassword'";
                            DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                            string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                            string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);                       
                            string url = "http://crm.astralpipes.com/changepassword.aspx?UserId=" + UId;
                            if (dtVar != null && dtVar.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtVar.Rows.Count; j++)
                                {
                                    ///////////////////////////////////////////
                                    if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                                    {
                                        if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                        {
                                            strMailBody = strMailBody.Replace("{EmployeeName}", UName.ToString());
                                        }
                                        if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PasswordRestLink}")
                                        {
                                            strMailBody = strMailBody.Replace("{PasswordRestLink}", url);
                                        }
                                    }
                                }
                            }

                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                            mail.Subject = strSubject;
                            mail.Body = strMailBody;

                            NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                            mail.To.Add(new MailAddress(Convert.ToString(txtEmailId.Text)));

                            SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                            mailclient.EnableSsl = false;
                            mailclient.UseDefaultCredentials = false;
                            mailclient.Credentials = mailAuthenticaion;
                            mail.IsBodyHtml = true;
                            mailclient.Send(mail);
                        }
                        //StringBuilder strBody = new StringBuilder();
                        //strBody.AppendLine("<table style='width:100%; color:padding:1em; #FFF23DC; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>Welcome to FFMS!</strong></td></tr><tr>");
                        //strBody.AppendLine("".PadLeft(20, ' ').Replace(" ", "&nbsp;"));
                        //strBody.AppendLine("<tr><td>Dear " + UName + ", </td></tr></table>");
                        //strBody.AppendLine("".PadLeft(20, ' ').Replace(" ", "&nbsp;"));
                        //strBody.AppendLine("<table>");
                        //strBody.AppendLine("<tr><td colspan=2> <p style='background-color: white; font-size: 14px; font-family: 'Segoe UI',Arial,Helvetica,sans-serif'>A request was made to send you your email and link to change password for your FFMS login. We can't send you your original password so instead we've created a link to change your password.</p></td></tr>");
                        //strBody.AppendLine("<tr><td colspan=2><p style='background-color: white; font-size: 14px; font-family: 'Segoe UI',Arial,Helvetica,sans-serif'>Click Here To Change Passowrd <a href=http://astral.dataman.net.in/changepassword.aspx?UserId=" + UId + ">Click Me!</a></p></td></tr>");
                        //strBody.AppendLine("<tr><td colspan=2> <p style='background-color: white; font-size: 14px; font-family: 'Segoe UI',Arial,Helvetica,sans-serif'>To log on to FFMS visit the homepage :(<a href=http://www.astral.dataman.net.in target='_blank'>http://www.astral.dataman.net.in</a>)</p></td></tr>");
                        //strBody.AppendLine("<tr><td colspan=2> <p style='background-color: white; font-size: 14px; font-family: 'Segoe UI',Arial,Helvetica,sans-serif'>Regards,<br>Admin</p></td></tr>");
                        //strBody.AppendLine("</table>");

                        //MailMessage mail = new MailMessage(defaultmailId, txtEmailId.Text, "FFMS-Reset Your Password", strBody.ToString());
                        //NetworkCredential mailAuthenticaion = new NetworkCredential(defaultmailId, defaultpassword);

                        //SmtpClient mailclient = new SmtpClient("mail.dataman.in", 25);
                        //mailclient.EnableSsl = false;
                        //mailclient.UseDefaultCredentials = false;
                        //mailclient.Credentials = mailAuthenticaion;
                        //mail.IsBodyHtml = true;
                        //mailclient.Send(mail);
                        BootstrapSuccessMessage.Visible = true;
                        successLabel1.Text = "Reset password link sent to your email";
                        txtLoginId.Text = string.Empty;
                        txtEmailId.Text = string.Empty;
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "5;url=Loginn.aspx";
                        this.Page.Controls.Add(meta);
                        Label1.Visible = true;
                        Label1.Text = "You will now be redirected in 5 seconds";
                    }
                    else
                    {
                        BootstrapErrorMessage.Visible = true;
                        errormsglabel.Text = "This email does not exists";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private bool IsvalidUser(string userName, string emailid)
        {
          
            //Ankita - 20/may/2016- (For Optimization)
            //string isValidUserqry = @"select * from MastLogin where Email='" + emailId + "'";
            string isValidUserqry = @"select Id,Name from MastLogin where Name='" + userName + "' and Email='" + emailid + "'";
            DataTable isValidUserdt = DbConnectionDAL.GetDataTable(CommandType.Text, isValidUserqry);

            if (isValidUserdt.Rows.Count > 0)
            {
                UId = Convert.ToInt32(isValidUserdt.Rows[0]["Id"]);
                UName = Convert.ToString(isValidUserdt.Rows[0]["Name"]);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            txtEmailId.Text = "";
            Response.Redirect("~/Loginn.aspx", true);
        }
    }
}