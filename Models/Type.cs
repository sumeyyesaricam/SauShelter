//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SauShelter.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Type
    {
        public Type()
        {
            this.Advert = new HashSet<Advert>();
            this.OtherAdvert = new HashSet<OtherAdvert>();
        }
    
        public System.Guid ID { get; set; }
        public string NAME { get; set; }
    
        public virtual ICollection<Advert> Advert { get; set; }
        public virtual ICollection<OtherAdvert> OtherAdvert { get; set; }
    }
}