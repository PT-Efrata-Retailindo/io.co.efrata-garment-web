using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Manufactures.Helpers.PDFTemplates
{
    public class GarmentSampleRequestPDFTemplate
    {
        public static MemoryStream Generate(GarmentSampleRequestDto garmentSampleRequest)
        {
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);
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

            string titleCaption;

            if (garmentSampleRequest.IsReceived)
            {
                titleCaption = "SURAT PERMINTAAN PEMBUATAN SAMPLE";
            } else
            {
                titleCaption = "DRAFT SURAT PERMINTAAN PEMBUATAN SAMPLE";
            }

            Paragraph title = new Paragraph(titleCaption, header_font);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10f;
            document.Add(title);

            #region Header
            Paragraph caption = new Paragraph("", normal_font);
            caption.Alignment = Element.ALIGN_RIGHT;
            caption.SpacingAfter = 10f;
            document.Add(caption);

            PdfPTable tableHeader = new PdfPTable(2);
            List<float> widths = new List<float>();
            widths.Add(0.25f);
            widths.Add(1f);

            tableHeader.SetWidths(widths.ToArray());

            cellLeftNoBorder.Phrase = new Phrase("RO Sample", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.RONoSample, normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("RO CC", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.RONoCC, normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("No Surat Sample", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.SampleRequestNo, normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Kategori Sample", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.SampleCategory, normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Tipe Sample", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.SampleTo, normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Tanggal Surat Sample", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Date.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd-MMM-yyyy", new CultureInfo("id-ID")), normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Kepada", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": Kabag Sample Room", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("cc", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": QC", normal_font);
            tableHeader.AddCell(cellLeftNoBorder);

            PdfPCell cellHeader = new PdfPCell(tableHeader); // dont remove
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 10f;
            document.Add(tableHeader);

            Paragraph sambutan = new Paragraph("Dengan hormat,\nMohon dibuatkan sample untuk dikirim ke buyer sbb :", normal_font);
            sambutan.SpacingAfter = 10f;
            document.Add(sambutan);

            PdfPTable tableHeader2 = new PdfPTable(2);

            tableHeader2.SetWidths(widths.ToArray());

            cellLeftNoBorder.Phrase = new Phrase("Buyer", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Buyer.Code + " - " + garmentSampleRequest.Buyer.Name, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Jenis Sample", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.SampleType, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Komoditi", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Comodity.Code + " - " + garmentSampleRequest.Comodity.Name, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Packing", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Packing, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Tanggal Kirim", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.SentDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Terlampir", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Attached, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("PO Buyer", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.POBuyer, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            cellLeftNoBorder.Phrase = new Phrase("Keterangan : ", normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);
            cellLeftNoBorder.Phrase = new Phrase(": " + garmentSampleRequest.Remark, normal_font);
            tableHeader2.AddCell(cellLeftNoBorder);

            PdfPCell cellHeader2 = new PdfPCell(tableHeader2); // dont remove
            tableHeader2.ExtendLastRow = false;
            tableHeader2.SpacingAfter = 20f;
            document.Add(tableHeader2);

            #endregion

            List<GarmentSampleRequestProductVM> products = new List<GarmentSampleRequestProductVM>();
            List<GarmentSampleRequestSpecificationVM> specifications = new List<GarmentSampleRequestSpecificationVM>();

            foreach (var product in garmentSampleRequest.SampleProducts)
            {
                GarmentSampleRequestProductVM garmentSampleRequestProductVM = new GarmentSampleRequestProductVM();
                garmentSampleRequestProductVM.Style = product.Style;
                garmentSampleRequestProductVM.Size = product.Size.Size;
                garmentSampleRequestProductVM.Color = product.Color;
                garmentSampleRequestProductVM.Fabric = product.Fabric;
                garmentSampleRequestProductVM.SizeDescription = product.SizeDescription;
                garmentSampleRequestProductVM.Quantity = product.Quantity;
                products.Add(garmentSampleRequestProductVM);
            }

            foreach (var specification in garmentSampleRequest.SampleSpecifications)
            {
                GarmentSampleRequestSpecificationVM garmentSampleRequestSpecificationVM = new GarmentSampleRequestSpecificationVM();
                garmentSampleRequestSpecificationVM.Inventory = specification.Inventory;
                garmentSampleRequestSpecificationVM.SpecificationDetail = specification.SpecificationDetail;
                garmentSampleRequestSpecificationVM.Quantity = specification.Quantity;
                garmentSampleRequestSpecificationVM.Uom = specification.Uom.Unit;
                garmentSampleRequestSpecificationVM.Remark = specification.Remark;
                specifications.Add(garmentSampleRequestSpecificationVM);
            }

            #region content
            Paragraph detailBarang = new Paragraph("Detail Barang", bold_font);
            detailBarang.SpacingAfter = 10f;
            document.Add(detailBarang);

            PdfPTable tableContent = new PdfPTable(6);
            List<float> widths2 = new List<float>();
            widths2.Add(1f);
            widths2.Add(1f);
            widths2.Add(1f);
            widths2.Add(1f);
            widths2.Add(1f);

            List<float> widths3 = new List<float>();
            widths3.Add(1f);
            widths3.Add(1f);
            widths3.Add(1f);
            widths3.Add(1f);
            widths3.Add(1f);
            widths3.Add(1f);

            tableContent.SetWidths(widths3.ToArray());

            cellCenter.Phrase = new Phrase("Style", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Size", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Colour", bold_font); 
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Fabric", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan Size", bold_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Qty", bold_font);
            tableContent.AddCell(cellCenter);

            double grandTotal = 0;
            foreach (var i in products)
            {
                cellCenter.Phrase = new Phrase(i.Style, normal_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Size, normal_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Color, normal_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Fabric, normal_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.SizeDescription, normal_font);
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Quantity.ToString(), normal_font);
                tableContent.AddCell(cellCenter);
                grandTotal += i.Quantity;
            }

            cellRight.Phrase = new Phrase("TOTAL", bold_font);
            cellRight.Colspan = 5;
            tableContent.AddCell(cellRight);
            cellCenter.Phrase = new Phrase(grandTotal.ToString(), bold_font);
            tableContent.AddCell(cellCenter);

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);

            Paragraph kelengkapanSample = new Paragraph("Kelengkapan Sample", bold_font);
            kelengkapanSample.SpacingAfter = 10f;
            document.Add(kelengkapanSample);

            PdfPTable tableContent2 = new PdfPTable(5);

            tableContent2.SetWidths(widths2.ToArray());

            cellCenter.Phrase = new Phrase("Inventory", bold_font);
            tableContent2.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Detail Spesifikasi", bold_font);
            tableContent2.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Quantity", bold_font);
            tableContent2.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            tableContent2.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Keterangan", bold_font);
            tableContent2.AddCell(cellCenter);

            foreach (var i in specifications)
            {
                cellLeft.Phrase = new Phrase(i.Inventory, normal_font);
                tableContent2.AddCell(cellLeft);
                cellLeft.Phrase = new Phrase(i.SpecificationDetail, normal_font);
                tableContent2.AddCell(cellLeft);
                cellCenter.Phrase = new Phrase(i.Quantity.ToString(), normal_font);
                tableContent2.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Uom, normal_font);
                tableContent2.AddCell(cellCenter);
                cellLeft.Phrase = new Phrase(i.Remark, normal_font);
                tableContent2.AddCell(cellLeft);
            }

            PdfPCell cellContent2 = new PdfPCell(tableContent2); // dont remove
            tableContent2.ExtendLastRow = false;
            tableContent2.SpacingAfter = 20f;
            document.Add(tableContent2);
            #endregion

            #region IMAGE
            var countImage = 0;
            byte[] image;
            if (garmentSampleRequest.ImagesFile != null)
            {
                foreach (var index in garmentSampleRequest.ImagesFile)
                {
                    countImage++;
                }


                if (countImage != 0)
                {
                    if (countImage > 5)
                    {
                        countImage = 5;
                    }

                    PdfPTable table_image = new PdfPTable(countImage);
                    float[] image_widths = new float[countImage];

                    for (var i = 0; i < countImage; i++)
                    {
                        image_widths.SetValue(5f, i);
                    }

                    if (countImage != 0)
                    {
                        table_image.SetWidths(image_widths);
                    }

                    table_image.TotalWidth = 570f;

                    foreach (var imageFromRo in garmentSampleRequest.ImagesFile)
                    {
                        try
                        {
                            image = Convert.FromBase64String(Base64.GetBase64File(imageFromRo));
                        }
                        catch (Exception)
                        {
                            //var webClient = new WebClient();
                            //roImage = webClient.DownloadData("https://bateeqstorage.blob.core.windows.net/other/no-image.jpg");
                            image = Convert.FromBase64String("/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAA0NDQ0ODQ4QEA4UFhMWFB4bGRkbHi0gIiAiIC1EKjIqKjIqRDxJOzc7STxsVUtLVWx9aWNpfZeHh5e+tb75+f8BDQ0NDQ4NDhAQDhQWExYUHhsZGRseLSAiICIgLUQqMioqMipEPEk7NztJPGxVS0tVbH1pY2l9l4eHl761vvn5///CABEIAAoACgMBIgACEQEDEQH/xAAVAAEBAAAAAAAAAAAAAAAAAAAAB//aAAgBAQAAAACnD//EABQBAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQIQAAAAf//EABQBAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQMQAAAAf//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEAAT8AH//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQIBAT8Af//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQMBAT8Af//Z");
                        }

                        Image images = Image.GetInstance(imgb: image);

                        if (images.Width > 60)
                        {
                            float percentage = 0.0f;
                            percentage = 60 / images.Width;
                            images.ScalePercent(percentage * 100);
                        }

                        PdfPCell imageCell = new PdfPCell(images);
                        imageCell.Border = 0;
                        table_image.AddCell(imageCell);
                    }

                    PdfPCell cell_image = new PdfPCell()
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 2,
                    };

                    foreach (var name in garmentSampleRequest.ImagesName)
                    {
                        cell_image.Phrase = new Phrase(name, normal_font);
                        table_image.AddCell(cell_image);
                    }

                    table_image.LockedWidth = true;
                    table_image.HorizontalAlignment = Element.ALIGN_LEFT;
                    table_image.SpacingBefore = 5f;
                    table_image.ExtendLastRow = false;
                    document.Add(table_image);
                }
            }
            
            #endregion

            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(3);
            cellCenterTopNoBorder.Phrase = new Paragraph("");// new Paragraph("Diterima,\n\n\n\n\n\n\n\nKoordinator Sample", normal_font);
            cellCenterTopNoBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph(""); //new Paragraph("Mengetahui,\n\n\n\n\n\n\n\nKasie / Kabag Penjualan", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Dibuat,\n\n\n\n\n\n\n\n( "+ garmentSampleRequest.CreatedBy +" )\nPenjualan", normal_font);
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

    public class GarmentSampleRequestProductVM
    {
        public string Style { get; set; }
        public string Color { get; set; }

        public string Fabric { get; set; }
        public string Size { get; set; }
        public string SizeDescription { get; set; }
        public double Quantity { get; set; }
    }

    public class GarmentSampleRequestSpecificationVM
    {
        public string Inventory { get; set; }
        public string SpecificationDetail { get; set; }
        public double Quantity { get; set; }
        public string Remark { get; set; }
        public string Uom { get; set; }
    }
}
