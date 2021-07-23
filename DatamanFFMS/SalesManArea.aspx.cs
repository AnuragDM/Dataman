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
    public partial class SalesManArea : System.Web.UI.Page
    {
        BAL.MastLink.MastLinkBAL ML = new BAL.MastLink.MastLinkBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                BindSalesPersons();
                // BindDDlCountry();
                BindDDLState();
                btnsave.Visible = false;
                btncancel.Visible = false;
            }
        }


        private void BindDDlCountry()
        {
            try
            {
                string countryqry = "select AreaId,AreaName from MastArea where AreaType='COUNTRY' and Active=1  order by AreaName";
                DataTable countrydt = DbConnectionDAL.GetDataTable(CommandType.Text, countryqry);
                if (countrydt.Rows.Count > 0)
                {
                    ddlcountry.DataSource = countrydt;
                    ddlcountry.DataTextField = "AreaName";
                    ddlcountry.DataValueField = "AreaId";
                    ddlcountry.DataBind();
                }
                ddlcountry.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindDDLState()
        {
            try
            {
                string stateqry = "select a1.AreaId,(a1.AreaName + ' - ' + a2.AreaName + ' - ' + a3.AreaName) as AreaName from MastArea a1 left join MastArea a2 on a1.underid=a2.areaid left join MastArea a3 on a2.underid=a3.areaid  where a1.AreaType='State' and a1.Active=1 order by a1.AreaName";
                DataTable statedt = DbConnectionDAL.GetDataTable(CommandType.Text, stateqry);
                if (statedt.Rows.Count > 0)
                {
                    lstState.DataSource = statedt;
                    lstState.DataTextField = "AreaName";
                    lstState.DataValueField = "AreaId";
                    lstState.DataBind();
                }
                ddlcountry.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindSalesPersons()
        {
            //string qry = "select SMID,SMName from MastSalesRep where Active=1  order by SMName";
            string qry = "select SMID,SMName AS SMName from MastSalesRep where smname not in ('.') and Active=1  order by SMName";
            fillDDLDirect(ddlprimecode, qry, "SMID", "SMName", 1);
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            if (xdt.Rows.Count > 0)
            {
                xddl.DataTextField = xtext.Trim();
                xddl.DataValueField = xvalue.Trim();
                xddl.DataBind();
            }
            xddl.Items.Insert(0, new ListItem("--Please select--"));
            //if (sele == 1)
            //{
            //    if (xdt.Rows.Count >= 1)
            //xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            //    else if (xdt.Rows.Count == 0)
            //        xddl.Items.Insert(0, new ListItem("---", "0"));
            //}
            //else if (sele == 2)
            //{
            //    xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            //}
            xdt.Dispose();
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            string areaIDStr = ""; string strdelete = "";
            //if (ddlcountry.SelectedIndex <= 0)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select country !');", true);
            //    return;
            //}
            foreach (RepeaterItem item in rpt.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkItem");
                //HiddenField areaId = (HiddenField)item.FindControl("areaIdHiddenField");
                Label areaId = (Label)item.FindControl("lblHF");
                if (chk.Checked == true)
                {
                    ML.Insert(Convert.ToInt32(ddlprimecode.SelectedValue), Convert.ToInt32(areaId.Text));
                }
                else
                {
                    strdelete = "delete from mastlink where Ecode='SA' and PrimCode=" + ddlprimecode.SelectedValue + " and Linkcode=" + Convert.ToInt32(areaId.Text) + "";
                    DbConnectionDAL.ExecuteQuery(strdelete);
                }
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Sucessfully!');", true);
            ddlprimecode.SelectedIndex = 0;
            ClearControls();
        }

        private void ClearControls()
        {
            rpt.DataSource = null;
            rpt.DataBind();
            btnsave.Visible = false;
            btncancel.Visible = false;
            rpt.Visible = false;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesManArea.aspx");
        }

        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcountry.SelectedIndex > 0)
            {
                int area = Convert.ToInt32(ddlcountry.SelectedValue);

                string regQry = @"select * from MastArea where AreaType='Region' and Active=1 and UnderId=" + area + " order by AreaName";
                DataTable regiondt = DbConnectionDAL.GetDataTable(CommandType.Text, regQry);
                if (regiondt.Rows.Count > 0)
                {
                    ddlregion.DataSource = regiondt;
                    ddlregion.DataTextField = "AreaName";
                    ddlregion.DataValueField = "AreaId";
                    ddlregion.DataBind();
                }
                ddlregion.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ddlregion.Items.Clear();
                lstState.Items.Clear();
                rpt.DataSource = null;
                rpt.DataBind();
                btnsave.Visible = false;
                btncancel.Visible = false;
            }
        }

        protected void ddlregion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlregion.SelectedIndex > 0)
            {
                int region = Convert.ToInt32(ddlregion.SelectedValue);

                string regQry = @"select * from MastArea where AreaType='State' and Active=1 and UnderId=" + region + " order by AreaName";
                DataTable regiondt = DbConnectionDAL.GetDataTable(CommandType.Text, regQry);
                if (regiondt.Rows.Count > 0)
                {
                    lstState.DataSource = regiondt;
                    lstState.DataTextField = "AreaName";
                    lstState.DataValueField = "AreaId";
                    lstState.DataBind();
                }

            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
                btnsave.Visible = false;
                btncancel.Visible = false;
            }
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            if (ddlstate.SelectedIndex > 0)
            //            {
            //                int state = Convert.ToInt32(ddlstate.SelectedValue);

            //                //                string areaQry = @"select mag.*,ma.AreaName,ma.AreaType,ma.UnderId from MastAreaGrp mag left join MastArea ma on mag.AreaId=ma.AreaId  
            //                //              where MainGrp=" + state + " and ma.AreaType='Area' and ma.Active=1";

            //                string areaQry = @"  SELECT       distinct ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            //                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            //                         WHERE        (country.AreaType = 'country' and state.AreaId=" + state + ") order by districtName,cityName,areaName";

            //                DataTable areaStatedt = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry);

            //                if (areaStatedt.Rows.Count > 0)
            //                {
            //                    mainDiv.Style.Add("display", "block");
            //                    gvPartyData.DataSource = areaStatedt;
            //                    gvPartyData.DataBind();
            //                }
            //                else
            //                {
            //                    mainDiv.Style.Add("display", "block");
            //                    gvPartyData.DataSource = areaStatedt;
            //                    gvPartyData.DataBind();
            //                }
            //            }
            //            else
            //            {
            //                gvPartyData.DataSource = null;
            //                gvPartyData.DataBind();
            //            }


        }


        protected void gvPartyData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField areaID = (e.Row.FindControl("areaIdHiddenField") as HiddenField);
                var Qry = "select * from mastLink where PrimCode =" + Convert.ToInt32(ddlprimecode.SelectedValue) + " and ECode='SA'";
                DataTable dtini = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                CheckBox chk = (e.Row.FindControl("chkItem") as CheckBox);
                if (dtini.Rows.Count > 0)
                {
                    for (int i = 0; i < dtini.Rows.Count; i++)
                    {
                        if (dtini.Rows[i]["LinkCode"].ToString() == areaID.Value)
                        {
                            chk.Checked = true;
                        }
                    }
                }
            }



            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string cityName = string.Empty; int cityID = 0, underId = 0;
            //    HiddenField areaID = (e.Row.FindControl("areaIdHiddenField") as HiddenField);
            //    HiddenField underID = (e.Row.FindControl("underIdHiddenField") as HiddenField);
            //    string CityNameQry = @"select AreaId,AreaName,UnderId from MastArea where AreaType='City' and Active=1 and AreaId=" + underID.Value + "";
            //    DataTable cityNameDT = DbConnectionDAL.GetDataTable(CommandType.Text, CityNameQry);
            //    if (cityNameDT.Rows.Count > 0)
            //    {
            //        cityName = cityNameDT.Rows[0]["AreaName"].ToString();
            //        cityID = Convert.ToInt32(cityNameDT.Rows[0]["AreaId"]);
            //        underId = Convert.ToInt32(cityNameDT.Rows[0]["UnderId"]);
            //    }
            //    Label areaLabel = (e.Row.FindControl("cityLabel") as Label);
            //    areaLabel.Text = cityName;

            //    string DistrictNameQry = @"select AreaId,AreaName from MastArea where AreaType='District' and Active=1 and AreaId=" + underId + "";
            //    DataTable districtNameDT = DbConnectionDAL.GetDataTable(CommandType.Text, DistrictNameQry);
            //    if (districtNameDT.Rows.Count > 0)
            //    {
            //        Label districtLabel = (e.Row.FindControl("districtLabel") as Label);
            //        districtLabel.Text = districtNameDT.Rows[0]["AreaName"].ToString();
            //    }

            //}
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField areaID = (e.Item.FindControl("areaIdHiddenField") as HiddenField);
                var Qry = "select * from mastLink where PrimCode =" + Convert.ToInt32(ddlprimecode.SelectedValue) + " and ECode='SA'";
                DataTable dtini = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                CheckBox chk = (e.Item.FindControl("chkItem") as CheckBox);
                if (dtini.Rows.Count > 0)
                {
                    for (int i = 0; i < dtini.Rows.Count; i++)
                    {
                        if (dtini.Rows[i]["LinkCode"].ToString() == areaID.Value)
                        {
                            chk.Checked = true;
                        }
                    }
                }
            }

        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            CheckBox chkAllBox = (CheckBox)item.FindControl("chkAll");
            for (int i = 0; i < rpt.Items.Count; i++)
            {
                CheckBox chk = (CheckBox)rpt.Items[i].FindControl("chkItem");
                if (chkAllBox.Checked)
                {
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
            }
        }

       public enum condition
        {
            lstState, lstCity, lstArea
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string areaQry = "";

            string strStID = "";
            string _whr = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select State.');", true);
                return;
            }

            string strStCity = "";
            foreach (ListItem item in lstCity.Items)
            {
                if (item.Selected)
                {
                    strStCity += item.Value + ",";
                }
            }
            strStCity = strStCity.TrimStart(',').TrimEnd(',');

            string strStArea = "";
            foreach (ListItem item in lstArea.Items)
            {
                if (item.Selected)
                {
                    strStArea += item.Value + ",";
                }
            }
            strStArea = strStArea.TrimStart(',').TrimEnd(',');





            // if (strStID != "" && ddlregion.SelectedIndex > 0 && strStArea != "")           
            // {

            // int region = Convert.ToInt32(ddlregion.SelectedValue);

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and area.AreaId IN (" + strStArea + ") and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";


            //}

            // else if (strStID != "" && ddlregion.SelectedIndex > 0 && strStCity!="")

            //{

            // int region = Convert.ToInt32(ddlregion.SelectedValue);              

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and city.AreaId IN (" + strStCity + ") and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";

            //}
            //else if (strStID != "" && ddlregion.SelectedIndex > 0)

            // {
            //int state = Convert.ToInt32(ddlstate.SelectedValue);
            //int region = Convert.ToInt32(ddlregion.SelectedValue);

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            //                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'
            //                       
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";

            // }
            //            else if (ddlregion.SelectedIndex > 0)
            //            {              
            //                int region = Convert.ToInt32(ddlregion.SelectedValue);

            ////                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            ////                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            ////                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            ////                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            ////                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            ////                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            ////                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            ////                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            ////                         WHERE        (country.AreaType = 'country' and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";


            //            }
            //else
            //{
            //gvPartyData.DataSource = null;
            //gvPartyData.DataBind();
            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            //                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            //                         WHERE        (country.AreaType = 'country' and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1
            //						 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
            //                        
            //                         WHERE (country.AreaType = 'country' and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM  dbo.MastArea AS state  LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
            //                         WHERE ( state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";


            // }

            //if (strStID != "" && strStCity != "" && strStArea != "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and area.AreaId IN (" + strStArea + ")  and";
            //}
            //else if (strStID != "" && strStCity != "" && strStArea == "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and ";
            //}
            //else if (strStID != "" && strStCity == "" && strStArea == "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  ";
            //}
            //else
            //{
            //    _whr = " ";
            //}

             switch (strStID)
            {
                case "":
                    _whr = " ";
                    break;
                default:
                    switch (strStCity)
                    {
                        case "":
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  ";
                                    break;
                            }
                            break;
                        default:
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and ";
                                    break;
                                default:
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and area.AreaId IN (" + strStArea + ")  and";
                                    break;

                            }
                            break;
                    }
                    break;
            }

            areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
                        FROM  dbo.MastArea AS state  LEFT OUTER JOIN
                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
                         WHERE ( " + _whr + " state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";
             DataTable areaStatedt = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry);

            if (areaStatedt.Rows.Count > 0)
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = areaStatedt;
                rpt.DataBind();
                rpt.Visible = true;
                btnsave.Visible = true;
                btncancel.Visible = true;
            }
            else
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = areaStatedt;
                rpt.DataBind();
                btnsave.Visible = false;
                btncancel.Visible = false;
            }
        }

        protected void lstState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStID = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID != "")
            {
                string Qry = @"select * from MastArea where AreaType='City' and Active=1 and UnderId IN (select AreaId from MastArea where AreaType='District' and Active=1 and UnderId IN (" + strStID + ")) order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    lstCity.DataSource = dt;
                    lstCity.DataTextField = "AreaName";
                    lstCity.DataValueField = "AreaId";
                    lstCity.DataBind();
                }

            }
            else
            {
                lstCity.Items.Clear();
                rpt.DataSource = null;
                rpt.DataBind();
                btnsave.Visible = false;
                btncancel.Visible = false;

            }
        }

        protected void lstCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStID = "";
            foreach (ListItem item in lstCity.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID != "")
            {
                string Qry = @"select * from MastArea where AreaType='Area' and Active=1 and UnderId IN (" + strStID + ") order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    lstArea.DataSource = dt;
                    lstArea.DataTextField = "AreaName";
                    lstArea.DataValueField = "AreaId";
                    lstArea.DataBind();
                }

            }
            else
            {
                lstArea.Items.Clear();
                rpt.DataSource = null;
                rpt.DataBind();
                btnsave.Visible = false;
                btncancel.Visible = false;

            }
        }
    }
}