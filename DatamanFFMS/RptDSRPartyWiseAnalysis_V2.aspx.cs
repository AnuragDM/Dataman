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
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class RptDSRPartyWiseAnalysis_V2 : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            frmTextBox.Attributes.Add("readonly", "readonly");
            toTextBox.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                Bindstate();
                frmTextBox.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                roleType = Settings.Instance.RoleType;
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");

                BindTreeViewControl();
            }
        }
        private void Bindstate()
        {
            try
            {
                if (Settings.Instance.RoleType == "Admin")
                {
                    string stateQry = @"select distinct stateid,statename from viewgeo where stateAct=1 order by statename";
                    DataTable dtMastState = DbConnectionDAL.GetDataTable(CommandType.Text, stateQry);
                    if (dtMastState.Rows.Count > 0)
                    {

                        ddlState.DataSource = dtMastState;
                        ddlState.DataTextField = "statename";
                        ddlState.DataValueField = "stateid";
                        ddlState.DataBind();
                    }

                }
                else
                {
                    string stateQry = @"select distinct stateid,statename from viewgeo where stateAct=1 and areaid in (SELECT distinct linkcode FROM MastLink WHERE primcode in
                   (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN ('" + Settings.Instance.SMID + "')) and Active=1)) order by statename";
                    DataTable dtMastState = DbConnectionDAL.GetDataTable(CommandType.Text, stateQry);
                    if (dtMastState.Rows.Count > 0)
                    {
                        ddlState.DataSource = dtMastState;
                        ddlState.DataTextField = "statename";
                        ddlState.DataValueField = "stateid";
                        ddlState.DataBind();
                    }

                }

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



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDSRPartyWiseAnalysis_V2.aspx", true);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", PartyId = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                if (node.Checked)
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr = smIDStr.TrimStart(',').TrimEnd(',');

            foreach (ListItem item in ddlParty.Items)
            {
                if (item.Selected)
                {
                    PartyId += item.Value + ",";
                }
            }
            PartyId = PartyId.TrimStart(',').TrimEnd(',');


            if (rblview.SelectedValue == "SalesPerson")
            {
                PartyId = "0";
                if (smIDStr == "") System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select salesperson');", true);
            }
            else if (rblview.SelectedValue == "Party")
            {
                if (PartyId == "") System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Party');", true);
            }

            if (Convert.ToDateTime(frmTextBox.Text) <= Convert.ToDateTime(toTextBox.Text))
            {

                DSR_PartyWiseAnlaysis(smIDStr, PartyId);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);

            }

        }


        private void DSR_PartyWiseAnlaysis(String Smid, string PartyId)
        {
            #region Variable Declaration
            string str = "", dsrQuery = "", QryChk = "", pQry = "", Visid = "", prty = "";
            DataTable gvdt = new DataTable();
            DataTable dtRecord = new DataTable();
            DataTable dtdesig = new DataTable();
            DataTable dtItem = new DataTable();
            DataTable dtsr = new DataTable();
            DataRow mDataRow;
            #endregion

            try
            {
                if (ddlDsrType.SelectedItem.Text == "Lock")
                { QryChk = "and tv.lock =1"; }
                if (ddlDsrType.SelectedItem.Text == "UnLock")
                { QryChk = "and TV.lock = 0"; }
                if (ddlDsrType.SelectedItem.Text == "All")
                { QryChk = "and TV.lock  in (1,0)"; }

                if (ddlStatus.SelectedValue != "3")
                {
                    if (ddlStatus.SelectedItem.Text == "Pending")
                    { QryChk += " and TV.AppStatus is null "; }
                    else
                    { QryChk += " and TV.AppStatus='" + ddlStatus.SelectedValue + "'"; }
                }

                if (ddlSType.SelectedValue != "All")
                {
                    if (ddlSType.SelectedValue == "Active")
                    { QryChk += "and isnull(emp.Active,0) =1"; }
                    else
                    { QryChk += "and isnull(emp.Active,0) =0"; }
                }
                if (ddlPType.SelectedValue != "All")
                {
                    if (ddlPType.SelectedValue == "Active")
                    { pQry = "  and PActive ='Active'"; }
                    else
                    { pQry = "  and PActive ='Deactive'"; }
                }


                string CityId = "0";

                if (rblview.SelectedValue == "Party")
                {


                    foreach (ListItem item in ddlCity.Items)
                    {
                        if (item.Selected)
                        {
                            CityId += item.Value + ",";
                        }
                    }
                    CityId = CityId.TrimStart(',').TrimEnd(',');


                    if (CityId != "0" && PartyId == "")
                    {
                        string partyQry = @"select STUFF( (SELECT ', ' +partyid from mastparty  WHERE cityid IN ('" + CityId + "') AND partydist=0 FOR XML PATH ('')), 1, 1, '')  AS partyId";
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                        if (dtRecord.Rows.Count > 0)
                        {
                            PartyId = dtRecord.Rows[0]["partyid"].ToString();

                        }


                    }
                    if (PartyId != "")
                    {
                        pQry += "  and a.PID in (" + PartyId.TrimStart(',').TrimEnd(',') + ") ";

                        prty += " and tcv.PartyId in (" + PartyId + ") ";
                    }

                }
                else
                {
                    QryChk = "and TV.smid  in (" + Smid + ")";
                }



                gvdt.Columns.Add(new DataColumn("Date", typeof(String)));

                str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";
                dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }


                gvdt.Columns.Add(new DataColumn("Region", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Joint Working User", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Outlet Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Outlet Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Outlet Address", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Outlet City", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Transaction Type", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Remark", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Product Class", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Product Segment", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Product Group", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Qnty", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Discount", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Value", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Net Value", typeof(String)));
                gvdt.Columns.Add(new DataColumn("NextVisit Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("NextVisit Time", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Competitor Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Competitor Item", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Competitor Stock", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Brand Activity", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Meet Activity", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Other Activity", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Other GeneralInfo", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Time Spent (In Minutes)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Road Show", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Scheme/offers", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Geo Address", typeof(String)));

                str = "select (itemname+'-'+IsNull(SyncId,'')) as itemname from mastitem where itemtype='ITEM' order by itemname ";
                dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtItem.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItem.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtItem.Rows[i]["itemname"].ToString(), typeof(String)));
                        gvdt.Columns.Add(new DataColumn(dtItem.Rows[i]["itemname"].ToString() + "1", typeof(String)));
                    }
                }

                dsrQuery = "select  VisId,TV.SMId,VDate,DSR_Type,ISNULL(AppStatus,'') as Status,Remark,(MSP.SMNAME+'-'+IsNull(MSP.SyncId,'')) AS WITHwHOM, emp.SMId,emp.SMName,emp.SyncId,emp.Mobile,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end Active,replace(convert(varchar,emp.CreatedDate,106),' ','/') CreatedDate,mdst.DesName,isnull(hq.HeadquarterName,'')HeadquarterName,(sr.smname+'-'+IsNull(sr.SyncId,'')) as reportingPerson,(mr.areaname+'-'+IsNull(mr.SyncId,'')) as Region   from TransVisit TV LEFT JOIN MASTSALESREP MSP ON MSP.SMID=TV.WithUserId left join mastsalesrep emp on emp.smid=tv.smid left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  left join mastarea mct on mct.areaid=emp.cityid left join mastarea md on md.areaid=mct.underid  left join  mastarea ms on ms.areaid=md.underid left join mastarea mr on mr.areaid=ms.underid where  mct.areatype='CITY' and md.areatype='DISTRICT' and ms.areatype='STATE' and mr.areatype='REGION' and tv.vdate  between '" + Settings.dateformat(frmTextBox.Text) + " 00:00' and '" + Settings.dateformat(toTextBox.Text) + " 23:59'  " + QryChk + "   order by TV.vdate,TV.SMId";
                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                if (dsrData.Rows.Count > 0)
                {
                    DataView dvdsrData = new DataView(dsrData);

                    foreach (DataRow drvst in dsrData.Rows)
                    {

                        Visid = drvst["VisId"].ToString();

                        #region Order
                        str = " select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.orddocid,case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.Remarks,os.[address],'Order' as Tran_type,os.TotalDiscount FROM TransOrder os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId union all SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.orddocid, case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.Remarks,os.[address],'Order' as Tran_type,os.TotalDiscount FROM Temp_TransOrder os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId) a  where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }


                                mDataRow["Date"] = Convert.ToDateTime(dr["Mobile_Created_date"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Remark"] = dr["Remarks"].ToString();
                                mDataRow["Total Discount"] = dr["TotalDiscount"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();

                                str = "select a.itemname,a.qnty,a.Amount from (select tr1.ItemId,(itm.ItemName +'-'+IsNull(itm.SyncId,'')) as itemname,sum(tr1.qty) qnty,sum (tr1.qty*tr1.Rate) Amount from transorder tr left join transvisit tv on tr.visid=tv.visid left join transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId left join MastItem itm on itm.ItemId=tr1.ItemId  where  tr.OrdDocId='" + dr["OrdDocId"] + "'  group by tr1.ItemId ,itm.ItemName,itm.SyncId  union select tr1.ItemId,(itm.ItemName +'-'+IsNull(itm.SyncId,'')) as itemname,sum(tr1.qty) qnty,sum (tr1.qty*tr1.Rate) Amount  from tEMP_transorder tr left join transvisit tv on tr.visid=tv.visid left join tEMP_transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId left join MastItem itm on itm.ItemId=tr1.ItemId where tr.OrdDocId='" + dr["OrdDocId"] + "'   group by tr1.ItemId,itm.ItemName,itm.SyncId) a ";
                                dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtItem.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtItem.Rows.Count; i++)
                                    {
                                        mDataRow[dtItem.Rows[i]["ItemName"].ToString()] = dtItem.Rows[i]["qnty"].ToString();
                                        mDataRow[dtItem.Rows[i]["ItemName"].ToString() + "1"] = dtItem.Rows[i]["Amount"].ToString();
                                    }
                                }

                                str = "select sum(qty) qty,sum(OrderAmount) OrderAmount,sum(NetAmount) NetAmount from ( select sum(tr1.qty) as qty , max(OrderAmount) OrderAmount,max(NetAmount) NetAmount from transorder tr  left join transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId  where  tr.OrdDocId='" + dr["OrdDocId"] + "'  GROUP BY TR1.OrdDocId union select sum(tr1.qty) as qty , max(OrderAmount) OrderAmount,max(NetAmount) NetAmount from Temp_transorder tr left join transvisit tv on tr.visid=tv.visid left join Temp_transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where tr.OrdDocId='" + dr["OrdDocId"] + "'  GROUP BY TR1.OrdDocId) a ";
                                DataTable dtOrder = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtOrder.Rows.Count > 0)
                                {
                                    mDataRow["Qnty"] = dtOrder.Rows[0]["qty"].ToString();
                                    mDataRow["Total Value"] = dtOrder.Rows[0]["OrderAmount"].ToString();
                                    mDataRow["Net Value"] = dtOrder.Rows[0]["NetAmount"].ToString();
                                }

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }


                        #endregion

                        #region Demo
                        str = "select  * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat, os.[address],'Demo' as Tran_type FROM Transdemo os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId union  SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid,  os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat,os.[address],'Demo' as Tran_type FROM Temp_Transdemo os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId) a  where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }


                                mDataRow["Date"] = Convert.ToDateTime(dr["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();

                                str = "select STUFF((SELECT ','+name  from ( select pc.name from ( SELECT ProductClassId from Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + "  union all SELECT ProductClassId from Temp_Transdemo where visid=133 and partyid=76) a left join MastItemClass pc on pc.id=a.ProductClassId)a   FOR XML PATH ('')), 1, 1, '')  AS PrdClass,STUFF((SELECT ','+proSeg  from (select pc.name proSeg from ( SELECT ProductSegmentId from Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + "  union all SELECT ProductSegmentId from Temp_Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + ") a left join MastItemSegment pc on pc.id=a.ProductSegmentId  )a   FOR XML PATH ('')), 1, 1, '')  AS proSeg,STUFF((SELECT ','+proGrp  from (select pc.itemname  proGrp from ( SELECT ProductMatGrp from Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + "  union all SELECT ProductMatGrp from Temp_Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + " ) a left join MastItem pc on pc.itemid=a.ProductMatGrp)a   FOR XML PATH ('')), 1, 1, '')  AS proGrp,STUFF((SELECT ','+Remarks  from (select a.Remarks from ( SELECT Remarks from Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + " union all SELECT Remarks from Temp_Transdemo where visid=" + dr["visid"].ToString() + " and partyid=" + dr["PID"].ToString() + " ) a )a   FOR XML PATH ('')), 1, 1, '')  AS Remarks ";
                                dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtItem.Rows.Count > 0)
                                {
                                    mDataRow["Remark"] = dtItem.Rows[0]["Remarks"].ToString();
                                    mDataRow["Product Class"] = dtItem.Rows[0]["PrdClass"].ToString();
                                    mDataRow["Product Segment"] = dtItem.Rows[0]["proSeg"].ToString();
                                    mDataRow["Product Group"] = dtItem.Rows[0]["proGrp"].ToString();
                                }

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }


                        #endregion

                        #region FailedVisit
                        str = "select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.FVDocId,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.Nextvisit, 106) AS NextVisitDate, os.VisitTime AS NextVisitTime FROM TransFailedVisit os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId WHERE ISNULL(PARTYDIST,0)=0 union all SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.FVDocId,os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active, 0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.Nextvisit, 106) AS NextVisitDate, os.VisitTime AS NextVisitTime FROM Temp_TransFailedVisit os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId = os.PartyId WHERE ISNULL(PARTYDIST,0)= 0) a where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            if (dtRecord.Rows.Count > 0)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtRecord.Rows[0]["SMId"].ToString() + "  and MainGrp<>" + dtRecord.Rows[0]["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }

                                mDataRow["Date"] = Convert.ToDateTime(dtRecord.Rows[0]["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dtRecord.Rows[0]["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dtRecord.Rows[0]["Party"].ToString();
                                mDataRow["Outlet Status"] = dtRecord.Rows[0]["PActive"].ToString();
                                mDataRow["Outlet Address"] = dtRecord.Rows[0]["Address1"].ToString();
                                mDataRow["Outlet City"] = dtRecord.Rows[0]["Beat"].ToString();
                                mDataRow["Transaction Type"] = dtRecord.Rows[0]["Tran_type"].ToString();
                                mDataRow["Remark"] = dtRecord.Rows[0]["Remarks"].ToString();
                                mDataRow["Geo Address"] = dtRecord.Rows[0]["address"].ToString();

                                mDataRow["NextVisit Date"] = dtRecord.Rows[0]["NextVisitDate"].ToString();
                                mDataRow["NextVisit Time"] = dtRecord.Rows[0]["NextVisitTime"].ToString();

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }

                        #endregion

                        #region Competitor
                        str = "select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.docid,case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat, os.Remarks,os.[address],'Competitor' as Tran_type,os.item as CompItm,IsNull(os.stock,'') as CompStock,os.qty as CompQnty, os.rate, os.Compname, os.BrandActivity, os.MeetActivity,os.RoadShow,os.[Scheme/offers]  as scheme,os.OtherGeneralInfo,os.OtherActivity,os.Discount FROM TransCompetitor os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId union all SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.docid, case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.Remarks,os.[address],'Competitor' as Tran_type,os.item as CompItm,IsNull(os.stock,'') as CompStock,os.qty as  CompQnty, os.rate, os.Compname, os.BrandActivity, os.MeetActivity, os.RoadShow,os.[Scheme/offers] as scheme,os.OtherGeneralInfo,os.OtherActivity,os.Discount FROM Temp_TransCompetitor os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId) a where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }

                                mDataRow["Date"] = Convert.ToDateTime(dr["Mobile_Created_date"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Remark"] = dr["Remarks"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();


                                mDataRow["Competitor Name"] = dr["Compname"].ToString();
                                mDataRow["Competitor Item"] = dr["CompItm"].ToString();
                                mDataRow["Competitor Stock"] = dr["CompStock"].ToString();
                                mDataRow["Qnty"] = dr["CompQnty"].ToString();
                                mDataRow["Total Value"] = dr["rate"].ToString();
                                mDataRow["Total Discount"] = dr["Discount"].ToString();
                                mDataRow["Brand Activity"] = dr["BrandActivity"].ToString();
                                mDataRow["Meet Activity"] = dr["MeetActivity"].ToString();
                                mDataRow["Other Activity"] = dr["OtherActivity"].ToString();
                                mDataRow["Other GeneralInfo"] = dr["OtherGeneralInfo"].ToString();
                                mDataRow["Road Show"] = dr["RoadShow"].ToString();
                                mDataRow["Scheme/offers"] = dr["scheme"].ToString();


                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }


                        #endregion

                        #region dcollection
                        str = "select distinct * from  (SELECT vl1.visid ,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.Remarks,os.[address],'Distributor Collection' as Tran_type FROM DistributerCollection os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.Distid = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=p.PartyId where p.partydist=1 ) a  where visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }


                                mDataRow["Date"] = Convert.ToDateTime(dr["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();

                                str = "select (select sum(amount) as amount from DistributerCollection where visid=" + dr["visid"].ToString() + " and distid=" + dr["PID"].ToString() + ") as Amount,(select STUFF((SELECT ','+ Remarks from DistributerCollection where visid=" + dr["visid"].ToString() + " and distid=" + dr["PID"].ToString() + "   FOR XML PATH ('')), 1, 1, ''))  AS Remark";
                                dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtItem.Rows.Count > 0)
                                {
                                    mDataRow["Total Value"] = dtItem.Rows[0]["Amount"].ToString();
                                    mDataRow["Remark"] = dtItem.Rows[0]["Remark"].ToString();
                                }

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }


                        #endregion

                        #region DFailedVisit
                        str = "select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.FVDocId,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.Nextvisit, 106) AS NextVisitDate, os.VisitTime AS NextVisitTime FROM TransFailedVisit os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId WHERE ISNULL(PARTYDIST,0)=1 union all SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.FVDocId,os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.Nextvisit, 106) AS NextVisitDate, os.VisitTime AS NextVisitTime FROM Temp_TransFailedVisit os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=os.PartyId WHERE ISNULL(PARTYDIST,0)=1) a where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                        if (dtRecord.Rows.Count > 0)
                        {
                            mDataRow = gvdt.NewRow();


                            str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtRecord.Rows[0]["SMId"].ToString() + "  and MainGrp<>" + dtRecord.Rows[0]["SMId"].ToString() + " )";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    if (dtsr.Rows[j]["DesName"].ToString() != "")
                                    {
                                        mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                    }
                                }
                            }

                            for (int i = 0; i < dtdesig.Rows.Count; i++)
                            {
                                if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                {
                                    mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                }
                            }

                            mDataRow["Date"] = Convert.ToDateTime(dtRecord.Rows[0]["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                            mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                            mDataRow["Region"] = drvst["Region"].ToString();
                            mDataRow["Name"] = drvst["SMName"].ToString();
                            mDataRow["SyncId"] = drvst["SyncId"].ToString();
                            mDataRow["Status"] = drvst["Active"].ToString();
                            mDataRow["Designation"] = drvst["DesName"].ToString();
                            mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                            mDataRow["Contact No"] = drvst["Mobile"].ToString();
                            mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                            mDataRow["Total Time Spent (In Minutes)"] = dtRecord.Rows[0]["TotalTimeSpentInMinutes"].ToString();
                            mDataRow["Outlet Name"] = dtRecord.Rows[0]["Party"].ToString();
                            mDataRow["Outlet Status"] = dtRecord.Rows[0]["PActive"].ToString();
                            mDataRow["Outlet Address"] = dtRecord.Rows[0]["Address1"].ToString();
                            mDataRow["Outlet City"] = dtRecord.Rows[0]["Beat"].ToString();
                            mDataRow["Transaction Type"] = dtRecord.Rows[0]["Tran_type"].ToString();
                            mDataRow["Remark"] = dtRecord.Rows[0]["Remarks"].ToString();
                            mDataRow["Geo Address"] = dtRecord.Rows[0]["address"].ToString();

                            mDataRow["NextVisit Date"] = dtRecord.Rows[0]["NextVisitDate"].ToString();
                            mDataRow["NextVisit Time"] = dtRecord.Rows[0]["NextVisitTime"].ToString();

                            gvdt.Rows.Add(mDataRow);
                            gvdt.AcceptChanges();
                        }



                        #endregion

                        #region discussion
                        str = "select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.remarkDist as Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.NextVisitDate, 106) AS NextVisitDate, os.NextVisitTime AS NextVisitTime FROM TransVisitDist os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON  p.PartyId =os.Distid LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=p.PartyId where isnull(p.partydist,0)=1 and isnull(os.type,'') in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') union all SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.remarkDist as Remarks,os.[address],'Failed Visit' as Tran_type,CONVERT(varchar, os.NextVisitDate, 106) AS NextVisitDate, os.NextVisitTime AS NextVisitTime FROM Temp_TransVisitDist os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON  p.PartyId =os.Distid  LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=p.PartyId where isnull(p.partydist,0)=1 and isnull(os.type,'') in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') ) a  where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }

                                mDataRow["Date"] = Convert.ToDateTime(dr["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();

                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Remark"] = dr["Remarks"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();

                                mDataRow["NextVisit Date"] = dtRecord.Rows[0]["NextVisitDate"].ToString();
                                mDataRow["NextVisit Time"] = dtRecord.Rows[0]["NextVisitTime"].ToString();

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }



                        #endregion

                        #region DepoMeetHeadOffice
                        str = "select * from  (SELECT vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid,os.VDate AS VisitDate, p.PartyId AS PID, (p.PartyName+'-'+IsNull(p.SyncId,'')) AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, (b.AreaName+'-'+IsNull(b.SyncId,'')) AS Beat,   os.remarkDist as Remarks,os.[address],os.type as Tran_type,CONVERT(varchar, os.NextVisitDate, 106) AS NextVisitDate, os.NextVisitTime AS NextVisitTime FROM TransVisitDist os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON  p.PartyId =os.Distid LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=p.PartyId where isnull(p.partydist,0)=1 and isnull(os.type,'')='' union all SELECT  vl1.visid,DATEDIFF(MINUTE, tcv.StartCallTime, tcv.EndCallTime) TotalTimeSpentInMinutes, vl1.smid, os.VDate AS VisitDate, p.PartyId, p.PartyName AS Party,p.Address1,case isnull(P.Active,0) when 1 then 'Active' else 'Deactive' end as PActive, b.AreaName AS Beat, os.remarkDist as Remarks,os.[address],os.type as Tran_type,CONVERT(varchar, os.NextVisitDate, 106) AS NextVisitDate, os.NextVisitTime AS NextVisitTime FROM Temp_TransVisitDist os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN Mastparty p ON  p.PartyId =os.Distid  LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate Left Join TransCall tcv ON tcv.VisId = vl1.VisId and tcv.PartyId=p.PartyId where isnull(p.partydist,0)=1 and isnull(os.type,'')='' ) a  where a.visid=" + Visid + "  " + pQry;
                        dtRecord = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtRecord.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecord.Rows)
                            {
                                mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }

                                mDataRow["Date"] = Convert.ToDateTime(dr["VisitDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                                mDataRow["Reporting Manager"] = drvst["reportingPerson"].ToString();
                                mDataRow["Region"] = drvst["Region"].ToString();
                                mDataRow["Name"] = drvst["SMName"].ToString();
                                mDataRow["SyncId"] = drvst["SyncId"].ToString();
                                mDataRow["Status"] = drvst["Active"].ToString();
                                mDataRow["Designation"] = drvst["DesName"].ToString();
                                mDataRow["HeadQuater"] = drvst["HeadquarterName"].ToString();
                                mDataRow["Contact No"] = drvst["Mobile"].ToString();
                                mDataRow["Joint Working User"] = drvst["WITHwHOM"].ToString();
                                mDataRow["Total Time Spent (In Minutes)"] = dr["TotalTimeSpentInMinutes"].ToString();
                                mDataRow["Outlet Name"] = dr["Party"].ToString();
                                mDataRow["Outlet Status"] = dr["PActive"].ToString();
                                mDataRow["Outlet Address"] = dr["Address1"].ToString();
                                mDataRow["Outlet City"] = dr["Beat"].ToString();
                                mDataRow["Transaction Type"] = dr["Tran_type"].ToString();
                                mDataRow["Remark"] = dr["Remarks"].ToString();
                                mDataRow["Geo Address"] = dr["address"].ToString();

                                mDataRow["NextVisit Date"] = dr["NextVisitDate"].ToString();
                                mDataRow["NextVisit Time"] = dr["NextVisitTime"].ToString();

                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }


                        #endregion


                    }
                }
                dtRecord.Dispose();
                dtdesig.Dispose();
                dtItem.Dispose();
                dtsr.Dispose();


                if (gvdt.Rows.Count > 0)
                {
                    ExportDataSetToExcel(gvdt);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('No Record Found');", true);
                }


            }
            catch (Exception ex)
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('" + ex.ToString() + "');", true);
            }
        }

        private void ExportDataSetToExcel(DataTable table)
        {
            try
            {
                //Create an Excel application instance
                Excel.Application excelApp = new Excel.Application();
                string path = Server.MapPath("ExportedFiles//");

                if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                {
                    Directory.CreateDirectory(path);
                }
                string filename = "DSR PartyWise Analysis Report " + frmTextBox.Text.Replace('/', ' ') + "-" + toTextBox.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

                string strPath = Server.MapPath("ExportedFiles//" + filename);
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Range chartRange;
                Excel.Range range, range1;
                int colIndex = 0;
                if (table.Rows.Count > 0)
                {
                    //Add a new worksheet to workbook with the Datatable name

                    Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                    excelWorkSheet.Name = "DSR PartyWise Analysis Report";

                    for (int i = 1; i < table.Columns.Count + 1; i++)
                    {
                        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                        if (table.Columns[i - 1].ColumnName == "Geo Address") colIndex = i + 1;
                        range = excelWorkSheet.Cells[1, i] as Excel.Range;
                        range.Cells.Font.Name = "Calibri";
                        range.Cells.Font.Bold = true;
                        range.Cells.Font.Size = 11;
                        range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);

                        if (i >= colIndex && colIndex != 0)
                        {
                            range = excelWorkSheet.Cells[1, i] as Excel.Range;
                            i++;
                            range1 = excelWorkSheet.Cells[1, i] as Excel.Range;
                            Excel.Range rng = (Excel.Range)excelWorkSheet.get_Range(range, range1);
                            rng.Cells.Merge();

                        }
                    }
                    if (colIndex != 0)
                    {
                        for (int i = colIndex; i < table.Columns.Count + 1; i++)
                        {
                            excelWorkSheet.Cells[2, i] = "Qnty";
                            range = excelWorkSheet.Cells[2, i] as Excel.Range;
                            range.Cells.Font.Name = "Calibri";
                            range.Cells.Font.Bold = true;
                            range.Cells.Font.Size = 11;
                            range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);

                            i++;

                            excelWorkSheet.Cells[2, i] = "Amount";
                            range = excelWorkSheet.Cells[2, i] as Excel.Range;
                            range.Cells.Font.Name = "Calibri";
                            range.Cells.Font.Bold = true;
                            range.Cells.Font.Size = 11;
                            range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                        }

                    }

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            excelWorkSheet.Cells[j + 3, l + 1] = table.Rows[j].ItemArray[l].ToString();
                        }
                    }

                    Excel.Range last = excelWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                    chartRange = excelWorkSheet.get_Range("A1", last);

                    foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                    {
                        cell.BorderAround2();
                    }

                    chartRange = excelWorkSheet.get_Range("A3", last);
                    Excel.FormatConditions fcs = chartRange.FormatConditions;
                    Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    format.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                    excelWorkSheet.Columns.AutoFit();
                    excelWorkSheet.Application.ActiveWindow.SplitRow = 2;
                    excelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                    for (int i = colIndex; i < table.Columns.Count; i++)
                    {
                        ((Excel.Range)excelWorkSheet.Cells[1, i]).EntireColumn.ColumnWidth = 22;
                    }


                    Excel.Worksheet worksheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
                    worksheet.Delete();

                    table.Dispose();

                    excelWorkBook.SaveAs(strPath);
                    excelWorkBook.Close();
                    excelApp.Quit();
                    Response.ContentType = "application/xlsx";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.TransmitFile(strPath);
                    Response.End();

                }
            }
            catch (System.Threading.ThreadAbortException threadEx)
            {
                //Abort Threading
            }
            catch (Exception ex)
            {
                ex.ToString();
            }




        }


        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

            string smIDStr = "", smIDStr12 = "";

            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                Session["treenodes"] = smIDStr12;

            }

        }

        protected void rblview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblview.SelectedValue == "Party")
            {
                Partyview.Visible = true;
                divtrview.Visible = false;
            }
            else
            {
                Partyview.Visible = false;
                divtrview.Visible = true;
            }

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string State = "";
                foreach (ListItem item in ddlState.Items)
                {
                    if (item.Selected)
                    {
                        State += item.Value + ",";
                    }

                }
                State = State.TrimEnd(',');

                string cityQry = @"select distinct cityid,cityname from viewgeo where stateid in (" + State + ") and CityAct=1 order by Cityname";
                DataTable dtMastCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                if (dtMastCity.Rows.Count > 0)
                {
                    ddlParty.Items.Clear();
                    ddlCity.DataSource = dtMastCity;
                    ddlCity.DataTextField = "cityname";
                    ddlCity.DataValueField = "cityid";
                    ddlCity.DataBind();
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string City = "";
                foreach (ListItem item in ddlCity.Items)
                {
                    if (item.Selected)
                    {
                        City += item.Value + ",";
                    }

                }
                City = City.TrimEnd(',');
                string partyQry = @"select partyid,Partyname from mastparty where partydist=0 and areaid in (select distinct areaid from viewgeo where cityid in (" + City + ") ) Order by partyname";
                DataTable dtMastParty = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                if (dtMastParty.Rows.Count > 0)
                {
                    ddlParty.DataSource = dtMastParty;
                    ddlParty.DataTextField = "Partyname";
                    ddlParty.DataValueField = "partyid";
                    ddlParty.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }



    }
}