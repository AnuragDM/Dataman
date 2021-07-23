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
    public partial class PriceListMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                //    BindPriceList();
                BindDateDDl();
            }
        }

        private void BindDateDDl()
        {
            try
            {
                string priceListDAteQry = @"select distinct Convert(DATE,WefDate) as WefDate from PriceList order by Convert(DATE,WefDate) desc";
                DataTable dtPriceListDate = DbConnectionDAL.GetDataTable(CommandType.Text, priceListDAteQry);
                if (dtPriceListDate.Rows.Count > 0)
                {
                    ddlDate.DataSource = dtPriceListDate;
                    ddlDate.DataTextField = "WefDate";
                    ddlDate.DataValueField = "WefDate";
                    ddlDate.DataBind();
                }
                ddlDate.Items.Insert(0, new ListItem("--Please select--"));

                dtPriceListDate.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindPriceList(string wefDate)
        {
            try
            {
                string priceListQry = @"select *,mi.ItemName from PriceList pl left join MastItem mi on pl.ItemId=mi.ItemId where WefDate='" + Settings.dateformat(wefDate) + "' order by wefDate desc";
                DataTable dtPriceList = DbConnectionDAL.GetDataTable(CommandType.Text, priceListQry);
                if (dtPriceList.Rows.Count > 0)
                {
                    rptmain.Style.Add("display","block");
                    pricelistrpt.DataSource = dtPriceList;
                    pricelistrpt.DataBind();
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    pricelistrpt.DataSource = dtPriceList;
                    pricelistrpt.DataBind();
                }

                dtPriceList.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDate.SelectedIndex != 0)
                {
                    BindPriceList(ddlDate.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}