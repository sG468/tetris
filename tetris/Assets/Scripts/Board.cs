using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //�񎟌��z��̍쐬
    private Transform[,] grid;

    [SerializeField]
    private int height = 30, width = 10, header = 10;

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    //�������u����ꏊ���ǂ����m�F����֐�
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            //�g����͂ݏo�Ă��Ȃ���
            if (!BoardOutCheck((int)pos.x, (int)pos.y)) 
            {
                return false;
            }

            //���̃u���b�N�����ɒu����Ă��Ȃ���
            if (BlockCheck((int)pos.x, (int)pos.y, block)) 
            {
                return false;
            }
        }

        return true;
    }

    //�g���ɂ���̂����肷��֐�
    bool BoardOutCheck(int x, int y)
    {
        //x����0�ȏ�width�����Ay����0�ȏ�
        return (x >= 0 && x < width && y >= 0);
    }

    //�ړ���Ƀu���b�N���Ȃ������肷��֐�
    bool BlockCheck(int x, int y, Block block)
    {
        //�񎟌��z�񂪋�łȂ��̂́A���̃u���b�N�����鎞
        //�e���Ⴄ�̂́A���̃u���b�N�����鎞
        return (grid[x, y] != null && grid[x, y].parent != block.transform);
    }
    
    //�u���b�N���������ꏊ���L�^����֐�
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //�S�Ă̍s���`�F�b�N���āA���܂��Ă���΍폜����֐�
    public void ClearAllRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y)) //�������ă`�F�b�N
            {
                DeleteRow(y); //���̍s������

                RowsDown(y + 1); //���낷

                y--;
            }
        }
    }

    //�S�Ă̍s���`�F�b�N����֐�
    bool IsComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    //�폜����֐�
    void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //��ɂ���u���b�N����i������֐�
    void RowsDown(int upperY)
    {
        for (int y = upperY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y]; //�V���v���ɏ�̒i�̏������̒i�ɑ��
                    grid[x,y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0); //��̒i��Transform�����̂܂ܓ����Ă���́Aposition����̒i�ɂȂ��Ă��܂��Ă���̂ŁA��i������B
                }
            }
        }
    }

    //�u���b�N����g����͂ݏo�����`�F�b�N����֐�
    public bool OverGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            if (item.transform.position.y >= height - header)
            {
                return true;
            }
        }

        return false;
    }

    
}
