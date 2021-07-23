using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data;
using System.Web.Services;
using DAL;

namespace AstralFFMS
{
    public partial class ViewAllMessages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckNotifications();
            }
        }
        public void CheckNotifications()
        {
            try
            {
                BindRepeater();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public string setClassNew(int Status)
        {
            string classToApply = string.Empty;
            if (Status == 0)
            {
                classToApply = "messagelabelNew";
            }
            else
            {
                classToApply = "messagelabelNew1";
            }

            return classToApply;

        }
        //[WebMethod]
        //public static void deleteNotification(string notID)
        //{

        //    string data = string.Empty;
        //    try
        //    {
        //        TransNotification TRL = op.TransNotifications.FirstOrDefault(u => u.NotiId == Convert.ToInt32(notID));
        //        if (TRL != null)
        //        {
        //            op.TransNotifications.DeleteOnSubmit(TRL);
        //            op.SubmitChanges();
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hf = (HiddenField)e.Item.FindControl("HiddenField1");
            if (e.CommandName == "DeleteNot")
            {   //Ankita - 11/may/2016- (For Optimization)
                //string tranQry = @"select * from TransNotification where NotiId=" + Convert.ToInt32(hf.Value) + "";
                string tranQry = @"select count(*) from TransNotification where NotiId=" + Convert.ToInt32(hf.Value) + "";
                int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, tranQry));
                //DataTable dtTransNot = DbConnectionDAL.GetDataTable(CommandType.Text, tranQry);
                //     TransNotification TRL = op.TransNotifications.FirstOrDefault(u => u.NotiId == Convert.ToInt32(hf.Value));
                //if (dtTransNot.Rows.Count != 0)
                if (cnt > 0)
                {
                    string tranDelQry = @"delete from TransNotification where NotiId=" + Convert.ToInt32(hf.Value) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, tranDelQry);
                    //    op.TransNotifications.DeleteOnSubmit(TRL);
                    //    op.SubmitChanges();
                    BindRepeater();
                }
            }
        }

        private void BindRepeater()
        {
            //string ckhVDateNotQry = @"select NotiId,VDate from TransNotification where userid=" + Convert.ToInt32(Settings.Instance.UserID) + "";
            //DataTable St1 = DbConnectionDAL.GetDataTable(CommandType.Text, ckhVDateNotQry);

            string bindNotTableQry = @"select trnsnot.NotiId, trnsnot.displayTitle, trnsnot.FromUserId, trnsnot.msgURL, trnsnot.pro_id, trnsnot.Status, trnsnot.userid, trnsnot.VDate,convert(date, trnsnot.VDate) as V1Date, convert(varchar(8), convert(time, trnsnot.VDate),100) as V1Time, salesrep.SMName, salesrep.SMId from TransNotification trnsnot left join MastSalesRep salesrep on trnsnot.FromUserId= salesrep.UserId where trnsnot.userid=" + Convert.ToInt32(Settings.Instance.UserID) + " order by trnsnot.NotiId desc";
            DataTable St = DbConnectionDAL.GetDataTable(CommandType.Text, bindNotTableQry);

            int count = St.Rows.Count;
            Repeater1.DataSource = St;
            Repeater1.DataBind();

        }
    }
}