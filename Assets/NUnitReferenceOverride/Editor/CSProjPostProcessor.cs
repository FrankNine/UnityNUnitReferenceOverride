using System.IO;
using System.Linq;
using System.Xml.Linq;

using UnityEngine;
using UnityEditor;

public class SolutionPostProcessor : AssetPostprocessor
{
	private const string CSPROJ_FILENAME = "Assembly-CSharp-Editor.csproj";
	private const string CSPROJ_SCHEMA = "http://schemas.microsoft.com/developer/msbuild/2003";
	
	private static void OnGeneratedCSProjectFiles()
	{
#if UNITY_5_6_OR_NEWER
		if (!NUnitPreferenceItem.IsOverridingNUnitReference) { return; }
		_OverrideNUnitReference();
#endif
	}

	private static void _OverrideNUnitReference()
	{
		Debug.Log("Patching .csproj for overriding NUnit reference...");
		
		XNamespace ns = CSPROJ_SCHEMA; 
		
		string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
		string csprojPath = Path.Combine(projectRootPath, CSPROJ_FILENAME);

		var csprojDocument = XDocument.Load(csprojPath);
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
		
		csprojDocument.Save(csprojPath);
	}
	
	private static bool _IsNUnitReference(XElement element)
	{
		XAttribute includeAttribute = element.Attribute("Include");
		return includeAttribute != null && includeAttribute.Value == "nunit.framework";
	}
}
