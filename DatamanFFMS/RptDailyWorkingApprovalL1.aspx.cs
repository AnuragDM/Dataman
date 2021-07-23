using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

namespace AstralFFMS
{
    public partial class RptDailyWorkingApprovalL1 : System.Web.UI.Page
    {
        int uid = 0;
        int smID = 0;
        int msg = 0;
        string userIdQryStr = string.Empty;
        string pageName = string.Empty;
        string roleType = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
               smID = Convert.ToInt32(Settings.Instance.SMID);
               //Ankita - 18/may/2016- (For Optimization)
               // GetRoleType(Settings.Instance.RoleID);
               roleType = Settings.Instance.RoleType;
               //BindSalePersonDDl();
               if (Request.QueryString["hasval"] != "Y")
               {
                   fill_TreeArea();
               }
                //12/12/15
                //frmTextBox.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                //toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //12/12/15


                //if (Request.QueryString["SMId"] != null && Request.QueryString["VisDocId"] != null)
                if (Request.QueryString["SMId"] != null)
                {
                    ListBox1.SelectedValue = Request.QueryString["SMId"];
             //       btnBack.Visible = true;

                    //frmdate.Style.Add("display", "none");
                    btnGo.Enabled = false;
                    Button2.Enabled = false;
                    btnGo.CssClass = "btn btn-primary";
                    Button2.CssClass = "btn btn-primary";
                    if (Request.QueryString["Page"] != null)
                    {
                        pageName = Request.QueryString["Page"];
                    }
             //       GetDailyWorkingSummaryL1New(Convert.ToInt32(Request.QueryString["SMId"]), Request.QueryString["VisDocId"]);
                   ViewState["smIDStr"] = "";
                   GetDailyWorkingSummaryL1New(Convert.ToInt32(Request.QueryString["SMId"]));
                }
                else
                {
                    btnBack.Visible = false;
                    //frmdate.Style.Add("display", "block");
                    btnGo.Enabled = true;
                    Button2.Enabled = true;
                }
            }
            if (Request.QueryString["hasval"] == "Y")
            {
                if (hid.Value == "Y")
                {
                    rptmain.Style.Add("display", "block");
                    GetDailyWorkingSummaryL1(Request.QueryString["smIDStr"].ToString(), Request.QueryString["userid"].ToString());
                    ShowAlert("Record Updated Successfully");
                    fill_TreeArea();
                    hid.Value = "N";
                }
            }

        }

        private void BindSalePersonDDl()
        {
          
            if (roleType == "Admin")
            {  //Ankita - 18/may/2016- (For Optimization)

                //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                DataTable dtcheckrole = new DataTable();
                dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                DataView dv1 = new DataView(dtcheckrole);
                //dv1.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
                dv1.RowFilter = "SMName<>'.'";
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
                //dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
                dv.RowFilter = "SMName<>'.'";
                dv.Sort = "SMName asc";
                ListBox1.DataSource = dv.ToTable();
                ListBox1.DataTextField = "SMName";
                ListBox1.DataValueField = "SMId";
                ListBox1.DataBind();
            }
        }
        void fill_TreeArea()
        {
            int lowestlvl = 0;
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
                //Ankita - 18/may/2016- (For Optimization)
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
                //Ankita - 18/may/2016- (For Optimization)s
                // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 18/may/2016- (For Optimization)
        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")
                levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
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
                    //SmidVar = string.Empty;
                    int mTotRows = dtChild.Rows.Count;
                    if (mTotRows > 0)
                    {
                        SmidVar = string.Empty;
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

        //public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        //{
        //    TreeNode child = new TreeNode();
        //    child.Text = SMName;
        //    child.Value = Smid;
        //    child.SelectAction = TreeNodeSelectAction.Expand;
        //    parent.ChildNodes.Add(child);
        //    child.CollapseAll();
        //}

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();
            if (ViewState["smIDStr"] != null)
            {
                string[] SplitSmid = ViewState["smIDStr"].ToString().Split(',');
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
        //public void FillChildArea(TreeNode parent, string ParentId, int LVL, int SMId)
        //{
        //    //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
        //    var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 order by SMName,lvl";
        //    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //    parent.ChildNodes.Clear();
        //    foreach (DataRow dr in dtChild.Rows)
        //    {
        //        TreeNode child = new TreeNode();
        //        child.Text = dr["SMName"].ToString().Trim();
        //        child.Value = dr["SMId"].ToString().Trim();
        //        child.SelectAction = TreeNodeSelectAction.Expand;
        //        parent.ChildNodes.Add(child);
        //        //child.ExpandAll();
        //        child.CollapseAll();
        //        FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
        //    }

        //}
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
        private static int GetSalesPerId(int uid)
        {
            try
            {
                string getsmIDqry = @"select SMId from MastSalesRep where UserId=" + Settings.Instance.SMID + "";
                DataTable dt_smID = DbConnectionDAL.GetDataTable(CommandType.Text, getsmIDqry);
                return Convert.ToInt32(dt_smID.Rows[0]["SMId"]);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }

        }
        //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "EmpName").ToString()))
        //            {
        //                e.Row.BackColor = Color.Red;
        //            }
        //            if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "doc_id").ToString()))
        //            {
        //                e.Row.BackColor = Color.LightGray;
        //            }

        //            if (gvData.DataKeys[e.Row.DataItemIndex].Values["doc_id"].ToString() != "0")
        //            {
        //                string app = gvData.DataKeys[e.Row.DataItemIndex].Values["app_by"].ToString();
                       
        //                LinkButton lnkApp = (LinkButton)e.Row.FindControl("lnkApp");
        //                LinkButton lnkRejected = (LinkButton)e.Row.FindControl("lnkRejected");

        //                LinkButton lnkDSRPreview = (LinkButton)e.Row.FindControl("lnkEdit");
        //                if (app == "0")
        //                {
        //                    lnkDSRPreview.Text = "Approve";
        //                    lnkApp.Text = "Approve";
        //                    lnkRejected.Text = "Rejected";
        //                }
        //                else
        //                {
        //                    lnkDSRPreview.Text = "";
        //                    lnkApp.Text = "";
        //                    lnkRejected.Text = "";
        //                }
        //                if (gvData.DataKeys[e.Row.DataItemIndex].Values["doc_id"].ToString() == "")
        //                {
        //                    lnkApp.Text = "";
        //                    lnkRejected.Text = "";
        //                    lnkDSRPreview.Text = "";
        //                    e.Row.BackColor = Color.LightGray;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

//        private void GetDailyWorkingSummaryL1(string SPID, string FromDate, string ToDate, string userIDWorkSum)
//        {
//            try
//            {
//                string query = "";
//               // int totalworkingDays = 0;
//                string v_remark = @"select vl1.SMId as srcode,CONVERT (varchar,vl1.VDate,106) as [VisitDate] ,cp.SMName as [Level1], 
//                                0 as [TotalOrder],0 AS DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],
//                                0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as [NewParties],0 as [LocalExpenses],
//                                0 as [TourExpenses],0 as [Demo],0 as TotalParty,vl1.Remark as Remarks,''  as AppRemark,'' as AType,cp.EmpName,'9' as Type
//
//                               from TransVisit vl1 left join MastSalesRep cp on cp.SMId =vl1.SMId 
//                     where vl1.SMId in (" + SPID + ") and vl1.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND vl1.Lock=1 and VDate not  in (" +
//                      "select VDate from TransFailedVisit Tf LEFT JOIN MastParty mp ON Tf.PartyId=mp.PartyId WHERE Tf.SMId in (" + SPID + ") AND mp.PartyDist=0 and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select VDate from TransDemo where SMId in (" + SPID + ") and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select VDate from TransOrder where SMId in (" + SPID + ") and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select paymentDate AS VDate from DistributerCollection where SMId in (" + SPID + ") and paymentDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select paymentDate AS VDate from TransCollection where SMId in (" + SPID + ") and paymentDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select VDate from TransFailedVisit Tf LEFT JOIN MastParty mp ON Tf.PartyId=mp.PartyId WHERE Tf.SMId in (" + SPID + ") AND mp.PartyDist=1 and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' union " +
//                      "select VDate from TransVisitDist WHERE SMId in (" + SPID + ") and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59')";

//                DataTable dt_Visit_Remark = DbConnectionDAL.GetDataTable(CommandType.Text, v_remark);
//                string visit_remark = "";
//                if (dt_Visit_Remark.Rows.Count > 0)
//                {
//                    string vdate = "", vRemark = "", vLevel1 = "", vEmpName = "";
//                    int vsmancode = 0;
//                    for (int vremark = 0; vremark < dt_Visit_Remark.Rows.Count; vremark++)
//                    {
//                        vdate = vLevel1 = vEmpName = vRemark = ""; vsmancode = 0;
//                        vdate = dt_Visit_Remark.Rows[vremark]["VisitDate"].ToString();
//                        vsmancode = Convert.ToInt32(dt_Visit_Remark.Rows[vremark]["srcode"].ToString());
//                        vLevel1 = dt_Visit_Remark.Rows[vremark]["Level1"].ToString();
//                        vEmpName = dt_Visit_Remark.Rows[vremark]["EmpName"].ToString();
//                        vRemark = dt_Visit_Remark.Rows[vremark]["Remarks"].ToString();

//                        visit_remark = visit_remark + @" union all select " + vsmancode + " as srcode,'" + vdate + "' as [VisitDate] ,'" + vLevel1 + "' as [Level1], " +
//                            "0 as [TotalOrder],0 AS DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited]," +
//                            "0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as [NewParties],0 as [LocalExpenses]," +
//                            "0 as [TourExpenses],0 as [Demo],0 as TotalParty,'" + vRemark + "' as Remarks,''  as AppRemark,'' as AType, '" + vEmpName + "' as [EmpName],'9' as Type";
//                    }
//                }

//                query = @"select a.SMID,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,
//                      sum(a.TotalOrder) as TotalOrder,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,
//                     sum(a.PerCallAvgCell) as PerCallAvgCell,sum(a.CallVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as 
//                    [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],
//                    sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],0 as [Collections]
//                   ,MAX(a.TotalParty) as TotalParty,max(a.Remarks) as Remarks,max(a.AppRemark) as AppRemark11,
//                   Max(CASE WHEN Type='9' THEN  
//			 CASE WHEN ( vl1.AppBy IS NULL OR vl1.appby = '0' ) THEN 'Pending'
//                    WHEN ( vl1.appstatus = 'Reject' ) THEN 'Reject'
//                    WHEN ( vl1.appstatus = 'Approve' ) THEN 'Approve' END
//             WHEN Type='8' THEN
//		         CASE WHEN ( AType='Approve' ) THEN 'Approve'
//                    WHEN ( AType='Reject' ) THEN 'Reject'
//                    WHEN ( AType='Pending' ) THEN 'Pending' END
//           END)                       AS AType,
//Max(case when (vl1.AppRemark IS NULL OR vl1.AppRemark='') then vl1.AppRemark else vl1.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type
//from 
//
//(SELECT Sn.SMID,d.VDate,sn.SMName AS Level1,0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],Count(*) as CallVisited,0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as NewParties,0 as [LocalExpenses], 0 [TourExpenses],0 [Demo],0 as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName, '9' AS Type FROM DailyCallvisited d LEFT JOIN mastarea ma ON ma.AreaId=d.BeatId LEFT JOIN MastSalesRep sn ON sn.SMId=d.SMID LEFT JOIN transvisit tv ON tv.VDate=d.VDate WHERE d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND d.SMID in (" + SPID + ") AND tv.Lock=1 GROUP BY d.VDate,sn.SMId,sn.SMName,ma.AreaName,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1, sum(om.OrderAmount) as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited,0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as NewParties,0 as [LocalExpenses],0 [TourExpenses],0 [Demo] ,(select COUNT(p.partyId) as total from MastParty p where p.BeatId=ma.AreaId) as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from TransOrder om inner join MastParty p on p.PartyId=om.PartyId inner join MastSalesRep sn on sn.SMId=om.SMId inner join MastArea ma ON ma.AreaId=p.BeatId where sn.SMId in (" + SPID + ") and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' group by VDate,sn.SMId,sn.SMName,ma.AreaName,ma.AreaId,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1,0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited ,COUNT (*) as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as NewParties,0 as [LocalExpenses], 0 [TourExpenses],0 [Demo] ,0 as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from TransOrder om left join MastSalesRep sn on sn.SMId=om.SMId left join MastParty p on p.PartyId=om.PartyId left join MastArea ma ON ma.AreaId=p.BeatId where om.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and om.SMId in (" + SPID + ") group by VDate,sn.SMId ,sn.SMName,ma.AreaName,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1,0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited ,0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as NewParties,0 as [LocalExpenses],0 [TourExpenses],0 [Demo] ,0 as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from TransOrder om left join MastSalesRep sn on sn.SMId=om.SMId inner join MastParty p on p.PartyId=om.PartyId left join MastArea ma ON ma.AreaId=p.BeatId where om.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and om.SMId in (" + SPID + ") group by VDate,sn.SMId ,sn.SMName,ma.AreaName,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1,0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited, 0 [RetailerProCalls], 0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as NewParties,0 as [LocalExpenses], 0 [TourExpenses],(select case when Count(*)>1 then 1 else 1 end from TransDemo om1 where om1.VDate=om.vdate AND om1.SMId in (" + SPID + ") and om1.AreaId=ma.AreaId and om1.PartyId=p.partyId ) [Demo] ,(select COUNT(p.PartyId) as total from MastParty p where p.BeatId=ma.AreaId) as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type  from TransDemo om inner join MastSalesRep sn on sn.SMId=om.SMId inner join MastParty p on p.PartyId=om.PartyId left join MastArea ma on ma.AreaId=p.BeatId where sn.SMId in (" + SPID + ") and VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' group by VDate,sn.SMId,sn.SMName,ma.AreaName,ma.AreaId,p.PartyId,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1, 0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited ,0 [RetailerProCalls], 0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss ,0 as NewParties,0 as [LocalExpenses],0 [TourExpenses],0 [Demo] ,0 as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from TransDemo om left join MastSalesRep sn on sn.SMId=om.SMId inner join MastParty p on p.PartyId=om.PartyId inner join MastArea ma ON ma.AreaId =p.BeatId where om.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and om.SMId in (" + SPID + ") group by vdate,sn.SMId ,sn.SMName,ma.AreaName,p.PartyId,sn.EmpName " +
//                " UNION ALL select Sn.SMID,VDate,sn.SMName AS Level1,0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as CallVisited ,0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss ,0 as NewParties ,0 as [LocalExpenses] ,0 [TourExpenses],0 [Demo],0 as TotalParty,'' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from TransDemo om left join MastSalesRep sn on sn.SMId=om.SMId inner join MastParty p on p.PartyId=om.PartyId left JOIN mastarea ma on ma.AreaId=p.BeatId where om.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and om.SMId in (" + SPID + ") group by VDate,sn.SMId,sn.SMName,ma.AreaName,p.PartyId,sn.EmpName " +
//                " UNION ALL SELECT Sn.smid,vdate,sn.smname AS Level1,0 AS TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 as [RetailerProCalls],(SELECT Count (*) FROM transfailedvisit om1 LEFT JOIN mastparty p ON p.partyid = om1.partyid WHERE  om1.vdate = om.vdate AND om1.smid IN ( " + SPID + " ) AND p.beatid = ma.areaid AND p.PartyDist=0) AS [FailedVisit],0 AS DistFailVisit,0 as DistDiscuss ,0 AS NewParties,0 AS [LocalExpenses],0 [TourExpenses],0 [Demo],(SELECT Count(p.partyid) AS total FROM   mastparty p WHERE  p.beatid = ma.areaid)     AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM   transfailedvisit om LEFT JOIN mastsalesrep sn ON sn.smid = om.smid INNER JOIN mastparty p ON p.partyid = om.partyid INNER JOIN mastarea ma ON ma.areaid = p.beatid WHERE  om.smid IN ( " + SPID + " ) AND p.PartyDist=0 AND vdate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY vdate, sn.smid, sn.smname, ma.areaname, ma.areaid, sn.empname " +
//                " UNION ALL SELECT Sn.smid,vdate,sn.smname AS Level1,0 AS TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 [RetailerProCalls], 0 AS FailedVisit,(SELECT Count (*) FROM   transfailedvisit om1 LEFT JOIN mastparty p ON p.partyid = om1.partyid WHERE  om1.vdate = om.vdate    AND om1.smid IN ( " + SPID + " ) AND p.cityid = ma.areaid AND p.partydist = 1) AS [DistFailVisit],0 as DistDiscuss,0 AS NewParties,0 AS [LocalExpenses],0 [TourExpenses],0 [Demo],(SELECT Count(p.partyid) AS total FROM   mastparty p WHERE  p.beatid = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM   transfailedvisit om LEFT JOIN mastsalesrep sn ON sn.smid = om.smid INNER JOIN mastparty p ON p.partyid = om.partyid INNER JOIN mastarea ma ON ma.areaid = p.CityId WHERE  om.smid IN ( " + SPID + " ) AND p.PartyDist=1 AND vdate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY vdate,sn.smid,sn.smname,ma.areaname,ma.areaid,sn.empname " +
//                " UNION ALL select Sn.SMID,Convert(VARCHAR(12),p.Created_Date ,106) as VDate,sn.SMName as Level1, 0 as TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell], 0 as CallVisited ,0 as [RetailerProCalls], 0 AS FailedVisit,0 AS [DistFailVisit],0 as DistDiscuss,COUNT(*) as NewParties,0 as [LocalExpenses],0 [TourExpenses], 0 [Demo] , (select COUNT(p.PartyId) as total from MastParty p where p.BeatId=ma.AreaId) as TotalParty, '' as Remarks,''  as AppRemark,'' as AType,sn.EmpName,'9' AS Type from MastParty p LEFT JOIN mastlogin ml ON p.Created_User_id=ml.Id left join MastSalesRep sn ON sn.UserId=ml.Id inner join MastArea ma on ma.AreaId=p.BeatId where p.Created_User_id in (" + userIDWorkSum + ")and (p.Created_Date between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59') group by p.Created_Date,sn.SMId,sn.SMName,ma.AreaName,ma.AreaId,sn.EmpName " +
//             // " UNION ALL SELECT Sn.smid,vdate,sn.smname AS Level1,0 AS TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 [RetailerProCalls], 0 AS FailedVisit,0 AS DistFailVisit,(SELECT Count (*) FROM TransVisitDist om1 LEFT JOIN mastparty p ON p.partyid = om1.DistId WHERE  om1.vdate = om.vdate  AND om1.smid IN ( " + SPID + " ) AND p.cityid = ma.areaid AND p.partydist = 1)AS [DistDiscuss],0 AS NewParties,0 AS [LocalExpenses],0 [TourExpenses],0 [Demo],(SELECT Count(p.partyid) AS total FROM mastparty p WHERE  p.CityId = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM TransVisitDist om LEFT JOIN mastsalesrep sn ON sn.smid = om.smid INNER JOIN mastparty p ON p.partyid = om.DistId INNER JOIN mastarea ma ON ma.areaid = p.CityId WHERE  om.smid IN ( " + SPID + " ) AND p.PartyDist=1 AND vdate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY vdate,sn.smid,sn.smname,ma.areaname,ma.areaid,sn.empname " +
//                " UNION ALL SELECT Sn.smid,om.vdate,sn.smname AS Level1,0 AS TotalOrder,0 as DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 [RetailerProCalls], 0 AS FailedVisit,0 AS DistFailVisit,(SELECT Count (*) FROM TransVisitDist om1 LEFT JOIN mastparty p ON p.partyid = om1.DistId WHERE  om1.vdate = om.vdate  AND om1.smid IN ( " + SPID + " ) AND p.cityid = ma.areaid AND p.partydist = 1)AS [DistDiscuss],0 AS NewParties,0 AS [LocalExpenses],0 [TourExpenses],0 [Demo],(SELECT Count(p.partyid) AS total FROM mastparty p WHERE  p.CityId = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM TransVisitDist om LEFT JOIN mastsalesrep sn ON sn.smid = om.smid INNER JOIN mastparty p ON p.partyid = om.DistId INNER JOIN mastarea ma ON ma.areaid = p.CityId LEFT JOIN Transvisit tv ON tv.VisId=om.VisId WHERE  om.smid IN ( " + SPID + " ) AND p.PartyDist=1 AND tv.Lock=1 AND om.vdate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY om.vdate,sn.smid,sn.smname,ma.areaname,ma.areaid,sn.empname " +
//            //  " UNION ALL SELECT Sn.smid,ExpenseDetails.BillDate [VisitDate],max(sn.smname) AS Level1,0 [TotalOrder], 0 AS DistributorCollection,0 AS PartyCollection,0 [PerCallAvgCell],0 [CallsVisited], 0 [RetailerProCalls], 0 [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 [NewParties],Sum(ExpenseDetails.ClaimAmount)  [LocalExpenses],sum(ExpenseDetails.ApprovedAmount)[TourExpenses],0 [Demo],0 AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,max(sn.empname) as empname,'7' AS Type FROM   (ExpenseDetails LEFT JOIN ExpenseGroup ON ExpenseDetails.ExpenseGroupID=ExpenseGroup.ExpenseGroupID LEFT JOIN mastarea ON ExpenseDetails.cityid = mastarea.areaid INNER JOIN mastsalesrep sn ON sn.smid = ExpenseGroup.SMID ) WHERE  ExpenseDetails.BillDate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND sn.smid IN ( " + SPID + " ) GROUP  BY ExpenseDetails.BillDate,sn.smid " +
//                " UNION ALL SELECT Sn.smid,ExpenseDetails.BillDate [VisitDate],max(sn.smname) AS Level1,0 [TotalOrder], 0 AS DistributorCollection,0 AS PartyCollection,0 [PerCallAvgCell],0 [CallsVisited], 0 [RetailerProCalls], 0 [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 [NewParties],Sum(ExpenseDetails.ClaimAmount)  [LocalExpenses],sum(ExpenseDetails.ApprovedAmount)[TourExpenses],0 [Demo],0 AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,max(sn.empname) as empname,'7' AS Type FROM   (ExpenseDetails LEFT JOIN ExpenseGroup ON ExpenseDetails.ExpenseGroupID=ExpenseGroup.ExpenseGroupID LEFT JOIN mastarea ON ExpenseDetails.cityid = mastarea.areaid INNER JOIN mastsalesrep sn ON sn.smid = ExpenseGroup.SMID LEFT JOIN transvisit tv ON tv.VDate =ExpenseDetails.BillDate) WHERE  ExpenseDetails.BillDate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND sn.smid IN ( " + SPID + " ) AND tv.Lock=1 GROUP  BY ExpenseDetails.BillDate,sn.smid " +
//            //  " UNION ALL SELECT Sn.smid,PaymentDate AS vdate,sn.smname AS Level1,0 AS TotalOrder,Sum(dc.Amount) AS DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 AS [RetailerProCalls],0 AS [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 AS NewParties,0 AS [LocalExpenses],0 AS [TourExpenses],0 AS [Demo],(SELECT Count(p.partyid) AS total FROM   mastparty p WHERE  p.beatid = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM   DistributerCollection dc INNER JOIN mastparty p ON p.partyid = dc.DistId INNER JOIN mastsalesrep sn ON sn.smid = dc.SMId INNER JOIN mastarea ma ON ma.areaid = p.CityId WHERE  sn.smid IN ( " + SPID + " ) AND p.PartyDist=1 AND PaymentDate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY PaymentDate,sn.smid,sn.smname,ma.areaname,ma.areaid,sn.empname " +
//                " UNION ALL SELECT Sn.smid,PaymentDate AS vdate,sn.smname AS Level1,0 AS TotalOrder,Sum(dc.Amount) AS DistributorCollection,0 AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 AS [RetailerProCalls],0 AS [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 AS NewParties,0 AS [LocalExpenses],0 AS [TourExpenses],0 AS [Demo],(SELECT Count(p.partyid) AS total FROM   mastparty p WHERE  p.beatid = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM   DistributerCollection dc INNER JOIN mastparty p ON p.partyid = dc.DistId INNER JOIN mastsalesrep sn ON sn.smid = dc.SMId INNER JOIN mastarea ma ON ma.areaid = p.CityId LEFT JOIN Transvisit tv ON tv.VDate = dc.PaymentDate WHERE  sn.smid IN ( " + SPID + " ) AND p.PartyDist=1 and tv.lock=1 AND PaymentDate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY PaymentDate,sn.smid,sn.smname,ma.areaname,ma.areaid,sn.empname " +
//            //  " UNION ALL SELECT Sn.smid,PaymentDate AS vdate,sn.smname AS Level1,0 AS TotalOrder,0 AS DistributorCollection,Sum(tc.Amount) AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 AS [RetailerProCalls],0 AS [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 AS NewParties,0 AS [LocalExpenses],0 AS [TourExpenses],0 AS [Demo],(SELECT Count(p.partyid) AS total FROM mastparty p WHERE  p.AreaId = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM TransCollection tc INNER JOIN mastparty p ON p.partyid = tc.PartyId INNER JOIN mastsalesrep sn ON sn.smid = tc.SMId INNER JOIN mastarea ma ON  ma.areaid = p.AreaId WHERE sn.smid IN ( " + SPID + " ) AND p.PartyDist=0 AND PaymentDate  BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY PaymentDate,sn.smid,sn.smname,ma.areaname,ma.areaid, sn.empname " + visit_remark + ") " +
//                " UNION ALL SELECT Sn.smid,tc.VDate,sn.smname AS Level1,0 AS TotalOrder,0 AS DistributorCollection,Sum(tc.Amount) AS PartyCollection,0 AS [PerCallAvgCell],0 AS CallVisited,0 AS [RetailerProCalls],0 AS [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 AS NewParties,0 AS [LocalExpenses],0 AS [TourExpenses],0 AS [Demo],(SELECT Count(p.partyid) AS total FROM mastparty p WHERE  p.AreaId = ma.areaid) AS TotalParty,'' AS Remarks,'' AS AppRemark,'' AS AType,sn.empname,'9' AS Type FROM TransCollection tc INNER JOIN mastparty p ON p.partyid = tc.PartyId INNER JOIN mastsalesrep sn ON sn.smid = tc.SMId INNER JOIN mastarea ma ON  ma.areaid = p.AreaId LEFT JOIN Transvisit tv ON tv.VisId=tc.VisId WHERE sn.smid IN ( " + SPID + " ) AND p.PartyDist=0 AND tv.Lock=1 AND tc.VDate BETWEEN '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' GROUP  BY tc.VDate,sn.smid,sn.smname,ma.areaname,ma.areaid, sn.empname " + visit_remark + ") " +
//                " a left  join TransVisit vl1 on vl1.SMId= a.SMID and vl1.VDate=a.VDate  Group by a.VDate,a.SMID,a.Level1,vl1.AppRemark,a.smid,a.EmpName Order by a.VDate";
                                               
//                DataTable gdt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
//                if (gdt.Rows.Count > 0)
//                {
//                    ViewState["GridData"] = gdt;
//                    ViewState["UserId"] = userIDWorkSum;
//                    gvData.DataSource = gdt;
//                    gvData.DataBind();
//                }
//                else
//                {
//                    gvData.DataSource = null;
//                    gvData.DataBind();
//                }
//            }
//            catch (Exception ex)
//            {
//                ex.ToString();
//            }

//        }

        private void GetDailyWorkingSummaryL1(string SPID, string userIDWorkSum)
        {
            try
            {
                string query = "";
                string QryChk = "";
                QryChk = "where a.lock1 =1 and a.Lock2=1";

                query = @"select a.SMID,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,(a.SyncId) as SyncId,sum(a.TotalOrder) as TotalOrder,
                sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,
                sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],
               sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],
               sum(a.DistDiscuss) as DistDiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],
               sum(a.Competitor) [Competitor],0 as [Collections],MAX(a.TotalParty) as TotalParty,max(a.Remarks) as Remarks,
               max(a.AppRemark) as AppRemark11, AType= (case when max(a.AType) is null then 'Pending' else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,
               a.EmpName,  max(a.type) AS type,max(a.RoleType) as RoleType  from ( " +
                 "SELECT View_DSR.SMID,View_DSR.VDate,View_DSR.Level1 AS Level1,View_DSR.SyncId,View_DSR.TotalOrder as TotalOrder,View_DSR.OrderAmountMail as OrderAmountMail, View_DSR.OrderAmountPhone as OrderAmountPhone,View_DSR.DistributorCollection as DistributorCollection,View_DSR.PartyCollection AS PartyCollection,View_DSR.PerCallAvgCell as [PerCallAvgCell],View_DSR.CallsVisited as [CallsVisited],View_DSR.RetailerProCalls as [RetailerProCalls],View_DSR.FailedVisit as [FailedVisit],View_DSR.DistFailVisit as DistFailVisit,View_DSR.DistDiscuss as DistDiscuss,View_DSR.NewParties as NewParties,View_DSR.LocalExpenses as [LocalExpenses], View_DSR.TourExpenses as [TourExpenses],View_DSR.Demo as [Demo],View_DSR.Competitor AS Competitor,View_DSR.TotalParty as TotalParty,View_DSR.Remarks as Remarks,View_DSR.AppRemark as AppRemark,View_DSR.AType as AType,View_DSR.EmpName as EmpName,View_DSR.Type AS Type, View_DSR.Lock1,View_DSR.Lock2,View_DSR.RoleType FROM View_DSR WHERE View_DSR.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SPID + ")) AND mr.RoleType IN ('AreaIncharge')) and View_DSR.Atype is null) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName Order by a.VDate"; 

                DataTable gdt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (gdt.Rows.Count > 0)
                {
                    ViewState["GridData"] = gdt;
                    ViewState["smIDStr"] = SPID;
                    ViewState["UserId"] = userIDWorkSum;                  
                    rpt.DataSource = gdt;
                    rpt.DataBind();
                }
                else
                {                   
                    rpt.DataSource=null;
                    rpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private void GetDailyWorkingSummaryL1New(int SPID)
        {
            try
            {
                string query = "";
                string QryChk = "";
                QryChk = "where a.lock1 =1 and a.Lock2=1";           

                query = @"select a.SMID,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,(a.SyncId) as SyncId,sum(a.TotalOrder) as TotalOrder,
                sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,
                sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],
               sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],
               sum(a.DistDiscuss) as DistDiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],
               sum(a.Competitor) [Competitor],0 as [Collections],MAX(a.TotalParty) as TotalParty,max(a.Remarks) as Remarks,
               max(a.AppRemark) as AppRemark11, AType= (case when max(a.AType) is null then 'Pending' else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,
               a.EmpName,  max(a.type) AS type,max(a.RoleType) as RoleType  from ( " +
               "SELECT View_DSR.SMID,View_DSR.VDate,View_DSR.Level1 AS Level1,View_DSR.SyncId,View_DSR.TotalOrder as TotalOrder,View_DSR.OrderAmountMail as OrderAmountMail, View_DSR.OrderAmountPhone as OrderAmountPhone,View_DSR.DistributorCollection as DistributorCollection,View_DSR.PartyCollection AS PartyCollection,View_DSR.PerCallAvgCell as [PerCallAvgCell],View_DSR.CallsVisited as [CallsVisited],View_DSR.RetailerProCalls as [RetailerProCalls],View_DSR.FailedVisit as [FailedVisit],View_DSR.DistFailVisit as DistFailVisit,View_DSR.DistDiscuss as DistDiscuss,View_DSR.NewParties as NewParties,View_DSR.LocalExpenses as [LocalExpenses], View_DSR.TourExpenses as [TourExpenses],View_DSR.Demo as [Demo],View_DSR.Competitor AS Competitor,View_DSR.TotalParty as TotalParty,View_DSR.Remarks as Remarks,View_DSR.AppRemark as AppRemark,View_DSR.AType as AType,View_DSR.EmpName as EmpName,View_DSR.Type AS Type, View_DSR.Lock1,View_DSR.Lock2,View_DSR.RoleType FROM View_DSR WHERE View_DSR.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SPID + ")) AND mr.RoleType IN ('AreaIncharge')) and View_DSR.Atype is null) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName Order by a.VDate"; 



                DataTable gdt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (gdt.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    ViewState["GridData"] = gdt;
                    ViewState["smIDStr"] = SPID;
                    rpt.DataSource = gdt;
                    rpt.DataBind();
                }
                else
                {
                   
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            rptmain.Style.Add("display", "block");
            string smIDStr = "";
            string smIDStr1 = "",  userIdStr = "";           
            //ViewState["smIDStr"] = "";

            //         string message = "";
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

            if (smIDStr1 == "")
            {
                DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtSMId);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";

                if (dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                    
                    
                }
            }

            //GetUserID
            //string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (" + smIDStr1 + ")";
            //string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.smid IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")  AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))) AND mr.roletype='AreaIncharge'))";
            string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) AND mr.roletype='AreaIncharge')";             
            DataTable userdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepUserIdQry);
            if (userdt.Rows.Count > 0)
            {
                for (int i = 0; i < userdt.Rows.Count; i++)
                {
                    userIdStr += userdt.Rows[i]["UserId"] + ",";
                }
            }
            userIdStr = userIdStr.TrimStart(',').TrimEnd(',');          

            ViewState["smIDStr"] = smIDStr1;
            GetDailyWorkingSummaryL1(smIDStr1, userIdStr);
                    
        }

        //protected void Save_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (TextArea1.Value != "")
        //        {
        //            string transVisitQry = @"update TransVisit set AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + smID + ", AppRemark='" + TextArea1.Value + "',AppStatus='" + approveStatusRadioButtonList.SelectedValue + "' where SMId=" + Convert.ToInt32(visHdf.Value) + " and VDate='"+Settings.dateformat(dateHdf.Value)+"'";
        //            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, transVisitQry);
        //            if (ViewState["smIDStr"].ToString() != "")
        //            {
        //                //GetDailyWorkingSummaryL1(ViewState["smIDStr"].ToString(), frmTextBox.Text, toTextBox.Text, ViewState["UserId"].ToString());
        //                  GetDailyWorkingSummaryL1(ViewState["smIDStr"].ToString(), ViewState["UserId"].ToString());
        //            }
        //            else if(Request.QueryString["SMId"] != null)
        //            {
        //                GetDailyWorkingSummaryL1New(Convert.ToInt32(Request.QueryString["SMId"]));

        //            }
        //            ShowAlert("Record Updated Successfully");
        //        }
        //        else
        //        {
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please Enter Remark');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //}


        [System.Web.Services.WebMethod]
        public static void UpdateDSR(string smid, string Vdate, string status, string Remark)
        {
            //return string.Format("Name: {0}{2}Age: {1}", smid, status, Environment.NewLine);
            try
            {
                string transVisitQry = @"update TransVisit set AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ", AppRemark='" + Remark + "',AppStatus='" + status + "' where SMId=" + Convert.ToInt32(smid) + " and VDate='" + Settings.dateformat(Vdate) + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, transVisitQry);
                //GetDailyWorkingApprovalL3(ViewState["smIDStr"].ToString(), ViewState["UserId"].ToString());
                //if (ViewState["smIDStr"].ToString() != "")
                //{                        
                //    GetDailyWorkingApprovalL3(ViewState["smIDStr"].ToString(), ViewState["UserId"].ToString());
                //}



            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            //       visHdf.Value = hdnVisItCode.Value;
            
        //    this.ModalPopupExtender4.Show();
        }

        //protected void Close_Click(object sender, EventArgs e)
        //{

        //}

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingApprovalL1.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PendingDSRList.aspx?SMId=" + Request.QueryString["SMId"]);
        }

        string visitDate = "";
        //protected void gvData_RowDataBound2(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "doc_id").ToString()))
        //            {
        //                e.Row.BackColor = Color.LightGray;
        //            }

        //            if (gvData.DataKeys[e.Row.DataItemIndex].Values["id"].ToString() != "0")
        //            {
        //                string app = gvData.DataKeys[e.Row.DataItemIndex].Values["app_by"].ToString();
        //                visitDate = gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString();
        //                //           LinkButton lnkApp = (LinkButton)e.Row.FindControl("lnkApp");
        //                //           LinkButton lnkRejected = (LinkButton)e.Row.FindControl("lnkRejected");

        //                LinkButton lnkDSRPreview = (LinkButton)e.Row.FindControl("lnkEdit");
        //                if (app == "0")
        //                {
        //                    lnkDSRPreview.Text = "Approve/Reject";
        //                    //                    lnkApp.Text = "Approve";
        //                    //                    lnkRejected.Text = "Rejected";
        //                }
        //                else
        //                {
        //                    lnkDSRPreview.Text = "";
        //                    //                    lnkApp.Text = "";
        //                    //                    lnkRejected.Text = "";
        //                }
        //                if (gvData.DataKeys[e.Row.DataItemIndex].Values["doc_id"].ToString() == "")
        //                {
        //                    //                       lnkApp.Text = "";
        //                    //                       lnkRejected.Text = "";
        //                    lnkDSRPreview.Text = "";
        //                    e.Row.BackColor = Color.LightGray;
        //                }
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        //protected void gvData_RowDataBound2(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
               
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "VDate").ToString()))
        //            {
        //                e.Row.BackColor = Color.LightGray;
        //            }

        //            HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
        //            Label lblType = (Label)e.Row.FindControl("lblType");

        //            if (hdnType.Value == "9")
        //            {
        //                lblType.Text = "DSR";
        //            }
        //            else if (hdnType.Value == "8")
        //            {
        //                lblType.Text = "LEAVE";
        //            }
        //            else if (hdnType.Value == "7")
        //            {
        //                lblType.Text = "EXPENSE";
        //            }
        //            else
        //            {
        //                lblType.Text = "HOLIDAY";
        //            }


        //            if (gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString() != "0")
        //            {
        //                string atype = gvData.DataKeys[e.Row.DataItemIndex].Values["AType"].ToString();
        //                visitDate = gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString();                       

        //                string type = DataBinder.Eval(e.Row.DataItem, "AType").ToString();
        //                if(type=="Pending" )
        //                {
        //                    e.Row.Visible = true;
        //                }
        //                else
        //                {
        //                    e.Row.Visible = false;
        //                }

        //                LinkButton lnkDSRPreview = (LinkButton)e.Row.FindControl("lnkEdit");
        //                if (atype == "Pending")
        //                {
        //                    lnkDSRPreview.Text = "Approve/Reject";
                           
        //                }
        //                else
        //                {
        //                    lnkDSRPreview.Text = "";
                           
        //                }
        //                if (gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString() == "")
        //                {
                           
        //                    lnkDSRPreview.Text = "";
        //                    e.Row.BackColor = Color.LightGray;
        //                }
        //            }
        //            Label lblCallVisited = (Label)e.Row.FindControl("lblCallsVisited");
        //            Label lblPerCallAvgSale = (Label)e.Row.FindControl("lblPerCallAvg");
        //            Label lblTotalSale = (Label)e.Row.FindControl("lblTotalOrder");
        //            decimal sale = Decimal.Parse(lblTotalSale.Text);                    
        //            decimal Callvisited = Decimal.Parse(lblCallVisited.Text);                   

        //            if (Callvisited != 0)
        //            {
        //                decimal PerCallAvgSale = decimal.Parse((sale / Callvisited).ToString()); //Decimal.Parse(lblPerCallAvgSale.Text);                      
        //                lblPerCallAvgSale.Text = PerCallAvgSale.ToString("#.##");
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "select")
            //{
            //    string[] commandArgs = new string[2];
            //    commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            //    visHdf.Value = commandArgs[0];
            //    dateHdf.Value = commandArgs[1];
            //    this.ModalPopupExtender4.Show();
            //}
            //if (e.CommandName == "selectDate")
            //{
            //    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            //    int index = gvRow.RowIndex;
            //    GridViewRow row = gvData.Rows[index];
            //    HiddenField hdnDate = (HiddenField)row.FindControl("hdnDate");
            //    HiddenField hdnSmiD = (HiddenField)row.FindControl("hdnSMId");
               
            //    Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&PAGE=APPROVAL-L1");
            //}

        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvData.PageIndex = e.NewPageIndex;
            //gvData.DataSource = ViewState["GridData"];
            //gvData.DataBind();
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnType = (HiddenField)e.Item.FindControl("hdnType");
                Label lblType = (Label)e.Item.FindControl("lblType");
                HiddenField hdnAType = (HiddenField)e.Item.FindControl("hdnAType");
                if (hdnType.Value == "9")
                {
                    lblType.Text = "DSR";
                }
                else if (hdnType.Value == "8")
                {
                    lblType.Text = "LEAVE";
                }
                else if (hdnType.Value == "7")
                {
                    lblType.Text = "EXPENSE";
                }
                else
                {
                    lblType.Text = "HOLIDAY";
                }
                
                    //string hdnType = gvData.DataKeys[e.Row.DataItemIndex].Values["AType"].ToString();
                    //visitDate = gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString();

                    //string type = DataBinder.Eval(e.Row.DataItem, "AType").ToString();
                if (hdnAType.Value == "Pending" || hdnAType.Value == "")
                    {
                        e.Item.Visible = true;
                    }
                    else
                    {
                        e.Item.Visible = false;
                    }

                LinkButton lnkDSRPreview = (LinkButton)e.Item.FindControl("lnkEdit");
                if (hdnAType.Value == "Pending" || hdnAType.Value == "")
                    {
                        lnkDSRPreview.Text = "Approve/Reject";

                    }
                    else
                    {
                        lnkDSRPreview.Text = "";

                    }
                    //if (gvData.DataKeys[e.Row.DataItemIndex].Values["VDate"].ToString() == "")
                    //{

                    //    lnkDSRPreview.Text = "";
                    //    e.Row.BackColor = Color.LightGray;
                    //}
                

            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                string[] commandArgs = new string[2];
                commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                visHdf.Value = commandArgs[0];
                dateHdf.Value = commandArgs[1];
                AjaxControlToolkit.ModalPopupExtender mp1 = ((AjaxControlToolkit.ModalPopupExtender)(e.Item.FindControl("ModalPopupExtender4")));
                TextArea1.Value = string.Empty;
                mp1.Show();
            }
            if (e.CommandName == "selectDate")
            {
                RepeaterItem gvRow = (RepeaterItem)(((LinkButton)e.CommandSource).NamingContainer);
                int index = gvRow.ItemIndex;
                RepeaterItem row = rpt.Items[index];
                HiddenField hdnDate = (HiddenField)row.FindControl("hdnDate");
                HiddenField hdnSmiD = (HiddenField)row.FindControl("hdnSMId");
                //           int smId = Convert.ToInt32(gvData.DataKeys[index].Values[0]);
                //           string date = gvData.DataKeys[index].Values[0].ToString();
                Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&PAGE=APPROVAL-L1");
            }

        }


        //protected void rptDSRSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        HiddenField hfIsNewTab = (HiddenField)e.Item.FindControl("appstatusHiddenField");
        //        LinkButton lbldescriptionlink = (LinkButton)e.Item.FindControl("LinkButton1");
        //        if (hfIsNewTab.Value == "Approve" || hfIsNewTab.Value == "Reject")
        //        {
        //            lbldescriptionlink.Visible = false;
        //        }
        //        else
        //        {
        //            lbldescriptionlink.Visible = true;
        //        }
        //    }

        //}
    }
}