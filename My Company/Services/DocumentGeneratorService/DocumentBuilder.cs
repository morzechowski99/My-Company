using Microsoft.AspNetCore.Hosting;
using My_Company.Services.DocumentGeneratorService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services.DocumentGeneratorService
{
    public class DocumentBuilder : IDocumentBuilder
    {

        private readonly IWebHostEnvironment environment;
        private string body;
        public DocumentBuilder(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void BuildBuyerData(AddressData buyerData)
        {
            var nip = buyerData.NIP != null ? "NIP: " + buyerData.NIP : "";
            var buyerDataString = $@"<div class=""p0 m0"">{buyerData.Name}</div>
            <div class=""p0 m0"">{nip}</div>
            <div class=""p0 m0"">{buyerData.Address1}</div>
            <div class=""p0 m0"">{buyerData.Address2}</div>";
            body = body.Replace("{BuyerData}", buyerDataString);
        }

        public void BuildCompanyName(string companyName)
        {
            body = body.Replace("{CompanyName}", companyName);
        }

        public void BuildDateOfCreation(string dateOfCreation)
        {
            body = body.Replace("{DateOfCreation}", dateOfCreation);
        }

        public void BuildDocumentNumber(string documentNumber)
        {
            body = body.Replace("{DocumentNumber}", documentNumber);
        }

        public void BuildPlaceOfCreation(string placeOfCreation)
        {
            body = body.Replace("{PlaceOfCreation}", placeOfCreation);
        }

        public void BuildSellerData(AddressData sellerData)
        {
            var nip = sellerData.NIP != null ? "NIP: " + sellerData.NIP : "";
            var sellerDataString = $@"<div class=""p0 m0"">{sellerData.Name}</div>
            <div class=""p0 m0"">{nip}</div>
            <div class=""p0 m0"">{sellerData.Address1}</div>
            <div class=""p0 m0"">{sellerData.Address2}</div>";
            body = body.Replace("{SellerData}", sellerDataString);
        }

        public void BuildTableBody(List<string[]> rows)
        {
            var rowsString = "";
            foreach (var row in rows)
            {
                var rowString = "<tr>";
                foreach(var data in row)
                {
                    rowsString +=  $@"<td>{data}</td>";
                }
                rowsString += "</tr>";
                rowsString += rowString;
            }
            body = body.Replace("{tableRows}", rowsString);
        }

        public void BuildTableHeader(string[] header)
        {
            var headers = "";
            foreach (var h in header)
            {
                headers += $@"<th style=""background-color:darkgray;"">{h}</th>";
            }
            body = body.Replace("{tableHeader}", headers);
        }

        public void CreateDocument()
        {
            body = File.ReadAllText(Path.Join(environment.WebRootPath, @"\Templates\documentTemplate.html"));
        }

        public string GetDocument()
        {
            return body;
        }
    }
}
