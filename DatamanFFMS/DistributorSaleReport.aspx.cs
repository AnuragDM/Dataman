using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DistributorSaleReport : System.Web.UI.Page
    {
        string roleType = "";
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


                List<DistStock> distributors = new List<DistStock>();
                distributors.Add(new DistStock());
                distreportrpt.DataSource = distributors;
                distreportrpt.DataBind();
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
            public string Distributor { get; set; }
            public string Date { get; set; }
            public string Itemname { get; set; }
            public string ExpectedStock { get; set; }
            public string CurrentStock { get; set; }
            public string InvoiceStock { get; set; }
            public string Totalsalebysalesperson { get; set; }
            public string Totalsalebydistributor { get; set; }
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
            BindDistributorDDl(matareaStr);
        }
        private void BindDistributorDDl(string areaIDStr)
        {
            try
            {
                if (areaIDStr != "")
                {
                    string citystr = "";

                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);


                    //   string distqry = @"select PartyId,PartyName from MastParty where smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select PartyId,PartyName from MastParty where Areaid in (" + areaIDStr + ")  and PartyDist=1 and Active=1 order by PartyName";
                    //              string distqry = @"select * from MastParty where CityId=" + dtCity.Rows[0]["AreaId"] + " and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
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
                cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                cityQry = cityQry + " and LinkTable='AREA' ))";
                ///and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                // cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                cityQry = cityQry + "  order by VG.areaName";
            }
            else
            {




                cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                cityQry = cityQry + "  order by VG.areaName";
            }
            DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            if (dtProdRep.Rows.Count > 0)
            {
                ListArea.DataSource = dtProdRep;
                ListArea.DataTextField = "Name";
                ListArea.DataValueField = "Id";
                ListArea.DataBind();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistributorSaleReport.aspx");
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            DataTable Dtstock = new DataTable();
            DataTable Dtorder = new DataTable();
            DataTable Dtinvoice = new DataTable();
            string currentstock = "";
            string orderstock = "";
            string invstock = "";
            string condition = "";
            string condition1 = "";
            Dt.Columns.Add("Distributor");
            Dt.Columns.Add("DistId");
            Dt.Columns.Add("Date");
            Dt.Columns.Add("Item");
            Dt.Columns.Add("ItemId");
            Dt.Columns.Add("PrevoiusStk");
            Dt.Columns.Add("CurrentStk");
            Dt.Columns.Add("totalinvstk");
            Dt.Columns.Add("totalsalebysalesperson");
            Dt.Columns.Add("totalsalebydistributor");
            string DistId = "";

            DistId = Request.Form[hiddistributor.UniqueID];
            if (DistId != "")
            {
                if (condition != "")
                {
                    condition = "and TO2.DistID  in (" + DistId + ") ";

                    condition1 = "and TO1.DistID in (" + DistId + ") ";
                    //   condition1 = "and TO2.PartyID in (" + DistId + ") ";
                }
                else
                {
                    condition = "where TO2.DistID  in (" + DistId + ") ";
                    condition1 = "where TO1.DistID in (" + DistId + ") ";
                    //   condition1 = "where TO2.PartyID in (" + DistId + ") ";
                }
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


            DataRow Dr;

            currentstock = "SELECT Sum(Qty) AS Qty,max(T.Itemid) AS itemid,Max(MI.itemname) AS itemname,Max(MP.PartyName) AS distributor,Max(Distid) AS DistId,Max(VDate) AS vdate ";
            currentstock = currentstock + " FROM (SELECT Qty,Itemid,DistId,VDate FROM    TransDistStock TO2 " + condition;
            currentstock = currentstock + " UNION all ";
            currentstock = currentstock + " SELECT Qty,Itemid,DistId,VDate FROM temp_TransDistStock TO2 " + condition + ")AS T ";
            currentstock = currentstock + " LEFT JOIN MastParty MP ON MP.PartyId=T.DistId LEFT JOIN Mastitem MI ON MI.ItemId=T.itemid ";
            currentstock = currentstock + " where Isnull(MP.PartyName,'')<>'' and Isnull(MI.itemname,'')<>'' ";
            currentstock = currentstock + "  GROUP BY T.vDate,T.DistId, T.Itemid ORDER BY Distid,itemid  ";


            //currentstock = "SELECT Sum(TO2.Qty) AS Currentstock ,max(TO2.Itemid) AS itemid,Max(TO2.Distid) AS distId,Max(TO2.VDate) AS Vdate,Max(TO2.smid) AS smid FROM TransDistStock TO2 ";
            //currentstock = currentstock + condition;
            //currentstock = currentstock + " GROUP BY vDate, DistId,Itemid,mrp  ORDER BY vdate";

            Dtstock = DbConnectionDAL.GetDataTable(CommandType.Text, currentstock);



            orderstock = "SELECT Sum(Qty) AS Qty,max(T.Itemid) AS itemid,Max(MI.itemname) AS itemname,Max(MP.PartyName) AS distributor,Max(Distid) AS DistId,Max(VDate) AS vdate ";
            orderstock = orderstock + " FROM (SELECT Qty,Itemid,DistId,VDate FROM    TransOrder1 TO2 " + condition;
            orderstock = orderstock + " UNION all ";
            orderstock = orderstock + " SELECT Qty,Itemid,DistId,VDate FROM temp_TransOrder1 TO2 " + condition + " )AS T ";
            orderstock = orderstock + " LEFT JOIN MastParty MP ON MP.PartyId=T.DistId LEFT JOIN Mastitem MI ON MI.ItemId=T.itemid ";
            orderstock = orderstock + "  GROUP BY T.vDate,T.DistId, T.Itemid ORDER BY Distid,itemid  ";

            //orderstock = "SELECT Sum(Qty) AS totalsale ,max(Itemid) AS itemid,Max(Distid) AS distId,Max(VDate) AS Vdate,Max(smid) AS smid FROM TransOrder1 TO2 ";
            //orderstock = orderstock + condition;
            //orderstock = orderstock + " GROUP BY vDate,DistId, Itemid ORDER BY vdate";

            Dtorder = DbConnectionDAL.GetDataTable(CommandType.Text, orderstock);


            invstock = "SELECT Sum(Qty) AS totalinv ,max(Itemid) AS itemid,Max(Distid) AS distId,Max(VDate) AS Vdate FROM TransDistInv1  TO2  ";
            invstock = invstock + condition;
            invstock = invstock + " GROUP BY vDate,DistId, Itemid ORDER BY Distid,itemid ";
            Dtinvoice = DbConnectionDAL.GetDataTable(CommandType.Text, invstock);
            if (Dtstock.Rows.Count > 0)
            {
                decimal expectedqty = 0;
                decimal totalsaledis = 0;
                int previousdistid = 0;
                int previousitemid = 0;
                string Previousdate = "";
                for (int i = 0; i < Dtstock.Rows.Count; i++)
                {
                    Dr = Dt.NewRow();

                    DataView dv = new DataView(Dtinvoice);
                    if (Previousdate == "")
                        dv.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + "  and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                    else
                    {
                        if (previousdistid == Convert.ToInt32(Dtstock.Rows[i]["DistId"].ToString()) && previousitemid == Convert.ToInt32(Dtstock.Rows[i]["ItemId"].ToString()))
                        {
                            dv.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + " and vdate>'" + Previousdate + "' and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                        }
                        else
                        {
                            dv.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + " and   vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                        }
                    }
                    dv.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + " and vdate>'" + Previousdate + "' and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                    DataTable dtfilterinv = dv.ToTable();


                    decimal invqty = 0;
                    decimal orderqty = 0;

                   
                    if (dtfilterinv.Rows.Count > 0)
                    {
                        invqty = Convert.ToDecimal(dtfilterinv.Compute("Sum(totalinv)", string.Empty));
                    }
                    else
                    {
                        invqty = 0;
                    }
                    DataView dv1 = new DataView(Dtorder);
                    //  dv1.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + "  and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";

                    if (Previousdate == "")
                        dv1.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + "  and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                    else
                    {
                        if (previousdistid == Convert.ToInt32(Dtstock.Rows[i]["DistId"].ToString()) && previousitemid == Convert.ToInt32(Dtstock.Rows[i]["ItemId"].ToString()))
                        {
                            dv1.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + " and vdate>'" + Previousdate + "' and  vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                        }
                        else
                        {
                            dv1.RowFilter = "distId = " + Dtstock.Rows[i]["DistId"].ToString() + " and itemid =  " + Dtstock.Rows[i]["ItemId"].ToString() + " and   vdate<='" + Dtstock.Rows[i]["Vdate"].ToString() + "'";
                        }
                    }
                    DataTable dtfilterord = dv1.ToTable();
                    if (dtfilterord.Rows.Count > 0)
                    {
                        orderqty =  Convert.ToDecimal(dtfilterord.Compute("Sum(Qty)", string.Empty)); 
                    }
                    else
                    {
                        orderqty = 0;
                    }
                    DataTable dtfiltersale = new DataTable();
                    if (Dt.Rows.Count > 0)
                    {
                        DataView dv2 = new DataView(Dt);
                        dv2.RowFilter = "DistId = " + Dtstock.Rows[i]["DistId"].ToString() + " and ItemId =  " + Dtstock.Rows[i]["ItemId"].ToString() + " ";
                        dtfiltersale = dv2.ToTable();
                    }

                    //if (dtfiltersale.Rows.Count== 0)
                    //{
                    //    Dr["Distributor"] = Dtstock.Rows[i]["distributor"].ToString();
                    //    Dr["DistId"] = Dtstock.Rows[i]["DistId"].ToString();
                    //    Dr["Date"] = Dtstock.Rows[i]["Vdate"].ToString();
                    //    Dr["Item"] = Dtstock.Rows[i]["itemname"].ToString();
                    //    Dr["ItemId"] = Dtstock.Rows[i]["ItemId"].ToString();
                    //    Dr["PrevoiusStk"] = expectedqty;
                    //    Dr["CurrentStk"] = Dtstock.Rows[i]["Qty"].ToString();
                    //    Dr["totalinvstk"]= invqty ;
                    //    Dr["totalsalebysalesperson"] = orderqty;
                    //    Dr["totalsalebydistributor"]=0;                                                                                                                                    
                    //}
                    //else
                    //{
                    if (previousdistid == Convert.ToInt32(Dtstock.Rows[i]["DistId"].ToString()) && previousitemid == Convert.ToInt32(Dtstock.Rows[i]["ItemId"].ToString()))
                    {
                        totalsaledis = expectedqty - Convert.ToDecimal(Dtstock.Rows[i]["Qty"].ToString());
                    }
                    else
                    {
                        totalsaledis = 0;
                        expectedqty = 0;
                    }

                    Dr["Distributor"] = Dtstock.Rows[i]["distributor"].ToString();
                    Dr["DistId"] = Dtstock.Rows[i]["DistId"].ToString();
                    Dr["Date"] = Dtstock.Rows[i]["Vdate"].ToString();
                    Dr["Item"] = Dtstock.Rows[i]["itemname"].ToString();
                    Dr["ItemId"] = Dtstock.Rows[i]["ItemId"].ToString();
                    Dr["PrevoiusStk"] = expectedqty;
                    Dr["CurrentStk"] = Dtstock.Rows[i]["Qty"].ToString();
                    Dr["totalinvstk"] = invqty;
                    Dr["totalsalebysalesperson"] = orderqty;
                    Dr["totalsalebydistributor"] = totalsaledis;
                    //  }
                    expectedqty = Convert.ToDecimal(Dtstock.Rows[i]["Qty"].ToString()) + invqty - orderqty;
                    previousdistid = Convert.ToInt32(Dtstock.Rows[i]["DistId"].ToString());
                    previousitemid = Convert.ToInt32(Dtstock.Rows[i]["ItemId"].ToString());
                    Previousdate = Dtstock.Rows[i]["Vdate"].ToString();
                    Dt.Rows.Add(Dr);
                }
            }


            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorSaleReport.csv");
            string headertext = "Distributor ".TrimStart('"').TrimEnd('"') + " ," + "Date".TrimStart('"').TrimEnd('"') + " ," + "Item".TrimStart('"').TrimEnd('"') + "," + "Expected Qty".TrimStart('"').TrimEnd('"') + "," + "Current Qty".TrimStart('"').TrimEnd('"') + "," + "Invoice Qty".TrimStart('"').TrimEnd('"') + "," + "Total Sale by Salesperson".TrimStart('"').TrimEnd('"') + "," + "Total Sale by Distributor".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;


            for (int j = 0; j < Dt.Rows.Count; j++)
            {
                for (int k = 0; k < Dt.Columns.Count; k++)
                {
                    if (k == 1 || k == 4 )
                    {
                        continue;
                    }
                    else
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
                }
                sb.Append(Environment.NewLine);
            }

            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorSaleReport.csv");
            Response.Write(sb.ToString());
            Response.End();





        }
    }
}