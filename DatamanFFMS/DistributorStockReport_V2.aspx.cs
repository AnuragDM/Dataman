using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using DAL;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class DistributorStockReport_V2 : System.Web.UI.Page
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
                //distreportrpt.DataSource = distributors;
                //distreportrpt.DataBind();


                List<DistStockWise> distributors1 = new List<DistStockWise>();
                distributors1.Add(new DistStockWise());
                //distreportrpt1.DataSource = distributors1;
                //distreportrpt1.DataBind();

                //     rptmaindis.Visible = false;
                //BindArea();
                if (Request.QueryString["DistId"] != null)
                {
                    distid = Request.QueryString["DistId"].ToString();
                    areaid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from mastparty where PartyId =" + Request.QueryString["DistId"]));
                    ListArea.Enabled = false;
                    BindDistributorDDl(areaid);
                    ListBox1.Enabled = false;

                }
                BindArea();
                //string pageName = Path.GetFileName(Request.Path);

                //if (btnexport.Text == "Export")
                //{
                //    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //    btnexport.CssClass = "btn btn-primary";
                //}
            }
        }

        public class DistStock
        {
            public string itemname { get; set; }
            public string partyname { get; set; }
            public string totalqty { get; set; }
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
        private void BindDistributorDDl(string areaIDStr)
        {
            try
            {
                string distqry = "";
                if (areaIDStr != "")
                {
                    string citystr = "";

                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);


                    //   string distqry = @"select PartyId,PartyName from MastParty where smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    if (Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
                        distqry = @"select PartyId,(PartyName+'-'+Mobile) AS PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1  and PartyId=" + Settings.Instance.DistributorID + " order by PartyName";
                    else
                        distqry = @"select PartyId,(PartyName+'-'+Mobile) AS PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1 order by PartyName";
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
            Response.Redirect("~/DistributorStockReport_V2.aspx");
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

                sb.Append("From Date : " + txtfmDate.Text + ", To Date : " + txttodate.Text);
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



                if (txtfmDate.Text != "")
                {
                    if (condition != "")
                    {
                        condition = condition + " and VDate='" + txtfmDate.Text + "'";

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

        }


        public void getdistWisestock1()
        {
            DataSet dataSet = new DataSet("Distributor Wise Stock Report");
            string DistId = "", Date = "", area = "", Qry = "", Qry1 = "", Qry2 = "", Qry3 = "", itm = "";
            string str = "";

            DataTable dt1 = new DataTable();
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    DistId += item.Value + ",";
                }
            }
            DistId = DistId.TrimEnd(',');

            foreach (ListItem item in ListArea.Items)
            {
                if (item.Selected)
                {
                    area += item.Value + ",";
                }
            }
            area = area.TrimEnd(',');

            string condition = "";
            string condition1 = "";
            if (DistId != "")
            {
                condition = " WHERE TS.DistId in (" + DistId + ") ";

                condition1 = "Where MP.UnderId in (" + DistId + ") ";

                Qry = Qry + "and Tpr.DistId in (" + DistId + ")";
                Qry1 = Qry1 + "and MP.UnderId in (" + DistId + ")";
                Qry2 = Qry2 + "and TS.DistId in (" + DistId + ")";
                Qry3 = Qry3 + "and MP.UnderId in (" + DistId + ")";
            }



            if (Date != "")
            {
                if (condition != "")
                {
                    condition = condition + " and VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";

                    condition1 = condition1 + " and VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    condition = "Where VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                    condition1 = "Where VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                }
            }

            if (area != "")
            {
                string qry = "SELECT STUFF( (SELECT ', ' +quotename(t2.COL) FROM ( select Itemname+'-'+Isnull(ms.SyncId,'') As COL from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from TransDistStock Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active=1 and PartyDist=1 " + Qry + "))" +

                " UNION select Itemname+'-'+Isnull(ms.SyncId,'') As Itemname from MastItem ms where ms.ItemId in (select DISTINCT Tpr.ItemId from TransRetailerInv1 Tpr LEFT JOIN MastParty MP ON MP.PartyId = Tpr.PartyId  LEFT JOIN MastParty MD ON MD.PartyId = MP.UnderId where MP.AreaId in(" + area + ") and MP.Active = 1 and MP.PartyDist = 0 " + Qry1 + " )) t2 FOR XML PATH ('')), 1, 1, '') as name";
                itm = DbConnectionDAL.GetStringScalarVal(qry);

//                string qry = "select Itemname+'-'+Isnull(ms.SyncId,'') As Itemname from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from TransDistStock Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active=1 and PartyDist=1 " + Qry + "))" +

//" UNION select Itemname+'-'+Isnull(ms.SyncId,'') As Itemname from MastItem ms where ms.ItemId in (select DISTINCT Tpr.ItemId from TransRetailerInv1 Tpr LEFT JOIN MastParty MP ON MP.PartyId = Tpr.PartyId  LEFT JOIN MastParty MD ON MD.PartyId = MP.UnderId where MP.AreaId in(" + area + ") and MP.Active = 1 and MP.PartyDist = 0 " + Qry1 + " )";
//                dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
//                foreach (DataRow row in dt1.Rows)
//                {
//                    itm += "[" + row["Itemname"].ToString() + "]" + ",";
//                }
//                itm = itm.TrimEnd(',');

                if (itm != "")
                {
                    str = "Select * from(SELECT Cast(Sum(openingqty) As varchar) AS openingqty,Cast(Sum(SaleQty) As varchar) AS SaleQty,Cast((Sum(openingqty)-Sum(SaleQty)) As varchar) AS Closingqty,Max(T.ItemName) AS ItemName,Max(T.Area) AS Area,Max(T.City) AS City,Max(T.State) AS State,Max(T.PartyName) AS PartyName,Max(T.Mobile) AS Mobile,Max(T.MRP) AS MRP From ( " +

                    " SELECT Sum(Qty) AS openingqty,0 AS SaleQty,Max(MI.ItemName)+'-'+Max(Isnull(MI.SyncId,'')) As ItemName,Max(mss.AreaName)+'-'+Max(Isnull(mss.SyncId,'')) As State,Max(msc.AreaName)+'-'+Max(Isnull(msc.SyncId,'')) As City,Max(msa.AReaName)+'-'+Max(Isnull(msa.SyncId,'')) As Area,Max(MP.PartyName)+'-'+Max(Isnull(MP.SyncId,'')) AS PartyName,Max(MP.Mobile) AS Mobile,Max(MI.ItemId) AS ItemId,Max(MI.MRP) AS MRP FROM TransDistStock TS LEFT JOIN Mastparty MP ON MP.PartyId=TS.DistId Left Join MastArea msa on msa.AreaId=Mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId LEFT JOIN MastItem MI ON MI.ItemId=TS.ItemId  WHERE Ts.AreaID in (" + area + ") " + Qry2 + " and TS.VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' GROUP BY TS.DistId,MI.MRP UNION all " +

                    "  SELECT 0 AS openingqty,Sum(Qty) AS SaleQty,Max(MI.ItemName)+'-'+Max(Isnull(MI.SyncId,'')) As ItemName,Max(mss.AreaName)+'-'+Max(Isnull(mss.SyncId,''))  As State,Max(msc.AreaName)+'-'+Max(Isnull(msc.SyncId,''))  As City,Max(msa.AReaName)+'-'+Max(Isnull(msa.SyncId,''))  As Area,Max(MD.PartyName)+'-'+Max(Isnull(MD.SyncId,''))  AS PartyName,Max(MP.Mobile) AS Mobile,Max(MI.ItemId) AS ItemId,Max(MI.MRP) AS MRP FROM TransRetailerInv1 TR1  LEFT JOIN MastParty MP ON MP.PartyId=TR1.PartyId  LEFT JOIN MastParty MD ON MD.PartyId=MP.UnderId Left Join MastArea msa on msa.AreaId=Md.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId LEFT JOIN MastItem MI ON MI.ItemId=TR1.ItemId LEFT JOIN MastArea MA ON MA.AreaId=MD.AreaId Where MD.Areaid in (" + area + ") " + Qry3 + " and TR1.VDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' GROUP BY MP.UnderId,MI.MRP) AS T GROUP BY T.PartyName,T.MRP)s PIVOT (SUM(MRP) FOR ItemName in (" + itm + ")) as PivotTable";

                    DataTable Dt = new DataTable();
                    Dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    dataSet.Tables.Add(Dt);

                    try
                    {
                        Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                        string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                        if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = "DistWise Stock Report.xlsx";

                        if (File.Exists(path + filename))
                        {
                            File.Delete(path + filename);
                        }

                        //Excel.Application excelApp = new Excel.Application();
                        string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                        Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                        Microsoft.Office.Interop.Excel.Range chartRange;
                        Excel.Range range;
                        Excel.Sheets xlSheets = null;
                        Excel.Worksheet xlWorksheet = null;
                        // Loop over DataTables in DataSet.
                        DataTableCollection collection = dataSet.Tables;

                        for (int i = collection.Count; i > 0; i--)
                        {
                            //Create Excel Sheets
                            xlSheets = ExcelApp.Sheets;
                            xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                           Type.Missing, Type.Missing, Type.Missing);

                            System.Data.DataTable table = collection[i - 1];

                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('" + collection[i - 1] + "');", true);
                            xlWorksheet.Name = table.TableName;

                            xlWorksheet.Name = "DistWise Stock";

                          
                            

                            for (int j = 1; j < table.Columns.Count + 1; j++)
                            {
                                ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                                range = xlWorksheet.Cells[1, j] as Excel.Range;
                                range.Cells.Font.Name = "Calibri";
                                range.Cells.Font.Bold = true;
                                range.Cells.Font.Size = 11;
                                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                            }

                            // Storing Each row and column value to excel sheet
                            for (int k = 0; k < table.Rows.Count; k++)
                            {
                                for (int l = 0; l < table.Columns.Count; l++)
                                {
                                    ExcelApp.Cells[k + 2, l + 1] =
                                    table.Rows[k].ItemArray[l].ToString();
                                }
                            }
                            ExcelApp.Columns.AutoFit();
                            xlWorksheet.Activate();
                            xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                            xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                            Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                            chartRange = xlWorksheet.get_Range("A1", last);
                            foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                            {
                                cell.BorderAround2();
                            }
                            Excel.FormatConditions fcs = chartRange.FormatConditions;
                            Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
            (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                            format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                        }
                        xlWorksheet.PageSetup.CenterFooter = "&B Copyright © 2016 e-iceblue. All Rights Reserved.";
                        xlWorkbook.SaveAs(strPath);
                        xlWorkbook.Close();
                        ExcelApp.Quit();
                        Response.ContentType = "Application/xlsx";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.TransmitFile(strPath);
                        Response.End();
                        //xlWorkbook.Close();
                        //ExcelApp.Quit();
                        //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                        //ExcelApp.Visible = true;
                    }
                    catch (Exception ex)
                    {

                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);
                    Dt.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No data exist');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Area');", true);
            }

            dataSet.Dispose();
        }


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getdiststock1()
        {
            string DistId = "", FromDate = "", ToDate = "", itm = "", area = "", Qry1 = "", Qry = "", Qry2 = "";

            DataSet dataSet = new DataSet("Sales Person Wise Stock Report");
            string str = "";
            DataTable dt1 = new DataTable();

            foreach (ListItem item in ListArea.Items)
            {
                if (item.Selected)
                {
                    area += item.Value + ",";
                }
            }
            area = area.TrimEnd(',');

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    DistId += item.Value + ",";
                }
            }
            DistId = DistId.TrimEnd(',');

            string strstockqty = "";
            string strstockamo = "";

            string strMinusqty = "";
            string strtotalqty = "";
            string strtotalamo = "";
            string strPlusqty = "";
            string condition = "";
            string condition1 = "";

            if (DistId != "")
            {
                condition = "and TO2.DistID  in (" + DistId + ") ";

                condition1 = "where Isnull(TO1.DistID,0) in (" + DistId + ") ";
                Qry = Qry + "and TO2.DistID  in (" + DistId + ")";
                Qry1 = Qry1 + " and Tpr.DistId in (" + DistId + ")";
                Qry2 = Qry2 + "and Isnull(TO1.DistID, 0) in (" + DistId + ")";
            }
            if (FromDate != "")
            {
                if (condition != "")
                {
                    condition = condition + " and TO2.VDate>='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                    condition1 = condition1 + " and TO2.VDate>='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    condition = " where TO2.VDate>='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                    condition1 = " where TO2.VDate>='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                }

            }
            if (ToDate != "")
            {
                if (condition != "")
                {
                    condition = condition + " and TO2.VDate<='" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "'";
                    condition1 = condition1 + " and TO2.VDate<='" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    condition = " where TO2.VDate<='" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "'";
                    condition1 = " where TO2.VDate<='" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "'";
                }

            }
            if (area != "")
            {
                string qry = "SELECT STUFF( (SELECT ', ' +quotename(t2.COL) FROM ( select Itemname+'-'+Isnull(ms.SyncId,'') As COL  from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from TransDistStock Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active=1 and PartyDist=1 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')" +

" UNION select Itemname+'-'+Isnull(ms.SyncId,'') As COL  from MastItem ms where ms.ItemId in (select DISTINCT Tpr.ItemId from TransDistInv1 Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active = 1 and PartyDist = 1 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')" +

" UNION select Itemname+'-'+Isnull(ms.SyncId,'') As COL  from MastItem ms where ms.ItemId in (select DISTINCT Tpr.ItemId from TransOrder1 Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active = 1 and PartyDist = 1 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')" +

" UNION select Itemname+'-'+Isnull(ms.SyncId,'') As COL  from MastItem ms where ms.ItemId in (select DISTINCT Tpr.ItemId from Temp_TransOrder1 Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in(" + area + ") and Active = 1 and PartyDist = 1 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')) t2 FOR XML PATH ('')), 1, 1, '') as name";
                itm = DbConnectionDAL.GetStringScalarVal(qry);
                //itm = itm.TrimEnd(',');


                /////////////stock qty

                strstockqty = "select * from (SELECT Max(mss.AreaName)+'-'+Max(Isnull(mss.SyncId,'')) As State,Max(msc.AreaName)+'-'+Max(Isnull(msc.SyncId,'')) As City,MAX(msa.AreaName)+'-'+Max(Isnull(msa.SyncId,'')) As Area,Max(MI.ItemName)+'-'+Max(Isnull(MI.SyncId,'')) As ItemName,Max(MP.PartyName)+'-'+Max(Isnull(MP.SyncId,'')) As PartyName,Sum(T.Qty)-Sum(T.minusqty)+Sum(T.plusqty) AS Totalqty from ( SELECT STKDocId AS Docid,ItemId,Qty,DistId,0 AS minusqty,0 AS plusqty,Isnull(MRP,0) AS MRP FROM temp_TransDistStock TO2  WHERE STKDocId in (SELECT  Min(STKDocid) AS Docid  From(SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from temp_TransDistStock UNION ALL " +
        " SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from TransDistStock) AS T GROUP BY DistId, itemid, Mrp) and TO2.AreaId in (" + area + ") " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION all " +

       " SELECT STKDocId AS Docid, ItemId, Qty, DistId,0 AS minusqty,0 AS plusqty, Isnull(MRP,0) AS MRP FROM TransDistStock TO2 WHERE STKDocId in(SELECT  Min(STKDocid) AS Docid From(SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from temp_TransDistStock UNION all  SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from TransDistStock)  AS T GROUP BY DistId,itemid,Mrp) and TO2.AreaId in (" + area + ")  " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Union all " +

       " SELECT TO2.DistInvDocId As Docid,TO1.itemid as itemid,0 AS Qty,TO2.DistId AS DistId,0 AS minusqty,Qty AS plusqty ,MI.MRP AS MRP FROM TransDistInv1 TO1 LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN MastParty MP ON MP.PartyId=TO1.DistId LEFT JOIN MastArea MA ON MA.AreaId=MP.AreaId LEFT JOIN TransDistInv TO2 ON TO2.DistInvDocId=TO1.DistInvDocId WHERE Isnull(TO1.ItemId,0)<>0 and MA.AreaId in (" + area + ") " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Union all " +

       " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM TransOrder1 TO1   LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId  where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION ALL " +

       " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM Temp_TransOrder1 TO1   LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId  where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION ALL " +

       " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM Temp_TransOrder1 TO1  LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId  LEFT JOIN Temp_TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') AS T Left join MastParty MP on MP.PartyId = T.DistId Left join MastItem MI on MI.Itemid = T.itemid Left Join MastArea msa on msa.AreaId = MP.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId WHERE Isnull(T.DistId, 0)<> 0 and isnull(T.ItemId, 0)<> 0 and Isnull(MI.itemname, '')<> ''  GROUP BY T.DistId,T.MRP)s " +

       " PIVOT (SUM(Totalqty) FOR ItemName in (" + itm + ")) as PivotTable ";

                DataTable Dt = new DataTable();
                Dt = DbConnectionDAL.GetDataTable(CommandType.Text, strstockqty);


                strstockamo = "select * from (SELECT Max(mss.AreaName)+'-'+Max(Isnull(mss.SyncId,'')) As State,Max(msc.AreaName)+'-'+Max(Isnull(msc.SyncId,'')) As City,MAX(msa.AreaName)+'-'+Max(Isnull(msa.SyncId,'')) As Area,Max(MI.ItemName)+'-'+Max(Isnull(MI.SyncId,'')) As ItemName,Max(MP.PartyName)+'-'+Max(Isnull(MP.SyncId,'')) As PartyName,Max(T.MRP) AS MRP  from ( SELECT STKDocId AS Docid,ItemId,Qty,DistId,0 AS minusqty,0 AS plusqty,Isnull(MRP,0) AS MRP FROM temp_TransDistStock TO2  WHERE STKDocId in (SELECT  Min(STKDocid) AS Docid  From(SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from temp_TransDistStock UNION ALL " +
      " SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from TransDistStock) AS T GROUP BY DistId, itemid, Mrp) and TO2.AreaId in (" + area + ") " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION all " +

     " SELECT STKDocId AS Docid, ItemId, Qty, DistId,0 AS minusqty,0 AS plusqty, Isnull(MRP,0) AS MRP FROM TransDistStock TO2 WHERE STKDocId in(SELECT  Min(STKDocid) AS Docid From(SELECT  STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from temp_TransDistStock UNION all  SELECT STKDocid, Itemid, Distid, Isnull(MRP, 0) AS MRP from TransDistStock)  AS T GROUP BY DistId,itemid,Mrp) and TO2.AreaId in (" + area + ")  " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Union all " +

     " SELECT TO2.DistInvDocId As Docid,TO1.itemid as itemid,0 AS Qty,TO2.DistId AS DistId,0 AS minusqty,Qty AS plusqty ,MI.MRP AS MRP FROM TransDistInv1 TO1 LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN MastParty MP ON MP.PartyId=TO1.DistId LEFT JOIN MastArea MA ON MA.AreaId=MP.AreaId LEFT JOIN TransDistInv TO2 ON TO2.DistInvDocId=TO1.DistInvDocId WHERE Isnull(TO1.ItemId,0)<>0 and MA.AreaId in (" + area + ") " + Qry + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Union all " +

     " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM TransOrder1 TO1   LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId  where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION ALL " +

     " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM Temp_TransOrder1 TO1   LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId  where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' UNION ALL " +

     " SELECT TO2.OrdDocId As Docid,TO1.itemid as itemid,0 AS Qty,Isnull(TO1.DistID,0) AS DistId,(CASE WHEN isnull(TO2.DispatchCancelType,'')='D' THEN TO1.DispatchQty WHEN isnull(TO2.DispatchCancelType,'')='' THEN qty ELSE 0 end  ) AS minusqty,0 AS plusqty,MI.MRP AS MRP FROM Temp_TransOrder1 TO1  LEFT JOIN Mastitem MI ON MI.ItemId=TO1.ItemId  LEFT JOIN Temp_TransOrder TO2 ON TO2.OrdDocId=TO1.OrdDocId where TO1.AreaId in (" + area + ") " + Qry2 + " and TO2.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') AS T Left join MastParty MP on MP.PartyId = T.DistId Left join MastItem MI on MI.Itemid = T.itemid Left Join MastArea msa on msa.AreaId = MP.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId WHERE Isnull(T.DistId, 0)<> 0 and isnull(T.ItemId, 0)<> 0 and Isnull(MI.itemname, '')<> ''  GROUP BY T.DistId,T.MRP)s " +

     " PIVOT (SUM(MRP) FOR ItemName in (" + itm + ")) as PivotTable ";

                DataTable Dt1 = new DataTable();
                Dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, strstockamo);

                dataSet.Tables.Add(Dt);
                dataSet.Tables.Add(Dt1);

                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "SPWise Stock Report.xlsx";

                    if (File.Exists(path + filename))
                    {
                        File.Delete(path + filename);
                    }

                    //Excel.Application excelApp = new Excel.Application();
                    string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                    Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Range chartRange;
                    Excel.Range range;
                    Excel.Sheets xlSheets = null;
                    Excel.Worksheet xlWorksheet = null;
                    // Loop over DataTables in DataSet.
                    DataTableCollection collection = dataSet.Tables;

                    for (int i = collection.Count; i > 0; i--)
                    {
                        //Create Excel Sheets
                        xlSheets = ExcelApp.Sheets;
                        xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                       Type.Missing, Type.Missing, Type.Missing);

                        System.Data.DataTable table = collection[i - 1];

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('" + collection[i - 1] + "');", true);
                        xlWorksheet.Name = table.TableName;
                        if (i - 1 == 1)
                        {
                            xlWorksheet.Name = "MRP wise Item";
                            xlWorksheet.PageSetup.LeftHeader = "&B Total Qty= Distributor Stock-Salesperson order (if  order dispatched then dispatch qty else if Pending Qty else 0)+Distributor Invoice Qty";
                        }
                        else if (i - 1 == 0)
                        {
                            xlWorksheet.PageSetup.LeftHeader = "&B Total Qty= Distributor Stock-Salesperson order (if  order dispatched then dispatch qty else if Pending Qty else 0)+Distributor Invoice Qty";
                            xlWorksheet.Name = "Qty wise Item";
                        }
                      

                        for (int j = 1; j < table.Columns.Count + 1; j++)
                        {
                            ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                            range = xlWorksheet.Cells[1, j] as Excel.Range;
                            range.Cells.Font.Name = "Calibri";
                            range.Cells.Font.Bold = true;
                            range.Cells.Font.Size = 11;
                            range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                        }

                        // Storing Each row and column value to excel sheet
                        for (int k = 0; k < table.Rows.Count; k++)
                        {
                            for (int l = 0; l < table.Columns.Count; l++)
                            {
                                ExcelApp.Cells[k + 2, l + 1] =
                                table.Rows[k].ItemArray[l].ToString();
                            }
                        }
                        ExcelApp.Columns.AutoFit();
                        xlWorksheet.Activate();
                        xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                        xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                        Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        chartRange = xlWorksheet.get_Range("A1", last);
                        foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                        {
                            cell.BorderAround2();
                        }
                        Excel.FormatConditions fcs = chartRange.FormatConditions;
                        Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
        (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                        format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    }
                    xlWorkbook.SaveAs(strPath);
                    xlWorkbook.Close();
                    ExcelApp.Quit();
                    Response.ContentType = "Application/xlsx";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.TransmitFile(strPath);
                    Response.End();
                    //xlWorkbook.Close();
                    //ExcelApp.Quit();
                    //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                    //ExcelApp.Visible = true;
                }
                catch (Exception ex)
                {

                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);
                Dt.Dispose();
                dt1.Dispose();
                Dt1.Dispose();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Area');", true);
                //leavereportrpt.DataSource = null;
                //leavereportrpt.DataBind();
            }

            dataSet.Dispose();
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex == 0)
            {
                getdiststock1();
            }
            else
            {
                getdistWisestock1();
            }
        }
    }
}