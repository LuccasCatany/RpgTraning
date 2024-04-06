using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enitity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;

    protected int facingDir = 1; // Direção que o jogador está encarando (1 para direita, -1 para esquerda)
    protected bool facingRight = true; // Indica se o jogador está encarando para a direita

    // Configurações de colisão
    [Header("Collision config")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance; // Distância para verificar se o jogador está no chão
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround; // Máscara de camada para identificar o que é considerado chão


    protected bool isGrounded; // Indica se o jogador está no chão
    protected bool isWallDetected;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();  

        if(wallCheck == null)
            wallCheck = transform;  
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionCheck(); // Função para verificar colisões// Função para verificar colisões
    }


        // Verifica se o jogador está no chão
    protected virtual void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGround);
    }

    // Inverte a orientação do jogador
    protected virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // Desenha uma linha para representar o ponto de verificação de chão no editor
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }

}
