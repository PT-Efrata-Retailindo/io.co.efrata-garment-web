using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.ExcelTemplates
{
    public class GarmentSubconContractAgreementExcelTemplate
    {
        public static MemoryStream GenerateExcelTemplate(GarmentSubconContractExcelDto dto)
        {
            DataTable result = new DataTable();

            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Subcon Contract");
            if (dto.ContractType!="SUBCON GARMENT")
            {
                sheet.Cells[$"A3:K3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                sheet.Cells[$"K4:K44"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                sheet.Cells[$"A44:K44"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                sheet.Cells[$"A4:A44"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

                sheet.Cells[$"B4:K4"].Merge = true;
                sheet.Cells[$"B4:K4"].RichText.Add("PERJANJIAN SUB KONTRAK").UnderLine = true;
                sheet.Cells[$"B4:K4"].Style.Font.Size = 10;
                sheet.Cells[$"B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[$"B4"].Style.Font.SetFromFont(new Font("Calibri", 12, FontStyle.Underline));

                #region identity
                sheet.Cells[$"B6"].Value = "No";
                sheet.Cells[$"C6:F6"].Merge = true;
                sheet.Cells[$"C6"].Value = $": {dto.ContractNo}";
                sheet.Cells[$"C7:F7"].Merge = true;
                sheet.Cells[$"B7"].Value = "Hal";
                sheet.Cells[$"C7"].Value = ":";

                sheet.Cells[$"I6:K6"].Merge = true;
                sheet.Cells[$"I6:K6"].Value = $"Sukoharjo, {dto.ContractDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}";
                sheet.Cells[$"I7:K7"].Merge = true;
                sheet.Cells[$"I7"].Value = "Kepada Yth,";
                sheet.Cells[$"I8:K8"].Merge = true;
                sheet.Cells[$"I8"].Value = $"{dto.Supplier.Name}";

                #endregion

                #region detail1
                sheet.Cells[$"B13:K13"].Merge = true;
                sheet.Cells[$"B13"].Value = "Dengan Hormat,";

                sheet.Cells[$"B15:K15"].Merge = true;
                sheet.Cells[$"B15"].Value = "Sesuai dengan persetujuan order ..... kami, maka bersama ini kami kirimkan surat kontrak";
                sheet.Cells[$"B16:K16"].Merge = true;
                sheet.Cells[$"B16"].Value = "dengan ketentuan dan syarat-syarat dibawah ini:";
                sheet.Cells[$"B18:K18"].Merge = true;
                sheet.Cells[$"B18"].Value = "PT. Dan Liris, selanjutnya disebut PIHAK PERTAMA akan mengirimkan";
                sheet.Cells[$"B19:K19"].Merge = true;
                sheet.Cells[$"B19"].Value = $"kepada {dto.Supplier.Name} dengan ketentuan sbb:";

                sheet.Cells[$"B20:C20"].Merge = true;
                sheet.Cells[$"B20"].Value = "Material";
                sheet.Cells[$"D20:K20"].Merge = true;
                sheet.Cells[$"D20"].Value = ":";
                sheet.Cells[$"B21:C21"].Merge = true;
                sheet.Cells[$"B21"].Value = "Jumlah Total";
                sheet.Cells[$"D21:K21"].Merge = true;
                sheet.Cells[$"D21"].Value = $": {dto.Quantity} {dto.Uom.Unit}";
                sheet.Cells[$"B22:C22"].Merge = true;
                sheet.Cells[$"B22"].Value = "Ongkos .....";
                sheet.Cells[$"D22:K22"].Merge = true;
                sheet.Cells[$"D22"].Value = ":";
                sheet.Cells[$"B23:C23"].Merge = true;
                sheet.Cells[$"B23"].Value = "Transportasi";
                sheet.Cells[$"D23:K23"].Merge = true;
                sheet.Cells[$"D23"].Value = ":";

                #endregion

                #region detail2
                sheet.Cells[$"B25:K25"].Merge = true;
                sheet.Cells[$"B25"].Value = $"{dto.Supplier.Name}, selanjutnya disebut PIHAK KEDUA menggunakan tersebut";
                sheet.Cells[$"B26:K26"].Merge = true;
                sheet.Cells[$"B26"].Value = "diatas untuk di ....., dan selanjutnya dikirim kembali ke PT. Dan Liris dengan ketentuan";
                sheet.Cells[$"B27:K27"].Merge = true;
                sheet.Cells[$"B27"].Value = "sebagai berikut:";

                sheet.Cells[$"B28:C28"].Merge = true;
                sheet.Cells[$"B28"].Value = "Material";
                sheet.Cells[$"D28:K28"].Merge = true;
                sheet.Cells[$"D28"].Value = ":";
                sheet.Cells[$"B29:C29"].Merge = true;
                sheet.Cells[$"B29"].Value = "Jumlah Total";
                sheet.Cells[$"D29:K29"].Merge = true;
                sheet.Cells[$"D29"].Value = $": {dto.Quantity} {dto.Uom.Unit}";
                sheet.Cells[$"B30:C30"].Merge = true;
                sheet.Cells[$"B30"].Value = "Packing";
                sheet.Cells[$"D30:K30"].Merge = true;
                sheet.Cells[$"D30"].Value = ":";
                sheet.Cells[$"B31:C31"].Merge = true;
                sheet.Cells[$"B31"].Value = "Masa Pengerjaan";
                sheet.Cells[$"D31:K31"].Merge = true;
                sheet.Cells[$"D31"].Value = ":";
                sheet.Cells[$"B32:C32"].Merge = true;
                sheet.Cells[$"B32"].Value = "Pembayaran";
                sheet.Cells[$"D32:K32"].Merge = true;
                sheet.Cells[$"D32"].Value = ":";

                #endregion

                sheet.Cells[$"B34:K34"].Merge = true;
                sheet.Cells[$"B34"].Value = "Sisa kain yang BS atau rusak dikembalikan ke PIHAK PERTAMA.";
                sheet.Cells[$"B35:K35"].Merge = true;
                sheet.Cells[$"B35"].Value = "Terima kasih atas perhatian dan kerjasamanya.";

                #region Signature
                var signatureIndex = 37;
                sheet.Cells[$"B{signatureIndex}:D{signatureIndex}"].Merge = true;
                sheet.Cells[$"B{signatureIndex}"].Value = "PIHAK PERTAMA";
                sheet.Cells[$"B{signatureIndex}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"B{signatureIndex + 4}:D{signatureIndex + 4}"].Merge = true;
                sheet.Cells[$"B{signatureIndex + 4}"].Value = "(                               )";
                sheet.Cells[$"B{signatureIndex + 4}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"B{signatureIndex + 5}:D{signatureIndex + 5}"].Merge = true;
                sheet.Cells[$"B{signatureIndex + 5}"].Value = "Kabag Pembelian";
                sheet.Cells[$"B{signatureIndex + 5}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"B{signatureIndex + 6}:D{signatureIndex + 6}"].Merge = true;
                sheet.Cells[$"B{signatureIndex + 6}"].Value = "PT. DAN LIRIS";
                sheet.Cells[$"B{signatureIndex + 6}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                sheet.Cells[$"H{signatureIndex}:J{signatureIndex}"].Merge = true;
                sheet.Cells[$"H{signatureIndex}"].Value = "PIHAK KEDUA";
                sheet.Cells[$"H{signatureIndex}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"H{signatureIndex + 4}:J{signatureIndex + 4}"].Merge = true;
                sheet.Cells[$"H{signatureIndex + 4}"].Value = "(                               )";
                sheet.Cells[$"H{signatureIndex + 4}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"H{signatureIndex + 5}:J{signatureIndex + 5}"].Merge = true;
                sheet.Cells[$"H{signatureIndex + 5}"].Value = "Direktur";
                sheet.Cells[$"H{signatureIndex + 5}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"H{signatureIndex + 6}:J{signatureIndex + 6}"].Merge = true;
                sheet.Cells[$"H{signatureIndex + 6}"].Value = dto.Supplier.Name;
                sheet.Cells[$"H{signatureIndex + 6}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                #endregion
            }
            else
            {

                sheet.Cells[$"A3:N3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

                sheet.Cells[$"B4:N4"].Merge = true;
                sheet.Cells[$"B4:N4"].RichText.Add("SURAT PERJANJIAN KONTRAK KERJA").Bold = true;
                sheet.Cells[$"B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[$"B4"].Style.Font.SetFromFont(new Font("Calibri", 12, FontStyle.Underline));

                sheet.Cells[$"B5:N5"].Merge = true;
                sheet.Cells[$"B5:N5"].RichText.Add($"NO : {dto.ContractNo}").Bold = true;
                sheet.Cells[$"B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[$"B5"].Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Underline));

                sheet.Cells[$"B7:N7"].Merge = true;
                sheet.Cells[$"B7"].Value = "Yang bertanda tangan di bawah ini:";

                sheet.Cells[$"B8:F8"].Merge = true;
                sheet.Cells[$"B8"].Value = "Nama";
                sheet.Cells[$"G8:N8"].Merge = true;
                sheet.Cells[$"G8"].Value = ":";

                sheet.Cells[$"B9:F9"].Merge = true;
                sheet.Cells[$"B9"].Value = "Jabatan";
                sheet.Cells[$"G9:N9"].Merge = true;
                sheet.Cells[$"G9"].Value = ": Purchasing Manager";

                sheet.Cells[$"B10:F10"].Merge = true;
                sheet.Cells[$"B10"].Value = "Nama Perusahaan";
                sheet.Cells[$"G10:N10"].Merge = true;
                sheet.Cells[$"G10"].Value = ": PT. Dan Liris";

                sheet.Cells[$"B11:F11"].Merge = true;
                sheet.Cells[$"B11"].Value = "Alamat";
                sheet.Cells[$"G11:N11"].Merge = true;
                sheet.Cells[$"G11"].Value = ": Banaran, Grogol, Sukoharjo";

                sheet.Cells[$"B12:N12"].Merge = true;
                sheet.Cells[$"B12"].Value = "Selanjutnya disebut pihak I (Pertama)";

                sheet.Cells[$"B14:F14"].Merge = true;
                sheet.Cells[$"B14"].Value = "Nama";
                sheet.Cells[$"G14:N14"].Merge = true;
                sheet.Cells[$"G14"].Value = ":";

                sheet.Cells[$"B15:F15"].Merge = true;
                sheet.Cells[$"B15"].Value = "Jabatan";
                sheet.Cells[$"G15:N15"].Merge = true;
                sheet.Cells[$"G15"].Value = ":";

                sheet.Cells[$"B16:F16"].Merge = true;
                sheet.Cells[$"B16"].Value = "Nama Perusahaan";
                sheet.Cells[$"G16:N16"].Merge = true;
                sheet.Cells[$"G16"].Value = ":";

                sheet.Cells[$"B17:F17"].Merge = true;
                sheet.Cells[$"B17"].Value = "Alamat";
                sheet.Cells[$"G17:N17"].Merge = true;
                sheet.Cells[$"G17"].Value = ":";

                sheet.Cells[$"B18:N18"].Merge = true;
                sheet.Cells[$"B18"].Value = "Selanjutnya disebut pihak II (Kedua)";


                sheet.Cells[$"B20:N20"].Merge = true;
                sheet.Cells[$"B20"].Value = "Dengan ini pihak I memberikan pekerjaan kepada pihak II dengan rincian syarat-syarat pekerjaan sbb:";

                sheet.Cells[$"B21"].Value = "1";
                sheet.Cells[$"B21"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C21:E21"].Merge = true;
                sheet.Cells[$"C21"].Value = "Job";
                sheet.Cells[$"F21:N21"].Merge = true;
                sheet.Cells[$"F21"].Value = dto.SubconCategory;

                sheet.Cells[$"B22"].Value = "2";
                sheet.Cells[$"B22"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C22:E22"].Merge = true;
                sheet.Cells[$"C22"].Value = "Brand";
                sheet.Cells[$"F22:N22"].Merge = true;
                sheet.Cells[$"F22"].Value = "";
                int row = 23;
                sheet.Cells[$"B{row}"].Value = "3";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Style";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"B{row}"].Value = "4";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Jenis Barang";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"B{row}"].Value = "5";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Price";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"B{row}"].Value = "6";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Jangka waktu Sub-Kontrak";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"B{row}"].Value = "7";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Barang yang dikirimkan";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "A. Material";
                row++;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "kepada pihak kedua";

                if(dto.Items !=null && dto.Items.Count > 0)
                {
                    var material = dto.Items.Where(a=>a.Product.Name=="FABRIC" || a.Product.Name=="INTERLINING").OrderBy(a=>a.Product.Name).ToList();
                    var acc = dto.Items.Where(a => a.Product.Name != "FABRIC" && a.Product.Name != "INTERLINING").OrderBy(a => a.Product.Name).ToList();
                    if (material != null)
                    {
                        foreach(var m in material)
                        {
                            sheet.Cells[$"F{row}:G{row}"].Merge = true;
                            sheet.Cells[$"F{row}"].Value = m.Product.Name;
                            sheet.Cells[$"H{row}:N{row}"].Merge = true;
                            sheet.Cells[$"H{row}"].Value = $": {m.Quantity} {m.Uom.Unit}";
                            row++;
                        }
                    }
                    else
                    {
                        sheet.Cells[$"F{row}:N{row}"].Merge = true;
                        sheet.Cells[$"F{row}"].Value = "-";
                        row++;
                    }

                    sheet.Cells[$"F{row}:N{row}"].Merge = true;
                    sheet.Cells[$"F{row}"].Value = "B. Accessories";
                    row++;
                    if (acc != null)
                    {
                        foreach (var a in acc)
                        {
                            sheet.Cells[$"F{row}:G{row}"].Merge = true;
                            sheet.Cells[$"F{row}"].Value = a.Product.Name;
                            sheet.Cells[$"H{row}:N{row}"].Merge = true;
                            sheet.Cells[$"H{row}"].Value = $": {a.Quantity} {a.Uom.Unit}";
                            row++;
                        }
                    }
                    else
                    {
                        sheet.Cells[$"F{row}:N{row}"].Merge = true;
                        sheet.Cells[$"F{row}"].Value = "-";
                        row++;
                    }
                }
                else
                {
                    sheet.Cells[$"F{row}:N{row}"].Merge = true;
                    sheet.Cells[$"F{row}"].Value = "-";
                    row++;
                    sheet.Cells[$"F{row}:N{row}"].Merge = true;
                    sheet.Cells[$"F{row}"].Value = "B. Accessories";
                    row++;
                    sheet.Cells[$"F{row}:N{row}"].Merge = true;
                    sheet.Cells[$"F{row}"].Value = "-";
                    row++;
                }

                sheet.Cells[$"B{row}"].Value = "8";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Packing";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"B{row}"].Value = "9";
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"C{row}:E{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Waste";
                sheet.Cells[$"F{row}:N{row}"].Merge = true;
                sheet.Cells[$"F{row}"].Value = "";
                row++;
                sheet.Cells[$"C{row}:N{row}"].Merge = true;
                sheet.Cells[$"C{row}"].Value = "Sisa produksi dikembalikan kepada pihak pertama";


                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL I").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"A{row}"].Value = "1";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "PIHAK PERTAMA memberikan Material kepada pihak II sesuai dengan kebutuhan produksi yang akan";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "dikerjakan PIHAK KEDUA";
                row++;
                sheet.Cells[$"A{row}"].Value = "2";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Komplain atas kekurangan Material diterima PIHAK PERTAMA dalam waktu 1 x 24 Jam";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL II").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"A{row}"].Value = "1";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Sebelum PIHAK KEDUA mengirimkan barang ke PIHAK PERTAMA, barang harus dicek terlebih dahulu oleh PIHAK PERTAMA";
                row++;
                sheet.Cells[$"A{row}"].Value = "2";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Komplain atau klaim dari PIHAK PERTAMA kepada PIHAK KEDUA karena adanya Barang Reject, dilakukan dalam waktu";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "4 hari kerja setelah Barang diterima oleh PIHAK PERTAMA";
                row++;
                sheet.Cells[$"A{row}"].Value = "3";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Rework dikembalikan oleh PIHAK PERTAMA kepada PIHAK KEDUA, untuk diperbaiki sesuai dengan kualitas yang telah";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "disepakati";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL III").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Jatuh tempo pembayaran sub cont, 30 hari kerja setelah terima tagihan";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL IV").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Transportasi menjadi tanggung jawab PIHAK KEDUA";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL V").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "PIHAK KEDUA mempunyai kewajiban-kewajiban sebagai berikut:";
                row++;
                sheet.Cells[$"A{row}"].Value = "1";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Bertanggung jawab sepenuhnya terhadap pengamanan barang serta keutuhan mutu barang yang diberikan PIHAK PERTAMA";
                row++;
                sheet.Cells[$"A{row}"].Value = "2";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "PIHAK KEDUA dilarang memindahkan pekerjaan yang diterima kepada PIHAK KETIGA atau pihak manapun tanpa persetujuan";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "dari PIHAK PERTAMA";
                row++;
                sheet.Cells[$"A{row}"].Value = "3";
                sheet.Cells[$"A{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Bersedia untuk melaksanakan revisi sesegera mungkin jika garment tidak sesuai dengan standart";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL VI").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Kedua belah pihak sepakat dan menyetujui isi dari surat perjanjian kontrak kerja ini yang telah ditandai dengan dibubuhkan tanda";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "tangan oleh masing-masing pihak diatas kertas bermaterai cukup, surat perjanjian kontrak kerja ini dibuat rangkap 2 (dua) dan";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "masing-masing mempunyai kekuatan hukum yang sama dan tak terpisahkan dan pihak kedua bersedia diaudit oleh dirjen";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "bea cukai atas pekerjaan sub kontrak ini.";
                row += 2;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}:N{row}"].RichText.Add("PASAL VII").Bold = true;
                sheet.Cells[$"B{row}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "Apabila timbul perselisihan/sengketa antara kedua belah pihak, maka kedua belah pihak sepakat untuk menyelesaikan dengan ";
                row++;
                sheet.Cells[$"B{row}:N{row}"].Merge = true;
                sheet.Cells[$"B{row}"].Value = "musyawarah dan apabila cara musyawarah menemui jalan buntu, maka kedua belah pihak sepakat untuk menempuh jalur hukum";
                row++;

                #region Signature
                var signatureIndex = row+4;
                sheet.Cells[$"B{signatureIndex}:D{signatureIndex}"].Merge = true;
                sheet.Cells[$"B{signatureIndex}"].Value = "PIHAK PERTAMA";
                sheet.Cells[$"B{signatureIndex}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"B{signatureIndex + 4}:D{signatureIndex + 4}"].Merge = true;
                sheet.Cells[$"B{signatureIndex + 4}"].Value = "(                               )";
                sheet.Cells[$"B{signatureIndex + 4}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"B{signatureIndex + 5}:D{signatureIndex + 5}"].Merge = true;
                sheet.Cells[$"B{signatureIndex + 5}"].Value = "Purchasing Manager";
                sheet.Cells[$"B{signatureIndex + 5}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"K{signatureIndex - 2}:M{signatureIndex - 2}"].Merge = true;
                sheet.Cells[$"K{signatureIndex - 2}"].Value = $"Sukoharjo, {dto.ContractDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}";

                sheet.Cells[$"K{signatureIndex}:M{signatureIndex}"].Merge = true;
                sheet.Cells[$"K{signatureIndex}"].Value = "PIHAK KEDUA";
                sheet.Cells[$"K{signatureIndex}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"K{signatureIndex + 4}:M{signatureIndex + 4}"].Merge = true;
                sheet.Cells[$"K{signatureIndex + 4}"].Value = "(                               )";
                sheet.Cells[$"K{signatureIndex + 4}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                sheet.Cells[$"K{signatureIndex + 5}:M{signatureIndex + 5}"].Merge = true;
                sheet.Cells[$"K{signatureIndex + 5}"].Value = "Direktur Marketing";
                sheet.Cells[$"K{signatureIndex + 5}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                #endregion

                row += 10;
                sheet.Cells[$"N4:N{row}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                sheet.Cells[$"A{row}:N{row}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                sheet.Cells[$"A4:A{row}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            }


            sheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Regular));

            sheet.Cells.Style.WrapText = true;

            sheet.PrinterSettings.LeftMargin = 0.39M;
            sheet.PrinterSettings.TopMargin = 0;
            sheet.PrinterSettings.RightMargin = 0;

            sheet.PrinterSettings.Orientation = eOrientation.Portrait;


            MemoryStream stream = new MemoryStream();
            package.DoAdjustDrawings = false;
            package.SaveAs(stream);
            return stream;
        }
    }
}
