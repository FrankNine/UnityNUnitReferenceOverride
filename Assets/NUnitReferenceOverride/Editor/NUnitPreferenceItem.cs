using System;

using UnityEngine;
using UnityEditor;

namespace NUnitReferenceOverride
{
    public class NUnitPreferenceItem
    {
        private static bool _isPrefsLoaded;

        private const string IsOverridingNUnitReferenceEditorPrefsKey  = "IsOverridingNUnitReference";
        private const string OverridingNUnitPathEditorPrefsKey         = "OverridingNUnitPath";
        private const string IsUsingCustomEditorCSProjFilenamePrefsKey = "IsUsingCustomEditorCSProjFilename";
        private const string CustomEditorCSProjFilenamePrefsKey        = "CustomEditorCSProjFilename";

        private static bool   _isOverridingNUnitReference;
        private static string _overridingNUnitPath;
        private static bool   _isUsingCustomEditorCSProjFilename;
        private static string _customCSProjName;

        public static bool IsOverridingNUnitReference
        {
            get { return EditorPrefs.GetBool(IsOverridingNUnitReferenceEditorPrefsKey, false); }
        }

        public static string OverridingNUnitPath
        {
            get { return EditorPrefs.GetString(OverridingNUnitPathEditorPrefsKey, String.Empty); }
        }

        public static bool IsUsingCustomEditorCSProjFilename
        {
            get { return EditorPrefs.GetBool(IsUsingCustomEditorCSProjFilenamePrefsKey, false); }
        }

        public static string CustomCSProjFilename
        {
            get { return EditorPrefs.GetString(CustomEditorCSProjFilenamePrefsKey, String.Empty); }
        }

        [PreferenceItem("NUnit Override")]
        private static void PreferencesGUI()
        {
#if UNITY_EDITOR_OSX
            _ShowOverrideNotRequiredRiderOSX();
#elif !UNITY_5_6_OR_NEWER
            _ShowOverrideNotRequiredNUnit2();
#else
            _ShowNUnitOverrideOptions();
#endif
        }

        private static void _ShowNUnitOverrideOptions()
        {
            if (!_isPrefsLoaded)
            {
                _isOverridingNUnitReference = IsOverridingNUnitReference;
                _overridingNUnitPath = OverridingNUnitPath;
                _isUsingCustomEditorCSProjFilename = IsUsingCustomEditorCSProjFilename;
                _customCSProjName = CustomCSProjFilename;

                _isPrefsLoaded = true;
            }

            _isOverridingNUnitReference = EditorGUILayout.Toggle("Is Overriding NUnit reference",
                _isOverridingNUnitReference);

            GUI.enabled = _isOverridingNUnitReference;
            {
                EditorGUILayout.LabelField("nunit.framework.dll path (NUnit 3.5, targeting .Net 3.5)");
                _overridingNUnitPath = EditorGUILayout.TextField(_overridingNUnitPath);

                _isUsingCustomEditorCSProjFilename = EditorGUILayout.Toggle("Is using custom .csproj filename",
                    _isUsingCustomEditorCSProjFilename);

                GUI.enabled = _isOverridingNUnitReference && _isUsingCustomEditorCSProjFilename;
                {
                    EditorGUILayout.LabelField("Custom editor .csproj name (With extension)");
                    _customCSProjName = EditorGUILayout.TextField(_customCSProjName);
                }
                GUI.enabled = _isOverridingNUnitReference;
            }

            if (GUILayout.Button("Patch now"))
            {
                SolutionPostProcessor.OverrideNUnitReference();
            }

            GUI.enabled = true;

            if (GUI.changed)
            {
                EditorPrefs.SetBool(IsOverridingNUnitReferenceEditorPrefsKey, _isOverridingNUnitReference);
                EditorPrefs.SetString(OverridingNUnitPathEditorPrefsKey, _overridingNUnitPath);
                EditorPrefs.SetBool(IsUsingCustomEditorCSProjFilenamePrefsKey, _isUsingCustomEditorCSProjFilename);
                EditorPrefs.SetString(CustomEditorCSProjFilenamePrefsKey, _customCSProjName);
            }
        }

        private static void _ShowOverrideNotRequiredNUnit2()
        {
            EditorGUILayout.LabelField("This version of Unity is using NUnit 2.");
            EditorGUILayout.LabelField("Overriding is not required.");
        }

        private static void _ShowOverrideNotRequiredRiderOSX()
        {
            EditorGUILayout.LabelField("This plugin is only for Visual Studio");
            EditorGUILayout.LabelField("for Windows with ReSharper");
        }
    }
}