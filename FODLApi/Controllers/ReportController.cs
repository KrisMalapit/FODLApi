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


                

                var v = _context.FuelOilDetails
                    .Where(a => a.Status == "Active")
                    .Where(a=>a.FuelOilId == rptVM.ReferenceId)
                    .Select(a => new {
                   
                 
                    a.FuelOils.Shift
                    ,
                    a.FuelOils.CreatedDate
                    ,
                    a.FuelOils.ReferenceNo
                    ,
                    UnitNo = a.Equipments.Name
                    ,
                    Location = a.Locations.List
                    //,
                    //LubeTruck = a.FuelOils.LubeTrucks.No
                    //,
                    //Dispenser = a.FuelOils.Dispensers.No
                    ,
                    SourceNo = a.FuelOils.LubeTrucks.No == "na" ? a.FuelOils.Dispensers.No : a.FuelOils.LubeTrucks.No
                    ,
                    a.SMR
                    ,
                    Time = a.CreatedDate
                    ,
                    DieselFuel = 0
                    ,a.Id
                    ,a.Signature

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
                             A.i.DieselFuel,
                             VolumeQty = string.IsNullOrEmpty(B.VolumeQty.ToString()) ? 0 : B.VolumeQty,
                             A.i.CreatedDate,
                             Component = string.IsNullOrEmpty(B.Components.Name) ? "" : B.Components.Name,
                             DescriptionLiquidation = string.IsNullOrEmpty(B.Items.DescriptionLiquidation) ? "" : B.Items.DescriptionLiquidation,
                             EP2 = 0,
                             Coolant = ""
                             ,
                             Signature = string.IsNullOrEmpty(A.i.Signature) ? "" : "Signed",
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

                         }
                   );



                //if (rptVM.fromDate != def)
                //{
                //    x = x.Where(a => a.CreatedDate >= rptVM.fromDate && a.CreatedDate <= rptVM.toDate);
                //}

                //if (rptVM.LevelId != null)
                //{
                //    x = x.Where(a => rptVM.LevelId.Contains(a.LevelId));
                //}
                //if (rptVM.EmployeeName != null)
                //{
                //    x = x.Where(a => rptVM.EmployeeName.Contains(a.EmployeeId));
                //}
                //if (rptVM.EmployeeType != 0)
                //{
                //    x = x.Where(a => a.TypeId == rptVM.EmployeeType);
                //}
                int c = x.Count();
                var lsts = x.ToList();
                DataTable dts = new DataTable();
                dts = ToDataTable(lsts);
                ReportDataSource datasources = new ReportDataSource("Liquidation", dts);
                LocalReport.DataSources.Clear();
                LocalReport.DataSources.Add(datasources);
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

    }
}
