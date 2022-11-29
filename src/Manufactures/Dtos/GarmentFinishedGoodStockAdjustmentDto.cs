using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{ 
	class GarmentFinishedGoodStockAdjustmentDto : BaseDto
	{
		public GarmentFinishedGoodStockAdjustmentDto(GarmentFinishedGoodStock transaction)
		{
			Id  = transaction.Identity;
			FinishedGoodStockNo = transaction.FinishedGoodStockNo;
			RONo = transaction.RONo;
			Article = transaction.Article;
			UnitId = transaction.UnitId;
			UnitCode = transaction.UnitCode;
			UnitName = transaction.UnitName;
			ComodityId = transaction.ComodityId;
			ComodityCode = transaction.ComodityCode;
			ComodityName = transaction.ComodityName;
			SizeId = transaction.SizeId;
			SizeName = transaction.SizeName;
			Quantity = transaction.Quantity;
			UomId = transaction.UomId;
			UomUnit = transaction.UomUnit;
			BasicPrice = transaction.BasicPrice;
			Price = transaction.Price;
		}
		public Guid Id { get; internal set; }
		public string FinishedGoodStockNo { get; private set; }
		public UnitDepartmentId UnitId { get; private set; }
		public string UnitCode { get; private set; }
		public string UnitName { get; private set; }
		public string RONo { get; private set; }
		public string Article { get; private set; }
		public GarmentComodityId ComodityId { get; private set; }
		public string ComodityCode { get; private set; }
		public string ComodityName { get; private set; }
		public SizeId SizeId { get; private set; }
		public string SizeName { get; private set; }
		public double Quantity { get; private set; }
		public UomId UomId { get; private set; }
		public string UomUnit { get; private set; }
		public double BasicPrice { get; private set; }
		public double Price { get; private set; }
		
	}
 
}
