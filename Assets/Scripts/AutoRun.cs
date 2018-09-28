using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoRun : MonoBehaviour {

    /// <summary>
    /// 車と同時に操作するメインカメラ
    /// </summary>
    public GameObject mainCamera;

    /// <summary>
    /// ゲームコントローラー
    /// </summary>
    public GameObject gameController;

    /// <summary>
    /// タイトルロゴの表示
    /// </summary>
    public GameObject titleCanvas;

    /// <summary>
    /// ゲームフラグ
    /// </summary>
    public GameObject gameFlags;

    /// <summary>
    /// フェードアウト用の真っ黒パネル
    /// </summary>
    public GameObject panelFadeout;

    Vector3 cameraBeginPosition;
    float offsetY;
    float offsetZ;
    float startTime;
    float fadeDeltaTime = 0f;
    float fadeOutSeconds = 1.0f;
    float musicStartTime;
    int creditCounter = 0;
    GameObject[] credits;

	// Use this for initialization
	void Start () {
        cameraBeginPosition = mainCamera.transform.position;
        offsetY = mainCamera.transform.position.y - transform.position.y;
        offsetZ = mainCamera.transform.position.z - transform.position.z;
        startTime = Time.time;
        credits = new GameObject[7];
        for (int i = 0; i < credits.Length; i++)
        {
            credits[i] = GameObject.Find("Credit_" + i.ToString());
            credits[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        cameraBeginPosition = mainCamera.transform.position;

        // 走り出して5秒後にカメラを切り替え
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(0) == false && Time.time - startTime > 5.0f)
        {
            cameraBeginPosition.x = 0.45f;
            offsetY = 1.49f;
            mainCamera.transform.rotation = new Quaternion(0f, 161.876f, 0f, 0f);

            gameFlags.GetComponent<GameFlag>().SetGameFlag(0, true);
        }

        // 走り出して10秒後に文章を表示する
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(1) == false && Time.time - startTime > 10.0f)
        {
            gameController.SetActive(true);
            gameFlags.GetComponent<GameFlag>().SetGameFlag(1, true);
        }

        // 文章を表示し終わった後、カントリーミュージックを再生する
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(2) == true 
            && ScenarioManager.Instance.IsActiveCanvas == false
            && gameFlags.GetComponent<GameFlag>().GetGameFlag(3) == false)
        {
            gameFlags.GetComponent<GameFlag>().SetGameFlag(3, true);
            GetComponents<AudioSource>()[1].Play();
            musicStartTime = Time.time;
        }

        // 走行音をフェードアウトして停止
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(3) == true && GetComponent<AudioSource>().isPlaying)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime >= fadeOutSeconds)
            {
                fadeDeltaTime = fadeOutSeconds;
                GetComponent<AudioSource>().Stop();
            }
            GetComponent<AudioSource>().volume = (float)(1.0 - fadeDeltaTime / fadeOutSeconds) * 1f;
        }

        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(3) == true)
        {
            // 音楽開始から8秒後にクレジット1を表示
            if (Time.time - musicStartTime > 8.0f && creditCounter == 0)
            {
                creditCounter++;
                credits[6].SetActive(true);
            }

            // 音楽開始から13秒後にクレジット2を表示
            if (Time.time - musicStartTime > 13.0f && creditCounter == 1)
            {
                creditCounter++;
                credits[6].SetActive(false);
                credits[5].SetActive(true);
            }

            // 音楽開始から18秒後にクレジット3を表示
            if (Time.time - musicStartTime > 18.0f && creditCounter == 2)
            {
                creditCounter++;
                credits[5].SetActive(false);
                credits[4].SetActive(true);
            }

            // 音楽開始から23秒後にクレジット4を表示
            if (Time.time - musicStartTime > 23.0f && creditCounter == 3)
            {
                creditCounter++;
                credits[4].SetActive(false);
                credits[3].SetActive(true);
            }

            // 音楽開始から28秒後にクレジット5を表示
            if (Time.time - musicStartTime > 28.0f && creditCounter == 4)
            {
                creditCounter++;
                credits[3].SetActive(false);
                credits[2].SetActive(true);
            }

            // 音楽開始から33秒後にクレジット6を表示
            if (Time.time - musicStartTime > 33.0f && creditCounter == 5)
            {
                creditCounter++;
                credits[2].SetActive(false);
                credits[1].SetActive(true);
            }

            // 音楽開始から38秒後にクレジット7を表示
            if (Time.time - musicStartTime > 38.0f && creditCounter == 6)
            {
                creditCounter++;
                credits[1].SetActive(false);
                credits[0].SetActive(true);
            }

            // 音楽開始から43秒後にタイトルを表示
            if (Time.time - musicStartTime > 43.0f && creditCounter == 7)
            {
                creditCounter++;
                credits[0].SetActive(false);
                titleCanvas.SetActive(true);
                fadeDeltaTime = 0;
                fadeOutSeconds = 5;
            }

            // 音楽開始から55秒あたりで音楽と画面をフェードアウト
            if (Time.time - musicStartTime > 55.0f && creditCounter > 7)
            {
                fadeDeltaTime += Time.deltaTime;
                if (fadeDeltaTime >= fadeOutSeconds)
                {
                    fadeDeltaTime = fadeOutSeconds;
                    GetComponents<AudioSource>()[1].Stop();
                    panelFadeout.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                    gameFlags.GetComponent<GameFlag>().SetGameFlag(4, true);

                }
                GetComponents<AudioSource>()[1].volume = (float)(1.0 - fadeDeltaTime / fadeOutSeconds) * 1f;
                Color c = panelFadeout.gameObject.GetComponent<Image>().color;
                panelFadeout.gameObject.GetComponent<Image>().color = new Color(c.r, c.g, c.b, (float)(fadeDeltaTime / fadeOutSeconds) * 1f);
            }
        }

        // 音楽も止まり画面も真っ暗になったらシーンを遷移する
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(4) == true)
        {
            SceneManager.LoadScene("01_BaseScene");
        }

        MoveCar();

        MoveCamera();
    }

    /// <summary>
    /// 車の移動処理
    /// </summary>
    private void MoveCar()
    {
        Vector3 beginPosition = transform.position;
        beginPosition.x = 1.273524f;
        beginPosition.z += 1;

        // 砂漠を超えそうになったら移動
        if (beginPosition.z > 3500f)
        {
            beginPosition.y = 5f;
            beginPosition.z = 1500f;
        }

        transform.position = beginPosition;

        // 車の揺れや転倒を防ぐ
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    /// <summary>
    /// カメラの移動処理
    /// </summary>
    private void MoveCamera()
    {
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(0) == false)
        {
            cameraBeginPosition.z += 1f;
        }
        if (gameFlags.GetComponent<GameFlag>().GetGameFlag(0) == true)
        {
            cameraBeginPosition.z = transform.position.z + 4.37f;
        }
        cameraBeginPosition.y = transform.position.y + offsetY;
        mainCamera.transform.position = cameraBeginPosition;
    }
}
