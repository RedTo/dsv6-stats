using System;
using System.Collections;
using System.Collections.Generic;
namespace dsv6_stats
{
    public class Veranstaltung
    {
        public List<Abschnitt> Abschnitte;
        public Dictionary<int, Abschnitt> AbschnittMap;
        public Veranstaltung()
        {
            Abschnitte = new List<Abschnitt>();
            AbschnittMap = new Dictionary<int, Abschnitt>();
        }

        public List<string> GetPZKeys(){
            List<string> ret = new List<string>();
            foreach(Abschnitt abs in Abschnitte){
                foreach(string key in abs.GetPZKeys()){
                    if (!ret.Contains(key))
                        ret.Add(key);
                }
            }
            return ret;
        }

        public void AddAbschnitt(Abschnitt abs) {
            Abschnitte.Add(abs);
            AbschnittMap.Add(abs.number, abs);
        }

        public void AddWettkampf(int absnr, Wettkampf wk){
            AbschnittMap.GetValueOrDefault(absnr).WKList.Add(wk);
        }

        public void printOverview(string file){
            var f = new System.IO.StreamWriter(file,false,System.Text.Encoding.UTF8);
            var keys = GetPZKeys();
            keys.Sort();
            f.Write(";;");
            foreach (string key in keys)
            {
                f.Write(key + ";");
            }
            f.WriteLine(";;");
            foreach(Abschnitt abs in Abschnitte){
                f.WriteLine("Abschnitt;{0};;", abs.number);
                foreach(Wettkampf wk in abs.WKList){
                    f.Write("{0};{1};",wk.WkNr,wk.WkKey);
                    foreach(string key in keys){
                        if(wk.pz.ContainsKey(key)){
                            var time = wk.pz.GetValueOrDefault(key);
                            if (time.StartsWith("00:") && time.Length == 11)
                                time = time.Remove(0, 3);
                            f.Write(time+";");
                        } else {
                            f.Write("--:--,--;");
                        }
                    }
                    f.WriteLine(";;");
                }
                f.WriteLine(";;");
            }
        }
    }
}
