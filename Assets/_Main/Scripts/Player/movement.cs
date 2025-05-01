using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    public LayerMask capaSuelo;
    public float tiempoBloqueoMovimiento = 3f;
    public float fuerzaGolpe;
    public AudioClip SonidoSalto;
    public AudioClip SonidoGolpe;
    public float coolDownParpadeo;
    public float suavizadoAceleracion = 10f;

    public bool puedeRecibirDaño = true;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private bool mirandoDerecha = true;
    private bool puedeHacerDobleSalto = false;
    private bool movimientoHabilitado = false;

    private float tiempoBufferSalto = 0.05f;
    private float tiempoUltimaTeclaSalto = 0f;

    private MovingPlatform currentPlatform;
    private bool standingOnPlatform = false;

    private float velocidadActual = 0f;

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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
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

        float inputTeclado = Input.GetAxis("Horizontal");
        float inputDpad = Input.GetAxis("DPadX");

        // Elegir el input más fuerte (DPad tiene prioridad si activo)
        float inputMovimiento = Mathf.Abs(inputDpad) > 0.1f ? inputDpad : inputTeclado;

        animator.SetBool("isRunning", Mathf.Abs(inputMovimiento) > 0.01f);

        // Detectar si estamos usando joystick (valor parcial)
        bool usandoJoystick = Mathf.Abs(inputMovimiento) < 0.99f;
        float factorSuavizado = usandoJoystick ? suavizadoAceleracion : suavizadoAceleracion * 2f;

        velocidadActual = Mathf.Lerp(velocidadActual, inputMovimiento * velocidad, factorSuavizado * Time.fixedDeltaTime);

        Vector2 platformVel = Vector2.zero;
        if (currentPlatform != null)
        {
            platformVel = (Vector2)(currentPlatform.DeltaMovimiento / Time.fixedDeltaTime);
        }

        if (currentPlatform != null && standingOnPlatform)
        {
            rb.velocity = new Vector2(velocidadActual + platformVel.x, platformVel.y);
        }
        else if (currentPlatform != null)
        {
            rb.velocity = new Vector2(velocidadActual + platformVel.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(velocidadActual, rb.velocity.y);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
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
