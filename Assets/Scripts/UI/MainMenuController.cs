using Core.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            if (startButton != null)
                startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            SceneLoader.LoadScene(SceneType.GameScene);
        }
    }
}