using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class saveSystem
{
    public static bool loadStatus = false;
    public static GameObject mainHero;
    static string json;
    public struct vectorSave
    {
        public Vector3 vector 
        {
            get => new Vector3(x, y, z);
        }
        public float x;
        public float y;
        public float z;
    }
    public class mainHeroSave
    {
        public vectorSave position;
        public float yRotation;
        public int shieldDuration;
        public int HPpoint;
    }
    public class itemSave
    {
        public int id;
        public vectorSave position;
        public vectorSave eulerRotation;
        public int count;
    }
    public class weaponSave
    {
        public int id;
        public vectorSave position;
        public vectorSave eulerRotation;
        public int damageBuff;
        public int speedBuff;
        public int magSizeBuff;
        public int accuracyBuff;
        public int durabilityBuff;
        public int ammoInMag;
        public int ammoId;
        public int durability;
    }
    public class enemySave
    {
        public string behavior;
        public vectorSave position;
        public float yRotation;
        public int shieldDuration;
        public int HPpoint;
        public List<vectorSave> patrulPoints;
    }
    public class wallSave
    {
        public vectorSave position;
        public int HPpoint;
        public int phase;
    }
    public class sceneSave
    {
        public string levelName;
        public List<wallSave> walls;
        public mainHeroSave mainHero;
        public List<enemySave> enemies;
        public List<weaponSave> weapons;
        public List<itemSave> items;
    }
    /// <summary>
    /// {
    ///  "position": {
     ///   "x": 0,
    ///    "y": 0,
     ///   "z": 0
     /// },
    ///  "behavior": "desactive",
     /// "yRotation": 0,
    ///  "shieldDuration": 100,
    ///  "patrulPoints": [
     ///   {
     ///     "x": 0,
     ///     "y": 0,
     ///     "z": 0
     ///   }
     /// ],
     /// "HPpoint": 1
   /// }
    /// </summary>
    /// <param name="save"></param>
    /// <returns></returns>
    private static List<vectorSave>loadListPatrulPoint(string save)
    {
        List<vectorSave> patrulPoints = new List<vectorSave> { };
        int indexOf = save.IndexOf("{", save.IndexOf("\"patrulPoints\""));
        while (indexOf != -1)
        {
            int lastIndexOf = save.IndexOf("}", indexOf);
            string patrulPointSave = save.Substring(indexOf, lastIndexOf - indexOf + 1);
            patrulPoints.Add(JsonUtility.FromJson<vectorSave>(patrulPointSave));
            indexOf = save.IndexOf("{", lastIndexOf);
        }
        return patrulPoints;
    }
    /// <summary>
    /// 
    ///{
    ///  "position": {
    ///    "x": 0,
    ///   "y": 0,
    ///    "z": 0
    /// },
    ///  "HPpoint": 1000,
    ///  "phase": 1
    ///}
/// </summary>
/// <param name="save"></param>
/// <returns></returns>
    private static vectorSave loadVector(string save,string vectorType="\"position\"")
    {
        int indexOf = save.IndexOf("{",save.IndexOf(vectorType)) ;
        string positionSave = save.Substring(indexOf, save.IndexOf("}",indexOf) - indexOf + 1);
        return JsonUtility.FromJson<vectorSave>(positionSave);
    }
    private static mainHeroSave loadMainHero(string save)
    {
        int indexOf = save.IndexOf("\"mainHero\"");
        string mainHeroJsonText = save.Substring(indexOf,
            json.IndexOf("\"enemies\"") - indexOf);
        indexOf = mainHeroJsonText.IndexOf("{");
        mainHeroJsonText = mainHeroJsonText.Substring(indexOf,
            mainHeroJsonText.LastIndexOf(",") - indexOf);
        mainHeroSave heroSave = JsonUtility.FromJson<mainHeroSave>(mainHeroJsonText);
        heroSave.position = loadVector(mainHeroJsonText);
        return heroSave;
    }
    private static List< wallSave >loadWalls(string save)
    {
        List<wallSave> walls=new List<wallSave> { };
        int indexOf = save.IndexOf("\"walls\"");
        string wallsSave = save.Substring(indexOf, save.IndexOf("\"mainHero\"") - indexOf);
        indexOf = wallsSave.IndexOf("{");
        while (indexOf != -1)
        {
            int lastIndexOf = wallsSave.IndexOf("}", wallsSave.IndexOf("phase")+1);
            string currentWallSave = wallsSave.Substring(indexOf, lastIndexOf - indexOf+1);
            walls.Add(JsonUtility.FromJson<wallSave>(currentWallSave));
            walls[walls.Count - 1].position = loadVector(currentWallSave);
            wallsSave = wallsSave.Remove(indexOf, lastIndexOf - indexOf+1);
            indexOf = wallsSave.IndexOf("{");
        }
        return walls; 
    }
    private static List<enemySave>loadEnemies(string save)
    {
        List<enemySave> enemies = new List<enemySave> { };
        int indexOf = save.IndexOf("\"enemies\"");
        string enemiesSave = save.Substring(indexOf, save.IndexOf("\"weapons\"") - indexOf);
        indexOf = enemiesSave.IndexOf("{");
        while (indexOf != -1)
        {
            int lastIndexOf= enemiesSave.IndexOf("}", enemiesSave.IndexOf("HPpoint") + 1);
            string currentEnemySave = enemiesSave.Substring(indexOf, lastIndexOf - indexOf + 1);
            enemies.Add(JsonUtility.FromJson<enemySave>(currentEnemySave));
            enemies[enemies.Count - 1].position = loadVector(currentEnemySave);
            enemies[enemies.Count - 1].patrulPoints = loadListPatrulPoint(currentEnemySave);
            enemiesSave = enemiesSave.Remove(indexOf, lastIndexOf - indexOf + 1);
            indexOf = enemiesSave.IndexOf("{");
        }
        return enemies;
    }
    private static List<weaponSave>loadWeapons(string save)
    {
        List<weaponSave> weapons = new List<weaponSave> { };
        int indexOf = save.IndexOf("\"weapons\"");
        string weaponsSave = save.Substring(indexOf, save.IndexOf("\"items\"") - indexOf);
        indexOf = weaponsSave.IndexOf("{");
        while (indexOf != -1)
        {
            int lastIndexOf = weaponsSave.IndexOf("}", weaponsSave.IndexOf("durability") + 1);
            string currentWeaponSave = weaponsSave.Substring(indexOf, lastIndexOf - indexOf + 1);
            weapons.Add(JsonUtility.FromJson<weaponSave>(currentWeaponSave));
            weapons[weapons.Count - 1].position = loadVector(currentWeaponSave);
            weapons[weapons.Count - 1].eulerRotation = loadVector(currentWeaponSave, "\"eulerRotation\"");
            weaponsSave = weaponsSave.Remove(indexOf, lastIndexOf - indexOf + 1);
            indexOf = weaponsSave.IndexOf("{");
        }
        return weapons;
    }
    private static List<itemSave>loadItems(string save)
    {
        List<itemSave> items = new List<itemSave> { };
        int indexOf = save.IndexOf("\"items\"");
        string itemsSave = save.Substring(indexOf, save.Length - indexOf);
        indexOf = itemsSave.IndexOf("{");
        while (indexOf != -1)
        {
            int lastIndexOf = itemsSave.IndexOf("}", itemsSave.IndexOf("count") + 1);
            string currentItemSave = itemsSave.Substring(indexOf, lastIndexOf - indexOf + 1);
            items.Add(JsonUtility.FromJson<itemSave>(currentItemSave));
            items[items.Count - 1].position = loadVector(currentItemSave);
            items[items.Count - 1].eulerRotation = loadVector(currentItemSave, "\"eulerRotation\"");
            itemsSave = itemsSave.Remove(indexOf, lastIndexOf - indexOf + 1);
            indexOf = itemsSave.IndexOf("{");
        }
        return items;
    }
    /*public static void loadSave()
    {
        loadStatus = true;
        //saveInit
        StreamReader reader = new StreamReader(@"C:\Users\muon1\Desktop\create\unity\Gubernia502\Gubernia502\Assets\saves\test.json");
        json = reader.ReadToEnd();
        sceneSave level = JsonUtility.FromJson<sceneSave>(json);
        if (level.levelName != null)
        {
            //loadMainHero
            level.mainHero = loadMainHero(json);
            Gubernia502.playerController.ermakLockControl.transform.position = level.mainHero.position.vector;
            Gubernia502.playerController.ermakLockControl.hpSystem.hitPoint = level.mainHero.HPpoint;
            Gubernia502.playerController.ermakLockControl.hpSystem.shieldDurability = level.mainHero.shieldDuration;
            Gubernia502.playerController.ermakLockControl.bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(0, level.mainHero.yRotation, 0);
            //loadWalls
            level.walls = loadWalls(json);
            if (level.walls.Count > 0)
            {
                for (int i = 0; i < level.walls.Count; i++)
                {
                    hitPointSystem wall = GameObject.Instantiate(Gubernia502.constData.levelObj[level.walls[i].phase - 1],
                        level.walls[i].position.vector, Quaternion.Euler(Vector3.zero)).GetComponent<hitPointSystem>();
                    wall.hitPoint = level.walls[i].HPpoint;
                }
            }
            //loadEnemies
            level.enemies = loadEnemies(json);
            if (level.enemies.Count > 0)
            {
                for (int i = 0; i < level.enemies.Count; i++)
                {
                    batrakBehavior enemy = GameObject.Instantiate(Gubernia502.constData.enemies[0],
                        level.enemies[i].position.vector,
                        Quaternion.Euler(Vector3.zero)).GetComponent<batrakBehavior>();
                    enemy.bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(new Vector3(0, level.enemies[i].yRotation, 0));
                    enemy.dmgSystem.hitPoint = level.enemies[i].HPpoint;
                    enemy.dmgSystem.shieldDurability = level.enemies[i].shieldDuration;
                    if (level.enemies[i].patrulPoints.Count > 0)
                    {
                        for(int j = 0; j < level.enemies[i].patrulPoints.Count; j++)
                        {
                            enemy.patrulPoint.Add(level.enemies[i].patrulPoints[j].vector);
                        }
                    }
                    switch (level.enemies[i].behavior)
                    {
                        case "desactiveIdle":
                            enemy.startBehavior = batrakBehavior.startMode.desactiveIdle;
                            break;
                        case "passivePatrul":
                            enemy.startBehavior = batrakBehavior.startMode.passivePatrul;
                            break;
                        case "stayOnPoint":
                            enemy.startBehavior = batrakBehavior.startMode.stayOnPoint;
                            break;
                    }
                    enemy.changeDefaultState();
                }
            }
            //loadWeapons
            level.weapons = loadWeapons(json);
            if (level.weapons.Count > 0)
            {
                for (int i = 0; i < level.weapons.Count; i++)
                {
                    collectibleWeaponsItem weapon = GameObject.Instantiate(Gubernia502.constData.gamingItems[level.weapons[i].id - 1],
                        level.weapons[i].position.vector,
                        Quaternion.Euler(level.weapons[i].eulerRotation.vector)).GetComponent<collectibleWeaponsItem>();
                    weapon.damageBuff = level.weapons[i].damageBuff;
                    weapon.speedBuff = level.weapons[i].speedBuff;
                    weapon.magSizeBuff = level.weapons[i].magSizeBuff;
                    weapon.accuracyBuff = level.weapons[i].accuracyBuff;
                    weapon.durabilityBuff = level.weapons[i].durabilityBuff;
                    weapon.ammoInMag = level.weapons[i].ammoInMag;
                    weapon.ammoId = level.weapons[i].ammoId;
                    weapon.durability = level.weapons[i].durability;
                }
            }
            //loadItems
            level.items = loadItems(json);
            if (level.items.Count > 0)
            {
                for (int i = 0; i < level.items.Count; i++)
                {
                    collectibleSimpleItem item = GameObject.Instantiate(Gubernia502.constData.gamingItems[level.items[i].id - 1],
                        level.items[i].position.vector,
                        Quaternion.Euler(level.items[i].eulerRotation.vector)).GetComponent<collectibleSimpleItem>();
                    item.count = level.items[i].count;
                }
            }
        }
        reader.Close();
        loadStatus = false ;
        Gubernia502.gameIsActive = true;
    }*/
    public static void loadSaveAsync(saveLoader saveLoader)
    {
        saveLoader.currentState = saveLoader.loadState.loadInit;
        StreamReader reader = new StreamReader(@"C:\Users\muon1\Desktop\create\unity\Gubernia502\Gubernia502\Assets\saves\"+
            Gubernia502.saveFileName+".json");
        json = reader.ReadToEnd();
        sceneSave level = JsonUtility.FromJson<sceneSave>(json);
        if (level != null)
        {
            level.mainHero = loadMainHero(json);
            level.walls = loadWalls(json);
            level.enemies = loadEnemies(json);
            level.weapons = loadWeapons(json);
            level.items = loadItems(json);
            saveLoader.waitHandler.Reset();
            Gubernia502.threadManager.startLoadLevel(saveLoader, level);
            saveLoader.waitHandler.WaitOne();
            reader.Close();
        }
        else
        {
            reader.Close();
            StreamWriter writer = new StreamWriter(@"C:\Users\muon1\Desktop\create\unity\Gubernia502\Gubernia502\Assets\saves\" +
            Gubernia502.saveFileName + ".json",false);
            writer.Write("{\n\t\"levelName\":\""+ Gubernia502.saveFileName + "\",\n\t\"walls\":"+
                "[\n\t],\n\t\"mainHero\": {\n\t\t\"position\": {\n\t\t\t\"x\": 0,\n\t\t\t\"y\":"+
                " 0,\n\t\t\t\"z\": 0\n\t\t},\n\t\t\"yRotation\": 0,\n\t\t\"shieldDuration\": 100," +
            "\n\t\t\"HPpoint\": 1\n\t},\n\t\"enemies\": [\n\t],\n\t\"weapons\": [\n\t],\n\t\"items\": [\n\t]\n}");
            Debug.Log("createSave \""+ Gubernia502.saveFileName + "\"");
            writer.Close();
        }
        loadStatus = false;
    }
    public static void loadMap()
    {
        GameObject.Instantiate(Gubernia502.constData.saveLoader).GetComponent<saveLoader>().startLoad();
    }
    public static void loadMainMenu()
    {
        GameObject.Instantiate(Gubernia502.constData.saveLoader).GetComponent<saveLoader>().startLoad(false);
    }
    public static void saveMap()
    {
        string str="{\n\t\"levelName\": \"" + Gubernia502.saveFileName + "\",\n\t\"walls\": [\n";
        if (Gubernia502.walls.Count > 0)
        {
            for(int i = 0; i <= Gubernia502.walls.Count-1; i++)
            {
                str=string.Concat(str, "\t\t{\n\t\t\t\"position\": {\n\t\t\t\t\"x\": " +
                    Gubernia502.walls[i].transform.position.x.ToString().Replace(",",".") +
                    ",\n\t\t\t\t\"y\": " + Gubernia502.walls[i].transform.position.y.ToString().Replace(",", ".")
                    + ",\n\t\t\t\t\"z\": " +Gubernia502.walls[i].transform.position.z.ToString().Replace(",", ".")
                    + "\n\t\t\t},\n\t\t\t\"HPpoint\": " +Gubernia502.walls[i].hitPoint + ",\n\t\t\t\"phase\": " 
                    + Gubernia502.walls[i].phase + "\n\t\t}");
                if (i != Gubernia502.walls.Count-1)
                {
                    str=string.Concat(str, ",");
                }
                str = string.Concat(str, "\n");
            }
        }
        str = string.Concat(str,"\t],\n\t\"mainHero\": {\n\t\t\"position\": {\n\t\t\t\"x\": "+
            Gubernia502.playerController.ermakLockControl.transform.position.x.ToString().Replace(",", ".") + ",\n\t\t\t\"y\": "+
            Gubernia502.playerController.ermakLockControl.transform.position.y.ToString().Replace(",", ".") + ",\n\t\t\t\"z\": "+
            Gubernia502.playerController.ermakLockControl.transform.position.z.ToString().Replace(",", ".") + 
            "\n\t\t},\n\t\t\"yRotation\": "+
            Gubernia502.playerController.ermakLockControl.bodyRotateScript.rotatedBody.eulerAngles.y.ToString().Replace(",", ".") + 
            ",\n\t\t\"shieldDuration\": "+Gubernia502.playerController.ermakLockControl.hpSystem.shieldDurability+",\n\t\t\"HPpoint\": "+
            Gubernia502.playerController.ermakLockControl.hpSystem.hitPoint+"\n\t},\n\t\"enemies\": [\n");
        if (Gubernia502.enemies.Count > 0)
        {
            for(int i = 0; i <= Gubernia502.enemies.Count-1; i++)
            {
                str = string.Concat(str, "\t\t{\n\t\t\t\"position\": {\n\t\t\t\t\"x\": " + 
                    Gubernia502.enemies[i].transform.position.x.ToString().Replace(",", ".") +
                    ",\n\t\t\t\t\"y\": " + Gubernia502.enemies[i].transform.position.y.ToString().Replace(",", ".") + 
                    ",\n\t\t\t\t\"z\": " +Gubernia502.enemies[i].transform.position.z.ToString().Replace(",", ".") + 
                    "\n\t\t\t},\n\t\t\t\"behavior\": \"" +Gubernia502.enemies[i].startBehavior + 
                    "\",\n\t\t\t\"yRotation\": " +
                    Gubernia502.enemies[i].bodyRotateScript.rotatedBody.transform.eulerAngles.y.ToString().Replace(",", ".") + 
                    ",\n\t\t\t\"shieldDuration\": " +
                    Gubernia502.enemies[i].dmgSystem.shieldDurability + ",\n\t\t\t\"patrulPoints\": [\n");
                if (Gubernia502.enemies[i].patrulPoint.Count > 0)
                {
                    for(int j = 0; j <= Gubernia502.enemies[i].patrulPoint.Count-1; j++)
                    {
                        str = string.Concat(str, "\t\t\t\t{\n\t\t\t\t\t\"x\": " + 
                            Gubernia502.enemies[i].patrulPoint[j].x.ToString().Replace(",", ".") +
                            ",\n\t\t\t\t\t\"y\": " + Gubernia502.enemies[i].patrulPoint[j].y.ToString().Replace(",", ".") + 
                            ",\n\t\t\t\t\t\"z\": " +Gubernia502.enemies[i].patrulPoint[j].z.ToString().Replace(",", ".") + 
                            "\n\t\t\t\t}");
                        if (j != Gubernia502.enemies[i].patrulPoint.Count-1)
                        {
                            str = string.Concat(str, ",");
                        }
                        str = string.Concat(str, "\n");
                    }
                }
                str = string.Concat(str, "\t\t\t],\n\t\t\t\"HPpoint\": " + Gubernia502.enemies[i].dmgSystem.hitPoint +
                    "\n\t\t}");
                if (i != Gubernia502.enemies.Count-1)
                {
                    str = string.Concat(str, ",");
                }
                str = string.Concat(str, "\n");
            }
        }
        str = string.Concat(str, "\t],\n\t\"weapons\": [\n");
        if (Gubernia502.weapons.Count > 0)
        {
            for(int i = 0; i <= Gubernia502.weapons.Count-1; i++)
            {
                str = string.Concat(str, "\t\t{\n\t\t\t\"id\": " + Gubernia502.weapons[i].item.id +
                    ",\n\t\t\t\"eulerRotation\": {\n\t\t\t\t\"x\": " + 
                    Gubernia502.weapons[i].transform.eulerAngles.x.ToString().Replace(",", ".") +",\n\t\t\t\t\"y\": " + 
                    Gubernia502.weapons[i].transform.eulerAngles.y.ToString().Replace(",", ".") + ",\n\t\t\t\t\"z\": " +
                    Gubernia502.weapons[i].transform.eulerAngles.z.ToString().Replace(",", ".") + 
                    "\n\t\t\t},\n\t\t\t\"position\": {\n\t\t\t\t\"x\": " +
                    Gubernia502.weapons[i].transform.position.x.ToString().Replace(",", ".") + 
                    ",\n\t\t\t\t\"y\": " + Gubernia502.weapons[i].transform.position.y.ToString().Replace(",", ".") +
                    ",\n\t\t\t\t\"z\": " + Gubernia502.weapons[i].transform.position.z.ToString().Replace(",", ".") + 
                    "\n\t\t\t},\n\t\t\t\"damageBuff\": " +Gubernia502.weapons[i].damageBuff + ",\n\t\t\t\"speedBuff\": " + 
                    Gubernia502.weapons[i].speedBuff +",\n\t\t\t\"magSizeBuff\": " + Gubernia502.weapons[i].magSizeBuff + 
                    ",\n\t\t\t\"accuracyBuff\": " + Gubernia502.weapons[i].accuracyBuff + ",\n\t\t\t\"durabilityBuff\": " +
                    Gubernia502.weapons[i].durabilityBuff + ",\n\t\t\t\"ammoInMag\": " + Gubernia502.weapons[i].ammoInMag +
                    ",\n\t\t\t\"ammoId\": " + Gubernia502.weapons[i].ammoId + ",\n\t\t\t\"durability\": " +
                    Gubernia502.weapons[i].durability + "\n\t\t}");
                if (i != Gubernia502.weapons.Count-1)
                {
                    str = string.Concat(str, ",");
                }
                str = string.Concat(str, "\n");
            }
        }
        str = string.Concat(str, "\t],\n\t\"items\": [\n");
        if (Gubernia502.items.Count > 0)
        {
            for(int i = 0; i <= Gubernia502.items.Count-1; i++)
            {
                str = string.Concat(str, "\t\t{\n\t\t\t\"position\": {\n\t\t\t\t\"x\": " + 
                    Gubernia502.items[i].transform.position.x.ToString().Replace(",", ".") +
                    ",\n\t\t\t\t\"y\": " + Gubernia502.items[i].transform.position.y.ToString().Replace(",", ".") + ",\n\t\t\t\t\"z\": " +
                    Gubernia502.items[i].transform.position.z.ToString().Replace(",", ".") + 
                    "\n\t\t\t},\n\t\t\t\"eulerRotation\": {\n\t\t\t\t\"x\": " +
                    Gubernia502.items[i].transform.eulerAngles.x.ToString().Replace(",", ".") + 
                    ",\n\t\t\t\t\"y\": " + Gubernia502.items[i].transform.eulerAngles.y.ToString().Replace(",", ".") +
                    ",\n\t\t\t\t\"z\": " + Gubernia502.items[i].transform.eulerAngles.z.ToString().Replace(",", ".") + 
                    "\n\t\t\t},\n\t\t\t\"id\": " +
                    Gubernia502.items[i].item.id + ",\n\t\t\t\"count\": " + Gubernia502.items[i].count + "\n\t\t}");
                if (i != Gubernia502.items.Count-1)
                {
                    str = string.Concat(str, ",");
                }
                str = string.Concat(str, "\n");
            }
        }
        str = string.Concat(str, "\t]\n}");
        Debug.Log(str);
        StreamWriter writer = new StreamWriter(@"C:\Users\muon1\Desktop\create\unity\Gubernia502\Gubernia502\Assets\saves\" +
            Gubernia502.saveFileName + ".json", false);
        writer.Write(str);
        writer.Close();
    }
    /*public static void createEmptySave()
    {
        FileInfo newJson = new FileInfo("emptySave.json");
        newJson.MoveTo(@"C:\Users\muon1\Desktop\create\unity\Gubernia502\Gubernia502\Assets\saves\emptySave.json");
        newJson.Create();
        StreamWriter streamWriter = newJson.CreateText();
        streamWriter.WriteLine("{\n\t\"levelName\":\"test\",\n\t\"walls\": [\n\t],\n\t\"mainHero\": {\n\t\t\"position\": {" +
            "\n\t\t\t\"x\": 0,\n\t\t\t\"y\": 0,\n\t\t\t\"z\": 0\n\t\t},\n\t\t\"yRotation\": 0,\n\t\t\"shieldDuration\": 100," +
            "\n\t\t\"HPpoint\": 1\n\t},\n\t\"enemies\": [\n\t],\n\t\"weapons\": [\n\t],\n\t\"items\": [\n\t]\n}");
        StreamReader streamReader = newJson.OpenText();
        string s = "";
        while ((s = streamReader.ReadLine()) != null)
        {
            Console.WriteLine(s);
        }
    }*/
}
