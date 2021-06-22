using UnityEngine;
using Moments;

[RequireComponent(typeof(Recorder)), AddComponentMenu("")]
public class Record : MonoBehaviour
{    
	public float progreso { get; set; }
    public bool guardando { get; set; }

    private Recorder grabar;
    private string archivo = "";    

    void Start()
	{
        grabar = GetComponent<Recorder>();        
        grabar.OnPreProcessingDone = OnProcessingDone;
        grabar.OnFileSaveProgress = OnFileSaveProgress;
        grabar.OnFileSaved = OnFileSaved;
	}

	void OnProcessingDone()
	{
        guardando = true;
	}

	void OnFileSaveProgress(int id, float percent)
	{
        progreso = percent * 100f;
	}

	void OnFileSaved(int id, string filepath)
	{
        archivo = filepath;
        guardando = false;
        grabar.Record();
	}

	void OnDestroy()
	{
	}

    public void GuardarGif(string carpeta, string nombre)
    {
        grabar.SaveFolder = Application.persistentDataPath + "/Resultados CSV/";
        grabar.Save(nombre);
        progreso = 0f;
    }

    public void GrabarGIF()
    {
        grabar.Record();
    }
}
