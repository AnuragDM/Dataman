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
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class ExpenseSelfSheet : System.Web.UI.Page
    {
        public decimal runningBillTotal = 0;
        public decimal runningClaimTotal = 0;
        public decimal runningApprovedTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            DateFrom.Attributes.Add("readonly", "readonly");
            DateTo.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                Bindgroup();
                BindExpenseType();
                DateFrom.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// DateTime.Now.AddSeconds(19800).ToString("dd/MMM/yyyy");
                DateTo.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// DateTime.Now.AddSeconds(19800).ToString("dd/MMM/yyyy");
             
            }
        }
        private void Bindgroup()
        {
            //Nishu - 21/07/2016
            string strgrp = "select ExpenseGroupId,GroupName from ExpenseGroup where SMID= " + Settings.Instance.SMID + "";
            fillDDLDirect(ddlexpGrp, strgrp, "ExpenseGroupId", "GroupName", 1);
        }
        private void BindExpenseType() {
            //Ankita - 11/may/2016- (For Optimization)
          //  string str = "select * from MastExpenseType where Active=1 or Id in(select ExpenseTypeId from ExpenseDetails Ed Inner Join ExpenseGroup Eg On Ed.ExpenseGroupId=Eg.ExpenseGroupId where Eg.UserId="+Settings.Instance.UserID+") order by Name";
            string str = "select Id,Name from MastExpenseType where Active=1 or Id in(select ExpenseTypeId from ExpenseDetails Ed Inner Join ExpenseGroup Eg On Ed.ExpenseGroupId=Eg.ExpenseGroupId where Eg.UserId=" + Settings.Instance.UserID + ") order by Name";
            fillDDLDirect(ddlexpType, str, "Id", "Name", 1);
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
        #region BindDataGrids
        private void DataGrid()
        {
            string addqry = "";
            if (ddlexpGrp.SelectedIndex > 0) { addqry += " and Eg.ExpenseGroupId=" + ddlexpGrp.SelectedValue + ""; }
            if (ddlexpType.SelectedIndex > 0) { addqry += " and ED.ExpenseTypeID="+ddlexpType.SelectedValue+""; }
            DataTable dt = new DataTable();
            string strQqy = " select * from (select Eg.GroupName,Eg.ExpenseGroupId,Ed.ExpenseDetailID,ED.BillAmount,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/') AS BillDate,ED.BillNumber,isnull(Ed.ApprovedAmount,0) as ApprovedAmount,ED.CityID,ED.ClaimAmount,ED.FromCity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/') AS ToDate,MET.Name,MAC.AreaName AS FromCityName,'' AS TocityName,MET.ExpenseTypeCode,Case ED.IsSupportingAttached when 1 then 'Yes' else 'No' end as IsSupportingAttached1,case isnull(eg.IsSubmitted,0) when 0 then 'Active' else case isnull(eg.IsApproved,0) when 0 then 'Submitted' when 1 then 'Approved' WHEN 2 THEN 'Submitted' end	end AS status,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,(SELECT TOP(1) statename FROM viewgeo WHERE  cityid = ed.cityid) AS fromstate,'' AS tostate,Ed.kmvisit, Ed.prekilometerrate, Ed.fromtime, Ed.totime, mtm.NAME AS Travelconvmode from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id Inner Join MastArea MAC on Ed.CityID=MAC.AreaId Inner Join ExpenseGroup Eg On Ed.ExpenseGroupId=Eg.ExpenseGroupId LEFT JOIN masttravelmode mtm  ON ed.travelmodeid = mtm.id where Eg.UserId=" + Settings.Instance.UserID + " and cast(Ed.BillDate as Date) >= cast('" + DateFrom.Text + "' as date) and cast(Ed.BillDate as Date) <= cast('" + DateTo.Text + "' as date) " + addqry + " group by Eg.GroupName,Eg.ExpenseGroupId,ED.BillDate,met.Name,Ed.ExpenseDetailID,ED.BillAmount,ED.ApprovedAmount,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MAC.AreaName,MET.ExpenseTypeCode,ED.IsSupportingAttached,IsApproved,IsSubmitted,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME" +
                 " Union " + " Select Eg.GroupName,Eg.ExpenseGroupId,Ed.ExpenseDetailID,ED.BillAmount,Replace(CONVERT(NVARCHAR, billdate, 106), ' ', '/') AS BillDate,ED.BillNumber,isnull(Ed.ApprovedAmount,0) as ApprovedAmount,ED.CityID,ED.ClaimAmount,ED.FromCity,Replace(CONVERT(NVARCHAR, ED.fromdate, 106), ' ', '/') AS FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,Replace(CONVERT(NVARCHAR, ED.todate, 106), ' ', '/')  AS ToDate,MET.Name,MAC.AreaName AS FromCityName,Mac1.areaname AS TocItyName,MET.ExpenseTypeCode,Case ED.IsSupportingAttached when 1 then 'Yes' else 'No' end as IsSupportingAttached1,case isnull(eg.IsSubmitted,0) when 0 then 'Active' else case isnull(eg.IsApproved,0) when 0 then 'Submitted' when 1 then 'Approved' WHEN 2 THEN 'Submitted' end end AS status,CASE ED.staywithrelative WHEN 1 THEN 'Yes' ELSE 'No' END AS StayWithRelative1,(SELECT TOP(1) statename FROM viewgeo WHERE  cityid = ed.cityid) AS fromstate,(SELECT TOP(1) statename FROM viewgeo WHERE cityid = ed.tocity) AS tostate,Ed.kmvisit, Ed.prekilometerrate, Ed.fromtime, Ed.totime, mtm.NAME AS Travelconvmode from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id Inner join MastArea MAC on Ed.FromCity=MAC.AreaId LEFT JOIN mastarea Mac1 ON Ed.tocity = Mac1.areaid Inner Join ExpenseGroup Eg On Ed.ExpenseGroupId=Eg.ExpenseGroupId LEFT JOIN masttravelmode mtm  ON ed.travelmodeid = mtm.id where Eg.UserId=" + Settings.Instance.UserID + " and  cast(Ed.BillDate as Date) >= cast('" + DateFrom.Text + "' as date) and cast(Ed.BillDate as Date) <= cast('" + DateTo.Text + "' as date)" + addqry + " group by Eg.GroupName,Eg.ExpenseGroupId,ED.BillDate,met.Name,Ed.ExpenseDetailID,ED.BillAmount,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.ApprovedAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MAC.AreaName,Mac1.areaname,MET.ExpenseTypeCode,ED.IsSupportingAttached,IsApproved,IsSubmitted,Ed.staywithrelative,Ed.kmvisit,Ed.prekilometerrate,Ed.fromtime,Ed.totime,mtm.NAME) a order by a.billdate,a.groupname,a.NAME DESC ";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQqy);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        #endregion

        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(DateFrom.Text) > Convert.ToDateTime(DateTo.Text))
            {
                rpt.DataSource = null;
                rpt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }
            DataGrid();
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblapprAmt = (Label)e.Item.FindControl("lblappramt");
                if (DataBinder.Eval(e.Item.DataItem, "Status").ToString().ToUpper() != "Approved".ToUpper())
                {
                    
                    lblapprAmt.Text = "0.0";
                }

                //runningBillTotal += Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "BillAmount"));
                //runningClaimTotal += Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "ClaimAmount"));
                //runningApprovedTotal += Convert.ToDecimal(lblapprAmt.Text);
            }
            
        }
    }
}