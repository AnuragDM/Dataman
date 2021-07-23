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
using Newtonsoft.Json;
using System.Web.Script.Services;

namespace AstralFFMS
{
    public partial class CityFareLimit : System.Web.UI.Page
    {
        CityTypeBAL CTB = new CityTypeBAL();
        string parameter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillTMControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 11/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            //if (btnSave.Text == "Save")
            //{

            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //else
            //{

            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    btnSave.CssClass = "btn btn-primary";
            //}
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                // btnDelete.Visible = false;
                BindCityType();
                mainDiv.Style.Add("display", "block");

                if (Request.QueryString["Id"] != null)
                {
                    FillTMControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
                else { BindDesignation(0); }
            }
        }

        #region Bind Dropdowns
        private void BindCityType()
        { //Ankita - 11/may/2016- (For Optimization)
            //string strQ = "select * from MastCityType order by Name";
            string strQ = "select ID,Name from MastCityType order by Name";
            fillDDLDirect(ddlcitytype, strQ, "ID", "Name", 1);
        }
        private void BindDesignation(int DesigId)
        {
            string str = "";
            if (DesigId > 0)
                str = "select DesId,DesName from MastDesignation where (active='1' or DesId in(" + DesigId + ")) order by Desname";
            else
                str = "select DesId,DesName from MastDesignation where Active='1' order by Desname";
            fillDDLDirect(ddldesignation, str, "DesId", "DesName", 1);
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
        private void fillRepeter()
        {
            string str = @"select MTE.Id,MTM.Name,MD.DesName,MTE.SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active1,MTE.Amount Fare from MastLocalFareLimt MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastCityType MTM on MTE.CityTypeId=MTM.Id order by MTE.Active";
            DataTable TravelModedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = TravelModedt;
            rpt.DataBind();
        }
        private void FillTMControls(int Id)
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
                //string Qry = @"select * from MastLocalConveyanceLimt where Id=" + Id;
                string Qry = @"select DesID,CityTypeId,Amount,SyncId,Active,Remarks from MastLocalFareLimt where Id=" + Id;
                DataTable TravelEldt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (TravelEldt.Rows.Count > 0)
                {
                    BindDesignation(Convert.ToInt32(TravelEldt.Rows[0]["DesID"]));
                    ddldesignation.SelectedValue = TravelEldt.Rows[0]["DesID"].ToString();
                    ddlcitytype.SelectedValue = TravelEldt.Rows[0]["CityTypeId"].ToString();
                    //Amount.Value = TravelEldt.Rows[0]["Amount"].ToString();
                    SyncId.Value = TravelEldt.Rows[0]["SyncId"].ToString();
                    Remarks.Value = TravelEldt.Rows[0]["Remarks"].ToString();
                    if (Convert.ToBoolean(TravelEldt.Rows[0]["Active"]) == true)
                    {
                        chkIsActive.Checked = true;
                    }
                    else
                    {
                        chkIsActive.Checked = false;
                    }

                    //btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        public class GetFareConveyance
        {
            public string Id { get; set; }
            public string CityTypeId { get; set; }
            public string CityType { get; set; }
            public string DesId { get; set; }
            public string Designation { get; set; }
            public string Remarks { get; set; }
            public string SyncId { get; set; }
            public string Active { get; set; }
            public string CreatedDate { get; set; }
            public string Fare { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string getEditdata(string Id)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                condition = " where MTE.Id= " + Convert.ToInt32(Id) + " ";
            }

            string Qry = @"select MTE.Id as Id,MTE.CityTypeId as CityTypeId,MTE.DesId as DesId,MTM.Name as CityType,MD.DesName as Designation,MTE.SyncId as SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active,MTE.CreatedDate as CreatedDate,MTE.Remarks as Remarks,MTE.Amount as Fare  from MastLocalFareLimt MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastCityType MTM on MTE.CityTypeId=MTM.Id " + condition + " order by MTE.Active";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            List<GetTravelConveyance> rst = new List<GetTravelConveyance>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Qry = @"select Id,TravelId,Distance,Amount from MastLocalFareLimtDetails where TravelId= " + Convert.ToInt32(dt.Rows[i]["Id"].ToString()) + "";

                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                    List<GetDistanceDetail> rst1 = new List<GetDistanceDetail>();
                    if (dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            rst1.Add(

                            new GetDistanceDetail
                            {

                                TId = dt1.Rows[j]["Id"].ToString(),
                                TravelId = dt1.Rows[j]["TravelId"].ToString(),
                                Distance = dt1.Rows[j]["Distance"].ToString(),
                                Amount = dt1.Rows[j]["Amount"].ToString(),
                            }
                        );
                        }
                    }

                    rst.Add(

                            new GetTravelConveyance
                            {

                                Id = dt.Rows[i]["Id"].ToString(),
                                CityTypeId = dt.Rows[i]["CityTypeId"].ToString(),
                                CityType = dt.Rows[i]["CityType"].ToString(),
                                DesId = dt.Rows[i]["DesId"].ToString(),
                                Designation = dt.Rows[i]["Designation"].ToString(),
                                Remarks = dt.Rows[i]["Remarks"].ToString(),
                                SyncId = dt.Rows[i]["SyncId"].ToString(),
                                Active = dt.Rows[i]["Active"].ToString(),
                                CreatedDate = dt.Rows[i]["CreatedDate"].ToString(),
                                //NighthaltAmt = dt.Rows[i]["NighthaltAmt"].ToString(),
                                TravelDistanceConveyance = rst1

                            }
                        );
                }



            }
            return JsonConvert.SerializeObject(rst);
        }

        public class GetDistanceDetail
        {
            public string TId { get; set; }
            public string TravelId { get; set; }
            public string Distance { get; set; }
            public string Amount { get; set; }

        }

        public class Result
        {

            public string Msg { get; set; }

        }

        [WebMethod(EnableSession = true)]
        public static string save_data(List<GetDistanceDetail> DistanceDetails, string Id, string CityType, string Designation, string Remark, string SyncId, string Active)
        {
            string PrevDist = "0";
            Result res = new Result();
            int retval = 0;
            int retval1 = 0;

            foreach (var item in DistanceDetails)
            {
                string Distance = item.Distance;
                if(Convert.ToDecimal(PrevDist)>Convert.ToDecimal(Distance))
                {
                    retval1 = 1;
                    break;
                }
                else
                {
                    PrevDist = Distance;
                }
            }

            if (retval1 == 0)
            {


                if (!string.IsNullOrEmpty(Id))
                {

                    if (Active == "true")
                        Active = "1";
                    else
                        Active = "0";


                    string str = "select 1 from MastLocalFareLimt where CityTypeId=" + Convert.ToInt32(CityType) + " and DesId=" + Convert.ToInt32(Designation) + " and Id <> " + Convert.ToInt32(Id) + "";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    str = "SELECT 1 FROM MastLocalFareLimt WHERE SyncId='" + SyncId + "' and Id <> " + Convert.ToInt32(Id) + "";
                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dt.Rows.Count > 0)
                    {
                        retval = -1;
                    }
                    else if ((dt1.Rows.Count > 0) && !string.IsNullOrEmpty(SyncId))
                    {
                        retval = -2;
                    }
                    else
                    {
                        string Query1 = "update MastLocalFareLimt set CityTypeId=" + Convert.ToInt32(CityType) + ",DesId=" + Convert.ToInt32(Designation) + ",Remarks='" + Remark.ToUpper() + "',SyncId='" + SyncId + "',Amount=0,Active='" + Convert.ToInt32(Active) + "',CreatedDate=GETDATE() where id=" + Convert.ToInt32(Id) + "";
                        DbConnectionDAL.ExecuteQuery(Query1);

                        Query1 = "delete from MastLocalFareLimtDetails where TravelId=" + Convert.ToInt32(Id) + " ";
                        DbConnectionDAL.ExecuteQuery(Query1);

                        foreach (var item in DistanceDetails)
                        {
                            string Distance = item.Distance;
                            string Amount = item.Amount;
                            str = "INSERT INTO MastLocalFareLimtDetails ([TravelId],[Amount],[Distance]) VALUES (" + Convert.ToInt32(Id) + "," + Convert.ToDecimal(Amount) + "," + Convert.ToDecimal(Distance) + ")";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                        }
                    }

                    if (retval == -1)
                    {
                        res.Msg = "Duplicate Entry Exists.";
                    }
                    else if (retval == -2)
                    {
                        res.Msg = "Duplicate SyncId Exists.";
                    }
                    else
                    {
                        res.Msg = "Record Updated Successfully.";
                    }
                }
                else
                {
                    if (Active == "true")
                        Active = "1";
                    else
                        Active = "0";

                    string str = "select 1 from MastLocalFareLimt where CityTypeId=" + Convert.ToInt32(CityType) + " and DesId=" + Convert.ToInt32(Designation) + "";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    str = "SELECT 1 FROM MastLocalFareLimt WHERE SyncId='" + SyncId + "'";
                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dt.Rows.Count > 0)
                    {
                        retval = -1;
                    }
                    else if ((dt1.Rows.Count > 0) && !string.IsNullOrEmpty(SyncId))
                    {
                        retval = -2;
                    }
                    else
                    {
                        str = "insert into MastLocalFareLimt(CityTypeId,DesId,Remarks,SyncId,Active,Amount,CreatedDate)OUTPUT INSERTED.Id values(" + Convert.ToInt32(CityType) + "," + Convert.ToInt32(Designation) + ",'" + Remark.ToUpper() + "','" + SyncId + "'," + Convert.ToInt32(Active) + ",0,GETDATE())";
                        string Inserted_Id = DbConnectionDAL.GetStringScalarVal(str);

                        foreach (var item in DistanceDetails)
                        {
                            string Distance = item.Distance;
                            string Amount = item.Amount;
                            str = "INSERT INTO MastLocalFareLimtDetails ([TravelId],[Amount],[Distance]) VALUES (" + Convert.ToInt32(Inserted_Id) + "," + Convert.ToDecimal(Amount) + "," + Convert.ToDecimal(Distance) + ")";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                        }
                    }
                    if (retval == -1)
                    {
                        res.Msg = "Duplicate Entry Exists.";
                    }
                    else if (retval == -2)
                    {
                        res.Msg = "Duplicate SyncId Exists.";
                    }
                    else
                    {
                        res.Msg = "Record Inserted Successfully.";
                    }
                }
            }
            else
            {
                res.Msg = "Distance should be in Increment Order";
            }
            return JsonConvert.SerializeObject(res);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            BindDesignation(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string id = HiddenTravelId.Value;

                if (!string.IsNullOrEmpty(id))
                {
                    string Query1 = "delete from MastLocalFareLimt where Id=" + Convert.ToInt32(id) + " ";
                    DbConnectionDAL.ExecuteQuery(Query1);

                    Query1 = "delete from MastLocalFareLimtDetails where TravelId=" + Convert.ToInt32(id) + " ";
                    DbConnectionDAL.ExecuteQuery(Query1);

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    fillRepeter();
                    ClearControls();
                    ddlcitytype.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Data to Delete.');", true);
                }
            }
        }

        private void ClearControls()
        {
            HiddenTravelId.Value = "";
            ddlcitytype.SelectedIndex = 0;
            ddldesignation.SelectedIndex = 0;
            SyncId.Value = "";
            Remarks.Value = "";
            chkIsActive.Checked = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/CityFareLimit.aspx");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        public class GetTravelConveyance
        {
            public string Id { get; set; }
            public string CityTypeId { get; set; }
            public string CityType { get; set; }
            public string DesId { get; set; }
            public string Designation { get; set; }
            public string Remarks { get; set; }
            public string SyncId { get; set; }
            public string Active { get; set; }
            public string CreatedDate { get; set; }
            public List<GetDistanceDetail> TravelDistanceConveyance { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string get_data(string Id)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                condition = " where MTE.Id= " + Convert.ToInt32(Id) + " ";
            }


            string Qry = @"select Id,TravelId,Distance,Amount from MastLocalFareLimtDetails where TravelId= " + Convert.ToInt32(Id) + "";

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            List<GetDistanceDetail> rst1 = new List<GetDistanceDetail>();
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    rst1.Add(
                        new GetDistanceDetail
                        {
                            TId = dt1.Rows[j]["Id"].ToString(),
                            TravelId = dt1.Rows[j]["TravelId"].ToString(),
                            Distance = dt1.Rows[j]["Distance"].ToString(),
                            Amount = dt1.Rows[j]["Amount"].ToString(),
                        }
                    );
                }
            }
            return JsonConvert.SerializeObject(rst1);
        }

        [WebMethod(EnableSession = true)]
        public static string getDtqdata(string Id, string Distance, string Amount)
        {
            string condition = "";

            string Query1 = "delete from MastLocalFareLimtDetails where TravelId=" + Convert.ToInt32(Id) + " and Distance=" + Convert.ToDecimal(Distance) + " and Amount=" + Convert.ToDecimal(Amount) + "";
            DbConnectionDAL.ExecuteQuery(Query1);
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                condition = " where MTE.Id= " + Convert.ToInt32(Id) + " ";
            }

            //string Qry = @"select MTE.Id,MTE.CityTypeId,MTE.DesId,MTM.Name as CityType,MD.DesName as Designation,MTE.SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active1,MTE.CreatedDate,MTE.Remarks  from MastTravelConveyanceLimt MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastCityType MTM on MTE.CityTypeId=MTM.Id " + condition + " order by MTE.Active";

            string Qry = @"select MTE.Id,MTE.CityTypeId,MTE.DesId,MTM.Name as CityType,MD.DesName as Designation,MTE.SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active1,MTE.Amount Fare,MTE.CreatedDate,MTE.Remarks from MastLocalFareLimt MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastCityType MTM on MTE.CityTypeId=MTM.Id " + condition + " order by MTE.Active";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            List<GetTravelConveyance> rst = new List<GetTravelConveyance>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Qry = @"select Id,TravelId,Distance,Amount from MastLocalFareLimtDetails where TravelId= " + Convert.ToInt32(dt.Rows[i]["Id"].ToString()) + "";

                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                    List<GetDistanceDetail> rst1 = new List<GetDistanceDetail>();
                    if (dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            rst1.Add(

                            new GetDistanceDetail
                            {

                                TId = dt1.Rows[j]["Id"].ToString(),
                                TravelId = dt1.Rows[j]["TravelId"].ToString(),
                                Distance = dt1.Rows[j]["Distance"].ToString(),
                                Amount = dt1.Rows[j]["Amount"].ToString(),
                            }
                        );
                        }
                    }

                    rst.Add(

                            new GetTravelConveyance
                            {

                                Id = dt.Rows[i]["Id"].ToString(),
                                CityTypeId = dt.Rows[i]["CityTypeId"].ToString(),
                                CityType = dt.Rows[i]["CityType"].ToString(),
                                DesId = dt.Rows[i]["DesId"].ToString(),
                                Designation = dt.Rows[i]["Designation"].ToString(),
                                Remarks = dt.Rows[i]["Remarks"].ToString(),
                                SyncId = dt.Rows[i]["SyncId"].ToString(),
                                Active = dt.Rows[i]["Active1"].ToString(),
                                CreatedDate = dt.Rows[i]["CreatedDate"].ToString(),

                                TravelDistanceConveyance = rst1

                            }
                        );
                }



            }
            return JsonConvert.SerializeObject(rst);

        }
    }
}