
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Shot : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Arrow arrowPrefab = default;
    [SerializeField]
    private Camera cameraPrefab = default;
    public TextMesh nameL1,nameL2,nameL3,nameL4,nameL5;
    public Transform muzzle,sCam;
    public bool set = true;
    public bool OnBtn = false;
    public int OwnerId{get; private set;}
    public TextMesh toriaezu;
    private Rigidbody arrowRig;
    private int arrowId = 0;
    private bool shotMove = false;
    private GameObject mainUI,subC;
    private GameObject Arrows = default;
    private Camera shotCam = default;
    void Start(){
        OwnerId = this.gameObject.GetComponent<PhotonView>().ViewID;
        mainUI = GameObject.Find("UI");
        subC = GameObject.Find("SubCam");
    }
    private void Update(){
        if(photonView.IsMine){
            photonView.RPC(nameof(HitByProjectile), RpcTarget.All, OwnerId);
            if(set){
                photonView.RPC(nameof(FireArrow), RpcTarget.All, ++arrowId, "Set");

            }
            else if(OnBtn&&!set){
                photonView.RPC(nameof(FireArrow), RpcTarget.All, ++arrowId, "Shot");
                shotMove = true;
                ShotMove();
            }
        }
    }

    [PunRPC]
    private void FireArrow(int id, string type){
        switch(type){
            case "Set":
                var _arrow = Instantiate(arrowPrefab);
                _arrow.Init(muzzle.position, muzzle.rotation);
                arrowRig = _arrow.GetComponent<Rigidbody>();
                Arrows = _arrow.gameObject;
                set = false;
            break;
            case "Shot":
                set = false;
                //AddForceの方
                Vector3 f = new Vector3(1f,1f,25f);
                arrowRig.AddForce(f,ForceMode.Impulse);
                
                //Rayの方
                /*var ray = new Ray (transform.position, transform.forward + new Vector3(0.07f,0.09f,0f));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 200.0f)){ //レイを飛ばしヒットした座標に向かって弾を飛ばす
                    arrowRig.velocity = (hit.point - transform.position).normalized * 1500.0f * Time.deltaTime;
                }
                else{ //レイにヒットしなければ射程距離の地点に向かって弾を飛ばす
                    arrowRig.velocity = (ray.GetPoint(20.0f) - transform.position).normalized * 1500.0f* Time.deltaTime;
                }
                if(hit.collider.tag=="red"){
                    Debug.Log("あたり");
                }*/
                OnBtn = false;
            break;
        }
    }
    [PunRPC]
    private void HitByProjectile(int ownerId) {
        if (ownerId == 1001){
            
        }
    }
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps) {
        if (target.ActorNumber != photonView.OwnerActorNr) { return; }
        if (changedProps["Score1"] is int score1) {
            nameL1.text = score1.ToString();
        }
        if (changedProps["Score2"] is int score2) {
            nameL2.text = score2.ToString();
        }
        if (changedProps["Score3"] is int score3) {
            nameL3.text = score3.ToString();
        }
        if (changedProps["Score4"] is int score4) {
            nameL4.text = score4.ToString();
        }
        if (changedProps["Score5"] is int score5) {
            nameL5.text = score5.ToString();
        }
        if( changedProps["Timer"] is float time) {
            
        }
    }
    void ShotMove(){
        if(shotMove){
            mainUI.SetActive(false);
            subC.SetActive(false);
            shotCam = Instantiate(cameraPrefab);
            shotCam.transform.position = new Vector3(sCam.position.x,sCam.position.y,sCam.position.z-5);
            shotCam.transform.rotation = sCam.rotation;
            shotCam.transform.SetParent(Arrows.transform);
            Invoke("OutUI", 2.7f);
            Invoke("OnUI", 3.0f);
        }
    }
    void OutUI(){
        //PhotonManager pm = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        //ScoreJudge sj = toriaezu.GetComponent<ScoreJudge>();
        //sj.SetAnim("Reset");
        Animator anim = GameObject.Find("Hit").GetComponent<Animator>();
        anim.SetBool("hit",false);
        anim.SetBool("miss",false);
    }
    void OnUI(){
        mainUI.SetActive(true);
        subC.SetActive(true);
        Destroy(shotCam);
        set = true;
        shotMove = false;
    }
}