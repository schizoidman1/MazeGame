using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Necessário para usar NavMesh

public class Enemy : MonoBehaviour
{
    public Transform respawnPoint; // Ponto de respawn do jogador
    public float detectionRadius = 10f; // Raio de detecção do inimigo
    public float chaseSpeed = 3.5f; // Velocidade de perseguição

    private Transform player; // Referência ao transform do jogador
    private NavMeshAgent navMeshAgent; // Agente do NavMesh
    private Animator animator; // Controlador de animação
    private bool isChasing = false; // Verifica se o inimigo está perseguindo

    void Start()
    {
        // Procurar pelo jogador na cena
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Configurar NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = chaseSpeed;
        }

        // Configurar Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator não encontrado no inimigo.");
        }
    }

    void Update()
    {
        if (player == null) return; // Se o jogador não for encontrado, sair

        // Calcular a distância entre o inimigo e o jogador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Jogador está no raio de visão, começar a perseguição
            isChasing = true;
        }
        else
        {
            // Jogador saiu do raio de visão
            isChasing = false;
        }

        // Atualizar o comportamento do inimigo
        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            if (isChasing)
            {
                navMeshAgent.SetDestination(player.position);

                // Ativar animação de movimento
                if (animator != null)
                {
                    animator.SetBool("Andando", true);
                }

                // Rotacionar o inimigo na direção do movimento
                Vector3 velocity = navMeshAgent.velocity;
                if (velocity != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * 10);
                }
            }
            else
            {
                navMeshAgent.ResetPath();

                // Parar animação de movimento
                if (animator != null)
                {
                    animator.SetBool("Andando", false);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Enviar o jogador de volta ao ponto de respawn
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;
                other.transform.position = respawnPoint.position;
                controller.enabled = true;
            }
            else
            {
                other.transform.position = respawnPoint.position;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar o raio de detecção no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
