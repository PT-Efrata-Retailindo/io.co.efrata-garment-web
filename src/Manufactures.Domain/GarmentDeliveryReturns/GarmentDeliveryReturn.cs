using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns
{
    public class GarmentDeliveryReturn : AggregateRoot<GarmentDeliveryReturn, GarmentDeliveryReturnReadModel>
    {
        public string DRNo { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public int UnitDOId { get; private set; }
        public string UnitDONo { get; private set; }
        public int UENId { get; private set; }
        public string PreparingId { get; private set; }
        public DateTimeOffset? ReturnDate { get; private set; }
        public string ReturnType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public StorageId StorageId { get; private set; }
        public string StorageCode { get; private set; }
        public string StorageName { get; private set; }
        public bool IsUsed { get; private set; }

        public GarmentDeliveryReturn(Guid identity, string drNo, string roNo, string article, int unitDOId, string unitDONo, int uenId, string preparingId, DateTimeOffset? returnDate, string returnType, UnitDepartmentId unitId, string unitCode, string unitName, StorageId storageId, string storageName, string storageCode, bool isUsed) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            DRNo = drNo;
            RONo = roNo;
            Article = article;
            UnitDOId = unitDOId;
            UnitDONo = unitDONo;
            UENId = uenId;
            PreparingId = preparingId;
            ReturnDate = returnDate;
            ReturnType = returnType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            StorageId = storageId;
            StorageCode = storageCode;
            StorageName = storageName;
            IsUsed = isUsed;

            ReadModel = new GarmentDeliveryReturnReadModel(identity)
            {
                DRNo = DRNo,
                RONo = RONo,
                Article = Article,
                UnitDOId = UnitDOId,
                UnitDONo = UnitDONo,
                UENId = UENId,
                PreparingId = PreparingId,
                ReturnDate = ReturnDate,
                ReturnType = ReturnType,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                StorageId = StorageId.Value,
                StorageCode = StorageCode,
                StorageName = StorageName,
                IsUsed = IsUsed,
            };
            ReadModel.AddDomainEvent(new OnGarmentDeliveryReturnPlaced(this.Identity));
        }

        public GarmentDeliveryReturn(GarmentDeliveryReturnReadModel readModel) : base(readModel)
        {
            DRNo = ReadModel.DRNo;
            RONo = ReadModel.RONo;
            Article = ReadModel.Article;
            UnitDOId = ReadModel.UnitDOId;
            UnitDONo = ReadModel.UnitDONo;
            UENId = ReadModel.UENId;
            PreparingId = ReadModel.PreparingId;
            ReturnDate = ReadModel.ReturnDate;
            ReturnType = ReadModel.ReturnType;
            UnitId = new UnitDepartmentId(ReadModel.UnitId);
            UnitCode = ReadModel.UnitCode;
            UnitName = ReadModel.UnitName;
            StorageId = new StorageId(ReadModel.StorageId);
            StorageCode = ReadModel.StorageCode;
            StorageName = ReadModel.StorageName;
            IsUsed = ReadModel.IsUsed;
        }

        public void setDRNo(string newDRNo)
        {
            Validator.ThrowIfNull(() => newDRNo);

            if (newDRNo != DRNo)
            {
                DRNo = newDRNo;
                ReadModel.DRNo = newDRNo;
            }
        }
        public void setRONo(string newRONo)
        {
            Validator.ThrowIfNull(() => newRONo);

            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }
        public void setArticle(string newArticle)
        {
            Validator.ThrowIfNull(() => newArticle);

            if (newArticle != Article)
            {
                Article = newArticle;
                ReadModel.Article = newArticle;
            }
        }
        public void setUnitDOId(int newUnitDOId)
        {
            if (newUnitDOId != UnitDOId)
            {
                UnitDOId = newUnitDOId;
                ReadModel.UnitDOId = newUnitDOId;
            }
        }
        public void setUnitDONo(string newUnitDONo)
        {
            Validator.ThrowIfNull(() => newUnitDONo);

            if (newUnitDONo != UnitDONo)
            {
                UnitDONo = newUnitDONo;
                ReadModel.UnitDONo = newUnitDONo;
            }
        }
        public void setUENId(int newUENId)
        {
            if (newUENId != UENId)
            {
                UENId = newUENId;
                ReadModel.UENId = newUENId;
            }
        }
        public void setPreparingId(string newPreparingId)
        {
            if (newPreparingId != PreparingId)
            {
                PreparingId = newPreparingId;
                ReadModel.PreparingId = newPreparingId;
            }
        }
        public void setReturnDate(DateTimeOffset? newReturnDate)
        {
            if (newReturnDate != ReturnDate)
            {
                ReturnDate = newReturnDate;
                ReadModel.ReturnDate = newReturnDate;
            }
        }
        public void setReturnType(string newReturnType)
        {
            Validator.ThrowIfNull(() => newReturnType);

            if (newReturnType != ReturnType)
            {
                ReturnType = newReturnType;
                ReadModel.ReturnType = newReturnType;
            }
        }
        public void setUnitId(UnitDepartmentId newUnitId)
        {
            Validator.ThrowIfNull(() => newUnitId);

            if (newUnitId != UnitId)
            {
                UnitId = newUnitId;
                ReadModel.UnitId = newUnitId.Value;
            }
        }

        public void setUnitCode(string newUnitCode)
        {
            Validator.ThrowIfNullOrEmpty(() => newUnitCode);

            if (newUnitCode != UnitCode)
            {
                UnitCode = newUnitCode;
                ReadModel.UnitCode = newUnitCode;
            }
        }

        public void setUnitName(string newUnitName)
        {
            Validator.ThrowIfNullOrEmpty(() => newUnitName);

            if (newUnitName != UnitName)
            {
                UnitName = newUnitName;
                ReadModel.UnitName = newUnitName;
            }
        }
        public void setStorageId(StorageId newStorageId)
        {
            Validator.ThrowIfNull(() => newStorageId);

            if (newStorageId != StorageId)
            {
                StorageId = newStorageId;
                ReadModel.StorageId = newStorageId.Value;
            }
        }

        public void setStorageCode(string newStorageCode)
        {
            Validator.ThrowIfNullOrEmpty(() => newStorageCode);

            if (newStorageCode != StorageCode)
            {
                StorageCode = newStorageCode;
                ReadModel.StorageCode = newStorageCode;
            }
        }

        public void setStorageName(string newStorageName)
        {
            Validator.ThrowIfNullOrEmpty(() => newStorageName);

            if (newStorageName != StorageName)
            {
                StorageName = newStorageName;
                ReadModel.StorageName = newStorageName;
            }
        }

        public void setIsUsed(bool newIsUsed)
        {
            if (newIsUsed != IsUsed)
            {
                IsUsed = newIsUsed;
                ReadModel.IsUsed = newIsUsed;
            }
        }
        public void SetModified()
        {
            MarkModified();
        }

        protected override GarmentDeliveryReturn GetEntity()
        {
            return this;
        }
    }
}