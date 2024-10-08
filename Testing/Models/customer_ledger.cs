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
    
    public partial class customer_ledger
    {
        public string id { get; set; }
        public string customerid { get; set; }
        public string transactionid { get; set; }
        public string receivedfrom { get; set; }
        public string description { get; set; }
        public string paymenttype { get; set; }
        public Nullable<decimal> credit { get; set; }
        public Nullable<decimal> debit { get; set; }
        public Nullable<decimal> balance { get; set; }
        public Nullable<System.DateTime> insertdate { get; set; }
        public string insertuser { get; set; }
        public string username { get; set; }
    
        public virtual useraccount useraccount { get; set; }
        public virtual customer customer { get; set; }
        public virtual transaction transaction { get; set; }
    }
}
