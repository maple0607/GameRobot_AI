using System.Collections;
using System.Collections.Generic;

public enum eMapType
{
    map_create_role = 0,
    map_world,
    map_dungeon,

    map_none = 255,
}

public class SceneInfo
{
    public int mMapID;
    public eMapType mMapType;

    public SceneInfo()
    {
    }

    public SceneInfo(int mapID, eMapType type)
    {
        mMapID = mapID;
        mMapType = type;
    }
}


public class SceneMgr
{
    public class SceneInfoEx : SceneInfo
    {
        public string mResInfoPath;
        public string mResPath;
        public string mResPakPath = string.Empty;
        public Dictionary<string, string> mOtherRes;

        public int mSkyBoxID;       

        public SceneInfoEx()
        {
            mMapID = 0;
            mMapType = eMapType.map_none;
            mSkyBoxID = 0;
        }
    }
    public int mCurSceneIdx = 0;
    private SceneInfoEx mCurSceneInfo = null;
    public SceneInfoEx CurSceneInfo
    {
        get
        {
            return mCurSceneInfo;
        }
        set
        {
            mCurSceneInfo = value;
        }
    }

    public SceneMgr()
    {
        mCurSceneInfo = new SceneInfoEx();
    }

    public void OnEnterMap(byte result, int mapID,int sceneIdx)
    {
        if (result == 1)
        {
            mCurSceneIdx = sceneIdx;
            //NewRobot.Robot.GetCurRobot().PrintExcept("OnEnterMap:" + mapID.ToString());
            this.StartLoadMap(GetSceneInfo(mapID));
        }
    }

    public SceneInfo GetSceneInfo(int mapID)
    {
        SceneInfo info;
        if (mapID / 10000 == 1)
            info = new SceneInfo(mapID, eMapType.map_world);
        else if (mapID == 0)
            info = new SceneInfo(mapID, eMapType.map_create_role);
        else
            info = new SceneInfo(mapID, eMapType.map_dungeon);

        return info;
    }

    private void StartLoadMap(SceneInfo info)
    {
        mCurSceneInfo.mMapID = info.mMapID;
        mCurSceneInfo.mMapType = info.mMapType;
    }
}
