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
using DAL;
using System.Web.Services;


namespace AstralFFMS
{
    public partial class OrderEntryItemWise : System.Web.UI.Page
    {
        BAL.Order.OrderEntryBAL dp = new BAL.Order.OrderEntryBAL();
        int PartyId = 0;
        int AreaId = 0;
        string parameter = "";
        string VisitID = "";
        string CityID = "";
        string Level = "0"; string pageSalesName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            parameter = Request["__EVENTARGUMENT"];

            if (parameter != "")
            {
                ViewState["CollId"] = parameter;            
                FillDeptControls(Convert.ToInt32(parameter));                
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Convert.ToInt32(Request.QueryString["PartyId"].ToString());
            }
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
            if (Request.QueryString["AreaId"] != null)
            {
                AreaId = Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            //Added
            if (Request.QueryString["PageView"] != null)
            {
                pageSalesName = Request.QueryString["PageView"].ToString();
            }
            //End
            if (!IsPostBack)
            {
                try
                {
                   
                    if (parameter == null )
                    {
                        //try
                        //{
                        //    string str = @"select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.visid=" + VisitID + " and a.partyid = " + PartyId + "";
                        //    DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        //    if (deptValueDt.Rows.Count > 0)
                        //    {
                        //        gridorder.DataSource = deptValueDt;
                        //        gridorder.DataBind();
                        //        //btnsave.Text = "Update";
                        //        btnDelete.Visible = false;
                        //        txtRemarks.Text = deptValueDt.Rows[0]["remarks"].ToString();
                        //    }
                        //    else
                        //    {
                        //        fillRepeteroredr();
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    ex.ToString();
                        //}

                        fillRepeteroredr();
                    }
                   
                  
                    try
                    {
                        lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                    // lblVisitDate5.Text = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString());
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;
                }
                catch
                {
                }
                GetPartyData(PartyId);
               // divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
            }
           
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {//Ankita - 12/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
                //customers.Add("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")");
            }
            return customers;
        }
        private void FillDeptControls(int OrdId)
        {
            try
            {
                string str ="";
               // string str = @"select * from Temp_TransOrder1  where OrdId=" + OrdId;
              //  string str = @"select m.ItemName,m.unit,t.* from Temp_TransOrder1 t,MastItem m where   t.ItemId=m.ItemId and t.ord1id=" + OrdId;
                if (OrdId != 0)
                {
                    str = @"select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark,a.ordid,'O' as [type]  from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.ord1id=" + OrdId;

                }
                else
                {
                    str = "Select itemid,Max(itemname) as itemname,max(unit) as unit,max(stdpack) as stdpack,max(mrp) as mrp,max(amt) as amt,Max(OrdDocId) as OrdDocId,max(qty) as qty,Max(ord1id) as ord1id,Max(remarks) as remarks,Max(remark) as remark,Max(ordid) as ordid,Max([type]) as [type]";
                    str = str + " From(select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark,a.ordid,'O' as [type] from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.PartyId=" + Convert.ToInt32(Request.QueryString["PartyId"].ToString()) + " AND c.VISID=" + Convert.ToInt32(Request.QueryString["VisitID"].ToString()) + "";
                    str = str + " Union all ";
                    str = str + " select a.itemid,b.itemname,0.00 as unit,b.stdpack,b.mrp,0.00 as amt,'' as OrdDocId,0.0 as qty,0 as ord1id,' 'as remarks,'' as remark,0 as ordid,'N' as [type]  from MastItemTemplat a,mastitem b where a.itemid=b.itemid and a.PartyId=" + Convert.ToInt32(Request.QueryString["PartyId"].ToString()) + ") as T Group by T.itemid";


                  //  str = @"select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark,a.ordid from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.PartyId=" + Convert.ToInt32(Request.QueryString["PartyId"].ToString());
                }
                    DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    gridorder.DataSource = deptValueDt;
                    gridorder.DataBind();
                    if (OrdId != 0)
                        btnsave.Text = "Update";
                    else
                        btnsave.Text = "Save";
                    btnDelete.Visible = false;
                    txtRemarks.Text=deptValueDt.Rows[0]["remarks"].ToString();
                   // divdocid.Visible = true;
                   // lbldocno.Text = deptValueDt.Rows[0]["OrdDocId"].ToString();
                }
                else
                {

                    gridorder.DataSource = null;
                    gridorder.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }      

        private void GetPartyData(int PartyId)
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select * from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            string str = "select PartyName,Address1,Address2,Mobile,Pin from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt1.Rows.Count > 0)
            {
                partyName.Text = dt1.Rows[0]["PartyName"].ToString();
                address.Text = dt1.Rows[0]["Address1"].ToString() + "" + dt1.Rows[0]["Address2"].ToString();
                mobile.Text = dt1.Rows[0]["Mobile"].ToString();
                lblzipcode.Text = dt1.Rows[0]["Pin"].ToString();
            }
        }
        private void clearcontrols()
        {
            //Remark.Text = "";
            //txtTotalAmount.Text = "0.00";
            btnDelete.Visible = false;
            btnsave.Text = "Save";
            fillRepeteroredr();
            txtRemarks.Text = null;
            //divdocid.Visible = false;
        }
        private void fillRepeter()
        {
            // Nishu 01/06/2016
            //string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
           // string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = @"select m.ItemName,t.* from Temp_TransOrder1 t,MastItem m where t.ItemId=m.ItemId and t.VisId=" + VisitID + " and t.SmId=" + Settings.Instance.DSRSMID + " and t.PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }
        private void fillRepeteroredr()
        {
            // Nishu 01/06/2016
            //string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            // string str = @"select * from Temp_TransOrder where  VisId=" + VisitID + " and SmId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = "Select itemid,Max(itemname) as itemname,max(unit) as unit,max(stdpack) as stdpack,max(mrp) as mrp,max(amt) as amt,Max(OrdDocId) as OrdDocId,max(qty) as qty,Max(ord1id) as ord1id,Max(remarks) as remarks,Max(remark) as remark,Max(ordid) as ordid,Max([type]) as [type]";
            str = str + " From(select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark,a.ordid,'O' as [type] from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.PartyId=" + PartyId + " and c.visid=" + VisitID + "";
             str = str + " Union all ";
             str = str + " select a.itemid,b.itemname,0.00 as unit,b.stdpack,b.mrp,0.00 as amt,'' as OrdDocId,0.0 as qty,0 as ord1id,' 'as remarks,'' as remark,0 as ordid,'N' as [type]  from MastItemTemplat a,mastitem b where a.itemid=b.itemid and a.PartyId=" + PartyId + ") as T Group by T.itemid";




            //string str = @"select a.itemid,b.itemname,0.00 as unit,b.stdpack,b.mrp,0.00 as amt,0.0 as qty,0 as ord1id,' 'as remarks  from MastItemTemplat a,mastitem b where a.itemid=b.itemid and a.PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(depdt.Rows.Count>0)
            {

                gridorder.DataSource = depdt;
                gridorder.DataBind();
            }
            else
            {
                txtRemarks.Visible = false;
                btnreset.Visible = false;
                btnsave.Visible = false;
                lbltxtRemarks.Visible = false;
                gridorder.DataSource = null;
                gridorder.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Make Item Template First');", true);
                return;
            }
            
        }
        private void InsertOrder(int index)
        {
            try
            {
                int RetSave = 0;
                string mszforatleastoneitem = "";
                if (gridorder.Rows.Count==0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Fill Item Template First');", true);
                    return;
                }              
                    
                    HiddenField hiditem1 = (gridorder.Rows[index].FindControl("HiddenField1") as HiddenField);
                    Label itemname = (gridorder.Rows[index].FindControl("lblitemname") as Label);
                    Label stdpack = (gridorder.Rows[index].FindControl("lblstdpack") as Label);
                    Label amount = (gridorder.Rows[index].FindControl("lblamount") as Label);
                    TextBox txtremark = (gridorder.Rows[index].FindControl("txtremark") as TextBox);
                    TextBox txtunit = (gridorder.Rows[index].FindControl("txtunit") as TextBox);
                    TextBox txtrate = (gridorder.Rows[index].FindControl("txtrate") as TextBox);
                    TextBox cases = (gridorder.Rows[index].FindControl("txtquantity") as TextBox);
                    double Cases1 = double.Parse(cases.Text, System.Globalization.CultureInfo.InvariantCulture);
                    double StdPack = double.Parse(stdpack.Text, System.Globalization.CultureInfo.InvariantCulture);
                    double Unit = double.Parse(txtunit.Text, System.Globalization.CultureInfo.InvariantCulture);
                    double Rate = double.Parse(txtrate.Text, System.Globalization.CultureInfo.InvariantCulture);
                    double TotalQty = ((Cases1 * StdPack) + Unit);
                    if ((TotalQty.ToString() == "0.00") || ((TotalQty.ToString() == "0")))
                    {
                        mszforatleastoneitem = "0";
                        return;
                       // continue;
                       // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select atleast one Cases or Unit');", true);
                    }
                    //if ((cases.Text == "0.00") || (txtunit.Text == "0"))
                    //{
                    //    //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select atleast one Cases or Unit');", true);
                    //    return;
                    //}                    
                    string docID = Settings.GetDocID("ORDSN", DateTime.Now);
                    Settings.SetDocID("ORDSN", docID);
                    int sno = 0;
                    int freeqty = 0;
                    int discount = 0;
                    int ord1id = Convert.ToInt32((DbConnectionDAL.GetScalarValue(CommandType.Text, "select ISNULL(max(ordid),0)+1 from temp_transorder1")).ToString());
                    RetSave = dp.InsertOrderEntryItemWise(Convert.ToInt64(VisitID), docID, Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.DSRSMID), PartyId, AreaId, ord1id, sno, Convert.ToInt32(hiditem1.Value), Convert.ToDecimal(TotalQty), Convert.ToDecimal(freeqty), Convert.ToDecimal(txtrate.Text.Trim()), Convert.ToDecimal(discount), txtRemarks.Text.Trim(), Convert.ToDecimal(amount.Text), Convert.ToDecimal(Cases1), Convert.ToDecimal(Unit));
                    mszforatleastoneitem = "1";
                    if (RetSave > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        btnDelete.Visible = false;
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        if (pageSalesName == "Secondary")
                        {
                            meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                        }
                        else
                        {
                            meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                        }
                        this.Page.Controls.Add(meta);
                    }
                    else
                    {
                        if (mszforatleastoneitem == "0")
                        {                            
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Qty. Should Be Greater Than 0');", true);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                        }
                    }
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        private void UpdateOrder(int index)
        {
            try
            {
                int RetSave = 0;
                string mszforatleastoneitem = "";

                HiddenField hiditem1 = (gridorder.Rows[index].FindControl("HiddenField1") as HiddenField);
                HiddenField hidord1id = (gridorder.Rows[index].FindControl("hidord1id") as HiddenField);
                Label itemname = (gridorder.Rows[index].FindControl("lblitemname") as Label);
                TextBox txtunit = (gridorder.Rows[index].FindControl("txtunit") as TextBox);
                TextBox txtrate = (gridorder.Rows[index].FindControl("txtrate") as TextBox);
                Label stdpack = (gridorder.Rows[index].FindControl("lblstdpack") as Label);
                Label amount = (gridorder.Rows[index].FindControl("lblamount") as Label);
                TextBox cases = (gridorder.Rows[index].FindControl("txtquantity") as TextBox);
                TextBox txtremark = (gridorder.Rows[index].FindControl("txtremark") as TextBox);
                double Cases1 = double.Parse(cases.Text, System.Globalization.CultureInfo.InvariantCulture);
                double StdPack = double.Parse(stdpack.Text, System.Globalization.CultureInfo.InvariantCulture);
                double Unit = double.Parse(txtunit.Text, System.Globalization.CultureInfo.InvariantCulture);
                double Rate = double.Parse(txtrate.Text, System.Globalization.CultureInfo.InvariantCulture);
                double TotalQty = ((Cases1 * StdPack) + Unit);

                if ((TotalQty.ToString() == "0.00") || ((TotalQty.ToString() == "0")))
                {
                    mszforatleastoneitem = "0";
                    return;
                }
                string docID = Settings.GetDocID("ORDSN", DateTime.Now);
                Settings.SetDocID("ORDSN", docID);
                int ord1id = Convert.ToInt32((DbConnectionDAL.GetScalarValue(CommandType.Text, "select ISNULL(max(ordid),0)+1 from temp_transorder1")).ToString());
                RetSave = dp.UpdateOrderEntryItemWise(Convert.ToInt64(ViewState["CollId"]), Convert.ToInt64(VisitID), Convert.ToInt32(Settings.Instance.UserID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.DSRSMID), PartyId, AreaId, txtRemarks.Text.Trim(), Convert.ToDecimal(amount.Text), Convert.ToInt32(hidord1id.Value), Convert.ToInt32(hiditem1.Value), Convert.ToDecimal(TotalQty), Convert.ToDecimal(Rate), Convert.ToDecimal(Cases1), Convert.ToDecimal(Unit));
                mszforatleastoneitem = "1";
                if (RetSave > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    btnDelete.Visible = false;
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    if (pageSalesName == "Secondary")
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                    }
                    else
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                    }

                    this.Page.Controls.Add(meta);
                }
                else
                {
                    if (mszforatleastoneitem == "0")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Item Qty. Should Be Greater Than 0');", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {


                for (int i = 0; i < gridorder.Rows.Count; i++)
                {

                    HiddenField hiditype = (gridorder.Rows[i].FindControl("Hidtype") as HiddenField);
                    if (hiditype.Value == "N")
                    {
                        InsertOrder(i);
                    }
                    else
                    {
                        UpdateOrder(i);
                    }
                }
                //if (btnsave.Text == "Update")
                //{
                //    UpdateOrder();
                //}
                //else
                //{
                //    InsertOrder();
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            clearcontrols();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //int retdel = dp.delete_itemwise(Convert.ToString(ViewState["CollId"]));
                //if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnsave.Text = "Save";
                    clearcontrols();
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    if (pageSalesName == "Secondary")
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName;
                    }
                    else
                    {
                        meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                    }

                    this.Page.Controls.Add(meta);
                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = false;
            btnsave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
            txtRemarks.Text = null;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnBack_Click1(object sender, EventArgs e)
        {
            if (pageSalesName == "Secondary")
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName);
            }
            else
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
        }  
        protected void txtquantity_TextChanged2(object sender, EventArgs e)
        {             
            TextBox txtquantity = sender as TextBox;   
            GridViewRow row = (GridViewRow)txtquantity.Parent.Parent;            
            Label lblStdPack = (row.FindControl("lblstdpack") as Label);
            TextBox cases = (row.FindControl("txtquantity") as TextBox);
            TextBox txtunit = (row.FindControl("txtunit") as TextBox);
            TextBox txtrate = (row.FindControl("txtrate") as TextBox);
            if (string.IsNullOrEmpty(cases.Text) == true)
            {
                cases.Text = "0.00";
            }
            if (string.IsNullOrEmpty(txtunit.Text) == true)
            {
                txtunit.Text = "0.00";
            }
            if (string.IsNullOrEmpty(txtrate.Text) == true)
            {
                txtrate.Text="0.00";
            }
            double Cases1 = double.Parse(cases.Text, System.Globalization.CultureInfo.InvariantCulture);
            double StdPack = double.Parse(lblStdPack.Text, System.Globalization.CultureInfo.InvariantCulture);
            double Unit = double.Parse(txtunit.Text, System.Globalization.CultureInfo.InvariantCulture);
            double Rate = double.Parse(txtrate.Text, System.Globalization.CultureInfo.InvariantCulture);
            double TotalQty = ((Cases1 * StdPack) + Unit);
            double Amount = (TotalQty * Rate) + 0.00; ;
            Label lblamt = (row.FindControl("lblamount") as Label);
            lblamt.Text = Convert.ToString(Amount);
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            string[] ordid = e.CommandArgument.ToString().Split(';');
           // int id = Convert.ToInt32(e.CommandArgument.ToString());
            int id = Convert.ToInt32(ordid[0]);
            int id1 = Convert.ToInt32(ordid[1]);
            int retdel;
            //string distqry = @"delete from temp_transorder1 where  ord1id=" + id;
            //if ((DbConnectionDAL.ExecuteQuery(distqry)) == 1)
           // int retdel = dp.delete_itemwise(Convert.ToString(id),Convert.ToString(ViewState["CollId"]));
            if (Convert.ToString(ViewState["CollId"]) == "")
            {
                retdel = dp.delete_itemwise(Convert.ToString(id), Convert.ToString(id1));
            }
            else
            {
                retdel = dp.delete_itemwise(Convert.ToString(id), Convert.ToString(ViewState["CollId"]));
            }
            if (retdel > 1)
            {
                DataTable dtchkchild = new DataTable();
                if (Convert.ToString(ViewState["CollId"]) != "")
                {
                     dtchkchild = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from temp_transorder1 where ordid=" + id1 + "");
                }
                else
                {
                    dtchkchild = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from temp_transorder1 where ordid=" + id1 + "");
                }

                    if (dtchkchild.Rows.Count == 0)
                    {
                        string deleteheader ="";
                            //if (Convert.ToString(ViewState["CollId"]) != "")
                            //   deleteheader = "delete from  temp_transorder where ordid=" + Convert.ToInt32(Convert.ToString(ViewState["CollId"])) + "";
                            //else
                                deleteheader = "delete from  temp_transorder where ordid=" + id1 + "";

                        DbConnectionDAL.ExecuteQuery(deleteheader);
                        //txtRemarks.Visible = false;
                        //btnreset.Visible = false;
                        //btnsave.Visible = false;
                        //lbltxtRemarks.Visible = false;
                        //btnreset_Click(null, null);
                    }
                    else
                    {
                        FillDeptControls(Convert.ToInt32(ViewState["CollId"]));
                    }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);              
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Failed To Delete Item');", true);              
            }
            //if (Convert.ToString(ViewState["CollId"]) != "")
            //{
                FillDeptControls(Convert.ToInt32(ViewState["CollId"]));
            //}else
            //{
            //    FillDeptControls(id1);
            //}
        }

        protected void gridorder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               if (e.Row.RowType == DataControlRowType.DataRow)           
                {
                    HiddenField hid = e.Row.FindControl("hidord1id") as HiddenField;
                    if(hid.Value=="0")
                    {
                        LinkButton lnkWebURL = e.Row.FindControl("LinkButton1") as LinkButton;                        
                        {
                            lnkWebURL.Visible = false;
                        }
                    }
                    string str = @"select distinct( a.itemid),b.itemname,a.unit,b.stdpack,a.rate as mrp,a.amount as amt,a.OrdDocId,a.cases as qty,a.ord1id,a.remarks,c.remark from  temp_transorder c left join   temp_transorder1 a on a.visid=c.visid left join mastitem b on  a.itemid=b.itemid where a.visid=" + Convert.ToInt32(Request.QueryString["VisitID"].ToString()) + " and a.partyid = " + Convert.ToInt32(Request.QueryString["PartyId"].ToString()) + "";
                    DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (deptValueDt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= deptValueDt.Rows.Count - 1; i++)
                        {                            
                            //txtRemarks.Text = deptValueDt.Rows[0]["remarks"].ToString();
                        }
                    }                  
                }  
        
            }
        }
    }
}