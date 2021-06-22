using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class StreamingGraph : MonoBehaviour
{
    public GraphChart grafica;

    float ultimoSegundo = 0f;

    void Update()
    {
        float tiempo = Time.time;
        if (ultimoSegundo + 1f < tiempo)
        {
            ultimoSegundo = tiempo;
            grafica.DataSource.AddPointToCategoryRealtime("Alegria", tiempo, Random.Range(0,100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Sorpresa", tiempo, Random.Range(0, 100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Tristeza", tiempo, Random.Range(0, 100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Temor", tiempo, Random.Range(0, 100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Enojo", tiempo, Random.Range(0, 100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Desprecio", tiempo, Random.Range(0, 100), 1f);
            grafica.DataSource.AddPointToCategoryRealtime("Disgusto", tiempo, Random.Range(0, 100), 1f);
        }
    }
}
