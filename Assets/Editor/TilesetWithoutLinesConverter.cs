using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TilesetWithoutLinesConverter : EditorWindow
{
    [MenuItem("Window/2D/TilesetWithoutLinesConverter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TilesetWithoutLinesConverter window = (TilesetWithoutLinesConverter)GetWindow(typeof(TilesetWithoutLinesConverter));
        window.Show();
    }

    Texture2D tileset;
    Texture2D newTileset;
    [Min(1)]
    RectInt tileRect;
    Vector2Int padding;

    void OnGUI()
    {
        GUILayout.Label("Tile bounds");
        tileRect = EditorGUILayout.RectIntField(tileRect);
        padding = EditorGUILayout.Vector2IntField("padding", padding);

        tileset = (Texture2D)EditorGUILayout.ObjectField(tileset, typeof(Texture2D), false);
        GUILayout.Label(tileset);

        if (GUILayout.Button("Replace tileset")) ReplaceTileset();

        GUILayout.Label(newTileset);

        if (GUILayout.Button("Save new tileset")) SaveTileset();
    }

    void ReplaceTileset()
    {
        newTileset = new Texture2D(tileset.width + tileset.width / tileRect.width * 2, tileset.height + tileset.height / tileRect.height * 2);

        for (int x = 0; x < tileset.width; x++)
        {
            for (int y = 0; y < tileset.height; y++)
            {
                Color color = tileset.GetPixel(x, y);
                Vector2Int newPosition = PositionConverter(x, y);

                // Fill extra pixels
                if (x % tileRect.width == 0)
                {
                    newTileset.SetPixel(x + x / tileRect.width * 2, y + 1 + y / tileRect.height * 2, color);
                }
                if (y % tileRect.width == 0)
                {
                    newTileset.SetPixel(x + 1 + x / tileRect.width * 2, y + y / tileRect.height * 2, color);
                }
                if (x % tileRect.width == 0 && y % tileRect.width == 0)
                {
                    newTileset.SetPixel(x + x / tileRect.width * 2, y + y / tileRect.height * 2, color);
                }

                // Move normal tiles
                newTileset.SetPixel(x + 1 + x / tileRect.width * 2, y + 1 + y / tileRect.height * 2, color);
            }
        }

        newTileset.Apply();
    }

    Vector2Int PositionConverter(int x, int y) =>
        new Vector2Int(x + 1 + x / tileRect.width * 2, y + 1 + y / tileRect.height * 2);
    

    void SaveTileset()
    {
        Undo.RecordObject(tileset, tileset.name);

        tileset.Resize(newTileset.width, newTileset.height);
        tileset.SetPixels(0, 0, newTileset.width, newTileset.height, newTileset.GetPixels());
        tileset.Apply();
    }
}
