using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float velocidadMovimiento;

    private int siguientePlataforma = 1;
    private bool ordenPlataformas = true;
    
    // Delta de movimiento calculado en cada FixedUpdate
    public Vector3 DeltaMovimiento { get; private set; }

    void Start()
    {
        if (puntosMovimiento.Length < 2)
        {
            Debug.LogError("Se requieren al menos 2 puntos de movimiento.");
        }
    }

    void FixedUpdate()
    {
        Vector3 posAntes = transform.position;

        if (ordenPlataformas && siguientePlataforma + 1 >= puntosMovimiento.Length)
            ordenPlataformas = false;

        if (!ordenPlataformas && siguientePlataforma <= 0)
            ordenPlataformas = true;

        if (Vector2.Distance(transform.position, puntosMovimiento[siguientePlataforma].position) < 0.1f)
        {
            if (ordenPlataformas)
                siguientePlataforma += 1;
            else
                siguientePlataforma -= 1;
        }

        transform.position = Vector2.MoveTowards(transform.position, 
            puntosMovimiento[siguientePlataforma].position, velocidadMovimiento * Time.fixedDeltaTime);

        DeltaMovimiento = transform.position - posAntes;
    }
}
