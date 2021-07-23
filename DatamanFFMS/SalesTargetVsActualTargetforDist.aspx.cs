using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Globalization;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace AstralFFMS
{
    public partial class SalesTargetVsActualTargetforDist : System.Web.UI.Page
    {
        int cnt = 0;
        int uid = 0;
        int smID = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                BindDDLMonth();
                roleType = Settings.Instance.RoleType;
                fillfyear();
                BindTreeViewControl();
                List<SaleTargetDist> distributors = new List<SaleTargetDist>();
                distributors.Add(new SaleTargetDist());
                distreportrpt.DataSource = distributors;
                distreportrpt.DataBind();
                //string pageName = Path.GetFileName(Request.Path);
                BindUnit();
                if (btnexport.Text == "Export")
                {
                    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
                }
            }
            if (Request.QueryString["Date"] != null && Request.QueryString["SMID"] != null)
            {
                dateHiddenField.Value = Request.QueryString["Date"];
                smIDHiddenField.Value = Request.QueryString["SMID"];
                smID = Convert.ToInt32(Request.QueryString["SMID"]);
            }
        }
        private void BindUnit()
        {
            string str = "select Id, Unit from MastUnit ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlunit.DataSource = dt;
                ddlunit.DataTextField = "Unit";
                ddlunit.DataValueField = "id";
                ddlunit.DataBind();
            }
            ddlunit.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }
        }

        private void fillfyear()
        {
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                txtcurrentyear.DataSource = dt;
                txtcurrentyear.DataTextField = "Yr";
                txtcurrentyear.DataValueField = "id";
                txtcurrentyear.DataBind();
            }

        }
        private void BindDDLMonth()
        {
            try
            {

                //for (int month = 1; month <= 12; month++)
                //{
                //    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                //    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));

                //}

                //listboxmonth.Items.Add(new ListItem("Select", "0", true));
                for (int month = 4; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }
                for (int month = 1; month <= 3; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void fillUnderUsers()
        {
            if (roleType == "Admin")
            {//Ankita - 18/may/2016- (For Optimization)
                //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                DataTable dtcheckrole = new DataTable();
                dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                DataView dv1 = new DataView(dtcheckrole);
                dv1.RowFilter = "SMName<>.";
                dv1.Sort = "SMName asc";

                //ddlunderUser.DataSource = dv1.ToTable();
                //ddlunderUser.DataTextField = "SMName";
                //ddlunderUser.DataValueField = "SMId";
                //ddlunderUser.DataBind();
            }
            else
            {
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        //ddlunderUser.DataSource = dv;
                        //ddlunderUser.DataTextField = "SMName";
                        //ddlunderUser.DataValueField = "SMId";
                        //ddlunderUser.DataBind();
                    }
                    catch { }

                }
            }
            //ddlunderUser.Items.Insert(0, new ListItem("All", "0"));
        }
        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            string smIDStr = "", smIDStr12 = "";
            if (cnt == 0)
            {
                foreach (TreeNode node in trview.CheckedNodes)                    {

                {
                    smIDStr12 = node.Value;
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindArea(smIDStr12);
                }
                else
                {
                    ListBox1.Items.Clear();
                    ListBox1.DataBind();
                }
                Session["treenodes"] = smIDStr12;

            }
            cnt = cnt + 1;
            return;
        }


        private void BindArea(string SMIDStr)
        {
            string cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
            cityQry=cityQry+" AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
           cityQry=cityQry+" FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
            cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
           cityQry=cityQry+"  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
           cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
           cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) ";
           cityQry = cityQry + "  order by VG.areaName";
            DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            if (dtProdRep.Rows.Count > 0)
            {
                ListArea.DataSource = dtProdRep;
                ListArea.DataTextField = "Name";
                ListArea.DataValueField = "Id";
                ListArea.DataBind();
            }
        }

        private void BindDistributorDDl(string areaIDStr)
        {
            try
            {
                if (areaIDStr != "")
                {
                    string citystr = "";

                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);


                 //   string distqry = @"select PartyId,PartyName from MastParty where smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select PartyId,PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1 order by PartyName";
                    //              string distqry = @"select * from MastParty where CityId=" + dtCity.Rows[0]["AreaId"] + " and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                }
                else
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    //distreportrpt.DataSource = null;
                    //distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void txtcurrentyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grd.DataSource = null;
            //grd.DataBind();
        }
        private string checkRole()
        {
            string st = @"select RoleType from MastSalesRep M left join MastRole R on M.RoleId=R.RoleId where SMId=" + Settings.DMInt32(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
            string RName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return RName;
        }
        private void fillgrid()
        {

            //string IDStr = "", IDStr1 = "";

            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    IDStr1 = node.Value;
            //    {
            //        IDStr += node.Value + ",";
            //    }
            //}
            //IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
            //if (IDStr1 == "")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Sales Person');", true);
            //    return;
            //}

            //string s = "";
            //if (itemgroupRadioButtonlst.SelectedValue == "Group")
            //{
            //    s = "select ItemId as Id,ItemName as MatGrp from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            //}
            //else
            //{
            //    s = "select ItemId as Id,ItemName as MatGrp from MastItem where ItemType='ITEM' and Active=1 order by ItemName";
            //}
            ////DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            //DataTable dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            //if (dtLocTraRep.Rows.Count > 0)
            //{
            //    grd.DataSource = dtLocTraRep;
            //    grd.DataBind();
            //    if (targetRadioButtonList.SelectedValue == "Amount")
            //    {
            //        //Addde on 21-12-2015
            //        grd.HeaderRow.Cells[2].Text = "Distributor (Rs. in Lakhs)";
            //        //grd.HeaderRow.Cells[3].Text = "State Head (Rs. in Lakhs)";
            //        //grd.HeaderRow.Cells[4].Text = "District Head (Rs. in Lakhs)";
            //        //grd.HeaderRow.Cells[5].Text = "City Head (Rs. in Lakhs)";
            //    }
            //    else
            //    {
            //        grd.HeaderRow.Cells[2].Text = "Region Head (in Quantity)";
            //        //grd.HeaderRow.Cells[3].Text = "State Head (in Quantity)";
            //        //grd.HeaderRow.Cells[4].Text = "District Head (in Quantity)";
            //        //grd.HeaderRow.Cells[5].Text = "City Head (in Quantity)";

            //    }


            //    //grd.Columns[2].Visible = true;
            //    grd.Columns[2].Visible = true;
            //    grd.Columns[3].Visible = true;
            //    grd.Columns[4].Visible = true;
            //    grd.Columns[5].Visible = true;

            //    if (checkRole() == "RegionHead")
            //    {
            //    }
            //    if (checkRole() == "StateHead")
            //    {
            //        grd.Columns[2].Visible = false;
            //        //GridView1.HeaderRow.Cells[3].Text = "Assign to Team";
            //    }

            //    if (checkRole() == "DistrictHead")
            //    {
            //        grd.Columns[2].Visible = false;
            //        grd.Columns[3].Visible = false;
            //        //GridView1.HeaderRow.Cells[4].Text = "Assign to Team";
            //    }
            //    if (checkRole() == "CityHead")
            //    {
            //        grd.Columns[2].Visible = false;
            //        grd.Columns[3].Visible = false;
            //        grd.Columns[4].Visible = false;
            //        //GridView1.HeaderRow.Cells[5].Text = "Assign to Team";
            //    }
            //    if (checkRole() == "AreaIncharge")
            //    {
            //        grd.Columns[2].Visible = false;
            //        grd.Columns[3].Visible = false;
            //        grd.Columns[4].Visible = false;
            //        grd.Columns[5].Visible = false;
            //    }
            //    //End

            //}
            //else
            //{
            //    grd.DataSource = null;
            //    grd.DataBind();
            //}


        }


        private string GetSalesPersonName(int smID)
        {
            //var query = from u in context.MastSalesReps
            //            where u.SMId == smID
            //            select new { u.SMId, u.SMName };

            //Ankita - 18/may/2016- (For Optimization)
            //string salesrepqry1 = @"select * from MastSalesRep where SMId=" + smID + "";
            string salesrepqry1 = @"select SMName from MastSalesRep where SMId=" + smID + "";
            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry1);
            if (dtsalesrepqry1.Rows.Count > 0)
            {
                return Convert.ToString(dtsalesrepqry1.Rows[0]["SMName"]);
            }
            else
            {
                return string.Empty;
            }
        }

        private int GetSalesPerId(string p)
        {
            //var query = from u in context.TransVisits
            //            where u.VisId == Convert.ToInt64(p)
            //            select new { u.SMId };

            //Ankita - 18/may/2016- (For Optimization)
            //string salesrepqry = @"select * from TransVisit where VisId=" + Convert.ToInt64(p) + "";
            string salesrepqry = @"select SMId from TransVisit where VisId=" + Convert.ToInt64(p) + "";
            DataTable dtsalesrepqry = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry);
            if (dtsalesrepqry.Rows.Count > 0)
            {
                return Convert.ToInt32(dtsalesrepqry.Rows[0]["SMId"]);
            }
            else
            {
                return 0;
            }
        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SalesTargetVsActualTargetforDist.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
        }

        protected void btnLock_Click(object sender, EventArgs e)
        {

        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            fillgrid();
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string FDisId = "", DisId = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lnkRegion = (Label)e.Row.FindControl("lnkRegion");
                Label lnkState = (Label)e.Row.Cells[0].FindControl("lnkState");
                Label lnkDistrict = (Label)e.Row.Cells[0].FindControl("lnkDistrict");
                Label lnkCity = (Label)e.Row.Cells[0].FindControl("lnkCity");
                Label lnkArea = (Label)e.Row.Cells[0].FindControl("lnkArea");

                Label lnkRegion1 = (Label)e.Row.FindControl("lnkRegion1");
                Label lnkState1 = (Label)e.Row.Cells[0].FindControl("lnkState1");
                Label lnkDistrict1 = (Label)e.Row.Cells[0].FindControl("lnkDistrict1");
                Label lnkCity1 = (Label)e.Row.Cells[0].FindControl("lnkCity1");
                Label lnkArea1 = (Label)e.Row.Cells[0].FindControl("lnkArea1");
                HiddenField hid = (HiddenField)e.Row.Cells[0].FindControl("hidPartyTypeId");

                // string MonthText = listboxmonth.SelectedItem.Text;
                string MonthStr = "", MonthStr1 = "", MonthStrvalue = "", MonthStrvalue1 = "";

                foreach (ListItem item in listboxmonth.Items)
                {
                    if (item.Selected)
                    {
                        MonthStr1 += item.Text + ",";

                    }
                }
                MonthStr = MonthStr1.TrimStart(',').TrimEnd(',');


                foreach (ListItem item in listboxmonth.Items)
                {
                    if (item.Selected)
                    {
                        MonthStrvalue1 += item.Value + ",";

                    }
                }
                MonthStrvalue = MonthStrvalue1.TrimStart(',').TrimEnd(',');


                //string IDStr = "", IDStr1 = "";

                //foreach (TreeNode node in trview.CheckedNodes)
                //{
                //    IDStr1 = node.Value;
                //    {
                //        IDStr += node.Value + ",";
                //    }
                //}
                //IDStr1 = IDStr.TrimStart(',').TrimEnd(',');

                if (lnkRegion != null)
                {
                    //DataTable d2 = Settings.FindunderUsers(IDStr1);
                    //DataView dv2 = new DataView(d2);
                    //dv2.RowFilter = "RoleType='RegionHead'";
                    //DataTable gauravDT1 = dv2.ToTable();
                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    decimal sumvalueRegion = 0;
                    string smIDStr = "";
                    string DistIDStr = "";
                    //for (int i = 0; i < gauravDT1.Rows.Count; i++)
                    //{
                    //    smIDStr += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                    //}
                    //smIDStr = smIDStr.TrimStart(',').TrimEnd(',');


                    //if(smIDStr != "")


                    DistIDStr = Request.Form[hiddistributor.UniqueID];

                    if (DistIDStr != "")
                    {
                        //string s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and  AssignedByID=" + Settings.Instance.UserID + " and SalesYear='" + txtcurrentyear.SelectedItem.Text + "'";
                        string s = "";
                        if (targetRadioButtonList.SelectedValue=="Amount")
                        {
                            s = @"select * from SalesTargetFromHODist Where PartyTypeId=" + hid.Value + " and DistID in (" + DistIDStr + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";    
                        }
                        else
                        {
                            s = @"select * from SalesTargetFromHODist Where PartyTypeId=" + hid.Value + " and DistID in (" + DistIDStr + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "' and Unit='" + ddlunit.SelectedItem.Text + "'";
                        }
                        

                        DataTable dtr2 = new DataTable();
                        dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                        if (dtr2.Rows.Count > 0)
                        {
                            try
                            {
                                try
                                {
                                    sum1 = dtr2.Compute("Sum(JanValue)", "");
                                }
                                catch
                                {

                                }
                                try
                                {
                                    sum2 = dtr2.Compute("Sum(FebValue)", "");
                                }
                                catch { }

                                try
                                {
                                    sum3 = dtr2.Compute("Sum(MarValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum4 = dtr2.Compute("Sum(AprValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum5 = dtr2.Compute("Sum(MayValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum6 = dtr2.Compute("Sum(JunValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum7 = dtr2.Compute("Sum(JulValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum8 = dtr2.Compute("Sum(AugValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum9 = dtr2.Compute("Sum(SepValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum10 = dtr2.Compute("Sum(OctValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum11 = dtr2.Compute("Sum(NovValue)", "");
                                }
                                catch { }
                                try
                                {
                                    sum12 = dtr2.Compute("Sum(DecValue)", "");
                                }
                                catch { }
                                if (MonthStr != "")
                                {
                                    if (MonthStr.Contains("Jan"))
                                    { sumvalueRegion = Convert.ToDecimal(sum1); }
                                    if (MonthStr.Contains("Feb"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum2); }
                                    if (MonthStr.Contains("Mar"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum3); }
                                    if (MonthStr.Contains("Apr"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum4); }
                                    if (MonthStr.Contains("May"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum5); }
                                    if (MonthStr.Contains("Jun"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum6); }
                                    if (MonthStr.Contains("Jul"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum7); }
                                    if (MonthStr.Contains("Aug"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum8); }
                                    if (MonthStr.Contains("Sep"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum9); }
                                    if (MonthStr.Contains("Oct"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum10); }
                                    if (MonthStr.Contains("Nov"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum11); }
                                    if (MonthStr.Contains("Dec"))
                                    { sumvalueRegion = sumvalueRegion + Convert.ToDecimal(sum12); }
                                    decimal df = Convert.ToDecimal(sumvalueRegion);
                                    lnkRegion.Text = df.ToString();
                                }
                                else
                                {
                                    decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                                    lnkRegion.Text = df.ToString();
                                }

                                //   decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                                //lnkRegion.Text = df.ToString();
                            }
                            catch
                            {
                                lnkRegion.Text = "0";
                            }
                        }
                        else
                        {
                            lnkRegion.Text = "0";
                        }

                    }
                    else
                    {
                        lnkRegion.Text = "0";
                    }
                    //DataTable d1 = Settings.UnderUsers(Settings.Instance.SMID);
                    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                    //DataTable d1 = Settings.FindunderUsers(IDStr1);
                    //DataView dv1 = new DataView(d1);
                    ////dv1.RowFilter = "RoleType='RegionHead'";
                    //DataTable gauravDT = dv1.ToTable();
                    //string smIDStr1 = "";
                    //for (int i = 0; i < gauravDT.Rows.Count; i++)
                    //{
                    //    smIDStr1 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                    //}
                    //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                    ////string s1 = @"select * from TransDistInv where VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId in (" + smIDStr1 + ") ";
                    //FDisId = BindDistributorDDl(smIDStr1);
                    //FDisId = FDisId.TrimStart(',').TrimEnd(',');
                    //if (smIDStr1 != "")
                    //{
                    if (MonthStrvalue != "")
                    {
                        string s1 = "";
                        //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                            s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                            s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                            s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        else
                            s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        DataTable dtr21 = new DataTable();
                        dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        try
                        {
                            lnkRegion1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        // string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + "";
                        ////  string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + "";
                        string s1 = "";
                        //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                        if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                            s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.underid=" + hid.Value + " ";
                        else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                            s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.underid=" + hid.Value + " ";
                        else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                            s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.itemid=" + hid.Value + "";
                        else
                            s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + DistIDStr + ")  and mi.itemid=" + hid.Value + " ";
                        DataTable dtr21 = new DataTable();
                        dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                        try
                        {
                            lnkRegion1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                        }
                        catch
                        {

                        }
                    }
                    //   }
                }
                //if (lnkState != null)
                //{
                //    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d2 = Settings.FindunderUsers(IDStr1);
                //    DataView dv2 = new DataView(d2);
                //    dv2.RowFilter = "RoleType='StateHead'";
                //    DataTable gauravDT1 = dv2.ToTable();
                //    object sum011 = 0; object sum21 = 0;
                //    object sum31 = 0;
                //    object sum41 = 0;
                //    object sum51 = 0;
                //    object sum61 = 0;
                //    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                //    decimal sumvalueState = 0;
                //    string smIDStr = "";
                //    string smIDStr1 = "";
                //    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                //    {
                //        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                //    string s = "";
                //    if (smIDStr1 != "")
                //    {
                //        s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and SmId in (" + smIDStr1 + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";
                //        //}


                //        DataTable dtr2 = new DataTable();
                //        try
                //        {
                //            dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                //        }
                //        catch { }
                //        if (dtr2.Rows.Count > 0)
                //        {
                //            try
                //            {
                //                try
                //                {
                //                    sum011 = dtr2.Compute("Sum(JanValue)", "");
                //                }
                //                catch
                //                {

                //                }
                //                try
                //                {
                //                    sum21 = dtr2.Compute("Sum(FebValue)", "");
                //                }
                //                catch { }

                //                try
                //                {
                //                    sum31 = dtr2.Compute("Sum(MarValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum41 = dtr2.Compute("Sum(AprValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum51 = dtr2.Compute("Sum(MayValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum61 = dtr2.Compute("Sum(JunValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum71 = dtr2.Compute("Sum(JulValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum81 = dtr2.Compute("Sum(AugValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum91 = dtr2.Compute("Sum(SepValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum101 = dtr2.Compute("Sum(OctValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum111 = dtr2.Compute("Sum(NovValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum121 = dtr2.Compute("Sum(DecValue)", "");
                //                }
                //                catch { }
                //                if (MonthStr != "")
                //                {
                //                    if (MonthStr.Contains("Jan"))
                //                    { sumvalueState = Convert.ToDecimal(sum011); }
                //                    if (MonthStr.Contains("Feb"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum21); }
                //                    if (MonthStr.Contains("Mar"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum31); }
                //                    if (MonthStr.Contains("Apr"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum41); }
                //                    if (MonthStr.Contains("May"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum51); }
                //                    if (MonthStr.Contains("Jun"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum61); }
                //                    if (MonthStr.Contains("Jul"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum71); }
                //                    if (MonthStr.Contains("Aug"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum81); }
                //                    if (MonthStr.Contains("Sep"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum91); }
                //                    if (MonthStr.Contains("Oct"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum101); }
                //                    if (MonthStr.Contains("Nov"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum111); }
                //                    if (MonthStr.Contains("Dec"))
                //                    { sumvalueState = sumvalueState + Convert.ToDecimal(sum121); }
                //                    decimal df = Convert.ToDecimal(sumvalueState);
                //                    lnkState.Text = df.ToString();
                //                }
                //                else
                //                {
                //                    decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                //                    lnkState.Text = df.ToString();
                //                }
                //            }
                //            catch
                //            {
                //                lnkState.Text = "0";
                //            }

                //        }
                //        else
                //        {
                //            //     lnkState.Text = "Target";
                //            lnkState.Text = "0";
                //        }
                //    }
                //    else
                //    {
                //        //     lnkState.Text = "Target";
                //        lnkState.Text = "0";
                //    }

                //    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d1 = Settings.FindunderUsers(IDStr1);
                //    DataView dv1 = new DataView(d1);
                //    //dv1.RowFilter = "RoleType='StateHead'";
                //    dv1.RowFilter = "RoleType='StateHead' or RoleType='DistrictHead' or RoleType='CityHead' or RoleType = 'AreaIncharge'";
                //    DataTable gauravDT = dv1.ToTable();
                //    string smIDStr2 = "";
                //    for (int i = 0; i < gauravDT.Rows.Count; i++)
                //    {
                //        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                //    FDisId = BindDistributorDDl(smIDStr2);
                //    FDisId = FDisId.TrimStart(',').TrimEnd(',');
                //    if (smIDStr2 != "")
                //    {
                //        if (MonthStrvalue != "")
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            ////string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";

                //            string s1 = "";
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";

                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkState1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }
                //            catch
                //            {

                //            }

                //        }
                //        else
                //        {
                //            smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ") and mi.underid=" + hid.Value + "";
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ") and mi.underid=" + hid.Value + "";

                //            string s1 = "";
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";

                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkState1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }
                //            catch
                //            {

                //            }
                //        }
                //    }
                //}

                //if (lnkDistrict != null)
                //{
                //    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d2 = Settings.FindunderUsers(IDStr1);
                //    DataView dv2 = new DataView(d2);
                //    dv2.RowFilter = "RoleType='DistrictHead'";

                //    DataTable gauravDT1 = dv2.ToTable();
                //    object sum011 = 0; object sum21 = 0;
                //    object sum31 = 0;
                //    object sum41 = 0;
                //    object sum51 = 0;
                //    object sum61 = 0;
                //    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                //    string smIDStr = "";
                //    string smIDStr1 = "";
                //    decimal sumvalueDistrict = 0;
                //    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                //    {
                //        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                //    if (smIDStr1 != "")
                //    {
                //        string s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and SMId in (" + smIDStr1 + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";
                //        DataTable dtr2 = new DataTable();
                //        try
                //        {
                //            dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                //        }
                //        catch { }
                //        if (dtr2.Rows.Count > 0)
                //        {
                //            try
                //            {

                //                try
                //                {
                //                    sum011 = dtr2.Compute("Sum(JanValue)", "");
                //                }
                //                catch
                //                {

                //                }
                //                try
                //                {
                //                    sum21 = dtr2.Compute("Sum(FebValue)", "");
                //                }
                //                catch { }

                //                try
                //                {
                //                    sum31 = dtr2.Compute("Sum(MarValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum41 = dtr2.Compute("Sum(AprValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum51 = dtr2.Compute("Sum(MayValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum61 = dtr2.Compute("Sum(JunValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum71 = dtr2.Compute("Sum(JulValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum81 = dtr2.Compute("Sum(AugValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum91 = dtr2.Compute("Sum(SepValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum101 = dtr2.Compute("Sum(OctValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum111 = dtr2.Compute("Sum(NovValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum121 = dtr2.Compute("Sum(DecValue)", "");
                //                }
                //                catch { }
                //                if (MonthStr != "")
                //                {
                //                    if (MonthStr.Contains("Jan"))
                //                    { sumvalueDistrict = Convert.ToDecimal(sum011); }
                //                    if (MonthStr.Contains("Feb"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum21); }
                //                    if (MonthStr.Contains("Mar"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum31); }
                //                    if (MonthStr.Contains("Apr"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum41); }
                //                    if (MonthStr.Contains("May"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum51); }
                //                    if (MonthStr.Contains("Jun"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum61); }
                //                    if (MonthStr.Contains("Jul"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum71); }
                //                    if (MonthStr.Contains("Aug"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum81); }
                //                    if (MonthStr.Contains("Sep"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum91); }
                //                    if (MonthStr.Contains("Oct"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum101); }
                //                    if (MonthStr.Contains("Nov"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum111); }
                //                    if (MonthStr.Contains("Dec"))
                //                    { sumvalueDistrict = sumvalueDistrict + Convert.ToDecimal(sum121); }
                //                    decimal df = Convert.ToDecimal(sumvalueDistrict);
                //                    lnkDistrict.Text = df.ToString();
                //                }
                //                else
                //                {
                //                    decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                //                    lnkDistrict.Text = df.ToString();
                //                }
                //            }
                //            catch
                //            {
                //                lnkDistrict.Text = "0";
                //            }

                //        }
                //        else
                //        {
                //            //   lnkDistrict.Text = "Target";
                //            lnkDistrict.Text = "0";
                //        }
                //    }
                //    else
                //    {
                //        //   lnkDistrict.Text = "Target";
                //        lnkDistrict.Text = "0";
                //    }

                //    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d1 = Settings.FindunderUsers(IDStr1);
                //    DataView dv1 = new DataView(d1);
                //    //dv1.RowFilter = "RoleType='DistrictHead'";
                //    dv1.RowFilter = "RoleType='DistrictHead' or RoleType='CityHead' or RoleType = 'AreaIncharge'";
                //    DataTable gauravDT = dv1.ToTable();
                //    string smIDStr2 = "";
                //    for (int i = 0; i < gauravDT.Rows.Count; i++)
                //    {
                //        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                //    FDisId = BindDistributorDDl(smIDStr2);
                //    FDisId = FDisId.TrimStart(',').TrimEnd(',');
                //    if (smIDStr2 != "")
                //    {
                //        if (MonthStrvalue != "")
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            //// string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";

                //            string s1 = "";
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr1 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";


                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkDistrict1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }
                //            catch
                //            {

                //            }

                //        }
                //        else
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ") and mi.underid=" + hid.Value + "";
                //            string s1 = "";

                //            ////@"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ") and mi.underid=" + hid.Value + "";


                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";


                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkDistrict1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }

                //            catch
                //            {

                //            }
                //        }
                //    }
                //}

                //// City ///

                //if (lnkCity != null)
                //{
                //    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d2 = Settings.FindunderUsers(IDStr1);
                //    DataView dv2 = new DataView(d2);
                //    dv2.RowFilter = "RoleType='CityHead'";

                //    DataTable gauravDT1 = dv2.ToTable();
                //    object sum011 = 0; object sum21 = 0;
                //    object sum31 = 0;
                //    object sum41 = 0;
                //    object sum51 = 0;
                //    object sum61 = 0;
                //    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                //    decimal sumvalueCity = 0;
                //    //  string smIDStr = "";
                //    string smIDStr1 = "";
                //    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                //    {
                //        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                //    if (smIDStr1 != "")
                //    {
                //        string s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and SMId in (" + smIDStr1 + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";
                //        DataTable dtr2 = new DataTable();
                //        try
                //        {
                //            dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                //        }
                //        catch { }
                //        if (dtr2.Rows.Count > 0)
                //        {
                //            try
                //            {

                //                try
                //                {
                //                    sum011 = dtr2.Compute("Sum(JanValue)", "");
                //                }
                //                catch
                //                {

                //                }
                //                try
                //                {
                //                    sum21 = dtr2.Compute("Sum(FebValue)", "");
                //                }
                //                catch { }

                //                try
                //                {
                //                    sum31 = dtr2.Compute("Sum(MarValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum41 = dtr2.Compute("Sum(AprValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum51 = dtr2.Compute("Sum(MayValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum61 = dtr2.Compute("Sum(JunValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum71 = dtr2.Compute("Sum(JulValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum81 = dtr2.Compute("Sum(AugValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum91 = dtr2.Compute("Sum(SepValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum101 = dtr2.Compute("Sum(OctValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum111 = dtr2.Compute("Sum(NovValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum121 = dtr2.Compute("Sum(DecValue)", "");
                //                }
                //                catch { }
                //                if (MonthStr != "")
                //                {
                //                    if (MonthStr.Contains("Jan"))
                //                    { sumvalueCity = Convert.ToDecimal(sum011); }
                //                    if (MonthStr.Contains("Feb"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum21); }
                //                    if (MonthStr.Contains("Mar"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum31); }
                //                    if (MonthStr.Contains("Apr"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum41); }
                //                    if (MonthStr.Contains("May"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum51); }
                //                    if (MonthStr.Contains("Jun"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum61); }
                //                    if (MonthStr.Contains("Jul"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum71); }
                //                    if (MonthStr.Contains("Aug"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum81); }
                //                    if (MonthStr.Contains("Sep"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum91); }
                //                    if (MonthStr.Contains("Oct"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum101); }
                //                    if (MonthStr.Contains("Nov"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum111); }
                //                    if (MonthStr.Contains("Dec"))
                //                    { sumvalueCity = sumvalueCity + Convert.ToDecimal(sum121); }
                //                    decimal df = Convert.ToDecimal(sumvalueCity);
                //                    lnkCity.Text = df.ToString();
                //                }
                //                else
                //                {
                //                    decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                //                    lnkCity.Text = df.ToString();
                //                }
                //            }
                //            catch
                //            {
                //                lnkCity.Text = "0";
                //            }

                //        }
                //        else
                //        {
                //            //     lnkCity.Text = "Target";
                //            lnkCity.Text = "0";
                //        }
                //    }
                //    else
                //    {
                //        //     lnkCity.Text = "Target";
                //        lnkCity.Text = "0";
                //    }
                //    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d1 = Settings.FindunderUsers(IDStr1);
                //    DataView dv1 = new DataView(d1);
                //    DataView checkdv1 = new DataView(d1);
                //    checkdv1.RowFilter = "RoleType='CityHead'";
                //    DataTable priDT = checkdv1.ToTable();
                //    if (priDT.Rows.Count > 0)
                //    {
                //        dv1.RowFilter = "RoleType='CityHead' or RoleType = 'AreaIncharge'";
                //    }
                //    else
                //    {
                //        dv1.RowFilter = "RoleType='CityHead'";
                //    }
                //    DataTable gauravDT = dv1.ToTable();
                //    string smIDStr2 = "";
                //    for (int i = 0; i < gauravDT.Rows.Count; i++)
                //    {
                //        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                //    FDisId = BindDistributorDDl(smIDStr2);
                //    FDisId = FDisId.TrimStart(',').TrimEnd(',');
                //    if (smIDStr2 != "")
                //    {
                //        if (MonthStrvalue != "")
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            //// string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            string s1 = "";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";


                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkCity1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }
                //            catch
                //            {

                //            }

                //        }
                //        else
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ") and mi.underid=" + hid.Value + "";
                //            /////   string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ") and mi.underid=" + hid.Value + "";

                //            string s1 = "";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + "";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";


                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkCity1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }

                //            catch
                //            {

                //            }
                //        }
                //    }
                //}

                ///////////////////////////

                //if (lnkArea != null)
                //{
                //    //DataTable d2 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d2 = Settings.FindunderUsers(IDStr1);
                //    DataView dv2 = new DataView(d2);
                //    dv2.RowFilter = "RoleType='AreaIncharge'";

                //    DataTable gauravDT1 = dv2.ToTable();
                //    object sum011 = 0; object sum21 = 0;
                //    object sum31 = 0;
                //    object sum41 = 0;
                //    object sum51 = 0;
                //    object sum61 = 0;
                //    object sum71 = 0; object sum81 = 0; object sum91 = 0; object sum101 = 0; object sum111 = 0; object sum121 = 0;
                //    decimal sumvalueArea = 0;
                //    //  string smIDStr = "";
                //    string smIDStr1 = "";
                //    for (int i = 0; i < gauravDT1.Rows.Count; i++)
                //    {
                //        smIDStr1 += gauravDT1.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                //    if (smIDStr1 != "")
                //    {
                //        //string s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and  SMId in (" + smIDStr1 + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "'";
                //        string s = @"select * from SalesTragetFromHO Where PartyTypeId=" + hid.Value + " and  SMId in (" + smIDStr1 + ") and SalesYear='" + txtcurrentyear.SelectedItem.Text + "' and Isnull(Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";
                //        DataTable dtr2 = new DataTable();
                //        try
                //        {
                //            dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                //        }
                //        catch { }
                //        if (dtr2.Rows.Count > 0)
                //        {
                //            try
                //            {

                //                try
                //                {
                //                    sum011 = dtr2.Compute("Sum(JanValue)", "");
                //                }
                //                catch
                //                {

                //                }
                //                try
                //                {
                //                    sum21 = dtr2.Compute("Sum(FebValue)", "");
                //                }
                //                catch { }

                //                try
                //                {
                //                    sum31 = dtr2.Compute("Sum(MarValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum41 = dtr2.Compute("Sum(AprValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum51 = dtr2.Compute("Sum(MayValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum61 = dtr2.Compute("Sum(JunValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum71 = dtr2.Compute("Sum(JulValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum81 = dtr2.Compute("Sum(AugValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum91 = dtr2.Compute("Sum(SepValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum101 = dtr2.Compute("Sum(OctValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum111 = dtr2.Compute("Sum(NovValue)", "");
                //                }
                //                catch { }
                //                try
                //                {
                //                    sum121 = dtr2.Compute("Sum(DecValue)", "");
                //                }
                //                catch { }
                //                if (MonthStr != "")
                //                {
                //                    if (MonthStr.Contains("Jan"))
                //                    { sumvalueArea = Convert.ToDecimal(sum011); }
                //                    if (MonthStr.Contains("Feb"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum21); }
                //                    if (MonthStr.Contains("Mar"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum31); }
                //                    if (MonthStr.Contains("Apr"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum41); }
                //                    if (MonthStr.Contains("May"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum51); }
                //                    if (MonthStr.Contains("Jun"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum61); }
                //                    if (MonthStr.Contains("Jul"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum71); }
                //                    if (MonthStr.Contains("Aug"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum81); }
                //                    if (MonthStr.Contains("Sep"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum91); }
                //                    if (MonthStr.Contains("Oct"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum101); }
                //                    if (MonthStr.Contains("Nov"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum111); }
                //                    if (MonthStr.Contains("Dec"))
                //                    { sumvalueArea = sumvalueArea + Convert.ToDecimal(sum121); }
                //                    decimal df = Convert.ToDecimal(sumvalueArea);
                //                    lnkArea.Text = df.ToString();
                //                }
                //                else
                //                {
                //                    decimal df = Convert.ToDecimal(sum011) + Convert.ToDecimal(sum21) + Convert.ToDecimal(sum31) + Convert.ToDecimal(sum41) + Convert.ToDecimal(sum51) + Convert.ToDecimal(sum61) + Convert.ToDecimal(sum71) + Convert.ToDecimal(sum81) + Convert.ToDecimal(sum91) + Convert.ToDecimal(sum101) + Convert.ToDecimal(sum111) + Convert.ToDecimal(sum121);
                //                    lnkArea.Text = df.ToString();
                //                }
                //            }
                //            catch
                //            {
                //                lnkArea.Text = "0";
                //            }

                //        }
                //        else
                //        {
                //            //   lnkArea.Text = "Target";
                //            lnkArea.Text = "0";
                //        }
                //    }
                //    else
                //    {
                //        //   lnkArea.Text = "Target";
                //        lnkArea.Text = "0";
                //    }


                //    //DataTable d1 = Settings.UnderUsers(Settings.DMInt32(ddlunderUser.SelectedValue).ToString());
                //    DataTable d1 = Settings.FindunderUsers(IDStr1);
                //    DataView dv1 = new DataView(d1);
                //    //dv1.RowFilter = "RoleType='AreaIncharge' or RoleType = 'DistrictHead'";
                //    dv1.RowFilter = "RoleType='AreaIncharge'";
                //    DataTable gauravDT = dv1.ToTable();
                //    string smIDStr2 = "";
                //    for (int i = 0; i < gauravDT.Rows.Count; i++)
                //    {
                //        smIDStr2 += gauravDT.Rows[i]["SMId"].ToString() + ",";

                //    }
                //    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                //    FDisId = BindDistributorDDl12(smIDStr2);
                //    FDisId = FDisId.TrimStart(',').TrimEnd(',');
                //    if (smIDStr2 != "")
                //    {
                //        if (MonthStrvalue != "")
                //        {
                //            // string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            ////  string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";

                //            string s1 = "";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " and month(t1.Vdate) in (" + MonthStrvalue + ")";


                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkArea1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }
                //            catch
                //            {

                //            }

                //        }
                //        else
                //        {
                //            //string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.SMId in (" + smIDStr2 + ") and mi.underid=" + hid.Value + "";
                //            //// string s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ") and mi.underid=" + hid.Value + "";
                //            string s1 = "";
                //            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + "";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.underid=" + hid.Value + " ";
                //            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Quantity")
                //                s1 = @"select Round(Sum(T1.Qty)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";
                //            else
                //                s1 = @"select Round(Sum(T1.Amount)/100000,2)  AS Net_Total from TransDistInv1 AS T1 LEFT JOIN TransDistInv AS T2 ON T1.DistInvId=T2.DistInvId left join mastitem as mi on mi.itemid=t1.itemid where T2.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and T2.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and T2.DistId in (" + FDisId + ")  and mi.itemid=" + hid.Value + " ";




                //            DataTable dtr21 = new DataTable();
                //            dtr21 = DbConnectionDAL.GetDataTable(CommandType.Text, s1);
                //            try
                //            {
                //                lnkArea1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dtr21.Rows[0]["Net_Total"]), 2));
                //            }

                //            catch
                //            {

                //            }
                //        }
                //    }

                //}
                //Label lk = (Label)e.Row.FindControl("lblActual");
                //HiddenField lk1 = (HiddenField)e.Row.FindControl("hid");
                //string s1 = "select SUM(BillAmount) from TransDistInv where DistId=" + lk1.Value + " and VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "'";
                //try
                //{
                //    string s = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, s1));
                //    lk.Text = s;
                //}
                //catch { }
            }

        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            string strtarget = "";
            string stractual = "";

            string strcomplete = "";
            string strtargetcondition = "";

            string strcondition = "";
            if (ddlunit.SelectedIndex>0)
            {
                strtargetcondition = " SH.DistID in (" + Request.Form[hiddistributor.UniqueID] + ") and SH.SalesYear='" + txtcurrentyear.SelectedValue + "' and Isnull(SH.Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(SH.TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "'";
            }
            else
            {
                strtargetcondition = " SH.DistID in (" + Request.Form[hiddistributor.UniqueID] + ") and SH.SalesYear='" + txtcurrentyear.SelectedValue + "' and Isnull(SH.Itemtype,'Group')='" + itemgroupRadioButtonlst.SelectedValue + "' and Isnull(SH.TargetFor,'Amount')='" + targetRadioButtonList.SelectedValue + "' and SH.unit ='" + ddlunit.SelectedItem.Text + "'";
            }

            if (ddlunit.SelectedIndex > 0)
            {

                strcondition = "  T1.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedValue) + "' and T1.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedValue) + "' and T1.DistId in (" + Request.Form[hiddistributor.UniqueID] + ") ";
            }
            else
            {
                strcondition = "  T1.VDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedValue) + "' and T1.VDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedValue) + "' and T1.DistId in (" + Request.Form[hiddistributor.UniqueID] + ")  and mi.unit ='" + ddlunit.SelectedItem.Text + "'";
            }

            strtarget = "SELECT SH.DistID,MP1.PartyName AS PartyName,MI.ItemId,MI.ItemName AS ProductGroup,'Target' AS Type,AprValue, MayValue, JunValue,";
            strtarget = strtarget + " JulValue, AugValue, SepValue, OctValue, NovValue, DecValue, JanValue, FebValue, MarValue,  0 AS Apr1,   0 AS May1,    0 AS Jun1,   0 AS Jul1,  0 AS Aug1,    0 AS Sep1,   0 AS Oct1,    0 AS Nov1,    0 AS Dec1,      0 AS Jan1,   0 AS Feb1,    0 AS Mar1";
            strtarget = strtarget + "  FROM SalesTargetFromHODist SH LEFT JOIN MastItem MI ON MI.ItemId=SH.PartyTypeId LEFT JOIN MastParty MP1 ON MP1.PartyId=SH.DistID where  " + strtargetcondition + " ";



            if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Quantity")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.Underid) AS itemid,Max(MI1.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Qty),0) AS Qty, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid Left join mastitem as mi1 on mi1.itemid=mi.Underid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.Underid) source ";
                stractual = stractual + " PIVOT (    SUM(Qty)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            else if (itemgroupRadioButtonlst.SelectedValue == "Group" && targetRadioButtonList.SelectedValue == "Amount")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.Underid) AS itemid,Max(MI1.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Amount),0) AS Amount, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid Left join mastitem as mi1 on mi1.itemid=mi.Underid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.Underid) source ";
                stractual = stractual + " PIVOT (    SUM(Amount)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            else if (itemgroupRadioButtonlst.SelectedValue == "Item" && targetRadioButtonList.SelectedValue == "Amount")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.itemid) AS itemid,Max(MI.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Amount),0) AS Amount, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.itemid) source ";
                stractual = stractual + " PIVOT (    SUM(Amount)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }

            else
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.itemid) AS itemid,Max(MI.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Qty),0) AS Qty, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.itemid) source ";
                stractual = stractual + " PIVOT (    SUM(Qty)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            strcomplete = " SELECT Max(T.PartyName) AS PartyName, Max(ProductGroup) AS Productgroup ,";
            strcomplete = strcomplete + " CAST(Sum(T.Apr1) AS VARCHAR) +'/'+ CAST(Sum(T.AprValue) AS VARCHAR) AS Apr,";
            strcomplete = strcomplete + " CAST(Sum(T.May1) AS VARCHAR) +'/'+ CAST(Sum(T.MayValue) AS VARCHAR) AS May,";
            strcomplete = strcomplete + " CAST(Sum(T.Jun1) AS VARCHAR) +'/'+ CAST(Sum(T.JunValue) AS VARCHAR) AS Jun,";
            strcomplete = strcomplete + " CAST(Sum(T.Jul1) AS VARCHAR) +'/'+ CAST(Sum(T.JulValue) AS VARCHAR) AS Jul,";
            strcomplete = strcomplete + " CAST(Sum(T.Aug1) AS VARCHAR) +'/'+ CAST(Sum(T.AugValue) AS VARCHAR) AS Aug,";
            strcomplete = strcomplete + " CAST(Sum(T.Sep1) AS VARCHAR) +'/'+ CAST(Sum(T.SepValue) AS VARCHAR) AS Sep,";
            strcomplete = strcomplete + " CAST(Sum(T.Oct1) AS VARCHAR) +'/'+ CAST(Sum(T.OctValue) AS VARCHAR) AS Oct,";
            strcomplete = strcomplete + " CAST(Sum(T.Nov1) AS VARCHAR) +'/'+ CAST(Sum(T.NovValue) AS VARCHAR) AS Nov,";
            strcomplete = strcomplete + "  CAST(Sum(T.Dec1) AS VARCHAR) +'/'+ CAST(Sum(T.DecValue) AS VARCHAR) AS Dec,";
            strcomplete = strcomplete + "  CAST(Sum(T.Jan1) AS VARCHAR) +'/'+ CAST(Sum(T.JanValue) AS VARCHAR) AS Jan,";
            strcomplete = strcomplete + "  CAST(Sum(T.Feb1) AS VARCHAR) +'/'+ CAST(Sum(T.FebValue) AS VARCHAR) AS Feb,";
            strcomplete = strcomplete + "  CAST(Sum(T.Mar1) AS VARCHAR) +'/'+ CAST(Sum(T.FebValue) AS VARCHAR) AS Mar";
            strcomplete = strcomplete + "    From(" + strtarget + "";
            strcomplete = strcomplete + " UNION ALL ";
            strcomplete = strcomplete + "" + stractual + "  )AS T GROUP BY T.DistID,T.itemid ORDER BY Max(T.PartyName)";



            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, strcomplete);


            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=SalesTargetvsActualtargetfordistributor.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + " ," + "Product".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;


            for (int j = 0; j < dtItem.Rows.Count; j++)
            {
                for (int k = 0; k < dtItem.Columns.Count; k++)
                {


                
                        if (dtItem.Rows[j][k].ToString().Contains(","))
                        {

                            sb.Append(String.Format("\"{0}\"", dtItem.Rows[j][k].ToString()) + ',');

                        }
                        else if (dtItem.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {


                            sb.Append(String.Format("\"{0}\"", dtItem.Rows[j][k].ToString()) + ',');

                        }
                        else
                        {

                            sb.Append(dtItem.Rows[j][k].ToString() + ',');


                        }
                   

                }
                sb.Append(Environment.NewLine);
            }

            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=SalesTargetvsActualtargetfordistributor.csv");
            Response.Write(sb.ToString());
            Response.End();

            //

            //if (grd.Rows.Count > 0)
            //{
            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "TagetVSActualSales.xls"));
            //    //Response.ContentType = "application/ms-excel";

            //    //Added 07-12-2015
            //    Response.AddHeader("content-disposition", "attachment;filename=TagetVSActualSales.xls");
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.ms-excel";
            //    //End

            //    StringWriter sw = new StringWriter();
            //    HtmlTextWriter htw = new HtmlTextWriter(sw);
            //    //grd.AllowPaging = false;
            //    //BindGridview();
            //    //Change the Header Row back to white color
            //    grd.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //    //Applying stlye to gridview header cells
            //    for (int i = 0; i < grd.HeaderRow.Cells.Count; i++)
            //    {
            //        grd.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
            //    }
            //    grd.RenderControl(htw);
            //    Response.Write(sw.ToString());
            //    Response.End();
            //}
        }



        [WebMethod(EnableSession = true)]

        public static string getTargetvsactualacheive(string Distid, string year,string targetfor,string itemtype,string unit)
        {

            string res = "";

            string str = "";
            string strtarget = "";
            string stractual = "";

            string strcomplete = "";
            string strtargetcondition = "";

             string strcondition ="";
            if (unit=="")
            {
                strtargetcondition = " SH.DistID in (" + Distid + ") and SH.SalesYear='" + year + "' and Isnull(SH.Itemtype,'Group')='" + itemtype + "' and Isnull(SH.TargetFor,'Amount')='" + targetfor + "'";    
            }
            else
            {
                strtargetcondition = " SH.DistID in (" + Distid + ") and SH.SalesYear='" + year + "' and Isnull(SH.Itemtype,'Group')='" + itemtype + "' and Isnull(SH.TargetFor,'Amount')='" + targetfor + "' and SH.unit ='" + unit + "'";
            }

            if (unit == "")
            {

                strcondition = "  T1.VDate>='" + Settings.GetStartFinancialYearfrmDate(year) + "' and T1.VDate<='" + Settings.GetStartFinancialYearToDate(year) + "' and T1.DistId in (" + Distid + ") ";
            }
            else
            {
                strcondition = "  T1.VDate>='" + Settings.GetStartFinancialYearfrmDate(year) + "' and T1.VDate<='" + Settings.GetStartFinancialYearToDate(year) + "' and T1.DistId in (" + Distid + ")  and mi.unit ='" + unit + "'";
            }

            strtarget = "SELECT SH.DistID,MP1.PartyName AS PartyName,MI.ItemId,MI.ItemName AS ProductGroup,'Target' AS Type,AprValue, MayValue, JunValue,";
            strtarget = strtarget + " JulValue, AugValue, SepValue, OctValue, NovValue, DecValue, JanValue, FebValue, MarValue,  0 AS Apr1,   0 AS May1,    0 AS Jun1,   0 AS Jul1,  0 AS Aug1,    0 AS Sep1,   0 AS Oct1,    0 AS Nov1,    0 AS Dec1,      0 AS Jan1,   0 AS Feb1,    0 AS Mar1";
            strtarget = strtarget + "  FROM SalesTargetFromHODist SH LEFT JOIN MastItem MI ON MI.ItemId=SH.PartyTypeId LEFT JOIN MastParty MP1 ON MP1.PartyId=SH.DistID where  " + strtargetcondition + " ";



            if (itemtype == "Group" && targetfor == "Quantity")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.Underid) AS itemid,Max(MI1.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Qty),0) AS Qty, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid Left join mastitem as mi1 on mi1.itemid=mi.Underid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.Underid) source ";
                stractual = stractual + " PIVOT (    SUM(Qty)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            else if (itemtype == "Group" && targetfor == "Amount")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.Underid) AS itemid,Max(MI1.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Amount),0) AS Amount, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid Left join mastitem as mi1 on mi1.itemid=mi.Underid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.Underid) source ";
                stractual = stractual + " PIVOT (    SUM(Amount)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            else if (itemtype == "Item" && targetfor == "Amount")
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.itemid) AS itemid,Max(MI.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Amount),0) AS Amount, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.itemid) source ";
                stractual = stractual + " PIVOT (    SUM(Amount)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }

            else
            {
                stractual = "SELECT DistID,PartyName,itemid,ProductGroup,'Actual' AS type,0 As  AprValue, 0 As MayValue,  0 AS JunValue, 0 AS JulValue,  0 AS AugValue, 0 As SepValue,   0 As OctValue,  0 AS NovValue,  0 AS DecValue, ";
                stractual = stractual + " 0 AS JanValue,0 As FebValue,0 As MarValue,Isnull( [4],0) AS Apr1,Isnull( [5],0) AS May1,Isnull([6],0) AS Jun1,Isnull( [7],0) AS Jul1, Isnull( [8],0) AS Aug1,Isnull( [9],0) AS Sep1,Isnull( [10],0) AS Oct1,Isnull( [11],0) AS Nov1,Isnull( [12],0) AS Dec1,Isnull([1],0) AS Jan1,Isnull([2],0) AS Feb1,Isnull([3],0) AS Mar1";
                stractual = stractual + " FROM (select T1.DistID,Year(Max(T1.VDate)) AS year1,Max(MP1.PartyName) AS PartyName,Max(MI.itemid) AS itemid,Max(MI.ItemName) AS ProductGroup,'Actual' AS Type,convert(char(3),Max( T1.VDate), 0) AS Month1,Isnull(Sum(T1.Qty),0) AS Qty, Month(T1.VDate) AS month from TransDistInv1 AS T1  LEFT JOIN MastParty MP1 ON MP1.PartyId= T1.DistID";
                stractual = stractual + " Left join mastitem as mi on mi.itemid=t1.itemid  where " + strcondition + "   GROUP BY T1.distID, Month(T1.VDate),mi.itemid) source ";
                stractual = stractual + " PIVOT (    SUM(Qty)    FOR month    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  ) AS pvtMonth";
            }
            strcomplete = " SELECT Max(T.itemid) AS DistId,Max(T.DistId) AS DistID,Max(T.PartyName) AS PartyName, Max(ProductGroup) AS Productgroup ,";
            strcomplete = strcomplete + " CAST(Sum(T.Apr1) AS VARCHAR) +'/'+ CAST(Sum(T.AprValue) AS VARCHAR) AS Apr,";
            strcomplete = strcomplete + " CAST(Sum(T.May1) AS VARCHAR) +'/'+ CAST(Sum(T.MayValue) AS VARCHAR) AS May,";
            strcomplete = strcomplete + " CAST(Sum(T.Jun1) AS VARCHAR) +'/'+ CAST(Sum(T.JunValue) AS VARCHAR) AS Jun,";
            strcomplete = strcomplete + " CAST(Sum(T.Jul1) AS VARCHAR) +'/'+ CAST(Sum(T.JulValue) AS VARCHAR) AS Jul,";
            strcomplete = strcomplete + " CAST(Sum(T.Aug1) AS VARCHAR) +'/'+ CAST(Sum(T.AugValue) AS VARCHAR) AS Aug,";
            strcomplete = strcomplete + " CAST(Sum(T.Sep1) AS VARCHAR) +'/'+ CAST(Sum(T.SepValue) AS VARCHAR) AS Sep,";
            strcomplete = strcomplete + " CAST(Sum(T.Oct1) AS VARCHAR) +'/'+ CAST(Sum(T.OctValue) AS VARCHAR) AS Oct,";
            strcomplete = strcomplete + " CAST(Sum(T.Nov1) AS VARCHAR) +'/'+ CAST(Sum(T.NovValue) AS VARCHAR) AS Nov,";
            strcomplete = strcomplete + "  CAST(Sum(T.Dec1) AS VARCHAR) +'/'+ CAST(Sum(T.DecValue) AS VARCHAR) AS Dec,";
            strcomplete = strcomplete + "  CAST(Sum(T.Jan1) AS VARCHAR) +'/'+ CAST(Sum(T.JanValue) AS VARCHAR) AS Jan,";
            strcomplete = strcomplete + "  CAST(Sum(T.Feb1) AS VARCHAR) +'/'+ CAST(Sum(T.FebValue) AS VARCHAR) AS Feb,";
            strcomplete = strcomplete + "  CAST(Sum(T.Mar1) AS VARCHAR) +'/'+ CAST(Sum(T.FebValue) AS VARCHAR) AS Mar";
            strcomplete = strcomplete + "    From(" + strtarget + "";
            strcomplete = strcomplete + " UNION ALL ";
            strcomplete = strcomplete + "" + stractual + "  )AS T GROUP BY T.DistID,T.itemid ORDER BY Max(T.PartyName)";



            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, strcomplete);
            return JsonConvert.SerializeObject(dtItem);            
            
            
            
            return res;

        }
        public class SaleTargetDist
        {
            public string Product { get; set; }
            public string Distributor { get; set; }
            public string Apr { get; set; }
            public string May { get; set; }
            public string Jun { get; set; }
            public string Jul { get; set; }
            public string Aug { get; set; }
            public string Sep { get; set; }
            public string Oct { get; set; }
            public string Nov { get; set; }
            public string Dec { get; set; }
            public string Jan { get; set; }
            public string Feb { get; set; }
            public string Mar { get; set; }

        }

        protected void ListArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matareaStr = "";
            foreach (ListItem matarea in ListArea.Items)
            {
                if (matarea.Selected)
                {
                    matareaStr += matarea.Value + ",";
                }
            }
            matareaStr = matareaStr.TrimStart(',').TrimEnd(',');
            BindDistributorDDl(matareaStr);
        }

        protected void targetRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetRadioButtonList.SelectedIndex >= 0)
            {
                if(targetRadioButtonList.SelectedValue=="Amount")
                {
                    hidunit.Value = string.Empty;
                    BindUnit();
                    divunit.Style.Add("display", "none");
                }
                else
                {
                    divunit.Style.Add("display", "block");
                }
            }
            //else
            //{
            //    lblPartTypeName.Visible = false;
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();
            //    GridView2.DataSource = null;
            //    GridView2.DataBind();
            //}
        }
    }
}