
neg20.fs(30,28): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(31,32): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(32,28): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(32,32): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(34,24): error FS0001: This expression was expected to have type
    obj    
but here has type
    string    

neg20.fs(35,24): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(43,15): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(44,19): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(45,15): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(45,19): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(47,11): error FS0001: This expression was expected to have type
    obj    
but here has type
    string    

neg20.fs(48,11): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(52,24): error FS0001: This expression was expected to have type
    A    
but here has type
    B    

neg20.fs(53,38): error FS0001: This expression was expected to have type
    System.ValueType    
but here has type
    int    

neg20.fs(60,26): error FS0001: This expression was expected to have type
    B    
but here has type
    A    

neg20.fs(61,27): error FS0001: This expression was expected to have type
    B1    
but here has type
    B2    

neg20.fs(62,26): error FS0001: This expression was expected to have type
    C    
but here has type
    B    

neg20.fs(66,25): error FS0001: This expression was expected to have type
    A    
but here has type
    B    

neg20.fs(67,26): error FS0001: This expression was expected to have type
    B    
but here has type
    C    

neg20.fs(70,31): error FS0001: This expression was expected to have type
    B    
but here has type
    C    

neg20.fs(71,34): error FS0001: Type mismatch. Expecting a
    A list    
but given a
    B list    
The type 'A' does not match the type 'B'

neg20.fs(75,30): error FS0001: This expression was expected to have type
    B    
but here has type
    C    

neg20.fs(76,34): error FS0001: Type mismatch. Expecting a
    A list    
but given a
    B list    
The type 'A' does not match the type 'B'

neg20.fs(80,23): error FS0193: Type constraint mismatch. The type 
    C list    
is not compatible with type
    seq<B>    
The type 'C list' is not compatible with the type 'seq<B>'

neg20.fs(81,34): error FS0001: Type mismatch. Expecting a
    A list    
but given a
    B list    
The type 'A' does not match the type 'B'

neg20.fs(83,47): error FS0001: This expression was expected to have type
    B    
but here has type
    C    

neg20.fs(87,54): error FS0001: This expression was expected to have type
    B    
but here has type
    C    

neg20.fs(92,19): error FS0001: This expression was expected to have type
    A    
but here has type
    B    

neg20.fs(96,26): error FS0001: This expression was expected to have type
    B    
but here has type
    A    

neg20.fs(97,26): error FS0001: This expression was expected to have type
    A    
but here has type
    B    

neg20.fs(99,26): error FS0001: This expression was expected to have type
    B    
but here has type
    A    

neg20.fs(108,12): error FS0001: Type mismatch. Expecting a
    B * B -> 'a    
but given a
    A * A -> Data    
The type 'B' does not match the type 'A'

neg20.fs(109,12): error FS0001: Type mismatch. Expecting a
    A * B -> 'a    
but given a
    A * A -> Data    
The type 'B' does not match the type 'A'

neg20.fs(110,12): error FS0001: Type mismatch. Expecting a
    B * A -> 'a    
but given a
    A * A -> Data    
The type 'B' does not match the type 'A'

neg20.fs(128,19): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(129,19): error FS0001: This expression was expected to have type
    obj    
but here has type
    string    

neg20.fs(131,5): error FS0041: No overloads match for method 'OM3'. The available overloads are shown below (or in the Error List window).
Possible overload: 'static member C.OM3 : x:'b * y:int -> int'.
Possible overload: 'static member C.OM3 : x:'b * y:'b -> int'.
Type constraint mismatch. The type 
    obj    
is not compatible with type
    int    
The type 'obj' is not compatible with the type 'int'
Type constraint mismatch. The type 
    obj    
is not compatible with type
    'a    
The type 'obj' is not compatible with the type ''a'

neg20.fs(152,13): error FS0033: The type 'Test.BadNumberOfGenericParameters.C<_>' expects 1 type argument(s) but is given 2

neg20.fs(153,13): error FS0033: The type 'Test.BadNumberOfGenericParameters.C<_>' expects 1 type argument(s) but is given 2

neg20.fs(154,13): error FS0502: The member or object constructor 'SM1' takes 0 type argument(s) but is here given 1. The required signature is 'static member C.SM1 : unit -> int'.

neg20.fs(155,13): error FS0502: The member or object constructor 'SM2' takes 0 type argument(s) but is here given 1. The required signature is 'static member C.SM2 : y:int -> int'.

neg20.fs(156,13): error FS0502: The member or object constructor 'SM3' takes 0 type argument(s) but is here given 1. The required signature is 'static member C.SM3 : y:int * z:int -> int'.

neg20.fs(157,13): error FS0495: The member or object constructor 'SM3' has no argument or settable return property 'x'. The required signature is static member C.SM3 : y:int * z:int -> int.

neg20.fs(158,13): error FS0502: The member or object constructor 'SM4' takes 1 type argument(s) but is here given 2. The required signature is 'static member C.SM4 : y:'a * z:'b -> int'.

neg20.fs(159,13): error FS0502: The member or object constructor 'SM5' takes 2 type argument(s) but is here given 1. The required signature is 'static member C.SM5 : y:'a * z:'b -> int'.

neg20.fs(162,13): error FS0502: The member or object constructor 'M1' takes 0 type argument(s) but is here given 1. The required signature is 'member C.M1 : unit -> int'.

neg20.fs(163,13): error FS0502: The member or object constructor 'M2' takes 0 type argument(s) but is here given 1. The required signature is 'member C.M2 : y:int -> int'.

neg20.fs(164,13): error FS0502: The member or object constructor 'M3' takes 0 type argument(s) but is here given 1. The required signature is 'member C.M3 : y:int * z:int -> int'.

neg20.fs(165,13): error FS0495: The member or object constructor 'M3' has no argument or settable return property 'x'. The required signature is member C.M3 : y:int * z:int -> int.

neg20.fs(166,13): error FS0502: The member or object constructor 'M4' takes 1 type argument(s) but is here given 2. The required signature is 'member C.M4 : y:'a * z:'b -> int'.

neg20.fs(167,13): error FS0502: The member or object constructor 'M5' takes 2 type argument(s) but is here given 1. The required signature is 'member C.M5 : y:'a * z:'b -> int'.

neg20.fs(182,14): error FS0041: No overloads match for method 'M'. The available overloads are shown below (or in the Error List window).
Possible overload: 'static member C2.M : fmt:string * args:int [] -> string'.
Possible overload: 'static member C2.M : fmt:string * args:int [] -> string'.
Type constraint mismatch. The type 
    obj    
is not compatible with type
    int    
The type 'obj' is not compatible with the type 'int'
Type constraint mismatch. The type 
    obj    
is not compatible with type
    int []    
The type 'obj' is not compatible with the type 'int []'

neg20.fs(183,14): error FS0001: This expression was expected to have type
    unit    
but here has type
    string    

neg20.fs(184,28): error FS0001: This expression was expected to have type
    int    
but here has type
    obj    

neg20.fs(184,34): error FS0001: This expression was expected to have type
    int    
but here has type
    obj    

neg20.fs(188,14): error FS0041: No overloads match for method 'M'. The available overloads are shown below (or in the Error List window).
Possible overload: 'static member C3.M : fmt:string * args:string [] -> string'.
Possible overload: 'static member C3.M : fmt:string * args:string [] -> string'.
Type constraint mismatch. The type 
    obj    
is not compatible with type
    string    
The type 'obj' is not compatible with the type 'string'
Type constraint mismatch. The type 
    obj    
is not compatible with type
    string []    
The type 'obj' is not compatible with the type 'string []'

neg20.fs(189,14): error FS0001: This expression was expected to have type
    unit    
but here has type
    string    

neg20.fs(190,28): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(190,34): error FS0001: This expression was expected to have type
    string    
but here has type
    obj    

neg20.fs(195,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(198,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(202,7): error FS0825: The 'DefaultValue' attribute may only be used on 'val' declarations

neg20.fs(204,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(207,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(210,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(213,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(216,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(219,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(222,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(225,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(228,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(231,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(234,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(237,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(240,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(243,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(249,9): error FS0842: This attribute is not valid for use on this language element

neg20.fs(255,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(258,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(261,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(265,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(268,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(271,5): error FS0842: This attribute is not valid for use on this language element

neg20.fs(278,14): error FS0507: No accessible member or object constructor named 'ProcessStartInfo' takes 0 arguments. Note the call to this member also provides 2 named arguments.

neg20.fs(280,14): error FS0508: No accessible member or object constructor named 'ProcessStartInfo' takes 0 arguments. The named argument 'Argument' doesn't correspond to any argument or settable return property for any overload.

neg20.fs(285,12): error FS0038: 'x' is bound twice in this pattern

neg20.fs(286,14): error FS0038: 'x' is bound twice in this pattern

neg20.fs(288,17): error FS0038: 'x' is bound twice in this pattern

neg20.fs(294,5): error FS0840: Unrecognized attribute target. Valid attribute targets are 'assembly', 'module', 'type', 'method', 'property', 'return', 'param', 'field', 'event', 'constructor'.

neg20.fs(67,9): error FS0037: Duplicate definition of value 'v2b'

neg20.fs(68,9): error FS0037: Duplicate definition of value 'v2c'
