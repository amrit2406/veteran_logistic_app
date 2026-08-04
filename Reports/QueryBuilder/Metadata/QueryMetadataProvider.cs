namespace veteran_logistic.Reports.QueryBuilder.Metadata;

/// <summary>
/// Provides metadata for modules and fields available in the query builder.
/// </summary>
public static class QueryMetadataProvider
{
    /// <summary>
    /// Gets all available modules with their field metadata.
    /// </summary>
    public static List<ModuleMetadata> GetAllModules()
    {
        return new List<ModuleMetadata>
        {
            GetLoadingModule(),
            GetUnloadingModule(),
            GetPaymentModule(),
            GetPartyBillingModule()
        };
    }

    /// <summary>
    /// Gets metadata for the Loading module.
    /// </summary>
    public static ModuleMetadata GetLoadingModule()
    {
        return new ModuleMetadata
        {
            ModuleId = "Loading",
            DisplayName = "Loading Register",
            Fields = new List<FieldMetadata>
            {
                new() { FieldId = "Id", DisplayName = "ID", DataType = FieldDataType.Number, PropertyPath = "Id", CanGroup = false, CanAggregate = false },
                new() { FieldId = "ChallanNumber", DisplayName = "Challan Number", DataType = FieldDataType.Text, PropertyPath = "ChallanNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "LoadingDate", DisplayName = "Loading Date", DataType = FieldDataType.Date, PropertyPath = "LoadingDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "TPNumber", DisplayName = "TP Number", DataType = FieldDataType.Text, PropertyPath = "TPNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ConsignorName", DisplayName = "Consignor", DataType = FieldDataType.Text, PropertyPath = "Consignor.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ConsigneeName", DisplayName = "Consignee", DataType = FieldDataType.Text, PropertyPath = "Consignee.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "SourceName", DisplayName = "Source", DataType = FieldDataType.Text, PropertyPath = "Source.LocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "DestinationName", DisplayName = "Destination", DataType = FieldDataType.Text, PropertyPath = "Destination.LocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleNumber", DisplayName = "Vehicle Number", DataType = FieldDataType.Text, PropertyPath = "Vehicle.VehicleNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleType", DisplayName = "Vehicle Type", DataType = FieldDataType.Text, PropertyPath = "VehicleType", CanGroup = true, CanAggregate = false },
                new() { FieldId = "Driver", DisplayName = "Driver", DataType = FieldDataType.Text, PropertyPath = "Driver", CanGroup = true, CanAggregate = false },
                new() { FieldId = "MaterialName", DisplayName = "Material", DataType = FieldDataType.Text, PropertyPath = "Material.MaterialName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "GrossWeight", DisplayName = "Gross Weight", DataType = FieldDataType.Number, PropertyPath = "GrossWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TareWeight", DisplayName = "Tare Weight", DataType = FieldDataType.Number, PropertyPath = "TareWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "LoadingWeight", DisplayName = "Loading Weight", DataType = FieldDataType.Number, PropertyPath = "LoadingWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "Rate", DisplayName = "Rate", DataType = FieldDataType.Number, PropertyPath = "Rate", CanGroup = false, CanAggregate = true },
                new() { FieldId = "GrossAmount", DisplayName = "Gross Amount", DataType = FieldDataType.Number, PropertyPath = "GrossAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelQuantity", DisplayName = "Fuel Quantity", DataType = FieldDataType.Number, PropertyPath = "FuelQuantity", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelAmount", DisplayName = "Fuel Amount", DataType = FieldDataType.Number, PropertyPath = "FuelAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelCash", DisplayName = "Fuel Cash", DataType = FieldDataType.Number, PropertyPath = "FuelCash", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelAdvance", DisplayName = "Fuel Advance", DataType = FieldDataType.Number, PropertyPath = "FuelAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ShortageWeight", DisplayName = "Shortage Weight", DataType = FieldDataType.Number, PropertyPath = "ShortageWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "CashAdvance", DisplayName = "Cash Advance", DataType = FieldDataType.Number, PropertyPath = "CashAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "OtherAdvance", DisplayName = "Other Advance", DataType = FieldDataType.Number, PropertyPath = "OtherAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "DriverCommission", DisplayName = "Driver Commission", DataType = FieldDataType.Number, PropertyPath = "DriverCommission", CanGroup = false, CanAggregate = true },
                new() { FieldId = "PaymentLocationName", DisplayName = "Payment Location", DataType = FieldDataType.Text, PropertyPath = "PaymentLocation.PaymentLocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "UnionVendorName", DisplayName = "Union/Vendor", DataType = FieldDataType.Text, PropertyPath = "UnionVendor.Name", CanGroup = true, CanAggregate = false },
                new() { FieldId = "OwnerName", DisplayName = "Owner", DataType = FieldDataType.Text, PropertyPath = "Owner.CompanyName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "IsActive", DisplayName = "Active", DataType = FieldDataType.Boolean, PropertyPath = "IsActive", CanGroup = true, CanAggregate = false }
            }
        };
    }

    /// <summary>
    /// Gets metadata for the Unloading module.
    /// </summary>
    public static ModuleMetadata GetUnloadingModule()
    {
        return new ModuleMetadata
        {
            ModuleId = "Unloading",
            DisplayName = "Unloading Register",
            Fields = new List<FieldMetadata>
            {
                new() { FieldId = "Id", DisplayName = "ID", DataType = FieldDataType.Number, PropertyPath = "Id", CanGroup = false, CanAggregate = false },
                new() { FieldId = "ChallanNumber", DisplayName = "Challan Number", DataType = FieldDataType.Text, PropertyPath = "ChallanNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "UnloadingDate", DisplayName = "Unloading Date", DataType = FieldDataType.Date, PropertyPath = "UnloadingDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "TPNumber", DisplayName = "TP Number", DataType = FieldDataType.Text, PropertyPath = "TPNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ConsignorName", DisplayName = "Consignor", DataType = FieldDataType.Text, PropertyPath = "Consignor.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ConsigneeName", DisplayName = "Consignee", DataType = FieldDataType.Text, PropertyPath = "Consignee.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "SourceName", DisplayName = "Source", DataType = FieldDataType.Text, PropertyPath = "Source.LocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "DestinationName", DisplayName = "Destination", DataType = FieldDataType.Text, PropertyPath = "Destination.LocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleNumber", DisplayName = "Vehicle Number", DataType = FieldDataType.Text, PropertyPath = "Vehicle.VehicleNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleType", DisplayName = "Vehicle Type", DataType = FieldDataType.Text, PropertyPath = "VehicleType", CanGroup = true, CanAggregate = false },
                new() { FieldId = "Driver", DisplayName = "Driver", DataType = FieldDataType.Text, PropertyPath = "Driver", CanGroup = true, CanAggregate = false },
                new() { FieldId = "MaterialName", DisplayName = "Material", DataType = FieldDataType.Text, PropertyPath = "Material.MaterialName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "GrossWeight", DisplayName = "Gross Weight", DataType = FieldDataType.Number, PropertyPath = "GrossWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TareWeight", DisplayName = "Tare Weight", DataType = FieldDataType.Number, PropertyPath = "TareWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "LoadingWeight", DisplayName = "Loading Weight", DataType = FieldDataType.Number, PropertyPath = "LoadingWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "GrossWeightUL", DisplayName = "Gross Weight UL", DataType = FieldDataType.Number, PropertyPath = "GrossWeightUL", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TareWeightUL", DisplayName = "Tare Weight UL", DataType = FieldDataType.Number, PropertyPath = "TareWeightUL", CanGroup = false, CanAggregate = true },
                new() { FieldId = "UnloadingWeight", DisplayName = "Unloading Weight", DataType = FieldDataType.Number, PropertyPath = "UnloadingWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ShortageWeight", DisplayName = "Shortage Weight", DataType = FieldDataType.Number, PropertyPath = "ShortageWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ChallanMoney", DisplayName = "Challan Money", DataType = FieldDataType.Number, PropertyPath = "ChallanMoney", CanGroup = false, CanAggregate = true },
                new() { FieldId = "Rate", DisplayName = "Rate", DataType = FieldDataType.Number, PropertyPath = "Rate", CanGroup = false, CanAggregate = true },
                new() { FieldId = "GrossAmount", DisplayName = "Gross Amount", DataType = FieldDataType.Number, PropertyPath = "GrossAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelQuantity", DisplayName = "Fuel Quantity", DataType = FieldDataType.Number, PropertyPath = "FuelQuantity", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelAmount", DisplayName = "Fuel Amount", DataType = FieldDataType.Number, PropertyPath = "FuelAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelCash", DisplayName = "Fuel Cash", DataType = FieldDataType.Number, PropertyPath = "FuelCash", CanGroup = false, CanAggregate = true },
                new() { FieldId = "FuelAdvance", DisplayName = "Fuel Advance", DataType = FieldDataType.Number, PropertyPath = "FuelAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "CashAdvance", DisplayName = "Cash Advance", DataType = FieldDataType.Number, PropertyPath = "CashAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "OtherAdvance", DisplayName = "Other Advance", DataType = FieldDataType.Number, PropertyPath = "OtherAdvance", CanGroup = false, CanAggregate = true },
                new() { FieldId = "DriverCommission", DisplayName = "Driver Commission", DataType = FieldDataType.Number, PropertyPath = "DriverCommission", CanGroup = false, CanAggregate = true },
                new() { FieldId = "PaymentLocationName", DisplayName = "Payment Location", DataType = FieldDataType.Text, PropertyPath = "PaymentLocation.PaymentLocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "UnionVendorName", DisplayName = "Union/Vendor", DataType = FieldDataType.Text, PropertyPath = "UnionVendor.Name", CanGroup = true, CanAggregate = false },
                new() { FieldId = "OwnerName", DisplayName = "Owner", DataType = FieldDataType.Text, PropertyPath = "Owner.CompanyName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "IsActive", DisplayName = "Active", DataType = FieldDataType.Boolean, PropertyPath = "IsActive", CanGroup = true, CanAggregate = false }
            }
        };
    }

    /// <summary>
    /// Gets metadata for the Payment module.
    /// </summary>
    public static ModuleMetadata GetPaymentModule()
    {
        return new ModuleMetadata
        {
            ModuleId = "Payment",
            DisplayName = "Payment Register",
            Fields = new List<FieldMetadata>
            {
                new() { FieldId = "Id", DisplayName = "ID", DataType = FieldDataType.Number, PropertyPath = "Id", CanGroup = false, CanAggregate = false },
                new() { FieldId = "ChallanNumber", DisplayName = "Challan Number", DataType = FieldDataType.Text, PropertyPath = "ChallanNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "TPNumber", DisplayName = "TP Number", DataType = FieldDataType.Text, PropertyPath = "TPNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleNumber", DisplayName = "Vehicle Number", DataType = FieldDataType.Text, PropertyPath = "VehicleNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "VehicleType", DisplayName = "Vehicle Type", DataType = FieldDataType.Text, PropertyPath = "VehicleType", CanGroup = true, CanAggregate = false },
                new() { FieldId = "MaterialName", DisplayName = "Material", DataType = FieldDataType.Text, PropertyPath = "MaterialName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "LoadingDate", DisplayName = "Loading Date", DataType = FieldDataType.Date, PropertyPath = "LoadingDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "UnloadingDate", DisplayName = "Unloading Date", DataType = FieldDataType.Date, PropertyPath = "UnloadingDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "LoadingWeight", DisplayName = "Loading Weight", DataType = FieldDataType.Number, PropertyPath = "LoadingWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "UnloadingWeight", DisplayName = "Unloading Weight", DataType = FieldDataType.Number, PropertyPath = "UnloadingWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "DriverCommission", DisplayName = "Driver Commission", DataType = FieldDataType.Number, PropertyPath = "DriverCommission", CanGroup = false, CanAggregate = true },
                new() { FieldId = "PaymentDate", DisplayName = "Payment Date", DataType = FieldDataType.Date, PropertyPath = "PaymentDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "PaymentType", DisplayName = "Payment Type", DataType = FieldDataType.Text, PropertyPath = "PaymentType", CanGroup = true, CanAggregate = false },
                new() { FieldId = "PaymentLocationName", DisplayName = "Payment Location", DataType = FieldDataType.Text, PropertyPath = "PaymentLocation.PaymentLocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "GrossAmount", DisplayName = "Gross Amount", DataType = FieldDataType.Number, PropertyPath = "GrossAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TDSPercentage", DisplayName = "TDS Percentage", DataType = FieldDataType.Number, PropertyPath = "TDSPercentage", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ChallanMoney", DisplayName = "Challan Money", DataType = FieldDataType.Number, PropertyPath = "ChallanMoney", CanGroup = false, CanAggregate = true },
                new() { FieldId = "Surcharge", DisplayName = "Surcharge", DataType = FieldDataType.Number, PropertyPath = "Surcharge", CanGroup = false, CanAggregate = true },
                new() { FieldId = "AdminCharge", DisplayName = "Admin Charge", DataType = FieldDataType.Number, PropertyPath = "AdminCharge", CanGroup = false, CanAggregate = true },
                new() { FieldId = "PayableAmount", DisplayName = "Payable Amount", DataType = FieldDataType.Number, PropertyPath = "PayableAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "PaymentStatus", DisplayName = "Payment Status", DataType = FieldDataType.Text, PropertyPath = "PaymentStatus", CanGroup = true, CanAggregate = false },
                new() { FieldId = "Beneficiary", DisplayName = "Beneficiary", DataType = FieldDataType.Text, PropertyPath = "Beneficiary", CanGroup = true, CanAggregate = false },
                new() { FieldId = "PAN", DisplayName = "PAN", DataType = FieldDataType.Text, PropertyPath = "PAN", CanGroup = true, CanAggregate = false },
                new() { FieldId = "UTRNumber", DisplayName = "UTR Number", DataType = FieldDataType.Text, PropertyPath = "UTRNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "MobileNumber", DisplayName = "Mobile Number", DataType = FieldDataType.Text, PropertyPath = "MobileNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "BankName", DisplayName = "Bank Name", DataType = FieldDataType.Text, PropertyPath = "BankName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "AccountNumber", DisplayName = "Account Number", DataType = FieldDataType.Text, PropertyPath = "AccountNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "IFSCCode", DisplayName = "IFSC Code", DataType = FieldDataType.Text, PropertyPath = "IFSCCode", CanGroup = true, CanAggregate = false },
                new() { FieldId = "IsActive", DisplayName = "Active", DataType = FieldDataType.Boolean, PropertyPath = "IsActive", CanGroup = true, CanAggregate = false }
            }
        };
    }

    /// <summary>
    /// Gets metadata for the Party Billing module.
    /// </summary>
    public static ModuleMetadata GetPartyBillingModule()
    {
        return new ModuleMetadata
        {
            ModuleId = "PartyBilling",
            DisplayName = "Party Bill Register",
            Fields = new List<FieldMetadata>
            {
                new() { FieldId = "Id", DisplayName = "ID", DataType = FieldDataType.Number, PropertyPath = "Id", CanGroup = false, CanAggregate = false },
                new() { FieldId = "BillNumber", DisplayName = "Bill Number", DataType = FieldDataType.Text, PropertyPath = "BillNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "BillDate", DisplayName = "Bill Date", DataType = FieldDataType.Date, PropertyPath = "BillDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "PartyName", DisplayName = "Party", DataType = FieldDataType.Text, PropertyPath = "Party.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ThirdPartyName", DisplayName = "Third Party", DataType = FieldDataType.Text, PropertyPath = "ThirdPartyName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "PermitNumber", DisplayName = "Permit Number", DataType = FieldDataType.Text, PropertyPath = "PermitNumber", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ConsignorName", DisplayName = "Consignor", DataType = FieldDataType.Text, PropertyPath = "Consignor.CustomerName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "DestinationName", DisplayName = "Destination", DataType = FieldDataType.Text, PropertyPath = "Destination.LocationName", CanGroup = true, CanAggregate = false },
                new() { FieldId = "FromDate", DisplayName = "From Date", DataType = FieldDataType.Date, PropertyPath = "FromDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ToDate", DisplayName = "To Date", DataType = FieldDataType.Date, PropertyPath = "ToDate", CanGroup = true, CanAggregate = false },
                new() { FieldId = "TotalRecords", DisplayName = "Total Records", DataType = FieldDataType.Number, PropertyPath = "TotalRecords", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TotalMaterialWeight", DisplayName = "Total Material Weight", DataType = FieldDataType.Number, PropertyPath = "TotalMaterialWeight", CanGroup = false, CanAggregate = true },
                new() { FieldId = "TotalAmount", DisplayName = "Total Amount", DataType = FieldDataType.Number, PropertyPath = "TotalAmount", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ChargeHead1", DisplayName = "Charge Head 1", DataType = FieldDataType.Text, PropertyPath = "ChargeHead1", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ChargeType1", DisplayName = "Charge Type 1", DataType = FieldDataType.Text, PropertyPath = "ChargeType1", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ChargeAmount1", DisplayName = "Charge Amount 1", DataType = FieldDataType.Number, PropertyPath = "ChargeAmount1", CanGroup = false, CanAggregate = true },
                new() { FieldId = "ChargeHead2", DisplayName = "Charge Head 2", DataType = FieldDataType.Text, PropertyPath = "ChargeHead2", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ChargeType2", DisplayName = "Charge Type 2", DataType = FieldDataType.Text, PropertyPath = "ChargeType2", CanGroup = true, CanAggregate = false },
                new() { FieldId = "ChargeAmount2", DisplayName = "Charge Amount 2", DataType = FieldDataType.Number, PropertyPath = "ChargeAmount2", CanGroup = false, CanAggregate = true },
                new() { FieldId = "GrandTotal", DisplayName = "Grand Total", DataType = FieldDataType.Number, PropertyPath = "GrandTotal", CanGroup = false, CanAggregate = true },
                new() { FieldId = "Remarks", DisplayName = "Remarks", DataType = FieldDataType.Text, PropertyPath = "Remarks", CanGroup = true, CanAggregate = false },
                new() { FieldId = "IsActive", DisplayName = "Active", DataType = FieldDataType.Boolean, PropertyPath = "IsActive", CanGroup = true, CanAggregate = false }
            }
        };
    }

    /// <summary>
    /// Gets a module by its ID.
    /// </summary>
    public static ModuleMetadata? GetModuleById(string moduleId)
    {
        return moduleId switch
        {
            "Loading" => GetLoadingModule(),
            "Unloading" => GetUnloadingModule(),
            "Payment" => GetPaymentModule(),
            "PartyBilling" => GetPartyBillingModule(),
            _ => null
        };
    }
}
