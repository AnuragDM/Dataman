using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class ComplaintRespondList : System.Web.UI.Page
    {
         string CSVal="";
         string complainDocNo = "";
         string ComplaintBy = "";
         string Cname = "";
         string Cemail = "";
         string SPemail = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkViewDemoImg);
            if (!string.IsNullOrEmpty(Request.QueryString["val"]))
                CSVal = Convert.ToString(Request.QueryString["val"]);
            if (!string.IsNullOrEmpty(Request.QueryString["DocId"]))
                complainDocNo = Convert.ToString(Request.QueryString["DocId"]);
            lblCSID.Text = complainDocNo;
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["CStatus"]))
                {
                    if (Request.QueryString["CStatus"].ToUpper() == "PENDING")
                    {                        
                        lblstatus.Text = "Pending";                       
                    }
                    else if (Request.QueryString["CStatus"].ToUpper() == "WIP")
                    {                        
                        RdbStatus.SelectedValue = "W";
                        lblstatus.Text = "WIP";
                    }
                    else
                    { RdbStatus.SelectedValue = "R"; lblstatus.Text = "Resolved"; }
                }
                BindData();
              GetComplaintByname();
            }
        }
        private void GetComplaintByname()
        {
            DataTable cdt = new DataTable();
            string qryC = "";
            if (CSVal == "C")
            {
                qryC = "Select EmpName,Email from MastLogin where Id=(select UserId from TransComplaint where DocId='" + complainDocNo + "')";
                lblDoctype.Text = "Complaint Doc ID";
            }
            else
             {
                 qryC = "Select EmpName,Email from MastLogin where Id=(select UserId from TransSuggestion where DocId='" + complainDocNo + "')";
                 lblDoctype.Text = "Suggestion Doc ID";
             }
            cdt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, qryC);
            if (cdt.Rows.Count > 0)
            {
                Cname = cdt.Rows[0]["EmpName"].ToString();
                Cemail = cdt.Rows[0]["Email"].ToString();
            }
        }
        private void BindData()
        {
            try 
            {
                string qry = ""; DataTable dtn = new DataTable();
                if(CSVal=="C")
                {
                    lblheading.Text = "Complaint Details";
                //    qry = "select * from TransComplaint where DocId='"+complainDocNo+"'";
                    qry = "select tc.*,msr.SMName,msr.Email,md.DepName,mcn.Name,mi.ItemName,mp.PartyName,mp.PartyType,pt.PartyTypeName from TransComplaint tc left join MastSalesRep msr on tc.SMId=msr.SMId left join MastItem mi on tc.ItemId=mi.ItemId left join MastComplaintNature mcn on tc.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId left join MastParty mp on tc.DistId=mp.PartyId left join PartyType pt on mp.PartyType=pt.PartyTypeId where tc.DocId='" + complainDocNo + "'";
                    dtn = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                    if (dtn.Rows.Count > 0)
                    {
                        trc.Attributes.Remove("style"); trc1.Attributes.Remove("style");
                        trs.Attributes.Add("style", "display:none"); trs1.Attributes.Add("style", "display:none"); trs2.Attributes.Add("style", "display:none");
                        txtcomplaint.Text = dtn.Rows[0]["Remark"].ToString(); txtcomplaint.Enabled = false;
                        lbldate.InnerText = Convert.ToDateTime(dtn.Rows[0]["Vdate"]).ToString("dd/MMM/yyyy");
                        lbldistributor.InnerText = dtn.Rows[0]["PartyName"].ToString();
                        SPemail = dtn.Rows[0]["Email"].ToString();
                        if (dtn.Rows[0]["SalesDistr"].ToString() == "S") { span_sm.Visible = true; } else { span_sm.Visible = false; }
                        lblsm.InnerText = dtn.Rows[0]["SMName"].ToString();
                        lblproduct.InnerText = dtn.Rows[0]["ItemName"].ToString();
                        if (!string.IsNullOrEmpty(dtn.Rows[0]["ManufactureDate"].ToString()))
                            lblmfddate.InnerText = Convert.ToDateTime(dtn.Rows[0]["ManufactureDate"]).ToString("dd/MMM/yyyy");
                        lblbatchno.InnerText = dtn.Rows[0]["Batchno"].ToString();
                        lblnature.InnerText = dtn.Rows[0]["Name"].ToString();
                        if (!string.IsNullOrEmpty(dtn.Rows[0]["PartyTypeName"].ToString()))
                            lblpartytype.InnerText = dtn.Rows[0]["PartyTypeName"].ToString();
                        else
                            lblpartytype.InnerText = "Distributor";
                        lbldept.InnerText = dtn.Rows[0]["DepName"].ToString();
                        if (!string.IsNullOrEmpty(dtn.Rows[0]["ImgUrl"].ToString()))
                        {
                            hdvImg.Value = dtn.Rows[0]["ImgUrl"].ToString(); tdimg.Visible = true;

                        }
                        else { tdimg.Visible = false; }
                    }
                }
                else
                {
                    lblheading.Text = "Suggestion Details";
                    //qry = "select ts.*,Mi.ItemName from TransSuggestion ts left join mastItem mI on ts.ItemId=mI.ItemId  where DocId='" + complainDocNo + "'";
                    qry = "select ts.*,msr.SMName,msr.Email,md.DepName,mcn.Name,mi.ItemName,mp.PartyName,mp.PartyType,pt.PartyTypeName from TransSuggestion ts left join MastSalesRep msr on ts.SMId=msr.SMId left join MastItem mi on ts.ItemId=mi.ItemId left join MastComplaintNature mcn on ts.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId left join MastParty mp on ts.DistId=mp.PartyId left join PartyType pt on mp.PartyType=pt.PartyTypeId where DocId='" + complainDocNo + "'";
                    dtn = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                    if (dtn.Rows.Count > 0)
                    {
                        trs.Attributes.Remove("style"); trs1.Attributes.Remove("style"); trs2.Attributes.Remove("style");
                        trc.Attributes.Add("style", "display:none"); trc1.Attributes.Add("style", "display:none");
                        lbldate.InnerText = Convert.ToDateTime(dtn.Rows[0]["Vdate"]).ToString("dd/MMM/yyyy");
                        lbldistributor.InnerText = dtn.Rows[0]["PartyName"].ToString();
                        if (dtn.Rows[0]["SalesDistr"].ToString() == "S") { span_sm.Visible = true; } else { span_sm.Visible = false; }
                        SPemail = dtn.Rows[0]["Email"].ToString();
                        lblsm.InnerText = dtn.Rows[0]["SMName"].ToString();
                        lblproduct.InnerText = dtn.Rows[0]["ItemName"].ToString();
                        lblnature.InnerText = dtn.Rows[0]["Name"].ToString();
                        if (!string.IsNullOrEmpty(dtn.Rows[0]["PartyTypeName"].ToString()))
                            lblpartytype.InnerText = dtn.Rows[0]["PartyTypeName"].ToString();
                        else
                            lblpartytype.InnerText = "Distributor";
                        lbldept.InnerText = dtn.Rows[0]["DepName"].ToString();
                        txtNAR.Text = dtn.Rows[0]["NewApplicationArea"].ToString(); txtNAR.Enabled = false;
                        txtTA.Text = dtn.Rows[0]["TechnicalAdvantage"].ToString(); txtTA.Enabled = false;
                        txtMPB.Text = dtn.Rows[0]["MakeProductBetter"].ToString(); txtMPB.Enabled = false;
                        if (!string.IsNullOrEmpty(dtn.Rows[0]["ImgUrl"].ToString()))
                        {
                            hdvImg.Value = dtn.Rows[0]["ImgUrl"].ToString(); tdimg.Visible = true;

                        }
                        else { tdimg.Visible = false; }
                    }
                }
                //Ankita - 11/may/2016- (For Optimization)
               // string str = "select tcr.*,Ml.Empname from TransComplaintRespond tcr inner join MastLogin Ml on tcr.respondedby=Ml.Id  where CompDocID='" + complainDocNo + "'";
                string str = "select tcr.RespondDate,tcr.Remarks,Ml.Empname from TransComplaintRespond tcr inner join MastLogin Ml on tcr.respondedby=Ml.Id  where CompDocID='" + complainDocNo + "'";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                rpt.DataSource = dt;
                rpt.DataBind();
             }
            catch (Exception ex) { ex.ToString(); }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try {
                bool status=false;
                string strcomplain = "";
                BAL.TransComplaint.TComplAll TObj = new BAL.TransComplaint.TComplAll();
                if (RdbStatus.SelectedValue == "R")
                    status = true;
                int val = TObj.InsertResponse(0, complainDocNo,Convert.ToInt32(Settings.Instance.UserID), txtremarks.Text.Trim(), status);
               if (val > 0)
               {
                   BindData();
                   //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Remark Added Successfully');", true);
                 string Sname = "";
         
                 Sname = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text,"Select EmpName from MastLogin where Id="+Settings.Instance.UserID+"").ToString();
                 if (CSVal == "C")
                 {
                      strcomplain = "Select EmpName from MastLogin where Id in (select UserId from TransComplaint where DocId='" + complainDocNo + "')".ToString();
                 }
                 else
                 {
                      strcomplain = "Select EmpName from MastLogin where Id in (select UserId from TransSuggestion where DocId='" + complainDocNo + "')".ToString();
                 }
                 
                 ComplaintBy = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text,strcomplain).ToString();               
               
                 SendEmail(complainDocNo, ComplaintBy,Sname, txtremarks.Text.Trim());
                 txtremarks.Text = "";
                 System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                   //HtmlMeta meta = new HtmlMeta();
                   //meta.HttpEquiv = "Refresh";
                   //meta.Content = "2;url=ComplainRespond.aspx";
                   //btnSave.Visible = false;
                   //this.Page.Controls.Add(meta);      
                   //Response.Redirect("~/ComplainRespond.aspx");
               }
               else {
                   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
               }
            }
            catch (Exception xe) { }
        }


        public void SendEmail(string ComplaintDocID, string EmpName, string RespondedBy, string Remark)
        {
            try
            {
                string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                string Status = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (RdbStatus.SelectedValue == "R") {
                        Status = "Resolved";}
                        else{Status= "WIP";}
                    
                    string strSubject = ""; string strMailBody = "";                    
                    if (CSVal == "C")
                    {
                        string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='ComplaintResponse'";
                        DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                        string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='ComplaintResponse'";
                        DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                        strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                        strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                        if (dtVar != null && dtVar.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtVar.Rows.Count; j++)
                            {
                                if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                                {
                                    //strSubject = strSubject.Replace("{CompDocID}", complainDocNo);
                                    strSubject = strSubject.Replace("{EmployeeName}", EmpName.ToString());
                                }

                                ///////////////////////////////////////////
                                if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                    {
                                        strMailBody = strMailBody.Replace("{EmployeeName}", EmpName.ToString());
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{CompDocID}")
                                    {
                                        strMailBody = strMailBody.Replace("{CompDocID}", complainDocNo);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Complain}")
                                    {
                                        strMailBody = strMailBody.Replace("{Complain}", txtcomplaint.Text);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Vdate}")
                                    {
                                        strMailBody = strMailBody.Replace("{Vdate}", lbldate.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{CompNature}")
                                    {
                                        strMailBody = strMailBody.Replace("{CompNature}", lblnature.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{CompDept}")
                                    {
                                        strMailBody = strMailBody.Replace("{CompDept}", lbldept.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{RespondedBy}")
                                    {
                                        strMailBody = strMailBody.Replace("{RespondedBy}", RespondedBy);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remark}")
                                    {
                                        strMailBody = strMailBody.Replace("{Remark}", Remark);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Status}")
                                    {
                                        strMailBody = strMailBody.Replace("{Status}", Status);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='SuggestionResponse'";
                        DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                        string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='SuggestionResponse'";
                        DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                        strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                        strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);
                        if (dtVar != null && dtVar.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtVar.Rows.Count; j++)
                            {
                                if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                                {
                                    //strSubject = strSubject.Replace("{SuggDocID}", complainDocNo);
                                    strSubject = strSubject.Replace("{EmployeeName}", EmpName.ToString());
                                }

                                ///////////////////////////////////////////
                                if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                    {
                                        strMailBody = strMailBody.Replace("{EmployeeName}", EmpName.ToString());
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggDocID}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggDocID}", complainDocNo);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggNAP}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggNAP}", txtNAR.Text);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggTA}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggTA}", txtTA.Text);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggPB}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggPB}", txtMPB.Text);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Vdate}")
                                    {
                                        strMailBody = strMailBody.Replace("{Vdate}", lbldate.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggNature}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggNature}", lblnature.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{SuggDept}")
                                    {
                                        strMailBody = strMailBody.Replace("{SuggDept}", lbldept.InnerText);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{RespondedBy}")
                                    {
                                        strMailBody = strMailBody.Replace("{RespondedBy}", RespondedBy);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Action}")
                                    {
                                        strMailBody = strMailBody.Replace("{Action}", Remark);
                                    }
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Status}")
                                    {
                                        strMailBody = strMailBody.Replace("{Status}", Status);
                                    }

                                }
                            }
                        }
                    }
                    string emailto = "";
                    GetComplaintByname();
                    emailto = Cemail;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;
                    mail.To.Add(new MailAddress(emailto));
                    if (!string.IsNullOrEmpty(SPemail))
                    {
                        if (SPemail.ToLower() != Cemail.ToLower()) 
                        {
                            mail.CC.Add(new MailAddress(SPemail));
                        }
                    }
                    if (!string.IsNullOrEmpty(hdvImg.Value))
                    {
                        string attachmentPath = System.Web.HttpContext.Current.Server.MapPath(hdvImg.Value);
                    //   mail.Attachments.Add(new Attachment(path));
                        //var path = Server.MapPath(Path.GetFileName(hdvImg.Value));
                        //    mail.Attachments.Add(new Attachment(Path.GetFileName(hdvImg.Value)));

                       Attachment inline = new Attachment(attachmentPath);
                       inline.ContentDisposition.Inline = true;
                       inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                       inline.ContentId = Guid.NewGuid().ToString();
                       inline.ContentType.MediaType = "image/png";
                       inline.ContentType.Name = Path.GetFileName(attachmentPath);

                       mail.Attachments.Add(inline);
                    }

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }
                //End


             
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void lnkViewDemoImg_Click(object sender, EventArgs e)
        {
            try
            {

                Response.ContentType = "image/png"; 
                if (hdvImg.Value != "")
                {
                    
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdvImg.Value));
                    Response.WriteFile(hdvImg.Value);
                    Response.End();
                }
                else
                {
                    Response.End();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

    }
}