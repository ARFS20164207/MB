using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARFS.Tools
{
    public class RendMaterials : MonoBehaviour
    {
        public List<RendMaterials> CustomRenderers = new List<RendMaterials>();
        private Renderer[] _renderers;
        private Renderer[] renderers
        {
            get
            {
                if (_renderers == null) _renderers = GetComponentsInChildren<Renderer>();
                return _renderers;
            }
        }
        public void SetMaterialToAll(Material mat)
        {
            foreach (Renderer renderer in renderers) renderer.material = mat;
        }
        public void SetColorToAll(Color col)
        {
            foreach (Renderer renderer in renderers) renderer.material.color = col;
        }

        public void SetMaterialByID(Material col,int index)
        {
            if (CustomRenderers == null) return;
            if(CustomRenderers.Count == 0) CustomRenderers.Add(this);
            if (CustomRenderers.Count == 1) { SetMaterialToAll(col); return;}
            if (index < 0 || index >= CustomRenderers.Count) return;
            CustomRenderers[index].SetMaterialToAll(col);
        }
        public RendMaterials GetRendlByID(int index)
        {
            if (CustomRenderers == null) return null;
            if (CustomRenderers.Count == 0) return null;
            int max = CustomRenderers.Count;
            index = ((index % max) + max) % max;
            return CustomRenderers[index];
        }
        public void SetColorByID(Color col, int index)
        {
            if (CustomRenderers == null) return;
            if (CustomRenderers.Count == 0) CustomRenderers.Add(this);
            if (CustomRenderers.Count == 1) { SetColorToAll(col); return; }
            if (index < 0 || index >= CustomRenderers.Count) return;
            CustomRenderers[index].SetColorToAll(col);
        }
    }
}