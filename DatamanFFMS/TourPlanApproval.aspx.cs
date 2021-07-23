using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class TourPlanApproval : System.Web.UI.Page
    {
        string pageName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
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
                string notQry1 = @"select r.TourPlanHId,r.DocId, r.VDate,r.AppStatus,msr.SMName,r.SMId
                                    from TransTourPlanHeader r
                                    left join MastSalesRep msr on msr.SMId=r.SMId
                                    where r.SMId=" + Convert.ToInt32(p) + " and r.AppStatus='Pending' order by r.VDate desc";

//                 string notQry1 = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
//                                    from TransTourPlan r
//                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
//                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
//                                    where r.SMId=" + Convert.ToInt32(p) + " and r.AppStatus='Pending' order by r.VDate desc";

                 DataTable dtNot1 = DbConnectionDAL.GetDataTable(CommandType.Text, notQry1);
                 rptmain.Style.Add("display", "block");
                 rpt.DataSource = dtNot1;
                 rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
            if (DdlSalesPerson.SelectedIndex > 0)
            {
                smiD = DdlSalesPerson.SelectedValue;            

                string notQry = @"select r.TourPlanHId,r.DocId, r.VDate,r.AppStatus,msr.SMName,r.SMId
                                    from TransTourPlanHeader r
                                    left join MastSalesRep msr on msr.SMId=r.SMId
                                    where r.SMId=" + smiD + " and r.AppStatus='Pending' order by r.VDate desc";

                DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                rptmain.Style.Add("display", "block");
                rpt.DataSource = dtNot;
                rpt.DataBind();
            }
            else
            {
//                string notQry = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
//                                    from TransTourPlan r
//                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
//                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
//                                    where r.SMId in (" + smIDStr1 + ")  and r.AppStatus='Pending' order by r.VDate desc";

                string notQry = @"select r.TourPlanHId,r.DocId, r.VDate,r.AppStatus,msr.SMName,r.SMId
                                    from TransTourPlanHeader r
                                    left join MastSalesRep msr on msr.SMId=r.SMId
                                    where r.SMId in (" + smIDStr1 + ") and r.AppStatus='Pending' order by r.VDate desc";
                
                DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                rptmain.Style.Add("display", "block");
                rpt.DataSource = dtNot;
                rpt.DataBind();
            }
        }

        private void BindSalePersonDDl()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            //dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + " and RoleType<>'AreaIncharge'";
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + " ";
            DdlSalesPerson.DataSource = dv.ToTable();
            DdlSalesPerson.DataTextField = "SMName";
            DdlSalesPerson.DataValueField = "SMId";
            DdlSalesPerson.DataBind();
            //Add Default Item in the DropDownList
            DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TourPlanApproval.aspx");
        }
    }
}