//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FODLApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Equipments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipments()
        {
            this.FuelOilDetails = new HashSet<FuelOilDetails>();
        }
    
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string ModelNo { get; set; }
        public string Status { get; set; }
        public string DepartmentCode { get; set; }
        public string FuelCodeDiesel { get; set; }
        public string FuelCodeOil { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelOilDetails> FuelOilDetails { get; set; }
    }
}