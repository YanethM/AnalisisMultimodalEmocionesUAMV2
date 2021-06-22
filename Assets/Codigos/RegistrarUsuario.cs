using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrarUsuario : MonoBehaviour {

    public BDLocal bdLocal;
    public Utilidades utilidades;
    public Camara camara;

    public InputField nombre;
    public InputField apellido;
    public InputField cedula;
    public InputField email;
    public InputField contrasena;

    public GameObject formulario;
    public GameObject mensajeRegistrando;
    public GameObject mensajeExito;
    public GameObject mensajeExiste;
    public GameObject mensajeError;

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeRegistrando.SetActive(false);
        if (bdLocal.rRegistrarUsuario == "Exito")
        {
            BorrarInformacion();
            camara.ReiniciarCamara();
            mensajeExito.SetActive(true);
        }
        else if (bdLocal.rRegistrarUsuario == "Existe")
            mensajeExiste.SetActive(true);
        else if (bdLocal.rRegistrarUsuario == "Error")
            mensajeError.SetActive(true);
        
        yield return new WaitForSeconds(2f);

        formulario.SetActive(true);
        mensajeExito.SetActive(false);
        mensajeExiste.SetActive(false);
        mensajeError.SetActive(false);
        bdLocal.rRegistrarUsuario = "";
    }

    public void Registrar()
    {
        if (VerificarDatosRegistro())
        {
            camara.GuardarFoto();
            formulario.SetActive(false);
            mensajeRegistrando.SetActive(true);
            bdLocal.InsertarUsuario(cedula.text, nombre.text, apellido.text, email.text, contrasena.text, camara.URLFoto());
        }
    }

    public void BorrarInformacion()
    {
        nombre.text = "";
        apellido.text = "";
        cedula.text = "";
        email.text = "";
        contrasena.text = "";

        nombre.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        apellido.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        cedula.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        email.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        contrasena.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
    }

    public bool VerificarDatosRegistro()
    {
        if (nombre.text.Length >= 4 && apellido.text.Length >= 4 && cedula.text.Length >= 8 && email.text.Length >= 12 && contrasena.text.Length >= 7 && ValidacionEmail(email.text) == true && camara.FotoTomada()!=null)
        {
            return true;
        }
        else
        {
            StartCoroutine(camara.MensajeAdvertencia());
            if (nombre.text == "" || nombre.text.Length < 4)
            {
                nombre.transform.Find("Text").GetComponent<Text>().color = Color.red;
                nombre.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (apellido.text == "" || apellido.text.Length < 4)
            {
                apellido.transform.Find("Text").GetComponent<Text>().color = Color.red;
                apellido.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (cedula.text == "" || cedula.text.Length < 8)
            {
                cedula.transform.Find("Text").GetComponent<Text>().color = Color.red;
                cedula.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (email.text == "" || email.text.Length < 12 || ValidacionEmail(email.text) == false)
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

    public bool ValidacionEmail(string valor)
    {
        string[] aux1;
        string[] aux2;
        aux1 = valor.Split("@"[0]);
        if (aux1.Length == 2)
        {
            aux2 = aux1[1].Split("."[0]);
            if (aux2.Length >= 2)
            {
                if (aux2[aux2.Length - 1].Length == 0)                
                    return false;                
                else                
                    return true;                
            }
            else            
                return false;            
        }
        else        
            return false;        
    }
}
