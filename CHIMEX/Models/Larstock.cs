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
    
    public partial class Larstock
    {
        public string id { get; set; }
        public string customerid { get; set; }
        public string product_id { get; set; }
        public int qty { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string insertuser { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual product product { get; set; }
        public virtual useraccount useraccount { get; set; }
    }
}
