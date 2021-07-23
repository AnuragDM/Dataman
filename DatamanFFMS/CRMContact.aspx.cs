using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BAL;
using System.Data;
using BusinessLayer;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.IO;
namespace AstralFFMS
{
    public partial class CRMContact : System.Web.UI.Page
    {
        CRMBAL CB = new CRMBAL();
        string parameter = "";
        DataTable ValueDt;
        int contactID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["Msg"]!=null)
            {
                clear();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", Session["Msg"].ToString(), true); 
                BindCountry();
                Session["Msg"] = null;
            }
            parameter = Request["__EVENTARGUMENT"];
            if ((parameter != "" ))
            {
                Page.Form.DefaultButton = btnSave.UniqueID;
                this.Form.DefaultButton = this.btnSave.UniqueID;
                ViewState["Contact_Id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }          

            string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }
            btndelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btndelete.CssClass = "btn btn-primary";

            if (Settings.Instance.RoleType == "Admin")
            {
                chkbxactive.Disabled = false;
            }
            else
            {
                chkbxactive.Disabled = true;
            }
            if (!Page.IsPostBack)
            {
                BindCountry(); BindStatus(); BindLeadSource(); BindTag(); BindSalePersons(); fillddlUrltype(); fillddlEmailtype(); fillddlphonetype(); 
                //BindState(); 
               // BindCity(); 
                Bindowner(); BindState1();
                CalendarExtender1.StartDate = Settings.GetUTCTime();
                txtnxtactndt.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");                

            }
            if (Request.QueryString["Contact_Id"] != null)
            {
                btnFind.Visible = true;
                FillControls(Convert.ToInt32(Request.QueryString["Contact_Id"]));
            }
        }
        private void Bindowner()
        {
            string strQ;
            if (Settings.Instance.UserName.ToUpper() == "SA")
            {

                strQ = " Select SMID,SMName from [MastSalesRep] where SMName <>'.'  order by SMName ";
            }

            else
            {
                strQ = " ((select MSRG.maingrp As SMID,MSR.SMName from mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.maingrp  where MSRG.smid in (" + Settings.Instance.SMID + ") and MSR.SMName <>'.'  union ";
                strQ = strQ + " SELECT MSRG.smid  As SMID,MSR.SMName FROM mastsalesrepgrp  MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.smid  WHERE  MSRG.maingrp in (" + Settings.Instance.SMID + ")  and MSR.SMName <>'.' ))  order by MSR.SMName ";


            }
            DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dtOwner.Rows.Count > 0)
            {
                foreach (DataRow row in dtOwner.Rows)
                {
                    if (row["SMID"].ToString() == Settings.Instance.SMID)
                    {
                        row["SMName"] = " Me";
                    }
                }
            }
            DataView dv = new DataView(dtOwner);
            dv.Sort = "SMName";
            dtOwner = dv.ToTable();
            ddlowner.DataSource = dtOwner;
            ddlowner.DataTextField = "SMName";
            ddlowner.DataValueField = "SMID";
            ddlowner.DataBind();
            if (dtOwner.Rows.Count>=1)
            {
                ddlowner.Items.Insert(0, new ListItem("--Select--", "0"));
            }
           
        }
        private void BindCountry()
        {
            string strC = "select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' order by AreaName";
            fillDDLDirect(ddlcountry, strC, "AreaID", "AreaName", 1);
            fillDDLDirect(ddlcompcountry,strC, "AreaID", "AreaName", 1);
            fillDDLDirect(ddlCountryList, strC, "AreaID", "AreaName", 1);
        }
        private void BindStatus()
        {
            string strBS = "select [Status_Id],[Status] from CRM_MastStatus order by Status_Id";
            fillDDLDirect(ddlstatus, strBS, "Status_Id", "Status", 0);
        }
        private void BindLeadSource()
        {
            string strLS = "select [Lead_Id],[Lead] from CRM_MastLeadSource order by Lead";
            fillDDLDirect(ddlleadsource, strLS, "Lead_Id", "Lead", 1);
        }
        private void BindState1()
        {
            string strC = "select ma.AreaID,ma.AreaName from mastarea ma where ma.AreaType='State' and ma.Active='1' order by ma.AreaName";
               fillDDLDirect(State, strC, "AreaID", "AreaName", 1);
               fillDDLDirect(txtcompstate, strC, "AreaID", "AreaName", 1);
               fillDDLDirect(ddlState, strC, "AreaID", "AreaName", 1);
            
        }
        private void BindState()
        {
            //string strC = "select distinct State from CRM_MastContact WHERE State <> 'NULL' AND State <> '' Order by State";
            //fillDDLDirect(ddlState, strC, "State", "State", 1);
            string strC = "select distinct State from CRM_MastContact WHERE State <> 'NULL' AND State <> '' Order by State";
            fillDDLDirect(ddlState, strC, "State", "State", 1);
            //BindCity();
        }
        private void BindCity(string state)
        {
            string strC = "select distinct City from CRM_MastContact WHERE City <> 'NULL' AND City <> '' AND Stateid='" + state   + "' Order by City";
            fillDDLDirect(ddlcity, strC, "City", "City", 1);
        }
        private void BindCityList()
        {
            string strC = "select distinct City from CRM_MastContact WHERE City <> 'NULL' AND City <> '' Order by City";
            fillDDLDirect(ddlcity, strC, "City", "City", 1);
        }
        private void BindSalePersons()
        {
            try
            {
                string str = "select SMId,SMName from MastSalesRep where (smid in (select maingrp from MastSalesRepGrp where smid=" + Settings.Instance.SMID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + Settings.Instance.SMID + ") ) and SMName<>'.'  order by SMName";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
              //  DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                dv.RowFilter = "SMName<>.";
                if (dv.ToTable().Rows.Count > 0)
                {
                    ddlsp.DataSource = dv.ToTable();
                    ddlsp.DataTextField = "SMName";
                    ddlsp.DataValueField = "SMId";
                    ddlsp.DataBind();
                    ddlsp.SelectedValue = Settings.Instance.SMID;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        [System.Web.Script.Services.ScriptMethod()]
[System.Web.Services.WebMethod]
        public static List<string> SearchCities(string prefixText, int count)
{
  
        string str  = "SELECT * FROM CRM_MastCity  where " +
            "City like '" + prefixText + "%' ORDER BY city";
        DataTable dtcity = DbConnectionDAL.GetDataTable(CommandType.Text, str);

       
     
            List<string> city = new List<string>();
            for (int i = 0; i < dtcity.Rows.Count; i++)
                  {

                    city.Add(dtcity.Rows[i]["City"].ToString());
                }
                       
            return city;
    
}
        
        private void BindTag()
        {
            string strBT = "SELECT Tag_Id,Tag FROM CRM_MastTag order by Tag";
            DataTable dtTag = new DataTable();
            dtTag = DbConnectionDAL.getFromDataTable(strBT);
            ddltag.DataSource = dtTag;
            ddltag.DataTextField = "Tag";
            ddltag.DataValueField = "Tag_Id";
            ddltag.DataBind();

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
        [WebMethod]
        private static int SaveData1()
        {
            int val = 0;

            return val;
        }
       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string chk = "N";
            string LeadType = "";
            //string confirmValue = Request.Form["confirm_value"];
            //if (confirmValue == "Yes")
            //{
            //    LeadType = "T";
            //}
            //else
            //{

            //    LeadType = "L";
            //}
            

            if (chkbxactive.Checked)
            {
                chk = "Y";
            }
            if (btnSave.Text == "Update")
            {
             int i=   Convert.ToInt32(ViewState["Contact_Id"]);
                update(i.ToString());
            }
            else
            {
              
                //if (Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, " SELECT count(*) FROM CRM_MastContact cm LEFT JOIN CRM_MastCompany cc ON CC.Comp_Id=CM.Contact_Id WHERE cm.FirstName ='" + Fname.Value.Replace("'", "''") + "' AND cm.LastName='" + Lname.Value.Replace("'", "''") + "' AND cc.CompName='" + Company.Value + "'")) > 0)
                //{
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record already exist for " + Fname.Value.Replace("'", "''") + Lname.Value.Replace("'", "''") + " ( " + Company.Value  + " )');", true);
                //}
                //else
                //{
                var phnval = Hidphonevalues.Value; var phnddlval = Hidphoneddlvalues.Value; 
                var phncontName = HidPhoneContNamehtml.Value; var EmailcontName = HidEmilCt.Value;
                var emailval = Hidemailvalues.Value; var emailddlval = Hidemailddlvalues.Value;
                var urlval = HidUrlvalues.Value; var urlddlval = HidUrlddlvalues.Value;
                string smIDStr1 = "";
               
                string strQ = "";

                if (Settings.Instance.UserName.ToUpper() == "SA")
                {

                   // strQ = " Select SMID,SMName from [MastSalesRep]   order by SMName "; //////////where SMName <>'.'
                    strQ = Settings.Instance.SMID;
                }

                else
                {
                    strQ = " SELECT Smid,SMName FROM MastSalesRep WHERE SMId IN (SELECT maingrp FROM MastSalesRepGrp WHERE SMId=" + Settings.Instance.SMID + ")";

                    DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    strQ = "";
                    for (int i = 0; i < dtOwner.Rows.Count; i++)
                    {

                        strQ += dtOwner.Rows[i]["SMID"].ToString() + ',';
                    }

                    strQ = strQ.TrimEnd(',');
                }
       
               


                smIDStr1 = strQ;


               // smIDStr1 = ddlsp.SelectedValue;
                string valuesToSave = string.Empty, ColsToSave = string.Empty;
                #region GetCustomValues
                if (!string.IsNullOrEmpty(hidcustomfields.Value))
                {
                    if (!string.IsNullOrEmpty(hidcustomval.Value))
                    {
                        string[] arrHidCV = hidcustomval.Value.Split('&');
                        if (arrHidCV.Length > 0)
                        {
                            for (int j = 0; j < arrHidCV.Length; j++)
                            {
                                string[] arrCFCols = arrHidCV[j].Split(':');
                                if (arrCFCols.Length > 1)
                                {
                                    for (int k = 0; k < arrCFCols.Length - 1; k++)
                                    {

                                        string[] arrHidCF = hidcustomfields.Value.Split('^');
                                        for (int i = 0; i < arrHidCF.Length; i++)
                                        {
                                            if (arrCFCols[k].ToString().Replace(" ", "") == arrHidCF[i].ToString().Replace(" ", ""))
                                            {
                                                if (string.IsNullOrEmpty(arrCFCols[k + 1]))
                                                {
                                                    valuesToSave += "' ',";
                                                }
                                                else
                                                {
                                                    string[] arrVS = arrCFCols[k + 1].Split('^');
                                                    if (arrVS.Length > 0)
                                                    {
                                                        string Cbvals = "'";
                                                        for (int m = 0; m < arrVS.Length; m++)
                                                        {
                                                           // Cbvals += arrVS[m] + ",";
                                                            if (arrVS.Length > 1)
                                                            {
                                                                Cbvals += arrVS[m] + "-";
                                                            }
                                                            else
                                                            {
                                                                Cbvals += arrVS[m] + ",";
                                                            }
                                                        }
                                                        Cbvals = Cbvals.Substring(0, Cbvals.Length - 1);
                                                        Cbvals += "',";
                                                        valuesToSave += Cbvals;
                                                    }
                                                    else
                                                    {
                                                        valuesToSave += "'" + arrCFCols[k + 1] + "',";
                                                    }
                                                }
                                                break;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    valuesToSave += "' ',";

                                }
                            }
                        }
                        valuesToSave = "," + valuesToSave.Substring(0, valuesToSave.Length - 1);
                    }
                }
                #endregion
                string[] arrCF = hidcustomfields.Value.Split('^');
                for (int i = 0; i < arrCF.Length; i++)
                {
                    ColsToSave += "[" + arrCF[i] + "],";
                }
                if (!string.IsNullOrEmpty(hidcustomfields.Value))
                    hidcustomfields.Value = "," + ColsToSave.Substring(0, ColsToSave.Length - 1);


                try
                {
                    if (txtcompzip.Value == "")
                    {
                        txtcompzip.Value = "0";
                    }
                    else
                    {
                       
                    }
                    String varname1 = "";
                    string CompID = "";
                    if (hidcompanyid.Value == "0")
                    {
                       
                        varname1 = varname1 + "insert into CRM_MastCompany(CompName,Description,Phone,Address,city,state,zip,country,StateId) OUTPUT INSERTED.Comp_Id values ('" + Company.Value.Replace("'", "''") + "','','" + txtcompphone.Value.Replace("'", "''") + "','" + txtcompadd.Value.Replace("'", "''") + "','" + txtcompcity.Text.Replace("'", "''") + "','" + txtcompstate.SelectedItem.Text.Replace("'", "''") + "','" + txtcompzip.Value.Replace("'", "''") + "','" + ddlcompcountry.SelectedValue + "','" + txtcompstate.SelectedValue + "')";
                        CompID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname1).ToString();
                    }else
                    {
                        varname1 = varname1 + "update CRM_MastCompany set CompName='" + Company.Value + "',	Description ='',Phone ='" + txtcompphone.Value + "',Address = '" + txtcompadd.Value + "', City = '" + txtcompcity.Text + "', State = '" + txtcompstate.SelectedItem.Text + "', Zip = " + txtcompzip.Value + ", Country = " + hidcompcountry.Value + ", StateId=" + txtcompstate.SelectedValue + " where comp_id=" + Convert.ToInt32(hidcompanyid.Value);
                        CompID = hidcompanyid.Value;
                    }


                   // string CompID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname1).ToString();

                    String varname12 = "";
                    varname12 = varname12 + "INSERT INTO [dbo].[CRM_MastContact] " + "\n";
                    varname12 = varname12 + "           ([FirstName] " + "\n";
                    varname12 = varname12 + "           ,[LastName] " + "\n";
                    varname12 = varname12 + "           ,[JobTitle] " + "\n";
                    varname12 = varname12 + "           ,[Address] " + "\n";
                    varname12 = varname12 + "           ,[City] " + "\n";
                    varname12 = varname12 + "           ,[State] " + "\n";
                    varname12 = varname12 + "           ,[Country] " + "\n";
                    varname12 = varname12 + "           ,[ZipCode] " + "\n";
                    varname12 = varname12 + "           ,[Status_Id] " + "\n";
                    varname12 = varname12 + "           ,[Tag_Id] " + "\n";
                    varname12 = varname12 + "           ,[Lead_Id] " + "\n";
                    varname12 = varname12 + "           ,[OwnerSp] " + "\n";
                    varname12 = varname12 + "           ,[Manager] " + "\n";
                    varname12 = varname12 + "           ,[SmId] " + "\n";
                    varname12 = varname12 + "           ,[CreatedDate] " + "\n";
                    varname12 = varname12 + "           ,[Flag] " + "\n";
                    varname12 = varname12 + "           ,[Active] " + "\n";
                    varname12 = varname12 + "           ,[StateId] " + "\n";
                    varname12 = varname12 + "           ,[Background],[CompanyId]" + hidcustomfields.Value + ") " + "\n OUTPUT INSERTED.Contact_Id";
                    varname12 = varname12 + "     VALUES " + "\n";
                    varname12 = varname12 + "           ('" + Fname.Value.Replace("'", "''") + "'\n";
                    varname12 = varname12 + "           ,' '\n";
                    varname12 = varname12 + "           ,'" + JobTitle.Value.Replace("'", "''") + "'\n";
                    varname12 = varname12 + "            ,'" + Address.Value.Replace("'", "''") + "'\n";
                    varname12 = varname12 + "           ,'" + City.Text.Replace("'", "''") + "'\n";
                    varname12 = varname12 + "          ,'" + State.SelectedItem.Text + "'\n";
                    varname12 = varname12 + "             ,'" + hidcontactcountry.Value + "'\n";
                    varname12 = varname12 + "            ,'" + Zip.Value.Replace("'", "''") + "'\n";
                    varname12 = varname12 + "            ,'" + ddlstatus.SelectedValue + "'\n";
                    varname12 = varname12 + "            ,'" + hidtags.Value + "'\n";
                    varname12 = varname12 + "             ,'" + ddlleadsource.SelectedValue + "'\n";
                    varname12 = varname12 + "             ,'" + smIDStr1 + "'\n";
                    varname12 = varname12 + "             ,'" + ddlsp.SelectedValue + "'\n";
                    varname12 = varname12 + "             ," + Settings.Instance.SMID + "\n";
                    varname12 = varname12 + "             ,Getdate() \n";
                    varname12 = varname12 + "             ,'L' \n";
                    varname12 = varname12 + "           ,'" + chk + "'," + State.SelectedValue + ",'" + txt.Text + "'," + (CompID) + valuesToSave + ")";
                    string ContactID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname12).ToString();
                    ViewState["contactID"] = Convert.ToInt32(ContactID);
                    ViewState["Fname"] = Fname.Value;
               
                    string[] arrPhnval = phnval.Split(',');
                    string[] arrPhnddlval = phnddlval.Split(',');
                    string[] phnctval = phncontName.Split(',');
                    string[] arrEmailval = emailval.Split(',');
                    for (int i = 0; i < arrPhnval.Length; i++)
                    {
                        if (arrPhnval[i].ToString() != "" || arrEmailval[i].ToString() != "")
                        {
                            String strphn = "";
                            strphn = strphn + "INSERT INTO [dbo].[CRM_ContactMobile] " + "\n";
                            strphn = strphn + "           ([Contact_Id] " + "\n";
                            strphn = strphn + "           ,[Phone] " + "\n";
                            strphn = strphn + "           ,[Email] " + "\n";
                            strphn = strphn + "           ,[PhoneType],ContactName) " + "\n";
                            strphn = strphn + "     VALUES " + "\n";
                            strphn = strphn + "           (" + (ContactID) + "\n";
                            strphn = strphn + "          ,'" + arrPhnval[i].ToString().Replace("'", "''") + "'\n";
                            strphn = strphn + "          ,'" + arrEmailval[i].ToString().Replace("'", "''") + "'\n";
                            strphn = strphn + "            ,'" + arrPhnddlval[i].ToString().Replace("'", "''") + "','" + phnctval[i].ToString().Replace("'", "''") + "')";

                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);
                        }
                    }
                 
                    string[] arrUrlval = urlval.Split(',');
                    string[] arrUrlddlval = urlddlval.Split(',');
                    for (int i = 0; i < arrUrlval.Length; i++)
                    {
                        if (arrUrlval[i].ToString() != "")
                        {
                            String strurl = "";
                            strurl = strurl + "INSERT INTO [dbo].[CRM_ContactURL] " + "\n";
                            strurl = strurl + "           ([Contact_Id] " + "\n";
                            strurl = strurl + "           ,[URL] " + "\n";
                            strurl = strurl + "           ,[URLType]) " + "\n";
                            strurl = strurl + "     VALUES " + "\n";
                            strurl = strurl + "           (" + (ContactID) + "\n";
                            strurl = strurl + "          ,'" + arrUrlval[i].ToString().Replace("'", "''") + "'\n";
                            strurl = strurl + "            ,'" + arrUrlddlval[i].ToString().Replace("'", "''") + "')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strurl);
                        }
                       
                    }
                    this.ModalPopupExtender2.Show();
                    if (Request.QueryString["Contact_Id"] != null)
                    {                     
                        Session["Insertmsg"] = "Record Inserted Successfully";
                    
                    }
                    else
                    {
                        rpt.DataSource = null;
                        rpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true); clear();
                    }
                   
                }

                catch (Exception ex) { ex.ToString(); System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true); }

            }
        //}
            BindState(); //BindCity(); 
            rpt.DataSource = null;
            rpt.DataBind();
        }
        protected void btnYes_Click(object sender,EventArgs e)
        {
            this.ModalPopupExtender2.Hide();
            this.ModalPopupExtender1.Show();
        }
        private void clear()
        {
            Hidemailvalues1.Value = string.Empty;
            Hidphonevalues1.Value = string.Empty;
            HidEmailContName1.Value = string.Empty;
            Hidphonevalues1.Value = string.Empty;
            HidUrlvalues1.Value = string.Empty;
            Fname.Value = string.Empty;
            //Lname.Value = string.Empty;
            JobTitle.Value = string.Empty;
            Company.Value = string.Empty;
            Phone1.Value = string.Empty;
            Email1.Value = string.Empty;
            Url1.Value = string.Empty;
            Address.Value = string.Empty;
            City.Text = string.Empty;
            State.SelectedIndex = 0;
            Zip.Value = string.Empty;
            ddlcountry.SelectedIndex = 0;
            ddlstatus.SelectedIndex = 0; 
            ddltag.SelectedIndex = 0;
            ddlleadsource.SelectedIndex = 0;
            //txtcompdesc.Value = string.Empty;
            txtcompadd.Value = string.Empty;
            txtcompcity.Text = string.Empty;
        
            txtcompphone.Value = string.Empty;
            txtcompstate.SelectedIndex = 0;
            txtcompzip.Value = string.Empty;
            txt.Text = string.Empty;
            hidcustomval.Value = string.Empty;
            Hidemailddlvalues.Value = string.Empty;
            Hidphoneddlvalues.Value = string.Empty;
            Hidemailvalues.Value = string.Empty;
            Hidphonevalues.Value = string.Empty;
            HidUrlddlvalues.Value = string.Empty;
            HidUrlvalues.Value = string.Empty;
            txtEmailcontact1.Value = string.Empty;
            Email1.Value = string.Empty;
            txtaddcontact1.Value = string.Empty;          
            Phone1.Value = string.Empty;
            HidEmailId1.Value = string.Empty; HidPhoneId1.Value = string.Empty; HidUrlId1.Value = string.Empty;
            rpt.DataSource = null;
            rpt.DataBind();
           // ClearControls();

        }
        protected void btnsavetask_Click(object sender,EventArgs e)
        {
            try
            {

                if (ViewState["contactID"].ToString() != "")
                {
                    //if (txtnxtactndt.Text != "")
                    //    {
                    DateTime chdate = Convert.ToDateTime(txtnxtactndt.Text);
                    string format = "MM/dd/yyyy HH:mm:ss"; 
                    //}
                    DateTime Currdt = Settings.GetUTCTime();
                    string docID = Settings.GetDocID("CRMTK", Currdt);
                    Settings.SetDocID("CRMT", docID);
                    String str = "";
                    str = str + "INSERT INTO [dbo].[CRM_Task] " + "\n";
                    str = str + "           ([DocId] " + "\n";
                    str = str + "           ,[AssignedTo] " + "\n";
                    str = str + "           ,[AssignedBy] " + "\n";
                    str = str + "           ,[Task] " + "\n";
                    str = str + "           ,[AssignDate] " + "\n";
                    str = str + "           ,[Ref_DocId] " + "\n";
                    str = str + "           ,[Ref_Sno] " + "\n";
                    str = str + "           ,[Status] " + "\n";
                    str = str + "           ,[CreatedBySmId] " + "\n";
                    str = str + "           ,[Contact_Id] " + "\n";
                    str = str + "           ,[CreatedDate]) " + "\n";
                    str = str + "     VALUES " + "\n";
                    str = str + "        ('" + docID + "',\n";
                    str = str + "          '" + ddlowner.SelectedValue + "',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "             '" + txtnxtaction.Text + "',\n";
                    str = str + "             '" + chdate.ToString(format) + "',\n";
                    str = str + "             '',\n";
                    str = str + "          '0',\n";
                    str = str + "              '" + DropDownList1.SelectedValue + "',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "            " + Convert.ToInt32(ViewState["contactID"].ToString()) + ",\n";
                    str = str + "             Getdate())";

                    DAL.DbConnectionDAL.ExecuteQuery(str);


                    string msgurl = ""; string displaytitle = "";
                    msgurl = "CRMTask.aspx?DocId=" + docID + " ";
                    string varname1 = "";
                    string Assignedby = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    string Assignedto = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + ddlowner.SelectedValue + " "));
                    int Assigntouserid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select UserId From MastSalesRep where SMId = " + ddlowner.SelectedValue + " "));
                    displaytitle = "Task By - " + Assignedby + " , To - " + Assignedto + "  " + txtnxtactndt.Text;
                    varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('CRMTASK', " + Assigntouserid + ", Getdate(),'" + msgurl + "', '" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + ddlowner.SelectedValue + ")";

                    DAL.DbConnectionDAL.ExecuteQuery(varname1);


                    if (ViewState["contactID"].ToString() != "")
                    {
                        string strupdowner = "update [CRM_MastContact] set OwnerSp =  (case when  [OwnerSp]  Like '%" + ddlowner.SelectedValue + "%' then [OwnerSp] else [OwnerSp] + ',' + '" + ddlowner.SelectedValue + "' end)   where Contact_Id=" + Convert.ToInt32(ViewState["contactID"].ToString()) + "";
                        DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
                    }


                    if (Request.QueryString["Contact_Id"] != null)
                    {
                      //  Session["Insertmsg"] = "Record Inserted Successfully";
                        //this.ModalPopupExtender2.Show();
                        string s = Request.QueryString["Contact_Id"];
                        Response.Redirect("CRMTask.aspx?Contact_Id=" + s);

                    }
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
          
            string s = Request.QueryString["Contact_Id"];
            Response.Redirect("CRMTask.aspx?Contact_Id=" + s);
        }
        protected void btnno_Click(object sender,EventArgs e)
        {
            string chdate = "";
            try
            {
                if (ViewState["contactID"].ToString() != "")
                {
                    if (txtnxtactndt.Text != "")
                    {
                        chdate = Convert.ToDateTime(txtnxtactndt.Text).ToShortDateString();
                    }
                    DateTime Currdt = Settings.GetUTCTime();
                    string docID = Settings.GetDocID("CRMTK", Currdt);
                    Settings.SetDocID("CRMT", docID);
                    String str = "";
                    str = str + "INSERT INTO [dbo].[CRM_Task] " + "\n";
                    str = str + "           ([DocId] " + "\n";
                    str = str + "           ,[AssignedTo] " + "\n";
                    str = str + "           ,[AssignedBy] " + "\n";
                    str = str + "           ,[Task] " + "\n";
                    str = str + "           ,[AssignDate] " + "\n";
                    str = str + "           ,[Ref_DocId] " + "\n";
                    str = str + "           ,[Ref_Sno] " + "\n";
                    str = str + "           ,[Status] " + "\n";
                    str = str + "           ,[CreatedBySmId] " + "\n";
                    str = str + "           ,[Contact_Id] " + "\n";
                    str = str + "           ,[CreatedDate]) " + "\n";
                    str = str + "     VALUES " + "\n";
                    str = str + "        ('" + docID + "',\n";
                    str = str + "          '" + Settings.Instance.SMID + "',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "             '" + ViewState["Fname"].ToString() + "',\n";
                    str = str + "             Getdate(),\n";
                    str = str + "             '',\n";
                    str = str + "          '0',\n";
                    str = str + "              'o',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "            " + Convert.ToInt32(ViewState["contactID"].ToString()) + ",\n";
                    str = str + "             Getdate())";


                    DAL.DbConnectionDAL.ExecuteQuery(str);


                    //string msgurl = ""; string displaytitle = "";
                    //msgurl = "CRMTask.aspx?DocId=" + docID + " ";
                    //string varname1 = "";
                    //string Assignedby = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //string Assignedto = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //int Assigntouserid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select UserId From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //displaytitle = "Task By - " + Assignedby + " , To - " + Assignedto + "  " + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy"); 
                    //varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('CRMTASK', " + Assigntouserid + ", Getdate(),'" + msgurl + "', '" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + Settings.Instance.SMID + ")";

                    //DAL.DbConnectionDAL.ExecuteQuery(varname1);

                    if (Request.QueryString["Contact_Id"] != null)
                    {
                       // Session["Insertmsg"] = "Record Inserted Successfully";
                        //this.ModalPopupExtender2.Show();
                        string s = Request.QueryString["Contact_Id"];
                        Response.Redirect("CRMTask.aspx?Contact_Id=" + s);

                    }

                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnFind_Click1(object sender, EventArgs e)
        {
            //fillRepeter();
            //  ClearControls();
            btndelete.Visible = false;
            btnSave.Text = "Save";
            hidchk.Value = "";
            Hidemailvalues1.Value = "";
            Hidemailddlvalues1.Value = "";
            Hidphonevalues1.Value = "";
            Hidphoneddlvalues1.Value = "";
            HidUrlddlvalues1.Value = "";
            HidUrlvalues1.Value = "";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
            BindCountry();
            BindState1();
            BindCityList();
        }

        private void fillRepeter()
        {
            string stradd = "";
          
            //if(ddlCountryList.SelectedValue !="0")
            //{
            //    stradd = "AND cmc.Country='" + ddlCountryList.SelectedValue + "'";
            //}
            //if (ddlState.SelectedValue != "0")
            //{
            //    stradd += "AND cmc.State='" + ddlState.SelectedItem.Text + "'";
            //}
            //if (ddlcity.SelectedValue != "0")
            //{
            //    stradd += " AND cmc.City='" + ddlcity.SelectedItem.Text + "'";
            //} 


            if (ddlCountryList.SelectedValue != "0")
            {
                stradd = "AND cmc.Country='" + ddlCountryList.SelectedValue + "'";
            }
            if (ddlState.SelectedValue != "0")
            {
                stradd += "AND cmc.Stateid='" + ddlState.SelectedValue + "'";
            }
            if (ddlcity.SelectedValue != "0")
            {
                stradd += " AND cmc.City='" + ddlcity.SelectedItem.Text + "'";
            }        
            
            //if (!string.IsNullOrEmpty(ddlcity.SelectedValue))
            //{
            //    stradd += " AND cmc.City='" + ddlcity.SelectedItem.Text + "'";
            //}
            //else if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            //{
            //    stradd += "AND cmc.StateId='" + ddlState.SelectedValue + "'";
            //}
            //else if (!string.IsNullOrEmpty(ddlCountryList.SelectedValue))
            //{
            //    stradd = "AND cmc.Country='" + ddlCountryList.SelectedValue + "'";
            //}
           
            //if (Settings.Instance.UserName.ToUpper() == "SA")
            //{

            //}
            //else
            //{

            //    string str12 = "select SMId,SMName from MastSalesRep where (smid in (select maingrp from MastSalesRepGrp where smid=" + Settings.Instance.SMID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + Settings.Instance.SMID + ") )  order by SMName";
            //    DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, str12);
            //    //DataTable dtOwner = Settings.UnderUsers(Settings.Instance.SMID);
            //    string strQ = "";
            //    for (int i = 0; i < dtOwner.Rows.Count; i++)
            //    {

            //        strQ += dtOwner.Rows[i]["SMID"].ToString() + ',';
            //    }
            //    strQ = strQ.TrimEnd(',');
            //    stradd += " AND cmc.SmId In (" + strQ + ")";
            //}

            stradd += " AND cmc.OwnerSp LIKE '%" + Settings.Instance.SMID +"%'";
            string str = @" 					
		select (select top 1( cmco.CompName) from CRM_MastCompany cmco where cmco.Comp_Id=cmc.CompanyId) as Compname,
		( select top 1( cms.Status) from CRM_MastStatus cms where cms.Status_Id=cmc.Status_Id ) as Status ,
		(select top 1(ccu.URL )from CRM_ContactURL ccu where cmc.Contact_Id=ccu.Contact_Id) as Url,
		(select top 1( ccm.Email) from CRM_ContactMobile ccm Where ccm.Contact_Id=cmc.Contact_Id) as email,
		(select top 1( ccm.Phone) from CRM_ContactMobile ccm Where ccm.Contact_Id=cmc.Contact_Id) as Phone, cmc.* from CRM_MastContact  cmc where Isnull(Flag,'') <>'T' " + stradd + " order by cmc.FirstName,cmc.LastName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void fillddlphonetype()
        {
            string str = @"select * from CRM_MastContactType where Data='PHONE' order by sort asc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlphonetype1.DataSource = dt;
            ddlphonetype1.DataTextField = "Value";
            ddlphonetype1.DataValueField = "Value";
            ddlphonetype1.DataBind();
        }
        private void fillddlEmailtype() 
        {
            string str = @"select * from CRM_MastContactType where Data='EMAIL' order by sort asc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlemailtype1.DataSource = dt;
            ddlemailtype1.DataTextField = "Value";
            ddlemailtype1.DataValueField = "Value";
            ddlemailtype1.DataBind();
        }
        private void fillddlUrltype() 
        {
            string str = @"select * from CRM_MastContactType where Data='WEB' order by sort asc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlurltype1.DataSource = dt;
            ddlurltype1.DataTextField = "Value";
            ddlurltype1.DataValueField = "Value";
            ddlurltype1.DataBind();
        }
        private void FillControls(int Contact_Id)
        {
            try
            {
                
                if (hidcustomfields.Value != "")
                {
                    string sq = hidcustomfields.Value;
                }
                Hidallurltype.Value = "";
                Hidallemailddlvalues1.Value = ""; Hidphoneallddlvalues1.Value = ""; HidEmailContName1.Value = ""; HidPhoneContName1.Value = ""; HidEmailId1.Value = ""; HidPhoneId1.Value = ""; HidUrlId1.Value = "";
                var phnval = Hidphonevalues.Value; var phnddlval = Hidphoneddlvalues.Value;
                var emailval = Hidemailvalues.Value; var emailddlval = Hidemailddlvalues.Value;
                var urlval = HidUrlvalues.Value; var urlddlval = HidUrlddlvalues.Value;
                string email = "Select * from CRM_ContactEmail where Contact_id="+Contact_Id+"";
                DataTable dtemail = DbConnectionDAL.GetDataTable(CommandType.Text, email);
                   email = "select value from CRM_MastContactType where Data='EMAIL'";
                DataTable dtaaemailtype = DbConnectionDAL.GetDataTable(CommandType.Text, email);
                if (dtemail.Rows.Count > 1)
                {
                    for (int i = 0; i < dtaaemailtype.Rows.Count;i++ )
                    {
                        Hidallemailddlvalues1.Value += dtaaemailtype.Rows[i]["value"].ToString() + ",";

                    }
                        for (int i = 1; i < dtemail.Rows.Count; i++)
                        {
                            Hidemailvalues1.Value += dtemail.Rows[i]["Email"].ToString() + ",";
                            Hidemailddlvalues1.Value += dtemail.Rows[i]["EmailType"].ToString() + ",";
                            HidEmailContName1.Value += dtemail.Rows[i]["ContactName"].ToString() + ",";
                            HidEmailId1.Value += dtemail.Rows[i]["CEmail_Id"].ToString() + ",";

                        }
                    Hidemailvalues1.Value = Hidemailvalues1.Value.TrimEnd(',');
                    Hidemailddlvalues1.Value = Hidemailddlvalues1.Value.TrimEnd(',');
                    HidEmailContName1.Value = HidEmailContName1.Value.TrimEnd(',');
                    Hidallemailddlvalues1.Value = Hidallemailddlvalues1.Value.TrimEnd(',');
                    HidEmailId1.Value = HidEmailId1.Value.TrimEnd(',');
                }

                string phone = "Select * from CRM_ContactMobile where Contact_id=" + Contact_Id + "";//
                DataTable dtphone = DbConnectionDAL.GetDataTable(CommandType.Text, phone);
                phone = "select value from CRM_MastContactType where Data='PHONE'";
                DataTable dtallphonetype = DbConnectionDAL.GetDataTable(CommandType.Text, phone);
                for (int i = 0; i < dtallphonetype.Rows.Count; i++)
                {
                    Hidphoneallddlvalues1.Value += dtallphonetype.Rows[i]["value"].ToString() + ",";

                }
                if (dtphone.Rows.Count > 1)
                {
                    for (int i =1; i < dtphone.Rows.Count; i++)
                    {
                        Hidphonevalues1.Value += dtphone.Rows[i]["phone"].ToString() + ",";
                        Hidphoneddlvalues1.Value += dtphone.Rows[i]["phonetype"].ToString() + ",";
                        HidPhoneContName1.Value += dtphone.Rows[i]["ContactName"].ToString() + ",";
                        HidPhoneId1.Value += dtphone.Rows[i]["CMbl_Id"].ToString() + ",";
                        Hidemailvalues1.Value += dtphone.Rows[i]["Email"].ToString() + ",";
                    }
                    Hidphoneallddlvalues1.Value = Hidphoneallddlvalues1.Value.TrimEnd(',');
                    if (Hidphonevalues1.Value.EndsWith(","))
                    {
                        Hidphonevalues1.Value = Hidphonevalues1.Value.Substring(0, Hidphonevalues1.Value.Length - 1);

                          //   dealval = dealval.substr(0, dealval.length - 1);
                    }
                    if (Hidphoneddlvalues1.Value.EndsWith(","))
                    {
                        Hidphoneddlvalues1.Value = Hidphoneddlvalues1.Value.Substring(0, Hidphoneddlvalues1.Value.Length - 1);

                    }
                    if (HidPhoneContName1.Value.EndsWith(","))
                    {
                        HidPhoneContName1.Value = HidPhoneContName1.Value.Substring(0, HidPhoneContName1.Value.Length - 1);
                    }
                    if (HidPhoneId1.Value.EndsWith(","))
                    {
                        HidPhoneId1.Value = HidPhoneId1.Value.Substring(0, HidPhoneId1.Value.Length - 1);
                    }
                    if (Hidemailvalues1.Value.EndsWith(","))
                    {
                        Hidemailvalues1.Value = Hidemailvalues1.Value.Substring(0, Hidemailvalues1.Value.Length - 1);
                    }
                   // Hidphonevalues1.Value = Hidphonevalues1.Value.TrimEnd(',');
                 //   Hidphoneddlvalues1.Value = Hidphoneddlvalues1.Value.TrimEnd(',');
                   // HidPhoneContName1.Value = HidPhoneContName1.Value.TrimEnd(',');
                  //  HidPhoneId1.Value = HidPhoneId1.Value.TrimEnd(',');
                   // Hidemailvalues1.Value = Hidemailvalues1.Value.TrimEnd(',');
                }
                string url = "Select * from CRM_ContactURL where Contact_id=" + Contact_Id + "";
                DataTable dturl = DbConnectionDAL.GetDataTable(CommandType.Text, url);
                url = "select value from CRM_MastContactType where Data='WEB'";
                DataTable dtallurltype = DbConnectionDAL.GetDataTable(CommandType.Text, url);
                for (int k = 0; k < dtallurltype.Rows.Count; k++)
                {

                    Hidallurltype.Value += dtallurltype.Rows[k]["value"].ToString() + ",";
                }
                if (dturl.Rows.Count > 1)
                {
                    for (int i = 1; i < dturl.Rows.Count; i++)
                    {
                        HidUrlvalues1.Value += dturl.Rows[i]["Url"].ToString() + ",";
                        HidUrlddlvalues1.Value += dturl.Rows[i]["Urltype"].ToString() + ",";
                        HidUrlId1.Value += dturl.Rows[i]["CUrl_Id"].ToString() + ",";
                    }

                    if (HidUrlvalues1.Value.EndsWith(","))
                    {
                        HidUrlvalues1.Value = HidUrlvalues1.Value.Substring(0, HidUrlvalues1.Value.Length - 1);
                    }
                    if (Hidallurltype.Value.EndsWith(","))
                    {
                        Hidallurltype.Value = Hidallurltype.Value.Substring(0, Hidallurltype.Value.Length - 1);
                    }
                    if (HidUrlddlvalues1.Value.EndsWith(","))
                    {
                        HidUrlddlvalues1.Value = HidUrlddlvalues1.Value.Substring(0, HidUrlddlvalues1.Value.Length - 1);
                    }
                    if (HidUrlId1.Value.EndsWith(","))
                    {
                        HidUrlId1.Value = HidUrlId1.Value.Substring(0, HidUrlId1.Value.Length - 1);
                    }
                   // HidUrlvalues1.Value = HidUrlvalues1.Value.TrimEnd(',');
                  //  Hidallurltype.Value = Hidallurltype.Value.TrimEnd(',');
                   // HidUrlddlvalues1.Value = HidUrlddlvalues1.Value.TrimEnd(',');
                  //  HidUrlId1.Value = HidUrlId1.Value.TrimEnd(',');
                }

                string str = @"  select cce.ContactName as emailCt,ccm.contactName as PhnCt,cmco.Description as compDesc,cmco.State as compState,cmco.City as compCity,cmco.Zip as compZip,cmco.Country as compCountry,cmco.StateId as CompStateId,cmco.Phone as compPhone,cmco.Address as txtcompadd,cmco.CompName,cms.Status,ccu.URL,ccm.Email,ccm.Phone,cce.CEmail_Id As emailid, ccm.CMbl_Id as mobileid,ccu.CUrl_Id as urlid, cmc.* from CRM_MastContact cmc left join CRM_ContactURL ccu on cmc.Contact_Id=ccu.Contact_Id
                             left join CRM_ContactEmail cce on cce.Contact_Id=cmc.Contact_Id
                             left join CRM_ContactMobile ccm on ccm.Contact_Id=cmc.Contact_Id
                             left join CRM_MastStatus cms on cms.Status_Id=cmc.Status_Id
                            left join CRM_MastCompany cmco on cmco.Comp_Id=cmc.CompanyId where cmc.Contact_Id=" + Contact_Id;
                ValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (ValueDt.Rows.Count > 0)
                {
                    string strQ = " select * FROM [dbo].[CRM_CustomFields] where [AttributeTable]='Contact' and Isnull(Active,1)=1  order by Custom_Id";
                    DataTable dtCustomfield = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    if (dtCustomfield.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCustomfield.Rows.Count; i++)
                        {
                            for (int j = 0; j < ValueDt.Columns.Count; j++)
                            {
                                string sw = dtCustomfield.Rows[i]["AttributeField"].ToString().ToUpper();
                                string ew = ValueDt.Columns[j].ToString().ToUpper();
                                // if (dtCustomfield.Rows[i]["AttributeField"] ==ValueDt.Columns[j])
                                int s = sw.CompareTo(ew);
                                if (sw.CompareTo(ew) == 0)
                                {
                                    //hidcustomval.Value += dtCustomfield.Rows[i]["AttributeField"].ToString() +":"+ ValueDt.Columns[j].ToString()+",";
                                    hidchk.Value += dtCustomfield.Rows[i]["AttributeField"].ToString() + ":" + ValueDt.Rows[0][ValueDt.Columns[j].ToString()].ToString() + ",";
                                }

                            }

                        }

                    }
                  
                    hidchk.Value = hidchk.Value.TrimStart(',');
              
                    hidContactid.Value = (Contact_Id).ToString();
                
                    if (!string.IsNullOrEmpty(hidcustomfields.Value))
                    {
                        string[] val = hidcustomfields.Value.Split('^');
                        string w = val[0];
                    }
                   // Lead.Value = ValueDt.Rows[0]["Lead"].ToString();
                    txtcompcity.Text = ValueDt.Rows[0]["compCity"].ToString();
                    txtcompstate.SelectedValue = ValueDt.Rows[0]["CompStateId"].ToString();
                    txtcompzip.Value = ValueDt.Rows[0]["compZip"].ToString();
                    ddlcompcountry.SelectedValue = ValueDt.Rows[0]["compCountry"].ToString();

                    //txtcompdesc.Value = ValueDt.Rows[0]["compDesc"].ToString();
                    txtcompphone.Value = ValueDt.Rows[0]["compPhone"].ToString();
                    txtcompadd.Value = ValueDt.Rows[0]["txtcompadd"].ToString();
                    ddlphonetype1.DataValueField=dtphone.Rows[0]["phonetype"].ToString();
                    //ddlemailtype1.DataValueField = dtemail.Rows[0]["Emailtype"].ToString();
                  //  ddlurltype1.DataValueField = dturl.Rows[0]["Urltype"].ToString();
                    Fname.Value = ValueDt.Rows[0]["FirstName"].ToString();
                    //Lname.Value = ValueDt.Rows[0]["LastName"].ToString();
                    //JobTitle.Value = ValueDt.Rows[0]["JobTitle"].ToString();
                    Company.Value = ValueDt.Rows[0]["CompName"].ToString();
                    Phone1.Value = ValueDt.Rows[0]["Phone"].ToString();
                    txtaddcontact1.Value = ValueDt.Rows[0]["PhnCt"].ToString();
                    txtEmailcontact1.Value=ValueDt.Rows[0]["emailCt"].ToString();
                    Email1.Value = ValueDt.Rows[0]["Email"].ToString();
                    Url1.Value = ValueDt.Rows[0]["URL"].ToString();
                    urlid.Value=ValueDt.Rows[0]["urlid"].ToString();
                    emailid.Value=ValueDt.Rows[0]["emailid"].ToString();
                    phoneid.Value=ValueDt.Rows[0]["mobileid"].ToString();

                    ddlsp.SelectedValue = ValueDt.Rows[0]["Manager"].ToString();
                    Address.Value = ValueDt.Rows[0]["Address"].ToString();
                    City.Text = ValueDt.Rows[0]["City"].ToString();
                    State.SelectedValue = ValueDt.Rows[0]["StateId"].ToString();
                    Zip.Value = ValueDt.Rows[0]["ZipCode"].ToString();
                    ddlcountry.SelectedValue = ValueDt.Rows[0]["Country"].ToString();
                    ddlstatus.SelectedValue = ValueDt.Rows[0]["Status_Id"].ToString();
                    ddltag.Value = ValueDt.Rows[0]["Tag_Id"].ToString();
                    ddlleadsource.SelectedValue = ValueDt.Rows[0]["Lead_Id"].ToString();
                    txt.Text = ValueDt.Rows[0]["Background"].ToString();
                    hidcustomval.Value = ValueDt.Rows[0]["Contact_Id"].ToString();
                    string www = hidcustomfields.Attributes.ToString();
                    btnSave.Text = "Update";
                    btndelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
            finally
            {
                
            }
        }

        private void ClearControls()
        {
            if (Request.QueryString["Contact_Id"] != null)
            {
                string s = Request.QueryString["Contact_Id"];
                Response.Redirect("CRMTask.aspx?Contact_Id=" + s);
            }
            else
            {
             //   Response.Redirect("CRMContact.aspx");
                hidchk.Value = "";
                clear();
            }
        }

        protected void btndelete_Click1(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = CB.deleteLeadContact(Convert.ToString(ViewState["Contact_Id"]));
                if (retdel == 1)
                {
                    Session["Msg"]="Record Deleted Successfully";
                    Session["Insertmsg"] = "Record Deleted Successfully";
                    
                   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btndelete.Visible = false;
                    btnSave.Text = "Save";
                    // Lead.Focus();
                }
                else
                {
                    Session["Insertmsg"] = "Record Cannot Be Deleted As It Is In Use";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot Be Deleted As It Is In Use');", true);
                    btndelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();
                    //  Lead.Focus();
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClearControls();
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void update(string contactid)
        {
            string chk = "N";
            if (chkbxactive.Checked)
            {
            }
            var phncontName = HidPhoneContNamehtml.Value; var EmailcontName = HidEmilCt.Value;
            string[] phnctval = phncontName.Split(','); string[] EmailCtArr = EmailcontName.Split(',');
            int Contact_id = Convert.ToInt32(contactid);
            ValueDt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select * from CRM_MastContact where contact_id=" + Contact_id + "");
            var phnval = Hidphonevalues.Value; var phnddlval = Hidphoneddlvalues.Value; var phoneid = HidPhoneID.Value;
            var emailval = Hidemailvalues.Value; var emailddlval = Hidemailddlvalues.Value; var emailid = HidEmailId.Value;
            var urlval = HidUrlvalues.Value; var urlddlval = HidUrlddlvalues.Value; var urlid = HidurlId.Value;
            string smIDStr1 = "";
            //foreach (ListItem item in ListBox1.Items)
            //{
            //    if (item.Selected)
            //    {
            //        smIDStr1 += item.Value + ",";
            //    }
            //}
          //  smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            string valuesToSave = string.Empty, ColsToSave = string.Empty;
            #region GetCustomValues
            if (!string.IsNullOrEmpty(hidcustomfields.Value))// all custom fields
            {
                if (!string.IsNullOrEmpty(hidcustomval.Value))// all custom fields with value
                {
                    string[] arrHidCV = hidcustomval.Value.Split('&');
                    if (arrHidCV.Length > 0)
                    {
                        for (int j = 0; j < arrHidCV.Length; j++)
                        {
                            string[] arrCFCols = arrHidCV[j].Split(':');
                            if (arrCFCols.Length > 1)
                            {
                                for (int k = 0; k < arrCFCols.Length - 1; k++)
                                {

                                    string[] arrHidCF = hidcustomfields.Value.Split('^');
                                    for (int i = 0; i < arrHidCF.Length; i++)
                                    {
                                        if (arrCFCols[k].ToString().Replace(" ", "") == arrHidCF[i].ToString().Replace(" ", ""))
                                        {
                                            if (string.IsNullOrEmpty(arrCFCols[k + 1]))
                                            {
                                                valuesToSave += "' ',";
                                            }
                                            else
                                            {
                                                string[] arrVS = arrCFCols[k + 1].Split('^');
                                                if (arrVS.Length > 0)
                                                {
                                                    string Cbvals = "'";
                                                    for (int m = 0; m < arrVS.Length; m++)
                                                    {
                                                        if (arrVS.Length > 1)
                                                        {
                                                            Cbvals += arrVS[m] + "-";
                                                        }
                                                        else
                                                        {
                                                            Cbvals += arrVS[m] + ",";
                                                        }
                                                    }
                                                    Cbvals = Cbvals.Substring(0, Cbvals.Length - 1);
                                                    Cbvals += "',";
                                                    valuesToSave += Cbvals;
                                                }
                                                else
                                                {
                                                    valuesToSave += "'" + arrCFCols[k + 1] + "',";
                                                }
                                            }
                                            break;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                valuesToSave += "' ',";

                            }
                        }
                    }
                    valuesToSave = "," + valuesToSave.Substring(0, valuesToSave.Length - 1);
                    valuesToSave = valuesToSave.TrimStart(',');
                }
            }
            #endregion
            string[] arrCF = hidcustomfields.Value.Split('^');
            for (int i = 0; i < arrCF.Length; i++)
            {
                arrCF[i] = "[" + arrCF[i] + "]";
            }
            if (!string.IsNullOrEmpty(hidcustomfields.Value))
             //   hidcustomfields.Value = "," + ColsToSave.Substring(0, ColsToSave.Length - 1);  
    
            try
            {
                string CompID = "";
                String varname1 = "";
                //string ww = hidcustomfields.Value.TrimStart(',');
                //string[] arrHidCF = ww.Split(',');
                string[] arrHidCF = arrCF;
                if (txtcompzip.Value == "")
                {
                    txtcompzip.Value = "0";
                }
                else
                {
                  
                }
                varname1 = varname1 + "update CRM_MastCompany set CompName='" + Company.Value + "',	Description ='',Phone ='" + txtcompphone.Value + "',Address = '" + txtcompadd.Value + "', City = '" + txtcompcity.Text + "', State = '" + txtcompstate.SelectedItem.Text + "', Zip = " + txtcompzip.Value + ", Country = " + ddlcompcountry.SelectedValue + ", StateId=" + txtcompstate.SelectedValue  + " where comp_id=" + Convert.ToInt32(ValueDt.Rows[0]["CompanyId"].ToString());
                DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname1);
              
                string f = "";
                for (int i = 0; i < arrHidCF.Length; i++)
                {
                    string[] ff = valuesToSave.Split(',');
                    for (int w = 0; w < ff.Length; w++)
                    {
                        if (i == w)
                        {
                            f += arrHidCF[i] + "=" + ff[w] + ",";

                        }
                    }
                }
                f = f.TrimEnd(','); string strQ = "";

                if (Settings.Instance.UserName.ToUpper() == "SA")
                {
                    // strQ = " Select SMID,SMName from [MastSalesRep]   order by SMName "; //////////where SMName <>'.'
                    strQ = Settings.Instance.SMID;
                }

                else
                {
                    strQ = " SELECT Smid,SMName FROM MastSalesRep WHERE SMId IN (SELECT maingrp FROM MastSalesRepGrp WHERE SMId=" + Settings.Instance.SMID + ")";
                    DataTable dtOwner = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                    strQ = "";
                    for (int i = 0; i < dtOwner.Rows.Count; i++)
                    {

                        strQ += dtOwner.Rows[i]["SMID"].ToString() + ',';
                    }

                    strQ = strQ.TrimEnd(',');

                }

             
                smIDStr1 = strQ;

                String varname12 = "";
                varname12 = varname12 + "Update [dbo].[CRM_MastContact] set " + "\n";
                varname12 = varname12 + "           [FirstName] ='" + Fname.Value.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[LastName] =' '" + "\n";
                varname12 = varname12 + "           ,[JobTitle] ='" + JobTitle.Value.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[Address] ='" + Address.Value.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[City] ='" + City.Text.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[State] ='" + State.SelectedItem.Text.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[StateId] =" + State.SelectedValue + "" + "\n";
                varname12 = varname12 + "           ,[Country]= '" + ddlcountry.SelectedValue + "'" + "\n";
                varname12 = varname12 + "           ,[ZipCode]= '" + Zip.Value.Replace("'", "''") + "'" + "\n";
                varname12 = varname12 + "           ,[Active]= '" + chk + "'" + "\n";
                varname12 = varname12 + "           ,[Status_Id] ='" + ddlstatus.SelectedValue + "'" + "\n";
                varname12 = varname12 + "           ,[Tag_Id] ='" + hidtags.Value + "'" + "\n";
                varname12 = varname12 + "           ,[Lead_Id]= '" + ddlleadsource.SelectedValue + "'" + "\n";
                //varname12 = varname12 + "           ,[OwnerSp] =" + Settings.Instance.SMID + "" + "\n";
                //varname12 = varname12 + "           ,[OwnerSp] =  (case when  [OwnerSp]  Like '%" + ddlsp.SelectedValue + "%' then [OwnerSp] else [OwnerSp] + ',' + '" + ddlsp.SelectedValue + "' end)  " + "\n";
                varname12 = varname12 + "           ,[OwnerSp] =  '" + smIDStr1 + "'" + "\n";
                varname12 = varname12 + "           ,[Manager] = '" + ddlsp.SelectedValue + "'" + "\n";
                varname12 = varname12 + "           ,[SmId] ='" + Settings.Instance.SMID + "'" + "\n";
                varname12 = varname12 + "           ,[CreatedDate] =Getdate()" + "\n";
                varname12 = varname12 + "           ,[Background] ='" + txt.Text.Replace("'", "''") + "'," + f + "  where Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]);
                DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, varname12);
                {
                //    DataTable dtmobid = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select * from CRM_ContactMobile where Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]));
                    DataTable dtmobid = new DataTable();
                        string[] arrPhnval = phnval.Split(',');
                        string[] arrPhnddlval = phnddlval.Split(',');
                        string[] arrEmailval = emailval.Split(',');
                        string[] arrphoneid = phoneid.Split(',');
                        for (int k = 0; k < arrPhnval.Length; k++)
                        {
                            
                                            String strphn = "";
                                            if (arrphoneid[k].ToString() != "")
                                            {
                                                //dtmobid = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select * from CRM_ContactMobile where Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]) + " and CMbl_Id =" + Convert.ToInt32(arrphoneid[k].ToString()));




                                                //if (dtmobid.Rows.Count == 0)
                                                //{

                                                //    strphn = strphn + "INSERT INTO [dbo].[CRM_ContactMobile] " + "\n";
                                                //    strphn = strphn + "           ([Contact_Id] " + "\n";
                                                //    strphn = strphn + "           ,[Phone] " + "\n";
                                                //    strphn = strphn + "           ,[Email] " + "\n";
                                                //    strphn = strphn + "           ,[PhoneType],ContactName) " + "\n";
                                                //    strphn = strphn + "     VALUES " + "\n";
                                                //    strphn = strphn + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                                                //    strphn = strphn + "          ,'" + arrPhnval[k].ToString().Replace("'", "''") + "'\n";
                                                //    strphn = strphn + "          ,'" + arrEmailval[k].ToString().Replace("'", "''") + "'\n";
                                                //    strphn = strphn + "            ,'" + arrPhnddlval[k].ToString().Replace("'", "''") + "','" + phnctval[k].ToString().Replace("'", "''") + "')";
                                                //    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);


                                                //}
                                                //else
                                                //{
                                                    strphn = strphn + "update [dbo].[CRM_ContactMobile] set" + "\n";

                                                    strphn = strphn + "           [Phone]='" + arrPhnval[k].ToString().Replace("'", "''") + "' " + "\n";

                                                    strphn = strphn + "           ,[Email]='" + arrEmailval[k].ToString().Replace("'", "''") + "' " + "\n";
                                                    strphn = strphn + "           ,[PhoneType]='" + arrPhnddlval[k].ToString() + "' " + "\n";
                                                    strphn = strphn + "           ,[ContactName]='" + phnctval[k].ToString().Replace("'", "''") + "' " + "\n";
                                                    strphn = strphn + "            where CMbl_Id=" + Convert.ToInt32(arrphoneid[k].ToString()) + " and  Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]);

                                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);

                                                //}
                                            }
                                            else
                                            {
                                                if (arrPhnval[k].ToString() != "" || arrEmailval[k].ToString() != "")
                                                {
                                                    strphn = strphn + "INSERT INTO [dbo].[CRM_ContactMobile] " + "\n";
                                                    strphn = strphn + "           ([Contact_Id] " + "\n";
                                                    strphn = strphn + "           ,[Phone] " + "\n";
                                                    strphn = strphn + "           ,[Email] " + "\n";
                                                    strphn = strphn + "           ,[PhoneType],ContactName) " + "\n";
                                                    strphn = strphn + "     VALUES " + "\n";
                                                    strphn = strphn + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                                                    strphn = strphn + "          ,'" + arrPhnval[k].ToString().Replace("'", "''") + "'\n";
                                                    strphn = strphn + "          ,'" + arrEmailval[k].ToString().Replace("'", "''") + "'\n";
                                                    strphn = strphn + "            ,'" + arrPhnddlval[k].ToString().Replace("'", "''") + "','" + phnctval[k].ToString().Replace("'", "''") + "')";
                                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);
                                                }
                                            }
                                            //else if (arrPhnval[k].ToString() == "" && phnctval[k].ToString() =="")
                                            //{

                                            //}
                                            //else 
                                            //{
                                            //    strphn = strphn + "INSERT INTO [dbo].[CRM_ContactMobile] " + "\n";
                                            //    strphn = strphn + "           ([Contact_Id] " + "\n";
                                            //    strphn = strphn + "           ,[Phone] " + "\n";
                                            //    strphn = strphn + "           ,[PhoneType],ContactName) " + "\n";
                                            //    strphn = strphn + "     VALUES " + "\n";
                                            //    strphn = strphn + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                                            //    strphn = strphn + "          ,'" + arrPhnval[k].ToString().Replace("'", "''") + "'\n";
                                            //    strphn = strphn + "            ,'" + arrPhnddlval[k].ToString().Replace("'", "''") + "','" + phnctval[k].ToString().Replace("'", "''") + "')";
                                            //    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strphn);
                                            //}
                                                       
                          
                        }
                   
                    //string[] arrEmailval = emailval.Split(',');

                    //string[] arrEmailddlval = emailddlval.Split(',');
                    //string[] arremailid = emailid.Split(',');
                    //insert into crm email
                   
               
                        //for (int k = 0; k < arrEmailval.Length; k++)
                        //{
                          
                        //        String stremail = "";
                        //        if (arremailid[k].ToString() != "")
                        //        {
                        //            dtmobid = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, "select * from CRM_ContactEmail where Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]) + " and  CEmail_Id=" + Convert.ToInt32(arremailid[k].ToString()));
                        //            if (dtmobid.Rows.Count == 0)
                        //            {

                        //                stremail = stremail + "INSERT INTO [dbo].[CRM_ContactEmail] " + "\n";
                        //                stremail = stremail + "           ([Contact_Id] " + "\n";
                        //                stremail = stremail + "           ,[Email] " + "\n";
                        //                stremail = stremail + "           ,[EmailType],ContactName) " + "\n";
                        //                stremail = stremail + "     VALUES " + "\n";
                        //                stremail = stremail + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                        //                stremail = stremail + "          ,'" + arrEmailval[k].ToString().Replace("'", "''") + "'\n";
                        //                stremail = stremail + "            ,'" + arrEmailddlval[k].ToString().Replace("'", "''") + "','" + EmailCtArr[k].ToString().Replace("'", "''") + "')";

                        //                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, stremail);
                        //            }
                        //            else
                        //            {
                        //                stremail = stremail + "Update [dbo].[CRM_ContactEmail] set " + "\n";
                        //                stremail = stremail + "           [Email]='" + arrEmailval[k].ToString().Replace("'", "''") + "'" + "\n";
                        //                stremail = stremail + "           ,[EmailType]='" + arrEmailddlval[k].ToString() + "' " + "\n";
                        //                stremail = stremail + "           ,[ContactName]='" + EmailCtArr[k].ToString().Replace("'", "''") + "' " + "\n";
                        //                stremail = stremail + "            where CEmail_Id=" + Convert.ToInt32(dtmobid.Rows[0]["CEmail_Id"].ToString()) + " and  Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]); ;
                        //                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, stremail);
                        //            }
                        //        }
                        //        else if (arrEmailval[k].ToString()=="" && EmailCtArr[k].ToString() =="")
                        //        {
                        //        }
                        //        else
                        //        {
                        //            stremail = stremail + "INSERT INTO [dbo].[CRM_ContactEmail] " + "\n";
                        //            stremail = stremail + "           ([Contact_Id] " + "\n";
                        //            stremail = stremail + "           ,[Email] " + "\n";
                        //            stremail = stremail + "           ,[EmailType],ContactName) " + "\n";
                        //            stremail = stremail + "     VALUES " + "\n";
                        //            stremail = stremail + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                        //            stremail = stremail + "          ,'" + arrEmailval[k].ToString().Replace("'", "''") + "'\n";
                        //            stremail = stremail + "            ,'" + arrEmailddlval[k].ToString().Replace("'", "''") + "','" + EmailCtArr[k].ToString().Replace("'", "''") + "')";

                        //            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, stremail);
                        //        }
                            
                        //}
                   
                    //insert into crm url
                    int z = 0;
                    string[] arrUrlval = urlval.Split(',');
                    string[] arrUrlddlval = urlddlval.Split(',');
                    string[] arrUrlid = urlid.Split(',');
                 
                        for (int k = 0; k < arrUrlval.Length; k++)
                        {
                           
                                String strurl = "";
                                if (arrUrlid[k].ToString() != "")
                                {

                                 
                                        strurl = strurl + "Update [dbo].[CRM_ContactURL] set" + "\n";

                                        strurl = strurl + "           [URL] ='" + arrUrlval[k].ToString().Replace("'", "''") + "'" + "\n";
                                        strurl = strurl + "           ,[URLType]='" + arrUrlddlval[k].ToString() + "' " + "\n";

                                        strurl = strurl + "            Where CUrl_Id=" + Convert.ToInt32(arrUrlid[k].ToString()) + " and  Contact_Id=" + Convert.ToInt32(ViewState["Contact_Id"]); ;
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strurl);
                                    
                                }
                               
                                else
                                {
                                    if (arrUrlval[k].ToString() != "")
                                    {
                                        strurl = strurl + "INSERT INTO [dbo].[CRM_ContactURL] " + "\n";
                                        strurl = strurl + "           ([Contact_Id] " + "\n";
                                        strurl = strurl + "           ,[URL] " + "\n";
                                        strurl = strurl + "           ,[URLType]) " + "\n";
                                        strurl = strurl + "     VALUES " + "\n";
                                        strurl = strurl + "           (" + Convert.ToInt32(ViewState["Contact_Id"]) + "\n";
                                        strurl = strurl + "          ,'" + arrUrlval[k].ToString().Replace("'", "''") + "'\n";
                                        strurl = strurl + "            ,'" + arrUrlddlval[k].ToString().Replace("'", "''") + "')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strurl);
                                    }
                                }

                            
                        }
                   
               
                    if (Request.QueryString["Contact_Id"] != null)
                    {
                        Session["Insertmsg"] = "Record Updated Successfully";
                        string s = Request.QueryString["Contact_Id"];
                        Response.Redirect("CRMTask.aspx?Contact_Id=" + s);
                        
                    }
                    else
                    {
                        ClearControls();
                        rpt.DataSource = null;
                        rpt.DataBind();
                     
                        Session["Msg"] = "Record Updated Successfully";
                       // Response.Redirect("CRMContact.aspx");
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        btnSave.Text = "Save";                       
                    }                  
                  
                }

                hidchk.Value = "";
            }
            catch (Exception ex) { ex.ToString(); System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while Updating the records');", true); }

        }

        protected void btncancel1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Contact_Id"] != null)
            {
                string s = Request.QueryString["Contact_Id"];
                Response.Redirect("CRMTask.aspx?Contact_Id=" + s);
               
            }
            else
            {
                
                Response.Redirect("CRMContact.aspx");
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillRepeter();
        }


        protected void btncancel_Click(object sender, EventArgs e)
        {
            string chdate = "";
            try
            {
                if (ViewState["contactID"].ToString() != "")
                {
                    if (txtnxtactndt.Text != "")
                    {
                        chdate = Convert.ToDateTime(txtnxtactndt.Text).ToShortDateString();
                    }
                    DateTime Currdt = Settings.GetUTCTime();
                    string docID = Settings.GetDocID("CRMTK", Currdt);
                    Settings.SetDocID("CRMT", docID);
                    String str = "";
                    str = str + "INSERT INTO [dbo].[CRM_Task] " + "\n";
                    str = str + "           ([DocId] " + "\n";
                    str = str + "           ,[AssignedTo] " + "\n";
                    str = str + "           ,[AssignedBy] " + "\n";
                    str = str + "           ,[Task] " + "\n";
                    str = str + "           ,[AssignDate] " + "\n";
                    str = str + "           ,[Ref_DocId] " + "\n";
                    str = str + "           ,[Ref_Sno] " + "\n";
                    str = str + "           ,[Status] " + "\n";
                    str = str + "           ,[CreatedBySmId] " + "\n";
                    str = str + "           ,[Contact_Id] " + "\n";
                    str = str + "           ,[CreatedDate]) " + "\n";
                    str = str + "     VALUES " + "\n";
                    str = str + "        ('" + docID + "',\n";
                    str = str + "          '" + Settings.Instance.SMID + "',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "             '" + ViewState["Fname"].ToString() + "',\n";
                    str = str + "             Getdate(),\n";
                    str = str + "             '',\n";
                    str = str + "          '0',\n";
                    str = str + "              'o',\n";
                    str = str + "            '" + Settings.Instance.SMID + "',\n";
                    str = str + "            " + Convert.ToInt32(ViewState["contactID"].ToString()) + ",\n";
                    str = str + "             Getdate())";


                    DAL.DbConnectionDAL.ExecuteQuery(str);

                    //string msgurl = ""; string displaytitle = "";
                    //msgurl = "CRMTask.aspx?DocId=" + docID + " ";
                    //string varname1 = "";
                    //string Assignedby = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //string Assignedto = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMName From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //int Assigntouserid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select UserId From MastSalesRep where SMId = " + Settings.Instance.SMID + " "));
                    //displaytitle = "Task By - " + Assignedby + " , To - " + Assignedto + "  " + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy"); 
                    //varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('CRMTASK', " + Assigntouserid + ", Getdate(),'" + msgurl + "', '" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + Settings.Instance.SMID + ")";

                    //DAL.DbConnectionDAL.ExecuteQuery(varname1);

                    if (Request.QueryString["Contact_Id"] != null)
                    {
                        //Session["Insertmsg"] = "Record Inserted Successfully";
                        //this.ModalPopupExtender2.Show();
                        string s = Request.QueryString["Contact_Id"];
                        Response.Redirect("CRMTask.aspx?Contact_Id=" + s);

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void txtcompstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtcompstate.SelectedIndex > 0)
            {
                string  strQ = " SELECT countryid  FROM ViewGeo WHERE stateid=" + txtcompstate.SelectedValue + " ";
                DataTable dtcountry = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dtcountry.Rows.Count > 0)
                {
                    ddlcompcountry.SelectedValue = dtcountry.Rows[0]["countryid"].ToString();
                }
            }
        }
        protected void State_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (State.SelectedIndex > 0)
            {
                string strQ = " SELECT countryid  FROM ViewGeo WHERE stateid=" + State.SelectedValue + " ";
                DataTable dtcountry = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dtcountry.Rows.Count > 0)
                {
                    ddlcountry.SelectedValue = dtcountry.Rows[0]["countryid"].ToString();
                }
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCity(ddlState.SelectedValue);
        }
    }
}