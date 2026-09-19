using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Название сцены, на которую нужно перейти")]
    [SerializeField] private string targetSceneName = "Simulation";

    public void LoadSimulationScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}