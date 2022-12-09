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
    
    public partial class order_cart
    {
        public string id { get; set; }
        public string product_id { get; set; }
        public string stock_id { get; set; }
        public string product_name { get; set; }
        public string customer_id { get; set; }
        public string transaction_id { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<decimal> unitprice { get; set; }
        public Nullable<decimal> total_amount { get; set; }
        public Nullable<System.DateTime> date_sold { get; set; }
        public string sold_by { get; set; }
        public string insert_user { get; set; }
        public string order_id { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual Order Order { get; set; }
        public virtual product product { get; set; }
        public virtual stock stock { get; set; }
        public virtual transaction transaction { get; set; }
        public virtual useraccount useraccount { get; set; }
    }
}
