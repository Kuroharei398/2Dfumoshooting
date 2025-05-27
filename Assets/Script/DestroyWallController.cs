using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWallController : MonoBehaviour
{
    //接触判定
    void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤー以外が接触した場合
        if (!collision.gameObject.CompareTag("Player"))
        {
            //接触したオブジェクトを破棄
            Destroy(collision.gameObject);
        }
    }
}
