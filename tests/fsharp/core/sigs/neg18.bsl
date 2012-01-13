
neg18.fs(7,13): error FS0001: The type 'int' is not compatible with the type 'seq<'a>'

neg18.fs(25,21): error FS0033: The type 'Test.AmbiguousTypeNameTests.TwoAmbiguousGenericTypes.M.C<_>' expects 1 type argument(s) but is given 0

neg18.fs(27,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(33,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(39,21): error FS0033: The type 'Test.AmbiguousTypeNameTests.TwoAmbiguousGenericTypes.M.C<_,_>' expects 2 type argument(s) but is given 0

neg18.fs(41,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(47,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(54,21): error FS0033: The type 'Test.AmbiguousTypeNameTests.TwoAmbiguousGenericTypes.M.C<_,_>' expects 2 type argument(s) but is given 0

neg18.fs(56,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(62,17): error FS1124: Multiple types exist called 'C', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'C<_>'.

neg18.fs(121,21): error FS0033: The type 'Test.AmbiguousTypeNameTests.OneNonAmbiguousGenericType.M3.C<_>' expects 1 type argument(s) but is given 0

neg18.fs(125,17): error FS1125: The instantiation of the generic type 'C' is missing and can't be inferred from the arguments or return type of this member. Consider providing a type instantiation when accessing this type, e.g. 'C<_>'.

neg18.fs(130,21): error FS0033: The type 'Test.AmbiguousTypeNameTests.OneNonAmbiguousGenericType.M3.C<_>' expects 1 type argument(s) but is given 0

neg18.fs(134,17): error FS1125: The instantiation of the generic type 'C' is missing and can't be inferred from the arguments or return type of this member. Consider providing a type instantiation when accessing this type, e.g. 'C<_>'.

neg18.fs(140,13): error FS0033: The type 'Test.AmbiguousTypeNameTests.OneNonAmbiguousGenericType.M3.C<_>' expects 1 type argument(s) but is given 0

neg18.fs(144,17): error FS1125: The instantiation of the generic type 'C' is missing and can't be inferred from the arguments or return type of this member. Consider providing a type instantiation when accessing this type, e.g. 'C<_>'.

neg18.fs(154,19): error FS1124: Multiple types exist called 'Foo', taking different numbers of generic parameters. Provide a type instantiation to disambiguate the type resolution, e.g. 'Foo<_>'.

neg18.fs(160,26): error FS1125: The instantiation of the generic type 'T' is missing and can't be inferred from the arguments or return type of this member. Consider providing a type instantiation when accessing this type, e.g. 'T<_>'.

neg18.fs(170,10): error FS0086: The '.[]' operator cannot be redefined. Consider using a different operator name

neg18.fs(182,28): error FS0064: This construct causes code to be less generic than indicated by the type annotations. The type variable 'a has been constrained to be type ''b'.

neg18.fs(185,26): error FS0064: This construct causes code to be less generic than indicated by the type annotations. The type variable 'b has been constrained to be type ''a'.

neg18.fs(186,12): error FS0064: This construct causes code to be less generic than indicated by the type annotations. The type variable 'a has been constrained to be type ''b'.
