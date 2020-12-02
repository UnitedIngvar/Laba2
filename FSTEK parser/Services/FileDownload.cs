using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using ClosedXML.Excel;
using System.Globalization;
using System.Windows;
using FSTEK_parser.Model;

namespace FSTEK_parser.Services
{
    public class FileDownload
    {
        WebClient client = new WebClient();

        //Downloads the File
        public void Download()
        {
            string url = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
            String appStartPath = Directory.GetCurrentDirectory();
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.AbsolutePath);

            if (File.Exists(appStartPath + "/" + fileName))
            {
                Compare(appStartPath + "/" + fileName, uri, appStartPath, fileName);
            }
            else
            {
                client.DownloadFileAsync(uri, appStartPath + "/" + fileName);
            }
        }

        //Compares the new version with the old one
        public void Compare(string PATH, Uri uri, string appStartPath, string fileName)
        {
            string changes = "";

            File.Move(PATH ,Path.GetFileName(PATH).Replace($"{fileName}", $"old{fileName}"));
            PATH = Environment.CurrentDirectory + "/" + Path.GetFileName(PATH).Replace($"{fileName}", $"old{fileName}");

            client.DownloadFileAsync(uri, appStartPath + "/" + fileName);
            string newPATH = appStartPath + "/" + fileName;

            using (var workbook = new XLWorkbook(PATH))
            {
                using (var newWorkbook = new XLWorkbook(newPATH))
                {
                    try
                    {
                        for (int i = 1; i <= workbook.Worksheets.Count; i++)
                        {
                            var worksheet = workbook.Worksheets.Worksheet(i);
                            var newWorksheet = newWorkbook.Worksheets.Worksheet(i);

                            int sheetCount = worksheet.RowsUsed().Count();
                            int newSheetCount = newWorksheet.RowsUsed().Count();

                            if (sheetCount != newSheetCount)
                            {
                                changes += $"КОЛИЧЕСТВО УГРОЗ: Было: {sheetCount - 2} - Стало: {newSheetCount - 2}\n";
                            }

                            for (int row = 3; row <= sheetCount; ++row)
                            {
                                DateTimeFormatInfo ruDt = new CultureInfo("ru-RU", false).DateTimeFormat;

                                DateTime editDate = Convert.ToDateTime(worksheet.Cell(row, 10).GetValue<string>(), ruDt);
                                DateTime newEditDate = Convert.ToDateTime(newWorksheet.Cell(row, 10).GetValue<string>(), ruDt);

                                if(editDate != newEditDate)
                                {
                                    changes += $"УБИ №{row - 2}:\n";

                                    for (int column = 2; column <= 5; column++)
                                    {
                                        var oldData = worksheet.Cell(row, column).GetValue<string>();
                                        var newData = newWorksheet.Cell(row, column).GetValue<string>();
                                        if (!oldData.Equals(newData))
                                        {
                                            switch (column)
                                            {
                                                case 2:
                                                    changes += $"\tНаименование УБИ: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                case 3:
                                                    changes += $"\tОписание: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                case 4:
                                                    changes += $"\tИсточник угрозы: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                case 5:
                                                    changes += $"\tОбъект воздействия: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    for (int column = 6; column <= 8; column++)
                                    {
                                        var oldData = worksheet.Cell(row, column).GetValue<int>();
                                        var newData = newWorksheet.Cell(row, column).GetValue<int>();
                                        if (oldData != newData)
                                        {
                                            switch (column)
                                            {
                                                case 6:
                                                    changes += $"\tНарушение конфиденциальности: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                case 7:
                                                    changes += $"\tНарушение целостности: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                case 8:
                                                    changes += $"\tНарушение доступности: Было: {oldData} - Стало: {newData}\n";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        File.Delete(PATH);
                    }
                }
            }
            if(changes.Length != 0) { MessageBox.Show("Обновление прошло успешно! Произошли следующие изменения:\n" +changes); }
            else { MessageBox.Show("Обновление прошло успешно! Изменений не выявлено"); }
        }
    }
}
