using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class ListaFacultades
{
    public string facultad;
    public List<string> listaCarreras;
}

public class RegistrarEstudiante : MonoBehaviour {

    public BDLocal bdLocal;
    public Utilidades utilidades;
    public Camara camara;

    public InputField nombre;
    public InputField apellido;
    public InputField cedula;
    public InputField email;
    public Dropdown departamento;
    public Dropdown programa;
    public ToggleGroup sexo;
    public ToggleGroup gafas;
    public InputField año;
    public InputField mes;
    public InputField dia;
    public InputField semestre;

    public GameObject formulario;
    public GameObject mensajeRegistrando;
    public GameObject mensajeExito;
    public GameObject mensajeExiste;
    public GameObject mensajeError;

    public List<ListaFacultades> listaFacultades;

    public void Start()
    {
        List<string> listaF = new List<string>();
        List<string> listaP = new List<string>();

        departamento.ClearOptions();
        programa.ClearOptions();

        for (int i = 0; i < listaFacultades.Count; i++)
            listaF.Add(listaFacultades[i].facultad);

        for (int i = 0; i < listaFacultades[2].listaCarreras.Count; i++)
            listaP.Add(listaFacultades[2].listaCarreras[i]);

        departamento.AddOptions(listaF);
        programa.AddOptions(listaP);
        departamento.value = 2;
    }

    public void CambiarProgramas()
    {
        List<string> listaP = new List<string>();
        programa.ClearOptions();

        for (int i = 0; i < listaFacultades[departamento.value].listaCarreras.Count; i++)
            listaP.Add(listaFacultades[departamento.value].listaCarreras[i]);

        programa.AddOptions(listaP);
    }

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeRegistrando.SetActive(false);
        if (bdLocal.rRegistrarEstudiante == "Exito")
        {
            BorrarInformacion();
            camara.ReiniciarCamara();
            mensajeExito.SetActive(true);
        }
        else if (bdLocal.rRegistrarEstudiante == "Existe")
            mensajeExiste.SetActive(true);
        else if (bdLocal.rRegistrarEstudiante == "Error")
            mensajeError.SetActive(true);

        yield return new WaitForSeconds(2f);

        formulario.SetActive(true);
        mensajeExito.SetActive(false);
        mensajeExiste.SetActive(false);
        mensajeError.SetActive(false);
        bdLocal.rRegistrarEstudiante = "";
    }

    public void Registrar()
    {       
        string estSexo = "";
        string estGafas = "";

        foreach (Toggle toggle in sexo.ActiveToggles())
            estSexo = toggle.GetComponentInChildren<Text>().text;

        foreach (Toggle toggle in gafas.ActiveToggles())
            estGafas = toggle.GetComponentInChildren<Text>().text;

        if (VerificarDatosRegistro())
        {
            string urlCuestionario = Application.persistentDataPath + "/Cuestionarios/" + nombre.text + "_" + cedula.text + ".xlsx";
            camara.GuardarFoto();
            formulario.SetActive(false);
            mensajeRegistrando.SetActive(true);
            bdLocal.InsertarEstudiante(cedula.text, nombre.text, apellido.text,email.text, departamento.options[departamento.value].text, programa.options[programa.value].text, estSexo, estGafas, año.text+"-"+mes.text+"-"+dia.text, semestre.text, camara.URLFoto(),"Si", urlCuestionario);
        }
    }

    public void BorrarInformacion()
    {
        nombre.text = "";
        apellido.text = "";
        cedula.text = "";
        email.text = "";
        departamento.value = 0;
        programa.value = 0;
        año.text = "";
        mes.text = "";
        dia.text = "";
        semestre.text = "";

        nombre.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        apellido.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        cedula.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        email.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        año.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        mes.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        dia.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        semestre.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
    }

    public bool VerificarDatosRegistro()
    {
        if (nombre.text.Length >= 4 && apellido.text.Length >= 4 && cedula.text.Length >= 8 && email.text.Length >= 12
            && año.text.Length == 4 && mes.text.Length > 0 && dia.text.Length > 0 && semestre.text.Length > 0 && semestre.text !="0"
            && ValidacionEmail(email.text) == true && camara.FotoTomada() != null)
        {
            return true;
        }
        else
        {
            if(camara.FotoTomada() == null)
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
            if (año.text == "" || año.text.Length < 3)
            {
                año.transform.Find("Text").GetComponent<Text>().color = Color.red;
                año.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (mes.text == "" || mes.text.Length < 1)
            {
                mes.transform.Find("Text").GetComponent<Text>().color = Color.red;
                mes.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (dia.text == "" || dia.text.Length < 0)
            {
                dia.transform.Find("Text").GetComponent<Text>().color = Color.red;
                dia.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (semestre.text == "" || semestre.text == "0" || semestre.text.Length < 1)
            {
                semestre.transform.Find("Text").GetComponent<Text>().color = Color.red;
                semestre.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
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
