using Excel;
using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class EmotivInformacion : MonoBehaviour {

    public BDLocal bdLocal;

    public List<float> engagement = new List<float>();
    public List<float> excitement = new List<float>();
    public List<float> stress = new List<float>();
    public List<float> relaxation = new List<float>();
    public List<float> interest = new List<float>();
    public List<float> focus = new List<float>();
    public List<float> promedios = new List<float>();

    public void MostrarVentana(int indiceProtocolo)
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Excel", ".csv"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".csv");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(CargarVentana(indiceProtocolo));
    }

    IEnumerator CargarVentana(int indiceProtocolo)
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Success)
        {
            StreamReader archivo = new StreamReader(FileBrowser.Result);

            bool finArchivo = false;
            int filas = 0;
            float sumatoria = 0;

            while (!finArchivo)
            {
                string cadena = archivo.ReadLine();
                if (cadena == null)
                {
                    finArchivo = true;
                    break;
                }
                filas++;
                if (filas > 2)
                {
                    var valores = cadena.Split(',');
                    string dato1 = valores[39];
                    string dato2 = valores[44];
                    string dato3 = valores[50];
                    string dato4 = valores[55];
                    string dato5 = valores[60];
                    string dato6 = valores[65];

                    if (dato1 != "")
                        engagement.Add(float.Parse(dato1));
                    if (dato2 != "")
                        excitement.Add(float.Parse(dato2));
                    if (dato3 != "")
                        stress.Add(float.Parse(dato3));
                    if (dato4 != "")
                        relaxation.Add(float.Parse(dato4));
                    if (dato5 != "")
                        interest.Add(float.Parse(dato5));
                    if (dato6 != "")
                        focus.Add(float.Parse(dato6));
                }
            }

            for (int i = 0; i < engagement.Count; i++)
                sumatoria += engagement[i];
            promedios.Add(sumatoria / engagement.Count);

            sumatoria = 0;

            for (int i = 0; i < excitement.Count; i++)
                sumatoria += excitement[i];
            promedios.Add(sumatoria / excitement.Count);

            sumatoria = 0;

            for (int i = 0; i < stress.Count; i++)
                sumatoria += stress[i];
            promedios.Add(sumatoria / stress.Count);

            sumatoria = 0;

            for (int i = 0; i < relaxation.Count; i++)
                sumatoria += relaxation[i];
            promedios.Add(sumatoria / relaxation.Count);

            sumatoria = 0;

            for (int i = 0; i < interest.Count; i++)
                sumatoria += interest[i];
            promedios.Add(sumatoria / interest.Count);

            sumatoria = 0;

            for (int i = 0; i < focus.Count; i++)
                sumatoria += focus[i];
            promedios.Add(sumatoria / focus.Count);
            bdLocal.InsertarPromedioEmotiv(indiceProtocolo, Mathf.FloorToInt(promedios[0] * 100), Mathf.FloorToInt(promedios[1] * 100), Mathf.FloorToInt(promedios[2] * 100), Mathf.FloorToInt(promedios[3] * 100), Mathf.FloorToInt(promedios[4] * 100), Mathf.FloorToInt(promedios[5] * 100));
        }
    }

    private static DataRowCollection LeerExcel(string url, int _sheetIndex = 0)
    {
        FileStream stream = File.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        return result.Tables[_sheetIndex].Rows;
    }
}
