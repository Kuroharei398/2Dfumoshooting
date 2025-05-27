using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region var-EnemyGenerate

    [Header("生成する敵オブジェクト")]
    [SerializeField] GameObject[] enemies;

    [Header("敵の初回生成時間")]
    [SerializeField] float genDelay = 2f;

    [Header("二回目以降の敵の生成間隔")]
    [SerializeField] float genInterval = 1.5f;

    [Header("敵生成位置の画面の余白")]
    [SerializeField] float genMarginY = 1f;

    #endregion

    #region var-Interval
    Vector2 genMin, genMax;                                     //画面サイズ取得用
    #endregion


    #region Start
    void Start()
    {
        //画面サイズを取得
        genMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        genMax = Camera.main.ViewportToWorldPoint(Vector2.one);

        //敵生成の関数を指定した時間毎にリピート
        InvokeRepeating("EnemyGenerate", genDelay, genInterval);
    }
    #endregion

    #region EnemyGenerate
    //敵を生成する
    void EnemyGenerate()
    {
        //敵を生成
        Instantiate(
            //敵の配列(生成する敵オブジェクト)からランダムで選択
            enemies[Random.Range(0 , enemies.Length)] , 
            
            //生成位置を指定(x:EnemyGenerator y:取得した画面サイズ±余白)
            new Vector2(transform.position.x , 
                        Random.Range(genMin.y + genMarginY , genMax.y - genMarginY)) ,
            
            //回転なし
            Quaternion.identity
            );
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
