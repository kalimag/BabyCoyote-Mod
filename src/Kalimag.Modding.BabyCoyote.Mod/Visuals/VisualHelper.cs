using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Kalimag.Modding.BabyCoyote.Mod.Visuals
{
    public static class VisualHelper
    {

        private static readonly Lazy<Sprite> WhitePixel = new Lazy<Sprite>(CreateWhitePixel, false);


        public static GameObject CreateSquareSprite(Color color, int sortingOrder = 0)
        {
            var obj = new GameObject();
            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = WhitePixel.Value;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;
            return obj;
        }

        private static Sprite CreateWhitePixel()
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            var sprite = Sprite.Create(tex, rect: new Rect(0, 0, 1, 1), pivot: Vector2.one * 0.5f, pixelsPerUnit: 1f);
            return sprite;
        }

        public static void AddVisuals(GameObject obj, bool trigger, Color color, int sortingOrder = 0, List<GameObject> visualsList = null)
        {
            var colliders = obj.GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                if (collider.isTrigger != trigger)
                    continue;

                if (collider is BoxCollider2D box)
                {
                    if (box.edgeRadius > 0)
                        ModController.AddNotification($"[VisualHelper] {obj.name} has BoxCollider2D with edgeRadius {box.edgeRadius}");
                    var visualObj = CreateSquareSprite(color, sortingOrder);
                    visualObj.transform.parent = obj.transform;
                    visualObj.transform.localPosition = box.offset;
                    visualObj.transform.localScale = box.size;
                    visualsList?.Add(visualObj);
                }
                else
                {
                    ModController.AddNotification($"[VisualHelper] {obj.name} has {collider.GetType().Name}");
                }
            }
        }
    }
}
