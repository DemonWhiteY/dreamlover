using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController 
{
    private static EmotionController instance;

    public static EmotionController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EmotionController();
            }
            return instance;
        }
    }

    private EmotionController()
    {
        // 私有构造函数，确保无法从外部实例化
    }
    public void updateEmotion(int emotion)
    {
        Camera mainCamera = Camera.main;
        AnimatorController animatorController= mainCamera.GetComponent<AnimatorController>();


        switch (emotion)
        {
            case 1:
                animatorController.SetTrigger("emo1");
                animatorController.SetInt("animation,15");
                
                break;
            case 2:
                animatorController.SetTrigger("emo1");
                animatorController.SetInt("animation,2");
                break;
            case 3:
                animatorController.SetTrigger("emo2");
                animatorController.SetInt("animation,2");
                break;
            case 4:
                animatorController.SetTrigger("emo5");
                animatorController.SetInt("animation,3");
                break;
            case 5:
                animatorController.SetTrigger("emo4");
                animatorController.SetInt("animation,3");
                break;
            case 6:
                animatorController.SetTrigger("emo5");
                animatorController.SetInt("animation,3");
                break;
            case 7:
                animatorController.SetTrigger("emo6");
                animatorController.SetInt("animation,18");
                break;
            case 8:
                animatorController.SetTrigger("emo7");
                animatorController.SetInt("animation,18");
                break;
            case 9:
                animatorController.SetTrigger("emo7");
                animatorController.SetInt("animation,17");
                break;
            case 10:
                animatorController.SetTrigger("emo8");
                animatorController.SetInt("animation,2");
                break;
            case 11:
                animatorController.SetTrigger("emo1");
                animatorController.SetInt("animation,13");
                break;
            case 12:
                animatorController.SetInt("animation,16");
                
                break;
            case 13:
                animatorController.SetInt("animation,8");

                break;
            case 14:
                animatorController.SetInt("animation,11");

                break;

            default:
                break;
        }
    }

   
}
