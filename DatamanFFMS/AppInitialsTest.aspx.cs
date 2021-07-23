using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using BusinessLayer;

namespace AstralFFMS
{
    public partial class AppInitialsTest : System.Web.UI.Page
    {
        string host = ""; string SMID = ""; string maxid = "";
        protected void Page_Load(object sender, EventArgs e)
        {          

            mainDiv.Style.Add("display", "block");
           // rptmain.Style.Add("display", "none");
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            try
            {
                LicenceInfo();
                if (host != "No")
                {
                    userinfo();
                    if (SMID != "")
                    {
                        EnviroSettinginfo();
                        TransPorterInfo();
                        ProjectInfo();
                        CountryInfo1();
                        RegionInfo1();
                        PartyTypeInfo1();
                        AreaInfo1();
                        DistributorsInfo1();
                        SchemeInfo1();
                        BeatsInfo1();
                        CityInfo1();
                        DistrictInfo1();
                        FailedVisitRemarkInfo1();
                        PartyInfoid1();
                        IndustryInfo1();
                        ProductsInfo1();
                        VehicleInfo();
                        TransportInfo();
                        DSRInfo();
                        JSGetDistributorFailedVisitInfo();
                        JSGetDistributorCollectionInfo();
                        PartyMaxId();
                        PartyInfo1();
                    }
                    else
                    {
                        string createText = "";
                        createText += "Smid Of This User Is Not Available";
                        System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);

                        TextFileCID.WriteLine(createText); TextFileCID.Close();
                    }

                }
                else
                {
                    string createText = "";
                    createText += "Company Url Of This Device or User Is Not Available";
                    System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);

                    TextFileCID.WriteLine(createText);
                    TextFileCID.Close();
                }

                //DownloadFile();
                DownloadFile1();
            }
            catch(Exception )
            {
                DownloadFile1();
            }
        }
        public void LicenceInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            string appId = "<App Id>";

            using (WebClient client = new WebClient())
            {
                url = "http://license.dataman.in//jsonwebservice.asmx/JGetLicenseDetailForCRM?DeviceNo=" + txtdevice.Value;
                string json = client.DownloadString(url);

                Deviceinfo Deviceinformation = (new JavaScriptSerializer()).Deserialize<Deviceinfo>(json);
                string info = Deviceinformation.CompName + Deviceinformation.CompUrl + Deviceinformation.AppType;
                createText += "********************************     App Initials For User " + txtdevice.Value + "    *******************************";
                createText += Environment.NewLine;
                createText += "--------------------------------       Start Checking Company Licence        --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                createText += Environment.NewLine;
                createText += "Company Name :" + Deviceinformation.CompName + " , Company Url :" + Deviceinformation.CompUrl + ", App Type :" + Deviceinformation.AppType;
                createText += Environment.NewLine;
                createText += "--------------------------------       End Checking Company Licence           -------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                host = Deviceinformation.CompUrl;
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();          

            }
        }

     
        public void userinfo()
        {
            //string compurl = LicenceInfo();
            string createText = ""; string CompanyUrl = "";
            string url = "";
            string appId = "<App Id>";
            using (WebClient client = new WebClient())
            {
              
                url = "http://" + host + "/And_Sync.asmx/xJSGetUserDetailByPDAId?PDA_Id=" + txtdevice.Value + "&minDate=0";
                string json = client.DownloadString(url);
            //   var blah = (new JavaScriptSerializer()).Deserialize<Userinfo>(json);
              //  RootObject r = JsonConvert.DeserializeObject<RootObject>(json);
                var objResponse1 = JsonConvert.DeserializeObject<List<UserInfo>>(json);

                createText = "--------------------------------       Start Checking User Details        -----------------------------------";
                //string Smid = objResponse1[0].smid; string Name = objResponse1[1].NM; string UserId = objResponse1[0].smid;
                //string Level = objResponse1[0].smid; string Days = objResponse1[0].smid; string Email = objResponse1[0].smid;
                //string Active = objResponse1[0].smid;
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                createText += Environment.NewLine;
                //createText += "User Device No :" + objResponse1.smid;
                createText += "ConPerId :" + objResponse1[0].smid.ToString() + " , User Name:" + objResponse1[0].NM.ToString() + ", UserId :" + objResponse1[0].Uid.ToString() + ", Password : ******* ,Level :" + objResponse1[0].Lvl.ToString();
                createText += Environment.NewLine;
                createText +="Dsr Allowed Days:" + objResponse1[0].AllDyz.ToString() + ", Email :" + objResponse1[0].Em.ToString() + ", Active :" + objResponse1[0].Act.ToString();
                createText += Environment.NewLine;
                createText += "--------------------------------       End Checking User Details        -------------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                SMID = objResponse1[0].smid;
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
                
            }
        }
        public void EnviroSettinginfo()
        {
            //string compurl = LicenceInfo();
            string createText = ""; string CompanyUrl = "";
            string url = "";
            string appId = "<App Id>";
            using (WebClient client = new WebClient())
            {
                url = "http://"+host+"//And_Sync.asmx/JSGetEnviroSetting";
                string json = client.DownloadString(url);
                //   var blah = (new JavaScriptSerializer()).Deserialize<Userinfo>(json);
                //  RootObject r = JsonConvert.DeserializeObject<RootObject>(json);
                var objResponse1 = JsonConvert.DeserializeObject<List<envirosetting>>(json);

                createText = "--------------------------------       Start Checking Enviro Settings        --------------------------------";
                //string Smid = objResponse1[0].smid; string Name = objResponse1[1].NM; string UserId = objResponse1[0].smid;
                //string Level = objResponse1[0].smid; string Days = objResponse1[0].smid; string Email = objResponse1[0].smid;
                //string Active = objResponse1[0].smid;
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                createText += Environment.NewLine;
                //createText += "User Device No :" + objResponse1.smid;
                createText += "Party Caption :" + objResponse1[0].PartyCaption.ToString() + " , Distributor Search:" + objResponse1[0].DistSearch.ToString() + ", Item Search :" + objResponse1[0].ItemSearch.ToString() + ",ItemWise Sale :" + objResponse1[0].Itemwisesale.ToString();
                createText += Environment.NewLine;
                createText +="AreaWise Distributor:" + objResponse1[0].AreawiseDistributor.ToString() + ",Distributor Disc Stock :" + objResponse1[0].DistDiscussionStock.ToString() + ", Dsr Remark Mandstory :" + objResponse1[0].DsrRemarkMandatory.ToString();
                createText += Environment.NewLine;
                createText += "--------------------------------       End Checking Enviro Settings        ----------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
             
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
                
            }
        }
        public void TransPorterInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://"+host+"//And_Sync.asmx/xJSGetTransporter?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<TransporterInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------       Start Importing Transporter Info      --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                createText += Environment.NewLine;
                createText += "Id:" + objResponse1[i].Id + " ,Transporter Name :" + objResponse1[i].Nm ;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------       End Importing Transporter Info         -------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void ProjectInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://"+host+"//And_Sync.asmx/xJSGetProjects?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<TransporterInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing Project Info        --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Id:" + objResponse1[i].Id + " ,Project Name :" + objResponse1[i].Nm;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Project Info           -------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void CountryInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://"+host+"//And_Sync.asmx/xJSGetCountry?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<CountryInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing Country Info        --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Id:" + objResponse1[i].Cid + " ,Country Name :" + objResponse1[i].NM;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Country Info           -------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void RegionInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetRegions?ConPer_Id="+SMID+"&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<CountryInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing Region Info         --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Region Name :" + objResponse1[i].NM;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Region Info            -------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void PartyTypeInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetPartyType?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<PartyTypeInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing Party Type Info    --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Party Type Name :" + objResponse1[i].Nm;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Party Type Info     --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void AreaInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetArea?ConPer_Id="+SMID+"&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<AreaInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------          Start Importing Area Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Area Name :" + objResponse1[i].Nm + ",MilliSeconds :" + objResponse1[i].MS;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------           End Importing Area Info        --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void DistributorsInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetDistributors?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<DistributorInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------     Start Importing Distributors Info   --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Distributor Name :" + objResponse1[i].Pnm + ",MilliSeconds :" + objResponse1[i].MS + ",Mo. No.:" + objResponse1[i].Mo + ",Email :" + objResponse1[i].Em + " ,Contact Person :" + objResponse1[i].Ctp;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Distributors Info   --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }

        }
        public void SchemeInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetScheme?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<TransporterInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------       Start Importing Scheme Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Scheme :" + objResponse1[i].Nm;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------          End Importing Scheme Info      --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void BeatsInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetBeats?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<BeatInfo>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing Beats Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Beat Name :" + objResponse1[i].NM ;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------          End Importing Beats Info       --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }

        }
        public void CityInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetCities?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xCities>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing City Info        --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "City Name :" + objResponse1[i].NM;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------          End Importing City Info        --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }

        }
        public void DistrictInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetDistricts?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xDistricts>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------        Start Importing District Info     --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "District Name :" + objResponse1[i].NM;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------          End Importing District Info     --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }

        }
        public void FailedVisitRemarkInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetFaildVisitRemark?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<FaildVisitRemark>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------Start Importing Failed Visit Remark Info --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Failed Visit Remark :" + objResponse1[i].FvName;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------  End Importing Failed Visit Remark Info --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }

        }
        public void PartyMaxId()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetPartiesNewmaxid?ConPer_Id="+SMID+"&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xPartiesNew>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------       Start Importing Max Id Info      --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                //for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Max Id Of Party :" + objResponse1[0].ID;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------        End Importing Max Id Info        --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
                maxid = objResponse1[0].ID;
            }

        }
        public void PartyInfo1()
        {
            int id = Convert.ToInt32(maxid); string createText = ""; string CompanyUrl = "";
            string url = "";
            int fromid = 1; int toid = 10000;
            if(id>10000)
            {
                int quotient = id / 10000;
                for(int k=1;k<=quotient+1;k++)
                {
                    using (WebClient client = new WebClient())
                    {
                        url = "http://" + host + "//And_Sync.asmx/xJSGetPartiesNewTest?ConPer_Id=" + SMID + "&minDate=0&fromid=" + fromid + "&toid=" + toid + "";
                        string json = client.DownloadString(url);
                        var objResponse1 = JsonConvert.DeserializeObject<List<xPartiesNew>>(json);
                        createText += Environment.NewLine;
                        createText += "--------------------------------       Start Importing Party Info       --------------------------------";
                        createText += Environment.NewLine;
                        createText += "Date ::" + DateTime.Now.ToString();
                        for (int i = 0; i < objResponse1.Count; i++)
                        {
                            createText += Environment.NewLine;
                            createText += "Party Name:" + objResponse1[i].Nm + ", Address:" + objResponse1[i].Ad1 + " ,Email :" + objResponse1[i].E + " ,Mobile :" + objResponse1[i].M + " ,Contact Person :" + objResponse1[i].Cp + " ,IsBlocked:" + objResponse1[i].blk;
                        }
                        createText += Environment.NewLine;
                        createText += "--------------------------------         End Importing Party Info       --------------------------------";
                       
                    }
                    fromid = toid + 1;
                    toid = fromid + 9999;
                }
              
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    url = "http://" + host + "//And_Sync.asmx/xJSGetPartiesNewTest?ConPer_Id=" + SMID + "&minDate=0&fromid=" + fromid + "&toid=" + toid + "";
                    string json = client.DownloadString(url);
                    var objResponse1 = JsonConvert.DeserializeObject<List<xPartiesNew>>(json);
                    createText += Environment.NewLine;
                    createText += "--------------------------------       Start Importing Party Info       --------------------------------";
                    createText += Environment.NewLine;
                    createText += "Date ::" + DateTime.Now.ToString();
                    for (int i = 0; i < objResponse1.Count; i++)
                    {
                        createText += Environment.NewLine;
                        createText += "Party Name:" + objResponse1[i].Nm + ", Address:" + objResponse1[i].Ad1 + " ,Email :" + objResponse1[i].E + " ,Mobile :" + objResponse1[i].M + " ,Contact Person :" + objResponse1[i].Cp + " ,IsBlocked:" + objResponse1[i].blk;
                    }
                    createText += Environment.NewLine;
                    createText += "--------------------------------         End Importing Party Info       --------------------------------";

                }
            }
            System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
            TextFileCID.WriteLine(createText);
            TextFileCID.Close();
           
           
        }
        public void PartyInfoid1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetPartiesNewmaxid?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xPartiesNew>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------       Start Importing Party Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Party Name:" + objResponse1[i].ID + ", Address:" + objResponse1[i].Ad1 + " ,Email :" + objResponse1[i].E + " ,Mobile :" + objResponse1[i].M + " ,Contact Person :" + objResponse1[i].Cp + " ,IsBlocked:" + objResponse1[i].blk;
                }
                maxid = objResponse1[0].ID;
                createText += Environment.NewLine;
                createText += "--------------------------------         End Importing Party Info       --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void IndustryInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetIndustry?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xIndustry>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------     Start Importing Industry Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Industry Name:" + objResponse1[i].Nm ;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------       End Importing Industry Info       --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void ProductsInfo1()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetProducts?minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xProducts>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------     Start Importing Item Info         --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Item Name:" + objResponse1[i].Itnm + ",Unit:" + objResponse1[i].Unit + " ,Mrp :" + objResponse1[i].MRP+ ",Std Pack :" + objResponse1[i].StdPk;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------       End Importing Item Info          --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void TransportInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetTransportVehicle?type=Transport";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xTransportVehicleused>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------     Start Importing Transport Info     --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Transport:" + objResponse1[i].Trpt ;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------       End Importing Transport Info      --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void VehicleInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/xJSGetTransportVehicle?type=vehicle";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<xTransportVehicleused>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------     Start Importing Transport Info     --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Vehicle:" + objResponse1[i].Trpt;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------       End Importing Transport Info      --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void DSRInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/JSGetDSR?ConPer_Id="+SMID+"&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<DSR>>(json);
                createText += Environment.NewLine;
                createText += "--------------------------------         Start Importing DSR Info       --------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Doc ID:" + objResponse1[i].visitDocId + ", Visit Date:" + objResponse1[i].Vdate + " ,Remark :" + objResponse1[i].Remark + " ,Next VisitDate :" + objResponse1[i].NextVisitDate + " ,App. Status:" + objResponse1[i].AppStatus + " ,App Remark:" + objResponse1[i].AppRemark;
                }
                createText += Environment.NewLine;
                createText += "--------------------------------          End Importing DSR Info         --------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void JSGetDistributorFailedVisitInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/JSGetDistributorFailedVisit?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<DistributorFailedVisit>>(json);
                createText += Environment.NewLine;
                createText += "-----------------------------Start Importing Distributor Failed Visit Info ------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Doc ID:" + objResponse1[i].FVDocId + ", Visit Date:" + objResponse1[i].VDate + " ,Remark :" + objResponse1[i].Remark + " ,Next VisitDate :" + objResponse1[i].NextVisitDate ;
                }
                createText += Environment.NewLine;
                createText += "------------------------------End Importing Distributor Failed Visit Info ------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void JSGetDistributorCollectionInfo()
        {
            string createText = ""; string CompanyUrl = "";
            string url = "";
            using (WebClient client = new WebClient())
            {
                url = "http://" + host + "//And_Sync.asmx/JSGetDistributorCollection?ConPer_Id=" + SMID + "&minDate=0";
                string json = client.DownloadString(url);
                var objResponse1 = JsonConvert.DeserializeObject<List<DistributorCollection>>(json);
                createText += Environment.NewLine;
                createText += "-----------------------------Start Importing Distributor Collection Info ------------------------------";
                createText += Environment.NewLine;
                createText += "Date ::" + DateTime.Now.ToString();
                for (int i = 0; i < objResponse1.Count; i++)
                {
                    createText += Environment.NewLine;
                    createText += "Doc ID:" + objResponse1[i].CollDocId + ", Visit Date:" + objResponse1[i].VDate + " ,Mode :" + objResponse1[i].Mode + " ,Amount:" + objResponse1[i].Amount + ", Bank:" + objResponse1[i].Bank + ", Branch:" + objResponse1[i].Branch + ", Remarks:" + objResponse1[i].Remarks;
                }
                createText += Environment.NewLine;
                createText += "------------------------------End Importing Distributor  Collection Info ------------------------------";
                System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt"), true);
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
        }
        public void DownloadFile()
        {
            string filePath = "TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt";
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
           // System.IO.File.Delete(Server.MapPath(filePath));
            Response.End();
        }
        public void DownloadFile1()
        {
            string filePath = "TextFileFolder/AppLogFile-" + txtdevice.Value + ".txt";
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition",
                            "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(Server.MapPath(filePath));
            Response.Flush();
            System.IO.File.Delete(Server.MapPath(filePath));
            Response.End();
        }
        public class DistributorCollection
        {
            public string CollId { get; set; }
            public string VisId { get; set; }
            public string CollDocId { get; set; }
            public string Android_Id { get; set; }
            public string VDate { get; set; }
            public string UserId { get; set; }
            public string SMID { get; set; }
            public string Partyid { get; set; }
            public string AreaId { get; set; }
            public string Mode { get; set; }
            public string Amount { get; set; }
            public string PaymentDate { get; set; }
            public string Cheque_DD_No { get; set; }
            public string Cheque_DD_Date { get; set; }
            public string Bank { get; set; }
            public string Branch { get; set; }
            public string Remarks { get; set; }
            public string CreatedDate { get; set; }
        }
        public class DistributorFailedVisit
        {
            public string FVId { get; set; }
            public string VisId { get; set; }
            public string FVDocId { get; set; }
            public string Android_Id { get; set; }
            public string VDate { get; set; }
            public string UserId { get; set; }
            public string SMID { get; set; }
            public string DistId { get; set; }
            public string AreaId { get; set; }
            public string Remark { get; set; }
            public string NextVisitDate { get; set; }
            public string VisitTime { get; set; }
            public string CreatedDate { get; set; }
            public string reasonId { get; set; }
        }

        public class DSR
        {
            public string VisId { get; set; }
            public string visitDocId { get; set; }
            public string UserId { get; set; }
            public string Vdate { get; set; }
            public string Remark { get; set; }
            public string SMID { get; set; }
            public string CityId { get; set; }
            public string DistId { get; set; }
            public string nCityId { get; set; }
            public string NextVisitDate { get; set; }
            public string FROMTime { get; set; }
            public string ToTime { get; set; }
            public string WithUserId { get; set; }
            public string ModeOfTransport { get; set; }
            public string VehicleUsed { get; set; }
            public string Industry { get; set; }
            public string Lock { get; set; }
            public string nWithUserId { get; set; }
            public string AppStatus { get; set; }
            public string AppBy { get; set; }
            public string AppRemark { get; set; }
            public string AppBySMID { get; set; }
            public string Android_Id { get; set; }
            public string cityIds { get; set; }
            public string cityName { get; set; }
            public string Createddate { get; set; }
            public string OrderAmountMail { get; set; }
            public string OrderAmountPhone { get; set; }

        }
        public class envirosetting
        {
            public string PartyCaption { get; set; }
            public string DistSearch { get; set; }
            public string ItemSearch { get; set; }
            public string Itemwisesale { get; set; }
            public string AreawiseDistributor { get; set; }
            public string Host { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Port { get; set; }
            public string FTP_DIRECTORY { get; set; }
            public string IMAGE_DIRECTORY_NAME { get; set; }
            public string DistDiscussionStock { get; set; }
            public string DsrRemarkMandatory { get; set; }
        }
        public class Deviceinfo
        {
            public string CompName { get; set; }
            public string CompUrl { get; set; }
            public string AppType { get; set; }
            public List<List> list { get; set; }
        }
       
        public class UserInfo
        {
            public string smid { get; set; }
            public string NM { get; set; }
            public string Uid { get; set; }
            public string Pwd { get; set; }
            public string Lvl { get; set; }
            public string Rlid { get; set; }
            public string RoleId { get; set; }
            public string AllDyz { get; set; }
            public string Em { get; set; }
            public string Act { get; set; }
            public string Rltyp { get; set; }
            public List<RootObject> list { get; set; }
        }
        public class TransporterInfo
        {
            public string Id { get; set; }
            public string Nm { get; set; }

        }
        public class CountryInfo
        {
            public string Cid { get; set; }
            public string NM { get; set; }

        }
        public class RegionInfo
        {
            public string Rid { get; set; } 
            public string NM { get; set; } 
            public string Cid { get; set; }

        }
        public class PartyTypeInfo
        {
            public string Id { get; set; }
            public string Nm { get; set; }
            public string MS { get; set; }
        }
        public class AreaInfo
        {
            public string MS { get; set; }
            public string Nm { get; set; }
            public string Id { get; set; }
            public string CID { get; set; }

        }
        public class DistributorInfo
        {     
            public string MS { get; set; }
            public string Pid { get; set; }
            public string Pnm { get; set; }
            public string Add1 { get; set; }
            public string Add2 { get; set; }
            public string Pin { get; set; }
            public string Aid { get; set; }
            public string Em { get; set; }
            public string Mo { get; set; }
            public string Indid { get; set; }
            public string Rmk { get; set; }
            public string Ctp { get; set; }
            public string CSTNo { get; set; }
            public string Vattin { get; set; }
            public string SrTx { get; set; }
            public string Panno { get; set; }
            public string Cid { get; set; }
            public string Crlmt { get; set; }
            public string OS { get; set; }
            public string Ph { get; set; }
            public string Opor { get; set; }
            public string Crd { get; set; }
            public string SMID { get; set; }
            public string SyncId { get; set; }
        }
        public class BeatInfo
        {
            public string BID { get; set; }
            public string NM { get; set; }
            public string AId { get; set; }
        }
        public class xCities
        {  
            public string MS { get; set; }
            public string NM { get; set; }
            public string Cid { get; set; }
            public string Did { get; set; }
        }
        public class xDistricts
        {
            public string Did { get; set; }
            public string NM { get; set; }
            public string Sid { get; set; }
        }
        public class FaildVisitRemark
        {
            public string ID { get; set; }
            public string FvName { get; set; }
            public string SyncId { get; set; }
            public string Active { get; set; }
            public string CreatedDate { get; set; }
            public string Millisecond { get; set; }
        }
        public class xPartiesNew
        { 
            public string MS { get; set; }
            public string ID { get; set; }
            public string Ad { get; set; }
            public string Bd { get; set; }
            public string PCd { get; set; }
            public string Cp { get; set; }
            public string Nm { get; set; }
            public string Ind { get; set; }
            public string DsId { get; set; }
            public string Ad1 { get; set; }
            public string Ad2 { get; set; }
            public string Pi { get; set; }
            public string Ct { get; set; }
            public string M { get; set; }
            public string E { get; set; }
            public string Pl { get; set; }
            public string blk { get; set; }
            public string CST { get; set; }
            public string VTn { get; set; }
            public string STNo { get; set; }
            public string Brzn { get; set; }
            public string Bdt { get; set; }
            public string Bby { get; set; }
            public string At { get; set; }
            public string R { get; set; }
            public string PAN { get; set; }
            public string DA { get; set; }
            public string DB { get; set; }
        }
        public class xIndustry
        { 
            public string Id { get; set; }
            public string Nm { get; set; }
            public string MS { get; set; }
        }
        public class xTransportVehicleused
        {
           
            public string Trpt { get; set; }

        }
        public class xProducts
        {
            public string MS { get; set; }
            public string Itid { get; set; }
            public string Itnm { get; set; }
            public string Unit { get; set; }
            public string StdPk { get; set; }
            public string MRP { get; set; }
            public string DP { get; set; }
            public string RP { get; set; }
            public string Uid { get; set; }
            public string Itcd { get; set; }
            public string DM { get; set; }
            public string SegId { get; set; }
            public string Clid { get; set; }
            public string Pg { get; set; }

        }
        public class RootObject
        {
            public List<UserInfo> People { get; set; }
        }
        public class List
        {
            public List<Deviceinfo> People { get; set; }
        }
    }
}