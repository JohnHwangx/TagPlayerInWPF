﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    public class PlayingSongOperator
    {
        public bool IsPlayingListChanged { get; set; }
        public List<int> RandomNumList { get; set; }
        public Song GetNextSong(PlayMode playMode, List<Song> playingList, int songIndex)
        {
            switch (playMode)
            {
                case PlayMode.LoopPlay:
                    return songIndex == playingList.Count - 1
                        ? playingList[0]
                        : playingList[++songIndex];
                case PlayMode.SinglePlay:
                    return playingList[songIndex];
                case PlayMode.SequentialPlay:
                    return songIndex == playingList.Count - 1
                        ? null
                        : playingList[++songIndex];
                case PlayMode.RandomPlay:
                    if (IsPlayingListChanged)
                    {
                        GetRandomList(playingList.Count);
                        IsPlayingListChanged = false;
                    }
                    var randomNum = RandomNumList.IndexOf(songIndex);
                    return randomNum == playingList.Count - 1
                        ? playingList[0]
                        : playingList[++randomNum];
            }
            return null;
        }

        public Song OnNextExecute(PlayMode playMode, List<Song> playingList, int songIndex)
        {
            switch (playMode)
            {
                case PlayMode.LoopPlay:
                case PlayMode.SinglePlay:
                case PlayMode.SequentialPlay:
                    return songIndex == playingList.Count - 1
                        ? playingList[0]
                        : playingList[++songIndex];
                case PlayMode.RandomPlay:
                    if (IsPlayingListChanged)
                    {
                        GetRandomList(playingList.Count);
                        IsPlayingListChanged = false;
                    }
                    var randomNum = RandomNumList.IndexOf(songIndex);
                    return randomNum == playingList.Count - 1
                        ? playingList[RandomNumList[0]]
                        : playingList[RandomNumList[++randomNum]];
                default:
                    throw new ArgumentOutOfRangeException(nameof(playMode), playMode, null);
            }
        }

        public Song OnLastExecute(PlayMode playMode, List<Song> playingList, int songIndex)
        {
            switch (playMode)
            {
                case PlayMode.LoopPlay:
                case PlayMode.SinglePlay:
                case PlayMode.SequentialPlay:
                    return songIndex == 0 ? playingList[playingList.Count - 1] : playingList[--songIndex];
                case PlayMode.RandomPlay:
                    if (IsPlayingListChanged)
                    {
                        GetRandomList(playingList.Count);
                        IsPlayingListChanged = false;
                    }
                    var randomNum = RandomNumList.IndexOf(songIndex);
                    return randomNum == 0
                        ? playingList[RandomNumList[playingList.Count - 1]]
                        : playingList[RandomNumList[--randomNum]];
                default:
                    throw new ArgumentOutOfRangeException(nameof(playMode), playMode, null);
            }
        }

        public void GetRandomList(int num)
        {
            var randomNumList = new int[num];
            for (int i = 0; i < num; i++)
            {
                randomNumList[i] = i;
            }

            Random randomNumbers = new Random();
            for (int i = 0; i < num; i++)
            {
                int randomNum = randomNumbers.Next(num);
                var temp = randomNumList[i];
                randomNumList[i] = randomNumList[randomNum];
                randomNumList[randomNum] = temp;
            }
            RandomNumList = new List<int>(randomNumList);
        }
    }
}
