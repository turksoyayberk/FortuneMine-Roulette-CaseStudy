using UnityEngine;

namespace Core.Game
{
    public class FpsController : MonoBehaviour
    {
        private const int Fps60 = 60;

        private void Awake()
        {
            Application.targetFrameRate = Fps60;
        }
    }
}