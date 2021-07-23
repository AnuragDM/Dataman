using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
//using DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
using System.IO;
public partial class InsertAddress : System.Web.UI.Page
{
    SqlConnection con = Connection.Instance.GetConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;

        if (!Page.IsPostBack)
        {
            txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            Txt_FromTime.Text = "00:01";
            TextBox2.Text = "23:59";
            BindGroup();
        }
    }
    #region Bind Dropdowns
    private void BindGroup()
    {
        ddlgrp.Items.Clear();
        string strqry = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
        fillDDLDirect(ddlgrp, strqry, "Code", "Description", 1);
    }
    private void BindPersons()
    {
        ddlperson.Items.Clear();
        string stqry = "select PersonMaster.PersonName as Person,PersonMaster.DeviceNo as Id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlgrp.SelectedValue + "' order by PersonMaster.PersonName";
        fillDDLDirect(ddlperson, stqry, "Id", "Person", 2);
    }
    #endregion
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

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        GetData();
        string script = "window.alert(\"Posting Completed\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    protected void btninsertadd_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = "select * from Addresses";
            DataTable dtbl = new DataTable();
            dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, qry);// DataAccessLayer.DAL.getFromDataTable(qry);
            if (dtbl.Rows.Count > 0)
            {
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                    DMT.InsertAddress(dtbl.Rows[i]["latitude"].ToString(), dtbl.Rows[i]["longitude"].ToString());
                }
            }
        }
        catch (Exception ex) { ex.ToString(); }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
        Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
        Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
        if (mtStartDt.Success && mtEndDt.Success)
        {
            if (txt_fromdate.Text != "")
            {
                DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim() + " " + Txt_FromTime.Text);
                DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim() + " " + TextBox2.Text);
                if (dt1 <= dt2)
                {
                }
                else
                {
                    ShowAlert("From DateTime cannot be greater than To DateTime");
                    txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    Txt_FromTime.Text = "00:01";
                    TextBox2.Text = "23:59";
                    return;
                }
            }
            else
            {
                ShowAlert("Pls Select From DateTime before selecting To DateTime");
            }
        }
        else
        {
            ShowAlert("Invalid Date!");
        }
    }
    protected void ddlgrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPersons();
    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    private void GetData()
    {
        try
        {
            string mLong = "";
            string mLat = "";

            string addqry = "";
            if (!string.IsNullOrEmpty(ddlperson.SelectedValue) && ddlperson.SelectedIndex > 0)
            {
                addqry = " and locationdetails.DeviceNo='" + ddlperson.SelectedValue + "'";
            }

            string qry = "select distinct ga1.address,locationdetails.Latitude1,locationdetails.Longitude1 from LocationDetails left join GAdd ga ON locationdetails.Latitude1=ga.Lat AND locationdetails.Longitude1=ga.Long left join GAdd1 ga1 on ga.addid=ga1.addid " +
              " WHERE cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime) " + addqry + " and ga1.address is null and Latitude1 is not null";


            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);// DataAccessLayer.DAL.getFromDataTable(qry);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mLong = dt.Rows[i]["latitude1"].ToString();
                    mLat = dt.Rows[i]["longitude1"].ToString();
                    WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                    string Address = DMT.InsertAddress(dt.Rows[i]["latitude1"].ToString(), dt.Rows[i]["longitude1"].ToString());

                    //string Address = DMT.InsertAddress(Latitude, Longitude);

                    if (!string.IsNullOrEmpty(Address))
                    {
                       // if (DataAccessLayer.DAL.GetIntScalarVal("select 1 from GAdd1 where Address='" + Address.Trim() + "'") <= 0)
                        if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,"select 1 from GAdd1 where Address='" + Address.Trim() + "'")) <= 0)
                        {
                            DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into GAdd1 (Address) values('" + Address.Trim() + "')");// DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd1 (Address) values('" + Address.Trim() + "')");
                            Int64 addId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select AddId from GAdd1 where Address='" + Address.Trim() + "'"));// DataAccessLayer.DAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");

                            if (addId > 0)
                            {
                                DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into GAdd (Lat,Long,AddId) values('" + dt.Rows[i]["latitude1"].ToString() + "','" + dt.Rows[i]["longitude1"].ToString() + "'," + addId + ")");// DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + dt.Rows[i]["latitude1"].ToString() + "','" + dt.Rows[i]["longitude1"].ToString() + "'," + addId + ")");
                                //DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + dt.Rows[i]["latitude1"].ToString() + "','" + dt.Rows[i]["longitude1"].ToString() + "'," + addId + ")");
                            }
                        }
                        else
                          //  if (DataAccessLayer.DAL.GetIntScalarVal("select 1 from GAdd where Lat='" + dt.Rows[i]["latitude1"].ToString() + "' and Long='" + dt.Rows[i]["longitude1"].ToString() + "'") <= 0)
                           if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,"select 1 from GAdd where Lat='" + dt.Rows[i]["latitude1"].ToString() + "' and Long='" + dt.Rows[i]["longitude1"].ToString() + "'")) <= 0)
                            {
                                Int64 addId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select AddId from GAdd1 where Address='" + Address.Trim() + "'"));// DataAccessLayer.DAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");
                                if (addId > 0)
                                {
                                    DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into GAdd (Lat,Long,AddId) values('" + dt.Rows[i]["latitude1"].ToString() + "','" + dt.Rows[i]["longitude1"].ToString() + "'," + addId + ")");
                                  //  DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + dt.Rows[i]["latitude1"].ToString() + "','" + dt.Rows[i]["longitude1"].ToString() + "'," + addId + ")");
                                }
                            }
                    }


                }
            }
            else
            {
                ShowAlert("No Records Found !!");
            }
        }
        catch (Exception ex)
        { ex.ToString(); }
    }
}