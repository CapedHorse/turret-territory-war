using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public GameObject turretMeshParent;
    
    [SerializeField] bool allowMove = false;//, moveRight = true;

    [SerializeField] Quaternion maxRotRight, maxRotLeft;
    
    [SerializeField] float lerp, fixedPlayTime;
    
    void Start()
    {
        InitiateRandomColor();
        
        float angle = GameManager.instance.gameSettings.turretRotateDegree;
        // transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x,
        //     Random.Range(transform.localEulerAngles.y - angle, transform.localEulerAngles.y + angle), transform.localEulerAngles.z);

        maxRotLeft = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y- angle, transform.localEulerAngles.z );
        maxRotRight = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y+ angle, transform.localEulerAngles.z );

        StartCoroutine(DelayedStart());
    }

    //for temporary, should have got it from game setting's team
    void InitiateRandomColor()
    {
        Color randColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), 0.6f, 1);
        foreach (var turretMesh in turretMeshParent.GetComponentsInChildren<MeshRenderer>())
        {
            turretMesh.materials[0].color = randColor;
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(Random.Range(0, 2));
        allowMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowMove)
        {
            //move to left at first
            fixedPlayTime += Time.fixedDeltaTime;
            lerp = .5f * (GameManager.instance.gameSettings.turretRotateSpeed 
                           + Mathf.Sin(Mathf.PI * fixedPlayTime * 0.1f));
            transform.localRotation = Quaternion.Lerp(maxRotLeft, maxRotRight, lerp);
        }
    }
}
