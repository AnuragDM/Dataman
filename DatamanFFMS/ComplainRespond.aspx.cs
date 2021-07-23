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
    public partial class ComplainRespond : System.Web.UI.Page
    {
        //string roleType = "";
        string loginname = "";
        string email = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txtmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
                //roleType = Settings.Instance.RoleType;
                loginname = Settings.Instance.UserName;
                //if (Settings.Instance.UserName == "SA")
                if (Settings.Instance.RoleType == "Admin")
                {
                    BindPartyType();
                    BindCompNat(); BindDept(); BindDistributors();
                }
                else 
                {
                    BindPartyType();
                    BindDepartment(); BindComplaintNature(); BindDistributors();
                }
            }
        }
        private void BindPartyType()
        {
            try
            { //Ankita - 11/may/2016- (For Optimization)
                //string partytypequery = @"select * from PartyType order by PartyTypeName";
                string partytypequery = @"select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
                DataTable partytypedt = DbConnectionDAL.GetDataTable(CommandType.Text, partytypequery);

                if (partytypedt.Rows.Count > 0)
                {
                    ddlpartytype.DataSource = partytypedt;
                    ddlpartytype.DataTextField = "PartyTypeName";
                    ddlpartytype.DataValueField = "PartyTypeId";
                    ddlpartytype.DataBind();
                }
                ddlpartytype.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindPartyTypePersons()
        {
            DataTable dtptype = new DataTable(); string str = "";
            if (ddlpartytype.SelectedItem.Text.ToUpper() == "DISTRIBUTOR")
            {
                str = "1";
            }
            //switch (ddlpartytype.SelectedItem.Text.ToUpper())
            //{
            //    case "RETAILER":
            //        lblpartytypepersons.InnerText = "Retailer Name";
            //        break;
            //    case "PLUMBER":
            //        lblpartytypepersons.InnerText = "Plumber Name";
            //        break;
            //    case "DISTRIBUTOR":
            //        lblpartytypepersons.InnerText = "Distributor Name";
            //        str = "1";
            //        break;
            //    case "ARCHITECT":
            //        lblpartytypepersons.InnerText = "Architect Name";
            //        break;
            //    case "ELECTRICIAN":
            //        lblpartytypepersons.InnerText = "Electrician Name";
            //        break;
            //    case "PROJECT":
            //        lblpartytypepersons.InnerText = "Project Name";
            //        break;
            //    case "FARMER":
            //        lblpartytypepersons.InnerText = "Farmer Name";
            //        break;
            //    //case default:
            //    //    lblpartytypepersons.InnerText="Retailer Name";
            //    //    break;
            //}
            if (str == "1")
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 ORDER BY PartyName";
            else
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " ORDER BY PartyName";
            dtptype = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlpartytypepersons.Items.Clear();
            if (dtptype.Rows.Count > 0)
            {
                ddlpartytypepersons.DataSource = dtptype;
                ddlpartytypepersons.DataTextField = "PartyName";
                ddlpartytypepersons.DataValueField = "PartyId";
                ddlpartytypepersons.DataBind();
            }
            ddlpartytypepersons.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void BindDistributors()
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
              //  string distquery = @"select * from mastparty where Partydist=1 order by PartyName";
                string distquery = @"select PartyId,PartyName from mastparty where Partydist=1 order by PartyName";
                DataTable distdt = DbConnectionDAL.GetDataTable(CommandType.Text, distquery);

                if (distdt.Rows.Count > 0)
                {
                    ddldistributor.DataSource = distdt;
                    ddldistributor.DataTextField = "PartyName";
                    ddldistributor.DataValueField = "PartyId";
                    ddldistributor.DataBind();
                }
                ddldistributor.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindDept()
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
               // string deptquery = @"select * from MastDepartment order by DepName";

                //25/01/2017 Nishu (Bind Department Nature wise)
                //string deptquery = @"select DepId,DepName from MastDepartment order by DepName";
                string deptquery = @"select DepId,DepName from MastDepartment where depid in (select deptid from MastComplaintNature) and Active=1 order by DepName";
                DataTable deptdt = DbConnectionDAL.GetDataTable(CommandType.Text, deptquery);

                if (deptdt.Rows.Count > 0)
                {
                    ddldept.DataSource = deptdt;
                    ddldept.DataTextField = "DepName";
                    ddldept.DataValueField = "DepId";
                    ddldept.DataBind();
                }
                ddldept.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindCompNat()
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
                //string cNquery = @"select * from MastComplaintNature order by Name";
                string cNquery = @"select Id,Name from MastComplaintNature where Naturetype = '" + ddlcompSugg.SelectedItem.Text + "' order by Name";
                DataTable cNdt = DbConnectionDAL.GetDataTable(CommandType.Text, cNquery);

                if (cNdt.Rows.Count > 0)
                {
                    ddlcompNature.DataSource = cNdt;
                    ddlcompNature.DataTextField = "Name";
                    ddlcompNature.DataValueField = "Id";
                    ddlcompNature.DataBind();
                }
                ddlcompNature.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindDepartment()
        {
            try
            {
                string str = "select email from mastlogin where name='" + loginname + "'";
                DataTable dtemail = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                email = dtemail.Rows[0]["email"].ToString();
                string deptquery = @"select Distinct md.DepId,md.DepName from MastDepartment md left join MastComplaintNature mc on md.DepId=mc.DeptId 
                                   where mc.EmailTo='" + email + "' and md.DepId in (select deptid from MastComplaintNature) order by DepName";
                DataTable deptdt = DbConnectionDAL.GetDataTable(CommandType.Text, deptquery);

                if (deptdt.Rows.Count > 0)
                {
                    ddldept.DataSource = deptdt;
                    ddldept.DataTextField = "DepName";
                    ddldept.DataValueField = "DepId";
                    ddldept.DataBind();
                }
                ddldept.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindComplaintNature()
        {
            try
            {
                string str = "select email from mastlogin where name='" + Settings.Instance.UserName + "'";
                DataTable dtemail = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                email = dtemail.Rows[0]["email"].ToString();
                string cNquery = @"select Id,Name from MastComplaintNature where EmailTo='" + email + "' and Naturetype = '" + ddlcompSugg.SelectedItem.Text + "' order by Name"; 
                DataTable cNdt = DbConnectionDAL.GetDataTable(CommandType.Text, cNquery);

                if (cNdt.Rows.Count > 0)
                {
                    ddlcompNature.DataSource = cNdt;
                    ddlcompNature.DataTextField = "Name";
                    ddlcompNature.DataValueField = "Id";
                    ddlcompNature.DataBind();
                }
                ddlcompNature.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void ddlpartytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartytype.SelectedIndex > 0)
            {
                BindPartyTypePersons();// lblpartytypepersons.InnerText = "Distributor Name";
            }
            else { ddlpartytypepersons.Items.Clear(); }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            string stradd = "";
            string itemStr = "";
            string itemStr1 = "";
            string deptStr = "";
            string deptStr1 = "";
            if (Convert.ToDateTime(txtmDate.Text) > Convert.ToDateTime(txttodate.Text))
            {
                rpt.DataSource = null;
                rpt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }
            if (Settings.Instance.RoleType == "Admin")
            {               

                if (!string.IsNullOrEmpty(txtmDate.Text)) { stradd += " and Vdate >='" + txtmDate.Text + "'"; }
                if (!string.IsNullOrEmpty(txttodate.Text)) { stradd += " and Vdate <='" + txttodate.Text + "'"; }
                if (ddlcomplby.SelectedValue == "D") { stradd += " and SalesDistr ='D'"; } else { stradd += " and SalesDistr ='S'"; }
                if (ddlcompNature.SelectedIndex > 0) { stradd += " and mcn.Id =" + ddlcompNature.SelectedValue + ""; }
                if (ddldept.SelectedIndex > 0) { stradd += " and mcn.DeptId =" + ddldept.SelectedValue + ""; }
                if (ddlcompSugg.SelectedValue == "1")
                {//TransComplaint
                    if (ddlcomplby.SelectedValue == "D")
                    {
                        if (ddldistributor.SelectedIndex > 0) { stradd += " and tc.distId =" + ddldistributor.SelectedValue + ""; }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and tc.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }

                    }
                    if (ddlStatus.SelectedValue == "P") { stradd += " and tc.Status is null "; }
                    else if (ddlStatus.SelectedValue == "W") { stradd += " and tc.Status ='W'"; }
                    else { stradd += " and tc.Status='R' "; }
                    str = "select ComplId as ID,DocId,Vdate,msr.SMName,mp.PartyName,mcn.Name,Status = (Case when isnull(tc.Status,'P') ='P' Then 'Pending' when isnull(tc.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'C' as val,md.Depname  from TransComplaint tc left join MastSalesRep msr on tc.SMId=msr.SMId left join MastParty mp on tc.DistId=mp.PartyId left join MastComplaintNature mcn on tc.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId where 1=1 " + stradd + " order by Vdate desc";                    
                }
                else
                {//TransSuggestion
                    if (ddlcomplby.SelectedValue == "D")
                    {
                        if (ddldistributor.SelectedIndex > 0) { stradd += " and ts.distId =" + ddldistributor.SelectedValue + ""; }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and ts.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }
                    }
                    if (ddlStatus.SelectedValue == "P") { stradd += " and ts.Status is null "; }
                    else if (ddlStatus.SelectedValue == "W") { stradd += " and ts.Status ='W'"; }
                    else { stradd += " and ts.Status='R' "; }
                   
                    str = "select SuggId as ID,DocId,Vdate,msr.SMName,mp.PartyName,mcn.Name,Status = (Case when isnull(ts.Status,'P') ='P' Then 'Pending' when isnull(ts.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'S' as val,md.Depname from TransSuggestion ts left join MastSalesRep msr on ts.SMId=msr.SMId left join MastParty mp on ts.DistId=mp.PartyId left join MastComplaintNature mcn on ts.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId where 1=1 " + stradd + " order by Vdate desc";
                }
           }
    
            else
            {
                foreach (ListItem item in ddlcompNature.Items)
                {
                    itemStr += item.Value + ",";
                }
                itemStr1 = itemStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in ddldept.Items)
                {
                    deptStr += item.Value + ",";
                }
                deptStr1 = deptStr.TrimStart(',').TrimEnd(',');

                if (!string.IsNullOrEmpty(txtmDate.Text)) { stradd += " and Vdate >='" + txtmDate.Text + "'"; }
                if (!string.IsNullOrEmpty(txttodate.Text)) { stradd += " and Vdate <='" + txttodate.Text + "'"; }
                if (ddlcomplby.SelectedValue == "D") { stradd += " and SalesDistr ='D'"; } else { stradd += " and SalesDistr ='S'"; }
                //if (ddlcompNature.SelectedIndex > 0) { stradd += " and mcn.Id =" + ddlcompNature.SelectedValue + ""; }
                //if (ddldept.SelectedIndex > 0) { stradd += " and mcn.DeptId =" + ddldept.SelectedValue + ""; }
                if (ddlcompSugg.SelectedValue == "1")
                {//TransComplaint
                    if (ddlcomplby.SelectedValue == "D")
                    {
                        if (ddldistributor.SelectedIndex > 0) { stradd += " and tc.distId =" + ddldistributor.SelectedValue + ""; }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and tc.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }

                    }
                    if (ddlStatus.SelectedValue == "P") { stradd += " and tc.Status is null "; }
                    else if (ddlStatus.SelectedValue == "W") { stradd += " and tc.Status ='W'"; }
                    else { stradd += " and tc.Status='R' "; }
                    str = "select ComplId as ID,DocId,Vdate,msr.SMName,mp.PartyName,mcn.Name,Status = (Case when isnull(tc.Status,'P') ='P' Then 'Pending' when isnull(tc.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'C' as val,md.Depname  from TransComplaint tc left join MastSalesRep msr on tc.SMId=msr.SMId left join MastParty mp on tc.DistId=mp.PartyId left join MastComplaintNature mcn on tc.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId where 1=1 " + stradd + " and mcn.Id in (" + itemStr1 + ") and mcn.DeptId in (" + deptStr1 + ")  order by Vdate desc";
                }
                else
                {//TransSuggestion
                    if (ddlcomplby.SelectedValue == "D")
                    {
                        if (ddldistributor.SelectedIndex > 0) { stradd += " and ts.distId =" + ddldistributor.SelectedValue + ""; }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and ts.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }
                    }
                    if (ddlStatus.SelectedValue == "P") { stradd += " and ts.Status is null "; }
                    else if (ddlStatus.SelectedValue == "W") { stradd += " and ts.Status ='W'"; }
                    else { stradd += " and ts.Status='R' "; }
                    str = "select SuggId as ID,DocId,Vdate,msr.SMName,mp.PartyName,mcn.Name,Status = (Case when isnull(ts.Status,'P') ='P' Then 'Pending' when isnull(ts.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'S' as val,md.Depname from TransSuggestion ts left join MastSalesRep msr on ts.SMId=msr.SMId left join MastParty mp on ts.DistId=mp.PartyId left join MastComplaintNature mcn on ts.ComplNatId=mcn.Id left join MastDepartment md on mcn.DeptId=md.DepId where 1=1 " + stradd + " and mcn.Id in (" + itemStr1 + ") and mcn.DeptId in (" + deptStr1 + ") order by Vdate desc";
                }

            }
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }

        protected void btnclr_Click(object sender, EventArgs e)
        {
            rpt.DataSource = null;
            rpt.DataBind();
            txtmDate.Text = ""; txttodate.Text = ""; ddlcompSugg.SelectedIndex = 0; ddlpartytype.SelectedIndex = 0;
            ddlpartytypepersons.SelectedIndex = -1; ddlpartytypepersons.Items.Clear(); ddlStatus.SelectedIndex = 0; ddlcomplby.SelectedIndex = 0; ddlcompNature.SelectedIndex = 0; ddldistributor.SelectedIndex = 0; ddldept.SelectedIndex = 0;
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (ddlcomplby.SelectedValue== "D")
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = false; 
                }
                else
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = true; 
                }
            }
            if (e.Item.ItemType == ListItemType.Header)
            {

                if (ddlcomplby.SelectedValue == "D")
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = false;
                }
                else
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = true;
                }
            }
        }

        protected void ddlcompSugg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Settings.Instance.RoleType == "Admin")
            { BindCompNat(); }
            else { BindComplaintNature(); }

        }
    }
}