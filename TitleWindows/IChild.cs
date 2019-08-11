using System.Collections.Generic;

namespace TileWindows {
	interface IChild<T,B> {
		void RecollectFontFamilies();
		bool EitherOf(string name);
		void ToggleTimer();
		string Tooltip { get; }
		void MainTick(object sender);
		void CollectColors();
		IEnumerable<KeyValuePair<string,T>> Cores { get; }
		string ToColorName(T c);
		string ToString(T c);
		B Invert(T c);
	}
}
