using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ProjectileGun : MonoBehaviour
{

    [Header("Reference")]
    public Transform attackPoint;
    public Camera cameraa;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public AudioSource shootSound;
    public AudioClip soundclip;
    public AnimationCurve chromlerpCurve;
    public float lerpTime;
    public float time;
    public Volume volume;
    private ChromaticAberration chrom;
    private bool running;

    [Header("Bullet")]
    public GameObject bullet;
    public float shootForce = 200;
    public float upForce;

    [Header("Key")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;
    

    [Header("Gun Stats")]
    public float timeBetweenShooting = 0.5f;
    public float spread = 3;
    public float reloadTime = 2;
    public float timeBetweenShots = 1;
    public int magazineSize = 100;
    public int bulletsPerTap = 1;
    public bool allowButtonHold = false;

    int bulletsLeft;
    int bulletsShot;

    public Rigidbody player;
    public float recoilForce;

    bool shooting, reloading, readyToShoot;

    public bool allowInvoke = false;

    

    private void Awake()
    {
        readyToShoot = true;
        bulletsLeft = magazineSize;
        //volume.profile.TryGet<ChromaticAberration>(out chrom);
    }

    private void Update()
    {
        MyInput();

        if (!running) return;

        time += Time.deltaTime;
        if (time > lerpTime)
        {
          time = 0;
          running = false;
        }
       float lerpRatio = time / lerpTime;
       //chrom.intensity.value = chromlerpCurve.Evaluate(lerpRatio);
    }
    void FixedUpdate()
    {
        
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(shootKey);
        } else
        {
            shooting = Input.GetKeyDown(shootKey);
        }

        if(Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading){
            Reload();
        }

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cameraa.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(90);
        }
        Vector3 direction = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cameraa.transform.up * upForce, ForceMode.Impulse);

        if(muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }
        ShootAnimation();
        
        shootSound.PlayOneShot(soundclip, 0.7F);



        bulletsLeft--;
        bulletsShot--;

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;


            player.AddForce(-direction.normalized * recoilForce, ForceMode.Impulse);
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }


    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void ShootAnimation()
    {
       running = true;
       time = 0; 

    }

}
