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
    public partial class BeatPlanApproval : System.Web.UI.Page
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
                string notQry1 = @"select distinct TransBeatPlan.DocId,TransBeatPlan.StartDate,TransBeatPlan.SMId,msr.SMName,TransBeatPlan.AppStatus from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.SMId=" + Convert.ToInt32(p) + " and TransBeatPlan.AppStatus='Pending' order by TransBeatPlan.StartDate desc";

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
        { //Ankita - 07/may/2016- (For Optimization)
            //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
            //DataTable dtRole = new DataTable();
            //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
         //   string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
            string RoleTy = Settings.Instance.RoleType;
            if (RoleTy == "RegionHead" || RoleTy == "StateHead")
            {
                DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt1);
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
                dv.Sort = "SMName asc";
                DdlSalesPerson.DataSource = dv;
                DdlSalesPerson.DataTextField = "SMName";
                DdlSalesPerson.DataValueField = "SMId";
                DdlSalesPerson.DataBind();
                DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + " and RoleType='AreaIncharge'";
                dv.Sort = "SMName asc";
                DdlSalesPerson.DataSource = dv;
                DdlSalesPerson.DataTextField = "SMName";
                DdlSalesPerson.DataValueField = "SMId";
                DdlSalesPerson.DataBind();
                //Add Default Item in the DropDownList
                DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {//Ankita - 07/may/2016- (For Optimization)
                string smiD = "";
                //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                //DataTable dtRole = new DataTable();
                //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                string RoleTy = Settings.Instance.RoleType;
                DataTable dtSMId = new DataTable();
                DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
                if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                {
                    DataView dv = new DataView(dt1);
                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
                    dtSMId = dv.ToTable();
                }
                else { dtSMId = dt1; }

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
                    string notQry = @"select distinct TransBeatPlan.DocId,TransBeatPlan.StartDate,TransBeatPlan.SMId,msr.SMName,TransBeatPlan.AppStatus from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.SMId=" + smiD + " and TransBeatPlan.AppStatus='Pending' order by TransBeatPlan.StartDate desc";
                    DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                    rptmain.Style.Add("display", "block");
                    rpt.DataSource = dtNot;
                    rpt.DataBind();
                }
                else
                {
                    string notQry = @"select distinct TransBeatPlan.DocId,TransBeatPlan.StartDate,TransBeatPlan.SMId,msr.SMName,TransBeatPlan.AppStatus from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.SMId in (" + smIDStr1 + ") and TransBeatPlan.AppStatus='Pending' order by TransBeatPlan.StartDate desc";
                    DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
                    rptmain.Style.Add("display", "block");
                    rpt.DataSource = dtNot;
                    rpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanApproval.aspx");
        }
    }
}