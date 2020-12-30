﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastController : MonoBehaviour
{
    public float maxDistanceRay = 100f;
    public static RaycastController instance;
    public Text birdName;
    public Transform gunFlashTarget;
    public float fireRate = 1.6f;
    private bool nextShot = true;
    private string objName = "";

    AudioSource audioSource;
    public AudioClip[] clips;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void playSound(int sound) {
        audioSource.clip = clips[sound];
        audioSource.Play();
    }

    void Start()
    {
        GameObject particle = GameObject.Find("gunFlashSmoke");
        particle.SetActive(false);
        // StartCoroutine(spawnNewBird());
        // playSound(2);
    }

    private IEnumerator spawnNewBird() {
        yield return new WaitForSeconds(0.75f);
        GameObject newBird = Instantiate(Resources.Load("Bird_Asset", typeof(GameObject))) as GameObject;
        newBird.transform.gameObject.layer = 8;
        newBird.transform.parent = GameObject.Find("ImageTarget").transform;
        newBird.transform.localScale = new Vector3(20f, 20f, 20f);

        Vector3 temp;

        temp.x = Random.Range(-4.5f,4.5f);
        temp.y = Random.Range(1.4f,1.1f);
        temp.z = Random.Range(-5.5f,5.5f);
        newBird.transform.position = new Vector3(temp.x, temp.y-7.5f, temp.z);

        // float speed = Random.Range(2.9f, 2.2f);
        // newBird.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void Fire() {
        if(nextShot) {
            StartCoroutine(takeShot());
            nextShot = false;
        }
    }

    private IEnumerator showParticles() {
        yield return new WaitForSeconds(1.2f);
        GameObject particle = GameObject.Find("gunFlashSmoke(Clone)");
        Destroy(particle);
    }

    private IEnumerator takeShot() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycastHit;

        GameObject gunFlash = Instantiate(Resources.Load("gunFlashSmoke", typeof(GameObject))) as GameObject;
        gunFlash.transform.position = gunFlashTarget.transform.position;
        StartCoroutine(showParticles());

        GameObject newBirdSpawn = GameObject.Find("Bird_Asset(Clone)");

        int layer_mask = LayerMask.GetMask("bird_layer");
        if(Physics.Raycast(ray, out raycastHit, maxDistanceRay, layer_mask)) {
            objName = raycastHit.collider.gameObject.name;
            birdName.text = objName;
            Vector3 birdPosition = raycastHit.collider.gameObject.transform.position;

            if(objName == "Bird_Asset") {
                Destroy(GameObject.Find("Bird_Asset"));
                StartCoroutine(spawnNewBird());
            }

            if(newBirdSpawn) {
                Destroy(GameObject.Find("Bird_Asset(Clone)"));
                StartCoroutine(spawnNewBird());
            }
        }



        yield return new WaitForSeconds(fireRate);

        nextShot = true;
    }

    void Update()
    {
        
    }
}