using AutoMapper;
using LiczbyNaSlowaNETCore;
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
            var productsPricesByVats = GroupProductsPricesByVat(order);
            var otherItemsNettos = CalculateOrderOtherItemsNettos(productsPricesByVats, order);
            var address = await config.GetDocumentAddress(repositoryWrapper.ConfigRepository);
            decimal totalValToPay;
            var html = builder.CreateDocument()
                .BuildCompanyName(await config.GetValue(ConfigKeys.Title, repositoryWrapper.ConfigRepository))
                .BuildPlaceOfCreation(address.DocumentPlace)
                .BuildDateOfCreation(order.OrderDate.ToString("dd.MM.yyyy"))
                .BuildSellerData(address)
                .BuildBuyerData(mapper.Map<AddressData>(order.Address))
                .BuildDocumentNumber($"Faktura nr {order.InvoiceNumber}")
                .BuildTableHeader("Lp.", "Nazwa", "Jm.", "Cena netto", "Ilość", "Wartość netto", "Stawka VAT", "Kwota VAT", "Wartość brutto")
                .BuildTableBody(GetInvoiceTableBody(order, otherItemsNettos))
                .BuildTableBodySummary(GetTableBodySummary(productsPricesByVats, otherItemsNettos, out totalValToPay))
                .BuildAdditionalInfo(await GetInvoiceAddtionalInfo(order))
                .BuildSummary(GetInvoiceSummary(totalValToPay))
                .GetDocument();

            return await GetDocument(html);
        }

        private List<string> GetInvoiceSummary(decimal totalValToPay)
        {
            List<string> lines = new()
            {
                $@"<div class=""p0 m0""><b>Do zapłaty:</b> {string.Format("{0:C}", totalValToPay)}</div>",
                $@"<div class=""p0 m0""><b>Słownie:</b> {NumberToText.Convert(totalValToPay, new NumberToTextOptions
                {
                    Stems = true,
                    Currency = Currency.PLN,
                })}</div>",
            };

            return lines;
        }

        private List<string[]> GetTableBodySummary(List<(int, int)> productsPricesByVats, List<(int, decimal)> otherItemsNettos, out decimal totalValToPay)
        {
            List<string[]> lines = new();
            var vatSum = 0.0M;
            var nettoSum = 0.0M;
            var isFirst = true;
            foreach (var item in productsPricesByVats)
            {
                var other = otherItemsNettos.FirstOrDefault(x => x.Item1 == item.Item1);
                decimal value = other == default ? 0.0M : other.Item2;
                var totalValue = item.Item2 / 100.0M + value;
                var vatValue = Math.Round(totalValue * item.Item1 / 100.0M, 2);
                nettoSum += totalValue;
                vatSum += vatValue;
                lines.Add(new string[] {
                    "",
                    "",
                    "",
                    "",
                    isFirst ? "W tym" : " ",
                    totalValue.ToString("0.00"),
                    item.Item1.ToString() + "%",
                    vatValue.ToString("0.00"),
                    (totalValue +vatValue).ToString("0.00")
                });
                isFirst = false;
            }
            lines.Add(new string[] {
                    "",
                    "",
                    "",
                    "",
                    "RAZEM",
                    nettoSum.ToString("0.00"),
                    " ",
                    vatSum.ToString("0.00"),
                    (nettoSum+vatSum).ToString("0.00")
                });
            totalValToPay = nettoSum + vatSum;
            return lines;
        }

        private async Task<List<string>> GetInvoiceAddtionalInfo(Order order)
        {
            List<string> lines = new()
            {
                $@"<div class=""p0 m0""><b>Sposób płatności:</b> {Dictionaries.PaymentMethodDictionary.PaymentDictionary[order.PaymentMethod]}</div>",
                $@"<div class=""p0 m0""><b>Data płatności:</b> {order.OrderDate.AddDays(7).ToString("dd.MM.yyyy")}</div>",
            };
            if (order.PaymentMethod == EnumTypes.PaymentMethodEnum.TraditionalTransfer)
            {
                var account = await config.GetDataToPayment(repositoryWrapper.ConfigRepository);
                lines.Add($@"<div class=""p0 m0""><b>Numer konta:</b> {account.AccountNumber}</div>");
            }
            return lines;
        }

        private async Task<Stream> GetDocument(string html)
        {
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

        private List<string[]> GetInvoiceTableBody(Order order, List<(int, decimal)> deliveryNettos)
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
            if (order.DeliveryPrice > 0 || order.PaymentPrice > 0)
            {

                foreach (var item in deliveryNettos)
                {
                    var vat = (item.Item1 / 100.0M) * item.Item2;
                    list.Add(new string[]
                    {
                    ( list.Count +1).ToString(),
                    $"Dostawa i płatność - VAT {item.Item1}%",
                    "szt.",
                    item.Item2.ToString("0.00"),
                    "1",
                    item.Item2.ToString("0.00"),
                    item.Item1.ToString() + "%",
                    (vat).ToString("0.00"),
                    (vat + item.Item2).ToString("0.00")
                    });
                }
            }

            return list;
        }

        private List<(int, int)> GroupProductsPricesByVat(Order order)
        {
            var vatRates = order.ProductOrders.Select(po => po.ProductVatRate).Distinct().ToList();
            List<(int, int)> values = new();
            foreach (var vat in vatRates)
            {
                var sum = 0;
                foreach (var product in order.ProductOrders.Where(po => po.ProductVatRate == vat))
                {
                    sum += product.ProductPrice * product.Count;
                }
                values.Add((vat, sum));
            }
            return values;
        }

        private List<(int, decimal)> CalculateOrderOtherItemsNettos(List<(int, int)> productsVat, Order order)
        {
            var orderSum = 0;
            foreach (var po in order.ProductOrders)
            {
                orderSum += po.ProductPrice;
            }
            List<(int, double)> ratios = new();
            foreach (var item in productsVat)
            {
                var ratio = item.Item2 / (double)orderSum;
                ratios.Add((item.Item1, ratio));
            }
            decimal nettoSum = order.DeliveryPrice + order.PaymentPrice;
            var divider = 0.0;
            foreach (var item in ratios)
            {
                divider += item.Item2 * (1 + item.Item1 / 100.0);
            }
            nettoSum = Math.Round(nettoSum / 100.0M / (decimal)divider, 2);
            List<(int, decimal)> deliveryNettos = new();
            foreach (var item in ratios)
            {
                deliveryNettos.Add((item.Item1, Math.Round(nettoSum * (decimal)item.Item2, 2)));
            }
            return deliveryNettos;
        }

        public async Task<Stream> GetWZDocument(Order order)
        {
            var address = await config.GetDocumentAddress(repositoryWrapper.ConfigRepository);
            var html = builder.CreateDocument()
                .BuildCompanyName(await config.GetValue(ConfigKeys.Title, repositoryWrapper.ConfigRepository))
                .BuildPlaceOfCreation(address.DocumentPlace)
                .BuildDateOfCreation(order.Packing.PackingEnd.Value.ToString("dd.MM.yyyy"))
                .BuildSellerData(address)
                .BuildBuyerData(mapper.Map<AddressData>(order.Address))
                .BuildDocumentNumber($"WZ nr {order.WZNumber}")
                .BuildTableHeader("Lp.", "Nazwa", "Jm.", "Ilość", "Uwagi")
                .BuildTableBody(GetWZTableBody(order))
                .BuildTableBodySummary(new List<string[]>())
                .BuildAdditionalInfo(await GetInvoiceAddtionalInfo(order))
                .BuildSummary(new List<string>() { $@"<div class=""p0 m0""><b>Powiązany dokument:</b> {order.InvoiceNumber}</div>" })
                .GetDocument();

            return await GetDocument(html);
        }

        private List<string[]> GetWZTableBody(Order order)
        {
            var list = new List<string[]>();
            for (int i = 0; i < order.ProductOrders.Count; i++)
            {
                var pr = order.ProductOrders.ElementAt(i);
                var idx = i + 1;
                list.Add(new string[]
                {
                    idx.ToString(),
                    pr.Product.Name,
                    "szt.",
                    pr.Count.ToString(),
                    ""
                });
            }
            return list;
        }

        public async Task<Stream> GetDeliveryDocument(Delivery delivery)
        {
            var address = await config.GetDocumentAddress(repositoryWrapper.ConfigRepository);
            var html = builder.CreateDocument()
                .BuildCompanyName(await config.GetValue(ConfigKeys.Title, repositoryWrapper.ConfigRepository))
                .BuildPlaceOfCreation(address.DocumentPlace)
                .BuildDateOfCreation(delivery.DeliveryDate.ToString("dd.MM.yyyy"))
                .BuildBuyerData(address)
                .BuildSellerData(mapper.Map<AddressData>(delivery.Supplier))
                .BuildDocumentNumber($"Dostawa {delivery.PZNumber}")
                .BuildTableHeader("Lp.", "Nazwa", "Jm.", "Ilość", "Sektor")
                .BuildTableBody(GetDeliveryTableBody(delivery))
                .BuildTableBodySummary(new List<string[]>())
                .BuildAdditionalInfo(new List<string>(), true)
                .BuildSummary(new List<string>(),true )
                .GetDocument();

            return await GetDocument(html);
        }
        
        public async Task<Stream> GetDeliveryCorrecingDocument(Delivery correcting, Delivery corrected)
        {
            var address = await config.GetDocumentAddress(repositoryWrapper.ConfigRepository);
            var html = builder.CreateDocument()
                .BuildCompanyName(await config.GetValue(ConfigKeys.Title, repositoryWrapper.ConfigRepository))
                .BuildPlaceOfCreation(address.DocumentPlace)
                .BuildDateOfCreation(correcting.DeliveryDate.ToString("dd.MM.yyyy"))
                .BuildBuyerData(address)
                .BuildSellerData(mapper.Map<AddressData>(correcting.Supplier))
                .BuildDocumentNumber($"Dostawa {correcting.PZNumber}")
                .BuildTablesDesciptions("Przed korektą","Po korekcie")
                .BuildTableHeader("Lp.", "Nazwa", "Jm.", "Ilość", "Sektor")
                .BuildSecondTableHeader("Lp.", "Nazwa", "Jm.", "Ilość", "Sektor")
                .BuildTableBody(GetDeliveryTableBody(corrected))
                .BuildSecondTableBody(GetDeliveryTableBody(correcting))
                .BuildTableBodySummary(new List<string[]>())
                .BuildAdditionalInfo(new List<string>() { $@"<div class=""p0 m0""><b>Powiązany dokument:</b> {corrected.PZNumber}</div>" })
                .BuildSummary(new List<string>(),true )
                .GetDocument();

            return await GetDocument(html);
        }

        private List<string[]> GetDeliveryTableBody(Delivery delivery)
        {
            var list = new List<string[]>();
            for (int i = 0; i < delivery.ProductDeliveries.Count; i++)
            {
                var pr = delivery.ProductDeliveries.ElementAt(i);
                var idx = i + 1;
                list.Add(new string[]
                {
                    idx.ToString(),
                    $"{pr.Product.Name} ({pr.Product.EANCode})",
                    "szt.",
                    pr.Count.ToString(),
                    $"{pr.Sector.Row.RowName}{pr.Sector.Order}"
                });
            }
            return list;
        }
    }
}
