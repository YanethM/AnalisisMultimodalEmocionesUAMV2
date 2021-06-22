using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ControladorGrafica : MonoBehaviour {
    public Image Felicidad;
    public Image Temor;
    public Image Disgusto;
    public Image Tristeza;
    public Image Enojo;
    public Image Sorpresa;
    public Image Desprecio;
    public float[] valor = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] conteo = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float vaux;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        barChar();
    }
    public void barChar()
    {
        valor = GameObject.Find("Expresiones").GetComponent<ListenerExpressions>().Valores;
        conteo = GameObject.Find("Expresiones").GetComponent<ListenerExpressions>().Conteo;
        conteo.Max();//valor para normalizar todas las expresiones
        //valor = GetComponent<ListenerExpressions>().Valores;
        //conteo = GetComponent<ListenerExpressions>().Conteo;

        Felicidad.rectTransform.sizeDelta = new Vector2(100, valor[0] / (valor.Max()+1f) * 300f);
        Felicidad.transform.localPosition = new Vector3(Felicidad.transform.localPosition.x, valor[0] / (valor.Max()+1) * 300f / 2f - 300f, Felicidad.transform.localPosition.z);
        Temor.rectTransform.sizeDelta = new Vector2(100, valor[1] / (valor.Max() + 1f) * 300f);
        Temor.transform.localPosition = new Vector3(Temor.transform.localPosition.x, valor[1] / (valor.Max()-1) * 300f / 2f - 300f, Temor.transform.localPosition.z);
        Disgusto.rectTransform.sizeDelta = new Vector2(100, valor[2] / (valor.Max() + 1f) * 300f);
        Disgusto.transform.localPosition = new Vector3(Disgusto.transform.localPosition.x, valor[2] / (valor.Max()-1) * 300f / 2f - 300f, Disgusto.transform.localPosition.z);
        Tristeza.rectTransform.sizeDelta = new Vector2(100, valor[3] / (valor.Max() + 1f) * 300f);
        Tristeza.transform.localPosition = new Vector3(Tristeza.transform.localPosition.x, valor[3] / (valor.Max()-1) * 300f / 2f - 300f, Tristeza.transform.localPosition.z);
        Enojo.rectTransform.sizeDelta = new Vector2(100, valor[4] / (valor.Max() + 1f) * 300f);
        Enojo.transform.localPosition = new Vector3(Enojo.transform.localPosition.x, valor[4] / (valor.Max()-1) * 300f / 2f - 300f, Enojo.transform.localPosition.z);
        Sorpresa.rectTransform.sizeDelta = new Vector2(100, valor[5] / (valor.Max() + 1f) * 300f);
        Sorpresa.transform.localPosition = new Vector3(Sorpresa.transform.localPosition.x, valor[5] / (valor.Max()-1) * 300f / 2f - 300f, Sorpresa.transform.localPosition.z);
        Desprecio.rectTransform.sizeDelta = new Vector2(100, valor[6] / (valor.Max() + 1f) * 300f);
        Desprecio.transform.localPosition = new Vector3(Desprecio.transform.localPosition.x, valor[6] / (valor.Max()-1) * 300f / 2f - 300f, Desprecio.transform.localPosition.z);
    }
}
/*valor = Random.Range(0, 200);
        Felicidad.rectTransform.sizeDelta = new Vector2(100, valor);
Felicidad.transform.localPosition = new Vector3(Felicidad.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Temor.rectTransform.sizeDelta = new Vector2(100, valor);
Temor.transform.localPosition = new Vector3(Temor.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Disgusto.rectTransform.sizeDelta = new Vector2(100, valor);
Disgusto.transform.localPosition = new Vector3(Disgusto.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Tristeza.rectTransform.sizeDelta = new Vector2(100, valor);
Tristeza.transform.localPosition = new Vector3(Tristeza.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Enojo.rectTransform.sizeDelta = new Vector2(100, valor);
Enojo.transform.localPosition = new Vector3(Enojo.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Sorpresa.rectTransform.sizeDelta = new Vector2(100, valor);
Sorpresa.transform.localPosition = new Vector3(Sorpresa.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
valor = Random.Range(0, 200);
        Desprecio.rectTransform.sizeDelta = new Vector2(100, valor);
Desprecio.transform.localPosition = new Vector3(Desprecio.transform.localPosition.x, valor / 2f, Felicidad.transform.localPosition.z);
*/