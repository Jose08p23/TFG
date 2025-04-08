using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullaje : MonoBehaviour
{
    public Transform[] patrolPoints; // Puntos de patrulla configurables desde el inspector
    public float speed = 2f; // Velocidad del enemigo

    private int currentPointIndex = 0; // Índice del punto actual
    private Transform targetPoint; // Punto de destino
    private Vector3 originalScale; // Tamaño original del enemigo

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            targetPoint = patrolPoints[currentPointIndex];
        }
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (patrolPoints.Length == 0) return; // Si no hay puntos de patrulla, no hacer nada

        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        
        // Girar el enemigo en la dirección del movimiento sin alterar su tamaño
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        
        // Si el enemigo ha llegado al punto de destino, cambiar al siguiente
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            targetPoint = patrolPoints[currentPointIndex];
        }
    }
}
