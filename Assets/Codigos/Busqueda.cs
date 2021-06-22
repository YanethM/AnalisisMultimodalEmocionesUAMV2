using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Busqueda : MonoBehaviour {

    public ListarInformacion informacion;
    public Transform contenedorEstudiantes;
    public Transform contenedorProtocolo;
    public string tipoBusqueda { get; set; }

    private Transform[] listaEstudiantes;
    private Transform[] listaProtocolos;
    
    private void Inicializar()
    {
        int contador = 0;

        listaEstudiantes = new Transform[contenedorEstudiantes.childCount];
        foreach (Transform estudiante in contenedorEstudiantes)
        {
            listaEstudiantes[contador] = estudiante;
            contador++;
        }

        contador = 0;
        listaProtocolos = new Transform[contenedorProtocolo.childCount];
        foreach (Transform protocolo in contenedorProtocolo)
        {
            listaProtocolos[contador] = protocolo;
            contador++;
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
                foreach (Estudiante estudiante in informacion.estudiantes)
                {
                    if (estudiante.nombre.ToLower().Contains(busqueda.text) || estudiante.cedula.ToLower().Contains(busqueda.text))
                        listaEstudiantes[contador].gameObject.SetActive(true);
                    else
                        listaEstudiantes[contador].gameObject.SetActive(false);
                    contador++;
                }
            }
            if (tipoBusqueda == "Protocolo")
            {
                foreach (Protocolo protocolo in informacion.protocolos)
                {
                    if (protocolo.nombre.ToLower().Contains(busqueda.text))
                        listaProtocolos[contador].gameObject.SetActive(true);
                    else
                        listaProtocolos[contador].gameObject.SetActive(false);
                    contador++;
                }
            }
        }
        else
        {
            for (int i = 0; i < informacion.estudiantes.Count; i++)
                listaEstudiantes[i].gameObject.SetActive(true);

            for (int i = 0; i < informacion.protocolos.Count; i++)
                listaProtocolos[i].gameObject.SetActive(true);
        }
    }
}
