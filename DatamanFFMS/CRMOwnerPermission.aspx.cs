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
    public partial class CRMOwnerPermission : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            if (Request.QueryString["Contact_Id"] != null)
            {
                Bindgvdata(Convert.ToInt32(Request.QueryString["Contact_Id"]));
                string Qry = "SELECT FirstName +' '+Isnull(LastName,'') As Lead FROM CRM_MastContact WHERE Contact_Id=" + Request.QueryString["Contact_Id"] + " ";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    LblLead.Text = dt.Rows[0]["Lead"].ToString();

                }
                
            }
           
            }
        }
        private void Bindgvdata(int ContactId)
        {
            string strQ = "";
            if (Settings.Instance.UserName.ToUpper() == "SA")
            {

                strQ = " select SMID,SMName,CAST (max(Own) AS BIT) As Owner   FROM  (Select SMID,SMName,0 AS Own from [MastSalesRep] where SMName <>'.'  union ";
                strQ = strQ + "select MSR.smid     As SMID,MSR.SMName,1 AS Own  from [MastSalesRep]  MSR     where MSR.smid in (SELECT * FROM dbo.GetIds(" + ContactId + ")) ) a GROUP BY SMID,SMName  ORDER BY SMName";
            }

            else
            {
                strQ = " select SMID,SMName,CAST (max(Own) AS BIT) As Owner FROM  (select MSRG.maingrp As SMID,MSR.SMName,0 AS Own  from mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.maingrp where MSRG.smid in (" + Settings.Instance.SMID + ") and MSR.SMName <>'.'  union  ";
                strQ = strQ + "SELECT MSRG.smid    As SMID,MSR.SMName,0 AS Own  FROM mastsalesrepgrp MSRG Left Join [MastSalesRep] MSR on MSR.smid=MSRG.smid    WHERE MSRG.maingrp in (" + Settings.Instance.SMID + ")  and MSR.SMName <>'.'  union ";
                strQ = strQ + "select MSR.smid     As SMID,MSR.SMName,1 AS Own  from [MastSalesRep]  MSR     where MSR.smid in (SELECT * FROM dbo.GetIds(" + ContactId + ") )) a GROUP BY SMID,SMName  ORDER BY SMName";

            }
           
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                divbtns.Visible = true;
                gvData.DataSource = dt;
                gvData.DataBind();

                ddlSales.DataSource = dt;
                ddlSales.DataTextField = "SMName";
                ddlSales.DataValueField = "SMID";
             
                ddlSales.DataBind();
                ddlSales.Items.Insert(0, new ListItem("--Select--", "0"));
                gvData.Visible = true;
                //foreach (GridViewRow row in gvData.Rows)
                //{
                   
                     
                //        gvData.Columns[3].Visible = true;
                //        gvData.Columns[4].Visible = true;
                //        gvData.Columns[5].Visible = true;
                //    }


                //}
            }
            else
            {
               // divbtns.Visible = false;
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string OwnerStr = "";
                int res = 0;
                foreach (GridViewRow gvr in gvData.Rows)
                {

                    if (((CheckBox)gvr.FindControl("ckView")).Checked == true)
                    {
                      
                        if (OwnerStr == "")
                          OwnerStr =   gvData.DataKeys[gvr.RowIndex]["SMID"].ToString() ;
                        else
                            OwnerStr = OwnerStr + ',' + gvData.DataKeys[gvr.RowIndex]["SMID"].ToString();
                       // ((TextBox)(gvr.Rows[e.RowIndex].Cells[1].Controls[0])).Text;
                         
                     
                    
                    }
                 
                    //page_ID = Convert.ToInt32(gvData.DataKeys[gvr.RowIndex]["PageId"].ToString());

                    //res = d.InsertUpdatePermission
                    //     (
                    //         Convert.ToInt32(ddlRole.SelectedValue),
                    //        Convert.ToInt32(gvData.DataKeys[gvr.RowIndex]["PageId"].ToString()),
                    //         ((CheckBox)gvr.FindControl("ckView")).Checked,
                    //         ((CheckBox)gvr.FindControl("ckAdd")).Checked,
                    //         ((CheckBox)gvr.FindControl("ckEdit")).Checked,
                    //         ((CheckBox)gvr.FindControl("ckDelete")).Checked,
                    //         ((CheckBox)gvr.FindControl("ckExport")).Checked
                    //     );
                }
                string strupdowner = "update [CRM_MastContact] set OwnerSp = '" + OwnerStr + "'   where Contact_Id=" + Request.QueryString["Contact_Id"] + "";
                res = DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
                if (res > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully!');", true);
                    Bindgvdata(Convert.ToInt32(Request.QueryString["Contact_Id"]));
                    Response.Redirect("CRMTask.aspx");
                    //  Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        protected void ckView_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb1 = (CheckBox)gvData.Rows[index].FindControl("ckView");
            string SMNme = gvData.Rows[index].Cells[2].Text; 

            if (((CheckBox)gvData.Rows[index].FindControl("ckView")).Checked == false)
            {
                string Qry = "SELECT * FROM CRM_MastContact WHERE Contact_Id=" + Request.QueryString["Contact_Id"] + " AND Manager ='" + gvData.DataKeys[index]["SMID"].ToString() + "'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    this.ModalPopupExtender2.Show();
                  //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ManagerNotification", "ManagerNotification('" + SMNme + " is manager !');", true);
                   //((CheckBox)gvData.Rows[index].FindControl("ckView")).Checked = true;
                   // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

                }
            }

            //here you can find your control and get value(Id).

        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Hide();
            this.ModalPopupExtender1.Show();
        }
        protected void btnno_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Hide();
            Bindgvdata(Convert.ToInt32(Request.QueryString["Contact_Id"]));
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender1.Hide();
            Bindgvdata(Convert.ToInt32(Request.QueryString["Contact_Id"]));
        }
        protected void btnsavemanager_Click(object sender, EventArgs e)
        {
            try
            {
                int res = 0;


                    string OwnerStr = "";
                    string strupdowner = "";
                foreach (GridViewRow gvr in gvData.Rows)
                {

                    if (((CheckBox)gvr.FindControl("ckView")).Checked == true)
                    {
                      
                        if (OwnerStr == "")
                          OwnerStr =   gvData.DataKeys[gvr.RowIndex]["SMID"].ToString() ;
                        else
                            OwnerStr = OwnerStr + ',' + gvData.DataKeys[gvr.RowIndex]["SMID"].ToString();
              
                         
                     
                    
                    }
                 
                   
                }
                 strupdowner = "update [CRM_MastContact] set OwnerSp = '" + OwnerStr + "'   where Contact_Id=" + Request.QueryString["Contact_Id"] + "";
                res = DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
                if (res > 0)
                {

                }
                 strupdowner = "update [CRM_MastContact] set Manager = '" + ddlSales.SelectedValue + "'   where Contact_Id=" + Request.QueryString["Contact_Id"] + "";
                res = DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
                if (res > 0)
                {
                    strupdowner = "update [CRM_MastContact] set OwnerSp =  (case when  [OwnerSp]  Like '%" + ddlSales.SelectedValue + "%' then [OwnerSp] else [OwnerSp] + ',' + '" + ddlSales.SelectedValue + "' end)   where Contact_Id=" + Request.QueryString["Contact_Id"] + "";
                    DAL.DbConnectionDAL.ExecuteQuery(strupdowner);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('New Manager Updated Successfully!');", true);
                    Bindgvdata(Convert.ToInt32(Request.QueryString["Contact_Id"]));
                    //  Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
        {
          // '' bool check = ((CheckBox)gvData.FindControl("ckView")).Checked;
            //if (((CheckBox)gvData.FindControl("ckView")).Checked == false)
            //{
            //    string Qry = "SELECT * FROM CRM_MastContact WHERE Contact_Id=" + Request.QueryString["Contact_Id"] + " AND Manager ='" + gvData.DataKeys[gvData.SelectedIndex].Value.ToString() + "'";
            //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            //    if (dt.Rows.Count > 0)
            //    {
            //        ((CheckBox)gvData.FindControl("ckView")).Checked = true;
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

            //    }
            //}

        }

        protected void gvData_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //if (((CheckBox)gvData.FindControl("ckView")).Checked == false)
            //{
            //    string Qry = "SELECT * FROM CRM_MastContact WHERE Contact_Id=" + Request.QueryString["Contact_Id"] + " AND Manager ='" + gvData.DataKeys[gvData.SelectedIndex].Value.ToString() + "'";
            //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            //    if (dt.Rows.Count > 0)
            //    {
            //        ((CheckBox)gvData.FindControl("ckView")).Checked = true;
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

            //    }
            //}
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (((CheckBox)gvData.FindControl("ckView")).Checked == false)
            //{
            //    string Qry = "SELECT * FROM CRM_MastContact WHERE Contact_Id=" + Request.QueryString["Contact_Id"] + " AND Manager ='" + gvData.DataKeys[gvData.SelectedIndex].Value.ToString() + "'";
            //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
            //    if (dt.Rows.Count > 0)
            //    {
            //        ((CheckBox)gvData.FindControl("ckView")).Checked = true;
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

            //    }
            //}
        }

      
       

    }
}