# CGameCtnChallenge (0x03043000 / 0x24003000)

The class behind every single map made in Trackmania.

## Chunks

- [0x001 - header chunk (Virtual Skipper)](#0x001---header-chunk-virtual-skipper)
- [0x002 - header chunk (map info)](#0x002---header-chunk-map-info)
- [0x003 - header chunk (common)](#0x003---header-chunk-common)
- [0x004 - header chunk (version)](#0x004---header-chunk-version)
- [0x005 - header chunk (XML)](#0x005---header-chunk-xml)
- [0x007 - header chunk (thumbnail)](#0x007---header-chunk-thumbnail)
- [0x008 - header chunk (author)](#0x008---header-chunk-author)
- [0x00D (vehicle)](#0x00D-vehicle)
- [0x00F (TM1.0 block data)](#0x00F-tm10-block-data)
- [0x011 (parameters)](#0x011-parameters)
- [0x012 (TM1.0 map name)](#0x012-tm10-map-name)
- [0x013 (legacy block data)](#0x013-legacy-block-data)
- [0x014 - skippable (legacy password)](#0x014---skippable-legacy-password)
- [0x016 - skippable](#0x016---skippable)
- [0x017 - skippable (checkpoints)](#0x017---skippable-checkpoints)
- [0x019 - skippable (mod)](#0x019---skippable-mod)
- [0x01A](#0x01A)
- [0x01B](#0x01B)
- [0x01C - skippable (play mode)](#0x01C---skippable-play-mode)
- [0x01D](#0x01D)
- [0x01F (block data)](#0x01F-block-data)
- [0x021 (legacy mediatracker)](#0x021-legacy-mediatracker)
- [0x022](#0x022)
- [0x023 (map origin)](#0x023-map-origin)
- [0x024 (music)](#0x024-music)
- [0x025 (map origin and target)](#0x025-map-origin-and-target)
- [0x026 (clip global)](#0x026-clip-global)
- [0x027](#0x027)
- [0x028 (comments)](#0x028-comments)
- [0x029 - skippable (password)](#0x029---skippable-password)
- [0x02A](#0x02A)
- 0x034 - skippable
- [0x036 - skippable (realtime thumbnail)](#0x036---skippable-realtime-thumbnail)
- 0x038 - skippable
- [0x03D - skippable (lightmaps)](#0x03D---skippable-lightmaps)
- 0x03E - skippable
- [0x040 - skippable (items)](#0x040---skippable-items)
- [0x042 - skippable (author)](#0x042---skippable-author)
- 0x043 - skippable
- [0x044 - skippable (metadata)](#0x044---skippable-metadata)
- [0x048 - skippable (baked blocks)](#0x048---skippable-baked-blocks)
- [0x049 (mediatracker)](#0x049-mediatracker)
- [0x04B - skippable (objectives)](#0x04B---skippable-objectives)
- [0x050 - skippable (offzones)](#0x050---skippable-offzones)
- [0x051 - skippable (title info)](#0x051---skippable-title-info)
- [0x052 - skippable (deco height)](#0x052---skippable-deco-height)
- [0x053 - skippable (bot paths)](#0x053---skippable-bot-paths)
- [0x054 - skippable (embedded objects)](#0x054---skippable-embedded-objects)
- 0x055 - skippable
- [0x056 - skippable (light settings)](#0x056---skippable-light-settings)
- 0x057 - skippable
- 0x058 - skippable
- [0x059 - skippable](#0x059---skippable)
- 0x05A - skippable [TM2020]
- [0x05F - skippable (free blocks) [TM2020]](#0x05F---skippable-free-blocks-tm2020)

### 0x001 - header chunk (Virtual Skipper)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();

    if (version < 1)
    {
        Ident mapInfo = r.ReadIdent();
        string mapName = r.ReadString();
    }

    bool u01 = r.ReadBoolean();
    int u02 = r.ReadInt32();

    if (version < 1)
        byte u03 = r.ReadByte();

    byte u04 = r.ReadByte();

    if (version < 9)
        BoatName boatName = (BoatName)r.ReadByte();

    if (version >= 9)
        Id boat = r.ReadId();

    if (version >= 12)
        Id boatAuthor = r.ReadId();

    RaceMode raceMode = (RaceMode)r.ReadByte();
    byte u05 = r.ReadByte();
    WindDirection windDirection = (WindDirection)r.ReadByte();
    byte windStrength = r.ReadByte();
    Weather weather = (Weather)r.ReadByte();
    byte u06 = r.ReadByte();
    StartDelay startDelay = (StartDelay)r.ReadByte();
    int startTime = r.ReadInt32();

    if (version >= 2)
    {
        int timeLimit = r.ReadInt32();
        bool noPenalty = r.ReadBoolean();
        bool inflPenalty = r.ReadBoolean();
        bool finishFirst = r.ReadBoolean();

        if (version >= 3)
        {
            byte nbAIs = r.ReadByte();

            if (version >= 4)
            {
                float courseLength = r.ReadSingle();

                if (version >= 5)
                {
                    int windShiftAngle = r.ReadInt32();
                    byte u07 = r.ReadByte();

                    if (version == 6 || version == 7)
                    {
                        bool u08 = r.ReadBoolean();
                        string u09 = r.ReadString();
                    }

                    if (version >= 7)
                    {
                        bool exactWind = !r.ReadBoolean(); // an exact wind is inverted in chunk representation

                        if (version >= 10)
                        {
                            int spawnPoints = r.ReadInt32();

                            if (version >= 11)
                            {
                                AILevel aILevel = (AILevel)r.ReadByte();

                                if (version >= 13)
                                {
                                    bool smallShifts = r.ReadBoolean();

                                    if (version >= 14)
                                    {
                                        bool noRules = r.ReadBoolean();
                                        bool startSailUp = r.ReadBoolean();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Enums

```cs
enum BoatName : byte
{
    Acc,
    Multi,
    Melges,
    OffShore
}

enum RaceMode : byte
{
    FleetRace,
    MatchRace,
    TeamRace
}

enum WindDirection : byte
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

enum Weather : byte
{
    Sunny,
    Cloudy,
    Rainy,
    Stormy
}

enum StartDelay : byte
{
    Immediate,
    OneMin,
    TwoMin,
    FiveMin,
    EightMin
}

enum AILevel : byte
{
    Easy,
    Intermediate,
    Expert,
    Pro
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| byte u03 | ~ | ~ | ~ | ~ | ~
| byte u04 | ~ | ~ | ~ | ~ | ~
| byte u05 | ~ | ~ | ~ | ~ | ~
| byte u06 | ~ | ~ | ~ | ~ | ~
| byte u07 | ~ | ~ | ~ | ~ | ~
| bool u08 | ~ | ~ | ~ | ~ | ~
| string u09 | ~ | ~ | ~ | ~ | ~

### 0x002 - header chunk (map info)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();

    if (version < 3)
    {
        Ident mapInfo = r.ReadIdent();
        string mapName = r.ReadString();
    }

    int u01 = r.ReadInt32();

    if (version >= 1)
    {
        int bronzeTime = r.ReadInt32();
        int silverTime = r.ReadInt32();
        int goldTime = r.ReadInt32();
        int authorTime = r.ReadInt32();

        if (version == 2)
            byte u02 = r.ReadByte();

        if (version >= 4)
        {
            int cost = r.ReadInt32();

            if (version >= 5)
            {
                bool isLapRace = r.ReadBoolean();

                if (version == 6)
                    int u03 = r.ReadInt32();

                if (version >= 7)
                {
                    int trackType = r.ReadInt32();

                    if (version >= 9)
                    {
                        int u04 = r.ReadInt32();

                        if (version >= 10)
                        {
                            int authorScore = r.ReadInt32();

                            if (version >= 11)
                            {
                                int editorMode = r.ReadInt32(); // bit 0: advanced/simple editor, bit 1: has ghost blocks

                                if (version >= 12)
                                {
                                    int u05 = r.ReadInt32();

                                    if (version >= 13)
                                    {
                                        int nbCheckpoints = r.ReadInt32();
                                        int nbLaps = r.ReadInt32();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~
| byte u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| int u04 | ~ | ~ | ~ | ~ | ~
| int u05 | ~ | ~ | ~ | ~ | ~

### 0x003 - header chunk (common)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();
    Ident mapInfo = r.ReadIdent();
    string mapName = r.ReadString();
    TrackKind kind = (TrackKind)r.ReadByte();

    if (version >= 1)
    {
        uint locked = r.ReadUInt32(); // Gives a big integer sometimes, can't confirm to be always boolean
        string password = r.ReadString();

        if (version >= 2)
        {
            Ident decoration = r.ReadIdent();

            if (version >= 3)
            {
                Vec2 mapOrigin = r.ReadVec2();

                if (version >= 4)
                {
                    Vec2 mapTarget = r.ReadVec2();

                    if (version >= 5)
                    {
                        byte[] u01 = r.ReadBytes(16);

                        if (version >= 6)
                        {
                            string mapType = r.ReadString();
                            string mapStyle = r.ReadString();

                            if (version <= 8)
                                bool u02 = r.ReadBoolean();
                            
                            if (version >= 8)
                            {
                                ulong lightmapCacheUID = r.ReadUInt64();

                                if (version >= 9)
                                {
                                    byte lightmapVersion = r.ReadByte();

                                    if (version >= 11)
                                        Id titleUID = r.ReadId();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Enums

```cs
enum TrackKind : byte
{
    EndMarker,
    Campaign,
    Puzzle,
    Retro,
    TimeAttack,
    Rounds,
    InProgress,
    Campaign_7,
    Multi,
    Solo,
    Site,
    SoloNadeo,
    MultiNadeo
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| byte[] u01 | ~ | ~ | ~ | ~ | ~
| bool u02 | ~ | ~ | ~ | ~ | ~

### 0x004 - header chunk (version)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
}
```

### 0x005 - header chunk (XML)

```cs
void Read(GameBoxReader r)
{
    string xml = r.ReadString();
}

```

### 0x007 - header chunk (thumbnail)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    if(version != 0)
    {
        int thumbnailSize = r.ReadInt32();
        r.ReadBytes("<Thumbnail.jpg>".Length);
        byte[] thumbnailJpeg = r.ReadBytes(thumbnailSize);
        r.ReadBytes("</Thumbnail.jpg>".Length);
        r.ReadBytes("<Comments.jpg>".Length);
        string comments = r.ReadString();
        r.ReadBytes("</Comments.jpg>".Length);
    }
}
```

### 0x008 - header chunk (author)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int authorVersion = r.ReadInt32();
    string authorLogin = r.ReadString();
    string authorNick = r.ReadString();
    string authorZone = r.ReadString();
    string authorExtraInfo = r.ReadString();
}
```

### 0x00D (vehicle)

```cs
void Read(GameBoxReader r)
{
    Ident playerModel = r.ReadIdent();
}
```

### 0x00F (TM1.0 block data)

```cs
void Read(GameBoxReader r)
{
    Ident mapInfo = r.ReadIdent();
    Int3 size = r.ReadInt3();

    int version = r.ReadInt32();
    int numBlocks = r.ReadInt32();

    for (var i = 0; i < numBlocks; i++)
    {
        CGameCtnBlock block = r.ReadNodeRef<CGameCtnBlock>();
    }

    bool needUnlock = r.ReadBoolean();
    Ident decoration = r.ReadIdent();
}
```

### 0x011 (parameters)

```cs
void Read(GameBoxReader r)
{
    CGameCtnCollectorList collectorList = r.ReadNodeRef<CGameCtnCollectorList>();
    CGameCtnChallengeParameters challengeParameters = r.ReadNodeRef<CGameCtnChallengeParameters>();
    TrackKind kind = (TrackKind)r.ReadInt32();
}
```

#### Enums

```cs
enum TrackKind : int
{
    EndMarker,
    Campaign,
    Puzzle,
    Retro,
    TimeAttack,
    Rounds,
    InProgress,
    Campaign_7,
    Multi,
    Solo,
    Site,
    SoloNadeo,
    MultiNadeo
}
```

### 0x012 (TM1.0 map name)

```cs
void Read(GameBoxReader r)
{
    string mapName = r.ReadString();
}
```

### 0x013 (legacy block data)

```cs
void Read(GameBoxReader r)
{
    Ident mapInfo = r.ReadIdent();
    string mapName = r.ReadString();
    Ident decoration = r.ReadIdent();
    Int3 size = r.ReadInt3();
    bool needUnlock = r.ReadBoolean();

    int numBlocks = r.ReadInt32();

    for (int i = 0; i < numBlocks; i++)
    {
        Id blockName = r.ReadId();
        Direction dir = (Direction)r.ReadByte();
        Byte3 coord = r.ReadByte3();
        short flags = r.ReadInt16();

        if ((flags & 0x8000) != 0) // block with skin
        {
            Id author = r.ReadId();
            CGameCtnBlockSkin skin = r.ReadNodeRef<CGameCtnBlockSkin>();
        }
    }
}
```

### 0x014 - skippable (legacy password)

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    string password = r.ReadString();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x015 (is lap race)

```cs
void Read(GameBoxReader r)
{
    bool isLapRace = r.ReadBoolean();
}
```

### 0x016 - skippable

```cs
void Read(GameBoxReader r)
{
    bool u01 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x017 - skippable (checkpoints)

Checkpoint positions in coords. Available up to TMUF.

```cs
void Read(GameBoxReader r)
{
    int numCheckpoints = r.ReadInt32();

    for (var i = 0; i < numCheckpoints; i++)
    {
        Int3 checkpointCoord = r.ReadInt3();
    }
}
```

### 0x019 - skippable (mod)

```cs
void Read(GameBoxReader r)
{
    FileRef modPackDesc = r.ReadFileRef();
}
```

### 0x01A

This one is maybe related to some kind of replay record.

```cs
void Read(GameBoxReader r)
{
    Node u01 = r.ReadNodeRef(); // -1
}
```

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| Node u01 | ~ | ~ | ~ | ~ | ~

### 0x01B

According to RE, this one could crash the parse if it's not 0.

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32(); // 0
}
```

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x01C - skippable (play mode)

```cs
void Read(GameBoxReader r)
{
    PlayMode playMode = (PlayMode)r.ReadInt32();
}
```

#### Enums

```cs
enum PlayMode : int
{
    Race,
    Platform,
    Puzzle,
    Crazy,
    Shortcut,
    Stunts
}
```

### 0x01D

This one is maybe related to some kind of replay record.

```cs
void Read(GameBoxReader r)
{
    Node u01 = r.ReadNodeRef(); // -1
}
```

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| Node u01 | ~ | ~ | ~ | ~ | ~

### 0x01F (block data)

```cs
void Read(GameBoxReader r)
{
    Ident mapInfo = r.ReadIdent();
    string mapName = r.ReadString();
    Ident decoration = r.ReadIdent();
    Int3 size = r.ReadInt3();
    bool needUnlock = r.ReadBoolean();

    int version = r.ReadInt32();
    int numBlocks = r.ReadInt32(); // Amount of blocks that aren't flag -1

    while ((r.PeekUInt32() & 0xC0000000) > 0)
    {
        Id blockName = r.ReadId();
        Direction dir = (Direction)r.ReadByte();
        Byte3 coord = r.ReadByte3();
        int flags = r.ReadInt32();
        
        if (flags == -1)
        {
            continue;
        }

        if ((flags & 0x8000) != 0) // block with skin
        {
            Id author = r.ReadId();
            CGameCtnBlockSkin skin = r.ReadNodeRef<CGameCtnBlockSkin>();
        }

        if ((flags & 0x100000) != 0) // TM2 waypoint
        {
            CGameWaypointSpecialProperty parameters = r.ReadNodeRef<CGameWaypointSpecialProperty>();
        }
    }
}
```

#### Enums

```cs
enum Direction : byte
{
    North,
    East,
    South,
    West 
}
```

### 0x021 (legacy mediatracker)

```cs
void Read(GameBoxReader r)
{
    CGameCtnMediaClip clipIntro = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClipGroup clipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
    CGameCtnMediaClipGroup clipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();
}
```

### 0x022

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x023 (map origin)

```cs
void Read(GameBoxReader r)
{
    Vec2 mapCoordOrigin = r.ReadVec2();
    mapCoordTarget = mapCoordOrigin;
}
```

### 0x024 (music)

```cs
void Read(GameBoxReader r)
{
    FileRef customMusicPackDesc = r.ReadFileRef();
}
```

### 0x025 (map origin and target)

```cs
void Read(GameBoxReader r)
{
    Vec2 mapCoordOrigin = r.ReadVec2();
    Vec2 mapCoordTarget = r.ReadVec2();
}
```

### 0x026 (clip global)

This chunk hasn't been found in any investigated game version. We are looking for help.

```cs
void Read(GameBoxReader r)
{
    Node clipGlobal = r.ReadNodeRef();
}
```

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| Node clipGlobal | ~ | ~ | ~ | ~ | ~

### 0x027

This chunk seems to be related to the TM1 map thumbnail position.

```cs
void Read(GameBoxReader r)
{
    bool archiveGmCamVal = r.ReadBoolean();

    if (archiveGmCamVal)
    {
        byte u01 = r.ReadByte();
        Vec3 u02 = r.ReadVec3();
        Vec3 u03 = r.ReadVec3();
        Vec3 u04 = r.ReadVec3();
        Vec3 u05 = r.ReadVec3();
        float u06 = r.ReadSingle();
        float u07 = r.ReadSingle();
        float u08 = r.ReadSingle();
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| byte u01 | ~ | ~ | ~ | ~ | ~
| Vec3 u02 | ~ | ~ | ~ | ~ | ~
| Vec3 u03 | ~ | ~ | ~ | ~ | ~
| Vec3 u04 | ~ | ~ | ~ | ~ | ~
| Vec3 u05 | ~ | ~ | ~ | ~ | ~
| float u06 | ~ | ~ | ~ | ~ | ~
| float u07 | ~ | ~ | ~ | ~ | ~
| float u08 | ~ | ~ | ~ | ~ | ~

### 0x028 (comments)

```cs
void Read(GameBoxReader r)
{
    bool archiveGmCamVal = r.ReadBoolean();

    if (archiveGmCamVal)
    {
        byte u01 = r.ReadByte();
        Vec3 u02 = r.ReadVec3();
        Vec3 u03 = r.ReadVec3();
        Vec3 u04 = r.ReadVec3();
        Vec3 u05 = r.ReadVec3();
        float u06 = r.ReadSingle();
        float u07 = r.ReadSingle();
        float u08 = r.ReadSingle();
    }

    string comments = r.ReadString();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| byte u01 | ~ | ~ | ~ | ~ | ~
| Vec3 u02 | ~ | ~ | ~ | ~ | ~
| Vec3 u03 | ~ | ~ | ~ | ~ | ~
| Vec3 u04 | ~ | ~ | ~ | ~ | ~
| Vec3 u05 | ~ | ~ | ~ | ~ | ~
| float u06 | ~ | ~ | ~ | ~ | ~
| float u07 | ~ | ~ | ~ | ~ | ~
| float u08 | ~ | ~ | ~ | ~ | ~

### 0x029 - skippable (password)

If you want to remove a password from a map, you can just remove this chunk.

```cs
void Read(GameBoxReader r)
{
    byte[] passwordHashMD5 = r.ReadBytes(16);
    uint crc32 = r.ReadUInt32();
}
```

### 0x02A

```cs
void Read(GameBoxReader r)
{
    bool u01 = r.ReadBoolean();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x034 - skippable

Undiscovered.

### 0x036 - skippable (realtime thumbnail)

```cs
void Read(GameBoxReader r)
{
    Vec3 thumbnailPosition = r.ReadVec3();
    Vec3 thumbnailPitchYawRoll = r.ReadVec3(); // in radians
    float thumbnailFOV = r.ReadSingle();

    // + 31 more unknown bytes
}
```

### 0x038 - skippable

Undiscovered.

### 0x03D - skippable (lightmaps)

```cs
void Read(GameBoxReader r)
{
    bool u01 = reader.ReadBoolean(); // maybe if shadows are calculated
    int version = reader.ReadInt();

    int frames = 1; // Default value if version is below 5
    if (version >= 5)
        int frames = reader.ReadInt(); // Read normally

    if (version >= 2)
    {
        int size = 0;

        for (var i = 0; i < frames; i++)
        {
            int size = r.ReadInt32();
            byte[] image = r.ReadBytes(size);

            if (version >= 3)
            {
                int size = r.ReadInt32();
                byte[] image = r.ReadBytes(size);
            }

            if (version >= 6)
            {
                int size = r.ReadInt32();
                byte[] image = r.ReadBytes(size);
            }
        }

        if (size != 0)
        {
            int uncompressedSize = r.ReadInt32();
            int compressedSize = r.ReadInt32();
            byte[] lightmapCacheData = r.ReadBytes(compressedSize); // ZLIB compressed data

            using (var ms = new MemoryStream(data))
            using (var zlib = new InflaterInputStream(ms))
            using (var gbxr = new GameBoxReader(zlib))
                CHmsLightMapCache lightmapCache = Parse(gbxr);

            // + more data afterwards
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x03E - skippable

Undiscovered.

### 0x040 - skippable (items)

**Note: This chunk has it's own lookback.**

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    if (version != 0)
    {
        int u01 = r.ReadInt32();
        int size = r.ReadInt32();
        int u02 = r.ReadInt32();
        
        int numItems = r.ReadInt32();

        for (var i = 0; i < numItems; i++)
            CGameCtnAnchoredObject item = Parse<CGameCtnAnchoredObject>(r);

        int u03 = r.ReadInt32();
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~

### 0x042 - skippable (author)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int authorVersion = r.ReadInt32();
    string authorLogin = r.ReadString();
    string authorNickname = r.ReadString();
    string authorZone = r.ReadString();
    string authorExtraInfo = r.ReadString();
}
```

### 0x043 - skippable (genealogy)

**Note: This chunk has it's own lookback.**

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int size = r.ReadInt32();
    byte[] data = r.ReadBytes(size);

    using(var ms = new MemoryStream(data));
    using(var r2 = new GameBoxReader(ms, this));

    int numGenealogies = r2.ReadInt32();

    for(var i = 0; i < numGenealogies; i++)
        CGameCtnZoneGenealogy zoneGenealogy = Parse<CGameCtnZoneGenealogy>(r2);
}
```

Note: There's usually a LOT of zone genealogy classes which with a the current parser of GBX.NET can take over 3 seconds to read all.
You can take the `byte[] data` to process on a seperate thread, or just at a time it's needed.

### 0x044 - skippable (metadata)

Note: Purely theoretical read of ManiaPlanet 4.1+ metadata

```cs
void Read(GameBoxReader r)
{
    int unknown = r.ReadInt32();
    int size = r.ReadInt32();
    uint classID = r.ReadUInt32(); // CScriptTraitsIdentdata
    int version = r.ReadInt32();

    byte typeCount = r.ReadByte();
    Type[] types = new Types[typeCount];

    for (var i = 0; i < typeCount; i++)
    {
        byte varType;

        switch (varType)
        {
            case 7: // Array
                ReadScriptArray();
                break;
            case 15: // Struct
                ReadScriptStruct();
                break;
        }

        types[i] = ConstructType();
    }

    byte varCount = r.ReadByte();

    for (var i = 0; i < varCount; i++)
    {
        string metadataVarNam = r.ReadString(); // If smaller than 255, length must be read as byte, not integer!
        byte typeIndex = r.ReadByte();

        ReadType(types[typeIndex]);
    }

    void ReadScriptArray()
    {
        ScriptVariable indexVar;

        byte indexType = r.ReadByte(); // Array index type
        if (indexType == 15) // Struct
            ReadScriptStruct(); // Haven't tested this case, might bug out, but structs can be apparently used as an index

        byte arrayType = r.ReadByte(); // Array value type
        if (arrayType == 7) // Array
            ReadScriptArray();
        else if (arrayType == 15) // Struct
            ReadScriptStruct();

        ReadUntilByteIsNotZero(); // Sometimes the amount of zero bytes is 1 when struct array, could be a struct read issue, usually 0
    }

    ScriptStruct ReadScriptStruct(out int defaultLength)
    {
        byte numMembers = r.ReadByte();
        string structName = r.ReadString();

        for (var i = 0; i < numMembers; i++)
        {
            string memberName = r.ReadString();
            byte memberType = r.ReadByte();

            switch (memberType)
            {
                case 7: // Array
                    ReadScriptArray();
                    break;
                case 15: // Struct
                    ReadScriptStruct();
                    break;
            }

            switch (memberType)
            {
                case ScriptType.Integer:
                    int default = r.ReadInt32();
                    break;
                case ScriptType.Real:
                    float default = r.ReadString();
                    break;
                case ScriptType.Vec2:
                    Vec2 default = r.ReadVec2();
                    break;
                case ScriptType.Vec3:
                    Vec3 default = r.ReadVec3();
                    break;
                case ScriptType.Int3:
                    Int3 default = r.ReadInt3();
                    break;
                case ScriptType.Int2:
                    Int2 default = r.ReadInt2();
                    break;
                case ScriptType.Array:
                    break;
                case ScriptType.Struct:
                    break;
                default:
                    byte default = r.ReadByte();
                    break;
            }
        }

        ReadUntilByteIsNotZero();
    }

    Type ReadType(Type type)
    {
        switch (type.Type)
        {
            case ScriptType.Boolean:
                byte boolean = r.ReadByte();
                break;
            case ScriptType.Integer:
                int integer = r.ReadInt32();
                break;
            case ScriptType.Real:
                float real = r.ReadSingle();
                break;
            case ScriptType.Text:
                string str = r.ReadString(); // If smaller than 255, length must be read as byte, not integer!
                break;
            case ScriptType.Vec2:
                Vec2 vec2 = r.ReadVec2();
                break;
            case ScriptType.Vec3:
                Vec3 vec3 = r.ReadVec3();
                break;
            case ScriptType.Int3:
                Int3 int3 = r.ReadInt3();
                break;
            case ScriptType.Int2:
                Int2 int2 = r.ReadInt2();
                break;
            case ScriptType.Array:
                byte numElements = r.ReadByte();

                if (numElements > 0)
                {
                    if (type.Key == ScriptType.Void)
                    {
                        for (var i = 0; i < numElements; i++)
                            ReadType(type.Value);
                    }
                    else
                    {
                        ReadType(type.Key);
                        for (var i = 0; i < numElements; i++)
                            ReadType(type.Key);
                    }
                }
                break;
            case ScriptType.Struct:
                for (var i = 0; i < type.Members.Length; i++)
                    type.Members[i] = ReadType(type.Members[i]);
                break;
        }
    }
}
```

#### Enums

```cs
enum ScriptType
{
    Void,
    Boolean,
    Integer,
    Real,
    Class, // Not allowed for metadata
    Text,
    Enum,
    Array,
    ParamArray,
    Vec2,
    Vec3,
    Int3,
    Iso4, // Not allowed for metadata
    Ident, // Not allowed for metadata
    Int2,
    Struct
}
```

### 0x048 - skippable (baked blocks)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int u01 = r.ReadInt32();

    int numBakedBlocks = r.ReadInt32();

    for (var i = 0; i < numBakedBlocks; i++)
    {
        Id blockName = r.ReadId();
        Direction dir = (Direction)r.ReadByte();
        Byte3 coord = r.ReadByte3();
        int flags = r.ReadInt32();
    }

    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~

### 0x049 (mediatracker)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    CGameCtnMediaClip clipIntro = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClip clipPodium = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClipGroup clipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
    CGameCtnMediaClipGroup clipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();

    if (version >= 2)
    {
        CGameCtnMediaClip clipAmbiance = r.ReadNodeRef<CGameCtnMediaClip>();

        int triggerSizeX = r.ReadInt32();
        int triggerSizeY = r.ReadInt32();
        int triggerSizeZ = r.ReadInt32();
    }
}
```

### 0x04B - skippable (objectives)

```cs
void Read(GameBoxReader r)
{
    string objectiveTextAuthor = r.ReadString();
    string objectiveTextGold = r.ReadString();
    string objectiveTextSilver = r.ReadString();
    string objectiveTextBronze = r.ReadString();
}
```

### 0x050 - skippable (offzones)

Offzones are presented as boxes with two defined coordinates (start coord and end coord) forming a big 3D box to save bytes.

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int triggerSizeX = r.ReadInt32();
    int triggerSizeY = r.ReadInt32();
    int triggerSizeZ = r.ReadInt32();

    int numOffzones = r.ReadInt32();

    for (var i = 0; i < numOffzones; i++)
    {
        Int3 startCoord = r.ReadInt3();
        Int3 endCoord = r.ReadInt3();
    }
}
```

### 0x051 - skippable (title info)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    string titleID = r.ReadId();
    string buildVersion = r.ReadString();
}
```

### 0x052 - skippable (deco height)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int decoBaseHeightOffset = r.ReadInt32();
}
```

### 0x053 - skippable (bot paths)

Bot paths used (or usable) by Shootmania.

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    int numBotPaths = r.ReadInt32();
    for (var i = 0; i < numBotPaths; i++)
    {
        int clan = r.ReadInt32();

        int numPoints = r.ReadInt32(); // Forms a path
        for (var j = 0; j < numPoints; j++)
            Vec3 point = r.ReadVec3();

        bool isFlying = r.ReadBoolean();
        CGameWaypointSpecialProperty waypointSpecialProperty = r.ReadNodeRef<CGameWaypointSpecialProperty>();
        bool isAutonomous = r.ReadBoolean(); // Not tested
    }
}
```

### 0x054 - skippable (embedded objects)

The chunk behind item embedding. It also references a list of used textures, but **the textures itself aren't possible to embed**.

ZIP contains DEFLATE compressed item, block, and material GBX files with their relative path from the user data folder. The data can be extracted to a file data stream with a zip file extension and it will be recognized by any file archiver.

Embedded ZIP generated by the game doesn't include item icons. It is being removed to save bytes.

Older chunk versions can exist but haven't been discovered or proved yet. This structure works as it should for ManiaPlanet 4.1 and Trackmania®.

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int u01 = r.ReadInt32();
    int size = r.ReadInt32();

    int numEmbeddedItems = r.ReadInt32();
    for (var i = 0; i < numEmbeddedItems; i++)
        Ident embeddedItem = r.ReadIdent();

    int zipSize = r.ReadInt32();
    byte[] zip = r.ReadBytes(zipSize); // Classic ZIP archive

    int numTextures = r.ReadInt32();
    for (var i = 0; i < numTextures; i++)
        string texture = r.ReadString();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x055 - skippable

Undiscovered.

### 0x056 - skippable (light settings)

Dynamic lighting settings and exact map time provided since ManiaPlanet 4. This feature doesn't work and was partially removed from Trackmania®, causing the day time being always -1 and a day duration defaulting to 5 minutes.

Day time is an Int32 (or UInt16 with another undiscovered value) ranging from 0 to 65535, where 0 is 0:00:00 (hh:mm:ss) and 65535 is 23:59:59. This value can be also -1 representing that day time isn't supported.

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int u01 = r.ReadInt32();
    int dayTime = r.ReadInt32(); // 0..65535 = 0:00:00..23:59:59, -1: not supported
    int u02 = r.ReadInt32();
    bool dynamicDaylight = r.ReadBoolean(); // if the map will use dynamic lighting
    int dayDuration = r.ReadInt32(); // in milliseconds
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~

### 0x057 - skippable

Undiscovered.

### 0x058 - skippable

Undiscovered.

### 0x059 - skippable

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32(); // 3

    Vec3 u01 = r.ReadVec3();

    if (version != 0)
    {
        bool u02 = r.ReadBoolean();

        if (version >= 3)
        {
            float u03 = r.ReadSingle();
            float u04 = r.ReadSingle();
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| Vec3 u01 | ~ | ~ | ~ | ~ | ~
| bool u02 | ~ | ~ | ~ | ~ | ~
| float u03 | ~ | ~ | ~ | ~ | ~
| float u04 | ~ | ~ | ~ | ~ | ~

### 0x05A - skippable [TM2020]

Undiscovered.

### 0x05F - skippable (free blocks) [TM2020]

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    List<Vec3> vectors = new List<Vec3>();
    while (r.BaseStream.Position < r.BaseStream.Length) // read the skippable chunk to the end basically
        vectors.Add(r.ReadVec3());
}
```

This chunk data strictly relies on the 0x01F chunk data.

To understand the vectors, the amount of them is undefined in this chunk. The structure of the vector list looks something like this:
- 1st free block in 0x01F
    - Vec3 absolutePositionInMap
    - Vec3 pitchYawRoll
    - for each clip of that block
        - Vec3 clipPosition
        - Vec3 clipPitchYawRoll
- 2nd free block in 0x01F
    - Vec3 absolutePositionInMap
    - Vec3 pitchYawRoll
    - for each clip of that block
        - Vec3 clipPosition
        - Vec3 clipPitchYawRoll
- ...

You can't tell the amount of free blocks without the chunk 0x01F (because of the clips). Free block in the block flags is defined by the bit 29. If the bit is set, the block is a free block. Free blocks also have a coordinate (0, 0, 0).

You also can't tell the amount of free blocks without knowing the amount of clip the block model has. It is unsure where this information is available, but probably in the CGameCtnBlockInfo nodes which are available in the PAK files.

Therefore, to read the chunk with a known `CGameCtnBlock` and `CGameCtnBlockInfo`:

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    foreach (var block in Blocks.Where(x => x.IsFree))
    {
        Vec3 absolutePositionInMap = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3();

        foreach (var clip in block.BlockInfo.Clips)
        {
            Vec3 clipPosition = r.ReadVec3();
            Vec3 clipPointPitchYawRoll = r.ReadVec3();
        }
    }
}
```