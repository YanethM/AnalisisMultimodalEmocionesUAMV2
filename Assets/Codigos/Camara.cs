using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Camara : MonoBehaviour {
   
    public RawImage imagen;
    public Button tomarFoto;
    public Button otraFoto;
    public GameObject mensajeAdvertencia;

    private Texture2D nuevaFoto;
    private WebCamTexture camara = null;
    private WebCamDevice[] cantidad = null;

    private string carpetaImagenes;
    private string urlFoto;

    private bool camaraIniciada = false;

    public void IniciarCamara()
    {
        carpetaImagenes = Application.persistentDataPath + "/Galeria";
        if (Directory.Exists(carpetaImagenes) == false)
        {
            Directory.CreateDirectory(carpetaImagenes);
        }

        cantidad = WebCamTexture.devices;
        if (cantidad.Length > 0 && camaraIniciada == false)
        {
            camaraIniciada = true;
            camara = new WebCamTexture();
            imagen.texture = camara;
            imagen.material.mainTexture = camara;
            camara.Play();
        }
    }

    public void DetenerCamara()
    {
        if (camaraIniciada)
        {
            camaraIniciada = false;
            camara.Stop();
        }
    }

    public void ReiniciarCamara()
    {
        tomarFoto.gameObject.SetActive(true);
        otraFoto.gameObject.SetActive(false);
        nuevaFoto = null;
        camara.Play();
    }

    public void TomarFoto()
    {
        if (cantidad.Length > 0)
        {
            tomarFoto.gameObject.SetActive(false);
            otraFoto.gameObject.SetActive(true);
            camara.Pause();
            StartCoroutine(Foto());
        }
    }

    IEnumerator Foto()
    {
        yield return new WaitForEndOfFrame();

        nuevaFoto = new Texture2D(camara.width, camara.height);
        nuevaFoto.SetPixels(camara.GetPixels());
        nuevaFoto.Apply();
        camara.Stop();
    }

    public IEnumerator MensajeAdvertencia()
    {
        mensajeAdvertencia.SetActive(true);
        yield return new WaitForSeconds(1);
        mensajeAdvertencia.SetActive(false);
    }
    public void GuardarFoto()
    {
        byte[] bytes = nuevaFoto.EncodeToPNG();
        string fecha = System.DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");
        urlFoto = carpetaImagenes + "/Img_" + fecha + ".png";
        File.WriteAllBytes(urlFoto, bytes);
    }

    public string URLFoto()
    {
        return urlFoto;
    }

    public Texture2D FotoTomada()
    {
        return nuevaFoto;
    }
}
