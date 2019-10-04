using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    MusicController musicController = null;
    EffectsController effectsController = null;

    bool mute = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        musicController = GetComponent<MusicController>();
        effectsController = GetComponent<EffectsController>();
    }

    public void PlayClip(string key)
    {
        effectsController.PlayClip(key);
    }

    public void MuteAll()
    {
        mute = !mute;
        musicController.ToggleMuteMusic(mute);
        effectsController.ToggleMuteSFX(mute);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MuteAll();
        }
    }
}