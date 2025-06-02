using System;
using System.Collections.Generic;

[Serializable]
public class ModoJuegoData
{
    public string nombreModo; // "Normal", "Lava", "Hitless"
    public List<EstadoNivel> niveles;

    public ModoJuegoData(string nombreModo, int cantidadNiveles)
    {
        this.nombreModo = nombreModo;
        niveles = new List<EstadoNivel>();

        for (int i = 0; i < cantidadNiveles; i++)
        {
            niveles.Add(new EstadoNivel());
        }
    }
}
