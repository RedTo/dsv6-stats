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
            Abschnitt abs;
            AbschnittMap.TryGetValue(absnr, out abs);
            abs.WKList.Add(wk);

        }

        public void printOverview(string file){
            var f = new System.IO.StreamWriter(file,false,System.Text.Encoding.UTF8);
            var keys = GetPZKeys();
            keys.Sort();
            keys.Reverse();

            f.Write(",,,,");
            int nkeys = 0;
            foreach (string key in keys)
            {
                f.Write("\"" + key + "\"" + ",");
                nkeys++;
            }
            f.WriteLine(",,");

            foreach(Abschnitt abs in Abschnitte){
                f.WriteLine("\"Abschnitt\",\"{0}\",\"{1}\",{2}", abs.number,abs.date.ToShortDateString(),"".PadRight(nkeys+3,','));
                foreach(Wettkampf wk in abs.WKList){

                    f.Write("\"{0}\",\"{1}\",\"{2}\",\"{3}\",",wk.WkNr,wk.WkKey,wk.sex,wk.typ);
                    foreach(string key in keys){
                        if(wk.pz.ContainsKey(key)){
                            string time;
                             wk.pz.TryGetValue(key,out time);
                            if (time.StartsWith("00:") && time.Length == 11)
                                time = time.Remove(0, 3);
                            f.Write("\"" + time + "\",");
                        } else {
                            f.Write("\"-\",");
                        }
                    }
                    f.WriteLine(",,");

                }
                f.WriteLine("".PadRight(nkeys + 4+2, ','));
            }
            f.Close();
        }
    }
}
