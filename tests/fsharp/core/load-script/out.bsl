
1.fsx


printfn "Hello"

2.fsx


#load "1.fsx"
printfn "World"

3.fsx


#load "2.fsx"
printfn "-the end"
Test 1=================================================
Hello
World
-the end
Test 2=================================================
Hello
World
-the end
Test 3=================================================

> [Loading D:\staging\staging\src\tests\fsharp\core\load-script\1.fsx
 Loading D:\staging\staging\src\tests\fsharp\core\load-script\2.fsx
 Loading D:\staging\staging\src\tests\fsharp\core\load-script\3.fsx]
Hello
World
-the end

namespace FSI_0002


namespace FSI_0002


namespace FSI_0002

> 
Test 4=================================================
Test 5=================================================

usesfsi.fsx(1,1): error FS0039: The namespace or module 'fsi' is not defined
Test 6=================================================
Test 7=================================================
Hello
Test 8=================================================
Hello
World
-the end
Test 9=================================================
Hello
World
-the end
Test 10=================================================
Hello
World
-the end
Test 11=================================================
COMPILED is defined
Test 12=================================================
COMPILED is defined
Test 13=================================================
INTERACTIVE is defined
Test 14=================================================
INTERACTIVE is defined
Test 15=================================================
Result = 99
Type from referenced assembly = System.Web.Mobile.CookielessData
Test 16=================================================
Result = 99
Type from referenced assembly = System.Web.Mobile.CookielessData
Done ==================================================
