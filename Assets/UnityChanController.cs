using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    //スクリプトでアニメーションの操作をするには、オブジェクトにアタッチしているAnimatorコンポーネントを取得しなくてはいけない。
    private Animator myAnimator;

    //Unityちゃんを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;
    //前方向の速度
    private float velocityZ = 16f;
    //横方向の速度
    private float velocityX = 10f;
    //上方向の速度
    private float velocityY = 10f;
    //左右の移動できる範囲
    private float movableRange = 3.4f;
    //動きを減速させる係数
    private float coefficient = 0.99f;
    //ゲーム終了の判定
    private bool isEnd = false;
    //ゲーム終了時に表示するテキスト
    private GameObject stateText;
    //スコアを表示するテキスト
    private GameObject scoreText;
    //得点
    private int score = 0;

    //左ボタン押下の判定
    private bool isLButtonDown = false;
    //右ボタン押下の判定
    private bool isRButtonDown = false;
    //ジャンプボタン押下の判定
    private bool isJButtonDown = false;


    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを開始「SetFloat」関数は、第一引数に与えられたパラメータに、第二引数の値を代入する関数。また、第一引数のバラメータがアニメーション再生の条件に使われている。
        this.myAnimator.SetFloat ("Speed", 1);
        //※UnityChanLocomotionsの設定では、Speedパラメータが0.8以上の値の場合に、走るアニメーションを再生する設定となっている

        //Rigidbodyコンポーネントを取得
        //速度を与えるためにはRigidbodyが必要
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");
        //シーン中のscoreTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならUnityちゃんの動きを減衰する
        if(this.isEnd) {
            this.velocityZ *= this.coefficient;
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }

        //横方向の入力による速度
        float inputVelocityX = 0;
        //上方向の入力による速度
        float inputVelocityY = 0;

        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
            //左方向への速度を代入
            inputVelocityX = -this.velocityX;
        }
        else if ((Input.GetKey (KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange) {
            //右方向への速度を代入
            inputVelocityX = this.velocityX;
        }

        //ジャンプしていない時にスペース押下でジャンプ
        if((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown) && this.transform.position.y < 0.5f) {
            //ジャンプアニメを再生。第一引数に与えられたパラメータに、第二引数の値を代入する関数
            this.myAnimator.SetBool("Jump", true);
            //上方向への速度を代入
            inputVelocityY = this.velocityY;
        }
        else {
            //現在のY軸の速度を代入 this.transform.position.yだとだめ？
            //this.myRigidbody.velocity.yは現在のY軸の速度。
            inputVelocityY = this.myRigidbody.velocity.y;
        }

        //Jumpステートの場合はjumpにfalseをセットする
        //Jumpパラメータをtrueにしたままではジャンプアニメーションを何度も再生し続けてしまうので、ジャンプ状態の時には、if文の中でJumpパラメータをfalseとする
        //「GetCurrentAnimatorStateInfo(0)」で現在のアニメーションの状態を取得。「IsName」関数で取得したステートの名前が引数の文字列と一致しているかどうかを調べる。
        if(this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            this.myAnimator.SetBool("Jump", false);
        }

        //Unityちゃんに前進するための速度を与える。前向きの速度を与えることで前進させている。
        //Rigidbodyクラスのvelocityは、Vector3型の変数で物体の持つ速度を表す。前のみの速度なので、0, 0 
        //this.myRigidbody.velocity = new Vector3(0, 0, this.velocityZ);
        this.myRigidbody.velocity = new Vector3 (inputVelocityX , inputVelocityY , velocityZ);
    }

    //トリガーモードで他のオブジェクトと接触した場合の処理
    //「OnTriggerEnter」関数は、自分のColliderが他のオブジェクトのColliderと接触した時に呼ばれる関数
    //この関数が呼ばれるためには少なくともどちらか一方のオブジェクトがTriggerモードでなくてはいけない
    void OnTriggerEnter(Collider other) {
        //障害物に衝突した場合
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
            this.isEnd = true;
            //stateTextにGAME OVERを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag") {
            this.isEnd = true;
            //stateTextにGAME CLEARを表示
            this.stateText.GetComponent<Text>().text = "GAME CLEAR";
        }

        //コインに衝突した場合
        if(other.gameObject.tag == "CoinTag") {
            //スコアを加算
            this.score += 10;
            //ScoreTextに表示
            this.scoreText.GetComponent<Text> ().text = "Score " + this.score + "pt";
            //パーティクルを再生
            GetComponent<ParticleSystem>().Play();
            //接触したコインオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }

    //ジャンプボタン押下時の処理 ★ここがpublic voidなのは、ボタンのインスペクタから呼び出すようにしているから
    public void GetMyJumpButtonDown() {
        this.isJButtonDown = true;
    }
    //ジャンプボタンを離した時の処理
    public void GetMyJumpButtonUp() {
        this.isJButtonDown = false;
    }
    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown() {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理
    public void GetMyLeftButtonUp() {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown() {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理
    public void GetMyRightButtonUp() {
        this.isRButtonDown = false;
    }
}

