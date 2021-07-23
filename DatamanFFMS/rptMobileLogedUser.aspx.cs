using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class rptMobileLogedUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                BindDDLMonth();
                ddlMonth.SelectedValue = System.DateTime.Now.Month.ToString();
            }
        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    ddlMonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));                    
                }
                //for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                //{
                //    yearDDL.Items.Add(new ListItem(i.ToString()));
                //    ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                //    ddlyearExpenses.Items.Add(new ListItem(i.ToString()));
                //    ddlyearRemark.Items.Add(new ListItem(i.ToString()));
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";      
    
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            DateTime first = new DateTime(System.DateTime.Now.Year, month, 1);
            DateTime last = first.AddMonths(1).AddSeconds(-1);
            string firstdate = first.ToString("yyyy-MM-dd");
            string lastdate = last.ToString("yyyy-MM-dd");
            
            if (ddlLoged.SelectedValue == "1" && ddlUser.SelectedValue == "1")
            {
                str = @"SELECT mp.PartyName as name,mp.mobile,ml.DistId as id,count(*) AS logincount FROM LogUserLogin ml LEFT JOIN MastParty mp ON ml.UserId=mp.UserId WHERE UsrLoginDateTime BETWEEN '" + firstdate + "' AND '" + lastdate + "' AND ml.DistId IS NOT NULL and distid  in (select partyid from mastparty where partydist=1) GROUP BY mp.PartyName,mp.mobile,ml.DistId ORDER BY logincount DESC";
            }

            else if (ddlLoged.SelectedValue == "2" && ddlUser.SelectedValue == "1")
            {
                str = @"SELECT mp.PartyName as name,mp.mobile,mp.partyid as id,0 AS logincount FROM MastParty mp WHERE mp.partyid not in (SELECT distinct ml.DistId FROM LogUserLogin ml WHERE UsrLoginDateTime BETWEEN '" + firstdate + "' AND '" + lastdate + "' AND ml.DistId IS NOT NULL) GROUP BY mp.PartyName,mp.mobile,mp.partyid order by mp.PartyName";
            }

            else if (ddlLoged.SelectedValue == "1" && ddlUser.SelectedValue == "2")
            {
                str = @"SELECT ms.SMName as name,ms.Mobile,ml.smid as id,count(*) AS logincount FROM LogUserLogin ml LEFT JOIN Mastsalesrep ms ON ml.smid=ms.smid WHERE UsrLoginDateTime BETWEEN '" + firstdate + "' AND '" + lastdate + "' AND ml.smid IS NOT NULL GROUP BY ms.SMName,ms.Mobile,ml.smid ORDER BY logincount DESC ";
            }

            else if (ddlLoged.SelectedValue == "2" && ddlUser.SelectedValue == "2")
            {
                str = @" SELECT ms.SMName as name,ms.Mobile,ms.smid as id,0 AS logincount FROM Mastsalesrep ms WHERE ms.smid not in (SELECT distinct ml.smid FROM LogUserLogin ml WHERE UsrLoginDateTime BETWEEN '" + firstdate + "' AND '" + lastdate + "' AND ml.smid IS NOT NULL) and ms.smname <> '.' GROUP BY ms.SMName,ms.Mobile,ms.smid order by ms.SMName";
            }           

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt.Rows.Count > 0)
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = dt;
                rpt.DataBind();
                rpt.Visible = true;               
            }
            else
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = dt;
                rpt.DataBind();
            }
        }

        private void ClearControls()
        {
            rpt.DataSource = null;
            rpt.DataBind();          
            rpt.Visible = false;
        }     


        protected void gvPartyData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    HiddenField areaID = (e.Row.FindControl("areaIdHiddenField") as HiddenField);
            //    var Qry = "select * from mastLink where PrimCode =" + Convert.ToInt32(ddlprimecode.SelectedValue) + " and ECode='SA'";
            //    DataTable dtini = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            //    CheckBox chk = (e.Row.FindControl("chkItem") as CheckBox);
            //    if (dtini.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dtini.Rows.Count; i++)
            //        {
            //            if (dtini.Rows[i]["LinkCode"].ToString() == areaID.Value)
            //            {
            //                chk.Checked = true;
            //            }
            //        }
            //    }
            //}
   
        }    

    }
}