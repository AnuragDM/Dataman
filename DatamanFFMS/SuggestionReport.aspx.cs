using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using System.Text;

namespace AstralFFMS
{
    public partial class SuggestionReport : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                //Added By - Nishu 06/12/2015 
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");

                txtfmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
                //End               
                //        BindProductClass();
                //BindSalePersonDDl();
                BindPartyType();
                BindMaterialGroup();
                BindDepartment();
                //Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                btnExport.Visible = false;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        private void BindDepartment()
        {
            try
            {
                //25/01/2017 Nishu (Bind Department Nature wise)
                //string str = @"SELECT T1.DepId,T1.DepName FROM MastDepartment AS T1 WHERE T1.Active=1 order by T1.DepName";
                string str = @"select DepId,DepName from MastDepartment where depid in ( select deptid from MastComplaintNature) and Active=1 order by DepName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    LstDepartment.DataSource = dt;
                    LstDepartment.DataTextField = "DepName";
                    LstDepartment.DataValueField = "DepId";
                    LstDepartment.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        //private void GetRoleType(string p)
        //{
        //    try
        //    { 
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        private void BindSalePersonDDl()
        {
            try
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

                    ListBox1.DataSource = dv1.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "SMName<>.";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ListBox1.DataSource = dv.ToTable();
                        ListBox1.DataTextField = "SMName";
                        ListBox1.DataValueField = "SMId";
                        ListBox1.DataBind();
                    }
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                roleType = Settings.Instance.RoleType;
                if (roleType == "Admin")
                {
                    string distqry1 = @"select PartyId,PartyName from MastParty where PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist1 = DbConnectionDAL.GetDataTable(CommandType.Text, distqry1);
                    if (dtDist1.Rows.Count > 0)
                    {
                        DistListbox.DataSource = dtDist1;
                        DistListbox.DataTextField = "PartyName";
                        DistListbox.DataValueField = "PartyId";
                        DistListbox.DataBind();

                    }

                }
                else
                {
                    if (SMIDStr != "")
                    {
                        string citystr = "";
                        string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";

                        DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);

                        for (int i = 0; i < dtCity.Rows.Count; i++)
                        {
                            citystr += dtCity.Rows[i]["AreaId"] + ",";
                        }
                        citystr = citystr.TrimStart(',').TrimEnd(',');
                        //string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";
                        string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";                                                                          
                        DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                        if (dtDist.Rows.Count > 0)
                        {

                            DistListbox.DataSource = dtDist;
                            DistListbox.DataTextField = "PartyName";
                            DistListbox.DataValueField = "PartyId";
                            DistListbox.DataBind();

                        }
                    }
                    else
                    {
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                        //rpt.DataSource = null;
                        //rpt.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindPartyType()
        {
            try
            { //Ankita - 18/may/2016- (For Optimization)
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
                ddlpartytype.Items.Insert(1, new ListItem("DISTRIBUTOR", null));
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
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 ORDER BY PartyName";
            }
            else
            {
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " ORDER BY PartyName";
            }
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
        private void BindMaterialGroup()
        {
            try
            {//Ankita - 18/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
            //    ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //protected void ddlMatGrp_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlMatGrp.SelectedIndex != 0)
        //    {
        //        string mastItemQry1 = @"select * from MastItem where Underid=" + ddlMatGrp.SelectedValue + " and ItemType='ITEM' and Active=1";
        //        DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
        //        if (dtMastItem1.Rows.Count > 0)
        //        {
        //            ddlProduct.DataSource = dtMastItem1;
        //            ddlProduct.DataTextField = "ItemName";
        //            ddlProduct.DataValueField = "ItemId";
        //            ddlProduct.DataBind();
        //        }
        //        ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
        //    }
        //    else
        //    {
        //        ClearControls();
        //    }
        //}

        private void ClearControls()
        {
            try
            {
              //  ddlProduct.Items.Clear();
                productListBox.Items.Clear();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string Qrychk = "", getComplQry = "", getDistComplQry = "", strDepartment = "", strCompNature = "";
                string smIDStr1 = "", smIDStr="", matProStrNew = "", matGrpStr = "";
                string DistIdStr1 = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                // For Distributor
                foreach (ListItem DistId in DistListbox.Items)
                {
                    if (DistId.Selected)
                    {
                        DistIdStr1 += DistId.Value + ",";
                    }
                }
                DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');

                Qrychk = " s1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and s1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                //For Material Group
                foreach (ListItem MatGrp in matGrpListBox.Items)
                {
                    if (MatGrp.Selected)
                    {
                        matGrpStr += MatGrp.Value + ",";
                    }
                }
                matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
                //For Product
                foreach (ListItem product in productListBox.Items)
                {
                    if (product.Selected)
                    {
                        matProStrNew += product.Value + ",";
                    }
                }
                matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');

                //For Department
                foreach (ListItem itm in LstDepartment.Items)
                {
                    if (itm.Selected)
                    {
                        strDepartment += itm.Value + ",";
                    }
                }
                strDepartment = strDepartment.TrimStart(',').TrimEnd(',');

                //For ComplaintNature
                foreach (ListItem itm in LstCompNature.Items)
                {
                    if (itm.Selected)
                    {
                        strCompNature += itm.Value + ",";
                    }
                }
                strCompNature = strCompNature.TrimStart(',').TrimEnd(',');

                if (matGrpStr != "")
                {
                    Qrychk = Qrychk + " and I.UnderId in (" + matGrpStr + ")";
                }

                if (matProStrNew != "")
                {
                    Qrychk = Qrychk + " and s1.ItemId in (" + matProStrNew + ") ";
                }

                if (strDepartment != "")
                {
                    Qrychk = Qrychk + " and md.DepId in (" + strDepartment + ") ";
                }

                if (strCompNature != "")
                {
                    Qrychk = Qrychk + " and s1.ComplNatId in (" + strCompNature + ") ";
                }
                if (ddlStatus.SelectedValue != "A")
                {
                    //Qrychk = Qrychk + " and s1.Status Is Null or  s1.Status ='" + ddlStatus.SelectedItem.Text + "'";
                    Qrychk = Qrychk + " and isnull(s1.Status,'P') ='" + ddlStatus.SelectedValue + "'";
                }
                string stradd = ""; 
                if (smIDStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        suggreportrpt.DataSource = new DataTable();
                        suggreportrpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(ddlpartytype.Text) && ddlpartytype.SelectedValue != "0")
                    {
                        if (ddlpartytype.SelectedItem.Text.ToUpper() == "DISTRIBUTOR") stradd += " and PartyType IS NULL";
                        else stradd += " and PartyType ='" + ddlpartytype.SelectedValue + "'";
                    }
                    if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and s1.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }
                    getComplQry = @"SELECT i.ItemName AS Item,NewApplicationArea AS NewAppArea,s1.TechnicalAdvantage AS TechAdv,isnull(pt.PartyTypeName,'Distributor') as PartyTypeName,s1.MakeProductBetter AS ProdBetter,convert(varchar,s1.Vdate,106) AS [Date],Status = (Case when isnull(s1.Status,'P') ='P' Then 'Pending' when isnull(s1.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),
                    cp.SMName AS SuggestionBy,cp.SyncId,isnull(mp.PartyName,'') AS Distributor,cn.Name AS ComplaintNature,md.DepName FROM TransSuggestion s1 left JOIN MastItem i ON i.ItemId=s1.ItemId 
                    left join MastSalesRep cp on cp.SMId=s1.SMId LEFT JOIN MastParty mp ON s1.DistId = mp.PartyId LEFT JOIN MastComplaintNature cn ON s1.ComplNatId=cn.Id LEFT JOIN MastDepartment md ON cn.DeptId=md.DepId left JOIN PartyType  pt ON mp.PartyType=pt.PartyTypeId where 1=1 and " + Qrychk + " " + stradd + " and s1.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and salesdistr='S' order by s1.Vdate desc ";
                    DataTable dtSuggestion = DbConnectionDAL.GetDataTable(CommandType.Text, getComplQry);
                    if (dtSuggestion.Rows.Count > 0)
                    {
                        suggreportrpt.DataSource = dtSuggestion;
                        suggreportrpt.DataBind();
                        btnExport.Visible = true;
                        trview.Nodes.Clear();
                    }
                    else
                    {
                        suggreportrpt.DataSource = dtSuggestion;
                        suggreportrpt.DataBind();
                        btnExport.Visible = false;
                        trview.Nodes.Clear();
                    }
                }
                else if (DistIdStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        suggreportrpt.DataSource = new DataTable();
                        suggreportrpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    getDistComplQry = @"SELECT i.ItemName AS Item,NewApplicationArea AS NewAppArea,s1.TechnicalAdvantage AS TechAdv,s1.MakeProductBetter AS ProdBetter,convert(varchar,s1.Vdate,106) AS [Date],Status = (Case when isnull(s1.Status,'P') ='P' Then 'Pending' when isnull(s1.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),mp.SyncId,'' as PartyTypeName,
                  mp.PartyName AS SuggestionBy,isnull(mp.PartyName,'') AS Distributor,cn.Name AS ComplaintNature,md.DepName FROM TransSuggestion s1 left JOIN MastItem i ON i.ItemId=s1.ItemId 
                LEFT JOIN MastParty mp ON s1.DistId = mp.PartyId LEFT JOIN MastComplaintNature cn ON s1.ComplNatId=cn.Id LEFT JOIN MastDepartment md ON cn.DeptId=md.DepId where 1=1 and " + Qrychk + " and s1.DistId in (" + DistIdStr1 + ") and salesdistr='D' order by s1.Vdate desc ";
                    DataTable dtComplaint = DbConnectionDAL.GetDataTable(CommandType.Text, getDistComplQry);
                    if (dtComplaint.Rows.Count > 0)
                    {
                        suggreportrpt.DataSource = dtComplaint;
                        suggreportrpt.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        suggreportrpt.DataSource = dtComplaint;
                        suggreportrpt.DataBind();
                        btnExport.Visible = false;
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person or Party Name');", true);
                    suggreportrpt.DataSource = null;
                    suggreportrpt.DataBind();
                }
                if (ddlSuggestion.SelectedItem.Text == "Sales Person")
                {
                    divdist.Attributes.Add("style", "display:none;");
                    divptype.Attributes.Remove("style");
                    divsp.Attributes.Remove("style");
                }
               else if (ddlSuggestion.SelectedItem.Text == "Distributor"){
                    divptype.Attributes.Add("style", "display:none;");
                    divsp.Attributes.Add("style", "display:none;");
                    divdist.Attributes.Remove("style");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SuggestionReport.aspx");
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            if (matGrpStr != "")
            { //Ankita - 18/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                 DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                 if (dtMastItem1.Rows.Count > 0)
                 {
                     productListBox.DataSource = dtMastItem1;
                     productListBox.DataTextField = "ItemName";
                     productListBox.DataValueField = "ItemId";
                     productListBox.DataBind();
                 }
          //       ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
             }
             else
             {
                 ClearControls();
             }
            if (ddlSuggestion.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                divsp.Attributes.Remove("style");
            }
            else if (ddlSuggestion.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
            }
        }

        protected void ddlSuggestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSuggestion.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                divsp.Attributes.Remove("style");
                BindPartyType(); ddlpartytypepersons.Items.Clear();
                DistListbox.Items.Clear();
                DistListbox.DataBind();
               // BindSalePersonDDl();
                roleType = Settings.Instance.RoleType;
                //fill_TreeArea();
                BindTreeViewControl();
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();

            }
            else if (ddlSuggestion.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
                ddlpartytype.Items.Clear(); ddlpartytypepersons.Items.Clear();
                ListBox1.Items.Clear();
                ListBox1.DataBind();
                string smIDStr12 = Settings.Instance.SMID;
                BindDistributorDDl(smIDStr12);
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();

            }
            else { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=SuggestionReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Suggestion By".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Party Type".TrimStart('"').TrimEnd('"') + "," + "Party Name".TrimStart('"').TrimEnd('"') + "," + "Department".TrimStart('"').TrimEnd('"') + "," + "Complaint Nature".TrimStart('"').TrimEnd('"') + "," + "New App Area".TrimStart('"').TrimEnd('"') + "," + "Tech Adv".TrimStart('"').TrimEnd('"') + "," + "Prod Better".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SuggestionBy", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyType", typeof(String)));

            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Department", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ComplaintNature", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NewAppArea", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TechAdv", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProdBetter", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Status", typeof(String)));
            
            //
            foreach (RepeaterItem item in suggreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label DateLabel = item.FindControl("DateLabel") as Label;
                dr["Date"] = DateLabel.Text;
                Label SuggestionByLabel = item.FindControl("SuggestionByLabel") as Label;
                dr["SuggestionBy"] = SuggestionByLabel.Text;
                Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
                dr["SyncId"] = SyncIdLabel.Text.ToString(); ;
                Label ItemLabel = item.FindControl("ItemLabel") as Label;
                dr["Product"] = ItemLabel.Text.ToString(); ;
                Label PartyTypeNameLabel = item.FindControl("PartyTypeNameLabel") as Label;
                dr["PartyType"] = PartyTypeNameLabel.Text.ToString(); ;

                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["PartyName"] = DistributorLabel.Text.ToString(); ;
                Label DepNameLabel = item.FindControl("DepNameLabel") as Label;
                dr["Department"] = DepNameLabel.Text.ToString(); ;
                Label ComplaintNatureLabel = item.FindControl("ComplaintNatureLabel") as Label;
                dr["ComplaintNature"] = ComplaintNatureLabel.Text.ToString(); ;
                Label NewAppAreaLabel = item.FindControl("NewAppAreaLabel") as Label;
                dr["NewAppArea"] = NewAppAreaLabel.Text.ToString(); ;
                Label TechAdvLabel = item.FindControl("TechAdvLabel") as Label;
                dr["TechAdv"] = TechAdvLabel.Text.ToString(); ;
                Label ProdBetterLabel = item.FindControl("ProdBetterLabel") as Label;
                dr["ProdBetter"] = ProdBetterLabel.Text.ToString(); ;
                Label StatusLabel = item.FindControl("StatusLabel") as Label;
                dr["Status"] = StatusLabel.Text.ToString(); ;

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0 || k == 0)
                        {
                            sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=SuggestionReport.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=SuggestionReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //suggreportrpt.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void LstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            //         string message = "";
            foreach (ListItem itm in LstDepartment.Items)
            {
                if (itm.Selected)
                {
                    str += itm.Value + ",";
                }
            }
            str = str.TrimStart(',').TrimEnd(',');

            if (str != "")
            {
                string str1 = @"SELECT T1.Id,T1.Name FROM MastComplaintNature AS T1 WHERE T1.NatureType='Suggestion' AND T1.Active=1 AND T1.DeptId IN (" + str + ")";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if (dt1.Rows.Count > 0)
                {
                    LstCompNature.DataSource = dt1;
                    LstCompNature.DataTextField = "Name";
                    LstCompNature.DataValueField = "Id";
                    LstCompNature.DataBind();
                }

            }
            if (ddlSuggestion.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                divsp.Attributes.Remove("style");
            }
            else if (ddlSuggestion.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
            }

        }

        protected void ddlpartytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartytype.SelectedIndex > 0)
            {
                BindPartyTypePersons();// lblpartytypepersons.InnerText = "Distributor Name";
            }
            else { ddlpartytypepersons.Items.Clear(); }
            if (ddlSuggestion.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                divsp.Attributes.Remove("style");
            }
            else if (ddlSuggestion.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
            }
        }

        protected void suggreportrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (ddlSuggestion.SelectedValue == "Distributor")
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

                if (ddlSuggestion.SelectedValue == "Distributor")
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

    }
}