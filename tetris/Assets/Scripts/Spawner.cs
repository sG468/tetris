using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Block[] blocks;

    [SerializeField]
    private Transform nextBlockPosition; //画面右上の次のブロックタブの座標

    private Block nextBlock; //次のブロックの情報を入れる変数

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

    //選ばれたブロックをSpawnerにセットする関数
    public Block SpawnBlock()
    {
        nextBlock.transform.position = transform.position;


        if (nextBlock)
        {
            return nextBlock;
        }
        else
        {
            return null;
        }
    }

    //ランダムで生成したブロックを、右上の小窓ウィンドウに表示する関数
    public void SetNextBlock()
    {
        nextBlock = Instantiate(GetRandomBlock(), nextBlockPosition.position, Quaternion.identity);
    }

}
