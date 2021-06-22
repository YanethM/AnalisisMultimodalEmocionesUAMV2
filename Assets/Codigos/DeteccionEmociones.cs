using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using System.Text;
using System.Linq;
using System;
using ChartAndGraph;

public class DeteccionEmociones : ImageResultsListener
{
    public string[] datos;
    public float[] deteccionValores;
    public GraphChart grafica;

    public List<float> alegria = new List<float>();
    public List<float> sorpresa = new List<float>();
    public List<float> tristeza = new List<float>();
    public List<float> temor = new List<float>();
    public List<float> enojo = new List<float>();
    public List<float> desprecio = new List<float>();
    public List<float> disgusto = new List<float>();
    public List<float> valencia = new List<float>();
    public List<float> compromiso = new List<float>();

    public bool comenzar { get; set; }

    private string[] emocionesDatos;
    private float ultimoSegundo = 0f;
    private float tiempo = 0;
    
    private void Start()
    {
        tiempo = 0;
    }

    public override void onFaceFound(float timestamp, int faceId)
    {
        Debug.Log("Found the face");
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        Debug.Log("Lost the face");
    }

    public override void onImageResults(Dictionary<int, Face> faces)
    {
        if (faces.Count > 0)
        {
            DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();
            if (dfv != null)
            {
                dfv.ShowFace(faces[0]);
            }
            datos = faces[0].ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < 9; i++)
            {
                emocionesDatos = datos[19 + i].Split(new string[] { ":" }, StringSplitOptions.None);
                float valor = float.Parse(emocionesDatos[1]);     
                deteccionValores[i] = valor;                
            }
        }
    }

    void Update()
    {
        if (comenzar == true)
        {
            tiempo += Time.deltaTime;
            if (ultimoSegundo + 1f < tiempo)
            {
                ultimoSegundo = tiempo;
                grafica.DataSource.AddPointToCategoryRealtime("Alegria", tiempo, deteccionValores[0], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Temor", tiempo, deteccionValores[1], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Disgusto", tiempo, deteccionValores[2], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Tristeza", tiempo, deteccionValores[3], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Enojo", tiempo, deteccionValores[4], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Sorpresa", tiempo, deteccionValores[5], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Desprecio", tiempo, deteccionValores[6], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Valencia", tiempo, deteccionValores[7], 1f);
                grafica.DataSource.AddPointToCategoryRealtime("Compromiso", tiempo, deteccionValores[8], 1f);
                alegria.Add(deteccionValores[0]);
                temor.Add(deteccionValores[1]);
                disgusto.Add(deteccionValores[2]);
                tristeza.Add(deteccionValores[3]);
                enojo.Add(deteccionValores[4]);
                sorpresa.Add(deteccionValores[5]);
                desprecio.Add(deteccionValores[6]);
                valencia.Add(deteccionValores[7]);
                compromiso.Add(deteccionValores[8]);
            }
        }
    }
}
