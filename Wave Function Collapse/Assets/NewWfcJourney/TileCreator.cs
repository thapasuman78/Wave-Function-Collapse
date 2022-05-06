using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileCreator : MonoBehaviour
{
    public int width;
    public int height;
    public float length;
    public float GridSize;
    public RuleBook MyRule;
    public Rect[,] GridRects;
    public TileConfig[] TileSamples;
    public TileConfig[] TileSets;
    public Dictionary<Vector2, TileConfig> TileList = new Dictionary<Vector2, TileConfig>(); // For GameObject ..addition and deletion
    public Dictionary<Vector2, int> GridDictionary = new Dictionary<Vector2, int>(); // for disabling painiting in same place
    [HideInInspector] public Texture[] TileTextures;
    public int Size;
    private HashSet<int> HistoryTiles = new HashSet<int>();
    void Start()
    {

    }

    public void GetTextures()
    {
        TileTextures = new Texture[TileSets.Length];
        for (int i = 0; i < TileSets.Length; i++)
        {
            TileTextures[i] = TileSets[i].Preview.sprite.texture;
        }
        Size = Mathf.CeilToInt((float)TileSets.Length / 4);
    }

    public void InitializeGrids()
    {
        GridRects = new Rect[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridRects[i, j] = new Rect(new Vector2(i, j) * length, Vector2.one * length);
            }
        }
    }

    public void CreatePrefab()
    {
        if (TileSamples.Length > 0)
            return;
        for (int i = 0; i < TileSamples.Length; i++)
        {
            TileSamples[i].CreatePrefab();
        }
    }


    public void CreateRuleFromInput()
    {
        MyRule.RulesList.Clear();
        foreach (KeyValuePair<Vector2,TileConfig> item in TileList)
        {
            int x = (int)item.Key.x;
            int y = (int)item.Key.y;
            //LEft
            int x1 = x - 1;
            //Right
            int x2 = x + 1;
            //Up
            int y1 = y + 1;
            //Down
            int y2 = y - 1;

                            
            if (HistoryTiles.Contains(item.Value.ID))
            {
                int index = MyRule.GetTile(item.Value.ID);
                if (TileList.ContainsKey(new Vector2(x1, y)) && !MyRule.RulesList[index].Left.Contains(TileList[new Vector2(x1, y)].ID))
                    MyRule.RulesList[index].Left.Add(TileList[new Vector2(x1,y)].ID);
                if (TileList.ContainsKey(new Vector2(x2, y)) && !MyRule.RulesList[index].Right.Contains(TileList[new Vector2(x2, y)].ID))
                    MyRule.RulesList[index].Right.Add(TileList[new Vector2(x2, y)].ID);
                if (TileList.ContainsKey(new Vector2(x, y1)) && !MyRule.RulesList[index].Up.Contains(TileList[new Vector2(x, y1)].ID))
                    MyRule.RulesList[index].Up.Add(TileList[new Vector2(x, y1)].ID);
                if (TileList.ContainsKey(new Vector2(x, y2)) && !MyRule.RulesList[index].Down.Contains(TileList[new Vector2(x, y2)].ID))
                    MyRule.RulesList[index].Down.Add(TileList[new Vector2(x, y2)].ID);
            }
            else
            {
                TileInfo tileInfo = new TileInfo { Id = item.Value.ID, Left = new List<int>(),Right = new List<int>(), Up = new List<int>(), Down = new List<int>() };

                if (TileList.ContainsKey(new Vector2(x1,y)))
                    tileInfo.Left.Add(TileList[new Vector2(x1, y)].ID);
                if (TileList.ContainsKey(new Vector2(x2, y)))
                    tileInfo.Right.Add(TileList[new Vector2(x2, y)].ID);
                if (TileList.ContainsKey(new Vector2(x, y1)))
                    tileInfo.Up.Add(TileList[new Vector2(x, y1)].ID);
                if (TileList.ContainsKey(new Vector2(x, y2)))
                    tileInfo.Down.Add(TileList[new Vector2(x, y2)].ID);

                HistoryTiles.Add(item.Value.ID);
                tileInfo.tileConfig = TileSets.First(a => a.ID == item.Value.ID);
                MyRule.AddToRule(tileInfo);
            }           
        }       
        Debug.Log("Rules Created");
        HistoryTiles.Clear();
    }

    public void DisplayRules()
    {
        for (int i = 0; i < MyRule.RulesList.Count; i++)
        {
            Debug.Log("TILE " + MyRule.RulesList[i].Id + " => Left : " + MyRule.RulesList[i].Left.Count);
            Debug.Log("TILE " + MyRule.RulesList[i].Id + " => Right : " + MyRule.RulesList[i].Right.Count);
            Debug.Log("TILE " + MyRule.RulesList[i].Id + " => Up : " + MyRule.RulesList[i].Up.Count);
            Debug.Log("TILE " + MyRule.RulesList[i].Id + " => Down : " + MyRule.RulesList[i].Down.Count);
        }
    }

    public GameObject AddPoint(int i, int j, int tileIndex)
    {
        Vector2 targetVector = new Vector2(i, j);
        if (GridDictionary.ContainsKey(targetVector) && GridDictionary[targetVector] == tileIndex)
        {
            Debug.Log("Allready Contains Tiles");
            return null;
        }
        TileConfig g = Instantiate(TileSets[tileIndex], GridRects[i, j].center, Quaternion.identity);
        g.transform.SetParent(transform);
        if (GridDictionary.ContainsKey(targetVector))
        {
            DestroyImmediate(TileList[targetVector].gameObject);
            TileList[targetVector] = g;
            GridDictionary[targetVector] = tileIndex;
            return g.gameObject;
        }
        TileList.Add(targetVector, g);
        GridDictionary.Add(targetVector, tileIndex);
        return g.gameObject;
        //Debug.Log("SPAWN AT - i :" + i + " , " + j);
    }

    public void RemovePoint(int i, int j)
    {
        Vector2 targetVector = new Vector2(i, j);
        if (!GridDictionary.ContainsKey(new Vector2(i, j)))
            return;
        GridDictionary.Remove(targetVector);
        DestroyImmediate(TileList[targetVector].gameObject);
        TileList.Remove(targetVector);
    }

    public void ClearAllTiles()
    {
        foreach (KeyValuePair<Vector2, TileConfig> item in TileList)
        {
            DestroyImmediate(item.Value.gameObject);
        }
        TileList.Clear();
        GridDictionary.Clear();
    }
}
