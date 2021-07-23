using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using BusinessLayer;
using System.Data;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class UploadDocuments : System.Web.UI.Page
    {
        BAL.Uploads.UploadBAL up = new BAL.Uploads.UploadBAL();
        string parameter = "", roleType = "";
        string smIDStr = "", smIDStr1 = "", distIdStr1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                divdocid.Visible = false;
                fileLabel.Style.Add("display", "none");
                btnDelete.Visible = false;               
                
            }
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillUploadDocControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        private void FillUploadDocControls(int p)
        {
            try
            {
                string upDocquery = @"select * from UploadDocuments where Id=" + p;
                DataTable upDocValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, upDocquery);
                if (upDocValueDt.Rows.Count > 0)
                {
                    txttitle.Text = upDocValueDt.Rows[0]["Title"].ToString();
                    RadioButtonList1.SelectedValue = upDocValueDt.Rows[0]["DocFor"].ToString();
                    chk.Checked =Convert.ToBoolean(upDocValueDt.Rows[0]["Active"].ToString());
                    if (upDocValueDt.Rows[0]["LinkUrl"] != string.Empty)
                    {
                        File1.Attributes.Add("value", upDocValueDt.Rows[0]["LinkUrl"].ToString());
                        fileLabel.Style.Add("display", "block");
                        fileLabel.Text = upDocValueDt.Rows[0]["LinkUrl"].ToString();
                    }
                    else
                    {
                        fileLabel.Style.Add("display", "none");
                        fileLabel.Text = string.Empty;
                    }
                    string docfor = upDocValueDt.Rows[0]["DocFor"].ToString();
                    string smids = upDocValueDt.Rows[0]["smids"].ToString();
                    ViewState["salesid"] = smids;

                    if (docfor == "Specific SalesPerson")
                    {
                        salesP.Style.Add("display", "block");
                        roleType = Settings.Instance.RoleType;
                       // fill_TreeArea();
                        BindTreeViewControl();
                    }
                    else
                    { salesP.Style.Add("display", "none"); }
                    btnsave.Text = "Update";
                    btnDelete.Visible = true;                   
                }

                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }     

        private int Save(string path)
        {
            string docID = Settings.GetDocID("VISSN", DateTime.Now);
            Settings.SetDocID("VISSN", docID);
            int Retsave = up.Insert(DateTime.Now.ToString(), docID, txttitle.Text, path, chk.Checked, RadioButtonList1.SelectedValue, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID),smIDStr1,distIdStr1);
            //int Retsave1 = up.InsertRecord(DateTime.Now.ToString(), docID, smIDStr1);
            return Retsave;           
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (RadioButtonList1.SelectedValue == "Specific SalesPerson")
                {
                    foreach (TreeNode node in trview.CheckedNodes)
                    {
                        smIDStr1 = node.Value;
                        {
                            smIDStr += node.Value + ",";
                        }
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                    if (smIDStr1 == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please select salesperson');", true);
                        return;
                    }
                }
                if (RadioButtonList1.SelectedValue == "Specific Distributor")
                {
                    foreach (ListItem item in ListBox1.Items)
                    {
                        if (item.Selected)
                        {
                            distIdStr1 += item.Value + ",";
                        }
                    }
                    distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');
                    if (distIdStr1 == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please select Distributor');", true);
                        return;
                    }
                }

                if (btnsave.Text == "Update")
                {
                    if (File1.HasFile)
                    {
                        string visitcode = ViewState["Id"].ToString();
                        string strurl = "Select LinkUrl From UploadDocuments Where id='" + ViewState["Id"].ToString() + "'";
                        string url = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, strurl));
                        string filename1 = url;
                        string filename = filename1.Replace("/", string.Empty);
                        string path = Server.MapPath("~/UploadDocuments/" + visitcode + "-" + filename.Replace("~", string.Empty).Trim());
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {

                            file.Delete();
                        }
                        UpdateDocUpload("~/" + File1.FileName);
                        string strDestPath = Server.MapPath("~/UploadDocuments/" + ViewState["Id"] + "-" + File1.FileName);
                        File1.PostedFile.SaveAs(strDestPath);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                    }
                    else
                    {
                        UpdateDocUpload(fileLabel.Text);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    }
                    ClearControls();
                }
                else
                {
                    if (File1.PostedFile != null)
                    {
                        try
                        {
                            int retsave = Save("~/" + File1.FileName);
                            if (retsave > 0)
                            {
                                string strDestPath = Server.MapPath("~/UploadDocuments/" + retsave + "-" + File1.FileName);
                                File1.PostedFile.SaveAs(strDestPath);

                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);
                                ClearControls();
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + File1.FileName + "');", true);
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + File1.FileName + "');", true);
                        }
                    }
                }
            }
            catch(Exception ex)
            { ex.ToString(); }
        }

        private void ClearControls()
        {
            try
            {
                txttitle.Text = string.Empty;
                chk.Checked = true;
                fileLabel.Style.Add("display", "none");
                btnsave.Text = "Save";
                btnDelete.Visible = false;
                trview.Nodes.Clear();
                salesP.Style.Add("display", "none");
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateDocUpload(string path)
        {
            try
            {
                string Docqry = @"update UploadDocuments set Title='" + txttitle.Text + "',DocFor='" + RadioButtonList1.SelectedValue + "',LinkURL='" + path + "',Active='" + chk.Checked + "',smids='" + smIDStr1 + "',distid='" + distIdStr1 + "' where id=" + ViewState["Id"] + "";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, Docqry);               
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void fillRepeter()
        {          
            string str = @"select * from UploadDocuments";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
            else {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
            fillRepeter();
        }

        protected void lnkdelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
              HiddenField HidLinkURL = (HiddenField)item.FindControl("HiddenField2");

            
            int del=  up.delete(hdnVisItCode.Value);
            fillRepeter();

             string filename1 = HidLinkURL.Value;
             string filename = filename1.Replace("/", string.Empty);

             string path = Server.MapPath("~/UploadDocuments/"+hdnVisItCode.Value+"-"+filename.Replace("~",string.Empty).Trim());
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                
                file.Delete();
            }

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UploadDocuments.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {  
                string appStatus = @"select * from UploadDocuments where id='" + Convert.ToString(ViewState["Id"]) + "'";
                DataTable deleteTLRdt = DbConnectionDAL.GetDataTable(CommandType.Text, appStatus);

                if (deleteTLRdt.Rows.Count > 0)
                {
                    int retdel = up.delete(Convert.ToString(ViewState["Id"]));
                    if (retdel == 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                        ClearControls();
                    }                 
                   
                    string filename = deleteTLRdt.Rows[0]["LinkUrl"].ToString().Replace("/", string.Empty);
                    string path = Server.MapPath("~/UploadDocuments/" + Convert.ToString(ViewState["Id"]) + "-" + filename.Replace("~", string.Empty).Trim());
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {

                        file.Delete();
                    }

                }
                else
                {

                }
            }
        }

        /////// Abhishek jaiswal 23may2017 start--------------------------------------------------------
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
        /////// Abhishek jaiswal 23may2017 End--------------------------------------------------------

       public void fill_TreeArea()
        {
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {
                //string strrole = "select SMID,SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                //St = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                //    lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,1);
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {
                //Ankita - 17/may/2016- (For Optimization)
                //lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID, lowestlvl);
                St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
            }


            trview.Nodes.Clear();

            if (St.Rows.Count <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
                return;
            }
            foreach (DataRow row in St.Rows)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = row["SMName"].ToString();
                tnParent.Value = row["SMId"].ToString();
                trview.Nodes.Add(tnParent);
                //tnParent.ExpandAll();
                tnParent.CollapseAll();
                //Ankita - 17/may/2016- (For Optimization)
                //FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 17/may/2016- (For Optimization)
        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")
                //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
                levelcnt = Convert.ToInt16("0") + 2;
            else
                levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


            GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
            DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

            if (FirstChildDataDt.Rows.Count > 0)
            {

                for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
                {
                    SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
                    FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
                }
                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

                for (int j = levelcnt + 1; j <= 6; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                    SmidVar = string.Empty;
                    int mTotRows = dtChild.Rows.Count;
                    if (mTotRows > 0)
                    {
                        var str = "";
                        for (int k = 0; k < mTotRows; k++)
                        {
                            SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
                        }

                        TreeNode Oparent = parent;
                        switch (j)
                        {
                            case 3:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                break;
                            case 4:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }
                                break;
                            case 5:
                                if (Oparent.Parent != null)
                                {
                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                    {
                                        foreach (TreeNode child in Pchild.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }


                                break;
                            case 6:
                                if (Oparent.Parent != null)
                                {
                                    if (Settings.Instance.RoleType == "Admin")
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode Qchild in Pchild.ChildNodes)
                                            {
                                                foreach (TreeNode child in Qchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }

                                                    dtChild.Select();
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode child in Pchild.ChildNodes)
                                            {
                                                str += child.Value + ","; parent = child;
                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                for (int l = 0; l < dr.Length; l++)
                                                {
                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                }
                                                dtChild.Select();
                                            }
                                        }
                                    }

                                }

                                else
                                {
                                    foreach (TreeNode child in Oparent.ChildNodes)
                                    {
                                        str += child.Value + ","; parent = child;
                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                        for (int l = 0; l < dr.Length; l++)
                                        {
                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                        }
                                        dtChild.Select();
                                    }
                                }

                                break;
                        }

                        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
                    }
                }
            }
        }

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();
            if (ViewState["salesid"] != null)
            {
                string[] SplitSmid = ViewState["salesid"].ToString().Split(',');
                if (SplitSmid.Length > 0)
                {
                    for (int i = 0; i < SplitSmid.Length; i++)
                    {
                        if (Smid == SplitSmid[i])
                        {
                            child.Checked = true;
                        }
                    }
                }
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedItem.Text == "Specific SalesPerson")
            {
                salesP.Style.Add("display", "block");
                divDistributor.Style.Add("display", "none");
                roleType = Settings.Instance.RoleType;               
                BindTreeViewControl();
            }
            else if (RadioButtonList1.SelectedItem.Text == "Specific Distributor")
            {
                divDistributor.Style.Add("display", "block");
                salesP.Style.Add("display", "none");       
                BindDistributorDDl();
            }
            else
            {
                salesP.Style.Add("display", "none");
                divDistributor.Style.Add("display", "none");
            }            
        }

        private void BindDistributorDDl()
        {
            try
            {
                string distqry = @"select mp.PartyId,(mp.PartyName + ' - ' + mp.Mobile + ' - ' + ma.areaname) as PartyName from MastParty mp left join mastarea ma on mp.Cityid=ma.areaid where mp.PartyDist=1 and mp.Active=1 and ma.areatype='City' order by mp.PartyName";                
                DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                if (dtDist.Rows.Count > 0)
                {
                    ListBox1.DataSource = dtDist;
                    ListBox1.DataTextField = "PartyName";
                    ListBox1.DataValueField = "PartyId";
                    ListBox1.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
     
    }
}