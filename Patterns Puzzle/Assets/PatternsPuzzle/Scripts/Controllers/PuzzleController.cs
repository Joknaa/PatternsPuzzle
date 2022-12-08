using UnityEngine;

namespace GameControllers {
    public class PuzzleController : MonoBehaviour {
        public static PuzzleController Instance => instance ??= FindObjectOfType<PuzzleController>();
        private static PuzzleController instance;

    }
}