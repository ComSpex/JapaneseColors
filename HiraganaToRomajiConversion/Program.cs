using System;
using System.Collections.Generic;
using ComSpex.JapaneseColors;

namespace HiraganaToRomajiConversion {
  class Program {
    static void Main(string[] args) {
      try {
        RomajiConverter rc = new RomajiConverter();
        Print(rc);
        Test(rc);
      } catch(Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.ToString());
        Console.ResetColor();
      }
    }
    private static void Print(RomajiConverter rc) {
      JapaneseColors jc = new JapaneseColors();
      foreach (KeyValuePair<string, NamedSolidColorBrush> core in jc.Cores) {
        try {
          Console.WriteLine("{0}={1}({2})", core.Value.Kanji, rc.ConvertHiraganaToRomaji(core.Value.Yomi), core.Value.Yomi);
        } catch {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("{0}={1}={2}", core.Value.Kanji, "N/A", core.Value.Yomi);
          Console.ResetColor();
          string[] yomis = core.Value.Yomi.Split(',');
          foreach(string yomi in yomis) {
            try {
              string yom = yomi.Trim();
              Console.WriteLine("\t{0}={1}({2})", core.Value.Kanji, rc.ConvertHiraganaToRomaji(yom), yom);
            } catch { }
          }
        }
      }
    }
    private static void Test(RomajiConverter rc) {
      for(;;) {
        Console.WriteLine("\nInput Hiragana or Katakana string and press Enter:");
        string text = Console.ReadLine();
        if (String.IsNullOrEmpty(text)) {
          return;
        }
        if (text.Equals("quit")||text.Contains("おわり")) {
          break;
        }
        string roma = rc.ConvertHiraganaToRomaji(text);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Romaji = '{0}'", roma);
        Console.ResetColor();
      }
    }
  }
}
