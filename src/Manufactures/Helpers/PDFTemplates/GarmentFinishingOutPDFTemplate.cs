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
    public class GarmentFinishingOutPDFTemplate
    {
        public static MemoryStream Generate(GarmentFinishingOutDto finishing, string buyer)
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
            cellHeaderContentCenter.AddElement(new Paragraph("BON HASIL PRODUKSI", header_font));
            cellHeaderContentCenter.AddElement(new Paragraph("Tanggal : " + finishing.FinishingOutDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            cellHeaderContentCenter.AddElement(new Paragraph("No. R/O : " + finishing.RONo, normal_font));
            tableHeader.AddCell(cellHeaderContentCenter);

            PdfPCell cellHeaderContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
            //cellHeaderContentRight.AddElement(new Phrase("FM-00-AD-09-011", normal_font));
            cellHeaderContentRight.AddElement(new Phrase("BUYER  :" + buyer, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("ART.NO : " + finishing.Article, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("NO.        : " + finishing.FinishingOutNo, normal_font));

            tableHeader.AddCell(cellHeaderContentRight);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            List<string> sizes = new List<string>();
            List<string> colors = new List<string>();
            Dictionary<string, double> detailData = new Dictionary<string, double>();
            Dictionary<string, string> remarks = new Dictionary<string, string>();

            foreach (var item in finishing.Items)
            {
                if (finishing.IsDifferentSize)
                {
                    foreach (var detail in item.Details)
                    {
                        if (!sizes.Contains(detail.Size.Size))
                            sizes.Add(detail.Size.Size);
                        if (!colors.Contains(item.Color))
                            colors.Add(item.Color);

                        var key = detail.Size.Size + "~" + item.Color;

                        if (detailData.ContainsKey(key))
                        {
                            detailData[key] += detail.Quantity;
                        }
                        else
                        {
                            detailData.Add(key, detail.Quantity);
                        }

                        if (remarks.ContainsKey(item.Color))
                        {
                            var dup = remarks.Where(a => a.Value == item.DesignColor && a.Key == item.Color).FirstOrDefault();
                            if (dup.Value == null)
                            {
                                var decol = remarks[item.Color].Split(", ").ToList();
                                remarks[item.Color] = decol.Where(a => a == item.DesignColor).FirstOrDefault() == null ? remarks[item.Color] + ", " + item.DesignColor : remarks[item.Color];
                            }
                        }
                        else
                        {
                            remarks.Add(item.Color, item.DesignColor);
                        }
                    }
                }
                else
                {
                    if (!sizes.Contains(item.Size.Size))
                        sizes.Add(item.Size.Size);
                    if (!colors.Contains(item.Color))
                        colors.Add(item.Color);

                    var key = item.Size.Size + "~" + item.Color;

                    if (detailData.ContainsKey(key))
                    {
                        detailData[key] += item.Quantity;
                    }
                    else
                    {
                        detailData.Add(key, item.Quantity);
                    }

                    if (remarks.ContainsKey(item.Color))
                    {
                        var dup = remarks.Where(a => a.Value == item.DesignColor && a.Key == item.Color).FirstOrDefault();
                        if (dup.Value == null)
                        {
                            var decol = remarks[item.Color].Split(", ").ToList();
                            remarks[item.Color] = decol.Where(a => a == item.DesignColor).FirstOrDefault() == null ? remarks[item.Color] + ", " + item.DesignColor : remarks[item.Color];
                        }
                    }
                    else
                    {
                        remarks.Add(item.Color, item.DesignColor);
                    }
                }

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

                cellCenter.Phrase = new Phrase(remarks[i].ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
            }

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);
            #endregion

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            cellCenterTopNoBorder.Phrase = new Paragraph("Diterima Oleh\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Diberikan Oleh\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph($"Dicetak : {DateTimeOffset.Now.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID"))}", normal_font);
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
