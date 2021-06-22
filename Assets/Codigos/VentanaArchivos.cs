using UnityEngine;
using System.Collections;
using SimpleFileBrowser;

public class VentanaArchivos : MonoBehaviour
{
    void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(MostrarVentana());
    }

    IEnumerator MostrarVentana()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
        }
    }
}