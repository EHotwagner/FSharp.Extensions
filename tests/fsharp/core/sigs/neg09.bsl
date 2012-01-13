
neg09.fsi(4,5): error FS0201: Namespaces cannot contain values. Consider using a module to hold your value declarations.

neg09.fsi(5,5): error FS0201: Namespaces cannot contain values. Consider using a module to hold your value declarations.

neg09.fs(4,5): error FS0201: Namespaces cannot contain values. Consider using a module to hold your value declarations.

neg09.fs(6,5): error FS0201: Namespaces cannot contain values. Consider using a module to hold your value declarations.

neg09.fs(20,32): error FS0941: Accessibility modifiers are not permitted on overrides or interface implementations

neg09.fs(26,8): error FS0497: The member or object constructor 'NamedMeth1' requires 1 additional argument(s). The required signature is 'abstract member IFoo.NamedMeth1 : arg1:int * arg2:int * arg3:int * arg4:int -> float'.

neg09.fs(49,19): error FS1173: Infix operator member '+' has 1 initial argument(s). Expected a tuple of 2 arguments, e.g. static member (+) (x,y) = ...

neg09.fs(49,19): error FS1174: Infix operator member '+' has extra curried arguments. Expected a tuple of 2 arguments, e.g. static member (+) (x,y) = ...

neg09.fs(38,16): error FS0071: Type constraint mismatch when applying the default type 'int' for a type inference variable. The type 'Quotations.Expr<int>' does not match the type 'int' Consider adding further type constraints

neg09.fs(2,11): error FS0193: Module 'N' requires a value 'member C.M : int'
