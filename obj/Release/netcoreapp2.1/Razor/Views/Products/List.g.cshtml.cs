#pragma checksum "E:\Clutchlit2\Clutchlit\Clutchlit\Views\Products\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "74502e2d0f08f5c370b265a2af2e6694a0fa931e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Products_List), @"mvc.1.0.view", @"/Views/Products/List.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Products/List.cshtml", typeof(AspNetCore.Views_Products_List))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\_ViewImports.cshtml"
using Clutchlit;

#line default
#line hidden
#line 2 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\_ViewImports.cshtml"
using Clutchlit.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"74502e2d0f08f5c370b265a2af2e6694a0fa931e", @"/Views/Products/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3906bc195ced385164f3d15e7c37db7594154460", @"/Views/_ViewImports.cshtml")]
    public class Views_Products_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Manufacturer>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("mark"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "mark", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control select2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\Products\List.cshtml"
  
    ViewData["Title"] = "Lista produktów | Produkty | Dział handlowy";
    ViewData["Title_1"] = "Produkty";
    ViewData["Title_2"] = "Lista produktów";

#line default
#line hidden
            BeginContext(198, 661, true);
            WriteLiteral(@"<div class=""box"">
    <div class=""box-header with-border"">
        <h3 class=""box-title"">Wyszukaj produkty do auta</h3>
        <div class=""box-tools pull-right"">
            <button type=""button"" class=""btn btn-box-tool"" data-widget=""collapse"" data-toggle=""tooltip"" title="""" data-original-title=""Zwiń"">
                <i class=""fa fa-minus""></i>
            </button>
            <button type=""button"" class=""btn btn-box-tool"" data-widget=""remove"" data-toggle=""tooltip"" title="""" data-original-title=""Zamknij"">
                <i class=""fa fa-times""></i>
            </button>                
        </div>
    </div>
    <div class=""box-body "">
");
            EndContext();
#line 20 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\Products\List.cshtml"
         using (Html.BeginForm("List", "Products", FormMethod.Post, new { @id = "ktypes_search" }))
        {

#line default
#line hidden
            BeginContext(971, 53, true);
            WriteLiteral("            <span class=\"col-md-4\">\r\n                ");
            EndContext();
            BeginContext(1024, 212, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d74f40e9590a48f1a45859926a9d9b16", async() => {
                BeginContext(1141, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
                BeginContext(1163, 46, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d8bec81b6b6c46f7a0dfe4d8f264c82f", async() => {
                    BeginContext(1189, 11, true);
                    WriteLiteral(" - Marka - ");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("disabled", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1209, 18, true);
                WriteLiteral("\r\n                ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
#line 23 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\Products\List.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = (new SelectList(Model, "Tecdoc_id", "Name"));

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1236, 163, true);
            WriteLiteral("\r\n            </span>\r\n            <span class=\"col-md-3\">\r\n                <select id=\"seriesA\" name=\"seriesA\" class=\"form-control select2\">\r\n                    ");
            EndContext();
            BeginContext(1399, 46, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a8852f901e554b33bf4ba381be3a904d", async() => {
                BeginContext(1425, 11, true);
                WriteLiteral(" - Model - ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("disabled", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1445, 190, true);
            WriteLiteral("\r\n                </select>\r\n            </span>\r\n            <span class=\"col-md-3\">\r\n                <select id=\"engines\" name=\"engines\" class=\"form-control select2\">\r\n                    ");
            EndContext();
            BeginContext(1635, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21a8cdfa468843e5b406e980c3e51d27", async() => {
                BeginContext(1661, 12, true);
                WriteLiteral(" - Silnik - ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("disabled", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1682, 232, true);
            WriteLiteral("\r\n                </select>\r\n\r\n            </span>\r\n            <span class=\"col-md-2\" style=\"text-align:center\">\r\n                <input id=\"button\" class=\"btn bg-orange\" type=\"submit\" value=\"Pokaż produkty\">\r\n            </span>\r\n");
            EndContext();
#line 41 "E:\Clutchlit2\Clutchlit\Clutchlit\Views\Products\List.cshtml"
        }

#line default
#line hidden
            BeginContext(1925, 3684, true);
            WriteLiteral(@"        </div>

    <div class=""box-header with-border"">
        <h3 class=""box-title"">Wybierz kategorie produktów</h3>
        
    </div>
    <div class=""box-body"">
        <div class=""form-group"">
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""479"" >
                ZESTAW SPRZĘGŁA
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""1000"">
                ZESTAW SPRZĘGŁA Z KOŁEM
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""577"">
                KOŁO ZAMACHOWE
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""262"">
                TARCZA SPRZĘGŁA
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""261"">
  ");
            WriteLiteral(@"              DOCISK SPRZĘGŁA
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""48"">
                ŁOŻYSKO OPOROWE
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""47"">
                WYSPRZĘGLIK CENTRALNY
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""620"">
                SIŁOWNIK SPRZĘGŁA
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""234"">
                POMPA SPRZĘGŁA
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""3985"">
                ŚRUBY
            </label>
            <label class=""col-lg-3"">
                <input type=""checkbox"" class=""flat-red category"" value=""1001""");
            WriteLiteral(@">
                USZCZELNIACZE
            </label>
        </div>
    </div>
</div>


<div class=""box"">
    <div class=""box-header with-border"">
        <h3 class=""box-title"">Lista produktów</h3>
        <div class=""box-tools pull-right"">
            <button type=""button"" class=""btn btn-box-tool"" data-widget=""collapse"" data-toggle=""tooltip"" title="""" data-original-title=""Zwiń"">
                <i class=""fa fa-minus""></i>
            </button>
            <button type=""button"" class=""btn btn-box-tool"" data-widget=""remove"" data-toggle=""tooltip"" title="""" data-original-title=""Zamknij"">
                <i class=""fa fa-times""></i>
            </button>
        </div>
    </div>
    <div class=""box-body "">

        <table id=""example2"" class=""table table-bordered table-hover"">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nazwa</th>
                    <th>Referencja</th>
                    <th>Cena brutto [S]</th>
                    ");
            WriteLiteral(@"<th>Cena brutto [D]</th>
                    <th class=""distributors"">Nazwa [D]</th>
                    <th>Marża</th>
                </tr>
            </thead>
            
            <tfoot>
                <tr>
                    <th>Id</th>
                    <th>Nazwa</th>
                    <th>Referencja</th>
                    <th>C brutto [S]</th>
                    <th>C brutto [D]</th>
                    <th class=""distributors"">Nazwa [D]</th>
                    <th>Marża</th>
                </tr>
            </tfoot>
        </table>

    </div>
    
</div>
 
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Manufacturer>> Html { get; private set; }
    }
}
#pragma warning restore 1591
