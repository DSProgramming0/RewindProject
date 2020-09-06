using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private TimeManager timeManager;
    private RaycastHit _hit;

    #region Fields
    public bool drawCrosshair = true;
    public Color crosshairColor = Color.white;
    public float width = 1;
    public float height = 3;

    [System.Serializable]
    public class spreading
    {
        public float sSpread = 20;
        public float maxSpread = 60;
        public float minSpread = 20;
        public float spreadPerSecond = 30;
        public float decreasePerSecond = 25;
    }

    public spreading spread = new spreading();

    Texture2D tex;
    float newHeight;
    GUIStyle lineStyle;

    #endregion

    #region Functions
    void Awake()
    {
        tex = new Texture2D(1, 1);
        lineStyle = new GUIStyle();
        lineStyle.normal.background = tex;
    }

    void OnGUI()
    {
        Vector2 centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        float screenRatio = Screen.height / 100;

        newHeight = height * screenRatio;

        if (drawCrosshair)
        {
            GUI.Box(new Rect(centerPoint.x - (width / 2), centerPoint.y - (newHeight + spread.sSpread), width, newHeight), GUIContent.none, lineStyle);
            GUI.Box(new Rect(centerPoint.x - (width / 2), (centerPoint.y + spread.sSpread), width, newHeight), GUIContent.none, lineStyle);
            GUI.Box(new Rect((centerPoint.x + spread.sSpread), (centerPoint.y - (width / 2)), newHeight, width), GUIContent.none, lineStyle);
            GUI.Box(new Rect(centerPoint.x - (newHeight + spread.sSpread), (centerPoint.y - (width / 2)), newHeight, width), GUIContent.none, lineStyle);
        }       

        spread.sSpread -= spread.decreasePerSecond * Time.deltaTime;
        spread.sSpread = Mathf.Clamp(spread.sSpread, spread.minSpread, spread.maxSpread);
    }
    
    void SetColor(Texture2D myTexture, Color myColor)
    {
        for (int y = 0; y < myTexture.height; y++)
        {
            for (int x = 0; x < myTexture.width; x++)
                myTexture.SetPixel(x, y, myColor);
            myTexture.Apply();
        }
    }
    #endregion

    void Shoot()
    {      
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {           
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            Instantiate(explosion, hit.point, Quaternion.LookRotation(_hit.normal));

            //timeManager.DoSlowMotion();
        }

        spread.sSpread += spread.spreadPerSecond * Time.deltaTime;
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.E) && timeManager.canUseTimeSkills)
        {
            timeManager.DoSlowMotion();
        }
    }  
}
