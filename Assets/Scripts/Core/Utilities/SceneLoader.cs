using UnityEngine.SceneManagement;

namespace Core.Utilities
{
    public static class SceneLoader
    {
        public static void LoadScene(SceneType sceneType)
        {
            SceneManager.LoadScene(sceneType.ToString());
        }
    }

    public enum SceneType
    {
        MainMenu,
        GameScene
    }
}