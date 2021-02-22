using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomElement: MonoBehaviour 
{
    //Room情報UI表示用
    public Text RoomName;   //部屋名
    public Text PlayerNumber;   //人数
    public Text RoomCreator;    //部屋作成者名
 
    //入室ボタンroomname格納用
    private string roomname;
 
    //GetRoomListからRoom情報をRoomElementにセットしていくための関数
    public void SetRoomInfo(string _RoomName,int _PlayerNumber,int _MaxPlayer,string _RoomCreator)
    {
        //入室ボタン用roomname取得
        roomname = _RoomName;
        RoomName.text ="Name   ："+_RoomName;
        PlayerNumber.text ="Number：" +_PlayerNumber+"/"+_MaxPlayer;
        RoomCreator.text = "Creater ："+_RoomCreator;
    }
 
    //入室ボタン処理
    public void OnJoinRoomButton()
    {
        //roomnameの部屋に入室
        PhotonNetwork.JoinRoom(roomname);
    }
}