using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;


//using System.Numerics;
using UnityEngine;

using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    public float speed = 12.0f; //母球 初速
    public float forceSpd = 9.0f;
    private float force = 0.0f; //母球 已經蓄了多少力 的大小
    public float distance = 14.14f; //攝影機 離 母球 距離 初始值
    public float xSpeed = 120.0f; //滑鼠左右移動速度
    public float ySpeed = 120.0f; //滑鼠上下移動速度
    public float yMinLimit = -20f;  //滑鼠上下 轉仰角 下限
    public float yMaxLimit = 80f;   //滑鼠上下 轉仰角 上限
    public float distanceMin = .5f;  //滾輪 拉 攝影機 離 母球 距離下限
    public float distanceMax = 15f;  //滾輪 拉 攝影機 離 母球 距離上限
    private Rigidbody rbody;
    float x = 0.0f;
    float y = 0.0f;
    public int maxforce = 12;

    [Range(0, 100)]
    public float fillValue = 0;
    public Image circleFillImage;
    public RectTransform handlerEdgeImage;
    public RectTransform fillHandler;
    public Image stare;
    public GameObject audio1;
    public GameObject cue;


    void Start()
    {
        offset = transform.position - player.transform.position;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rbody = player.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        if (Input.GetMouseButton(0)) //按住時 才有作用
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);  // 限制 仰角 傾仰範圍
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            offset = rotation * negDistance; //依新角度，距離 重新算相對位置
            transform.rotation = rotation; // 攝影機 新角度
        }
        // 攝影機新位置 = 新相對位置 + 母球位置
        transform.position = player.transform.position + offset;
        if (Input.GetMouseButton(1))  // 按滑鼠右鍵 按住 蓄力
        {
            force += Time.deltaTime * forceSpd; // 大小和時間成正比
            if (force >= maxforce)
            {
                force = maxforce;
            }
            FillCircleValue(force , maxforce);
            cue.transform.position = player.transform.position + new Vector3(offset.x * 1.1f, offset.y * 1.1f, offset.z * 1.1f);
            cue.transform.rotation = transform.rotation;
        }
        else if (Input.GetMouseButtonUp(1))  // 按滑鼠右鍵 放開 發射
        {
            //眼睛看的方向the direction of camera(eye)：
            // Camera.main.transform.forward
            cue.transform.position = transform.position;
            cue.transform.rotation = transform.rotation;
            GameObject clone1 = Instantiate(audio1, player.transform.position, player.transform.rotation);
            clone1.SetActive(true);
            Vector3 movement = Camera.main.transform.forward;
            movement.y = 0.0f;      // no vertical movement 不上下移動
                                    //力量模式impulse:衝力，speed：初速大小
            rbody.AddForce(movement * speed * force, ForceMode.Impulse);
            force = 0.0f;  // 力量用盡歸0，準備下次重新蓄力
        }

    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        if (angle > 360F)
        {
            angle -= 360F;
        }
            return Mathf.Clamp(angle, min, max);
        

    }
    void FillCircleValue(float value,float maxvalue)
    {
        
        float fillAmount = (value / maxvalue);
        circleFillImage.fillAmount = fillAmount;
        float angle = fillAmount * 360;
        stare.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        //double x = Math.Sin(angle * Math.PI / 180.0) * (double)-32.52; 
        //double y = Math.Cos(angle * Math.PI / 180.0) * (double)-32.52;
        float angleX = Mathf.Sin(angle * Mathf.Deg2Rad) * (float)-32.52;
        float angleY = Mathf.Cos(angle * Mathf.Deg2Rad) * (float)-32.52;
        stare.rectTransform.localPosition = new Vector3(angleX, angleY, 0);
        //handlerEdgeImage.localEulerAngles = new Vector3(0, 0, angle); 


    }
}
