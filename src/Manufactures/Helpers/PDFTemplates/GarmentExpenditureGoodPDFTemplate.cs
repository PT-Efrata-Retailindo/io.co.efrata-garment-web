using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Manufactures.Helpers.PDFTemplates
{
    public class GarmentExpenditureGoodPDFTemplate
    {
        public static MemoryStream Generate(GarmentExpenditureGoodDto exGood, string buyer)
        {
            Document document = new Document(PageSize.A5.Rotate(), 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell cellRightNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellJustifyNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
            PdfPCell cellJustifyAllNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL };

            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };


            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            #region Header
            PdfPTable tableHeader = new PdfPTable(3);
            tableHeader.SetWidths(new float[] { 1f, 1f, 1f });

            PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft.AddElement(new Phrase("PT EFRATA RETAILINDO", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("SUKOHARJO", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("BANARAN, GROGOL", normal_font));
            tableHeader.AddCell(cellHeaderContentLeft);

            PdfPCell cellHeaderContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentCenter.AddElement(new Paragraph("BON PENGIRIMAN", header_font));
            cellHeaderContentCenter.AddElement(new Paragraph("Tanggal : " + exGood.ExpenditureDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            
            tableHeader.AddCell(cellHeaderContentCenter);

            PdfPCell cellHeaderContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
            //cellHeaderContentRight.AddElement(new Phrase("FM-00-AD-09-008 B", normal_font));
            cellHeaderContentRight.AddElement(new Phrase("BUYER  :" + buyer, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("ART.NO : " + exGood.Article, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("NO.        : " + exGood.ExpenditureGoodNo, normal_font));
            tableHeader.AddCell(cellHeaderContentRight);


            PdfPCell cellHeaderContentLeft2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft2.AddElement(new Phrase("Kepada Yth. Bag.Penjualan, untuk dikirimkan kepada ....", normal_font));
            cellHeaderContentLeft2.Colspan = 2;
            tableHeader.AddCell(cellHeaderContentLeft2);

            PdfPCell cellHeaderContentRight2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentRight2.AddElement(new Phrase("", normal_font));
            tableHeader.AddCell(cellHeaderContentRight2);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            List<string> sizes = new List<string>();
            List<string> colors = new List<string>();
            Dictionary<string, double> detailData = new Dictionary<string, double>();
            Dictionary<string, string> remarks = new Dictionary<string, string>();

            foreach (var item in exGood.Items)
            {

                if (!sizes.Contains(item.Size.Size))
                    sizes.Add(item.Size.Size);
                if (!colors.Contains(item.Description))
                    colors.Add(item.Description);

                var key = item.Size.Size + "~" + item.Description;

                if (detailData.ContainsKey(key))
                {
                    detailData[key] += item.Quantity;
                }
                else
                {
                    detailData.Add(key, item.Quantity);
                }

                //if (remarks.ContainsKey(item.Color))
                //{
                //    var dup = remarks.Where(a => a.Value == item.Description && a.Key == item.Color);
                //    remarks[item.Color] = dup == null ? remarks[item.Color] + ", " + item.Description : remarks[item.Color];
                //}
                //else
                //{
                //    remarks.Add(item.Color, item.Description);
                //}

            }

            sizes.Sort();

            #region content

            PdfPTable tableContent = new PdfPTable(4 + sizes.Count());
            List<float> widths = new List<float>();
            widths.Add(2f);
            widths.Add(5f);

            foreach (var s in sizes)
            {
                widths.Add(3f);
            }

            widths.Add(4f);
            widths.Add(4f);

            tableContent.SetWidths(widths.ToArray());

            cellCenter.Phrase = new Phrase("No", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Colour", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Size", bold_font);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = sizes.Count();
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Quantity", bold_font);
            cellCenter.Colspan = 1;
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);

            foreach (var s in sizes)
            {
                cellCenter.Phrase = new Phrase(s, bold_font);
                cellCenter.Rowspan = 1;
                cellCenter.Colspan = 1;
                tableContent.AddCell(cellCenter);
            }

            int count = 0;

            foreach (var i in colors)
            {

                count++;
                cellCenter.Phrase = new Phrase(count.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                double total = 0;
                foreach (var s in sizes)
                {
                    if (detailData.ContainsKey(s + "~" + i))
                    {
                        cellCenter.Phrase = new Phrase(detailData[s + "~" + i].ToString(), normal_font);
                        cellCenter.Rowspan = 1;
                        tableContent.AddCell(cellCenter);
                        total += detailData[s + "~" + i];
                    }
                    else
                    {
                        cellCenter.Phrase = new Phrase("", normal_font);
                        cellCenter.Rowspan = 1;
                        tableContent.AddCell(cellCenter);
                    }
                }
                cellCenter.Phrase = new Phrase(total.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase("", normal_font);//remarks[i].ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
            }

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 15f;
            document.Add(tableContent);
            #endregion

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(5);

            cellLeftNoBorder.Phrase = new Paragraph($"{exGood.Description}", normal_font);
            cellLeftNoBorder.Colspan = 3;
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            cellLeftNoBorder.Colspan = 1;
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph($"TTL CTNS : {exGood.Carton.ToString()}", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Paragraph($"No. Inv : {exGood.Invoice}", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph($"No. R/O : {exGood.RONo}", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Paragraph($"{exGood.ContractNo}", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);


            cellCenterTopNoBorder.Phrase = new Paragraph("\n\n", normal_font);
            cellCenterTopNoBorder.Colspan = 5;
            tableSignature.AddCell(cellCenterTopNoBorder);


            cellCenterTopNoBorder.Phrase = new Paragraph("Expedisi\n\n\n\n\n\n\n\n(                                   )", normal_font);
            cellCenterTopNoBorder.Colspan = 1;
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("\n\n\n\n\n\n\n\n                                   ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Kasie\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("\n\n\n\n\n\n\n\n                                   ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Bag. Gudang\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);


            cellLeftNoBorder.Phrase = new Paragraph($"Dicetak : {DateTimeOffset.Now.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID"))}", normal_font);
            cellLeftNoBorder.Colspan = 2;
            tableSignature.AddCell(cellLeftNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            cellCenterTopNoBorder.Colspan = 1;
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);

            PdfPCell cellSignature = new PdfPCell(tableSignature);
            tableSignature.ExtendLastRow = false;
            document.Add(tableSignature);

            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
