using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;            // Velocidad de movimiento horizontal
    public float fuerzaSalto = 10f;         // Fuerza aplicada en el salto
    public LayerMask capaSuelo;             // Capa que representa el suelo (para detectar aterrizajes)
    public float tiempoBloqueoMovimiento = 3f;
    public float fuerzaGolpe;
    public AudioClip SonidoSalto;
    public AudioClip SonidoGolpe;
    public float coolDownParpadeo;

    public bool puedeRecibirDaño = true;    // Indica si el jugador puede recibir daño

    private Rigidbody2D rb;                 // Componente de física del jugador
    private BoxCollider2D boxCollider;      // Para detección de colisiones con el suelo
    private Animator animator;              // Controla las animaciones
    private bool mirandoDerecha = true;     // Controla la orientación del personaje
    private bool puedeHacerDobleSalto = false; // Permite doble salto (si está activado)
    private bool movimientoHabilitado = false; // Indica si el jugador puede moverse

    private float tiempoBufferSalto = 0.05f;  // Tolerancia para detectar el salto
    private float tiempoUltimaTeclaSalto = 0f;  // Último instante en que se pulsó salto

    // Referencia a plataforma en movimiento (si aplica)
    private MovingPlatform currentPlatform;
    // Indica si el jugador está “parado” sobre la plataforma
    private bool standingOnPlatform = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
        boxCollider = GetComponent<BoxCollider2D>(); 
        animator = GetComponent<Animator>();       
        StartCoroutine(HabilitarMovimientoDespuesDeTiempo());
    }

    IEnumerator HabilitarMovimientoDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoBloqueoMovimiento);
        movimientoHabilitado = true;
    }

    void FixedUpdate()
    {
        ProcesarMovimiento();
    }

    void Update()
    {
        if (movimientoHabilitado)
        {
            // Captura la entrada para el salto (buffer)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tiempoUltimaTeclaSalto = tiempoBufferSalto;
            }
            else
            {
                tiempoUltimaTeclaSalto -= Time.deltaTime;
            }
            
            ProcesarSalto();
        }
        
        ActualizarAnimaciones();
    }

    void ProcesarMovimiento()
    {
        if (!movimientoHabilitado)
            return;

        float inputMovimiento = Input.GetAxis("Horizontal");
        animator.SetBool("isRunning", Mathf.Abs(inputMovimiento) > 0.01f);

        float horizontalInputVel = inputMovimiento * velocidad;

        Vector2 platformVel = Vector2.zero;
        if (currentPlatform != null)
        {
            platformVel = (Vector2)(currentPlatform.DeltaMovimiento / Time.fixedDeltaTime);
        }

        if (currentPlatform != null && standingOnPlatform)
        {
            rb.velocity = new Vector2(horizontalInputVel + platformVel.x, platformVel.y);
        }
        else if (currentPlatform != null)
        {
            rb.velocity = new Vector2(horizontalInputVel + platformVel.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontalInputVel, rb.velocity.y);
        }

        GestionarOrientacion(inputMovimiento);
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool EstaEnSuelo()
    {
        float extraHeight = 0.05f;
        Vector2 origin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        Vector2 size = new Vector2(boxCollider.bounds.size.x * 0.8f, 0.02f);
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, extraHeight, capaSuelo);
        return raycastHit.collider != null && raycastHit.normal.y > 0.7f;
    }

    void ProcesarSalto()
    {
        if (tiempoUltimaTeclaSalto > 0)
        {
            // Permitir salto si estamos en suelo o sobre una plataforma
            if (EstaEnSuelo() || standingOnPlatform)
            {
                Saltar();
                AudioManager.Instance.ReproducirSonido(SonidoSalto);
                puedeHacerDobleSalto = true;
            }
            else if (puedeHacerDobleSalto)
            {
                Saltar();
                AudioManager.Instance.ReproducirSonido(SonidoSalto);
                puedeHacerDobleSalto = false;
            }
        }
    }

    void Saltar()
    {
        tiempoUltimaTeclaSalto = 0;
        standingOnPlatform = false;
        currentPlatform = null;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
    }

    void ActualizarAnimaciones()
    {
        if (standingOnPlatform)
        {
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            return;
        }

        bool grounded = EstaEnSuelo();
        animator.SetBool("isGrounded", grounded);

        if (grounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            puedeHacerDobleSalto = true;
            return;
        }

        if (rb.velocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (rb.velocity.y < -0.1f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
    }

    // Aquí se reactivará la vulnerabilidad SOLO si se ha aterrizado "completamente"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
            // Solo se reactiva si la velocidad vertical es casi nula (evita reactivación por golpes)
            if (Mathf.Abs(rb.velocity.y) < 0.1f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
                puedeRecibirDaño = true;
            }
        }
        
        MovingPlatform plataforma = collision.gameObject.GetComponent<MovingPlatform>();
        if (plataforma != null)
        {
            currentPlatform = plataforma;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        MovingPlatform plataforma = collision.gameObject.GetComponent<MovingPlatform>();
        if (plataforma != null)
        {
            bool onTop = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    onTop = true;
                    break;
                }
            }
            standingOnPlatform = onTop;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            currentPlatform = null;
            standingOnPlatform = false;
        }
    }

    // El método AplicarGolpe aplica siempre el knockback, sin condicionar el daño
    public void AplicarGolpe()
    {
        movimientoHabilitado = false;
        AudioManager.Instance.ReproducirSonido(SonidoGolpe);
        Vector2 direccionGolpe = rb.velocity.x > 0 ? new Vector2(-1, 1) : new Vector2(1, 1);
        rb.AddForce(direccionGolpe * fuerzaGolpe);
        StartCoroutine(EsperarYActivarMovimiento());
    }

    IEnumerator EsperarYActivarMovimiento()
    {
        yield return new WaitForSeconds(0.1f);
        while (!EstaEnSuelo())
        {
            yield return null;
        }
        movimientoHabilitado = true;
    }

    public IEnumerator Parpadear()
    {
        float tiempoTotal = 0f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float tiempoParpadeo = 0.1f;

        while (tiempoTotal < coolDownParpadeo)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(tiempoParpadeo);
            tiempoTotal += tiempoParpadeo;
        }

        sr.enabled = true;
    }
}
