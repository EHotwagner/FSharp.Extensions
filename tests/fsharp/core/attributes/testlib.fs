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

module TestLibModule

module ValAttributesDifferent = 
   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Text identical in both")>]
   let x1 = 1

   // expect warning, and attribute from signature to be included
   [<System.ObsoleteAttribute("Text differs in implementation")>]
   let x2 = 1

   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   let x3 = 1

   // expect no warning, and attribute to be in compiled code
   //[<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   let x4 = 1



module TyconAttributesDifferent = 
   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Text identical in both")>]
   type C1 = A | B

   // expect warning, and attribute from signature to be included
   [<System.ObsoleteAttribute("Text differs in implementation")>]
   type C2 = A | B

   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   type C3 = A | B

   // expect no warning, and attribute to be in compiled code
   //[<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   type C4 = A | B


module ModuleAttributesDifferent = 
   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Text identical in both")>]
   module M1 = 
       let x = 1

   // expect warning, and attribute from signature to be included
   [<System.ObsoleteAttribute("Text differs in implementation")>]
   module M2 = 
       let x = 1

   // expect no warning, and attribute to be in compiled code
   [<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   module M3 = 
       let x = 1

   // expect no warning, and attribute to be in compiled code
   //[<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   module M4 = 
       let x = 1


module UnionCaseAttributesDifferent = 
   // expect no warning, and attribute to be in compiled code
   type U1 =    
       | [<System.ObsoleteAttribute("Text identical in both")>]
         A of int
       | B of string

   // expect warning, and attribute from signature to be included
   type U2 =    
       | [<System.ObsoleteAttribute("Text differs in implementation")>]
         A of int
       | B of string

   // expect no warning, and attribute to be in compiled code
   type U3 =    
       | [<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
         A of int
       | B of string

   // expect no warning, and attribute to be in compiled code
   type U4 =    
       | //[<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
         A of int
       | B of string

module ParamAttributesDifferent = 
   
   // identical in signature and implementation
   let x1 ([<System.CLSCompliantAttribute(true)>] p : int) = p

   // differs in signature
   let x2 ([<System.CLSCompliantAttribute(false)>] p : int) = p

   // missing in signature
   let x3 ([<System.CLSCompliantAttribute(true)>] p : int) = p

   // in signature but not implementation
   let x4 ((* [<System.CLSCompliantAttribute(true)>] *) p : int) = p

module TypeParamAttributesDifferent = 
   
   // identical in signature and implementation
   let x1<[<System.CLSCompliantAttribute(true)>] 'T>(x:'T) = x

   // differs in signature
   let x2<[<System.CLSCompliantAttribute(false)>] 'T>(x:'T) = x

   // missing in signature
   let x3<[<System.CLSCompliantAttribute(true)>] 'T>(x:'T) = x

   // in signature but not implementation
   let x4<(* [<System.CLSCompliantAttribute(true)>] *) 'T>(x:'T) = x


type ThisLibAssembly = X | Y
