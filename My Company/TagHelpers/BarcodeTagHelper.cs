using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.TagHelpers
{
    [HtmlTargetElement("barcode")]
    public class BarcodeTagHelper : TagHelper
    {
        public string Code { get; set; }
        public string Content { get; set; } = "Drukuj";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"https://bwipjs-api.metafloor.com/?bcid=code128&text={Code}&scale=3&includetext");
            output.Attributes.SetAttribute("target", "_blank");
            output.Content.SetContent(Content);
        }
    }
}
