using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Utilities
{
    public static class HTMLUtilities
    {
        public static string GetHTMLColor(string color)
        {
            if (color.StartsWith("#")) return color;

            if (Int32.TryParse(color, out var colorValue))
            {
                return ColorTranslator.ToHtml(Color.FromArgb(colorValue));
            }
            return color;
        }
    }
}
