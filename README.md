# ftb
Simple file to c# byte array file generator

This is a small command line application that allows to take a file and convert as an equivalent c# byte array.

Arguments:

-i {s}            Input file name
-o {s}            Output file name
-col {i}          Variable count per line
-ns {s}           Namespace
-class {s}        Class name (Default to input file name)
-field {s}        Variable name (Default to input file name)
-classmod {s}     Class modifier (Defaults to public static partial)
-fieldmod {s}     Variable modifier (Default to public static readonly)

Example result:

```
using System;

namespace MyNamespace 
{
public partial static class GeneratedFile 
{ 
   public static byte[] MyBinaryData  = 
{ 
   77 , 90 , 144 , 0 , 3 , 
   0 , 0 , 0 , 4 , 0 , 
   0 , 0 , 255 , 255 , 0 , 
   0 , 184 , 0 , 0 , 0 
};
}
}
```
This is useful to quickly embed some binary data in an assembly without having to resort to any on disk mechanism, or some Assembly manifest loading.

Just access your field instead.

Licensed under "do whatever you want with it".
