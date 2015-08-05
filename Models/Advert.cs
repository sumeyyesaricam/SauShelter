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
    
    public partial class Advert
    {
        public System.Guid ID { get; set; }
        [Display(Name="�lan Tarihi")]
        [Required]
        public System.DateTime ADVERTDATE { get; set; }
        [Display(Name = "Oda Say�s�")]
        public Nullable<System.Guid> ROOMCOUNTID { get; set; }
        [Display(Name = "Bulundu�u Kat")]
        public Nullable<System.Guid> FLOORID { get; set; }
        [Display(Name = "Is�tma Sistemi")]
        public Nullable<System.Guid> HEATINGID { get; set; }
        [Display(Name = "A��klama")]
        public string EXPLANATION { get; set; }
        [Display(Name = "Ba�l�k")]
        [Required]
        public string TITLE { get; set; }
        [Display(Name = "Banyo Say�s�")]
        public Nullable<int> BATHCOUNT { get; set; }
        [Display(Name = "Fiyat�")]
        [Required]
        public Nullable<int> COST { get; set; }
        [Display(Name = "Ka� Ki�ilik")]
        public Nullable<int> PERSONCOUNT { get; set; }
        [Display(Name = "�lan T�r�")]
        [Required]
        public System.Guid ATYPEID { get; set; }
        [Display(Name = "Balkon")]
        public Nullable<bool> BALCONY { get; set; }
        [Display(Name = "Bah�e")]
        public Nullable<bool> GARDEN { get; set; }
        [Display(Name = "Yap�m Y�l�")]
        public Nullable<System.DateTime> CONSTRUCTIONDATE { get; set; }
        [Display(Name = "Adres")]
        public Nullable<System.Guid> ADDRESSID { get; set; }
        [Display(Name = "Aidat")]
        public Nullable<int> MONTHLYFEE { get; set; }
        [Display(Name = "E�yal�")]
        public Nullable<bool> FULLYFURNISHED { get; set; }
        [Display(Name = "MetreKare")]
        public Nullable<int> SQUAREFEET { get; set; }
        [Display(Name = "T�r�")]
        [Required]
        public System.Guid TYPID { get; set; }
        [Display(Name = "�lan Sahibi")]
        public System.Guid OWNERID { get; set; }
        [Display(Name = "�lan S�resi")]
        [Required]
        public Nullable<System.Guid> TIMEID { get; set; }
        [Display(Name = "Di�er")]
        public string OTHER { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual AdvertType AdvertType { get; set; }
        public virtual ApartmentFloor ApartmentFloor { get; set; }
        public virtual DeliveryTime DeliveryTime { get; set; }
        public virtual Heating Heating { get; set; }
        public virtual Insider Insider { get; set; }
        public virtual RoomCount RoomCount { get; set; }
        public virtual Type Type { get; set; }
    }
}
