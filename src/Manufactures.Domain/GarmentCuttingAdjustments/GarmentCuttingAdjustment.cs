using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentCuttingAdjustments
{
    public class GarmentCuttingAdjustment : AggregateRoot<GarmentCuttingAdjustment, GarmentCuttingAdjustmentReadModel>
    {
        public string AdjustmentNo { get; private set; }
        public string CutInNo { get; private set; }
        public Guid CutInId { get; private set; }
        public string RONo { get; private set; }
        public decimal TotalFC { get; private set; }
        public decimal TotalActualFC { get; private set; }
        public decimal TotalQuantity { get; private set; }
        public decimal TotalActualQuantity { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public DateTimeOffset AdjustmentDate { get; private set; }
        public string AdjustmentDesc { get; private set; }

        public GarmentCuttingAdjustment(Guid identity, string adjustmentNo, string cutInNo, Guid cutInId, string rONo, decimal totalFC, decimal totalActualFC, decimal totalQuantity, decimal totalActualQuantity, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset adjustmentDate, string adjustmentDesc) : base(identity)
        {
            Identity = identity;
            AdjustmentNo = adjustmentNo;
            CutInNo = cutInNo;
            CutInId = cutInId;
            RONo = rONo;
            TotalFC = totalFC;
            TotalActualFC = totalActualFC;
            TotalQuantity = totalQuantity;
            TotalActualQuantity = totalActualQuantity;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            AdjustmentDate = adjustmentDate;
            AdjustmentDesc = adjustmentDesc;

            ReadModel = new GarmentCuttingAdjustmentReadModel(Identity)
            {
                AdjustmentNo=AdjustmentNo,
                CutInNo = CutInNo,
                CutInId=CutInId,
                RONo = RONo,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                AdjustmentDate = AdjustmentDate,
                TotalActualFC = TotalActualFC,
                TotalActualQuantity=TotalActualQuantity,
                TotalFC=TotalFC,
                TotalQuantity=TotalQuantity,
                AdjustmentDesc=AdjustmentDesc
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingAdjustmentPlaced(Identity));
        }
        public GarmentCuttingAdjustment(GarmentCuttingAdjustmentReadModel readModel) : base(readModel)
        {
            CutInNo = readModel.CutInNo;
            CutInId = readModel.CutInId;
            AdjustmentNo = readModel.AdjustmentNo;
            RONo = readModel.RONo;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            AdjustmentDate = readModel.AdjustmentDate;
            TotalFC = readModel.TotalFC;
            TotalQuantity = readModel.TotalQuantity;
            TotalActualFC = readModel.TotalActualFC;
            TotalActualQuantity = readModel.TotalActualQuantity;
            AdjustmentDesc = readModel.AdjustmentDesc;
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingAdjustment GetEntity()
        {
            return this;
        }

    }
}
