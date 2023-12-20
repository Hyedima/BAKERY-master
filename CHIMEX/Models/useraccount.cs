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
    
    public partial class useraccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public useraccount()
        {
            this.Balances = new HashSet<Balance>();
            this.BankDeposits = new HashSet<BankDeposit>();
            this.carts = new HashSet<cart>();
            this.customer_ledger = new HashSet<customer_ledger>();
            this.customers = new HashSet<customer>();
            this.Empties_Backload = new HashSet<Empties_Backload>();
            this.EmptiesReceiveds = new HashSet<EmptiesReceived>();
            this.empty_stock = new HashSet<empty_stock>();
            this.EspectedEmpties = new HashSet<EspectedEmpty>();
            this.Expensis = new HashSet<Expensi>();
            this.Expensis1 = new HashSet<Expensi>();
            this.larstock_added = new HashSet<larstock_added>();
            this.Larstocks = new HashSet<Larstock>();
            this.main_stocks = new HashSet<main_stocks>();
            this.main_stocks_Report = new HashSet<main_stocks_Report>();
            this.main_stocks_Report_temp = new HashSet<main_stocks_Report_temp>();
            this.order_cart = new HashSet<order_cart>();
            this.Orders = new HashSet<Order>();
            this.sales = new HashSet<sale>();
            this.sales_canceled = new HashSet<sales_canceled>();
            this.sales_canceled1 = new HashSet<sales_canceled>();
            this.sales_canceled2 = new HashSet<sales_canceled>();
            this.sales_canceled3 = new HashSet<sales_canceled>();
            this.stock_complain = new HashSet<stock_complain>();
            this.stocks = new HashSet<stock>();
            this.stocks_added = new HashSet<stocks_added>();
            this.stocks_closing = new HashSet<stocks_closing>();
            this.stocks_opening = new HashSet<stocks_opening>();
            this.supplies = new HashSet<supply>();
            this.transactions = new HashSet<transaction>();
            this.productions = new HashSet<production>();
            this.salarybatches = new HashSet<salarybatch>();
        }
    
        public string id { get; set; }
        public string branchid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string othernames { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public Nullable<System.DateTime> regdate { get; set; }
        public string usertype { get; set; }
        public string isactive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Balance> Balances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankDeposit> BankDeposits { get; set; }
        public virtual Branch Branch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> carts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customer_ledger> customer_ledger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customer> customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Empties_Backload> Empties_Backload { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmptiesReceived> EmptiesReceiveds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<empty_stock> empty_stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EspectedEmpty> EspectedEmpties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expensi> Expensis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expensi> Expensis1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<larstock_added> larstock_added { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Larstock> Larstocks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<main_stocks> main_stocks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<main_stocks_Report> main_stocks_Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<main_stocks_Report_temp> main_stocks_Report_temp { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_cart> order_cart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sale> sales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_canceled> sales_canceled { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_canceled> sales_canceled1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_canceled> sales_canceled2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_canceled> sales_canceled3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stock_complain> stock_complain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stock> stocks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stocks_added> stocks_added { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stocks_closing> stocks_closing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stocks_opening> stocks_opening { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supply> supplies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaction> transactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<production> productions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<salarybatch> salarybatches { get; set; }
    }
}
