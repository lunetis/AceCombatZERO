using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAircraft : AircraftAI
{
    [SerializeField]
    float destroyDelay = 1;

    [SerializeField]
    Transform smokeTransformParent;

    [SerializeField]
    [Range(0, 1)]
    float playerTrackingRate = 0.5f;

    [SerializeField]
    float minimumPlayerDistance = 2000; // If current distance is longer than this value, AI will follow the player

    protected override Vector3 CreateWaypoint()
    {
        float rate = Random.Range(0.0f, 1.0f);
        float distance = Vector3.Distance(transform.position, GameManager.PlayerAircraft.transform.position);
        
        if(rate < playerTrackingRate || distance > minimumPlayerDistance)
        {
            Debug.Log("Tracking");
            return GameManager.PlayerAircraft.transform.position;
        }
        else
        {
            Debug.Log("Not Tracking");
            return base.CreateWaypoint();
        }
    }
    
    protected override void DestroyObject() 
    {
        CommonDestroyFunction();
        Invoke("DelayedDestroy", destroyDelay);
    }

    void DelayedDestroy()
    {
        GameObject obj = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        obj.transform.localScale *= 3;
        Destroy(gameObject);
    }

    public override void OnDamage(float damage, int layer)
    {
        base.OnDamage(damage, layer);

        for(int i = 0; i < smokeTransformParent.childCount; i++)
        {
            GameManager.Instance.CreateDamageSmokeEffect(smokeTransformParent.GetChild(i));
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        // currentWaypoint = GameManager.PlayerAircraft.transform.position;
    }
}
