using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadedZones : MonoBehaviour
{
    [SerializeField] List<Level> zoneParents;
    [SerializeField] List<ZoneEntry> zoneEntries;


    // Start is called before the first frame update
    void Start()
    {
        zoneParents = new List<Level>();
        zoneEntries = new List<ZoneEntry>();
    }

    public void AddEntry(int zoneID)
    {
        if(zoneEntries == null)
            zoneEntries = new List<ZoneEntry>();
        zoneEntries.Add(new ZoneEntry(zoneID));
    }

    public void AddZoneParent(int zoneID, Level parent)
    {
        zoneParents.Add(parent);
        var entry = zoneEntries.Find(x => x.zoneID == zoneID);
        if (entry == null)
        {
            zoneParents.Remove(parent);
            parent.DestroyObject();
        }
    }

    public void AddZoneNeighbour(int zoneID, int neighbourID)
    {
        var entry = zoneEntries.Find(x => x.zoneID == zoneID);
        if (entry == null)
            return;
        if (entry.neighbourIDs.Any(x => x == neighbourID))
            return;
        entry.AddNeighbour(neighbourID);
    }

    public void UnloadZones(int oldZoneID, int newZoneID)
    {
        var entry = zoneEntries.Find(x => x.zoneID == oldZoneID);
        if (entry == null)
            return;
        List<int> watchList = new List<int>(entry.neighbourIDs);

        foreach (var watchListID in watchList)
        {
            var watchListEntry = zoneEntries.Find(x => x.zoneID == watchListID);
            if(watchListEntry == null)
                entry.neighbourIDs.Remove(watchListID);
            List<int> possibleRemovals = new List<int>(watchListEntry.neighbourIDs);

            foreach (var possibleRemovalID in possibleRemovals)
            {
                var possibleRemoval = zoneEntries.Find(x => x.zoneID == possibleRemovalID);
                if (possibleRemoval == null)
                    entry.neighbourIDs.Remove(watchListID);
                bool removeThis = true;
                foreach(var neighbour in possibleRemoval.neighbourIDs)
                {
                    if (oldZoneID == neighbour || newZoneID == neighbour)
                        removeThis = false;
                }
                if (removeThis)
                {
                    RemoveZoneEntry(possibleRemovalID);
                    watchListEntry.neighbourIDs.Remove(possibleRemovalID);
                }
            }   
        }
    }

    public bool DoesEntryExit(int zoneID)
    {
        if (zoneEntries == null)
            return false;
        var entry = zoneEntries.Find(x => x.zoneID == zoneID);
        if (entry != null)
            return true;
        else
            return false;
    }

    public void RemoveZoneEntry(int zoneID)
    {
        var entry = zoneEntries.Find(x => x.zoneID == zoneID);
        if (entry == null)
            return;
        var destroyParent = zoneParents.Find(x=>x.id == entry.parentIdx);
        zoneParents.Remove(destroyParent);
        if(destroyParent!=null)
            destroyParent.DestroyObject();
        zoneEntries.Remove(entry);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            foreach (var entry in zoneEntries)
            {
                Debug.Log(entry.ToString());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UnloadZones(5, 6);
        }
    }
}

public class ZoneEntry
{
    public int zoneID;
    public int parentIdx;
    public List<int> neighbourIDs;

    public ZoneEntry(int pZoneID)
    {
        zoneID = pZoneID;
        neighbourIDs = new List<int>();
    }

    public void AddParentIdx(int newParentIdx)
    {
        parentIdx = newParentIdx;
    }

    public void AddNeighbour(int newNeighbour)
    {
        neighbourIDs.Add(newNeighbour);
    }

    public bool IsNeighbour(int neighbourID)
    {
        foreach (var neighbour in neighbourIDs)
        {
            if (neighbour == neighbourID)
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        string neighbours = "";
        foreach (var neighbour in neighbourIDs)
        {
            neighbours += neighbour + ", ";
        }
        return "Zone Id: " + zoneID + ", Parent Id: " + parentIdx + ", Neighbours:" + neighbours;
    }
}
