using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridIndex : MonoBehaviour
{
    public int index_X;
    public int index_Y;

    public void SetValues(int x, int y)
    {
        index_X = x;
        index_Y = y;
    }
}
