using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringUtils {

	public static string chopLines(string input, int maxLineLength) {
		int dummy;
		return chopLines (input, maxLineLength, out dummy);
	}

	public static string chopLines(string input, int maxLineLength, out int nLines) {

		int numberOfLines = 1;

		int maxLine;
		char[] data;
		List<int> indexes = new List<int>();
		data = input.ToCharArray ();
		maxLine = input.Length;

		const int MAXLINES = 10;

		/* get list of all spaces */
		for (int i = 0; i < input.Length; ++i) {
			if (data [i] == ' ')
				indexes.Add (i);
		}

		int offset = maxLineLength;
		int indexOfLastSpace = input.LastIndexOf (' ');
		int Length = input.Length;
		if (indexOfLastSpace != -1) {
			Length -= (input.Length - indexOfLastSpace);
			while (offset < input.Length) {


				/* find closest space */
				int closest = -1;
				int minDistance;
				minDistance = input.Length * input.Length; // soft of absolute value
				for (int i = 0; i < indexes.Count; ++i) {
					if ((indexes [i] - offset) * (indexes [i] - offset) < minDistance) {
						minDistance = (indexes [i] - offset) * (indexes [i] - offset);
						closest = i;
					}
				}

				if (closest != -1) {
					data [indexes [closest]] = '\n';
					++numberOfLines;
				}
				if (offset != indexes [closest])
					offset = indexes [closest];
				offset += maxLineLength;

				if (numberOfLines > MAXLINES)
					break;

			}

		}

		nLines = numberOfLines;
		return new string (data);

	}



}
