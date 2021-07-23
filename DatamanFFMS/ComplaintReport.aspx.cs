using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace AstralFFMS
{
    public partial class ComplaintReport : System.Web.UI.Page
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
                //BindProductClass();
                //BindSalePersonDDl();
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                BindPartyType();
                BindMaterialGroup();
                BindDepartment();                
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                // GetRoleType(Settings.Instance.RoleID);
                btnExport.Visible = false;

                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";

            }


        }
        private void BindPartyType()
        {
            try
            {//Ankita - 13/may/2016- (For Optimization)
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

                partytypedt.Dispose();
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

            dtptype.Dispose();
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

                dt.Dispose();
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

        //private void BindSalePersonDDl()
        //{
        //    try
        //    {
        //        if (roleType == "Admin")
        //        {//Ankita - 13/may/2016- (For Optimization)
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            dv1.RowFilter = "SMName<>.";
        //            dv1.Sort = "SMName asc";

        //            ListBox1.DataSource = dv1.ToTable();
        //            ListBox1.DataTextField = "SMName";
        //            ListBox1.DataValueField = "SMId";
        //            ListBox1.DataBind();
        //        }
        //        else
        //        {
        //            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //            DataView dv = new DataView(dt);
        //            //     dv.RowFilter = "RoleName='Level 1'";
        //            dv.RowFilter = "SMName<>.";
        //            if (dv.ToTable().Rows.Count > 0)
        //            {
        //                ListBox1.DataSource = dv.ToTable();
        //                ListBox1.DataTextField = "SMName";
        //                ListBox1.DataValueField = "SMId";
        //                ListBox1.DataBind();
        //            }
        //        }
        //        //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
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

                St.Dispose();
                Rows = null;
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

            Rows = null;
        }

        private void BindMaterialGroup()
        {
            try
            {//Ankita - 13/may/2016- (For Optimization)
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
                //   ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
                dtProdRep.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
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
                    dtDist1.Dispose();
                }
                else
                {
                    if (SMIDStr != "")
                    {
                        string citystr = "";
                        //string cityQry = @"  select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select distinct underid from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID + ")) and Active=1 )) and areatype='City' and Active=1 order by AreaName";
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

                        dtCity.Dispose();
                        dtDist.Dispose();
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

     

        private void ClearControls()
        {
            try
            {
                //    ddlProduct.Items.Clear();
                productListBox.Items.Clear();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                complreportrpt.DataSource = null;
                complreportrpt.DataBind();

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
                string Qrychk = "", getComplQry = "", getDistComplQry = "",strDepartment="",strCompNature="";
                string smIDStr1 = "", smIDStr="", matProStrNew = "", matGrpStr = "";
                string DistIdStr1 = "";
                //foreach (ListItem item in ListBox1.Items)
                //{
                //    if (item.Selected)
                //    {
                //        smIDStr1 += item.Value + ",";
                //    }
                //}
                //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
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
                //For Product
                foreach (ListItem product in productListBox.Items)
                {
                    if (product.Selected)
                    {
                        matProStrNew += product.Value + ",";
                    }
                }
                matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');
                //For Material Group
                foreach (ListItem MatGrp in matGrpListBox.Items)
                {
                    if (MatGrp.Selected)
                    {
                        matGrpStr += MatGrp.Value + ",";
                    }
                }
                matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

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
                if (smIDStr1 != "")
                {
                    Qrychk = " CompDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and CompDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                }
                else
                {
                    Qrychk = " VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                }
                if (smIDStr1 != "")
                {
                    if (matProStrNew != "")
                        Qrychk = Qrychk + " and ItemId in (" + matProStrNew + ") ";

                    if (matGrpStr != "")
                    {
                        Qrychk = Qrychk + " and UnderId in (" + matGrpStr + ")";
                    }

                    if (strDepartment != "")
                    {
                        Qrychk = Qrychk + " and DepId in (" + strDepartment + ") ";
                    }

                    if (strCompNature != "")
                    {
                        Qrychk = Qrychk + " and ComplNatId  in (" + strCompNature + ") ";
                    }
                    if (ddlStatus.SelectedValue != "A")
                    {
                        // Qrychk = Qrychk + " and Status Is Null or  Status ='" + ddlStatus.SelectedItem.Text + "'";
                        Qrychk = Qrychk + " and isnull(Status,'P') ='" + ddlStatus.SelectedValue + "'";
                    }
                }
                else
                {
                    if (matProStrNew != "")
                        Qrychk = Qrychk + " and c.ItemId in (" + matProStrNew + ") ";

                    if (matGrpStr != "")
                    {
                        Qrychk = Qrychk + " and i.UnderId in (" + matGrpStr + ")";
                    }

                    if (strDepartment != "")
                    {                     
                        Qrychk = Qrychk + " and md.DepId in (" + strDepartment + ") "; 
                    }

                    if (strCompNature != "")
                    {                      
                        Qrychk = Qrychk + " and c.ComplNatId  in (" + strCompNature + ") "; 
                    }
                    if (ddlStatus.SelectedValue != "A")
                    {
                        // Qrychk = Qrychk + " and Status Is Null or  Status ='" + ddlStatus.SelectedItem.Text + "'";
                        Qrychk = Qrychk + " and isnull(Status,'P') ='" + ddlStatus.SelectedValue + "'";
                    }
                }
                string stradd = ""; 
                if (smIDStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        complreportrpt.DataSource = new DataTable();
                        complreportrpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }
                    
                    
                    if (!string.IsNullOrEmpty(ddlpartytype.Text) && ddlpartytype.SelectedValue != "0")
                    {
                        if (ddlpartytype.SelectedItem.Text.ToUpper() == "DISTRIBUTOR") stradd += " and PartyType IS NULL";
                        else stradd += " and PartyType ='" + ddlpartytype.SelectedValue + "'";
                        
                    }
                    if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0") { stradd += " and c.DistId ='" + ddlpartytypepersons.SelectedValue + "'"; }
                    //Ankita - 13/may/2016- (For Optimization)
//                    getComplQry = @"select Convert(varchar(20),c.VDate,106) AS CompDate,cp.SMName as CompBY,cp.SyncId,ma.AreaName as City,isnull(c.Status,'P')as Status,isnull(pt.PartyTypeName,'Distributor') as PartyTypeName,
//                    i.ItemName as Item,md.DepName AS DepName,cn.Name AS ComplaintNature, c.Remark [Complaint],c.Imgurl as URL,isnull(mp.PartyName,'') AS Distributor from TransComplaint c 
//                    left join MastItem i on i.ItemId=c.ItemId inner join MastSalesRep cp on cp.SMId=c.SMId INNER JOIN MastArea ma ON ma.AreaId=cp.CityId INNER JOIN MastComplaintNature cn ON cn.Id=c.ComplNatId
//                    LEFT JOIN MastParty mp ON c.DistId = mp.PartyId left JOIN PartyType  pt ON mp.PartyType=pt.PartyTypeId LEFT JOIN MastDepartment md  ON cn.DeptId=md.DepId where 1=1 and " + Qrychk + " and c.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) " + stradd + " order by c.Vdate desc ";
                    getComplQry = "select * from [View_ComplaintReport] where 1=1 and " + Qrychk + " and SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) " + stradd + " order by CompDate desc ";
                    DataTable dtComplaint = DbConnectionDAL.GetDataTable(CommandType.Text, getComplQry);
                    if (dtComplaint.Rows.Count > 0)
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                        btnExport.Visible = true;
                        //trview.Nodes.Clear();
                    }
                    else
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                        btnExport.Visible = false;
                        //trview.Nodes.Clear();
                    }

                    dtComplaint.Dispose();
                }

                else if (DistIdStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        complreportrpt.DataSource = new DataTable();
                        complreportrpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    getDistComplQry = @"select Convert(varchar(20),c.VDate,106) AS CompDate,cp.PartyName as CompBY,cp.SyncId,ma.AreaName as City,Status = (Case when isnull(c.Status,'P') ='P' Then 'Pending' when isnull(c.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'' as PartyTypeName,
                    i.ItemName as Item,md.DepName AS DepName,cn.Name AS ComplaintNature, c.Remark [Complaint],c.Imgurl as URL,isnull(cp.PartyName,'') AS Distributor from TransComplaint c left join MastItem i on i.ItemId=c.ItemId left join MastParty cp on cp.PartyId=c.DistId
                    left JOIN MastArea ma ON ma.AreaId=cp.CityId left JOIN MastComplaintNature cn ON cn.Id=c.ComplNatId  LEFT JOIN MastDepartment md  ON cn.DeptId=md.DepId  where " + Qrychk + " and c.DistId in (" + DistIdStr1 + ") and c.SMID = 0  order by c.Vdate desc ";
                   //getDistComplQry = @"select * from [View_ComplaintReport] where " + Qrychk + " and DistId in (" + DistIdStr1 + ") and SMID = 0  order by CompDate desc ";
                    DataTable dtComplaint = DbConnectionDAL.GetDataTable(CommandType.Text, getDistComplQry);
                    if (dtComplaint.Rows.Count > 0)
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        complreportrpt.DataSource = dtComplaint;
                        complreportrpt.DataBind();
                        btnExport.Visible = false;
                    }
                    dtComplaint.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person or Distributor');", true);
                    complreportrpt.DataSource = null;
                    complreportrpt.DataBind();
                }
                if (ddlComplaint.SelectedItem.Text == "Sales Person")
                {
                    divdist.Attributes.Add("style", "display:none;");
                    divptype.Attributes.Remove("style");
                    //divsp.Attributes.Remove("style");
                }
                else if (ddlComplaint.SelectedItem.Text == "Distributor")
                {
                    divptype.Attributes.Add("style", "display:none;");
                    //divsp.Attributes.Add("style", "display:none;");
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
            Response.Redirect("~/ComplaintReport.aspx");
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
            {//Ankita - 13/may/2016- (For Optimization)
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
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }
            if (ddlComplaint.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                //divsp.Attributes.Remove("style");
            }
            else if (ddlComplaint.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                //divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
            }
        }

        protected void ddlComplaint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlComplaint.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                //divsp.Attributes.Remove("style");
                BindPartyType(); ddlpartytypepersons.Items.Clear();
                DistListbox.Items.Clear();
                DistListbox.DataBind();
                //BindSalePersonDDl();
               
                //fill_TreeArea();
              
                complreportrpt.DataSource = null;
                complreportrpt.DataBind();

            }
            else if (ddlComplaint.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                //divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
                ddlpartytype.Items.Clear(); ddlpartytypepersons.Items.Clear();
                //ListBox1.Items.Clear();
                //ListBox1.DataBind();
                string smIDStr12 = Settings.Instance.SMID;
                BindDistributorDDl(smIDStr12);
                complreportrpt.DataSource = null;
                complreportrpt.DataBind();

            }
              else { }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ComplaintReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Complaint By".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Party Type".TrimStart('"').TrimEnd('"') + "," + "Party Name".TrimStart('"').TrimEnd('"') + "," + "Department".TrimStart('"').TrimEnd('"') + "," + "Complaint Nature".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Complaint".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ComplaintBy", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyType", typeof(String)));

            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Department", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ComplaintNature", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Complaint", typeof(String)));           
            dtParams.Columns.Add(new DataColumn("Status", typeof(String)));
           
            foreach (RepeaterItem item in complreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label CompDateLabel = item.FindControl("CompDateLabel") as Label;
                dr["Date"] = CompDateLabel.Text;
                Label CompBYLabel = item.FindControl("CompBYLabel") as Label;
                dr["ComplaintBy"] = CompBYLabel.Text;
                Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
                dr["SyncId"] = SyncIdLabel.Text.ToString(); ;
                Label CityLabel = item.FindControl("CityLabel") as Label;
                dr["City"] = CityLabel.Text.ToString(); ;
                Label PartyTypeNameLabel = item.FindControl("PartyTypeNameLabel") as Label;
                dr["PartyType"] = PartyTypeNameLabel.Text.ToString();
                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["PartyName"] = DistributorLabel.Text.ToString();
                Label DepNameLabel = item.FindControl("DepNameLabel") as Label;
                dr["Department"] = DepNameLabel.Text.ToString(); ;
                Label ComplaintNatureLabel = item.FindControl("ComplaintNatureLabel") as Label;
                dr["ComplaintNature"] = ComplaintNatureLabel.Text.ToString();
                Label ItemLabel = item.FindControl("ItemLabel") as Label;
                dr["Product"] = ItemLabel.Text.ToString();
                Label ComplaintLabel = item.FindControl("ComplaintLabel") as Label;
                dr["Complaint"] = ComplaintLabel.Text.ToString();                
                Label StatusLabel = item.FindControl("StatusLabel") as Label;
                dr["Status"] = StatusLabel.Text.ToString();

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
            Response.AddHeader("content-disposition", "attachment;filename=ComplaintReport.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();
            dtParams.Dispose();
            sb.Clear();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=ComplaintReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //complreportrpt.RenderControl(hw);
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
                string str1 = @"SELECT T1.Id,T1.Name FROM MastComplaintNature AS T1 WHERE T1.NatureType='Complaint' AND T1.Active=1 AND T1.DeptId IN ("+str+")";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if (dt1.Rows.Count > 0)
                {
                    LstCompNature.DataSource = dt1;
                    LstCompNature.DataTextField = "Name";
                    LstCompNature.DataValueField = "Id";
                    LstCompNature.DataBind();
                }
                dt1.Dispose();
            }
            if (ddlComplaint.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                //divsp.Attributes.Remove("style");
            }
            else if (ddlComplaint.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                //divsp.Attributes.Add("style", "display:none;");
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
            if (ddlComplaint.SelectedItem.Text == "Sales Person")
            {
                divdist.Attributes.Add("style", "display:none;");
                divptype.Attributes.Remove("style");
                //divsp.Attributes.Remove("style");
            }
            else if (ddlComplaint.SelectedItem.Text == "Distributor")
            {
                divptype.Attributes.Add("style", "display:none;");
                //divsp.Attributes.Add("style", "display:none;");
                divdist.Attributes.Remove("style");
            }
        }

        protected void complreportrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (ddlComplaint.SelectedValue == "Distributor")
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

                if (ddlComplaint.SelectedValue == "Distributor")
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