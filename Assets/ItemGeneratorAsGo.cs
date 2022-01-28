using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneratorAsGo : MonoBehaviour
{
    //carPrefabを入れる なぜこれらはpublicにしているのか→インスペクタに表示される
    public GameObject carPrefab;
    //coinPrefabを言える
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;

    //スタート地点
    private int startPos = 80;
    //ゴール地点
    private int goalPos = 360;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;
    //スタート地点でアイテムの出すZ方向の範囲
    private int startItemPos = 50;
    //アイテムを置く間隔
    private int itemSpace = 15;
    //最期にアイテムを置いたZ軸
    private float itemLastPosZ;
    //最期にアイテムを置いた時のUnityちゃんのZ軸
    private float UnityChanLastPosZ;

    //Unityちゃんオブジェクト
    private GameObject unityChan;
    Vector3 unityChanPos;

    // Start is called before the first frame update
    void Start()
    {
        //Unityちゃんオブジェクトを取得
        this.unityChan = GameObject.Find("unitychan");

        //スタート時にstartItemPos分だけアイテムを置く
        for(int i = startPos; i < startPos + startItemPos; i += 15) {
            //どのアイテムを出すのかをランダムに設定 1～11の間でランダム
            int num = Random.Range(1, 11);
            if(num <= 2) {
                //コーンをx軸方向に一直線に生成
                for(float j = -1; j <= 1; j += 0.4f) {
                    GameObject cone = Instantiate(conePrefab);
                    cone.transform.position = new Vector3(4 * j, cone.transform.position.y, i);
                }
            }
            else {
                //レーンごとにアイテムを生成
                for(int j = -1; j <= 1; j++) {
                    //アイテムの種類を決める
                    int item = Random.Range(1, 11);
                    //アイテムを置くZ座標のオフセットをランダムに設定
                    int offsetZ = Random.Range(-5, 6);

                    //60%コイン配置：30%車配置：10%何もなし
                    //「Instantiate () 」は、()内に指定したPrefabのインスタンスをGameObject型として生成
                    if(1 <= item && item <= 6) {
                        //コインを生成
                        GameObject coin = Instantiate(coinPrefab);
                        coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, i + offsetZ);
                    }
                    else if(7 <= item && item <= 9) {
                        //車を生成
                        GameObject car = Instantiate(carPrefab);
                        car.transform.position = new Vector3(posRange * j, car.transform.position.y, i + offsetZ);
                    }
                }
            }
            this.itemLastPosZ = i;
            this.UnityChanLastPosZ = startPos;
            Debug.Log("itemLastPosz: " + itemLastPosZ);
            Debug.Log("UnityChanLastPosZ: " + UnityChanLastPosZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Unityちゃんの位置を取得
        this.unityChanPos = this.unityChan.transform.position;
        Debug.Log(unityChanPos.z);
        
        //Unityちゃんの位置が、最後にアイテムを置いたUnityちゃんの場所+アイテム間隔より大きい、かつ、アイテムを置く場所がゴールより小さい場合にアイテムを置く。
        if(this.unityChanPos.z >= this.UnityChanLastPosZ + itemSpace && itemLastPosZ + itemSpace < goalPos) {
            Debug.Log("アイテムを置く");
            itemLastPosZ += itemSpace;
            UnityChanLastPosZ += itemSpace;

            //どのアイテムを出すのかをランダムに設定 1～11の間でランダム
            int num = Random.Range(1, 11);
            if(num <= 2) {
                //コーンをx軸方向に一直線に生成
                for(float j = -1; j <= 1; j += 0.4f) {
                    GameObject cone = Instantiate(conePrefab);
                    cone.transform.position = new Vector3(4 * j, cone.transform.position.y, itemLastPosZ);
                }
            }
            else {
                //レーンごとにアイテムを生成
                for(int j = -1; j <= 1; j++) {
                    //アイテムの種類を決める
                    int item = Random.Range(1, 11);
                    //アイテムを置くZ座標のオフセットをランダムに設定
                    int offsetZ = Random.Range(-5, 6);

                    //60%コイン配置：30%車配置：10%何もなし
                    //「Instantiate () 」は、()内に指定したPrefabのインスタンスをGameObject型として生成
                    if(1 <= item && item <= 6) {
                        //コインを生成
                        GameObject coin = Instantiate(coinPrefab);
                        coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, itemLastPosZ + offsetZ);
                    }
                    else if(7 <= item && item <= 9) {
                        //車を生成
                        GameObject car = Instantiate(carPrefab);
                        car.transform.position = new Vector3(posRange * j, car.transform.position.y, itemLastPosZ + offsetZ);
                    }
                }
            }            
        }
        
    }
}
