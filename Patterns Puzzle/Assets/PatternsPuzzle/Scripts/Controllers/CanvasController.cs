using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameControllers {
    public class CanvasController : MonoBehaviour {
        public static CanvasController Instance => instance ??= FindObjectOfType<CanvasController>();
        private static CanvasController instance;
        
        public Canvas Canvas => GetCanvas();
        private Canvas _canvas;
        
        public ScrollRect ScrollRect;
        public HorizontalLayoutGroup HorizontalLayoutGroup;



        private ScrollView _scrollView;

        private Canvas GetCanvas() {
            if (_canvas != null) return _canvas;
            _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }
}