//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CHIMEX.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emptytype
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Emptytype()
        {
            this.Empties_Backload = new HashSet<Empties_Backload>();
            this.EmptiesReceiveds = new HashSet<EmptiesReceived>();
            this.EspectedEmpties = new HashSet<EspectedEmpty>();
        }
    
        public string id { get; set; }
        public string name { get; set; }
        public Nullable<decimal> cost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Empties_Backload> Empties_Backload { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmptiesReceived> EmptiesReceiveds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EspectedEmpty> EspectedEmpties { get; set; }
    }
}
