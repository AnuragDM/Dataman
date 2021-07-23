using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class LeaveApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if(!IsPostBack)
            {
                btnCancel.Visible = true;
                BindSalePersonDDl();
                if (Request.QueryString["SMId"] != null)
                {
                    BindRpt(Request.QueryString["SMId"]);
                }
            }
            if (Request.QueryString["Open"] != null)
            {
                string open = Request.QueryString["Open"];
                if (open == "D")
                {
                    btnGo_Click(null, null);// for dashboard
                }
            }
            
        }
        private void BindRpt(string p)
        {
            try
            {
                string notQry1 = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
                where r.SMId=" + Convert.ToInt32(p) + " and r.AppStatus='Pending' order by r.Vdate desc";

                DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry1);
                rptmain.Style.Add("display", "block");
                rpt.DataSource = dtNot;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindSalePersonDDl()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DdlSalesPerson.DataSource = dv.ToTable();
            DdlSalesPerson.DataTextField = "SMName";
            DdlSalesPerson.DataValueField = "SMId";
            DdlSalesPerson.DataBind();
            //Add Default Item in the DropDownList
            DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            
            string smiD = "";
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            string smIDStr = "";
            string smIDStr1 = "";
            if (dtSMId.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSMId.Rows)
                {
                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            }
            if(DdlSalesPerson.SelectedIndex>0)
            {
                smiD = DdlSalesPerson.SelectedValue;
                string notQry = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
                where r.SMId=" + Convert.ToInt32(smiD) + " and r.AppStatus='Pending' order by r.Vdate desc";

                //string notQry = @"select TransNotification.NotiId,TransNotification.pro_id,TransNotification.userid,TransNotification.VDate,TransNotification.displayTitle,TransNotification.msgURL,TransNotification.SMId,TransNotification.ToSMId,MastSalesRep.SMName from TransNotification left join MastSalesRep on TransNotification.SMId=MastSalesRep.SMId where TransNotification.SMId=" + smiD + " and TransNotification.pro_id='LEAVEREQUEST'";
                DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                rptmain.Style.Add("display", "block");
                rpt.DataSource = dtNot;
                rpt.DataBind();
            }
            else
            {
                //string notQry = @"select TransNotification.NotiId,TransNotification.pro_id,TransNotification.userid,TransNotification.VDate,TransNotification.displayTitle,TransNotification.msgURL,TransNotification.SMId,TransNotification.ToSMId,MastSalesRep.SMName from TransNotification left join MastSalesRep on TransNotification.SMId=MastSalesRep.SMId where TransNotification.SMId in (" + smIDStr1 + ") and TransNotification.pro_id='LEAVEREQUEST'";
                string notQry = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
                where r.SMId in (" + smIDStr1 + ") and r.AppStatus='Pending' order by r.Vdate desc";
                DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                rptmain.Style.Add("display", "block");
                rpt.DataSource = dtNot;
                rpt.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveApproval.aspx");
        }
    }
}