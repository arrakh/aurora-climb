using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arr.Ink
{
    public class InkAudioController : MonoBehaviour
    {
        [FormerlySerializedAs("inkController")] [SerializeField] private InkStoryController inkStoryController;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip[] clips;

        private Dictionary<string, AudioClip> clipsDict = new();

        private void Awake()
        {
            foreach (var clip in clips)
                clipsDict[clip.name] = clip;
        }

        private void OnEnable() => inkStoryController.OnStoryTags += OnTags;

        private void OnDisable() => inkStoryController.OnStoryTags -= OnTags;

        private void OnTags(List<string> tags)
        {
            foreach (var tag in tags)
            {
                if (!tag.StartsWith("audio")) continue;
                var audio = tag.Replace("audio ", "").Trim();

                if (!clipsDict.TryGetValue(audio, out var clip)) throw new Exception($"COULD NOT GET AUDIO: {audio}");
                audioSource.PlayOneShot(clip);
            }
        }
    }
}