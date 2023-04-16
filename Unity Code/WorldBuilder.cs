using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum ObjectType
{
    Wall,
    WallHorizontal,
    WallVertical,
    Door,
    Ghost,
    Floor
};

public class WorldBuilder : MonoBehaviour
{
    //[SerializeField] string fileName;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject mapObjectParent;
    [SerializeField] List<GameObject> mapObjectPrefabs;
    [SerializeField] DatabaseManager databaseManager;

    [SerializeField] LoadedZones loadedZones;

    TextAsset mapFile;
    Zone zone;
    List<ConnectedZone> connectedZones;

    // Start is called before the first frame update
    void Start()
    {
        databaseManager.CallGetZone("1", this);
        //databaseManager.CallGetZone("5", this);
        //databaseManager.CallGetZone("6", this);
        //databaseManager.CallGetZone("7", this);
        //databaseManager.CallGetZone("8", this);
        //databaseManager.CallGetZone("9", this);
        //PrintResult();
        //databaseManager.CallTest();   
        //BeginGenerateMap(new Zone("7","level7","0","0", "11", "6"));

    }

    public void BeginGenerateMap(Zone foundZone)
    {
        zone = foundZone;
        if (!loadedZones.DoesEntryExit(zone.ID))
        {
            Debug.Log("3");
            loadedZones.AddEntry(zone.ID);
            //generate new zone entry here
            //Debug.Log(zone.Name);
            mapFile = Resources.Load<TextAsset>("zones/" + zone.Name);
            text.text = "";
            GenerateMap(zone.X, zone.Y, 2);
        }
        else
        {
            Debug.Log("4");
            databaseManager.CallGetConnectedZones(""+zone.ID, this);
        }
    }

    public void BeginGeneratingConnected(int originalZone, List<ConnectedZone> foundConnectedZones)
    {
        Debug.Log("conzon");
        connectedZones = new List<ConnectedZone> (foundConnectedZones);
        foreach(var conZon in connectedZones)
        {
            Debug.Log("conzon: "+conZon.Zone.ToString());
            //add neighbours to the zone entry here
            //Debug.Log(conZon.Zone.ID);
            zone = conZon.Zone;
            loadedZones.AddZoneNeighbour(originalZone, zone.ID);
            if (!loadedZones.DoesEntryExit(zone.ID))
            {
                loadedZones.AddEntry(zone.ID);
                mapFile = Resources.Load<TextAsset>("zones/" + zone.Name);
                text.text = "";
                GenerateMap(zone.X, zone.Y);
            }
            loadedZones.AddZoneNeighbour(zone.ID, originalZone);
        }
        Debug.Log("endzon");
    }

    //zeroX, zeroY are the coordinates of where the zone starts
    //aka think of them as the new 0,0 position
    private void GenerateMap(float zeroX, float zeroY, int generateNeighbours = 0)
    {
        List<WorldObject> worldObjects = new List<WorldObject>();
        var splits = new string[] {"\n", "\r\n" };
        string[] mapText = mapFile.text.Split(splits,System.StringSplitOptions.None);
        worldObjects.Add(new WorldObject(ObjectType.Floor, 0,0, zone.Width, zone.Height));//(int)zone.X, (int)zone.Y
        for (int i = 0; i < mapText.Length; i++)
        {
            for (int j = 0; j < mapText[i].Length; j++)
            {
                switch (mapText[i][j])
                {
                    case 'x':
                    case 'X':
                        StoreWall(i, j, mapText, worldObjects);
                        break;
                    case 'd':
                    case 'D':
                        StoreDoor(i, j, mapText, worldObjects);
                        break;
                    case 'g':
                    case 'G':
                        StoreGhost(i, j, mapText, worldObjects);
                        break;
                }
            }
        }
        InstantiatePrefabs(zeroX, zeroY, worldObjects);
        //if(generateNeighbours > 0)
            //databaseManager.CallGetConnectedZones(""+zone.ID, this, generateNeighbours-1);
        //Debug.Break();
    }

    private void InstantiatePrefabs(float zeroX, float zeroY, List<WorldObject> worldObjects)
    {
        GameObject parent = Instantiate(mapObjectParent);
        parent.GetComponent<Level>().id = zone.ID;
        loadedZones.AddZoneParent(zone.ID, parent.GetComponent<Level>());

        GameObject worldObj;
        foreach(var obj in worldObjects)
        {
            switch (obj.type)
            {
                case ObjectType.Wall:
                case ObjectType.WallHorizontal:
                case ObjectType.WallVertical:
                    worldObj = Instantiate(mapObjectPrefabs[1], new Vector3(obj.unityX, 0.5f, -obj.unityY), new Quaternion(0, 0, 0, 0), parent.transform);
                    worldObj.transform.localScale += new Vector3(obj.width, 0, obj.height);
                    //Debug.Log(obj.type + " X: " + obj.cartMinX + " " + obj.cartMaxX + " Y: " + obj.cartMinY + " " + obj.cartMaxY + " unity X " + obj.unityX + " width " + obj.width + " unity Y " + obj.unityY + " height " + obj.height);

                    break;
                case ObjectType.Floor:
                    //Debug.Log(obj.type + " X: " + obj.cartMinX + " " + obj.cartMaxX + " Y: " + obj.cartMinY + " " + obj.cartMaxY + " unity X " + obj.unityX + " width " + obj.width + " unity Y " + obj.unityY + " height " + obj.height);
                    worldObj = Instantiate(mapObjectPrefabs[0], new Vector3(obj.unityX, 0.5f, -obj.unityY), new Quaternion(0, 0, 0, 0), parent.transform);
                    worldObj.transform.localScale += new Vector3(obj.width, 0, obj.height);
                    var boxCol = parent.GetComponent<BoxCollider>();
                    boxCol.size = new Vector3(obj.width, 0, obj.height);
                    boxCol.center = new Vector3(obj.unityX, 0.5f, -obj.unityY);
                    break;
                case ObjectType.Door:
                    //Debug.Log(obj.type + " X: " + obj.cartMinX + " " + obj.cartMaxX + " Y: " + obj.cartMinY + " " + obj.cartMaxY + " unity X " + obj.unityX + " width " + obj.width + " unity Y " + obj.unityY + " height " + obj.height);
                    worldObj = Instantiate(mapObjectPrefabs[2], new Vector3(obj.unityX, 0.5f, -obj.unityY), new Quaternion(0, 0, 0, 0), parent.transform);
                    worldObj.transform.localScale += new Vector3(obj.width, 0, obj.height);
                    break;
                case ObjectType.Ghost:
                    //Debug.Log(obj.type + " X: " + obj.cartMinX + " " + obj.cartMaxX + " Y: " + obj.cartMinY + " " + obj.cartMaxY + " unity X " + obj.unityX + " width " + obj.width + " unity Y " + obj.unityY + " height " + obj.height);
                    worldObj = Instantiate(mapObjectPrefabs[3], new Vector3(obj.unityX, 1f, -obj.unityY), new Quaternion(0, 0, 0, 0), parent.transform);
                    //worldObj.transform.localScale += new Vector3(obj.width, 0, obj.height);
                    worldObj.GetComponent<GhostBtn>().GenerateGhost(1, 3);
                    break;
                default:
                    break;
            }
        }
        parent.transform.position = new Vector3(zeroX, 0f, zeroY);
    }

    //private void PrintResult()
    //{
    //    foreach(var obj in worldObjects)
    //    {
    //        Debug.Log(obj.type + " X: " + obj.cartMinX + " " +obj.cartMaxX + " Y: " +obj.cartMinY + " " +obj.cartMaxY+ " unity X " + obj.unityX + " width " + obj.width + " unity Y " + obj.unityY + " height " + obj.height);
    //    }
    //}

    private void StoreDoor(int i, int j, string[] mapText, List<WorldObject> worldObjects)
    {
        WorldObject obj;
        List<ObjectType> filter = new List<ObjectType>();
        filter.Add(ObjectType.Door);
        if (i > 0)
        {
            if (mapText[i - 1][j] == 'd' || mapText[i - 1][j] == 'D')
            {
                obj = FindWorldObject(j, i - 1, filter, worldObjects);
                if (obj != null && obj.type == ObjectType.Door)
                {
                    obj.AddSegment(y: i);
                    return;
                }
            }
        }
        if (j > 0)
        {
            if ((mapText[i][j - 1] == 'd' || mapText[i][j - 1] == 'D'))
            {
                obj = FindWorldObject(j - 1, i, filter, worldObjects);
                if (obj != null && obj.type == ObjectType.Door)
                {
                    obj.AddSegment(x: j);
                    return;
                }
            }
        }
        worldObjects.Add(new WorldObject(ObjectType.Door, j, i));
    }

    private void StoreGhost(int i, int j, string[] mapText, List<WorldObject> worldObjects)
    {
        WorldObject obj;
        List<ObjectType> filter = new List<ObjectType>();
        filter.Add(ObjectType.Ghost);
        if (i > 0)
        {
            if (mapText[i - 1][j] == 'd' || mapText[i - 1][j] == 'D')
            {
                obj = FindWorldObject(j, i - 1, filter, worldObjects);
                if (obj != null && obj.type == ObjectType.Ghost)
                {
                    return; //two ghost shouldn't be next to each other
                }
            }
        }
        if (j > 0)
        {
            if ((mapText[i][j - 1] == 'd' || mapText[i][j - 1] == 'D'))
            {
                obj = FindWorldObject(j - 1, i, filter, worldObjects);
                if (obj != null && obj.type == ObjectType.Ghost)
                {
                    return;//two ghost shouldn't be next to each other
                }
            }
        }
        worldObjects.Add(new WorldObject(ObjectType.Ghost, j, i));
    }

    private void StoreWall(int i, int j, string[] mapText, List<WorldObject> worldObjects)
    {
        WorldObject obj;
        List<ObjectType> filter = new List<ObjectType>();
        filter.Add(ObjectType.Wall);
        filter.Add(ObjectType.WallVertical);
        filter.Add(ObjectType.WallHorizontal);
        if (i > 0 )
        {
            if (mapText[i - 1][j] == 'x' || mapText[i - 1][j] == 'X')
            {
                obj = FindWorldObject(j, i - 1, filter, worldObjects);
                if (obj != null && obj.type != ObjectType.WallHorizontal)
                {
                    obj.AddSegment(y: i);
                    return;
                }
            }
        }
        if (j > 0 )
        {
            if (mapText[i][j - 1] == 'x' || mapText[i][j - 1] == 'X')
            {
                obj = FindWorldObject(j - 1, i, filter, worldObjects);
                if (obj != null && obj.type != ObjectType.WallVertical)
                {
                    obj.AddSegment(x: j);
                    return;
                }
            }

        }
        worldObjects.Add(new WorldObject(ObjectType.Wall, j, i));
    }

    private WorldObject FindWorldObject(int x = -1, int y = -1, List<ObjectType> filter = null, List<WorldObject> worldObjects = null)
    {
        List<WorldObject> foundObject = null;
        if(worldObjects == null)
        {
            return null;
        }

        if (x > -1)
        {
            foundObject = worldObjects.FindAll(obj => (obj.cartMinX <= x && obj.cartMaxX >= x));
        }
        if (y > -1)
        {
            if (foundObject == null)
                foundObject = worldObjects.FindAll(obj => (obj.cartMinY <= y && obj.cartMaxY >= y));
            else
                foundObject = foundObject.FindAll(obj => (obj.cartMinY <= y && obj.cartMaxY >= y));
        }
        if (filter != null)
        {
            if (foundObject == null)
                foundObject = worldObjects.Where(ele => filter.Any(x => ele.type == x)).ToList();
            else
                foundObject = foundObject.Where(ele => filter.Any(x => ele.type == x)).ToList();

        }

        return foundObject[0];
    }
}

public class WorldObject
{
    public ObjectType type;
    public int cartMinX;
    public int cartMinY;
    public int cartMaxX;
    public int cartMaxY;
    public float unityX;
    public float unityY;
    public float zoneX;
    public float zoneY;
    public float width;
    public float height;

    public WorldObject(ObjectType pType, int x, int y)
    {
        type = pType;
        cartMaxX = x;
        cartMinX = x;
        cartMaxY = y;
        cartMinY = y;
        width = (float)(cartMaxX - cartMinX);
        unityX = ((float)cartMinX) + (width / 2);
        height = (float)(cartMaxY - cartMinY);
        unityY = ((float)cartMinY) + (height / 2);
    }
    public WorldObject(ObjectType pType, int x, int y, float pWidth, float pHeight)
    {
        type = pType;
        cartMaxX = x;
        cartMinX = x;
        cartMaxY = y;
        cartMinY = y;
        width = pWidth;
        unityX = ((float)cartMinX) + (width / 2);
        height = pHeight;
        unityY = ((float)cartMinY) + (height / 2);
        if(type == ObjectType.Floor)
        {
            unityX -= 0.5f;
            unityY -= 0.5f;
        }
    }

    public void AddSegment(int x = -1, int y = -1)
    {
        if (x > -1)
        {
            if (x < cartMinX)
                cartMinX = x;
            if (x > cartMaxX)
                cartMaxX = x;
            width = (float)(cartMaxX - cartMinX);
            unityX = ((float)cartMinX) + (width / 2);
            if (type != ObjectType.Door)
                type = ObjectType.WallHorizontal;
        }
        if (y > -1)
        {
            if (y < cartMinY)
                cartMinY = y;
            if (y > cartMaxY)
                cartMaxY = y;
            height = (float)(cartMaxY - cartMinY);
            unityY = ((float)cartMinY) + (height / 2);
            if (type != ObjectType.Door)
                type = ObjectType.WallVertical;
        }
    }
}