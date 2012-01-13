//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2011 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------

// F# internalsVisibleTo checks
printf "publicF   2          = %2d\n" (Library.M.publicF 2)
printf "internalF 2          = %2d\n" (Library.M.internalF 2)
printf "signatureInternalF 2 = %2d\n" (Library.M.signatureInternalF 2)

// C# PublicClass
printf   "APublicClass.InternalProperty   = %2d\n" LibraryCS.APublicClass.InternalProperty
//printf "APublicClass.PrivateProperty    = %2d\n" LibraryCS.APublicClass.PrivateProperty     // private members are not visible via InternalsVisibleTo

// C# InternalClass
printf   "AInternalClass.InternalProperty = %2d\n" LibraryCS.AInternalClass.InternalProperty
//printf "AInternalClass.PrivateProperty  = %2d\n" LibraryCS.AInternalClass.PrivateProperty   // private members are not visible via InternalsVisibleTo

// C# PrivateClass (is just an internal class)
printf   "APrivateClass.InternalProperty  = %2d\n" LibraryCS.APrivateClass.InternalProperty   // for types, private *IS* visible (private is internal)
//printf "APrivateClass.PrivateProperty   = %2d\n" LibraryCS.APrivateClass.PrivateProperty    // private members are not visible via InternalsVisibleTo

//printf "privateF  2 = %d\n"   (Library.M.privateF  2) // inaccessable


(* Check that internalVisibleTo items can be used in internal items *)
module internal Repro3737 =
  let internal internalModuleInternalVal_uses_csInternalType (x : LibraryCS.AInternalClass) = 123
  let internal internalModuleInternalVal_uses_fsInternalType (x : Library.P.InternalClass)  = 123
  let internal internalModuleInternalVal_uses_fsInternalObject = Library.P.InternalObject

  let internalModuleNormalVal_uses_csInternalType (x : LibraryCS.AInternalClass) = 123
  let internalModuleNormalVal_uses_fsInternalType (x : Library.P.InternalClass)  = 123
  let internalModuleNormalVal_uses_fsInternalObject = Library.P.InternalObject
