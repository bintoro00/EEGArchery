using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class CreateRoomManager : MonoBehaviourPunCallbacks
{
    public Animator anim;
    public GameObject LobbyP;
    void Start(){
        LobbyP.SetActive(false);
    }
    public void Connect(){
        //Photonに接続できていなければ
        if (!PhotonNetwork.IsConnected)
        {           
            PhotonNetwork.ConnectUsingSettings();   //Photonに接続する
            Debug.Log("Connecting for Photon");
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                PhotonNetwork.NickName = "player" + Random.Range(1, 99999);
            }
            //SceneManager.LoadScene("Lobby");
            LobbyP.SetActive(true);
            anim.SetBool("slide",true);
        }
    }
    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}