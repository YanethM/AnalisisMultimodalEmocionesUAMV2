using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListarInformacion : MonoBehaviour {

    public BDLocal bdLocal;
    public Transform contenedorEstudiantes;
    public Transform contenedorProtocolo;
    public string tipoBusqueda { get; set; }

    [SerializeField]
    public Usuario usuario;

    [SerializeField]
    public List<Estudiante> estudiantes;

    [SerializeField]
    public List<Protocolo> protocolos;

    private Transform[] listaEstudiantes;
    private Transform[] listaProtocolos;

    void Start()
    {
        ObtenerUsuario();
        ObtenerEstudiantes();
        ObtenerProtocolos();
        tipoBusqueda = "Estudiante";
    }

    public void Inicializar()
    {
        int contador = 0;

        listaEstudiantes = new Transform[contenedorEstudiantes.childCount];
        foreach (Transform estudiante in contenedorEstudiantes)
        {
            estudiante.gameObject.SetActive(true);
            listaEstudiantes[contador] = estudiante;
            contador++;
        }

        contador = 0;
        listaProtocolos = new Transform[contenedorProtocolo.childCount];
        foreach (Transform protocolo in contenedorProtocolo)
        {
            protocolo.gameObject.SetActive(true);
            listaProtocolos[contador] = protocolo;
            contador++;
        }
    }

    public void ObtenerUsuario()
    {
        bdLocal.ObtenerUsuario(PlayerPrefs.GetString("Cedula"));
    }

    public void ObtenerEstudiantes()
    {
        bdLocal.ObtenerEstudiantes();
    }

    public void ObtenerProtocolos()
    {
        bdLocal.ObtenerProtocolos();
    }

    public void ListarProtocolo()
    {
        if (bdLocal.rObtenerProtocolos != "")
        {
            string[] informacion = bdLocal.rObtenerProtocolos.Split('&');
            protocolos = new List<Protocolo>();
            for (int i = 0; i < informacion.Length - 1; i++)
            {
                string[] cadena = informacion[i].Split('?');
                Protocolo protocolo = new Protocolo();
                protocolo.indice = cadena[0];
                protocolo.tipo = cadena[1];
                protocolo.nombre = cadena[2];
                protocolo.departamento = cadena[3];
                protocolo.intervalo = cadena[4];
                protocolo.url = cadena[5];
                protocolo.habilitado = cadena[6];
                protocolos.Add(protocolo);
            }
            bdLocal.rObtenerProtocolos = "";
        }
    }

    public void ListarUsuario()
    {
        if (bdLocal.rObtenerUsuario != "")
        {
            string[] informacion = bdLocal.rObtenerUsuario.Split('?');
            usuario = new Usuario();
            usuario.cedula = informacion[0];
            usuario.nombre = informacion[1];
            usuario.apellido = informacion[2];
            usuario.email = informacion[3];
            usuario.contrasena = informacion[4];
            usuario.urlFoto = informacion[5];
        }
        bdLocal.rObtenerUsuario = "";
    }

    public void ListarEstudiantes()
    {
        if (bdLocal.rObtenerEstudiantes != "")
        {
            string[] informacion = bdLocal.rObtenerEstudiantes.Split('&');
            estudiantes = new List<Estudiante>();
            for (int i = 0; i < informacion.Length - 1; i++)
            {
                string[] cadena = informacion[i].Split('?');
                Estudiante estudiante = new Estudiante();
                estudiante.cedula = cadena[0];
                estudiante.nombre = cadena[1];
                estudiante.apellido = cadena[2];
                estudiante.email = cadena[3];
                estudiante.departamento = cadena[4];
                estudiante.programa = cadena[5];
                estudiante.sexo = cadena[6];
                estudiante.gafas = cadena[7];
                estudiante.fNacimiento = cadena[8];
                estudiante.semestre = cadena[9];
                estudiante.urlFoto = cadena[10];
                estudiante.habilitado = cadena[11];
                estudiante.urlCuestionario = cadena[12];
                estudiantes.Add(estudiante);
            }
            bdLocal.rObtenerEstudiantes = "";
        }
    }

    public void Buscar(InputField busqueda)
    {
        int contador = 0;
        Inicializar();
        if (busqueda.text != "")
        {
            if (tipoBusqueda == "Estudiante")
            {
                foreach (Estudiante estudiante in estudiantes)
                {
                    if (estudiante.nombre.ToLower().Contains(busqueda.text.ToLower()) || estudiante.apellido.ToLower().Contains(busqueda.text.ToLower()) || estudiante.cedula.ToLower().Contains(busqueda.text.ToLower()))
                        listaEstudiantes[contador].gameObject.SetActive(true);
                    else
                        listaEstudiantes[contador].gameObject.SetActive(false);
                    contador++;
                }
            }
            if (tipoBusqueda == "Protocolo")
            {
                foreach (Protocolo protocolo in protocolos)
                {
                    if (protocolo.nombre.ToLower().Contains(busqueda.text.ToLower()))
                        listaProtocolos[contador].gameObject.SetActive(true);
                    else
                        listaProtocolos[contador].gameObject.SetActive(false);
                    contador++;
                }
            }
        }
        else
        {
            for (int i = 0; i < estudiantes.Count; i++)
                listaEstudiantes[i].gameObject.SetActive(true);

            for (int i = 0; i < protocolos.Count; i++)
                listaProtocolos[i].gameObject.SetActive(true);
        }
    }
}
