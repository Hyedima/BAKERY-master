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
    
    public partial class main_stocks_Report
    {
        public string id { get; set; }
        public string main_stock_id { get; set; }
        public string product_id { get; set; }
        public string stock_Type { get; set; }
        public int qty { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string insertuser { get; set; }
        public string notes { get; set; }
        public string batch_id { get; set; }
    
        public virtual main_stocks main_stocks { get; set; }
        public virtual useraccount useraccount { get; set; }
    }
}
