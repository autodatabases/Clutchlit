#pragma checksum "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\GetList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "802592581375b5904a1a17c4311ba42e417453c7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AllegroAuctions_GetList), @"mvc.1.0.view", @"/Views/AllegroAuctions/GetList.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/AllegroAuctions/GetList.cshtml", typeof(AspNetCore.Views_AllegroAuctions_GetList))]
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
#line 1 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\_ViewImports.cshtml"
using Clutchlit;

#line default
#line hidden
#line 2 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\_ViewImports.cshtml"
using Clutchlit.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"802592581375b5904a1a17c4311ba42e417453c7", @"/Views/AllegroAuctions/GetList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3906bc195ced385164f3d15e7c37db7594154460", @"/Views/_ViewImports.cshtml")]
    public class Views_AllegroAuctions_GetList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("getoffers"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\GetList.cshtml"
  
    ViewData["Title"] = "Lista aukcji | Aukcje | Dział allegro";
    ViewData["Title_1"] = "Aukcje";
    ViewData["Title_2"] = "Lista aukcji";

#line default
#line hidden
            BeginContext(155, 422, true);
            WriteLiteral(@"

<a href=""https://allegro.pl/auth/oauth/authorize?response_type=code&client_id=4fe0e5e7114c415fb7f69cbfd1daab16&redirect_uri=http://clutchlit.trimfit.pl/AllegroAuctions/GetList/api/"" class=""btn bg-orange"">Zaloguj się do Allegro</a>
<a id=""end_selected"" class=""btn bg-orange"">Wyłącz zaznaczone</a>
<a id=""start_selected"" class=""btn bg-orange"">Włącz zaznaczone</a>
<a id=""allegro_test"" class=""btn bg-orange"">Test</a>
");
            EndContext();
            BeginContext(577, 127, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d636fffcc7d7437f8e116f6f529732d4", async() => {
                BeginContext(612, 85, true);
                WriteLiteral("\r\n    <input id=\"button2\" class=\"btn bg-orange\" type=\"submit\" value=\"Pokaż aukcje\">\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(704, 1556, true);
            WriteLiteral(@"
<div class=""box"">
    
    <div class=""box-header with-border"">
        <h3 class=""box-title"">Lista aukcji</h3>
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

        <table id=""example3"" class=""table table-bordered table-hover"">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Tytuł aukcji</th>
                    <th>Kategoria</th>
                    <th>Miniatura</th>
                    <th>Cena</th>
                    <th>Obserwujący</th>
                    <th>Odwiedzin<");
            WriteLiteral(@"/th>
                    <th>Status</th>
                </tr>
            </thead>

            <tfoot>
                <tr>
                    <th>Id</th>
                    <th>Tytuł aukcji</th>
                    <th>Kategoria</th>
                    <th>Miniatura</th>
                    <th>Cena</th>
                    <th>Obserwujący</th>
                    <th>Odwiedzin</th>
                    <th>Status</th>
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
