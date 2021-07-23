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
using System.IO;
using BAL.AdvanceReq;
using System.Net.Mail;
using System.Net;

namespace AstralFFMS
{
    public partial class ExpenseAdvanceReq : System.Web.UI.Page
    {
        AdvanceReqBAL obj = new AdvanceReqBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["ID"] = parameter;
                FillControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
                txtmDate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
            }
            //Ankita - 11/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);         
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }

            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.Enabled = btnSave.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
            if(!Page.IsPostBack)
            { 

                txtFrTime.Value = "10:00am";
                txtToTime.Value = "06:00pm";
                FillState();
                btnDelete.Visible = false;
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                EffDateFrom.Attributes.Add("readonly", "readonly");
                EffDateTo.Attributes.Add("readonly", "readonly");
                //Ankita - 11/may/2016- (For Optimization)
                //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
                //   + Settings.Instance.RoleID + "").ToString();
                string RoleType = Settings.Instance.RoleType;
                if (RoleType.ToUpper() == "Admin".ToUpper())
                {
                    Approve.Visible = true;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    mainDiv.Style.Add("display", "none");
                    rptmain.Style.Add("display", "block");
                    btnBack.Visible = false;
                    btnBack1.Visible = true;
                    btnFind.Visible = false;
                    fillRepeter();
                   
                }
                else
                {
                    mainDiv.Style.Add("display", "block");
                    rptmain.Style.Add("display", "none");
                    btnBack.Visible = true;
                    btnBack1.Visible = false;
                    Approve.Visible = false;
                    btnSave.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnFind.Visible = true;
                }
            }
        }

        private void FillControls(int ID)
        {
            string str = @"SELECT * FROM TransExpAdvance WHERE ID="+ID;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(dt!=null && dt.Rows.Count>0)
            {
                EffDateFrom.Text = Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString("dd/MMM/yyyy");
                EffDateTo.Text = Convert.ToDateTime(dt.Rows[0]["ToDate"]).ToString("dd/MMM/yyyy");
                Remarks.InnerText = Convert.ToString(dt.Rows[0]["Remarks"]);
                txtAmount.Text = Convert.ToString(dt.Rows[0]["Amount"]);
                txtAppAmt.Text = Convert.ToString(dt.Rows[0]["ApprovedAmt"]);
                txtFrTime.Value = Convert.ToString(dt.Rows[0]["FromTime"]);
                txtToTime.Value = Convert.ToString(dt.Rows[0]["ToTime"]);
                FillState();

                string strst1 = @"SELECT Distinct T1.StateID,T2.AreaName AS StateName FROM TransExpAdvance1 AS T1 Inner JOIN MastArea AS T2
                                        ON T1.StateID=T2.AreaId WHERE T1.ExpAdvID=" + ID + " ORDER By T2.AreaName";
                DataTable dtst = DbConnectionDAL.GetDataTable(CommandType.Text, strst1);

                foreach (ListItem item in lstState.Items)
                {
                    for (int i = 0; i < dtst.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(item.Value) == Convert.ToInt32(dtst.Rows[i]["StateID"]))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }


                string strst = "", strst3 = "";
                foreach (ListItem item in lstState.Items)
                {
                    if (item.Selected)
                    {
                        strst += item.Value + ",";                     
                    }
                }

                strst = strst.TrimStart(',').TrimEnd(',');               

                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId in (" + strst + ") order by CityName";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                DataView dv = new DataView(dt1);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "CityName<>.";
                if (dv.ToTable().Rows.Count > 0)
                {
                    ListBox1.DataSource = dv.ToTable();
                    ListBox1.DataTextField = "CityName";
                    ListBox1.DataValueField = "CityID";
                    ListBox1.DataBind();
                }
                //Ankita - 11/may/2016- (For Optimization)
                //string str1 = @"SELECT * FROM TransExpAdvance1 WHERE ExpAdvID=" + ID;
                string str1 = @"SELECT VisitCityID FROM TransExpAdvance1 WHERE ExpAdvID=" + ID;
                DataTable dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                foreach (ListItem item in ListBox1.Items)
                {
                    for(int i=0;i<dt2.Rows.Count;i++)
                    {
                        if(Convert.ToInt32(item.Value) == Convert.ToInt32(dt2.Rows[i]["VisitCityID"]))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }

                DataTable dtStCity = new DataTable();
                dtStCity.Columns.Add("State");
                dtStCity.Columns.Add("City");

                if(dtst!=null && dtst.Rows.Count>0)
                {
                    for(int i=0;i<dtst.Rows.Count;i++)
                    {
                        dtStCity.Rows.Add();
                        dtStCity.Rows[dtStCity.Rows.Count - 1]["State"] = dtst.Rows[i]["StateName"];

                        string strCity = "";

                        //string strCQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId =" + Convert.ToInt32(dtst.Rows[i]["StateID"]) + " order by CityName";
                        string strCQ = @"SELECT Distinct T1.VisitCityID AS CityID,T2.AreaName AS CityName FROM TransExpAdvance1 AS T1 Inner JOIN MastArea AS T2
                                        ON T1.VisitCityID=T2.AreaId WHERE T1.StateID=" + Convert.ToInt32(dtst.Rows[i]["StateID"]) + " AND T1.ExpAdvID=" + ID + " ORDER By T2.AreaName";
                        DataTable dtCQ = DbConnectionDAL.GetDataTable(CommandType.Text, strCQ);

                        if(dtCQ!=null && dtCQ.Rows.Count>0)
                        {
                            for(int j=0;j<dtCQ.Rows.Count;j++)
                            {
                                strCity += Convert.ToString(dtCQ.Rows[j]["CityName"]) + ",";  
                            }
                        }
                        strCity = strCity.TrimStart(',').TrimEnd(',');
                        dtStCity.Rows[dtStCity.Rows.Count - 1]["City"] = strCity;
                    }
                }

                if(dtStCity!=null && dtStCity.Rows.Count>0)
                {
                    gvDetails.DataSource = dtStCity;
                    gvDetails.DataBind();
                }
                //Ankita - 11/may/2016- (For Optimization)
                //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
                //  + Settings.Instance.RoleID + "").ToString();
                string RoleType = Settings.Instance.RoleType;
                if (RoleType.ToUpper() == "Admin".ToUpper())
                {
                   
                    btnDelete.Visible = false;
                    CalendarExtender3.Enabled = false;
                    CalendarExtender4.Enabled = false;
                    //EffDateFrom.Attributes.Add("disabled", "");
                    //EffDateTo.Attributes.Add("disabled", "");
                    //txtAmount.Attributes.Add("disabled", "disabled");
                    txtAmount.ReadOnly = true;
                    Remarks.Attributes.Add("disabled", "disabled");
                    txtFrTime.Attributes.Add("disabled", "disabled");
                    txtToTime.Attributes.Add("disabled", "disabled");                   
                    CityandState.Visible=true;
					VisitState.Visible=false;
                    VisitCity.Visible = false;

                    if (txtAppAmt.Text != "")
                    {                      
                        txtAppAmt.Attributes.Add("disabled", "disabled");  
                        btnApprove.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        txtAppAmt.Attributes.Remove("disabled");
                        btnApprove.Attributes.Remove("disabled");
                    }
                  
                }
                else
                {
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;

                    CityandState.Visible = false;
                    VisitState.Visible = true;
                    VisitCity.Visible = true;

                    if(txtAppAmt.Text!="")
                    {                      
                        btnSave.Attributes.Add("disabled", "disabled");
                        btnDelete.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        btnSave.Attributes.Remove("disabled");
                        btnDelete.Attributes.Remove("disabled");
                    }
                }
               
            }
        }

        private void FillState()
        {
            try
            {
                string strQ = "select AreaID,AreaName from mastarea MA where AreaType='State' and Active='1' order by AreaName";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                DataView dv = new DataView(dt);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "AreaName<>.";
                if (dv.ToTable().Rows.Count > 0)
                {
                    lstState.DataSource = dv.ToTable();
                    lstState.DataTextField = "AreaName";
                    lstState.DataValueField = "AreaID";
                    lstState.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        //protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + Convert.ToInt32(ddlState.SelectedItem.Value.Trim()) + " order by CityName";
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
        //    DataView dv = new DataView(dt);
        //    //     dv.RowFilter = "RoleName='Level 1'";
        //    dv.RowFilter = "CityName<>.";
        //    if (dv.ToTable().Rows.Count > 0)
        //    {
        //        ListBox1.DataSource = dv.ToTable();
        //        ListBox1.DataTextField = "CityName";
        //        ListBox1.DataValueField = "CityID";
        //        ListBox1.DataBind();
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(EffDateFrom.Text) > Convert.ToDateTime(EffDateTo.Text))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }

            if (Convert.ToDateTime(EffDateFrom.Text) == Convert.ToDateTime(EffDateTo.Text))
            {
                string fromtime = EffDateFrom.Text + " " + txtFrTime.Value;
                string totime = EffDateTo.Text + " " + txtToTime.Value;
                if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Time Should be less than To Time');", true);
                    return;
                }
            }

            bool flag = true;

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    flag = false;
                    break;
                }
            }
            if(flag)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select city to visit');", true);
                return;
            }

            DataTable dtVisitCity = new DataTable();
            dtVisitCity.Columns.Add("CityID");
            dtVisitCity.Columns.Add("StateID");

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    dtVisitCity.Rows.Add();
                    dtVisitCity.Rows[dtVisitCity.Rows.Count - 1]["CityID"] = item.Value;

                    string strQ = "SELECT DISTINCT stateid,statename FROM ViewGeo WHERE cityid="+Convert.ToInt32(item.Value)+"";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);

                    dtVisitCity.Rows[dtVisitCity.Rows.Count - 1]["StateID"] = dt.Rows[0]["stateid"];
                }
            }

            DateTime Currdt = Settings.GetUTCTime();
            //context.sp_Getdocid("PORD", Currdt, ref DocID);
            string docID = Settings.GetDocID("ADEXP", Currdt);
            Settings.SetDocID("ADEXP", docID);

            if (btnSave.Text == "Update")
            {
                UpdateAdvanceReq(EffDateFrom.Text, EffDateTo.Text, Convert.ToDecimal(txtAmount.Text), Remarks.InnerText, "0", dtVisitCity, Settings.Instance.UserID, txtFrTime.Value, txtToTime.Value);
            }
            else
            {
                InsertAdvanceReq(EffDateFrom.Text, EffDateTo.Text, Convert.ToDecimal(txtAmount.Text), Remarks.InnerText, "0", dtVisitCity, docID, Settings.Instance.UserID,txtFrTime.Value,txtToTime.Value);

            }

        }

        private void InsertAdvanceReq(string FromDate, string ToDate, decimal amt, string Remarks1, string StateID, DataTable dtVisitCity, string docID, string UserID,string FromTime,string ToTime)
        {
            try
            {
                int retval = obj.InsertAdvanceReq(FromDate, ToDate, amt, Remarks1, StateID, dtVisitCity, docID, UserID,FromTime,ToTime);
                if(retval>0)
                {
                    SendEmail();
                    EffDateFrom.Text = "";
                    EffDateTo.Text = "";
                    txtAmount.Text = "";
                    Remarks.InnerText = "";
                    EffDateTo.Text = "";
                    txtFrTime.Value = "";
                    txtToTime.Value = "";
                    ListBox1.DataSource = new DataTable();
                    ListBox1.DataBind();
                    lstState.DataSource = new DataTable();
                    lstState.DataBind();
                    FillState();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void SendEmail()
        {
            try
            {
                string defaultmailId = "", defaultpassword = "", port = "";
                string qry = "select SenderEmailID,SenderPassword,Port,ExpenseAdminEmail,MailServer from [MastEnviro]";

                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

                string[] emailToAll = new string[20];
                string emailNew = "";

                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["SenderEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["SenderPassword"].ToString();
                    port = checkemaildt.Rows[0]["Port"].ToString();

                    emailNew = checkemaildt.Rows[0]["ExpenseAdminEmail"].ToString();
                    emailToAll = emailNew.Split(';');
                }

                string strEmail = @"SELECT Email AS email1,EmpName FROM MastLogin WHERE Id=" + Settings.Instance.UserID;
                DataTable dtEmail = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strEmail);
                if (dtEmail.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='AdvanceRequestSubmited'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='AdvanceRequestSubmited'";
                    DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                    string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                    string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                    if (dtVar != null && dtVar.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtVar.Rows.Count; j++)
                        {
                            if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {  
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                {
                                    strSubject = strSubject.Replace("{EmployeeName}", Convert.ToString(dtEmail.Rows[0]["EmpName"]));
                                }
                            }
                            
                        }
                    }

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.From = new MailAddress(defaultmailId);
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;
                    mail.To.Add(new MailAddress(Convert.ToString(dtEmail.Rows[0]["email1"])));
                    //mail.To.Add(new MailAddress(Convert.ToString(checkemaildt.Rows[0]["ExpenseAdminEmail"])));

                    if (emailToAll.Length > 0)
                    {
                        for (int i = 0; i < emailToAll.Length; i++)
                        {
                            mail.To.Add(new MailAddress(emailToAll[i]));
                        }
                    }

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(checkemaildt.Rows[0]["SenderEmailId"]), Convert.ToString(checkemaildt.Rows[0]["SenderPassword"]));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString(checkemaildt.Rows[0]["MailServer"]), Convert.ToInt32(checkemaildt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }




            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateAdvanceReq(string FromDate, string ToDate, decimal amt, string Remarks1, string StateID, DataTable dtVisitCity, string UserID, string FromTime, string ToTime)
        {
            try
            {
                int retval = obj.UpdateAdvanceReq(Convert.ToInt32(ViewState["ID"]), FromDate, ToDate, amt, Remarks1, StateID, dtVisitCity, UserID, FromTime, ToTime);
                if (retval > 0)
                {
                    ClearControls(); FillState();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseAdvanceReq.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = obj.DeleteExpAdv(Convert.ToString(ViewState["ID"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                   
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                   
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        { //Ankita - 11/may/2016- (For Optimization)
            //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
            //      + Settings.Instance.RoleID + "").ToString();
            string RoleType = Settings.Instance.RoleType;
            if (RoleType.ToUpper() == "Admin".ToUpper())
            {
                fillRepeter();
                mainDiv.Style.Add("display", "none");
                rptmain.Style.Add("display", "block");
            }
            else
            {
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
           
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        private void ClearControls()
        {
            EffDateFrom.Text = "";
            EffDateTo.Text = "";
            txtAmount.Text = "";
            Remarks.InnerText = "";
            EffDateTo.Text = "";
            txtFrTime.Value = "";
            txtToTime.Value = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            ListBox1.DataSource = new DataTable();
            ListBox1.DataBind();
            lstState.DataSource = new DataTable();
            lstState.DataBind();
            txtAppAmt.Text = "";
        }

        private void fillRepeter()
        {
            string str = ""; string mainQry = ""; string mainQry1 = "";
            //Ankita - 11/may/2016- (For Optimization)
            string RoleType = Settings.Instance.RoleType;
            //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
            //       + Settings.Instance.RoleID + "").ToString();

            if (ddlStatus.SelectedValue == "All")
            {
                mainQry = " and T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
                mainQry1 = "T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
            }
            else
            {
                mainQry = " and T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and T1.RecStatus='" + ddlStatus.SelectedItem.Value.Trim() + "'";
                mainQry1 = "T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and T1.RecStatus='" + ddlStatus.SelectedItem.Value.Trim() + "'";
            }

            if (RoleType.ToUpper() == "Admin".ToUpper())
            {
                str = @"SELECT T1.ID,T1.FromDate,T1.ToDate,T1.Amount,isnull(T1.ApprovedAmt,0) AS ApprovedAmt,
                            DocId,T2.EmpName,T1.RecStatus,T3.AreaName StateName
                            FROM TransExpAdvance AS T1 LEFT JOIN MastLogin AS T2 ON T1.UserID=T2.Id
                            LEFT JOIN MastArea AS T3 ON T1.StateID=T3.AreaId Where " + mainQry1 + "";
            }
            else
            {
                str = @"SELECT T1.ID,T1.FromDate,T1.ToDate,T1.Amount,isnull(T1.ApprovedAmt,0) AS ApprovedAmt,
                            DocId,T2.EmpName,T1.RecStatus,T3.AreaName StateName
                            FROM TransExpAdvance AS T1 LEFT JOIN MastLogin AS T2 ON T1.UserID=T2.Id
                            LEFT JOIN MastArea AS T3 ON T1.StateID=T3.AreaId Where T1.UserID=" + Convert.ToInt32(Settings.Instance.UserID) + "" + mainQry + "";
            }
           
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }

      

        private void SendApprovedEmail()
        {
            try
            {
                string defaultmailId = "", defaultpassword = "", port = "";
                string qry = "select SenderEmailID,SenderPassword,Port,ExpenseAdminEmail,MailServer from [MastEnviro]";

                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

                string[] emailToAll = new string[20];               
                string emailNew = "";
               


                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["SenderEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["SenderPassword"].ToString();
                    port = checkemaildt.Rows[0]["Port"].ToString();


                    emailNew = checkemaildt.Rows[0]["ExpenseAdminEmail"].ToString();
                    emailToAll = emailNew.Split(';');
                }

                //string strEmail = @"SELECT Email AS email1,EmpName FROM MastLogin WHERE Id=" + Settings.Instance.UserID;
                //DataTable dtEmail = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strEmail);

                string str = @"SELECT T2.EmpName,T2.Email FROM TransExpAdvance AS T1 INNER JOIN MastLogin AS T2
                                ON T1.UserID=T2.Id WHERE T1.ID=" + Convert.ToInt32(ViewState["ID"]);
                DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='AdvanceRequestApproved'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='AdvanceRequestApproved'";
                    DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                    string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                    string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                    if (dtVar != null && dtVar.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtVar.Rows.Count; j++)
                        {
                            if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                {
                                    strSubject = strSubject.Replace("{EmployeeName}", Convert.ToString(dt.Rows[0]["EmpName"]));
                                }
                            }

                        }
                    }

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.From = new MailAddress(defaultmailId);
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;
                    mail.To.Add(new MailAddress(Convert.ToString(dt.Rows[0]["Email"])));
                    //mail.To.Add(new MailAddress(Convert.ToString(checkemaildt.Rows[0]["ExpenseAdminEmail"])));

                    if (emailToAll.Length > 0)
                    {
                        for (int i = 0; i < emailToAll.Length; i++)
                        {
                            mail.To.Add(new MailAddress(emailToAll[i]));
                        }
                    }

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(checkemaildt.Rows[0]["SenderEmailId"]), Convert.ToString(checkemaildt.Rows[0]["SenderPassword"]));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString(checkemaildt.Rows[0]["MailServer"]), Convert.ToInt32(checkemaildt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }




            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCanelApp_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = ""; string mainQry = ""; string mainQry1 = "";
            //Ankita - 11/may/2016- (For Optimization)
            //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
            //       + Settings.Instance.RoleID + "").ToString();
            string RoleType = Settings.Instance.RoleType;
            if(ddlStatus.SelectedValue=="All")
            {
                mainQry = " and T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
                mainQry1 = "T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
            }
            else
            {
                mainQry = " and T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and T1.RecStatus='" + ddlStatus.SelectedItem.Value.Trim() + "'";
                mainQry1 = "T1.CreatedOn between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and T1.RecStatus='" + ddlStatus.SelectedItem.Value.Trim() + "'";
            }
           

            if (RoleType.ToUpper() == "Admin".ToUpper())
            {
                str = @"SELECT T1.ID,T1.FromDate,T1.ToDate,T1.Amount,isnull(T1.ApprovedAmt,0) AS ApprovedAmt,
                            DocId,T2.EmpName,T1.RecStatus,T3.AreaName StateName
                            FROM TransExpAdvance AS T1 LEFT JOIN MastLogin AS T2 ON T1.UserID=T2.Id
                            LEFT JOIN MastArea AS T3 ON T1.StateID=T3.AreaId Where " + mainQry1 + "";
            }
            else
            {
                str = @"SELECT T1.ID,T1.FromDate,T1.ToDate,T1.Amount,isnull(T1.ApprovedAmt,0) AS ApprovedAmt,
                            DocId,T2.EmpName,T1.RecStatus,T3.AreaName StateName
                            FROM TransExpAdvance AS T1 LEFT JOIN MastLogin AS T2 ON T1.UserID=T2.Id
                            LEFT JOIN MastArea AS T3 ON T1.StateID=T3.AreaId Where T1.UserID=" + Convert.ToInt32(Settings.Instance.UserID) + "" + mainQry + "";
            }


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }

        protected void lstState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strst = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    strst += item.Value + ",";
                }
            }
            strst = strst.TrimStart(',').TrimEnd(',');

            string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId in (" + strst + ") order by CityName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            DataView dv = new DataView(dt);
            //     dv.RowFilter = "RoleName='Level 1'";
            dv.RowFilter = "CityName<>.";
            if (dv.ToTable().Rows.Count > 0)
            {
                ListBox1.DataSource = dv.ToTable();
                ListBox1.DataTextField = "CityName";
                ListBox1.DataValueField = "CityID";
                ListBox1.DataBind();
            }
        }

      

        protected void btnApprove_Click2(object sender, EventArgs e)
        {
                btnDelete.Visible = false;
                if (txtAppAmt.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter approved Amount');", true);
                    return;
                }

                if (Convert.ToDecimal(txtAppAmt.Text) <= 0M)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter approved Amount greater than 0');", true);
                    return;
                }

                if (Convert.ToDecimal(txtAppAmt.Text) > Convert.ToDecimal(txtAmount.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Approved amount must be less than or equal to amount');", true);
                    return;
                }

                int retval = obj.UpdateAdvanceReqForAppAmt(Convert.ToInt32(ViewState["ID"]), Convert.ToDecimal(txtAppAmt.Text), Settings.Instance.UserID);
                if (retval > 0)
                {
                    btnApprove.Attributes.Add("disabled", "disabled");
                    SendApprovedEmail();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Approved Successfully');", true);
                    return;
                }
            
        }
    }
}