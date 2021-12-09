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

        public IDocumentBuilder BuildAdditionalInfo(List<string> lines)
        {
            var html = "<hr/>";
            lines.ForEach(line => html += $"{line} <hr/>");
            body = body.Replace("{AdditionalInfo}", html);
            return this;
        }

        public IDocumentBuilder BuildBuyerData(AddressData buyerData)
        {
            var nip = buyerData.NIP != null ? "NIP: " + buyerData.NIP : "";
            var buyerDataString = $@"<div class=""p0 m0"">{buyerData.Name}</div>
            <div class=""p0 m0"">{nip}</div>
            <div class=""p0 m0"">{buyerData.Address1}</div>
            <div class=""p0 m0"">{buyerData.Address2}</div>";
            body = body.Replace("{BuyerData}", buyerDataString);
            return this;
        }

        public IDocumentBuilder BuildCompanyName(string companyName)
        {
            body = body.Replace("{CompanyName}", companyName);
            return this;
        }

        public IDocumentBuilder BuildDateOfCreation(string dateOfCreation)
        {
            body = body.Replace("{DateOfCreation}", dateOfCreation);
            return this;
        }

        public IDocumentBuilder BuildDocumentNumber(string documentNumber)
        {
            body = body.Replace("{DocumentNumber}", documentNumber);
            return this;
        }

        public IDocumentBuilder BuildPlaceOfCreation(string placeOfCreation)
        {
            body = body.Replace("{PlaceOfCreation}", placeOfCreation);
            return this;
        }

        public IDocumentBuilder BuildSellerData(AddressData sellerData)
        {
            var nip = sellerData.NIP != null ? "NIP: " + sellerData.NIP : "";
            var sellerDataString = $@"<div class=""p0 m0"">{sellerData.Name}</div>
            <div class=""p0 m0"">{nip}</div>
            <div class=""p0 m0"">{sellerData.Address1}</div>
            <div class=""p0 m0"">{sellerData.Address2}</div>";
            body = body.Replace("{SellerData}", sellerDataString);
            return this;
        }

        public IDocumentBuilder BuildSummary(List<string> lines, bool empty = false)
        {
            var html = "<hr/>";
            lines.ForEach(line => html += $"{line} <hr/>");
            body = body.Replace("{Summary}", empty ? "" : html);
            return this;
        }

        public IDocumentBuilder BuildTableBody(List<string[]> rows)
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
            return this;
        } 
        
        public IDocumentBuilder BuildTableBodySummary(List<string[]> rows)
        {
            var rowsString = "";
            foreach (var row in rows)
            {
                var rowString = @"<tr class=""borderNone"">";
                foreach(var data in row)
                {
                    if(data != "")
                    rowsString +=  $@"<td>{data}</td>";
                    else
                        rowsString += $@"<td class=""borderNone""></td>";
                }
                rowsString += "</tr>";
                rowsString += rowString;
            }
            body = body.Replace("{tableRowsSummary}", rowsString);
            return this;
        }

        public IDocumentBuilder BuildTableHeader(params string[] header)
        {
            var headers = "";
            foreach (var h in header)
            {
                headers += $@"<th style=""background-color:darkgray;"">{h}</th>";
            }
            body = body.Replace("{tableHeader}", headers);
            return this;
        }

        public IDocumentBuilder CreateDocument()
        {
            body = File.ReadAllText(Path.Join(environment.WebRootPath, @"\Templates\documentTemplate.html"));
            return this;
        }

        public string GetDocument()
        {
            return body;
        }
    }
}
