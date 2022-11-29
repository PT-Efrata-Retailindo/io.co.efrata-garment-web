using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.ViewModels.GarmentPreparings
{
    public class CreateViewModel
    {
        [Display(Name = "UENId")]
        [Required]
        public int UENId { get; set; }

        [Display(Name = "UENNo")]
        [Required]
        public string UENNo { get; set; }

        [Display(Name = "UnitId")]
        [Required]
        public int UnitId { get; set; }

        [Display(Name = "UnitCode")]
        [Required]
        public string UnitCode { get; set; }

        [Display(Name = "UnitName")]
        [Required]
        public string UnitName { get; set; }

        [Display(Name = "ProcessDate")]
        [Required]
        public DateTimeOffset? ProcessDate { get; set; }

        [Display(Name = "RONo")]
        [Required]
        public string RONo { get; set; }

        [Display(Name = "Article")]
        [Required]
        public string Article { get; set; }

        [Display(Name = "IsCuttingIn")]
        [Required]
        public bool IsCuttingIn { get; set; }

        [Display(Name = "Items")]
        [Required]
        public List<GarmentPreparingItemValueObject> Items { get; set; }

        [Display(Name = "BuyerId")]
        [Required]
        public int BuyerId { get; set; }

        [Display(Name = "BuyerCode")]
        [Required]
        public string BuyerCode { get; set; }

        [Display(Name = "BuyerName")]
        [Required]
        public string BuyerName { get; set; }
    }
}
