using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HiraganaToRomajiConversion {
  //https://happylilac.net/roman-hyo2.pdf
  public enum RomajiStyle : int {
    Kunrei,
    Hebon
  }
  public partial class RomajiConverter {
    Dictionary<string, string> dicOne = new Dictionary<string, string>();
    Dictionary<string, string> dicTwo = new Dictionary<string, string>();
    protected RomajiStyle style = RomajiStyle.Hebon;
    public RomajiStyle Style => style;
    public RomajiConverter() {
      string hiraOne = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん　ゃゅょぇゑー・" +
        "がぎぐげござじずぜぞだぢづでどばびぶべぼ" +
        "ぱぴぷぺぽ";
      string hiraTwo = "きゃきゅきょしゃしゅしょちゃちゅちょにゃにゅにょひゃひゅひょみゃみゅみょりゃりゅりょぎゃぎゅぎょじゃじゅじょぢゃぢゅぢょびゃびゅびょぴゃぴゅぴょ";
      string hiraTsu = "っ";
      string romaTwo= "a i u e o kakikukekosasisusesotatitutetonaninunenohahihuhehomamimumemoyayuyorarirurerowa" +
        (style == RomajiStyle.Hebon ? "wo" : "o ") + "n _ yayuyoe ye- . " +
        "gagigugegozazizuzezodadidudedobabibubebo" +
        "papipupepo";
      string romaTrio = "kyakyukyo" + (style == RomajiStyle.Hebon ? "shashusho" : "syasyusyo") + (style == RomajiStyle.Hebon ? "chachucho" : "tyatyutyo") +
        "nyanyunyo" + "hyahyuhyo" + "myamyumyo" + "ryaryuryo" + "gyagyugyo" +
        (style == RomajiStyle.Hebon ? "jyajyujyo" : "zyazyuzyo") +
        (style == RomajiStyle.Hebon ? "jyajyujyo" : "zyazyuzyo") +
        "byabyubyo" + "pyapyupyo";
      try {
        for (int i = 0, j = 0; i < hiraOne.Length; ++i, j += 2) {
          string l, r;
          dicOne.Add(l = hiraOne.Substring(i, 1), r = romaTwo.Substring(j, 2).Trim());
          debug(l, r);
        }
        for (int i = 0, j = 0; i < hiraTwo.Length; i += 2, j += 3) {
          string l, r;
          dicTwo.Add(l = hiraTwo.Substring(i, 2), r = romaTrio.Substring(j, 3));
          debug(l, r);
        }
        dicOne.Add(hiraTsu, "tu");
      } catch (Exception ex) {
        throw ex;
      }
    }
    public virtual IEnumerable<string> ShowDicOne() {
      foreach (KeyValuePair<string, string> dic in dicOne) {
        yield return String.Format("{0}={1} ", dic.Key, dic.Value);
      }
      debug("\n");
    }
    public virtual IEnumerable<string> ShowDicTwo() {
      foreach (KeyValuePair<string, string> dic in dicTwo) {
        yield return String.Format("{0}={1} ", dic.Key, dic.Value);
      }
      debug("\n");
    }
    protected virtual void debug(string l, string r) {
    }
    protected virtual void debug(string format,params object[] args){
    }
    public string ConvertHiraganaToRomaji(string text, bool topcapital = true) {
      CheckIfKatakana(ref text);
      // https://www.lexilogos.com/keyboard/hiragana_conversion.htm
      string roma = String.Empty;
      for (int i = 0; i < text.Length; ++i) {
        string key = text.Substring(i, 1);
        string two = text.Substring(i, i < text.Length - 1 ? 2 : 1);
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
        roma = roma.Substring(0, 1).ToUpperInvariant() + roma.Substring(1, roma.Length - 1);
      }
      return roma;
    }
    protected void CheckIfKatakana(ref string text) {
      Match Ma = Regex.Match(text, @"[\u30A1-\u30F6][\u30FA\u30FC]*");
      if (Ma.Success) {
        text = ConvertUnicode(text, false);
        debug("Converted:{0}",text);
      }
    }
    protected string ConvertUnicode(string text, bool toKatakana) {
      byte[] codes = Encoding.Unicode.GetBytes(text);
      byte[] kanas = new byte[codes.Length];
      for (int i = 0, j = 1; i < codes.Length; i += 2, j += 2) {
        byte a = codes[i];
        byte b = codes[j];
        int code = swap(ref a, ref b);
        debug("0x{0:X2}{1:X2}=0x{2:X4}",a,b,code);
        if (code != 0x30FC && code != 0x30FB) {
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
    protected int swap(ref byte a, ref byte b) {
      byte c = a;
      a = b;
      b = c;
      return (a << 8) | b;
    }
  }
}
