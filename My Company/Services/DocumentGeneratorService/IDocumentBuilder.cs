using My_Company.Services.DocumentGeneratorService.Models;
using System.Collections.Generic;

namespace My_Company.Services.DocumentGeneratorService
{
    public interface IDocumentBuilder
    {
        void CreateDocument();
        void BuildCompanyName(string companyName);
        void BuildPlaceOfCreation(string placeOfCreation);
        void BuildDateOfCreation(string dateOfCreation);
        void BuildSellerData(AddressData sellerData);
        void BuildBuyerData(AddressData buyerData);
        void BuildDocumentNumber(string documentNumber);
        void BuildTableHeader(string[] header);
        void BuildTableBody(List<string[]> rows);
        string GetDocument();
    }
}
