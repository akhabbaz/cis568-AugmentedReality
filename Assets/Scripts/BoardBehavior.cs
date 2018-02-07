using UnityEngine;
using System.Collections.Generic;
using Photon;

public class BoardBehavior : Photon.MonoBehaviour {

    public GameObject SplatterPrefab;

    private List<GameObject> splatters = new List<GameObject>();

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnCollisionEnter(Collision collision)
    {
       
        var other = collision.collider.gameObject;
        Vector3 hit_position = other.transform.position;
        if (other.CompareTag("Ball"))
        {
            PhotonNetwork.Destroy(other);
            Quaternion rot =  Quaternion.AngleAxis(Random.Range(0f, 360f), new Vector3(0, 0, 1)) ; //*transform.rotation;
            var splatter = Instantiate(SplatterPrefab, hit_position, rot) as GameObject;

            splatter.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;

            splatters.Add(splatter);
        }

    }
}
