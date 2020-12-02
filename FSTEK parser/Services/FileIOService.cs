using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FSTEK_parser.Model;
using System.Globalization;


namespace FSTEK_parser.Services
{
    class FileIOService
    {
        private readonly string PATH;

        public FileIOService(string path)
        {
            PATH = path;
        }

        public PagingCollectionView LoadData()
        {
            PagingCollectionView cview;
            List<ThreatModel> modelList = new List<ThreatModel>();
            if (File.Exists(PATH))
            {
                using (var workbook = new XLWorkbook(PATH))
                {
                    try
                    {
                        // Берем в ней первый лист
                        for (int i = 1; i <= workbook.Worksheets.Count; i++)
                        {
                            var worksheet = workbook.Worksheets.Worksheet(i);
                            int sheetCount = worksheet.RowsUsed().Count();
                            // Перебираем диапазон нужных строк
                            for (int row = 3; row <= sheetCount; ++row)
                            {
                                DateTimeFormatInfo ruDt = new CultureInfo("ru-RU", false).DateTimeFormat;
                                // По каждой строке формируем объект
                                ThreatModel threat = new ThreatModel
                                {
                                    Id = worksheet.Cell(row, 1).GetValue<int>(),
                                    Name = worksheet.Cell(row, 2).GetValue<string>(),
                                    Description = worksheet.Cell(row, 3).GetValue<string>(),
                                    Source = worksheet.Cell(row, 4).GetValue<string>(),
                                    Object = worksheet.Cell(row, 5).GetValue<string>(),

                                    Conf = (worksheet.Cell(row, 6).GetValue<int>()).ToString(),
                                    Sust = (worksheet.Cell(row, 7).GetValue<int>()).ToString(),
                                    Access = (worksheet.Cell(row, 8).GetValue<int>()).ToString(),

                                    InDate = Convert.ToDateTime(worksheet.Cell(row, 9).GetValue<string>(), ruDt),
                                    EditDate = Convert.ToDateTime(worksheet.Cell(row, 9).GetValue<string>(), ruDt),
                                };
                                modelList.Add(threat);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            else
            {
                return null;
            }
            cview = new PagingCollectionView(modelList, 15);
            return cview;
        }
    }
}
