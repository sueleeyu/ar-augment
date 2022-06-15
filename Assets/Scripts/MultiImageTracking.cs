using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class MultiImageTracking : MonoBehaviour
    {
        public Text toggleText;//��ťtitle
        ARTrackedImageManager ImgTrackedManager;
        private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        private void Awake()
        {
            ImgTrackedManager = GetComponent<ARTrackedImageManager>();
        }

        void Start()
        {
            //����Ԥ�Ƽ�
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

        #region ���������ͼ�����
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
                trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);//��ģ������
                Instantiate(prefabs[trackedImage.referenceImage.name], trackedImage.transform);//ʵ����Ԥ�Ƽ�

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