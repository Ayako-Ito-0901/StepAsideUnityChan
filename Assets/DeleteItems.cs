using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItems : MonoBehaviour
{
    //Unityちゃんオブジェクト
    private GameObject unityChan;
    Vector3 unityChanPos;
    //deleteプラス位置
    private float plusPos = 5; //5だとちょうどよいかも
    //アイテム
    Vector3 itemPos;

    // Start is called before the first frame update
    void Start()
    {
        //Unityちゃんオブジェクトを取得
        this.unityChan = GameObject.Find("unitychan");
    }

    // Update is called once per frame
    void Update()
    {
        //Unityちゃんの位置を取得
        this.unityChanPos = this.unityChan.transform.position;
        
        //Unityちゃんの位置よりさらにplusPos分だけ後ろのアイテムを削除
        if(this.transform.position.z < this.unityChanPos.z - plusPos) {
            Destroy(this.gameObject);
        }
    }
}
