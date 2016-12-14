using System;
using System.Collections;
using System.Collections.Generic;

public class tagItemInfo
{
	public int itemid;
	public int num;
	public Job jobRequire;
}

public class tagTaskReward
{
	public int money;
	public int exp;
	public List<tagItemInfo> items;
}

public enum enTaskGrade
{
	etg_none,
	etg_main,
	etg_sub,
	etg_daily,
}

public enum enTaskType
{
	TASK_ERROR,
	TASK_KILL_MONSTER                   = 1,
	TASK_PASS_DUNGEON                   = 2,
	TASK_COLLECT_ITEM                   = 3,
	TASK_TALK_NPC                       = 4,
	TASK_STRENGTHEN_EQUIP               = 5,
	TASK_SPLIT_EQUIP                    = 6,
}

public class tagTaskInfo
{	
	public int taskid;
	public string taskNameKey;
	public enTaskGrade taskGrade;
	public string taskDescKey;
	public string taskBeginKey;
	public string taskEndKey;
	public tagTaskReward reward;
	public int executeMapId;// which map or dungeon to do this task
	public enTaskType taskType;
	public int acceptLvl;
	public int mDalogId;
	public List<string>  taskStrings = new List<string>();
}

public class TaskInfoUpdate
{
	public Int32	 taskId;
	public enTaskState		 state;				//##1任务已接 2任务完成 3任务结束 4任务可接
	public List<int> taskParams1;
	//public List<int> taskParams2;
	
	public TaskInfoUpdate()
	{
		taskId = 0;
		state = enTaskState.ets_none;
		taskParams1 = new List<int>();
		//taskParams2 = new List<int>();
	}

	public void Init(byte[] data,ref int offset){
		taskId = BitConverter.ToInt32 (data,offset);         offset += sizeof(int);
		offset += 1;
		state = BitConverter.ToBoolean (data,offset)? enTaskState.ets_completed:enTaskState.ets_accepted; offset += 1;
		int num = BitConverter.ToInt32 (data,offset);        offset += sizeof(int);
		taskParams1.Add (num);
		//tagTaskInfo info = TaskMgr.Instance.GetTaskInfo (taskId);
		//string[] str = info.taskStrings[0].Trim().Split (',');
		//taskParams2.Add (int.Parse (str[1]));
	}
}

public class TaskMgr
{
    private Dictionary<int, tagTaskInfo> mTasks = new Dictionary<int, tagTaskInfo>();
    private Dictionary<string, List<string>> mDes = new Dictionary<string, List<string>>();
    private Dictionary<int, TaskInfoUpdate> mCurrentTask = new Dictionary<int, TaskInfoUpdate>();

    public TaskMgr()
    {
    }

    public static enTaskType ParseTaskType(string val)
    {
        switch (val)
        {
            case "killMonster":
                return enTaskType.TASK_KILL_MONSTER;
            case "passDungeon":
                return enTaskType.TASK_PASS_DUNGEON;
            case "collectItem":
                return enTaskType.TASK_COLLECT_ITEM;
            case "talkNpc":
                return enTaskType.TASK_TALK_NPC;
            case "strengthEquip":
                return enTaskType.TASK_STRENGTHEN_EQUIP;
            case "splitEquip":
                return enTaskType.TASK_SPLIT_EQUIP;
        }
        return enTaskType.TASK_ERROR;
    }

    public tagTaskInfo GetTaskInfo(int taskid)
    {
        if (mTasks.ContainsKey(taskid))
            return mTasks[taskid];
        return null;
    }

    public List<TaskInfoUpdate> GetCurrentTaskList()
    {
        List<TaskInfoUpdate> list = new List<TaskInfoUpdate>();
        if (mCurrentTask.Count <= 0)
            return list;

        foreach(KeyValuePair<int, TaskInfoUpdate> d in mCurrentTask)
        {
            list.Add(d.Value);
        }
        return list;
    }


    public void OnTaskList(List<TaskInfoUpdate> info)
    {
        mCurrentTask.Clear();
        foreach (TaskInfoUpdate task in info)
        {
            mCurrentTask[task.taskId] = task;
        }
    }

    public void OnTaskComplete(int id, bool finish)
    {
        if (mCurrentTask.ContainsKey(id))
        {
            if (finish)
            {
                mCurrentTask[id].state = enTaskState.ets_over;
                mCurrentTask.Remove(id);
            }
        }
    }
    public void OnNotifyTask(TaskInfoUpdate info)
    {
        mCurrentTask[info.taskId] = info;
    }
}

