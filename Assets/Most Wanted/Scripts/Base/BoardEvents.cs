using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardEvents : MonoBehaviour
{
    // A singleton instance of the event manager
    public static BoardEvents Instance;
    [SerializeField] bool SendLogs = true;

    // A custom event that takes a string as a parameter
    public UnityEvent<string> CustomEventString;
    public UnityEvent<int> CustomEventInt;
    public UnityEvent<bool> CustomEventBool;
    public UnityEvent<BoardCell> OnSelected;
    public UnityEvent<BoardCell,bool> OnHover;
    public UnityEvent<BoardCell,bool> OnHoverExit;
    public UnityEvent<MoveStats,bool> OnMove;

    // A built-in event that takes no parameters
    public UnityEvent DefaultEvent;
    private void Invoke<T>(UnityEvent<T> CustomEvent, T parameter)
    {
        CustomEvent.Invoke(parameter);
    }
    private void Invoke<T,U>(UnityEvent<T,U> CustomEvent, T parameter, U parameter2)
    {
        CustomEvent.Invoke(parameter, parameter2);
    }

    // A method to invoke the event manager with a given event name and optional parameter
    public void InvokeString(BoardCustomEvents eventName, string parameter = null)
    {
        if(SendLogs) {Debug.Log(eventName + ": " + parameter);}
        switch (eventName)
        {
            case BoardCustomEvents.CustomEvent: Invoke(CustomEventString, parameter); break;
            default: InvokeVoid(eventName); break;

        }
    }
    public void InvokeInt(BoardCustomEvents eventName, int parameter)
    {
        if (SendLogs) { Debug.Log(eventName + ": " + parameter); }
        switch (eventName)
        {
            case BoardCustomEvents.CustomEvent: Invoke(CustomEventInt, parameter); break;
            default: InvokeVoid(eventName); break;
        }
    }
    public void InvokeBool(BoardCustomEvents eventName, bool parameter)
    {
        if (SendLogs) { Debug.Log(eventName + ": " + parameter); }
        switch (eventName)
        {
            case BoardCustomEvents.CustomEvent: Invoke(CustomEventBool, parameter); break;
            default: InvokeVoid(eventName); break;
        }
    }
    public void InvokeCell(BoardCustomEvents eventName, BoardCell parameter)
    {
        if (SendLogs) { Debug.Log(eventName + ": " + parameter); }
        switch (eventName)
        {
            case BoardCustomEvents.OnSelected: Invoke(OnSelected, parameter); break;
            case BoardCustomEvents.OnHover: Invoke(OnHover, parameter,true); break;
            case BoardCustomEvents.OnHoverExit: Invoke(OnHoverExit, parameter,false); break;
            default: InvokeVoid(eventName); break;
        }
    }
    public void InvokeStatsBool(BoardCustomEvents eventName, MoveStats parameter, bool parameter2)
    {
        if (SendLogs) { Debug.Log(eventName + ": " + parameter + " & " + parameter2); }
        switch (eventName)
        {
            case BoardCustomEvents.OnMove: Invoke(OnMove, parameter, parameter2); break;
            default: InvokeVoid(eventName); break;
        }
    }
    public void InvokeVoid(BoardCustomEvents eventName)
    {
        switch (eventName)
        {
            case BoardCustomEvents.DefaultEvent: DefaultEvent.Invoke(); break;
            // Otherwise, print an error message
            default: Debug.LogError("Invalid event name: " + eventName); break;
        }
    }


    // A method to initialize the singleton instance
    private void Awake()
    {
        // If the instance is null, assign this object to it
        if (Instance == null)
        {
            Instance = this;
        }
        // Otherwise, destroy this object
        else
        {
            Destroy(gameObject);
        }
    }



}
    public enum BoardCustomEvents
    {
        None = 0,
        CustomEvent,
        DefaultEvent,
        OnSelected,
        OnHover,
        OnHoverExit,
        OnMove
    }
