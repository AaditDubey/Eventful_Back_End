using AutoMapper.Internal;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Time.Commerce.Application.Helpers;

public static class ExcelHelper
{
    public static byte[] ExportToExcel<T>(this IEnumerable<T> data, string language,
        Dictionary<string, Dictionary<string, string>> headerLanguages, string sheetName = null)
    {
        var publishStatusTranslations = new Dictionary<string, Dictionary<bool, string>>
        {
            { "en", new Dictionary<bool, string> { { true, "Active" }, { false, "Inactive" } } },
            { "vi", new Dictionary<bool, string> { { true, "Đã kích hoạt" }, { false, "Chưa kích hoạt" } } }
        };
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet(sheetName ?? "Sheet1");

        var properties = TypeDescriptor.GetProperties(typeof(T));

        var columnIndex = 1;
        foreach (var header in headerLanguages[language]
                     .Where(header => properties.Find(header.Key, false) != null))
        {
            var cell = ws.Cell(1, columnIndex);
            cell.Value = header.Value;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            columnIndex++;
        }

        MemoryStream stream;
        var enumerable = data.ToList();
        if (!enumerable.Any())
        {
            stream = new MemoryStream();
            wb.SaveAs(stream);
            return stream.ToArray();
        }

        var rowIndex = 2;
        foreach (var item in enumerable)
        {
            columnIndex = 1;
            foreach (var prop in headerLanguages[language]
                         .Select(header => properties.Find(header.Key, false)).Where(prop => prop != null))
            {
                var cell = ws.Cell(rowIndex, columnIndex);
                var dataValue = prop.GetValue(item);
                if (prop.Name == "Published" && dataValue != null)
                {
                    dataValue = publishStatusTranslations[language].ContainsKey((bool)dataValue)
                        ? publishStatusTranslations[language][(bool)dataValue]
                        : dataValue;
                }

                HandleFormatData<T>(prop, cell, dataValue);

                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                columnIndex++;
            }

            rowIndex++;
        }

        stream = new MemoryStream();
        wb.SaveAs(stream);
        return stream.ToArray();
    }

    public static async Task<IEnumerable<T>> GetImportExcelDataAsync<T>(this IFormFile file, string language,
        Dictionary<string, Dictionary<string, string>> headerLanguages, string sheetName = null) where T : new()
    {
        if (!CheckExtensionFileExcel(file))
        {
            throw new ArgumentException("The provided file has an incorrect format xls,xlsx.");
        }

        var listT = new Collection<T>();
        if (file == null || file.Length == 0)
        {
            return listT;
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream).ConfigureAwait(false);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(sheetName ?? "Sheet1");
        var typeofT = typeof(T);
        foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip the header row
        {
            var item = new T();
            var cellIndex = 1;
            foreach (var propertyInfo in headerLanguages[language]
                         .Select(headerLanguage => typeofT.GetProperty(headerLanguage.Key))
                         .Where(propertyInfo => propertyInfo != null))
            {
                SetValue(propertyInfo, item, row.Cell(cellIndex).GetValue<string>(), language);
                cellIndex++;
            }

            listT.Add(item);
        }

        return listT;
    }

    public static bool CheckExtensionFileExcel(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        return extension.ToLower() == ".xlsx" || extension.ToLower() == ".xls";
    }

    private static void SetValue(PropertyInfo info, object instance, object value, string language)
    {
        var publishStatusTranslations = new Dictionary<string, Dictionary<bool, string>>
        {
            { "en", new Dictionary<bool, string> { { true, "Active" }, { false, "Inactive" } } },
            { "vi", new Dictionary<bool, string> { { true, "Đã kích hoạt" }, { false, "Chưa kích hoạt" } } }
        };
        object parseValue;
        var targetType = info.PropertyType.IsNullableType()
            ? Nullable.GetUnderlyingType(info.PropertyType)
            : info.PropertyType;
        if (info.PropertyType.IsNullableType() && value == null)
            parseValue = null;
        else if (targetType is { IsEnum: true })
            parseValue = Enum.Parse(targetType, value.ToString() ?? string.Empty);
        else if (targetType == typeof(Guid))
            parseValue = string.IsNullOrEmpty(value.ToString()) ? Guid.Empty : new Guid(value.ToString());
        else if (info.Name == "Published")
        {
            var key = publishStatusTranslations[language].FirstOrDefault(x => x.Value == value.ToString()).Key;
            parseValue = Convert.ChangeType(key, TypeCode.Boolean);
        }
        else
            parseValue = Convert.ChangeType(value.ToString(), targetType);

        info.SetValue(instance, parseValue, null);
    }

    private static void HandleFormatData<T>(PropertyDescriptor prop, IXLCell cell, object data)
    {
        if (data == null)
            return;
        if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(decimal) ||
            prop.PropertyType == typeof(double))
        {
            cell.Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number
                .Precision2WithSeparatorAndParens);
            cell.Value = Convert.ToDouble(data);
        }
        else if (prop.PropertyType == typeof(DateTime))
        {
            cell.Style.NumberFormat.Format = "MM/dd/yyyy HH:mm:ss";
            cell.Value = (DateTime)data;
        }
        else
        {
            cell.Value = data.ToString();
        }
    }
}