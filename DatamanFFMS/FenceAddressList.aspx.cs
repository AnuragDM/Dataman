using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using DataAccessLayer;
using BusinessLayer;
using System.Text.RegularExpressions;
using DAL;
using BAL;
using System.IO;
public partial class FenceAddressList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;

        if (!Page.IsPostBack)
        {
            GetPersons();
            BindGrid();
        }
    }
    private void GetPersons()
    {
        string str = "select distinct PersonMaster.ID,PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID in (select GroupId from UserGrp where  UserGrp.UserID = '" + Settings.Instance.UserID + "')  order by PersonMaster.PersonName";
        fillDDLDirect(ddlperson, str, "ID", "PersonName", 1);
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
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    protected void gvData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvData.EditIndex = e.NewEditIndex;
        BindGrid(); 
    }
    protected void gvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvData.EditIndex = -1;
        BindGrid();   
    }
    protected void gvData_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = (GridViewRow)gvData.Rows[e.RowIndex];
        int id = Int32.Parse(gvData.DataKeys[e.RowIndex].Value.ToString());
        TextBox address = (TextBox)row.FindControl("txt_Address");

        //updating the record  
     int a=Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "update FenceAddress set Address='" + address.Text + "' where Id=" + id + ""));
    //  DataAccessLayer.DAL.GetIntScalarVal("update FenceAddress set Address='" + address.Text + "' where Id=" + id + "");
        //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
        gvData.EditIndex = -1;
        //Call ShowData method for displaying updated data  
        BindGrid();   
    }
    protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = (GridViewRow)gvData.Rows[e.RowIndex];
        string delt = "delete from FenceAddress where id=" + Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value.ToString()) + "";
        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, delt);
     //   DataAccessLayer.DAL.ExecuteQuery(delt);
        BindGrid();
    }
   
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    private void BindGrid()
    {
        try
        {
            string addstr = "";
            if (ddlperson.SelectedIndex > 0)
                addstr = " and Fa.PersonID_createdFence=" + ddlperson.SelectedValue + "";
            if (!string.IsNullOrEmpty(txt_createddate.Text))
                addstr += " and cast(Fa.Created_date as date)= cast('" + txt_createddate.Text + "' as date)";

            string str = "select *,convert(varchar(12),Created_date,113) as Createddate from FenceAddress Fa left join GroupMaster gm on fa.GroupID=gm.GroupID left join PersonMaster pm on Fa.PersonID_createdFence=pm.ID where gm.groupId in (select GroupId from UserGrp where  UserGrp.UserID = '" + Settings.Instance.UserID + "') " + addstr + " order by gm.groupid,address";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);// DataAccessLayer.DAL.getFromDataTable(str);
            if (dt.Rows.Count > 0)
            {
                gvData.DataSource = dt;
                gvData.DataBind();
            }
            else
            {
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }
        catch (Exception xe) { xe.ToString(); }
    }


    protected void txt_createddate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_createddate.Text))
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_createddate.Text, regexDt.ToString());
            // Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
            if (mtStartDt.Success)
            {
            }
            else
            {
                ShowAlert("Invalid Date!");
            }
        }
    }
}