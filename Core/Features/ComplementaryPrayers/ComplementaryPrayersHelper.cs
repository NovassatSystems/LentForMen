using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public static class ComplementaryPrayersHelper
    {
        //public static PrayerWrapper GetPrayer
    }

    public enum PrayerType
    {
        VindeEspiritoSanto,
        OferecimentoDoDia,
        Angelus,
        CoroaDaMisericordia,
        Magnificat,
        ExameConsciencia
    }

    public class PrayerWrapper
    {
        public string Title { get; set; }
        public string Prayer { get; set; }
        public string Jaculatoria1 { get; set; }
        public string Jaculatoria2 { get; set; }
        public string Jaculatoria3 { get; set; }
    }
}
