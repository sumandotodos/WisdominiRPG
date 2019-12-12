using UnityEngine;
using System.Collections;

public class TextSweeper {


	public static string colorFromShade(string rgbcol, int nShades, int shade) {

		float alpha = 255.0f;
		float alphaDelta = 255.0f / (float)nShades;
		alphaDelta = alphaDelta * shade;
		if (alphaDelta > 255.0)
			alphaDelta = 255.0f;
		int intRes = (int)alphaDelta;
		return rgbcol + intRes.ToString ("X2");

	}

	public static string sweepIn(string original, int nShades, int position, string rgbcolor) {

		string res = "";

		if (position < nShades) { // starting case
							

			for (int i = 0; i < position; ++i) {

				res += "<color=" + colorFromShade(rgbcolor, nShades, nShades - position + i) + ">" + original.Substring (i, 1) +
				"</color>";

			}

			res += "<color=#"+rgbcolor+"00>" + original.Substring (position, original.Length - position) +
			"</color>";

		}

		if ((position >= nShades) && (position < (original.Length - nShades))) { // middle case

			res = "<color=#"+rgbcolor+"ff>" + original.Substring (0, position - nShades) + "</color>";
			for (int i = 0; i < nShades; ++i) {

				res += "<color=" + colorFromShade(rgbcolor, nShades, i) + ">" + original.Substring (position - nShades + i, 1) +
				"</color>";

			}
			res += "<color=#"+rgbcolor+"00>" + original.Substring (position, original.Length - position) +
			"</color>";

		}

		if (position >= (original.Length - nShades)) { // ending case

			res = "<color=#"+rgbcolor+"ff>" + original.Substring (0, position) + "</color>";
			for (int i = 0; i < original.Length - position; ++i) {

				res += "<color=" + colorFromShade(rgbcolor, nShades, i) + ">" + original.Substring (position + i, 1) +
				"</color>";

			}

		}

		return res;

	}


	

	//public static string sweepOut(string original, int nShades, int position) {

	//}

}
