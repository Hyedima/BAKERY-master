//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmptiesReceived
    {
        public string id { get; set; }
        public string emptcustid { get; set; }
        public string customerid { get; set; }
        public string itemid { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> insertdate { get; set; }
        public string insertuser { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual Emptytype Emptytype { get; set; }
        public virtual useraccount useraccount { get; set; }
    }
}
