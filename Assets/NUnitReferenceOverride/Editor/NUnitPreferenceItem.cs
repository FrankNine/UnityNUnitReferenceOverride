using System;
using UnityEngine;
using UnityEditor;

public class NUnitPreferenceItem {

	private static bool _isPrefsLoaded;

	private const string IsOverridingNUnitReferenceEditorPrefsKey = "IsOverridingNUnitReference";
	private const string OverridingNUnitPathEditorPrefsKey = "OverridingNUnitPath";
	
	private static bool _isOverridingNUnitReference;
	private static string _overridingNUnitPath;
	
	public static bool IsOverridingNUnitReference
	{
		get { return EditorPrefs.GetBool(IsOverridingNUnitReferenceEditorPrefsKey, false); }
	}

	public static string OverridingNUnitPath
	{
		get{return EditorPrefs.GetString(OverridingNUnitPathEditorPrefsKey, String.Empty);}
	}
	
	[PreferenceItem("NUnit Override")]
	private static void PreferencesGUI()
	{
		if (!_isPrefsLoaded)
		{
			_isOverridingNUnitReference = IsOverridingNUnitReference;
			_overridingNUnitPath = OverridingNUnitPath;
			
			_isPrefsLoaded = true;
		}

		_isOverridingNUnitReference = EditorGUILayout.Toggle("Is Overriding .csproj NUnit reference", 
															 _isOverridingNUnitReference);
		GUI.enabled = _isOverridingNUnitReference;
		_overridingNUnitPath = EditorGUILayout.TextField(_overridingNUnitPath);
		GUI.enabled = true;

		if (GUI.changed)
		{
			EditorPrefs.SetBool(IsOverridingNUnitReferenceEditorPrefsKey, _isOverridingNUnitReference);
			EditorPrefs.SetString(OverridingNUnitPathEditorPrefsKey, _overridingNUnitPath);
		}
	}
}
