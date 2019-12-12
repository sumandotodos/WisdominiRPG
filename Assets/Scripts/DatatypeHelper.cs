using System;

public enum Datatype { dtInt, dtFloat, dtString };

public class DatatypeHelper
{

	static public DataType detectType(string strRep) {

		int nDots = 0;
		int nNonNumbers = 0;

		char []charArray;

		char c;

		if (strRep.Equals ("true"))
			return DataType.Bool;
		if (strRep.Equals ("false"))
			return DataType.Bool;

		charArray = strRep.ToCharArray();

		for(int i = 0; i< strRep.Length; ++i) {

			c = charArray[i];

			if(c=='.') ++nDots;
			if(!((c>='0') && (c<='9'))) ++nNonNumbers;

		}

		// sort type

		if(nDots == 0 && nNonNumbers == 0) return DataType.Int;

		if(nDots == 1 && nNonNumbers == 0) return DataType.Float;

		return DataType.String;

	}

	public DatatypeHelper() {

	}

}


