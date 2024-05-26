using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner; //スポナー
    Block activeBlock; //生成されたブロック格納


    [SerializeField]
    private float dropInterval = 0.25f;//ブロックが落ちていく間隔（時間的に）
    float nextdropTimer;//次にブロックが落ちるまでの時間(Time.timeがこの値を越えたらブロックが一段分落ちる)


    Board board; //ボードのスクリプトを格納



    //入力受付タイマー
    float nextKeyDownTime, nextKeyLeftRightTime, nextKeyRotateTime, nextKeyHardUpTime;

    //入力インターバル(カチャカチャ押されても一定間隔処理を置くためや、同時押下を防ぐための変数)
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, nextKeyHardUpInterval;



    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    //ゲームオーバー判定
    bool gameOver;


    void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納する
        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTime = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTime = Time.time + nextKeyRotateInterval;
        nextKeyHardUpTime = Time.time + nextKeyHardUpInterval;

        //スポナークラスからブロック生成関数を読んで変数に格納する
        if (!activeBlock)
        {
            spawner.SetNextBlock(); 
            activeBlock = spawner.SpawnBlock();
            spawner.SetNextBlock();
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
    }

    //キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime) //右移動
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight(); //右に動かす

            nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //左移動
            || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft(); //左に動かす

            nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.X) && (Time.time > nextKeyRotateTime)) //右回転
        {
            activeBlock.RotateRight();
            nextKeyRotateTime = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotateTime)) //左回転
        {
            activeBlock.RotateLeft();
            nextKeyRotateTime = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight();
            }
        }
        else if (Input.GetKey(KeyCode.W) && (Time.time > nextKeyHardUpTime)) //ハードドロップ
        {
            nextKeyHardUpTime = Time.time + nextKeyHardUpInterval;
            HardDrop();
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTime) //基本的にここが実行される
            || (Time.time > nextdropTimer))
        {
            activeBlock.MoveDown(); //下に動かす

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
        spawner.SetNextBlock();

        nextKeyDownTime = Time.time;
        nextKeyLeftRightTime = Time.time;
        nextKeyRotateTime = Time.time;

        board.ClearAllRows();//ブロックが着地をするたびに、埋まっていないか確認。埋まっていれば削除する
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

    //Wボタンが押されたときに、ハードドロップを起こす
    public void HardDrop()
    {
        do
        {
            activeBlock.MoveDown();
        } while (board.CheckPosition(activeBlock));

        BottomBoard();
    }
}
