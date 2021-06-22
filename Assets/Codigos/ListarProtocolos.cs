using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ListarProtocolos : MonoBehaviour {

    public ListarInformacion informacion;
    public BDLocal bdLocal;
    public Utilidades utilidades;

    public Transform listaContenedor;
    public GameObject editarImagenes;
    public GameObject editarVideos;
    public GameObject perfilImagenes;
    public GameObject perfilVideo;
    public GameObject borrar;
    public GameObject descripcion;
    public GameObject imagen;
    public GameObject video;
    public GameObject archivoImagen;
    public GameObject archivoVideo;

    public GameObject mensajeNingunProtocolo;
    public GameObject mensajeActualizando;
    public GameObject mensajeExito;
    public GameObject mensajeError;

    private int contImgs = 0;
    private int contVideos = 0;
    public List<string> urlImagenes = new List<string>();
    public List<string> urlVideos = new List<string>();

    private ToggleGroup grupoSeleccion;

    private void Start()
    {
        grupoSeleccion = GetComponent<ToggleGroup>();
        PlayerPrefs.SetInt("ProtocoloID",0);
    }

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeActualizando.SetActive(false);
        if (bdLocal.rActualizarProtocolo == "Exito" || bdLocal.rBorrarProtocolo == "Exito")
            mensajeExito.SetActive(true);
        else if (bdLocal.rActualizarProtocolo == "Error" || bdLocal.rBorrarProtocolo == "Error")
            mensajeError.SetActive(true);

        yield return new WaitForSeconds(2f);

        mensajeExito.SetActive(false);
        mensajeError.SetActive(false);

        editarImagenes.SetActive(false);
        editarVideos.SetActive(false);
        perfilImagenes.SetActive(false);
        perfilVideo.SetActive(false);
        borrar.SetActive(false);

        informacion.ObtenerProtocolos();
        bdLocal.rActualizarProtocolo = "";
        bdLocal.rBorrarProtocolo = "";
    }

    public void CrearLista()
    {
        informacion.ListarProtocolo();

        foreach (Transform hijo in listaContenedor)
            Destroy(hijo.gameObject);

        for (int i = 0; i < informacion.protocolos.Count; i++)
        {
            GameObject protocolo;

            protocolo = Instantiate(descripcion, descripcion.transform.position, descripcion.transform.rotation);
            protocolo.name = descripcion.name + " " + (i + 1);
            protocolo.transform.GetChild(1).GetComponent<Toggle>().group = grupoSeleccion;
            protocolo.transform.GetChild(2).GetComponent<Text>().text = informacion.protocolos[i].nombre;
            protocolo.transform.GetChild(3).GetComponent<Text>().text = informacion.protocolos[i].departamento;
            protocolo.transform.GetChild(4).GetComponent<Text>().text = informacion.protocolos[i].tipo;            

            int indice = i;

            protocolo.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                PlayerPrefs.SetInt("ProtocoloID",indice + 1);
            });

            protocolo.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate
            {
                if (informacion.protocolos[indice].tipo == "Lista de Imagenes")
                {
                    editarImagenes.SetActive(true);
                }
                if (informacion.protocolos[indice].tipo == "Video Local")
                {
                    editarVideos.SetActive(true);
                }
                Editar(int.Parse(informacion.protocolos[indice].indice), informacion.protocolos[indice].tipo, informacion.protocolos[indice].nombre, informacion.protocolos[indice].departamento, 
                    informacion.protocolos[indice].intervalo, informacion.protocolos[indice].url);                
            });

            protocolo.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(delegate
            {
                string[] imagenes = new string[2] { "1", "2" };
                if (informacion.protocolos[indice].tipo == "Lista de Imagenes")
                {
                    perfilImagenes.SetActive(true);
                }
                if (informacion.protocolos[indice].tipo == "Video Local")
                {
                    perfilVideo.SetActive(true);
                }
                VerPerfil(int.Parse(informacion.protocolos[indice].indice), informacion.protocolos[indice].tipo, informacion.protocolos[indice].nombre, informacion.protocolos[indice].departamento,
                    informacion.protocolos[indice].intervalo, informacion.protocolos[indice].url, imagenes);
            });

            protocolo.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate
            {
                borrar.SetActive(true);
                Borrar(int.Parse(informacion.protocolos[indice].indice), informacion.protocolos[indice].nombre);
            });

            protocolo.transform.SetParent(listaContenedor, false);
        }
        if (informacion.protocolos.Count == 0)
            mensajeNingunProtocolo.SetActive(true);
        else
            mensajeNingunProtocolo.SetActive(false);
    }

    public void Editar(int indice, string tipo, string nombre, string departamento, string intervalo, string url)
    {
        if (tipo == "Lista de Imagenes")
        {
            editarImagenes.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = tipo;
            editarImagenes.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = nombre;
            editarImagenes.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = departamento;
            editarImagenes.transform.GetChild(7).GetChild(2).GetComponent<InputField>().text = intervalo;

            string[] archivos = Directory.GetFiles(url);

            foreach (Transform hijo in editarImagenes.transform.GetChild(8).GetChild(0).GetChild(0).transform)
                Destroy(hijo.gameObject);

            for (int i = 0; i < Directory.GetFiles(url).Length; i++)
            {
                GameObject imagenProtocolo;
                imagenProtocolo = Instantiate(imagen, imagen.transform.position, imagen.transform.rotation);
                StartCoroutine(utilidades.CargarImagen(url + Path.GetFileName(archivos[i]), imagenProtocolo.transform.GetChild(0).GetComponent<Image>()));

                int contador = i;

                imagenProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContendio(tipo, imagenProtocolo, archivos[contador], "Antigua");
                });
                imagenProtocolo.transform.SetParent(editarImagenes.transform.GetChild(8).GetChild(0).GetChild(0).transform, false);
            }

            editarImagenes.transform.GetChild(9).GetComponent<Button>().onClick.AddListener(delegate
            {
                mensajeActualizando.SetActive(true);
                bdLocal.ActualizarProtocolo(indice, editarImagenes.transform.GetChild(7).GetChild(2).GetComponent<InputField>().text, url);

                for (int j = 0; j < urlImagenes.Count; j++)                
                    CopiarArchivo(urlImagenes[j], url, "Img_" + (j + urlImagenes.Count) + ".png");
                urlImagenes.Clear();
            });
        }
        if (tipo == "Video Local")
        {
            editarVideos.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = tipo;
            editarVideos.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = nombre;
            editarVideos.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = departamento;

            string[] archivos = Directory.GetFiles(url);

            foreach (Transform hijo in editarVideos.transform.GetChild(7).GetChild(0).GetChild(0).transform)
                Destroy(hijo.gameObject);

            for (int i = 0; i < Directory.GetFiles(url).Length; i++)
            {
                GameObject videoProtocolo;
                videoProtocolo = Instantiate(video, video.transform.position, video.transform.rotation);

                int contador = i;

                videoProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContendio(tipo, videoProtocolo, archivos[contador], "Antigua");
                });
                videoProtocolo.transform.SetParent(editarVideos.transform.GetChild(7).GetChild(0).GetChild(0).transform, false);
            }

            editarVideos.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate
            {
                mensajeActualizando.SetActive(true);
                bdLocal.ActualizarProtocolo(indice,"", url);

                for (int j = 0; j < urlVideos.Count; j++)
                    CopiarArchivo(urlVideos[j], url, "Video_" + (j + urlVideos.Count) + ".mp4");
                urlVideos.Clear();
            });

        }
        mensajeActualizando.transform.GetChild(3).GetComponent<Text>().text = "Actualizando";
        mensajeExito.transform.GetChild(3).GetComponent<Text>().text = "Información actualizada";
    }

    public void VerPerfil(int indice, string tipo, string nombre, string departamento, string intervalo, string url, string[] imagenes)
    {
        if (tipo == "Lista de Imagenes")
        {
            perfilImagenes.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = nombre;
            perfilImagenes.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = departamento;
            perfilImagenes.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = intervalo;

            string[] archivos = Directory.GetFiles(url);

            foreach (Transform hijo in perfilImagenes.transform.GetChild(6).GetChild(0).transform)
                Destroy(hijo.gameObject);

            for (int i = 0; i < Directory.GetFiles(url).Length; i++)
            {
                GameObject imagenProtocolo;

                imagenProtocolo = Instantiate(archivoImagen, archivoImagen.transform.position, archivoImagen.transform.rotation);
                StartCoroutine(utilidades.CargarImagen(url + Path.GetFileName(archivos[i]), imagenProtocolo.GetComponent<Image>()));
                imagenProtocolo.transform.SetParent(perfilImagenes.transform.GetChild(6).GetChild(0).transform, false); 
            }
        }
        if (tipo == "Video Local")
        {
            perfilVideo.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = nombre;
            perfilVideo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = departamento;

            string[] archivos = Directory.GetFiles(url);

            foreach (Transform hijo in perfilVideo.transform.GetChild(5).GetChild(0).transform)
                Destroy(hijo.gameObject);

            for (int i = 0; i < Directory.GetFiles(url).Length; i++)
            {
                GameObject videoProtocolo;

                videoProtocolo = Instantiate(archivoVideo, archivoVideo.transform.position, archivoVideo.transform.rotation);
                videoProtocolo.transform.SetParent(perfilVideo.transform.GetChild(5).GetChild(0).transform, false);
            }
        }
    }

    public void Borrar(int id,string nombre)
    {
        borrar.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "¿Desea borrar el protocolo " + nombre + " del sistema?";
        borrar.transform.GetChild(2).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
        {
            mensajeActualizando.SetActive(true);
            bdLocal.BorrarProtocolo(id);
        });
        mensajeActualizando.transform.GetChild(3).GetComponent<Text>().text = "Borrando";
        mensajeExito.transform.GetChild(3).GetComponent<Text>().text = "Protocolo borrado";
    }

    public void MostrarVentana(string tipo)
    {
        if (tipo == "Lista de Imagenes")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Imagenes", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
            FileBrowser.SetDefaultFilter(".jpg");
        }
        if (tipo == "Video Local")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Videos", ".mp4", ".ogv"));
            FileBrowser.SetDefaultFilter(".mp4");
        }
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(CargarVentana(tipo));
    }

    IEnumerator CargarVentana(string tipo)
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
            if (tipo == "Lista de Imagenes")
            {
                contImgs++;
                GameObject imagenProtocolo = Instantiate(imagen, imagen.transform.position, imagen.transform.rotation);
                imagenProtocolo.name = imagenProtocolo.name + " " + contImgs;
                StartCoroutine(utilidades.CargarImagen(FileBrowser.Result, imagenProtocolo.transform.GetChild(0).GetComponent<Image>()));

                string url = FileBrowser.Result;
                imagenProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContendio(tipo, imagenProtocolo, url, "Nueva");
                });

                urlImagenes.Add(FileBrowser.Result);
                imagenProtocolo.transform.SetParent(editarImagenes.transform.GetChild(8).GetChild(0).GetChild(0).transform, false);             
            }
            if (tipo == "Video Local")
            {
                contVideos++;
                GameObject videoProtocolo = Instantiate(video, video.transform.position, video.transform.rotation);
                videoProtocolo.name = videoProtocolo.name + " " + contVideos;

                string url = FileBrowser.Result;
                videoProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContendio(tipo, videoProtocolo, url, "Nueva");
                });

                urlVideos.Add(FileBrowser.Result);
                videoProtocolo.transform.SetParent(editarVideos.transform.GetChild(7).GetChild(0).GetChild(0).transform, false);
            }            
        }
    }

    private void BorrarContendio(string tipo, GameObject obj, string url, string contendio)
    {
        if (contendio == "Nueva")
        {
            if (tipo == "Lista de Imagenes")
            {
                contImgs--;
                urlImagenes.Remove(url);
            }
            if (tipo == "Video Local")
            {
                contVideos--;
                urlVideos.Remove(url);
            }
        }

        if (contendio == "Antigua")
        {
            File.Delete(url);
        }

        Destroy(obj);
    }

    public void CopiarArchivo(string url, string destino, string nombre)
    {
        byte[] bytes = File.ReadAllBytes(url);
        File.WriteAllBytes(destino + "/" + nombre, bytes);
    }
}
