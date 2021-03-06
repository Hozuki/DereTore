using System;
using System.Globalization;
using System.IO;
using DereTore.Exchange.Audio.HCA;

namespace DereTore.Apps.Hcacc {
    internal static class Program {

        private static int Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine(HelpMessage);
                return -1;
            }
            var inputFileName = args[0];
            var outputFileName = args[1];
            CipherConfig ccFrom = new CipherConfig(), ccTo = new CipherConfig();
            try {
                for (var i = 0; i < args.Length; ++i) {
                    var arg = args[i];
                    if (arg[0] == '-' || arg[0] == '/') {
                        switch (arg.Substring(1)) {
                            case "ot":
                                if (i < args.Length - 1) {
                                    var us = ushort.Parse(args[++i]);
                                    if (us != 0 && us != 1 && us != 56) {
                                        Console.WriteLine("ERROR: invalid cipher type.");
                                        return -2;
                                    }
                                    ccTo.CipherType = (CipherType)us;
                                }
                                break;
                            case "i1":
                                if (i < args.Length - 1) {
                                    ccFrom.Key1 = uint.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                            case "i2":
                                if (i < args.Length - 1) {
                                    ccFrom.Key2 = uint.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                            case "im":
                                if (i < args.Length - 1) {
                                    ccFrom.KeyModifier = ushort.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                            case "o1":
                                if (i < args.Length - 1) {
                                    ccTo.Key1 = uint.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                            case "o2":
                                if (i < args.Length - 1) {
                                    ccTo.Key2 = uint.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                            case "om":
                                if (i < args.Length - 1) {
                                    ccTo.KeyModifier = ushort.Parse(args[++i], NumberStyles.HexNumber);
                                }
                                break;
                        }
                    }
                }
            } catch (Exception) {
                return -3;
            }
            try {
                using (var inputStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read)) {
                    using (var outputStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write)) {
                        var converter = new CipherConverter(inputStream, outputStream, ccFrom, ccTo);
                        converter.Convert();
                    }
                }
            } catch (Exception) {
                return -4;
            }
            return 0;
        }

        private static readonly string HelpMessage = "Usage: hcacc.exe <input HCA> <output HCA> [options]\n" +
                                                     "\n" +
                                                     "Options:\n" +
                                                     "  -ot <output cipher type>\n" +
                                                     "  -i1 <input key 1>\n" +
                                                     "  -i2 <input key 2>\n" +
                                                     "  -im <input key modifier>\n" +
                                                     "  -o1 <output key 1>\n" +
                                                     "  -o2 <output key 2>\n" +
                                                     "  -om <output key modifier>\n";

    }
}
