using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.Text.RegularExpressions;
using System.IO;

namespace AstralFFMS
{
    public partial class EmailSettings : System.Web.UI.Page
    {
        BAL.EmailSetting.EmailSettings dp = new BAL.EmailSetting.EmailSettings();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {         

            if (!IsPostBack)
            {
                Password.Attributes["type"] = "password";
                LoadData();
               
            }
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
        }

        private void LoadData()
        {
            string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.PurchaseEmailId,T1.PurchaseEmailCC,
                            T1.ChangePasswordEmailId,T1.ChangePasswordCC,T1.ForgetPasswordEmailId,T1.ForgetPasswordCC,
                            T1.OrderListEmailId,t1.OrderListEmailCC,isnull(T1.MailServer,'') AS MailServer,T1.ExpenseAdminEmail 
                            FROM MastEnviro AS T1";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(dt!=null && dt.Rows.Count>0)
            {
                txtMailServer.Text = dt.Rows[0]["MailServer"].ToString();
                SenderEmailID.Value = dt.Rows[0]["SenderEmailId"].ToString();
                Password.Text = dt.Rows[0]["SenderPassword"].ToString();
                Port.Text = dt.Rows[0]["Port"].ToString();
                PurchaseEmailID.Value = dt.Rows[0]["PurchaseEmailId"].ToString();
                PurchaseEmailCC.Value = dt.Rows[0]["PurchaseEmailCC"].ToString();
                OrderApprovalEmailId.Value = dt.Rows[0]["OrderListEmailId"].ToString();
                OrderApprovalEmailCC.Value = dt.Rows[0]["OrderListEmailCC"].ToString();
                ChangePasswordEmailId.Value = dt.Rows[0]["ChangePasswordEmailId"].ToString();
                ChangePasswordEmailCC.Value = dt.Rows[0]["ChangePasswordCC"].ToString();
                ExpenseAdminEmailID.Value = dt.Rows[0]["ExpenseAdminEmail"].ToString();
            }
        }  
      

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {          
                if (SenderEmailID.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Sender Email ID.');", true);
                    return;
                }
                if (Password.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Password.');", true);
                    return;
                }
                if (Port.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Port.');", true);
                    return;
                }
                if (txtMailServer.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Mail Server.');", true);
                    return;
                }
                if (PurchaseEmailID.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Purchase Email ID.');", true);
                    return;
                }
                if (PurchaseEmailCC.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Purchase Email CC.');", true);
                    return;
                }
                if (OrderApprovalEmailId.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter order approval Email ID.');", true);
                    return;
                }
                if (OrderApprovalEmailCC.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter order approval Email CC.');", true);
                    return;
                }
                if (ChangePasswordEmailId.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter change password Email ID.');", true);
                    return;
                }
                if (ChangePasswordEmailCC.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter change password Email CC.');", true);
                    return;
                }

                if (ChangePasswordEmailCC.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Expense Admin Email ID.');", true);
                    return;
                }
              

                if (btnSave.Text == "Update")
                {
                    UpdateEmailSettings();
                }
                else
                {
                    InsertEmailSettings();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertEmailSettings()
        {
            int retsave = dp.Insert(SenderEmailID.Value, Password.Text, Port.Text, PurchaseEmailID.Value, PurchaseEmailCC.Value, 
                OrderApprovalEmailId.Value, OrderApprovalEmailCC.Value, ChangePasswordEmailId.Value, ChangePasswordEmailCC.Value,txtMailServer.Text,ExpenseAdminEmailID.Value);

            if (retsave != 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();               
            }
        }

        private void UpdateEmailSettings()
        {
            int retsave = dp.Insert(SenderEmailID.Value, Password.Text, Port.Text, PurchaseEmailID.Value, PurchaseEmailCC.Value,
                OrderApprovalEmailId.Value, OrderApprovalEmailCC.Value, ChangePasswordEmailId.Value, ChangePasswordEmailCC.Value, txtMailServer.Text, ExpenseAdminEmailID.Value);

            if (retsave != 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
            }
        }
       

        private void ClearControls()
        {
            txtMailServer.Text = "";
            SenderEmailID.Value="";
			Password.Text="";
			Port.Text ="";
			PurchaseEmailID.Value="";
			PurchaseEmailCC.Value="";
			OrderApprovalEmailId.Value="";
            OrderApprovalEmailCC.Value = "";
            ChangePasswordEmailId.Value="";
            ChangePasswordEmailCC.Value = "";
            ExpenseAdminEmailID.Value = "";

        }

     

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/EmailSettings.aspx");
        }      

        protected void btnFind_Click(object sender, EventArgs e)
        {           
            btnSave.Text = "Update";            
           
        }

       
    }
}