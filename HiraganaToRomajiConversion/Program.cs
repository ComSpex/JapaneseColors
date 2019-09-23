using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfAppone;

namespace HiraganaToRomajiConversion {
  class Program {
    static Dictionary<string, string> dicOne = new Dictionary<string, string>();
    static Dictionary<string, string> dicTwo = new Dictionary<string, string>();
    //https://happylilac.net/roman-hyo2.pdf
    static RomajiStyle style = RomajiStyle.Hebon;
    static void Main(string[] args) {
      string hiras = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん　ゃゅょぇゑー・" +
        "がぎぐげござじずぜぞだぢづでどばびぶべぼ" +
        "ぱぴぷぺぽ";
      string hiraPairs="きゃきゅきょしゃしゅしょちゃちゅちょにゃにゅにょひゃひゅひょみゃみゅみょりゃりゅりょぎゃぎゅぎょじゃじゅじょぢゃぢゅぢょびゃびゅびょぴゃぴゅぴょ";
      string hiraTsu = "っ";
      string romas = "a i u e o kakikukekosasisusesotatitutetonaninunenohahihuhehomamimumemoyayuyorarirurerowa"+
        (style==RomajiStyle.Hebon?"wo":"o ")+"n _ yayuyoe ye- . " +
        "gagigugegozazizuzezodadidudedobabibubebo" +
        "papipupepo";
      string romaTrio = "kyakyukyo"+(style==RomajiStyle.Hebon?"shashusho":"syasyusyo")+(style==RomajiStyle.Hebon?"chachucho":"tyatyutyo")+
        "nyanyunyo"+"hyahyuhyo"+"myamyumyo"+"ryaryuryo"+"gyagyugyo"+
        (style==RomajiStyle.Hebon?"jyajyujyo":"zyazyuzyo")+
        (style==RomajiStyle.Hebon?"jyajyujyo":"zyazyuzyo")+
        "byabyubyo"+"pyapyupyo";
#if false
      string kanas = ConvertUnicode(hiras, true);
      string hirag = ConvertUnicode(kanas, false);
      Console.WriteLine("\n{0}\n{1}",kanas,hirag);
      return;
#endif
      try {
        for (int i = 0, j = 0; i < hiras.Length; ++i, j += 2) {
          string l, r;
          dicOne.Add(l=hiras.Substring(i, 1), r=romas.Substring(j, 2).Trim());
          //Console.Write("({2}): {0}={1}", l, r,i);
        }
        for (int i = 0, j = 0;i<hiraPairs.Length; i += 2, j += 3) {
          string l, r;
          dicTwo.Add(l=hiraPairs.Substring(i, 2), r=romaTrio.Substring(j, 3));
          //Console.Write("({2}): {0}={1}", l, r, i);
        }
        dicOne.Add(hiraTsu,"tu");
        //ShowDicOne();
        Print();
        //Test();
      } catch (Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.ToString());
        Console.ResetColor();
      }
    }
    private static void ShowDicOne() {
      foreach (KeyValuePair<string, string> dic in dicOne) {
        Console.Write("{0}={1} ", dic.Key, dic.Value);
      }
      Console.WriteLine();
    }
    private static void Print() {
      JapaneseColors jc = new JapaneseColors();
      foreach (KeyValuePair<string, NamedSolidColorBrush> core in jc.Cores) {
        try {
          Console.WriteLine("{0}={1}({2})", core.Value.Kanji, ConvertHiraganaToRomaji(core.Value.Yomi), core.Value.Yomi);
        } catch {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("{0}={1}={2}", core.Value.Kanji, "N/A", core.Value.Yomi);
          Console.ResetColor();
          string[] yomis = core.Value.Yomi.Split(',');
          foreach(string yomi in yomis) {
            try {
              string yom = yomi.Trim();
              Console.WriteLine("\t{0}={1}({2})", core.Value.Kanji, ConvertHiraganaToRomaji(yom), yom);
            } catch { }
          }
        }
      }
    }
    private static void Test() {
      for(;;) {
        Console.WriteLine("\nInput Hiragana string and press Enter:");
        string text = Console.ReadLine();
        if (text.Equals("quit")||text.Contains("おわり")) {
          break;
        }
        CheckIfKatakana(ref text);
        string roma = ConvertHiraganaToRomaji(text);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Romaji = '{0}'", roma);
        Console.ResetColor();
      }
    }
    private static string ConvertHiraganaToRomaji(string text,bool topcapital = true) {
      CheckIfKatakana(ref text);
      // https://www.lexilogos.com/keyboard/hiragana_conversion.htm
      string roma = String.Empty;
      for (int i = 0; i < text.Length; ++i) {
        string key = text.Substring(i, 1);
        string two = text.Substring(i, i<text.Length-1?2:1);
        string r = String.Empty;
        if (dicTwo.Keys.Contains(two)) {
          r = dicTwo[two];
          ++i;
        } else {
          r = dicOne[key];
        }
        if (key.Equals("っ")) {
          key = text.Substring(i + 1, 1);
          r = dicOne[key];
          r = r.Substring(0, 1);
        }
        roma += r;
      }
      if (style == RomajiStyle.Hebon) {
        roma = roma.Replace("si", "shi");
        roma = roma.Replace("zi", "ji");
        roma = roma.Replace("tu", "tsu");
        roma = roma.Replace("ti", "chi");
        roma = roma.Replace("jya", "ja");
        roma = roma.Replace("jyu", "ju");
        roma = roma.Replace("jyo", "jo");
      }
      if (topcapital) {
        roma = roma.Substring(0, 1).ToUpperInvariant() + roma.Substring(1,roma.Length-1);
      }
      return roma;
    }
    private static void CheckIfKatakana(ref string text) {
      Match Ma = Regex.Match(text, @"[\u30A1-\u30F6][\u30FA\u30FC]*");
      if (Ma.Success) {
        text = ConvertUnicode(text, false);
        //Console.WriteLine("Converted:{0}",text);
      }
    }
    private static string ConvertUnicode(string text,bool toKatakana) {
      byte[] codes=Encoding.Unicode.GetBytes(text);
      byte[] kanas = new byte[codes.Length];
      for(int i = 0,j=1; i < codes.Length; i+=2,j+=2) {
        byte a = codes[i];
        byte b = codes[j];
        int code=swap(ref a, ref b);
        //Console.Write("0x{0:X2}{1:X2}=0x{2:X4}",a,b,code);
        if (code != 0x30FC&&code!=0x30FB) {
          if (toKatakana) {
            code += 0x60;
          } else {
            code -= 0x60;
          }
        }
        byte c = (byte)((code >> 8) & 0xff);
        byte d = (byte)(code & 0x00ff);
        swap(ref c, ref d);
        kanas[i] = c;
        kanas[j] = d;
      }
      return Encoding.Unicode.GetString(kanas);
    }
    private static int swap(ref byte a, ref byte b) {
      byte c = a;
      a = b;
      b = c;
      return (a << 8) | b;
    }
  }
}
