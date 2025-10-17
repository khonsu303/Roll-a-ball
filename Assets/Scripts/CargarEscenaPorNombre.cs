using UnityEngine;

public class CargarEscenaPorNombre : MonoBehaviour
{
    public void CargarEscena(string nombreEscena)
    {
        Application.LoadLevel(nombreEscena);
    }
}
