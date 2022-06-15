using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class MultiImageTracking : MonoBehaviour
    {
        public Text toggleText;//按钮title
        ARTrackedImageManager ImgTrackedManager;
        private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        private void Awake()
        {
            ImgTrackedManager = GetComponent<ARTrackedImageManager>();
        }

        void Start()
        {
            //加载预制件
            GameObject one = Resources.Load<GameObject>("Prefabs/One");
            prefabs.Add("One", one);
            GameObject two= Resources.Load<GameObject>("Prefabs/Two");
            prefabs.Add("Two", two);

            GameObject qrcode = Resources.Load<GameObject>("Prefabs/QRCode");
            prefabs.Add("QRCode", qrcode);

            Logger.Log("MultiImageTracking-Start");
        }

        private void OnEnable()
        {
            ImgTrackedManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        void OnDisable()
        {
            ImgTrackedManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        #region 启用与禁用图像跟踪
        public void ToggleImageTracking()
        {
            ImgTrackedManager.enabled = !ImgTrackedManager.enabled;

            string text = "Enable Tracked Image";
            if (ImgTrackedManager.enabled)
            {
                text = "Disable Tracked Image";
                SetAllImagesActive(true);
            }
            else
            {
                text = "Enable Tracked Image";
                SetAllImagesActive(false);
            }

            if (ImgTrackedManager != null)
                toggleText.text = text;
        }

        void SetAllImagesActive(bool value)
        {
            foreach (var img in ImgTrackedManager.trackables)
                img.gameObject.SetActive(value);
        }
    #endregion

        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                // Give the initial image a reasonable default scale
                var minLocalScalar = Mathf.Min(trackedImage.size.x, trackedImage.size.y) / 2;
                trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);//对模型缩放
                Instantiate(prefabs[trackedImage.referenceImage.name], trackedImage.transform);//实例化预制件

                //OnImagesChanged(trackedImage);
            }            
        }

        private void OnImagesChanged(ARTrackedImage referenceImage)
        {
            //Debug.Log("Image name:" + referenceImage.referenceImage.name);
            Logger.Log("OnImagesChanged:" + referenceImage.referenceImage.name);
            Instantiate(prefabs[referenceImage.referenceImage.name], referenceImage.transform);

        }

        public void ClearInstance()
        {
            Logger.Log("clearInstance");

            foreach (var key in prefabs.Keys)
            {
                Destroy(prefabs[key]);
                prefabs.Remove(key);
            }

        }
    }
}