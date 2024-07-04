using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UEasyUI
{
    [AddComponentMenu("UI/Effects/TextOutline", 15)]
    public class TextOutline : Shadow
    {
        List<UIVertex> verts;
        protected TextOutline()
        { }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;
            if(verts == null)
            {
                verts = new List<UIVertex> ();
            }

            vh.GetUIVertexStream(verts);

            var neededCpacity = verts.Count * 5;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            var start = 0;
            var end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, 0);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, 0);

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            verts.Clear();
        }
    }
}

