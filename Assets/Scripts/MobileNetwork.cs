using UnityEngine;

public class MobileNetwork : Photon.PunBehaviour
{
 
    void Start()
    {
	    PhotonNetwork.ConnectUsingSettings("0.1");
    }
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    public override void OnJoinedLobby()
    {
	    PhotonNetwork.JoinRandomRoom();
    }
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
	    base.OnPhotonJoinRoomFailed(codeAndMsg);
    }

    // TODO-2.a: the same as 1.b
    //   and join a room


    public override void OnJoinedRoom()
    {
	    Debug.Log("Joined Random Room Success");
	    base.OnJoinedRoom();
	    GetComponent<MobileShooter>().Activate();
    }


}
