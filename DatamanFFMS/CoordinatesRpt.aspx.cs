using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
//using DataAccessLayer;
using System.Data;
using System.Text.RegularExpressions;
using DAL;
using BAL;
using System.IO;
public partial class CoordinatesRpt : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    Settings SetObj = Settings.Instance;
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;

        if (!Page.IsPostBack)
        {
            txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            BindGroup();
            Txt_FromTime.Text = "00:01";
            TextBox2.Text = "23:59";
            BindPerson(ddlgroup.SelectedValue);
        }
        if (SetObj.GroupID != "0" && SetObj.GroupID != "")
        {

            if (!IsPostBack)
            {
                ddlgroup.SelectedValue = SetObj.GroupID;
                BindPerson(ddlgroup.SelectedValue);
            }
        }
        else
        {
            SetObj.GroupID = ddlgroup.SelectedValue;
        }
        if (SetObj.PersonID != "0" && SetObj.PersonID != "")
        {
            if (!IsPostBack)

                ddlperson.SelectedValue = SetObj.PersonID;
        }
        else
        {
            SetObj.PersonID = ddlperson.SelectedValue;
        }
    }
    protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetObj.GroupID = ddlgroup.SelectedValue;
        BindPerson(ddlgroup.SelectedValue);
    }
    protected void ddlperson_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetObj.PersonID = ddlperson.SelectedValue;
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
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
    protected void Button3_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in gvData.Rows)
        {
            if (((CheckBox)gvr.FindControl("ckView")).Checked)
                upd.DeleteCoordinates(((HiddenField)gvr.FindControl("hid")).Value);
        }

        ShowAlert("Record Deleted Successfully"); BindData();

    }
    #region BindDropdowns
    private void BindGroup()
    {
        string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
      fillDDLDirect(ddlgroup, str, "Code", "Description", 1);
    }
    private void BindPerson(string group)
    {
        string str = "select PersonMaster.PersonName,PersonMaster.ID from PersonMaster inner join GrpMapp on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + group + "' order by PersonMaster.PersonName";
       fillDDLDirect(ddlperson, str, "ID", "PersonName", 1);
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
    #region Bind Data
    private void BindData()
    {
        try
        {
            Common cs = new Common();
            string PersonDeviceNo = string.Empty;
            if (ddlperson.SelectedItem.Text != "--Select--")
                PersonDeviceNo = cs.GetDeviceNoByPersonID(ddlperson.SelectedValue);

            string qry = "select Id,latitude,longitude,convert(nvarchar(20),locationdetails.CurrentDate,113) as CurrentDate,Description as address from LocationDetails where DeviceNo='" + PersonDeviceNo + "' and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime) ";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);// DataAccessLayer.DAL.getFromDataTable(qry);
            if (dt.Rows.Count > 0)
            {
                btndelete.Visible = true;
                gvData.DataSource = dt;
                gvData.DataBind();
            }
            else
            {
                btndelete.Visible = false;
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }
        catch (Exception ex) { ex.ToString(); }
    }

    #endregion
    #region General
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
   
    #endregion
}