using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Monopy.PreceRateWage.Common
{
    public class ExcelHelper
    {
        public static bool IsMergeCell(ISheet sheet, int rowNum, int colNum, out int rowSpan, out int colSpan)
        {
            bool result = false;
            rowSpan = 0;
            colSpan = 0;
            if ((rowNum < 0) || (colNum < 0)) return result;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowNum && range.FirstColumn == colNum)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            try
            {
                result = sheet.GetRow(rowNum).GetCell(colNum).IsMergedCell;
            }
            catch
            {
            }
            return result;
        }

        public static string MyGetCellValue(ICell cell)
        {
            string myResult = string.Empty;
            if (cell == null)
            {
                return myResult;
            }
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        myResult = cell.DateCellValue.ToString();
                    }
                    else
                    {
                        myResult = cell.NumericCellValue == 0 ? string.Empty : cell.NumericCellValue.ToString();
                    }
                    break;

                case CellType.String:
                    myResult = cell.StringCellValue.Replace("\r\n", string.Empty).Replace(Environment.NewLine, string.Empty).Replace("\n", string.Empty);
                    break;

                case CellType.Formula:
                    try
                    {
                        myResult = cell.NumericCellValue == 0 ? string.Empty : cell.NumericCellValue.ToString();
                    }
                    catch
                    {
                        myResult = cell.RichStringCellValue.String;
                    }
                    break;

                case CellType.Boolean:
                    myResult = cell.BooleanCellValue.ToString();
                    break;

                case CellType.Error:
                    myResult = cell.ErrorCellValue.ToString();
                    break;

                default:
                    break;
            }
            return myResult.Trim();
        }

        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }
            if (!cell.IsMergedCell)
            {
                return MyGetCellValue(cell);
            }
            else
            {
                ICell firstRowMergedCell;
                for (int i = 0; i <= cell.RowIndex; i++)
                {
                    for (int j = 0; j <= cell.ColumnIndex; j++)
                    {
                        if (cell.RowIndex - i >= 0 || cell.ColumnIndex - j >= 0)
                        {
                            firstRowMergedCell = cell.Sheet.GetRow(cell.RowIndex - i).GetCell(cell.ColumnIndex - j);
                            if (IsMergeCell(firstRowMergedCell.Sheet, cell.RowIndex - i, cell.ColumnIndex - j, out int rowSpan, out int colSpan))
                            {
                                if ((rowSpan > 1 || colSpan > 1) && (firstRowMergedCell.RowIndex + rowSpan >= cell.RowIndex || firstRowMergedCell.ColumnIndex + colSpan >= cell.ColumnIndex))
                                {
                                    return MyGetCellValue(firstRowMergedCell);
                                }
                            }
                        }
                    }
                }
                return null;
            }
        }

        private readonly int EXCEL03_MaxRow = 65535;

        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public byte[] DataTable2Excel(DataTable dt, string sheetName, string title = "")
        {
            IWorkbook book = new HSSFWorkbook();
            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, sheetName, title);
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, sheetName + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, lastPageItemCount, book, sheetName + page.ToString());
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return ms.ToArray();
        }

        private void DataWrite2Sheet(DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName, string title = "")
        {
            ISheet sheet = book.CreateSheet(sheetName);
            IRow header;
            int rowIndex;
            if (string.IsNullOrEmpty(title))
            {
                header = sheet.CreateRow(0);
                rowIndex = 1;
            }
            else
            {
                IRow rowTitle = sheet.CreateRow(0);
                ICell cellTitle = rowTitle.CreateCell(0);
                cellTitle.SetCellValue(title);
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                ICellStyle styleTitle = book.CreateCellStyle();
                //设置单元格的样式：水平对齐居中
                styleTitle.Alignment = HorizontalAlignment.Center;
                //新建一个字体样式对象
                IFont font = book.CreateFont();
                font.FontHeight = 400;
                //设置字体加粗样式
                font.Boldweight = short.MaxValue;
                //使用SetFont方法将字体样式添加到单元格样式中
                styleTitle.SetFont(font);
                //将新的样式赋给单元格
                cellTitle.CellStyle = styleTitle;

                header = sheet.CreateRow(1);
                rowIndex = 2;
            }
            ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                cell.SetCellValue(val);
                cell.CellStyle = style;
            }
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dtRow = dt.Rows[i];
                IRow excelRow = sheet.CreateRow(rowIndex++);
                for (int j = 0; j < dtRow.ItemArray.Length; j++)
                {
                    ICell cell = excelRow.CreateCell(j);
                    cell.SetCellValue(dtRow[j].ToString());
                    cell.CellStyle = style;
                }
            }
        }

        public static void SetCellValue(string sourceFileName, string destFileName, int rowNum, int colNum, string value, int sheetIndex = 0)
        {
            using (FileStream file = File.OpenRead(sourceFileName))
            {
                IWorkbook workbook = WorkbookFactory.Create(file);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                sheet.GetRow(rowNum).GetCell(colNum).SetCellValue(value);
                using (FileStream sw = File.Create(destFileName))
                {
                    workbook.Write(sw);
                }
            }
        }
    }

    public class ExcelHelper<T> where T : class, new()
    {
        public List<T> ReadExcel(string excelFullName, int ExcelBeginRow, int propertyBeginIndex, int cellFirstRow = 0, int sheetIndex = 0, int propertyEndIndex = 0, bool isNeedNo = false, string NoName = "No")
        {
            int i;
            int j;
            try
            {
                using (FileStream fs = new FileStream(excelFullName, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;
                    try
                    {
                        workbook = WorkbookFactory.Create(fs);
                    }
                    catch
                    {
                        return null;
                    }
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    List<T> list = new List<T>();
                    for (i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        if (i < ExcelBeginRow)
                        {
                            continue;
                        }

                        IRow row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        T t = new T();
                        Type type = t.GetType();
                        PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        propertyInfos[0].SetValue(t, Guid.NewGuid(), null);
                        for (j = cellFirstRow; j < (propertyInfos.Length - propertyBeginIndex - propertyEndIndex); j++)
                        {
                            var item = row.GetCell(j);
                            if (item != null)
                            {
                                propertyInfos[propertyBeginIndex + j - cellFirstRow].SetValue(t, ExcelHelper.GetCellValue(item), null);
                            }
                        }
                        if (isNeedNo)
                        {
                            for (int k = 0; k < propertyInfos.Length; k++)
                            {
                                if (propertyInfos[k].Name == NoName)
                                {
                                    propertyInfos[k].SetValue(t, (i - ExcelBeginRow + 1).ToString(), null);
                                }
                            }
                        }
                        list.Add(t);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool WriteExcle(string sourceFileName, string destFileName, List<T> list, int ExcelBeginRow, List<int> propertysNos, int sheetIndex = 0, int titleRow = 0, int titleCell = 0, string title = "")
        {
            if (list.Count == 0)
            {
                return false;
            }
            try
            {
                FileStream file = File.OpenRead(sourceFileName);
                IWorkbook workbook = WorkbookFactory.Create(file);
                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                if (!string.IsNullOrEmpty(title))
                {
                    var tCell = sheet.GetRow(titleRow).GetCell(titleCell);
                    tCell.CellStyle = style;
                    string sCell = ExcelHelper.GetCellValue(tCell).ToUpper();
                    sCell = sCell.Replace("XXXX", title.Split('-')[0]);
                    sCell = sCell.Replace("XX", title.Split('-')[1]);
                    tCell.SetCellValue(sCell);
                }
                Type type = list[0].GetType();
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < list.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + ExcelBeginRow);
                    for (int j = 0; j < propertysNos.Count; j++)
                    {
                        row.CreateCell(j);
                        object obj = propertyInfos[propertysNos[j]].GetValue(list[i], null);
                        row.Cells[j].SetCellValue(obj == null ? string.Empty : obj.ToString());
                        row.Cells[j].CellStyle = style;
                    }
                }
                FileStream sw = File.Create(destFileName);
                workbook.Write(sw);
                sw.Close();
                sw.Dispose();
                file.Close();
                file.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="list"></param>
        /// <param name="ExcelBeginRow"></param>
        /// <param name="propertyBeginIndex"></param>
        /// <param name="propertyEndIndex"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="titleRow"></param>
        /// <param name="titleCell"></param>
        /// <param name="title">XXXX-XX-X-YY 如：2017-09-21-模具</param>
        /// <returns></returns>
        public bool WriteExcle(string sourceFileName, string destFileName, List<T> list, int ExcelBeginRow, int propertyBeginIndex, int propertyEndIndex = 0, int sheetIndex = 0, int titleRow = 0, int titleCell = 0, string title = "", int cellFirstColumn = 0)
        {
            if (list.Count == 0)
            {
                return false;
            }
            try
            {
                FileStream file = File.OpenRead(sourceFileName);
                IWorkbook workbook = WorkbookFactory.Create(file);
                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                ISheet sheet = workbook.GetSheetAt(sheetIndex);

                if (!string.IsNullOrEmpty(title))
                {
                    var tCell = sheet.GetRow(titleRow).GetCell(titleCell);
                    tCell.CellStyle = style;
                    string sCell = ExcelHelper.GetCellValue(tCell).ToUpper();
                    string[] titles = title.Split('-');
                    sCell = sCell.Replace("XXXX", title.Split('-')[0]);
                    sCell = sCell.Replace("XX", title.Split('-')[1]);
                    if (titles.Length >= 3)
                    {
                        sCell = sCell.Replace("X", title.Split('-')[2]);
                    }
                    if (titles.Length >= 4)
                    {
                        sCell = sCell.Replace("YY", title.Split('-')[3]);
                    }
                    tCell.SetCellValue(sCell);
                }

                Type type = list[0].GetType();
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < list.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + ExcelBeginRow);
                    for (int j = cellFirstColumn; j < propertyInfos.Length - propertyBeginIndex - propertyEndIndex + cellFirstColumn; j++)
                    {
                        row.CreateCell(j - cellFirstColumn);
                        object obj = propertyInfos[j - cellFirstColumn + propertyBeginIndex].GetValue(list[i], null);
                        row.Cells[j - cellFirstColumn].SetCellValue(obj == null ? string.Empty : obj.ToString());
                        row.Cells[j - cellFirstColumn].CellStyle = style;
                        //if (obj != null && double.TryParse(obj.ToString(), out double tmpDouble))
                        //{
                        //    row.Cells[j].SetCellType(CellType.Numeric);
                        //}
                    }
                }
                FileStream sw = File.Create(destFileName);
                workbook.Write(sw);
                sw.Close();
                sw.Dispose();
                file.Close();
                file.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}