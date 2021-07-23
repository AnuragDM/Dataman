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

namespace AstralFFMS
{
    public partial class Crmtarget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //reference  OrderEntryItemWise
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnsave.Text == "Save")
            {
                //btnsave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //// btnsave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnsave.CssClass = "btn btn-primary";
            }
            else
            {
                //btnsave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //// btnsave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnsave.CssClass = "btn btn-primary";
            }
            if (!IsPostBack)
            {
                // btnsave.Visible = false;
                // lblPartTypeName.Visible = false;
                fillfyear(); fillddlforsp();
            }

        }
        private void fillfyear()
        {
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlyear.DataSource = dt;
                ddlyear.DataTextField = "Yr";
                ddlyear.DataValueField = "id";
                ddlyear.DataBind();
            }
            ddlyear.Items.Insert(0, new ListItem("-- Select --", "0"));
        }


        private string checkRole()
        {
            return Settings.Instance.RoleType;
        }

        protected void ddlname_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fillgrid(string sql, string condition, string query)
        {
            if (sql == null)
                sql = "select ct.*,msp.smname from tbl_Crmtarget ct left join mastsalesrep msp on msp.smid=ct.smid";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                GridView4.DataSource = dt;
                GridView4.DataBind();
                btnsave.Visible = true; ddlyear.Enabled = false;
                rbpersonwise.Enabled = false; rbRolewise.Enabled = false;
            }
            else
            {
                sql = @"select 'No Record Found' as value,SMName='No Record Found', id='',year='',roletye='',smid='',   mar= '0.00' ,apr= '0.00' ,may ='0.00' ,jun='0.00' ,july ='0.00' ,aug ='0.00' ,sep ='0.00' ,oct ='0.00' ,nov= '0.00' ,dec ='0.00' ,jan ='0.00' ,feb= '0.00',Cdt='',entrytype='' 
                        from mastsalesrep  where smname ='.'";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                GridView4.DataSource = dt;
                GridView4.DataBind();
                btnsave.Visible = false;

            }
            //DropDownList ddlname =
            //(DropDownList)GridView4.HeaderRow.FindControl("ddlname");
            ////string d = "select roletype,rolevalue from mastroletype ";
            //fillDDLDirect(ddlname, query, "rolevalue", "roletype", 1);
            //if (condition != null)
            //    ddlname.SelectedValue = condition;
        }
        private void fillddlforsp()
        {
            //dropdown when person wise is checked

            string query = @"select roletype,rolevalue from mastroletype";
            fillDDLDirect(ddlroleforsp, query, "rolevalue", "roletype", 1);
            query = @"select areaname,areaid from mastarea where areatype='state'";
            fillDDLDirect(ddlstate, query, "areaid", "areaname", 1);

        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            //function to fill drop down
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

        //protected void ddlname_SelectedIndexChanged1(object sender, EventArgs e)
        //{
        //    DropDownList ddlname = (DropDownList)sender;
        //    string sql = null;
        //    //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,sql);
        //    string value = "";
        //    Session["ddlname"] = ddlname.SelectedItem.Value; ;
        //    bool isChecked = rbRolewise.Checked;
        //    if (isChecked)
        //    {
        //        value = rbRolewise.Text;
        //        value = @"select roletype,rolevalue from mastroletype order by rolevalue";
        //        sql = @"select '" + ddlname.SelectedItem.Value + "' as smname,ct.* from tbl_Crmtarget ct left join mastsalesrep msp on msp.smid=ct.smid where ct.year='" + ddlyear.SelectedItem.Text + "' and ct.roletype ='" + ddlname.SelectedItem.Value + "' and entrytype='RW'";
        //        DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
        //        if (dt.Rows.Count == 0)
        //        {
        //            sql = @"select SMName='" + ddlname.SelectedItem.Value + "',   mar= '0.00' ,apr= '0.00' ,may ='0.00' ,jun='0.00' ,july ='0.00' ,aug ='0.00' ,sep ='0.00' ,oct ='0.00' ,nov= '0.00' ,dec ='0.00' ,jan ='0.00' ,feb= '0.00'  from mastsalesrep where smname ='.' ";
        //        }
        //    }
        //    else
        //    {
        //        value = rbpersonwise.Text;
        //        value = @"select roletype=smname,rolevalue=smid from mastsalesrep where smname<>'.' order by smname ";
        //        sql = @"select '" + ddlname.SelectedItem.Text + "' as smname,ct.* from tbl_Crmtarget ct left join mastsalesrep msp on msp.smid=ct.smid where ct.year='" + ddlyear.SelectedItem.Text + "' and ct.smid ='" + ddlname.SelectedItem.Value + "' and entrytype='PW'";
        //        DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
        //        if (dt.Rows.Count == 0)
        //        {
        //            //if no row found
        //            sql = @"select SMName='" + ddlname.SelectedItem.Text + "',   mar= '0.00' ,apr= '0.00' ,may ='0.00' ,jun='0.00' ,july ='0.00' ,aug ='0.00' ,sep ='0.00' ,oct ='0.00' ,nov= '0.00' ,dec ='0.00' ,jan ='0.00' ,feb= '0.00'   from mastsalesrep where smname ='.' ";
        //        }
        //    }
        //    btnsave.Visible = true;
        //    fillgrid(sql, ddlname.SelectedItem.Value, value);
        //}

        protected void btngo_Click(object sender, EventArgs e)
        {
            //disable bcoz these values are used in insert so user dont make any change
            GridView4.Visible = true;
            ddlroleforsp.Enabled = true;

            string value = ""; string targetquery = null; string sql = string.Empty; DataTable dt = new DataTable();
            bool isChecked = rbRolewise.Checked; btnsave.Visible = true;
            if (isChecked)//if role wise is checked
            {
                value = rbRolewise.Text;
                value = @"select roletype,rolevalue from mastroletype order by rolevalue";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, value);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        value = @"  select  top 1 '" + dt.Rows[i]["rolevalue"].ToString() + "' as value,'" + dt.Rows[i]["roletype"].ToString() + "' as smname,ct.* from tbl_Crmtarget ct left join mastsalesrep msp on msp.smid=ct.smid where ct.year='" + ddlyear.SelectedItem.Text + "' and ct.roletype ='" + dt.Rows[i]["rolevalue"].ToString() + "' and entrytype='RW'";
                        DataTable dtnew = DbConnectionDAL.GetDataTable(CommandType.Text, value);
                        if (dtnew.Rows.Count == 0)
                        {
                            value = @" select '" + dt.Rows[i]["rolevalue"].ToString() + "' as value,SMName='" + dt.Rows[i]["roletype"].ToString() + "', id='',year='',roletye='',smid='',   mar= '0.00' ,apr= '0.00' ,may ='0.00' ,jun='0.00' ,july ='0.00' ,aug ='0.00' ,sep ='0.00' ,oct ='0.00' ,nov= '0.00' ,dec ='0.00' ,jan ='0.00' ,feb= '0.00',Cdt='',entrytype=''  from mastsalesrep where smname ='.' ";
                        }
                        if (i != 0)
                            sql += "union  " + value;
                        else
                            sql += value;

                    }


                }

            }
            else// if person wise is checked
            {
                value = rbpersonwise.Text;
                string filter = null;
                if (ddlstate.SelectedValue != "0")
                {
                    if (ddlcity.SelectedValue != "0")
                        filter += "and cityid='" + ddlcity.SelectedItem.Value + "'";
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select City');", true);
                        return;
                    }
                }
                if (ddlroleforsp.SelectedValue != "0")
                    filter += "and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'";

                value = @"select roletype=smname,rolevalue=smid from mastsalesrep where smname<>'.' " + filter + "  order by smname ";// to get salesperson
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, value);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //to check whether data is found in table or not
                        value = @"  select  top 1 '" + dt.Rows[i]["rolevalue"].ToString() + "' as value,'" + dt.Rows[i]["roletype"].ToString() + "' as smname,ct.* from tbl_Crmtarget ct left join mastsalesrep msp on msp.smid=ct.smid where ct.year='" + ddlyear.SelectedItem.Text + "' and ct.smid ='" + dt.Rows[i]["rolevalue"].ToString() + "'  and entrytype='PW'";
                        DataTable dtnew = DbConnectionDAL.GetDataTable(CommandType.Text, value);
                        if (dtnew.Rows.Count == 0)
                        {
                            //when data is not found in table then this is executed so that grid is filled properly for new entry   
                            value = @" select '" + dt.Rows[i]["rolevalue"].ToString() + "' as value,SMName='" + dt.Rows[i]["roletype"].ToString() + "', id='',year='',roletye='',smid='',   mar= '0.00' ,apr= '0.00' ,may ='0.00' ,jun='0.00' ,july ='0.00' ,aug ='0.00' ,sep ='0.00' ,oct ='0.00' ,nov= '0.00' ,dec ='0.00' ,jan ='0.00' ,feb= '0.00',Cdt='',entrytype=''  from mastsalesrep where smname ='.' ";
                        }
                        if (i != 0)
                            sql += "union  " + value;// for union in second time
                        else
                            sql += value;// escaping from union for first time

                    }


                }
                else
                {
                    sql = value;//since no sp is found for this so pass sp query so that grid may fill with no record found message;
                }
            }

            fillgrid(sql, null, value);
        }


        protected void btnsave_Click(object sender, EventArgs e)
        {
            string value = ""; string targetquery = null;
            bool isChecked = rbRolewise.Checked;
            if (isChecked)// when role wise (RW) button is checked
            {
                //value = rbRolewise.Text;
                //value = @"select roletype,rolevalue from mastroletype order by rolevalue";
                for (int i = 0; i < GridView4.Rows.Count; i++)
                {
                    InsertOrder("RW", i);

                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfuly');", true);
                Cancel_Click(null, null);
            }
            else// when person wise button is checked (PW)
            {
                //value = rbpersonwise.Text;
                //value = @"select roletype=smname,rolevalue=smid from mastsalesrep where smname<>'.' order by smname ";
                for (int i = 0; i < GridView4.Rows.Count; i++)
                {
                    InsertOrder("PW", i);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfuly');", true);
                Cancel_Click(null, null);
            }
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["ValidationMsz"])))
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + Session["ValidationMsz"].ToString() + "');", true);
            //    Session["ValidationMsz"] = null;
            //}
        }

        private void InsertOrder(string flag, int index)
        {

            try
            {
                string insert = null;
                TextBox txtMar = (GridView4.Rows[index].FindControl("txtMar") as TextBox);
                TextBox txtApr = (GridView4.Rows[index].FindControl("txtApr") as TextBox);
                TextBox txtMay = (GridView4.Rows[index].FindControl("txtMay") as TextBox);
                TextBox txtJun = (GridView4.Rows[index].FindControl("txtJun") as TextBox);
                TextBox txtJul = (GridView4.Rows[index].FindControl("txtJul") as TextBox);
                TextBox txtAug = (GridView4.Rows[index].FindControl("txtAug") as TextBox);
                TextBox txtSep = (GridView4.Rows[index].FindControl("txtSep") as TextBox);
                TextBox txtOct = (GridView4.Rows[index].FindControl("txtOct") as TextBox);
                TextBox txtNov = (GridView4.Rows[index].FindControl("txtNov") as TextBox);
                TextBox txtDec = (GridView4.Rows[index].FindControl("txtDec") as TextBox);
                TextBox txtJan = (GridView4.Rows[index].FindControl("txtJan") as TextBox);
                TextBox txtFeb = (GridView4.Rows[index].FindControl("txtFeb") as TextBox);
                HiddenField hidval = (GridView4.Rows[index].FindControl("hidval") as HiddenField);
                //string value = GridView4.Rows.Cells[index].Text;
                if ((txtMar.Text == "") && (txtApr.Text == "") && (txtMay.Text == "") && (txtJun.Text == "") && (txtJul.Text == "") && (txtAug.Text == "") && (txtSep.Text == "") && (txtOct.Text == "") && (txtNov.Text == "") && (txtDec.Text == "") && (txtJan.Text == "") && (txtFeb.Text == ""))
                {
                   // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Target');", true);
                    return;
                }
                //if (validtarget(txtMar.Text)!="valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtMar.Text));", true);
                //    break1 = "yes"; txtMar.Focus(); txtMar.Attributes.Add("backgroundColor", "Red"); return break1; 
                //}
                //if (validtarget(txtApr.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtApr.Text));", true); break1 = "yes"; txtApr.Focus(); txtApr.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtMay.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtMay.Text));", true); break1 = "yes"; txtMay.Focus(); txtMay.Attributes.Add("backgroundColor", "Red"); return break1; 
                //}
                //if (validtarget(txtJun.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtJun.Text));", true); break1 = "yes"; txtJun.Focus(); txtJun.Attributes.Add("backgroundColor", "Red"); return break1; 
                //} 
                //if (validtarget(txtJul.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtJul.Text));", true); break1 = "yes"; txtJul.Focus(); txtJul.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtAug.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtAug.Text));", true); break1 = "yes"; txtAug.Focus(); txtAug.Attributes.Add("backgroundColor", "Red"); return break1; 
                //}
                //if (validtarget(txtSep.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtSep.Text));", true); break1 = "yes"; txtSep.Focus(); txtSep.Attributes.Add("backgroundColor", "Red"); return break1; 
                //}
                //if (validtarget(txtOct.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtOct.Text));", true); break1 = "yes"; txtOct.Focus(); txtOct.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtNov.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtNov.Text));", true); break1 = "yes"; txtNov.Focus(); txtNov.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtDec.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtDec.Text));", true); break1 = "yes"; txtDec.Focus(); txtDec.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtJan.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtJan.Text));", true); break1 = "yes"; txtJan.Focus(); txtJan.Attributes.Add("backgroundColor", "Red"); return break1;
                //}
                //if (validtarget(txtFeb.Text) != "valid")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(validtarget(txtFeb.Text));", true); break1 = "yes"; txtFeb.Focus(); txtFeb.Attributes.Add("backgroundColor", "Red"); return break1; 
                //}
                //if ((validtarget(txtMar.Text) != "true") || (validtarget(txtApr.Text) != "true") || (validtarget(txtMay.Text) != "true") || (validtarget(txtJun.Text) != "true") || (validtarget(txtJul.Text) != "true") || (validtarget(txtAug.Text) != "true") || (validtarget(txtSep.Text) != "true") || (validtarget(txtOct.Text) != "true") || (validtarget(txtNov.Text) != "true") || (validtarget(txtDec.Text) != "true") || (validtarget(txtJan.Text) != "true") || (validtarget(txtFeb.Text) != "true"))
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Target Like 9999999.99');", true); return;
                //}
                //insert rolewise  records in table;
                if (flag == "RW")
                {
                    //delete existing records from table than insert new ones;
                    insert = @"delete from tbl_Crmtarget where year='" + ddlyear.SelectedItem.Text + "' and roletype ='" + hidval.Value + "' and entrytype='RW'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, insert);
                    //checking that target is not alotted to sp individually
                    insert = @"select smid ,smname from mastsalesrep where salesreptype='" + hidval.Value + "'";
                    DataTable dtsmname = DbConnectionDAL.GetDataTable(CommandType.Text, insert);

                    for (int i = 0; i < dtsmname.Rows.Count; i++)
                    {
                        //now inserting taget one by one to all sp under role 
                        insert = @"INSERT INTO [dbo].[tbl_Crmtarget]
                              ([year]  ,[roletype],[Smid] ,[mar] ,[apr]  ,[may] ,[jun] ,[july] ,[aug]   ,[sep]   ,[oct] ,[nov]   ,[dec] ,[jan] ,[feb] ,[entrytype])   VALUES
                            ('" + ddlyear.SelectedItem.Text.Trim() + "','" + hidval.Value + "','" + dtsmname.Rows[i]["smid"] + "','" + txtMar.Text.Trim() + "' ,'" + txtApr.Text.Trim() + "','" + txtMay.Text.Trim() + "','" + txtJun.Text.Trim() + "','" + txtJul.Text.Trim() + "','" + txtAug.Text.Trim() + "','" + txtSep.Text.Trim() + "','" + txtOct.Text.Trim() + "','" + txtNov.Text.Trim() + "','" + txtDec.Text.Trim() + "','" + txtJan.Text.Trim() + "','" + txtFeb.Text.Trim() + "','RW')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, insert);
                    }

                }
                //insert personwise  records in table;
                else
                {
                    if ((txtMar.Text == "0.00") && (txtApr.Text == "0.00") && (txtMay.Text == "0.00") && (txtJun.Text == "0.00") && (txtJul.Text == "0.00") && (txtAug.Text == "0.00") && (txtSep.Text == "0.00") && (txtOct.Text == "0.00") && (txtNov.Text == "0.00") && (txtDec.Text == "0.00") && (txtJan.Text == "0.00") && (txtFeb.Text == "0.00"))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Target');", true); return;
                    }
                    //delete existing records from table than insert new ones; whether it is in rolewise or person wise
                    insert = @"delete from tbl_Crmtarget where year='" + ddlyear.SelectedItem.Text + "' and smid ='" + hidval.Value + "' and  entrytype='PW'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, insert);
                    string roletype = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select salesreptype from mastsalesrep where smid='" + Convert.ToInt32(hidval.Value) + "'"));
                    insert = @"INSERT INTO [dbo].[tbl_Crmtarget]
                             ([year]  ,[roletype],[Smid] ,[mar] ,[apr]  ,[may] ,[jun] ,[july] ,[aug]   ,[sep]   ,[oct] ,[nov]   ,[dec] ,[jan] ,[feb] ,[entrytype])
                              VALUES
                              ('" + ddlyear.SelectedItem.Text.Trim() + "','" + roletype + "','" + hidval.Value + "','" + txtMar.Text.Trim() + "' ,'" + txtApr.Text.Trim() + "','" + txtMay.Text.Trim() + "','" + txtJun.Text.Trim() + "','" + txtJul.Text.Trim() + "','" + txtAug.Text.Trim() + "','" + txtSep.Text.Trim() + "','" + txtOct.Text.Trim() + "','" + txtNov.Text.Trim() + "','" + txtDec.Text.Trim() + "','" + txtJan.Text.Trim() + "','" + txtFeb.Text.Trim() + "','PW')";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, insert);

                }

            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }

        }

        private string validtarget(string text)
        {
            string result = "valid";
            if (string.IsNullOrEmpty(text))
            {
                result = "valid";
            }
            else if (text.Contains("."))
            {
                string[] words = text.Split('.');
                if (words[0].Length > 7)
                {
                    result = "Enter only 7 digit before decimal like 9999999.99";
                }
                else if (words[1].Length > 2)
                {
                    result = "Enter only two digit after decimal like 9999999.99";
                }

                else
                {
                    result = "valid";
                }
            }
            else if (text.Length > 7)
            {
                result = "Enter only 7 digit like 9999999.99";
            }
            return result;
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  string query = @"select areaname,areaid from mastarea where areatype='city' and underid="+ddlstate.SelectedItem.Value+"";
            string query = @"select cityid,cityname from viewgeo  where stateid=" + ddlstate.SelectedItem.Value + "";
            fillDDLDirect(ddlcity, query, "cityid", "cityname", 1);
        }

        protected void rbpersonwise_CheckedChanged(object sender, EventArgs e)
        {
            if (rbpersonwise.Checked)
            {
                divforsp.Visible = true;
            }
            else
            {
                divforsp.Visible = false;
                ddlstate.SelectedValue = "0";
                ddlroleforsp.SelectedValue = "0";
                ddlcity.SelectedValue = "0";
            }
        }

        protected void rbRolewise_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRolewise.Checked)
            {
                divforsp.Visible = false;
                ddlstate.SelectedValue = "0";
                ddlroleforsp.SelectedValue = "0";
                ddlcity.SelectedValue = "0";
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlroleforsp.SelectedValue = "0"; ddlyear.SelectedValue = "0";
            ddlstate.SelectedValue = "0";
            ddlcity.SelectedValue = "0";
            rbpersonwise.Checked = false;
            rbRolewise.Checked = false;
            GridView4.Visible = false;
            btnsave.Visible = false;
            ddlyear.Enabled = true; rbpersonwise.Enabled = true; rbRolewise.Enabled = true; ddlroleforsp.Enabled = true;
            divforsp.Visible = false;
            // Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }
    }
}