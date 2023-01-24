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
    public class GarmentServiceSubconCuttingPDFTemplate
    {
        public static MemoryStream Generate(GarmentServiceSubconCuttingDto garmentSubconCutting)
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
            cellCenterNoBorder.Phrase = new Paragraph("SUBCON JASA KOMPONEN\n\n\n", bold_font);
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
            cellHeaderContentCenter.AddElement(new Paragraph("Tanggal Subcon  : " + garmentSubconCutting.SubconDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            cellHeaderContentCenter.AddElement(new Paragraph("No Subcon          : " + garmentSubconCutting.SubconNo, normal_font));
            tableHeader.AddCell(cellHeaderContentCenter);

            PdfPCell cellHeaderContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentRight.AddElement(new Phrase("Jenis Subcon           : " + garmentSubconCutting.SubconType, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("Unit Asal                  : " + garmentSubconCutting.Unit.Name, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("Buyer                       : " + garmentSubconCutting.Buyer.Name, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("Jumlah Kemasan     : " + garmentSubconCutting.QtyPacking, normal_font));
            cellHeaderContentRight.AddElement(new Phrase("Satuan Kemasan     : " + garmentSubconCutting.Uom.Unit, normal_font));

            tableHeader.AddCell(cellHeaderContentRight);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            List<GarmentSubconCuttingItemVM> itemData = new List<GarmentSubconCuttingItemVM>();
            List<string> listSize = new List<string>();
            Dictionary<string, string> sizeData = new Dictionary<string, string>();

            foreach (var item in garmentSubconCutting.Items)
            {
                foreach (var detail in item.Details)
                {
                    foreach (var size in detail.Sizes)
                    {
                        var data = itemData.FirstOrDefault(x => x.RoNo == item.RONo && x.DesignColor == detail.DesignColor && x.Color == size.Color);
                        if(data == null)
                        {
                            List<GarmentSubconCuttingSize> sizes = new List<GarmentSubconCuttingSize>();
                            GarmentSubconCuttingSize garmentSubconCuttingSize = new GarmentSubconCuttingSize();

                            GarmentSubconCuttingItemVM garmentSubconCuttingItemVM = new GarmentSubconCuttingItemVM();

                            garmentSubconCuttingItemVM.RoNo = item.RONo;
                            garmentSubconCuttingItemVM.Article = item.Article;
                            garmentSubconCuttingItemVM.Comodity = item.Comodity.Code + " - " + item.Comodity.Name;

                            garmentSubconCuttingItemVM.DesignColor = detail.DesignColor;

                            garmentSubconCuttingSize.Size = size.Size.Size;
                            garmentSubconCuttingSize.Quantity = size.Quantity;
                            garmentSubconCuttingItemVM.Unit = size.Uom.Unit;
                            garmentSubconCuttingItemVM.Color = size.Color;

                            sizes.Add(garmentSubconCuttingSize);
                            listSize.Add(garmentSubconCuttingSize.Size);
                            garmentSubconCuttingItemVM.Sizes = sizes;
                            itemData.Add(garmentSubconCuttingItemVM);
                        }
                        else
                        {
                            var existingSize = data.Sizes.FirstOrDefault(x => x.Size == size.Size.Size);
                            if (existingSize == null)
                            {
                                GarmentSubconCuttingSize garmentSubconCuttingSize = new GarmentSubconCuttingSize();

                                garmentSubconCuttingSize.Size = size.Size.Size;
                                garmentSubconCuttingSize.Quantity = size.Quantity;

                                listSize.Add(garmentSubconCuttingSize.Size);
                                data.Sizes.Add(garmentSubconCuttingSize);
                            }
                            else
                            {
                                existingSize.Quantity += size.Quantity;
                            }
                        }
                    }
                }
            }

            listSize.Sort();
            #region content

            PdfPTable tableContent = new PdfPTable(7 + listSize.Count());
            List<float> widths = new List<float>();
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(6f);
            widths.Add(4f);

            foreach (var s in listSize)
            {
                widths.Add(2f);
            }

            widths.Add(3f);
            widths.Add(3f);
            widths.Add(3f);

            tableContent.SetWidths(widths.ToArray());

            cellCenter.Phrase = new Phrase("R0", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Artikel", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Komoditi", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Size", bold_font);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = listSize.Count();
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            cellCenter.Colspan = 1;
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Warna", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);

            foreach (var s in listSize)
            {
                cellCenter.Phrase = new Phrase(s, bold_font);
                cellCenter.Rowspan = 1;
                cellCenter.Colspan = 1;
                tableContent.AddCell(cellCenter);
            }

            double grandTotal = 0;
            foreach (var i in itemData)
            {
                cellCenter.Phrase = new Phrase(i.RoNo, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Article, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Comodity, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.DesignColor, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                double total = 0;
                foreach (var s in listSize)
                {
                    var selectedSize = i.Sizes.FirstOrDefault(x => x.Size == s);
                    if (selectedSize != null)
                    {
                        cellCenter.Phrase = new Phrase(selectedSize.Quantity.ToString(), normal_font);
                        cellCenter.Rowspan = 1;
                        tableContent.AddCell(cellCenter);
                        total += selectedSize.Quantity;
                        grandTotal += total;
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
                cellCenter.Phrase = new Phrase(i.Unit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Color, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
            }

            cellRight.Phrase = new Phrase("TOTAL", bold_font);
            cellRight.Rowspan = 1;
            cellRight.Colspan = 4 + listSize.Count();
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

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            cellCenterTopNoBorder.Phrase = new Paragraph("Penerima\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Bag. Cutting\n\n\n\n\n\n\n\n(                                   )", normal_font);
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

    public class GarmentSubconCuttingItemVM
    {
        public string RoNo { get; set; }
        public string Article { get; set; }
        public string Comodity { get; set; }
        public string DesignColor { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public List<GarmentSubconCuttingSize> Sizes { get; set; }

    }

    public class GarmentSubconCuttingSize
    {
        public string Size { get; set; }
        public double Quantity { get; set; }
    }
}
