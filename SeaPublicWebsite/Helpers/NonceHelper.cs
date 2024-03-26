namespace SeaPublicWebsite.Helpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class NonceHelper
    {
        public static string ScriptNonce(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.Items["ScriptNonce"] as string;
        }
    }
}