using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MyRules", menuName = "Create Rule")]
public class RuleBook : ScriptableObject
{
    public List<TileInfo> RulesList;

    public void AddToRule(TileInfo newRule)
    {
        RulesList.Add(newRule);
    }

    public int GetTile(int id)
    {
        return RulesList.IndexOf(RulesList.Find(a => a.Id == id));
    }

    public List<TileConfig> HasRight(List<TileConfig> sourceTiles, List<TileConfig> targetTiles)
    {
        List<TileConfig> invalidTile = new List<TileConfig>();

        for (int i = 0; i < sourceTiles.Count; i++)
        {
            TileInfo suppose = RulesList.Find(a => a.Id == sourceTiles[i].ID);

            for (int j = 0; j < targetTiles.Count; j++)
            {
                if (RulesList[RulesList.IndexOf(suppose)].Right.Contains(targetTiles[j].ID) && !invalidTile.Contains(targetTiles[j]))
                {
                    invalidTile.Add(targetTiles[j]);
                }
            }
        }
        return invalidTile;
    }

    public List<TileConfig> HasLeft(List<TileConfig> sourceTiles, List<TileConfig> targetTiles)
    {
        List<TileConfig> invalidTile = new List<TileConfig>();

        for (int i = 0; i < sourceTiles.Count; i++)
        {
            TileInfo suppose = RulesList.Find(a => a.Id == sourceTiles[i].ID);

            for (int j = 0; j < targetTiles.Count; j++)
            {
                if (RulesList[RulesList.IndexOf(suppose)].Left.Contains(targetTiles[j].ID) && !invalidTile.Contains(targetTiles[j]))
                {
                    invalidTile.Add(targetTiles[j]);
                }
            }
        }
        return invalidTile;
    }
    public List<TileConfig> HasUp(List<TileConfig> sourceTiles, List<TileConfig> targetTiles)
    {
        List<TileConfig> invalidTile = new List<TileConfig>();

        for (int i = 0; i < sourceTiles.Count; i++)
        {
            TileInfo suppose = RulesList.Find(a => a.Id == sourceTiles[i].ID);

            for (int j = 0; j < targetTiles.Count; j++)
            {
                if (RulesList[RulesList.IndexOf(suppose)].Up.Contains(targetTiles[j].ID) && !invalidTile.Contains(targetTiles[j]))
                {
                    invalidTile.Add(targetTiles[j]);
                }
            }
        }
        return invalidTile;
    }
    public List<TileConfig> HasDown(List<TileConfig> sourceTiles, List<TileConfig> targetTiles)
    {
        List<TileConfig> invalidTile = new List<TileConfig>();

        for (int i = 0; i < sourceTiles.Count; i++)
        {
            TileInfo suppose = RulesList.Find(a => a.Id == sourceTiles[i].ID);

            for (int j = 0; j < targetTiles.Count; j++)
            {
                if (RulesList[RulesList.IndexOf(suppose)].Down.Contains(targetTiles[j].ID) && !invalidTile.Contains(targetTiles[j]))
                {
                    invalidTile.Add(targetTiles[j]);
                }
            }
        }
        return invalidTile;
    }
}

[System.Serializable]
public class TileInfo
{
    public int Id;
    public TileConfig tileConfig;
    public List<int> Up;
    public List<int> Right;
    public List<int> Down;
    public List<int> Left;
}
