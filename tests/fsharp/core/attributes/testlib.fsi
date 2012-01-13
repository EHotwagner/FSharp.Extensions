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
   [<System.ObsoleteAttribute("Text identical in both")>]
   val x1 : int

   [<System.ObsoleteAttribute("Text differs in signature")>]
   val x2 : int

   //[<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   val x3 : int

   [<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   val x4 : int

module TyconAttributesDifferent = 
   [<System.ObsoleteAttribute("Text identical in both")>]
   type C1 = A | B

   [<System.ObsoleteAttribute("Text differs in signature")>]
   type C2 = A | B

   //[<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   type C3 = A | B

   [<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   type C4 = A | B

module ModuleAttributesDifferent = 
   [<System.ObsoleteAttribute("Text identical in both")>]
   module M1 = 
       val x : int

   [<System.ObsoleteAttribute("Text differs in signature")>]
   module M2 = 
       val x : int

   //[<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
   module M3 = 
       val x : int

   [<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
   module M4 = 
       val x : int

module UnionCaseAttributesDifferent = 
   // expect no warning, and attribute to be in compiled code
   type U1 =    
       | [<System.ObsoleteAttribute("Text identical in both")>]
         A of int
       | B of string

   // expect warning, and attribute from signature to be included
   type U2 =    
       | [<System.ObsoleteAttribute("Text differs in signature")>]
         A of int
       | B of string

   // expect no warning, and attribute to be in compiled code
   type U3 =    
       // [<System.ObsoleteAttribute("Attribute is in implementation but not signature")>]
       | A of int
       | B of string

   // expect no warning, and attribute to be in compiled code
   type U4 =    
       | [<System.ObsoleteAttribute("Attribute is in signature but not implementation")>]
         A of int
       | B of string

module ParamAttributesDifferent = 
   
   // identical in signature and implementation
   val x1 : [<System.CLSCompliantAttribute(true)>] p : int -> int

   // differs in signature
   val x2 : [<System.CLSCompliantAttribute(true)>] p : int -> int

   // missing in signature
   val x3 : (* [<System.CLSCompliantAttribute(true)>] *) p : int -> int

   // in signature but not implementation
   val x4 : [<System.CLSCompliantAttribute(true)>] p : int -> int

module TypeParamAttributesDifferent = 
   
   // identical in signature and implementation
   val x1< [<System.CLSCompliantAttribute(true)>] 'T> : 'T -> 'T

   // differs in signature
   val x2< [<System.CLSCompliantAttribute(true)>] 'T> : 'T -> 'T

   // missing in signature
   val x3< (* [<System.CLSCompliantAttribute(true)>] *) 'T> : 'T -> 'T

   // in signature but not implementation
   val x4< [<System.CLSCompliantAttribute(true)>] 'T> : 'T -> 'T

type ThisLibAssembly 
