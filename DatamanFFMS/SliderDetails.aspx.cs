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
using System.Web.Services;
using System.Web.Script.Services;


namespace AstralFFMS
{
    public partial class SliderDetails : System.Web.UI.Page
    {
        LocationBAL LB = new LocationBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
           
            if (!string.IsNullOrEmpty(parameter))
            {
                ViewState["Id"] = parameter;
                FillLocControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                deleteDiv.Style.Add("display", "block");

                //imgpreview.Style.Add("display", "none");
            }
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            //if (btnSave.Text == "Save")
            //{
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //// btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            //btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                fillRepeter();
                PopulateState(0);
                ListBox1.Items.Clear();
               // BindDistributorDDl();
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                CalendarExtender1.StartDate = Settings.GetUTCTime();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                if (Request.QueryString["Id"] != null)
                {
                    FillLocControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
                else
                { //BindParent(0);
                }
            }
        }

        private void BindDistributorDDl()
        {
            string strQ = "", _search = ""; ;
            string strusername = DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT ml.Name FROM MastLogin ml LEFT JOIN mastsalesrep ms ON ml.Id=ms.UserId WHERE ms.SMId=" + Settings.Instance.SMID + "").ToString();
            if (strusername.ToLower() == "lakshya")
            {
                strQ = " Select SMID,SMName from [MastSalesRep] where SMName <>'.'  order by SMName ";
                _search = "And SMID IN (" + strQ + ")";
            }
            else
            {
                strQ = " select MSRG.maingrp As SMID from mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.maingrp  where MSRG.smid in (" + Settings.Instance.SMID + ") and MSR.SMName <>'.'  union ";
                strQ = strQ + " SELECT MSRG.smid  As SMID FROM mastsalesrepgrp  MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.smid  WHERE  MSRG.maingrp in (" + Settings.Instance.SMID + ")  and MSR.SMName <>'.' ";
                _search = "And SMID IN (" + strQ + ")";

            }
            string citystr = "";
            string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                citystr += dtCity.Rows[i]["AreaId"] + ",";
            }
            citystr = citystr.TrimStart(',').TrimEnd(',');

            string sql = "select partyid,partyname+'-'+mp.SyncId+'-'+vg.AreaName +'-'+ vg.CityName +'-'+ vg.StateName  as PartyName  from mastparty mp left join [dbo].[ViewGeo] vg on mp.AreaId =vg.areaId";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "partyid";
                        ListBox1.DataBind();
                   }
        }
        protected void lnkViewDemoImg_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    LinkButton btn = (LinkButton)sender;
            //    var item = (RepeaterItem)btn.NamingContainer;
            //    HiddenField hdnId = (HiddenField)item.FindControl("linkHiddenField");
            //    HiddenField sTypeHdf = (HiddenField)item.FindControl("sTypeHdf");
            //    if (sTypeHdf.Value == "Distributor Discussion" || sTypeHdf.Value == "Competitor" || sTypeHdf.Value == "Demo")
            //    {
            //        Response.ContentType = ContentType;
            //        if (hdnId.Value != "")
            //        {
            //            btn.Visible = true;
            //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
            //            Response.WriteFile(hdnId.Value);
            //            Response.End();
            //        }
            //        else
            //        {
            //            btn.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //        btn.Visible = false;
            //    }


            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
        }

        private void fillRepeter()
        {
            //Query Optimized - Abhishek - 28-05-2016
            //string str = @"select ma.*,case ma.Active when 1 then 'Yes' else 'No' end as Active1,ma1.AreaName as Parent from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID  where ma.AreaType='REGION'";
            string str = @"SELECT [ID],[ImagePath],[CreatedBySMID],[FromDate],[Todate],[UpdatedBySmid],[UpdatedDate],[IsDeleted],[DeletedBySMId],[DeletedDate],[DeletedRemarks],[Remarks] FROM [dbo].[SliderDetails] Where IsDeleted is null";
            DataTable Locdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Locdt;
            rpt.DataBind();
        }
        private void FillLocControls(int Id)
        {
            try
            {
                ListBox1.Items.Clear();
               // BindDistributorDDl();
                string MatGrpQry = @"SELECT [ID],[ImagePath],[CreatedBySMID],[FromDate],[Todate],[UpdatedBySmid],[UpdatedDate],[IsDeleted],[DeletedBySMId],[DeletedDate],[DeletedRemarks],[Remarks] FROM [dbo].[SliderDetails] where Id=" + Id;
                DataTable LocValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, MatGrpQry);
                if (LocValueDt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(LocValueDt.Rows[0]["ImagePath"].ToString()))
                    {
                        imgpreview.Src = LocValueDt.Rows[0]["ImagePath"].ToString();
                        imgpreview.Style.Add("display", "block");

                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    txtRemarks.InnerText = LocValueDt.Rows[0]["Remarks"].ToString().Replace("'","");
                    txtfmDate.Text = Convert.ToDateTime(LocValueDt.Rows[0]["FromDate"]).ToString("dd/MMM/yyyy");
                    txttodate.Text = Convert.ToDateTime(LocValueDt.Rows[0]["Todate"]).ToString("dd/MMM/yyyy");
                    txtDeleteRemarks.InnerText = LocValueDt.Rows[0]["DeletedRemarks"].ToString().Replace("'", "");
                    MatGrpQry = "Select DistId from DistributorSliderDetails Where SliderId=" + Id + "";
                    DataTable dtDist = DbConnectionDAL.getFromDataTable(MatGrpQry);

                    MatGrpQry = "Select CityId from MastParty where PartyId in(Select DistId from DistributorSliderDetails Where SliderId=" + Id + ")";
                    DataTable dtcity = DbConnectionDAL.getFromDataTable(MatGrpQry);

                    MatGrpQry = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' And AreaId IN( Select StateId from ViewGeo Where CityId IN(Select CityId from MastParty where PartyId in(Select DistId from DistributorSliderDetails Where SliderId=" + Id + ")))";
                    DataTable dtStateFill = DbConnectionDAL.getFromDataTable(MatGrpQry);

                    string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId in(Select StateId from  ViewGeo where CityId IN(Select CityId from MastParty where PartyId in(Select DistId from DistributorSliderDetails Where SliderId=" + Id + "))) and cityact=1 order by CityName";
                    DataTable dtState = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);

                    strQ = "select distinct partyid,partyname+'-'+mp.SyncId+'-'+maa.AreaName +'-'+ ma.AreaName  +'-'+ mas.AreaName   as PartyName  from mastparty mp inner join MastArea maa on mp.AreaId=maa.AreaId inner join MastArea ma on mp.CityId=ma.AreaId inner join MastArea mad on ma.UnderId=mad.AreaId inner join MastArea mas on mad.UnderId=mas.AreaId Where mp.CityId in(Select CityId from MastParty where PartyId in(Select DistId from DistributorSliderDetails Where SliderId=" + Id + ")) And mp.Active=1 and mp.PartyDist=1";
                    DataTable dtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);

                    PopulateState(0);
                    ListBox1.DataSource = dtDistributor;
                    ListBox1.DataTextField = "PartyName";
                    ListBox1.DataValueField = "partyid";
                    ListBox1.DataBind();

                    lstCity.DataSource = dtState;
                    lstCity.DataTextField = "CityName";
                    lstCity.DataValueField = "CityID";
                    lstCity.DataBind();
                  
                    foreach (DataRow dr in dtDist.Rows)
                    {
                       if (ListBox1.Items.FindByValue(Convert.ToString(dr["DistId"]))!=null) ListBox1.Items.FindByValue(Convert.ToString(dr["DistId"])).Selected=true;
                    }
                    foreach (DataRow dr in dtStateFill.Rows)
                    {
                        if (lstState.Items.FindByValue(Convert.ToString(dr["AreaID"])) != null) lstState.Items.FindByValue(Convert.ToString(dr["AreaID"])).Selected = true;
                    }
                    foreach (DataRow dr in dtcity.Rows)
                    {
                        if (lstCity.Items.FindByValue(Convert.ToString(dr["CityID"])) != null) lstCity.Items.FindByValue(Convert.ToString(dr["CityID"])).Selected = true;
                    }
                   
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
     
       public void PopulateState(int StateId)
        {
           
            string str = "";

            if (StateId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='State' and (Active='1' or Areaid in (" + StateId + "))  order by AreaName";
            else
                str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";

             DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
             lstState.DataSource = dt;
             lstState.DataTextField = "AreaName";
             lstState.DataValueField = "AreaID";
             lstState.DataBind();

        }
     
        private void InsertLoc()
        {
            string _sql = "";
            if (FileUpload1.PostedFile.FileName == "")
            {
                lstState.ClearSelection();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Image');", true);
                return;
            }
            string smIDStr1 = "", Qrychk = "", Query = "",_stateId="",_cityId="";
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            //foreach (ListItem item in lstState.Items)
            //{
            //    if (item.Selected)
            //    {
            //        _stateId += item.Value + ",";
            //    }
            //}
            //  _stateId = _stateId.TrimStart(',').TrimEnd(',');
            //  foreach (ListItem item in lstCity.Items)
            //  {
            //      if (item.Selected)
            //      {
            //          _cityId += item.Value + ",";
            //      }
            //  }
            //  _cityId = _cityId.TrimStart(',').TrimEnd(',');
              _sql = "Insert Into SliderDetails([ImagePath],[CreatedBySMID],[FromDate],[Todate],[Remarks]) values('-'," + Settings.Instance.SMID + ",'" + Settings.dateformat(txtfmDate.Text) + "','" + Settings.dateformat(txttodate.Text) + "','" + Convert.ToString(txtRemarks.InnerText).Replace("'", "") + "');SELECT SCOPE_IDENTITY();";
            string Id = DbConnectionDAL.GetStringScalarVal(_sql);
            if (FileUpload1.PostedFile != null)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string extension = Path.GetExtension(FileName);
                if (!string.IsNullOrEmpty(extension))
                {
                    string subPath = "SliderImages"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string filepath = "~/SliderImages/" + '-' + FileName;
                    FileInfo file1 = new FileInfo(filepath);
                    if (file1.Exists)// to check whether file exist or not ,if exist rename it
                    {
                        file1.Delete();
                    }
                    FileUpload1.SaveAs(Server.MapPath(filepath));
                    string qry = @"update SliderDetails set ImagePath='" + filepath + "' where ID=" + Id + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                }
            }
            string multiCharString = HiddenDistributor.Value;
            string[] multiArray = multiCharString.Split(',');
            
                foreach (string author in multiArray)
                {
                    if (!string.IsNullOrEmpty(author))
                    {
                        _sql = "Insert Into DistributorSliderDetails Values(" + author + "," + Convert.ToInt32(Id) + ")";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                    }
                }
           
            //foreach (ListItem item in ListBox1.Items)
            //{
            //    if (item.Selected)
            //    {
            //        _sql = "Insert Into DistributorSliderDetails Values(" + item.Value + "," + Id + ")";
            //        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
            //        //smIDStr1 += item.Value + ",";
            //    }
            //}
           
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
            ClearControls();
        }
        private void UpdateLoc()
        {
            try
            {
                string _sql = "";
                //if (FileUpload1.PostedFile.FileName == "")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Image');", true);
                //    return;
                //}
                _sql = "Update SliderDetails Set [UpdatedBySmid]=" + Settings.Instance.SMID + ",[FromDate]='" + Settings.dateformat(txtfmDate.Text) + "',[Todate]='" + Settings.dateformat(txttodate.Text) + "',[Remarks]='" +Convert.ToString(txtRemarks.InnerText).Replace("'","") + "',UpdatedDate=getdate() Where Id=" + Convert.ToInt32(ViewState["Id"]) + "";
                string Id = DbConnectionDAL.GetStringScalarVal(_sql);
                if (FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        string subPath = "SliderImages"; // your code goes here

                        bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                        string filepath = "~/SliderImages/" + '-' + FileName;
                        FileInfo file1 = new FileInfo(filepath);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload1.SaveAs(Server.MapPath(filepath));
                        string qry = @"update SliderDetails set ImagePath='" + filepath + "' where ID=" + Convert.ToInt32(ViewState["Id"]) + "";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                    }
                }

                _sql = "Delete from DistributorSliderDetails where SliderId=" + Convert.ToInt32(ViewState["Id"]) + "";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                string multiCharString = HiddenDistributor.Value;
                string[] multiArray = multiCharString.Split(',');
                
                    foreach (string author in multiArray)
                    {
                        if (!string.IsNullOrEmpty(author))
                        {
                            _sql = "Insert Into DistributorSliderDetails Values(" + author + "," + Convert.ToInt32(ViewState["Id"]) + ")";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        }
                    }
               
                //foreach (ListItem item in ListBox1.Items)
                //{
                    
                //   if (item.Selected)
                //    {
                      
                //        _sql = "Insert Into DistributorSliderDetails Values(" + item.Value + "," + Convert.ToInt32(ViewState["Id"]) + ")";
                //        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                      
                //        //smIDStr1 += item.Value + ",";
                //    }
                //}
             
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                deleteDiv.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage("+ex.Message.ToString()+");", true);
            }
          
        }
        private void ClearControls()
        {
           
            txtRemarks.InnerText = "";
            txtDeleteRemarks.InnerText = "";
            imgpreview.Src = "";
            imgpreview.Style.Add("display", "none");
            lstState.ClearSelection();
            lstCity.ClearSelection();
            ListBox1.ClearSelection();
            //ListBox1.Items.Clear();
          
            // ddlParentLoc.SelectedIndex = 0;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // BindParent(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string _sql = "";
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    if (string.IsNullOrEmpty(txtDeleteRemarks.InnerText)) { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter the delete remarks');", true); return; }
                    //_sql = "Delete from SliderDetails where Id=" + Convert.ToInt32(ViewState["Id"]) + "";
                    //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);

                    _sql = "Update SliderDetails Set DeletedBySMId=" + Settings.Instance.SMID + ",DeletedDate=getdate(),DeletedRemarks='" + Convert.ToString( txtDeleteRemarks.InnerText).Replace("'","") + "',IsDeleted='D' Where Id=" + Convert.ToInt32(ViewState["Id"]) + "  ";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    deleteDiv.Style.Add("display", "none");
                    fillRepeter();
                  
                    BindDistributorDDl();
                }
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(" + ex.Message.ToString() + ");", true);
            }
          
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/SliderDetails.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                //{
                //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                //    return;
                //}
                   
                if (btnSave.Text == "Update")
                {
                    UpdateLoc();
                }
                else
                {
                    InsertLoc();
                }
              
                fillRepeter();
                //BindDistributorDDl();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");


        }


    }
    
    
}