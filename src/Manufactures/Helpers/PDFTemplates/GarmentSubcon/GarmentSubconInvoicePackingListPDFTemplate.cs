using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.PDFTemplates.GarmentSubcon
{
    public class GarmentSubconInvoicePackingListPDFTemplate
    {
        public static MemoryStream Generate(GarmentSubconInvoicePackingListDto garmentSubconInvoicePacking)
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
            cellCenterNoBorder.Phrase = new Paragraph("INVOICE/PACKINGLIST\n\n\n", bold_font);
            tableTiltle.AddCell(cellCenterNoBorder);

            PdfPCell cellTitle = new PdfPCell(tableTiltle);
            tableTiltle.ExtendLastRow = false;
            document.Add(tableTiltle);

            PdfPTable tableHeader = new PdfPTable(1);
            tableHeader.SetWidths(new float[] { 1f });

            PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft.AddElement(new Phrase("No Invoice    : "+garmentSubconInvoicePacking.InvoiceNo, bold_font));
            cellHeaderContentLeft.AddElement(new Phrase("Tanggal        : " + garmentSubconInvoicePacking.Date.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("No Subcon   : " + garmentSubconInvoicePacking.Supplier.Name ,normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("                      " + garmentSubconInvoicePacking.Supplier.Address, normal_font));
            tableHeader.AddCell(cellHeaderContentLeft);


            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            #region content

            PdfPTable tableContent = new PdfPTable(5);
            tableContent.SetWidths(new float[] { 1f, 3f, 3f, 3f, 2.5f });

            cellCenter.Phrase = new Phrase("No", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Description", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            tableContent.AddCell(cellCenter);
            //cellCenter.Phrase = new Phrase("Satuan", bold_font);
            //tableContent.AddCell(cellCenter);
            //cellCenter.Phrase = new Phrase("----", bold_font);
            //tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Hrg Satuan Rp", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jml Harga Rp", bold_font);
            tableContent.AddCell(cellCenter);

            int indexItem = 0;

            if (garmentSubconInvoicePacking.Items.Count == 0)
            {
                cellCenter.Phrase = new Phrase(indexItem + 1.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase(garmentSubconInvoicePacking.Remark, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase("", normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                //cellCenter.Phrase = new Phrase("", normal_font);
                //cellCenter.Rowspan = 1;
                //tableContent.AddCell(cellCenter);

                //cellCenter.Phrase = new Phrase("-", normal_font);
                //cellCenter.Rowspan = 1;
                //tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase("Rp " + (garmentSubconInvoicePacking.CIF), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);

                cellCenter.Phrase = new Phrase("Rp " + (garmentSubconInvoicePacking.CIF * 0), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
            }
            else
            { 
                //List<string, string> iwak = new List<string, string>;
                foreach (var a in garmentSubconInvoicePacking.Items)
                {
                    cellCenter.Phrase = new Phrase(indexItem + 1.ToString(), normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(garmentSubconInvoicePacking.Remark, normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase(a.Quantity.ToString(), normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    //cellCenter.Phrase = new Phrase(a.Uom.Unit, normal_font);
                    //cellCenter.Rowspan = 1;
                    //tableContent.AddCell(cellCenter);

                    //cellCenter.Phrase = new Phrase("-", normal_font);
                    //cellCenter.Rowspan = 1;
                    //tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase("Rp " + a.CIF, normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                    cellCenter.Phrase = new Phrase("Rp " + a.CIF * a.Quantity, normal_font);
                    cellCenter.Rowspan = 1;
                    tableContent.AddCell(cellCenter);

                }
            }


            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);

            #endregion

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(1);

            cellCenterTopNoBorder.Phrase = new Paragraph("NW  : " + garmentSubconInvoicePacking.NW + " KG", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("GW  : "+garmentSubconInvoicePacking.GW+" KG", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(" ", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);

            cellLeftNoBorder.Phrase = new Paragraph("(AMBASSADOR GARMINDO)", normal_font);
            tableSignature.AddCell(cellLeftNoBorder);

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
