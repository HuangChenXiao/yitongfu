//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class sh_business_commison
    {
        public int id { get; set; }
        public string commorderno { get; set; }
        public Nullable<int> sh_businessid { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string bankaccountno { get; set; }
        public string bankusername { get; set; }
        public string bankname { get; set; }
        public string bankaddress { get; set; }
        public string mobileno { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public Nullable<System.DateTime> audittime { get; set; }
        public string reason { get; set; }
    }
}
