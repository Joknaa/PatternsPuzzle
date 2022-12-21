using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameControllers {
    public class CanvasController : MonoBehaviour {
        public static CanvasController Instance => instance ??= FindObjectOfType<CanvasController>();
        private static CanvasController instance;
        private static string DragNDropLayer = "Drag'n'Drop";
        public Canvas Canvas => GetCanvas();
        private Canvas _canvas;
        
        public ScrollRect ScrollRect;
        public HorizontalLayoutGroup HorizontalLayoutGroup;
        
        private ScrollView _scrollView;
        private GraphicRaycaster m_Raycaster;
        private PointerEventData m_PointerEventData;
        private EventSystem m_EventSystem;
        private List<RaycastResult> results = new List<RaycastResult>();
        
        private void Start() {
            m_Raycaster = GetComponent<GraphicRaycaster>();
            m_EventSystem = GetComponent<EventSystem>();
        }

        public List<RaycastResult> CheckRaycast(Vector3 position) {
            results.Clear();
            
            m_PointerEventData = new PointerEventData(m_EventSystem) { position = position };
            m_Raycaster.Raycast(m_PointerEventData, results);
            print("results: " + results.Count);
            return results;
        }

        public void UpdateContentSpacing() {
            HorizontalLayoutGroup.spacing = PuzzleController.Instance.CurrentPuzzle.TileDimensions.x * 1.5f;
        }

        private Canvas GetCanvas() {
            if (_canvas != null) return _canvas;
            _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }
}