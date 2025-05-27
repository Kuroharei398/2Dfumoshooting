using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Enemy_move
    [Header("敵の移動関連")]
    [SerializeField] float enemyMoveSpeed = 3f;                     //敵の移動速度
    #endregion

    #region Enemy_internal
    Rigidbody2D enemyRB;                                            //敵のリジッドボディ
    #endregion

    #region Enemy_shots
    [Header("敵のショット関連")]
    [SerializeField] float enemyShotSpeed = 5f;    //敵の弾の発射スピード
    [SerializeField] GameObject enemyBullet;       //敵の弾のリジッドボディ
    [SerializeField] Transform firePos;             //敵の弾の発射ポイント
    [SerializeField] float shotThreshold = 1f;      //発射間隔の閾値
    #endregion

    float shotDelayRest = 0, shotDelay = 0;         //発射間隔の初期値・時間

    #region Start
    void Start()
    {
        //敵のリジッドボディを取得
        enemyRB = GetComponent<Rigidbody2D>();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    #region FixedUpdate
    private void FixedUpdate()
    {
        //敵の行動
        EnemyMove();
    }
    #endregion

    #region EnemyMove
    void EnemyMove()
    {
        //敵に進行方向(画面左)のベクトルを持たせる
        enemyRB.velocity = new Vector2(-enemyMoveSpeed, 0);

        //弾の発射-----------------------------

        //ゲーム内時間を加算
        shotDelay += Time.deltaTime;

        //弾の発射間隔が閾値以下の場合
        if (shotDelay <= shotThreshold)
        {
            //処理を中断
            return;
        }
        //それ以外
        else
        {
            //弾発射
            EnemyShot(firePos);
        }

        //発射間隔のリセット
        shotDelay = shotDelayRest;
    }
    #endregion

    #region Enemy hit detection
    //敵の当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーと接触した場合
        if(collision.gameObject.CompareTag("Player"))
        {
            // 自身を破棄
            Destroy(gameObject);

            //接触したプレイヤーを破棄
            Destroy(collision.gameObject);
        }

        //プレイヤーの弾と接触した場合
        else if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            //自身を破棄
            Destroy(gameObject);

            //接触したプレイヤーの弾を破棄
            Destroy(collision.gameObject);
        }
    }
    #endregion

    #region EnemyShot
    //敵の弾の発射
    void EnemyShot(Transform firePos)
    {
        //弾のコピーを生成
        GameObject bulletClone = Instantiate(enemyBullet, firePos.position, Quaternion.identity);

        // プレイヤーの向きに合わせて弾の速度を設定
        float shotDirection = transform.localScale.x > 0 ? -1f : 1f; // 向きに応じて方向を決定

        //弾のコピーのRigidbodyへベクトルを与える
        bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(enemyShotSpeed * shotDirection, 0);
    }
    #endregion
}
