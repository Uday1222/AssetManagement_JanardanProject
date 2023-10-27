using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AssetManagement.ExcelUtility
{
    public class ExcelData<T> : ResourceCleaner
    {
        private List<PropertyInfo> propertyValues = null;
        public OpenXMLData ReportDataToExcelData(List<T> reportData)
        {
            OpenXMLData data = new OpenXMLData();

            if (reportData != null)
            {
                this.propertyValues = typeof(T).GetProperties().ToList();

                propertyValues.ToList().ForEach(x => data.Headers.Add(x.GetCustomAttributes(typeof(DisplayNameAttribute)).Cast<DisplayNameAttribute>().Single().DisplayName));

                foreach (T obj in reportData)
                {
                    List<string> strData = new List<string>();
                    propertyValues.ForEach(x =>
                    {
                        object o = obj.GetType().GetProperty(x.Name).GetValue(obj);
                        string val = o == null ? string.Empty : o.ToString();
                        strData.Add(val.CleanInvalidXmlChars());
                    });
                    data.DataRows.Add(strData);
                }
            }

            return data;
        }


    }




    public class OpenXMLData
    {
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public OpenXMLData()
        {
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }

    }

    public static class Utilities
    {
        public static string CleanInvalidXmlChars(this string text)
        {
            //string re = @"[^\x09\x0A\x0D\x96\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            //return Regex.Replace(text, re, "");

            // string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            string r = @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]";
            return Regex.Replace(text, r, "", RegexOptions.Compiled);
        }
    }
}
