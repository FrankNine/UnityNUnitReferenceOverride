using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using UnityEngine;
using UnityEditor;

public class SolutionPostProcessor : AssetPostprocessor
{
	private const string CSPROJ_DEFAULT_FILENAME = "Assembly-CSharp-Editor.csproj";
	private const string CSPROJ_SCHEMA = "http://schemas.microsoft.com/developer/msbuild/2003";
	
	private static void OnGeneratedCSProjectFiles()
	{
#if UNITY_5_6_OR_NEWER && UNITY_EDITOR_WIN
		if (!NUnitPreferenceItem.IsOverridingNUnitReference) { return; }
	    OverrideNUnitReference();
#endif
    }

    public static void OverrideNUnitReference()
	{
		Debug.Log("Patching .csproj for overriding NUnit reference...");
		
		XNamespace ns = CSPROJ_SCHEMA; 
		
		string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
		string editorCSProjPath = _GetEditorCSProjPath(projectRootPath);
        if (string.IsNullOrEmpty(editorCSProjPath)) { return; }

        Debug.LogFormat("Patching {0}...", editorCSProjPath);

        var csprojDocument = XDocument.Load(editorCSProjPath);
		var referenceNodes = csprojDocument.Root.Descendants(ns + "Reference").ToArray();
		var nunitReferenceNodeArray = referenceNodes.Where(_IsNUnitReference).ToArray();
		if (nunitReferenceNodeArray.Length == 0)
		{
			Debug.LogWarning("Cannot find NUnit reference");
			return;
		}
		
		var nunitReferenceNode = nunitReferenceNodeArray.First();
		var nunitHintPathNode = nunitReferenceNode.Element(ns + "HintPath");
		nunitHintPathNode.Value = NUnitPreferenceItem.OverridingNUnitPath;
		
		csprojDocument.Save(editorCSProjPath);
	}

    private static string _GetEditorCSProjPath(string projectRootPath)
    {
        if (NUnitPreferenceItem.IsUsingCustomEditorCSProjFilename)
        {
            string customEditorCSProjPath = Path.Combine(projectRootPath, NUnitPreferenceItem.CustomCSProjFilename);
            if (File.Exists(customEditorCSProjPath))
            {
                return customEditorCSProjPath;
            }
            else
            {
                Debug.LogWarningFormat("NUnit override cannot find custom project file at: {0}",
                    customEditorCSProjPath);
            }
        }

        {
            string vstuEditorCSProjPath = _GetVSTUEditorCSProjPath(projectRootPath);
            if (File.Exists(vstuEditorCSProjPath))
            {
                return vstuEditorCSProjPath;
            }
            else
            {
                Debug.LogWarningFormat(
                    "NUnit override cannot find VSTU patched project file at: {0}, did you install Visual Studio Tools for Unity?",
                    vstuEditorCSProjPath);
            }
        }

        {
            string defaultEditorCSProjPath = Path.Combine(projectRootPath, CSPROJ_DEFAULT_FILENAME);
            if (File.Exists(defaultEditorCSProjPath))
            {
                return defaultEditorCSProjPath;
            }
            else
            {
                Debug.LogWarningFormat("NUnit override cannot find project file at: {0}. Give up patching",
                    defaultEditorCSProjPath);
                return string.Empty;
            }
        }
    }

    private static string _GetVSTUEditorCSProjPath(string projectRootPath)
    {
        string projectName = Path.GetFileName(projectRootPath);
        string editorCSProjFilename = string.Format("{0}.Editor.csproj", projectName);
        return Path.Combine(projectRootPath, editorCSProjFilename);
    }

    private static bool _IsNUnitReference(XElement element)
	{
		XAttribute includeAttribute = element.Attribute("Include");
		return includeAttribute != null && 
		       string.Equals(includeAttribute.Value , "nunit.framework", StringComparison.OrdinalIgnoreCase);
	}
}
