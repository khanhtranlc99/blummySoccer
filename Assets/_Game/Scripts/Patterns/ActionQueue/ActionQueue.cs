using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{
    public bool finished
    {
        get
        {
            return this.list.Count == 0;
        }
    }
    public int ActionCount
    {
        get { return this.list.Count; }
    }
    public void Update()
    {
        while (this.list.Count != 0)
        {
            ActionQueue.Node node = this.list[0];
            bool flag = node.Update(Time.deltaTime);
            if (!flag)
            {
                break;
            }
            this.list.RemoveAt(0);
        }
    }

    public void PushTime(float time)
    {
        while (this.list.Count != 0)
        {
            ActionQueue.Node node = this.list[0];
            bool flag = node.Update(time);
            if (!flag)
            {
                break;
            }
            this.list.RemoveAt(0);
        }
    }

    public ActionQueue.Node PushAction(Func<float, bool> action)
    {
        ActionQueue.Node node = new ActionQueue.Node();
        this.list.Add(node.PushAction(action));
        return node;
    }

    public ActionQueue.Node PushNode(ActionQueue.Node node)
    {
        this.list.Add(node);
        return node;
    }

    private List<ActionQueue.Node> list = new List<ActionQueue.Node>();

    public class Node
    {
        public ActionQueue.Node PushAction(Func<float, bool> func)
        {
            this.m_FuncList.Add(func);
            return this;
        }

        public bool Update(float deltaTime)
        {
            int num = 0;
            while (num != this.m_FuncList.Count)
            {
                if (this.m_FuncList[num](deltaTime))
                {
                    this.m_FuncList.RemoveAt(num);
                }
                else
                {
                    num++;
                }
            }
            if (this.m_FuncList.Count == 0)
            {
                int num2 = 0;
                while (num2 != this.m_NextNodeList.Count)
                {
                    bool flag = this.m_NextNodeList[num2].Update(deltaTime);
                    if (flag)
                    {
                        this.m_NextNodeList.RemoveAt(num2);
                    }
                    else
                    {
                        num2++;
                    }
                }
                return this.m_NextNodeList.Count == 0;
            }
            return false;
        }

        public void Attacth2SelfNextNode(ActionQueue.Node node)
        {
            this.m_NextNodeList.Add(node);
        }

        public ActionQueue.Node Attacth2SelfNextNodeReturnNext(ActionQueue.Node node)
        {
            this.m_NextNodeList.Add(node);
            return node;
        }

        private List<Func<float, bool>> m_FuncList = new List<Func<float, bool>>();

        public List<ActionQueue.Node> m_NextNodeList = new List<ActionQueue.Node>();
    }
}
