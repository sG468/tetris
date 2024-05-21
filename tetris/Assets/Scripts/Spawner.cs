using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Block[] blocks;

    //�����_���ȃu���b�N��1�I�Ԋ֐�
    Block GetRandomBlock()
    {
        int i = Random.Range(0, blocks.Length); //0�ȏ�7�������琔�l���I�΂��

        if (blocks[i])
        {
            return blocks[i];
        }
        else
        {
            return null;
        }
    }

    //�I�΂ꂽ�u���b�N�𐶐�����֐�
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position,
            Quaternion.identity);

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
