using FODLApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAMSApi.Models.ViewModel;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using FODLApi.FODLWebService;
using FODLApi.Models.ViewModel;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Data.Entity;

namespace FODLApi.Controllers
{
    

    public class ReportController : ApiController
    {
        private FODLEntities _context = new FODLEntities();
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/printreport")]

        public byte[] PrintReports(string rvm)
        {
            var o = JsonConvert.DeserializeObject(rvm);
            ReportViewModel rptVM = JsonConvert.DeserializeObject<ReportViewModel>(rvm);
            string report = rptVM.Report;
            try
            {
                DataSet ds = new DataSet();
                LocalReport LocalReport = new LocalReport
                {
                    ReportPath = baseDir + "\\Reports\\" + report + ".rdlc"
                };


                DateTime def = new DateTime(1, 1, 1);

                if (report == "rptLiquidation")
                {
                    var v = _context.FuelOilDetails
                                        .Where(a => a.Status == "Active")
                                        .Where(a => a.FuelOilId == rptVM.ReferenceId)
                                        .Select(a => new {


                                            a.FuelOils.Shift
                                        ,
                                            a.FuelOils.CreatedDate
                                        ,
                                            a.FuelOils.ReferenceNo
                                        ,
                                            UnitNo = a.Equipments.No
                                        ,
                                            Location = a.Locations.List
                                        //,
                                        //LubeTruck = a.FuelOils.LubeTrucks.No
                                        //,
                                        //Dispenser = a.FuelOils.Dispensers.No
                                        ,
                                            SourceNo = a.FuelOils.LubeTrucks.No == "na" ? a.FuelOils.Dispensers.Name : a.FuelOils.LubeTrucks.No
                                        ,
                                            a.SMR
                                        ,
                                            Time = a.CreatedDate
                                        //,
                                        //    DieselFuel = 0
                                        ,
                                            a.Id
                                        ,
                                            a.Signature
                                            ,a.FuelOils.CreatedBy
                                        });
                    var lst = v.ToList();
                    var x = v.GroupJoin(
                          _context.FuelOilSubDetails // B
                          .Where(a => a.Status == "Active"),
                          i => i.Id, //A key
                          p => p.FuelOilDetailId,//B key
                          (i, g) =>
                             new
                             {
                                 i, //holds A data
                             g  //holds B data
                         }
                       ).SelectMany(
                          temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                          (A, B) =>
                             new
                             {

                                 A.i.ReferenceNo,
                                 A.i.SourceNo,
                                 EquipmentNo = A.i.UnitNo,
                                 A.i.Shift,
                                 A.i.Location,
                                 A.i.SMR,
                                 A.i.Time,
                                 //A.i.DieselFuel,
                                 VolumeQty = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : B.VolumeQty,
                                 A.i.CreatedDate,
                                 Component = string.IsNullOrEmpty(B.Components.Name) ? "" : B.Components.Name,
                                 DescriptionLiquidation = string.IsNullOrEmpty(B.Items.DescriptionLiquidation) ? "" : B.Items.DescriptionLiquidation,
                                 EP2 = 0,
                                 Coolant = ""
                                 ,
                                 Signature = string.IsNullOrEmpty(A.i.Signature) ? "" : "Signed",
                                 DieselFuel = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Items.No == "FO000001" ? B.VolumeQty : 0),
                                 E30 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Engine" & B.Items.DescriptionLiquidation == "30" ? B.VolumeQty : 0),
                                 E15W = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Engine" & B.Items.DescriptionLiquidation == "15W40" ? B.VolumeQty : 0),
                                 T30 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Transmission" & B.Items.DescriptionLiquidation == "30" ? B.VolumeQty : 0),
                                 TPTT = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Transmission" & B.Items.DescriptionLiquidation == "PTT30" ? B.VolumeQty : 0),
                                 T15W = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Transmission" & B.Items.DescriptionLiquidation == "15W40" ? B.VolumeQty : 0),
                                 H30 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Hydraulics" & B.Items.DescriptionLiquidation == "30" ? B.VolumeQty : 0),
                                 H15 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Hydraulics" & B.Items.DescriptionLiquidation == "15W40" ? B.VolumeQty : 0),
                                 H10 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Hydraulics" & B.Items.DescriptionLiquidation == "10" ? B.VolumeQty : 0),
                                 HPTT = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Hydraulics" & B.Items.DescriptionLiquidation == "PTT30" ? B.VolumeQty : 0),
                                 H68 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Hydraulics" & B.Items.DescriptionLiquidation == "68" ? B.VolumeQty : 0),
                                 F30 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Final Drives" & B.Items.DescriptionLiquidation == "30" ? B.VolumeQty : 0),
                                 FPTT = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Final Drives" & B.Items.DescriptionLiquidation == "PTT50" ? B.VolumeQty : 0),
                                 F140 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "Final Drives" & B.Items.DescriptionLiquidation == "140" ? B.VolumeQty : 0),
                                 P30 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "PTO/Damper" & B.Items.DescriptionLiquidation == "30" ? B.VolumeQty : 0),
                                 P15 = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : (B.Components.Name == "PTO/Damper" & B.Items.DescriptionLiquidation == "15W40" ? B.VolumeQty : 0),
                                 A.i.CreatedBy
                             }
                       );
                    var lsts = x.ToList();
                    DataTable dts = new DataTable();
                    dts = ToDataTable(lsts);
                    ReportDataSource datasources = new ReportDataSource("Liquidation", dts);
                    LocalReport.DataSources.Clear();
                    LocalReport.DataSources.Add(datasources);
                }
                else
                {

                }

                //int c = x.Count();

                return LocalReport.Render(rptVM.rptType);

            }
            catch (Exception e)
            {

                throw;
            }


        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/uploadnav")]
        public JsonResult<NavisionViewModel> UploadToNavision(string batchno,string referenceno)
        {
            string status = "";
            string result = "";


            //int[] fuelid = _context.FuelOils.Where(a => a.Status == "Posted").Select(a => a.Id).ToArray();
            int[] fuelid;

            try
            {
                if (string.IsNullOrEmpty(referenceno))
                {

                    fuelid = _context.FuelOils
                        .Where(a => a.Status == "Posted")
                        .Select(a => a.Id).ToArray();
                }
                else
                {
                    fuelid = referenceno.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                }


                var v =

               _context.FuelOilSubDetails
                  .Where(a => fuelid.Contains(a.FuelOilDetails.FuelOilId))
                  .Where(a => a.Status == "Active")

                  .Select(a => new
                  {
                      EntryType = "Negative Adjmt.",
                      ItemNo = a.Items.No,
                      PostingDate = a.FuelOilDetails.FuelOils.TransactionDate,
                      DocumentDate = a.FuelOilDetails.FuelOils.CreatedDate,
                      Qty = a.VolumeQty,
                      EquipmentCode = a.FuelOilDetails.Equipments.No,
                      a.FuelOilDetails.Locations.OfficeCode,
                      FuelCode = a.Items.TypeFuel == "OIL-LUBE" ? a.FuelOilDetails.Equipments.FuelCodeOil : a.FuelOilDetails.Equipments.FuelCodeDiesel,
                      LocationCode = "SMPC-SITE",
                      a.FuelOilDetails.Equipments.DepartmentCode,
                      a.Id,
                      a.Status,
                      a.FuelOilDetailId,
                      FuelOilSubDetailsId = a.Id,
                      DocumentNo = a.FuelOilDetails.FuelOils.ReferenceNo
                  });

            DataTable dt = new DataTable();
            var lst = v.ToList();
            dt = ToDataTable(lst);

            //---for dev
            
            //FODL_Web_Service ns = new FODL_Web_Service(); //for thulium
            FODLWebServiceMinesite.FODL_Web_Service ns = new FODLWebServiceMinesite.FODL_Web_Service(); // for aprodite

                ns.Url = "http://aprodite.semiraramining.net:7057/BC130_SMPC_TEST/WS/Semirara/Codeunit/FODL_Web_Service";
            //ns.Url = "http://thulium.smcdacon.com:7067/BC130_SMPC_TEST/WS/Semirara/Codeunit/FODL_Web_Service";
                NetworkCredential netCred = new NetworkCredential(@"semiraramining\handshake", "M1ntch0c0l@t3");
           


                Uri uri = new Uri(ns.Url);
                ICredentials credentials = netCred.GetCredential(uri, "Basic");
                ns.Credentials = credentials;
                ns.PreAuthenticate = true;


                int i = 3;
                Boolean NoError = true;
            
                (string.Format("============================== Transfer started  =============================" + DateTime.Now)).WriteLog();
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    try
                    {

                        string.Format("	Batch: {0} DocumentNo: {1} - Item {2} - Fuel {3} - Equipment {4}", batchno, dr["DocumentNo"].ToString() , dr["ItemNo"].ToString(), dr["EquipmentCode"].ToString(), dr["FuelCode"].ToString()).WriteLog();


                        var res = ns.UploadToNavision(batchno, (i * 1000), dr["ItemNo"].ToString(), Convert.ToDateTime(dr["PostingDate"]), Convert.ToDateTime(dr["DocumentDate"]), Convert.ToInt32(dr["Qty"]), dr["EquipmentCode"].ToString(),
                            dr["OfficeCode"].ToString(), dr["FuelCode"].ToString(), dr["LocationCode"].ToString(), dr["DepartmentCode"].ToString());

                        if (res == "Success")
                        {
                            var subdetail = _context.FuelOilSubDetails.Find(Convert.ToInt32(dr["Id"]));
                            subdetail.Status = "Transferred";
                            _context.Entry(subdetail).State = EntityState.Modified;
                            _context.SaveChanges();

                        }
                        string.Format("Result : " + res);
                    }
                    catch (Exception ex)
                    {
                        result = "Unexpected error: " + ex.Message;
                        NoError = false;
                        string.Format("	Error/s: FuelSub Id: {0} Exception: {1} ", dr["FuelOilSubDetailsId"].ToString(), ex.Message.ToString()).WriteLog();
                        string.Format("Result : " + "Fail");
                    }
                    
                }

                if (NoError == false)
                {
                    (string.Format("===== Error/s found: All journal line has been rolled back. No changes made ======" + DateTime.Now)).WriteLog();
                }
                else
                {

                    //var fo = _context.FuelOils.Where(a => a.Status == "Posted");
                    //_context.Entry(fo).Property("FirstName").IsModified = true;
                    //_context.SaveChanges();

                    _context.FuelOils
                        //.Where(a => a.Status == "Posted")
                        .Where(a => fuelid.Contains(a.Id))
                        .ToList()
                        .ForEach(e => e.Status = "Transferred");

                    _context.SaveChanges();
                    result = "success";
                }

                (string.Format("============================== Transfer finished at =============================" + DateTime.Now)).WriteLog();


            }
            catch (Exception ex)
            {
                result = "Unexpected error in NAVISION API : " + ex.Message;
                (string.Format(result)).WriteLog();
            }

            var model = new NavisionViewModel
            {
                batchno = batchno,
                status = status,
                message = result
            };


            return Json(model);
        }

    }
}
