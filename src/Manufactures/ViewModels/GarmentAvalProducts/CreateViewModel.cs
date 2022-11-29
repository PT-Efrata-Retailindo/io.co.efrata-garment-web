using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Manufactures.ViewModels.GarmentAvalProducts
{
    public class CreateViewModel
    {
        [Display(Name = "RONo")]
        [Required]
        public string RONo { get; set; }

        [Display(Name = "Article")]
        [Required]
        public string Article { get; set; }

        [Display(Name = "AvalDate")]
        [Required]
        public DateTimeOffset? AvalDate { get; set; }

        [Display(Name = "UnitId")]
        [Required]
        public int UnitId { get; set; }
        [Display(Name = "UnitCode")]
        [Required]
        public string UnitCode { get; set; }
        [Display(Name = "UnitName")]
        [Required]
        public string UnitName { get; set; }

        [Display(Name = "Items")]
        [Required]
        public List<GarmentAvalProductItemValueObject> Items { get; set; }
    }
}