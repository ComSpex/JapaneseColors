using System;
using System.Collections.Generic;
using System.Reflection;

namespace WpfAppone {
  /// <summary>
  /// JapaneseColors
  /// </summary>
  public partial class JapaneseColors {
		public static bool SkipAlias=false;
		static JapaneseColors() { }
		void Report(string format,params object[] args) {
			string text = String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text);
		}
		public IEnumerable<KeyValuePair<string,NamedSolidColorBrush>> Cores {
			get {
				FieldInfo[] props=this.GetType().GetFields(BindingFlags.Static|BindingFlags.Public);
				List<NamedSolidColorBrush> list = new List<NamedSolidColorBrush>();
				Dictionary<NamedSolidColorBrush,string> dics = new Dictionary<NamedSolidColorBrush,string>();
				foreach(FieldInfo prop in props) {
					if(prop==null) { continue; }
					try {
						NamedSolidColorBrush nscb = prop.GetValue(null) as NamedSolidColorBrush;
						if(nscb==null) { continue; }
						nscb.Kanji=prop.Name;
						if(list.Contains(nscb)) { continue; }
						list.Add(nscb);
						dics.Add(nscb,prop.Name);
					} catch { }
				}
				list.Sort();
				foreach(NamedSolidColorBrush nscb in list) {
					if(nscb==null) { continue; }
					if(nscb.IsAlias) {
						if(SkipAlias) {
							continue;
						}
					}
					yield return new KeyValuePair<string,NamedSolidColorBrush>(dics[nscb],nscb);
				}
			}
		}
		/// <summary>
		/// Traditional colors in Japan
		/// </summary>
		public static NamedSolidColorBrush 一斤染		=new NamedSolidColorBrush(252,212,213,"いっこんぞめ");
		public static NamedSolidColorBrush 桃色			=new NamedSolidColorBrush(249,174,165,"ももいろ");
		public static NamedSolidColorBrush 紅梅			=new NamedSolidColorBrush(246,144,150,"こうばい");
		public static NamedSolidColorBrush 中紅２			=new NamedSolidColorBrush(200,081,121,"なかべに");
		public static NamedSolidColorBrush 桜色			=new NamedSolidColorBrush(253,221,218,"さくらいろ");
		public static NamedSolidColorBrush 退紅３			=new NamedSolidColorBrush(208,114,122,"あらぞめ");
		public static NamedSolidColorBrush 薄紅			=new NamedSolidColorBrush(248,165,173,"うすべに");
		public static NamedSolidColorBrush 鴇色２			=new NamedSolidColorBrush(251,194,188,"ときいろ");
		public static NamedSolidColorBrush 桜鼠			=new NamedSolidColorBrush(216,198,188,"さくらねずみ");
		public static NamedSolidColorBrush 長春色		=new NamedSolidColorBrush(238,141,123,"ちょうしゅんいろ");
		public static NamedSolidColorBrush 唐紅			=new NamedSolidColorBrush(234,062,011,"からくれない");
		public static NamedSolidColorBrush 臙脂			=new NamedSolidColorBrush(160,003,047,"えんじ");
		public static NamedSolidColorBrush 深緋			=new NamedSolidColorBrush(194,000,036,"こきあけ,こきひ");
		public static NamedSolidColorBrush 甚三紅２		=new NamedSolidColorBrush(238,130,124,"じんざもみ");// http://dictionary.goo.ne.jp/leaf/jn2/114253/m0u/
		public static NamedSolidColorBrush 水柿２			=new NamedSolidColorBrush(228,171,155,"みずがき");
		public static NamedSolidColorBrush 梅鼠			=new NamedSolidColorBrush(173,121,132,"うめねずみ");
		public static NamedSolidColorBrush 蘇芳香２		=new NamedSolidColorBrush(168,105,101,"すおうこう");
		public static NamedSolidColorBrush 赤紅			=new NamedSolidColorBrush(197,061,067,"あかべに");
		public static NamedSolidColorBrush 真朱２			=new NamedSolidColorBrush(236,109,113,"しんしゅ,まおそ");
		public static NamedSolidColorBrush 小豆色		=new NamedSolidColorBrush(160,073,064,"あずきいろ");
		public static NamedSolidColorBrush 銀朱２			=new NamedSolidColorBrush(200,085,084,"ぎんしゅ");
		public static NamedSolidColorBrush 海老茶２		=new NamedSolidColorBrush(141,041,030,"えびちゃ");
		public static NamedSolidColorBrush 栗梅２			=new NamedSolidColorBrush(108,025,018,"くりうめ");
		public static NamedSolidColorBrush 曙色３			=new NamedSolidColorBrush(249,163,131,"あけぼのいろ");
		public static NamedSolidColorBrush 珊瑚礁色		=new NamedSolidColorBrush(245,121,093,"さんごしょういろ");
		public static NamedSolidColorBrush 猩々緋		=new NamedSolidColorBrush(231,000,029,"しょうじょうひ");
		public static NamedSolidColorBrush 芝翫茶２		=new NamedSolidColorBrush(185,108,000,"しかんちゃ");	// http://www.shikanko.co.jp/shikanko/
		public static NamedSolidColorBrush 柿渋色２		=new NamedSolidColorBrush(189,120,098,"かきしぶいろ");
		public static NamedSolidColorBrush 団十郎茶２		=new NamedSolidColorBrush(189,120,098,"だんじゅうろうちゃ",true);
		public static NamedSolidColorBrush 紅樺			=new NamedSolidColorBrush(182,061,027,"べにかば");
		public static NamedSolidColorBrush 紅鳶			=new NamedSolidColorBrush(154,073,063,"べにとび");
		public static NamedSolidColorBrush 紅檜皮		=new NamedSolidColorBrush(123,071,065,"べにひはだ");
		public static NamedSolidColorBrush 黒鳶			=new NamedSolidColorBrush(067,047,047,"くろとび");
		public static NamedSolidColorBrush 紅緋			=new NamedSolidColorBrush(254,018,013,"べにひ");
		public static NamedSolidColorBrush 照柿			=new NamedSolidColorBrush(248,146,100,"てりかき");
		public static NamedSolidColorBrush 緋２				=new NamedSolidColorBrush(186,038,054,"あけ");
		public static NamedSolidColorBrush 紅柄色２		=new NamedSolidColorBrush(170,086,046,"べんがらいろ");
		public static NamedSolidColorBrush 檜皮色		=new NamedSolidColorBrush(107,066,036,"ひはだいろ");
		public static NamedSolidColorBrush 宍色２			=new NamedSolidColorBrush(239,171,147,"ししいろ");
		public static NamedSolidColorBrush 赤香色２		=new NamedSolidColorBrush(246,184,148,"あかこういろ");
		public static NamedSolidColorBrush 黄丹２			=new NamedSolidColorBrush(247,130,037,"おうたん");//皇太子色
		public static NamedSolidColorBrush 纁２				=new NamedSolidColorBrush(251,160,039,"そひ");
		public static NamedSolidColorBrush 蘇比２			=new NamedSolidColorBrush(251,160,039,"そひ",true);
		public static NamedSolidColorBrush 遠州茶２		=new NamedSolidColorBrush(202,130,105,"えんしゅうちゃ");
		public static NamedSolidColorBrush 唐茶２			=new NamedSolidColorBrush(160,103,005,"からちゃ");
		public static NamedSolidColorBrush 樺茶２			=new NamedSolidColorBrush(114,098,080,"かばちゃ");
		public static NamedSolidColorBrush 雀茶２			=new NamedSolidColorBrush(152,046,033,"すずめちゃ");
		public static NamedSolidColorBrush 栗皮茶２		=new NamedSolidColorBrush(130,069,034,"くりかわちゃ");
		public static NamedSolidColorBrush 百塩茶２		=new NamedSolidColorBrush(084,045,036,"ももしおちゃ");
		public static NamedSolidColorBrush 鳶色２			=new NamedSolidColorBrush(122,056,015,"とびいろ");
		public static NamedSolidColorBrush 胡桃染２		=new NamedSolidColorBrush(160,100,112,"くるみぞめ");
		public static NamedSolidColorBrush 樺色２			=new NamedSolidColorBrush(197,089,026,"かばいろ");
		public static NamedSolidColorBrush 黄櫨染３		=new NamedSolidColorBrush(215,075,034,"こうろぜん");//天皇色
		public static NamedSolidColorBrush 焦茶２			=new NamedSolidColorBrush(106,077,050,"こげちゃ");
		public static NamedSolidColorBrush 深支子２		=new NamedSolidColorBrush(239,187,044,"こきくちなし");
		public static NamedSolidColorBrush 洗柿２			=new NamedSolidColorBrush(240,182,148,"あらいがき");
		public static NamedSolidColorBrush 代赭色２		=new NamedSolidColorBrush(179,108,060,"たいしゃいろ");
		public static NamedSolidColorBrush 赤白橡２		=new NamedSolidColorBrush(254,210,174,"あかしろつるばみ");
		public static NamedSolidColorBrush 礪茶２			=new NamedSolidColorBrush(159,111,085,"とのちゃ");
		public static NamedSolidColorBrush 洒落柿２		=new NamedSolidColorBrush(247,189,143,"しゃれがき");
		public static NamedSolidColorBrush 薄柿			=new NamedSolidColorBrush(212,172,173,"うすがき");
		public static NamedSolidColorBrush 萱草色２		=new NamedSolidColorBrush(253,169,000,"かんぞういろ");
		public static NamedSolidColorBrush 梅染２			=new NamedSolidColorBrush(180,138,118,"うめぞめ");
		public static NamedSolidColorBrush 紅鬱金２		=new NamedSolidColorBrush(203,131,071,"べにうこん");
		public static NamedSolidColorBrush 憲法染		=new NamedSolidColorBrush(087,071,056,"けんぽうぞめ");
		public static NamedSolidColorBrush 枇杷茶２		=new NamedSolidColorBrush(174,124,079,"びわちゃ");
		public static NamedSolidColorBrush 琥珀色２		=new NamedSolidColorBrush(234,147,010,"こはくいろ");
		public static NamedSolidColorBrush 淡香２			=new NamedSolidColorBrush(243,191,136,"うすこう");
		public static NamedSolidColorBrush 朽葉			=new NamedSolidColorBrush(145,115,071,"くちば");
		public static NamedSolidColorBrush 金茶２			=new NamedSolidColorBrush(206,122,025,"きんちゃ");
		public static NamedSolidColorBrush 丁子染２		=new NamedSolidColorBrush(221,184,126,"ちょうじぞめ");
		public static NamedSolidColorBrush 狐色２			=new NamedSolidColorBrush(217,151,047,"きつねいろ");
		public static NamedSolidColorBrush 伽羅色２		=new NamedSolidColorBrush(216,163,115,"きゃらいろ");
		public static NamedSolidColorBrush 煤竹			=new NamedSolidColorBrush(091,049,035,"すすたけ");
		public static NamedSolidColorBrush 白茶２			=new NamedSolidColorBrush(218,196,165,"しらちゃ");
		public static NamedSolidColorBrush 黄土色２		=new NamedSolidColorBrush(195,145,067,"おうどいろ");
		public static NamedSolidColorBrush 黄唐茶２		=new NamedSolidColorBrush(237,211,161,"うすき");
		public static NamedSolidColorBrush 山吹色２		=new NamedSolidColorBrush(255,197,037,"やまぶきいろ");
		public static NamedSolidColorBrush 玉子色		=new NamedSolidColorBrush(255,223,133,"たまごいろ");
		public static NamedSolidColorBrush 櫨染			=new NamedSolidColorBrush(217,166,046,"はじぞめ");
		public static NamedSolidColorBrush 山吹茶２		=new NamedSolidColorBrush(200,153,050,"やまぶきちゃ");
		public static NamedSolidColorBrush 桑染３			=new NamedSolidColorBrush(218,188,145,"くわぞめ");
		public static NamedSolidColorBrush 生壁色２		=new NamedSolidColorBrush(170,140,099,"なまかべいろ");
		public static NamedSolidColorBrush 支子			=new NamedSolidColorBrush(255,216,120,"くちなし");
		public static NamedSolidColorBrush 玉蜀黍色２		=new NamedSolidColorBrush(238,195,098,"とうもろこしいろ");
		public static NamedSolidColorBrush 白橡２			=new NamedSolidColorBrush(203,185,148,"しろつるばみ");
		public static NamedSolidColorBrush 黄橡			=new NamedSolidColorBrush(162,112,051,"きつるばみ");
		public static NamedSolidColorBrush 藤黄２			=new NamedSolidColorBrush(247,193,020,"とうおう");
		public static NamedSolidColorBrush 花葉色２		=new NamedSolidColorBrush(251,210,107,"はなばいろ");
		public static NamedSolidColorBrush 鳥の子色２		=new NamedSolidColorBrush(255,232,217,"とりのこいろ");
		public static NamedSolidColorBrush 鬱金色２		=new NamedSolidColorBrush(247,194,041,"うこんいろ");
		public static NamedSolidColorBrush 黄朽葉		=new NamedSolidColorBrush(203,140,045,"きくちば");
		public static NamedSolidColorBrush 利休白茶		=new NamedSolidColorBrush(208,193,156,"りきゅうしらちゃ");
		public static NamedSolidColorBrush 利休茶		=new NamedSolidColorBrush(137,120,069,"りきゅうちゃ");
		public static NamedSolidColorBrush 灰汁色		=new NamedSolidColorBrush(188,176,156,"あくいろ");
		public static NamedSolidColorBrush 肥後煤竹２		=new NamedSolidColorBrush(137,120,088,"ひごすすたけ");
		public static NamedSolidColorBrush 路考茶		=new NamedSolidColorBrush(171,131,054,"ろこうちゃ");
		public static NamedSolidColorBrush 海松茶２		=new NamedSolidColorBrush(087,084,061,"みるちゃ");
		public static NamedSolidColorBrush 菜種油色		=new NamedSolidColorBrush(222,192,049,"なたねあぶらいろ");
		public static NamedSolidColorBrush 鶯茶			=new NamedSolidColorBrush(113,092,031,"うぐいすちゃ");
		public static NamedSolidColorBrush 菜の花色		=new NamedSolidColorBrush(252,217,000,"なのはないろ");
		public static NamedSolidColorBrush 刈安			=new NamedSolidColorBrush(255,222,000,"かりやす");
		public static NamedSolidColorBrush 黄檗２			=new NamedSolidColorBrush(252,226,063,"きはだ");
		public static NamedSolidColorBrush 蒸栗色		=new NamedSolidColorBrush(235,225,169,"むしくりいろ");
		public static NamedSolidColorBrush 青朽葉		=new NamedSolidColorBrush(173,162,080,"あおくちば");
		public static NamedSolidColorBrush 女郎花		=new NamedSolidColorBrush(242,242,176,"おみなえし");
		public static NamedSolidColorBrush 鶯色			=new NamedSolidColorBrush(138,123,041,"うぐいすいろ");
		public static NamedSolidColorBrush 鶸色			=new NamedSolidColorBrush(214,208,000,"ひわいろ");
		public static NamedSolidColorBrush 青白橡２		=new NamedSolidColorBrush(133,145,109,"あおしろつるばみ");
		public static NamedSolidColorBrush 璃寛茶		=new NamedSolidColorBrush(117,096,040,"りかんちゃ");
		public static NamedSolidColorBrush 藍媚茶		=new NamedSolidColorBrush(085,086,071,"あいこびちゃ");
		public static NamedSolidColorBrush 苔色			=new NamedSolidColorBrush(137,144,043,"こけいろ");
		public static NamedSolidColorBrush 海松色		=new NamedSolidColorBrush(102,101,042,"みるいろ");
		public static NamedSolidColorBrush 千歳茶		=new NamedSolidColorBrush(072,073,063,"せんざいちゃ");
		public static NamedSolidColorBrush 梅辛茶		=new NamedSolidColorBrush(170,167,081,"ばいこうちゃ");
		public static NamedSolidColorBrush 岩井茶		=new NamedSolidColorBrush(111,111,067,"いわいちゃ");
		public static NamedSolidColorBrush 柳煤竹		=new NamedSolidColorBrush(091,099,086,"やなぎすすたけ");
		public static NamedSolidColorBrush 裏柳			=new NamedSolidColorBrush(193,216,172,"うらやなぎ");
		public static NamedSolidColorBrush 淡萌黄		=new NamedSolidColorBrush(157,195,087,"うすもえぎ");
		public static NamedSolidColorBrush 萌黄			=new NamedSolidColorBrush(167,189,000,"もえぎ");
		public static NamedSolidColorBrush 松葉色		=new NamedSolidColorBrush(094,134,048,"まつばいろ");
		public static NamedSolidColorBrush 薄青			=new NamedSolidColorBrush(147,182,156,"うすあお");
		public static NamedSolidColorBrush 若竹色		=new NamedSolidColorBrush(124,194,142,"わかたけいろ");
		public static NamedSolidColorBrush 千歳緑		=new NamedSolidColorBrush(051,087,025,"せんざいみどり");
		public static NamedSolidColorBrush 緑				=new NamedSolidColorBrush(062,179,112,"みどり");
		public static NamedSolidColorBrush 白緑			=new NamedSolidColorBrush(218,234,208,"びゃくろく");
		public static NamedSolidColorBrush 錆青磁		=new NamedSolidColorBrush(166,200,178,"さびせいじ");
		public static NamedSolidColorBrush 緑青			=new NamedSolidColorBrush(103,152,118,"ろくしょう");
		public static NamedSolidColorBrush 木賊色		=new NamedSolidColorBrush(061,110,060,"とくさいろ");
		public static NamedSolidColorBrush 御納戸茶		=new NamedSolidColorBrush(061,101,105,"おなんどちゃ");
		public static NamedSolidColorBrush 青竹色		=new NamedSolidColorBrush(123,194,146,"あおたけいろ");
		public static NamedSolidColorBrush 利休鼠		=new NamedSolidColorBrush(140,134,132,"りきゅうねずみ");
		public static NamedSolidColorBrush 沈香茶２		=new NamedSolidColorBrush(067,103,082,"とのちゃ");
		public static NamedSolidColorBrush 水浅葱		=new NamedSolidColorBrush(140,210,188,"みずあさぎ");
		public static NamedSolidColorBrush 青碧			=new NamedSolidColorBrush(071,131,132,"せいへき");
		public static NamedSolidColorBrush 鉄色			=new NamedSolidColorBrush(016,046,036,"てついろ");
		public static NamedSolidColorBrush 高麗納戸		=new NamedSolidColorBrush(016,069,057,"こうらいなんど");
		public static NamedSolidColorBrush 湊鼠			=new NamedSolidColorBrush(187,188,186,"みなとねずみ");
		public static NamedSolidColorBrush 青鈍			=new NamedSolidColorBrush(050,067,086,"あおにび");
		public static NamedSolidColorBrush 鉄御納戸		=new NamedSolidColorBrush(069,087,101,"てつおなんど");
		public static NamedSolidColorBrush 水色			=new NamedSolidColorBrush(127,204,227,"みずいろ");
		public static NamedSolidColorBrush 瓶覗			=new NamedSolidColorBrush(168,203,205,"かめのぞき");
		public static NamedSolidColorBrush 浅葱			=new NamedSolidColorBrush(000,165,191,"あさぎ");
		public static NamedSolidColorBrush 新橋色		=new NamedSolidColorBrush(090,194,217,"しんばしいろ");
		public static	NamedSolidColorBrush 藍鼠			=new NamedSolidColorBrush(107,129,142,"あいねずみ");
		public static NamedSolidColorBrush 藍色			=new NamedSolidColorBrush(000,116,168,"あいいろ");
		public static NamedSolidColorBrush 御納戸色		=new NamedSolidColorBrush(017,125,138,"おなんどいろ");
		public static NamedSolidColorBrush 花浅葱		=new NamedSolidColorBrush(042,131,162,"はなあさぎ");
		public static NamedSolidColorBrush 千草色		=new NamedSolidColorBrush(048,139,174,"ちくさいろ");
		public static NamedSolidColorBrush 舛花色		=new NamedSolidColorBrush(091,126,145,"ますはないろ");
		public static NamedSolidColorBrush 縹				=new NamedSolidColorBrush(000,134,173,"はなだ");
		public static NamedSolidColorBrush 花田			=new NamedSolidColorBrush(000,134,173,"はなだ",true);
		public static NamedSolidColorBrush 熨斗目花色	=new NamedSolidColorBrush(066,101,121,"のしめはないろ");
		public static NamedSolidColorBrush 御召御納戸	=new NamedSolidColorBrush(076,100,115,"おめしおなんど");
		public static NamedSolidColorBrush 空色			=new NamedSolidColorBrush(115,184,226,"そらいろ");
		public static NamedSolidColorBrush 黒橡			=new NamedSolidColorBrush(050,044,040,"くろつるばみ");
		public static NamedSolidColorBrush 群青色		=new NamedSolidColorBrush(009,064,142,"ぐんじょういろ");
		public static NamedSolidColorBrush 紺				=new NamedSolidColorBrush(000,038,101,"こん");
		public static NamedSolidColorBrush 褐色２			=new NamedSolidColorBrush(000,056,071,"かちいろ");
		public static NamedSolidColorBrush 瑠璃色２		=new NamedSolidColorBrush(000,091,160,"るりいろ");
		public static NamedSolidColorBrush 紺青色		=new NamedSolidColorBrush(000,084,153,"こんじょういろ");
		public static NamedSolidColorBrush 瑠璃紺		=new NamedSolidColorBrush(035,076,142,"るりこん");
		public static NamedSolidColorBrush 紅碧			=new NamedSolidColorBrush(132,145,195,"べにみどり");
		public static NamedSolidColorBrush 紺桔梗		=new NamedSolidColorBrush(077,090,175,"こんききょう");
		public static NamedSolidColorBrush 藤鼠			=new NamedSolidColorBrush(166,165,196,"ふじねずみ");
		public static NamedSolidColorBrush 紅掛花色		=new NamedSolidColorBrush(104,105,155,"べにかけはないろ");
		public static NamedSolidColorBrush 藤色			=new NamedSolidColorBrush(186,167,204,"ふじいろ");
		public static NamedSolidColorBrush 二藍			=new NamedSolidColorBrush(136,138,188,"ふたあい");
		public static NamedSolidColorBrush 藤紫			=new NamedSolidColorBrush(154,143,189,"ふじむらさき");
		public static NamedSolidColorBrush 桔梗色		=new NamedSolidColorBrush(072,048,147,"ききょういろ");
		public static NamedSolidColorBrush 紫苑色		=new NamedSolidColorBrush(135,120,146,"しおんいろ");
		public static NamedSolidColorBrush 滅紫			=new NamedSolidColorBrush(089,066,085,"めっし");
		public static NamedSolidColorBrush 紫紺			=new NamedSolidColorBrush(064,011,054,"しこん");
		public static NamedSolidColorBrush 深紫			=new NamedSolidColorBrush(073,055,089,"こきむらさき");
		public static NamedSolidColorBrush 薄色			=new NamedSolidColorBrush(206,180,185,"うすいろ");
		public static	NamedSolidColorBrush 半色			=new NamedSolidColorBrush(166,154,189,"はしたいろ");
		public static NamedSolidColorBrush 菫色			=new NamedSolidColorBrush(112,101,163,"すみれいろ");
		public static NamedSolidColorBrush 紫				=new NamedSolidColorBrush(136,072,152,"むらさき");
		public static NamedSolidColorBrush 黒紅			=new NamedSolidColorBrush(048,040,051,"くろべに");
		public static NamedSolidColorBrush 菖蒲色		=new NamedSolidColorBrush(177,104,168,"あやめいろ");
		public static NamedSolidColorBrush 紅藤			=new NamedSolidColorBrush(204,166,191,"べにふじ");
		public static NamedSolidColorBrush 杜若			=new NamedSolidColorBrush(094,056,098,"かきつばた");
		public static NamedSolidColorBrush 江戸紫		=new NamedSolidColorBrush(094,056,098,"えどむらさき",true);
		public static NamedSolidColorBrush 鳩羽鼠		=new NamedSolidColorBrush(158,139,142,"はとばねずみ");
		public static NamedSolidColorBrush 葡萄鼠		=new NamedSolidColorBrush(112,091,103,"ぶどうねずみ");
		public static NamedSolidColorBrush 蒲葡			=new NamedSolidColorBrush(082,047,096,"えびぞめ");
		public static NamedSolidColorBrush 藤煤竹		=new NamedSolidColorBrush(090,083,089,"ふじすすたけ");
		public static NamedSolidColorBrush 牡丹			=new NamedSolidColorBrush(203,040,136,"ぼたん");
		public static NamedSolidColorBrush 梅紫			=new NamedSolidColorBrush(170,076,143,"うめむらさき");
		public static NamedSolidColorBrush 似せ紫		=new NamedSolidColorBrush(081,055,067,"にせむらさき");
		public static NamedSolidColorBrush 紫鳶			=new NamedSolidColorBrush(095,065,075,"むらさきとび");
		public static NamedSolidColorBrush 蘇芳			=new NamedSolidColorBrush(178,062,082,"すおう");
		public static NamedSolidColorBrush 桑の実色		=new NamedSolidColorBrush(085,041,091,"くわのみいろ");
		public static NamedSolidColorBrush 紅消鼠		=new NamedSolidColorBrush(082,071,072,"べにけしねずみ");
		public static NamedSolidColorBrush 白練			=new NamedSolidColorBrush(243,243,242,"しろねり");
		public static NamedSolidColorBrush 白鼠			=new NamedSolidColorBrush(220,221,221,"しろねずみ");
		public static NamedSolidColorBrush 銀鼠			=new NamedSolidColorBrush(216,213,204,"ぎんねずみ");
		public static NamedSolidColorBrush 素鼠			=new NamedSolidColorBrush(145,149,150,"すねずみ");
		public static NamedSolidColorBrush 丼鼠			=new NamedSolidColorBrush(089,084,085,"どぶねずみ");
		public static NamedSolidColorBrush 藍墨茶		=new NamedSolidColorBrush(071,074,077,"あいすみちゃ");
		public static NamedSolidColorBrush 檳榔子染		=new NamedSolidColorBrush(067,061,060,"びんろうじぞめ");
		public static NamedSolidColorBrush 墨				=new NamedSolidColorBrush(000,010,002,"すみ");
		public static NamedSolidColorBrush 黒色			=new NamedSolidColorBrush(000,000,000,"くろいろ");
	}
}
