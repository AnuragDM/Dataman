using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace AstralFFMS
{
    public partial class DailySalesReportL1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                BindSalePersonDDl();
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "";
                string smIDStr1 = "", Qrychk = "", Query = "", QryDemo = "", QryFv = "", QryOrder = "", QryMain = "", beat = "";
                //         string message = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                if (smIDStr1 != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (

                select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransOrder  om
                 inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where smid in (" + smIDStr1 + ") and VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' group by b.AreaName,b.AreaId,p.AreaId union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where SMId in (" + smIDStr1 + ") and VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' group by b.AreaName,b.AreaId,p.AreaId union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where SMId in (" + smIDStr1 + ") and VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId union all select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from MastParty p inner join MastArea b on b.AreaId=p.AreaId where p.UserId in (" + smIDStr1 + ") and p.Created_Date between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' group by  b.AreaName,b.AreaId,b.AreaName,p.AreaId )a Group by a.City_Name,a.Beat_Id,a.Description Order by a.Description";

                    DataTable dtbeats = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtbeats.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtbeats.Rows.Count; i++)
                        {
                            beat += dtbeats.Rows[i]["Beat_Id"].ToString() + ",";
                        }
                        beat = beat.TrimStart(',').TrimEnd(',');
                        //     Qrychk = " d.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and d.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                        if (orderCheckBox.Checked)
                        {
                            if (QryMain.Length != 0)
                            {
                                QryMain = @" union all select convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,
                    case when (p.PartyId=pn.PartyId and p.Created_Date =pn.VDate ) then p.PartyName + '##' else p.PartyName end as Party,
                    '' as Item,'' AS Qty, '' AS Rate,os.OrderAmount as Value,
                    os.Remarks,'' as IsPartyConverted,'' as AvailabilityShop,'' as CompleteAppDetail,
                    '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,
                    cp1.SMName as L3Name from TransOrder os LEFT Join Mastparty p on 
                    p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                    left join TransPartyNewProduct pn on pn.PartyId=os.PartyId and pn.VDate=os.VDate
                    left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp
                     on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smIDStr1 + ") and os.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) group by p.PartyName, os.VDate,pn.ItemId,pn.VDate,os.PartyId,pn.PartyId, os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.OrderAmount";
                            }
                            else
                            {
                                QryMain = @"select convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,
                    case when (p.PartyId=pn.PartyId and p.Created_Date =pn.VDate ) then p.PartyName + '##' else p.PartyName end as Party,
                    '' as Item,'' AS Qty, '' AS Rate,os.OrderAmount as Value,
                    os.Remarks,'' as IsPartyConverted,'' as AvailabilityShop,'' as CompleteAppDetail,
                    '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,
                    cp1.SMName as L3Name from TransOrder os LEFT Join Mastparty p on 
                    p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                    left join TransPartyNewProduct pn on pn.PartyId=os.PartyId and pn.VDate=os.VDate
                    left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp
                     on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smIDStr1 + ") and os.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) group by p.PartyName, os.VDate,pn.ItemId,pn.VDate,os.PartyId,pn.PartyId, os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.OrderAmount";
                            }
                        }

                        if (demoCheckBox.Checked)
                        {
                            if (QryMain.Length != 0)
                            {
                                QryMain = QryMain + @"union all select CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName as Party,
                    i.ItemName as [Item],0 as Qty,0 as Rate,0 as Value,d.Remarks,case IsPartyConverted 
                    when 0 then 'No' else 'Yes' end as IsPartyConverted,d.AvailablityShop as AvailabilityShop,d.CompleteAppDetail
                    as CompleteAppDetail,c.Item as CompItem,c.Qty as compQty,c.rate as ComRate,
                    b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name from TransDemo d inner join MastItem i on i.ItemId=d.ItemId 
                    inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join TransCompetitor c ON
                     c.PartyId=d.PartyId and c.VDate=d.VDate left join TransVisit vl1 on vl1.SMId=d.SMId AND
                     d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 
                     on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                     (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and d.SMId in (" + smIDStr1 + ")";
                            }
                            else
                            {
                                QryMain = @"select CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName as Party,
                    i.ItemName as [Item],0 as Qty,0 as Rate,0 as Value,d.Remarks,case IsPartyConverted 
                    when 0 then 'No' else 'Yes' end as IsPartyConverted,d.AvailablityShop as AvailabilityShop,d.CompleteAppDetail
                    as CompleteAppDetail,c.Item as CompItem,c.Qty as compQty,c.rate as ComRate,
                    b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name from TransDemo d inner join MastItem i on i.ItemId=d.ItemId 
                    inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join TransCompetitor c ON
                     c.PartyId=d.PartyId and c.VDate=d.VDate left join TransVisit vl1 on vl1.SMId=d.SMId AND
                     d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 
                     on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                     (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' and d.SMId in (" + smIDStr1 + ")";
                            }
                        }

                        if (fvCheckBox.Checked)
                        {
                            if (QryMain.Length != 0)
                            {
                                QryMain = QryMain + @"union all select CONVERT (varchar,fv.VDate,106) as VisitDate,'FailedVisit' as Stype,p.PartyId,p.PartyName as Party,
                    '' as Item,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as IsPartyConverted,
                    '' as AvailabilityShop,'' as CompleteAppDetail,'' as CompItem,0 as CompQty,0 as ComRate,
                    b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name from TransFailedVisit fv inner join MastParty p ON
                    p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                    and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON
                     cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                     (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and fv.SMId in (" + smIDStr1 + ") and fv.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' ";
                            }
                            else
                            {
                                QryMain = @"select CONVERT (varchar,fv.VDate,106) as VisitDate,'FailedVisit' as Stype,p.PartyId,p.PartyName as Party,
                    '' as Item,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as IsPartyConverted,
                    '' as AvailabilityShop,'' as CompleteAppDetail,'' as CompItem,0 as CompQty,0 as ComRate,
                    b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name from TransFailedVisit fv inner join MastParty p ON
                    p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                    and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON
                     cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                     (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and fv.SMId in (" + smIDStr1 + ") and fv.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' ";
                            }
                        }
                      
                        if (QryMain != "")
                        {
                            Query = @"select *,'' as Match from (select * from (" + QryMain + ") a )b Order by b.visitDate,b.partyid ";
                        }
                        DataTable dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        if (dtLocTraRep.Rows.Count > 0)
                        {
                            rptmain.Style.Add("display", "block");
                            distreportrpt.DataSource = dtLocTraRep;
                            distreportrpt.DataBind();
                        }
                        else
                        {
                            rptmain.Style.Add("display", "block");
                            distreportrpt.DataSource = dtLocTraRep;
                            distreportrpt.DataBind();
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Beats are present');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person');", true);
                    distreportrpt.DataSource = null;
                    distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DailySalesReportL1.aspx");
        }
    }
}