using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private Slider Slider;

	private void Start()
	{
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
	}

	public void SetMusicVolume()
    {
        float volume = Slider.value;
        AudioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
	{
        Slider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
	}

    public void BackToMainMenu()
    {
        GameObject.FindObjectOfType<MainMenu>().ShowMainMenu();
    }
}
