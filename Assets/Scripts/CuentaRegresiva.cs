using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CuentaRegresiva : MonoBehaviour
{
    public float duracion = 60f;
    public Text textoTiempo;
    public Text mensajeFinal;

    private float tiempoActual;
    private bool activo = true;

    void Start()
    {
        tiempoActual = duracion;
        mensajeFinal.text = "";
        ActualizarUI();
    }

    void Update()
    {
        if (!activo) return;

        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0f)
        {
            tiempoActual = 0f;
            activo = false;
            mensajeFinal.text = "SE ACABO EL TIEMPO, PERDISTE";
            StartCoroutine(RetornarMenu());
        }

        ActualizarUI();
    }

    void ActualizarUI()
    {
        int min = (int)(tiempoActual / 60);
        int seg = (int)(tiempoActual % 60);
textoTiempo.text = string.Format("{0:00}:{1:00}", min, seg);
    }

    IEnumerator RetornarMenu()
    {
        yield return new WaitForSeconds(5f);
        Application.LoadLevel("MenuPrincipal"); // Compatible con Unity 5.x
    }
}
