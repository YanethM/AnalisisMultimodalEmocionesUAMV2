using Excel;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class AplicarProtocolo : MonoBehaviour {

    public BDLocal bdLocal;
    public ListarInformacion informacion;
    public Utilidades utilidades;
    public Record grabar;

    public GameObject aplicarProtocolo;
    public GameObject deteccionEmociones;
    public GameObject deteccionParametros;
    public GameObject mensajeError;
    public GameObject mensajeEspera;
    public GameObject mensajeFinalizo;
    public GameObject mensajeGIF;
    public Image imagen;
    public VideoPlayer video;
    public Text tiempo;
    public List<Sprite> imagenes = new List<Sprite>();
    public List<string> videos = new List<string>();
    public List<string> contenidoCambio = new List<string>();
    public UnityEvent iniciarProtocolo;
    public UnityEvent terminarProtocolo;

    private bool comenzar { get; set; }
    private bool generarGIF { get; set; }
    private string tipoProtocolo = "";
    private double segundosProtocolo = 0;
    private int contadorContenido = 0;
    private int contadorVideos = 0;
    private float tiempoTranscurrido = 0;
    private float tiempoIntervalo = 0;
    private int intervalo = 0;
    private string carpetaResultados;
    private string carpetaEmotiv;

    private int protocolo;
    private int estudiante;
    private string fecha;
    private double ultimoSegundo = 0f;

    void Start()
    {
        generarGIF = false;
        carpetaResultados = Application.persistentDataPath + "/Resultados CSV/";
        carpetaEmotiv = Application.persistentDataPath + "/Resultados Emotiv/";
        CrearCarpeta();
    }

    void Update ()
    {
        if (comenzar == true)
        {
            if (tipoProtocolo == "Lista de Imagenes")
            {
                if (tiempoTranscurrido < segundosProtocolo)
                {
                    tiempoTranscurrido += Time.deltaTime;
                    tiempo.text = string.Format("{0}:{1:00}", Mathf.FloorToInt(tiempoTranscurrido) / 60, (Math.Abs(Mathf.FloorToInt(tiempoTranscurrido))) % 60);
                    if (tiempoIntervalo < intervalo)
                    {
                        tiempoIntervalo += Time.deltaTime;
                    }
                    else
                    {
                        contadorContenido++;
                        tiempoIntervalo = 0;
                        if (contadorContenido < imagenes.Count)
                            imagen.sprite = imagenes[contadorContenido];
                    }

                    if (ultimoSegundo + 1f < tiempoTranscurrido)
                    {
                        ultimoSegundo = tiempoTranscurrido;
                        contenidoCambio.Add("Img_" + contadorContenido);
                    }
                }
                else
                {                    
                    comenzar = false;
                    mensajeGIF.SetActive(true);
                    deteccionParametros.SetActive(false);
                    terminarProtocolo.Invoke();
                    generarGIF = true;
                    grabar.GuardarGif(carpetaResultados, informacion.estudiantes[estudiante - 1].nombre + "_" + fecha + "_" + informacion.protocolos[protocolo - 1].nombre);
                }
            }
            if (tipoProtocolo == "Video Local")
            {
                tiempoTranscurrido += Time.deltaTime;
                tiempo.text = string.Format("{0}:{1:00}", Mathf.FloorToInt(tiempoTranscurrido) / 60, (Math.Abs(Mathf.FloorToInt(tiempoTranscurrido))) % 60);

                if (ultimoSegundo + 1f < tiempoTranscurrido)
                {
                    ultimoSegundo = tiempoTranscurrido;
                    contenidoCambio.Add("Video_" + contadorVideos);
                }
            }
        }
        else {
            if (generarGIF == true)
            {
                mensajeGIF.transform.GetChild(3).GetComponent<Text>().text = "Generando GIF " + (Mathf.FloorToInt(grabar.progreso) + 1) + "%";
                if (Mathf.FloorToInt(grabar.progreso) + 1 == 100)
                {
                    generarGIF = false;
                    StartCoroutine(MensajeFinalizo());
                }
            }
        }
    }

    public void VerificarProtocolo()
    {
        protocolo = PlayerPrefs.GetInt("ProtocoloID");
        estudiante = PlayerPrefs.GetInt("EstudianteID");
        fecha = DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");

        if (protocolo != 0 && estudiante != 0)
        {
            imagenes.Clear();
            videos.Clear();
            aplicarProtocolo.SetActive(true);
            deteccionParametros.SetActive(true);
            deteccionEmociones.SetActive(true);           
            mensajeEspera.SetActive(false);
            tipoProtocolo = informacion.protocolos[protocolo - 1].tipo;
            string[] archivos = Directory.GetFiles(informacion.protocolos[protocolo - 1].url);

            if (tipoProtocolo == "Lista de Imagenes")
            {                
                for (int i = 0; i < Directory.GetFiles(informacion.protocolos[protocolo - 1].url).Length; i++)            
                    imagenes.Add(utilidades.CargarFoto(informacion.protocolos[protocolo - 1].url + Path.GetFileName(archivos[i]), 1920, 1080));
                intervalo = int.Parse(informacion.protocolos[protocolo - 1].intervalo) / archivos.Length;
                segundosProtocolo = int.Parse(informacion.protocolos[protocolo - 1].intervalo);
                imagen.sprite = imagenes[0];
            }

            if (tipoProtocolo == "Video Local")
            {
                for (int i = 0; i < Directory.GetFiles(informacion.protocolos[protocolo - 1].url).Length; i++)
                    videos.Add(informacion.protocolos[protocolo - 1].url + Path.GetFileName(archivos[i]));
                video.source = VideoSource.Url;
                video.url = informacion.protocolos[protocolo - 1].url + Path.GetFileName(archivos[0]);
                video.Prepare();
                video.Play();
                video.loopPointReached += VideoTermino;
            }
            comenzar = true;
            grabar.GrabarGIF();
            iniciarProtocolo.Invoke();
        }
        else
        {
            StartCoroutine(MensajeError());
        }        
    }

    public void CrearCarpeta()
    {
        if (!Directory.Exists(carpetaResultados))
        {
            try
            {
                Directory.CreateDirectory(carpetaResultados);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }

        if (!Directory.Exists(carpetaEmotiv))
        {
            try
            {
                Directory.CreateDirectory(carpetaEmotiv);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    private void VideoTermino(VideoPlayer videoplayer)
    {
        if (contadorVideos < videos.Count - 1)
        {
            contadorVideos++;
            video.source = VideoSource.Url;
            video.url = videos[contadorVideos];
            video.Prepare();
            video.Play();
        }
        else
        {
            comenzar = false;
            mensajeGIF.SetActive(true);
            deteccionParametros.SetActive(false);
            terminarProtocolo.Invoke();
            generarGIF = true;
            grabar.GuardarGif(carpetaResultados, informacion.estudiantes[estudiante - 1].nombre + "_" + fecha + "_" + informacion.protocolos[protocolo - 1].nombre);
        }
    }

    public void GenerarExcel(DeteccionEmociones emociones)
    {
        float sumatoria = 0;
        List<float> promedios = new List<float>();

        for (int i = 0; i < emociones.alegria.Count; i++)        
            sumatoria += emociones.alegria[i];
        promedios.Add(sumatoria / emociones.alegria.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.temor.Count; i++)
            sumatoria += emociones.temor[i];
        promedios.Add(sumatoria / emociones.temor.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.disgusto.Count; i++)
            sumatoria += emociones.disgusto[i];
        promedios.Add(sumatoria / emociones.disgusto.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.tristeza.Count; i++)
            sumatoria += emociones.tristeza[i];
        promedios.Add(sumatoria / emociones.tristeza.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.enojo.Count; i++)
            sumatoria += emociones.enojo[i];
        promedios.Add(sumatoria / emociones.enojo.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.sorpresa.Count; i++)
            sumatoria += emociones.sorpresa[i];
        promedios.Add(sumatoria / emociones.sorpresa.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.desprecio.Count; i++)
            sumatoria += emociones.desprecio[i];
        promedios.Add(sumatoria / emociones.desprecio.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.compromiso.Count; i++)
            sumatoria += emociones.compromiso[i];
        promedios.Add(sumatoria / emociones.compromiso.Count);

        sumatoria = 0;
        for (int i = 0; i < emociones.valencia.Count; i++)
            sumatoria += emociones.valencia[i];
        promedios.Add(sumatoria / emociones.valencia.Count);

        string archivoExcel = carpetaResultados + informacion.estudiantes[estudiante - 1].nombre + "_" + fecha + "_" + informacion.protocolos[protocolo - 1].nombre + ".xlsx";

        FileInfo nombreExcel = new FileInfo(archivoExcel);
        string nombreHoja = "Resultados";

        if (nombreExcel.Exists)
        {
            nombreExcel = new FileInfo(archivoExcel);
        }
        using (ExcelPackage paquete = new ExcelPackage(nombreExcel))
        {
            ExcelWorksheet excel = paquete.Workbook.Worksheets.Add(nombreHoja);

            excel.Cells[1, 1].Value = "Segundo";
            excel.Cells[1, 2].Value = "Imagen Protocolo";
            excel.Cells[1, 3].Value = "Alegria";
            excel.Cells[1, 4].Value = "Temor";
            excel.Cells[1, 5].Value = "Disgusto";
            excel.Cells[1, 6].Value = "Tristeza";
            excel.Cells[1, 7].Value = "Enojo";
            excel.Cells[1, 8].Value = "Sorpresa";
            excel.Cells[1, 9].Value = "Desprecio";
            excel.Cells[1, 10].Value = "Compromiso";
            excel.Cells[1, 11].Value = "Valencia";

            for (int i = 0; i < emociones.alegria.Count; i++)
            {
                excel.Cells[i + 2, 1].Value = (i + 1);
                excel.Cells[i + 2, 2].Value = contenidoCambio[i];
                excel.Cells[i + 2, 3].Value = emociones.alegria[i];
                excel.Cells[i + 2, 4].Value = emociones.temor[i];
                excel.Cells[i + 2, 5].Value = emociones.disgusto[i];
                excel.Cells[i + 2, 6].Value = emociones.tristeza[i];
                excel.Cells[i + 2, 7].Value = emociones.enojo[i];
                excel.Cells[i + 2, 8].Value = emociones.sorpresa[i];
                excel.Cells[i + 2, 9].Value = emociones.desprecio[i];
                excel.Cells[i + 2, 10].Value = emociones.compromiso[i];
                excel.Cells[i + 2, 11].Value = emociones.valencia[i];
            }

            excel.Cells[1, 14].Value = "Promedio Alegria";
            excel.Cells[1, 15].Value = "Promedio Temor";
            excel.Cells[1, 16].Value = "Promedio Disgusto";
            excel.Cells[1, 17].Value = "Promedio Tristeza";
            excel.Cells[1, 18].Value = "Promedio Enojo";
            excel.Cells[1, 19].Value = "Promedio Sorpresa";
            excel.Cells[1, 20].Value = "Promedio Desprecio";            
            excel.Cells[1, 21].Value = "Promedio Compromiso";
            excel.Cells[1, 22].Value = "Promedio Valencia";

            excel.Cells[2, 14].Value = promedios[0];
            excel.Cells[2, 15].Value = promedios[1];
            excel.Cells[2, 16].Value = promedios[2];
            excel.Cells[2, 17].Value = promedios[3];
            excel.Cells[2, 18].Value = promedios[4];
            excel.Cells[2, 19].Value = promedios[5];
            excel.Cells[2, 20].Value = promedios[6];
            excel.Cells[2, 21].Value = promedios[7];
            excel.Cells[2, 22].Value = promedios[8];

            paquete.Save();
        }

        bdLocal.InsertarProtocoloEstudiante(informacion.estudiantes[estudiante - 1].cedula, informacion.protocolos[protocolo - 1].nombre , archivoExcel, promedios[0], promedios[1], promedios[2], promedios[3], promedios[4], promedios[5], promedios[6], promedios[7], promedios[8]);
    }

    IEnumerator MensajeError()
    {
        mensajeError.SetActive(true);
        if (protocolo == 0 && estudiante == 0)
            mensajeError.transform.GetChild(3).GetComponent<Text>().text = "Te falta escoger el Estudiante/Protocolo";
        else if (protocolo == 0 && estudiante != 0)
            mensajeError.transform.GetChild(3).GetComponent<Text>().text = "Te falta escoger el Protocolo";
        else if (protocolo != 0 && estudiante == 0)
            mensajeError.transform.GetChild(3).GetComponent<Text>().text = "Te falta escoger al Estudiante";
        yield return new WaitForSeconds(2f);
        mensajeError.SetActive(false);
    }

    IEnumerator MensajeFinalizo()
    {
        intervalo = 0;
        contadorContenido = 0;
        tiempoIntervalo = 0;
        segundosProtocolo = 0;
        tiempoTranscurrido = 0;
        mensajeGIF.SetActive(false);
        mensajeFinalizo.SetActive(true);
        yield return new WaitForSeconds(3f);
        mensajeFinalizo.SetActive(false);
        utilidades.CargarEscena("2 - Menu Principal");
    }
}
