using UnityEngine;
using System.Collections;

/*
 * 
 *  Version.cs
 * 
 * 
 *   Version keeping constants and methods
 * 
 * 
 * 
 *   sep 2016 by Emilio Pomares
 * 
 * 	 (c) FlyGames
 * 
 * 
 */

public enum VersionComparisonResult { error, firstLower, equal, firstHigher };
public enum VersionType { development, demo, beta, alpha, rc, retail };

public class Version : MonoBehaviour {

	/* references */

	/* properties */
	int state = 0;


	/* constants */
	const string CurrentVersion = "0.1.1";
	const string DevelopmentVersion = "1012";
	public const VersionType versionType = VersionType.development;
	const bool showVersion = true;
	const int rcNumber = 0;


	/* methods */
	public static string checkLatestVersion()
	{		
		WWW www;
		string uuid = SystemInfo.deviceUniqueIdentifier;

		WWWForm form = new WWWForm ();
		form.AddField ("uuid", uuid);

		string script = Utils.WisdominiServer + "/checkVersion";
		www = new WWW (script, form);

		if (www != null) {
			while (!www.isDone) {
				// wait?!?
			}
			return www.text;
		} else
			return "0.0.0"; // lowest possible version, to prevent update	
	}

	public static bool mustShowVersion() 
	{
		return showVersion;
	}
		

	public static string currentVersion() 
	{
		return CurrentVersion;
	}
		
	public static VersionComparisonResult compareVersions(string v1, string v2) 
	{
		char[] separator = { '.' };
		string[] chop1 = v1.Split (separator);
		string[] chop2 = v2.Split (separator);
		if (chop1.Length != 3)
			return VersionComparisonResult.error;
		if (chop2.Length != 3)
			return VersionComparisonResult.error;

		// temporal variables
		int value1, value2;

		// check major version
		int.TryParse (chop1[0], out value1);
		int.TryParse (chop2[0], out value2);
		if (value1 < value2)
			return VersionComparisonResult.firstLower;
		else if (value1 > value2)
			return VersionComparisonResult.firstHigher;

		// if flow reaches this point, 
		// major version is equal, check minor version
		int.TryParse(chop1[1], out value1);
		int.TryParse(chop2[1], out value2);
		if (value1 < value2)
			return VersionComparisonResult.firstLower;
		else if (value1 > value2)
			return VersionComparisonResult.firstHigher;		

		// minor version also equal, check revision
		int.TryParse(chop1[1], out value1);
		int.TryParse(chop2[1], out value2);
		if (value1 < value2)
			return VersionComparisonResult.firstLower;
		else if (value1 > value2)
			return VersionComparisonResult.firstHigher;

		// both versions equal
		return VersionComparisonResult.equal;
	}

	// compare v to CurrentVersion
	public static VersionComparisonResult compareVersions(string v) 
	{
		return compareVersions (v, CurrentVersion);
	}

	public static bool newVersionAvailable() 
	{
		return compareVersions (checkLatestVersion(), CurrentVersion) == VersionComparisonResult.firstHigher;
	}

	public static string getVersionString() 
	{
		string result = "0.0.0";

		if (versionType == VersionType.development)
		{
			result = CurrentVersion + "." + DevelopmentVersion + " devel";
		}
		else if (versionType == VersionType.demo) 
		{
			result = CurrentVersion + " demo";
		} 
		else if (versionType == VersionType.beta) 
		{
			result = CurrentVersion + " beta";
		} 
		else if (versionType == VersionType.rc) 
		{
			result = CurrentVersion + " rc" + rcNumber;
		} 
		else if (versionType == VersionType.retail) 
		{
			result = CurrentVersion;
		}
		return result;
	}
}
