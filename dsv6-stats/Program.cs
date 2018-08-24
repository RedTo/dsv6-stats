using System;

namespace dsv6_stats
{
    class Program
    {
        static void Main(string[] args)
        {
            XtractData(args[1],args[2]);
        }

        public static void XtractData(string filename, string tfilename)
        {
            System.Console.WriteLine("Read DSV6 file: "+filename);
            var v = new Veranstaltung();
            string[] bs = BlockFile(filename); //Dateinhalte einlesen und vorbereiten
            int i = 0;
            var WKL = new System.Collections.Generic.Dictionary<string, Wettkampf>();
            do
            {
                if(bs[i].Trim().StartsWith("PFLICHTZEIT:")){
                    var xwknr = bs[i].Split(":")[1].Trim();
                    Wettkampf xwk;
                    WKL.TryGetValue(xwknr,out xwk); 
                    var pz = bs[i+5].Trim();
                    string cls; 
                    if (bs[i + 4] == bs[i + 3])
                        cls = bs[i + 3];
                    else
                        cls = bs[i + 3] + "-" + bs[i + 4];

                    if(bs[i + 3]=="0" || bs[i+3]=="")
                        cls = "-" + bs[i + 4];
                    if (bs[i + 4] == "9999"|| bs[i + 4] == "")
                        cls = bs[i + 3] + "-";
                    if ((bs[i + 3] == "0"|| bs[i+3]=="") && (bs[i + 4] == "9999"|| bs[i + 4] == ""))
                        cls = "offen";
                    var key = cls;
                    xwk.AddPZ(key, pz);
                }
                if(bs[i].Trim().StartsWith("ABSCHNITT:")){
                    var absnr= int.Parse(bs[i].Split(":")[1]);
                    var absdate = DateTime.Parse(bs[i + 1]);
                    var abs = new Abschnitt(absnr, absdate);
                    v.AddAbschnitt(abs);
                }
                if (bs[i].Trim().StartsWith("WETTKAMPF:"))
                {
                    var xwknr = bs[i].Split(":")[1].Trim();
                    var xabsnr = int.Parse(bs[i + 2]);
                    var xwkkey = bs[i + 4] + bs[i + 5] +" "+ bs[i + 7] + " " + bs[i + 1];
                    //System.Console.WriteLine(xwknr.ToString() + " " + xabsnr.ToString() + " " + xwkkey);
                    var wk = new Wettkampf(xwknr, xwkkey, bs[i + 7]);
                    WKL.Add(xwknr, wk);
                    v.AddWettkampf(xabsnr, wk);
                }
                i += 1;
                //System.Console.WriteLine(bs[i-1]);
                //System.Console.WriteLine(bs[i]);
            } while (i < bs.Length && bs[i] != "DATEIENDE");
            System.Console.WriteLine("Write CSV file: " + tfilename);
            v.printOverview(tfilename);
        }


        static string[] BlockFile(string file)
        {
            var tf = new System.IO.StreamReader(file, System.Text.Encoding.UTF8);
            var sb = new System.Text.StringBuilder();
            //System.Console.WriteLine("---");
            //Einlesen und Kommentare entfernen:
            while (!tf.EndOfStream) {
            outerloop:
                char _c, _cc;

                _c = Char.ConvertFromUtf32(tf.Read()).ToCharArray()[0];
                if (_c == '(') {
                    _cc = Char.ConvertFromUtf32(tf.Peek()).ToCharArray()[0];
                    if (_cc == '*') {
                        //Kommentar
                        tf.Read(); //*
                        while (!tf.EndOfStream) {
                            _c = Char.ConvertFromUtf32(tf.Read()).ToCharArray()[0];
                            if (_c == '*') {
                                _cc = Char.ConvertFromUtf32(tf.Peek()).ToCharArray()[0];
                                if (_cc == ')') {
                                    tf.Read(); //*) komplett verarbeiten
                                    goto outerloop;
                                }
                            }
                        }
                    } else {
                        sb.Append(_c);
                    }
                } else if (_c == '\r' || _c == '\n') {
                    // ignore linebreaks
                } else {
                    // process non-comment-(
                    sb.Append(_c);
                }
                //System.Console.WriteLine(_c);
            }
            //System.Console.WriteLine("---");
            return sb.ToString().Split(";");
        }
    }
}
