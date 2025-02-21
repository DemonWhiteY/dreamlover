using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingPanel : MonoBehaviour
{
    public Slider loadingBar;  // 进度条 UI 元素
    public Text loadingText;   // 进度百分比显示（可选）
    public float activationDuration = 3.0f;  // 模拟场景激活所需时间

    void Start()
    {
        // 开始异步加载新场景
        StartCoroutine(LoadSceneAsync("Rico"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 开始加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // 禁用自动场景激活，等到加载完成后再激活
        asyncOperation.allowSceneActivation = false;

        // 显示加载进度
        while (!asyncOperation.isDone)
        {
            // 0.9f 代表加载的实际进度，最大值是 0.9f，因为最后的 10% 是场景激活时间
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.3f);
            loadingBar.value = progress;

            // 更新进度百分比显示（可选）
            if (loadingText != null)
            {
                loadingText.text = (progress * 100f).ToString("F0") + "%";
            }

            // 检查是否加载完成
            if (asyncOperation.progress >= 0.3f)
            {
                // 模拟场景激活的过渡时间
                yield return StartCoroutine(SimulateActivationProgress());

                // 激活场景
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator SimulateActivationProgress()
    {
        float elapsed = 0f;

        // 模拟激活进度
        while (elapsed < activationDuration)
        {
            elapsed += Time.deltaTime;
            float activationProgress = Mathf.Clamp01(elapsed / activationDuration);

            // 更新进度条和进度百分比
            loadingBar.value = 0.3f + (activationProgress * 0.7f);
            if (loadingText != null)
            {
                loadingText.text = "Loading:" + ((loadingBar.value) * 99f).ToString("F0") + "%";
            }

            yield return null;
        }
    }
}
