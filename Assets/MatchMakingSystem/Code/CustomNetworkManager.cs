using UnityEngine.SceneManagement;

public class CustomNetworkManager : Mirror.NetworkManager
{

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
