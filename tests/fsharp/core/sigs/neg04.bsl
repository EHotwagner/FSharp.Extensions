
neg04.fs(5,21): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(7,21): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(11,8): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(14,8): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(22,8): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(26,8): error FS0912: This declaration element is not permitted in an augmentation

neg04.fs(32,8): error FS0039: The field, constructor or member 'Nan' is not defined

neg04.fs(46,69): error FS0001: Type mismatch. Expecting a
    'a * 'b * 'c * 'e    
but given a
    'a * 'b * 'c    
The tuples have differing lengths of 4 and 3

neg04.fs(46,99): error FS0001: Type mismatch. Expecting a
    'a * 'b * 'c * 'e    
but given a
    'a * 'b * 'c    
The tuples have differing lengths of 4 and 3

neg04.fs(47,30): error FS0001: Type mismatch. Expecting a
    seq<'a> -> 'f    
but given a
    'g list -> 'h    
The type 'seq<'a>' does not match the type ''f list'

neg04.fs(61,25): error FS0001: This expression was expected to have type
    ClassType1    
but here has type
    Object    

neg04.fs(70,21): error FS0698: Invalid constraint: the type used for the constraint is sealed, which means the constraint could only be satisfied by at most one solution

neg04.fs(70,21): error FS0064: This construct causes code to be less generic than indicated by the type annotations. The type variable 'a has been constrained to be type 'c<string>'.

neg04.fs(70,12): error FS0663: This type parameter has been used in a way that constrains it to always be 'c<string>'

neg04.fs(70,21): error FS0698: Invalid constraint: the type used for the constraint is sealed, which means the constraint could only be satisfied by at most one solution

neg04.fs(76,19): error FS0698: Invalid constraint: the type used for the constraint is sealed, which means the constraint could only be satisfied by at most one solution

neg04.fs(76,19): error FS0064: This construct causes code to be less generic than indicated by the type annotations. The type variable 'a has been constrained to be type 'd'.

neg04.fs(76,10): error FS0663: This type parameter has been used in a way that constrains it to always be 'd'

neg04.fs(76,19): error FS0698: Invalid constraint: the type used for the constraint is sealed, which means the constraint could only be satisfied by at most one solution

neg04.fs(81,58): error FS0001: This expression was expected to have type
    int    
but here has type
    'a * 'b    

neg04.fs(83,39): error FS0752: The operator 'expr.[idx]' has been used an object of indeterminate type based on information prior to this program point. Consider adding further type constraints

neg04.fs(85,47): error FS0039: The field, constructor or member 'Item' is not defined

neg04.fs(87,73): error FS0752: The operator 'expr.[idx]' has been used an object of indeterminate type based on information prior to this program point. Consider adding further type constraints

neg04.fs(91,72): error FS0752: The operator 'expr.[idx]' has been used an object of indeterminate type based on information prior to this program point. Consider adding further type constraints

neg04.fs(121,12): error FS0193: Type constraint mismatch. The type 
    R    
is not compatible with type
    IBar    
The type 'R' is not compatible with the type 'IBar'

neg04.fs(126,12): error FS0193: Type constraint mismatch. The type 
    U    
is not compatible with type
    IBar    
The type 'U' is not compatible with the type 'IBar'

neg04.fs(131,12): error FS0193: Type constraint mismatch. The type 
    Struct    
is not compatible with type
    IBar    
The type 'Struct' is not compatible with the type 'IBar'

neg04.fs(135,10): error FS0193: Type constraint mismatch. The type 
    R    
is not compatible with type
    IBar    
The type 'R' is not compatible with the type 'IBar'

neg04.fs(138,10): error FS0193: Type constraint mismatch. The type 
    U    
is not compatible with type
    IBar    
The type 'U' is not compatible with the type 'IBar'

neg04.fs(141,10): error FS0193: Type constraint mismatch. The type 
    Struct    
is not compatible with type
    IBar    
The type 'Struct' is not compatible with the type 'IBar'

neg04.fs(144,10): error FS0193: Type constraint mismatch. The type 
    int * int    
is not compatible with type
    IBar    
The type 'int * int' is not compatible with the type 'IBar'

neg04.fs(147,10): error FS0193: Type constraint mismatch. The type 
    int []    
is not compatible with type
    IBar    
The type 'int []' is not compatible with the type 'IBar'

neg04.fs(150,10): error FS0193: Type constraint mismatch. The type 
    int -> int    
is not compatible with type
    IBar    
The type 'int -> int' is not compatible with the type 'IBar'

neg04.fs(159,47): error FS0692: This function value is being used to construct a delegate type whose signature includes a byref argument. You must use an explicit lambda expression taking 1 arguments.

neg04.fs(163,54): error FS0692: This function value is being used to construct a delegate type whose signature includes a byref argument. You must use an explicit lambda expression taking 1 arguments.

neg04.fs(178,13): error FS0001: The type '('a -> unit) when 'a :> IDuplex<'a> and 'a :> IServer<'a>' does not support the 'equality' constraint because it is a function type
