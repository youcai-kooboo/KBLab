//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Data.Model
{
    [Table("ClientScope")]
    public partial class ClientScope
    {
    	[Key]  
        public int ID { get; set; }
    	[ForeignKey("Client")]
        public int ClientID { get; set; }
    	[ForeignKey("Scope")]
        public int ScopeID { get; set; }
    
    	public virtual Client Client { get; set; }
    	public virtual Scope Scope { get; set; }
    }
}