using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner; //�X�|�i�[
    Block activeBlock; //�������ꂽ�u���b�N�i�[

    [SerializeField]
    private float dropInterval = 0.25f;//���Ƀu���b�N��������܂ł̃C���^�[�o������
    float nextdropTimer;//���Ƀu���b�N��������܂ł̎���


    Board board; //�{�[�h�̃X�N���v�g���i�[

    //���͎�t�^�C�}�[
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    //���̓C���^�[�o��
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    //�p�l���̊i�[
    [SerializeField]
    private GameObject gameOverPanel;

    //�Q�[���I�[�o�[����
    bool gameOver;

    // Start is called before the first frame update

    void Start()
    {
        //�X�|�i�[�I�u�W�F�N�g���X�|�i�[�ϐ��Ɋi�[����
        spawner = GameObject.FindObjectOfType<Spawner>();

        //�{�[�h��ϐ��Ɋi�[����
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //�^�C�}�[�̏����ݒ�
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //�X�|�i�[�N���X����u���b�N�����֐���ǂ�ŕϐ��Ɋi�[����
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
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

        ////Update�Ŏ��Ԃ̔�������Ĕ��莟��ŗ����֐����Ă�
        //if(Time.time > nextdropTimer)
        //{
        //    nextdropTimer = Time.time + dropInterval;

        //    if (activeBlock)
        //    {
        //        activeBlock.MoveDown();

        //        //Update��Board�N���X�̊֐����Ăяo���ă{�[�h����o�Ă��Ȃ����m�F
        //        if (!board.CheckPosition(activeBlock))
        //        {
        //            activeBlock.MoveUp();

        //            board.SaveBlockInGrid(activeBlock);

        //            activeBlock = spawner.SpawnBlock();
        //        }
        //    }
        //}
        
    }

    //�L�[�̓��͂����m���ău���b�N�𓮂����֐�
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight(); //�E�ɓ�����

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft(); //���ɓ�����

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer)
            || (Time.time > nextdropTimer)) 
        {
            activeBlock.MoveDown(); //���ɓ�����

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                if (board.OverLimit(activeBlock))
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

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();//���܂��Ă���΍폜����
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
}
