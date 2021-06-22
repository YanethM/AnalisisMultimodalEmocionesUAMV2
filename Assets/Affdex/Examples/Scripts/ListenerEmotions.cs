using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using System.Text;
using System.Linq;
using System;

public class ListenerEmotions : ImageResultsListener
{
    public string[] datos;
    public Text textArea;
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

            // Adjust font size to fit the selected platform.
            if ((Application.platform == RuntimePlatform.IPhonePlayer) ||
                (Application.platform == RuntimePlatform.Android))
            {
                textArea.fontSize = 36;
            }
            else
            {
                textArea.fontSize = 20;
            }
            //textArea.text = faces[0].ToString();
            datos = faces[0].ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
            textArea.text = "Emociones" + "\n" + datos[19].Replace("Smile", "Sonrisa") + "\n" + datos[4].Replace("InnerBrowRaise", "Elevacion Interna de la ceja") + "\n" + datos[5].Replace("BrowRaise", "Elevacion de la ceja") + "\n" + datos[6].Replace("BrowFurrow", "Cejas arrugadas") + "\n" + datos[7].Replace("NoseWrinkle", "Arrugar la nariz") + "\n" + datos[8].Replace("UpperLipRaise", "Elevacion del labio superior") + "\n" + datos[9].Replace("LipCornerDepressor", "Depresion de las esquinas del labio") + "\n" + datos[10].Replace("ChinRaise", "Elevacion de la barbilla") + "\n" + datos[11].Replace("LipPucker", "Fruncir labios") + "\n" + datos[12].Replace("LipPress", "Presionar Labios") + "\n" + datos[13].Replace("LipSuck", "Chupar Labios") + "\n" + datos[14].Replace("MouthOpen", "Boca abierta") + "\n" + datos[15].Replace("Smirk", "Sonrisa distorsionada") + "\n" + datos[16].Replace("EyeClosure", "Ojos cerrados");
            textArea.CrossFadeColor(Color.white, 0.2f, true, false);
        }
        else
        {
            textArea.CrossFadeColor(new Color(1, 0.7f, 0.7f), 0.2f, true, false);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}