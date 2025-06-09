using System;

[Serializable]
public class EstadoNivel
{
    public bool completado;
    public int monedasRecogidas; // de 0 a 50
    public float mejorTiempo; // en segundos

    public string nombreNivel = ""; // ✅ Nuevo campo persistente
    public float porcentajeGuardado = 0f; // ✅ Se actualiza al guardar

    // Propiedad calculada opcional (no serializable)
    public float PorcentajeCompletado => monedasRecogidas * 2f;
}
