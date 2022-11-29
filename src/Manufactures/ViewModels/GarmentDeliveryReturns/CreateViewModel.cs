using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Manufactures.ViewModels.GarmentDeliveryReturns
{
    public class CreateViewModel
    {
        [Display(Name = "DRNo")]
        [Required]
        public string DRNo { get; set; }
        [Display(Name = "RONo")]
        [Required]
        public string RONo { get; set; }
        [Display(Name = "Article")]
        [Required]
        public string Article { get; set; }
        [Display(Name = "UnitDOId")]
        [Required]
        public int UnitDOId { get; set; }
        [Display(Name = "UnitDONo")]
        [Required]
        public string UnitDONo { get; set; }
        [Display(Name = "UENId")]
        [Required]
        public int UENId { get; set; }
        [Display(Name = "PreparingId")]
        [Required]
        public string PreparingId { get; set; }
        [Display(Name = "ReturnDate")]
        [Required]
        public DateTimeOffset ReturnDate { get; set; }
        [Display(Name = "ReturnType")]
        [Required]
        public string ReturnType { get; set; }
        [Display(Name = "UnitId")]
        [Required]
        public int UnitId { get; set; }

        [Display(Name = "UnitCode")]
        [Required]
        public string UnitCode { get; set; }

        [Display(Name = "UnitName")]
        [Required]
        public string UnitName { get; set; }

        [Display(Name = "StorageId")]
        [Required]
        public int StorageId { get; set; }

        [Display(Name = "StorageCode")]
        [Required]
        public string StorageCode { get; set; }

        [Display(Name = "StorageName")]
        [Required]
        public string StorageName { get; set; }

        [Display(Name = "IsUsed")]
        [Required]
        public bool IsUsed { get; set; }

        [Display(Name = "Items")]
        [Required]
        public List<GarmentDeliveryReturnItemValueObject> Items { get; set; }
    }
}