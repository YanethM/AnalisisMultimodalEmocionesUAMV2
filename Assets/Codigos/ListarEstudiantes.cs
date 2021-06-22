using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ChartAndGraph;

public class ListarEstudiantes : MonoBehaviour {

    public ListarInformacion informacion;
    public BDLocal bdLocal;
    public Utilidades utilidades;
    public EmotivInformacion emotivInfo;
    public BarChart barraGrafica1;
    public BarChart barraGrafica2;

    public Transform listaContenedor;
    public Transform listaContenedorProtocolos;
    public GameObject editar;
    public GameObject perfil;
    public GameObject borrar;
    public GameObject graficaAffextiva;
    public GameObject graficaEmotiv;
    public GameObject descripcion;
    public GameObject protocoloNombre;

    public GameObject mensajeNingunEstudiante;
    public GameObject mensajeActualizando;
    public GameObject mensajeExito;
    public GameObject mensajeError;
    private ToggleGroup grupoSeleccion;

    private void Start()
    {
        grupoSeleccion = GetComponent<ToggleGroup>();
        PlayerPrefs.SetInt("EstudianteID",0);
    }

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeActualizando.SetActive(false);
        if (bdLocal.rActualizarEstudiante == "Exito" || bdLocal.rBorrarEstudiante == "Exito")
            mensajeExito.SetActive(true);        
        else if (bdLocal.rActualizarEstudiante == "Error" || bdLocal.rBorrarEstudiante == "Error")
            mensajeError.SetActive(true);

        yield return new WaitForSeconds(2f);

        mensajeExito.SetActive(false);
        mensajeError.SetActive(false);

        editar.SetActive(false);
        perfil.SetActive(false);
        borrar.SetActive(false);

        informacion.ObtenerEstudiantes();
        bdLocal.rActualizarEstudiante = "";
        bdLocal.rBorrarEstudiante = "";        
    }

    public void CrearLista()
    {
        informacion.ListarEstudiantes();
        foreach (Transform hijo in listaContenedor)
            Destroy(hijo.gameObject);

        for (int i = 0; i < informacion.estudiantes.Count; i++)
        {
            GameObject estudiante = Instantiate(descripcion, descripcion.transform.position, descripcion.transform.rotation);
            estudiante.name = descripcion.name + " " + (i + 1);
            estudiante.transform.GetChild(1).GetComponent<Toggle>().group = grupoSeleccion;
            estudiante.transform.GetChild(2).GetComponent<Text>().text = informacion.estudiantes[i].nombre + " " + informacion.estudiantes[i].apellido;
            estudiante.transform.GetChild(3).GetComponent<Text>().text = informacion.estudiantes[i].cedula;
            estudiante.transform.GetChild(4).GetComponent<Text>().text = informacion.estudiantes[i].departamento;
            estudiante.transform.GetChild(5).GetComponent<Text>().text = informacion.estudiantes[i].programa;

            int indice = i;

            estudiante.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                PlayerPrefs.SetInt("EstudianteID", indice + 1);
            });

            estudiante.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate
            {
                editar.SetActive(true);
                Editar(informacion.estudiantes[indice].cedula, informacion.estudiantes[indice].nombre,
                    informacion.estudiantes[indice].apellido, informacion.estudiantes[indice].email, informacion.estudiantes[indice].departamento, informacion.estudiantes[indice].programa, informacion.estudiantes[indice].sexo,
                    informacion.estudiantes[indice].gafas, informacion.estudiantes[indice].fNacimiento, informacion.estudiantes[indice].semestre, utilidades.CargarFoto(informacion.estudiantes[indice].urlFoto, 490, 562));
            });

            estudiante.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(delegate
            {
                perfil.SetActive(true);

                foreach (Transform hijo in listaContenedorProtocolos)
                    Destroy(hijo.gameObject);

                VerPerfil(informacion.estudiantes[indice].cedula, informacion.estudiantes[indice].nombre,
                    informacion.estudiantes[indice].apellido, informacion.estudiantes[indice].email, informacion.estudiantes[indice].departamento, informacion.estudiantes[indice].programa, informacion.estudiantes[indice].sexo,
                    informacion.estudiantes[indice].gafas, informacion.estudiantes[indice].fNacimiento, informacion.estudiantes[indice].semestre, informacion.estudiantes[indice].urlCuestionario, utilidades.CargarFoto(informacion.estudiantes[indice].urlFoto, 490, 562));
            });

            estudiante.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate
            {
                borrar.SetActive(true);
                Borrar(informacion.estudiantes[indice].cedula, informacion.estudiantes[indice].nombre);
            });

            estudiante.transform.SetParent(listaContenedor, false);
        }
        if (informacion.estudiantes.Count == 0)
            mensajeNingunEstudiante.SetActive(true);
        else
            mensajeNingunEstudiante.SetActive(false);
    }

    public void Editar(string cedula, string nombre, string apellido, string email, string departamento, string programa, string sexo, string gafas, string fNacimiento, string semestre, Sprite foto)
    {
        Dropdown dep = editar.transform.GetChild(8).GetChild(2).GetComponent<Dropdown>();
        Dropdown pro = editar.transform.GetChild(9).GetChild(2).GetComponent<Dropdown>();
        ToggleGroup gaf = editar.transform.GetChild(11).GetComponent<ToggleGroup>();
       
        editar.transform.GetChild(4).GetComponent<Image>().sprite = foto;
        editar.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = nombre + " " + apellido;
        editar.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = cedula;
        editar.transform.GetChild(7).GetChild(1).GetComponent<InputField>().text = email;
        dep.value = dep.options.FindIndex(option => option.text == departamento);
        pro.value = pro.options.FindIndex(option => option.text == programa);

        foreach (Transform opcion in editar.transform.GetChild(11))
        {
            if (opcion.gameObject.name == gafas)            
                opcion.GetComponent<Toggle>().isOn = true;
        }
        
        editar.transform.GetChild(10).GetChild(1).GetComponent<Text>().text = sexo;
        editar.transform.GetChild(12).GetChild(1).GetComponent<Text>().text = fNacimiento;
        editar.transform.GetChild(13).GetChild(1).GetComponent<InputField>().text = semestre;

        editar.transform.GetChild(14).GetComponent<Button>().onClick.AddListener(delegate 
        {
            mensajeActualizando.SetActive(true);
            bdLocal.ActualizarEstudiante(cedula, editar.transform.GetChild(7).GetChild(1).GetComponent<InputField>().text, dep.options[dep.value].text, 
                pro.options[pro.value].text, gaf.ActiveToggles().FirstOrDefault().name, editar.transform.GetChild(13).GetChild(1).GetComponent<InputField>().text);
        });
        mensajeActualizando.transform.GetChild(3).GetComponent<Text>().text = "Actualizando";
        mensajeExito.transform.GetChild(3).GetComponent<Text>().text = "Información actualizada";
    }

    public void VerPerfil(string cedula, string nombre, string apellido, string email, string departamento, string programa, string sexo, string gafas, string fNacimiento, string semestre, string urlCuestionario, Sprite foto)
    {
        bdLocal.ObtenerProtocolosHechos(cedula);
        string[] informacion = bdLocal.rObtenerEstudianteProtocolo.Split('&');
        bdLocal.rObtenerEstudianteProtocolo = "";
        perfil.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = nombre + " " + apellido;
        perfil.transform.GetChild(2).GetChild(2).GetComponent<Image>().sprite = foto;
        perfil.transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Text>().text = cedula;
        perfil.transform.GetChild(2).GetChild(4).GetChild(1).GetComponent<Text>().text = email;
        perfil.transform.GetChild(2).GetChild(5).GetChild(1).GetComponent<Text>().text = departamento;
        perfil.transform.GetChild(2).GetChild(6).GetChild(1).GetComponent<Text>().text = programa;
        perfil.transform.GetChild(2).GetChild(7).GetChild(1).GetComponent<Text>().text = sexo;
        perfil.transform.GetChild(2).GetChild(8).GetChild(1).GetComponent<Text>().text = gafas;
        perfil.transform.GetChild(2).GetChild(9).GetChild(1).GetComponent<Text>().text = fNacimiento;
        perfil.transform.GetChild(2).GetChild(10).GetChild(1).GetComponent<Text>().text = semestre;

        perfil.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (File.Exists(urlCuestionario))
            {
                Application.OpenURL(urlCuestionario);
            }
        });

        for (int i = 0; i < informacion.Length-1; i++)
        {
            string[] cadena = informacion[i].Split('?');
            GameObject protocolo = Instantiate(protocoloNombre, protocoloNombre.transform.position, protocoloNombre.transform.rotation);
            protocolo.name = protocoloNombre.name + " " + (i + 1);
            protocolo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = cadena[2];
            protocolo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
            {
                Application.OpenURL(cadena[3]);
            });
            protocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
            {
                graficaAffextiva.SetActive(true);
                GraficarPromedioAffextiva(float.Parse(cadena[4]), float.Parse(cadena[5]), float.Parse(cadena[6]), float.Parse(cadena[7]), float.Parse(cadena[8]), float.Parse(cadena[9]), float.Parse(cadena[10]), float.Parse(cadena[11]), float.Parse(cadena[12]));
            });

            protocolo.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate
            {
                emotivInfo.MostrarVentana(int.Parse(cadena[0]));
            });

            protocolo.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
            {
                bdLocal.ObtenerPromediosEmotiv(int.Parse(cadena[0]));
                string[] emotivInfo = bdLocal.rObtenerPromedioEmotiv.Split('?');
                int suma = int.Parse(emotivInfo[13]) + int.Parse(emotivInfo[14]) + int.Parse(emotivInfo[15]) + int.Parse(emotivInfo[16]) + int.Parse(emotivInfo[17]) + int.Parse(emotivInfo[18]);
                if (suma != 0 )
                {
                    graficaEmotiv.SetActive(true);
                    GraficarPromedioEmotiv(int.Parse(emotivInfo[13]), int.Parse(emotivInfo[14]), int.Parse(emotivInfo[15]), int.Parse(emotivInfo[16]), int.Parse(emotivInfo[17]), int.Parse(emotivInfo[18]));
                }
            });
            protocolo.transform.SetParent(listaContenedorProtocolos, false);
        }       
     }

    public void Borrar(string cedula, string nombre)
    {
        borrar.transform.GetChild(2).GetChild(2).GetComponent<Text>().text ="¿Desea borrar al estudiante " + nombre + " del sistema?";
        borrar.transform.GetChild(2).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
        {
            mensajeActualizando.SetActive(true);
            bdLocal.BorrarEstudiante(cedula);
        });
        mensajeActualizando.transform.GetChild(3).GetComponent<Text>().text = "Borrando";
        mensajeExito.transform.GetChild(3).GetComponent<Text>().text = "Estudiante borrado";
    }

    public void GraficarPromedioAffextiva(float alegria, float temor, float disgusto, float tristeza, float enojo, float sorpresa, float desprecio, float valencia, float compromiso)
    {
        barraGrafica1.DataSource.SetValue("Alegria", "Promedio", Mathf.FloorToInt(alegria));
        barraGrafica1.DataSource.SetValue("Temor", "Promedio", Mathf.FloorToInt(temor));
        barraGrafica1.DataSource.SetValue("Disgusto", "Promedio", Mathf.FloorToInt(disgusto));
        barraGrafica1.DataSource.SetValue("Tristeza", "Promedio", Mathf.FloorToInt(tristeza));
        barraGrafica1.DataSource.SetValue("Enojo", "Promedio", Mathf.FloorToInt(enojo));
        barraGrafica1.DataSource.SetValue("Sorpresa", "Promedio", Mathf.FloorToInt(sorpresa));
        barraGrafica1.DataSource.SetValue("Desprecio", "Promedio", Mathf.FloorToInt(desprecio));
        barraGrafica1.DataSource.SetValue("Valencia", "Promedio", Mathf.FloorToInt(valencia));
        barraGrafica1.DataSource.SetValue("Compromiso", "Promedio", Mathf.FloorToInt(compromiso));
    }

    public void GraficarPromedioEmotiv(int engagement, int excitement, int stress, int relaxation, int interest, int focus)
    {
        barraGrafica2.DataSource.SetValue("Engagement", "Promedio", engagement);
        barraGrafica2.DataSource.SetValue("Excitement", "Promedio", excitement);
        barraGrafica2.DataSource.SetValue("Stress", "Promedio", stress);
        barraGrafica2.DataSource.SetValue("Relaxation", "Promedio", relaxation);
        barraGrafica2.DataSource.SetValue("Interest", "Promedio", interest);
        barraGrafica2.DataSource.SetValue("Focus", "Promedio", focus);
    }
}
