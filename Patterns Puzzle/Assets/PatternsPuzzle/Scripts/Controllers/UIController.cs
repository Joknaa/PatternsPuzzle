using System;
using UnityEngine;

namespace GameControllers {
    public class UIController : MonoBehaviour {
        public static UIController Instance => instance ??= FindObjectOfType<UIController>();
        private static UIController instance;

        public static Action<float> OnProgressChanged;


        public void UpdateProgress(float progress) {
            OnProgressChanged?.Invoke(progress);
        }
    }
}