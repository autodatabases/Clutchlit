#pragma checksum "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aa81f26a4b3e8b2cf38543d6d426407717fc505c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AllegroAuctions_AddAuction), @"mvc.1.0.view", @"/Views/AllegroAuctions/AddAuction.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/AllegroAuctions/AddAuction.cshtml", typeof(AspNetCore.Views_AllegroAuctions_AddAuction))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa81f26a4b3e8b2cf38543d6d426407717fc505c", @"/Views/AllegroAuctions/AddAuction.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3906bc195ced385164f3d15e7c37db7594154460", @"/Views/_ViewImports.cshtml")]
    public class Views_AllegroAuctions_AddAuction : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
  
    ViewData["Title"] = "Wystaw aukcję | Aukcje | Dział allegro";
    ViewData["Title_1"] = "Aukcje";
    ViewData["Title_2"] = "Dodaj nową aukcję";

#line default
#line hidden
            BeginContext(161, 927, true);
            WriteLiteral(@"

<div class=""box"">
    <div class=""images-loader"" style=""background:url(/images/Pacman-0.7s-91px.svg) center center no-repeat; display:none; background-color:rgba(0, 00, 0, .5); position:absolute; width:100%; z-index:1; height:100%;""></div>

        <div class=""box-header with-border"">
            <h3 class=""box-title"">Wystaw nową aukcję</h3>
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
            BeginContext(1088, 4512, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c8608474278047d089480dc24c588fa9", async() => {
                BeginContext(1138, 372, true);
                WriteLiteral(@"


                <div class=""col-md-12"">
                    <div class=""form-group"">
                        <label class=""col-sm-2"" for=""auction_title"">Tytuł aukcji</label>
                        <div class=""col-sm-10"">
                            <input type=""text"" class=""form-control"" name=""auction_title"" id=""auction_title"" />
                            ");
                EndContext();
                BeginContext(1511, 17, false);
#line 32 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                       Write(ViewData["token"]);

#line default
#line hidden
                EndContext();
                BeginContext(1528, 245, true);
                WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n\r\n                </div>\r\n                <div class=\"col-md-12\">\r\n                    <h4>Wybierz kategorię</h4>\r\n                    <div class=\"col-md-2\">\r\n                        ");
                EndContext();
                BeginContext(1774, 158, false);
#line 40 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                   Write(Html.DropDownList("maincategories", new SelectList((System.Collections.IEnumerable)ViewData["MainCategories"], "Id", "Name"), new { @class = "form-control" }));

#line default
#line hidden
                EndContext();
                BeginContext(1932, 189, true);
                WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-2\">\r\n                        <select name=\"category2\" id=\"category2\" class=\"form-control\">\r\n                            ");
                EndContext();
                BeginContext(2121, 40, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b397cc9939cf4524a014cdabfb1f9c17", async() => {
                    BeginContext(2129, 23, true);
                    WriteLiteral("-- Wybierz kategorię --");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2161, 224, true);
                WriteLiteral("\r\n                        </select>\r\n                    </div>\r\n                    <div class=\"col-md-2\">\r\n                        <select name=\"category3\" id=\"category3\" class=\"form-control\">\r\n                            ");
                EndContext();
                BeginContext(2385, 40, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "006e9239c1ce40ccbfb36ab53c468f4c", async() => {
                    BeginContext(2393, 23, true);
                    WriteLiteral("-- Wybierz kategorię --");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2425, 224, true);
                WriteLiteral("\r\n                        </select>\r\n                    </div>\r\n                    <div class=\"col-md-2\">\r\n                        <select name=\"category4\" id=\"category4\" class=\"form-control\">\r\n                            ");
                EndContext();
                BeginContext(2649, 40, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "48dd276ab2a44d8ea036dff3aaed9ca6", async() => {
                    BeginContext(2657, 23, true);
                    WriteLiteral("-- Wybierz kategorię --");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2689, 224, true);
                WriteLiteral("\r\n                        </select>\r\n                    </div>\r\n                    <div class=\"col-md-2\">\r\n                        <select name=\"category5\" id=\"category5\" class=\"form-control\">\r\n                            ");
                EndContext();
                BeginContext(2913, 40, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1fd529b0468a494a8e38ec39a7dcd256", async() => {
                    BeginContext(2921, 23, true);
                    WriteLiteral("-- Wybierz kategorię --");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2953, 436, true);
                WriteLiteral(@"
                        </select>
                    </div>
                    <div class=""col-md-2"">
                        <button id=""confirmCategory"" class=""btn btn-github"" type=""submit"">Wybierz</button>
                    </div>
                </div>
                <div class=""col-md-12"">

                    <div class=""col-md-6"">
                        <h4>Wybierz cennik dostaw</h4>
                        ");
                EndContext();
                BeginContext(3390, 153, false);
#line 70 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                   Write(Html.DropDownList("shippingrate", new SelectList((System.Collections.IEnumerable)ViewData["Shipping"], "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
                EndContext();
                BeginContext(3543, 319, true);
                WriteLiteral(@"
                    </div>
                    <div class=""col-md-6"">
                        <h4>
                            Warunki oferty
                        </h4>
                        <div class=""col-md-12"">
                            <h5>Informacja o gwarancjach</h5>
                            ");
                EndContext();
                BeginContext(3863, 152, false);
#line 78 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                       Write(Html.DropDownList("warrantyDoc", new SelectList((System.Collections.IEnumerable)ViewData["Warranty"], "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
                EndContext();
                BeginContext(4015, 168, true);
                WriteLiteral("\r\n                        </div>\r\n                        <div class=\"col-md-12\">\r\n                            <h5>Warunki reklamacji</h5>\r\n                            ");
                EndContext();
                BeginContext(4184, 166, false);
#line 82 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                       Write(Html.DropDownList("impliedWarrantyDoc", new SelectList((System.Collections.IEnumerable)ViewData["ImpliesWarranty"], "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
                EndContext();
                BeginContext(4350, 166, true);
                WriteLiteral("\r\n                        </div>\r\n                        <div class=\"col-md-12\">\r\n                            <h5>Polityka zwrotów</h5>\r\n                            ");
                EndContext();
                BeginContext(4517, 160, false);
#line 86 "D:\WORK\Customers\[SPRZĘGŁA]\Clutchlit\Clutchlit\Views\AllegroAuctions\AddAuction.cshtml"
                       Write(Html.DropDownList("returnPolicyDoc", new SelectList((System.Collections.IEnumerable)ViewData["ReturnPolicy"], "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
                EndContext();
                BeginContext(4677, 916, true);
                WriteLiteral(@"
                        </div>

                    </div>
                </div>
                <div class=""col-md-12"">
                    <h4>Parametry oferty</h4>
                    <div class=""col-md-6"" id=""offer_parameters"">
                    </div>
                </div>
                <div class=""col-md-12"">
                    
                    <div class=""col-md-6"">
                        <h4>Zdjęcia</h4>

                        <input type=""file"" id=""imageUploadForm"" name=""image"" multiple=""multiple"" />
                        <br />
                        <button id=""confirmPhotos"" class=""btn btn-github"" type=""button"">Wyślij zdjęcia do allegro</button>
                    </div>
                    <div class=""col-md-6"">
                        <strong>Postęp: </strong> <div id=""progress""></div> 
                    </div>
                </div>
            ");
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
            BeginContext(5600, 34, true);
            WriteLiteral("\r\n        </div>\r\n\r\n    </div>\r\n\r\n");
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
