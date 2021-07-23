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
    public partial class DSRPendingList : System.Web.UI.Page
    {
        string roleType = "", total = "";
        DataTable dtEmployee = null;
        string sql = "";
        string secondarySql = "";
        string PrimarySql = "";
        string UnApprovedSql = "";
        string name = "";
        string Date = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            if (!IsPostBack)
            {
              //  FromDate.Text = Request.QueryString["Date"];
                fillUser();
                //  this.BindWithSalesPersonDDl(Convert.ToInt32(Settings.Instance.SMID));
                this.fillAllRecord();
            }
        }
        private void fillUser()
        {
            DataTable dt = null;
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            //Date = FromDate.Text;
           
            if (roleType.Equals("Admin"))
            {

                sql = "select  distinct(t.smid),msp.smname from TransVisit t left join mastsalesrep msp on msp.smid=t.smid left join mastrole mr on mr.roleid=msp.roleid  where     appstatus is null  and  lock=1 order by msp.smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

            }
            else
            {
                sql = "select distinct(t.smid),msp.smname from TransVisit  t left join mastsalesrep msp on msp.smid=t.smid left join mastrole mr on mr.roleid=msp.roleid  where  appstatus is null  and  lock=1 and t.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + "))and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " ))) order by msp.smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    lstUndeUser.DataSource = dt;
                    lstUndeUser.DataTextField = "smname";
                    lstUndeUser.DataValueField = "SMId";
                    lstUndeUser.DataBind();
                }
            }
            lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            string filter = "";
            if (lstUndeUser.SelectedValue!="0")
            {
                filter = "t.smid=" + lstUndeUser.SelectedValue + " and ";
            }
            if (roleType.Equals("Admin"))
            {
                    lblHeading.InnerText = "DSR Pending List";
                    sql = "select  msp.smname,t.vdate  ,t.smid  ,mr.rolename from TransVisit t left join mastsalesrep msp on msp.smid=t.smid left join mastrole mr on mr.roleid=msp.roleid  where  " + filter + "   appstatus is null and  lock=1  order by msp.smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    rptDsrlist.DataSource = dt;
                    rptDsrlist.DataBind();
                   
            }
            else
            {
                    lblHeading.InnerText = "DSR Pending List";
                    sql = "select msp.smname,t.vdate ,t.smid  ,mr.rolename from TransVisit  t left join mastsalesrep msp on msp.smid=t.smid left join mastrole mr on mr.roleid=msp.roleid  where  " + filter + "   appstatus is null  and  lock=1 and t.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + "))and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " ))) order by msp.smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    rptDsrlist.DataSource = dt;
                    rptDsrlist.DataBind();
                }

        }
        //protected void FromDate_TextChanged(object sender, EventArgs e)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
        //    this.fillAllRecord();
        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        //}
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DSRPendingList.csv");
                string headertext = "Sale Person".TrimStart('"').TrimEnd('"') + "," + "Role".TrimStart('"').TrimEnd('"') + "," + "DSR Date".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                dtParams.Columns.Add(new DataColumn("rolename", typeof(String)));
                dtParams.Columns.Add(new DataColumn("vdate", typeof(String)));
               
                foreach (RepeaterItem item in rptDsrlist.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lblsmname = item.FindControl("lblsmname") as Label;
                    dr["smname"] = lblsmname.Text.ToString();
                    Label lblrolename = item.FindControl("lblrolename") as Label;
                    dr["rolename"] = lblrolename.Text.ToString();
                    Label lblvdate = item.FindControl("lblvdate") as Label;
                    dr["vdate"] = lblvdate.Text.ToString();
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
                Response.AddHeader("content-disposition", "attachment;filename=DSRPendingList.csv");
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
        protected void lstUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
    }
}