using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LG.Utility {
    public static class MvcHtmlStringExtend {
        /// <summary>
        /// 更具Css路径，返回<link .../> html
        /// </summary>
        /// <param name="hh"></param>
        /// <param name="cssPath"></param>
        /// <returns></returns>
        public static MvcHtmlString Css(this HtmlHelper htmlHelper, string cssPath) {
            if (string.IsNullOrWhiteSpace(cssPath)) return MvcHtmlString.Create(string.Empty);
            return MvcHtmlString.Create("<link href=\"" + cssPath + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        /// <summary>
        /// 更具Javascript路径，返回<script ... /> html
        /// </summary>
        /// <param name="hh"></param>
        /// <param name="cssPath"></param>
        /// <returns></returns>
        public static MvcHtmlString Javascript(this HtmlHelper htmlHelper, string javascriptPath) {
            if (string.IsNullOrWhiteSpace(javascriptPath)) return MvcHtmlString.Create(string.Empty);
            return MvcHtmlString.Create("<script type=\"text/javascript\" src=\"" + javascriptPath + "\" ></script>");
        }
    }
}
