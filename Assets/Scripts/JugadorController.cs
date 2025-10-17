using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Agregamos
using UnityEngine.UI;

public class JugadorController : MonoBehaviour
{
    //Declarlo la variable de tipo RigidBody que luego asociaremos a nuestro Jugador
    private Rigidbody rb;

    //Inicializo el contador de coleccionables recogidos
    private int contador;

    //Inicializo variables para los textos
    public Text textoContador, textoGanar;

    //Declaro la variable pública velocidad para poder modificarla desde la Inspector window
    public float velocidad;

    // Use this for initialization
    void Start()
    {
        //Capturo esa variable al iniciar el juego
        rb = GetComponent<Rigidbody>();

        //Inicio el contador a 0 contador = 0;
        contador = 0;

        //Actualizo el texto del contador por pimera vez
        setTextoContador();

        //Inicio el texto de ganar a vacío
        textoGanar.text = "";
    }

    // Para que se sincronice con los frames de física del motor
    void FixedUpdate()
    {
        //Estas variables nos capturan el movimiento en horizontal y vertical de nuestro teclado
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        //Un vector 3 es un trío de posiciones en el espacio XYZ, en este caso el que corresponde al movimiento
        Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);

        //Asigno ese movimiento o desplazamiento a mi RigidBody, multiplicado por la velocidad que quiera darle
        rb.AddForce(movimiento * velocidad);
    }

    //Se ejecuta al entrar a un objeto con la opción isTrigger seleccionada
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coleccionable"))
        {
            //Desactivo el objeto
            other.gameObject.SetActive(false);

            //Incremento el contador en uno (también se peude hacer como contador++)
            contador = contador + 1;

            //Actualizo elt exto del contador
            setTextoContador();
        }
    }

    //Actualizo el texto del contador (O muestro el de ganar si las ha cogido todas)
    void setTextoContador()
    {
        textoContador.text = "Contador: " + contador.ToString();
        if (contador >= 12)
        {
			textoGanar.text = "HAS GANADO!";
        StartCoroutine(
            EsperarYVolver()
        );

        }
    }
        IEnumerator EsperarYVolver()
{
    yield return new WaitForSeconds(10f);
            Application.LoadLevel("MenuPrincipal");
}
}
