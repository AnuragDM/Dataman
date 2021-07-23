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
    public partial class DashboardPrimaryDetail : System.Web.UI.Page
    {
        string roleType = "", sql = "";
        string name = "", Date = "", Partytype = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnexportvisit);
            scriptManager.RegisterPostBackControl(this.btncollection);
            scriptManager.RegisterPostBackControl(this.btndiscussion);
            scriptManager.RegisterPostBackControl(this.btnprodutive);
            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                fillMode();
                fillFailedVisit();
                ToDate.Text = Request.QueryString["Date"];

                this.fillAllRecord();
            }
        }

        public void fillAllRecord()
        {
            DataTable dt = null;
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            Partytype = Request.QueryString["Type"];
            int SMId = Convert.ToInt32(Request.QueryString["SMId"]);
            Date = FromDate.Text;
            string To = ToDate.Text;
            string mode = "";
            string FailedVisit = "";

            if (ddlmode.SelectedValue != "0")
            {
                mode = ddlmode.SelectedValue;
            }
            if (name.Equals("Discussion"))
            {
                lblHeading.InnerText = "Total Discussion Detail";
                if (Partytype == "P")
                {


                    sql = @"select * from  (select msp.SMId,msp.smname,mp.partyname,td.remarkdist,td.VDate as created_date
from TransVisitDist td left join mastparty mp on  mp.partyid=td.DistId left join MastSalesRep msp on msp.smid=td.SMId 
WHERE Isnull(MP.PartyType,0)=0 and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' AND td.Type IS null union select msp.SMId,msp.smname,mp.partyname,td.remarkdist,td.VDate as created_date from Temp_TransVisitDist td left join mastparty mp on  mp.partyid=td.DistId left join MastSalesRep msp on msp.smid=td.SMId WHERE Isnull(MP.PartyType,0)=0 and  td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' AND td.Type IS null) a order by created_date,smname desc";

                }
                else
                {
                    sql = @"select * from  (select msp.SMId,msp.smname,mp.partyname,td.remarkdist,td.VDate as created_date
from TransVisitDist td left join mastparty mp on  mp.partyid=td.DistId left join MastSalesRep msp on msp.smid=td.SMId  
LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL'  and  td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' AND td.Type IS null union select msp.SMId,msp.smname,mp.partyname,td.remarkdist,td.VDate as created_date from Temp_TransVisitDist td left join mastparty mp on  mp.partyid=td.DistId left join MastSalesRep msp on msp.smid=td.SMId LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType    where PT.PartyTypeName='INSTITUTIONAL'  and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' AND td.Type IS null) a order by created_date,smname desc";

                }
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (ddlmode.SelectedValue != "0")
                //        {
                //            var result = dt.Select("mode=" + ddlmode.SelectedValue + "");
                //            dt = result.CopyToDataTable();
                //        }
                //    }
                //}
                rptDiscussion.DataSource = dt;
                rptDiscussion.DataBind();
                rptCollection.Visible = false;
                rptFaildvisit.Visible = false;
                rptProductive.Visible = false;
                btncollection.Visible = false;
                btnexportvisit.Visible = false;
                DIVMode.Visible = false;
                DIVFailedVisit.Visible = false;

                btnprodutive.Visible = false;


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
                    if (Partytype == "P")
                    {

                        sql = @"select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks
                        ,mfr.fvname as Reason,td.VDate as created_date from temp_TransFailedVisit  td 
                        left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId 
                        left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid 
                       WHERE Isnull(MP.PartyType,0)=0 and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' " + FailedVisit + " and  partydist=1 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   WHERE Isnull(MP.PartyType,0)=0 and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' " + FailedVisit + " and  partydist=1) a order by created_date,smname desc";
                    }
                    else
                    {
                        sql = @"select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks
                        ,mfr.fvname as Reason,td.VDate as created_date from temp_TransFailedVisit  td 
                        left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId 
                        left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL'  and  td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' " + FailedVisit + " and  partydist=1 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   where PT.PartyTypeName='INSTITUTIONAL'  and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' " + FailedVisit + " and  partydist=1) a order by created_date,smname desc";
                    }
                }
                else
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason
,td.VDate as created_date from temp_TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid 
left join MastSalesRep msp on msp.smid=td.SMId left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid 
 WHERE Isnull(MP.PartyType,0)=0 and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=1 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   WHERE Isnull(MP.PartyType,0)=0 and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=1) a order by created_date,smname desc";
                    }
                    else
                    {
                        sql = @"select * from   (select msp.SMId, msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason
,td.VDate as created_date from temp_TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid 
left join MastSalesRep msp on msp.smid=td.SMId left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL'  and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=1 union  select msp.SMId,msp.smname,mp.partyname,td.remarks,mfr.fvname as Reason,td.VDate as created_date from TransFailedVisit  td left join mastparty mp on  mp.partyid=td.partyid left join MastSalesRep msp on msp.smid=td.SMId  left join mastfailedvisitremark mfr on mfr.fvid=td.reasonid   LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType     where PT.PartyTypeName='INSTITUTIONAL' and td.smid=" + SMId + " and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "' and  partydist=1) a order by created_date,smname desc";
                    }
                }
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptFaildvisit.DataSource = dt;
                rptFaildvisit.DataBind();
                rptCollection.Visible = false;
                rptDiscussion.Visible = false;
                rptProductive.Visible = false;
                btncollection.Visible = false;
                btndiscussion.Visible = false;
                btnprodutive.Visible = false;
                DIVMode.Visible = false;
            }
            else if (name.Equals("Collection"))
            {
                lblHeading.InnerText = "Total Distributor Collection Detail";
                if (ddlmode.SelectedValue != "0")
                {
                    if (Partytype == "P")
                    {
                        sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from DistributerCollection td left join mastparty mp on mp.partyid=td.distid left join MastSalesRep msp on msp.smid=td.SMId  WHERE Isnull(MP.PartyType,0)=0  and td.mode='" + mode + "' and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                    }
                    else
                    {
                        sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from DistributerCollection td left join mastparty mp on mp.partyid=td.distid left join MastSalesRep msp on msp.smid=td.SMId  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   where PT.PartyTypeName='INSTITUTIONAL' and td.mode='" + mode + "' and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                    }
                }
                else
                {
                    if (Partytype == "P")
                    {
                        sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from DistributerCollection td left join mastparty mp on mp.partyid=td.distid left join MastSalesRep msp on msp.smid=td.SMId  WHERE Isnull(MP.PartyType,0)=0  and td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                    }
                    else
                    {
                        sql = "select * from   (select msp.SMId,msp.smname,mp.partyname,td.mode,td.Cheque_DDNo,Cheque_DD_Date= case when Cheque_DD_Date='1900-01-01 00:00:00.000' then null else Cheque_DD_Date end,td.bank,td.Branch,td.amount,td.PaymentDate,td.remarks,td.VDate as created_date from DistributerCollection td left join mastparty mp on mp.partyid=td.distid left join MastSalesRep msp on msp.smid=td.SMId  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   where PT.PartyTypeName='INSTITUTIONAL' and  td.smid=" + SMId + "  and CAST(td.VDate as datetime) >='" + Date + "' and CAST(td.VDate as datetime) <='" + To + "') a order by created_date,smname desc";
                    }
                     }
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptCollection.DataSource = dt;
                rptCollection.DataBind();
                rptFaildvisit.Visible = false;
                rptDiscussion.Visible = false;
                rptProductive.Visible = false;
                btnexportvisit.Visible = false;
                btndiscussion.Visible = false;
                btnprodutive.Visible = false;
                DIVFailedVisit.Visible = false;
            }
            else if (name.Equals("Productive"))
            {
                lblHeading.InnerText = "Total Distributor Productive Detail";
                if (Partytype == "P")
                {
                    sql = "select * from (SELECT msp.SMId,msp.smname,mp.partyname,mi.itemname,t1.qty,t1.VDate as created_date FROM TransPurchOrder1 t1 LEFT JOIN transpurchorder t ON t.PODocId=t1.podocid left join mastparty mp on  mp.partyid=t1.distid left join MastSalesRep msp on msp.smid=t.SMId left join mastitem mi on  mi.itemid=t1.itemid  WHERE Isnull(MP.PartyType,0)=0  and t.smid=" + SMId + " and CAST(t1.VDate as datetime) >='" + Date + " 00:00" + "' and CAST(t1.VDate as datetime) <='" + To + " 23:59" + "' ) a order by created_date,smname desc ";
                }
                else
                {
                    sql = "select * from (SELECT msp.SMId,msp.smname,mp.partyname,mi.itemname,t1.qty,t1.VDate as created_date FROM TransPurchOrder1 t1 LEFT JOIN transpurchorder t ON t.PODocId=t1.podocid left join mastparty mp on  mp.partyid=t1.distid left join MastSalesRep msp on msp.smid=t.SMId left join mastitem mi on  mi.itemid=t1.itemid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   where PT.PartyTypeName='INSTITUTIONAL' and   t.smid=" + SMId + " and CAST(t1.VDate as datetime) >='" + Date + " 00:00" + "' and CAST(t1.VDate as datetime) <='" + To + " 23:59" + "' ) a order by created_date,smname desc ";

                }
                     dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                rptProductive.DataSource = dt;
                rptProductive.DataBind();
                rptFaildvisit.Visible = false;
                rptDiscussion.Visible = false;
                rptCollection.Visible = false;
                btncollection.Visible = false;
                btnexportvisit.Visible = false;
                btndiscussion.Visible = false;
                DIVMode.Visible = false;
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

                ddlmode.SelectedValue = "0";
                this.fillAllRecord();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }
        protected void btnexportvisit_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TotalNon-ProductiveDetail.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Reason".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

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
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Mode".TrimStart('"').TrimEnd('"') + "," + "Cheque No.".TrimStart('"').TrimEnd('"') + "," + "Cheque Date".TrimStart('"').TrimEnd('"') + "," + "Bank".TrimStart('"').TrimEnd('"') + "," + "Branch".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + "," + "Payment Date".TrimStart('"').TrimEnd('"') + "," + "Remark".TrimStart('"').TrimEnd('"');

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

        protected void btnproductive_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ProductiveDetail.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("itemname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("qty", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


            foreach (RepeaterItem item in rptProductive.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lbldate = item.FindControl("lbldate") as Label;
                dr["created_date"] = lbldate.Text.ToString();
                Label lblsname = item.FindControl("lblsname") as Label;
                dr["smname"] = lblsname.Text.ToString();
                Label lblptyname = item.FindControl("lblptyname") as Label;
                dr["partyname"] = lblptyname.Text.ToString();
                Label lblitemname = item.FindControl("lblitemname") as Label;
                dr["itemname"] = lblitemname.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["qty"] = lblqty.Text.ToString();

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
            Response.AddHeader("content-disposition", "attachment;filename=ProductiveDetail.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
        protected void btndiscussion_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DiscussionDetail.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Discussion".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("created_date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("partyname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("remarkdist", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


            foreach (RepeaterItem item in rptDiscussion.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lbldate = item.FindControl("lbldate") as Label;
                dr["created_date"] = lbldate.Text.ToString();
                Label lblsname = item.FindControl("lblsname") as Label;
                dr["smname"] = lblsname.Text.ToString();
                Label lblptyname = item.FindControl("lblptyname") as Label;
                dr["partyname"] = lblptyname.Text.ToString();
                Label lblremarkdist = item.FindControl("lblremarkdist") as Label;
                dr["remarkdist"] = lblremarkdist.Text.ToString();

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
            Response.AddHeader("content-disposition", "attachment;filename=DiscussionDetail.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
