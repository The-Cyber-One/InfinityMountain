using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TilesetLineRemover : EditorWindow
{
    [MenuItem("Window/2D/Tileset Line Remover")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TilesetLineRemover window = (TilesetLineRemover)GetWindow(typeof(TilesetLineRemover));
        window.Show();

    }

    void OnEnable()
    {
        Undo.undoRedoPerformed += UpdateTexture;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= UpdateTexture;
    }

    Texture2D tileset;
    Texture2D newTileset;
    [Min(1)]
    RectInt tileRect;
    Vector2Int padding;
    string tilesetPath;

    void OnGUI()
    {
        GUILayout.Label("Tile offset and size");
        tileRect = EditorGUILayout.RectIntField(tileRect);
        tileRect.ClampToBounds(new RectInt(0, 0, int.MaxValue, int.MaxValue));
        tileRect.width = Mathf.Max(tileRect.width, 1);
        tileRect.height = Mathf.Max(tileRect.height, 1);

        padding = EditorGUILayout.Vector2IntField("Padding", padding);

        tileset = (Texture2D)EditorGUILayout.ObjectField(tileset, typeof(Texture2D), false);
        tilesetPath = AssetDatabase.GetAssetPath(tileset);

        GUILayout.FlexibleSpace();

        if (tileset)
        {
            GUILayout.Box(tileset, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Replace tileset")) ReplaceTileset();
        }
        else newTileset = null;

        if (newTileset)
        {
            GUILayout.Box(newTileset, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Save new tileset")) SaveTileset();
        }
    }

    void ReplaceTileset()
    {
        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(tilesetPath);
        textureImporter.isReadable = true;
        textureImporter.SaveAndReimport();

        newTileset = new Texture2D(tileset.width + tileset.width / tileRect.width * 2, tileset.height + tileset.height / tileRect.height * 2);

        for (int x = 0; x < tileset.width; x++)
        {
            for (int y = 0; y < tileset.height; y++)
            {
                Color color = tileset.GetPixel(x, y);
                Vector2Int newPosition = PositionConverter(x, y);

                // Move normal tiles
                newTileset.SetPixel(newPosition.x, newPosition.y, color);

                // Fill extra pixels
                if (x % tileRect.width == 0) // Left filling
                {
                    newTileset.SetPixel(newPosition.x - 1, newPosition.y, color);
                }
                if (y % tileRect.width == 0) // Bottom filling
                {
                    newTileset.SetPixel(newPosition.x, newPosition.y - 1, color);
                }
                if (x % tileRect.width == 0 && y % tileRect.width == 0) // Bottom left filling
                {
                    newTileset.SetPixel(newPosition.x - 1, newPosition.y - 1, color);
                }

                if (x % tileRect.width == tileRect.width - 1) // Right filling
                {
                    newTileset.SetPixel(newPosition.x + 1, newPosition.y, color);
                }
                if (y % tileRect.width == tileRect.width - 1) // Top filling
                {
                    newTileset.SetPixel(newPosition.x, newPosition.y + 1, color);
                }
                if (x % tileRect.width == tileRect.width - 1 && y % tileRect.width == tileRect.width - 1) // Top right filling
                {
                    newTileset.SetPixel(newPosition.x + 1, newPosition.y + 1, color);
                }

                if (x % tileRect.width == 0 && y % tileRect.width == tileRect.width - 1) // Top left filling
                {
                    newTileset.SetPixel(newPosition.x - 1, newPosition.y + 1, color);
                }
                if (x % tileRect.width == tileRect.width - 1 && y % tileRect.width == 0) // Bottom right filling
                {
                    newTileset.SetPixel(newPosition.x + 1, newPosition.y - 1, color);
                }
            }
        }

        newTileset.Apply();
    }

    Vector2Int PositionConverter(int x, int y) => new Vector2Int(x + tileRect.x + 1 + x / tileRect.width * (2 + padding.x), y + tileRect.y + 1 + y / tileRect.height * (2 + padding.y));

    void UpdateTexture()
    {
        File.WriteAllBytes(tilesetPath, tileset.EncodeToPNG());
        AssetDatabase.Refresh();
    }

    void SaveTileset()
    {
        Undo.RecordObject(tileset, tileset.name);
        tileset.Resize(newTileset.width, newTileset.height);
        tileset.SetPixels(0, 0, newTileset.width, newTileset.height, newTileset.GetPixels());
        tileset.Apply();

        UpdateTexture();
    }
}
