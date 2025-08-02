/*****************************************************************************
// File Name :         Game Data.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 22nd, 2024
//
// Brief Description : Holds the High Score information to be converted into a JSon File.
*****************************************************************************/
[System.Serializable]
public class GameData
{
    public float PlayerPosX;
    public float PlayerPosY;
    public float CurrentTimerTime;
    public int CurrentBackpack;
    public string RoomName;
    public int LevelsUnlocked;
    public float[] BestLevelTimes = new float[9];
}