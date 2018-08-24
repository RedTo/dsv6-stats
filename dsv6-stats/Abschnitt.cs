using System;
using System.Collections.Generic;
namespace dsv6_stats
{
    public class Abschnitt
    {
        public int number;
        public DateTime date;
        public List<Wettkampf> WKList;

        public Abschnitt(int number, DateTime date)
        {
            this.number = number;
            this.date = date;
            WKList = new List<Wettkampf>();
        }

        public List<string> GetPZKeys()
        {
            List<string> ret = new List<string>();
            foreach (Wettkampf wk in WKList)
            {
                foreach (string key in wk.GetPZKeys())
                {
                    if (!ret.Contains(key))
                        ret.Add(key);
                }
            }
            return ret;
        }
    }
}
