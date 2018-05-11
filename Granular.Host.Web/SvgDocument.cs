using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retyped.dom;

namespace Granular.Host
{
    public static class SvgDocument
    {
        public const string NamespaceUri = "http://www.w3.org/2000/svg";
        public const string XlinkNamespaceUri = "http://www.w3.org/1999/xlink";

        public static HTMLElement CreateElement(string qualifiedName)
        {
            return (HTMLElement)document.createElementNS(NamespaceUri, qualifiedName);
        }
    }
}
