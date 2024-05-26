using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //�ړ��֐�
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //�ړ��֐����ĂԊ֐�
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    //��]�p
    public void RotateRight()
    {
        transform.Rotate(0, 0, -90);
    }

    public void RotateLeft()
    {
        transform.Rotate(0, 0, 90);
    }
}
