#pragma checksum "C:\dotNetInternShip\BookStore\BookStore\Areas\Customer\Views\Item\OrderConfirmation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "94d1c3082930a0f91cec05b125b50d9b0ca780de"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Customer_Views_Item_OrderConfirmation), @"mvc.1.0.view", @"/Areas/Customer/Views/Item/OrderConfirmation.cshtml")]
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
#nullable restore
#line 1 "C:\dotNetInternShip\BookStore\BookStore\Areas\Customer\Views\_ViewImports.cshtml"
using BookStore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\dotNetInternShip\BookStore\BookStore\Areas\Customer\Views\_ViewImports.cshtml"
using BookStore.BusinessLogic.ServiceModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"94d1c3082930a0f91cec05b125b50d9b0ca780de", @"/Areas/Customer/Views/Item/OrderConfirmation.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"12d5eb473f5e1c0c6e9941440470ea86bf57f184", @"/Areas/Customer/Views/_ViewImports.cshtml")]
    public class Areas_Customer_Views_Item_OrderConfirmation : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""container row"">
    <div class=""col-12 text-center"">
        <h1 class=""text-primary text-center"">Order Submitted Succesfully</h1>
        <br />
    </div>
    <div class=""col-12 text-center"">
        Thank you for your order!<br />
        We have received your order and we will send a follow up email shortly!
        <br />
        Your order number is:");
#nullable restore
#line 11 "C:\dotNetInternShip\BookStore\BookStore\Areas\Customer\Views\Item\OrderConfirmation.cshtml"
                        Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<int> Html { get; private set; }
    }
}
#pragma warning restore 1591
