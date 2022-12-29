using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MelonLoader;

namespace MyBhapticsTactsuit
{
    public class TactsuitVR
    {
        public bool suitDisabled = true;
        public bool systemInitialized = false;
        private static ManualResetEvent HeartBeat_mrse = new ManualResetEvent(false);
        private static ManualResetEvent NeckTingle_mrse = new ManualResetEvent(false);
        public Dictionary<String, FileInfo> FeedbackMap = new Dictionary<String, FileInfo>();


        public void HeartBeatFunc()
        {
            while (true)
            {
                HeartBeat_mrse.WaitOne();
                bHapticsLib.bHapticsManager.PlayRegistered("HeartBeat");
                Thread.Sleep(1000);
            }
        }

        public void NeckTingleFunc()
        {
            while (true)
            {
                NeckTingle_mrse.WaitOne();
                bHapticsLib.bHapticsManager.PlayRegistered("NeckTingleShort");
                Thread.Sleep(2050);
            }
        }

        public TactsuitVR()
        {
            LOG("Initializing suit");
            suitDisabled = false;
            RegisterAllTactFiles();
            LOG("Starting HeartBeat and NeckTingle thread...");
            Thread HeartBeatThread = new Thread(HeartBeatFunc);
            HeartBeatThread.Start();
            Thread NeckTingleThread = new Thread(NeckTingleFunc);
            NeckTingleThread.Start();
        }

        public void LOG(string logStr)
        {
            MelonLogger.Msg(logStr);
        }



        void RegisterAllTactFiles()
        {
            string configPath = Directory.GetCurrentDirectory() + "\\Mods\\bHaptics";
            DirectoryInfo d = new DirectoryInfo(configPath);
            FileInfo[] Files = d.GetFiles("*.tact", SearchOption.AllDirectories);
            for (int i = 0; i < Files.Length; i++)
            {
                string filename = Files[i].Name;
                string fullName = Files[i].FullName;
                string prefix = Path.GetFileNameWithoutExtension(filename);
                // LOG("Trying to register: " + prefix + " " + fullName);
                if (filename == "." || filename == "..")
                    continue;
                string tactFileStr = File.ReadAllText(fullName);
                try
                {
                    bHapticsLib.bHapticsManager.RegisterPatternFromJson(prefix, tactFileStr);
                    LOG("Pattern registered: " + prefix);
                }
                catch (Exception e) { LOG(e.ToString()); }

                FeedbackMap.Add(prefix, Files[i]);
            }
            systemInitialized = true;
            //PlaybackHaptics("HeartBeat");
        }

        public void PlaybackHaptics(String key, float intensity = 1.0f, float duration = 1.0f)
        {
            if (FeedbackMap.ContainsKey(key))
            {
                if ((intensity != 1.0f)|(duration != 1.0f))
                {
                    bHapticsLib.ScaleOption scaleOption = new bHapticsLib.ScaleOption(intensity, duration);
                    //float locationAngle = 0.0f;
                    //float locationHeight = 0.0f;
                    //bHapticsLib.RotationOption rotationOption = new bHapticsLib.RotationOption(locationAngle, locationHeight);
                    bHapticsLib.bHapticsManager.PlayRegistered(key, key, scaleOption);
                }
                
                // LOG("Playing back: " + key);
                bHapticsLib.bHapticsManager.PlayRegistered(key);
            }
            else
            {
                LOG("Feedback not registered: " + key);
            }
        }

        public void StartHeartBeat()
        {
            HeartBeat_mrse.Set();
        }

        public void StopHeartBeat()
        {
            HeartBeat_mrse.Reset();
        }

        public void StartNeckTingle()
        {
            NeckTingle_mrse.Set();
        }

        public void StopNeckTingle()
        {
            NeckTingle_mrse.Reset();
        }

        public void StopHapticFeedback(String effect)
        {
            bHapticsLib.bHapticsManager.StopPlaying(effect);
        }

        public void StopAllHapticFeedback()
        {
            StopHeartBeat();
            StopNeckTingle();
            foreach (String key in FeedbackMap.Keys)
            {
                bHapticsLib.bHapticsManager.StopPlaying(key);
            }
        }


    }
}
