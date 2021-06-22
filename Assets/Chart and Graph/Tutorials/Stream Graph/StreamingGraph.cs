using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class Grafica : MonoBehaviour
{
    public GraphChart Graph;
    float lastTime = 0f;

    void Update()
    {
        float time = Time.time;
        if (lastTime + 1f < time)
        {
            lastTime = time;
            Graph.DataSource.AddPointToCategoryRealtime("Alegria", time, Random.Range(0,100), 1f);
            Graph.DataSource.AddPointToCategoryRealtime("Sorpresa", time, Random.Range(0, 100), 1f);
            Graph.DataSource.AddPointToCategoryRealtime("Tristeza", time, Random.Range(0, 100), 1f); 
            Graph.DataSource.AddPointToCategoryRealtime("Temor", time, Random.Range(0, 100), 1f); 
            Graph.DataSource.AddPointToCategoryRealtime("Enojo", time, Random.Range(0, 100), 1f); 
            Graph.DataSource.AddPointToCategoryRealtime("Desprecio", time, Random.Range(0, 100), 1f); 
            Graph.DataSource.AddPointToCategoryRealtime("Disgusto", time, Random.Range(0, 100), 1f);
        }
    }
}
