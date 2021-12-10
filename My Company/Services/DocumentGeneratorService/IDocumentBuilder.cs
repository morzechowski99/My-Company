using My_Company.Services.DocumentGeneratorService.Models;
using System.Collections.Generic;

namespace My_Company.Services.DocumentGeneratorService
{
    public interface IDocumentBuilder
    {
        IDocumentBuilder CreateDocument();
        IDocumentBuilder BuildCompanyName(string companyName);
        IDocumentBuilder BuildPlaceOfCreation(string placeOfCreation);
        IDocumentBuilder BuildDateOfCreation(string dateOfCreation);
        IDocumentBuilder BuildSellerData(AddressData sellerData);
        IDocumentBuilder BuildBuyerData(AddressData buyerData);
        IDocumentBuilder BuildDocumentNumber(string documentNumber);
        IDocumentBuilder BuildTableHeader(params string[] header);
        IDocumentBuilder BuildTableBody(List<string[]> rows);
        IDocumentBuilder BuildAdditionalInfo (List<string> lines, bool empty = false);
        IDocumentBuilder BuildTableBodySummary(List<string[]> rows);
        IDocumentBuilder BuildSummary (List<string> lines, bool empty = false);
        string GetDocument();
        IDocumentBuilder BuildTablesDesciptions(string description1, string description2);
        IDocumentBuilder BuildSecondTableHeader(params string[] header);
        IDocumentBuilder BuildSecondTableBody(List<string[]> rows);
    }
}
