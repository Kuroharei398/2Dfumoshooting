using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim = null;   //プレイヤーのアニメーターを格納する変数

    [SerializeField] float moveSpeed = 5f;  //プレイヤーの移動速度
    [SerializeField] float marginX = 1f , marginY = 1f; //移動範囲の余白

    #region Plyare_shots
    [Header("プレイヤーのショット関連")]
    [SerializeField] float playerShotSpeed = 5f;    //プレイヤーの弾の発射スピード
    [SerializeField] GameObject playerBullet;       //プレイヤーの弾のリジッドボディ
    [SerializeField] Transform firePos;             //プレイヤーの弾の発射ポイント
    [SerializeField] float shotThreshold = 1f;      //発射間隔の閾値
    #endregion

    Rigidbody2D playerRB;                           //プレイヤーのリジッドボディ
    Vector2 moveDirection , screenMin , screenMax;  //移動用ベクトル、画角の最小値、最大値
    Vector2 playerPos;                              //プレイヤーの最終位置
    float shotDelayRest = 0, shotDelay = 0;         //発射間隔の初期値・時間

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動時の入力受付
        InputProcess();

        //弾の発射-----------------------------

        //ゲーム内時間を加算
        shotDelay += Time.deltaTime;

        //弾の発射間隔が閾値以下の場合
        if (shotDelay <= shotThreshold)
        {
            //処理を中断
            return;
        }
        //Bボタンが押された場合
        var  Fire = Input.GetAxis("Fire1");
        if (Fire > 0)
        {
            //弾発射
            PlayerShot(firePos);
        }

        //発射間隔のリセット
        shotDelay = shotDelayRest;
    }

    #region PlayerMove
    void InputProcess()
    {
        //入力値を代入
        float horizontalkey = Input.GetAxis("Horizontal");
        float verticalkey = Input.GetAxis("Vertical");

        //移動用ベクトルを正規化
        moveDirection = new Vector2(horizontalkey, verticalkey).normalized;

        //プレイヤーの移動範囲を制限
        MoveClamp();

        //playerRBのvelocityに代入
        playerRB.velocity = moveDirection * moveSpeed;

        //右に動かした場合
        if (horizontalkey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);

            anim.SetBool("run", true);
        }

        /*//左に動かした場合
        else if (horizontalkey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);

            anim.SetBool("run", true);
        }*/

        //それ以外
        else
        {
            anim.SetBool("run", false);
        }
    }
    #endregion
    void MoveClamp()
    {
        //プレイヤーの位置を取得
        playerPos = transform.position;

        //ビューポート座標からワールド座標で画面の最小値・最大値を取得
        screenMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        screenMax = Camera.main.ViewportToWorldPoint(Vector2.one);

        //移動範囲を制限
        playerPos.x = Mathf.Clamp(playerPos.x, screenMin.x + marginX, screenMax.x - marginX);
        playerPos.y = Mathf.Clamp(playerPos.y, screenMin.y + marginY, screenMax.y - marginY);

        //プレイヤーの位置を取得した位置にする(画面範囲内)
        transform.position = playerPos;
    }

    #region PlayerShot
    //プレイヤーの弾の発射
    void PlayerShot(Transform firePos)
    {
        //弾のコピーを生成
        GameObject bulletClone = Instantiate(playerBullet , firePos.position , Quaternion.identity);

        // プレイヤーの向きに合わせて弾の速度を設定
        float shotDirection = transform.localScale.x > 0 ? 1f : -1f; // 向きに応じて方向を決定

        //弾のコピーのRigidbodyへベクトルを与える
        bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(playerShotSpeed * shotDirection, 0);
    }
    #endregion

    #region PlayerDestroy

    //接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵の弾と接触した場合
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            //自身を破壊
            Destroy(gameObject);

            //敵の弾を破壊
            Destroy(collision.gameObject);
        }
    }

    #endregion
    
}
