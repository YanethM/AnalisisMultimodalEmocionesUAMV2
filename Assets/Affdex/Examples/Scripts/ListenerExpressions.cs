using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using System.Text;
using System.Linq;
using System;

public class ListenerExpressions : ImageResultsListener
{
    public string[] datos;
    public Text textAreaExpresiones;
    public Text textAreaEmociones;
    string[] Emociones;
    public List<string> NomEmociones = new List<string>();
    //public List<float> NumEmociones = new List<float>();
    float[,] NumEmociones = new float[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0 } , { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
    float aux = 0;
    public float[] Valores = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] Conteo = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Text cara;
    public override void onFaceFound(float timestamp, int faceId)
    {
        Debug.Log("Found the face");
        cara.text = "Cara encontrada";
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        Debug.Log("Lost the face");
        cara.text = "Cara perdida";
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

            // Adjust font size to fit the selected platform.
            if ((Application.platform == RuntimePlatform.IPhonePlayer) ||
                (Application.platform == RuntimePlatform.Android))
            {
                textAreaExpresiones.fontSize = 36;
                textAreaEmociones.fontSize = 36;
            }
            else
            {
                textAreaExpresiones.fontSize = 20;
                textAreaEmociones.fontSize = 36;
            }
            //textArea.text = faces[0].ToString();
            datos = faces[0].ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
            textAreaExpresiones.text = "Expresiones" + "\n" + datos[3].Replace("Smile","Sonrisa") + "\n" + datos[4].Replace("InnerBrowRaise","Elevacion Interna de la ceja") + "\n" + datos[5].Replace("BrowRaise", "Elevacion de la ceja") + "\n" + datos[6].Replace("BrowFurrow", "Cejas arrugadas") + "\n" + datos[7].Replace("NoseWrinkle", "Arrugar la nariz") + "\n" + datos[8].Replace("UpperLipRaise", "Elevacion del labio superior") + "\n" + datos[9].Replace("LipCornerDepressor", "Depresion de las esquinas del labio") + "\n" + datos[10].Replace("ChinRaise", "Elevacion de la barbilla") + "\n" + datos[11].Replace("LipPuker", "Fruncir labios") + "\n" + datos[12].Replace("LipPress", "Presionar Labios") + "\n" + datos[13].Replace("LipSuck", "Chupar Labios") + "\n" + datos[14].Replace("MouthOpen", "Boca abierta") + "\n" + datos[15].Replace("Smirk", "Sonrisa distorsionada") + "\n" + datos[16].Replace("EyeClosure", "Ojos cerrados");
            textAreaExpresiones.CrossFadeColor(Color.white, 0.2f, true, false);
            for (int i=0;i<9;i++)
            {
                Emociones = datos[19+i].Split(new string[] { ":" }, StringSplitOptions.None);
                //NomEmociones.Add(Emociones[0]);Esto ya lo se no requiero calcularlo en cada iteración.
                if (float.Parse(Emociones[1]) > 0.5)//& float.Parse(Emociones[7]) > 0
                {
                    aux = (float.Parse(Emociones[1]));
                    NumEmociones[0, i] = (NumEmociones[0, i] + aux);
                    NumEmociones[1, i]++;
                    Valores[i] = NumEmociones[0, i];
                    Conteo[i] = NumEmociones[1, i];
                }
            }
            //valor.barChar(Valores);
            
            textAreaEmociones.text = "Emociones" + "\n" + datos[19].Replace("Joy", "Felicidad") + "\n" + datos[20].Replace("Fear", "Temor") + "\n" + datos[21].Replace("Disgust", "Disgusto") + "\n" + datos[22].Replace("Sadness", "Tristesa") + "\n" + datos[23].Replace("Anger", "Enojo") + "\n" + datos[24].Replace("Surprice", "Sorpresa") + "\n" + datos[25].Replace("Contempt", "Desprecio") + "\n" + datos[26].Replace("Valence", "Valencia") + "\n" + datos[27].Replace("engagement", "Compromiso") + "\n" + "Acumulado" + NumEmociones[0,0] + "\n" + "Repeticiones" + NumEmociones[1, 0] + "\n" + "Acumulado" + NumEmociones[0, 6] + "\n" + "Repeticiones" + NumEmociones[1, 6];
        }
        else
        {
            textAreaExpresiones.CrossFadeColor(new Color(1, 0.7f, 0.7f), 0.2f, true, false);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}