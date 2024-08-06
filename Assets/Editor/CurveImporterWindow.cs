using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Globalization;

public class CurveImporterWindow : EditorWindow
{
    string[] csvFiles;
    int selectedCsvIndex = 0;
    string csvFolderPath = "Assets/Csv";
    AnimationCurve animationCurve;
    GameObject targetGameObject;
    Component selectedComponent;
    int selectedComponentIndex = 0;
    int selectedCurveFieldIndex = 0;
    List<string> componentNames = new List<string>();
    List<string> curveFieldNames = new List<string>();

    [MenuItem("Tools/Curve Importer")]
    public static void ShowWindow()
    {
        GetWindow<CurveImporterWindow>("Curve Importer");
    }

    void OnEnable()
    {
        RefreshCsvFileList();
    }

    void OnGUI()
    {
        // Dropdown for selecting CSV file
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select CSV File", GUILayout.Width(100));
        selectedCsvIndex = EditorGUILayout.Popup(selectedCsvIndex, csvFiles);
        EditorGUILayout.EndHorizontal();

        // Refresh button
        if (GUILayout.Button("Refresh File List"))
        {
            RefreshCsvFileList();
        }

        // Import button
        if (GUILayout.Button("Import Curve"))
        {
            ImportCurve();
        }

        // Display curve information and target selection
        if (animationCurve != null)
        {
            DisplayCurveInfo();
            SelectTargetGameObject();
        }
    }

    void RefreshCsvFileList()
    {
        if (Directory.Exists(csvFolderPath))
        {
            string[] files = Directory.GetFiles(csvFolderPath, "*.csv");
            csvFiles = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                csvFiles[i] = Path.GetFileName(files[i]);
            }
        }
        else
        {
            Debug.LogWarning($"Folder '{csvFolderPath}' not found.");
        }
    }

    void ImportCurve()
    {
        if (csvFiles.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No CSV files found in the Csv folder.", "OK");
            return;
        }

        string selectedCsvFilePath = Path.Combine(csvFolderPath, csvFiles[selectedCsvIndex]);

        if (File.Exists(selectedCsvFilePath))
        {
            animationCurve = CreateCurveFromCSV(selectedCsvFilePath);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "File does not exist at the selected path.", "OK");
        }
    }

    AnimationCurve CreateCurveFromCSV(string path)
    {
        AnimationCurve curve = new AnimationCurve();
        List<Keyframe> keyframes = new List<Keyframe>();

        try
        {
            string[] lines = File.ReadAllLines(path).Skip(1).ToArray(); // Skips the header line
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                float time = float.Parse(values[0], CultureInfo.InvariantCulture);
                float value = float.Parse(values[1], CultureInfo.InvariantCulture);

                keyframes.Add(new Keyframe(time, value)); // Adding frames
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error reading CSV file: {ex.Message}");
        }

        curve.keys = keyframes.ToArray();

        // Make the keyframes both tangents linear
        for (int i = 0; i < curve.keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
        }

        return curve;
    }

    void DisplayCurveInfo()
    {
        EditorGUILayout.LabelField("Curve Information", EditorStyles.boldLabel);
        EditorGUILayout.CurveField("Animation Curve", animationCurve);
    }

    void SelectTargetGameObject()
    {
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", targetGameObject, typeof(GameObject), true);

        if (targetGameObject != null)
        {
            PopulateComponentAndFieldLists();
            DisplayComponentAndFieldSelection();
        }
    }

    void PopulateComponentAndFieldLists()
    {
        Component[] components = targetGameObject.GetComponents<Component>();
        componentNames.Clear();
        foreach (Component component in components)
        {
            componentNames.Add(component.GetType().Name);
        }

        selectedComponentIndex = Mathf.Clamp(selectedComponentIndex, 0, componentNames.Count - 1);
        selectedComponent = components[selectedComponentIndex];

        curveFieldNames.Clear();
        curveFieldNames.AddRange(GetAnimationCurveFieldNames(selectedComponent));
        selectedCurveFieldIndex = Mathf.Clamp(selectedCurveFieldIndex, 0, curveFieldNames.Count - 1);
    }

    void DisplayComponentAndFieldSelection()
    {
        selectedComponentIndex = EditorGUILayout.Popup("Select Component", selectedComponentIndex, componentNames.ToArray());

        if (curveFieldNames.Count > 0)
        {
            selectedCurveFieldIndex = EditorGUILayout.Popup("Select Curve Field", selectedCurveFieldIndex, curveFieldNames.ToArray());
            if (GUILayout.Button("Apply Curve"))
            {
                ApplyCurveToSelectedField();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No AnimationCurve fields found on the selected component.", MessageType.Warning);
        }
    }

    List<string> GetAnimationCurveFieldNames(Component component)
    {
        List<string> fieldNames = new List<string>();
        FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(AnimationCurve))
            {
                fieldNames.Add(field.Name);
            }
        }
        return fieldNames;
    }

    void ApplyCurveToSelectedField()
    {
        if (selectedComponent != null && selectedCurveFieldIndex < curveFieldNames.Count)
        {
            FieldInfo field = selectedComponent.GetType().GetField(curveFieldNames[selectedCurveFieldIndex], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null && field.FieldType == typeof(AnimationCurve))
            {
                Undo.RecordObject(selectedComponent, "Apply AnimationCurve");
                field.SetValue(selectedComponent, animationCurve);
                EditorUtility.SetDirty(selectedComponent);
                Debug.Log($"Curve applied to {targetGameObject.name}.{selectedComponent.GetType().Name}.{field.Name}");
            }
        }
    }
}
