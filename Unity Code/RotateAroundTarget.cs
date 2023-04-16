using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour
{
    public Transform target;
    public int speed;
    public float maxHeight;
    public float vSpeed;
    public bool switchesRotation;
    public float switchTimerMax;
    public bool phasesOut;
    public float phaseTimerMax;
    public CapsuleCollider hitbox;
    public SkinnedMeshRenderer ghostBody;
    public List<Material> materials;


    bool goUp;
    float direction;
    float switchTimer;
    float phaseTimer;
    bool phasedOut;
    Vector3 originalPos;
    Quaternion originalRotation;
    Vector3 originalScale;

    void Start()
    {
        //var player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (target == null) target = this.gameObject.transform;
        originalPos = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        ResetTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.RotateAround(target.transform.position, direction * target.transform.up, speed * Time.deltaTime);
        if (goUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (vSpeed * Time.deltaTime), transform.position.z);
            if(transform.position.y >= maxHeight)
            {
                goUp = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (vSpeed * Time.deltaTime), transform.position.z);
            if (transform.position.y <= -maxHeight)
            {
                goUp = true;
            }
        }

        if (switchesRotation)
        {
            switchTimer -= Time.deltaTime;
            if (switchTimer <= 0)
            {
                switchTimer = switchTimerMax;
                direction *= -1f;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1f);
            }
        }
        if (phasesOut)
        {
            phaseTimer -= Time.deltaTime;
            //Debug.Log(phaseTimer);
            if (phaseTimer <= 0)
            {
                phaseTimer = phaseTimerMax;
                phasedOut = !phasedOut;
                hitbox.enabled = !phasedOut;
                if (!phasedOut)
                    ghostBody.material = materials[0];
                else
                    ghostBody.material = materials[1];
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetTarget()
    {
        transform.position = originalPos;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
        goUp = true;
        direction = 1f;
        switchTimer = switchTimerMax;
        phaseTimer = phaseTimerMax;
        if (phasesOut)
        {
            ghostBody.material = materials[0];
            phasedOut = false;
        }
    }
}
