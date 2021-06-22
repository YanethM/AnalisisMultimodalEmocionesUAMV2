using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class RegistrarProtocolo : MonoBehaviour {

    public BDLocal bdLocal;
    public Utilidades utilidades;

    public Dropdown tipo;
    public InputField nombre;
    public Dropdown departamento;
    public InputField intervalo;

    public GameObject listaImagenes;
    public GameObject listaVideos;
    public Transform contenedorImagenes;
    public Transform contenedorVideos;
    public GameObject imagen;
    public GameObject video;

    public GameObject formulario;
    public GameObject mensajeRegistrando;
    public GameObject mensajeExito;
    public GameObject mensajeError;
    public GameObject mensajeImagenes;
    public GameObject mensajeVideo;

    private int contImgs = 0;
    private int contVideos = 0;
    private string carpetaProtocolo;
    public List<string> urlContenido = new List<string>();

    private void Start()
    {
        foreach (Transform hijo in contenedorImagenes)
            Destroy(hijo.gameObject);

        foreach (Transform hijo in contenedorVideos)
            Destroy(hijo.gameObject);
    }

    public void Mensaje()
    {
        StartCoroutine(Resultado());
    }

    IEnumerator Resultado()
    {
        mensajeRegistrando.SetActive(false);
        if (bdLocal.rRegistrarProtocolo == "Exito")
        {
            CrearCarpeta();
            BorrarInformacion();
            mensajeExito.SetActive(true);
        }
        else if (bdLocal.rRegistrarProtocolo == "Error")
            mensajeError.SetActive(true);

        yield return new WaitForSeconds(2f);

        formulario.SetActive(true);
        mensajeExito.SetActive(false);
        mensajeError.SetActive(false);
        bdLocal.rRegistrarProtocolo = "";
    }

    public void Registrar()
    {
        if (VerificarDatosRegistro())
        {
            formulario.SetActive(false);
            mensajeRegistrando.SetActive(true);
            carpetaProtocolo = Application.persistentDataPath + "/Protocolos/" + bdLocal.CantidadRegistros("Protocolo") + "-" + nombre.text.Trim() + "/";
            bdLocal.InsertarProtocolo(tipo.options[tipo.value].text, nombre.text, departamento.options[departamento.value].text, intervalo.text, carpetaProtocolo ,"Si");
        }
    }

    public void BorrarInformacion()
    {
        foreach (Transform hijo in contenedorImagenes)
            Destroy(hijo.gameObject);

        foreach (Transform hijo in contenedorVideos)
            Destroy(hijo.gameObject);

        urlContenido.Clear();

        tipo.value = 0;
        nombre.text = "";
        departamento.value = 0;
        intervalo.text = "";

        nombre.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
        intervalo.transform.Find("Text").GetComponent<Text>().color = utilidades.colorTexto;
    }

    public bool VerificarDatosRegistro()
    {
        if (tipo.value == 0)
        {
            if (nombre.text.Length >= 4 && intervalo.text.Length > 0 && intervalo.text != "0" && contImgs > 2)
            {
                return true;
            }
        }
        else {
            if (nombre.text.Length >= 4 && contVideos > 0)
            {
                return true;
            }
        }
        if (tipo.value != 0)
        {
            if (contVideos <= 0)
                mensajeVideo.SetActive(true);
        }
        else {
            if (intervalo.text == "" || intervalo.text == "0" || intervalo.text.Length < 1)
            {
                intervalo.transform.Find("Text").GetComponent<Text>().color = Color.red;
                intervalo.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
            }
            if (contImgs <= 2)
                mensajeImagenes.SetActive(true);

        }
        if (nombre.text == "" || nombre.text.Length < 4)
        {
            nombre.transform.Find("Text").GetComponent<Text>().color = Color.red;
            nombre.transform.Find("Placeholder").GetComponent<Text>().color = Color.red;
        }

        return false;        
    }

    public void MostrarVentana(string tipo)
    {
        if (tipo == "Imagen")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Imagenes", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
            FileBrowser.SetDefaultFilter(".jpg");
        }
        if (tipo == "Video")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Videos", ".mp4", ".ogv"));
            FileBrowser.SetDefaultFilter(".mp4");
        }
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(CargarVentana(tipo));
    }

    public void CambiarTipo()
    {
        intervalo.text = "0";
        if (tipo.options[tipo.value].text == "Lista de Imagenes")
        {
            listaImagenes.SetActive(true);
            listaVideos.SetActive(false);
            mensajeImagenes.SetActive(false);
            mensajeVideo.SetActive(false);
            intervalo.transform.parent.gameObject.SetActive(true);
        }

        if (tipo.options[tipo.value].text == "Video Local")
        {
            listaImagenes.SetActive(false);
            listaVideos.SetActive(true);
            mensajeImagenes.SetActive(false);
            mensajeVideo.SetActive(false);
            intervalo.transform.parent.gameObject.SetActive(false);
        }
    }

    public void CrearCarpeta()
    {
        if (!Directory.Exists(carpetaProtocolo))
        {
            try
            {
                Directory.CreateDirectory(carpetaProtocolo);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }

            if (tipo.options[tipo.value].text == "Lista de Imagenes")
            {
                for (int i = 0; i < urlContenido.Count; i++)
                    CopiarArchivo(urlContenido[i], carpetaProtocolo, "Img_" + i + ".png");
            }

            if (tipo.options[tipo.value].text == "Video Local")
            {
                for (int i = 0; i < urlContenido.Count; i++)
                    CopiarArchivo(urlContenido[i], carpetaProtocolo, "Video_" + i + ".mp4");                
            }
            urlContenido.Clear();
        }    
    }

    IEnumerator CargarVentana(string tipo)
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);

            if (tipo == "Imagen")
            {
                contImgs++;
                GameObject imagenProtocolo = Instantiate(imagen, imagen.transform.position, imagen.transform.rotation);
                imagenProtocolo.name = imagenProtocolo.name + " " + contImgs;
                StartCoroutine(utilidades.CargarImagen(FileBrowser.Result, imagenProtocolo.transform.GetChild(0).GetComponent<Image>()));

                string url = FileBrowser.Result;
                imagenProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContenido(tipo,imagenProtocolo, url);
                });

                urlContenido.Add(FileBrowser.Result);
                imagenProtocolo.transform.SetParent(contenedorImagenes, false);
            }
            if (tipo == "Video")
            {
                contVideos++;
                GameObject videoProtocolo = Instantiate(video, video.transform.position, video.transform.rotation);
                videoProtocolo.name = videoProtocolo.name + " " + contVideos;

                string url = FileBrowser.Result;
                videoProtocolo.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
                {
                    BorrarContenido(tipo,videoProtocolo, url);
                });

                urlContenido.Add(FileBrowser.Result);
                videoProtocolo.transform.SetParent(contenedorVideos, false);
            }
        }
    }

    public void CopiarArchivo(string url,string destino,string nombre)
    {
        byte[] bytes = File.ReadAllBytes(url);
        File.WriteAllBytes(destino + "/"+ nombre, bytes);
    }

    private void BorrarContenido(string tipo, GameObject obj,string url)
    {
        if (tipo == "Imagen")
            contImgs--;
        if (tipo == "Video")
            contVideos--;
        urlContenido.Remove(url);
        Destroy(obj);
    }
}
