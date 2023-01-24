using Manufactures.Dtos.GarmentSubcon;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Manufactures.Helpers.PDFTemplates
{
    public class GarmentServiceSubconSewingPDFTemplate
    {
        public static MemoryStream Generate(GarmentServiceSubconSewingDto garmentSubconSewing)
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
            PdfPTable tableTiltle = new PdfPTable(1);
            cellCenterNoBorder.Phrase = new Paragraph("SUBCON JASA GARMENT WASH\n\n\n", bold_font);
            tableTiltle.AddCell(cellCenterNoBorder); 

            PdfPCell cellTitle = new PdfPCell(tableTiltle);
            tableTiltle.ExtendLastRow = false;
            document.Add(tableTiltle);

            PdfPTable tableHeader = new PdfPTable(3);
            tableHeader.SetWidths(new float[] { 1f, 1f, 1f });

            PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft.AddElement(new Phrase("PT EFRATA RETAILINDO", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("SUKOHARJO", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("BANARAN, GROGOL", normal_font));
            tableHeader.AddCell(cellHeaderContentLeft);

            PdfPCell cellHeaderContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentCenter.AddElement(new Paragraph("Tanggal Subcon    : " + garmentSubconSewing.ServiceSubconSewingDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            cellHeaderContentCenter.AddElement(new Paragraph("No Subcon            : " + garmentSubconSewing.ServiceSubconSewingNo, normal_font));
            tableHeader.AddCell(cellHeaderContentCenter);

            PdfPCell cellHeaderContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentRight.AddElement(new Phrase("Buyer : " + garmentSubconSewing.Buyer.Name, normal_font));

            tableHeader.AddCell(cellHeaderContentRight);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            List<GarmentSubconSewingItemVM> itemData = new List<GarmentSubconSewingItemVM>();

            foreach (var item in garmentSubconSewing.Items)
            {
                foreach (var detail in item.Details)
                {
                    var data = itemData.FirstOrDefault(x => x.RoNo == item.RONo && x.DesignColor == detail.DesignColor && x.Color == detail.Color);

                    GarmentSubconSewingItemVM garmentSubconSewingItemVM = new GarmentSubconSewingItemVM();
                    garmentSubconSewingItemVM.RoNo = item.RONo;
                    garmentSubconSewingItemVM.Article = item.Article;
                    garmentSubconSewingItemVM.Comodity = item.Comodity.Code + " - " + item.Comodity.Name;

                    if (data == null)
                    {
                        garmentSubconSewingItemVM.DesignColor = detail.DesignColor;
                        garmentSubconSewingItemVM.Color = detail.Color;
                        garmentSubconSewingItemVM.Unit = detail.Unit.Code;
                        garmentSubconSewingItemVM.Quantity = detail.Quantity;
                        garmentSubconSewingItemVM.UomUnit = detail.Uom.Unit;
                        garmentSubconSewingItemVM.Remark = detail.Remark;
                        garmentSubconSewingItemVM.UnitName = item.Unit.Name;
                        itemData.Add(garmentSubconSewingItemVM);
                    }
                    else
                    {
                        data.Quantity += detail.Quantity;
                    }
                }
            }

            #region content

            PdfPTable tableContent = new PdfPTable(10);
            List<float> widths = new List<float>();
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(6f);
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(3f);
            widths.Add(3f);
            widths.Add(3f);
            widths.Add(4f);

            tableContent.SetWidths(widths.ToArray());

            cellCenter.Phrase = new Phrase("R0", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Area", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Artikel", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Komoditi", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Warna", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Design Warna", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Unit", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan", bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);

            double grandTotal = 0;
            foreach (var i in itemData)
            {
                cellCenter.Phrase = new Phrase(i.RoNo, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.UnitName, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Article, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Comodity, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Color, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.DesignColor, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Unit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Quantity.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.UomUnit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Remark, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                grandTotal += i.Quantity;
            }

            cellRight.Phrase = new Phrase("TOTAL", bold_font);
            cellRight.Rowspan = 1;
            cellRight.Colspan = 5;
            tableContent.AddCell(cellRight);
            cellCenter.Phrase = new Phrase(grandTotal.ToString(), bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("", normal_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("", normal_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);
            #endregion
            Paragraph qtyUomPacking = new Paragraph("Jumlah Kemasan : " + garmentSubconSewing.QtyPacking + "      " + "Satuan Kemasan : " + garmentSubconSewing.UomUnit, normal_font);
            qtyUomPacking.SpacingAfter = 5f;
            document.Add(qtyUomPacking);
            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            cellCenterTopNoBorder.Phrase = new Paragraph("Penerima\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Bag. Sewing\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph($"\nDicetak : {DateTimeOffset.Now.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID"))}", normal_font);
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

    public class GarmentSubconSewingItemVM
    {
        public string RoNo { get; set; }
        public string Article { get; set; }
        public string Comodity { get; set; }
        public string DesignColor { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string UomUnit { get; set; }
        public string Remark { get; set; }
        public string UnitName { get; set; }
        public string Color { get; set; }
    }
}
