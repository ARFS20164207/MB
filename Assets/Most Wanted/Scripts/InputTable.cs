using System;
using Most_Wanted.Scripts.Base;
using Most_Wanted.Scripts.V2;
using UnityEngine;

namespace Most_Wanted.Scripts
{
    
    public class InputTable : MonoBehaviour,ITableWorld
    {
        public GameObject cursor;
        public Transform EsquinaRD;
        public Transform EsquinaLU;
        public Vector3 normalDirection;
        public Camera mainCamera;
        
        public IController _controller;
        public BoardGame _controller2;
        private void Start()
        {
            normalDirection = (EsquinaLU.position - EsquinaRD.position);
        }

        private void OnMouseEnter()
        {
        }

        private void OnMouseExit()
        {
          
        }

        private void OnMouseDown()
        {
            cursor.SetActive((true));
            if (mainCamera != null)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    if (_controller != null)
                    {
                        _controller.TableInteract(getCell(hit.point));
                        return;
                    }

                    if (_controller2.TryGetComponent(out _controller))
                    {
                        for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 7; j++)
                        {
                            _controller.CellReference(new Vector2(i + 1, j + 1), CellToWorld(i + 1, j + 1));
                        }
                        _controller.TableInteract(getCell(hit.point));
                    }

                }
            }
        }

        private void OnMouseDrag()
        {
            if (mainCamera == null)return;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                cursor.transform.position = hit.point;
            }
        }

        private void OnMouseUp()
        {
            cursor.SetActive((false));
        }

        private void OnMouseOver()
        {
            
        }

        Vector2 getCell(Vector3 refPosition)
        {
            float x = (EsquinaLU.position.x - refPosition.x)/normalDirection.x;
            float y = (EsquinaLU.position.y - refPosition.y)/normalDirection.y;
            float z = (EsquinaLU.position.z - refPosition.z)/normalDirection.z;

            float xCell = Mathf.Round((x * 9) + .5f);
            float yCell = Mathf.Round((z * 7) + .5f);
            return new Vector2(xCell, yCell);
        }

        public Vector3 CellToWorld(int x, int y)
        {
            float X = EsquinaLU.position.x - (-.5f+x) *(normalDirection.x/9);
            float Y = EsquinaLU.position.y - 0 *(normalDirection.y/1);
            float Z = EsquinaLU.position.z - (-.5f+y) *(normalDirection.z/7);
            return new Vector3(X, Y, Z);
        }
    }
}
