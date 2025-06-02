using System;

[Serializable]
public class EstadoNivel
{
    public bool completado;
    public int monedasRecogidas; // de 0 a 50
    public float mejorTiempo; // en segundos

    // Calculado, no se guarda en disco
    public float PorcentajeCompletado => monedasRecogidas * 2f;
}
