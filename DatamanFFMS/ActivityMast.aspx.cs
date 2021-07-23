using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class ActivityMast : System.Web.UI.Page
    {
        String Level = "0";
        string pageSalesName = "", discount = "", strItem = "", strDItem = "", loseQty = "", stockQty = "", bUF = "", Rate = "", cQty = "", unit = "", roleType = "", parameter = "", VisitID = "", CityID = "", distID = "", PartyID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }

            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }

            if (Request.QueryString["DistID"] != null)
            {
                distID = Request.QueryString["DistID"].ToString();
            }
            if (Request.QueryString["PartyID"] != null)
            {
                PartyID = Request.QueryString["PartyID"].ToString();
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ActivityID = "";
                if (string.IsNullOrEmpty(hdnValue.Value))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Title');", true); return;
                }
               
                //ActivityID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select isnull((select Activity_Id from ActivityTransaction where VisId=" + Convert.ToInt64(VisitID) + " and HeaderId=" + hdnValue.Value + "),0)").ToString();
               
                string[] arrHidCV = hidcustomval.Value.Split('&');
                string[] arrHidCF = hidcustomfields.Value.Split('^');
                //string[] arrCFCols = arrHidCV[j].Split(':');

                string valuesToSave = string.Empty, ColsToSave = string.Empty; string distpartytype = string.Empty; string distpartyid = string.Empty;

                if (arrHidCV != null)
                {
                    if (arrHidCV.Length > 0)
                    {
                        for (int i = 0; i < arrHidCV.Length; i++)
                        {
                            string[] arrCFCols = arrHidCV[i].Split(':');
                            if (arrCFCols.Length > 0)
                            {
                                //for (int j = 0; j < arrCFCols.Length; j++)
                                //{
                                if (ColsToSave == "")
                                    ColsToSave = "[" + arrCFCols[0].ToString() + "]";
                                else if (ColsToSave != "")
                                    ColsToSave = ColsToSave + "," + "[" + arrCFCols[0].ToString() + "]";

                                if (valuesToSave == "")
                                {
                                    if (arrCFCols[1].ToString() == "")
                                        valuesToSave = "'" + " " + "'";
                                    else if (arrCFCols[1].ToString() != "")
                                        valuesToSave = "'" + arrCFCols[1].ToString() + "'";
                                }
                                else if (valuesToSave != "")
                                    valuesToSave = valuesToSave + ",'" + arrCFCols[1].ToString() + "'";
                                //}

                            }

                        }
                    }
                }
                hidcustomfields.Value = ColsToSave;

                //string[] arrCF = hidcustomfields.Value.Split('^');
                //for (int i = 0; i < arrCF.Length; i++)
                //{
                //    ColsToSave += "[" + arrCF[i] + "],";
                //}
                //if (!string.IsNullOrEmpty(hidcustomfields.Value))
                //    hidcustomfields.Value = "," + ColsToSave.Substring(0, ColsToSave.Length - 1);

                String varname12 = "";

                if (Request.QueryString["DistID"] != null)
                {
                    distpartytype = "Distributor";
                    distpartyid = Request.QueryString["DistID"].ToString();
                }
                else if (Request.QueryString["PartyID"] != null)
                {
                    distpartytype = "Retailer";
                    distpartyid = Request.QueryString["PartyID"].ToString();
                }

                string qry = "select fromdate,todate from MastActivityCustomHeader where Header_Id=" + hdnValue.Value;
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

                int val = Convert.ToInt32(Settings.Instance.DSRSMID);

                ActivityID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, "select isnull((select Activity_Id from ActivityTransaction where VisId=" + Convert.ToInt64(VisitID) + " and HeaderId=" + hdnValue.Value + " and DistId=" + Convert.ToInt64(distpartyid) + "),0)").ToString();


                if (ActivityID == "" || ActivityID == "0")
                {
                    string VDate = GetVisitDate(Convert.ToInt32(VisitID));
                    varname12 = " INSERT INTO ActivityTransaction (VisId,	VDate,	DistId,	SMId,HeaderId,FromDate,ToDate,ForType, " + hidcustomfields.Value + ") " + "\n OUTPUT INSERTED.Activity_Id";
                    varname12 += " VALUES (" + Convert.ToInt64(VisitID) + ",'" + Convert.ToDateTime(VDate).ToString("MM/dd/yyyy") + "'," + Convert.ToInt32(distpartyid) + ",";
                    varname12 += " " + Convert.ToInt32(Settings.Instance.DSRSMID) + "," + hdnValue.Value + ",'" + Convert.ToDateTime(dt.Rows[0]["FromDate"].ToString()).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString()).ToString("MM/dd/yyyy") + "',	'" + distpartytype + "', " + valuesToSave + "	) ";
                    ActivityID = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, varname12).ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Saved Successfully');", true);
                }
                else
                {

                    string value = "";
                    string[] strField = ColsToSave.Split(',');
                    string[] strValue = valuesToSave.Split(',');
                    for (int i = 0; i < strField.Length; i++)
                    {
                        if (value == "")
                            value = strField[i].ToString() + "=" + strValue[i].ToString();
                        else if (value != "")
                            value = value + "," + strField[i].ToString() + "=" + strValue[i].ToString();
                    }
                    // ,[F11],[F12],[F13],[F14]
                    varname12 = "update ActivityTransaction set " + value + " where Activity_Id='" + ActivityID + "'";
                    DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, varname12);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                }
            }
            catch(Exception ex)
            { ex.ToString(); }

        }
       
        protected void btnBck_Click(object sender, EventArgs e)
        {
            string Level = Request.QueryString["Level"].ToString();
            string party = Request.QueryString["PartyId"] != null ? Request.QueryString["PartyId"].ToString() : "";
            //string party = Request.QueryString["PageV"].ToString();
            string dist = Request.QueryString["DistId"]!=null?Request.QueryString["DistId"].ToString():"";
            if (Level == "1")
            {
                if (dist != null && dist!="")
                    Response.Redirect("~/DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
                else if (party != null && party != "")
                    Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level +"& PageV="+"'Secondary'");
            }

            //else if (Level == "2")
            //{
            //    Response.Redirect("~/DistributorDashboardLevel2.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            //}

            //else
            //{
            //    Response.Redirect("~/DistributorDashboardLevel3.aspx?PartyId=" + DistId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            //}
        }

        private string GetVisitDate(int VisiID)
        {
            string st = "select VDate from TransVisit where VisId=" + VisitID;
            string VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return VisitDate;
        }
    }
}