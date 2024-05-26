using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //二次元配列の作成
    private Transform[,] grid;

    [SerializeField]
    private int height = 30, width = 10, header = 10;

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    //そこが置ける場所かどうか確認する関数
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            //枠からはみ出ていないか
            if (!BoardOutCheck((int)pos.x, (int)pos.y)) 
            {
                return false;
            }

            //他のブロックが既に置かれていないか
            if (BlockCheck((int)pos.x, (int)pos.y, block)) 
            {
                return false;
            }
        }

        return true;
    }

    //枠内にあるのか判定する関数
    bool BoardOutCheck(int x, int y)
    {
        //x軸は0以上width未満、y軸は0以上
        return (x >= 0 && x < width && y >= 0);
    }

    //移動先にブロックがないか判定する関数
    bool BlockCheck(int x, int y, Block block)
    {
        //二次元配列が空でないのは、他のブリックがある時
        //親が違うのは、他のブロックがある時
        return (grid[x, y] != null && grid[x, y].parent != block.transform);
    }
    
    //ブロックが落ちた場所を記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //全ての行をチェックして、埋まっていれば削除する関数
    public void ClearAllRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y)) //横を見てチェック
            {
                DeleteRow(y); //その行を消す

                RowsDown(y + 1); //下ろす

                y--;
            }
        }
    }

    //全ての行をチェックする関数
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

    //削除する関数
    void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //上にあるブロックを一段下げる関数
    void RowsDown(int upperY)
    {
        for (int y = upperY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y]; //シンプルに上の段の情報を下の段に代入
                    grid[x,y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0); //上の段のTransformがそのまま入っている故、positionが上の段になってしまっているので、一段下げる。
                }
            }
        }
    }

    //ブロックが上枠からはみ出たかチェックする関数
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
