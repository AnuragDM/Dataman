using DAL;
using BusinessLayer;
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
    public partial class MergeParty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                btnSubmit.Enabled = false;
                btnSubmit.CssClass = "btn btn-primary";
                BindDDLCity();
            }
        }

        private void BindDDLCity()
        {
            try
            {//Ankita - 20/may/2016- (For Optimization)
                //string cityQry = @"select * from MastArea where AreaType='CITY' and Active=1 order by AreaName";
                string cityQry = @"select AreaId,AreaName from MastArea where AreaType='CITY' and Active=1 order by AreaName";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                if (dtMastItem1.Rows.Count > 0)
                {
                    ddlOldCity.DataSource = dtMastItem1;
                    ddlOldCity.DataTextField = "AreaName";
                    ddlOldCity.DataValueField = "AreaId";
                    ddlOldCity.DataBind();

                    ddlNewCity.DataSource = dtMastItem1;
                    ddlNewCity.DataTextField = "AreaName";
                    ddlNewCity.DataValueField = "AreaId";
                    ddlNewCity.DataBind();
                }
                ddlOldCity.Items.Insert(0, new ListItem("--Please select--","0"));
                ddlNewCity.Items.Insert(0, new ListItem("--Please select--", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlOldCity_SelectedIndexChanged(object sender, EventArgs e)
        {//Ankita - 20/may/2016- (For Optimization)
            if (ddlOldCity.SelectedIndex != 0)
            {
                //string areaQry1 = @"select * from MastArea where Underid=" + ddlOldCity.SelectedValue + " and AreaType='Area' and Active=1 order by AreaName";
                string areaQry1 = @"select AreaId,AreaName from MastArea where Underid=" + ddlOldCity.SelectedValue + " and AreaType='Area' and Active=1 order by AreaName";
                DataTable dtMastArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry1);
                if (dtMastArea.Rows.Count > 0)
                {
                    ddlOldArea.Items.Clear();
                    ddlOldArea.DataSource = dtMastArea;
                    ddlOldArea.DataTextField = "AreaName";
                    ddlOldArea.DataValueField = "AreaId";
                    ddlOldArea.DataBind();
                    ddlOldArea.Items.Insert(0, new ListItem("--Please select--", "0"));
                }
                else
                {
                    ddlOldArea.Items.Clear();
                }
            }
            else
            {
                ddlOldArea.Items.Clear();
                ddlOldBeat.Items.Clear();
            }
        }
        protected void ddlOldArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOldArea.SelectedIndex != 0)
            {//Ankita - 20/may/2016- (For Optimization)
                string beatQry1 = @"select AreaId,AreaName from MastArea where Underid=" + ddlOldArea.SelectedValue + " and AreaType='Beat' and Active=1 order by AreaName";
                //string beatQry1 = @"select * from MastArea where Underid=" + ddlOldArea.SelectedValue + " and AreaType='Beat' and Active=1 order by AreaName";
              
                DataTable dtMastBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQry1);
                if (dtMastBeat.Rows.Count > 0)
                {
                    ddlOldBeat.DataSource = dtMastBeat;
                    ddlOldBeat.DataTextField = "AreaName";
                    ddlOldBeat.DataValueField = "AreaId";
                    ddlOldBeat.DataBind();
                    ddlOldBeat.Items.Insert(0, new ListItem("--Please select--", "0"));
                }
                else
                {
                    ddlOldBeat.Items.Clear();
                }
            }
            else
            {
                ddlOldBeat.Items.Clear();
            }
        }

        protected void ddlNewCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNewCity.SelectedIndex != 0)
            {//Ankita - 20/may/2016- (For Optimization)
                //string areaQry2 = @"select * from MastArea where Underid=" + ddlNewCity.SelectedValue + " and AreaType='Area' and Active=1 order by AreaName";
                string areaQry2 = @"select AreaId,AreaName from MastArea where Underid=" + ddlNewCity.SelectedValue + " and AreaType='Area' and Active=1 order by AreaName";
                DataTable dtMastArea1 = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry2);
                if (dtMastArea1.Rows.Count > 0)
                {
                    ddlNewArea.DataSource = dtMastArea1;
                    ddlNewArea.DataTextField = "AreaName";
                    ddlNewArea.DataValueField = "AreaId";
                    ddlNewArea.DataBind();
                    ddlNewArea.Items.Insert(0, new ListItem("--Please select--"));
                }
                else
                {
                    ddlNewArea.Items.Clear();
                }
            }
            else
            {
                ddlNewArea.Items.Clear();
                ddlNewBeat.Items.Clear();
            }
        }

        protected void ddlNewArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNewArea.SelectedIndex != 0)
            {//Ankita - 20/may/2016- (For Optimization)
                //string beatQry12 = @"select * from MastArea where Underid=" + ddlNewArea.SelectedValue + " and AreaType='Beat' and Active=1 order by AreaName";
                string beatQry12 = @"select AreaId,AreaName from MastArea where Underid=" + ddlNewArea.SelectedValue + " and AreaType='Beat' and Active=1 order by AreaName";
                DataTable dtMastBeat1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatQry12);
                if (dtMastBeat1.Rows.Count > 0)
                {
                    ddlNewBeat.DataSource = dtMastBeat1;
                    ddlNewBeat.DataTextField = "AreaName";
                    ddlNewBeat.DataValueField = "AreaId";
                    ddlNewBeat.DataBind();
                    ddlNewBeat.Items.Insert(0, new ListItem("--Please select--"));
                }
                else
                {
                    ddlNewBeat.Items.Clear();
                }
            }
            else
            {
                ddlNewBeat.Items.Clear();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlOldCity.SelectedValue != "" && ddlOldArea.SelectedValue != "" && ddlOldBeat.SelectedValue != "" && ddlNewCity.SelectedValue != "" && ddlNewArea.SelectedValue != "" && ddlNewBeat.SelectedValue != "")
            {

                string updQry = "update MastParty set CityId=" + ddlNewCity.SelectedValue + ",AreaId=" + ddlNewArea.SelectedValue + ",BeatId=" + ddlNewBeat.SelectedValue + " where PartyDist=0 and CityId=" + ddlOldCity.SelectedValue + " and AreaId=" + ddlOldArea.SelectedValue + " and BeatId=" + ddlOldBeat.SelectedValue + "";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updQry);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);     
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MergeParty.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string getpartyquery = @"select mp.*,ma.AreaName as City,ma1.AreaName as Area,ma2.AreaName as Beat from MastParty mp 
                left join MastArea ma on mp.CityId=ma.AreaId
                left join MastArea ma1 on mp.AreaId=ma1.AreaId
                left join MastArea ma2 on mp.BeatId=ma2.AreaId  where mp.CityId=" + ddlOldCity.SelectedValue + " and mp.AreaId=" + ddlOldArea.SelectedValue + " and mp.BeatId=" + ddlOldBeat.SelectedValue + " and mp.PartyDist=0 order by PartyName";
            DataTable dtParty = DbConnectionDAL.GetDataTable(CommandType.Text, getpartyquery);
            if (dtParty.Rows.Count > 0)
            {
                mainDiv.Style.Add("display", "block");
                ViewState["gridData"] = dtParty;
                gvPartyData.DataSource = dtParty;
                gvPartyData.DataBind();
                btnSubmit.Enabled = true;
                btnSubmit.CssClass = "btn btn-primary";
            }
            else
            {
                mainDiv.Style.Add("display", "block");
                gvPartyData.DataSource = dtParty;
                gvPartyData.DataBind();
                ViewState["gridData"] = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MergeParty.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string partyIDStr = "";
            for (int i = 0; i < gvPartyData.Rows.Count; i++)
            // foreach (GridViewRow gvr in gvData.Rows)
            {
                CheckBox chk = (CheckBox)gvPartyData.Rows[i].FindControl("chkItem");
                HiddenField partyId = (HiddenField)gvPartyData.Rows[i].FindControl("partyIdHiddenField");
                if (chk.Checked == true)
                {
             //       string PartyCode = gvPartyData.Rows[i].Cells[1].Text;
                    partyIDStr += partyId.Value + ",";
                }
            }
            partyIDStr = partyIDStr.TrimStart(',').TrimEnd(',');
            ViewState["partyID"] = partyIDStr;
            conditionalDiv.Style.Add("display", "block");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ddlOldCity.SelectedValue != "" && ddlOldArea.SelectedValue != "" && ddlOldBeat.SelectedValue != "" && ddlNewCity.SelectedValue != "" && ddlNewArea.SelectedValue != "" && ddlNewBeat.SelectedValue != "")
            {
                if (ViewState["partyID"] != "")
                {
                    string updQry = "update MastParty set CityId=" + ddlNewCity.SelectedValue + ",AreaId=" + ddlNewArea.SelectedValue + ",BeatId=" + ddlNewBeat.SelectedValue + " where PartyDist=0 and CityId=" + ddlOldCity.SelectedValue + " and AreaId=" + ddlOldArea.SelectedValue + " and BeatId=" + ddlOldBeat.SelectedValue + " and PartyId in (" + ViewState["partyID"] + ")";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updQry);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true); 
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select party');", true);
                    gvPartyData.DataSource = null;
                    gvPartyData.DataBind();
                    ViewState["gridData"] = null;
                    ViewState["partyID"] = null;
                }
            }
        }

        protected void gvPartyData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPartyData.PageIndex = e.NewPageIndex;
            gvPartyData.DataSource = ViewState["gridData"];
            gvPartyData.DataBind();
            //          BindGrid();
        }
    }
}