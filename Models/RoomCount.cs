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
    using System.ComponentModel.DataAnnotations;
    
    public partial class RoomCount
    {
        public RoomCount()
        {
            this.Advert = new HashSet<Advert>();
        }
    
        public System.Guid ID { get; set; }
        [Display(Name="Oda Say�s�")]
        public string NAME { get; set; }
    
        public virtual ICollection<Advert> Advert { get; set; }
    }
}
