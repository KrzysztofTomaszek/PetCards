using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    PostProcessVolume post;
    [SerializeField] PostProcessProfile low;
    [SerializeField] PostProcessProfile medium;
    [SerializeField] PostProcessProfile high;
    void Start()
    {
        post = gameObject.GetComponent<PostProcessVolume>();
        if (QualitySettings.GetQualityLevel() == 0)
        {
            post.profile = low;
        }
        else if (QualitySettings.GetQualityLevel() == 1)
        {
            post.profile = medium;
        }
        else if (QualitySettings.GetQualityLevel() == 2)
        {
            post.profile = high;
        }
    }

}
