using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRun : MonoBehaviour {

    /// <summary>
    /// 車と同時に操作するメインカメラ
    /// </summary>
    public GameObject camera;

    /// <summary>
    /// 車と同時に操作する砂煙
    /// </summary>
    public GameObject dustStorm;

    /// <summary>
    /// 車と同時に操作する砂煙2
    /// </summary>
    public GameObject dustStrom2;

    /// <summary>
    /// メッセージウィンドウ
    /// </summary>
    public GameObject canvas;

    /// <summary>
    /// ゲームコントローラー
    /// </summary>
    public GameObject gameController;

    Vector3 beginPosition;
    Vector3 cameraBeginPosition;
    Vector3 dustPosition;
    float offsetHeight;
    float offsetZ;
    float offsetHeightDust;
    float startTime;
    bool[] cameraSwitch;


	// Use this for initialization
	void Start () {
        beginPosition = transform.position;
        cameraBeginPosition = camera.transform.position;
        offsetHeight = camera.transform.position.y - transform.position.y;
        offsetHeightDust = dustStorm.transform.position.y - transform.position.y;
        offsetZ = camera.transform.position.z - transform.position.z;
        startTime = Time.time;
        cameraSwitch = new bool[10];
	}
	
	// Update is called once per frame
	void Update () {

        cameraBeginPosition = camera.transform.position;

        // 走り出して5秒後にカメラを切り替え
        if (cameraSwitch[0] == false && Time.time - startTime > 5.0f)
        {
            cameraBeginPosition.x = 0.45f;
            offsetHeight = 1.49f;
            camera.transform.rotation = new Quaternion(0f, 161.876f, 0f, 0f);

            cameraSwitch[0] = true;
        }

        // 走り出して10秒後に文章を表示する
        if (cameraSwitch[1] == false && Time.time - startTime > 10.0f)
        {
            canvas.SetActive(true);
            gameController.SetActive(true);
            cameraSwitch[1] = true;
        }

        // 車の移動
        beginPosition = transform.position;
        beginPosition.x = 1.273524f;
        beginPosition.z += 1;

        // 砂漠を超えそうになったら移動
        if (beginPosition.z > 3500f)
        {
            beginPosition.y = 5f;
            beginPosition.z = 1500f;
        }

        transform.position = beginPosition;

        // 砂煙の移動処理
        dustPosition = dustStorm.transform.position;
        dustPosition.z += 1;
        dustPosition.y = beginPosition.y + offsetHeightDust;
        dustStorm.transform.position = dustPosition;

        dustPosition = dustStrom2.transform.position;
        dustPosition.z += 1;
        dustPosition.y = beginPosition.y + offsetHeightDust;
        dustStrom2.transform.position = dustPosition;

        // カメラの移動処理
        if (cameraSwitch[0] == false)
        {
            cameraBeginPosition.z += 1f;
        }
        if (cameraSwitch[0] == true)
        {
            cameraBeginPosition.z = transform.position.z + 4.37f;
        }
        cameraBeginPosition.y = beginPosition.y + offsetHeight;
        camera.transform.position = cameraBeginPosition;

        transform.rotation = new Quaternion(0, 0, 0, 0);

    }
}
