using System;
using System.Collections.Generic;
using System.Linq;
using Most_Wanted.Scripts.Base;
using UnityEngine;

namespace Most_Wanted.Scripts.V2
{
    public class LocalFeedback:BoardFeedback
    {
        public new Transform selectContainer;
        public List<Transform> selects = new List<Transform>();
        public new Transform captureContainer;
        public List<Transform> captures = new List<Transform>();
        public new Transform movesContainer;
        public List<Transform> moves = new List<Transform>();
        private void Start()
        {
            //Initialize(selectContainer, selects);
            //Initialize(captureContainer, captures);
            //Initialize(movesContainer, moves);
            BoardEvents.Instance.OnPosibleMoves.AddListener(Signals);
        }

        public override bool ClearSignals(Transform container)
        {
           ClearList(selects);
           ClearList(captures);
           ClearList(moves);
            return true;
        }

        public override bool Signals(bool[,] results)
        {
            return true;
        }
        public override void Signals(bool[,] results, BoardPiece player)
        {
            ClearSignals(null);
            for (int i = 0; i < results.GetLength(0); i++)
            {
                for (int j = 0; j < results.GetLength(1); j++)
                {
                    if (!results[i, j])continue;
                    Cell cell = BoardGame.instance.board[i, j];
                    if(cell==null) continue;
                    if (cell.currentPiece == player) GetChild(selects).position = cell.position;
                    else if(cell.currentPiece == null) GetChild(moves).position = cell.position;
                    else if(cell.currentPiece.controller != player.controller) GetChild(captures).position = cell.position;
                }
            }
        }

        void ClearList(List<Transform> childrenList)
        {
            foreach (Transform child in childrenList)
            {
                child.gameObject.SetActive(false);
            }
        }
        Transform GetChild(List<Transform> childrenList)
        {
            Transform child= childrenList.First((x) => !x.gameObject.activeInHierarchy);
            child.gameObject.SetActive(true);
            return child;
        }
        Transform GetChild(Transform container)
        {
            Transform[] children = container.GetComponentsInChildren<Transform>();
            Transform child= children.First((x) => !x.gameObject.activeInHierarchy);
            return child;
        }
        void Initialize(Transform container,List<Transform> childrenList)
        {
            childrenList = new List<Transform>();
            Transform[] children = container.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                childrenList.Add(child);
            }
        }
    }
}