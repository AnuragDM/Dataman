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
    public partial class DistOutstandingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                BindSalePersonDDl();
            }
        }
        private void BindSalePersonDDl()
        {
            try
            {
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "SMName<>.";
                if (dv.ToTable().Rows.Count > 0)
                {
                    ListBox1.DataSource = dv.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "";
            string smIDStr1 = "";
            //         string message = "";
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');


            string outrptquery = @"select TransDistributerLedger.DistId, partyname, (sum(AmtDr)-sum(AmtCr)) [Balance] 
                from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId
                left join MastArea on MastArea.AreaId=MastParty.CityId where mastparty.PartyDist=1 and MastParty.CityId in (select AreaId from MastArea where AreaId in (select UnderId from MastArea where AreaId in (SELECT linkcode FROM   mastlink WHERE  primtable = 'SALESPERSON' AND linktable = 'AREA' AND primcode in (" + smIDStr1 + ")))) group by DistId,PartyName having (sum(AmtDr)-sum(AmtCr))>0 order by Partyname, Balance desc";
            DataTable outrptdt = DbConnectionDAL.GetDataTable(CommandType.Text, outrptquery);
            if(outrptdt.Rows.Count>0)
            {
                rptDistOutRep.DataSource = outrptdt;
                rptDistOutRep.DataBind();
            }
            else
            {
                rptDistOutRep.DataSource = outrptdt;
                rptDistOutRep.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistOutstandingReport.aspx");
        }

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            GetDetailLedgerData(Convert.ToInt32(hdnVisItCode.Value));
        }
        private void GetDetailLedgerData(int distId)
        {
            try
            {
                string str = " select PartyName from MastParty where partyid= '" + distId + "'";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt1.Rows.Count > 0)
                    lblDist.Text = dt1.Rows[0]["PartyName"].ToString();
                DataTable dt = Settings.DetailDistLedger(distId);
                if (dt.Rows.Count > 0)
                {
                    detailDistOutDiv.Style.Add("display","block");
                    rptDistLedger.DataSource = dt;
                    rptDistLedger.DataBind();
                }
                else
                {
                    detailDistOutDiv.Style.Add("display", "block");
                    rptDistLedger.DataSource = dt;
                    rptDistLedger.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}