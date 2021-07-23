using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class LocationTrackingReport : System.Web.UI.Page
    {
        int SMID=0;
        string Vdate;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["smid"]))
                 {
                    SMID = Convert.ToInt32(Request.QueryString["smid"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Date"]))
                {
                    Vdate = Convert.ToString(Request.QueryString["Date"]);
                    txttodate.Text = Vdate;
                    txtfmDate.Text = Vdate;
                }
                else
                {
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                    txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                }
           //     BindDistributorDDl();
             //   BindSalePersonDDl();
                //Added 
              
                  LocationData();
                //End
                //txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
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
                    //DdlSalesPerson.DataSource = dv.ToTable();
                    //DdlSalesPerson.DataTextField = "SMName";
                    //DdlSalesPerson.DataValueField = "SMId";
                    //DdlSalesPerson.DataBind();

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
                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
                {
                    string smIDStr1 = "", Qrychk = "", Query = "";
                  
                  Qrychk = " a.CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and a.CurrentDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                  LocationData();

//                    Query = @"Select a.deviceNo, a.latitude,a.Longitude , a.currentDate , a.Description , b.SMId , b.SMName from viewlocationdetails a 
//                    inner join MastSalesRep b on a.DeviceNo = b.DeviceNo
//                    where b.SMId in (" + SMID + ") and " + Qrychk + "";
//                    DataTable dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
//                    if (dtLocTraRep.Rows.Count > 0)
//                    {
//                        spname.InnerText = dtLocTraRep.Rows[0]["SMName"].ToString();
//                        detailDistOutDiv.Style.Add("display", "block");
//                        distlocrpt.DataSource = dtLocTraRep;
//                        distlocrpt.DataBind();
//                    }
//                    else
//                    {
//                        spname.InnerText = "";
//                        detailDistOutDiv.Style.Add("display", "block");
//                        distlocrpt.DataSource = dtLocTraRep;
//                        distlocrpt.DataBind();
//                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    distlocrpt.DataSource = null;
                    distlocrpt.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void LocationData()
        {
            string Qrychk = "", Query = "";
            if (!string.IsNullOrEmpty(Vdate.ToString())) { Qrychk = " cast(a.CurrentDate as date)='" + Vdate + "'"; }
            else
            {
                Qrychk = " a.CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and a.CurrentDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            }
            Query = @"Select a.deviceNo, a.latitude,a.Longitude , a.currentDate , a.Description , b.SMId , b.SMName from viewlocationdetails a 
                    inner join MastSalesRep b on a.DeviceNo = b.DeviceNo
                    where b.SMId in (" + SMID + ") and " + Qrychk + "";
            DataTable dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dtLocTraRep.Rows.Count > 0)
            {
                spname.InnerText = dtLocTraRep.Rows[0]["SMName"].ToString();
                detailDistOutDiv.Style.Add("display", "block");
                distlocrpt.DataSource = dtLocTraRep;
                distlocrpt.DataBind();
            }
            else
            {
                spname.InnerText = "";
                detailDistOutDiv.Style.Add("display", "block");
                distlocrpt.DataSource = dtLocTraRep;
                distlocrpt.DataBind();
            }
            Vdate = string.Empty;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LocationTrackingReport.aspx");
        }
    }
}