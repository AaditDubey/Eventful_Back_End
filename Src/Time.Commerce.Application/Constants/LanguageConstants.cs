using Time.Commerce.Contracts.Enums.Reports;

namespace Time.Commerce.Application.Constants.Reports;


public static class LanguageConstants
{
    public static readonly Dictionary<ReportType, Dictionary<string, Dictionary<string, string>>> HeaderLanguages
        = new()
        {
            [ReportType.Category] = new Dictionary<string, Dictionary<string, string>>
            {
                    {
                        "en", new Dictionary<string, string>
                        {
                            { "Id", "Id" },
                            { "Name", "Name" },
                            { "Published", "Status" },
                            { "MetaKeywords", "MetaKeywords" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "CreatedOn" },
                        }
                    },
                    {
                        "vi", new Dictionary<string, string>
                        {
                            { "Id", "Id" },
                            { "Name", "Tên" },
                            { "Published", "Trạng thái" },
                            { "MetaKeywords", "MetaKeywords" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "Ngày tạo" },
                        }
                    }
            },
            [ReportType.Brand] = new Dictionary<string, Dictionary<string, string>>
            {
                    {
                        "en", new Dictionary<string, string>
                        {
                            { "Name", "Name" },
                            { "Published", "Status" },
                            { "Description", "Description" },
                            { "PriceRanges", "PriceRanges" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "CreatedOn" },
                        }
                    },
                    {
                        "vi", new Dictionary<string, string>
                        {
                            { "Name", "Tên" },
                            { "Published", "Trạng thái" },
                            { "Description", "Mô tả" },
                            { "PriceRanges", "Phạm vi giá" },
                            { "MetaKeywords", "MetaKeywords" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "Ngày tạo" },
                        }
                    }
            },
            [ReportType.Product] = new Dictionary<string, Dictionary<string, string>>
            {
                    {
                        "en", new Dictionary<string, string>
                        {
                            { "Name", "Name" },
                            { "Sku", "Sku" },
                            { "ShortDescription", "ShortDescription" },
                            { "Published", "Status" },
                            { "Price", "Price" },
                            { "OldPrice", "OldPrice" },
                            { "StartPrice", "StartPrice" },
                            { "EndPrice", "EndPrice" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "CreatedOn" },
                        }
                    },
                    {
                        "vi", new Dictionary<string, string>
                        {
                            { "Name", "Tên" },
                            { "Sku", "Sku" },
                            { "ShortDescription", "Mô tả" },
                            { "Published", "Trạng thái" },
                            { "Price", "Giá" },
                            { "OldPrice", "Giá chưa giảm" },
                            { "StartPrice", "Giá khởi điểm" },
                            { "EndPrice", "Giá kết thúc" },
                            { "MetaKeywords", "MetaKeywords" },
                            { "MetaTitle", "MetaTitle" },
                            { "MetaDescription", "MetaDescription" },
                            { "CreatedOn", "Ngày tạo" },
                        }
                    }
            }
        };
}