using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Harbor.Common.Util
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 将Excel导入DataTable
        /// </summary>
        /// <param name="fileName">导入的文件路径（包括文件名）</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            var data = new DataTable();
            using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var fi = new FileInfo(fileName);
            var ext = fi.Extension;

            IWorkbook workbook = ext switch
            {
                ".xlsx" => new XSSFWorkbook(fs),
                ".xls" => new HSSFWorkbook(fs),
                _ => throw new NotImplementedException("不支持的格式"),
            };

            ISheet sheet;
            if (!string.IsNullOrEmpty(sheetName))
            {
                sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }

            if (sheet == null) throw new Exception("无法获取到表格数据");

            var firstrow = sheet.GetRow(0);
            int cellCount = firstrow.LastCellNum; //行最后一个cell的编号 即总的列数
            int startrow;
            if (isFirstRowColumn)
            {
                for (int i = firstrow.FirstCellNum; i < cellCount; i++)
                {
                    var cell = firstrow.GetCell(i);
                    var cellvalue = cell?.StringCellValue;
                    if (cellvalue == null) continue;
                    var column = new DataColumn(cellvalue);
                    data.Columns.Add(column);
                }

                startrow = sheet.FirstRowNum + 1;
            }
            else
            {
                startrow = sheet.FirstRowNum;
            }

            //读数据行
            var rowcount = sheet.LastRowNum;
            for (var i = startrow; i < rowcount; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null)
                {
                    continue; //没有数据的行默认是null
                }

                var datarow = data.NewRow(); //具有相同架构的行
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        datarow[j] = row.GetCell(j).ToString() ?? string.Empty;
                    }
                }

                data.Rows.Add(datarow);
            }
            return data;
        }
    }
}
