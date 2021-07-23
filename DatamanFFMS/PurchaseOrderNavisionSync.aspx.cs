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

namespace AstralFFMS
{
    public partial class PurchaseOrderNavisionSync : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DistributorApp_SyncNavisitionPurchaseOrder();               
            }
            catch (Exception ex)
            {
                ex.ToString();
               // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        public void DistributorApp_SyncNavisitionPurchaseOrder()
        {
            try
            {
                string strheader = @"SELECT PODocId,vdate AS [VDate],DistributorSyncId AS DSyncID,replace(mp.partyName,'''','' ) AS DName,Ma.SyncId AS DPostCode,Remarks,
                 [DivisionCode],Isnull([PendingOrder],0) as [PendingOrder] FROM TransPurchOrder tp left Join MastParty mp on tp.Distid=mp.partyId left join mastArea ma on 
                 Mp.areaid=ma.AreaId where mp.partydist=1 and orderDownLoaded IS Null";

                DataTable dtheader = DbConnectionDAL.GetDataTable(CommandType.Text, strheader);
                if (dtheader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtheader.Rows.Count; i++)
                    {
                        DateTime vdateheader = Convert.ToDateTime(dtheader.Rows[i]["VDate"]);
                        string headerdate = vdateheader.ToString("yyyy-MM-dd HH:mm:ss.fff");

                        string insertheader = @"INSERT INTO [dbo].[Shubham Goldiee Masale (P) Ltd$MSales Header] ([PODocID],[Vdate],[DSyncID],[DName],[DPostCode],[ERPOrderID],[Remark],[Division Code],[SO Created],[Created SO NO],[Pending Cancel]) VALUES  ('" + dtheader.Rows[i]["PODocId"] + "','" + headerdate + "','" + dtheader.Rows[i]["DSyncID"] + "', '" + dtheader.Rows[i]["DName"] + "','" + dtheader.Rows[i]["DPostCode"] + "','" + string.Empty + "','" + dtheader.Rows[i]["Remarks"] + "','" + dtheader.Rows[i]["DivisionCode"] + "',0, '" + string.Empty + "','" + dtheader.Rows[i]["PendingOrder"] + "')";
                        int existheader = Convert.ToInt32(DbConnectionDAL.ExecuteQuerynavision(insertheader));
                        string updateheader = @" UPDATE TransPurchOrder SET [OrderDownloaded] ='Y' where PODocId = '" + dtheader.Rows[i]["PODocId"] + "'";
                        int updateexistsheader = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(updateheader));
                    }
                }

                string strline = @"SELECT POrd1ID, PODocId,vdate AS [VDate],DistributorSyncId AS DSyncID,mi.SyncId AS [ItemSyncId],mi.ItemName AS [Item Name],
                 tp.Qty, tp.[DivisionCode] FROM TransPurchOrder1 tp left JOIN mastitem mi on  
                 tp.ItemId = mi.ItemId where orderDownLoaded IS Null";

                DataTable dtline = DbConnectionDAL.GetDataTable(CommandType.Text, strline);
                if (dtline.Rows.Count > 0)
                {
                    for (int j = 0; j < dtline.Rows.Count; j++)
                    {
                        DateTime vdateline = Convert.ToDateTime(dtline.Rows[j]["VDate"]);
                        string linedate = vdateline.ToString("yyyy-MM-dd HH:mm:ss.fff");

                        string insertline = @"INSERT INTO [dbo].[Shubham Goldiee Masale (P) Ltd$MSales Line] ([PODocID],[Vdate],[DSyncID],[ItemSyncID],[Item Name],[Quantity],[Document Type],[Division]) VALUES  ('" + dtline.Rows[j]["PODocId"] + "','" + linedate + "','" + dtline.Rows[j]["DSyncID"] + "', '" + dtline.Rows[j]["ItemSyncId"] + "','" + dtline.Rows[j]["Item Name"] + "','" + dtline.Rows[j]["Qty"] + "','" + string.Empty + "','" + dtline.Rows[j]["DivisionCode"] + "')";
                        int existline = Convert.ToInt32(DbConnectionDAL.ExecuteQuerynavision(insertline));

                        string updateline = @" UPDATE TransPurchOrder1 SET [OrderDownloaded] ='Y' where POrd1ID = " + dtline.Rows[j]["POrd1ID"] + "";
                        int updateexistsline = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(updateline));
                    }
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Purchase Order Downloaded Successfully');", true);
            }
            catch (Exception ex)
            {
                //ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while download the Purchase Order');", true);
            }

        }        
    }
}