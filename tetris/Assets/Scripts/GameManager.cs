using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner; //スポナー
    Block activeBlock; //生成されたブロック格納

    [SerializeField]
    private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    float nextdropTimer;//次にブロックが落ちるまでの時間


    Board board; //ボードのスクリプトを格納

    //入力受付タイマー
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    //入力インターバル
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    //ゲームオーバー判定
    bool gameOver;

    // Start is called before the first frame update

    void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納する
        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //スポナークラスからブロック生成関数を読んで変数に格納する
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //ゲームオーバーの非表示設定
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

        ////Updateで時間の判定をして判定次第で落下関数を呼ぶ
        //if(Time.time > nextdropTimer)
        //{
        //    nextdropTimer = Time.time + dropInterval;

        //    if (activeBlock)
        //    {
        //        activeBlock.MoveDown();

        //        //UpdateでBoardクラスの関数を呼び出してボードから出ていないか確認
        //        if (!board.CheckPosition(activeBlock))
        //        {
        //            activeBlock.MoveUp();

        //            board.SaveBlockInGrid(activeBlock);

        //            activeBlock = spawner.SpawnBlock();
        //        }
        //    }
        //}
        
    }

    //キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight(); //右に動かす

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft(); //左に動かす

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
            activeBlock.MoveDown(); //下に動かす

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
                    //底に着いた時の処理
                    BottomBoard();
                }   
            }
        }       
    }

    //ボードの底に着いた時に次のブロックを生成する関数
    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();//埋まっていれば削除する
    }

    //ゲームオーバーになったらパネルを表示する
    void GameOver()
    {
        activeBlock.MoveUp();

        //ゲームオーバーパネルの非表示設定
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //シーンを再読み込みする（ボタン押下で呼ぶ）
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
