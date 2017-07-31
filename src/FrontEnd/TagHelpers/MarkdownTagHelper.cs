using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace FrontEnd.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        private readonly HtmlEncoder _htmlEncoder;

        public MarkdownTagHelper(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        [HtmlAttributeName("markdown")]
        public string Markdown { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Markdown))
                return;

            var html = $"</p><p>{ String.Join("</p><p>", Markdown.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)) }";
            output.Content.AppendHtml(html);
        }
    }
}
