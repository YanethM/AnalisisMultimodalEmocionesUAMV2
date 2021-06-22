using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InciarSesion : MonoBehaviour {

    public BDLocal bdLocal;
    public Utilidades utilidades;
    public Camara camara;

    public InputField email;
    public InputField contrasena;

    public GameObject formulario;
    public GameObject mensajeIngresando;
    public GameObject mensajeExito;
    public GameObject mensajeNoExiste;
    public GameObject mensajeError;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        email.text = PlayerPrefs.GetString("Email");
        contrasena.text = PlayerPrefs.GetString("Contrasena");
    }

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeIngresando.SetActive(false);
        if (bdLocal.rIniciarSesion == "Exito")
        {            
            mensajeExito.SetActive(true);
            PlayerPrefs.SetString("Email", email.text);
            PlayerPrefs.SetString("Contrasena", contrasena.text);
            camara.DetenerCamara();
            BorrarInformacion();
        }
        else if (bdLocal.rIniciarSesion == "No Existe")
            mensajeNoExiste.SetActive(true);
        else if (bdLocal.rIniciarSesion == "Contraseña Erronea")
            mensajeError.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (bdLocal.rIniciarSesion == "Exito")        
            utilidades.CargarEscena("2 - Menu Principal");        
        else        
            formulario.SetActive(true);
        
        mensajeExito.SetActive(false);
        mensajeNoExiste.SetActive(false);
        mensajeError.SetActive(false);
        bdLocal.rIniciarSesion = "";
    }

    public void IniciarSesion()
    {
        if (VerificarDatosRegistro())
        {
            formulario.SetActive(false);
            mensajeIngresando.SetActive(true);
            bdLocal.IniciarSesion(email.text,contrasena.text);
        }
    }

    public bool VerificarDatosRegistro()
    {
        if (email.text.Length >= 8 && contrasena.text.Length >= 7 )
        {
            return true;
        }
        else
        {
            if (email.text == "" || email.text.Length < 8)
            {
                email.transform.Find("Text").GetComponent<Text>().color = Color.red;
                email.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (contrasena.text == "" || contrasena.text.Length < 7)
            {
                contrasena.transform.Find("Text").GetComponent<Text>().color = Color.red;
                contrasena.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            return false;
        }
    }

    public void BorrarInformacion()
    {
        email.text = "";
        contrasena.text = "";

        email.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        contrasena.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
    }
}
