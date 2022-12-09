using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace OknaaEXTENSIONS {
    public static class Extensions {
        #region Vector Coordinate Manipulation

        /// <summary>
        /// Changes the X value of a Vector3, while keeping the other values the same.
        /// </summary>
        /// <param name="v">the vector3</param>
        /// <param name="x">the New value of X</param>
        /// <returns></returns>
        public static Vector3 SetX(this ref Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }
        public static Vector3 SetXY(this ref Vector3 v, float x, float y) {
            v.x = x;
            v.y = y;
            return v;
        }
        public static Vector3 SetXZ(this ref Vector3 v, float x, float z) {
            v.x = x;
            v.z = z;
            return v;
        }
        
        /// <summary>
        /// Changes the Y value of a Vector3, while keeping the other values the same.
        /// </summary>
        /// <param name="v">the vector3</param>
        /// <param name="y">the New value of Y</param>
        /// <returns></returns>
        public static Vector3 SetY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }
        
        /// <summary>
        /// Changes the Z value of a Vector3, while keeping the other values the same.
        /// </summary>
        /// <param name="v">the vector3</param>
        /// <param name="z">the New value of Z</param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }
        
        public static Vector3 LerpX(this Vector3 initial, Vector3 final, float t) {
            initial.x = Mathf.Lerp(initial.x, final.x, t);
            return initial;
        }
        public static Vector3 LerpY(this Vector3 initial, Vector3 final, float t) {
            initial.y = Mathf.Lerp(initial.y, final.y, t);
            return initial;
        }
        public static Vector3 LerpZ(this Vector3 initial, Vector3 final, float t) {
            initial.z = Mathf.Lerp(initial.z, final.z, t);
            return initial;
        }
        public static Vector3 LerpXY(this Vector3 initial, Vector3 final, float t) {
            initial.x = Mathf.Lerp(initial.x, final.x, t);
            initial.y = Mathf.Lerp(initial.y, final.y, t);
            return initial;
        }
        public static Vector3 LerpXZ(this Vector3 initial, Vector3 final, float t) {
            initial.x = Mathf.Lerp(initial.x, final.x, t);
            initial.z = Mathf.Lerp(initial.z, final.z, t);
            return initial;
        }
        public static Vector3 LerpYZ(this Vector3 initial, Vector3 final, float t) {
            initial.y = Mathf.Lerp(initial.y, final.y, t);
            initial.z = Mathf.Lerp(initial.z, final.z, t);
            return initial;
        }

        public static float RandomCoord(this Vector2 v) => UnityEngine.Random.value < 0.5f ? v.x : v.y;
        public static int RandomCoord(this Vector2Int v) => UnityEngine.Random.value < 0.5f ? v.x : v.y;

        #endregion
        
        
        /// <summary>
        /// Places rect transform to have the same dimensions as 'other', even if they don't have same parent
        /// </summary>
        public static void MatchOther(this RectTransform thisRectTransform, RectTransform other) {
            Vector2 thisOriginalPivot = thisRectTransform.pivot;
            thisOriginalPivot = other.pivot;
            
            thisRectTransform.position = other.position;
            thisRectTransform.localScale = other.localScale;

            thisRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, other.rect.width);
            thisRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, other.rect.height);
            
            thisRectTransform.ForceUpdateRectTransforms();
            thisRectTransform.pivot = thisOriginalPivot;
        }
        
        
        #region GameObject & Children

        public static List<Scene> GetAllLoadedScenes(this SceneManager sceneManager) {
            int countLoaded = SceneManager.sceneCount;
            List<Scene> loadedScenes = new List<Scene>();

            for (int i = 0; i < countLoaded; i++) {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }

            return loadedScenes;
        }
        
        /// <summary>
        /// get all gameobject children recursively
        /// </summary>
        public static List<GameObject> GetALLChildren(this GameObject go) {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in go.transform) {
                children.Add(child.gameObject);
                children.AddRange(child.gameObject.GetALLChildren());
            }
            return children;
        }
        
        public static List<GameObject> GetDirectChildren(this GameObject parent) {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform) {
                children.Add(child.gameObject);
            }

            return children;
        }
        

        #endregion
        #region ====> List Extensions <====

        /// <summary> 
        /// Takes a random element of a list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Random<T>(this IList<T> list) {
            T randomElement;
            try {
                randomElement = list[UnityEngine.Random.Range(0, list.Count)];
            }
            catch (Exception e) {
                Console.WriteLine("You cant take a random element out of an empty/null list. " + e);
                throw;
            }

            return randomElement;
        }

        /// <summary>
        /// Converts an IEnumerable to a List
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<T> source) {
            return source != null ? new List<T>(source) : new List<T>();
        }
        

        /// <summary>
        /// Fills the list with int values from start to end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IList<int> AddRange<T>(this IList<int> list, int start, int end) {
            for (int i = start; i < end; i++) {
                list.Add(i);
            }

            return list;
        }

        /// <summary>
        /// Removes the elements of one list from another list
        /// </summary>
        /// <param name="list">this list we are gonna remove from</param>
        /// <param name="subtractedList">the list we are gonna remove</param>
        /// <returns></returns>
        public static IList<T> Subtract<T>(this IList<T> list, IList<T> subtractedList) {
            List<T> newList = new List<T>();
            foreach (var element in list) {
                if (!subtractedList.Contains(element)) newList.Add(element);
            }

            return newList;
        }

        /// <summary> 
        /// Gets the next element in a list, if the CURRENT element is NULL, or is the last one, the first one is returned
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentElement">the current element of the list</param>
        /// <returns></returns>
        public static T Next<T>(this IList<T> list, T currentElement = default) {
            var currentIndex = currentElement == null ? 0 : list.IndexOf(currentElement);
            var nextIndex = currentIndex + 1;
            if (nextIndex >= list.Count) nextIndex = 0;
            return list[nextIndex];
        }


        /// <summary> 
        /// Checks if a number is between two other numbers, the order of "a" and "b" is irrelevant.
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T number, T a, T b) where T : IComparable<T> {
            var min = a.CompareTo(b) < 0 ? a : b;
            var max = a.CompareTo(b) < 0 ? b : a;
            return number.CompareTo(min) >= 0 && number.CompareTo(max) <= 0;
        }
        

        #endregion
        #region ====> Materials Extensions <====

        public static Color SetColor(this Color originalColor, float r = -1, float g = -1, float b = -1, float a = -1) {
            // if the r,g,b,a values are not set, use the original color values
            r = Math.Abs(r - (-1)) < 0.01f ? originalColor.r : r;
            g = Math.Abs(g - (-1)) < 0.01f ? originalColor.g : g;
            b = Math.Abs(b - (-1)) < 0.01f ? originalColor.b : b;
            a = Math.Abs(a - (-1)) < 0.01f ? originalColor.a : a;
            
            originalColor = new Color(r, g, b, a);
            return originalColor;
        }
        
        /// <summary>
        ///  Turns the material's rendering mode into Opaque
        /// </summary>
        public static void ToOpaqueMode(this Material material) {
            material.SetOverrideTag("RenderType", "Opaque");
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }

        /// <summary>
        ///  Turns the material's rendering mode into Fade
        /// </summary>
        public static void ToFadeMode(this Material material) {
            material.SetOverrideTag("RenderType", "Fade");
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)RenderQueue.Transparent;
        }

        /// <summary>
        ///  Sets the Alpha (transparency) of a material, 0 means fully transparent, 1 means fully visible.
        /// </summary>
        public static void SetAlpha(this Material material, bool enable, float alpha = 1f) {
            if (enable) material.ToFadeMode();

            var newColor = material.color;
            newColor.a = alpha;
            material.color = newColor;
        }
        
        /// <summary>
        ///  Makes the material transparent, then plays a Fade Out animation.
        /// </summary>
        public static IEnumerator FadeOut(this Material material, bool enable, float duration = 1f, float initialValue = 0f, float endValue = 0f, bool instantly = false) {
            if (enable) material.ToFadeMode();

            var newColor = material.color;
            if (instantly) {
                newColor.a = endValue;
                material.color = newColor;
                yield break;
            }
            var timeElapsed = 0f;

            while (newColor.a > endValue) {
                newColor.a = Mathf.Lerp(newColor.a, endValue, (timeElapsed / duration));
                timeElapsed += Time.deltaTime;
                material.color = newColor;
                yield return new WaitForEndOfFrame();
            }
        }


        /// <summary>
        /// Plays a Fade In animation, then makes the Material opaque
        /// </summary>
        public static IEnumerator FadeIn(this Material material, float duration = 1f, float initialValue = 0f, float endValue = 1f, bool instantly = false) {
            var newColor = material.color;
            newColor.a = initialValue;

            if (instantly) {
                newColor.a = endValue;
                material.color = newColor;
                yield break;
            }

            var timeElapsed = 0f;
            while (newColor.a < endValue) {
                newColor.a = Mathf.Lerp(newColor.a, endValue, (timeElapsed / duration));
                timeElapsed += Time.deltaTime;
                material.color = newColor;
                yield return new WaitForEndOfFrame();
            }

            
        }
        
        /// <summary>
        /// Plays a Fade animation, depending on the "endValue" parameter, the material will either fade in or fade out.
        /// It compares the current alpha value of the material with the "endValue" parameter, if the current alpha is lower than the "endValue" parameter, it will fade in, otherwise it will fade out.
        /// </summary>
        
        public static IEnumerator FadeTowardsAlpha(this Material material, float endValue, float duration = 1f) {
            material.ToFadeMode();
            
            var isFadeIn = material.color.a < endValue;
            var newColor = material.color;
            var timeElapsed = 0f;

            while (Mathf.Abs(newColor.a - endValue) > 0.01f) {
                newColor.a = Mathf.Lerp(newColor.a, endValue, (timeElapsed / duration));
                timeElapsed += Time.deltaTime;
                material.color = newColor;
                yield return new WaitForEndOfFrame();
            }


            if (isFadeIn && material.color.a >= 1f)
                material.ToFadeMode();
          

        }

        #endregion
        #region ====> Texture Extensions <====
        
        /// <summary>
        /// Duplicates the source texture, and returns a readable copy, same as checking the Read/Write box in the editor.
        /// Use this when you have to access a new texture pixels at runtime, and you cant enable the Read/Write access beforehand. 
        /// </summary>
        public static Texture2D GetReadableCopy(this Texture2D source) {
            RenderTexture newRenderTexture = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, newRenderTexture);
            RenderTexture previousRenderTexture = RenderTexture.active;
            RenderTexture.active = newRenderTexture;
        
            Texture2D readableTexture = new Texture2D(source.width, source.height);
            readableTexture.ReadPixels(new Rect(0, 0, newRenderTexture.width, newRenderTexture.height), 0, 0);
            readableTexture.Apply();
            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(newRenderTexture);
            return readableTexture;
            
        }
        
        #endregion
    }
}