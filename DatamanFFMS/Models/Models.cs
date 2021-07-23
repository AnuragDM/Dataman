using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AstralFFMS.Models
{
    public class Models
    {
        public class FieldDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
        
        }
     
       public class SalesPersonDetails
       {
           public string Role { get; set; }
           public string Department { get; set; }
           public string Designation { get; set; }
           public string Employee { get; set; }
           public string Email { get; set; }
       }

       public class FillSalePersonControls
       {

            public string  SMName{ get; set; }
            public string  SyncId { get; set; }          
            public string  DOA{ get; set; }              
            public string  Address1{ get; set; }   
            public string  Address2{ get; set; }   
            public string  EmpName{ get; set; }   
            public string  Pin{ get; set; }   
            public string  Mobile{ get; set; }   
            public string  DeviceNo{ get; set; }                  
            public string  DOB{ get; set; }   
            public string  Email{ get; set; }   
            public string  Remarks{ get; set; }   
            public string  DSRAllowDays{ get; set; }                      
            public string  RoleID { get; set; }   
            public string  DesID{ get; set; }                
            public string  DeptID{ get; set; }                   
            public string  ReportToID{ get; set; }               
            public string  ResCentre{ get; set; }                    
            public string  CityID{ get; set; }                  
            public string  UserID{ get; set; }                        
            public string  chkhomecity{ get; set; }   
            public string  chkIsActive{ get; set; }
            public string  SalesRepType { get; set; } 
            public string  divblock{ get; set; }   
            public string  BlockReason{ get; set; }
            public string  save { get; set; }
            public string  delete { get; set; }
                 
       }
    }
}