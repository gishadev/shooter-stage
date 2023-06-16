using UnityEngine;
using UnityEngine.SceneManagement;

namespace gishadev.Shooter.Infrastructure
{
    public class GameEnterPoint : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene("Stage");
        }
    }
}