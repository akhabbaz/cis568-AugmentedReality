using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;


public class TransferVector3 : Photon.MonoBehaviour {

    public static readonly byte[] memVec3 = new byte[3 * 4]; 
    private static short SerializeVector3(StreamBuffer outStream, object customObject )
    {
        Vector3 vo = (Vector3) customObject;
        byte[] bytes = memVec3;
        int index = 0;
        Protocol.Serialize(vo.x, bytes, ref index);
        Protocol.Serialize(vo.y, bytes, ref index);
        Protocol.Serialize(vo.z, bytes, ref index);
        outStream.Write(bytes, 0, 3 * 4);
        return 3 * 4;
    } 
    private static object DeserializeVector3(StreamBuffer inStream, short length)
    {
        Vector3 vo = new Vector3();
        lock (memVec3)
        {
            inStream.Read(memVec3, 0, 3 * 4);
            int index = 0;
            Protocol.Deserialize(out vo.x, memVec3, ref index);
            Protocol.Deserialize(out vo.y, memVec3, ref index);
            Protocol.Deserialize(out vo.z, memVec3, ref index);
        }
        return vo;
    }
	// Use this for initialization
	void Start () {
        PhotonPeer.RegisterType(typeof(Vector3), (byte)'W', SerializeVector3, DeserializeVector3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
