using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Movimentação E Animação
public class PlayerMover : MonoBehaviour
{
    public Transform myCamera;
    private Vector3 movement;

    private Animator animatorController;
    private NavMeshAgent agent;

    public bool caminhar; //Andar bem mais lento

    [Header("Velocidade do Player")]
    public float velocidadeParaCaminhar;
    public float velocidadeParaAndar;
    public float velocidadeParaCorrer;

    private bool correndo;
    public float velocidadeParaVirarOPlayer;

    private float velocidade;

    private void Start()
    {
        animatorController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Movimentacao();
        RotatePlayer();
        PlayerAnimator();
    }

    private void PlayerAnimator()
    {
        if (movement != Vector3.zero) animatorController.SetBool("Walk", true);
        else animatorController.SetBool("Walk", false);
    }

    private void Movimentacao()
    {
        movement = myCamera.forward * Input.GetAxisRaw("Vertical");
        movement += myCamera.right * Input.GetAxisRaw("Horizontal");

        movement.Normalize(); //Normalizar

        if (caminhar) velocidade = velocidadeParaCaminhar;
        if (correndo) velocidade = velocidadeParaCorrer;

        if (!caminhar && !correndo) velocidade = velocidadeParaAndar;

        movement *= velocidade; // 

        movement.y = 0; // Para evitar bugs

        Vector3 movementVelocity = movement;
        agent.velocity = movementVelocity; //Para fazer o player ANDAR

        if (Input.GetKeyDown(KeyCode.CapsLock)) caminhar = !caminhar; //Caminhar estilo Mafia 2

        //correndo = Input.GetKey(KeyCode.LeftShift); //Deixar só "Left shift"
        //correndo = Input.GetKey(KeyCode.RightShift); //Deixar só "Right shift"

        correndo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); //Deixar os dois "Left shift" e "Right shift"

        animatorController.SetBool("Run", correndo);
    }

    private void RotatePlayer()
    {
        Vector3 targerDirection = Vector3.zero;

        targerDirection = myCamera.forward * Input.GetAxisRaw("Vertical");
        targerDirection += myCamera.right * Input.GetAxisRaw("Horizontal");

        targerDirection.Normalize();
        targerDirection.y = 0; // Para evitar bugs

        if (targerDirection == Vector3.zero) targerDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targerDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, velocidadeParaVirarOPlayer * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}