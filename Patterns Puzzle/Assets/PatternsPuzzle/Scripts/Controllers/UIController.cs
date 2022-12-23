using System;
using UnityEngine;

namespace GameControllers {
    public class UIController : MonoBehaviour {
        public static UIController Instance => instance ??= FindObjectOfType<UIController>();
        private static UIController instance;

        public static Action<int> OnProgressChanged;


        public void UpdateProgress(int progress) {
            OnProgressChanged?.Invoke(progress);
        }
    }
}