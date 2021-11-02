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
    
    public partial class FuelOilDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelOilDetails()
        {
            this.FuelOilSubDetails = new HashSet<FuelOilSubDetails>();
        }
    
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int EquipmentId { get; set; }
        public int LocationId { get; set; }
        public int FuelOilId { get; set; }
        public string Status { get; set; }
        public string SMR { get; set; }
        public string Signature { get; set; }
        public int OldId { get; set; }
    
        public virtual Equipments Equipments { get; set; }
        public virtual FuelOils FuelOils { get; set; }
        public virtual Locations Locations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelOilSubDetails> FuelOilSubDetails { get; set; }
    }
}
