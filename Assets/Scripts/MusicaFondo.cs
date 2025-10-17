using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicaFondo : MonoBehaviour
{
	private static MusicaFondo instancia;
	private AudioSource audioSource; 

	public static MusicaFondo Instancia 
	{ 
		get { return instancia; } 
	}

	void Awake()
	{
		if (instancia == null)
		{
			instancia = this;
			DontDestroyOnLoad(gameObject);

			audioSource = GetComponent<AudioSource>();
			audioSource.loop = true; // La música de ambiente debe repetirse
		}
		else
		{
			Destroy(gameObject); 
		}
	}

	// Usado por AudioManager para iniciar la música
	public void ReproducirMusica(AudioClip clip)
	{
		if (audioSource.clip != clip || !audioSource.isPlaying)
		{
			audioSource.clip = clip;
			audioSource.Play();
		}
	}

	// Usado por AudioManager al recoger el Anillo
	public void DetenerMusica()
	{
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
	}
}