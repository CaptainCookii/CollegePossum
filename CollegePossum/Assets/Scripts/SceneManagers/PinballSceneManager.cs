using UnityEngine;

public class PinballSceneManager : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.SetMusicLayer("Pachinko", true);
    }

}
