using ARFS.Tools;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BoardRender : MonoBehaviour
{
    [SerializeField] protected RendMaterials rendColors;
    private Material _materialA;
    private Material _materialB;

    public void SetMaterial(Material a, Material B)
    {
        _materialA = a;
        _materialB = B;
    }
    public void ApplyMaterial(int itemIndex,bool isFistMaterial = true)
    {
        RendMaterial(isFistMaterial ? _materialA : _materialB, itemIndex);
    }
    public Color ApplyMaterial(ItemSel itemIndex, bool isFistMaterial = true)
    {
        Material target = isFistMaterial ? _materialA : _materialB;
        RendMaterial(target, (int)itemIndex);
        if(target==null)return Color.white;
        return target.color;
    }
    public Color ApplyMaterial(Material newMaterial,ItemSel itemIndex)
    {
        Material target = newMaterial;
        RendMaterial(target, (int)itemIndex);
        if (target == null) return Color.white;
        return target.color;
    }
    public RendMaterials GetRendMaterialByID(ItemSel itemIndex)
    {
        return rendColors.GetRendlByID((int)itemIndex);
    }
    public void SetActiveMaterial(int itemIndex, bool isActive = true)
    {
        EnableMaterial(isActive, itemIndex);
    }
    public void SetActiveMaterial(ItemSel itemIndex, bool isActive = true)
    {
        EnableMaterial(isActive, (int)itemIndex);
    }
    private void RendMaterial(Material mat, int selector = 0)
    {
        int max = rendColors.CustomRenderers.Count;
        selector = ((selector % max) + max) % max;
        rendColors.SetMaterialByID(mat, selector);
    }
    private void EnableMaterial(bool enable, int selector = 0)
    {
        int max = rendColors.CustomRenderers.Count;
        selector = ((selector % max) + max) % max;
        RendMaterials tempRend =  rendColors.GetRendlByID(selector);
        if(tempRend != null) { tempRend.gameObject.SetActive(enable);}
    }
    public enum ItemSel
    {
        Common = 0,
        FistColor = 1,
        SecondColor = 2,
        SeleccionColor = 3,
        DangerColor = 4,
    }
}
