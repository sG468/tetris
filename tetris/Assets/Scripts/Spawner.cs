using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Block[] blocks;

    //ランダムなブロックを1つ選ぶ関数
    Block GetRandomBlock()
    {
        int i = Random.Range(0, blocks.Length); //0以上7未満から数値が選ばれる

        if (blocks[i])
        {
            return blocks[i];
        }
        else
        {
            return null;
        }
    }

    //選ばれたブロックを生成する関数
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
