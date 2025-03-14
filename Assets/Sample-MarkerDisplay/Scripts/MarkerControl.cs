using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YVR.Core;

namespace YVR.Enterprise.LBE.Samples.MarkerDisplay
{
    public class MarkerControl : MonoBehaviour
    {
        public GameObject markerPrefab;
        private Dictionary<long, GameObject> m_MarkerID2GODict = new();
        public TextMeshPro statisticDisplayText = null;

        private void Start()
        {
            YVRManager.instance.hmdManager.SetPassthrough(true);

            LBEPlugin.instance.SetMarkerDetectionEnable(true);
            LBEPlugin.instance.SetMarkerTrackingUpdateCallback(OnReceiveMarkerTrackingUpdateData);
        }

        private void OnReceiveMarkerTrackingUpdateData(MarkerTrackingUpdateData data)
        {
            if (!m_MarkerID2GODict.TryGetValue(data.markerId, out GameObject markerGO))
            {
                markerGO = Instantiate(markerPrefab);
                m_MarkerID2GODict.Add(data.markerId, markerGO);
            }

            markerGO.transform.SetPositionAndRotation(data.markerPose.position, data.markerPose.orientation);
            markerGO.GetComponentInChildren<TextMeshPro>().text
                = $"Marker: {data.markerId} \n Confidence: {data.confidence}";

            statisticDisplayText.text = $"Marker Count is {m_MarkerID2GODict.Count}";
        }
    }
}