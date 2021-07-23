using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class ExpenseSheetSummaryPrint : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                int ExpGrpId = Convert.ToInt32(Session["ExpGrpId"]);
                LoadData(ExpGrpId);
                //Session["ExpGrpId"] = null;
                //HttpContext.Current.Response.Write("<script>window.print();</script>");                

            }
        }

        private void LoadData(int ExpGrpId)
        {
            string str =string.Format(@"SELECT Distinct T2.SMName,T4.Name,isnull(T2.SyncId,'') AS Code,isnull(T1.VoucherNo,0) AS VoNo,T1.GroupName,T6.cityName AS City,T6.stateName AS State,
							Isnull(T7.Name,'') AS Grade,
                            T5.DesName AS Designation,Convert(varchar(15),CONVERT(date,T1.CreatedOn,103),106) AS CreatedDate,(select smname from mastsalesrep where smid =(select CreatedBy from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as CreatedBy, " +
                           " Isnull(Convert(varchar(15),CONVERT(date,T1.DateOfSubmission,103),106),'') AS SubmittedDate," +
                           " Isnull(T1.TotalAmount,0) AS Total,Isnull(T1.totalverifiedamt,0) AS VerifiedAmount,Isnull(T1.TotalApprovedAmount,0) AS ApprovedAmount,Isnull(T1.AdvanceAmount,0) AS AdvanceAmount,T1.VerifiedByDateTime" +
                           " FROM ExpenseGroup AS T1" +
                           " Left JOIN MastSalesRep AS T2" +
                          "  ON T1.SMID=T2.SMId" +
                          "  LEFT JOIN MastGrade AS T3" +
                         "   ON T2.GradeId=T3.Id" +
                          "  LEFT JOIN MastLogin AS T4" +
                          "  ON T2.UserId=T4.Id " +
                           " LEFT JOIN MastDesignation AS T5" +
                          "  ON T4.DesigId=T5.DesId" +
                          "  LEFT JOIN ViewGeo AS T6" +
                          "  ON T2.CityId=T6.cityid " +
                          "  LEFT JOIN MastGrade AS T7" +
                          "  ON T2.GradeId=T7.Id  WHERE T1.ExpenseGroupID=" + ExpGrpId + "");
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                lblSalesPersonName.Text = " " + Convert.ToString(dt.Rows[0]["SMName"]);
                lblGroupName.Text = " " + Convert.ToString(dt.Rows[0]["GroupName"]);
                lblCreatedBy.Text = " " + Convert.ToString(dt.Rows[0]["SMName"]);
                lblVoucherNo.Text = " " + Convert.ToString(dt.Rows[0]["VoNo"]);
                lblDesignation.Text = " " + Convert.ToString(dt.Rows[0]["Designation"]);
                lblTotal.Text = " " + Convert.ToString(dt.Rows[0]["Total"]);
                lblTotalApproved.Text = " " + Convert.ToString(dt.Rows[0]["ApprovedAmount"]);
                lblTotalVerified.Text = " " + Convert.ToString(dt.Rows[0]["VerifiedAmount"]);
                //lblTotal.Text = lblTotalApproved.Text;
                //lblGrade.Text = " " + Convert.ToString(dt.Rows[0]["Grade"]);
                //lblCode.Text = " " + Convert.ToString(dt.Rows[0]["Code"]);
                lblVerfiedDatetIme.Text = " " + Convert.ToString(dt.Rows[0]["VerifiedByDateTime"]);
                if ((dt.Rows[0]["CreatedDate"]).ToString()!="")
                {
                    lblCreated.Text = " " + DateTime.Parse(Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToShortDateString()).ToString("dd/MMM/yyyy"); //Convert.ToString(dt.Rows[0]["CreatedDate"]);
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
                //if ((dt.Rows[0]["SubmittedDate"]).ToString()!="")
                //{
                //    lblSubmitted.Text = " " + DateTime.Parse(Convert.ToDateTime(dt.Rows[0]["SubmittedDate"]).ToShortDateString()).ToString("dd/MMM/yyyy");// Convert.ToString(dt.Rows[0]["SubmittedDate"]);
                //}
                //else
                //{
                //    lblSubmitted.Text = " " + string.Empty;
                //}
               // lblTotal.Text = " " + Convert.ToString(dt.Rows[0]["Total"]);
              //  lblTotalApproved.Text = " " + Convert.ToString(dt.Rows[0]["ApprovedAmount"]);

            }
            //imggoldiee.ImageUrl = this.GetAbsoluteUrl(imggoldiee.ImageUrl);
            string str1 = @"SELECT T1.ExpenseDetailID,T1.ExpenseGroupID,T2.Id AS ExpenseTypeID,T2.Name AS ExpenseTypeName,T1.BillNumber,
                            Convert(varchar(15),CONVERT(date,T1.BillDate,103),106) AS BillDate,Convert(varchar(15),CONVERT(date,T1.CreatedOn,103),106) AS CreatedOn,
                            Isnull(T1.BillAmount,0) AS BillAmount,Isnull(T1.ClaimAmount,0) AS ClaimAmount,
                            CASE WHEN T1.IsSupportingAttached=1 THEN 'Yes'
                            ELSE 'No' END AS Enclosed,CASE WHEN T1.IsSupportingApproved=1 THEN 'Received'
                            ELSE 'Not Received' END AS SupportingStatus,Isnull(T1.ApprovedAmount,0) AS ApprovedAmount,
                            Isnull(T1.Remarks,0) AS Remarks,CASE WHEN T1.IsSupportingApproved=1 THEN 'Yes'
                            WHEN T1.IsSupportingApproved=0 THEN 'No' ELSE ''END AS IsSupportingApproved,GSTNO,
                            vendor,isnull(cgstamt,0) AS cgstamt,isnull(sgstamt,0) AS sgstamt,isnull(igstamt,0) AS igstamt ,[dbo].[GetTravelModeName](T1.travelmodeid ) Name,fma1.AreaName as farea,Tma1.AreaName as Tarea,t1.da,t1.KMVisit as kms,T1.Remarks as MainRemarks,(select smname from mastsalesrep where smid =(select approvedbysenior from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as senior,(select smname from mastsalesrep where smid =(select VerifiedBy from ExpenseGroup where ExpenseGroupID=" + ExpGrpId + ")) as VerifiedBy,0 as AdvanceAmount,isnull(T1.VerifiedAmt,0) as VerifiedAmt,isnull(T1.NighthaltAmt,0) NighthaltAmt  FROM ExpenseDetails  AS T1 LEFT JOIN MastExpenseType AS T2    ON T1.ExpenseTypeID=T2.Id left join [MastTravelMode] as mtm on mtm.Id=t1.TravelModeID left join mastarea as fma1 on fma1.areaid=t1.FromCity	left join mastarea as Tma1 on Tma1.areaid=t1.ToCity	left join mastarea as fma on fma.areaid=t1.FromArea " +
                               " WHERE T1.ExpenseGroupID=" + ExpGrpId + " order by T1.ExpenseDetailID";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            DataTable dtFinal = dt1.Clone();
            DataTable dtChild = new DataTable();
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                //gvDetails.DataSource = dt1;
                //gvDetails.DataBind();
                //lblexpgp.Text = dt1.Rows[0]["groupname"].ToString();
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
                              
                                //dtFinal.Rows[dtFinal.Rows.Count - 1]["ExpenseTypeName"] = dtChild.Rows[0]["ExpenseTypeName"];
                                ////dtFinal.Rows[dtFinal.Rows.Count - 1]["Name"] = dtChild.Rows[0]["Name"];
                                ////dtFinal.Rows[dtFinal.Rows.Count - 1]["Travelledcity"] = dtChild.Rows[0]["Travelledcity"];
                                ////dtFinal.Rows[dtFinal.Rows.Count - 1]["travelledarea"] = dtChild.Rows[0]["travelledarea"];
                                //decimal BillAmount = 0M;
                                //decimal ClaimAmount = 0M;
                                //decimal ApprovedAmount = 0M;
                                //for (int j = 0; j < dtChild.Rows.Count; j++)
                                //{
                                //    BillAmount += Convert.ToDecimal(dtChild.Rows[j]["BillAmount"]);
                                //    ClaimAmount += Convert.ToDecimal(dtChild.Rows[j]["ClaimAmount"]);
                                //    ApprovedAmount += Convert.ToDecimal(dtChild.Rows[j]["ApprovedAmount"]);
                                //}
                                //dtFinal.Rows[dtFinal.Rows.Count - 1]["BillAmount"] = BillAmount;
                                //dtFinal.Rows[dtFinal.Rows.Count - 1]["ClaimAmount"] = ClaimAmount;
                                //dtFinal.Rows[dtFinal.Rows.Count - 1]["ApprovedAmount"] = ApprovedAmount;


                                //for (int j = 0; j < dtChild.Rows.Count; j++)
                                //{
                                //    dtFinal.Rows.Add();
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["BillNumber"] = dtChild.Rows[j]["BillNumber"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["Name"] = dtChild.Rows[j]["Name"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["farea"] = dtChild.Rows[j]["farea"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["Tarea"] = dtChild.Rows[j]["Tarea"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["BillDate"] = dtChild.Rows[j]["BillDate"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["Da"] = dtChild.Rows[j]["Da"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["BillAmount"] = dtChild.Rows[j]["BillAmount"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["ClaimAmount"] = dtChild.Rows[j]["ClaimAmount"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["ApprovedAmount"] = dtChild.Rows[j]["ApprovedAmount"];
                                //    dtFinal.Rows[dtFinal.Rows.Count - 1]["Remarks"] = dtChild.Rows[j]["Remarks"];
                                //}
                            }
                        }
                    }
                    lblState.Text = " " + Convert.ToString(dt1.Rows[0]["senior"]);
                    lblVerified.Text = " " + Convert.ToString(dt1.Rows[0]["VerifiedBy"]);
                    //lblGroupName.Text = " " + Convert.ToString(dt.Rows[0]["GroupName"]);
                    gvDetails.DataSource = dt1;
                    gvDetails.DataBind();
                    decimal decTotalBillAmt = 0M;
                    decimal decTotalDa = 0M;
                    decimal decNgtHlt = 0M;
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
                            decNgtHlt += Convert.ToDecimal(gvrow.Cells[6].Text.Trim());
                            decTotalDa += Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                            decTotalFAre += Convert.ToDecimal(gvrow.Cells[8].Text.Trim());
                            decTotalBillAmt += Convert.ToDecimal(gvrow.Cells[9].Text.Trim());
                            decVerifiedAmt += Convert.ToDecimal(gvrow.Cells[10].Text.Trim());
                            decTotalAppr += Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                            //Added on 22-12-2015
                            claimAmt += Convert.ToDecimal(gvrow.Cells[9].Text.Trim());
                            // decTotalApp += Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                            gvrow.Style.Add("font-weight", "bold");
                        }
                    }
                    decimal finalamount = (decTotalBillAmt - (Convert.ToDecimal(dt.Rows[0]["AdvanceAmount"].ToString())));
                    //gvDetails.FooterRow.Cells[4].Style=fon
                    gvDetails.FooterRow.Cells[4].Text = Convert.ToString(disttrav);
                    gvDetails.FooterRow.Cells[6].Text = Convert.ToString(decNgtHlt);
                    gvDetails.FooterRow.Cells[7].Text = Convert.ToString(decTotalDa);
                    gvDetails.FooterRow.Cells[8].Text = Convert.ToString(decTotalFAre);
                    gvDetails.FooterRow.Cells[10].Text = Convert.ToString(decTotalBillAmt);
                    //Added on 22-12-2015
                    gvDetails.FooterRow.Cells[10].Text = Convert.ToString(decVerifiedAmt);
                    gvDetails.FooterRow.Cells[9].Text = Convert.ToString(claimAmt);
                    lbladvamo.Text = dt.Rows[0]["AdvanceAmount"].ToString();
                    gvDetails.FooterRow.Cells[11].Text = Convert.ToString(decTotalAppr);
                    lblfnlamo.Text = finalamount.ToString();
                    //gvDetails.FooterRow.Cells[12].Text = dt.Rows[0]["AdvanceAmount"].ToString();
                    //gvDetails.FooterRow.Cells[13].Text = finalamount.ToString();
                    //End
                    gvDetails.FooterRow.Style.Add("font-weight", "bold");

                    lblTotal.Text = lblTotalApproved.Text;//Convert.ToString(decTotalBillAmt);
                    //lblTotalApproved.Text = Convert.ToString(decTotalApp);

                }
                // Added Nishu 22/07/2016 For csv export

                //Response.ContentType = "Application/x-msexcel";
                //Response.AddHeader("content-disposition", "attachment;filename=ExpenseReport.csv");
                //Response.Write(ExportToCSVFile(gvDetails));
                //Response.End();

                //

                //if (Convert.ToString(Session["ExcelExport1"]) == "Yes")
                //{
                //    Session["ExcelExport1"] = null;
                //    Response.ContentType = "application/x-msexcel";
                //    Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xls"); Response.ContentEncoding = Encoding.UTF8;
                //    StringWriter tw = new StringWriter(); HtmlTextWriter hw = new HtmlTextWriter(tw); tbl1.RenderControl(hw);
                //    Response.Write(tw.ToString());
                //    Response.End();
                //}

            }
        }
        private string GetAbsoluteUrl(string relativeUrl)
        {
            relativeUrl = relativeUrl.Replace("~/", string.Empty);
            string[] splits = Request.Url.AbsoluteUri.Split('/');
            if (splits.Length >= 2)
            {
                string url = splits[0] + "//";
                for (int i = 2; i < splits.Length - 1; i++)
                {
                    url += splits[i];
                    url += "/";
                }

                return url + relativeUrl;
            }
            return relativeUrl;
        }
        private string ExportToCSVFile(GridView gv)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("Expense sheet summary");
            sb.Append(Environment.NewLine);
          //  sb.Append(lblSalesPersonName.Text + ",,,,,,,Code: " + lblCode.Text + ",Voucher Number: " + lblVoucherNo.Text);            
            sb.Append(Environment.NewLine);
         //   sb.Append("City: " + lblCity.Text + ",,,,,,State: " + lblState.Text + ",Grade: " + lblGrade.Text + ",Designation: " + lblDesignation.Text);           
            sb.Append(Environment.NewLine);
            sb.Append("Created: " + lblCreated.Text + ",,,,,," );           
            sb.Append(Environment.NewLine);
            for (int k = 0; k < gv.Columns.Count; k++)
            {                
                sb.Append(gv.Columns[k].HeaderText + ',');
            }
            //append new line
            sb.Append("\r\n");
            decimal[] totalVal = new decimal[6];
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                for (int k = 0; k < gv.Columns.Count; k++)
                {
                    if (k == 2)
                    {
                        //sb.Append(gv.Rows[i].Cells[2].Text.ToString());
                        sb.Append(String.Format("\"{0}\"", gv.Rows[i].Cells[2].Text.Replace("&nbsp;", "")).ToString() + ',');                        
                    }
                    else
                    {
                        sb.Append(gv.Rows[i].Cells[k].Text.Replace(",", "").Replace("&nbsp;", "") + ",");
                    }
                }
                //append new line
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //if (Convert.ToString(Session["ExcelExport1"]) == "Yes")
            //{
            //Session["ExcelExport1"] = null;
            using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
            {
                using (HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                {
                    base.Render(htmlWrite);
                    DownloadExcel(stringWrite.ToString());
                }
            }
            //}
            //else
            //{
            //    base.Render(writer);
            //}
        }

        public void DownloadExcel(string text)
        {
            try
            {
                HttpResponse response = Page.Response;                
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("content-disposition", " filename=" + lblSalesPersonName.Text.Trim().Replace(" ", "-").Replace(",", "-") + "-" + lblGroupName.Text.Replace(" ", "-").Replace(",", "-") + ".xls");               
                Response.ClearContent();
                Response.Buffer = true;               
                response.Write(text);
                response.Flush();
                response.End();               
            }
            catch (ThreadAbortException)
            {
                //If the download link is pressed we will get a thread abort.
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
            //PrintWebControl(mainDiv, "");
        }


    }
}