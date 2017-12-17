using System.IO;
using System.Linq;
using System.Xml.Linq;

using UnityEngine;
using UnityEditor;

public class SolutionPostProcessor : AssetPostprocessor
{
	private const string CSPROJ_FILENAME = "Assembly-CSharp-Editor.csproj";
	
	private static void OnGeneratedCSProjectFiles()
	{
		if (!NUnitPreferenceItem.IsOverridingNUnitReference){return;}
		Debug.Log("Patching .csproj for overriding NUnit reference...");
		
		XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
		
		string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
		string csprojPath = Path.Combine(projectRootPath, CSPROJ_FILENAME);

		XDocument csprojDocument = XDocument.Load(csprojPath);
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
