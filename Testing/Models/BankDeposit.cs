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
    
    public partial class BankDeposit
    {
        public string id { get; set; }
        public string deposited_by { get; set; }
        public string bank { get; set; }
        public string description { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<System.DateTime> insertdate { get; set; }
        public string insertuser { get; set; }
    
        public virtual useraccount useraccount { get; set; }
    }
}
