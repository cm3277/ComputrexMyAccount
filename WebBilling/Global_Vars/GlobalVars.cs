using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebInvoicing.Global_Vars
{
    public static class GlobalVars
    {
        public static string partsMarkUpTolerance = "Parts marked up";

        public static string perHourTimeName = "Per hour time";

        public static string perHourBusinessName = "Business pricing";

        public static string perHourHomeName = "Home pricing";

        public static double businessPricingFallback = 172;

        public static double homePricingFallback = 125;

        public static double perHourTimeMaxFallback = 70;

        public static double partsMarkUpPercentFallback = 10;

        public static double partsMarkUpMinFallback = 10;
    }
}