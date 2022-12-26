using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class SoundController : MonoBehaviour {
        [Header("BG Music")]
        [SerializeField] private AudioSource audioSourceMusic;
        [SerializeField] private AudioClip BGMusic;


        [Header("SFX")]
        [SerializeField] private AudioSource audioSourceSFX;
        [SerializeField] private AudioClip CorrectTilePlaced;


        private void Awake() {
            Tile.OnTilePlacedInCorrectSlot += PlaySFX_CorrectTilePlaced;
            PlayBackGroundMusic();
        }

        private void PlayBackGroundMusic() {
            audioSourceMusic.clip = BGMusic;
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
        }


        private void PlaySFX_CorrectTilePlaced(int i) => audioSourceSFX.PlayOneShot(CorrectTilePlaced);

        private void OnDestroy() {
            Tile.OnTilePlacedInCorrectSlot -= PlaySFX_CorrectTilePlaced;
        }
    }
}