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
                //Debug.Log($"Clearing fromMP: {FromMP},{FromMP.MapCon.ToMP}");
                FromMP.MapCon.ToMP = null;
                FromMP = null;
            }
            if (ToMP != null)
            {
                //Debug.Log($"Clearing toMP: {ToMP},{ToMP.MapCon.FromMP}");
                ToMP.MapCon.FromMP = null;
                ToMP = null;
            }
        }

        public bool IsConnectedWithTO()
        {
            return ToMP != null;
        }

        public bool IsConnectedWithFrom()
        {
            return FromMP != null;
        }
        
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
                case XZCoords.NONE:
                    return XZCoords.NONE;
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
            bool did = CouldConnectTo(out float i);
            if (did)
            {
                TargetRotation = i;
                ToMP.MapCon.FromMP = ThisMP;
            }
            return did;
        }
        
        public bool CouldConnectTo(out float rot)
        {
            XZCoords toMobEntry = ToMP.MapCon.MobEntry;
            
            for (int i = 0; i < 4; i++)
            {
                if (IsConnectable(MobExit, toMobEntry) && IsPlacedRight(MobExit, ThisMP, ToMP))// && HasEmptyExit(MobExit, ThisMP))
                {
                    // Debug.Log($"Did connect From:{ThisMP} with MobExit {MobExit} - to To:{ToMP} with Mobentry {toMobEntry}");
                    rot = 90f * i;
                    return true;
                }
                // Debug.Log($"Tried but failed: this {ThisMP} with MobExit {MobExit} - to {ToMP} with Mobentry {toMobEntry}");
                MobExit = RotateXZCoordinateCW(MobExit);
                MobEntry = RotateXZCoordinateCW(MobEntry);
            }

            // Debug.LogWarning($"Could NOT connect From:{ThisMP.MapPartGo} To:{ToMP.MapPartGo}");
            rot = 0f;
            return false;
        }

        private bool HasEmptyExit(XZCoords exit, MapPart thisMP)
        {
            MapPart mapPart;
            switch (exit)
            {
                case XZCoords.UP:
                    mapPart = thisMP.GetSibling(thisMP.GetGridPosition() + Vector3Int.up);
                    break;
                case XZCoords.RIGHT:
                    mapPart = thisMP.GetSibling(thisMP.GetGridPosition() + Vector3Int.right);
                    break;
     
                case XZCoords.DOWN:
                    mapPart = thisMP.GetSibling(thisMP.GetGridPosition() + Vector3Int.down);
                    break;
    
                case XZCoords.LEFT:
                    mapPart = thisMP.GetSibling(thisMP.GetGridPosition() + Vector3Int.left);
                    break;
                case XZCoords.NONE:
                case XZCoords.CENTER:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (mapPart != null)
            {
                bool isnotcon = !mapPart.MapCon.IsConnectedWithTO();
                if (!isnotcon)
                {
                    // Debug.Log($"mappart is connected: {mapPart}");
                }
                return isnotcon;
            }
            return true;
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
                XZCoords.NONE => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString()
        {
            return $"MapConnection with From [{FromMP}] - This [{ThisMP}] - To [{ToMP}]";
        }
    }
}