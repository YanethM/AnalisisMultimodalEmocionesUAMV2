using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Utilidades : MonoBehaviour {

    public Color colorTexto;
    public GameObject mensajeCarga;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))        
            Application.Quit();        
    }

    public Sprite CargarFoto(string urlFoto, int ancho, int alto)
    {
        if (string.IsNullOrEmpty(urlFoto)) return null;
        if (File.Exists(urlFoto))
        {
            byte[] bytes = File.ReadAllBytes(urlFoto);
            Texture2D textura = new Texture2D(ancho, alto, TextureFormat.RGB24, false);
            textura.filterMode = FilterMode.Trilinear;
            textura.LoadImage(bytes);
            Sprite sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), Vector2.zero);
            return sprite;
        }
        return null;
    }

    public IEnumerator CargarImagen(string urlFoto, Image imagen)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://"+urlFoto);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
            imagen.sprite = Sprite.Create(((DownloadHandlerTexture)www.downloadHandler).texture, new Rect(0, 0, ((DownloadHandlerTexture)www.downloadHandler).texture.width, ((DownloadHandlerTexture)www.downloadHandler).texture.height), Vector2.zero); ;
    }

    public void CambiarColorTexto(GameObject campo)
    {
        campo.transform.Find("Text").GetComponent<Text>().color = colorTexto;
        campo.transform.Find("Placeholder").GetComponent<Text>().color = new Color(colorTexto.r, colorTexto.g, colorTexto.b, 0.5f);
    }

    public void CargarEscena(string escena)
    {
        StartCoroutine(Escena(escena));
    }

    IEnumerator Escena(string escena)
    {
        AsyncOperation carga = SceneManager.LoadSceneAsync(escena);
        mensajeCarga.SetActive(true);
        while (!carga.isDone)
        {
            yield return null;
        }
    }
}
