using UnityEngine;

public class MovimientoObstaculo : MonoBehaviour
{
    public Vector3 puntoA;
    public Vector3 puntoB;
    public float velocidad = 2f;

    private bool haciaB = true;

    void Update()
    {
        if (haciaB)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoB, velocidad * Time.deltaTime);
            if (Vector3.Distance(transform.position, puntoB) < 0.1f)
                haciaB = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoA, velocidad * Time.deltaTime);
            if (Vector3.Distance(transform.position, puntoA) < 0.1f)
                haciaB = true;
        }
    }
}
