using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using System.Net;
using System.Web.Script.Serialization;

using System.Net.Mail;


namespace AstralFFMS
{
    public partial class ProspectDistributorApprovalForm : System.Web.UI.Page
    {

        string qry = "";

        BAL.LeaveRequest.LeaveAll lvAll = new BAL.LeaveRequest.LeaveAll();

        protected void Page_Load(object sender, EventArgs e)
        {

            lblPageHeader.Text = "Prospect Distributor Approval Form";

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");

            if (!IsPostBack)
            {
                string pageName = Path.GetFileName(Request.Path);
                string Pageheader = Settings.Instance.GetPageHeaderName(pageName);

                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                distitemsalerpt.DataSource = distributors;
                distitemsalerpt.DataBind();



                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");

                string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');
                btnExport.CssClass = "btn btn-primary";
                btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
                btnApprove.Visible = Convert.ToBoolean(SplitPerm[1]);
                btnReject.Visible = Convert.ToBoolean(SplitPerm[1]);
                btnExport.Visible = true;
                btnExport.CssClass = "btn btn-primary";
                bindState();
                BindSalesPerson();
            }


            if (!string.IsNullOrEmpty(Request.QueryString["partyid"]))
            {
                DataTable dt1;
                string party = Request.QueryString["partyid"].ToString();
                qry = "select ROW_NUMBER() over(order by pd.partyname) as sno,pd.partyid,pd.PartyName,pd.Mobile,(pd.Address1+ ' - ' + pd.Address2) as addres,pd.GSTIN,sp.SMName as createdBy,cast(pd.Created_Date as date) as Created_Date,case when  ISNULL(pd.Approved_Rejected,'')='A' then 'A' when  ISNULL(pd.Approved_Rejected,'')='R' then 'R' else '0' end as status,sp.smid,pd.cityid from MastProspect_Distributor pd left join MastSalesRep sp on pd.[Created Userid]=sp.UserId where pd.partyid=" + party;
                dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                if (dt1.Rows.Count > 0)
                {
                    lstSalesP.SelectedValue = dt1.Rows[0]["smid"].ToString();
                    txtfmDate.Text = Convert.ToDateTime(dt1.Rows[0]["Created_Date"]).ToString("dd/MMM/yyyy");
                    txttodate.Text = Convert.ToDateTime(dt1.Rows[0]["Created_Date"]).ToString("dd/MMM/yyyy");
                    ddlStatus.SelectedValue = dt1.Rows[0]["status"].ToString();
                    qry = "select UnderId from mastarea where areatype='DISTRICT' and areaid in (select underid from mastarea where areatype='CITY' and areaid=" + dt1.Rows[0]["cityid"].ToString() + ") ";
                    string stid = DbConnectionDAL.GetStringScalarVal(qry);
                    lstState.SelectedValue = stid;
                    bindCity(stid);
                    lstCity.SelectedValue = dt1.Rows[0]["cityid"].ToString();
                }

            }
        }




        public class Distributors
        {
            public string VDate { get; set; }
            public string Distributor { get; set; }
            public string MaterialGROUP { get; set; }
            public string Item { get; set; }
            public string Qty { get; set; }
            public string rate { get; set; }
            public string Amount { get; set; }
        }


        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProspectDistributorApprovalForm.aspx");
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            distitemsalerpt.DataSource = null;
            distitemsalerpt.DataBind();
            btnExport.Visible = false;
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "", whr = "", smid = "", stateid = "", cityid = "";

                foreach (ListItem item in lstSalesP.Items)
                {
                    if (item.Selected) smid += item.Value + ",";
                }

                if (string.IsNullOrEmpty(smid.TrimEnd(',')) && smid.TrimEnd(',') == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select SalesPerson');", true);
                    return;
                }
                foreach (ListItem item in lstState.Items)
                {
                    if (item.Selected) stateid += item.Value + ",";
                }
                foreach (ListItem item in lstCity.Items)
                {
                    if (item.Selected) cityid += item.Value + ",";
                }

                if (!string.IsNullOrEmpty(stateid) && stateid.Trim() != "null") whr = "  and mpd.CityId in (select areaid from MastArea where AreaType='CITY' AND UnderId IN (select areaid from MastArea where AreaType='DISTRICT' AND UnderId IN (" + stateid.TrimEnd(',') + ")))";

                if (!string.IsNullOrEmpty(cityid) && cityid != "null") whr = " and mpd.cityid in (" + cityid.TrimEnd(',') + ")";

                if (ddlStatus.SelectedValue != "0") whr += "and isnull(mpd.Approved_Rejected,'')='" + ddlStatus.SelectedValue + "'";


                str = "select mpd.[PartyName],(mpd.[Address1] + ' - ' +  mpd.[Address2] + ' - ' + cast (mpd.[Pin] as varchar)) as addres ,mct.AreaName,mpd.[Email],mpd.[Mobile],mpd.[Phone No Office] as officePhone,mpd.[Phone No Residence] as ResidencePhone,mpd.[Business Nature] as bNature,mpd.[Partner_Propector_Director Name] as dirName,mpd.[Storage Facility] as storageFacility,mpd.[Storage Facility Square fit] as storageFacilitySqrFit,mpd.[No Of Emp_SalesPerson] as empSales,mpd.[No Of Emp_Others] as empOther,mpd.[Investment Proposed] as invPurpose,mpd.[Monthly Exp_turnover] as mnthlyTurnOver,mpd.[Dist_underYou] as distU,mpd.[Retailer_UnderYou] as retialerU,mpd.[No Of System_Computing] as systemComputing,mpd.[Avail trans for delivery] as tranForDelivery,mpd.[NewsPaper Published] as newsPaperPublished,mpd.[Festival_fairs In Your area_Timings] as festTime,mpd.[GSTIN],mpd.[PAN NO] as panno,mpd.[ADHAAR NO] as adhaar,mpd.[FSSAI Lisence No] as FSSAINo,mpd.[Bank Name] as Bank,mpd.[Branch Name] as Branch,mpd.[ACCNO],mpd.[Other Branch Name] as otherBranch,mpd.[Remark],case when  ISNULL(mpd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(mpd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as status,sp1.smname as [Approved_RejectedBy],cast(mpd.[Approved_RejectedDate] as date) as Approved_RejectedDate ,case when  ISNULL(mpd.Approved_Rejected,'')='' then '' else mpd.Approved_RejectedRemark end as ARremk,mpd.[contactPersonName],sp.smname as createdBy,mpd.Created_Date from MastProspect_Distributor mpd left join MastSalesRep sp on mpd.[Created Userid]=sp.UserId  left join MastArea mct on mpd.CityId=mct.AreaId left join mastsalesrep sp1 on sp1.smid=mpd.Approved_RejectedBySMID where  mct.AreaType='CITY' and sp.smid in (" + smid.TrimEnd(',') + ")  and mpd.Created_Date>='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + " 00:00' and mpd.Created_Date<='" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + " 23:59 ' " + whr;
                DataTable dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtParams.Rows.Count > 0)
                {
                    string headertext = "Prospect Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Email".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Phone No Office".TrimStart('"').TrimEnd('"') + "," + "Phone No Residence".TrimStart('"').TrimEnd('"') + "," + "Business Nature".TrimStart('"').TrimEnd('"') + "," + "Partner Propector Director Name".TrimStart('"').TrimEnd('"') + "," + "Storage Facility".TrimStart('"').TrimEnd('"') + "," + "Storage Facility Square fit".TrimStart('"').TrimEnd('"') + "," + "No Of Employee SalesPerson".TrimStart('"').TrimEnd('"') + "," + "No Of Employee Others".TrimStart('"').TrimEnd('"') + "," + "Investment Proposed".TrimStart('"').TrimEnd('"') + "," + "Monthly Exppected turnover".TrimStart('"').TrimEnd('"') + "," + "Distributor Under You".TrimStart('"').TrimEnd('"') + "," + "Retailer Under You".TrimStart('"').TrimEnd('"') + "," + "No Of System Computing".TrimStart('"').TrimEnd('"') + "," + "Available Transport For Delivery".TrimStart('"').TrimEnd('"') + "," + "NewsPaper Published".TrimStart('"').TrimEnd('"') + "," + "Festival/fairs In Your area_Timings".TrimStart('"').TrimEnd('"') + "," + "GSTIN".TrimStart('"').TrimEnd('"') + "," + "PAN NO".TrimStart('"').TrimEnd('"') + "," + "ADHAAR NO".TrimStart('"').TrimEnd('"') + "," + "FSSAI Lisence No".TrimStart('"').TrimEnd('"') + "," + "Bank Name".TrimStart('"').TrimEnd('"') + "," + "Branch Name".TrimStart('"').TrimEnd('"') + "," + "Account No".TrimStart('"').TrimEnd('"') + "," + "Other Branch Name".TrimStart('"').TrimEnd('"') + "," + "Remark".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "Approved/Rejected By".TrimStart('"').TrimEnd('"') + "," + "Approval/Rejection Date".TrimStart('"').TrimEnd('"') + "," + "Approval/Rejection Remark".TrimStart('"').TrimEnd('"') + "," + "ContactPerson Name".TrimStart('"').TrimEnd('"') + "," + "CreatedBy".TrimStart('"').TrimEnd('"') + "," + "Created Date".TrimStart('"').TrimEnd('"');

                    StringBuilder sb = new StringBuilder();
                    sb.Append(headertext);
                    sb.AppendLine(System.Environment.NewLine);
                    for (int j = 0; j < dtParams.Rows.Count; j++)
                    {
                        for (int k = 0; k < dtParams.Columns.Count; k++)
                        {
                            if (dtParams.Rows[j][k].ToString().Contains(","))
                            {
                                if (k == 32 || k == 36)
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
                                if (k == 32 || k == 36)
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
                                if (k == 32 || k == 36)
                                {
                                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                    Response.AddHeader("content-disposition", "attachment; filename=ProspectDistributorDetails.csv");
                    Response.Write(sb.ToString());
                    Response.End();

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('No Record Found');", true);
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }

        }


        private void bindState()
        {
            qry = "select areaid,areaname from mastarea where areatype='STATE' order by areaname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            lstState.DataSource = dt;
            lstState.DataValueField = "areaid";
            lstState.DataTextField = "areaname";
            lstState.DataBind();
        }
        private void bindCity(string stateid)
        {
            qry = "select areaid,areaname from mastarea where areatype='CITY' and underid in (select areaid from mastarea where areatype='DISTRICT' and underid in (" + stateid + ") ) order by areaname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            lstCity.DataSource = dt;
            lstCity.DataValueField = "areaid";
            lstCity.DataTextField = "areaname";
            lstCity.DataBind();

        }


        protected void lstState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stateid = "";

            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    stateid += item.Value + ",";
                }
            }
            stateid = stateid.TrimStart(',').TrimEnd(',');

            if (stateid != "")
            {
                bindCity(stateid);
            }
            else
            {
                lstCity.Items.Clear();
                lstCity.DataBind();

            }

        }





        private void BindSalesPerson()
        {
            string str = "select (sp.SMName + ' - ' + desg.DesName + ' - ' +  sp.Mobile) as smname,sp.smid from mastsalesrep as sp left join MastLogin lg on sp.UserId=lg.id left join MastDesignation desg on lg.DesigId=desg.desid where SMName<>'.' and ISNULL(sp.active,0)=1 order by smname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                lstSalesP.DataSource = dt;
                lstSalesP.DataValueField = "smid";
                lstSalesP.DataTextField = "smname";
                lstSalesP.DataBind();
            }
            else
            {
                lstSalesP.DataSource = null;
                lstSalesP.DataBind();

            }

        }


        [WebMethod(EnableSession = true)]
        public static string bindgrid(string smid, string stateid, string cityid, string Fromdate, string Todate, string status)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "", whr = "";

                if (!string.IsNullOrEmpty(stateid) && stateid.Trim() != "null")
                {
                    whr = "  and pd.CityId in (select areaid from MastArea where AreaType='CITY' AND UnderId IN (select areaid from MastArea where AreaType='DISTRICT' AND UnderId IN (" + stateid + ")))";
                }

                if (!string.IsNullOrEmpty(cityid) && cityid != "null")
                {
                    whr = " and pd.cityid in (" + cityid + ")";
                }
                if (status != "0")
                {
                    whr += "and isnull(pd.Approved_Rejected,'')='" + status + "'";
                }

                str = "select ROW_NUMBER() over(order by pd.partyname) as sno,pd.partyid,pd.PartyName,pd.Mobile,(pd.Address1+ ' - ' + pd.Address2 + ' - ' + cast(pd.pin as varchar)) as addres,pd.GSTIN,sp.SMName as createdBy,cast(pd.Created_Date as date) as Created_Date,case when  ISNULL(pd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(pd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as status from MastProspect_Distributor pd left join MastSalesRep sp on pd.[Created UserId]=sp.UserId where sp.smid in (" + smid + ")  and replace(Convert(varchar,pd.created_date,106),' ','/')>='" + Convert.ToDateTime(Fromdate).ToString("dd/MMM/yyyy") + "' and replace(Convert(varchar,pd.created_date,106),' ','/')<='" + Convert.ToDateTime(Todate).ToString("dd/MMM/yyyy") + "' " + whr;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return JsonConvert.SerializeObject(dt);

        }

        [WebMethod(EnableSession = true)]
        public static string bindgridbyPartyid(string partyid)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "";
                str = "select ROW_NUMBER() over(order by pd.partyname) as sno,pd.partyid,pd.PartyName,pd.Mobile,(pd.Address1+ ' - ' + pd.Address2 + ' - ' + cast(pd.pin as varchar)) as addres,pd.GSTIN,sp.SMName as createdBy,cast(pd.Created_Date as date) as Created_Date,case when  ISNULL(pd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(pd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as status from MastProspect_Distributor pd left join MastSalesRep sp on pd.[Created Userid]=sp.UserId where pd.partyid=" + partyid;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return JsonConvert.SerializeObject(dt);

        }


        [WebMethod(EnableSession = true)]
        public static string bindApprovalGrid(string partyid)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                str = "select mpd.[PartyId],mpd.[PartyName],(mpd.[Address1] + ' - ' +  mpd.[Address2] + ',Pin -' + cast (mpd.[Pin] as varchar)) as addres ,mct.AreaName,mpd.[Email],mpd.[Mobile],mpd.[Phone No Office] as officePhone,mpd.[Phone No Residence] as ResidencePhone,mpd.[Business Nature] as bNature,mpd.[Partner_Propector_Director Name] as dirName,mpd.[Storage Facility] as storageFacility,mpd.[Storage Facility Square fit] as storageFacilitySqrFit,mpd.[No Of Emp_SalesPerson] as empSales,mpd.[No Of Emp_Others] as empOther,mpd.[Investment Proposed] as invPurpose,mpd.[Monthly Exp_turnover] as mnthlyTurnOver,mpd.[Dist_underYou] as distU,mpd.[Retailer_UnderYou] as retialerU,mpd.[No Of System_Computing] as systemComputing,mpd.[Avail trans for delivery] as tranForDelivery,mpd.[NewsPaper Published] as newsPaperPublished,mpd.[Festival_fairs In Your area_Timings] as festTime,mpd.[GSTIN],mpd.[PAN NO] as panno,mpd.[ADHAAR NO] as adhaar,mpd.[FSSAI Lisence No] as FSSAINo,mpd.[Bank Name] as Bank,mpd.[Branch Name] as Branch,mpd.[ACCNO],mpd.[Other Branch Name] as otherBranch,mpd.[Remark],case when  ISNULL(mpd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(mpd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as status,mspAR.smname as [Approved_RejectedBy],cast(mpd.[Approved_RejectedDate] as date) as Approved_RejectedDate ,case when  ISNULL(mpd.Approved_Rejected,'')='' then '' else mpd.Approved_RejectedRemark end as ARremk,mpd.[contactPersonName],sp.smname as createdBy,mpd.Created_Date,case  when mpd.[GSTIN DOC] is not null then 'http://' + (select CompUrl from MastEnviro)+REPLACE(isnull(mpd.[GSTIN DOC],''),'~','')  else '' end  as GSTINDOC,case  when mpd.[PAN DOC] is not null then 'http://' + (select CompUrl from MastEnviro)+REPLACE(isnull(mpd.[PAN DOC],''),'~','')  else '' end  as PANDOC,case  when mpd.[ADHAAR DOC]  is not null then 'http://' + (select CompUrl from MastEnviro)+REPLACE(isnull(mpd.[ADHAAR DOC] ,''),'~','')  else '' end  as ADHAARDOC,case  when mpd.[FSSAI Lisence DOC]  is not null then 'http://' + (select CompUrl from MastEnviro)+REPLACE(isnull(mpd.[FSSAI Lisence DOC],''),'~','')  else '' end  as FSSAILisenceDOC ,case  when mpd.[Bank DOC]  is not null then 'http://' + (select CompUrl from MastEnviro)+REPLACE(isnull(mpd.[Bank DOC],''),'~','')  else '' end  as BankDOC  from MastProspect_Distributor mpd left join MastSalesRep sp on mpd.[Created Userid]=sp.UserId  left join MastArea mct on mpd.CityId=mct.AreaId  left join mastsalesrep mspAR on mpd.[Approved_RejectedBySMID]=mspAR.smid where  mct.AreaType='CITY'  and mpd.PartyId=" + partyid;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string approveRejectDist(string partyid, string ARremk, string AR)
        {
            DataTable dt = new DataTable();
            string str = "", retval = "", pro_id = "", msgurl = "", _msg = "", PartyName = "", username = "", userSmid = "", underid = "", displaytitle = "", createdBysmid = "", retResponse = "", CityId = "", distArea = ""; bool resposneSucceed = true;
            string ToMail = "", CcMail = ""; int roleId = 0;
            SqlConnection con = new SqlConnection(DbConnectionDAL.sqlConnectionstring);
            SqlTransaction tran = null;

            try
            {
                str = "select smid,smname from mastsalesrep where userid=" + Settings.Instance.UserID;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    userSmid = dt.Rows[0]["smid"].ToString();
                    username = dt.Rows[0]["smname"].ToString();
                }
                str = "select mpd.*,msp.smid from MastProspect_Distributor mpd left join mastsalesrep msp on mpd.[created Userid]=msp.userid  where partyid=" + partyid;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    PartyName = dt.Rows[0]["partyname"].ToString();
                    distArea = PartyName;
                    createdBysmid = dt.Rows[0]["created Userid"].ToString();
                    CityId = dt.Rows[0]["cityid"].ToString();
                }

                if (AR=="A")
                {
                    DataTable gstin = DbConnectionDAL.GetDataTable(CommandType.Text, "select GSTINNo from mastparty where isnull(active,0)=1 and isnull(partydist,0)=1 and GSTINNo='" + dt.Rows[0]["gstin"].ToString() + "'");
                    if (gstin.Rows.Count > 0)
                    {
                        retResponse = "Prospect Distributor can not be approved due to GSTIN- " + dt.Rows[0]["gstin"].ToString() + " is already exist.";
                        return JsonConvert.SerializeObject(retResponse);
                    }

                    DataTable mobile = DbConnectionDAL.GetDataTable(CommandType.Text, "select mobile from mastparty where isnull(active,0)=1 and isnull(partydist,0)=1 and mobile='" + dt.Rows[0]["mobile"].ToString() + "'");
                    if (mobile.Rows.Count > 0)
                    {
                        retResponse = "Prospect Distributor can not be approved due to  Mobile- " + dt.Rows[0]["mobile"].ToString() + " is already exist.";
                        return JsonConvert.SerializeObject(retResponse);
                    }

                     roleId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleId from mastrole where roletype='Distributor' AND ROLENAME='DISTRIBUTOR'"));
                   
                }
               

                try
                {
                    con.Open();
                    tran = con.BeginTransaction();
                    if (AR == "A")
                    {
                        InsertAreaAndDistributor(partyid, ref retResponse, ref resposneSucceed, dt, roleId, con, ref tran);
                    }

                    if (resposneSucceed)
                    {
                        str = "update MastProspect_Distributor set Approved_Rejected='" + AR + "',Approved_RejectedBySMID=" + userSmid + ",Approved_RejectedDate=GETDATE(),Approved_RejectedRemark='" + ARremk + "'  output inserted.partyid where PartyId=" + partyid;
                       int partyretVal= SqlHelper.ExecuteQuery(CommandType.Text, str, ref tran, con, (SqlParameter)null);
                      if (partyretVal>0)
                        {
                            tran.Commit();
                            tran = null;
                            con.Close();
                        }
                        else
                      {
                          if (tran != null)
                          {
                              tran.Rollback();
                              con.Close();
                              tran = null;
                          }
                          resposneSucceed = false;
                      }
                    }
                }
                catch (Exception ex)
                {
                    retResponse = "Error while inserting Area/Distributor ,Prospect Distributor can not be approved.";
                    resposneSucceed = false;
                    if (tran != null)
                    {
                        tran.Rollback();
                        con.Close();
                        tran = null;
                    }
                    return JsonConvert.SerializeObject(retResponse);
                }


                if (resposneSucceed)
                {
                    retval = partyid;
                    if (!string.IsNullOrEmpty(retval) && retval.Trim() != "")
                    {

                        str = "select * from MastProspect_Distributor  where partyid=" + retval;
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dt.Rows.Count > 0)
                        {
                            PartyName = dt.Rows[0]["partyname"].ToString();
                            distArea = PartyName;
                            createdBysmid = dt.Rows[0]["created Userid"].ToString();
                            CityId = dt.Rows[0]["cityid"].ToString();
                        }


                        //Push notification <<<
                        if (AR == "R")
                        {
                            pro_id = "PROSPECTDISTREJECT";
                            _msg = "A prospect distributor " + PartyName + " has been rejected by " + username;
                            displaytitle = "Prospect Distributor Approval";
                            retResponse = "Rejected SuccessFully";
                        }
                        else if (AR == "A")
                        {
                            pro_id = "PROSPECTDISTAPPROVE";
                            _msg = "A prospect distributor " + PartyName + " has been approved by " + username;
                            displaytitle = "Prospect Distributor Rejection";
                            retResponse = " Approved SuccessFully";
                        }


                        msgurl = "ProspectDistributorApprovalForm.aspx?partyid=" + partyid;

                        ////CreatedBy PushNotification<<<
                        str = "select userid,smid,mobile,email from mastsalesrep where userid=" + createdBysmid;
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dt.Rows.Count > 0)
                        {
                            ToMail = dt.Rows[0]["email"].ToString();
                            str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[ToSmid],[Title])  output inserted.notiid values ('" + pro_id + "'," + Convert.ToInt32(dt.Rows[0]["userid"].ToString().Trim()) + ",getdate(),'" + msgurl + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(Settings.Instance.UserID) + "," + userSmid + "," + dt.Rows[0]["smid"].ToString() + ",'" + displaytitle + "') ";
                            retval = DbConnectionDAL.GetStringScalarVal(str);
                            if (!string.IsNullOrEmpty(retval))
                            {
                                PushNotificationForDistAndManager(_msg, dt.Rows[0]["userid"].ToString(), dt.Rows[0]["mobile"].ToString(), "CRM MANAGER", displaytitle);
                            }
                        }
                        ////CreatedBy PushNotificatio>>>>>end

                        //SendMail<<<<

                        retResponse += SendMailForProsDistApprovalRejection(partyid, ToMail);

                        ///SendMail>>>>end
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(retResponse);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return JsonConvert.SerializeObject(retResponse);
        }


        public static string PushNotificationForDistAndManager(string msg, string userid, string DeviceNo, string ProductType, string title)
        {
            var result = "";
            string Query = "", Query1 = "";
            DataTable dt = new DataTable();
            string serverKey = "", senderId = "";
            try
            {


                DataTable dtserverdetail = DbConnectionDAL.GetDataTable(CommandType.Text, "Select DistApp_FireBase_ServerKey,DistApp_FireBase_SenderID,ManagerApp_FireBase_ServerKey,ManagerApp_FireBase_SenderID,compurl,CompCode from Mastenviro ");
                if (!string.IsNullOrEmpty(DeviceNo))
                {
                    string regid_query = "select Reg_id from LineMaster where Upper(Product)='" + ProductType + "' and  CompCode='" + dtserverdetail.Rows[0]["CompCode"].ToString() + "' and Mobile='" + DeviceNo + "' ";

                    string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";

                    Query1 = "select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='" + ProductType + "' AND Active ='Y'";
                    string us = DbConnectionDAL.GetStringScalarVal(Query1);

                    SqlConnection cn = new SqlConnection(constrDmLicense);
                    SqlCommand cmd = new SqlCommand(regid_query, cn);
                    SqlCommand cmd1 = new SqlCommand(Query1, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd1.CommandType = CommandType.Text;
                    cn.Open();
                    string regId = cmd.ExecuteScalar() as string;
                    string licenceinfo = cmd1.ExecuteScalar() as string;
                    cn.Close();
                    cmd = null;
                    if (!string.IsNullOrEmpty(regId))
                    {
                        Query1 = "insert into TransPushNotification(smid,[Subject],Content,WebFlag) output inserted.id " +
                            "values (" + userid + ",'" + title + "','" + msg + "','Y')";
                        string Id = DbConnectionDAL.GetStringScalarVal(Query1);

                        if (ProductType == "FFMS")
                        {
                            serverKey = dtserverdetail.Rows[0]["DistApp_FireBase_ServerKey"].ToString();
                            senderId = dtserverdetail.Rows[0]["DistApp_FireBase_SenderID"].ToString();
                        }
                        else if (ProductType == "CRM MANAGER")
                        {
                            serverKey = dtserverdetail.Rows[0]["ManagerApp_FireBase_ServerKey"].ToString();
                            senderId = dtserverdetail.Rows[0]["ManagerApp_FireBase_SenderID"].ToString();
                        }


                        string webAddr = "https://fcm.googleapis.com/fcm/send";

                        //var result = "-1";
                        var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                        tRequest.ContentType = "application/json";
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                        tRequest.Method = "POST";
                        var payload = new
                        {
                            to = regId,
                            priority = "high",
                            content_available = true,

                            data = new
                            {
                                body = msg,
                                title = title,
                                msg = msg
                            }
                        };

                        var serializer = new JavaScriptSerializer();
                        using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                        {
                            string json = serializer.Serialize(payload);

                            streamWriter.Write(json);
                            streamWriter.Flush();
                        }

                        var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();

                            Query1 = "update TransPushNotification set serverresponse='" + result + "' where id=" + Id + "";
                            DbConnectionDAL.ExecuteQuery(Query1);
                        }
                    }
                    else
                    {
                        result += "Registration No Is Null";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }


        public static string SendMailForProsDistApprovalRejection(string PartyId, string ToMail)
        {
            string inserTomail = ""; string insertSubject = ""; string _subject = "", orderStatus = "";
            string Subject, body, remark = "";
            string str = ""; string Message = "";
            string apprvdBy = "", status = "";
            string defaultmailId = ""; string defaultpassword = ""; string host = ""; string port = "", compurl = "", displaytitle = "";
            string retval = "";
            DataTable dt;

            try
            {
                str = "select SenderEmailID,SenderPassword,Port,MailServer,CompUrl from [MastEnviro]";
                DataTable checkemaildt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (checkemaildt.Rows.Count > 0)
                {
                    defaultmailId = checkemaildt.Rows[0]["SenderEmailID"].ToString();
                    defaultpassword = checkemaildt.Rows[0]["SenderPassword"].ToString();
                    host = checkemaildt.Rows[0]["MailServer"].ToString();
                    port = checkemaildt.Rows[0]["Port"].ToString();
                    compurl = checkemaildt.Rows[0]["CompUrl"].ToString();
                }

                str = "select sp1.smname as apprvRejBy,pd.PartyName,pd.Mobile,(pd.Address1+ ' - ' + pd.Address2) as addres,mct.AreaName,pd.Email,sp.SMName as createdBy,cast(pd.Created_Date as date) as Created_Date,pd.GSTIN,pd.Remark,pd.[PAN No],case when  ISNULL(pd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(pd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as status,case when  ISNULL(pd.Approved_Rejected,'')='' then '' else pd.Approved_RejectedRemark end as ARremk from MastProspect_Distributor pd left join MastSalesRep sp on pd.[Created Userid]=sp.UserId  left join MastArea mct on pd.CityId=mct.AreaId left join mastsalesrep sp1 on sp1.smid=pd.Approved_RejectedBySMID where  mct.AreaType='CITY' and pd.PartyId=" + PartyId;
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    status = dt.Rows[0]["status"].ToString();

                    if (status == "Approved")
                    {
                        displaytitle = "Prospect Distributor Approval";
                        Message = "Prospect distributor " + dt.Rows[0]["PartyName"] + " has been approved.";
                        remark = "<span style='font-weight: bold;'> Status : </span> <span style='color:#18AA39'> Approved </span> <br /><span style='font-weight: bold;'> Approved By  : </span> <span style='color:#F13214'> " + dt.Rows[0]["apprvRejBy"] + " </span> <br /> <span style='font-weight: bold;'> Approval Remark  : </span> <span style='color:#18AA39'> " + dt.Rows[0]["ARremk"] + "</span>  ";
                    }
                    else
                    {

                        displaytitle = "Prospect Distributor Rejection";
                        Message = "Prospect distributor " + dt.Rows[0]["PartyName"] + " has been rejected.";
                        remark = " <span style='font-weight: bold;'> Status : </span> <span style='color:#F13214'> Rejected </span> <br /><span style='font-weight: bold;'> Rejected By  : </span> <span style='color:#F13214'> " + dt.Rows[0]["apprvRejBy"] + " </span><br /><span style='font-weight: bold;'> Rejection Remark  : </span> <span style='color:#F13214'> " + dt.Rows[0]["ARremk"] + " </span>  ";
                    }

                    Subject = "Regarding  " + displaytitle;
                    insertSubject = Subject;
                    inserTomail = ToMail;

                    //header

                    body = "This is a system generated mail, do not need reply on this.";

                    body += @"<br /><span style='font-size:14px;'> Dear SalesPerson ,</span>";
                    body += "<br /><span style='font-size:14px;'> " + Message + "</span>";
                    body += "<br /><span style='font-weight: bold;font-size:16px;'><u>Prospect Distributor Details : -</u></span><br />";

                    body += "<table align='center' border='1' width='100%' >";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>DistributorName</span></td><td><span style='color:#9F000F;font-size:14px;'> " + dt.Rows[0]["PartyName"] + "</span></td></tr>";
                    body += "<tr><td style='white-space:normal;'><span style='font-weight: bold;font-size:14px;'>Address</span></td><td><span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["addres"].ToString() + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>Email</span></td><td><span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["Email"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>Mobile</span> </td><td><span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["Mobile"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>GSTIN</span> </td><td><span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["GSTIN"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>PANNo</span></td><td> <span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["PAN No"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>CreatedBy</span></td><td> <span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["createdBy"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>CreatedDate</span> </td><td><span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["Created_Date"] + "</span></td></tr>";
                    body += "<tr><td><span style='font-weight: bold;font-size:14px;'>Remark</span> </td><td> <span style='color:#3386FF;font-size:14px;'> " + dt.Rows[0]["Remark"] + "</span></td></tr>";

                    body += "</table>";

                    body += remark + " <br />";
                    body += "Thanks. <br /> Kindly visit following link for Further Process http://" + compurl;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(defaultmailId);
                    mail.Subject = Subject;
                    mail.Body = body;
                    mail.To.Add(new MailAddress(ToMail));

                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(checkemaildt.Rows[0]["SenderEmailId"]), Convert.ToString(checkemaildt.Rows[0]["SenderPassword"]));
                    SmtpClient mailclient = new SmtpClient(Convert.ToString(checkemaildt.Rows[0]["MailServer"]), Convert.ToInt32(checkemaildt.Rows[0]["Port"]));

                    mailclient.EnableSsl = true;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);

                    Message = "-";
                    retval = " And Mail has been Sent Successfully";
                    _subject = "Mail Successfully Send for:" + Subject + " at " + DateTime.Now + "";
                    str = "Insert into [CRM_EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate]) Values('" + _subject + "','" + Message + "','" + ToMail + "','Success',1,getdate())";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                }
                else
                {
                    retval = " And Mail Failure";
                }
            }
            catch (Exception ex)
            {

                retval = " And Mail Failure";
                _subject = "Mail Failure for:" + insertSubject + " at " + DateTime.Now + "";
                str = "Insert into  [CRM_EmailDataStatus]([Subject],[ErrorMessage],[EmailId],[EmailStatus],[UserId],[VDate]) Values('" + _subject + "','error','" + inserTomail + "','Failure',1,getdate())";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            return retval;
        }

        public static int InsertLocation(Int32 Parent, string Name, string SyncId, string Active, string Description, string AreaType, int CityType, int CityConveyanceType, string STDCode, string ISDCode, int BuisnessPlace, int Section, int CostCentre, ref SqlTransaction tran, SqlConnection con)
        {
            try
            {
                DbParameter[] dbParam = new DbParameter[14];
                dbParam[0] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 150, Parent);
                dbParam[1] = new DbParameter("@AreaName", DbParameter.DbType.VarChar, 150, Name);
                dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
                dbParam[3] = new DbParameter("@isActive", DbParameter.DbType.VarChar, 1, Active);
                dbParam[4] = new DbParameter("@AreaDesc", DbParameter.DbType.VarChar, 500, Description);
                dbParam[5] = new DbParameter("@AreaType", DbParameter.DbType.VarChar, 10, AreaType);
                dbParam[6] = new DbParameter("@CityType", DbParameter.DbType.Int, 1, CityType);
                dbParam[7] = new DbParameter("@CityConveyanceType", DbParameter.DbType.Int, 1, CityConveyanceType);
                dbParam[8] = new DbParameter("@STDCode", DbParameter.DbType.VarChar, 10, STDCode);
                dbParam[9] = new DbParameter("@ISDCode", DbParameter.DbType.VarChar, 10, ISDCode);
                dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                dbParam[11] = new DbParameter("@BuisnessPlace", DbParameter.DbType.Int, 1, BuisnessPlace);
                dbParam[12] = new DbParameter("@Section", DbParameter.DbType.Int, 1, Section);
                dbParam[13] = new DbParameter("@CostCentre", DbParameter.DbType.Int, 1, CostCentre);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertAreaForm", dbParam, con, ref tran);
                return Convert.ToInt32(dbParam[10].Value);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                    tran = null;
                    con.Close();
                }
                return 0;
            }
        }

        public static int InsertDistributors(SqlConnection con, ref SqlTransaction tran, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
         int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int Areid, int ProspectDistId, string Partygstin = "", string imageurl = "", int Partytype = 0, string DistType = "0", string SuperDistID = "0")
        {
            try
            {
                DbParameter[] dbParam = new DbParameter[40];
                dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
                dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
                dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
                dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
                dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
                dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
                dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
                dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
                dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
                dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
                dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
                dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
                dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
                dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
                dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
                dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
                dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
                dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
                dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
                if (!Active)
                {
                    dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                    dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                    dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
                }
                else
                {
                    dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                    dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                    dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
                }
                dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
                dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
                dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
                dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, Telex);
                dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
                dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
                dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);

                dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

                if (DOA != "")
                {
                    dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
                }
                else
                {
                    dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
                }

                if (DOB != "")
                {
                    dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
                }
                else
                {
                    dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
                }
                dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);



                if (Partytype == 0)
                {
                    dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, DBNull.Value);
                }
                else
                {
                    dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, Partytype);
                }

                if (Partygstin == "")
                {
                    dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, DBNull.Value);
                }
                else
                {
                    dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
                }

                dbParam[35] = new DbParameter("@imgurl", DbParameter.DbType.VarChar, 300, imageurl);
                dbParam[36] = new DbParameter("@Createduserid", DbParameter.DbType.Int, 12, UserId);
                if (DistType == "0")
                {
                    dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DBNull.Value);
                }
                else
                {
                    dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DistType);
                }

                if (SuperDistID == "0")
                {
                    dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, DBNull.Value);
                }
                else
                {
                    dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, Convert.ToInt32(SuperDistID));
                }
                dbParam[39] = new DbParameter("@ProspectDistId", DbParameter.DbType.Int, 12, Convert.ToInt32(ProspectDistId));
                //dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
                //dbParam[34] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorForm_ProspectDist", dbParam, con, ref tran);
                return Convert.ToInt32(dbParam[29].Value);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                    tran = null;
                    con.Close();
                }
                return 0;
            }

        }


        public static void InsertAreaAndDistributor(string partyid, ref string retResponse, ref bool resposneSucceed, DataTable dt, int roleId, SqlConnection con, ref SqlTransaction tran)
        {
            string str = "", PartyName = "", distArea = "", createdBysmid = "", CityId = "";

            int areaid = 0;
            try
            {

                //Create distributor  & area 
                PartyName = dt.Rows[0]["partyname"].ToString();
                distArea = PartyName;
                createdBysmid = dt.Rows[0]["created Userid"].ToString();
                CityId = dt.Rows[0]["cityid"].ToString();

                areaid = InsertLocation(Convert.ToInt32(CityId), "Area" + distArea, "Area" + distArea, "1", "", "AREA", 0, 0, "", "", 0, 0, 0, ref tran, con);
                if (areaid == -1)
                {
                    areaid = InsertLocation(Convert.ToInt32(CityId), "Area-" + distArea, "Area-" + distArea, "1", "", "AREA", 0, 0, "", "", 0, 0, 0, ref tran, con);
                    if (areaid < 0)
                    {
                        retResponse = "Prospect Distributor can not be approved due to  Area   " + "Area-" + distArea + " is already exist.";
                        resposneSucceed = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                            con.Close();
                            tran = null;
                        }
                        return;
                    }
                    else if (areaid == 0)
                    {
                        retResponse = "Error while inserting Area(Transaction),Prospect Distributor can not be approved.";
                        resposneSucceed = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                            con.Close();
                            tran = null;
                        }
                        return;
                    }
                }
                else if (areaid == -2)
                {
                    areaid = InsertLocation(Convert.ToInt32(CityId), "Area-" + distArea, "Area-" + distArea, "1", "", "AREA", 0, 0, "", "", 0, 0, 0, ref tran, con);
                    if (areaid < 0)
                    {
                        retResponse = "Prospect Distributor can not be approved due to  Area  SyncId " + "Area-" + distArea + " is already exist.";
                        resposneSucceed = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                            con.Close();
                            tran = null;
                        }
                        return;
                    }
                    else if (areaid == 0)
                    {
                        retResponse = "Error while inserting Area(Transaction),Prospect Distributor can not be approved.";
                        resposneSucceed = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                            con.Close();
                            tran = null;
                        }
                        return;
                    }
                }
                else if (areaid == 0)
                {
                    retResponse = "Error while inserting Area(Transaction),Prospect Distributor can not be approved.";
                    resposneSucceed = false;
                    if (tran != null)
                    {
                        tran.Rollback();
                        con.Close();
                        tran = null;
                    }
                    return;
                }

                if (areaid > 0)
                {
                    int UserId = Convert.ToInt32(Settings.Instance.UserID);
                    bool active = true;
                    int dist = InsertDistributors(con, ref tran, PartyName, dt.Rows[0]["address1"].ToString(), dt.Rows[0]["address2"].ToString(), CityId, dt.Rows[0]["pin"].ToString(), dt.Rows[0]["email"].ToString(), dt.Rows[0]["mobile"].ToString(), "", PartyName, "", PartyName, active, dt.Rows[0]["mobile"].ToString(), roleId, dt.Rows[0]["contactPersonName"].ToString(), "", "", "", dt.Rows[0]["PAN NO"].ToString(), 0, 0, 0, 0, UserId, "", "", "", Convert.ToInt32(dt.Rows[0]["smid"].ToString()), "", "", areaid, Convert.ToInt32(partyid), dt.Rows[0]["GSTIN"].ToString(), "", 0, "DIST", "0");

                    //-1 for duplicate user name and -2 for Duplicate syncid, >0 for successFully inserted.

                    if (dist == 0)
                    {
                        retResponse = "Error while inserting Distributor(Transaction),Prospect Distributor can not be approved.";
                        resposneSucceed = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                            con.Close();
                            tran = null;
                        }
                        return;
                    }
                    else if (dist == -1)
                    {
                        dist = InsertDistributors(con, ref tran, PartyName, dt.Rows[0]["address1"].ToString(), dt.Rows[0]["address2"].ToString(), CityId, dt.Rows[0]["pin"].ToString(), dt.Rows[0]["email"].ToString(), dt.Rows[0]["mobile"].ToString(), "", PartyName, "", PartyName + "123", active, dt.Rows[0]["mobile"].ToString(), roleId, dt.Rows[0]["contactPersonName"].ToString(), "", "", "", dt.Rows[0]["PAN NO"].ToString(), 0, 0, 0, 0, UserId, "", "", "", Convert.ToInt32(dt.Rows[0]["smid"].ToString()), "", "", areaid, Convert.ToInt32(partyid), dt.Rows[0]["GSTIN"].ToString(), "", 0, "DIST", "0");
                        if (dist == 0)
                        {
                            retResponse = "Error while inserting Distributor(Transaction),Prospect Distributor can not be approved.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist == -1)
                        {
                            retResponse = "Prospect Distributor can not be approved due to  UserName- " + PartyName + "123" + "  is already exist.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist == -2)
                        {
                            retResponse = "Prospect Distributor can not be approved due to  SyncId- " + PartyName + " is already exist.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist > 0)
                        {
                            resposneSucceed = true;
                        }


                    }
                    else if (dist == -2)
                    {
                        dist = InsertDistributors(con, ref tran, PartyName, dt.Rows[0]["address1"].ToString(), dt.Rows[0]["address2"].ToString(), CityId, dt.Rows[0]["pin"].ToString(), dt.Rows[0]["email"].ToString(), dt.Rows[0]["mobile"].ToString(), "", PartyName + "_", "", PartyName + "123", active, dt.Rows[0]["mobile"].ToString(), roleId, dt.Rows[0]["contactPersonName"].ToString(), "", "", "", dt.Rows[0]["PAN NO"].ToString(), 0, 0, 0, 0, UserId, "", "", "", Convert.ToInt32(dt.Rows[0]["smid"].ToString()), "", "", areaid, Convert.ToInt32(partyid), dt.Rows[0]["GSTIN"].ToString(), "", 0, "DIST", "0");

                        if (dist == 0)
                        {
                            retResponse = "Error while inserting Distributor(Transaction),Prospect Distributor can not be approved.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist == -1)
                        {
                            retResponse = "Prospect Distributor can not be approved due to  UserName- " + PartyName + "123" + "  is already exist.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist == -2)
                        {
                            retResponse = "Prospect Distributor can not be approved due to  SyncId- " + PartyName + " is already exist.";
                            resposneSucceed = false;
                            if (tran != null)
                            {
                                tran.Rollback();
                                con.Close();
                                tran = null;
                            }
                            return;
                        }
                        else if (dist > 0)
                        {
                            resposneSucceed = true;
                        }
                    }
                    else if (dist > 0)
                    {
                        resposneSucceed = true;
                    }

                }

                // Distributor & Area Creation end
            }
            catch (Exception ex)
            {
                retResponse = "Error while inserting Area/Distributor ,Prospect Distributor can not be approved.";
                resposneSucceed = false;
                if (tran != null)
                {
                    tran.Rollback();
                    con.Close();
                    tran = null;
                }
            }

        }




    }
}