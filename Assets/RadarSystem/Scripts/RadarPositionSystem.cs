using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class RadarPositionSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Segundos entre una deteccion y otra")]
    private float timeBetweenRadarActivate=1.5f;
    [SerializeField, Tooltip("Maxima distancia entre los gameobjects frente al jugador")]
    private float Distance=300;// maximum distance of game objects
    [SerializeField, Tooltip("Lista de texturas 2d de los iconos de los personajes")]
    private Sprite[] Navtexture;// texturas de los iconos de personajes
    [SerializeField, Tooltip("Mascara de los aliados a mostrar")]
    private LayerMask mascaraEnemigos;
    [SerializeField, Tooltip("Mascara de los aliados a mostrar")]
    private LayerMask mascaraAliados;
    /* [SerializeField, Tooltip("Lista de tags de los enemigos para detectar")]
     private string[] EnemyTag;// lista de tags*/
    [SerializeField, Tooltip("Icono para marcar al usuario del radar")]
    private Sprite NavCompass;// compass del usuario del radar texture
    [SerializeField, Tooltip("Textura del fondo del radar")]
    private Sprite CompassBackground;// background texture
    private bool inUse;//Determina si debe activarse y mostrarse el radar o no
    [SerializeField]
    private bool activate;//Determina si debe activarse y mostrarse el radar o no
    [SerializeField, Tooltip("Color para coloear radar")]
    private Color ColorMult;
    [SerializeField, Tooltip("Imagen del canvar del radar")]
    private Image imagenRadar;
    private List<GameObject> iconos;
    [SerializeField]
    private GameObject prefabIcon;
    private void Awake()
    {
        inUse = false;
        iconos=new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        imagenRadar.color = ColorMult;
        imagenRadar.sprite = CompassBackground;
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {

            RadarOn();
        }
        else
        {
            RadarOff();
        }
    }
  

    void DrawNav2(GameObject[] enemylists, Sprite navtexture)
    {      
        float distancePlayerEnemy = 0;
        GameObject gameobjectIcon;

            for (int i = 0; i < enemylists.Length; i++)
            {
                distancePlayerEnemy = Vector3.Distance(transform.position, enemylists[i].transform.position);
                if (distancePlayerEnemy <= Distance)
                {
                    gameobjectIcon = prefabIcon;
                    gameobjectIcon = Instantiate(prefabIcon, imagenRadar.transform);
                    iconos.Add(gameobjectIcon);
                    gameobjectIcon.transform.Translate(new Vector3((transform.position.x - enemylists[i].transform.position.x)*(100/Distance), (transform.position.z - enemylists[i].transform.position.z)*(Distance / 100), 0));
                    gameobjectIcon.GetComponent<Image>().sprite = navtexture;
                    gameobjectIcon.tag = "Icon";
                    gameobjectIcon.name = $"IconoEnemy_{i}";
                }
            }       
    }


private GameObject[] SearchByRaycast(LayerMask mask)
{
        List<GameObject> enemies = new List<GameObject>();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,Distance, transform.forward, Distance, mask);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                enemies.Add(hit.collider.gameObject);
            }
        }
        

    return enemies.ToArray();
}

/*private GameObject[] SearchEnemiesByTags()
    {
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < EnemyTag.Length; i++)
        {
            enemies.AddRange(GameObject.FindGameObjectsWithTag(EnemyTag[i]));
        }
        return enemies.ToArray();
    }*/

    void RadarOn()
    {
        if (!inUse)
        {
            imagenRadar.gameObject.SetActive(true);
            StartCoroutine(SetObjectsInRadar());
        }
    }


    void RadarOff()
    {
        imagenRadar.gameObject.SetActive(false);
        StopCoroutine(SetObjectsInRadar());
    }

    IEnumerator SetObjectsInRadar()
    {
        inUse = true;
        WaitForSeconds wait = new WaitForSeconds(timeBetweenRadarActivate);
        while (inUse)
        {
                ClearIconsRadar();
                DrawNav2(SearchByRaycast(mascaraEnemigos), Navtexture[0]);
                DrawNav2(SearchByRaycast(mascaraAliados), NavCompass);
            yield return wait;
            Debug.Log($"Ha esperado {timeBetweenRadarActivate} segundos, onda radio");
            inUse = false;
        }

    }

    private void ClearIconsRadar()
    {
        if (iconos.Count > 0)
        {
            foreach (GameObject go in iconos)
            {
                Destroy(go);
            }
            iconos.Clear();
        }
    }

    private int GetMask(LayerMask mascarabusqueda)
    {
        return LayerMask.GetMask(LayerMask.LayerToName(mascarabusqueda));
    }

}
