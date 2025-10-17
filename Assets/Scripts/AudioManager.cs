using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Para los SFX (Asegura que haya un AudioSource en el objeto)
public class AudioManager : MonoBehaviour
{
	[Header("Clips de Audio")]
	public AudioClip musicaAmbienteClip;
	public AudioClip sfxCuboClip; 

	[Header("Configuración SFX")]
	// Esta fuente se usará para los efectos de sonido
	public AudioSource sfxSource; 

	void Start()
	{
		// Se intenta asignar automáticamente el AudioSource del objeto si no fue asignado en el Inspector
		if (sfxSource == null)
		{
			sfxSource = GetComponent<AudioSource>();
		}

		// 1. Inicia la música de fondo usando el Singleton MusicaFondo
		if (MusicaFondo.Instancia != null && musicaAmbienteClip != null)
		{
			MusicaFondo.Instancia.ReproducirMusica(musicaAmbienteClip);
		}
	}

	void OnEnable()
	{
		// Suscribirse al evento de recogida al habilitar el script
		FormaPersonalizada.OnColeccionableRecogido += ManejarSonidoColeccionable;
	}

	void OnDisable()
	{
		// Desuscribirse del evento al deshabilitar el script (limpieza importante)
		FormaPersonalizada.OnColeccionableRecogido -= ManejarSonidoColeccionable;
	}

	void ManejarSonidoColeccionable(FormaPersonalizada.TipoForma forma)
	{
		// Si la instancia de MusicaFondo se destruyó o el sfxSource no está, salir
		if (MusicaFondo.Instancia == null || sfxSource == null) return;

		switch (forma)
		{
		case FormaPersonalizada.TipoForma.Cubo:
			// Si es un cubo, reproducir un sonido (PlayOneShot evita interrumpir otros SFX)
			if (sfxCuboClip != null)
			{
				sfxSource.PlayOneShot(sfxCuboClip);
			}
			break;

		case FormaPersonalizada.TipoForma.Anillo:
			// Si es un anillo, apagar la música llamando al método del Singleton
			MusicaFondo.Instancia.DetenerMusica();
			break;
		}
	}
}