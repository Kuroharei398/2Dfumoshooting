using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    //メンバ変数
    [Header("スクロールの速さ")][SerializeField] float scrollspeed = 0.1f;
    [Header("移動：x方向(1：正 / -1：負 / 0：移動なし)")][SerializeField] int moveX = 1;
    [Header("移動：y方向(1：正 / -1：負 / 0：移動なし)")][SerializeField] int moveY = 1;

    //プレイヤーの動きに追従する場合
    [Header("プレイヤーの動きに追従して背景が動く")][SerializeField] bool playerFollow = false;
    [Header("プレイヤーの動きに追従させる割合")][SerializeField] float playerMove = 0.01f;
    [Header("動きを追従するオブジェクト")][SerializeField] GameObject player;

    float scrollMax = 1f;       //スクロールの最終地点(最大値)
    Vector2 offset;             //背景をずらすための管理用
    Renderer material;          //マテリアル用

    // Start is called before the first frame update
    void Start()
    {
        //マテリアル取得
        material = GetComponent<Renderer>();

        //プレイヤーの追従がtrueかつプレイヤーがnullの場合
        if(playerFollow && player == null)
        {
            //プレイヤー取得
            player = GameObject.FindWithTag("Player").GetComponent<GameObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの追従がfalseの場合
        if(!playerFollow)
        {
            //playerMoveを0にする
            playerMove = 0;
        }

        //プレイヤーの追従がtrueかつ、プレイヤーがnullの場合
        if(playerFollow && player == null)
        {
            //処理を中断
            return;
        }

        //ゲーム内時間で0からscrollMax(1)になり、0に戻ってを繰り返す
        float scrollX = Mathf.Repeat(Time.time * scrollspeed * moveX, scrollMax),
              scrollY = Mathf.Repeat(Time.time * scrollspeed * moveY, scrollMax);

        //プレイヤーの位置を取得
        float movePointX = player.transform.position.x,
              movePointY = player.transform.position.y;

        //オフセットを作成
        offset = new Vector2(scrollX + movePointX * playerMove ,        //X
                             scrollY + movePointY * playerMove);        //Y

        //マテリアルにオフセットを設定
        material.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
