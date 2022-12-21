using System;
using TMPro;
using UnityEngine;

namespace PatternsPuzzle.Scripts.UI {
    public class UIProgressDisplay : MonoBehaviour {
        public static UIProgressDisplay Instance => instance ??= FindObjectOfType<UIProgressDisplay>();
        private static UIProgressDisplay instance;

        [SerializeField] private TMP_Text progressText;

        private void Awake() {
            if (progressText == null) progressText = GetComponent<TMP_Text>();
            progressText.text = $"Progress: 0%";
        }

        public void SetProgress(float progress) {
            progressText.text = $"Progress: {progress * 100}%";
        }
    }
}