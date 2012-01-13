
neg08.fs(7,24): error FS0033: The type 'System.Collections.Generic.List<_>' expects 1 type argument(s) but is given 0

neg08.fs(9,24): error FS0033: The type 'System.Collections.Generic.List<_>' expects 1 type argument(s) but is given 2

neg08.fs(16,8): error FS0670: This code is not sufficiently generic. The type variable 'b could not be generalized because it would escape its scope.

neg08.fs(21,1): error FS0919: Exception abbreviations must refer to existing exceptions or F# types deriving from System.Exception

neg08.fs(28,16): error FS0790: This type is not a record type. Values of class and struct types must be created using calls to object constructors.

neg08.fs(30,9): error FS0789: '{ }' is not a valid expression. Records must include at least one field. Empty sequences are specified by using Seq.empty or an empty list '[]'.

neg08.fs(43,18): error FS0859: No abstract property was found that corresponds to this override

neg08.fs(48,18): error FS0855: No abstract or interface member was found that corresponds to this override

neg08.fs(55,5): error FS0773: Cannot create an extension of a sealed type

neg08.fs(55,5): error FS0776: Object construction expressions may only be used to implement constructors in class types

neg08.fs(61,19): error FS0039: The field, constructor or member 'value__' is not defined

neg08.fs(79,5): error FS0502: The member or object constructor 'Random' takes 0 type argument(s) but is here given 1. The required signature is 'static member Variable.Random : y:Variable<'a> -> Variable<'a>'.

neg08.fs(5,5): error FS0034: Module 'Neg08' contains
    val push : 'a -> '_b -> unit    
but its signature specifies
    val push : 'a -> 'a -> unit    
The types differ
