using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TileConfig : MonoBehaviour
{
    public RuleBook MyRule;
    public int ID;
    public SpriteRenderer Preview;
    public int GridID_X { get; set; }
    public int GridID_Y { get; set; }

    private readonly string PrefabPath = "Assets/NewWfcJourney/Prefabs/";
    void Start()
    {
        
    }

    public void CreatePrefab()
    {
        string path = PrefabPath + gameObject.name + ".prefab";
        string localPath = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.AutomatedAction);
        Debug.Log("Prefab Created");
    }

    public void AssignSpriteRenderer()
    {
        Preview = GetComponent<SpriteRenderer>();
    }
}


