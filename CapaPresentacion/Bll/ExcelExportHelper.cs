using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Bll
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  

                if (dataTable.Rows.Count > 0)
                {

                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {



                        //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        //if (maxLength < 150)
                        //{
                        //    workSheet.Column(columnIndex).AutoFit();
                        //}


                        columnIndex++;
                    }
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders 
                if (dataTable.Rows.Count > 0)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }
                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }

        #region Excel 2 headers

        public static byte[] ExportExcel2(DataTable dataTable, string header = "", string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 4;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                if (header == "ConsultaMovimiento")
                {
                    workSheet.Cells["B" + 3].Value = "FECHA";
                    workSheet.Cells["B" + 3].Style.Font.Size = 12;

                    workSheet.Cells["C" + 3].Value = "INICIAL";
                    workSheet.Cells["C" + 3].Style.Font.Size = 12;
                    workSheet.Cells["C" + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["C" + 3].Style.Font.Bold = true;
                    workSheet.SelectedRange[3, 3, 3, 4].Merge = true;

                    workSheet.Cells["E" + 3].Value = "VENTA";
                    workSheet.Cells["E" + 3].Style.Font.Size = 12;
                    workSheet.Cells["E" + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["E" + 3].Style.Font.Bold = true;
                    workSheet.SelectedRange[3, 5, 3, 6].Merge = true;

                    workSheet.Cells["G" + 3].Value = "INGRESO";
                    workSheet.Cells["G" + 3].Style.Font.Size = 12;
                    workSheet.Cells["G" + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["G" + 3].Style.Font.Bold = true;
                    workSheet.SelectedRange[3, 7, 3, 8].Merge = true;

                    workSheet.Cells["I" + 3].Value = "SALIDA";
                    workSheet.Cells["I" + 3].Style.Font.Size = 12;
                    workSheet.Cells["I" + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["I" + 3].Style.Font.Bold = true;
                    workSheet.SelectedRange[3, 9, 3, 10].Merge = true;

                    workSheet.Cells["K" + 3].Value = "SALDO";
                    workSheet.Cells["K" + 3].Style.Font.Size = 12;
                    workSheet.Cells["K" + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["K" + 3].Style.Font.Bold = true;
                    workSheet.SelectedRange[3, 11, 3, 12].Merge = true;

                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);
                }

                // autofit width of cells with small content  

                if (dataTable.Rows.Count > 0)
                {

                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {

                        //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        //if (maxLength < 150)
                        //{
                        //    workSheet.Column(columnIndex).AutoFit();
                        //}


                        columnIndex++;
                    }
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    //r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#dcdcdc"));
                }

                using (ExcelRange r = workSheet.Cells[startRowFrom - 1, 1, startRowFrom - 1, 11])
                {
                    //r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#c1c1c1"));
                }

                // format cells - add borders 
                if (dataTable.Rows.Count > 0)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1 - 2, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }
                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel2<T>(string[] header, List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel2(ListToDataTable<T>(data), "ConsultaMovimiento", Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportExcelStock_Ecom1<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcelStock_ECOM2(ListToDataTable<T>(data), "StockAlmacen", Heading, showSlno, ColumnsToTake);
        }

        //nuevo
        public static byte[] ExportExcelStock_ECOM2(DataTable dataTable, string header = "", string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", ""));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 4;
                //int startRowFrom = 0;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                if (header == "StockAlmacen")
                {
                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);
                }

                // autofit width of cells with small content  

                if (dataTable.Rows.Count > 0)
                {

                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {

                        columnIndex++;
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1 - 2, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }


                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }
                workSheet.DeleteRow(startRowFrom);
                result = package.GetAsByteArray();
            }

            return result;
        }


        //public void CreateCSVFile(ref DataTable dt, string strFilePath)
        //{
        //    try
        //    {
        //        // Create the CSV file to which grid data will be exported.
        //        System.IO.StreamWriter sw = new StreamWriter(strFilePath, false);
        //        // First we will write the headers.
        //        //DataTable dt = m_dsProducts.Tables[0];
        //        int iColCount = dt.Columns.Count;
        //        for (int i = 0; i < iColCount; i++)
        //        {
        //            sw.Write(dt.Columns[i]);
        //            if (i < iColCount - 1)
        //            {
        //                sw.Write(",");
        //            }
        //        }
        //        sw.Write(sw.NewLine);

        //        // Now write all the rows.

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            for (int i = 0; i < iColCount; i++)
        //            {
        //                if (!Convert.IsDBNull(dr[i]))
        //                {
        //                    sw.Write(dr[i].ToString());
        //                }
        //                if (i < iColCount - 1)
        //                {
        //                    sw.Write(",");
        //                }
        //            }

        //            sw.Write(sw.NewLine);
        //        }
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
        public static byte[] ExportExcel3(DataTable _dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            DataTable dataTable = _dataTable.Clone();
            dataTable.Columns[3].DataType = typeof(String);
            foreach (DataRow row in _dataTable.Rows)
            {
                dataTable.ImportRow(row);
            }


            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }

                foreach (DataRow item in dataTable.Rows)
                {
                    item[3] = (Convert.ToBoolean(item[3]) ? "TRUE" : "FALSE");
                }

                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  

                if (dataTable.Rows.Count > 0)
                {

                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {



                        //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        //if (maxLength < 150)
                        //{
                        //    workSheet.Column(columnIndex).AutoFit();
                        //}


                        columnIndex++;
                    }
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders 
                if (dataTable.Rows.Count > 0)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }
                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel3<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel3(ListToDataTable2<T>(data), Heading, showSlno, ColumnsToTake);
        }
        public static DataTable ListToDataTable2<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                    //if (i == 3)
                    //{
                    //    values[i] = (Convert.ToBoolean(properties[i].GetValue(item)) ? "TRUE" : "FALSE");
                    //}
                    //else
                    //{
                    //    values[i] = properties[i].GetValue(item);
                    //}                    
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel_Prestashop<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcelStock_PRESTA(ListToDataTable<T>(data), "PRESTA", Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportExcelStock_PRESTA(DataTable dataTable, string header = "", string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {


            for (int i = 0; i< dataTable.Rows.Count; i++)
            {
                if ( dataTable.Rows[i]["PRESTA_FECING"].ToString() == "01/01/1900")
                {
                    dataTable.Rows[i]["PRESTA_FECING"] = "";
                }

                if (dataTable.Rows[i]["fecha_facturacion"].ToString() == "01/01/1900")
                {
                    dataTable.Rows[i]["fecha_facturacion"] = "";
                }
            }


            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", ""));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 4;
                //int startRowFrom = 0;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                if (header == "PRESTA")
                {
                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);
                }

                // autofit width of cells with small content  

                if (dataTable.Rows.Count > 0)
                {

                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {

                        columnIndex++;
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1 - 2, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }


                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }
                workSheet.DeleteRow(startRowFrom);
                result = package.GetAsByteArray();
            }

            return result;
        }


    }
}