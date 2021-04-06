using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [Flags]
    public enum InputType
    {
        None = 0,
        MOB = 1,
        OBJ = 2,
        TEXT = 4,
        NUM = 8,
        FILE = 16,
        TURF = 32,
        KEY = 64,
        NULL = 128,
        AREA = 256,
        ICON = 512,
        SOUND = 1024,
        MESSAGE = 2048,
        ANYTHING = 4096,
        PASSWORD = 32768,
        COMMAND_TEXT = 65536,
        COLOR = 131072
    }
}
