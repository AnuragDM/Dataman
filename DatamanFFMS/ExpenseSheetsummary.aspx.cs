using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace AstralFFMS
{
    public partial class ExpenseSheetSummary : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ExpenseGroupId"] != null)
                {
                    int ExpGrpId = Convert.ToInt32(Request.QueryString["ExpenseGroupId"]);
                    LoadData(ExpGrpId);
                    Session["ExpGrpId"] = ExpGrpId;

                    if (Request.QueryString["Flag"] != null)
                    {
                        if (Convert.ToString(Request.QueryString["Flag"]) == "SE")
                        {
                            btnExcelExport.Enabled = Settings.Instance.CheckExportPermission("ExpenseSelfSheet.aspx", Convert.ToString(Session["user_name"]));
                            btnExcelExport.CssClass = "btn btn-primary";
                        }
                    }
                    else
                    {
                        btnExcelExport.Enabled = Settings.Instance.CheckExportPermission("ExpenseApproval.aspx", Convert.ToString(Session["user_name"]));
                        btnExcelExport.CssClass = "btn btn-primary";
                    }
                }
            }
        }

        private void LoadData(int ExpGrpId)
        {
            //            string str = @"SELECT Distinct T2.SMName,T4.Name,isnull(T2.SyncId,'') AS Code,isnull(T1.VoucherNo,0) AS VoNo,T1.GroupName,T6.cityName AS City,T6.stateName AS State,
            //							Isnull(T7.Name,'') AS Grade,
            //                            T5.DesName AS Designation,Convert(varchar(15),CONVERT(date,T1.CreatedOn,103),106) AS CreatedDate,(select smname from mastsalesrep where smid =(select CreatedBy from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as CreatedBy,Isnull(Convert(varchar(15),CONVERT (date,T1.DateOfSubmission,103),106),'') AS SubmittedDate," +
            //                           " Isnull(T1.TotalAmount,0) AS Total,Isnull(T1.totalverifiedamt,0) AS VerifiedAmount,Isnull(T1.TotalApprovedAmount,0) AS ApprovedAmount,Isnull(T1.AdvanceAmount,0) AS AdvanceAmount,T1.VerifiedByDateTime FROM ExpenseGroup AS T1 Left JOIN MastSalesRep AS T2 ON T1.SMID=T2.SMId"+
            //                           " LEFT JOIN MastGrade AS T3  ON T2.GradeId=T3.Id  LEFT JOIN MastLogin AS T4  ON T2.UserId=T4.Id LEFT JOIN MastDesignation AS T5"+
            //                            " LEFT JOIN ViewGeo AS T6 ON T2.CityId=T6.cityid  LEFT JOIN MastGrade AS T7  ON T2.GradeId=T7.Id WHERE T1.ExpenseGroupID=" + ExpGrpId + "";

            string str = @"SELECT Distinct T2.SMName,T4.Name,isnull(T2.SyncId,'') AS Code,isnull(T1.VoucherNo,0) AS VoNo,T1.GroupName,T6.cityName AS City,T6.stateName AS State,Isnull(T7.Name,'') AS Grade,T5.DesName AS Designation,Convert(varchar(15),CONVERT(date,T1.CreatedOn,103),106) AS CreatedDate,IsNull(iscr.Smname,'') as CreatedBy,  Isnull(Convert(varchar(15),CONVERT(date,T1.DateOfSubmission,103),106),'') AS SubmittedDate, Isnull(T1.TotalAmount,0) AS Total,Isnull(T1.totalverifiedamt,0) AS VerifiedAmount,Isnull(T1.TotalApprovedAmount,0) AS ApprovedAmount,case when T1.IsApproved=1 then 'Yes' else 'No' end as Approved,Isnull(T1.AdvanceAmount,0) AS AdvanceAmount,IsNull(T1.VerifiedByDateTime,'') as VerifiedByDateTime,IsNull(isap.Smname,'') as ApprovedBy,IsNull(isvr.Smname,'') as VerifiedBy FROM ExpenseGroup AS T1 Left JOIN MastSalesRep AS T2  ON T1.SMID=T2.SMId  LEFT JOIN MastGrade AS T3   ON T2.GradeId=T3.Id  LEFT JOIN MastLogin AS T4  ON T2.UserId=T4.Id  LEFT JOIN MastDesignation AS T5  ON T4.DesigId=T5.DesId  LEFT JOIN ViewGeo AS T6  ON T2.CityId=T6.cityid   LEFT JOIN MastGrade AS T7  ON T2.GradeId=T7.Id Left JOIN MastSalesRep AS isap  ON T1.ApprovedBySenior=isap.SMId Left JOIN MastSalesRep AS iscr  ON T1.CreatedBy=iscr.UserId Left JOIN MastSalesRep AS isvr  ON T1.VerifiedBy=isvr.SMId  WHERE T1.ExpenseGroupID=" + ExpGrpId + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                lblSalesPersonName.Text = " " + Convert.ToString(dt.Rows[0]["SMName"]);
                lblExpenseGroupName.Text = " " + Convert.ToString(dt.Rows[0]["GroupName"]);
                //lblGrade.Text = " " + Convert.ToString(dt.Rows[0]["Grade"]);
                //lblCode.Text = " " + Convert.ToString(dt.Rows[0]["Code"]);
                lblVoucherNo.Text = " " + Convert.ToString(dt.Rows[0]["VoNo"]);
                //lblCity.Text = " " + Convert.ToString(dt.Rows[0]["City"]);
                lblCreatedBy.Text = " " + Convert.ToString(dt.Rows[0]["CreatedBy"]);
                lblDesignation.Text = " " + Convert.ToString(dt.Rows[0]["Designation"]);
                lblState.Text = " " + Convert.ToString(dt.Rows[0]["ApprovedBy"]);
                lblVerifiedBy.Text = " " + Convert.ToString(dt.Rows[0]["VerifiedBy"]);
                if(Convert.ToString(dt.Rows[0]["Approved"])=="Yes")
                {
                    ttlapp.Visible = true;
                    ttl.Visible = false;
                }
                else
                {
                    ttlapp.Visible = false;
                    ttl.Visible = true;
                }

                if ((dt.Rows[0]["CreatedDate"]).ToString() != "")
                {
                    lblCreated.Text = " " + DateTime.Parse(Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToShortDateString()).ToString("dd/MMM/yyyy");//Convert.ToString(dt.Rows[0]["CreatedDate"]);
                }
                else
                {
                    lblCreated.Text = " " + string.Empty;
                }
                if ((dt.Rows[0]["SubmittedDate"]).ToString() != "")
                {
                    lblSubmitted.Text = " " + DateTime.Parse(Convert.ToDateTime(dt.Rows[0]["SubmittedDate"]).ToShortDateString()).ToString("dd/MMM/yyyy"); //Convert.ToString(dt.Rows[0]["SubmittedDate"]);
                }
                else
                {
                    lblSubmitted.Text = " " + string.Empty;
                }
                lblTotal.Text = " " + Convert.ToString(dt.Rows[0]["Total"]);
                lblTotalApproved.Text = " " + Convert.ToString(dt.Rows[0]["ApprovedAmount"]);
                lblTotalVerified.Text = " " + Convert.ToString(dt.Rows[0]["VerifiedAmount"]);
                lblVDT.Text = " " + Convert.ToString(dt.Rows[0]["VerifiedByDateTime"]);
                //}

                string str1 = @"SELECT T1.ExpenseDetailID,T1.ExpenseGroupID,T2.Id AS ExpenseTypeID,T2.Name AS ExpenseTypeName,T1.BillNumber,                             Convert(varchar(15),CONVERT(date,T1.BillDate,103),106) AS BillDate,Convert(varchar(15),CONVERT(date,T1.CreatedOn,103),106) AS CreatedOn,Isnull(T1.BillAmount,0) AS BillAmount,Isnull(T1.ClaimAmount,0) AS ClaimAmount,CASE WHEN T1.IsSupportingAttached=1 THEN 'Yes' ELSE 'No' END AS Enclosed,CASE WHEN T1.IsSupportingApproved=1 THEN 'Received' ELSE 'Not Received' END AS SupportingStatus,Isnull(T1.ApprovedAmount,0) AS ApprovedAmount,Isnull(T1.Remarks,0) AS Remarks,CASE WHEN T1.IsSupportingApproved=1 THEN 'Yes' WHEN T1.IsSupportingApproved=0 THEN 'No' ELSE ''END AS IsSupportingApproved,GSTNO,vendor,isnull(cgstamt,0) AS cgstamt,isnull(sgstamt,0) AS sgstamt,isnull(igstamt,0) AS igstamt ,[dbo].[GetTravelModeName](T1.travelmodeid ) Name,fma1.AreaName as farea,Tma1.AreaName as Tarea,t1.da,t1.KMVisit as kms,IsNull(t1.extraKm,'0') as exkms,CASE WHEN T1.returnJourneyFlag=1 THEN 'Yes' ELSE 'No' END AS RetJnr,T1.Remarks as MainRemarks,(select smname from mastsalesrep where smid =(select approvedbysenior from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as senior,(select smname from mastsalesrep where smid =(select VerifiedBy from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as VerifiedBy,0 as AdvanceAmount,isnull(T1.VerifiedAmt,0) as VerifiedAmt,isnull(T1.NighthaltAmt,0) NighthaltAmt  FROM ExpenseDetails  AS T1 LEFT JOIN MastExpenseType AS T2    ON T1.ExpenseTypeID=T2.Id left join [MastTravelMode] as mtm on mtm.Id=t1.TravelModeID left join mastarea as fma1 on fma1.areaid=t1.FromCity	left join mastarea as Tma1 on Tma1.areaid=t1.ToCity	left join mastarea as fma on fma.areaid=t1.FromArea " + " WHERE T1.ExpenseGroupID=" + ExpGrpId + " order by T1.ExpenseDetailID";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                DataTable dtFinal = dt1.Clone();
                DataTable dtChild = new DataTable();
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    //gvDetails.DataSource = dt1;
                    //gvDetails.DataBind();

                    string[] TobeDistinct = { "ExpenseTypeID" };
                    DataTable dtDistinct = GetDistinctRecords(dt1, TobeDistinct);

                    if (dtDistinct != null && dtDistinct.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDistinct.Rows.Count; i++)
                        {
                            DataRow[] drow = dt1.Select("ExpenseTypeID=" + Convert.ToInt32(dtDistinct.Rows[i]["ExpenseTypeID"]) + "");

                            if (drow != null && drow.Length > 0)
                            {
                                dtChild = drow.CopyToDataTable();
                                if (dtChild != null && dtChild.Rows.Count > 0)
                                {
                                    dtFinal.Rows.Add();

                                    dtFinal.Rows[dtFinal.Rows.Count - 1]["ExpenseTypeName"] = dtChild.Rows[0]["ExpenseTypeName"];
                                    //dtFinal.Rows[dtFinal.Rows.Count - 1]["Name"] = dtChild.Rows[0]["Name"];
                                    //dtFinal.Rows[dtFinal.Rows.Count - 1]["Travelledcity"] = dtChild.Rows[0]["Travelledcity"];
                                    //dtFinal.Rows[dtFinal.Rows.Count - 1]["travelledarea"] = dtChild.Rows[0]["travelledarea"];
                                    decimal BillAmount = 0M;
                                    decimal ClaimAmount = 0M;
                                    decimal ApprovedAmount = 0M;
                                    decimal VerifiedAmount = 0M;
                                    for (int j = 0; j < dtChild.Rows.Count; j++)
                                    {
                                        BillAmount += Convert.ToDecimal(dtChild.Rows[j]["BillAmount"]);
                                        ClaimAmount += Convert.ToDecimal(dtChild.Rows[j]["ClaimAmount"]);
                                        ApprovedAmount += Convert.ToDecimal(dtChild.Rows[j]["ApprovedAmount"]);
                                        VerifiedAmount += Convert.ToDecimal(dtChild.Rows[j]["VerifiedAmt"]);
                                    }
                                    dtFinal.Rows[dtFinal.Rows.Count - 1]["BillAmount"] = BillAmount;
                                    dtFinal.Rows[dtFinal.Rows.Count - 1]["ClaimAmount"] = ClaimAmount;
                                    dtFinal.Rows[dtFinal.Rows.Count - 1]["ApprovedAmount"] = ApprovedAmount;
                                    dtFinal.Rows[dtFinal.Rows.Count - 1]["VerifiedAmt"] = VerifiedAmount;

                                    for (int j = 0; j < dtChild.Rows.Count; j++)
                                    {
                                        dtFinal.Rows.Add();
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["BillNumber"] = dtChild.Rows[j]["BillNumber"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["Name"] = dtChild.Rows[j]["Name"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["farea"] = dtChild.Rows[j]["farea"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["Tarea"] = dtChild.Rows[j]["Tarea"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["BillDate"] = dtChild.Rows[j]["BillDate"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["NighthaltAmt"] = dtChild.Rows[j]["NighthaltAmt"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["Da"] = dtChild.Rows[j]["Da"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["BillAmount"] = dtChild.Rows[j]["BillAmount"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["ClaimAmount"] = dtChild.Rows[j]["ClaimAmount"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["Enclosed"] = dtChild.Rows[j]["Enclosed"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["SupportingStatus"] = dtChild.Rows[j]["SupportingStatus"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["ApprovedAmount"] = dtChild.Rows[j]["ApprovedAmount"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["VerifiedAmt"] = dtChild.Rows[j]["VerifiedAmt"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["Remarks"] = dtChild.Rows[j]["Remarks"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["IsSupportingApproved"] = dtChild.Rows[j]["IsSupportingApproved"];
                                        dtFinal.Rows[dtFinal.Rows.Count - 1]["GSTNO"] = dtChild.Rows[j]["GSTNO"];
                                        //dtFinal.Rows[dtFinal.Rows.Count - 1]["vendor"] = dtChild.Rows[j]["vendor"];
                                        //dtFinal.Rows[dtFinal.Rows.Count - 1]["cgstamt"] = dtChild.Rows[j]["cgstamt"];
                                        //dtFinal.Rows[dtFinal.Rows.Count - 1]["sgstamt"] = dtChild.Rows[j]["sgstamt"];
                                        //dtFinal.Rows[dtFinal.Rows.Count - 1]["igstamt"] = dtChild.Rows[j]["igstamt"];
                                    }
                                }
                            }
                        }

                        gvDetails.DataSource = dt1;
                        gvDetails.DataBind();
                        decimal decTotalBillAmt = 0M;
                        decimal decTotalDa = 0M;
                        decimal decNgtHlt = 0M;
                        decimal decExtr = 0M;
                        decimal decTotalFAre = 0M;
                        decimal disttrav = 0M;
                        decimal decTotalAppr = 0M;
                        //Added on 22-12-2015
                        decimal claimAmt = 0M;
                        //End
                        decimal decTotalApp = 0M;
                        decimal decVerifiedAmt = 0M;
                        foreach (GridViewRow gvrow in gvDetails.Rows)
                        {
                            if (gvrow.Cells[0].Text.Trim() == "&nbsp;")
                            {
                                gvrow.Style.Add("font-weight", "normal");

                            }
                            else
                            {
                                disttrav += Convert.ToDecimal(gvrow.Cells[4].Text.Trim());
                                decExtr += Convert.ToDecimal(gvrow.Cells[5].Text.Trim());
                                decNgtHlt += Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                                decTotalDa += Convert.ToDecimal(gvrow.Cells[8].Text.Trim());
                                decTotalFAre += Convert.ToDecimal(gvrow.Cells[9].Text.Trim());
                                decTotalBillAmt += Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                                decVerifiedAmt += Convert.ToDecimal(gvrow.Cells[12].Text.Trim());
                                decTotalAppr += Convert.ToDecimal(gvrow.Cells[13].Text.Trim());
                                //Added on 22-12-2015
                                claimAmt += Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                                //End
                                //  decTotalApp += Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                                //gvrow.Style.Add("font-weight", "bold");
                            }
                        }
                        decimal finalamount = (decTotalBillAmt - (Convert.ToDecimal(dt.Rows[0]["AdvanceAmount"].ToString())));
                        //gvDetails.FooterRow.Cells[4].Style=fon
                        gvDetails.FooterRow.Cells[3].Text = "Total";
                        gvDetails.FooterRow.Cells[4].Text = Convert.ToString(disttrav);
                        gvDetails.FooterRow.Cells[5].Text = Convert.ToString(decExtr);
                        gvDetails.FooterRow.Cells[7].Text = Convert.ToString(decNgtHlt);
                        gvDetails.FooterRow.Cells[8].Text = Convert.ToString(decTotalDa);
                        gvDetails.FooterRow.Cells[9].Text = Convert.ToString(decTotalFAre);
                        //gvDetails.FooterRow.Cells[11].Text = Convert.ToString(decTotalBillAmt);
                        //Added on 22-12-2015
                        gvDetails.FooterRow.Cells[11].Text = Convert.ToString(claimAmt);
                        gvDetails.FooterRow.Cells[12].Text = Convert.ToString(decVerifiedAmt);
                        lbladvamo.Text = dt.Rows[0]["AdvanceAmount"].ToString();
                        gvDetails.FooterRow.Cells[13].Text = Convert.ToString(decTotalAppr);
                        lblfnlamo.Text = finalamount.ToString();
                        //gvDetails.FooterRow.Cells[12].Text = dt.Rows[0]["AdvanceAmount"].ToString();
                        //gvDetails.FooterRow.Cells[13].Text = finalamount.ToString();
                        //End
                        gvDetails.FooterRow.Style.Add("font-weight", "bold");

                        //lblTotal.Text = Convert.ToString(decTotalBillAmt);
                        //if (Convert.ToInt32(lblTotalApproved.Text!="0")
                        lblTotal.Text = finalamount.ToString();
                        //lblTotalApproved.Text = Convert.ToString(decTotalApp);

                    }
                }
            }
        }

        //Following function will return Distinct records for Name, City and State column.
        public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseApproval.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseSheetSummaryPrint.aspx");
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            Session["ExcelExport1"] = "Yes";
            Response.Redirect("~/ExpenseSheetSummaryPrint.aspx");
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[15].Text == "Yes")
                {
                    e.Row.Cells[16].Visible = true;
                }
                else
                {
                    e.Row.Cells[16].Visible = false;
                }
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
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