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
using System.Globalization;
using System.Collections;
using Telerik.Web.UI;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace AstralFFMS
{
    public partial class DashboardSecondaryDetail : System.Web.UI.Page
    {
        string roleType = "", sql = "";
        string name = "", Date = "";
        string To = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FromDate.Attributes.Add("readonly", "readonly");
            ToDate.Attributes.Add("readonly", "readonly");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            scriptManager.RegisterPostBackControl(this.btnexportvisit);
            scriptManager.RegisterPostBackControl(this.btncollection);
            scriptManager.RegisterPostBackControl(this.btndemo);
            scriptManager.RegisterPostBackControl(this.btnCompetitor);
            

            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                ToDate.Text = Request.QueryString["Date"];               
                fillMode();
                fillFailedVisit();
            }
            this.fillAllRecord();
        }

        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            int SMId = Convert.ToInt32(Request.QueryString["SMId"]);
            Date = FromDate.Text;
            To = ToDate.Text;
            string mode = "";
            string FailedVisit = "";
          //  string bn = ddlmode.SelectedItem.Text;
            if (!string.IsNullOrEmpty(ddlmode.SelectedValue))
            {
                if (ddlmode.SelectedValue != "0")
                {
                    mode = ddlmode.SelectedValue;
                }
            }
            if (name.Equals("TotalOrder"))
            {
                lblHeading.InnerText = "Total Order Detail";
                sql = "select * from (SELECT msp.SMId,msp.smname,mp.partyname,mi.itemname,t1.qty,t1.amount,t1.VDate as created_date FROM TRANSORDER1 t1 left join mastparty mp on  mp.partyid=t1.partyid left join MastSalesRep msp on msp.smid=t1.SMId left join mastitem mi on  mi.itemid=t1.itemid WHERE t1.smid=" + SMId + " and CAST(t1.VDate as datetime) >='" + Date + "' and CAST(t1.VDate as datetime) <='" + To + "' union SELECT msp.SMId,msp.smname,mp.partyname,mi.itemname,t1.qty,t1.amount,t1.VDate as created_date FROM temp_TRANSORDER1 t1 left join mastparty mp on  mp.partyid=t1.partyid left join MastSalesRep msp on msp.smid=t1.SMId left join mastitem mi on  mi.itemid=t1.itemid WHERE t1.smid=" + SMId + " and CAST(t1.VDate as datetime) >='" + Date + "' and CAST(t1.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptTotalOrder.DataSource = dt;
                rptTotalOrder.DataBind();
                rptDemo.Visible = false;
                rptFaildvisit.Visible = false;
                rptCompetitor.Visible = false;
                rptCollection.Visible = false;
                btnexportvisit.Visible = false;
                btncollection.Visible = false;
                btndemo.Visible = false;
                btnCompetitor.Visible = false;
                DIVFailedVisit.Visible = false;
                DIVMode.Visible = false;
                
            }
            else if (name.Equals("Demo"))
            {
                lblHeading.InnerText = "Total Demo Detail";
                sql = "select * from  (select msp.SMId,msp.smname,mp.partyname,td.remarks,mc.Name AS ProdClass,ms.Name AS ProdSegment,mi.itemname as ProductGrp,td.VDate as created_date from transdemo  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId left join mastitem mi on  mi.itemid=td.ProductMatGrp LEFT JOIN mastitemclass mc ON mc.Id=td.ProductClassId LEFT JOIN mastitemsegment ms ON ms.Id=td.ProductSegmentId WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' union select  msp.SMId,msp.smname,mp.partyname,td.remarks,mc.Name AS ProdClass,ms.Name AS ProdSegment,mi.itemname as ProductGrp,td.VDate as created_date from temp_transdemo td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId left join mastitem mi on  mi.itemid=td.ProductMatGrp LEFT JOIN mastitemclass mc ON mc.Id=td.ProductClassId LEFT JOIN mastitemsegment ms ON ms.Id=td.ProductSegmentId WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptDemo.DataSource = dt;
                rptDemo.DataBind();
                rptCollection.Visible = false;
                rptFaildvisit.Visible = false;
                rptCompetitor.Visible = false;
                rptTotalOrder.Visible = false;
                btnExport.Visible = false;
                btnexportvisit.Visible = false;
                btncollection.Visible = false;
                btnCompetitor.Visible = false;
                DIVFailedVisit.Visible = false;
                DIVMode.Visible = false;
                
            }
            else if (name.Equals("Non-Productive"))
            {
                lblHeading.InnerText = "Total Non-Productive Detail";
                if (ddlFailedVisit.SelectedValue != "")
                {
                    FailedVisit = "and td.Reasonid in(" + ddlFailedVisit.SelectedValue.ToString() + ")";
                    // FailedVisit = ddlFailedVisit.SelectedValue;

                }
                if (ddlFailedVisit.SelectedValue != "0")
                {
                    sql = "select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from temp_TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid WHERE td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' " + FailedVisit + " and  partydist=0 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "'  " + FailedVisit + " and  partydist=0) a order by created_date,smname desc";
                }
                else
                {
                    //sql = "select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from temp_TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid WHERE td.smid=" + SMId + "  and td.vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and td.vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + To + "'), 106), ' ', '/') and  partydist=0 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   WHERE td.smid=" + SMId + " and td.vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and td.vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + To + "'), 106), ' ', '/') and  partydist=0) a order by smname";

                    sql = "select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from temp_TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid WHERE td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=0 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=0) a order by created_date,smname desc";
                }
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptFaildvisit.DataSource = dt;
                rptFaildvisit.DataBind();
                rptCollection.Visible = false;
                rptCompetitor.Visible = false;
                rptTotalOrder.Visible = false;
                rptDemo.Visible = false;
                btnExport.Visible = false;
                btncollection.Visible = false;
                btndemo.Visible = false;
                btnCompetitor.Visible = false;
                DIVMode.Visible = false;
            }
            else if (name.Equals("Competitor"))
            {
                lblHeading.InnerText = "Total Competitor Detail";
                sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.item,td.qty,td.rate,td.discount,td.remarks,td.VDate as created_date from temp_transcompetitor  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' union select msp.SMId,msp.smname,mp.partyname,td.item,td.qty,td.rate,td.discount,td.remarks,td.VDate as created_date from transcompetitor  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptCompetitor.DataSource = dt;
                rptCompetitor.DataBind();
                rptCollection.Visible = false;
                rptFaildvisit.Visible = false;
                rptTotalOrder.Visible = false;
                rptDemo.Visible = false;
                btnExport.Visible = false;
                btnexportvisit.Visible = false;
                btncollection.Visible = false;
                btndemo.Visible = false;
                DIVFailedVisit.Visible = false;
                DIVMode.Visible = false;

            }
            else if (name.Equals("Collection"))
            {
                lblHeading.InnerText = "Total Collection Detail";
                //if (!string.IsNullOrEmpty(ddlmode.SelectedValue)|| (ddlmode.SelectedValue!="0"))
                ////if (ddlmode.SelectedValue != "0")
                //{
                //    mode = ddlmode.SelectedValue;
                //}
                if (ddlmode.SelectedValue != "0")
                {
                    sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from temp_transcollection  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.mode='" + mode + "' and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' union select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from transcollection td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.mode='" + mode + "' and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                }
                else
                {
                    sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from temp_transcollection  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' union select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from transcollection td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  WHERE td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                }
                
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptCollection.DataSource = dt;
                rptCollection.DataBind();
                rptFaildvisit.Visible = false;
                rptCompetitor.Visible = false;
                rptTotalOrder.Visible = false;
                rptDemo.Visible = false;
                btnExport.Visible = false;
                btnexportvisit.Visible = false;
                btndemo.Visible = false;
                btnCompetitor.Visible = false;
                DIVFailedVisit.Visible = false;
            }

            if (Convert.ToDateTime(FromDate.Text) > Convert.ToDateTime(ToDate.Text))
            {
                rptCollection.DataSource = new DataTable();
                rptCollection.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }

        }
        private void fillMode()
        {
            try
            {
                string strmode = "select distinct mode from distributercollection";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strmode);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlmode.DataSource = dt;
                        ddlmode.DataTextField = "mode";
                        ddlmode.DataValueField = "mode";
                        ddlmode.DataBind();
                    }
                }
                ddlmode.Items.Insert(0, new ListItem("-- All --", "0"));
                ddlmode.Items.Insert(1, new ListItem("Draft", "1"));
                ddlmode.Items.Insert(2, new ListItem("Cheque", "2"));

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private void fillFailedVisit()
        {
            try
            {
                string strvisit = " select Fvid, FVName from MastFailedVisitRemark";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strvisit);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        ddlFailedVisit.DataSource = dt;
                        ddlFailedVisit.DataTextField = "FVName";
                        ddlFailedVisit.DataValueField = "Fvid";
                        ddlFailedVisit.DataBind();
                    }
                }
                ddlFailedVisit.Items.Insert(0, new ListItem("-- All --", "0"));

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void FailedVisit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);

                this.fillAllRecord();

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }

        }
        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

        protected void ToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);

                this.fillAllRecord();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TotalOrderDetails.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("itemname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("amount", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


            foreach (RepeaterItem item in rptTotalOrder.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lbldate = item.FindControl("lbldate") as Label;
                dr["created_date"] = lbldate.Text.ToString();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["smname"] = lblsmname.Text.ToString();
                Label lblpartyname = item.FindControl("lblpartyname") as Label;
                dr["partyname"] = lblpartyname.Text.ToString();
                Label lblitem = item.FindControl("lblitem") as Label;
                dr["itemname"] = lblitem.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["qty"] = lblqty.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["amount"] = lblamount.Text.ToString();
                //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                // dr["TotalOrder"] = lbltotalorder.Text.ToString();


                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "created_date desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[6];
            
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
                                if (k == 4 || k == 5)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
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
                                if (k == 4 || k == 5)
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
                            if (k == 0)
                            {
                                // sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                                //Total For Columns
                                if (k == 4 || k == 5)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 1; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        // totalStr += "0" + ',';
                    }

                    else
                    {
                        if (x == 1)
                        {
                            totalStr += "" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }

                    }
                }
                sb.Append(",,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TotalOrderDetails.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("itemname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("amount", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


            foreach (RepeaterItem item in rptTotalOrder.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lbldate = item.FindControl("lbldate") as Label;
                dr["created_date"] = lbldate.Text.ToString();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["smname"] = lblsmname.Text.ToString();
                Label lblpartyname = item.FindControl("lblpartyname") as Label;
                dr["partyname"] = lblpartyname.Text.ToString();
                Label lblitem = item.FindControl("lblitem") as Label;
                dr["itemname"] = lblitem.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["qty"] = lblqty.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["amount"] = lblamount.Text.ToString();
                //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                // dr["TotalOrder"] = lbltotalorder.Text.ToString();


                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
           // dv.Sort = "created_date desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[6];

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
                            if (k == 4 || k == 5)
                            {
                                if (udtNew.Rows[j][k].ToString() != "")
                                {
                                    totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                }
                            }
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
                            if (k == 4 || k == 5)
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
                        if (k == 0)
                        {
                            // sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            //Total For Columns
                            if (k == 4 || k == 5)
                            {
                                if (udtNew.Rows[j][k].ToString() != "")
                                {
                                    totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                }
                            }
                            //End
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            string totalStr = string.Empty;
            for (int x = 1; x < totalVal.Count(); x++)
            {
                if (totalVal[x] == 0)
                {
                    // totalStr += "0" + ',';
                }

                else
                {
                    if (x == 1)
                    {
                        totalStr += "" + ',';
                    }
                    else
                    {
                        totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                    }

                }
            }
            sb.Append(",,,Total," + totalStr);
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();
        }
        
         protected void btnexportvisit_Click(object sender, EventArgs e)
         {
                 Response.Clear();
                 Response.ContentType = "text/csv";
                 Response.AddHeader("content-disposition", "attachment;filename=TotalNon-ProductiveDetail.csv");
                 string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Reason".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

                 StringBuilder sb = new StringBuilder();
                 sb.Append(headertext);
                 sb.AppendLine(System.Environment.NewLine);
                 string dataText = string.Empty;

                 DataTable dtParams = new DataTable();
                 dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("Reason", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));

                 //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


                 foreach (RepeaterItem item in rptFaildvisit.Items)
                 {
                     DataRow dr = dtParams.NewRow();
                     Label lbldte = item.FindControl("lbldte") as Label;
                     dr["created_date"] = lbldte.Text.ToString();
                     Label lblsname = item.FindControl("lblsname") as Label;
                     dr["smname"] = lblsname.Text.ToString();
                     Label lblptyname = item.FindControl("lblptyname") as Label;
                     dr["partyname"] = lblptyname.Text.ToString();
                     Label lblreason = item.FindControl("lblreason") as Label;
                     dr["Reason"] = lblreason.Text.ToString();
                     Label lblremark = item.FindControl("lblremark") as Label;
                     dr["remarks"] = lblremark.Text.ToString();

                     //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                     // dr["TotalOrder"] = lbltotalorder.Text.ToString();


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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                 Response.AddHeader("content-disposition", "attachment;filename=TotalNon-ProductiveDetail.csv");
                 Response.Write(sb.ToString());
                 Response.End();
             }
         protected void btncollection_Click(object sender, EventArgs e)
         {
              Response.Clear();
                 Response.ContentType = "text/csv";
                 Response.AddHeader("content-disposition", "attachment;filename=TotalCollectionDetail.csv");
                 string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Mode".TrimStart('"').TrimEnd('"') + "," + "Cheque No.".TrimStart('"').TrimEnd('"') + "," + "Cheque Date".TrimStart('"').TrimEnd('"') + "," + "Bank".TrimStart('"').TrimEnd('"') + "," + "Branch".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + "," + "Payment Date".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

                 StringBuilder sb = new StringBuilder();
                 sb.Append(headertext);
                 sb.AppendLine(System.Environment.NewLine);
                 string dataText = string.Empty;

                 DataTable dtParams = new DataTable();
                 dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("mode", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("Cheque_DDNo", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("Cheque_DD_Date", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("bank", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("Branch", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("amount", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("PaymentDate", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));

                 //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


                 foreach (RepeaterItem item in rptCollection.Items)
                 {
                     DataRow dr = dtParams.NewRow();
                     Label lbldate = item.FindControl("lbldate") as Label;
                     dr["created_date"] = lbldate.Text.ToString();
                     Label lblsmname = item.FindControl("lblsmname") as Label;
                     dr["smname"] = lblsmname.Text.ToString();
                     Label lblpartyname = item.FindControl("lblpartyname") as Label;
                     dr["partyname"] = lblpartyname.Text.ToString();
                     Label lblmode = item.FindControl("lblmode") as Label;
                     dr["mode"] = lblmode.Text.ToString();
                     Label lblcheckdd = item.FindControl("lblcheckdd") as Label;
                     dr["Cheque_DDNo"] = lblcheckdd.Text.ToString();
                     Label lblcheckdate = item.FindControl("lblcheckdate") as Label;
                     dr["Cheque_DD_Date"] = lblcheckdate.Text.ToString();
                     Label lblbank = item.FindControl("lblbank") as Label;
                     dr["bank"] = lblbank.Text.ToString();
                     Label lblbranch = item.FindControl("lblbranch") as Label;
                     dr["Branch"] = lblbranch.Text.ToString();
                     Label lblamount = item.FindControl("lblamount") as Label;
                     dr["amount"] = lblamount.Text.ToString();
                     Label lblpaymentdate = item.FindControl("lblpaymentdate") as Label;
                     dr["PaymentDate"] = lblpaymentdate.Text.ToString();
                     Label lblremark = item.FindControl("lblremark") as Label;
                     dr["remarks"] = lblremark.Text.ToString();

                     //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                     // dr["TotalOrder"] = lbltotalorder.Text.ToString();


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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                 Response.AddHeader("content-disposition", "attachment;filename=TotalCollectionDetail.csv");
                 Response.Write(sb.ToString());
                 Response.End();
             }
         protected void btndemo_Click(object sender, EventArgs e)
         {
                 Response.Clear();
                 Response.ContentType = "text/csv";
                 Response.AddHeader("content-disposition", "attachment;filename=DemoDetails.csv");
                 string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Segment".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

                 StringBuilder sb = new StringBuilder();
                 sb.Append(headertext);
                 sb.AppendLine(System.Environment.NewLine);
                 string dataText = string.Empty;

                 DataTable dtParams = new DataTable();
                 dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("ProdClass", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("ProdSegment", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("ProductGrp", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));

                 //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


                 foreach (RepeaterItem item in rptDemo.Items)
                 {
                     DataRow dr = dtParams.NewRow();
                     Label lbldate = item.FindControl("lbldate") as Label;
                     dr["created_date"] = lbldate.Text.ToString();
                     Label lblsmname = item.FindControl("lblsmname") as Label;
                     dr["smname"] = lblsmname.Text.ToString();
                     Label lblpartyname = item.FindControl("lblpartyname") as Label;
                     dr["partyname"] = lblpartyname.Text.ToString();
                     Label lblprodclass = item.FindControl("lblprodclass") as Label;
                     dr["ProdClass"] = lblprodclass.Text.ToString();
                     Label lblsegment = item.FindControl("lblsegment") as Label;
                     dr["ProdSegment"] = lblsegment.Text.ToString();
                     Label lblgrp = item.FindControl("lblgrp") as Label;
                     dr["ProductGrp"] = lblgrp.Text.ToString();
                     Label lblremark = item.FindControl("lblremark") as Label;
                     dr["remarks"] = lblremark.Text.ToString();

                     //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                     // dr["TotalOrder"] = lbltotalorder.Text.ToString();


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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                 Response.AddHeader("content-disposition", "attachment;filename=DemoDetails.csv");
                 Response.Write(sb.ToString());
                 Response.End();
             }
         protected void btnCompetitor_Click(object sender, EventArgs e)
         {
              Response.Clear();
                 Response.ContentType = "text/csv";
                 Response.AddHeader("content-disposition", "attachment;filename=CompetitorDetails.csv");
                 string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Std. Packing".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Discount %".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

                 StringBuilder sb = new StringBuilder();
                 sb.Append(headertext);
                 sb.AppendLine(System.Environment.NewLine);
                 string dataText = string.Empty;

                 DataTable dtParams = new DataTable();
                 dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("item", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("qty", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("discount", typeof(String)));
                 dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));

                 //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


                 foreach (RepeaterItem item in rptCompetitor.Items)
                 {
                     DataRow dr = dtParams.NewRow();
                     Label lbldate = item.FindControl("lbldate") as Label;
                     dr["created_date"] = lbldate.Text.ToString();
                     Label lblsmname = item.FindControl("lblsmname") as Label;
                     dr["smname"] = lblsmname.Text.ToString();
                     Label lblpartyname = item.FindControl("lblpartyname") as Label;
                     dr["partyname"] = lblpartyname.Text.ToString();
                     Label lblitem = item.FindControl("lblitem") as Label;
                     dr["item"] = lblitem.Text.ToString();
                     Label lblqty = item.FindControl("lblqty") as Label;
                     dr["qty"] = lblqty.Text.ToString();
                     Label lblrate = item.FindControl("lblrate") as Label;
                     dr["rate"] = lblrate.Text.ToString();
                     Label lbldiscount = item.FindControl("lbldiscount") as Label;
                     dr["discount"] = lbldiscount.Text.ToString();
                     Label lblremark = item.FindControl("lblremark") as Label;
                     dr["remarks"] = lblremark.Text.ToString();

                     //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                     // dr["TotalOrder"] = lbltotalorder.Text.ToString();


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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                                 // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                 Response.AddHeader("content-disposition", "attachment;filename=CompetitorDetails.csv");
                 Response.Write(sb.ToString());
                 Response.End();
             }
         }
}


        

    

                
                
    
