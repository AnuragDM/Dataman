using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class AgileReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        DataTable YrTable = new DataTable("Year");
        DataColumn dtColumn;
        DataRow myDataRow;

        DataTable AgileTable = new DataTable("Agile");
        DataColumn dtColumn1;
        DataRow myDataRow1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            AgileTable.Columns.Add("PartyName", typeof(string));
            AgileTable.Columns.Add("Curr", typeof(string));
            AgileTable.Columns.Add("NinOT", typeof(string));
            AgileTable.Columns.Add("OtOF", typeof(string));
            AgileTable.Columns.Add("OFOE", typeof(string));
            AgileTable.Columns.Add("OELst", typeof(string));
            AgileTable.Columns.Add("lastyr", typeof(string));
            AgileTable.Columns.Add("total", typeof(string));
            AgileTable.Columns.Add("syncid", typeof(string));

            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                BindDistributorDDl();
                //txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        private void BindDistributorDDl()
        {
            try
            {
                string distqry = @"select DISTINCT ma.PartySyncId as PartyId,mp.PartyName as PartyName from MastAgile ma left join MastParty mp on ma.PartySyncId=mp.SyncId where mp.PartyDist=1 and mp.Active=1 order by mp.PartyName";
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

        //private void BindStaffChkList()
        //{
        //    try
        //    {


        //        ListBox lstyr = new ListBox();
        //        for (int i = System.DateTime.Now.Year; i >= (System.DateTime.Now.Year - 4); i--)
        //        {
        //            DataRow row = YrTable.NewRow();
        //            //int CurrentYear = i;
        //            // int NextYear = i + 1;
        //            // string FinYear = null;
        //            // FinYear = CurrentYear + "-" + NextYear;
        //            lstyr.Items.Add(new ListItem(i.ToString()));
        //            row["YrId"] = i.ToString();
        //            row["YrName"] = (i).ToString() + " - " + (i + 1).ToString();
        //            YrTable.Rows.Add(row);
        //        }
        //        LstYear.DataSource = YrTable;
        //        LstYear.DataTextField = "YrName";
        //        LstYear.DataValueField = "YrId";
        //        LstYear.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string smIDStr = "", smIDStr1 = "", distId = "", year = "", Querybeat = "", matBeatNew = "";

            decimal sumcurr = 0, sumprev = 0, sumnin = 0, sumot = 0, sumof = 0, sumoe = 0, sumlst = 0, total = 0;

            foreach (ListItem Dist in ListBox1.Items)
            {
                if (Dist.Selected)
                {
                    distId += "'" + Dist.Value + "',";
                }
            }
            distId = distId.TrimStart(',').TrimEnd(',');
            if (distId != "")
            {
                string[] yr = year.Split('-');
                DateTime start = Settings.GetUTCTime();//0
                DateTime ninty = start.AddDays(-90);//90
                DateTime ot = ninty.AddDays(-30);//120
                DateTime of = ot.AddDays(-30);//150
                DateTime oe = of.AddDays(-30);//180
                DateTime sixmonth = oe.AddDays(-185);//365


                string query = "Select Distinct PartySyncId from MastAgile where PartySyncId in (" + distId + ")";
                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    foreach (DataRow row in dtItem.Rows)
                    {
                        sumcurr = 0; sumprev = 0; sumnin = 0; sumot = 0; sumof = 0; sumoe = 0; sumlst = 0; total = 0;

                        string query1 = "Select max(mp.PartyName) as PartyName,(select sum(cast(mag.Payment as decimal)) from MastAgile mag where max(ma.PartySyncId)=mag.PartySyncId and ma.VchNo=mag.VchNo and mag.mode='1' ) as VchPay,isnull((select sum(cast(mag.Payment as decimal)) from MastAgile mag where max(ma.PartySyncId)=mag.PartySyncId and ma.VchNo=mag.VchNo and mag.mode='2' ),'0') as Vchrec,max(ma.Date) as Date,max(ma.DueDate) as DueDate,ma.VchNo,ma.PartySyncId as syncid from MastAgile ma left join MastParty mp on ma.PartySyncId=mp.SyncId where mp.PartyDist=1 and mp.Active=1 and ma.PartySyncId = '" + row["PartySyncId"] + "' group by ma.PartySyncId,ma.VchNo";
                        DataTable dtgetdet = DbConnectionDAL.GetDataTable(CommandType.Text, query1);
                        if (dtgetdet.Rows.Count > 0)
                        {
                            foreach (DataRow rw in dtgetdet.Rows)
                            {
                                if (Convert.ToDateTime(rw["DueDate"].ToString()) <= start && Convert.ToDateTime(rw["DueDate"].ToString()) >= ninty)
                                {
                                    sumnin += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }
                                if (Convert.ToDateTime(rw["DueDate"].ToString()) < ninty && Convert.ToDateTime(rw["DueDate"].ToString()) >= ot)
                                {
                                    sumot += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }
                                if (Convert.ToDateTime(rw["DueDate"].ToString()) < ot && Convert.ToDateTime(rw["DueDate"].ToString()) >= of)
                                {
                                    sumof += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }
                                if (Convert.ToDateTime(rw["DueDate"].ToString()) < of && Convert.ToDateTime(rw["DueDate"].ToString()) >= oe)
                                {
                                    sumoe += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }
                                if (Convert.ToDateTime(rw["DueDate"].ToString()) < oe && Convert.ToDateTime(rw["DueDate"].ToString()) >= sixmonth)
                                {
                                    sumlst += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }

                                if (Convert.ToDateTime(rw["DueDate"].ToString()) < sixmonth)
                                {
                                    sumprev += Convert.ToDecimal(rw["VchPay"].ToString()) + Convert.ToDecimal(rw["Vchrec"].ToString());
                                }
                                //AgileTable.Columns.Add("ZeroNint", typeof(string));

                                total = sumcurr + sumnin + sumot + sumof + sumoe + sumlst + sumprev;
                            }
                            if (total != 0)
                            {
                                DataRow agrow = AgileTable.NewRow();

                                agrow["PartyName"] = dtgetdet.Rows[0]["PartyName"].ToString();

                                if (sumnin < 0)
                                {
                                    agrow["Curr"] = (sumnin * -1).ToString();
                                }
                                else
                                {
                                    agrow["Curr"] = sumnin.ToString();
                                }

                                if (sumot < 0)
                                {
                                    agrow["NinOT"] = (sumot * -1).ToString();
                                }
                                else
                                {
                                    agrow["NinOT"] = sumot.ToString();
                                }

                                if (sumof < 0)
                                {
                                    agrow["OtOF"] = (sumof * -1).ToString();
                                }
                                else
                                {
                                    agrow["OtOF"] = sumof.ToString();
                                }

                                if (sumoe < 0)
                                {
                                    agrow["OFOE"] = (sumoe * -1).ToString();
                                }
                                else
                                {
                                    agrow["OFOE"] = sumoe.ToString();
                                }

                                if (sumlst < 0)
                                {
                                    agrow["OELst"] = (sumlst * -1).ToString();
                                }
                                else
                                {
                                    agrow["OELst"] = sumlst.ToString();
                                }

                                if (sumprev < 0)
                                {
                                    agrow["lastyr"] = (sumprev * -1).ToString();
                                }
                                else
                                {
                                    agrow["lastyr"] = sumprev.ToString();
                                }

                                if (total < 0)
                                {
                                    agrow["total"] = (total * -1).ToString();
                                }
                                else
                                {
                                    agrow["total"] = total.ToString();
                                }

                                agrow["Curr"] = (sumnin * -1).ToString();
                                agrow["NinOT"] = (sumot * -1).ToString();
                                agrow["OtOF"] = (sumof * -1).ToString();
                                agrow["OFOE"] = (sumoe * -1).ToString();
                                agrow["OELst"] = (sumlst * -1).ToString();
                                agrow["lastyr"] = (sumprev * -1).ToString();
                                
                                agrow["syncid"] = dtgetdet.Rows[0]["syncid"].ToString();

                                AgileTable.Rows.Add(agrow);
                                AgileTable.AcceptChanges();
                            }
                        }
                    }
                    detailDistrpt.DataSource = AgileTable;
                    detailDistrpt.DataBind();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No data found');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Distributor');", true);
            }
        }

        protected void detailDistrpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                HiddenField hdnsync = (HiddenField)e.Item.FindControl("hdnsyncid");
                //HiddenField hdnp = (HiddenField)e.Item.FindControl("hdnparty");
                Response.Redirect("AgingDetail.aspx?sync=" + hdnsync.Value.Replace("#", "%23"));
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=AgingSummaryReport.csv");
            string headertext = "Party Name".TrimStart('"').TrimEnd('"') + "," + "(0-90) Days".TrimStart('"').TrimEnd('"') + "," + "(91-120) Days".TrimStart('"').TrimEnd('"') + "," + "(121-150) Days".TrimStart('"').TrimEnd('"') + "," + "(151-180) Days".TrimStart('"').TrimEnd('"') + "," + "(181-365) Days".TrimStart('"').TrimEnd('"') + "," + "(>=366) Days".TrimStart('"').TrimEnd('"') + "," + "Total".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Party", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Ninty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OneTwenty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OneFifty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OneEighty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TheeSixtyFive", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Last", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Total", typeof(String)));

            foreach (RepeaterItem item in detailDistrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblparty = item.FindControl("lblparty") as Label;
                dr["Party"] = lblparty.Text.ToString();
                Label lblcurr = item.FindControl("lblcurr") as Label;
                dr["Ninty"] = lblcurr.Text.ToString();
                Label lblninty = item.FindControl("lblninty") as Label;
                dr["OneTwenty"] = lblninty.Text.ToString();
                Label lblonetwozero = item.FindControl("lblonetwozero") as Label;
                dr["OneFifty"] = lblonetwozero.Text.ToString();
                Label lblonefivezero = item.FindControl("lblonefivezero") as Label;
                dr["OneEighty"] = lblonefivezero.Text.ToString();
                Label lbloneegtzero = item.FindControl("lbloneegtzero") as Label;
                dr["TheeSixtyFive"] = lbloneegtzero.Text.ToString();
                Label lbllastyr = item.FindControl("lbllastyr") as Label;
                dr["Last"] = lbllastyr.Text.ToString();
                Label lbltotal = item.FindControl("lbltotal") as Label;
                dr["Total"] = lbltotal.Text.ToString();
                dtParams.Rows.Add(dr);
            }
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        string h = dtParams.Rows[j][k].ToString();
                        string d = h.Replace(",", " ");
                        dtParams.Rows[j][k] = "";
                        dtParams.Rows[j][k] = d;
                        dtParams.AcceptChanges();
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=AgingSummaryReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AgileReport.aspx");
        }
    }
}