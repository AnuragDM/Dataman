using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Text;
using System.IO;

namespace AstralFFMS
{
    public partial class Retailerdispatchorderform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {

                txtfmDate.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                List<distitemsalerpt1> distributors = new List<distitemsalerpt1>();
                distributors.Add(new distitemsalerpt1());
                distitemsalerpt.DataSource = distributors;
                distitemsalerpt.DataBind();
                List<itemdetail> itemdetail = new List<itemdetail>();
                itemdetail.Add(new itemdetail());
                rpt.DataSource = distributors;
                rpt.DataBind();
                List<itemdetail> itemdetail1 = new List<itemdetail>();
                itemdetail1.Add(new itemdetail());
                Repeater1.DataSource = distributors;
                Repeater1.DataBind();
                 List<itemdetail> itemdetail2 = new List<itemdetail>();
                itemdetail1.Add(new itemdetail());
                Repeater2.DataSource = distributors;
                Repeater2.DataBind();
                //string pageName = Path.GetFileName(Request.Path);
                string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');


                hidaddpermission.Value = SplitPerm[1];
                hidviewpermission.Value = SplitPerm[0];
                hiddeletepermission.Value = SplitPerm[3];

                //if (btnSave.Text == "Save")
                //{
                //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //    // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                //    btnSave.CssClass = "btn btn-primary";
                //}
                //else
                //{
                //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                //    btnSave.CssClass = "btn btn-primary";
                //}
                //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
                ////btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
                //btnDelete.CssClass = "btn btn-primary";
            
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";


            }
        }
     public class itemdetail
     {
         public string Docid { get; set; }
         public string ItemId { get; set; }
         public string ItemName  { get; set; }
           public string OrderQty { get; set; }
           public string Rate  { get; set; }
               public string Discount  { get; set; }

               public string OrderAmount { get; set; }
               public string DispatchQty { get; set; }
     }

        public class distitemsalerpt1
        {
            public string PartyName { get; set; }
            public string Address { get; set; }
            public string Mobile { get; set; }
            public string orderdate { get; set; }
            public string totalqty { get; set; }
            public string totalamount { get; set; }
            public string Status { get; set; }
        }
        [WebMethod(EnableSession=true)]
        public static ArrayList BindState()
        {
             ArrayList list = new ArrayList();
            string str = "";
            string condtion="";
            string areaid = "";
            try
            {

         
            if(Settings.Instance.RoleType.ToUpper()=="ADMIN")
            {

            }
            else if (Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
            {
               areaid=Convert.ToString( DbConnectionDAL.GetScalarValue(CommandType.Text,"Select AreaId from mastparty where PartyId ="  + Settings.Instance.DistributorID +""));
               condtion = "and Areaid =" + areaid  + "";

            }

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


                cityQry="SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
cityQry = cityQry + " AND    MA1.areaid in (select linkcode from mastlink where primtable='SALESPERSON'  and LinkTable='AREA' ) ORDER BY MA1.AreaName";

            }
            else
            {

                cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) ORDER BY MA1.AreaName";



                //cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
                //cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
                //cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
                //cityQry = cityQry + " where Vg.CityID in (select distinct underid from mastarea ";
                //cityQry = cityQry + "  where areaid in (select linkcode from mastlink where primtable='SALESPERSON' ";
                //cityQry = cityQry + " and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE ";
                //cityQry = cityQry + "  SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and Active=1 ) ";
                //cityQry = cityQry + "  order by VG.areaName";
            }
            if (areaid != "")
            {


                   cityQry = "SELECT MS.AreaName+'-'+MC.AreaName+'-'+MA1.AreaName AS Name,MA1.AreaId AS id   FROM Mastarea MA1";
                cityQry = cityQry + " LEFT JOIN MastArea MC ON MC.AreaId=MA1.underid LEFT JOIN MastArea MD ON MD.AreaId=MC.underid";
                cityQry = cityQry + " LEFT JOIN MastArea MS ON MS.AreaId=MD.underid  WHERE MA1.AreaType='Area'";
                cityQry = cityQry + " AND    MA1.areaid in (" + areaid + ")";
               // cityQry = @" SELECT DISTINCT(VG.stateName)+'-'+Vg.cityName+'-'+Vg.AreaName As Name,VG.areaName,Vg.statename,VG.AreaId As Id,";
               // cityQry = cityQry + " AreaType As [Type_Id],Vg.CityID,Ma.SyncId,Ma.Active,CreatedDate ";
               // cityQry = cityQry + " FROM MastLink ML INNER JOIN ViewGeo VG ON ML.LinkCode=VG.areaId Inner Join MastArea Ma On Vg.AreaId=Ma.AreaId ";
               // cityQry = cityQry + " where Vg.areaid in  (" + areaid + ")";
               //// cityQry = cityQry + "  where areaid in (" + areaid + ")) ";
               // cityQry = cityQry + "  order by VG.areaName";
            } 
              //  str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' " +  condtion +" order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                 }
             }
            }
            catch (Exception ex)
            {

                throw;
            }
             //if (list.Count == 1)
             //    PopulateCities(dt.Rows[0]["Id"].ToString());
             return list;
        }
        [WebMethod(EnableSession = true)]
        public static ArrayList PopulateCityByMultiState(string StateID)
        {
            ArrayList list = new ArrayList();
            if (StateID != "")
            {
                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId in (" + StateID + ") and cityact=1 order by CityName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new ListItem(dr["CityName"].ToString(), dr["CityID"].ToString()));
                    }
                }
            }
            return list;
        }
        
        [WebMethod(EnableSession = true)]
        public static ArrayList BindDistributor(string CityId)
        {
            ArrayList list = new ArrayList();
               string strQ = "";
               try
               {

           
            if (CityId != "")
            {

               if(Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
                 strQ = "select MP.PartyId As Id,MP.PartyName As Name from MastParty MP where MP.areaid in (" + CityId + ") and PartyDist=1 and Active=1 and partyid=" + Settings.Instance.DistributorID + " order by PartyName";
               else
                  strQ = "select MP.PartyId As Id,MP.PartyName As Name from MastParty MP where MP.areaid in (" + CityId + ") and PartyDist=1 and Active=1 order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                    }
                }
            }
               }
               catch (Exception ex)
               {

                   throw;
               }
            return list;
        }

        [WebMethod(EnableSession = true)]
        public static string BindRetailer(string AreaId, string DistId)
        {
            ArrayList list = new ArrayList();
            string strQ = "";
            string res = "";
            try
            {

            if (AreaId != "" && DistId != "")
            {

                if (Settings.Instance.RoleType.ToUpper() == "DISTRIBUTOR")
                    strQ = "select MP.PartyId As Id,MP.PartyName + ' - ' + MP.Mobile + ' - ' + MA.AreaName As Name from MastParty MP left join MastArea MA on MA.AreaId=MP.areaid where MP.areaid in (" + AreaId + ") and MP.UnderId in (" + DistId + ") and PartyDist=0 and MP.Active=1 and partyid=" + Settings.Instance.DistributorID + " order by PartyName";
                else
                    strQ = "select MP.PartyId As Id,MP.PartyName + ' - ' + MP.Mobile + ' - ' + MA.AreaName As Name from MastParty MP left join MastArea MA on MA.AreaId=MP.areaid where MP.areaid in (" + AreaId + ") and MP.UnderId in (" + DistId + ") and PartyDist=0 and MP.Active=1 order by PartyName";

                    //strQ = "select MP.PartyId As Id,MP.PartyName + ' - ' + MP.Mobile + ' - ' + MA.AreaName As Name from MastParty MP left join MastArea MA on MA.AreaId=MP.areaid where MP.Partyid in (10013) and PartyDist=0 and MP.Active=1 order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
                    //}


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        res += "<option  value=" + dt.Rows[i]["Id"] + ">" + dt.Rows[i]["Name"] + "</option>";
                    }

                }
            }

            }
            catch (Exception ex)
            {

                throw;
            }
            return res;
        }

        protected void ExportCSV(object sender, EventArgs e)
       
        {


            if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
            {
              //  GetDailyWorkingSummaryL1(smIDStr1, frmTextBox.Text, toTextBox.Text, userIdStr, "0");
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date cannot be less than From Date.');", true);
                return;
            }

            string str = "";
            string condition = "";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerDispatchorderdetail.csv");
            string headertext = "DocId".TrimStart('"').TrimEnd('"') + "," + "Party Name".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "SalesPerson".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "orderdate".TrimStart('"').TrimEnd('"') + "," + "Total Quantity".TrimStart('"').TrimEnd('"') + "," + "Total Amount".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();

            string hiddistributor1 = Request.Form[hiddistributor.UniqueID];
            string hidretailer1 = Request.Form[hidretailer.UniqueID];

            if (hiddistributor1 != "")
            {
                condition = "where TD.DistID  in (" + hiddistributor1 + ") ";
            }

            if (txtfmDate.Text != "")
            {
                if (condition != "")
                {
                    condition = condition + " and TD.vdate>='" + txtfmDate.Text + "'";
                }
                else
                {
                    condition = " where TD.vdate>='" + txtfmDate.Text + "'";
                }

            }
            if (txttodate.Text != "")
            {
                if (condition != "")
                {
                    condition = condition + " and TD.vdate<='" + txttodate.Text + "'";
                }
                else
                {
                    condition = " where TD.vdate<='" + txttodate.Text + "'";
                }

            }
            if (ddlstatus.SelectedValue != "0")
            {
                if (condition != "")
                {
                    condition = condition + " and status='" + ddlstatus.SelectedValue + "'";
                }
                else
                {
                    condition = condition + " where status='" + ddlstatus.SelectedValue + "'";
                }

            }

            if (hidretailer1 != "")
            {
                if (condition != "")
                {
                    condition = condition + " and TD.PartyId  in (" + hidretailer1 + ") ";
                }
                else
                {
                    condition = condition + " where TD.PartyId  in (" + hidretailer1 + ") ";
                }
            }
            //   condition = "where TD.DistID in (" + Distid + ") and TD.orderdate>='" + Fromdate + "' and TD.orderdate<='" + Todate + "' ";



            str = "SELECT TD.Docid ,TD.PartyName,Isnull(MP1.PartyName,''),Isnull(TD.SMName,'') As SMName,TD.Address,Cast(TD.Mobile As varchar),TD.orderdate1,TD.totalqty,TD.totalamount, TD.Status from( SELECT vdate,  Replace(Convert(VARCHAR,Created_date,106),' ','/')+' '+LEFT(Convert(VARCHAR,Created_date,108),5) As createddatetime,  Replace(Convert(VARCHAR,TO2.Mobile_Created_date,106),' ','/') AS orderdate1,   T.Docid ,T.PartyName,T.PartyId ,T.Address,T.Mobile,TO2.Created_date  As orderdate,T.totalqty,T.totalamount,T.DistID,TO2.smid ,MS.SMName, TO2.Remarks,  (CASE WHEN Isnull(TO2.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(TO2.DispatchCancelType,'P')='C'    THEN 'Cancelled' ELSE 'Dispatched' END) AS Status   From(SELECT TO1.OrdDocId AS Docid ,Max(MP.PartyName) ";
            str = str + " AS PartyName,Max(MP.PartyId) AS PartyId ,Isnull(Max(MP.Address1),'')+' '+Isnull(Max(MP.Address2),'') AS Address,   Max(MP.Mobile) AS Mobile, Max(TO1.VDate) AS orderdate,Sum(TO1.Qty) AS totalqty,Sum(TO1.amount)- Isnull(Sum(DiscountAmount),0) AS totalamount,   Max(TO1.Distid) AS DistID  FROM TransOrder1 TO1 LEFT JOIN MastParty MP ON MP.PartyId =TO1.PartyId ";
            str = str + " GROUP BY TO1.OrdDocId)AS T LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=T.Docid LEFT JOIN MastSalesRep MS ON MS.Smid=TO2.SMId";
            str = str + " UNION ALL ";
            str = str + " SELECT vdate, Replace(Convert(VARCHAR,Created_date,106),' ','/')+' '+LEFT(Convert(VARCHAR,Created_date,108),5) As createddatetime,   Replace(Convert(VARCHAR,TO2.Mobile_Created_date,106),' ','/') AS orderdate1, T.Docid ,T.PartyName,T.PartyId ,T.Address,T.Mobile,TO2.Created_date  As orderdate,T.totalqty,T.totalamount,T.DistID,TO2.smid ,MS.SMName, TO2.Remarks,(CASE WHEN Isnull(TO2.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(TO2.DispatchCancelType,'P')='C' THEN 'Cancelled' ELSE 'Dispatched' END) AS Status ";
            str = str + "  From(SELECT TO1.OrdDocId AS Docid ,Max(MP.PartyName) AS PartyName,Max(MP.PartyId) AS PartyId ,Isnull(Max(MP.Address1),'')+' '+Isnull(Max(MP.Address2),'') AS Address,Max(MP.Mobile) AS Mobile, Max(TO1.VDate) AS orderdate,Sum(TO1.Qty) AS totalqty,Sum(TO1.amount)- Isnull(Sum(DiscountAmount),0) AS totalamount,Max(TO1.Distid) AS DistID ";
            str = str + " FROM Temp_TransOrder1 TO1 LEFT JOIN MastParty MP ON MP.PartyId =TO1.PartyId GROUP BY TO1.OrdDocId)AS T LEFT JOIN Temp_TransOrder TO2 ON TO2.OrdDocId=T.Docid LEFT JOIN MastSalesRep MS ON MS.Smid=TO2.SMId)AS TD  Left join mastParty MP1 on  MP1.PartyId=TD.DistId " + condition + " order by TD.orderdate desc";
         
            //str = "SELECT  TD.Docid ,TD.PartyName,Isnull(MP1.PartyName,''),Isnull(TD.SMName,'') As SMName,TD.Address,Cast(TD.Mobile As varchar),TD.orderdate1,TD.totalqty,TD.totalamount, TD.Status  from( SELECT Replace(Convert(VARCHAR,TO2.Created_date,106),' ','/') AS orderdate1,   T.Docid ,T.PartyName,T.PartyId ,T.Address,T.Mobile,T.orderdate,T.totalqty,T.totalamount,T.DistID,TO2.smid ,MS.SMName, TO2.Remarks,  (CASE WHEN Isnull(TO2.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(TO2.DispatchCancelType,'P')='C'    THEN 'Cancelled' ELSE 'Dispatched' END) AS Status   From(SELECT TO1.OrdDocId AS Docid ,Max(MP.PartyName) ";
            //str = str + " AS PartyName,Max(MP.PartyId) AS PartyId ,Isnull(Max(MP.Address1),'')+' '+Isnull(Max(MP.Address2),'') AS Address,   Max(MP.Mobile) AS Mobile, Max(TO1.VDate) AS orderdate,Sum(TO1.Qty) AS totalqty,Sum(TO1.amount) AS totalamount,   Max(TO1.Distid) AS DistID  FROM TransOrder1 TO1 LEFT JOIN MastParty MP ON MP.PartyId =TO1.PartyId ";
            //str = str + " GROUP BY TO1.OrdDocId)AS T LEFT JOIN TransOrder TO2 ON TO2.OrdDocId=T.Docid LEFT JOIN MastSalesRep MS ON MS.Smid=TO2.SMId";
            //str = str + " UNION ALL ";
            //str = str + " SELECT Replace(Convert(VARCHAR,TO2.Created_date,106),' ','/') AS orderdate1, T.Docid ,T.PartyName,T.PartyId ,T.Address,T.Mobile,T.orderdate,T.totalqty,T.totalamount,T.DistID,TO2.smid ,MS.SMName, TO2.Remarks,(CASE WHEN Isnull(TO2.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(TO2.DispatchCancelType,'P')='C' THEN 'Cancelled' ELSE 'Dispatched' END) AS Status ";
            //str = str + "  From(SELECT TO1.OrdDocId AS Docid ,Max(MP.PartyName) AS PartyName,Max(MP.PartyId) AS PartyId ,Isnull(Max(MP.Address1),'')+' '+Isnull(Max(MP.Address2),'') AS Address,Max(MP.Mobile) AS Mobile, Max(TO1.VDate) AS orderdate,Sum(TO1.Qty) AS totalqty,Sum(TO1.amount) AS totalamount,Max(TO1.Distid) AS DistID ";
            //str = str + " FROM Temp_TransOrder1 TO1 LEFT JOIN MastParty MP ON MP.PartyId =TO1.PartyId GROUP BY TO1.OrdDocId)AS T LEFT JOIN Temp_TransOrder TO2 ON TO2.OrdDocId=T.Docid LEFT JOIN MastSalesRep MS ON MS.Smid=TO2.SMId)AS TD  Left join mastParty MP1 on  MP1.PartyId=TD.DistId " + condition + " order by TD.orderdate desc";

            dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {


               
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {

                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {


                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                        }
                        else
                        {

                            sb.Append(dtParams.Rows[j][k].ToString() + ',');


                        }
                  
                }
                sb.Append(Environment.NewLine);
            }

            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerDispatchorderdetail.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void btnexportdetail_Click(object sender, EventArgs e)
        {
            string str = "";
            string condition = "";
            decimal DisAmount = 0;
            decimal OrdAmount = 0;
            decimal NetAmount = 0;
            decimal Dispatchqty = 0;
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerOrderDetail.csv");
         
            DataTable dtParams = new DataTable();

            int cnt = 0;


            cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select count(*) From transorder where OrdDocId = '" + Request.Form[hiddendocid.UniqueID] + "'"));
            if (cnt > 0)
            {

                str = "SELECT Replace(Convert(VARCHAR,dispatchcanceldatetime,106),' ','/')+' '+LEFT(Convert(VARCHAR,dispatchcanceldatetime,108),5) As dispatchcanceldatetime,   isnull(MP1.PartyName,'') As Distributor,Isnull(UN.Name,'') As Doneby, MP.PartyName AS PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') AS Address ,t.remarks,Replace(Convert(VARCHAR,t.vdate,106),' ','/') AS vdate, (CASE WHEN Isnull(t.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(t.DispatchCancelType,'P')='C'    THEN 'Cancelled' ELSE 'Dispatched' END) AS Status,t.DispatchRemarks As dispatchcancelremark, mi.ItemName as ItemName,t1.Qty as OrderQty,Case when t1.BaseUnitQty !=0 then cast(Isnull(t1.BaseUnitQty,0) as varchar)+ ' ' +t1.BaseUnit else ''  end + ' ' + Case when t1.PrimaryUnitQty !=0 then cast(Isnull(t1.PrimaryUnitQty,0) as varchar)+ ' ' +t1.PrimaryUnit else ''  end + ' ' + Case when t1.SecondaryUnitQty !=0 then cast(Isnull(t1.SecondaryUnitQty,0) as varchar)+ ' ' +t1.SecondaryUnit else ''  end as QtyDescription,t1.rate AS  rate,Isnull(t1.MarginPercentage,0) AS Margin,Isnull(t1.Discount,0) AS Discount,";
                str = str + "Isnull(t1.DiscountAmount,0) AS DiscountAmount,t1.amount as OrderAmount,(Isnull(t1.amount,0)-Isnull(t1.DiscountAmount,0)) as NetAmount,Isnull(t1.DispatchQty,0) As DispatchQty FROM TransOrder1 t1 left join transorder t on t.OrdDocId=t1.OrdDocId";
                str = str + " left join MastParty MP on MP.PartyId=t.PartyId";
                str = str + " left join mastitem mi on t1.ItemId = mi.ItemId ";
                str = str + " Left join (SELECT PartyName AS Name,userid FROM mastparty WHERE Isnull(userid,0)<>0 UNION SELECT EmpName AS Name,userid FROM MastSalesRep WHERE  Isnull(userid,0)<>0) UN on UN.userid=t.DispatchCancel_userd  Left join mastParty MP1 on  MP1.PartyId=t1.DistId where t1.OrdDocId = '" + Request.Form[hiddendocid.UniqueID] + "'";
            }
            else
            {
                str = "SELECT Replace(Convert(VARCHAR,dispatchcanceldatetime,106),' ','/')+' '+LEFT(Convert(VARCHAR,dispatchcanceldatetime,108),5) As dispatchcanceldatetime,   isnull(MP1.PartyName,'') As Distributor,Isnull(UN.Name,'') As Doneby, MP.PartyName AS PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') AS Address,t.remarks,Replace(Convert(VARCHAR,t.vdate,106),' ','/') AS vdate, (CASE WHEN Isnull(t.DispatchCancelType,'P')='P' THEN 'Pending' WHEN Isnull(t.DispatchCancelType,'P')='C'    THEN 'Cancelled' ELSE 'Dispatched' END) AS Status,t.DispatchRemarks As dispatchcancelremark,mi.ItemName as ItemName,t1.Qty as OrderQty,Case when t1.BaseUnitQty !=0 then cast(Isnull(t1.BaseUnitQty,0) as varchar)+ ' ' +t1.BaseUnit else ''  end + ' ' + Case when t1.PrimaryUnitQty !=0 then cast(Isnull(t1.PrimaryUnitQty,0) as varchar)+ ' ' +t1.PrimaryUnit else ''  end + ' ' + Case when t1.SecondaryUnitQty !=0 then cast(Isnull(t1.SecondaryUnitQty,0) as varchar)+ ' ' +t1.SecondaryUnit else ''  end as QtyDescription,t1.rate AS  rate,Isnull(t1.MarginPercentage,0) AS Margin,Isnull(t1.Discount,0) AS Discount,";
                str = str + "Isnull(t1.DiscountAmount,0) AS DiscountAmount,t1.amount as OrderAmount,(Isnull(t1.amount,0)-Isnull(t1.DiscountAmount,0)) as NetAmount,Isnull(t1.DispatchQty,0) As DispatchQty  FROM Temp_TransOrder1 t1 left join Temp_TransOrder t on t.OrdDocId=t1.OrdDocId";
                str = str + " left join MastParty MP on MP.PartyId=t.PartyId";
                str = str + " left join mastitem mi on t1.ItemId = mi.ItemId ";
                str = str + " Left join (SELECT PartyName AS Name,userid FROM mastparty WHERE Isnull(userid,0)<>0 UNION SELECT EmpName AS Name,userid FROM MastSalesRep WHERE  Isnull(userid,0)<>0) UN on UN.userid=t.DispatchCancel_userd  Left join mastParty MP1 on  MP1.PartyId=t1.DistId where t1.OrdDocId = '" + Request.Form[hiddendocid.UniqueID] + "'";


            }
            dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, str);


            string headertext = " PartyName : ," + dtParams.Rows[0]["PartyName"].ToString() + " ( " + dtParams.Rows[0]["vdate"].ToString() + " ) ";

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
       sb.AppendLine(System.Environment.NewLine);
            headertext = " Address : ," + dtParams.Rows[0]["Address"].ToString() + "";
            sb.Append(headertext);
     sb.AppendLine(System.Environment.NewLine);

            headertext = " Remark While Punch Order : ," + dtParams.Rows[0]["remarks"].ToString() + "";
            sb.Append(headertext);
       sb.AppendLine(System.Environment.NewLine);

            headertext = " Docid : ," + Request.Form[hiddendocid.UniqueID] + "";
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);



            headertext = "Item Name".TrimStart('"').TrimEnd('"') + "," + "Order Qty".TrimStart('"').TrimEnd('"') + "," + "Order Qty Description".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Margin".TrimStart('"').TrimEnd('"') + "," + "Discount%".TrimStart('"').TrimEnd('"') + "," + "Discount Amount".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"') + "," + "Net Amount".TrimStart('"').TrimEnd('"') + "," + "Dispatch Qty".TrimStart('"').TrimEnd('"');

          
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);

           
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 9; k < dtParams.Columns.Count; k++)
                {



                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {


                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else
                    {

                        sb.Append(dtParams.Rows[j][k].ToString() + ',');


                    }


                    if (k==15)
                    {
                        DisAmount = DisAmount + Convert.ToDecimal(dtParams.Rows[j][k].ToString());
                    }
                    if (k == 16)
                    {
                        OrdAmount = OrdAmount + Convert.ToDecimal(dtParams.Rows[j][k].ToString());
                    }
                    if (k == 17)
                    {
                        NetAmount = NetAmount + Convert.ToDecimal(dtParams.Rows[j][k].ToString());
                    }
                    if (k == 18)
                    {
                        Dispatchqty = Dispatchqty + Convert.ToDecimal(dtParams.Rows[j][k].ToString());
                    }

                }
                sb.Append(Environment.NewLine);
            }
            sb.Append(',');
            sb.Append(',');
            sb.Append(',');
            sb.Append(',');
            sb.Append(',');
            sb.Append("Total :,");
            sb.Append(DisAmount + ",");
            sb.Append(OrdAmount + ",");
            sb.Append(NetAmount + ",");
            sb.Append(Dispatchqty + ",");
            sb.AppendLine(System.Environment.NewLine);
            headertext = " Status : ," + dtParams.Rows[0]["Status"].ToString() + "";
            sb.Append(headertext);
           sb.AppendLine(System.Environment.NewLine);


            headertext = " Remark : ," + dtParams.Rows[0]["dispatchcancelremark"].ToString() + "";
            sb.Append(headertext);
  sb.AppendLine(System.Environment.NewLine);

            headertext = " DoneBy : ," + dtParams.Rows[0]["Doneby"].ToString() + "";
            sb.Append(headertext);
         //   sb.AppendLine(System.Environment.NewLine);
          headertext = " Date : ," + dtParams.Rows[0]["dispatchcanceldatetime"].ToString() + "";
            sb.Append(headertext);
          //  sb.AppendLine(System.Environment.NewLine);
           
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerOrderDetail.csv");
            Response.Write(sb.ToString());
            Response.End();

          
        }


    }
}