using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet editingPlanet;
    private Editor propertiesEditor;

    private void OnEnable()
    {
        editingPlanet = target as Planet;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Planet"))
        {
            editingPlanet.Regenerate();
        }

        using var checker = new EditorGUI.ChangeCheckScope();
        CreateCachedEditor(editingPlanet.Properties, null, ref propertiesEditor);
        propertiesEditor.OnInspectorGUI();

        if (checker.changed)
        {
            editingPlanet.OnPlanetPropertiesUpdated();
        }
    }
}
