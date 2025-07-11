using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardPieceDots : MonoBehaviour
{
    public List<GameObject> N1, N2, N3, N4, N5, N6, N7, N8, N9, N10, container = new List<GameObject>();
    public int visualStrengh = 0;

    private void Start()
    {
        container = new List<GameObject>();
        MakeList(N1,container);
        MakeList(N2,container);
        MakeList(N3,container);
        MakeList(N4,container);
        MakeList(N5,container);
        MakeList(N6,container);
        MakeList(N7,container);
        MakeList(N8,container);
        MakeList(N9,container);
        MakeList(N10,container);
        SetNumber(visualStrengh);
    }

    private void Update()
    {
        //SetNumber(visualStrengh);
    }

    public void SetNumber(int strengh)
    {
        List<GameObject> last = null;
        ActiveNumber(container, false);
        switch (strengh)
        {
            case 1: last = N1;break;
            case 2: last = N2;break;
            case 3: last = N3;break;
            case 4: last = N4;break;
            case 5: last = N5;break;
            case 6: last = N6;break;
            case 7: last = N7;break;
            case 8: last = N8;break;
            case 9: last = N9;break;
            case 10: last = N10;break;
            default: break;
        }

        if (last == null) return;
        ActiveNumber(last, true);
        visualStrengh = strengh;
    }

    void ActiveNumber(List<GameObject> N, bool enable)
    {
        foreach (var itemChild in N)
        {
            itemChild.SetActive(enable);
        }
    }
    void MakeList(List<GameObject> N, List<GameObject> M)
    {
        foreach (var itemChild in N)
        {
            if(!M.Contains(itemChild))
            { M.Add(itemChild); }
        }
    }

}

