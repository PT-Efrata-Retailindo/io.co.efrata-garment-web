using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Helpers.PDFTemplates.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutPDFTemplate
    {
        public static MemoryStream Generate(GarmentSubconDeliveryLetterOutDto garmentSubconDLOut, string supplier)
        {
            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell cellRightNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellJustifyNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
            PdfPCell cellJustifyAllNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL };

            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };


            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A5.Rotate(), 10, 10, 100, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.PageEvent = new GarmentLocalCoverLetterPdfTemplatePageEvent(garmentSubconDLOut);
            document.Open();

            Paragraph date = new Paragraph("Tanggal " + garmentSubconDLOut.DLDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            date.Alignment = Element.ALIGN_RIGHT;

            document.Add(date);

            #region Identity


            PdfPTable tableIdentity = new PdfPTable(4);
            tableIdentity.SetWidths(new float[] { 2f, 7f, 3f, 5f });
            PdfPCell cellIdentityContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };

            cellIdentityContentLeft.Phrase = new Phrase("Tujuan", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + supplier, normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING" ? "NO. PO" : "", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING" ? ": " + garmentSubconDLOut.PONo : "", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("No. PO", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconDLOut.EPONo, normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING" ? "Dasar Pengeluaran" : "", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING" ? ": " + garmentSubconDLOut.UENNo : "", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            cellIdentityContentLeft.Phrase = new Phrase("Kategori Subcon", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase(": " + garmentSubconDLOut.SubconCategory, normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);
            cellIdentityContentLeft.Phrase = new Phrase("", normal_font);
            tableIdentity.AddCell(cellIdentityContentLeft);

            PdfPCell cellIdentity = new PdfPCell(tableIdentity);
            tableIdentity.ExtendLastRow = false;
            tableIdentity.SpacingAfter = 10f;
            tableIdentity.SpacingBefore = 10f;
            document.Add(tableIdentity);

            #endregion

            #region TableContent
            double total = 0;
            if (garmentSubconDLOut.SubconCategory == "SUBCON JASA GARMENT WASH")
            {
                PdfPTable tableContent = new PdfPTable(9);
                tableContent.SetWidths(new float[] { 1.5f, 3f, 3f, 3f, 3f, /*3f,*/ 2.5f, 2.5f, 2.5f, 2.5f });

                cellCenter.Phrase = new Phrase("No", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Packing List", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("RO", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Article", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Komoditi", bold_font);
                tableContent.AddCell(cellCenter);
                //cellCenter.Phrase = new Phrase("Warna", bold_font);
                //tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Jumlah Kemasan", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan Kemasan", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Quantity", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan", bold_font);
                tableContent.AddCell(cellCenter);


                //for (int indexItem = 0; indexItem < garmentSubconDLOut.Items.Count; indexItem++)
                //{
                //    GarmentSubconDeliveryLetterOutItemDto item = garmentSubconDLOut.Items[indexItem];

                //    cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                //    tableContent.AddCell(cellCenter);

                int indexItem = 0;
                foreach (var DLItem in garmentSubconDLOut.Items)
                {
                    var cols = DLItem.SubconSewing.Items.Count;
                    foreach (var item in DLItem.SubconSewing.Items)
                    {
                        if (cols > 0)
                        {
                            cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                            cellCenter.Rowspan = cols;
                            cellCenter.VerticalAlignment = Element.ALIGN_TOP;
                            tableContent.AddCell(cellCenter);
                            indexItem++;

                            cellLeft.Phrase = new Phrase("GARMENT WASH", normal_font);
                            tableContent.AddCell(cellLeft);

                            cellLeft.Phrase = new Phrase($"{DLItem.SubconNo}", normal_font);
                            tableContent.AddCell(cellLeft);

                            cellCenter.Phrase = new Phrase($"{item.Article}", normal_font);
                            tableContent.AddCell(cellCenter);

                            cellCenter.Phrase = new Phrase($"{item.Comodity.Name}", normal_font);
                            tableContent.AddCell(cellCenter);

                            //cellCenter.Phrase = new Phrase($"{DLItem.DesignColor}", normal_font);
                            //tableContent.AddCell(cellCenter);
                        }

                        cellRight.Phrase = new Phrase($"{DLItem.QtyPacking}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase($"{DLItem.UomSatuanUnit}", normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{DLItem.Quantity}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase("PCS", normal_font);
                        tableContent.AddCell(cellLeft);

                        total += DLItem.Quantity;
                    }
                }

                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 8;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase("PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);
                //cellLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT" || garmentSubconDLOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL" ? "MTR" : "PCS", bold_font);
                //cellLeft.Colspan = 1;
                //tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }
            else if (garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING")
            {
                PdfPTable tableContent = new PdfPTable(5);
                tableContent.SetWidths(new float[] { 1.5f, 8f, 6f, 2.5f, 2.5f });

                cellCenter.Phrase = new Phrase("No", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Kode/Nama Barang", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Keterangan Lain", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Quantity", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan", bold_font);
                tableContent.AddCell(cellCenter);

                for (int indexItem = 0; indexItem < garmentSubconDLOut.Items.Count; indexItem++)
                {
                    GarmentSubconDeliveryLetterOutItemDto item = garmentSubconDLOut.Items[indexItem];

                    cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                    tableContent.AddCell(cellCenter);

                    cellLeft.Phrase = new Phrase(item.Product.Code + "/" + item.Product.Name, normal_font);
                    tableContent.AddCell(cellLeft);

                    cellLeft.Phrase = new Phrase(item.DesignColor, normal_font);
                    tableContent.AddCell(cellLeft);

                    cellRight.Phrase = new Phrase($"{item.Quantity}", normal_font);
                    tableContent.AddCell(cellRight);

                    cellLeft.Phrase = new Phrase("PCS", normal_font);
                    tableContent.AddCell(cellLeft);

                    total += item.Quantity;
                }

                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 3;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase("PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }

            else if (garmentSubconDLOut.SubconCategory != "SUBCON SEWING")
            {
                PdfPTable tableContent = new PdfPTable(7);
                tableContent.SetWidths(new float[] { 1.5f, 8f, 6f, 2.5f, 2.5f, 2.5f, 2.5f });

                if (garmentSubconDLOut.SubconCategory == "SUBCON JASA KOMPONEN")
                {
                    cellCenter.Phrase = new Phrase("No", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Nama/Jenis Barang", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Packing List", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Jumlah Kemasan", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Satuan Kemasan", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Quantity", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Satuan", bold_font);
                    tableContent.AddCell(cellCenter);


                    for (int indexItem = 0; indexItem < garmentSubconDLOut.Items.Count; indexItem++)
                    {
                        GarmentSubconDeliveryLetterOutItemDto item = garmentSubconDLOut.Items[indexItem];

                        cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                        tableContent.AddCell(cellCenter);

                        var code = item.SubconNo.Substring(3);
                        var type = code.StartsWith("PL") ? "PLISKET" : code.StartsWith("B") ? "BORDIR" : code.StartsWith("PR") ? "PRINT" : "OTHER";
                        cellLeft.Phrase = new Phrase(type, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellLeft.Phrase = new Phrase($"{item.SubconNo}", normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{item.QtyPacking}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase($"{item.UomSatuanUnit}", normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{item.Quantity}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase("PCS", normal_font);
                        tableContent.AddCell(cellLeft);

                        total += item.Quantity;
                    }
                }
                //else if (garmentSubconDLOut.SubconCategory == "SUBCON CUTTING SEWING")
                //{

                //    cellCenter.Phrase = new Phrase("No", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("Kode/Nama Barang", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("Keterangan Lain", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("Quantity", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("Satuan", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("", bold_font);
                //    tableContent.AddCell(cellCenter);
                //    cellCenter.Phrase = new Phrase("", bold_font);
                //    tableContent.AddCell(cellCenter);

                //    for (int indexItem = 0; indexItem < garmentSubconDLOut.Items.Count; indexItem++)
                //    {
                //        GarmentSubconDeliveryLetterOutItemDto item = garmentSubconDLOut.Items[indexItem];

                //        cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                //        tableContent.AddCell(cellCenter);

                //        cellLeft.Phrase = new Phrase(item.Product.Code + "/" + item.Product.Name, normal_font);
                //        tableContent.AddCell(cellLeft);

                //        cellLeft.Phrase = new Phrase(item.DesignColor, normal_font);
                //        tableContent.AddCell(cellLeft);

                //        cellRight.Phrase = new Phrase($"{item.Quantity}", normal_font);
                //        tableContent.AddCell(cellRight);

                //        cellLeft.Phrase = new Phrase("PCS", normal_font);
                //        tableContent.AddCell(cellLeft);

                //        //cellLeft.Phrase = new Phrase("", normal_font);
                //        //tableContent.AddCell(cellLeft);

                //        //cellLeft.Phrase = new Phrase("", normal_font);
                //        //tableContent.AddCell(cellLeft);

                //        total += item.Quantity;
                //    }
                //}
                else if (garmentSubconDLOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT" || garmentSubconDLOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                {
                    cellCenter.Phrase = new Phrase("No", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Packing List", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Nama/Jenis Barang", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Jumlah Kemasan", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Satuan Kemasan", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Quantity", bold_font);
                    tableContent.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase("Satuan", bold_font);
                    tableContent.AddCell(cellCenter);


                    for (int indexItem = 0; indexItem < garmentSubconDLOut.Items.Count; indexItem++)
                    {
                        GarmentSubconDeliveryLetterOutItemDto item = garmentSubconDLOut.Items[indexItem];

                        cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                        tableContent.AddCell(cellCenter);

                        cellLeft.Phrase = new Phrase(item.SubconNo, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellLeft.Phrase = new Phrase(garmentSubconDLOut.Remark, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{item.QtyPacking}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase(item.UomSatuanUnit, normal_font);
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{item.Quantity}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase("MTR", normal_font);
                        tableContent.AddCell(cellLeft);

                        total += item.Quantity;
                    }
                }
                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 5;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase(garmentSubconDLOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT" || garmentSubconDLOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL" ? "MTR" : "PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }

            else
            {
                PdfPTable tableContent = new PdfPTable(6);
                tableContent.SetWidths(new float[] { 1.5f, 3f, 3f, 3f, 2.5f, 2.5f });

                cellCenter.Phrase = new Phrase("No", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("No RO", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("No PO", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Warna", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Quantity", bold_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase("Satuan", bold_font);
                tableContent.AddCell(cellCenter);
                int indexItem = 0;
                foreach (var DLItem in garmentSubconDLOut.Items)
                {
                    var cols = DLItem.SubconCutting.Items.Count;
                    foreach (var item in DLItem.SubconCutting.Items)
                    {
                        if (cols > 0)
                        {
                            cellCenter.Phrase = new Phrase((indexItem + 1).ToString(), normal_font);
                            cellCenter.Rowspan = cols;
                            cellCenter.VerticalAlignment = Element.ALIGN_TOP;
                            tableContent.AddCell(cellCenter);
                            indexItem++;

                            cellLeft.Phrase = new Phrase(DLItem.RONo, normal_font);
                            cellLeft.Rowspan = cols;
                            cellCenter.VerticalAlignment = Element.ALIGN_TOP;
                            tableContent.AddCell(cellLeft);

                            cellLeft.Phrase = new Phrase(DLItem.POSerialNumber, normal_font);
                            tableContent.AddCell(cellLeft);
                            cols = 0;
                        }

                        cellLeft.Phrase = new Phrase(item.DesignColor, normal_font);
                        cellLeft.Rowspan = 1;
                        tableContent.AddCell(cellLeft);

                        cellRight.Phrase = new Phrase($"{item.TotalCuttingOut}", normal_font);
                        tableContent.AddCell(cellRight);

                        cellLeft.Phrase = new Phrase("PCS", normal_font);
                        tableContent.AddCell(cellLeft);

                        total += item.TotalCuttingOut;
                    }
                }

                cellLeft.Phrase = new Phrase("TOTAL", bold_font);
                cellLeft.Colspan = 4;
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase($"{total}", bold_font);
                cellRight.Colspan = 1;
                tableContent.AddCell(cellRight);
                cellLeft.Phrase = new Phrase("PCS", bold_font);
                cellLeft.Colspan = 1;
                tableContent.AddCell(cellLeft);

                PdfPCell cellContent = new PdfPCell(tableContent);
                tableContent.ExtendLastRow = false;
                tableContent.SpacingAfter = 5f;
                document.Add(tableContent);
            }

            #endregion
            if (garmentSubconDLOut.SubconCategory != "SUBCON BB FABRIC WASH/PRINT" && garmentSubconDLOut.SubconCategory != "SUBCON BB SHRINKAGE/PANEL")
            {
                Paragraph remark = new Paragraph("Keterangan : " + garmentSubconDLOut.Remark, normal_font);
                document.Add(remark);
            }

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);

            PdfPCell cellSignatureContent = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            cellSignatureContent.Phrase = new Phrase("Diterima Oleh\n\n\n\n\n(                              )", normal_font);
            tableSignature.AddCell(cellSignatureContent);
            cellSignatureContent.Phrase = new Phrase("Hormat Kami,\n\n\n\n\n(                              )", normal_font);
            tableSignature.AddCell(cellSignatureContent);


            PdfPCell cellSignature = new PdfPCell(tableSignature); // dont remove
            tableSignature.ExtendLastRow = false;
            tableSignature.SpacingBefore = 10f;
            document.Add(tableSignature);

            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        class GarmentLocalCoverLetterPdfTemplatePageEvent : iTextSharp.text.pdf.PdfPageEventHelper
        {
            private GarmentSubconDeliveryLetterOutDto dto;

            public GarmentLocalCoverLetterPdfTemplatePageEvent(GarmentSubconDeliveryLetterOutDto dto)
            {
                this.dto = dto;
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);

                float height = writer.PageSize.Height, width = writer.PageSize.Width;
                float marginLeft = document.LeftMargin, marginTop = document.TopMargin, marginRight = document.RightMargin - 10;

                cb.SetFontAndSize(bf, 6);

                #region LEFT

                var branchOfficeY = height - marginTop + 35;

                byte[] imageByteDL = Convert.FromBase64String(Base64ImageStrings.LOGO_AG_58_58);
                Image imageDL = Image.GetInstance(imageByteDL);
                imageDL.SetAbsolutePosition(marginLeft, branchOfficeY);
                cb.AddImage(imageDL, inlineImage: true);
                //for (int i = 0; i < branchOffices.Length; i++)
                //{
                //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, branchOffices[i], marginLeft, branchOfficeY - 10 - (i * 8), 0);
                //}

                #endregion

                #region CENTER

                var headOfficeX = width / 2 - 200;
                var headOfficeY = height - marginTop + 75;

                byte[] imageByte = Convert.FromBase64String(Base64ImageStrings.LOGO_NAME);
                Image image = Image.GetInstance(imageByte);
                if (image.Width > 160)
                {
                    float percentage = 0.0f;
                    percentage = 160 / image.Width;
                    image.ScalePercent(percentage * 100);
                }
                image.SetAbsolutePosition(headOfficeX, headOfficeY);
                cb.AddImage(image, inlineImage: true);

                string[] headOffices = {
                    "Head Office : JL. MERAPI NO. 23, Banaran, Grogol, Sukoharjo 57552",
                    "Central Java, Indonesia",
                    "TELP.: (+62 271) 732888                  PO BOX 166 Solo, 57100" ,
                    "Website : www.ambassadorgarmindo.com" ,
                };
                for (int i = 0; i < headOffices.Length; i++)
                {
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, headOffices[i], headOfficeX, headOfficeY - image.ScaledHeight - (i * 10), 0);
                }

                #endregion

                #region LINE

                cb.MoveTo(marginLeft - 10, height - marginTop + 25);
                cb.LineTo(width - marginRight, height - marginTop + 25);
                cb.Stroke();

                #endregion

                #region TITLE

                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED), 10);

                var titleY = height - marginTop;
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "SURAT JALAN SUBCON", width / 2, titleY + 10, 0);

                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED), 8);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "NO : " + dto.DLNo, width / 2, titleY, 0);

                #endregion

                cb.EndText();
            }
        }
    }
}
