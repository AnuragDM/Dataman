using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class DSREntryFormLevel3 : System.Web.UI.Page
    {
        int msg = 0;
         int uid = 0;
         int smID = 0;
         int dsrDays = 0;
        string VisitID = "0";
        BAL.DSRLevel3.DSLevel3BAL dp = new BAL.DSRLevel3.DSLevel3BAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            txtVisitDate.Attributes.Add("readonly", "readonly");
            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            CalendarExtender1.EndDate=DateTime.Now;
           
          
            if (parameter != "")
            {
                ViewState["VisId"] = parameter;
                
                divdocid.Visible = false;
                Settings.Instance.VistID = Convert.ToString(ViewState["VisId"]);
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
                LockedValues(Settings.GetVisitLocked(Convert.ToInt32(ViewState["VisId"])));
            }
            string pageName = Path.GetFileName(Request.Path);          
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            //Ankita - 10/may/2016- (For Optimization)
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }

           
            if (!IsPostBack)
            {
                basicExample.Value = "10:00am";
                basicExample1.Value = "06:00pm";
                //Added
                Settings.Instance.BindTimeToDDL(basicExampleDDL);
                Settings.Instance.BindTimeToDDL(basicExample1DDL);
                basicExampleDDL.SelectedValue = "10:00";
                basicExample1DDL.SelectedValue = "18:00";
                //End
                fillDSRType();
               // fillDSRNarration();
                DSRMarket.Visible = false;
                DdlDSRNarrtion.Items.Insert(0, new ListItem("-- Select Narration Type --", "0"));
                txtVisitDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
               
                if (Request.QueryString["VisitID"] != null)
                {
                    try
                    {
                        ViewState["VisId"] = Request.QueryString["VisitID"];
                        FillDeptControls(Convert.ToInt32(Request.QueryString["VisitID"].ToString()));
                       // LockedValues(Settings.GetVisitLocked(Convert.ToInt32(ViewState["VisId"])));
                    }
                    catch
                    {
                    }
                }
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='StateHead' OR RoleType='RegionHead'";
                    ddlUndeUser.DataSource = dv;
                    ddlUndeUser.DataTextField = "SMName";
                    ddlUndeUser.DataValueField = "SMId";
                    ddlUndeUser.DataBind();
                    DIVUnder.Visible = true;
                }
                // btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
               
                  BindDDlCity();
                  BindStaffChkList(accStaffCheckBoxList, ddlUndeUser.SelectedValue);
                if (Request.QueryString["VisId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["VisId"]));
                }
            }
        }

        private void BindStaffChkList(CheckBoxList accStaffCheckBoxList, string smID)
        {
            try
            {
                //      string salerepQuery = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                //      DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, salerepQuery);
                //       string salerepUplineQuery = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(dtChild.Rows[0]["UnderId"]) + "";
                string salerepUplineQuery = @"select * from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid=" + Convert.ToInt32(smID) + ") and SMName<>'.' and SMId<>" + Convert.ToInt32(smID) + " order by SMId";
                DataTable dtChild1 = DbConnectionDAL.GetDataTable(CommandType.Text, salerepUplineQuery);
                //          string salerepDowlineQuery = @"select * from MastSalesRep where UnderId=" + Convert.ToInt32(dtChild.Rows[0]["SMId"]) + "";
                DataTable dtChild2 = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtChild2);
                dv.RowFilter = "SMId<>" + Convert.ToInt32(smID) + " and SMName<>'.'";
                //          DataTable dtChild2 = DbConnectionDAL.GetDataTable(CommandType.Text, salerepDowlineQuery);
                dtChild1.Merge(dv.ToTable());
                DataView dv1 = new DataView(dtChild1);
                DataTable dtNew = new DataTable();
                dtNew.Merge(dv1.ToTable(true, "SMId", "SMName"));
                accStaffCheckBoxList.DataSource = dtNew;
                accStaffCheckBoxList.DataTextField = "SMName";
                accStaffCheckBoxList.DataValueField = "SMId";
                accStaffCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void fillDSRType()
        {  //Ankita - 10/may/2016- (For Optimization)
            //string str = "select * from MastDsrNarrationType";
            string str = "select Id,NarrationType from MastDsrNarrationType";
            DataTable dt=new DataTable();
            dt= DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlDSRType.DataSource = dt;
                ddlDSRType.DataTextField = "NarrationType";
                ddlDSRType.DataValueField = "Id";
                ddlDSRType.DataBind();
            }
            ddlDSRType.Items.Insert(0, new ListItem("-- Select Narration Type --", "0"));
        }

        private void fillDSRNarration()
        {
            DdlDSRNarrtion.Items.Clear();
            //Ankita - 10/may/2016- (For Optimization)
            //string str = "select * from MastDSRNarr where Active=1 and NarrType=" + ddlDSRType.SelectedValue;
            string str = "select Id,Name from MastDSRNarr where Active=1 and NarrType=" + ddlDSRType.SelectedValue;
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                DdlDSRNarrtion.DataSource = dt;
                DdlDSRNarrtion.DataTextField = "Name";
                DdlDSRNarrtion.DataValueField = "Id";
                DdlDSRNarrtion.DataBind();
            }
            DdlDSRNarrtion.Items.Insert(0, new ListItem("-- Select Narration Type --", "0"));
        }

        private void fillNarration()
        {//Ankita - 10/may/2016- (For Optimization)
           // string str = "select * from MastDsrNarrationType";
            string str = "select Id,NarrationType from MastDsrNarrationType";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlDSRType.DataSource = dt;
            ddlDSRType.DataTextField = "NarrationType";
            ddlDSRType.DataValueField = "Id";
            ddlDSRType.DataBind();
            ddlDSRType.Items.Insert(0, new ListItem("-- Select Narration Type --", "0"));
        }
        private void LockedValues(bool Lock)
        {
            //if (Lock)
            //{
            //    btnSave.Text = "Locked";
            //    btnSave.Enabled = false;
            //}
            //else
            //{
            //    btnSave.Enabled = true;
            //}
        }
        private void fillRepeter()
        {//Ankita - 10/may/2016- (For Optimization)
           // string str = @"select * from TransDSRL3 where SMId=" + ddlUndeUser.SelectedValue;
            string str = @"select DSRL3Id,VDate,NarrationType,FromTime,ToTime from TransDSRL3 where SMId=" + ddlUndeUser.SelectedValue;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
            
        }
      
        private void BindDDlCity()
        {//Ankita - 10/may/2016- (For Optimization)
            ddlcity.Items.Clear();
            string str = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                          and PrimCode=" + ddlUndeUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
//            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
//                          and PrimCode=" + ddlUndeUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
//            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
//                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlUndeUser.SelectedValue + ")) and areatype='City' and Active=1 order by AreaName ";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlcity.DataSource = obj;
                ddlcity.DataTextField = "AreaName";
                ddlcity.DataValueField = "AreaId";
                ddlcity.DataBind();
            }
            ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private string FillDistributorByID(int Partyid)
        {
            string str = "select PartyName from MastParty where PartyId=" + Partyid;
            return Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
        }
       
        private void FillDeptControls(int VisId)
        {
            try
            {
                string str = @"select * from TransDSRL3  where DSRL3Id=" + ViewState["VisId"];
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    txtVisitDate.ReadOnly = true;
                    divdocid.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["VisId"].ToString();
                    try
                    {
                        txtVisitDate.Text = DateTime.Parse(deptValueDt.Rows[0]["VDate"].ToString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                    ddlcity.SelectedValue = deptValueDt.Rows[0]["CityId"].ToString();
                    basicExample.Value = deptValueDt.Rows[0]["FromTime"].ToString();
                    basicExample1.Value = deptValueDt.Rows[0]["ToTime"].ToString();
                    //Added
                    basicExampleDDL.SelectedValue = deptValueDt.Rows[0]["FromTime"].ToString();
                    basicExample1DDL.SelectedValue = deptValueDt.Rows[0]["ToTime"].ToString();
                    //End
                    ddlDSRType.SelectedValue = deptValueDt.Rows[0]["DSRType"].ToString();
                    if (ddlDSRType.SelectedIndex == 2)
                    {
                        DSRMarket.Visible = true;
                    }
                    else
                    {
                        DSRMarket.Visible = false;
                    }
                    fillDSRNarration();

                    DdlDSRNarrtion.SelectedValue = deptValueDt.Rows[0]["MDSRNarrId"].ToString();
                
                    txtremarks.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    hfCustomerId.Value = deptValueDt.Rows[0]["DistId"].ToString();
                    txtdistName.Text = FillDistributorByID(Convert.ToInt32(deptValueDt.Rows[0]["DistId"].ToString()));
                    string[] accStaffAll = new string[50];
                    string accStaff = deptValueDt.Rows[0]["Colleague"].ToString();
                   
                    accStaffAll = accStaff.Split(',');
                    if (accStaffAll.Length > 0)
                    {
                        for (int i = 0; i < accStaffAll.Length; i++)
                        {
                            ListItem currentCheckBox = accStaffCheckBoxList.Items.FindByText(accStaffAll[i].ToString());
                            if (currentCheckBox != null)
                            {
                                currentCheckBox.Selected = true;
                            }
                        }
                    }
                  
                    txtDistributorRep.Text = deptValueDt.Rows[0]["DistributorRep"].ToString();
                    


                   //  btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRecord();
                }
                else
                {
                    InsertRecord();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }


        private void InsertRecord()
        {
            //Added
            //string fromtime = txtVisitDate.Text + " " + basicExample.Value;
            //string totime = txtVisitDate.Text + " " + basicExample1.Value;
            string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
            string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
            if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
            }//End
            else
            {
                string docID = Settings.GetDocID("VISSN", DateTime.Now);
                Settings.SetDocID("VISSN", docID);
                int distID = 0;
                if (hfCustomerId.Value != "")
                {
                    try
                    {
                        distID = Convert.ToInt32(hfCustomerId.Value);
                    }
                    catch
                    {

                    }
                }
                string smIDStr = "";
                string smIDStr1 = "";
                //         string message = "";
                foreach (ListItem item in accStaffCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                //int retsave = dp.Insert(docID, 0, txtVisitDate.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(distID), Convert.ToInt32(0), smIDStr1, txtDistributorRep.Text, Convert.ToInt32(DdlDSRNarrtion.SelectedValue), txtremarks.Text, ddlDSRType.SelectedValue, basicExample.Value, basicExample1.Value);


                int retsave = dp.Insert(docID, 0, txtVisitDate.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(distID), Convert.ToInt32(0), smIDStr1, txtDistributorRep.Text, Convert.ToInt32(DdlDSRNarrtion.SelectedValue), txtremarks.Text, ddlDSRType.SelectedValue, basicExampleDDL.SelectedValue, basicExample1DDL.SelectedValue);

                if (retsave != 0)
                {
                    Settings.Instance.VistID = Convert.ToString(retsave);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully-" + docID + "');", true);
                    ClearControls();
                    divdocid.Visible = false;
                    Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                    // btnDelete.Visible = false;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                }
            }

        }

        private int checkDateForUserExists(DateTime Date)
        {
            string str = "select count(*) from TransDSRL3  where VDate='" + Settings.dateformat1(Date.ToShortDateString()) + "' and SMId='" + ddlUndeUser.SelectedValue + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }


        private void ClearControls()
        {
            //   txtVisitDate.Text = "";
            txtVisitDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
            // btnDelete.Visible = false;
            btnSave.Text = "Save";

            divdocid.Visible = false;
            txtmDate.Text = "";
            txttodate.Text = "";
            DdlDSRNarrtion.SelectedIndex = 0;
            hfCustomerId.Value = "0";
            ddlDSRType.SelectedIndex = 0;
            ddlUndeUser.SelectedIndex = 0;
            // txtColleague.Text = "";
            txtdistName.Text = "";
            txtDistributorRep.Text = "";
            txtmDate.Text = "";
            txtremarks.Text = "";
            txttodate.Text = "";
            txtVisitDate.Text = "";
            //   ddlcity.SelectedIndex = 0;
        }

        private void UpdateRecord()
        {
            //Added
            //string fromtime = txtVisitDate.Text + " " + basicExample.Value;
            //string totime = txtVisitDate.Text + " " + basicExample1.Value;
            string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
            string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
            if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
            }//End
            else
            {
                string smIDStr = "";
                string smIDStr1 = "";
                //         string message = "";
                foreach (ListItem item in accStaffCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                //int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), "", 0, txtVisitDate.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(hfCustomerId.Value), 0, smIDStr1, txtDistributorRep.Text, Convert.ToInt32(DdlDSRNarrtion.SelectedValue), txtremarks.Text, ddlDSRType.SelectedValue, basicExample.Value, basicExample1.Value);
                int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), "", 0, txtVisitDate.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(hfCustomerId.Value), 0, smIDStr1, txtDistributorRep.Text, Convert.ToInt32(DdlDSRNarrtion.SelectedValue), txtremarks.Text, ddlDSRType.SelectedValue, basicExampleDDL.SelectedValue, basicExample1DDL.SelectedValue);
                if (retsave == 1)
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    // btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be update');", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSREntryFormLevel3.aspx");
        }
     
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();

            //Added as per UAT - on 16/12/2015 
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End
            //txtmDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
            //txttodate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy"); 
            // btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
        {  //Ankita - 10/may/2016- (For Optimization)
          //  string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and CityId=" + contextKey + "";
            string str = "select PartyId,PartyName FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and CityId=" + contextKey + "";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["PartyName"].ToString(), dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }
        protected void ddlUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDlCity();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
            {
                if (txtmDate.Text != "" && txttodate.Text != "")
                {
                    //str = @"select *, case when DSRType=1 then 'DSR-Office' when DSRType=2 then  'DSR-Market' end DSR from TransDSRL3 where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
                    str = @"select TransDSRL3.*,mdt.NarrationType from TransDSRL3 left join MastDsrNarrationType mdt on TransDSRL3.DSRType=mdt.Id where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
                }
                else if (txtmDate.Text != "")
                {
                    //str = @"select *, case when DSRType=1 then 'DSR-Office' when DSRType=2 then  'DSR-Market' end DSR  from TransDSRL3 where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";
                    str = @"select TransDSRL3.*,mdt.NarrationType from TransDSRL3 left join MastDsrNarrationType mdt on TransDSRL3.DSRType=mdt.Id where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";
                }
                else
                {
                    //str = @"select *, case when DSRType=1 then 'DSR-Office' when DSRType=2 then  'DSR-Market' end DSR from TransDSRL3  where   SMId=" + ddlUndeUser.SelectedValue + " order by VDate desc";
                    str = @"select TransDSRL3.*,mdt.NarrationType from TransDSRL3 left join MastDsrNarrationType mdt on TransDSRL3.DSRType=mdt.Id  where   SMId=" + ddlUndeUser.SelectedValue + " order by VDate desc";
                }
                DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (depdt.Rows.Count > 0)
                {
                    rpt.DataSource = depdt;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        protected void ddlDSRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDSRType.SelectedIndex == 2)
            {
                DSRMarket.Visible = true;
            }
            else
            {
                DSRMarket.Visible = false;
            }
            fillDSRNarration();
        }

    }
}









