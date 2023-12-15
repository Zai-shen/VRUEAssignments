using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class MapConnection
    {
        public float TargetRotation;

        public MapPart FromMP;
        public MapPart ThisMP;
        public MapPart ToMP;

        public XZCoords MobEntry;
        public XZCoords MobExit;

        public MapConnection(MapPart thisMp)
        {
            ThisMP = thisMp;
            SetMobEntryExit();
        }

        public void SetMobEntryExit()
        {
            MobExit = ThisMP.MapTSo.MobExit;
            MobEntry = ThisMP.MapTSo.MobEntry;
        }

        public void Clear()
        {
            if (FromMP != null)
            {
                FromMP.MapCon.ToMP = null;
                FromMP = null;
            }
            if (ToMP != null)
            {
                ToMP.MapCon.FromMP = null;
                ToMP = null;
            }
        }

        public bool IsConnectedWithTO()
        {
            return ToMP != null;
        }
        
        // public bool TryConnectTo(MapTile other)
        // {
        //     bool clockWise = Random.value > 0.5f;
        //
        //     for (int i = 0; i < 4; i++)
        //     {
        //         if (IsConnectable(this.MobExit, other.MobEntry, other))
        //         {
        //             Debug.Log($"Connecting this {gameObject.name} with MobExit {MobExit} to {other.gameObject.name} with Mobentry {other.MobEntry}");
        //             transform.Rotate(Vector3.up, (clockWise ? 90f : -90f) * i);
        //             return true;
        //         }
        //         
        //         if (clockWise)
        //         {
        //             RotateClockWise();
        //         }
        //         else
        //         {
        //             RotateCounterClockWise();
        //         }
        //     }
        //
        //     // Debug.Log($"Thispos{transform.position} otherpos{other.transform.position}");
        //     Debug.LogWarning($"Could NOT connect this {gameObject.name} with MobExit {MobExit} to {other.gameObject.name} with Mobentry {other.MobEntry}");
        //     return false;
        // }
        
        private XZCoords RotateXZCoordinateCW(XZCoords coord)
        {
            switch (coord)
            {
                case XZCoords.UP:
                    return XZCoords.RIGHT;
                case XZCoords.RIGHT:
                    return XZCoords.DOWN;
                case XZCoords.DOWN:
                    return XZCoords.LEFT;
                case XZCoords.LEFT:
                    return XZCoords.UP;
                case XZCoords.CENTER:
                    return XZCoords.CENTER;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coord), coord, null);
            }
        }

        private bool IsConnectable(XZCoords exit, XZCoords entry)
        {
            return (exit == XZCoords.RIGHT && entry == XZCoords.LEFT) ||
                   (exit == XZCoords.LEFT && entry == XZCoords.RIGHT) ||
                   (exit == XZCoords.UP && entry == XZCoords.DOWN) ||
                   (exit == XZCoords.DOWN && entry == XZCoords.UP);
        }

        public bool ConnectTo()
        {
            XZCoords toMobEntry = ToMP.MapCon.MobEntry;
            
            for (int i = 0; i < 4; i++)
            {
                if (IsConnectable(MobExit, toMobEntry) && IsPlacedRight(MobExit, ThisMP, ToMP))
                {
                    Debug.Log($"Did connect From:{ThisMP} with MobExit {MobExit} - to To:{ToMP} with Mobentry {toMobEntry}");
                    TargetRotation = 90f * i;
                    ToMP.MapCon.FromMP = ThisMP;
                    return true;
                }
                Debug.Log($"Tried but failed: this {ThisMP} with MobExit {MobExit} - to {ToMP} with Mobentry {toMobEntry}");
                MobExit = RotateXZCoordinateCW(MobExit);
                MobEntry = RotateXZCoordinateCW(MobEntry);
            }

            Debug.LogWarning($"Could NOT connect From:{ThisMP} To:{ToMP}");
            return false;
        }

        private bool IsPlacedRight(XZCoords exit, MapPart from, MapPart to)
        {
            // Debug.Log($"Exit dir: {exit} frompos:{from.GetGridPosition()} topos:{to.GetGridPosition()}");
            return exit switch
            {
                XZCoords.UP => from.GetGridPosition().z < to.GetGridPosition().z,
                XZCoords.DOWN => from.GetGridPosition().z > to.GetGridPosition().z,
                XZCoords.LEFT => from.GetGridPosition().x > to.GetGridPosition().x,
                XZCoords.RIGHT => from.GetGridPosition().x < to.GetGridPosition().x,
                XZCoords.CENTER => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}