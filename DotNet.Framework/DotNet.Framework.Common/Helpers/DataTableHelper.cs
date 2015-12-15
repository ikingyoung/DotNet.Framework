using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Helpers
{
    class DataTableHelper
    {
        /// <summary>
        /// Create a DataTable
        /// </summary>
        /// <param name="_colsName">colunms name as a array</param>
        /// <returns>a DataTable instance</returns>
        public static DataTable CreateDataTable(IEnumerable<string> colsNames)
        {
            DataTable _rtDT = new DataTable();

            foreach (var colName in colsNames)
            {
                _rtDT.Columns.Add(colName.Trim());
            }
            
            return _rtDT;
        }

        /// <summary>
        /// get a DataTable which From DataGridView
        /// </summary>
        /// <param name="_dgv">a DataGridView instance</param>
        /// <returns>a DataTable instance</returns>
        //public static DataTable GridView2DataTable(DataGridView _dgv)
        //{
        //    DataTable _retDataTabel = new DataTable();
        //    //DataGridViewRow _headCol = _dgv.c;
        //    //int _colCount = 0;

        //    if (_dgv.Rows.Count > 0)
        //    {
        //        foreach (DataGridViewColumn _dgvCol in _dgv.Columns)
        //        {
        //            _retDataTabel.Columns.Add(new DataColumn(_dgvCol.HeaderText.ToString().Trim().Replace(" ", "")));
        //        }
        //        foreach (DataGridViewRow _dgvRow in _dgv.Rows)
        //        {
        //            DataRow _dtRow = _retDataTabel.NewRow();
        //            for (int _iLoop = 0; _iLoop < _dgvRow.Cells.Count; _iLoop++)
        //            {
        //                string _value = _dgvRow.Cells[_iLoop].Value == null ? String.Empty : _dgvRow.Cells[_iLoop].Value.ToString().Trim();
        //                _dtRow[_iLoop] = _value;
        //            }
        //            _retDataTabel.Rows.Add(_dtRow);
        //        }
        //    }
        //    else
        //    {
        //        _retDataTabel = null;
        //    }
        //    return _retDataTabel;

        //}

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="validateLayout">是否检查数据列数与标题列数一样</param>
        /// <param name="topRowIsHeader"></param>
        public static DataTable CsvToDataTable(string fileName, bool validateLayout, bool topRowIsHeader)
        {
            var lines = File.ReadAllLines(fileName);
            return DataTableFromCSV(lines, validateLayout, topRowIsHeader);
        }

        /// <summary>
        /// 转换Csv文件为DataTable
        /// </summary>
        /// <param name="csvFileContents"></param>
        /// <param name="validateLayout">是否检查数据列数与标题列数一样</param>
        /// <param name="topRowIsHeader"></param>
        /// <returns></returns>
        public static DataTable DataTableFromCSV(string[] csvFileContents, bool validateLayout, bool topRowIsHeader)
        {
            DataTable outputDataTable = new DataTable();
            List<string[]> csvFileRows = new List<string[]>();
            List<string> columns = new List<string>();

            #region Pre-parse the file
            int columnCount = 0;

            bool gotHeaders = false;
            int rowNumber = 0;

            foreach (string line in csvFileContents)
            {
                string[] parts = ExtractCSVElements(line);

                //initial set of header names but only if the top row is header option is set
                if (!gotHeaders && topRowIsHeader)
                {
                    columns.AddRange(parts);
                    columnCount = parts.Length;
                    gotHeaders = true;
                }
                else
                {
                    if (parts.Length > 0)
                    {
                        csvFileRows.Add(parts);
                    }
                }

                if (parts.Length > columnCount)
                {
                    //if set to validate the layout and that the first row contains the headers then we know any extra columns are wrong
                    if (validateLayout && gotHeaders)
                    {
                        throw new Exception("Row has extra data columns: " + rowNumber.ToString());
                    }

                    //new column detected mid-data-set!
                    for (int i = columnCount; i < parts.Length; i++)
                    {
                        columns.Add("Column " + i.ToString());
                    }

                    columnCount = parts.Length;
                }

                //we always ignore zero length rows as the last line can be empty
                if (parts.Length < columnCount && parts.Length != 0)
                {
                    if (validateLayout)
                    {
                        throw new Exception("Row has missing data columns: " + rowNumber.ToString());
                    }
                }


                rowNumber++;
            }

            #endregion

            #region Build the data tables layout and data

            //columns
            foreach (string column in columns)
            {
                outputDataTable.Columns.Add(column);
            }

            //rows
            foreach (string[] row in csvFileRows)
            {

                outputDataTable.Rows.Add(row.ToArray());
            }
            #endregion

            return outputDataTable;
        }
        /// <summary>
        /// Extract the elements of a line from a CSV file with support for quotes
        /// </summary>
        /// <param name="line">The data to parse</param>
        private static string[] ExtractCSVElements(string line)
        {
            List<string> elements = new List<string>();

            //do the initial split, based on commas
            string[] firstParts = line.Split(',');

            //reparse it
            StringBuilder temporaryPart = new StringBuilder("");
            bool inside = false;
            foreach (string part in firstParts)
            {
                //are we inside a quoted part, or did we just find a quote?
                if (!inside //we're not inside
                 && (!part.Contains("\"") //and we don't contain a quote
                 || ( //or we're handling a single quote enclosed element
                        part.StartsWith("\"") //we start with a quote
                        && part.EndsWith("\"") //and we end with a quote)
                    )
                ))
                {
                    elements.Add(part);
                }
                else
                {
                    if (inside)
                    {
                        //we are still inside a quote...
                        temporaryPart.Append("," + part);

                        if (part.Contains("\""))
                        {
                            //then we are also at the end!
                            elements.Add(temporaryPart.Replace("\"", "").ToString()); //add the part minus its quotes to the array
                            //all done!
                            inside = false;
                        }
                    }
                    else
                    {
                        //else we just found a quote!
                        inside = true;
                        temporaryPart = new StringBuilder(part);
                    }
                }
            }

            return elements.ToArray();
        }
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public void SaveDataTableToCsv(DataTable dt, string fileName)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();
            //MessageBox.Show("CSV文件保存成功！");
        }
    }
}
