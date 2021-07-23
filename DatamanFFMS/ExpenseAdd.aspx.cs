using DAL;
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
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Web.Mail;
using System.Net;
using System.Net.Mail;

namespace AstralFFMS
{
    public partial class ExpenseAdd : System.Web.UI.Page
    {
        ExpensesGroupBAL EXG = new ExpensesGroupBAL();
        ExpenseBAL EB = new ExpenseBAL();
      
        public decimal runningBillTotal = 0;
        public decimal runningClaimTotal = 0;
        public string ConvAmtVal = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Instance.SMID) && Convert.ToInt32(Settings.Instance.SMID) < 1)
            {
                Response.Redirect("~/LogIn.aspx");
            }
            if (Convert.ToInt32(Settings.Instance.UserID) <= 0)
            {
                Response.Redirect("~/LogIn.aspx");
            }
         //   GetGrpExp();
            btnAddNewExpense.Enabled =Settings.Instance.CheckAddPermission("Expensegrp.aspx", Convert.ToString(Session["user_name"]));
            btnAddNewExpense.CssClass = "btn btn-primary";
            //Page.Form.DefaultButton = btnAddNewExpense.UniqueID;
          //  this.Form.DefaultButton = this.btnAddNewExpense.UniqueID;
          
         
            //string createText = " Exp Add smid = '" + Settings.Instance.SMID + "' and Exp Grp =" + ExpGrpId+"";

            //using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/Exptest.txt"), true))
            //{
            //    TextFileCID.WriteLine(createText);
            //    TextFileCID.Close();
            //}
          
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ExpenseGroupId"].ToString()))
                {
                    ViewState["ExpGrpId"] = Convert.ToInt32(Request.QueryString["ExpenseGroupId"]);
                    GetGrpDetails(Convert.ToInt32(Request.QueryString["ExpenseGroupId"].ToString()));

                }
                BillDate.Attributes.Add("readonly", "readonly");
                fromdt.Attributes.Add("readonly", "readonly");
                todt.Attributes.Add("readonly", "readonly");
                TrBillDt.Attributes.Add("readonly", "readonly");
                TrDateFrom.Attributes.Add("readonly", "readonly");
                TrDateTo.Attributes.Add("readonly", "readonly");
                ConvBillDt.Attributes.Add("readonly", "readonly");
                
                BindExpenseType();
                BindState();
                DataGrid();
             
            }
        }
        #region BindDropdowns
        private void BindExpenseType()
        {//Ankita - 11/may/2016- (For Optimization)
          //  string str = "select * from MastExpenseType where active=1 order by Name ";
            string str = "select Id,name from MastExpenseType where active=1 order by Name ";
            fillDDLDirect(ddlexpensetype, str, "Id", "name", 0);
        }

        //void GetGrpExp()
        //{
        //    string strdtls = "select [ExpGrp] from [TempExpense] where SMID=" + Settings.Instance.SMID + "";
        //    ExpGrpId =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strdtls));
        //}
            
        private void BindState()
        {
            string strQ = "select AreaID,AreaName from mastarea MA where AreaType='State' and Active='1' order by AreaName";
            // string strQ = "select AreaID,AreaName from mastarea MA Inner Join MastLink ML On MA.AreaID=ML.LinkCode where AreaType='State' and Active='1' and ML.Ecode='SA' and PrimCode="+ Settings.Instance.SMID + " order by AreaName";
            fillDDLDirect(ddlState, strQ, "AreaID", "AreaName", 1);
            fillDDLDirect(ddltr1state, strQ, "AreaID", "AreaName", 1);
            fillDDLDirect(ddltr2state, strQ, "AreaID", "AreaName", 1);
            fillDDLDirect(ddlConvstate, strQ, "AreaID", "AreaName", 1);
        }
        private void BindCurrentState()
        {
            string str = "SELECT DISTINCT(stateId) FROM ViewGeo WHERE cityid=(select cityId from MastsalesRep where Smid=" + Settings.Instance.SMID + ")";
            string StateId = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
            ddlState.SelectedValue = StateId;
            ddlConvstate.SelectedValue = StateId;
            BindCity(Convert.ToInt32(StateId), 1);
            BindCity(Convert.ToInt32(StateId), 4);
        }



        private void BindCurrentCity()
        {
            BindCity(Convert.ToInt32(ddlState.SelectedValue), 1);
        }
        private void BindCity(Int32 StateID, int Flag)
        {
            string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + StateID + " order by CityName";
            // string strQ = "select Distinct(CityID),CityName from ViewGeo VG inner join MastLink ML On VG.cityid=ML.LinkCode where VG.StateId=" + StateID + " and ML.ECode='SA' and ml.PrimCode=" + Settings.Instance.SMID + " order by CityName";
            if (Flag == 1)
                fillDDLDirect(ddlCity, strQ, "CityID", "CityName", 1);
            else if (Flag == 2)
                fillDDLDirect(ddltr1city, strQ, "CityID", "CityName", 1);
            else if (Flag == 3)
                fillDDLDirect(ddltr2city, strQ, "CityID", "CityName", 1);
            else
                fillDDLDirect(ddlConvcity, strQ, "CityID", "CityName", 1);
            if (!string.IsNullOrEmpty(Settings.Instance.SMID.ToString()) && Convert.ToInt32(Settings.Instance.SMID) > 0)
            {
                string str = "select cityId from MastsalesRep where Smid=" + Settings.Instance.SMID + "";
                string CityId = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();

                if (ViewState["ExpDetailId"].ToString() == "0")
                {
                    if(Flag==1)
                    ddlCity.SelectedValue = CityId;
                    if (Flag==4)
                    ddlConvcity.SelectedValue = CityId;
                }
            }

        }
        private void BindConveyanceMode()
        {   //Ankita - 11/may/2016- (For Optimization)
            string str = "select Id,Name from MastTravelMode where Active=1 and IsTravelConveyance=0 order by Name";
            // string str = "select * from MastTravelMode where Active=1 and IsTravelConveyance=0 order by Name";
            fillDDLDirect(ddlconvtravelmode, str, "Id", "Name", 1);

        }
        private void BindTravelMode()
        {//Ankita - 11/may/2016- (For Optimization)
            //string str = "select * from MastTravelMode where Active=1 and IsTravelConveyance=1 order by Name";
            string str = "select Id,Name from MastTravelMode where Active=1 and IsTravelConveyance=1 order by Name";
            fillDDLDirect(ddlTravelMode, str, "Id", "Name", 1);
        }
     
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDist(string prefixText, string contextKey)
        {//Ankita - 11/may/2016- (For Optimization)
           
            string str = "select PartyId,PartyName FROM MastParty where PartyName like ('%" + prefixText + "%') and Active=1 and CityId in (" + contextKey + ") order by PartyName";
            // string str = "select * FROM MastParty where PartyName like ('%" + prefixText + "%') and Active=1 and CityId in (" + contextKey + ") order by PartyName";
            //string str = "select * FROM MastParty where PartyName like ('%" + prefixText + "%') and PartyDist=0 and Active=1 order by PartyName";

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
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }
        #endregion
        private void GetGrpDetails(int ExpenseGroupId)
        {
            string str = @"select cast(GroupName as varchar) +' -- '+ replace(convert(NVARCHAR, fromdate, 106), ' ', '/')+ ' to ' + replace(convert(NVARCHAR, todate, 106), ' ', '/') as GrpDetails  from ExpenseGroup where ExpenseGroupId=" + ExpenseGroupId + " and (IsDeactivated<>1 or IsSubmitted<>1)";
            lblexpgrp.InnerText = (DbConnectionDAL.GetScalarValue(CommandType.Text, str)).ToString();
        }
        private decimal GetDesig(bool ChangeCityVal)
        {
            decimal limit = 0;
            string str = "";
            if (ChangeCityVal)
            {
                if (ddlConvcity.SelectedValue != "" && ddlConvcity.SelectedValue != "0")
////                str = @" SELECT isnull(ConveyanceAmt,0) AS ConveyanceAmt from 
////                           MastEmployeeCityConvLimit where CityId=" + ddlConvcity.SelectedValue + ""
////                                                                    +" Union all "+
////                   "SELECT isnull((select isnull(Amount,0) from MastLocalConveyanceLimt where DesId=" + Settings.Instance.DesigID + " and CityTypeId=(select CityType from MastArea where AreaId=" + ddlConvcity.SelectedValue + " and AreaType='city')),0) AS ConveyanceAmt  ";
                    str = "SELECT Isnull(CASE isnull((SELECT Isnull(ConveyanceAmt,0) FROM MastEmployeeCityConvLimit WHERE CityId=" + ddlConvcity.SelectedValue + " AND SmId=" + Settings.Instance.SMID + "),0) WHEN 0 THEN (Isnull((SELECT Isnull(amount, 0) FROM   mastlocalconveyancelimt WHERE  desid = " + Settings.Instance.DesigID + " AND citytypeid = (SELECT citytype FROM   mastarea WHERE  areaid = " + ddlConvcity.SelectedValue + " AND areatype ='city')),0)) ELSE (isnull((SELECT Isnull(ConveyanceAmt,0) FROM MastEmployeeCityConvLimit WHERE CityId=" + ddlConvcity.SelectedValue + "  AND SmId=" + Settings.Instance.SMID + "),0)) end ,0)AS ConveyanceAmt ";
            }
            else
            {
                str =
                    @"SELECT isnull((select isnull(Amount,0) from MastLocalConveyanceLimt where DesId=" + Settings.Instance.DesigID + " and CityTypeId=(select CityType from MastArea where AreaId=" + ddlConvcity.SelectedValue + " and AreaType='city')),0) AS ConveyanceAmt  ";

            }
            if(!string.IsNullOrEmpty(str))
            limit = Convert.ToDecimal(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return Math.Round(limit,2);
        }
        protected void btnAddNewExpense_Click(object sender, EventArgs e)
        {
            string ExpCode = "0";
            ViewState["ExpDetailId"] = 0;
            ClearControls();
            chkallowtosave.Checked = false; chkConvAllow0.Checked = false; chkConvSA.Checked = false;
            chkStayWithRelative.Checked = false; chkSuppAtt.Checked = false; chktrSuppAttc.Checked = false; 
            chltrAllowtosave.Checked = false;
            //Added on 21-12-2015
            string EnviroQry = @"SELECT EmpGraceDays FROM MastEnviro";
            DataTable dtEnv = DbConnectionDAL.GetDataTable(CommandType.Text, EnviroQry);
            int GP = 0;
            if (dtEnv.Rows.Count > 0)
            {
                GP = Convert.ToInt32(dtEnv.Rows[0]["EmpGraceDays"]);
            }
            //End
            BillDate.Text = DateTime.UtcNow.ToString("dd/MMM/yyyy");
            CalendarExtender1.StartDate = Settings.GetUTCTime().AddDays(-GP);
            CalendarExtender1.EndDate = Settings.GetUTCTime();
            CalendarExtender2.StartDate = Settings.GetUTCTime().AddDays(-GP);
            CalendarExtender2.EndDate = Settings.GetUTCTime();
            CalendarExtender7.StartDate = Settings.GetUTCTime().AddDays(-GP);
            CalendarExtender7.EndDate = Settings.GetUTCTime();
            ConvBillDt.Text = DateTime.UtcNow.ToString("dd/MMM/yyyy");
            TrBillDt.Text = DateTime.UtcNow.ToString("dd/MMM/yyyy");
           
            string str = "select ExpenseTypeCode from MastExpenseType where ID=" + ddlexpensetype.SelectedValue + "";
          ViewState["ExpCode"] = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
          ExpCode = ViewState["ExpCode"].ToString();
            if (!string.IsNullOrEmpty(Settings.Instance.SMID.ToString()) && Convert.ToInt32(Settings.Instance.SMID) > 0)
            {
                if (ExpCode.ToUpper() == "CONVEYANCETRAVEL" || ExpCode.ToUpper() == "CONVEYANCE")
                { BindState(); BindCurrentState();  }
            }
            if (ExpCode.ToUpper() == "BOARDING" || ExpCode.ToUpper() == "CAPITALPURCHASE" || ExpCode.ToUpper() == "COMMUNICATION" || ExpCode.ToUpper() == "ENTERTAINMENT" || ExpCode.ToUpper() == "FOODSTAFF" || ExpCode.ToUpper() == "LAUNDRY" || ExpCode.ToUpper() == "LODGING" || ExpCode.ToUpper() == "OTHER" || ExpCode.ToUpper() == "COURIER" || ExpCode.ToUpper() == "STATIONARY")
            {
                Session["statusExp1"] = true;
                lblExname.InnerText = " - " + ddlexpensetype.SelectedItem.Text;
            
                if (ExpCode.ToUpper() == "LODGING")
                {
                    Settings.Instance.BindTimeToDDL(startTimeDDL);
                    Settings.Instance.BindTimeToDDL(endTimeDDL);
                    lblExname.InnerText = " - " + ddlexpensetype.SelectedItem.Text;
                    trStayRelative.Attributes.Remove("Style");
                    Tr_dateto.Attributes.Remove("Style"); Tr_dateto1.Attributes.Remove("Style");
                    trgstno.Attributes.Remove("Style"); 
                    trcgstamt.Attributes.Remove("Style");
                    tr1.Attributes.Remove("Style");
                    trgstinno.Attributes.Remove("Style");
                    //lblvendor.Visible = true;
                    //txtpartyvendor1.Visible = true;
                }
                else
                {
                    if (ExpCode.ToUpper() == "BOARDING")
                    {
                        trStayRelative.Attributes.Add("style", "display:none;");
                        Tr_dateto.Attributes.Add("style", "display:none;"); Tr_dateto1.Attributes.Add("style", "display:none;");
                        trgstno.Attributes.Add("style", "display:none;"); 
                        trcgstamt.Attributes.Add("style", "display:none;");
                        tr1.Attributes.Add("style", "display:none;");
                        trgstinno.Attributes.Add("style", "display:none;");
                        //lblvendor.Visible = false;
                        //txtpartyvendor1.Visible = false;
                    }
                    else
                    {
                        trStayRelative.Attributes.Add("style", "display:none;");
                        Tr_dateto.Attributes.Add("style", "display:none;"); Tr_dateto1.Attributes.Add("style", "display:none;");
                        trgstno.Attributes.Remove("Style"); 
                        trcgstamt.Attributes.Remove("Style");
                        tr1.Attributes.Remove("Style");
                        trgstinno.Attributes.Remove("Style");
                        //lblvendor.Visible = true;
                        //txtpartyvendor1.Visible = true;
                        
                    }
                    
                }
                this.ModalPopupExtender1.Show();
                this.ModalPopupExtender2.Hide();
                this.ModalPopupExtender3.Hide();
            }
            else if (ExpCode.ToUpper() == "TRAVEL")
            {
                Session["statusExp3"] = true;
                Settings.Instance.BindTimeToDDL(starttimeddltr);
                Settings.Instance.BindTimeToDDL(endtimeddltr);
                BindTravelMode();
                ConvAmt.Enabled = true; ConvClaimAmt.Enabled = true;
                lblExName1.InnerText = " - " + ddlexpensetype.SelectedItem.Text;
                this.ModalPopupExtender3.Show();
                this.ModalPopupExtender1.Hide();
                this.ModalPopupExtender2.Hide();
            }
            else
            {
                Session["statusExp2"] = true;         
                if (ExpCode.ToUpper() == "CONVEYANCETRAVEL")
                {

                    BindConveyanceMode(); 
                    //Binding for the reason if already binded in conveyance by selected value 
                    string strQ = "select AreaID,AreaName from mastarea MA where AreaType='State' and Active='1' order by AreaName";
                    fillDDLDirect(ddlConvstate, strQ, "AreaID", "AreaName", 1);
                    ConvAmt.Enabled = true; ConvClaimAmt.Enabled = true;
                    trconv.Attributes.Remove("style"); ddlConvstate.Enabled = true; ddlConvcity.Enabled = true;
                    
                }
                else
                {
                    ConvAmt.Enabled = false; ConvClaimAmt.Enabled = false;
                    trconv.Attributes.Add("style", "display:none;"); 
                    //Added on 24-12-2015
                    string strChangeCity = @"SELECT AllowChangeCity FROM MastSalesRep where Smid=" + Settings.Instance.SMID;
                    string ChangeCity = DbConnectionDAL.GetScalarValue(CommandType.Text, strChangeCity).ToString();
                    if (ChangeCity == "True")
                    {                     
                    
                        string strAllowState = @"SELECT Distinct T2.stateid AS AreaID,T2.stateName AS AreaName FROM MastEmployeeCityConvLimit AS T1
                                                        Inner JOIN ViewGeo AS T2
                                                        ON T1.CityId=T2.cityid WHERE  T1.SmId=" + Settings.Instance.SMID + "";
                        DataTable dtC = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strAllowState);
                        if (dtC.Rows.Count > 0)
                        {
                            //ddlConvstate.SelectedIndex = -1;
                            ddlConvstate.Items.Clear(); ddlConvstate.DataBind(); ddlConvcity.Items.Clear();
                            ddlConvstate.Enabled = true; ddlConvcity.Enabled = true;
                            fillDDLDirect(ddlConvstate, strAllowState, "AreaID", "AreaName", 1);
                            ddlConvstate.SelectedValue ="0" ;
                            lblConverror.InnerText = "";
                         
                            //BindCity(Convert.ToInt32(ddlConvstate.SelectedValue), 4);
                        }
                        else
                        {
                            lblConverror.InnerText = "Please contact admin for state/city not alloted!";
                            lblConverror.Attributes.Remove("class");
                            ddlConvstate.Items.Clear(); ddlConvcity.Items.Clear();
                           // BindCurrentState();
                           // ddlConvstate.Enabled = false; ddlConvcity.Enabled = false;
                           
                        }
                          //  ddlConvstate.Items.Insert(0, new ListItem("--Select--", "0")); }
                        ConvAmtVal = GetDesig(true).ToString(); ConvClaimAmt.Text = ConvAmtVal; ConvAmt.Text = ConvAmtVal;
                    }
                    else
                    {
                        BindCurrentState();
                        ConvAmtVal = GetDesig(false).ToString(); ConvClaimAmt.Text = ConvAmtVal; ConvAmt.Text = ConvAmtVal;
                        ddlConvstate.Enabled = false; ddlConvcity.Enabled = false;
                    }
                    //End
                    ConvAmt.Enabled = false; ConvClaimAmt.Enabled = false;
                }
              
                lblExNameConv.InnerText = " - " + ddlexpensetype.SelectedItem.Text;
                this.ModalPopupExtender2.Show();
                this.ModalPopupExtender3.Hide();
                this.ModalPopupExtender1.Hide();
            }
        }
        private void ClearControls()
        {
            Amount.Text = "";
            ClaimAmount.Text = "";
            BillDate.Text = "";
            BillNumber.Text = "";
            ddlCity.Items.Clear();
            ddlState.SelectedIndex = 0;
            TrRemarks.Text = "";
            ddltr1city.Items.Clear();
            ddltr1state.SelectedIndex = 0;
            ddltr2city.Items.Clear();
            ddltr2state.SelectedIndex = 0;
            ddlTravelMode.SelectedIndex = 0;
            RatePerKm.Text = "";
            KmVisited.Text = "";
            Remarks.Text = "";
            chkSuppAtt.Checked = false;
            chktrSuppAttc.Checked = false;
            chkallowtosave.Checked = false;
            chkConvAllow0.Checked = false;
            TrDateTo.Text = "";
            TrDateFrom.Text = "";
            TrClaimAmount.Text = "";
            TrAmount.Text = "";
            TrBillNum.Text = "";
            TrBillDt.Text = "";
            todt.Text = "";
            fromdt.Text = "";
            ConvAmt.Text = "";
            ConvBillDt.Text = "";
            ConvBillNum.Text = "";
            ConvClaimAmt.Text = "";
            ConvRemarks.Text = "";
            ViewState["ClientProduct"] = null;
            chkStayWithRelative.Checked = false;
            txtgstextender2.Text = null;
            txtgstextender3.Text = null;
            txtgstnoextender1.Text = null;
            chkGstnNo1.Checked = false;
            chkGstnNo2.Checked = false;
            chkGstnNo3.Checked = false;
            txtpartyvendor1.Text = null;
            txtpartyvendor2.Text = null;
            txtpartyvendor3.Text = null;
            txtCGSTAmt1.Text = null;
            txtCGSTAmt2.Text = null;
            txtCGSTAmt3.Text = null;
            txtSGSTAmt1.Text = null;
            txtSGSTAmt2.Text = null;
            txtSGSTAmt3.Text = null;
        }
       
        private bool CheckValidDate(DateTime dt)
        {//Ankita - 11/may/2016- (For Optimization)
            bool val = false;
            //string strq = "select * from Expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + " and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) >= cast(Fromdate as date) and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) <= cast(Todate as date)";
            //DataTable dtbl = DbConnectionDAL.getFromDataTable(strq);
            string strq = "select count(*) from Expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + " and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) >= cast(Fromdate as date) and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) <= cast(Todate as date)";
            int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strq));
            //if (dtbl.Rows.Count > 0) { val = true; }
            if (cnt > 0) { val = true; }
            return val;
        }

        //Added on 21-12-2015
        private bool CheckValidBillDate(DateTime dt, DateTime toDT)
        {//Ankita - 11/may/2016- (For Optimization)
            bool val = false;
          //  string strq = "select * from Expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + " and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) >= cast(Fromdate as date) and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) <= cast(Todate as date)";
            string strq = "select count(*) from Expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + " and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) >= cast(Fromdate as date) and cast('" + dt.ToString("dd/MMM/yyyy") + "' as date ) <= cast(Todate as date)";
           // DataTable dtbl = DbConnectionDAL.getFromDataTable(strq);
            int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strq));
            //if (dtbl.Rows.Count > 0) { val = true; }
            if (cnt > 0) { val = true; }
            return val;
        }
        //End
        protected void btnSaveNewExp_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Settings.Instance.UserID) <= 0)
                {
                    Response.Redirect("~/LogIn.aspx");
                }
                 if(Convert.ToBoolean(Session["statusExp1"]))
                {
                    // spinner1.Attributes.Remove("style");
                    lblerrorAddExp.InnerHtml = "";
                    string SelctdCity = Request.Form[ddlCity.UniqueID];
                    string SelctdState = ddlState.SelectedValue;
                    if (!CheckValidDate(Convert.ToDateTime(BillDate.Text)))
                    {
                        BindState();
                        ddlState.SelectedValue = SelctdState;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                        fillDDLDirect(ddlCity, strQ, "CityID", "CityName", 1);

                        ddlCity.SelectedValue = SelctdCity;
                        lblerrorAddExp.InnerText = "Bill Date should be between Expense Group Dates";
                        lblerrorAddExp.Attributes.Remove("class");
                        this.ModalPopupExtender1.Show();
                        return;
                    }
                    else //Added on 21-12-2015
                    {
                        if (ViewState["ExpCode"].ToString().ToUpper() == "LODGING")
                        {
                            if (!(Convert.ToDateTime(BillDate.Text) >= Convert.ToDateTime(todt.Text)))
                            {
                                BindState();
                                ddlState.SelectedValue = SelctdState;
                                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                                fillDDLDirect(ddlCity, strQ, "CityID", "CityName", 1);

                                ddlCity.SelectedValue = SelctdCity;
                                lblerrorAddExp.InnerText = "Bill Date should be greater than or equal to To Date";
                                lblerrorAddExp.Attributes.Remove("class");
                                this.ModalPopupExtender1.Show();
                                return;
                            }
                            string fromtime = fromdt.Text + " " + startTimeDDL.SelectedValue;
                            string totime = todt.Text + " " + endTimeDDL.SelectedValue;
                            //string fromtime = fromdt.Text + " " + basicExample.Value;
                            //string totime = todt.Text + " " + basicExample1.Value;
                            if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                            {
                                BindState();
                                ddlState.SelectedValue = SelctdState;
                                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                                fillDDLDirect(ddlCity, strQ, "CityID", "CityName", 1);

                                ddlCity.SelectedValue = SelctdCity;
                                lblerrorAddExp.InnerText = "To Date and Time should be greater than From Date and Time"; lblerrorAddExp.Attributes.Remove("class"); this.ModalPopupExtender1.Show();
                                return;
                            }
                        }
                    } //End

                    string city = Request.Form[ddlCity.UniqueID];
                    if (city == "0" || string.IsNullOrEmpty(city)) city = ddlCity.SelectedValue;
                    if (city == "0" || string.IsNullOrEmpty(city))
                    {
                        BindState();
                        ddlState.SelectedValue = SelctdState;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                        fillDDLDirect(ddlCity, strQ, "CityID", "CityName", 1);

                        ddlCity.SelectedValue = SelctdCity;
                        lblerrorAddExp.InnerText = "Please select city again !!";
                        lblerrorAddExp.Attributes.Remove("class");
                        this.ModalPopupExtender1.Show();
                        return;
                    }
                    if (string.IsNullOrEmpty(ViewState["ExpGrpId"].ToString())) { ViewState["ExpGrpId"] = "0"; }
                    if (string.IsNullOrEmpty(ViewState["ExpDetailId"].ToString())) { ViewState["ExpDetailId"] = "0"; }
                     decimal cgstamt = 0;
                     decimal sgstamt = 0;
                     decimal igstamt = 0;
                     if(txtCGSTAmt1.Text != "")
                     {
                         cgstamt = Convert.ToDecimal(txtCGSTAmt1.Text);
                     }                    
                     if(txtSGSTAmt1.Text != "")
                     {
                         sgstamt = Convert.ToDecimal(txtSGSTAmt1.Text);
                     }
                     if (txtIGSTAmt1.Text != "")
                     {
                         igstamt = Convert.ToDecimal(txtIGSTAmt1.Text);
                     }

                     int RetVal = EB.InsertExpenses(Convert.ToInt32(ViewState["ExpDetailId"]), SyncId.Text, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(ddlexpensetype.SelectedValue), BillNumber.Text.Trim(), Convert.ToDateTime(BillDate.Text), 0, 0, fromdt.Text, todt.Text, Convert.ToInt32(city), Remarks.Text.Trim(), Convert.ToDecimal(ClaimAmount.Text), Convert.ToDecimal(Amount.Text), Convert.ToInt32(Settings.Instance.UserID), chkSuppAtt.Checked, string.Empty, Convert.ToInt32(ViewState["ExpGrpId"]), chkStayWithRelative.Checked, 0, 0, startTimeDDL.SelectedValue, endTimeDDL.SelectedValue, txtgstnoextender1.Text, chkGstnNo1.Checked, txtpartyvendor1.Text, cgstamt, sgstamt, igstamt, 0, 0, 0, string.Empty, string.Empty, "");

                    if (RetVal == -1)
                    {
                        ClearControls();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Expense already exist.');", true);
                        return;
                    }
                    else
                    {
                       Session["statusExp1"] = false;
                        DataGrid();
                        ClearControls();
                        //if (ExpDetailId > 0)
                        //{
                        //    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Expense Updated Successfully');", true);
                        //}
                        // else { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Expense Added Successfully');", true); }
                        //    return;

                    }
                }
                 else { DataGrid(); }
            }
            catch (Exception ex)
            {
                lblerrorAddExp.Attributes.Remove("class");
                this.ModalPopupExtender1.Show();
                lblerrorAddExp.InnerHtml = "There are some errors while saving records.";
            }
            
        }
        protected void btnTrsave_Click(object sender, EventArgs e)
        {

            try
            {
                if (Convert.ToInt32(Settings.Instance.UserID) <= 0)
                {
                    Response.Redirect("~/LogIn.aspx");
                }
                if(Convert.ToBoolean(Session["statusExp3"]))
                {
                    lblerrorTravel.InnerHtml = "";
                    string SelctdCity1 = Request.Form[ddltr1city.UniqueID];
                    string SelctdState1 = ddltr1state.SelectedValue;
                    string SelctdCity2 = Request.Form[ddltr2city.UniqueID];
                    string SelctdState2 = ddltr2state.SelectedValue;

                    if (!CheckValidDate(Convert.ToDateTime(TrBillDt.Text)))
                    {
                        BindState();
                        ddltr1state.SelectedValue = SelctdState1;
                        ddltr2state.SelectedValue = SelctdState2;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState1 + " order by CityName";
                        fillDDLDirect(ddltr1city, strQ, "CityID", "CityName", 1);
                        string strQ1 = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState2 + " order by CityName";
                        fillDDLDirect(ddltr2city, strQ1, "CityID", "CityName", 1);
                        ddltr1city.SelectedValue = SelctdCity1;
                        ddltr2city.SelectedValue = SelctdCity2;
                        if (chltrAllowtosave.Checked) { TrClaimAmount.Enabled = false; TrAmount.Enabled = false; TrClaimAmount.Text = "0"; TrAmount.Text = "0.0"; }
                        lblerrorTravel.InnerText = "Bill Date should be between Expense Group Dates"; lblerrorTravel.Attributes.Remove("class"); this.ModalPopupExtender3.Show(); return;
                    }
                    //if (Convert.ToDateTime(TrDateFrom.Text) > Convert.ToDateTime(TrDateTo.Text))
                    //{
                    //    lblerrorTravel.InnerText = "To Date Should be greater than From Date"; lblerrorTravel.Attributes.Remove("class"); this.ModalPopupExtender3.Show();
                    //    return;
                    //}

                    //Added
                    //string fromtime = TrDateFrom.Text + " " + txtTrFromTime.Value;
                    //string totime = TrDateTo.Text + " " + txtTrToTime.Value;
                    string fromtime = TrDateFrom.Text + " " + starttimeddltr.SelectedValue;
                    string totime = TrDateTo.Text + " " + endtimeddltr.SelectedValue;
                    if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                    {
                        BindState();
                        ddltr1state.SelectedValue = SelctdState1;
                        ddltr2state.SelectedValue = SelctdState2;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState1 + " order by CityName";
                        fillDDLDirect(ddltr1city, strQ, "CityID", "CityName", 1);
                        string strQ1 = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState2 + " order by CityName";
                        fillDDLDirect(ddltr2city, strQ1, "CityID", "CityName", 1);
                        ddltr1city.SelectedValue = SelctdCity1;
                        ddltr2city.SelectedValue = SelctdCity2;
                        if (chltrAllowtosave.Checked) { TrClaimAmount.Enabled = false; TrAmount.Enabled = false; TrClaimAmount.Text = "0"; TrAmount.Text = "0.0"; }
                        lblerrorTravel.InnerText = "To Date and Time should be greater than From Date and Time"; lblerrorTravel.Attributes.Remove("class"); this.ModalPopupExtender3.Show();
                        return;
                    }
                    //End

                    string city1 = ddltr1city.SelectedValue;
                    if (city1 == "0" || string.IsNullOrEmpty(city1)) city1 = Request.Form[ddltr1city.UniqueID];
                    string city2 = ddltr2city.SelectedValue;
                    if (city2 == "0" || string.IsNullOrEmpty(city2)) city2 = Request.Form[ddltr2city.UniqueID];
                    if (city1 == "0" || string.IsNullOrEmpty(city1) || city2 == "0" || string.IsNullOrEmpty(city2))
                    {
                        BindState();
                        ddltr1state.SelectedValue = SelctdState1;
                        ddltr2state.SelectedValue = SelctdState2;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState1 + " order by CityName";
                        fillDDLDirect(ddltr1city, strQ, "CityID", "CityName", 1);
                        string strQ1 = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState2 + " order by CityName";
                        fillDDLDirect(ddltr2city, strQ1, "CityID", "CityName", 1);
                        ddltr1city.SelectedValue = SelctdCity1;
                        ddltr2city.SelectedValue = SelctdCity2;
                        if (chltrAllowtosave.Checked) { TrClaimAmount.Enabled = false; TrAmount.Enabled = false; TrClaimAmount.Text = "0"; TrAmount.Text = "0.0"; }
                        lblerrorTravel.InnerText = "Please select source and destination Cities"; lblerrorTravel.Attributes.Remove("class"); this.ModalPopupExtender3.Show();
                        return;
                    }                  

                    if (string.IsNullOrEmpty(TrAmount.Text)) { TrAmount.Text = "0"; }
                    if (string.IsNullOrEmpty(TrClaimAmount.Text)) { TrClaimAmount.Text = "0"; }
                    if (string.IsNullOrEmpty(ViewState["ExpGrpId"].ToString())) { ViewState["ExpGrpId"] = "0"; }
                    if (string.IsNullOrEmpty(ViewState["ExpDetailId"].ToString())) { ViewState["ExpDetailId"] = "0"; }

                    decimal cgstamt = 0;
                    decimal sgstamt = 0;
                    decimal igstamt = 0;
                    if (txtCGSTAmt3.Text != "")
                    {
                        cgstamt = Convert.ToDecimal(txtCGSTAmt3.Text);
                    }
                    if (txtSGSTAmt3.Text != "")
                    {
                        sgstamt = Convert.ToDecimal(txtSGSTAmt3.Text);
                    }
                    if (txtIGSTAmt3.Text != "")
                    {
                        igstamt = Convert.ToDecimal(txtIGSTAmt3.Text);
                    }
                    int RetVal = EB.InsertExpenses(Convert.ToInt32(ViewState["ExpDetailId"]), TrSyncID.Text, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(ddlexpensetype.SelectedValue), TrBillNum.Text.Trim(), Convert.ToDateTime(TrBillDt.Text), Convert.ToInt32(city1), Convert.ToInt32(city2), TrDateFrom.Text, TrDateTo.Text, 0, TrRemarks.Text.Trim(), Convert.ToDecimal(TrClaimAmount.Text), Convert.ToDecimal(TrAmount.Text), Convert.ToInt32(Settings.Instance.UserID), chktrSuppAttc.Checked, ddlTravelMode.SelectedValue, Convert.ToInt32(ViewState["ExpGrpId"]), chkStayWithRelative.Checked, 0, 0, starttimeddltr.SelectedValue, endtimeddltr.SelectedValue, txtgstextender3.Text, chkGstnNo3.Checked, txtpartyvendor3.Text, cgstamt, sgstamt, igstamt, 0, 0, 0, string.Empty, string.Empty, "");
                    DataGrid();
                    Session["statusExp1"]= false;
                    ClearControls();
                    //if (ExpDetailId > 0)
                    //{
                    //    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Expense Updated Successfully');", true);
                    //}
                }    //else { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Expense Added Successfully');", true); }
            }
            catch (Exception x)
            {
                lblerrorTravel.Attributes.Remove("class");
                this.ModalPopupExtender3.Show();
                lblerrorTravel.InnerHtml = "There are some errors while saving records.";
            }
        }
        protected void btnConvSave_Click(object sender, EventArgs e)
        {
            
            int RetVal = 0;
            try
            {
                if (Convert.ToInt32(Settings.Instance.UserID) <= 0)
                {
                    Response.Redirect("~/LogIn.aspx");
                }
                if (Convert.ToBoolean(Session["statusExp2"]))
                {
                    lblConverror.InnerHtml = "";
                    string SelctdCity = Request.Form[ddlConvcity.UniqueID];
                    string SelctdState = ddlConvstate.SelectedValue;
                    if (!CheckValidDate(Convert.ToDateTime(ConvBillDt.Text)))
                    {
                        BindState();
                        ddlConvstate.SelectedValue = SelctdState;

                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                        fillDDLDirect(ddlConvcity, strQ, "CityID", "CityName", 1);
                        ddlConvcity.SelectedValue = SelctdCity;
                        if (ViewState["ExpCode"].ToString() == "CONVEYANCE") { trconv.Attributes.Add("style", "display:none;"); }
                        else { trconv.Attributes.Remove("style"); if (Convert.ToDecimal(RatePerKm.Text) > 0) { spntrmode.Attributes.Remove("style"); } else { spntrmode.Attributes.Add("style", "display:none"); } }
                        lblConverror.InnerText = "Bill Date should be between Expense Group Dates"; lblConverror.Attributes.Remove("class"); this.ModalPopupExtender2.Show(); return;
                    }
                   
                    int travelid = 0; decimal perkmrate = 0, KmVisit = 0;
                    if (!string.IsNullOrEmpty(RatePerKm.Text))
                        perkmrate = Convert.ToDecimal(RatePerKm.Text);
                    if (!string.IsNullOrEmpty(KmVisited.Text))
                        KmVisit = Convert.ToDecimal(KmVisited.Text);
                    if (!string.IsNullOrEmpty(ddlconvtravelmode.SelectedValue))
                        travelid = Convert.ToInt32(ddlconvtravelmode.SelectedValue);
                    string city1 = ddlConvcity.SelectedValue;
                    if ((city1 == "0") || string.IsNullOrEmpty(city1)) city1 = Request.Form[ddlConvcity.UniqueID];
                    if (city1 == "0" || string.IsNullOrEmpty(city1))
                    {
                        BindState();
                        ddlConvstate.SelectedValue = SelctdState;
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                        fillDDLDirect(ddlConvcity, strQ, "CityID", "CityName", 1);
                        ddlConvcity.SelectedValue = SelctdCity;
                        if (ViewState["ExpCode"].ToString() == "CONVEYANCE") { trconv.Attributes.Add("style", "display:none;"); }
                        else { trconv.Attributes.Remove("style"); }
                        lblConverror.Attributes.Remove("class");
                        this.ModalPopupExtender2.Show();
                        lblConverror.InnerHtml = "* Please select city again !!";
                        return;
                    }
                    if (string.IsNullOrEmpty(ViewState["ExpGrpId"].ToString())) { ViewState["ExpGrpId"] = "0"; }
                    if (string.IsNullOrEmpty(ViewState["ExpDetailId"].ToString())) { ViewState["ExpDetailId"] = "0"; }
                    decimal cgstamt = 0;
                    decimal sgstamt = 0;
                    decimal igstamt = 0;
                    if (txtCGSTAmt2.Text != "")
                    {
                        cgstamt = Convert.ToDecimal(txtCGSTAmt2.Text);
                    }
                    if (txtSGSTAmt2.Text != "")
                    {
                        sgstamt = Convert.ToDecimal(txtSGSTAmt2.Text);
                    }
                    if (txtIGSTAmt2.Text != "")
                    {
                        igstamt = Convert.ToDecimal(txtIGSTAmt2.Text);
                    }
                    RetVal = EB.InsertExpenses(Convert.ToInt32(ViewState["ExpDetailId"]), "", Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(ddlexpensetype.SelectedValue), ConvBillNum.Text.Trim(), Convert.ToDateTime(ConvBillDt.Text), 0, 0, "", "", Convert.ToInt32(city1), ConvRemarks.Text.Trim(), Convert.ToDecimal(ConvClaimAmt.Text), Convert.ToDecimal(ConvAmt.Text), Convert.ToInt32(Settings.Instance.UserID), chkConvSA.Checked, travelid.ToString(), Convert.ToInt32(ViewState["ExpGrpId"]), chkStayWithRelative.Checked, perkmrate, KmVisit, "", "", txtgstextender2.Text, chkGstnNo2.Checked, txtpartyvendor2.Text, cgstamt, sgstamt, igstamt, 0, 0, 0, string.Empty, string.Empty, "");
                    if (RetVal > 0)
                    {
                       Session["statusExp2"] = false;                       
                    }
                    else
                    {
                        BindState();
                        ddlConvstate.SelectedValue = SelctdState;
                        if (ViewState["ExpCode"].ToString() == "CONVEYANCE") { trconv.Attributes.Add("style", "display:none;"); }
                        else { trconv.Attributes.Remove("style"); }
                        string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + SelctdState + " order by CityName";
                        fillDDLDirect(ddlConvcity, strQ, "CityID", "CityName", 1);
                        ddlConvcity.SelectedValue = SelctdCity;
                        lblConverror.Attributes.Remove("class");
                        this.ModalPopupExtender2.Show();
                        lblConverror.InnerHtml = "This record already exists!";
                        return;
                    }
                    ClearControls();
                    ViewState["ClientProduct"] = null;
                    DataGrid();
                    Response.Redirect("~/ExpenseAdd.aspx?ExpenseGroupId=" + ViewState["ExpGrpId"] + "");
                }
            }
            catch (Exception ex)
            {
                //if (x.Message.ToString() != "{Unable to evaluate expression because the code is optimized or a native frame is on top of the call stack.}")
                //{
                //    ViewState["ClientProduct"] = null;
                //    gdvprodgrp.DataSource = null; gdvprodgrp.DataBind();
                //    lblConverror.Attributes.Remove("class");
                //    this.ModalPopupExtender2.Show();
                //    lblConverror.InnerHtml = "There are some errors while saving records.";
                //}
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Settings.Instance.UserID) <= 0)
            {
                Response.Redirect("~/LogIn.aspx");
            }
           // spinner.Attributes.Remove("style");
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdfExpDetailId = (HiddenField)item.FindControl("hdfExpDetailId");
            HiddenField hdfExpName = (HiddenField)item.FindControl("hdfExpType");
            HiddenField hdfExpNameVal = (HiddenField)item.FindControl("hdfExpName");
            ViewState["ExpDetailId"] = Convert.ToInt32(hdfExpDetailId.Value);
            ViewState["ExpCode"] = hdfExpName.Value;
            if (hdfExpName.Value.ToUpper() == "BOARDING" || hdfExpName.Value.ToUpper() == "CAPITALPURCHASE" || hdfExpName.Value.ToUpper() == "COMMUNICATION" || hdfExpName.Value.ToUpper() == "ENTERTAINMENT" || hdfExpName.Value.ToUpper() == "FOODSTAFF" || hdfExpName.Value.ToUpper() == "LAUNDRY" || hdfExpName.Value.ToUpper() == "LODGING" || hdfExpName.Value.ToUpper() == "OTHER" || hdfExpName.Value.ToUpper() == "COURIER" || hdfExpName.Value.ToUpper() == "STATIONARY")
            {
               Session["statusExp1"] = true;
                lblExname.InnerText = " - " + hdfExpNameVal.Value;
                if (hdfExpName.Value.ToUpper() == "LODGING")
                {
                    Settings.Instance.BindTimeToDDL(startTimeDDL);
                    Settings.Instance.BindTimeToDDL(endTimeDDL);
                    trStayRelative.Attributes.Remove("Style");
                    Tr_dateto.Attributes.Remove("Style"); Tr_dateto1.Attributes.Remove("Style");
                    trgstno.Attributes.Remove("Style"); 
                    trcgstamt.Attributes.Remove("Style");
                    //lblvendor.Visible = true;
                    //txtpartyvendor1.Visible = true;
                }
                else
                {
                    if (hdfExpName.Value.ToUpper() == "BOARDING")
                    {
                        trStayRelative.Attributes.Add("style", "display:none;");
                        Tr_dateto.Attributes.Add("style", "display:none;"); Tr_dateto1.Attributes.Add("style", "display:none;");
                        trgstno.Attributes.Add("style", "display:none;"); 
                        trcgstamt.Attributes.Add("style", "display:none;");
                        //lblvendor.Visible = false;
                        //txtpartyvendor1.Visible = false;
                    }
                    else
                    {
                        trStayRelative.Attributes.Add("style", "display:none;");
                        Tr_dateto.Attributes.Add("style", "display:none;"); Tr_dateto1.Attributes.Add("style", "display:none;");
                        trgstno.Attributes.Remove("Style"); 
                        trcgstamt.Attributes.Remove("Style");
                        //lblvendor.Visible = true;
                        //txtpartyvendor1.Visible = true;

                    }    
                    
                }
           
                GetValOnEdits(Convert.ToInt32(hdfExpDetailId.Value), 1);
                this.ModalPopupExtender1.Show();
            }
            else if (hdfExpName.Value.ToUpper() == "TRAVEL")
            {
                Session["statusExp3"] = true;
                Settings.Instance.BindTimeToDDL(starttimeddltr);
                Settings.Instance.BindTimeToDDL(endtimeddltr);
                lblExName1.InnerText = " - " + hdfExpNameVal.Value;
                GetValOnEdits(Convert.ToInt32(hdfExpDetailId.Value), 2);
                this.ModalPopupExtender3.Show();
            }
            else
            {
                Session["statusExp2"] = true;
                if ((hdfExpName.Value.ToUpper() == "CONVEYANCETRAVEL"))
                {
                    BindConveyanceMode();
                    trconv.Attributes.Remove("style"); ddlConvstate.Enabled = true; ddlConvcity.Enabled = true;
                }
                else
                {
                   
                    trconv.Attributes.Add("style", "display:none;");
                    ConvAmt.Enabled = false; ConvClaimAmt.Enabled = false;
                }

                GetValOnEdits(Convert.ToInt32(hdfExpDetailId.Value), 3);
                this.ModalPopupExtender2.Show();
            }

        }
        private int GetStateByCityID(Int32 CityId)
        {
            int StateId = 0;
            StateId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select StateId from ViewGeo where cityid=" + CityId + ""));
            return StateId;
        }
        private void GetValOnEdits(Int32 ExpenseDetailId, int flag)
        {
            try
            {
                string cgstvalue = string.Empty, sgstvalue = string.Empty, igstvalue = string.Empty;
                string str = "select *,isnull(gstno,'')[GST] from ExpenseDetails where ExpenseDetailId=" + ExpenseDetailId + "";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    string EnviroQry = @"SELECT EmpGraceDays FROM MastEnviro";
                    DataTable dtEnv = DbConnectionDAL.GetDataTable(CommandType.Text, EnviroQry);
                    int GP = 0;
                    if (dtEnv.Rows.Count > 0)
                    {
                        GP = Convert.ToInt32(dtEnv.Rows[0]["EmpGraceDays"]);
                    }
                    //End

                    ddlexpensetype.SelectedValue = dt.Rows[0]["ExpenseTypeID"].ToString();
                    //   BindState();
                    //Added
                 
                    //End
                    if (flag == 1)
                    {
                        CalendarExtender1.StartDate = Settings.GetUTCTime().AddDays(-GP);
                        CalendarExtender1.EndDate = Settings.GetUTCTime();
                        ddlState.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["CityId"])).ToString();
                        BindCity(Convert.ToInt32(ddlState.SelectedValue), 1);
                        ddlCity.SelectedValue = dt.Rows[0]["CityId"].ToString();
                        txtgstnoextender1.Text = dt.Rows[0]["GST"].ToString();
                        Amount.Text = dt.Rows[0]["BillAmount"].ToString();
                        ClaimAmount.Text = dt.Rows[0]["ClaimAmount"].ToString();
                        BillDate.Text = Convert.ToDateTime(dt.Rows[0]["BillDate"]).ToString("dd/MMM/yyyy");
                        BillNumber.Text = dt.Rows[0]["BillNumber"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["IsSupportingAttached"]) == true)
                            chkSuppAtt.Checked = true;
                        else
                            chkSuppAtt.Checked = false;
                        if (Convert.ToBoolean(dt.Rows[0]["StayWithRelative"]) == true)
                        {
                            chkStayWithRelative.Checked = true;
                            chkSuppAtt.Enabled = false;
                            chkGstnNo1.Enabled = false;
                        }
                        else
                        {
                            chkStayWithRelative.Checked = false;
                        }
                        Remarks.Text = dt.Rows[0]["Remarks"].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[0]["FromDate"].ToString()))
                        {
                            fromdt.Text = Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString("dd/MMM/yyyy");
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[0]["ToDate"].ToString()))
                        {
                            todt.Text = Convert.ToDateTime(dt.Rows[0]["ToDate"]).ToString("dd/MMM/yyyy");
                        }
                        //basicExample.Value = dt.Rows[0]["FromTime"].ToString();
                        //basicExample1.Value = dt.Rows[0]["ToTime"].ToString();   
                        startTimeDDL.SelectedValue = dt.Rows[0]["FromTime"].ToString();                      
                        endTimeDDL.SelectedValue = dt.Rows[0]["ToTime"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["IsGSTNNo"]) == true)
                            chkGstnNo1.Checked = true;
                        else
                            chkGstnNo1.Checked = false;
                        txtpartyvendor1.Text = dt.Rows[0]["Vendor"].ToString();

                        txtCGSTAmt1.Text = dt.Rows[0]["CGSTAmt"].ToString();
                        txtSGSTAmt1.Text = dt.Rows[0]["SGSTAmt"].ToString();
                        txtIGSTAmt1.Text = dt.Rows[0]["IGSTAmt"].ToString();

                        //cgstvalue = dt.Rows[0]["CGSTAmt"].ToString();
                        //sgstvalue = dt.Rows[0]["SGSTAmt"].ToString();
                        //igstvalue = dt.Rows[0]["IGSTAmt"].ToString();
                        //if(cgstvalue == "0.00")
                        //{ txtCGSTAmt1.Text = string.Empty; }
                        //else
                        //{ txtCGSTAmt1.Text = dt.Rows[0]["CGSTAmt"].ToString(); }
                        //if (sgstvalue == "0.00")
                        //{ txtSGSTAmt1.Text = string.Empty; }
                        //else
                        //{ txtSGSTAmt1.Text = dt.Rows[0]["SGSTAmt"].ToString(); }
                        //if (igstvalue == "0.00")
                        //{ txtIGSTAmt1.Text = string.Empty; }
                        //else
                        //{ txtIGSTAmt1.Text = dt.Rows[0]["IGSTAmt"].ToString(); }                     


                    }
                    else if (flag == 2)
                    {                       
                        CalendarExtender2.StartDate = Settings.GetUTCTime().AddDays(-GP);
                        CalendarExtender2.EndDate = Settings.GetUTCTime();
                        TrRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                        TrAmount.Text = dt.Rows[0]["BillAmount"].ToString();                       
                        TrClaimAmount.Text = dt.Rows[0]["ClaimAmount"].ToString();
                        TrBillDt.Text = Convert.ToDateTime(dt.Rows[0]["BillDate"]).ToString("dd/MMM/yyyy");
                        TrBillNum.Text = dt.Rows[0]["BillNumber"].ToString();
                        if (Convert.ToInt32(dt.Rows[0]["ClaimAmount"]) <= 0)
                        {
                            chltrAllowtosave.Checked = true;
                            TrClaimAmount.Enabled = false;
                            TrAmount.Enabled = false;
                        }
                        else { chltrAllowtosave.Checked = false;
                        TrClaimAmount.Enabled = true;
                        TrAmount.Enabled = true;
                        }

                        if (Convert.ToBoolean(dt.Rows[0]["IsSupportingAttached"]) == true)
                            chktrSuppAttc.Checked = true;
                        else
                            chktrSuppAttc.Checked = false;
                        if (Convert.ToBoolean(dt.Rows[0]["StayWithRelative"]) == true)
                            chkStayWithRelative.Checked = true;
                        else
                            chkStayWithRelative.Checked = false;
                        if (!string.IsNullOrEmpty(dt.Rows[0]["FromDate"].ToString()))
                        {
                            TrDateFrom.Text = Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString("dd/MMM/yyyy");
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[0]["ToDate"].ToString()))
                        {
                            TrDateTo.Text = Convert.ToDateTime(dt.Rows[0]["ToDate"]).ToString("dd/MMM/yyyy");
                        }
                        ddltr1state.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["FromCity"])).ToString();
                        BindCity(Convert.ToInt32(ddltr1state.SelectedValue), 2);
                        ddltr1city.SelectedValue = dt.Rows[0]["FromCity"].ToString();
                        ddltr2state.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["ToCity"])).ToString();
                        BindCity(Convert.ToInt32(ddltr2state.SelectedValue), 3);
                        ddltr2city.SelectedValue = dt.Rows[0]["ToCity"].ToString();
                        BindTravelMode();
                        ddlTravelMode.SelectedValue = dt.Rows[0]["TravelModeId"].ToString();
                        //txtTrFromTime.Value = dt.Rows[0]["FromTime"].ToString();
                        //txtTrToTime.Value = dt.Rows[0]["ToTime"].ToString();
                        starttimeddltr.SelectedValue = dt.Rows[0]["FromTime"].ToString();
                        endtimeddltr.SelectedValue = dt.Rows[0]["ToTime"].ToString();
                        txtgstextender3.Text = dt.Rows[0]["GST"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["IsGSTNNo"]) == true)
                            chkGstnNo3.Checked = true;
                        else
                            chkGstnNo3.Checked = false;
                        txtpartyvendor3.Text = dt.Rows[0]["Vendor"].ToString();
                        txtCGSTAmt3.Text = dt.Rows[0]["CGSTAmt"].ToString();
                        txtSGSTAmt3.Text = dt.Rows[0]["SGSTAmt"].ToString();
                        txtIGSTAmt3.Text = dt.Rows[0]["IGSTAmt"].ToString();

                        //cgstvalue = dt.Rows[0]["CGSTAmt"].ToString();
                        //sgstvalue = dt.Rows[0]["SGSTAmt"].ToString();
                        //igstvalue = dt.Rows[0]["IGSTAmt"].ToString();
                        //if (cgstvalue == "0.00")
                        //{ txtCGSTAmt3.Text = string.Empty; }
                        //else
                        //{ txtCGSTAmt3.Text = dt.Rows[0]["CGSTAmt"].ToString(); }
                        //if (sgstvalue == "0.00")
                        //{ txtSGSTAmt3.Text = string.Empty; }
                        //else
                        //{ txtSGSTAmt3.Text = dt.Rows[0]["SGSTAmt"].ToString(); }
                        //if (igstvalue == "0.00")
                        //{ txtIGSTAmt3.Text = string.Empty; }
                        //else
                        //{ txtIGSTAmt3.Text = dt.Rows[0]["IGSTAmt"].ToString(); }       
                        
                    }
                    else
                    {
                        CalendarExtender7.StartDate = Settings.GetUTCTime().AddDays(-GP);
                        CalendarExtender7.EndDate = Settings.GetUTCTime();
                        ConvRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                        ConvAmt.Text = dt.Rows[0]["BillAmount"].ToString();
                        ConvClaimAmt.Text = dt.Rows[0]["ClaimAmount"].ToString();
                        txtgstextender2.Text = dt.Rows[0]["GST"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["IsGSTNNo"]) == true)
                            chkGstnNo2.Checked = true;
                        else
                            chkGstnNo2.Checked = false;
                        txtpartyvendor2.Text = dt.Rows[0]["Vendor"].ToString();
                        txtCGSTAmt2.Text = dt.Rows[0]["CGSTAmt"].ToString();
                        txtSGSTAmt2.Text = dt.Rows[0]["SGSTAmt"].ToString();
                        txtIGSTAmt2.Text = dt.Rows[0]["IGSTAmt"].ToString();

                        //cgstvalue = dt.Rows[0]["CGSTAmt"].ToString();
                        //sgstvalue = dt.Rows[0]["SGSTAmt"].ToString();
                        //igstvalue = dt.Rows[0]["IGSTAmt"].ToString();
                        //if (cgstvalue == "0.00")
                        //{ txtCGSTAmt2.Text = string.Empty; }
                        //else
                        //{ txtCGSTAmt2.Text = dt.Rows[0]["CGSTAmt"].ToString(); }
                        //if (sgstvalue == "0.00")
                        //{ txtSGSTAmt2.Text = string.Empty; }
                        //else
                        //{ txtSGSTAmt2.Text = dt.Rows[0]["SGSTAmt"].ToString(); }
                        //if (igstvalue == "0.00")
                        //{ txtIGSTAmt2.Text = string.Empty; }
                        //else
                        //{ txtIGSTAmt2.Text = dt.Rows[0]["IGSTAmt"].ToString(); }       

                            if (Convert.ToInt32(dt.Rows[0]["ClaimAmount"]) <= 0)
                            {
                                chkConvAllow0.Checked = true;
                                ConvAmt.Enabled = false;
                                ConvClaimAmt.Enabled = false;
                                KmVisited.Enabled = false;

                            }
                            else { chkConvAllow0.Checked = false; ConvAmt.Enabled = true; ConvClaimAmt.Enabled = true; KmVisited.Enabled = true; }
                       
                        ConvBillDt.Text = Convert.ToDateTime(dt.Rows[0]["BillDate"]).ToString("dd/MMM/yyyy");
                        ConvBillNum.Text = dt.Rows[0]["BillNumber"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["IsSupportingAttached"]) == true)
                            chkConvSA.Checked = true;
                        else
                            chkConvSA.Checked = false;

                        RatePerKm.Text = dt.Rows[0]["Prekilometerrate"].ToString();
                        if (Convert.ToDecimal(RatePerKm.Text) > 0) { spntrmode.Attributes.Remove("style"); } else { spntrmode.Attributes.Add("style", "display:none"); }
                        KmVisited.Text = dt.Rows[0]["KMVisit"].ToString();

                      
                        BindConveyanceMode();
                        ddlconvtravelmode.SelectedValue = dt.Rows[0]["TravelModeId"].ToString();
                        string strq = "select ep.partyid as ClientID, ep.productgroup as ProdGrp,mp.partyname as client,ep.productgroup as ProdGrpID,ep.Remarks from expenseparty ep inner join mastparty mp on mp.partyid=ep.partyId where expensedetailid=" + ExpenseDetailId + "";
                        ViewState["ClientProduct"] = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
                        //DataTable dtNewExp = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
                        //if (dtNewExp.Rows.Count > 0)
                        //{
                        //    //ddlparty.SelectedValue = dtNewExp.Rows[0]["ClientID"].ToString();
                        //}
                      
                        if (ViewState["ExpCode"].ToString().ToUpper() == "CONVEYANCE")
                        {
                            string strChangeCity = @"SELECT AllowChangeCity FROM MastSalesRep where Smid=" + Settings.Instance.SMID;
                            string ChangeCity = DbConnectionDAL.GetScalarValue(CommandType.Text, strChangeCity).ToString();
                            if (ChangeCity == "True")
                            {
                                string strAllowState = @"SELECT Distinct T2.stateid AS AreaID,T2.stateName AS AreaName FROM MastEmployeeCityConvLimit AS T1
                                                        Inner JOIN ViewGeo AS T2
                                                        ON T1.CityId=T2.cityid WHERE  T1.SmId=" + Settings.Instance.SMID;

                                DataTable dtAllowState = DbConnectionDAL.GetDataTable(CommandType.Text, strAllowState);
                                if (dtAllowState.Rows.Count > 0)
                                {
                                    fillDDLDirect(ddlConvstate, strAllowState, "AreaID", "AreaName", 1);
                                    ddlConvstate.Enabled = true; ddlConvcity.Enabled = true;
                                    ddlConvstate.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["CityId"])).ToString();
                                    string strAllowCity = @"SELECT Distinct T2.CityId AS AreaID,T2.CityName AS AreaName FROM MastEmployeeCityConvLimit AS T1
                                                        Inner JOIN ViewGeo AS T2
                                                   ON T1.CityId=T2.cityid WHERE  T1.SmId=" + Settings.Instance.SMID + " and T2.StateId ="+ ddlConvstate.SelectedValue+"";
                                    fillDDLDirect(ddlConvcity, strAllowCity, "AreaID", "AreaName", 1);
                                    //     BindCity(Convert.ToInt32(ddlConvstate.SelectedValue), 4);
                                    ddlConvcity.SelectedValue = dt.Rows[0]["CityId"].ToString();


                                }
                                else { ddlConvstate.Enabled = false; ddlConvcity.Enabled = false; }
                            }
                            else
                            {
                                ddlConvstate.Enabled = false; ddlConvcity.Enabled = false; ddlConvstate.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["CityId"])).ToString();
                                BindCity(Convert.ToInt32(ddlConvstate.SelectedValue), 4);
                                ddlConvcity.SelectedValue = dt.Rows[0]["CityId"].ToString();
                            }
                        }
                        else
                        {
                            ddlConvstate.SelectedValue = GetStateByCityID(Convert.ToInt32(dt.Rows[0]["CityId"])).ToString();
                            BindCity(Convert.ToInt32(ddlConvstate.SelectedValue), 4);
                            ddlConvcity.SelectedValue = dt.Rows[0]["CityId"].ToString();
                        }
                       
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        #region BindDataGrids
        private void DataGrid()
        {
            DataTable dt = new DataTable();
            string strQqy = "select ED.Gstno,Case ED.IsGSTNNo when 1 then 'Yes' else 'No' end as IsGSTNNo,ED.Vendor,ED.CGSTAmt,ED.SGSTAmt,Ed.IGSTAmt,Ed.ExpenseDetailID,ED.BillAmount,ED.BillDate,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MET.Name,MAC.AreaName,MET.ExpenseTypeCode,Case ED.IsSupportingAttached when 1 then 'Yes' else 'No' end as IsSupportingAttached1 from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id Inner Join MastArea MAC on Ed.CityID=MAC.AreaId where ED.ExpenseGroupID=" + ViewState["ExpGrpId"] + " group by ED.Gstno,ED.IsGSTNNo,ED.Vendor,ED.CGSTAmt,ED.SGSTAmt,Ed.IGSTAmt,met.Name,Ed.ExpenseDetailID,ED.BillAmount,ED.BillDate,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MAC.AreaName,MET.ExpenseTypeCode,ED.IsSupportingAttached" +
                " Union " + " Select ED.Gstno,Case ED.IsGSTNNo when 1 then 'Yes' else 'No' end as IsGSTNNo,ED.Vendor,ED.CGSTAmt,ED.SGSTAmt,Ed.IGSTAmt,Ed.ExpenseDetailID,ED.BillAmount,convert(varchar(12),ED.BillDate,106) as BillDate,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MET.Name,MAC.AreaName,MET.ExpenseTypeCode,Case ED.IsSupportingAttached when 1 then 'Yes' else 'No' end as IsSupportingAttached1 from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id Inner join MastArea MAC on Ed.FromCity=MAC.AreaId where ED.ExpenseGroupID=" + ViewState["ExpGrpId"] + " group by ED.Gstno,ED.IsGSTNNo,ED.Vendor,ED.CGSTAmt,ED.SGSTAmt,Ed.IGSTAmt,met.Name,Ed.ExpenseDetailID,ED.BillAmount,ED.BillDate,ED.BillNumber,ED.CityID,ED.ClaimAmount,ED.FromCity,ED.FromDate,ED.IsSupportingAttached,ED.Remarks,Ed.ToCity,ED.ToDate,ED.TravelModeID,MAC.AreaName,MET.ExpenseTypeCode,ED.IsSupportingAttached";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQqy);
            rpt.DataSource = dt;
            rpt.DataBind();
            if (dt.Rows.Count > 0)
            {
                lnksubmit.Visible = true;
            }
            else {
                lnksubmit.Visible = false;
            }
        }
        #endregion


        private bool GetvalidSubmitDate()
        {
            //string strDs = "SELECT count(*) FROM ExpenseGroup WHERE IsSubmitted=1 AND SMID=" + Settings.Instance.SMID + " AND FromDate >=(SELECT FromDate FROM ExpenseGroup WHERE ExpenseGroupID=" + ViewState["ExpGrpId"] + ")";
            string strDs = "SELECT count(*) FROM ExpenseGroup WHERE IsSubmitted=1 AND SMID=" + Settings.Instance.SMID + " AND ToDate >=(SELECT FromDate FROM ExpenseGroup WHERE ExpenseGroupID=" + ViewState["ExpGrpId"] + ")";
            if(Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text,strDs)) > 0)
                return false;
            else
            return true;
        }
        private string GetIp()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            return myIP;
        }       
  
        protected void lnksubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtexp = new DataTable();
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    // Nishu 06/08/2016 (group id go to mail in place of voucher no.)
                    //string strSmid = "select Smid from expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + "";                    
                    string strSmid = "select Smid,VoucherNo from expensegroup where ExpenseGroupID=" + ViewState["ExpGrpId"] + "";                    
                     dtexp = DbConnectionDAL.GetDataTable(CommandType.Text, strSmid);
                     if (dtexp.Rows.Count > 0)
                     {

                         int smid = Convert.ToInt32(dtexp.Rows[0]["Smid"]);
                         int voucherNo = Convert.ToInt32(dtexp.Rows[0]["VoucherNo"]);
                         ViewState["voucherNo"] = voucherNo;

                         if (smid == Convert.ToInt32(Settings.Instance.SMID))
                         {
                             if (GetvalidSubmitDate())
                             {
                                 try
                                 {
                                     string Updt = "update ExpenseGroup set IsSubmitted=1,IsApproved=0,DateOfSubmission=DateAdd(ss,19800,GetUtcdate()) where ExpenseGroupId=" + ViewState["ExpGrpId"] + " and SMID=" + Settings.Instance.SMID + "";
                                     DbConnectionDAL.ExecuteQuery(Updt);
                                     EB.InsertExpenseLog(Convert.ToInt32(ViewState["ExpGrpId"]), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), "Sub", Dns.GetHostName(), GetIp(), 0);
                                     SendEmail();
                                     Response.Redirect("~/expensegrp.aspx");
                                 }
                                 catch (Exception ex)
                                 { ex.ToString(); }
                             }
                             else { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This expense group cannot be submitted as the expense group with the forward dates has been already submitted or approved');", true); }
                         }
                     }
                     else
                     { Response.Redirect("~/LogIn.aspx", true); }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    LinkButton btn = (LinkButton)sender;
                    var item = (RepeaterItem)btn.NamingContainer;
                    HiddenField hdfExpDetailId = (HiddenField)item.FindControl("hdfExpDetailId");
                    //string delqry = "delete from ExpenseParty where ExpenseDetailID=" + hdfExpDetailId.Value + "";
                    //DbConnectionDAL.ExecuteQuery(delqry);
                    string delqry1 = "delete from ExpenseDetails where ExpenseDetailID=" + hdfExpDetailId.Value + "";
                    DbConnectionDAL.ExecuteQuery(delqry1);

                    string updgrp = "update ExpenseGroup set TotalAmount= (select ISNULL(sum(ClaimAmount),0) from expensedetails " +
                        " where ExpenseGroupID=" + ViewState["ExpGrpId"] + ") where ExpenseGroupID=" + ViewState["ExpGrpId"] + "";
                    DbConnectionDAL.ExecuteQuery(updgrp);
                    DataGrid();
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

    
   
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkedit = (LinkButton)e.Item.FindControl("LinkButton1");
                LinkButton lnkdelete = (LinkButton)e.Item.FindControl("LinkButton2");
                lnkdelete.Visible = Settings.Instance.CheckDeletePermission("Expensegrp.aspx", Convert.ToString(Session["user_name"]));
          lnkedit.Visible = Settings.Instance.CheckEditPermission("Expensegrp.aspx", Convert.ToString(Session["user_name"]));
            }
        }

         
        private void SendEmail()
        {
            try
            {
                string defaultmailId = "", defaultpassword = "", port = "", QryDsr = "", QryFailV = "";
                string qry = "select SenderEmailID,SenderPassword,Port,ExpenseAdminEmail,MailServer from [MastEnviro]";

                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                string[] emailToAll = new string[20];
                string emailNew = "";
                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["SenderEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["SenderPassword"].ToString();
                    port = checkemaildt.Rows[0]["Port"].ToString();

                    emailNew = checkemaildt.Rows[0]["ExpenseAdminEmail"].ToString();
                    emailToAll = emailNew.Split(';');
                }
                DataTable dt = new DataTable();
                // Nishu 06/08/2016  (Created date wrong go in mail)
                //dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select Met.Name,Ed.CreatedOn,Ml.EmpName from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id inner join MastLogin Ml on Ed.Createdby=Ml.Id where ED.ExpenseGroupID=" + ViewState["ExpGrpId"] + "");
                //
                dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select Met.Name,eg.CreatedOn,Ml.EmpName from ExpenseDetails  ED inner join MastExpenseType MET on ED.ExpenseTypeID=MET.Id LEFT JOIN ExpenseGroup eg ON eg.ExpenseGroupid = ed.ExpenseGroupID inner join MastLogin Ml on Ed.Createdby=Ml.Id where ED.ExpenseGroupID=" + ViewState["ExpGrpId"] + "");
                
                string strClaimAmt = @"SELECT Sum(ClaimAmount) AS ClaimAmount FROM ExpenseDetails WHERE ExpenseGroupID=" + ViewState["ExpGrpId"];
                DataTable dtClaimAmt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strClaimAmt);

                string strEmail = @"SELECT Email AS email1 FROM MastLogin WHERE Id=" + Settings.Instance.UserID;
                DataTable dtEmail = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strEmail);
                if (dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='ExpenseSheetSubmited'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='ExpenseSheetSubmited'";
                    DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                    string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                    string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                    if (dtVar != null && dtVar.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtVar.Rows.Count; j++)
                        {
                            if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ExpenseID}")
                                {
                                    //strSubject = strSubject.Replace("{ExpenseID}", Convert.ToString(ViewState["ExpGrpId"]));
                                    strSubject = strSubject.Replace("{ExpenseID}", Convert.ToString(ViewState["voucherNo"]));
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                {
                                    strSubject = strSubject.Replace("{EmployeeName}", Convert.ToString(dt.Rows[0]["EmpName"]));
                                }
                            }

                            if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ExpenseID}")
                                {
                                    //strMailBody = strMailBody.Replace("{ExpenseID}", Convert.ToString(ViewState["ExpGrpId"]));
                                    strMailBody = strMailBody.Replace("{ExpenseID}", Convert.ToString(ViewState["voucherNo"]));
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{EmployeeName}")
                                {
                                    strMailBody = strMailBody.Replace("{EmployeeName}", Convert.ToString(dt.Rows[0]["EmpName"]));
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{DateOfCreation}")
                                {
                                    strMailBody = strMailBody.Replace("{DateOfCreation}", string.Format("{0:dd/MMM/yyyy}", dt.Rows[0]["CreatedOn"]));
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{TotalAmount}")
                                {
                                    strMailBody = strMailBody.Replace("{TotalAmount}", Convert.ToString(dtClaimAmt.Rows[0]["ClaimAmount"]));
                                }
                            }
                        }
                    }

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.From = new MailAddress(defaultmailId);
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;
                    mail.To.Add(new MailAddress(Convert.ToString(dtEmail.Rows[0]["email1"])));
                    //mail.To.Add(new MailAddress(Convert.ToString(checkemaildt.Rows[0]["ExpenseAdminEmail"])));

                    if (emailToAll.Length > 0)
                    {
                        for (int i = 0; i < emailToAll.Length; i++)
                        {
                            mail.To.Add(new MailAddress(emailToAll[i]));
                        }
                    }

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(checkemaildt.Rows[0]["SenderEmailId"]), Convert.ToString(checkemaildt.Rows[0]["SenderPassword"]));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString(checkemaildt.Rows[0]["MailServer"]), Convert.ToInt32(checkemaildt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


    }
}