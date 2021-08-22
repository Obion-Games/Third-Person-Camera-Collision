using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float distanciaMin = 1.0f; //Distacia min entre o player
    public float distanciaMax = 4.0f; //Distancia max entre o player

    public float smooth = 10.0f;
    private Vector3 dollyDir;

    public float distanciaDaCamera; //distancia da camera para o player

    private void Start()
    {
        dollyDir = transform.localPosition.normalized;
        distanciaDaCamera = transform.localPosition.magnitude;
    }

    private void Update()
    {
        //Para o colisor da camera // Para detectar se ta colidindo 
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * distanciaMax);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        distanciaDaCamera = Mathf.Clamp((hit.distance * 0.87f), distanciaMin, distanciaMax);

        else distanciaDaCamera = distanciaMax;

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distanciaDaCamera, Time.deltaTime * smooth);
    }
}