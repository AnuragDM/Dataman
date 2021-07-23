using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using DAL;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace AstralFFMS
{
    public partial class AgingDetail : System.Web.UI.Page
    {
        int uid = 0;
        int smID = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;

        DataTable AgileTable = new DataTable("Agile");
        DataColumn dtColumn1;
        DataRow myDataRow1;



        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            AgileTable.Columns.Add("RecNo", typeof(string));
            AgileTable.Columns.Add("Type", typeof(string));
            AgileTable.Columns.Add("Date", typeof(DateTime));
            AgileTable.Columns.Add("DueDate", typeof(DateTime));
            AgileTable.Columns.Add("Total", typeof(string));
            AgileTable.Columns.Add("Pending", typeof(string));
            AgileTable.Columns.Add("PartyName", typeof(string));
            AgileTable.Columns.Add("Curr", typeof(string));
            AgileTable.Columns.Add("NinOT", typeof(string));
            AgileTable.Columns.Add("OtOF", typeof(string));
            AgileTable.Columns.Add("OFOE", typeof(string));
            AgileTable.Columns.Add("OELst", typeof(string));
            AgileTable.Columns.Add("lastyr", typeof(string));
            //AgileTable.Columns.Add("total", typeof(string));
            AgileTable.Columns.Add("syncid", typeof(string));

            if (Request.QueryString["Page"] != null)
            {
                pageName = Request.QueryString["Page"];
            }

            if (Request.QueryString["sync"] != null)
            {
                syncHiddenField.Value = Request.QueryString["sync"].Replace("#", "%23");

                getdetails(syncHiddenField.Value);
            }
        }

        public void getdetails(string sync)
        {
            string smIDStr = "", smIDStr1 = "", distId = "", year = "", Querybeat = "", matBeatNew = "";

            decimal sumcurr = 0, sumprev = 0, sumnin = 0, sumot = 0, sumof = 0, sumoe = 0, sumlst = 0, total = 0;
            if (sync != "")
            {
                string[] yr = year.Split('-');
                DateTime start = Settings.GetUTCTime();//0
                DateTime ninty = start.AddDays(-90);//90
                DateTime ot = ninty.AddDays(-30);//120
                DateTime of = ot.AddDays(-30);//150
                DateTime oe = of.AddDays(-30);//180
                DateTime sixmonth = oe.AddDays(-185);//365


                string query = "select [Ref. No.] as Ref,'OpBal' as Type,Date as Dated,DueDate as DueDate,VchPay as TotalAmt,Vchrec as RecAmt,(cast(VchPay as decimal)+CAST(Vchrec as decimal)) as PendingAmt,PartyName,syncid from (Select max(mp.PartyName) as PartyName,(select sum(cast(mag.Payment as decimal)) from MastAgile mag where max(ma.PartySyncId)=mag.PartySyncId and ma.VchNo=mag.VchNo and mag.mode='1') as VchPay,isnull((select sum(cast(mag.Payment as decimal)) from MastAgile mag where max(ma.PartySyncId)=mag.PartySyncId and ma.VchNo=mag.VchNo and mag.mode='2' ),'0') as Vchrec,max(ma.Date) as Date,max(ma.DueDate) as DueDate,ma.VchNo as [Ref. No.],ma.PartySyncId as syncid from MastAgile ma left join MastParty mp on ma.PartySyncId=mp.SyncId where mp.PartyDist=1 and mp.Active=1 and ma.PartySyncId = '" + sync.Replace("%23", "#") + "' group by ma.PartySyncId,ma.VchNo)tbl";
                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    foreach (DataRow row in dtItem.Rows)
                    {
                        sumcurr = 0; sumprev = 0; sumnin = 0; sumot = 0; sumof = 0; sumoe = 0; sumlst = 0; total = 0;

                        if (Convert.ToDateTime(row["DueDate"].ToString()) <= start && Convert.ToDateTime(row["DueDate"].ToString()) >= ninty)
                        {
                            sumnin = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }
                        if (Convert.ToDateTime(row["DueDate"].ToString()) < ninty && Convert.ToDateTime(row["DueDate"].ToString()) >= ot)
                        {
                            sumot = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }
                        if (Convert.ToDateTime(row["DueDate"].ToString()) < ot && Convert.ToDateTime(row["DueDate"].ToString()) >= of)
                        {
                            sumof = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }
                        if (Convert.ToDateTime(row["DueDate"].ToString()) < of && Convert.ToDateTime(row["DueDate"].ToString()) >= oe)
                        {
                            sumoe = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }
                        if (Convert.ToDateTime(row["DueDate"].ToString()) < oe && Convert.ToDateTime(row["DueDate"].ToString()) >= sixmonth)
                        {
                            sumlst = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }

                        if (Convert.ToDateTime(row["DueDate"].ToString()) < sixmonth)
                        {
                            sumprev = Convert.ToDecimal(row["TotalAmt"].ToString()) + Convert.ToDecimal(row["RecAmt"].ToString());
                        }

                        total = sumcurr + sumnin + sumot + sumof + sumoe + sumlst + sumprev;

                        if (Convert.ToDecimal(row["PendingAmt"].ToString()) != 0)
                        {
                            DataRow agrow = AgileTable.NewRow();

                            agrow["RecNo"] = row["Ref"].ToString();
                            agrow["Type"] = row["Type"].ToString();
                            agrow["Date"] = row["Dated"].ToString();
                            agrow["DueDate"] = row["DueDate"].ToString();

                            if (Convert.ToDecimal(row["TotalAmt"].ToString()) < 0)
                            {
                                agrow["Total"] = (Convert.ToDecimal(row["TotalAmt"].ToString())* -1).ToString();
                            }
                            else
                            {
                                agrow["Total"] = row["TotalAmt"].ToString();
                            }

                            if (Convert.ToDecimal(row["PendingAmt"].ToString()) < 0)
                            {
                                agrow["Pending"] = (Convert.ToDecimal(row["PendingAmt"].ToString()) * -1).ToString();
                            }
                            else
                            {
                                agrow["Pending"] = row["PendingAmt"].ToString();
                            }
                            
                            agrow["PartyName"] = row["PartyName"].ToString();

                            if (sumnin<0)
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
                            
                            agrow["syncid"] = row["syncid"].ToString();

                            AgileTable.Rows.Add(agrow);
                            AgileTable.AcceptChanges();


                        }
                    }
                    //int count = AgileTable.Rows.Count;
                    currDateLabel.Text = AgileTable.Rows[0]["PartyName"].ToString();
                    DateTime minimunDate = AgileTable.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["Date"])).Min();
                    lblrng.Text = minimunDate.ToString("dd/MMM/yyyy") + " to " + Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                    lblstatus.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
         //   Response.AddHeader("content-disposition", "attachment;filename=AgingDetails-"+currDateLabel.Text+".csv");
            string headertext = "Ref No".TrimStart('"').TrimEnd('"') + "," + "Type".TrimStart('"').TrimEnd('"') + "," + "Dated".TrimStart('"').TrimEnd('"') + "," + "Due Date".TrimStart('"').TrimEnd('"') + "," + "Total Amt".TrimStart('"').TrimEnd('"') + "," + "Pending Amt".TrimStart('"').TrimEnd('"') + "," + "(0-90) Days".TrimStart('"').TrimEnd('"') + "," + "(91-120) Days".TrimStart('"').TrimEnd('"') + "," + "(121-150) Days".TrimStart('"').TrimEnd('"') + "," + "(151-180) Days".TrimStart('"').TrimEnd('"') + "," + "(181-365) Days".TrimStart('"').TrimEnd('"') + "," + "(>=366) Days".TrimStart('"').TrimEnd('"');

            //StringBuilder sb = new StringBuilder();
            //sb.Append(headertext);
            //sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("RefNo", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Type", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Dated", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DueDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotalAmt", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PendingAmt", typeof(String)));
            dtParams.Columns.Add(new DataColumn("090Days", typeof(String)));
            dtParams.Columns.Add(new DataColumn("91120Days", typeof(String)));
            dtParams.Columns.Add(new DataColumn("121150Days", typeof(String)));
            dtParams.Columns.Add(new DataColumn("151180Days", typeof(String)));
            dtParams.Columns.Add(new DataColumn("181365Days", typeof(String)));
            dtParams.Columns.Add(new DataColumn("366Days", typeof(String)));   

            foreach (DataRow dr1 in AgileTable.Rows)
            {
                DataRow dr = dtParams.NewRow();
                dr["RefNo"] = dr1["RecNo"];
                dr["Type"] = Convert.ToString(dr1["Type"]);
                dr["Dated"] = Convert.ToString(dr1["Date"]);
                dr["DueDate"] = Convert.ToString(dr1["DueDate"]);
                dr["TotalAmt"] = Convert.ToString(dr1["Total"]);
                dr["PendingAmt"] = Convert.ToString(dr1["Pending"]);
                dr["090Days"] = Convert.ToString(dr1["Curr"]);
                dr["91120Days"] = Convert.ToString(dr1["NinOT"]);
                dr["121150Days"] = Convert.ToString(dr1["OtOF"]);
                dr["151180Days"] = Convert.ToString(dr1["OFOE"]);
                dr["181365Days"] = Convert.ToString(dr1["OELst"]);
                dr["366Days"] = Convert.ToString(dr1["lastyr"]);

                dtParams.Rows.Add(dr);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Distribuor :," + currDateLabel.Text);
            sb.AppendLine(System.Environment.NewLine);
            sb.Append("From :," + lblrng.Text);
            sb.AppendLine(System.Environment.NewLine);
            sb.Append("Bill Status as on :," + lblstatus.Text);
            sb.AppendLine(System.Environment.NewLine);
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);

            DataView dv = dtParams.DefaultView;
            //dv.Sort = "VDate desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[12];
            try
            {
                for (int j = 0; j < udtNew.Rows.Count; j++)
                {
                    for (int k = 0; k < udtNew.Columns.Count; k++)
                    {

                        if (udtNew.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 2)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else if (k == 3)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');

                                if (k == 4 || k == 5 || k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }

                        }
                        else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 2)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else if (k == 3)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');

                                if (k == 4 || k == 5 || k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (k == 2)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else if (k == 3)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');

                                if (k == 4 || k == 5 || k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 4; x < totalVal.Count(); x++)
                {
                    if (x == 4 || x == 5 || x == 6 || x == 7 || x == 8 || x == 9 || x == 10 || x == 11)
                    {

                        if (totalVal[x] == 0)
                        {
                            totalStr += "0" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }
                    }
                    else
                    {
                        totalStr += " " + ',';
                    }
                }
                sb.Append(",,,Total," + totalStr);
                sb.AppendLine(System.Environment.NewLine);
                string my_String = Regex.Replace(currDateLabel.Text, @"[^0-9a-zA-Z]+", "-");
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=AgingDetails.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }        
}            
             
             
             
             
             
             
             