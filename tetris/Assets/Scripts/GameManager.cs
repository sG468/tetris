using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner; //�X�|�i�[
    Block activeBlock; //�������ꂽ�u���b�N�i�[


    [SerializeField]
    private float dropInterval = 0.25f;//�u���b�N�������Ă����Ԋu�i���ԓI�Ɂj
    float nextdropTimer;//���Ƀu���b�N��������܂ł̎���(Time.time�����̒l���z������u���b�N����i��������)


    Board board; //�{�[�h�̃X�N���v�g���i�[



    //���͎�t�^�C�}�[
    float nextKeyDownTime, nextKeyLeftRightTime, nextKeyRotateTime, nextKeyHardUpTime;

    //���̓C���^�[�o��(�J�`���J�`��������Ă����Ԋu������u�����߂�A����������h�����߂̕ϐ�)
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, nextKeyHardUpInterval;



    //�p�l���̊i�[
    [SerializeField]
    private GameObject gameOverPanel;

    //�Q�[���I�[�o�[����
    bool gameOver;


    void Start()
    {
        //�X�|�i�[�I�u�W�F�N�g���X�|�i�[�ϐ��Ɋi�[����
        spawner = GameObject.FindObjectOfType<Spawner>();

        //�{�[�h��ϐ��Ɋi�[����
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //�^�C�}�[�̏����ݒ�
        nextKeyDownTime = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTime = Time.time + nextKeyRotateInterval;
        nextKeyHardUpTime = Time.time + nextKeyHardUpInterval;

        //�X�|�i�[�N���X����u���b�N�����֐���ǂ�ŕϐ��Ɋi�[����
        if (!activeBlock)
        {
            spawner.SetNextBlock(); 
            activeBlock = spawner.SpawnBlock();
            spawner.SetNextBlock();
        }

        //�Q�[���I�[�o�[�̔�\���ݒ�
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        PlayerInput();      
    }

    //�L�[�̓��͂����m���ău���b�N�𓮂����֐�
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime) //�E�ړ�
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight(); //�E�ɓ�����

            nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //���ړ�
            || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft(); //���ɓ�����

            nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.X) && (Time.time > nextKeyRotateTime)) //�E��]
        {
            activeBlock.RotateRight();
            nextKeyRotateTime = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotateTime)) //����]
        {
            activeBlock.RotateLeft();
            nextKeyRotateTime = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight();
            }
        }
        else if (Input.GetKey(KeyCode.W) && (Time.time > nextKeyHardUpTime)) //�n�[�h�h���b�v
        {
            nextKeyHardUpTime = Time.time + nextKeyHardUpInterval;
            HardDrop();
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTime) //��{�I�ɂ��������s�����
            || (Time.time > nextdropTimer))
        {
            activeBlock.MoveDown(); //���ɓ�����

            nextKeyDownTime = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                if (board.OverGrid(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    //��ɒ��������̏���
                    BottomBoard();
                }
            }
        }       
    }

    //�{�[�h�̒�ɒ��������Ɏ��̃u���b�N�𐶐�����֐�
    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();
        spawner.SetNextBlock();

        nextKeyDownTime = Time.time;
        nextKeyLeftRightTime = Time.time;
        nextKeyRotateTime = Time.time;

        board.ClearAllRows();//�u���b�N�����n�����邽�тɁA���܂��Ă��Ȃ����m�F�B���܂��Ă���΍폜����
    }

    //�Q�[���I�[�o�[�ɂȂ�����p�l����\������
    void GameOver()
    {
        activeBlock.MoveUp();

        //�Q�[���I�[�o�[�p�l���̔�\���ݒ�
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //�V�[�����ēǂݍ��݂���i�{�^�������ŌĂԁj
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //W�{�^���������ꂽ�Ƃ��ɁA�n�[�h�h���b�v���N����
    public void HardDrop()
    {
        do
        {
            activeBlock.MoveDown();
        } while (board.CheckPosition(activeBlock));

        BottomBoard();
    }
}
