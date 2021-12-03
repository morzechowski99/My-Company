using AutoMapper;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Services.DocumentGeneratorService.Models;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Services.DocumentGeneratorService
{
    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly IDocumentBuilder builder;
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public DocumentGenerator(IDocumentBuilder builder, IConfig config, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.builder = builder;
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<Stream> GetInvoice(Order order)
        {
            builder.CreateDocument();
            builder.BuildCompanyName(await config.GetValue(ConfigKeys.Title, repositoryWrapper.ConfigRepository));
            builder.BuildPlaceOfCreation("Warszawa");
            builder.BuildDateOfCreation(order.OrderDate.ToString("dd.MM.yyyy"));
            builder.BuildSellerData(new Models.AddressData { Address1 = "łąkowa 11", Address2 = "18-200 łapy", NIP = "23492384", Name = "temp" });
            builder.BuildBuyerData(mapper.Map<AddressData>(order.Address));
            builder.BuildDocumentNumber("Faktura nr " + order.InvoiceNumber);
            builder.BuildTableHeader(new string[] { "Lp.", "Nazwa", "Jm.", "Cena netto","Ilość", "Wartość netto", "Stawka VAT", "Kwota VAT", "Wartość brutto" });
            builder.BuildTableBody(GetTableBody(order));
            var html = builder.GetDocument();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
            }); 
            return pdfContent;
        }

        private List<string[]> GetTableBody(Order order)
        {
            var list = new List<string[]>();
            for (int i = 0; i < order.ProductOrders.Count; i++)
            {
                var pr = order.ProductOrders.ElementAt(i);
                var idx = i + 1;
                var price = pr.ProductPrice / 100.0M;
                var vat = Math.Round(price * pr.ProductVatRate / 100.0M, 2);
                list.Add(new string[]
                {
                    idx.ToString(),
                    pr.Product.Name,
                    "szt.",
                    price.ToString("0.00"),
                    pr.Count.ToString(),
                    (price * pr.Count).ToString("0.00"),
                    pr.ProductVatRate.ToString() + "%",
                    (vat * pr.Count).ToString("0.00"),
                    ((price + vat)*pr.Count).ToString("0.00")
                });
            }
            return list;
        }
    }
}
