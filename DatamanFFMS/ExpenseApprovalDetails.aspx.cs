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
using System.Web.Services;
using System.Net;
namespace AstralFFMS
{
    public partial class ExpenseApprovalDetails : System.Web.UI.Page
    {
        //  Int32 ExpGrpId = 0;  bool ExpStatus = true;
        ExpenseBAL EB = new ExpenseBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ExpenseGroupId"] != null)
                {
                    SetData();
                    if (Request.QueryString["Status"] != null)
                    {
                        if (Request.QueryString["Status"].ToUpper() == "View".ToUpper() || Request.QueryString["Status"].ToUpper() == "Active".ToUpper() || Request.QueryString["Status"].ToUpper() == "Approved".ToUpper())
                        {
                            ViewState["ExpStatus"] = false;
                            if (Request.QueryString["Status"].ToUpper() == "Approved".ToUpper())
                            {
                                btnUnapprove.Visible = true; lnkSubmit.Visible = false;
                            }
                            else { btnUnapprove.Visible = false; lnkSubmit.Visible = true; }
                            //  ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                            btnSaveExpSheet.Visible = false; btnUnsubmit.Visible = false; lnkSubmit.Visible = false;
                        }
                        else
                        {
                            btnUnapprove.Visible = false;
                            ViewState["ExpStatus"] = true;
                        }

                    }
                    else
                    { btnSaveExpSheet.Visible = true; btnUnsubmit.Visible = true; lnkSubmit.Visible = true; }
                    ViewState["ExpGrpId"] = Convert.ToInt32(Request.QueryString["ExpenseGroupId"]);
                    DataGrid();
                }
            }

        }
        #region BindDataGrids
        private void DataGrid()
        {
            DataTable dt = new DataTable();
            // Change Nishu 24/06/2016 order by Bill Date

            //string strQqy = "Select * from ( SELECT Ed.expensedetailid,msr.smid,ml.empname,ED.billamount,Isnull(Ed.approvedamount, 0) AS ApprovedAmount,Isnull(ed.issupportingapproved, 0) AS issupportingapproved,CASE Isnull(Ed.approvedamount, 0) WHEN 0 THEN 0 ELSE Ed.approvedamount END AS SavedAmt, Ed.approverremarks,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/')    AS BillDate,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate, ED.issupportingattached,ED.remarks,Ed.tocity, Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/')   AS ToDate, MET.NAME,MAC.areaname AS FromCityName,'' AS TocityName,MET.expensetypecode,CASE ED.issupportingattached WHEN 1 THEN 'Yes' ELSE 'No'  END AS IsSupportingAttached1,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,(SELECT TOP(1) statename FROM viewgeo WHERE  cityid = ed.cityid) AS fromstate,'' AS tostate, Ed.travelmodeid, Ed.kmvisit, Ed.prekilometerrate, Ed.fromtime, Ed.totime, mtm.NAME AS Travelconvmode FROM   expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id  INNER JOIN mastlogin Ml ON ED.createdby = Ml.id LEFT JOIN mastsalesrep Msr ON Msr.userid = ml.id INNER JOIN mastarea MAC ON Ed.cityid = MAC.areaid LEFT JOIN masttravelmode mtm  ON ed.travelmodeid = mtm.id WHERE  ED.expensegroupid = " + ViewState["ExpGrpId"] + " GROUP  BY met.NAME,ED.billdate, Ed.expensedetailid,ED.billamount,Ed.approvedamount,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,ED.fromdate,ED.issupportingattached,Ed.issupportingapproved,ED.remarks,Ed.tocity,ED.todate,ED.travelmodeid,MAC.areaname,MET.expensetypecode,ED.issupportingattached,approverremarks,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME,msr.smid,ml.empname" +
            //      " UNION " + " SELECT Ed.expensedetailid,msr.smid,ml.empname,ED.billamount,Isnull(Ed.approvedamount, 0) AS ApprovedAmount,Isnull(ed.issupportingapproved, 0) AS issupportingapproved,CASE Isnull(Ed.approvedamount, 0) WHEN 0 THEN 0  ELSE Ed.approvedamount END AS SavedAmt, Ed.approverremarks,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/')    AS BillDate,ED.billnumber, ED.cityid,ED.claimamount,ED.fromcity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate, ED.issupportingattached,ED.remarks,Ed.tocity,Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/')   AS ToDate, MET.NAME,MAC.areaname AS FromcityName,Mac1.areaname AS TocItyName,MET.expensetypecode,CASE ED.issupportingattached WHEN 1 THEN 'Yes' ELSE 'No'  END AS IsSupportingAttached1,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,(SELECT TOP(1) statename FROM viewgeo WHERE  cityid = ed.fromcity) AS fromstate,(SELECT TOP(1) statename FROM viewgeo WHERE cityid = ed.tocity) AS tostate, Ed.travelmodeid,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime, Ed.totime,mtm.NAME AS Travelconvmode FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id INNER JOIN mastlogin Ml ON ED.createdby = Ml.id LEFT JOIN mastsalesrep Msr ON Msr.userid = ml.id INNER JOIN mastarea MAC ON Ed.fromcity = MAC.areaid LEFT JOIN mastarea Mac1 ON Ed.tocity = Mac1.areaid LEFT JOIN masttravelmode mtm ON ed.travelmodeid = mtm.id WHERE  ED.expensegroupid = " + ViewState["ExpGrpId"] + " GROUP BY met.NAME, ED.billdate,Ed.expensedetailid,ED.billamount,Ed.approvedamount,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,ED.fromdate,ED.issupportingattached,Ed.issupportingapproved,ED.remarks, Ed.tocity,ED.todate,ED.travelmodeid,MAC.areaname, Mac1.areaname,MET.expensetypecode,ED.issupportingattached,approverremarks,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME,msr.smid,ml.empname ) As Te Order By cast( billdate as date)";

            string strQqy = "Select * from ( SELECT Ed.expensedetailid,msr.smid,ml.empname,ED.billamount,t5.DesName,eg.ExpenseGroupId,EG.GroupName,ED.NighthaltAmt,ED.DA,ED.BillAmount as BAmt,[dbo].[GetTravelModeName](ED.travelmodeid ) TrName,Isnull(Convert(varchar(15),CONVERT(date,EG.DateOfSubmission,103),106),'') AS SubmittedDate,Isnull(Ed.approvedamount, 0) AS ApprovedAmount,Isnull(ed.issupportingapproved, 0) AS issupportingapproved,CASE Isnull(Ed.approvedamount, 0) WHEN 0 THEN 0 ELSE Ed.approvedamount END AS SavedAmt, Ed.approverremarks,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/')    AS BillDate,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate, ED.issupportingattached,ED.remarks,Ed.tocity, Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/')   AS ToDate, MET.NAME,MAC.areaname AS FromCityName,'' AS TocityName,MET.expensetypecode,CASE ED.issupportingattached WHEN 1 THEN 'Yes' ELSE 'No'  END AS IsSupportingAttached1,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,dbo.GetStateCity(ed.cityid) AS fromstate,'' AS tostate, Ed.travelmodeid, Ed.kmvisit,IsNull(Ed.extraKm,'0') as exkms,CASE WHEN Ed.returnJourneyFlag=1 THEN 'Yes' ELSE 'No' END AS RetJnr, Ed.prekilometerrate, Ed.fromtime, Ed.totime,[dbo].[GetTravelModeName](ED.travelmodeid) AS Travelconvmode,ED.CreatedBy,Ed.gstno,Isnull(ED.IsGSTNNo,0) AS IsGSTNNo,Isnull(ED.vendor, '') as vendor,Isnull(ed.CGSTAmt,0) as CGSTAmt,Isnull(ed.SGSTAmt,0) as SGSTAmt ,Isnull(ed.taxcode,'') as taxcode,Isnull(ed.IGSTAMT,0) as IGSTAmt FROM expensedetails ED Left JOIN mastexpensetype MET ON ED.expensetypeid = MET.id  Left JOIN mastlogin Ml ON ED.createdby = Ml.id LEFT JOIN mastsalesrep Msr ON Msr.userid = ml.id Left JOIN mastarea MAC ON Ed.cityid = MAC.areaid " +
                //LEFT JOIN masttravelmode mtm  ON ed.travelmodeid = mtm.id 
                " LEFT JOIN ExpenseGroup EG  ON ed.ExpenseGroupID = EG.ExpenseGroupID LEFT JOIN MastDesignation AS T5 ON Ml.DesigId=T5.DesId WHERE met.expensetypecode Not in ('TRAVEL') and ED.expensegroupid = " + ViewState["ExpGrpId"] + " " +
            //GROUP  BY met.NAME,ED.billdate, Ed.expensedetailid,ED.billamount,Ed.approvedamount,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,ED.fromdate,ED.issupportingattached,Ed.issupportingapproved,ED.remarks,Ed.tocity,ED.todate,ED.travelmodeid,MAC.areaname,MET.expensetypecode,ED.issupportingattached,approverremarks,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME,msr.smid,ml.empname
                 " UNION " + " SELECT Ed.expensedetailid,msr.smid,ml.empname,ED.billamount,t5.DesName,eg.ExpenseGroupId,EG.GroupName,ED.NighthaltAmt,ED.DA,ED.BillAmount as BAmt,[dbo].[GetTravelModeName](ED.travelmodeid ) TrName,Isnull(Convert(varchar(15),CONVERT(date,EG.DateOfSubmission,103),106),'') AS SubmittedDate,Isnull(Ed.approvedamount, 0) AS ApprovedAmount,Isnull(ed.issupportingapproved, 0) AS issupportingapproved,CASE Isnull(Ed.approvedamount, 0) WHEN 0 THEN 0  ELSE Ed.approvedamount END AS SavedAmt, Ed.approverremarks,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/')    AS BillDate,ED.billnumber, ED.cityid,ED.claimamount,ED.fromcity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate, ED.issupportingattached,ED.remarks,Ed.tocity,Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/')   AS ToDate, MET.NAME,MAC.areaname AS FromcityName,Mac1.areaname AS TocItyName,MET.expensetypecode,CASE ED.issupportingattached WHEN 1 THEN 'Yes' ELSE 'No'  END AS IsSupportingAttached1,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,dbo.GetStateCity(ed.fromcity) AS fromstate,dbo.GetStateCity(ed.tocity)  AS tostate, Ed.travelmodeid,Ed.kmvisit,IsNull(Ed.extraKm,'0') as exkms,CASE WHEN Ed.returnJourneyFlag=1 THEN 'Yes' ELSE 'No' END AS RetJnr,Ed.prekilometerrate,Ed.fromtime, Ed.totime,[dbo].[GetTravelModeName](ED.travelmodeid) AS Travelconvmode,ED.CreatedBy,Ed.gstno,Isnull(ED.IsGSTNNo,0) AS IsGSTNNo,Isnull(ED.vendor, '') as vendor,Isnull(ed.CGSTAmt,0) as CGSTAmt,Isnull(ed.SGSTAmt,0) as SGSTAmt ,Isnull(ed.taxcode,'') as taxcode,Isnull(ed.IGSTAMT,0) as IGSTAmt FROM expensedetails ED Left JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Left JOIN mastlogin Ml ON ED.createdby = Ml.id LEFT JOIN mastsalesrep Msr ON Msr.userid = ml.id Left JOIN mastarea MAC ON Ed.fromcity = MAC.areaid LEFT JOIN mastarea Mac1 ON Ed.tocity = Mac1.areaid " +
                //LEFT JOIN masttravelmode mtm ON ed.travelmodeid = mtm.id 
                " LEFT JOIN ExpenseGroup EG  ON ed.ExpenseGroupID = EG.ExpenseGroupID LEFT JOIN MastDesignation AS T5 ON Ml.DesigId=T5.DesId WHERE met.expensetypecode in ('TRAVEL') and  ED.expensegroupid = " + ViewState["ExpGrpId"] + " " +
            //GROUP BY met.NAME, ED.billdate,Ed.expensedetailid,ED.billamount,Ed.approvedamount,ED.billnumber,ED.cityid,ED.claimamount,ED.fromcity,ED.fromdate,ED.issupportingattached,Ed.issupportingapproved,ED.remarks, Ed.tocity,ED.todate,ED.travelmodeid,MAC.areaname, Mac1.areaname,MET.expensetypecode,ED.issupportingattached,approverremarks,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME,msr.smid,ml.empname
             " ) As Te Order By cast( billdate as date)";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQqy);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblSalesPersonName.Text = " " + Convert.ToString(dt.Rows[0]["EmpName"]);
                lblExpenseGroupName.Text = " " + Convert.ToString(dt.Rows[0]["GroupName"]);
                lblDesignation.Text = " " + Convert.ToString(dt.Rows[0]["DesName"]);
                if ((dt.Rows[0]["SubmittedDate"]).ToString() != "")
                {
                    lblSubmitted.Text = " " + DateTime.Parse(Convert.ToDateTime(dt.Rows[0]["SubmittedDate"]).ToShortDateString()).ToString("dd/MMM/yyyy"); //Convert.ToString(dt.Rows[0]["SubmittedDate"]);
                }
                else
                {
                    lblSubmitted.Text = " " + string.Empty;
                }
                hdncrtdby.Value = Convert.ToString(dt.Rows[0]["CreatedBy"]);

                //str = @"SELECT Id, Itemid, ImgUrl, ThumbnailImgUrl FROM ItemMastImage";
                //DataTable Itemimagesdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //string imagehtml = "";
                //Itemdt.Columns.Add("Imagehtml");
                //for (int i = 0; i < Itemdt.Rows.Count; i++)
                //{
                //    imagehtml = "";
                //    DataRow[] Drs = Itemimagesdt.Select("Itemid=" + Itemdt.Rows[i]["Itemid"].ToString() + "");
                //    DataTable itemimagebyiddt = new DataTable();
                //    if (Drs.Count() > 0)
                //    {
                //        itemimagebyiddt = Itemimagesdt.Select("Itemid=" + Itemdt.Rows[i]["Itemid"].ToString() + "").CopyToDataTable();
                //    }
                //    //     DataTable 
                //    //DataView dv = new DataView(Itemimagesdt);
                //    //dv.RowFilter = "Itemid In (" + Itemdt.Rows[i]["Itemid"].ToString() + ")" ;
                //    for (int j = 0; j < itemimagebyiddt.Rows.Count; j++)
                //    {
                //        imagehtml = imagehtml + "<img   src='" + itemimagebyiddt.Rows[j]["ImgUrl"].ToString().Replace(@"~", string.Empty) + "' style='cursor: pointer;' width='30' height='30' onclick='Showpic(this.src)'/>";
                //    }
                //    Itemdt.Rows[i]["Imagehtml"] = imagehtml;
                //}
            }
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        #endregion

        private void SetData()
        {
            string spdata = "select SMID,UserID from ExpenseGroup where ExpenseGroupId=" + Request.QueryString["ExpenseGroupId"] + "";
            DataTable dtsp = new DataTable();
            dtsp = DAL.DbConnectionDAL.getFromDataTable(spdata);
            if (dtsp.Rows.Count > 0)
            {
                ViewState["SpSmid"] = Convert.ToInt32(dtsp.Rows[0]["SMID"]);
                ViewState["SpUserId"] = Convert.ToInt32(dtsp.Rows[0]["UserID"]);
            }
        }
        private string GetIp()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            return myIP;
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            try
            {//For Approval
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //    DataTable dtexp = new DataTable();
                    //    dtexp.Columns.Add("TExpenseDetailId");
                    //    dtexp.Columns.Add("IsApprRej");
                    //    dtexp.Columns.Add("PassRejRemarks");
                    //    dtexp.Columns.Add("ApprovedAmt");
                    //    dtexp.Columns.Add("IsSupportingApproved");
                    string crtby = Convert.ToString(hdncrtdby.Value);
                    string smid = Settings.Instance.SMID;
                    foreach (RepeaterItem ri in rpt.Items)
                    {
                        if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                        {

                            decimal ApprAmt = 0;
                            int chkval = 0;
                            decimal cgstamt = 0;
                            decimal igstamt = 0;
                            decimal sgstamt = 0;

                            //TextBox ApproveRemarks = (TextBox)ri.FindControl("txtremarks");
                            TextBox ApprAmount = (TextBox)ri.FindControl("txtPassAmt");
                            if (!string.IsNullOrEmpty(ApprAmount.Text))
                                ApprAmt = Convert.ToDecimal(ApprAmount.Text);
                            CheckBox ChkIsPendingSA = (CheckBox)ri.FindControl("chkSA");
                            CheckBox ChkIsApprovedSA = (CheckBox)ri.FindControl("chkPSA");
                            HiddenField HidExpDetId = (HiddenField)ri.FindControl("hdfExpDetailId");
                            //DropDownList ddlTaxCodeApproved = (ri.FindControl("ddlTaxCode") as DropDownList);
                            //string txtcodeApproved = ddlTaxCodeApproved.SelectedValue.ToString();
                            //CheckBox ChkIsGSTINReg = (CheckBox)ri.FindControl("chkGstIN");
                            //TextBox GSTINNo = (TextBox)ri.FindControl("txtGstinNo");
                            //TextBox Vendor = (TextBox)ri.FindControl("txtVendor");
                            //TextBox CGSTAmount = (TextBox)ri.FindControl("txtCGSTAmt");
                            //TextBox SGSTAmount = (TextBox)ri.FindControl("txtSGSTAmt");
                            //TextBox IGSTAmount = (TextBox)ri.FindControl("txtIGSTAmt");
                            //if (ChkIsGSTINReg.Checked == true)
                            //{
                            //    if (GSTINNo.Text.Length < 15)
                            //    {
                            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter 15 digit GSTIN No.');", true);
                            //    }
                            //}
                            //if (!string.IsNullOrEmpty(CGSTAmount.Text))
                            //    cgstamt = Convert.ToDecimal(CGSTAmount.Text);
                            //if (!string.IsNullOrEmpty(SGSTAmount.Text))
                            //    sgstamt = Convert.ToDecimal(SGSTAmount.Text);
                            //if (!string.IsNullOrEmpty(IGSTAmount.Text))
                            //    igstamt = Convert.ToDecimal(IGSTAmount.Text);

                            //if (ChkIsApprovedSA.Checked)
                            //    chkval = 1;
                            //else
                            //    chkval = 0;


                            if (string.IsNullOrEmpty(ViewState["ExpGrpId"].ToString())) { ViewState["ExpGrpId"] = 0; }
                            EB.UpdateExpenseStatus(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(HidExpDetId.Value), Convert.ToInt32(smid), 1, Remarks.Value, ApprAmt, chkval, 1, "", false, "", "", 0, 0, 0);
                        }
                    }
                    EB.InsertExpenseLog(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(ViewState["SpUserId"]), Convert.ToInt32(ViewState["SpSmid"]), "Appr", Dns.GetHostName(), GetIp(), Convert.ToInt32(Settings.Instance.UserID));
                    Response.Redirect("~/ExpenseApproval.aspx");
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void btnUnsubmit_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string strUnsubmit = "update ExpenseGroup set IsSubmitted=0,DateOfSubmission=NULL where ExpenseGroupId=" + ViewState["ExpGrpId"] + "";
                DbConnectionDAL.ExecuteQuery(strUnsubmit);
                EB.InsertExpenseLog(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(ViewState["SpUserId"]), Convert.ToInt32(ViewState["SpSmid"]), "Unsub", Dns.GetHostName(), GetIp(), Convert.ToInt32(Settings.Instance.UserID));
                Response.Redirect("~/ExpenseApproval.aspx");
            }
        }

        protected void btnSaveExpSheet_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //DataTable dtexp = new DataTable();
                    //dtexp.Columns.Add("TExpenseDetailId");
                    //dtexp.Columns.Add("IsApprRej");
                    //dtexp.Columns.Add("PassRejRemarks");
                    //dtexp.Columns.Add("ApprovedAmt");
                    //dtexp.Columns.Add("IsSupportingApproved");
                    foreach (RepeaterItem ri in rpt.Items)
                    {
                        if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                        {

                            decimal ApprAmt = 0;
                            //decimal cgstamt = 0;
                            //decimal igstamt = 0;
                            //decimal sgstamt = 0;
                            int chkval = 0;
                            //TextBox ApproveRemarks = (TextBox)ri.FindControl("txtremarks");
                            TextBox ApprAmount = (TextBox)ri.FindControl("txtPassAmt");
                            if (!string.IsNullOrEmpty(ApprAmount.Text))
                                ApprAmt = Convert.ToDecimal(ApprAmount.Text);
                            CheckBox ChkIsPendingSA = (CheckBox)ri.FindControl("chkSA");
                            CheckBox ChkIsApprovedSA = (CheckBox)ri.FindControl("chkPSA");
                            HiddenField HidExpDetId = (HiddenField)ri.FindControl("hdfExpDetailId");
                            string smid = Settings.Instance.SMID;
                            //DropDownList ddlTaxCodeApproved = (ri.FindControl("ddlTaxCode") as DropDownList);
                            //string txtcodeApproved = ddlTaxCodeApproved.SelectedValue.ToString();
                            //if (ChkIsApprovedSA.Checked)
                            //    chkval = 1;
                            //else
                            //    chkval = 0;
                            //CheckBox ChkIsGSTINReg = (CheckBox)ri.FindControl("chkGstIN");
                            //TextBox GSTINNo = (TextBox)ri.FindControl("txtGstinNo");
                            //TextBox Vendor = (TextBox)ri.FindControl("txtVendor");
                            //TextBox CGSTAmount = (TextBox)ri.FindControl("txtCGSTAmt");
                            //TextBox SGSTAmount = (TextBox)ri.FindControl("txtSGSTAmt");
                            //TextBox IGSTAmount = (TextBox)ri.FindControl("txtIGSTAmt");
                            //if (!string.IsNullOrEmpty(CGSTAmount.Text))
                            //    cgstamt = Convert.ToDecimal(CGSTAmount.Text);
                            //if (!string.IsNullOrEmpty(SGSTAmount.Text))
                            //    sgstamt = Convert.ToDecimal(SGSTAmount.Text);
                            //if (!string.IsNullOrEmpty(IGSTAmount.Text))
                            //    igstamt = Convert.ToDecimal(IGSTAmount.Text);
                            //if (ChkIsGSTINReg.Checked == true)
                            //{
                            //    if (GSTINNo.Text.Length < 15)
                            //    {
                            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter 15 digit GSTIN No.');", true);
                            //        return;
                            //    }
                            //}

                            if (string.IsNullOrEmpty(ViewState["ExpGrpId"].ToString())) { ViewState["ExpGrpId"] = 0; }
                            // EB.UpdateExpenseStatus(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(HidExpDetId.Value), 2, ApproveRemarks.Text, ApprAmt, chkval, 2,txtcodeApproved);
                            EB.UpdateExpenseStatus(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(HidExpDetId.Value), Convert.ToInt32(smid), 2, Remarks.Value, ApprAmt, chkval, 2, "", false, "", "", 0, 0, 0);
                        }
                    }
                    EB.InsertExpenseLog(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(ViewState["SpUserId"]), Convert.ToInt32(ViewState["SpSmid"]), "Ses", Dns.GetHostName(), GetIp(), Convert.ToInt32(Settings.Instance.UserID));
                    Response.Redirect("~/ExpenseApproval.aspx");


                }

            }
            catch (Exception ex) { ex.ToString(); }
        }
        public class PartyDetails
        {
            public string partyname { get; set; }
            public string productgroup { get; set; }
            public string remarks { get; set; }
        }

        [WebMethod]
        public static PartyDetails[] BindDatatable(Int32 ExpenseDetailId)
        {
            DataTable dt = new DataTable();
            List<PartyDetails> details = new List<PartyDetails>();

            string strQ = "select mp.partyname,ep.productgroup,ep.remarks from expenseparty ep inner join mastparty mp on ep.partyid=mp.partyid  where ep.ExpenseDetailId=" + ExpenseDetailId + "";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            foreach (DataRow dtrow in dt.Rows)
            {
                PartyDetails party = new PartyDetails();
                party.partyname = dtrow["partyname"].ToString();
                party.productgroup = dtrow["productgroup"].ToString();
                party.remarks = dtrow["remarks"].ToString();
                details.Add(party);

            }
            return details.ToArray();
        }

        public class AttendenceDetails
        {
            public string DSRStatus { get; set; }
        }

        [System.Web.Services.WebMethod]
        public static AttendenceDetails[] BindAttendenceStatus(Int32 Smid, string Billdate)
        {
            List<AttendenceDetails> dsrdetails = new List<AttendenceDetails>();
            //int dyasInMonth = 30;
            DateTime mDate1 = DateTime.Now, mDate2 = DateTime.Now;
            mDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            mDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(30);
            DateTime currentdate = Convert.ToDateTime(Billdate);
            //int dyasInMonth = DateTime.DaysInMonth(year, Month);
            DataTable gvdt = new DataTable();
            gvdt.Columns.Add(new DataColumn("DSRStatus", typeof(String)));

            string mSRepCode = "", transTemp = "", strholiday = String.Empty;
            strholiday = "select distinct smid,DAY(holiday_date) as day1,holiday_date,Reason,areaID,AreaType from View_Holiday where smid = " + Smid + " and holiday_date = '" + Billdate + "'";
            DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, strholiday);
            DataView dvdt_holiday = new DataView(dt_holiday);

            string Leave_str = "", strleave = "", status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
            int dayloop = 0;

            mSRepCode = @"select SMId,SMName,SyncId from MastSalesRep where SMId in (" + Smid + ") and Active=1 order by SMId";
            DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);

            dsrQuery = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where smid = " + Smid + " and vdate = '" + Billdate + "' order by vl.SMId,vdate";
            DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
            DataView dvdsrData = new DataView(dsrData);

            string No_Days = "select NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + Smid + ") and (lr.FromDate = '" + Billdate + "' OR lr.ToDate = '" + Billdate + "') and lr.appstatus in ('Approve','Pending') order by smid,FromDate";
            DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
            DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

            foreach (DataRow dr in ssdt.Rows)
            {
                DataRow mDataRow = gvdt.NewRow();

                dvdsrData.RowFilter = "smid=" + dr["SMId"];
                foreach (DataRow dsr1 in dvdsrData.ToTable().Rows)
                {
                    DsrDate = Convert.ToDateTime(dsr1["vdate"].ToString()).Day.ToString().Trim();
                    if (dsr1["SMId"].ToString() == dr["SMId"].ToString())
                    {
                        if (dsr1["Status"].ToString() == "Approve") { mDataRow["DSRStatus"] = "P - Present"; }
                        if (dsr1["Status"].ToString() == "Reject") { mDataRow["DSRStatus"] = "E/R - DSR Rejected"; }
                        if (dsr1["Status"].ToString() == "") { mDataRow["DSRStatus"] = "E - DSR Entry"; }
                    }
                }

                dvdt_holiday.RowFilter = "smid=" + dr["SMId"];
                foreach (DataRow dsr1 in dvdt_holiday.ToTable().Rows)
                {
                    DsrDate = Convert.ToDateTime(dsr1["holiday_date"].ToString()).Day.ToString().Trim();
                    mDataRow["DSRStatus"] = "H - Holiday"; // Filling Holiday
                }

                //for (int k = 1; k <= dyasInMonth; k++)
                //{
                //    if (DateTime.Parse(k + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString().Trim() == "Sunday")
                //    {
                //        mDataRow["d" + k.ToString()] = "Off"; //Filling Sunday
                //    }
                //}

                if (currentdate.DayOfWeek.ToString().Trim() == "Sunday")
                {
                    mDataRow["DSRStatus"] = "Off"; //Filling Sunday
                }

                dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"];
                if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                {
                    for (int cc = 0; cc < dvdt_NoOfdays.ToTable().Rows.Count; cc++)
                    {
                        if (dvdt_NoOfdays.ToTable().Rows[cc]["SMId"].ToString() == dr["SMId"].ToString())
                        {
                            double daysNo = Convert.ToDouble(dvdt_NoOfdays.ToTable().Rows[cc]["NoOfDays"].ToString());
                            DateTime fromdate = new DateTime();
                            DateTime todate = new DateTime();
                            //todate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["NoDays"].ToString());
                            //fromdate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());

                            //dayloop = Convert.ToInt32((todate - fromdate).TotalDays);
                            //for (int c1 = 0; c1 <= dayloop; c1++)
                            //{
                            DateTime dateTime1 = new DateTime();
                            dateTime1 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                            //dateTime1 = dateTime1.AddDays(c1);

                            //if (dateTime1 > mDate2)
                            //{
                            //}
                            //else if (dateTime1 < mDate1)
                            //{
                            //}
                            //else
                            //{
                            DateTime dateTime2 = new DateTime();
                            DateTime dateTime3 = new DateTime();
                            dateTime2 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                            dateTime3 = Convert.ToDateTime(dateTime1.ToString());
                            double NrOfDays = ((dateTime3 - dateTime2).TotalDays) * 2;
                            strleave = dvdt_NoOfdays.ToTable().Rows[cc]["LeaveString"].ToString();
                            status_pr = dvdt_NoOfdays.ToTable().Rows[cc]["AppStatus"].ToString();
                            string str = strleave.Substring((Convert.ToInt32(NrOfDays)), 2);

                            dvdsrData.RowFilter = "smid=" + dr["SMId"];
                            foreach (DataRow dsr in dvdsrData.ToTable().Rows)
                            {
                                if (dsr["SMId"].ToString() == dr["SMId"].ToString())
                                {
                                    if (dateTime3.Day.ToString().Trim() == Convert.ToDateTime(dsr["vdate"].ToString()).Day.ToString().Trim())
                                    {
                                        if (dsr["Status"].ToString() == "Approve")
                                        { ls_val += "P - Present"; }
                                        if (dsr["Status"].ToString() == "Reject")
                                        { ls_val += "E/R - DSR Rejected"; }
                                        if (dsr["Status"].ToString() == "")
                                        { ls_val += "E - DSR Entry"; }
                                    }
                                }
                            }
                            dvdsrData.RowFilter = null;

                            foreach (DataRow dsr1 in dvdt_holiday.ToTable().Rows)
                            {
                                if (ls_val != "") { ls_val += ", "; }
                                DsrDate = Convert.ToDateTime(dsr1["holiday_date"].ToString()).Day.ToString().Trim();
                                ls_val += "H - Holiday"; // Filling Holiday
                            }

                            if (currentdate.DayOfWeek.ToString().Trim() == "Sunday")
                            {
                                if (ls_val != "") { ls_val += ", "; }
                                ls_val += "Off"; //Filling Sunday
                            }


                            if (ls_val != "") { ls_val += ", "; }
                            if (status_pr == "Pending")
                            {
                                if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL - Second Half Leave"; }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                {
                                    transTemp = ls_val.Replace(", ", "");
                                    if (transTemp != "")
                                    { ls_val = "FHL - First Half Leave" + ", " + transTemp; }
                                    else
                                    { ls_val += "FHL - First Half Leave"; }
                                }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L"; }
                            }
                            if (status_pr == "Approve")
                            {
                                if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/A - Second Half Leave Approved"; }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                {
                                    transTemp = ls_val.Replace(", ", "");
                                    if (transTemp != "")
                                    { ls_val = "FHL/A - First Half Leave Approved" + ", " + transTemp; }
                                    else
                                    { ls_val += "FHL/A - First Half Leave Approved"; }
                                }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/A - Leave Approved"; }
                            }
                            if (status_pr == "Reject")
                            {
                                if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/R - Second Half Leave Rejected"; }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                {
                                    transTemp = ls_val.Replace(", ", "");
                                    if (transTemp != "")
                                    { ls_val = "FHL/R - First Half Leave Rejected" + ", " + transTemp; }
                                    else
                                    { ls_val += "FHL/R - First Half Leave Rejected"; }
                                }
                                if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/R - Leave Rejected"; }
                            }
                            //mDataRow["DSRStatus" + dateTime3.Day.ToString().Trim()] = ls_val;
                            mDataRow["DSRStatus"] = ls_val;
                            ls_val = "";
                            //}
                            //}

                        }
                    }

                }
                dvdt_holiday.RowFilter = null;
                dvdsrData.RowFilter = null;
                dvdt_NoOfdays.RowFilter = null;
                gvdt.Rows.Add(mDataRow);
                gvdt.AcceptChanges();
                foreach (DataRow dtrow in gvdt.Rows)
                {
                    AttendenceDetails Dsr = new AttendenceDetails();
                    Dsr.DSRStatus = dtrow["DSRStatus"].ToString();
                    dsrdetails.Add(Dsr);

                }
            }
            return dsrdetails.ToArray();
        }

        [WebMethod]
        public static PartyDetails[] BindDataGrid(Int32 ExpenseDetailId)
        {
            DataTable dt = new DataTable();
            List<PartyDetails> details = new List<PartyDetails>();

            string strQ = "select mp.partyname,ep.productgroup,ep.remarks from expenseparty ep inner join mastparty mp on ep.partyid=mp.partyid  where ep.ExpenseDetailId=" + ExpenseDetailId + "";
            dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            foreach (DataRow dtrow in dt.Rows)
            {
                PartyDetails party = new PartyDetails();
                party.partyname = dtrow["partyname"].ToString();
                party.productgroup = dtrow["productgroup"].ToString();
                party.remarks = dtrow["remarks"].ToString();
                details.Add(party);

            }
            return details.ToArray();
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb = (Label)e.Item.FindControl("myid");
                Label lb1 = (Label)e.Item.FindControl("myidto");
                Label lb2 = (Label)e.Item.FindControl("Label1");
                TextBox txtappramt = (TextBox)e.Item.FindControl("txtPassAmt");
                // Label txtclaimamt = (Label)e.Item.FindControl("lblclaimAmt");
                TextBox txtapprremrks = (TextBox)e.Item.FindControl("txtremarks");
                CheckBox chkSR = (CheckBox)e.Item.FindControl("chkPSA");
                HiddenField hdCnt = (HiddenField)e.Item.FindControl("hfCnt");
                LinkButton btn = e.Item.FindControl("LinkButton1") as LinkButton;
                if (hdCnt.Value != "False")
                {
                    btn.Visible = true;
                }
                else btn.Visible = false;
                //CheckBox chkgst = (CheckBox)e.Item.FindControl("chkGstIN");
                //DropDownList ddlTaxCode = (e.Item.FindControl("ddlTaxCode") as DropDownList);
                //HiddenField HidTaxCode = (HiddenField)e.Item.FindControl("hdftaxcode");
                //string strcode = "Select Id,name,IsGstin from Masttextcode";
                //DataTable dttaxcode = new DataTable();
                //dttaxcode = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcode);
                //if (dttaxcode.Rows.Count > 0)
                //{
                //    ddlTaxCode.DataSource = dttaxcode;
                //    ddlTaxCode.DataTextField = "Name";
                //    ddlTaxCode.DataValueField = "Id";
                //    ddlTaxCode.DataBind();
                //    ddlTaxCode.Items.Insert(0, new ListItem("--Select--", "0"));
                //}

                //if (chkgst.Checked == false)
                //{
                //    string str = "Select Id from Masttextcode where isgstin=0";
                //    string strvalue = DAL.DbConnectionDAL.GetStringScalarVal(str);
                //    ddlTaxCode.SelectedValue = strvalue;
                //}
                //string strcheck = "SELECT ed.TaxCode FROM ExpenseDetails ed LEFT JOIN masttextcode mt ON mt.Id=ed.TaxCode WHERE ed.ExpenseGroupID='" + Request.QueryString["ExpenseGroupId"] + "' and ed.taxcode=" + HidTaxCode.Value + "";
                //string strvaluecheck = DAL.DbConnectionDAL.GetStringScalarVal(strcheck);
                //ddlTaxCode.SelectedValue = strvaluecheck;

                //string chk = "0";
                //if (chkgst.Checked == true)
                //    chk = "1";
                //{

                //    string strcode = "Select Id,name,IsGstin from Masttextcode where isgstin='" + chk + "'";
                //    DataTable dttaxcode = new DataTable();
                //    dttaxcode = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcode);
                //    if (dttaxcode.Rows.Count > 0)
                //    {                       
                //        ddlTaxCode.DataSource = dttaxcode;
                //        ddlTaxCode.DataTextField = "Name";
                //        ddlTaxCode.DataValueField = "Id";
                //        ddlTaxCode.DataBind();                       
                //    }
                //    ddlTaxCode.Items.Insert(0, new ListItem("--Select--", "0"));
                //}

                if (!Convert.ToBoolean(ViewState["ExpStatus"]))
                {
                    txtappramt.Enabled = false; chkSR.Enabled = false;
                    // ddlTaxCode.Enabled = false;
                }
                else
                {
                    txtappramt.Enabled = true; chkSR.Enabled = true;
                    // ddlTaxCode.Enabled = true;
                }
                if ((DataBinder.Eval(e.Item.DataItem, "ExpenseTypeCode")).ToString().ToUpper() == "Travel".ToUpper())
                {
                    lb.ForeColor = System.Drawing.Color.Blue;
                    lb1.ForeColor = System.Drawing.Color.Blue;
                    lb1.Visible = true;
                    lb2.Visible = true;
                    lb.ToolTip = "From " + (DataBinder.Eval(e.Item.DataItem, "FromCityName")).ToString() + " To " + (DataBinder.Eval(e.Item.DataItem, "TocityName")).ToString() + "";
                    lb1.ToolTip = "From " + (DataBinder.Eval(e.Item.DataItem, "FromCityName")).ToString() + " To " + (DataBinder.Eval(e.Item.DataItem, "TocityName")).ToString() + "";
                }
                else
                {
                    lb1.Visible = false;
                    lb2.Visible = false;
                }

            }
            if (e.Item.ItemType == ListItemType.Header)
            {
                Button btnset1 = (Button)e.Item.FindControl("btnset");
                Button btnunset1 = (Button)e.Item.FindControl("btnunset");
                Button btnset2 = (Button)e.Item.FindControl("btnSetTxtcode");
                Button btnunset2 = (Button)e.Item.FindControl("btnUnsetTxtcode");
                //   DropDownList ddlTaxCodesetunset = (e.Item.FindControl("ddlsettaxcode") as DropDownList);     
                //if (Request.QueryString["Status"].ToUpper() == "Approved".ToUpper())
                //{
                //    btnset1.Visible = true; btnunset1.Visible = true;
                //    btnset2.Visible = true; btnunset2.Visible = true;
                //}
                //else
                //{
                //    btnset1.Visible = true; btnunset1.Visible = true;
                //    btnset2.Visible = true; btnunset2.Visible = true;
                //}
                ////Added
                //if (Request.QueryString["Status"].ToUpper() == "View".ToUpper())
                //{
                //    btnset1.Enabled = false; btnunset1.Enabled = false;
                //    btnset2.Enabled = false; btnunset2.Enabled = false;
                //}
                //else
                //{
                //    btnset1.Enabled = true; btnunset1.Enabled = true;
                //    btnset2.Visible = true; btnunset2.Visible = true;
                //}
                //string strcodesetunset = "Select Id,name,IsGstin from Masttextcode where isgstin=1";
                //DataTable dttaxcodesetunset = new DataTable();
                //dttaxcodesetunset = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcodesetunset);
                //if (dttaxcodesetunset.Rows.Count > 0)
                //{
                //    ddlTaxCodesetunset.DataSource = dttaxcodesetunset;
                //    ddlTaxCodesetunset.DataTextField = "Name";
                //    ddlTaxCodesetunset.DataValueField = "Id";
                //    ddlTaxCodesetunset.DataBind();
                //}
                //ddlTaxCodesetunset.Items.Insert(0, new ListItem("--Select--", "0"));
                //End
            }
        }

        protected void btnUnapprove_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string strUnsubmit = "update ExpenseGroup set [TotalApprovedAmount]=0.0,IsSubmitted=1,IsApproved=0 where ExpenseGroupId=" + ViewState["ExpGrpId"] + "";
                DbConnectionDAL.ExecuteQuery(strUnsubmit);
                string strUpdApprAmt = "update [ExpenseDetails] set [ApprovedAmount]=0.0 where ExpenseGroupId=" + ViewState["ExpGrpId"] + "";
                DbConnectionDAL.ExecuteQuery(strUpdApprAmt);
                EB.InsertExpenseLog(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(ViewState["SpUserId"]), Convert.ToInt32(ViewState["SpSmid"]), "UnAppr", Dns.GetHostName(), GetIp(), Convert.ToInt32(Settings.Instance.UserID));
                //string strUpdApprAmtGrp = "update [ExpenseGroup] set [TotalApprovedAmount]=0.0 where ExpenseGroupId=" + Session["ExpGrpId"] + "";
                //DbConnectionDAL.ExecuteQuery(strUpdApprAmtGrp);
                Response.Redirect("~/ExpenseApproval.aspx");
            }
        }

        protected void btnset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rpt.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox PassAmt = (TextBox)item.FindControl("txtPassAmt");
                        Label ClaimAmt = (Label)item.FindControl("lblclaimAmt");
                        PassAmt.Text = ClaimAmt.Text;
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void btnunset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rpt.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox PassAmt = (TextBox)item.FindControl("txtPassAmt");
                        PassAmt.Text = "0.0";
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void btnUnsetTxtcode_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    foreach (RepeaterItem item in rpt.Items)
            //    {
            //        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //        {
            //            CheckBox chk = (CheckBox)item.FindControl("chkGstIN");
            //            //   TextBox txtCode = (TextBox)item.FindControl("txtTextCode");
            //            DropDownList ddlTaxCode = (DropDownList)item.FindControl("ddlTaxCode");
            //            string strcode = "Select Id,name,IsGstin from Masttextcode";
            //            DataTable dttaxcode = new DataTable();
            //            dttaxcode = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcode);

            //            if (chk.Checked == true)
            //            {
            //                if (dttaxcode.Rows.Count > 0)
            //                {
            //                    ddlTaxCode.DataSource = dttaxcode;
            //                    ddlTaxCode.DataTextField = "Name";
            //                    ddlTaxCode.DataValueField = "Id";
            //                    ddlTaxCode.DataBind();
            //                    ddlTaxCode.Items.Insert(0, new ListItem("--Select--", "0"));
            //                }
            //            }
            //            else
            //            {

            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{ ex.ToString(); }

        }

        protected void btnSetTxtcode_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    foreach (RepeaterItem item in rpt.Items)
            //    {
            //        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //        {
            //            CheckBox chk = (CheckBox)item.FindControl("chkGstIN");
            //            // TextBox txtCode = (TextBox)item.FindControl("txtTextCode");
            //            DropDownList ddlTaxCode = (DropDownList)item.FindControl("ddlTaxCode");
            //            string strcode = "Select Id,name,IsGstin from Masttextcode";
            //            DataTable dttaxcode = new DataTable();
            //            dttaxcode = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcode);

            //            if (chk.Checked == true)
            //            {
            //                if (dttaxcode.Rows.Count > 0)
            //                {
            //                    ddlTaxCode.DataSource = dttaxcode;
            //                    ddlTaxCode.DataTextField = "Name";
            //                    ddlTaxCode.DataValueField = "Id";
            //                    ddlTaxCode.DataBind();
            //                    // ddlTaxCode.Items.Insert(0, new ListItem("--Select--", "0"));
            //                }
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { ex.ToString(); }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "viewimg")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string ExpenseGroupID = commandArgs[0];
                string ExpenseDetailId = commandArgs[1];
                // string ExpenseGroupID = e.CommandArgument.ToString();
                GetFile(ExpenseGroupID, ExpenseDetailId);
                //GetAtt(ExpenseGroupID); 
                //  rty();
            }
        }

        public void GetFile(string ExpenseGroupID, string ExpenseDetailId)
        {
            string sql = "";
            string root = string.Empty;
            string[] files = null;
            List<string> list = new List<string>();
            List<ListItem> filesPath = new List<ListItem>();
            DataTable dt1 = new DataTable();
            DataColumn imagename = new DataColumn("imagename");
            try
            {
                sql = "Select ea.AttachmentUrl From ExpenseAttachmentDetails ea inner join ExpenseDetails ed on ed.ExpenseDetailID=ea.ExpenseDetailID Where ed.ExpenseGroupId=" + ExpenseGroupID + " and ed.ExpenseDetailId=" + ExpenseDetailId + "";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
                if (dt.Rows.Count > 0)
                {
                    dt1.Columns.Add(imagename);
                    foreach (DataRow dr in dt.Rows)
                    {
                        root = "http://" + host + "/" + dr["AttachmentUrl"].ToString().Replace(@"~/", string.Empty);
                        dt1.Rows.Add(root);

                    }
                    dt1.AcceptChanges();

                    Mylist.DataSource = dt1;
                    Mylist.DataBind();
                    mpePop.Show();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Group do not have any attachment');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            mpePop.Hide();
        }
    }
}