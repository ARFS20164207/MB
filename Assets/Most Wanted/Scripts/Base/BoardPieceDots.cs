using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardPieceDots : MonoBehaviour
{
    public List<GameObject> N1, N2, N3, N4, N5, N6, N7, N8, N9, N10;
    public int visualStrengh = 0;
    public void SetNumber(int strengh)
    {
        List<GameObject> last = null;

        if (strengh == 1) last = N1; else ActiveNumber(N1, false);
        if (strengh == 2) last = N2; else ActiveNumber(N2, false);
        if (strengh == 3) last = N3; else ActiveNumber(N3, false);
        if (strengh == 4) last = N4; else ActiveNumber(N4, false);
        if (strengh == 5) last = N5; else ActiveNumber(N5, false);
        if (strengh == 6) last = N6; else ActiveNumber(N6, false);
        if (strengh == 7) last = N7; else ActiveNumber(N7, false);
        if (strengh == 8) last = N8; else ActiveNumber(N8, false);
        if (strengh == 9) last = N9; else ActiveNumber(N9, false);
        if (strengh == 10) last = N10; else ActiveNumber(N10, false);

        if (last != null)
        { ActiveNumber(last, true);visualStrengh = strengh;}
    }

    void ActiveNumber(List<GameObject> N, bool enable)
    {
        foreach (var itemChild in N)
        {
            itemChild.SetActive(enable);
        }
    }

}

