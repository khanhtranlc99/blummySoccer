using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class TweenHandler
{
    internal class TweenMemory{
        private List<Tween> ListTween = new List<Tween>();
        public void AddTween(Tween tw){
            if(!ListTween.Contains(tw))
                ListTween.Add(tw);
        }
        public void KillAll(){
            foreach (var tw in ListTween)
                tw.Kill();
        }
    }
    private static Dictionary<string, TweenMemory> s_TweenTable = new Dictionary<string, TweenMemory>();
    private static Dictionary<Type, TweenMemory> t_TweenTable = new Dictionary<Type, TweenMemory>();
    public static void KillAllTween(string obj){
        TweenMemory TWMemory;
        if(s_TweenTable.TryGetValue(obj, out TWMemory))
            TWMemory.KillAll();
    }
    public static void KillAllTween(Type obj){
        TweenMemory TWMemory;
        if(t_TweenTable.TryGetValue(obj, out TWMemory))
            TWMemory.KillAll();
    }
    //Extension
    public static void AddTween(this Tween tw, string obj){
        TweenMemory TWMemory;
        if(s_TweenTable.TryGetValue(obj, out TWMemory))
            TWMemory.AddTween(tw);
        else{
            TWMemory = new TweenMemory();
            TWMemory.AddTween(tw);
            s_TweenTable.Add(obj, TWMemory);
        }
    }
    public static void AddTweener(string obj, Tween Tweener){
        TweenMemory TWMemory;
        if(s_TweenTable.TryGetValue(obj, out TWMemory))
            TWMemory.AddTween(Tweener);
        else{
            TWMemory = new TweenMemory();
            TWMemory.AddTween(Tweener);
            s_TweenTable.Add(obj, TWMemory);
        }
    }
    public static void AddTweener(Type obj, Tween Tweener){
        TweenMemory TWMemory;
        if(t_TweenTable.TryGetValue(obj, out TWMemory))
            TWMemory.AddTween(Tweener);
        else{
            TWMemory = new TweenMemory();
            TWMemory.AddTween(Tweener);
            t_TweenTable.Add(obj, TWMemory);
        }
    }
    public static void DeleteTweener(string obj){
        TweenMemory TWMemory;
        if(s_TweenTable.TryGetValue(obj, out TWMemory))
            s_TweenTable.Remove(obj);
    }
    public static void DeleteTweener(Type obj){
        TweenMemory TWMemory;
        if(t_TweenTable.TryGetValue(obj, out TWMemory))
            t_TweenTable.Remove(obj);
    }
}
