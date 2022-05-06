using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BrandWfc : MonoBehaviour
{
    public RuleBook MyRule;
    public int width = 10;
    public int height = 10;
    public GameObject SquareGrid;
    public GameObject Viewer;
    public GridInfo[,] gridInfo;
    Stack<GridInfo> NeighborGridStack = new Stack<GridInfo>();
    bool canInteract;
    private Ray ray;
    private Camera mainCam;
    RaycastHit2D hit;
    public LayerMask layerMask;
    Collider2D lastHit;
    int counter;
    private Color lastColor;
    private bool onSelection;
    List<TileConfig> datas = new List<TileConfig>();

    void Start()
    {
        CreateGrid();
        GridInfo firstChoseTile = gridInfo[5, 5];
        mainCam = Camera.main;
        //Observe(firstChoseTile);
    }

    public void Update()
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, layerMask);
        if (hit.collider != null)
        {
            if (lastHit != hit.collider)
            {
                if (lastHit != null)
                    lastHit.GetComponent<SpriteRenderer>().color = lastColor;

                lastColor = hit.collider.GetComponent<SpriteRenderer>().color;
                hit.collider.GetComponent<SpriteRenderer>().color = Color.green;
                lastHit = hit.collider;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!onSelection)
                {
                    GridIndex a = hit.collider.GetComponent<GridIndex>();
                    //Observe(gridInfo[a.GridID_X, a.GridID_Y], a.ID);
                    Debug.Log(a.index_X);
                    Debug.Log(a.index_Y);
                    onSelection = true;
                    hit.collider.GetComponent<Collider2D>().enabled = false;    
                    DisplayOptions(gridInfo[a.index_X, a.index_Y], a.index_X, a.index_Y);
                }
                else
                {
                    Viewer.SetActive(false);
                    TileConfig a = hit.collider.GetComponent<TileConfig>();
                    Observe(gridInfo[a.GridID_X, a.GridID_Y], a.ID);
                    HideOptions();
                    onSelection = false;
                }
            }           
        }
        //else 
        //{
        //    if(lastHit != null)
        //    {
        //        lastHit = null;
        //        lastHit.GetComponent<SpriteRenderer>().color = Color.white;
        //    }
        //}
    }

    public void Observe(GridInfo chosenTile, int choice)
    {
        //int choice = Random.Range(0, chosenTile.TilesList.Count);
        //Debug.Log(chosenTile.);
        List<TileConfig> removeTile = chosenTile.TilesList.Where(a => a.ID != choice).ToList();
        for (int i = 0; i < removeTile.Count; i++)
        {
            //Destroy(removeTile[i].gameObject);
            chosenTile.TilesList.Remove(removeTile[i]);
        }
        chosenTile.IsFinal = true;
        counter++;
        //Debug.Log(chosenTile.TilesList.Count == 1);
        //Debug.Log(chosenTile.TilesList[0].ID);
        UpdateNeighbor(chosenTile);
        // Debug.Log(chosenTile.TilesList.Count);
        DisplayVisual();
    }

    /*public void StepByStep()
    {
        GridInfo nextTile = EntropyCalculation();
        Observe(nextTile);
    }*/

    public void DisplayVisual()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(gridInfo[i,j].TilesList.Count == 0)
                {
                    Debug.Log("MOJ");
                }
                if (gridInfo[i, j].IsFinal)
                {
                    TileConfig a = Instantiate(gridInfo[i, j].TilesList[0],gridInfo[i,j].position,Quaternion.identity);
                    //a.transform.position = gridInfo[i, j].position;
                    //a.transform.localScale = Vector2.one;
                    a.GetComponent<Collider2D>().enabled = false;
                    a.GetComponent<SpriteRenderer>().color = Color.white;
                    a.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
            }
        }
        Debug.Log(counter == width * height ? "COMPLETED" : "NOT COMPLETED");
    }


    public void CreateGrid()
    {
        gridInfo = new GridInfo[width, height];
        TileConfig[] allTiles = new TileConfig[MyRule.RulesList.Count];
        for (int k = 0; k < allTiles.Length; k++)
        {
            allTiles[k] = MyRule.RulesList[k].tileConfig;
        }
        //float tileLength = 0.25f;
        bool white = false;
        for (int i = 0; i < width; i++)
        {
            white = !white;
            for (int j = 0; j < height; j++)
            {
                GameObject gridInstance = Instantiate(SquareGrid, new Vector2(i, j), Quaternion.identity);
                gridInfo[i, j] = new GridInfo(allTiles, i, j, new Vector2(i, j));
                gridInstance.GetComponent<GridIndex>().SetValues(i, j);
                gridInstance.GetComponent<SpriteRenderer>().color = white ? Color.white : Color.black;
                white = !white;
                /*Vector2 startPos = new Vector2(i - 0.5f + (tileLength / 2), j + 0.5f - (tileLength / 2));
                float x = 0;
                for (int k = 0; k < 10; k++)
                {
                    x += 0.05f;
                    float pos = k % 3;
                    if (k != 0 && k % 3 == 0)
                    {
                        startPos = new Vector2(startPos.x, startPos.y - 0.25f);
                        x = 0.05f;
                    }
                    TileConfig a = Instantiate(MyRule.RulesList[k].tileConfig, new Vector2(startPos.x + pos * tileLength + x, startPos.y), Quaternion.identity);
                    a.GridID_X = i;
                    a.GridID_Y = j;
                    a.transform.localScale = Vector2.one * tileLength;
                    allTiles[k] = a;
                }*/
            }
        }
        Debug.Log("Grid Created");
    }

    public void DisplayOptions(GridInfo info, int x, int y)
    {
        Viewer.SetActive(true);
        Vector2 startPos = new Vector2(7.5f - 3 + 0.5f, 4.5f + 3 - 0.5f);
        int a = 0, b = 0; float space = 0.1f;
        Debug.Log(info.TilesList.Count);
        for (int i = 0; i < info.TilesList.Count; i++)
        {
            if (i != 0 && i % 5 == 0)
            {
                a = 0;
                b += 1;
            }
            TileConfig d = Instantiate(info.TilesList[i], new Vector2(startPos.x + a, startPos.y - b), Quaternion.identity);
            d.GridID_X = x;
            d.GridID_Y = y;
            datas.Add(d);
            a++;
        }
    }

    public void HideOptions()
    {
        for (int i = 0; i < datas.Count; i++)
        {
            Destroy(datas[i].gameObject);
        }
        datas.Clear();
    }

    public GridInfo EntropyCalculation()
    {
        Dictionary<TileConfig, int> FreqDict = new Dictionary<TileConfig, int>();
        Dictionary<TileConfig, float> WeightDict = new Dictionary<TileConfig, float>();
        int frequency = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!gridInfo[i, j].IsFinal)
                {
                    for (int k = 0; k < gridInfo[i, j].TilesList.Count; k++)
                    {
                        if (FreqDict.ContainsKey(gridInfo[i, j].TilesList[k]))
                        {
                            FreqDict[gridInfo[i, j].TilesList[k]] += 1;
                        }
                        else
                        {
                            FreqDict.Add(gridInfo[i, j].TilesList[k], 1);
                        }
                        frequency++;
                    }
                }
            }
        }

        foreach (var item in FreqDict)
        {
            WeightDict.Add(item.Key, (float)item.Value / (float)frequency);
        }

        GridInfo minEntropyGrid = null;
        foreach (var item in gridInfo)
        {
            float weightSum = 0;
            float currentWeightLog = 0;
            if (!item.IsFinal)
            {
                foreach (var child in item.TilesList)
                {
                    weightSum += WeightDict[child];
                    currentWeightLog += Mathf.Log(WeightDict[child]);
                }
                float totalWeightLog = Mathf.Log(weightSum);
                float entropyValue = totalWeightLog - (currentWeightLog / weightSum);

                if (minEntropyGrid == null)
                    minEntropyGrid = item;
                else
                {
                    if (minEntropyGrid.entropyValue > entropyValue)
                    {
                        minEntropyGrid = item;
                    }
                }
            }
        }
        return minEntropyGrid;
    }

    public void UpdateNeighbor(GridInfo currentGrid)
    {
        for (int i = currentGrid.x - 1; i <= currentGrid.x + 1; i += 2)
        {
            if (i >= 0 && i < width)
            {
                GridInfo NeighborGrid = gridInfo[i, currentGrid.y];
                if (!NeighborGrid.IsFinal && !NeighborGridStack.Contains(NeighborGrid))
                {
                    List<TileConfig> validList = i > currentGrid.x ? MyRule.HasRight(currentGrid.TilesList, NeighborGrid.TilesList) : MyRule.HasLeft(currentGrid.TilesList, NeighborGrid.TilesList);
                    List<TileConfig> invalidList = NeighborGrid.TilesList.Except(validList).ToList();
                    //Debug.Log("INVALID : " + invalidList.Count);
                    if (invalidList.Count > 0)
                    {
                        for (int j = 0; j < invalidList.Count; j++)
                        {
                            NeighborGrid.TilesList.Remove(invalidList[j]);
                            //Destroy(invalidList[j].gameObject);
                        }
                        if (NeighborGrid.TilesList.Count == 1)
                        {
                            counter++;
                            NeighborGrid.IsFinal = true;
                        }
                        NeighborGridStack.Push(NeighborGrid);
                    }
                }
            }
        }

        for (int i = currentGrid.y - 1; i <= currentGrid.y + 1; i += 2)
        {
            if (i >= 0 && i < height)
            {
                GridInfo NeighborGrid = gridInfo[currentGrid.x, i];
                if (!NeighborGrid.IsFinal && !NeighborGridStack.Contains(NeighborGrid))
                {
                    List<TileConfig> validList = i > currentGrid.y ? MyRule.HasUp(currentGrid.TilesList, NeighborGrid.TilesList) : MyRule.HasDown(currentGrid.TilesList, NeighborGrid.TilesList);
                    List<TileConfig> invalidList = NeighborGrid.TilesList.Except(validList).ToList();
                    //Debug.Log("INVALID : " + invalidList.Count);
                    if (invalidList.Count > 0)
                    {
                        for (int j = 0; j < invalidList.Count; j++)
                        {
                            NeighborGrid.TilesList.Remove(invalidList[j]);
                            //Destroy(invalidList[j].gameObject);
                        }
                        if (NeighborGrid.TilesList.Count == 1)
                        {
                            counter++;
                            NeighborGrid.IsFinal = true;
                        }
                        NeighborGridStack.Push(NeighborGrid);
                    }  
                }
            }
        }
        while (NeighborGridStack.Count > 0)
        {
            UpdateNeighbor(NeighborGridStack.Pop());
        }
    }
}

public class GridInfo
{
    public int x;
    public int y;
    public Vector2 position;
    public List<TileConfig> TilesList;
    public Stack<TileConfig> ValidStack;
    public Stack<TileConfig> InvalidStack;
    public float entropyValue;
    public bool IsFinal;
    public bool IsVisited;


    public GridInfo(TileConfig[] targetTiles, int _x, int _y, Vector2 pos)
    {
        TilesList = targetTiles.ToList();
        x = _x;
        y = _y;
        position = pos;
        ValidStack = new Stack<TileConfig>();
        InvalidStack = new Stack<TileConfig>();
    }
}
