using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public delegate byte[] SerializeMethod(object customObject)
{

}
public class TransferVector3 : Photon.MonoBehaviour {

    public static readonly byte[] memVec3 = new byte[3 * 4]; 
    private static short SerializeVector3(StreamBuffer streamBuffer, ) 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
