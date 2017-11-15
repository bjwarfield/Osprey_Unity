using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player3DControl : Entity
{

    //movement vars
    public float speed = 1.0f;
    public float horizontalRange = 4.8f;
    public float verticalRange = 7.5f;
    public float rotationSpeed = 1.0f;
    public float tiltRange = 40.0f;
    public float tilt;

    //weapon vars
    public GameObject[] shot;
    public GameObject[] gunBarrel;
    public float shotFireRate = 25.0f;
    private float lastShot;

    //status vars
    public bool godMode = false;
    private bool tempShield = false;
    //private polarType polarity = polarType.LIGHT;
    //private bool polarize = false;
    public float shieldTimer = 2.0f;
    public float respawnTime = 2.0f;
    public float maxEnergy = 100.0f;
    public float energy;

    //misc
    //public GameObject gibs = null;

    //utility vars
    //private Transform thisTransform = null;
    //[SerializeField]
    //private Renderer shipRenderer = null;
    //[SerializeField]
    //private Material[] materials;
    [SerializeField]
    private GameObject[] shields;
    [SerializeField]
    private Texture[] textures;
    [SerializeField]
    private Text[] UITexts;
    private string col_none = "Shields: Neutral";
    private string col_breech = "Shields: Breeched!!";
    private string col_charge = "Shields: Charging";
    private string energyPrefix = "Energy: ";
    // Use this for initialization
    void Start () {
        thisTransform = GetComponent<Transform>();
        lastShot = 0.0f;
        polarity = polarType.LIGHT;
        thisRenderer.material.mainTexture = textures[(int)polarType.LIGHT];
        thisRenderer.enabled = true;
        thisRenderer.sortingLayerName = "Player";
        shields[(int)polarType.LIGHT].SetActive(true);
        shields[(int)polarType.DARK].SetActive(false);
        tilt = 0.0f;

        UITexts[0].text = col_none;
        UITexts[0].color = new Color(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {

        //get user inputs for movement vectors
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        thisTransform.Translate(speed * Time.deltaTime * movement.normalized, Space.World);

        //calculate tilt
        //bind tilt to range [-180,540] 
        tilt += 180.0f;
        tilt = tilt % 720.0f;
        if (tilt < 0) { tilt += 720.0f; }
        tilt -= 180.0f;
        //Debug.Log(tilt);

        //calculate target tilt in regards to input and polarity
        float targetTilt = -(Input.GetAxis("Horizontal") * tiltRange) + ((int)polarity * 360);
        //Debug.Log(targetTilt);

        //tilt = Mathf.Lerp(tilt, targetTilt, rotationSpeed * Time.deltaTime);
        //tilt = Mathf.MoveTowards(tilt, targetTilt, rotationSpeed * Time.deltaTime);
        tilt = Mathf.SmoothStep(tilt, targetTilt, 0.5f);

        //set mesh material based on rotation value. Light: [-180, 180], Dark (180,540]\
 
        if (tilt > 180 && thisRenderer.material.mainTexture == textures[(int)polarType.LIGHT])
        {
            thisRenderer.material.mainTexture = textures[(int)polarType.DARK];
            shields[(int)polarType.LIGHT].SetActive(false);
            shields[(int)polarType.DARK].SetActive(true);
            //polarize = false;
        }else if(tilt < 180 && thisRenderer.material.mainTexture == textures[(int)polarType.DARK])
        {
            thisRenderer.material.mainTexture = textures[(int)polarType.LIGHT];
            shields[(int)polarType.LIGHT].SetActive(true);
            shields[(int)polarType.DARK].SetActive(false);
            //polarize = false;
        }

        //apply tilt value to object transform
        thisTransform.rotation = Quaternion.Euler(270f, 0f, tilt);




        //bind movement to gamespace
        movement = new Vector3(
            Mathf.Clamp(thisTransform.position.x, -horizontalRange, horizontalRange),
            Mathf.Clamp(thisTransform.position.y, -verticalRange, verticalRange),
            0.0f
            );
        thisTransform.position = movement;


        //fire shot
        if(Input.GetButton("Fire1") && Time.time > lastShot + (1.0f / shotFireRate))
        {
            //loob through bgun barrels and create a shot for each
            foreach(GameObject barrel in gunBarrel)
            {
                Instantiate(shot[(int)polarity], barrel.transform.position, Quaternion.identity);
                //set shot time limiter
                lastShot = Time.time;
            }
        }

        //switch polarity whith alt fire
        if (Input.GetButtonDown("Fire2"))
        {
            polarSwap();
        }


        //set UI text
        UITexts[1].text = energyPrefix + energy;
    }


    //disable gameobject on death condition
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("EnemyBullet"))
        {
           
            Entity bullet = other.gameObject.GetComponent<EnemyBulletControl>();
            //Debug.Log(bullet.getPolarity());
            if (bullet.Polarity== polarity)
            {
                UITexts[0].text = col_charge;
                UITexts[0].color = new Color(0, 1, 0);
                energy += 5.0f;
                if (energy > maxEnergy) energy = maxEnergy;
            }
            else
            {
                UITexts[0].text = col_breech;
                UITexts[0].color = new Color(1, 0, 0);
            }
        }


        //skip if godMode or respawn shielded
        if (godMode || tempShield) return;

        //check if other is enemy tagged
        if(other.CompareTag("Enemy"))
        {

            

                //instantiate death effects
             if (gibs)
            {
                Instantiate(gibs, thisTransform.position, gibs.transform.rotation);
            }

            //disble gameObject and set respawn time
            gameObject.SetActive(false);
            Invoke("Respawn", respawnTime);
        }


    }


    //reset player position and reactivate gameobject
    private void respawn()
    {
        thisTransform.position = new Vector3(0.0f, -verticalRange, 0.0f);
        gameObject.SetActive(true);
        tempShield = true;
        Invoke("EndShield", shieldTimer);
    }


    //disable temporary shield
    private void endShield()
    {
        tempShield = false;
    }

    //change polarity 
    private void polarSwap()
    {
        polarity = polarity == polarType.LIGHT ? polarType.DARK : polarType.LIGHT;
        //polarize = true;
    }
}
