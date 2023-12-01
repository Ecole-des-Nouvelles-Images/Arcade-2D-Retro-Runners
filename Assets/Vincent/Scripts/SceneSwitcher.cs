using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public string SceneToUnload;
    public string SceneToLoad;
    
    [ContextMenu("SwitchScene")]
    public void SwitchScene()
    {
        SceneManager.UnloadSceneAsync(SceneToUnload);
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Additive);
    }
    
}