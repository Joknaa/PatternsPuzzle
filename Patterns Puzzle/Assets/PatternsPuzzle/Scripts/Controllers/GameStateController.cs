using System;

namespace GameControllers {
    public enum GameState {
        PuzzleSelectionPanel,
        Playing,
    }
    
    public class GameStateController {
        public static GameStateController Instance => instance ??= new GameStateController();
        private static GameStateController instance;
        public event Action OnGameStateChanged;

        public GameState CurrentGameState {
            get => _currentGameState;
            set => SetState(value);
        }
        private GameState _currentGameState = GameState.PuzzleSelectionPanel;
        
        
        private void SetState(GameState newState) {
            if (_currentGameState == newState) return;
            _currentGameState = newState;
            OnGameStateChanged?.Invoke();
        }
        

        public bool isNotPlaying() => CurrentGameState != GameState.Playing;
        
        
        public void OnApplicationQuit() {
            instance = null;
        }
    }
}