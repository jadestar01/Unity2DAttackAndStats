using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class Mob : MonoBehaviour
{
    public float detectingRadius = 3;
    public float trackingRadius = 5;
    public float moveSpeed;
    public bool inDetecting;
    public bool inTracking;
    bool isActive;

    public Transform Character;
    public GameObject detect;
    public GameObject tracking;

    Vector3 detectingPos;

    void Start()
    {
        tracking.SetActive(false);
    }

    void Update()
    {
        TrackingActivatedFuction();
        TrackingPlayerDown();
    }

    void TrackingActivatedFuction()
    {
        if (!inTracking) { tracking.SetActive(false); isActive = false; }
        if (inDetecting) { tracking.SetActive(true); isActive = true;  detectingPos = transform.position; }
    }

    void TrackingPlayerDown()
    {
        if (inTracking)
        {
            transform.DOMove(Character.transform.position, moveSpeed);
        }
        else { }
    }
}
