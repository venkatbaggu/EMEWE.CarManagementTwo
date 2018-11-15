using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Speech.Synthesis;
using SpeechLib;
using System.Windows.Forms;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public static class VoiceReade
    {

        public static string readTxt;
        public static int readCount;
        public static void Read()
        {
            try
            {
                CommonalityEntity.isvoic = false;
                PromptBuilder myprompt = new PromptBuilder();//实例化对象构造一个语音定义
                SpeechSynthesizer ss = new SpeechSynthesizer();//实例化合成文字到语音 (TTS) 语音配合使用的类
                ss.Volume = 100;//声音大小
                ss.Rate = Convert.ToInt32(System.Configuration.ConfigurationManager.ConnectionStrings["Rate"].ToString());//速度

                ss.SelectVoice("VW Lily");//设置语音包
                string wavPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Sounds\\ring.wav";//要读的音频文件地址
                if (!CommonalityEntity.ishujiao)
                {
                    myprompt.AppendAudio(wavPath);//将音频文件追加到PromptBuilder
                } 
                myprompt.AppendText(readTxt);//将文本追加到PromptBuilder
                for (int i = 0; i < readCount; i++)
                {
                    ss.Speak(myprompt);//阅读
                }
                CommonalityEntity.isvoic = true;
                ss.Dispose();//销毁
            }
            catch 
            {

                
            }
        }

        /// <summary>
        /// 语音播放
        /// </summary>
        public static void NewRead()
        {
            try
            {
                //读取已有文件
                string wavPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Sounds\\ring.wav";//要读的音频文件地址
                SpVoiceClass pp = new SpVoiceClass();
                SpFileStreamClass spFs = new SpFileStreamClass();
                spFs.Open(wavPath, SpeechStreamFileMode.SSFMOpenForRead, true);
                ISpeechBaseStream Istream = spFs as ISpeechBaseStream;
               
                //文字转语音播放
                SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                SpVoice spVoice = new SpVoice();//声源
                spVoice.Rate = Convert.ToInt32(System.Configuration.ConfigurationManager.ConnectionStrings["Rate"].ToString());//速度
                spVoice.Volume = 100;
                spVoice.WaitUntilDone(-1);
                spVoice.SpeakStream(Istream, spFlags);
                //循环播放
                for (int i = 0; i < readCount; i++)
                {
                    spVoice.WaitUntilDone(-1);
                    spVoice.Speak(readTxt, spFlags);//文字转语音播放
                }
                spFs.Close();
                //直接读取音频文件
                //SoundPlayer soundPlayer = new SoundPlayer();
                //soundPlayer.SoundLocation = wavPath;
                //soundPlayer.Load();
                //soundPlayer.Play();
            }
            catch(Exception err)
            {
                MessageBox.Show("语音播报失败!");
                CommonalityEntity.WriteTextLog(err.ToString());
            }
        }
    }
}
