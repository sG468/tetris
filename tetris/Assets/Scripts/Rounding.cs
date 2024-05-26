using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rounding //四捨五入のためのクラスを作る
{
    //positionの1cm分がテトリミノのブロック一つ分であるため、四捨五入をすれば二次元配列の要素番号になるため管理しやすい。
    public static Vector2 Round(Vector2 i)
    {
        return new Vector2(Mathf.Round(i.x), Mathf.Round(i.y));
    }

    public static Vector3 Round(Vector3 i)
    {
        return new Vector3(Mathf.Round(i.x), Mathf.Round(i.y));
    }
}
