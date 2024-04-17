using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarDetector : MonoBehaviour
{
    [SerializeField, Tooltip("Segundos entre una deteccion y otra")]
    private float tiempoEntreDetecciones;
    [SerializeField, Tooltip("Distancia maxima del radar")]
    private float maxDistancia;
    [SerializeField,Tooltip("Mascara de los enemigos a mostrar")]
    private LayerMask mascaraEnemigos;
    [SerializeField, Tooltip("Mascara de los aliados a mostrar")]
    private LayerMask mascaraAliados;
    [SerializeField, Tooltip("Mascara de los aliados a mostrar")]
    private bool isRadarActive;
    private bool inUse;

    private void Awake()
    {
        //hitsEnemies = new List<RaycastHit>();
        isRadarActive = false;
        inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRadarActive&&!inUse) {
            StartCoroutine(DeteccionRadar());

        }
    }


    IEnumerator DeteccionRadar()
    {
        inUse = true;
        RaycastHit[] hits;
        while (isRadarActive)
        {
            hits=Physics.SphereCastAll(transform.position, maxDistancia,transform.forward,maxDistancia,mascaraEnemigos);
            if(hits.Length > 0)
            {
                MostrarObjetivosRaycast(hits, Color.red);
            }
            yield return new WaitForSeconds(tiempoEntreDetecciones);
            Debug.Log($"Ha esperado: {tiempoEntreDetecciones}");
        }
        inUse = false;  
    }

    private void MostrarObjetivosCollider(Collider[] colliders,Color colorMostrar)
    {
        if(colliders.Length > 0)
        {
            foreach(Collider col in colliders)
            {
                Debug.Log($"Name={col.name}, Posicion= {col.transform.position}");
            }
        }
    }

    private void MostrarObjetivosRaycast(RaycastHit[] hits, Color colorMostrar)
    {
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"Name={hit.collider.name}, Posicion= {hit.collider.transform.position}");
            }
        }
    }

   /* void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, maxDistancia);
    }*/


    private int GetMask(LayerMask mascarabusqueda)
    {
        return LayerMask.GetMask(LayerMask.LayerToName(mascarabusqueda));
    }
}
