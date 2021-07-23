using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Text;
namespace AstralFFMS
{
    public partial class DistributorStockReport : System.Web.UI.Page
    {
          string roleType = "";
          string areaid = "";
          string distid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {

                roleType = Settings.Instance.RoleType;
                txtfmDate.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");

                List<DistStock> distributors = new List<DistStock>();
                distributors.Add(new DistStock());
                distreportrpt.DataSource = distributors;
                distreportrpt.DataBind();


                List<DistStockWise> distributors1 = new List<DistStockWise>();
                distributors1.Add(new DistStockWise());
                distreportrpt1.DataSource = distributors1;
                distreportrpt1.DataBind();

       //     rptmaindis.Visible = false;
                //BindArea();
                if (Request.QueryString["DistId"] != null)
                {
                    distid=Request.QueryString["DistId"].ToString();
                    areaid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from mastparty where PartyId =" + Request.QueryString["DistId"]));
                    ListArea.Enabled=false;
                    BindDistributorDDl(areaid);
                    ListBox1.Enabled = false;
                    
                }
           BindArea();
                //string pageName = Path.GetFileName(Request.Path);

                if (btnexport.Text == "Export")
                {
                    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
                }
            }
         
        }
            public class DistStock
            {
                public string itemname {get;set;}
                public string partyname {get;set;}
                public string totalqty {get;set;}
            }


            public class DistStockWise
            {
                public string partyname { get; set; }
                public string itemname { get; set; }
       
                public string openingqty { get; set; }
                public string Saleqty { get; set; }
                public string Closingqty { get; set; }
            }
        protected void ListArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matareaStr = "";
            if (ListArea.SelectedIndex != -1)
            {
                foreach (ListItem matarea in ListArea.Items)
                {
                    if (matarea.Selected)
                    {
                        matareaStr += matarea.Value + ",";
                    }
                }
                matareaStr = matareaStr.TrimStart(',').TrimEnd(',');
                if (ddltype.SelectedValue == "1")
                {
                    todate.Visible = true;
                    lbl.InnerText = "From Date :";
                }
                else
                {
                    todate.Visible = false;
                    lbl.InnerText = "Date :";
                }
                BindDistributorDDl(matareaStr);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Area');", true);
                return;
            }
        }
        private void BindDistributorDDl(string areaIDStr)
        {
            try
            {
                 string distqry ="";
                if (areaIDStr != "")
                {
                    string citystr = "";

                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);


                    //   string distqry = @"select PartyId,PartyName from MastParty where smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                     if(Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
                         distqry = @"select PartyId,PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1  and PartyId=" + Settings.Instance.DistributorID + " order by PartyName";
                     else
                         distqry = @"select PartyId,PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1 order by PartyName";
                    //              string distqry = @"select * from MastParty where CityId=" + dtCity.Rows[0]["AreaId"] + " and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                    ListBox1.SelectedValue = distid;
                    dtDist.Dispose();
                }
                else
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    //distreportrpt.DataSource = null;
                    //distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindArea()
        {
            string cityQry = "";
            if (Settings.Instance.RoleType.ToUpper() == "ADMIN")
            {
                //cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                //cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                //cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                //cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                //cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                //cityQry = cityQry + " and LinkTable='AREA' ))";
                /////and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                //// cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                //cityQry = cityQry + "  order by VG.areaName";


                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.Active=1 ORDER BY MA1.AreaName";

            }
            else
            {

                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND   MA1.Active=1  and  MA1.areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) ORDER BY MA1.AreaName";


            }
            if (areaid != "")
            {


                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (" + areaid + ") and  MA1.Active=1 ";
            
            } 
            DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            if (dtProdRep.Rows.Count > 0)
            {
                ListArea.DataSource = dtProdRep;
                ListArea.DataTextField = "Name";
                ListArea.DataValueField = "Id";
                ListArea.DataBind();
            }
            ListArea.SelectedValue = areaid;
            dtProdRep.Dispose();
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistributorStockReport.aspx");
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string str = "";

            string strstockqty = "";

            string strMinusqty = "";
            string strtotalqty = "";
            string strPlusqty = "";
            string condition = "";
            string condition1 = "";

            string DistId = "";

            DistId = Request.Form[hiddistributor.UniqueID];
            if (ddltype.SelectedValue == "1")
            {


                if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                {
                    //  GetDailyWorkingSummaryL1(smIDStr1, frmTextBox.Text, toTextBox.Text, userIdStr, "0");
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                    return;
                }
             
                if (DistId != "")
                {

                    condition = "and TO2.DistID  in (" + DistId + ") ";

                    condition1 = "where Isnull(TO1.DistID,0) in (" + DistId + ") ";

                }
                if (txtfmDate.Text != "")
                {
                    if (condition != "")
                    {
                        condition = condition + " and TO2.VDate>='" + txtfmDate.Text + "'";
                        condition1 = condition1 + " and TO2.VDate>='" + txtfmDate.Text + "'";
                    }
                    else
                    {
                        condition = " where TO2.VDate>='" + txtfmDate.Text + "'";
                        condition1 = " where TO2.VDate>='" + txtfmDate.Text + "'";
                    }

                }
                if (txttodate.Text != "")
                {
                    if (condition != "")
                    {
                        condition = condition + " and TO2.VDate<='" + txttodate.Text + "'";
                        condition1 = condition1 + " and TO2.VDate<='" + txttodate.Text + "'";
                    }
                    else
                    {
                        condition = " where TO2.VDate<='" + txttodate.Text + "'";
                        condition1 = " where TO2.VDate<='" + txttodate.Text + "'";
                    }

                }





                /////////////stock qty


                strstockqty = " SELECT STKDocId AS Docid,ItemId,Qty,DistId,0 AS minusqty,0 AS plusqty,Isnull(MRP,0) AS MRP FROM temp_TransDistStock TO2 ";
                strstockqty = strstockqty + " WHERE STKDocId in(SELECT  Min(STKDocid) AS Docid  From(SELECT STKDocid,Itemid,Distid,Isnull(MRP,0) AS MRP from temp_TransDistStock ";
                strstockqty = strstockqty + " UNION ALL ";
                strstockqty = strstockqty + " SELECT STKDocid,Itemid,Distid,Isnull(MRP,0) AS MRP from TransDistStock) AS T GROUP BY DistId,itemid,Mrp)  " + condition + " ";
                strstockqty = strstockqty + " UNION all ";
                strstockqty = strstockqty + " SELECT STKDocId AS Docid,ItemId,Qty,DistId ,0 AS minusqty,0 AS plusqty,Isnull(MRP,0) AS MRP FROM TransDistStock TO2 ";
                strstockqty = strstockqty + " WHERE STKDocId in(SELECT  Min(STKDocid) AS Docid From(SELECT STKDocid,Itemid,Distid,Isnull(MRP,0) AS MRP from temp_TransDistStock ";
                strstockqty = strstockqty + " UNION all ";
                strstockqty = strstockqty + " SELECT STKDocid,Itemid,Distid,Isnull(MRP,0) AS MRP from TransDistStock) ";
                strstockqty = strstockqty + " AS T GROUP BY DistId,itemid,Mrp)  " + condition + " ";


                ////minus qty
                strMinusqty = " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM TransOrder1 TO1 ";
                strMinusqty = strMinusqty + "  LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId  " + condition1 + "";
                strMinusqty = strMinusqty + " UNION ALL ";
                strMinusqty = strMinusqty + " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM Temp_TransOrder1 TO1 ";
                strMinusqty = strMinusqty + " LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId  LEFT JOIN Temp_TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId " + condition1 + "";

                ////plus qty

                strPlusqty = " SELECT TO2.DistInvDocId As Docid,TO1.itemid as itemid,0 AS Qty,TO2.DistId AS DistId,0 AS minusqty,Qty AS plusqty ,MI.MRP AS MRP FROM TransDistInv1 TO1 ";
                strPlusqty = strPlusqty + "  LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransDistInv TO2 ON TO2.DistInvDocId=TO1.DistInvDocId WHERE Isnull(TO1.ItemId,0)<>0  " + condition + " ";



                ///complete
                ///
                strtotalqty = "SELECT Max(MP.PartyName) As PartyName ,Max(MI.ItemName) As ItemName,Sum(T.Qty)-Sum(T.minusqty)+Sum(T.plusqty) AS Totalqty,Max(T.MRP) AS MRP ";
                strtotalqty = strtotalqty + " from(" + strstockqty + " ";
                strtotalqty = strtotalqty + " Union all " + strPlusqty + "";
                strtotalqty = strtotalqty + " Union all " + strMinusqty + "";
                strtotalqty = strtotalqty + " ) AS T Left join MastParty MP on MP.PartyId = T.DistId ";
                strtotalqty = strtotalqty + " Left join MastItem MI on MI.Itemid = T.itemid WHERE Isnull(T.DistId,0)<>0 and isnull(T.ItemId,0)<>0 and Isnull(MI.itemname,'')<>'' ";
                strtotalqty = strtotalqty + " GROUP BY T.DistId,T.itemid,T.MRP order by Max(MP.PartyName) ";


                DataTable Dt = new DataTable();
                Dt = DbConnectionDAL.GetDataTable(CommandType.Text, strtotalqty);

                sb.Append("From Date : " + txtfmDate.Text +", To Date : " + txttodate.Text );
                sb.AppendLine(System.Environment.NewLine);


                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DistributorStockReport.csv");
                string headertext = "Distributor ".TrimStart('"').TrimEnd('"') + " ," + "Item".TrimStart('"').TrimEnd('"') + "," + "Total Qty".TrimStart('"').TrimEnd('"') + "," + "MRP".TrimStart('"').TrimEnd('"');

               
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;


                for (int j = 0; j < Dt.Rows.Count; j++)
                {
                    for (int k = 0; k < Dt.Columns.Count; k++)
                    {
                     


                            if (Dt.Rows[j][k].ToString().Contains(","))
                            {

                                sb.Append(String.Format("\"{0}\"", Dt.Rows[j][k].ToString()) + ',');

                            }
                            else if (Dt.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                            {


                                sb.Append(String.Format("\"{0}\"", Dt.Rows[j][k].ToString()) + ',');

                            }
                            else
                            {

                                sb.Append(Dt.Rows[j][k].ToString() + ',');


                            }

                       
                    }
                    sb.Append(Environment.NewLine);
                }

             
                Dt.Dispose();
                
            }
            else
            {
                if (DistId != "")
                {
                    condition = " WHERE TS.DistId in (" + DistId + ") ";

                    condition1 = "Where MP.UnderId in (" + DistId + ") ";
                }



                if (txtfmDate.Text != "" )
                {
                    if (condition != "")
                    {
                        condition = condition + " and VDate='" + txtfmDate.Text  + "'";

                        condition1 = condition1 + " and VDate='" + txtfmDate.Text + "'";
                    }
                    else
                    {
                        condition = "Where VDate='" + txtfmDate.Text + "'";
                        condition1 = "Where VDate='" + txtfmDate.Text + "'";
                    }
                }


                str = "SELECT Max(T.PartyName) AS PartyName,Max(T.ItemName) AS ItemName, Cast(Sum(openingqty) As varchar) +' '+ Max(T.Unit)  AS openingqty,Cast(Sum(SaleQty) As varchar) +' '+ Max(T.Unit) AS SaleQty,Cast((Sum(openingqty)-Sum(SaleQty)) As varchar) +' '+ Max(T.Unit) AS Closingqty From(SELECT Sum(Qty) AS openingqty,0 AS SaleQty,Max(MI.ItemName) AS ItemName,Max(MP.PartyName) AS PartyName ,Max(MP.PartyId) AS PartyId,Max(MI.ItemId) AS ItemId,Max(MI.MRP) AS MRP,Max(TS.TallyUnit) As Unit  FROM TransDistStock TS LEFT JOIN Mastparty MP ON MP.PartyId=TS.DistId LEFT JOIN MastItem MI ON MI.ItemId=TS.ItemId " + condition + " GROUP BY TS.DistId,TS.ItemId,MI.MRP  UNION all  SELECT  0 AS openingqty,Sum(Qty) AS SaleQty,Max(MI.ItemName) AS ItemName,Max(MD.PartyName) AS PartyName  ,Max(MP.UnderId) AS PartyId,Max(MI.ItemId) AS ItemId,Max(MI.MRP) AS MRP,Max(TR1.Unit) As Unit FROM TransRetailerInv1 TR1  LEFT JOIN MastParty MP ON MP.PartyId=TR1.PartyId  LEFT JOIN MastParty MD ON MD.PartyId=MP.UnderId  LEFT JOIN MastItem MI ON MI.ItemId=TR1.ItemId  " + condition1 + " GROUP BY MP.UnderId,TR1.ItemId,MI.MRP) AS T GROUP BY T.PartyId,T.ItemId,T.MRP";

                DataTable Dt = new DataTable();
                Dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                sb.Append("Date : " + txtfmDate.Text);
                sb.AppendLine(System.Environment.NewLine);

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DistributorStockReport.csv");
                string headertext = "Distributor ".TrimStart('"').TrimEnd('"') + " ," + "Item".TrimStart('"').TrimEnd('"') + "," + "Opening Qty".TrimStart('"').TrimEnd('"') + "," + "Sale Qty".TrimStart('"').TrimEnd('"') + "," + "Closing Qty".TrimStart('"').TrimEnd('"');


                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;


                for (int j = 0; j < Dt.Rows.Count; j++)
                {
                    for (int k = 0; k < Dt.Columns.Count; k++)
                    {
                      


                            if (Dt.Rows[j][k].ToString().Contains(","))
                            {

                                sb.Append(String.Format("\"{0}\"", Dt.Rows[j][k].ToString()) + ',');

                            }
                            else if (Dt.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                            {


                                sb.Append(String.Format("\"{0}\"", Dt.Rows[j][k].ToString()) + ',');

                            }
                            else
                            {

                                sb.Append(Dt.Rows[j][k].ToString() + ',');


                            }

                        }
                   
                    sb.Append(Environment.NewLine);
                }

               
                Dt.Dispose();
            }

            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorStockReport.csv");
            Response.Write(sb.ToString());
         //   ExportRetailer();
            Response.End();

            sb.Clear();
            //Dt.Dispose();
        }

      
    }
}