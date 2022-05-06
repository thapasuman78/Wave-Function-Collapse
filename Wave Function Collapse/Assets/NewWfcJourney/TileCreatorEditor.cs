using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileCreator))]
public class TileCreatorEditor : Editor
{
    TileCreator tileCreator;
    private bool canDraw;
    private int selection;
    private int controlId;

    private void OnEnable()
    {
        tileCreator = (TileCreator)target;
        controlId = GUIUtility.GetControlID(FocusType.Passive);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Init Grids"))
            {
                tileCreator.InitializeGrids();
                canDraw = true;
                Debug.Log("Grid Initialized");
                Repaint();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Init Textures"))
            {
                tileCreator.GetTextures();
                Debug.Log("Textures Intialized");
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear Tiles"))
            {
                tileCreator.ClearAllTiles();
                Debug.Log("Tiles Cleared");
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);
        if(GUILayout.Button("Create Prefabs"))
        {
            tileCreator.CreatePrefab();
        }

        EditorGUILayout.Space(5);
        if (GUILayout.Button("Create Rules"))
        {
            tileCreator.CreateRuleFromInput();
        }

        EditorGUILayout.Space(5);
        if (GUILayout.Button("Display Rules"))
        {
            tileCreator.DisplayRules();
        }

        EditorGUILayout.Space(10);
        if (tileCreator.TileSets.Length > 0)
        {
            selection = GUILayout.SelectionGrid(selection, tileCreator.TileTextures, 4, GUILayout.Height(tileCreator.GridSize * tileCreator.Size));
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            for (int i = 0; i < tileCreator.width; i++)
            {
                for (int j = 0; j < tileCreator.height; j++)
                {
                    if (tileCreator.GridRects[i, j].Contains(ray.origin))
                    {
                        Undo.RecordObject(target, "Add Point");
                        GameObject g= tileCreator.AddPoint(i, j, selection);
                        if (g != null)
                            Undo.RegisterCreatedObjectUndo(g, "Point Revert");
                    }
                }
            }
        }
        else if (e.type == EventType.MouseDown && e.button == 1)
        {
            for (int i = 0; i < tileCreator.width; i++)
            {
                for (int j = 0; j < tileCreator.height; j++)
                {
                    if (tileCreator.GridRects[i, j].Contains(ray.origin))
                    {
                        tileCreator.RemovePoint(i, j);
                    }
                }
            }
        }
        else if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(controlId);
        }

        DrawTiles();
    }

    public void DrawTiles()
    {
        if (tileCreator.GridRects == null && tileCreator.GridRects.Length == 0)
            return;
        for (int i = 0; i < tileCreator.width; i++)
        {
            for (int j = 0; j < tileCreator.height; j++)
            {
                if (!tileCreator.GridDictionary.ContainsKey(new Vector2(i, j)))
                    Handles.DrawSolidRectangleWithOutline(tileCreator.GridRects[i, j], new Color(1, 1, 1, 0.5f), Color.black);
            }
        }
    }
}
