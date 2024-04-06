using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Enitity
{


    // Configurações de movimento
    [Header("Move config")]
    [SerializeField] private float moveSpeed; // Velocidade de movimento do jogador
    [SerializeField] private float jumpFoce; // Força do pulo do jogador


    // Informações de Dash
    [Header("Dash Info")]
    [SerializeField] private float dashSpeed; // Velocidade do dash
    [SerializeField] private float dashDuration; // Duração do dash
    private float dashTime; // Tempo atual do dash
    [SerializeField] private float dashCooldown; // Tempo de recarga do dash
    private float dashCooldownTimer; // Tempo atual de recarga do dash

    // Informações de Ataque
    [Header("Attack Info")]
    [SerializeField] private float comboTime = .3f; // Tempo máximo entre ataques para contabilizar como combo
    private float comboTimeWindow; // Tempo atual para registrar combo
    private bool isAttacking; // Indica se o jogador está atacando
    private int comboCounter; // Contador de combos

    private float xInput; // Entrada horizontal do jogador

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        Movement(); // Função para controlar o movimento do jogador
        CheckInput(); // Função para verificar as entradas do jogador

        // Atualiza o tempo do dash e o tempo de recarga do dash
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        // Atualiza o tempo para registrar combo
        comboTimeWindow -= Time.deltaTime;

        // Controla a orientação do jogador
        FlipController();

        // Controla as animações do jogador
        AnimatorControllers();
    }

    // Chamado quando um ataque termina
    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }



    // Verifica as entradas do jogador
    private void CheckInput()
    {
        xInput = (Input.GetAxisRaw("Horizontal"));

        // Inicia o evento de ataque ao pressionar o botão esquerdo do mouse
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        // Executa o pulo ao pressionar a barra de espaço
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Executa o dash ao pressionar a tecla Shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    // Inicia o evento de ataque
    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;

        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }
        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    // Executa a habilidade de dash
    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    // Controla o movimento do jogador
    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    // Executa o pulo do jogador
    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpFoce);
            
    }

    // Controla as animações do jogador
    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    // Controla a inversão da orientação do jogador
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
}