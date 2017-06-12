using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    public enum PlayMode
    {
        LoopPlay, SinglePlay, SequentialPlay, RandomPlay
    }

    public enum PlayState { 无文件, 无列表, 播放, 暂停, 停止 }
}
