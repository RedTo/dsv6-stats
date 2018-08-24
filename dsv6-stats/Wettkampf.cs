using System;
using System.Collections;
using System.Collections.Generic;
namespace dsv6_stats
{
    public class Wettkampf
    {
        public string WkNr;
        public string WkKey;
        public string sex;
        public string typ;
        public Dictionary<string, string> pz;
        public Wettkampf(string nr, string key, string sex,string typ)
        {
            this.pz = new Dictionary<string, string>();
            this.sex = sex;
            this.WkNr = nr;
            this.WkKey = key;
            this.typ = typ;
        }

        public List<string> GetPZKeys()
        {
            List<string> ret = new List<string>();
            foreach (string key in pz.Keys)
            {
                ret.Add(key);
            }
            return ret;
        }

        public void AddPZ(string key, string time){
            pz.Add(key, time);
        }
    }
}
