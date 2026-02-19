using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabPainter2D : EditorWindow
{
    private List<GameObject> prefabs = new List<GameObject>();
    private int selectedIndex = -1;
    private GameObject parent;
    private bool eraseMode = false;
    private bool isEnablePainter = true;
    private float heightOffset = 0f;
    private GameObject previewInstance;
    private GameObject lastPreviewedPrefab;

    [MenuItem("Tools/2D Prefab Painter")]
    public static void ShowWindow() => GetWindow<PrefabPainter2D>("2D Prefab Painter");

    private void OnGUI()
    {
        GUILayout.Label("2D Prefab Painter", EditorStyles.boldLabel);

        // Enable/disable
        isEnablePainter = GUILayout.Toggle(isEnablePainter, isEnablePainter ? "Enabled" : "Disabled", "Button");
        if (!isEnablePainter) DestroyPreview();

        // Drag & drop
        GUILayout.Label("Drag prefabs here:");
        Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop prefabs here");

        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (Object obj in DragAndDrop.objectReferences)
                {
                    if (obj is GameObject go && !prefabs.Contains(go))
                        prefabs.Add(go);
                }
            }
            evt.Use();
        }

        // Prefab thumbnails
        GUILayout.Label("Select Prefab:");
        int columns = 4;
        int rows = Mathf.CeilToInt(prefabs.Count / (float)columns);
        float thumbSize = 64;

        for (int y = 0; y < rows; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < columns; x++)
            {
                int i = y * columns + x;
                if (i >= prefabs.Count) break;

                GameObject prefab = prefabs[i];
                Texture2D preview = AssetPreview.GetAssetPreview(prefab) ?? AssetPreview.GetMiniThumbnail(prefab);

                GUIStyle style = new GUIStyle(GUI.skin.button);
                if (i == selectedIndex) style.normal.background = Texture2D.grayTexture;

                if (GUILayout.Button(preview, style, GUILayout.Width(thumbSize), GUILayout.Height(thumbSize)))
                    selectedIndex = i;
            }
            GUILayout.EndHorizontal();
        }

        // Parent & options
        parent = (GameObject)EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true);
        heightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);
        eraseMode = GUILayout.Toggle(eraseMode, eraseMode ? "Erase Mode" : "Place Mode", "Button");
    }

    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        DestroyPreview();
    } 

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isEnablePainter || selectedIndex < 0) return;

        Event e = Event.current;
        GameObject prefab = prefabs[selectedIndex];

        // Ray from SceneView GUI point
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        // XY plane at Z = 0
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        if (!xyPlane.Raycast(ray, out float enter)) return;

        Vector3 pos = ray.GetPoint(enter) + new Vector3(0, 0, heightOffset);
        Quaternion rot = prefab.transform.rotation;

        // Draw preview
        if (!eraseMode)
            DrawPrefabPreview2D(prefab, pos, rot);

        // Place / erase
        if ((e.type == EventType.MouseDown) && e.button == 0)
        {
            if (eraseMode)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.5f);
                foreach (var h in hits)
                {
                    PaintedMarker marker = h.GetComponentInParent<PaintedMarker>();
                    if (marker != null)
                        Undo.DestroyObjectImmediate(marker.gameObject);
                }
            }
            else if (prefab != null)
            {
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(newObj, "Place Prefab");

                if (parent != null)
                    newObj.transform.SetParent(parent.transform);

                newObj.transform.position = pos;
                newObj.transform.rotation = rot;

                newObj.AddComponent<PaintedMarker>();
            }

            e.Use();
        }

        HandleUtility.Repaint();
    }

    private void DrawPrefabPreview2D(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return;

        // Destroy previous preview if prefab changed
        if (previewInstance == null || lastPreviewedPrefab != prefab)
        {
            DestroyPreview();
            previewInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            previewInstance.name = prefab.name + "_Preview";

            // Disable colliders and scripts
            foreach (var col in previewInstance.GetComponentsInChildren<Collider2D>())
                col.enabled = false;

            foreach (var mb in previewInstance.GetComponentsInChildren<MonoBehaviour>())
                mb.enabled = false;

            lastPreviewedPrefab = prefab;
        }

        // Set position and rotation (conserve prefab rotation)
        previewInstance.transform.position = position;
        previewInstance.transform.rotation = rotation;

        // Make semi-transparent
        foreach (var sr in previewInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
    }

    private void DestroyPreview()
    {
        if (previewInstance != null)
        {
            DestroyImmediate(previewInstance);
            previewInstance = null;
            lastPreviewedPrefab = null;
        }
    }
}
