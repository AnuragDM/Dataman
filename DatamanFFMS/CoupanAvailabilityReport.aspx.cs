using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;
using System.Drawing;
using System.Text;
using System.Net;

namespace AstralFFMS
{
    public partial class CoupanAvailabilityReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {         
           
            if (!IsPostBack)
            {
                BindCoupanScheme();
                BindZone();
                BindDistributor();
                Repeater1.Visible = false;
                lblnouser.Visible = false;
                btnNOUExport.Visible = false;             
                string pageName = Path.GetFileName(Request.Path);

                if (btnexport.Text == "Export")
                {
                    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
                    btnNOUExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnNOUExport.CssClass = "btn btn-primary";
                }
            }
        }
        private void BindCoupanScheme()
        {
            try
            {
                string str = @"SELECT SchemeId,SchemeName FROM MastCoupanScheme where Active=1 ORDER BY SchemeName";
                fillDDLDirect(ddlScheme, str, "SchemeId", "SchemeName", 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindZone()
        {
            try
            {
                string str = @"SELECT distinct Zone FROM MastZone where Active=1 ORDER BY Zone";
                fillDDLDirect(ddlZone, str, "Zone", "Zone", 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private Int32 BindPrefix(string zone)
        {
            try
            {
                string str = @"SELECT Prefix FROM MastZone where Active=1 and Zone='" + zone + "' ORDER BY Prefix";
                fillDDLDirect(ddlPrefix, str, "Prefix", "Prefix", 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }
        private void BindDistributor()
        {
            try
            {
                string str = @"SELECT mp.PartyId,(mp.PartyName + ' - ' + ma.AreaName) AS PartyName FROM MastParty mp LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId where mp.partydist=1 and mp.Active=1 ORDER BY mp.PartyName";
                fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName", 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }
      

        
//        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
//        {
//            if (e.CommandName == "MeetEdit")
//            {
//                Repeater1.Visible = true;
//                Repeater1.DataSource = null;
//                Repeater1.DataBind();
//                rptnoofusers.DataSource = null;
//                rptnoofusers.DataBind();                
//                lblnouser.Visible = false;                
//                string str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
//                from TransMeetPlanEntry M left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
//  	            left join MastItemClass MI on m.IndId=MI.Id  where  M.MeetPlanId=" + e.CommandArgument.ToString();
//                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
//                if (dt.Rows.Count > 0)
//                {
//                    Repeater1.DataSource = dt;
//                    Repeater1.DataBind();
//                }

//                string str1 = "  select l.*,MST.SMName,E.MeetDate,E.MeetName,ma.AreaName,mb.AreaName as BeatName from TransAddMeetUser l left join TransMeetPlanEntry E on l.[MeetId]=e.MeetPlanId  left join PartyType P on l.PartyType=p.PartyTypeId Left Join MastSalesRep MST on MST.SMId = E.SMId  Left Join MastArea ma on l.AreaId=ma.AreaId     Left Join MastArea mb on l.AreaId=mb.AreaId  where l.MeetId=" + Settings.DMInt32(e.CommandArgument.ToString()) + " order by l.ContactPersonName";
//                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
              
//                if (dt1.Rows.Count > 0)
//                {
//                    rptnoofusers.DataSource = dt1;
//                    rptnoofusers.DataBind();                  
//                    btnNOUExport.Visible = true;
//                    lblnouser.Visible = true;
//                }

//             string str2 = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
//             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
//             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + Settings.DMInt32(e.CommandArgument.ToString());
//                DataTable dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str2);
//                if (dt2.Rows.Count > 0)
//                {
                  
//                }

//                string str3 = "  select l.*,E.MeetName as Meet from TransMeetImage l left join TransMeetPlanEntry E on l.[MeetPlanID]=E.MeetPlanId  where l.MeetPlanID=" + Settings.DMInt32(e.CommandArgument.ToString()) + " order by E.MeetName";
//                DataTable dt3 = DbConnectionDAL.GetDataTable(CommandType.Text, str3);
//                if (dt3.Rows.Count > 0)
//                {
                                  
//                }

//            }
//        }
//        protected void rptImageUpload_ItemCommand(object source, RepeaterCommandEventArgs e)
//        {
//            if (e.CommandName == "ShowImage")
//            {
//                try
//                {
//                    Control control;
//                    control = e.Item.FindControl("ImageId");
//                    string str = string.Format("Select imgUrl,imgName from TransMeetImage Where Id={0}", e.CommandArgument.ToString());
//                    DataTable getdata = DbConnectionDAL.GetDataTable(CommandType.Text, str);
//                    string url = Convert.ToString(getdata.Rows[0]["imgUrl"]);
//                    string imgName = Convert.ToString(getdata.Rows[0]["imgName"]);
//                    Response.ContentType = "image/jpg";
//                    string filePath = url;
//                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + imgName + "\"");
//                    Response.TransmitFile(Server.MapPath(filePath));
//                    Response.End();
//                }
//                catch (Exception ex)
//                {
//                    ex.ToString();
//                }

//            }
//        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillCoupan();
        }

        private void fillCoupan()
        {
            try
            {                
                    string str = "";
                    string stradd = "";
                    if(ddlZone.SelectedValue!= "0")
                    {                       
                        stradd = " AND tc.Zone='" + ddlZone.SelectedValue + "'";
                    }
                    if(ddlPrefix.SelectedValue != "")
                    {
                        stradd +=   "AND tc.Prefix='" + ddlPrefix.SelectedValue + "'";
                    }                    
                    if(ddlDistributor.SelectedValue !="0")
                    {
                        stradd +=   "AND tc.distid = '" + ddlDistributor.SelectedValue + "'";
                    }
                    if(txtstartCoupon.Text !="")
                    {
                        stradd += "AND tc.CoupanNo BETWEEN '" + txtstartCoupon.Text + "' AND '" + txtEndCoupon.Text + "'";
                    }
                    str = @"SELECT mp.PartyId, mp.SyncId,mp.PartyName,ma.AreaName AS Area,ma1.AreaName AS City, (SELECT count(*) FROM TransCoupan tc1 WHERE tc1.DistId=mp.PartyId) AS IssuedCoupan , (SELECT count(*) FROM TransCoupan tc1 WHERE tc1.RetailerId IS NULL AND tc1.DistId=mp.PartyId) AS AvailableCoupons FROM TransCoupan tc LEFT JOIN mastparty mp ON tc.DistId=mp.PartyId LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId LEFT JOIN mastarea ma1 ON mp.CityId=ma1.AreaId
                    where mp.partydist=1 and DistId IS NOT NULL and tc.SchemeId=" + ddlScheme.SelectedValue + " " + stradd + "  GROUP BY mp.PartyId, mp.PartyName,mp.SyncId,ma.AreaName,ma1.AreaName ORDER BY mp.PartyName";             

                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        rpt.DataSource = dt;
                        rpt.DataBind();
                    }
                    else
                    {
                        rpt.DataSource = null;
                        rpt.DataBind();
                    }               
                
            }
            catch { }
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MeetReport.csv");
            string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Sales Person Code".TrimStart('"').TrimEnd('"') + "," + "Meet Date".TrimStart('"').TrimEnd('"') + "," + "MeetName".TrimStart('"').TrimEnd('"') + "," + "Venue".TrimStart('"').TrimEnd('"') + "," + "Party Name".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Distributor Code".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "City Code".TrimStart('"').TrimEnd('"') + "," + "City Type".TrimStart('"').TrimEnd('"') + "," + "Planned Users".TrimStart('"').TrimEnd('"') + "," + "Type Of Gift".TrimStart('"').TrimEnd('"') + "," + "Distributor Sharing %".TrimStart('"').TrimEnd('"') + "," + "Astral Sharing %".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Approx Budget".TrimStart('"').TrimEnd('"') + "," + "Approved Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Remark".TrimStart('"').TrimEnd('"') + "," + "Expense Approved Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Approved Remarks".TrimStart('"').TrimEnd('"') + "," + "Meet Status".TrimStart('"').TrimEnd('"') + "," + "Approval Remark".TrimStart('"').TrimEnd('"') + "," + "Approval Date".TrimStart('"').TrimEnd('"') + "," + "Meet Type".TrimStart('"').TrimEnd('"') + "," + "No Of Staff".TrimStart('"').TrimEnd('"') + "," + "Comments".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Qty Required".TrimStart('"').TrimEnd('"') + "," + "Actual User Count".TrimStart('"').TrimEnd('"') + "," + "Balance Qty".TrimStart('"').TrimEnd('"') + "," + "Image Upload".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPersonCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Venue", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Distributor", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DistributorCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CityCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CityType", typeof(String)));

            dtParams.Columns.Add(new DataColumn("NoofUsers", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TypeOfGift", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpShareDist", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpShareSelf", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProductClass", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ApproxBudget", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ApprovedAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseApprovedAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseApprovedRemarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetStatus", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Approval/CancelRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Approval/CancelDate", typeof(String)));

            dtParams.Columns.Add(new DataColumn("MeetTypeName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NoStaff", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Comments", typeof(String)));
            dtParams.Columns.Add(new DataColumn("BeatName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("valueofRetailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Actualusercount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("BalanceQty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ImageUpload", typeof(String)));
            foreach (RepeaterItem item in rpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label SMNameLabel = item.FindControl("SMNameLabel") as Label;
                dr["SalesPerson"] = SMNameLabel.Text;
                Label SAPCodeLabel = item.FindControl("SAPCodeLabel") as Label;
                dr["SalesPersonCode"] = SAPCodeLabel.Text;
                Label MeetDateLabel = item.FindControl("MeetDateLabel") as Label;
                dr["MeetDate"] = MeetDateLabel.Text.ToString();
                Label MeetNameLabel = item.FindControl("MeetNameLabel") as Label;
                dr["MeetName"] = MeetNameLabel.Text.ToString();
                Label VenueLabel = item.FindControl("VenueLabel") as Label;
                dr["Venue"] = VenueLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label PartyNameLabel = item.FindControl("PartyNameLabel") as Label;
                dr["PartyName"] = PartyNameLabel.Text.ToString();
                Label NoOfUserLabel = item.FindControl("NoOfUserLabel") as Label;

                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["Distributor"] = DistributorLabel.Text.ToString();
                Label DisSyncldLabel = item.FindControl("DisSyncldLabel") as Label;
                dr["DistributorCode"] = DisSyncldLabel.Text.ToString();
                Label CityLabel = item.FindControl("CityLabel") as Label;
                dr["City"] = CityLabel.Text.ToString();
                Label CityCodeLabel = item.FindControl("CityCodeLabel") as Label;
                dr["CityCode"] = CityCodeLabel.Text.ToString();
                Label lblCityType = item.FindControl("lblCityType") as Label;
                dr["CityType"] = lblCityType.Text.ToString();

                dr["NoofUsers"] = NoOfUserLabel.Text.ToString();
                Label typeOfGiftEnduserLabel = item.FindControl("typeOfGiftEnduserLabel") as Label;
                dr["TypeOfGift"] = typeOfGiftEnduserLabel.Text.ToString();

                Label Label1 = item.FindControl("Label1") as Label;
                dr["ExpShareDist"] = Label1.Text.ToString();
                Label Label2 = item.FindControl("Label2") as Label;
                dr["ExpShareSelf"] = Label2.Text.ToString();
                //Label IndNameLabel = item.FindControl("IndNameLabel") as Label;
                //dr["ProductClass"] = IndNameLabel.Text.ToString();
                Label meetproductLabel = item.FindControl("meetproductLabel") as Label;
                dr["ProductClass"] = meetproductLabel.Text.ToString();
                Label LambBudgetLabel = item.FindControl("LambBudgetLabel") as Label;
                dr["ApproxBudget"] = LambBudgetLabel.Text.ToString();
                Label AppAmountLabel = item.FindControl("AppAmountLabel") as Label;
                dr["ApprovedAmount"] = AppAmountLabel.Text.ToString();
                Label ExpenseApprovedAmountLabel = item.FindControl("ExpenseApprovedAmountLabel") as Label;
                dr["ExpenseAmount"] = ExpenseApprovedAmountLabel.Text.ToString();
                Label ExpenseApprovedRemarkLabel = item.FindControl("ExpenseApprovedRemarkLabel") as Label;
                dr["ExpenseRemark"] = ExpenseApprovedRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label FinalApprovedAmountLabel = item.FindControl("FinalApprovedAmountLabel") as Label;
                dr["ExpenseApprovedAmount"] = FinalApprovedAmountLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label FinalApprovedRemarkLabel = item.FindControl("FinalApprovedRemarkLabel") as Label;
                dr["ExpenseApprovedRemarks"] = FinalApprovedRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label MeetStatus_label = item.FindControl("lblAppStatus") as Label;
                dr["MeetStatus"] = MeetStatus_label.Text.ToString();
                Label AppRemarkLabel = item.FindControl("AppRemarkLabel") as Label;
                dr["Approval/CancelRemark"] = AppRemarkLabel.Text.ToString();
                Label AppdateLabel = item.FindControl("AppdateLabel") as Label;
                dr["Approval/CancelDate"] = AppdateLabel.Text.ToString();

                Label MeetTypeNameLabel = item.FindControl("MeetTypeNameLabel") as Label;
                dr["MeetTypeName"] = MeetTypeNameLabel.Text.ToString();
                Label NoStaffLabel = item.FindControl("NoStaffLabel") as Label;
                dr["NoStaff"] = NoStaffLabel.Text.ToString();
                Label CommentsLabel = item.FindControl("CommentsLabel") as Label;
                dr["Comments"] = CommentsLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label BeatNameLabel = item.FindControl("BeatNameLabel") as Label;
                dr["BeatName"] = BeatNameLabel.Text.ToString();
                Label valueofRetailerLabel = item.FindControl("valueofRetailerLabel") as Label;
                dr["valueofRetailer"] = valueofRetailerLabel.Text.ToString();
                Label ActualusercountLabel = item.FindControl("ActualusercountLabel") as Label;
                dr["Actualusercount"] = ActualusercountLabel.Text.ToString();
                Label BalanceQtyLabel = item.FindControl("BalanceQtyLabel") as Label;
                dr["BalanceQty"] = BalanceQtyLabel.Text.ToString();
                Label lblImageUpload = item.FindControl("lblImageUpload") as Label;
                dr["imageupload"] = lblImageUpload.Text.ToString();
                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 2)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 2)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 2)
                        {
                            sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MeetReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=MeetReport.xls");
            //Response.ContentType = "applicatio/excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htm = new HtmlTextWriter(sw);

            //StringWriter sw1 = new StringWriter();
            //HtmlTextWriter htm1 = new HtmlTextWriter(sw1);

            //StringWriter sw2 = new StringWriter();
            //HtmlTextWriter htm2 = new HtmlTextWriter(sw2);

            //Repeater MeetReport = this.Repeater1;
            //Repeater MeetReport1 = this.rptnoofusers;
            //Repeater MeetReport2 = this.rptproductList;

            //MeetReport.RenderControl(htm);

            //MeetReport1.RenderControl(htm1);
            //MeetReport2.RenderControl(htm2);

            //Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet Name");
            //Response.Write("<font size='4'><horizontalalign='center'>"+sw.ToString());

            //if (rptnoofusers.Items.Count>0)
            //{
            //    Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet User's List");
            //}

            //Response.Write("<font size='4'><horizontalalign='center'>"+sw1.ToString());

            //if (rptproductList.Items.Count > 0)
            //{
            //    Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet Product List");
            //}

            //Response.Write("<font size='4'><horizontalalign='center'>" + sw2.ToString());

            //Response.End();
        }
        protected void btnNouexport_Click(object sender, EventArgs e)
        {

            try
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=NoOfUser.csv");
                string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Meet Date".TrimStart('"').TrimEnd('"') + "," + "Meet Name".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Contact Person".TrimStart('"').TrimEnd('"') + "," + "Mobile No".TrimStart('"').TrimEnd('"') + "," + "EmailId".TrimStart('"').TrimEnd('"') + "," + "Potential".TrimStart('"').TrimEnd('"') + "," + "DOB".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                //
                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("SMName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MeetDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MeetName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("AreaName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("BeatName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("ContactPersonName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MobileNo", typeof(String)));
                dtParams.Columns.Add(new DataColumn("EmailId", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Potential", typeof(String)));
                dtParams.Columns.Add(new DataColumn("DOB", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                foreach (RepeaterItem item in rptnoofusers.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label SMNam = item.FindControl("SMName") as Label;
                    dr["SMName"] = SMNam.Text;
                    Label MeetDate = item.FindControl("MeetDate") as Label;
                    dr["MeetDate"] = MeetDate.Text;
                    Label MeetName = item.FindControl("MeetName") as Label;
                    dr["MeetName"] = MeetName.Text;
                    Label AreaName = item.FindControl("AreaName") as Label;
                    dr["AreaName"] = AreaName.Text;
                    Label BeatName = item.FindControl("BeatName") as Label;
                    dr["BeatName"] = BeatName.Text;
                    Label Name = item.FindControl("Name") as Label;
                    dr["PartyName"] = Name.Text;
                    Label ContactPersonName = item.FindControl("ContactPersonName") as Label;
                    dr["ContactPersonName"] = ContactPersonName.Text;
                    Label MobileNo = item.FindControl("MobileNo") as Label;
                    dr["MobileNo"] = MobileNo.Text;
                    Label EmailId = item.FindControl("EmailId") as Label;
                    dr["EmailId"] = EmailId.Text;
                    Label Potential = item.FindControl("Potential") as Label;
                    dr["Potential"] = Potential.Text;
                    Label DOB = item.FindControl("DOB") as Label;
                    dr["DOB"] = DOB.Text;
                    Label Address = item.FindControl("Address") as Label;
                    dr["Address"] = Address.Text;
                    dtParams.Rows.Add(dr);
                }
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 10 || k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 10 || k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=NoOfUser.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            { ex.ToString(); }
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPrefix(ddlZone.SelectedValue);
        }
    }

}