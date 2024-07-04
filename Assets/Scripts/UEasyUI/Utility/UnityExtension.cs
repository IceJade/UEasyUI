using System;
using UnityEngine;
using UnityEngine.UI;

namespace UEasyUI
{
    /// <summary>
    /// Unity 扩展。
    /// </summary>
    public static class UnityExtension
    {
        /// <summary>
        /// 获取active的子节点数量
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int GetActiveChildCount(this Transform t)
        {
            var count = 0;
            foreach (Transform child in t)
            {
                if (child.gameObject.activeSelf) count++;
            }

            return count;
        }

        /// <summary>
        /// 数字转换成罗马数字形式
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToRoman(this int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + (number - 1000).ToRoman();
            if (number >= 900) return "CM" + (number - 900).ToRoman();
            if (number >= 500) return "D" + (number - 500).ToRoman();
            if (number >= 400) return "CD" + (number - 400).ToRoman();
            if (number >= 100) return "C" + (number - 100).ToRoman();
            if (number >= 90) return "XC" + (number - 90).ToRoman();
            if (number >= 50) return "L" + (number - 50).ToRoman();
            if (number >= 40) return "XL" + (number - 40).ToRoman();
            if (number >= 10) return "X" + (number - 10).ToRoman();
            if (number >= 9) return "IX" + (number - 9).ToRoman();
            if (number >= 5) return "V" + (number - 5).ToRoman();
            if (number >= 4) return "IV" + (number - 4).ToRoman();
            if (number >= 1) return "I" + (number - 1).ToRoman();
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        #region GameObject

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 递归设置游戏对象的层次。
        /// </summary>
        /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
        /// <param name="layer">目标层次的编号。</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].gameObject.layer = layer;
            }
        }

        /// <summary>
        /// 获取 GameObject 是否在场景中。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>GameObject 是否在场景中。</returns>
        /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        //供lua检查gameObject是否valid
        public static bool IsNull(this UnityEngine.Object o)
        {
            return o == null;
        }

        /// <summary>
        /// 无需 Null check 的情况下不要调用该接口 GameObject == null 有额外的性能消耗
        /// 扩展SetActive, 提升性能
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="active"></param>
        public static void SetActiveEx(this GameObject obj, bool active)
        {
            if (obj && obj.activeSelf != active)
            {
                obj.SetActive(active);
            }
        }

        // 编辑器模式下只能使用DestroyImmediate
        // 而运行模式使用DestroyImmediate又会有隐患。。。所以这个函数出现了
        public static void DestroyEx(this GameObject obj)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                GameObject.DestroyImmediate(obj, false);
            }
        }

        #endregion

        #region Transform

        public static void ForceRebuildLayoutImmediate(this RectTransform parent)
        {
            if (parent == null)
            {
                Log.Error("ForceRebuildLayoutImmediate Error, RectTransform is null????????");
                return;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }

        /// <summary>
        /// 设置绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.x = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.y = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.z = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.x += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.y += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值增量。</param>
        public static void AddPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.z += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值。</param>
        public static void AddLocalPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.x += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值。</param>
        public static void AddLocalPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.y += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值。</param>
        public static void AddLocalPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.z += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 分量值。</param>
        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.x = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 分量值。</param>
        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.y = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 分量值。</param>
        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.z = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 分量增量。</param>
        public static void AddLocalScaleX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.x += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 分量增量。</param>
        public static void AddLocalScaleY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.y += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 分量增量。</param>
        public static void AddLocalScaleZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.z += deltaValue;
            transform.localScale = v;
        }

        #endregion Transform
    }
}