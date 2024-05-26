using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Block[] blocks;

    [SerializeField]
    private Transform nextBlockPosition; //��ʉE��̎��̃u���b�N�^�u�̍��W

    private Block nextBlock; //���̃u���b�N�̏�������ϐ�

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

    //�I�΂ꂽ�u���b�N��Spawner�ɃZ�b�g����֐�
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

    //�����_���Ő��������u���b�N���A�E��̏����E�B���h�E�ɕ\������֐�
    public void SetNextBlock()
    {
        nextBlock = Instantiate(GetRandomBlock(), nextBlockPosition.position, Quaternion.identity);
    }

}
