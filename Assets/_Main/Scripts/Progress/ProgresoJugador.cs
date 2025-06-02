using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ProgresoJugador
{
    public ModoJuegoData normal;
    public ModoJuegoData lava;
    public ModoJuegoData hitless;

    private static string rutaArchivo => Path.Combine(Application.persistentDataPath, "progreso.json");

    public ProgresoJugador()
    {
        normal = new ModoJuegoData("Normal", 10);
        lava = new ModoJuegoData("Lava", 10);
        hitless = new ModoJuegoData("Hitless", 10);
    }

    // Guardar el progreso actual a disco
    public void Guardar()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Progreso guardado en: " + rutaArchivo);
        Debug.Log("Archivo de progreso en: " + Application.persistentDataPath);
        Debug.Log("Contenido del archivo: " + json);
    }

    // Cargar el progreso desde disco (si existe)
    public static ProgresoJugador Cargar()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            ProgresoJugador progreso = JsonUtility.FromJson<ProgresoJugador>(json);
            Debug.Log("Progreso cargado desde: " + rutaArchivo);
            return progreso;
        }
        else
        {
            Debug.Log("No se encontró progreso guardado. Creando uno nuevo.");
            return new ProgresoJugador();
        }
    }

    // Opcional: eliminar progreso
    public static void Borrar()
    {
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
            Debug.Log("Progreso eliminado.");
        }
    }
}
