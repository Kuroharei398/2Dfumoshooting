using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWallController : MonoBehaviour
{
    //�ڐG����
    void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ȊO���ڐG�����ꍇ
        if (!collision.gameObject.CompareTag("Player"))
        {
            //�ڐG�����I�u�W�F�N�g��j��
            Destroy(collision.gameObject);
        }
    }
}
