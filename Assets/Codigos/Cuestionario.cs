using Excel;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Pregunta
{
    public string pregunta;
    public Toggle meGusta;
    public Toggle noMeGusta;
}

public class Cuestionario : MonoBehaviour {

    public BDLocal bdLocal;
    public ListarInformacion informacion;
    public GameObject cuestionario;
    public GameObject formulario;
    public GameObject mensajeError;
    public GameObject mensajeRegistrando;

    public List<GameObject> listaPreguntas = new List<GameObject>();
    private List<Pregunta> preguntas = new List<Pregunta>();

    public int indicePreguntas = 0;
    private string carpetaCuestionario;
    private int estudiante;

    void Start ()
    {
        carpetaCuestionario = Application.persistentDataPath + "/Cuestionarios/";
        CrearCarpeta();
        Inicializar();
    }

    public void Inicializar()
    {
        formulario.transform.GetChild(5).gameObject.SetActive(false);
        for (int i = 0; i < listaPreguntas.Count; i++)
        {
            listaPreguntas[i].SetActive(false);
            for (int j = 0; j < listaPreguntas[i].transform.childCount; j++)
            {
                Pregunta pregunta = new Pregunta();
                pregunta.pregunta = listaPreguntas[i].transform.GetChild(j).transform.GetChild(1).GetComponent<Text>().text;
                pregunta.meGusta = listaPreguntas[i].transform.GetChild(j).transform.GetChild(2).GetComponent<Toggle>();
                pregunta.noMeGusta = listaPreguntas[i].transform.GetChild(j).transform.GetChild(3).GetComponent<Toggle>();
                preguntas.Add(pregunta);
            }
        }
        indicePreguntas = 0;
        listaPreguntas[0].SetActive(true);
    }

    public void CrearCarpeta()
    {
        
        if (!Directory.Exists(carpetaCuestionario))
        {
            try
            {
                Directory.CreateDirectory(carpetaCuestionario);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    public void SiguientePregunta()
    {
        indicePreguntas += 1;

        if (indicePreguntas > listaPreguntas.Count - 2)        
            formulario.transform.GetChild(5).gameObject.SetActive(true);        

        if (indicePreguntas > listaPreguntas.Count - 1)        
            indicePreguntas = listaPreguntas.Count - 1;
                 
        listaPreguntas[indicePreguntas - 1].SetActive(false);
        listaPreguntas[indicePreguntas].SetActive(true);
     }

    public void AnteriorPregunta()
    {
        indicePreguntas -= 1;

        if (indicePreguntas < 0)
            indicePreguntas = 0;

        if (indicePreguntas > -1)
        {
            listaPreguntas[indicePreguntas + 1].SetActive(false);
            formulario.transform.GetChild(5).gameObject.SetActive(false);
        }
        listaPreguntas[indicePreguntas].SetActive(true);
    }

    public void VerificarEstudiante()
    {
        estudiante = PlayerPrefs.GetInt("EstudianteID");

        if (estudiante != 0)
        {
            for (int i = 0; i < listaPreguntas.Count; i++)            
                listaPreguntas[i].SetActive(false);  
                      
            indicePreguntas = 0;
            listaPreguntas[0].SetActive(true);

            for (int i = 0; i < preguntas.Count; i++)
            {
                preguntas[i].meGusta.isOn = false;
                preguntas[i].noMeGusta.isOn = false;
            }
            cuestionario.SetActive(true);
        }
        else
        {
            StartCoroutine(MensajeError());
        }
    }

    public void GenerarExcel()
    {
        mensajeRegistrando.SetActive(true);
        string fecha = DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");

        string archivoExcel = carpetaCuestionario + informacion.estudiantes[estudiante - 1].nombre + "_" + informacion.estudiantes[estudiante - 1].cedula + ".xlsx";
        CopiarArchivo(Application.streamingAssetsPath + "/KuderV3.xlsx", carpetaCuestionario, informacion.estudiantes[estudiante - 1].nombre + "_" + informacion.estudiantes[estudiante - 1].cedula + ".xlsx");

        FileInfo nombreExcel = new FileInfo(archivoExcel);

        if (nombreExcel.Exists)
        {
            nombreExcel = new FileInfo(archivoExcel);
        }
        using (ExcelPackage paquete = new ExcelPackage(nombreExcel))
        {
            ExcelWorkbook libro = paquete.Workbook;
            ExcelWorksheet excel = libro.Worksheets.First();
            
            int columna = 0;
            int indicePregunta = 0;
            for (int i = 12; i > 0; i--)
            {      
                columna = (i * 3) - 1;
                for (int j = 1; j < 43; j++)
                {                    
                    if (preguntas[indicePregunta].meGusta.isOn == true)
                    {
                        excel.Cells[j + 1, columna - 1].Value = "x";
                    }
                    else if (preguntas[indicePregunta].noMeGusta.isOn == true)
                    {
                        excel.Cells[j + 1, columna + 1].Value = "x";
                    }
                    indicePregunta += 1;
                }            
            }
            paquete.Save();
        }
        StartCoroutine(MensajeRegistrado());
    }

    public void CopiarArchivo(string url, string destino, string nombre)
    {
        byte[] bytes = File.ReadAllBytes(url);
        File.WriteAllBytes(destino + "/" + nombre, bytes);
    }

    IEnumerator MensajeRegistrado()
    {
        formulario.SetActive(false);        
        yield return new WaitForSeconds(2f);
        mensajeRegistrando.SetActive(false);
        cuestionario.SetActive(false);
        formulario.SetActive(true);        
    }

    IEnumerator MensajeError()
    {
        mensajeError.SetActive(true);
        yield return new WaitForSeconds(2f);
        mensajeError.SetActive(false);
    }
}
