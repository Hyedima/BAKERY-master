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
    
    public partial class paymentvoucher
    {
        public string id { get; set; }
        public string pvno { get; set; }
        public string cheque_no { get; set; }
        public string head { get; set; }
        public string subhead { get; set; }
        public string being { get; set; }
        public string amount_word { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string approved_by { get; set; }
        public Nullable<System.DateTime> approved_on { get; set; }
    }
}
