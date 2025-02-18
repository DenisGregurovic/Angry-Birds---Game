using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioSource musicSource;
	public AudioClip musicClip;

	public static AudioManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		musicSource.clip = musicClip;
		musicSource.Play();
	}
}
