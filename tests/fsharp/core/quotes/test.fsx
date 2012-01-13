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

#r "cslib.dll"


#nowarn "57"
let mutable failures = []
let report_failure s = 
    stderr.WriteLine " NO"; failures <- s :: failures
let test s b = stderr.Write(s:string);  if b then stderr.WriteLine " OK" else report_failure s
let check s v1 v2 = 
   stderr.Write(s:string);  
   if (v1 = v2) then 
       stderr.WriteLine " OK" 
   else
       eprintf " FAILED: got %A, expected %A" v1 v2 
       report_failure s


let argv = System.Environment.GetCommandLineArgs() 
let SetCulture() = 
    if argv.Length > 2 && argv.[1] = "--culture" then  
        let cultureString = argv.[2] 
        let culture = new System.Globalization.CultureInfo(cultureString) 
        stdout.WriteLine ("Running under culture "+culture.ToString()+"...");
        System.Threading.Thread.CurrentThread.CurrentCulture <-  culture
  
do SetCulture()    


open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns

module TypedTest = begin 




    let x = <@ 1 @>

    test "check SByte"   ((<@  1y   @> |> (function SByte 1y -> true | _ -> false))) 
    test "check Int16"   ((<@  1s   @> |> (function Int16 1s -> true | _ -> false))) 
    test "check Int32"   ((<@  1   @> |> (function Int32 1 -> true | _ -> false))) 
    test "check Int64"   ((<@  1L   @> |> (function Int64 1L -> true | _ -> false))) 
    test "check Byte"     ((<@  1uy   @> |> (function Byte 1uy -> true | _ -> false))) 
    test "check UInt16"   ((<@  1us   @> |> (function UInt16 1us -> true | _ -> false))) 
    test "check UInt32"   ((<@  1u   @> |> (function UInt32 1u -> true | _ -> false))) 
    test "check UInt64"   ((<@  1UL   @> |> (function UInt64 1UL -> true | _ -> false))) 
    test "check String"  ((<@  "1" @> |> (function String "1" -> true | _ -> false))) 

    test "check ~SByte"   ((<@  "1"   @> |> (function SByte _ -> false | _ -> true))) 
    test "check ~Int16"   ((<@  "1"   @> |> (function Int16 _ -> false | _ -> true))) 
    test "check ~Int32"   ((<@  "1"   @> |> (function Int32 _ -> false | _ -> true))) 
    test "check ~Int64"   ((<@  "1"   @> |> (function Int64 _ -> false | _ -> true))) 
    test "check ~Byte"   ((<@  "1"   @> |> (function Byte _ -> false | _ -> true))) 
    test "check ~UInt16"   ((<@  "1"   @> |> (function UInt16 _ -> false | _ -> true))) 
    test "check ~UInt32"   ((<@  "1"   @> |> (function UInt32 _ -> false | _ -> true))) 
    test "check ~UInt64"   ((<@  "1"   @> |> (function UInt64 _ -> false | _ -> true))) 
    test "check ~String" ((<@  1   @> |> (function String "1" -> false | _ -> true))) 

    test "check AndAlso" ((<@ true && true  @> |> (function AndAlso(Bool(true),Bool(true)) -> true | _ -> false))) 
    test "check OrElse"  ((<@ true || true  @> |> (function OrElse(Bool(true),Bool(true)) -> true | _ -> false))) 
    test "check AndAlso" ((<@ true && true  @> |> (function AndAlso(Bool(true),Bool(true)) -> true | _ -> false))) 
    test "check OrElse"  ((<@ true || true  @> |> (function OrElse(Bool(true),Bool(true)) -> true | _ -> false))) 
    test "check AndAlso" ((<@ false && false @> |> (function AndAlso(Bool(false),Bool(false)) -> true | _ -> false))) 
    test "check OrElse"  ((<@ false || false @> |> (function OrElse(Bool(false),Bool(false)) -> true | _ -> false))) 
    test "check AndAlso - encoded" ((<@ true && false @> |> (function IfThenElse(Bool(true),Bool(false),Bool(false)) -> true | _ -> false))) 
    test "check OrElse - encoded" ((<@ true || false @> |> (function IfThenElse(Bool(true),Bool(true),Bool(false)) -> true | _ -> false))) 


    test "check ForIntegerRangeLoop"   (<@ for i = 1 to 10 do printf "hello" @> |> (function ForIntegerRangeLoop(v,Int32(1),Int32(10),b) -> true | _ -> false))
    test "check ForIntegerRangeLoop"   (<@ for i in 1 .. 10 do printf "hello" @> |> (function ForIntegerRangeLoop(v,Int32(1),Int32(10),b) -> true | _ -> false))
    // In this example, the types of the start and end points are not known at the point the loop
    // is typechecked. There was a bug (6064) where the transformation to a ForIntegerRangeLoop was only happening
    // when types were known
    test "check ForIntegerRangeLoop"   (<@ for i in failwith "" .. failwith "" do printf "hello" @> |> (function ForIntegerRangeLoop(v,_,_,b) -> true | _ -> false))
    // A slight non orthogonality is that all other 'for' loops go to (quite complex) the desugared form
    test "check Other Loop"   (<@ for i in 1 .. 2 .. 10 do printf "hello" @> |> (function Let(v,_,b) -> true | _ -> false))
    test "check Other Loop"   (<@ for i in 1L .. 10L do printf "hello" @> |> (function Let(v,_,b) -> true | _ -> false))

    let mutable mutableX = 1
    test "check mutableX top level set"   ((<@  mutableX  <- 10 @> |> (function PropertySet(None,pinfo,[],Int32 10) when pinfo.Name = "mutableX" -> true | _ -> false))) 
    test "check mutableX top level get"   ((<@  mutableX   @> |> (function PropertyGet(None,pinfo,[]) when pinfo.Name = "mutableX" -> true | _ -> false))) 


    let structFieldSetFromArray () = 
        <@ let mutable arr = [| S() |]
           arr.[0].x <- 3 @>

    let structFieldGetFromArray () = 
        <@ let mutable arr = [| S() |]
           arr.[0].x  @>

    test "check struct field set from array"   
     ((structFieldSetFromArray() |> 
        (function 
          | Let (varr, NewArray (_, [ DefaultValue _ ]),FieldSet (Some (Call (None, getter, [arr; Int32 0])), field, Int32 3)) -> true 
          | _ -> false))) 

    test "check struct field get from array"   
     ((structFieldGetFromArray() |> 
        (function 
          | Let (varr, NewArray (_, [ DefaultValue _ ]),FieldGet (Some (Call (None, getter, [arr; Int32 0])), field)) -> true 
          | _ -> false))) 


    test "checkIsMutable1" 
        (let e = <@@ let mutable x = 1 in if x = 1 then x <- 2 @@>

         match e with
                  |Let(v,e1,e2) -> v.IsMutable
                  |_ -> failwith "unexpected shape") 

    test "checkIsMutable2" 
        (let e = <@@ let x = 1 in if x = 1 then 2 else 3 @@>

         match e with
                  |Let(v,e1,e2) -> not v.IsMutable
                  |_ -> failwith "unexpected shape") 

    test "checkIsMutable3" 
        (let e = <@@ let f (x:int) = 1 in f 3 @@>

         match e with
                  |Let(v,e1,e2) -> not v.IsMutable
                  |_ -> failwith "unexpected shape") 

    test "checkType" 
        (let e = <@@ let mutable x = 1 in if x = 1 then x <- 2 @@>

         match e with
                  |Let(v,e1,e2) -> v.Type = typeof<int>
                  |_ -> failwith "unexpected shape") 


    type MyEnum = Foo = 0 | Bar = 1
    test "klnwce-0" 
        (match <@@ MyEnum.Foo @@> with  | Value(x,ty) when ty = typeof<MyEnum> && (x:?>MyEnum)=MyEnum.Foo -> true | _ -> false)
    test "klnwce-1" 
        (match <@@ MyEnum.Bar @@> with  | Value(x,ty) when ty = typeof<MyEnum> && (x:?>MyEnum)=MyEnum.Bar  -> true | _ -> false)
    test "klnwce-2" 
        (match <@@ System.DayOfWeek.Monday @@> with  | Value(x,ty) when ty = typeof<System.DayOfWeek> && (x:?>System.DayOfWeek)=System.DayOfWeek.Monday -> true | _ -> false)
    test "klnwce-3" 
        (<@@ System.DayOfWeek.Monday @@>.Type = typeof<System.DayOfWeek >)
    test "klnwce-4" 
        (match <@@ (fun () -> MyEnum.Bar) @@> with  | Lambda(_,Value(x,ty)) when ty = typeof<MyEnum> && (x:?>MyEnum)=MyEnum.Bar -> true | _ -> false)
        
    test "check NewArray"   (<@ [| |] :int[] @> |> (function NewArray(typ,[]) when typ = typeof<int32> -> true | _ -> false))
    test "check NewArray"   (<@ [| 1;2;3 |] @> |> (function NewArray(typ,[Int32(1);Int32(2);Int32(3)]) when typ = typeof<int32> -> true | _ -> false))
    test "check NewRecord"   (<@ { contents = 3 } @> |> (function NewRecord(typ,args) -> true | _ -> false))
    test "check NewUnion"   (<@ [] @> |> (function NewUnionCase(unionCase,args) -> true | _ -> false))
    test "check NewUnion"   (<@ [1] @> |> (function NewUnionCase(unionCase,args) -> true | _ -> false))
    test "check NewUnion"   (<@ None @> |> (function NewUnionCase(unionCase,args) -> true | _ -> false))
    test "check NewUnion"   (<@ Some(1) @> |> (function NewUnionCase(unionCase,args) -> true | _ -> false))

    test "check NewDelegate"   (<@ new System.EventHandler<System.EventArgs>(fun sender evArgs -> ()) @> |> (function NewDelegate(ty,[v1;v2],_) when v1.Name = "sender" && v2.Name = "evArgs" -> true | _ -> false))

    test "check NewTuple (2)"   (<@ (1,2) @>           |> (function NewTuple([Int32(1);Int32(2)]) -> true | _ -> false))
    test "check NewTuple (3)"   (<@ (1,2,3) @>         |> (function NewTuple([Int32(1);Int32(2);Int32(3)]) -> true | _ -> false))
    test "check NewTuple (4)"   (<@ (1,2,3,4) @>       |> (function NewTuple([Int32(1);Int32(2);Int32(3);Int32(4)]) -> true | _ -> false))
    test "check NewTuple (5)"   (<@ (1,2,3,4,5) @>     |> (function NewTuple([Int32(1);Int32(2);Int32(3);Int32(4);Int32(5)]) -> true | _ -> false))
    test "check NewTuple (6)"   (<@ (1,2,3,4,5,6) @>   |> (function NewTuple([Int32(1);Int32(2);Int32(3);Int32(4);Int32(5);Int32(6)]) -> true | _ -> false))
    test "check NewTuple (6)"   (<@ (1,2,3,4,5,6,7) @> |> (function NewTuple([Int32(1);Int32(2);Int32(3);Int32(4);Int32(5);Int32(6);Int32(7)]) -> true | _ -> false))

    test "check  Lambda"  ((<@ (fun (x:int) -> x) @>               |> (function Lambda(v,_) -> true | _ -> false))) 
    test "check  Lambda"  ((<@ (fun (x:int,y:int) -> x) @>         |> (function Lambda(v,_) -> true | _ -> false))) 
    test "check  Lambda"  ((<@ (fun (p:int*int) -> p) @>           |> (function Lambda(v,_) -> true | _ -> false))) 
    test "check  Lambda"  ((<@ (fun () -> 1) @>           |> (function Lambda(v,_) -> true | _ -> false))) 

    test "check  Lambdas" ((<@ (fun (x:int) -> x) @>               |> (function Lambdas([[v]],_) -> true | _ -> false))) 
    test "check  Lambdas" ((<@ (fun (x:int,y:int) -> x) @>         |> (function Lambdas([[v1;v2]],_) -> true | _ -> false))) 
    test "check ~Lambdas" ((<@ (fun (x:int) (y:int) -> x) @>       |> (function Lambdas([[v1;v2]],_) -> false | _ -> true))) 
    test "check  Lambdas" ((<@ (fun (x:int,y:int) (z:int) -> z) @> |> (function Lambdas([[v1;v2];[v3]],_) -> true | _ -> false))) 
    test "check  Lambdas" ((<@ (fun ((x:int,y:int),(z:int)) -> z) @> |> (function Lambdas([[v1;v2]],_) -> true | _ -> false))) 
    test "check  Lambdas" ((<@ (fun ((x:int),(y:int,z:int)) -> z) @> |> (function Lambdas([[v1;v2]],_) -> true | _ -> false))) 
    //
    //test "check  Lambdas" ((<@ (fun [(x:int)] -> x) @> |> (function Lambdas([[v1]],_) -> true | _ -> false))) 
    test "check  Lambdas" ((<@ (fun () -> 1) @> |> (function Lambdas([[v1]],_) -> true | _ -> false))) 

    test "check  Let" ((<@ let x = 1 in x @> |> (function Let(v,Int32(1),Var(v2)) when v = v2 -> true | _ -> false))) 
    test "check  Let" ((<@ let x = 1 
                           let y = 2 
                           x,y @> |> (function Let(vx,Int32(1),Let(vy,Int32(2),NewTuple([Var(vx2);Var(vy2)]))) when vx.Name = "x" && vx = vx2 && vy = vy2 -> true | _ -> false))) 

    test "check  Let" ((<@ let x = 1 
                           let x = 2 
                           x,x @> |> (function Let(vx,Int32(1),Let(vy,Int32(2),NewTuple([Var(vx2);Var(vy2)]))) when vx.Name = "x" && vy.Name = "x" && vy = vx2 && vy = vy2 -> true | _ -> false))) 

    test "check  Let" ((<@ let f () = 1 in f @> |> (function Let(v,Lambda(_,Int32(1)),Var(v2)) when v = v2 -> true | _ -> false))) 

    test "check  LetRecursive" ((<@ let rec f (x:int) : int = 1 in f @> |> (function LetRecursive([vf,Lambda(vx,Int32(1))],Var(vf2)) when vf = vf2 -> true | _ -> false))) 

    test "check  LetRecursive" ((<@ let rec f (x:int) : int = 1 
                                    and     g (x:int) = 2 
                                    (f,g) @> |> (function LetRecursive([(vf,Lambda(vx,Int32(1)));(vg,Lambda(vx2,Int32(2)))],NewTuple[Var(vf2);Var(vg2)]) when (vf = vf2 && vg = vg2)-> true | _ -> false))) 

    test "check  Application" ((<@ let f () = 1 in f () @> |> (function Let(fv1,Lambda(_,Int32(1)),Application(Var(fv2),Unit)) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Application" ((<@ let f (x:int) = 1 in f 1 @> |> (function Let(fv1,Lambda(_,Int32(1)),Application(Var(fv2),Int32(1))) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Application" ((<@ let f (x:int) (y:int) = 1 in f 1 2 @> |> (function Let(fv1,Lambda(_,Lambda(_,Int32(1))),Application(Application(Var(fv2),Int32(1)),Int32(2))) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Application" ((<@ let f (x:int,y:int) = 1 in f (1,2) @> |> (function Let(fv1,Lambdas(_,Int32(1)),Application(Var(fv2),NewTuple[Int32(1);Int32(2)])) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Applications" ((<@ let f (x:int) (y:int) = 1 in f 1 2 @> |> (function Let(fv1,Lambdas(_,Int32(1)),Applications(Var(fv2),[[Int32(1)];[Int32(2)]])) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Applications" ((<@ let f (x:int,y:int) = 1 in f (1,2) @> |> (function Let(fv1,Lambdas(_,Int32(1)),Applications(Var(fv2),[[Int32(1);Int32(2)]])) when fv1 = fv2 -> true | _ -> false))) 
    test "check  Applications" ((<@ let f () = 1 in f () @> |> (function Let(fv1,Lambdas(_,Int32(1)),Applications(Var(fv2),[[]])) when fv1 = fv2 -> true | _ -> false))) 

    test "check  pattern matching 1" 
        ((<@ function (x:int) -> x  @> 
             |> (function Lambda(argv1,Let(xv1,Var(argv2),Var(xv2))) when xv1 = xv2 && argv1 = argv2 -> true | _ -> false))) 

    test "check  incomplete pattern matching 1" 
        ((<@ function (None : int option) -> 1  @> 
             // Pipe the quotation into a matcher that checks its form
             |> (function Lambda(argv1,IfThenElse(UnionCaseTest(Var(argv2),ucase1),Int32(1),Call(None,minfo,[_])))  when argv1 = argv2 && minfo.Name = "Raise" && ucase1.Name = "None" -> true 
                        | _ -> false))) 
             
    test "check  pattern matching 2" 
        ((<@ function { contents = (x:int) } -> x  @> 
             // Pipe the quotation into a matcher that checks its form
             |> (function Lambda(argv1,Let(xv1,PropertyGet(Some(Var(argv2)),finfo,[]),Var(xv2))) when xv1 = xv2 && argv1 = argv2 -> true 
                        | _ -> false))) 

    test "check  pattern matching 3" 
        ((<@ function ([]:int list) -> 1 | _ -> 2  @> 
             // Pipe the quotation into a matcher that checks its form
             |> (function Lambda(argv1,IfThenElse(UnionCaseTest(Var(argv2),ucase),Int32(1),Int32(2))) when argv1 = argv2 -> true | _ -> false))) 

    test "check  pattern matching 4" 
        ((<@ function ([]:int list) -> 1 | h::t -> 2  @> 
             // Pipe the quotation into a matcher that checks its form
             |> (function Lambda(argv1,IfThenElse(UnionCaseTest(Var(argv2),ucaseCons),
                                                  Let(tv1,PropertyGet(Some(Var(argv3)),pinfoTail,[]),
                                                    Let(hv1,PropertyGet(Some(Var(argv4)),pinfoHead,[]),
                                                         Int32(2))),
                                                  Int32(1))) when (argv1 = argv2 && 
                                                                   argv1 = argv3 && 
                                                                   argv1 = argv4 && 
                                                                   ucaseCons.Name = "Cons" && 
                                                                   pinfoTail.Name = "Tail" && 
                                                                   pinfoTail.Name = "Tail") -> true 
                        | _ -> false))) 

    test "check  pattern matching 5" 
        ((<@ function h::t -> 2  | ([]:int list) -> 1 @> 
             |> (function Lambda(argv1,IfThenElse(UnionCaseTest(Var(argv2),ucaseEmpty),
                                                  Int32(1),
                                                  Let(tv1,PropertyGet(Some(Var(argv3)),pinfoTail,[]),
                                                    Let(hv1,PropertyGet(Some(Var(argv4)),pinfoHead,[]),
                                                         Int32(2))))) when (argv1 = argv2 && 
                                                                            argv1 = argv3 && 
                                                                            argv1 = argv4 && 
                                                                            ucaseEmpty.Name = "Empty" && 
                                                                            pinfoTail.Name = "Tail" && 
                                                                            pinfoTail.Name = "Tail") -> true 
                        | _ -> false))) 

    test "check  pattern matching 6" 
        ((<@ function [h1;(h2:int)] -> 2 | _ -> 0 @> 
             |> (function Lambda(argv1,
                                 IfThenElse(UnionCaseTest(Var(argv2),ucaseCons),
                                            IfThenElse(UnionCaseTest(PropertyGet(Some(Var(argv3)),pinfoTail,[]),ucaseCons2),
                                                       IfThenElse(UnionCaseTest(PropertyGet(Some(PropertyGet(Some(Var(argv4)),pinfoTail2,[])),pinfoTail3,[]),ucaseEmpty),
                                                                  Let(h1v1,PropertyGet(Some(Var(argv5)),pinfoHead,[]),
                                                                    Let(h2v1,PropertyGet(Some(PropertyGet(Some(Var(argv6)),pinfoTail4,[])),pinfoHead2,[]),
                                                                         Int32(2))),
                                                                  Int32(0)),
                                                       Int32(0)),
                                            Int32(0))) 
                                when (argv1 = argv2 && 
                                      argv1 = argv3 && 
                                      argv1 = argv4 && 
                                      argv1 = argv5 && 
                                      argv1 = argv6 && 
                                      h1v1.Name = "h1" && 
                                      h2v1.Name = "h2" && 
                                      ucaseEmpty.Name = "Empty" && 
                                      pinfoTail.Name = "Tail" && 
                                      pinfoTail2.Name = "Tail" && 
                                      pinfoTail3.Name = "Tail" && 
                                      pinfoTail4.Name = "Tail" && 
                                      pinfoHead.Name = "Head" && 
                                      pinfoHead2.Name = "Head") -> true
                        | _ -> false))) 

    // Check the elaborated form of a pattern match that uses an active pattern 
    let (|RefCell|) (x : int ref) = x.Value
    test "check  pattern matching 7" 
        ((<@ function RefCell(x) -> x @> 
             |> (function Lambda(argv1,
                                 Let(apv1, Call(None,minfo,[Var(argv2)]),
                                     Let(xv1, Var(apv2),
                                         Var(xv2))))
                                when (argv1 = argv2 && 
                                      xv1 = xv2  && 
                                      apv1 = apv2  && 
                                      minfo.Name = "|RefCell|") -> true
                        | _ -> false))) 

    // Check calling .NET things
    test "check  NewObject" ((<@ new System.Object() @> |> (function NewObject(_,[]) -> true | _ -> false))) 
    test "check  NewObject" ((<@ new System.String('c',3) @> |> (function NewObject(_,[Char('c');Int32(3)]) -> true | _ -> false))) 
    
    test "check  Call (static)" ((<@ System.Object.Equals("1","2") @> |> (function Call(None,_,[Coerce(String("1"),_);Coerce(String("2"),_)]) -> true | _ -> false))) 
    test "check  Call (instance)" ((<@ ("1").Equals("2") @> |> (function Call(Some(String("1")),_,[String("2")]) -> true | _ -> false))) 
    test "check  Call (instance)" ((<@ ("1").GetHashCode() @> |> (function Call(Some(String("1")),_,[]) -> true | _ -> false))) 
    test "check  PropertyGet (static)" ((<@ System.DateTime.Now @> |> (function PropertyGet(None,_,[]) -> true | _ -> false))) 
    test "check  PropertyGet (instance)" ((<@ ("1").Length @> |> (function PropertyGet(Some(String("1")),_,[]) -> true | _ -> false))) 

    test "check  PropertySet (static)" ((<@ System.Environment.ExitCode <- 1 @> |> (function PropertySet(None,_,[],Int32(1)) -> true | _ -> false))) 
    test "check  PropertySet (instance)" ((<@ ("1").Length @> |> (function PropertyGet(Some(String("1")),_,[]) -> true | _ -> false))) 

    test "check null (string)"   (<@ (null:string) @> |> (function Value(null,ty) when ty = typeof<string> -> true | _ -> false))

    let v = Expr.GlobalVar<int>("IntVar")
    test "check var (GlobalVar)"   (v |> (function Var(v2) when v2.Name = "IntVar" -> true | _ -> false))

    test "check Var"   (<@ %v @> |> (function Var(v2) when v2.Name = "IntVar"  -> true | _ -> false))
    test "check Coerce"   (<@ 3 :> obj @> |> (function Coerce(x,ty) when ty = typeof<obj> -> true | _ -> false))
    test "check Sequential"   (<@ (); () @> |> (function Sequential(Unit,Unit) -> true | _ -> false))
    test "check Sequential"   (<@ ""; () @> |> (function Sequential(Sequential(String(""),Unit),Unit) -> true | _ -> false)) (* changed for bug 3628 fix *)
    test "check Sequential"   (<@ (); "" @> |> (function Sequential(Unit,String("")) -> true | _ -> false))
    test "check Sequential"   (<@ (); (); () @> |> (function Sequential(Unit,Sequential(Unit,Unit)) -> true | _ -> false))
    test "check WhileLoop"   (<@ while true do () done @> |> (function WhileLoop(Bool(true),Unit) -> true | _ -> false))
    test "check TryFinally"   (<@ try 1 finally () @> |> (function TryFinally(Int32(1),Unit) -> true | _ -> false))
    //test "check AddressOf"   (<@ let mutable x = 1 in &x @> |> (function AddressOf(Int32(1),Unit) -> true | _ -> false))

    //test "check TypeTest"   (<@ match new obj() with :? string  -> 1 @> |> (function TypeTest(x,ty) when ty = typeof<string> -> true | _ -> false))
    //test "check Coerce"   (<@ new obj() :?> string @> |> (function Coerce(x,ty) when ty = typeof<string> -> true | _ -> false))
(*
    | Expr.FieldGet(objOpt,fieldInfo) 
    | Expr.FieldSet _ -> failwithf "Field-set expressions can't be converted to LINQ expressions"
*)

    <@ new obj() :?> int @>
   //<@ { new System.IComparable with member x.CompareTo(y) = 0 } @>
   
    // check TryGetReflectedDefinition on locally-defined f

    [<ReflectedDefinition>]
    let f (x:int) = 1

    test "clewlkjncew" 
        ((<@ f 1 @> |> (function Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsSome | _ -> false))) 


    // check failure of TryGetReflectedDefinition on non-ReflectedDefinition for locally-defined f3

    //[<ReflectedDefinition>]
    let f3 (x:int) = 1
    test "ejnwe98" 
        ((<@ f3 1 @> |> (function Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsNone | _ -> false))) 

    [<ReflectedDefinition>]
    let rec f2 (x:int) = not (f2 x)

    // check success of TryGetReflectedDefinition on local recursive f2
    test "cwuic9en" 
              ((<@ f2 1 @> 
               
               |> (function Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsSome | _ -> false))) 


    // test GetFreeVars
    
    test "check lambda closed"       (Seq.length ((<@ (fun (x:int)  -> 1) @>).GetFreeVars()   ) = 0)
    test "check for loop closed"         (Seq.length ((<@ for i = 1 to 10 do () done @>).GetFreeVars()) = 0)
    test "check while loop closed"       (Seq.length ((<@ while true do () done @>).GetFreeVars()) = 0)
    test "check let rec closed"          (Seq.length ((<@ let rec f (x:int) = f (f x) in f @>).GetFreeVars()) = 0)

    module AddressOfTests = 
        [<Struct>]
        type S(z : int) =
            [<DefaultValue>] val mutable x : int

        [<Struct>]
        type S2(z : int) =
            [<DefaultValue>] val mutable s : S

        test "check Struct 1"   (<@ S(1).x  @> |> (function Let(_,NewObject _, FieldGet (Some (Var _), _)) -> true | _ -> false))
        test "check Struct 2a"  (<@ (fun (s2: S2) -> s2.s.x)  @> |> (function Lambda(_,FieldGet(Some(FieldGet(Some(Var _),_)),_)) -> true | _ -> false))
        test "check Struct 2"   (<@ (fun (arr: S[]) -> arr.[0])  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "GetArray" -> true | _ -> false))
        test "check Struct 3"   (<@ (fun (arr: S[,]) -> arr.[0,0])  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "GetArray2D" -> true | _ -> false))
        test "check Struct 4"   (<@ (fun (arr: S[,,]) -> arr.[0,0,0])  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "GetArray3D" -> true | _ -> false))
        test "check Struct 5"   (<@ (fun (arr: S[,,,]) -> arr.[0,0,0,0])  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "GetArray4D" -> true | _ -> false))
        test "check Struct 2 arr"   (<@ (fun (arr: S[]) -> arr.[0].x)  @> |> (function Lambda(_,FieldGet(Some(Call(None, minfo, _)),_)) when minfo.Name = "GetArray" -> true | _ -> false))
        test "check Struct 3 arr"   (<@ (fun (arr: S[,]) -> arr.[0,0].x)  @> |> (function Lambda(_,FieldGet(Some(Call(None, minfo, _)),_)) when minfo.Name = "GetArray2D" -> true | _ -> false))
        test "check Struct 4 arr"   (<@ (fun (arr: S[,,]) -> arr.[0,0,0].x)  @> |> (function Lambda(_,FieldGet(Some(Call(None, minfo, _)),_)) when minfo.Name = "GetArray3D" -> true | _ -> false))
        test "check Struct 5 arr"   (<@ (fun (arr: S[,,,]) -> arr.[0,0,0,0].x)  @> |> (function Lambda(_,FieldGet(Some(Call(None, minfo, _)),_)) when minfo.Name = "GetArray4D" -> true | _ -> false))
        test "check Struct 6"   (<@ (fun (arr: int[]) -> arr.[0] <- 0)  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "SetArray" -> true | _ -> false))
        test "check Struct 7"   (<@ (fun (arr: int[,]) -> arr.[0,0] <- 0)  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "SetArray2D" -> true | _ -> false))
        test "check Struct 8"   (<@ (fun (arr: int[,,]) -> arr.[0,0,0] <- 0)  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "SetArray3D" -> true | _ -> false))
        test "check Struct 9"   (<@ (fun (arr: int[,,,]) -> arr.[0,0,0,0] <- 0)  @> |> (function Lambda(_,Call(None, minfo, _)) when minfo.Name = "SetArray4D" -> true | _ -> false))
        test "check Struct C"   (<@ S()  @> |> (function DefaultValue _ -> true | _ -> false))
        test "check Mutate 1"   (<@ let mutable x = 0 in x <- 1 @> |> (function Let(v,Int32 0, VarSet(v2,Int32 1)) when v = v2 -> true | _ -> false))

        let q = <@ let mutable x = 0 in x <- 1 @>

    // Test basic expression splicing
    let f8383 (x:int) (y:string) = 0
    let test2 =   
        let v = 1 in 
        let s = "2" in   
        <@ f8383 v s @>

    let f8384 (x:'a) (y:string) = 0
    let test3a =   
        let v = 1 in 
        let s = "2" in   
        <@ f8384 v s @>

    let test3b() =   
        let v = 1 in 
        let s = "2" in   
        <@ f8384 v s @>
    
    check "value splice 1" test2 <@ f8383 1 "2" @>
    check "value splice 2" test3a <@ f8384 1 "2" @>
    check "value splice 3" (test3b()) <@ f8384 1 "2" @>

    test "value splice 4"  (let v1 = 3 in let v2 = 1+2 in  <@ 1 + v1 @> = <@ 1 + v2 @>)
    test "expr splice 1"  (<@ %(<@ 1 @>) @> = <@ 1 @>)
    
    // Test basic type splicing

    let f8385 (x:'a) (y:string) = <@ (x,y) @>
    check "type splice 1" (f8385 1 "a") <@ (1,"a") @>
    check "type splice 2" (f8385 "b" "a") <@ ("b","a") @>



    test "check TryGetReflectedDefinition (local f)" ((<@ f 1 @> |> (function  Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsSome | _ -> false)))

    test "check TryGetReflectedDefinition (local recursive f2)" 
        ((<@ f2 1 @> |> (function Call(None,minfo,args)  -> Quotations.Expr.TryGetReflectedDefinition(minfo) <> None | _ -> false))) 


    test "check lambda closed"       (Seq.length ((<@ (fun (x:int)  -> 1) @>).GetFreeVars()) = 0)
    test "check for loop closed"         (Seq.length ((<@ for i = 1 to 10 do () done @>).GetFreeVars()) = 0)
    test "check while loop closed"       (Seq.length ((<@ while true do () done @>).GetFreeVars()) = 0)
    test "check let rec closed"          (Seq.length ((<@ let rec f (x:int) = f (f x) in f @>).GetFreeVars()) = 0)

    // Check we can use ReflectedDefinition on a floating point pattern match
    type T = | A of float

    
    test "check NewUnionCase"   (<@ A(1.0) @> |> (function NewUnionCase(unionCase,args) -> true | _ -> false))
    

    [<ReflectedDefinition>]
    let foo v = match v with  | A(1.0) -> 0 | _ -> 1
      
    test "check TryGetReflectedDefinition (local f)" 
        ((<@ foo (A(1.0)) @> |> (function Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsSome | _ -> false))) 

    [<ReflectedDefinition>]
    let test3297327 v = match v with  | A(1.0) -> 0 | _ -> 1
      
    test "check TryGetReflectedDefinition (local f)" 
        ((<@ foo (A(1.0)) @> |> (function Call(None,minfo,args) -> Quotations.Expr.TryGetReflectedDefinition(minfo).IsSome | _ -> false))) 

end

(*
module SubstiutionTest = begin
  let tm = (<@ (fun x y -> x + y + y) @>)
  // TEST INVALID - this match fails because a variable is escaping.
  let Some(x,y,y') = Template <@. (fun x y -> _ + _ + _) .@> tm
  let Some(xyy) = Template <@. (fun (x:int) (y:int) -> _) .@> tm
  test "check free vars (tm)" (List.length (freeInExpr tm.Raw) = 0)
  test "check free vars (x)" (List.length (freeInExpr x.Raw) = 1)
  test "check free vars (y)" (List.length (freeInExpr y.Raw) = 1)
  test "check free vars (xyy)" (List.length (freeInExpr xyy.Raw) = 2)
  

  let Some xv = Var.Query(x.Raw)
  let Some body = Template <@. (fun x -> _) .@> tm
  test "check free vars (body)" (List.length (freeInExpr body.Raw) = 1)
  let body2 = substitute (fun _ -> None) (fun v -> if v = xv then (printf "Yes!\n"; Some((<@ 1 @>).Raw)) else None) body
  test "check free vars (body2)" (List.length (freeInExpr body2.Raw) = 0)
  let body3 = substitute (fun _ -> None) (fun v -> if v = xv then Some y.Raw else None) body
  test "check free vars (body3)" (List.length (freeInExpr body3.Raw) = 1)

end
*)

(*

module TomasP = begin
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns

    let ex1 = <@ 1 + 2 @>
    let ex2 = <@ 1 + 10/5 @>

    type simple_expr =
      | Int of int
      | Add of simple_expr * simple_expr
      | Sub of simple_expr * simple_expr
      | Mul of simple_expr * simple_expr
      | Div of simple_expr * simple_expr


    let what_is x =
      match x with  
        | Int32 (_) -> "number";
        | _ -> 
        match x with  
          | Application(_) -> "application";
          | _ -> 
            "something else..."
        
    // Prints "number"      
    do print_string (what_is <@ 1 @>)

    // Prints "application"         
    do print_string (what_is <@ 1 + 2 @>)

    let rec parse x =
      match x with  
        // x contains the number so we can simply return Int(x)
        | Int32 (x) -> Int(x); 
        | Applications (GenericTopDnUse <@ (+) @> tyargs,[a1;a2]) -> Add(parse a1, parse a2)
        | Applications (GenericTopDnUse <@ (-) @> tyargs,[a1;a2]) -> Sub(parse a1, parse a2)
        | Applications (GenericTopDnUse <@ ( * ) @> tyargs,[a1;a2]) -> Mul(parse a1, parse a2)
        | Applications (GenericTopDnUse <@ ( / ) @> tyargs,[a1;a2]) -> Div(parse a1, parse a2)
        | _ -> failwith "parse"

    let a = 1
    let q = <@ if (a = 0) then 1 else 2 @>
    let ex4 = 
     match q with
      | IfThenElse (cond,trueBranch,falseBranch) ->
          // cond        - 'expr' that represents condition
          // trueBranch  - 'expr' that represents the true branch
          // falseBranch - 'expr' that represents the false branch
          print_string "If-then-else statement"
          
      | _ -> 
          print_string "Something else"

    type a = | B of string
    [<ReflectedDefinition>]
    let processStuff sequence = Seq.iter (function B packet -> ()) sequence

end

module ErrorEstimateTest = 
    open Quotations
    open Quotations.Expr

    //let f x = x + 2.0*x*x
    //let t = <@ fun x -> x + 2*x*x @>

    type Error = Err of float

    let rec errorEstimateAux t (env : Map<_,_>) = 
        match t with 
        | GenericTopDnApp <@ (+) @> (tyargs,[xt;yt]) -> 
            let x,Err(xerr) = errorEstimateAux xt env
            let y,Err(yerr) = errorEstimateAux yt env
            (x+y,Err(xerr+yerr))
        | GenericTopDnApp <@ (-) @> (tyargs,[xt;yt]) -> 
            let x,Err(xerr) = errorEstimateAux xt env
            let y,Err(yerr) = errorEstimateAux yt env
            (x-y,Err(xerr+yerr))
        | GenericTopDnApp <@ ( * ) @> (tyargs,[xt;yt]) -> 
            let x,Err(xerr) = errorEstimateAux xt env
            let y,Err(yerr) = errorEstimateAux yt env
            
            (x*y,Err(xerr*abs(y)+yerr*abs(x)+xerr*yerr))

        // TBD...        
        | GenericTopDnApp <@ ( / ) @> (tyargs,[xt;yt]) ->
            let x,Err(xerr) = errorEstimateAux xt env
            let y,Err(yerr) = errorEstimateAux yt env
            // check:
            (x/y,Err(abs((y*xerr - yerr*x)/(y+yerr))))
            
        | GenericTopDnApp <@ abs @> (tyargs,[xt]) -> 
            let x,Err(xerr) = errorEstimateAux xt env
            (abs(x),Err(xerr))
        | Let((var,vet), bodyt) -> 
            let varv,verr = errorEstimateAux vet env
            errorEstimateAux bodyt (env.Add(var.Name,(varv,verr)))

        | App(ResolvedTopDnUse(info,Lambda(v,body)),arg) -> 
            errorEstimateAux  (MkLet((v,arg),body)) env
        | Var(x) -> env.[x]
        | Double(n) -> (n,Err(0.0))       
        | _ -> failwithf "unrecognized term: %A" t


    let rec errorEstimateFun (t : Expr) = 
        match t with 
        | Lambda(x,t) ->
            (fun xv -> errorEstimateAux t (Map.ofSeq [(x.Name,xv)]))
        | ResolvedTopDnUse(info,body) -> 
            errorEstimateFun body 
        | _ -> failwithf "unrecognized term: %A - expected a lambda" t



    let errorEstimate (t : Expr<float -> float>) = errorEstimateFun t.Raw 

    let rec errorEstimate2 (t : Expr<float -> float -> float>) = 
        match t.Raw with 
        | Lambdas([x;y],t) ->
            (fun xv yv -> errorEstimateAux t (Map.ofSeq [(x.Name,xv); (y.Name,yv)]))
        | _ -> failwithf "unrecognized term: %A - expected a lambda of two args" t

    let (±) x = Err(x)
    //fsi.AddPrinter (fun (x,Err(v)) -> sprintf "%g±%g" x v)

    errorEstimate <@ fun x -> x @> (1.0,±0.1)
    errorEstimate <@ fun x -> 2.0*x @> (1.0,±0.1)
    errorEstimate <@ fun x -> x*x @> (1.0,±0.1)
    errorEstimate <@ fun x -> 1.0/x @> (0.5,±0.1)

    errorEstimate <@ fun x -> let y = x + x 
                              y*y + 2.0 @> (1.0,±0.1)

    errorEstimate <@ fun x -> x+2.0*x+3.0*x*x @> (1.0,±0.1)

    errorEstimate <@ fun x -> x+2.0*x+3.0/(x*x) @> (0.3,±0.1)

    [<ReflectedDefinition>]
    let poly x = x+2.0*x+3.0/(x*x)

    errorEstimate <@ poly @> (0.3,±0.1)
    errorEstimate <@ poly @> (30271.3,±0.0001)
*)

module MoreTests = 


    open Microsoft.FSharp.Quotations

    module OneModule =
        let ModuleFunctionNoArgs() = 1
        let ModuleFunctionOneArg(x:int) = 1
        let ModuleFunctionOneUnitArg(x:unit) = 1
        let ModuleFunctionOneTupledArg(x:int*int) = 1
        let ModuleFunctionTwoArgs(x:int,y:int) = 1
    
    
    type ClassOneArg(a:int) = 
        static member TestStaticMethodOneTupledArg(x:int*int) = 1
        static member TestStaticMethodOneArg(x:int) = x
        static member TestStaticMethodNoArgs() = 1
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        static member TestStaticProp = 3
        member c.TestInstanceProp = 3
        member c.TestInstanceIndexProp with get(idx:int) = 3
        member c.TestInstanceSettableIndexProp with set (idx:int) (v:int) = ()
        member c.TestInstanceSettableIndexProp2 with set (idx1:int, idx2:int) (v:int) = ()
        member c.TestInstanceMethodNoArgs() = 1
        member c.TestInstanceMethodOneArg(x:int) = x
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y

        member this.GetterIndexer
            with get (x:int) = 1

        member this.TupleGetterIndexer
            with get (x:int*int) = 1

        member this.Item
            with get (x:int) = 1
                
        member this.TupleSetterIndexer
            with get (x : int*int) = 1

        member this.SetterIndexer
            with set (x:int) (y:int) = ()

        member this.Item
            with set (x:int) (y:int) = ()
            
        member this.Setter
            with set (x : int) = ()
                
        member this.TupleSetter
            with set (x : int*int) = ()


    type ClassNoArg() = 
        static member TestStaticMethodOneTupledArg(x:int*int) = x
        static member TestStaticMethodOneArg(x:int) = x
        static member TestStaticMethodNoArgs() = 1
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        static member TestStaticProp = 3
        static member TestStaticSettableProp with set (v:int) = ()
        static member TestStaticSettableIndexProp with set (idx:int) (v:int) = ()
        member c.TestInstanceProp = 3
        member c.TestInstanceIndexProp with get(idx:int) = 3
        member c.TestInstanceSettableIndexProp with set (idx:int) (v:int) = ()
        member c.TestInstanceMethodNoArgs() = 1
        member c.TestInstanceMethodOneArg(x:int) = x
        member c.TestInstanceMethodOneTupledArg(x:int*int) = 1
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y

    type GenericClassNoArg<'a>() = 
        static member TestStaticMethodOneTupledArg(x:int*int) = x
        static member TestStaticMethodOneArg(x:int) = x
        static member TestStaticMethodNoArgs() = 1
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        static member TestStaticProp = 3
        member c.TestInstanceProp = 3
        member c.TestInstanceIndexProp with get(idx:int) = 3
        member c.TestInstanceSettableIndexProp with set (idx:int) (v:int) = ()
        member c.TestInstanceMethodNoArgs() = 1
        member c.TestInstanceMethodOneArg(x:int) = x
        member c.TestInstanceMethodOneTupledArg(x:int*int) = 1
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y


    let isMeth (inp : Expr<_>) = match inp with Call _ -> true | _ -> false
    let isPropGet (inp : Expr<_>) = match inp with PropertyGet _ -> true | _ -> false
    let isPropSet (inp : Expr<_>) = match inp with PropertySet _ -> true | _ -> false

    //printfn "res = %b" (isPropGet <@ ClassOneArg.TestStaticProp @>)
    test "test3931a" (isMeth <@ OneModule.ModuleFunctionNoArgs() @>)
    test "test3931a" (isMeth <@ OneModule.ModuleFunctionOneArg(1) @>)
    test "test3931a" (isMeth <@ OneModule.ModuleFunctionOneUnitArg() @>)
    test "test3931a" (isMeth <@ OneModule.ModuleFunctionTwoArgs(1,2) @>)
    test "test3931a" (isMeth <@ OneModule.ModuleFunctionOneTupledArg(1,2) @>)
    let p = (1,2)
    // This case doesn't match because F# performs type-base arity analysis for module 'let' bindings
    // and we see this untupling here.
    // Thus this is elaborated into 'let v = p in let v1 = p#1 in let v2 = p#2 in f(v1,v2)'
    // test "test3931a" (isMeth <@ OneModule.ModuleFunctionOneTupledArg(p) @>)
         
    //printfn "res = %b" (isPropGet <@ ClassOneArg.TestStaticProp @>)
    test "test3932a" (isMeth <@ ClassOneArg.TestStaticMethodOneArg(3) @>)
    test "test3932f" (isMeth <@ ClassOneArg.TestStaticMethodNoArgs() @>)
    test "test3932g" (isMeth <@ ClassOneArg.TestStaticMethodTwoArgs(3,4) @>)

    test "test3932qA" (isPropGet <@ ClassOneArg(3).TestInstanceProp @>)
    test "test3932qB" (isPropGet <@ ClassOneArg(3).TestInstanceIndexProp(4) @>)
    test "test3932qC" (isPropSet <@ ClassOneArg(3).TestInstanceSettableIndexProp(4) <- 5 @>)
    test "test3932qD" (isPropSet <@ ClassOneArg(3).TestInstanceSettableIndexProp2(4,5) <- 6 @>)
    test "test3932q77" (match <@ ClassOneArg(3).TestInstanceSettableIndexProp2(4,5) <- 6 @> with 
                        | PropertySet(Some _, _, [Int32(4); Int32(5)], Int32(6)) -> true 
                        | _ -> false)

    test "test3932wA" (isMeth <@ ClassOneArg(3).TestInstanceMethodNoArgs() @>)
    test "test3932wB" (isMeth <@ ClassOneArg(3).TestInstanceMethodOneArg(3) @>)
    test "test3932e" (isMeth <@ ClassOneArg(3).TestInstanceMethodTwoArgs(3,4) @>)

    test "test3932q1" (isPropSet <@ ClassOneArg(3).Setter <- 3 @>)
    test "test3932q2" (isPropGet <@ ClassOneArg(3).GetterIndexer(3) @>)
    test "test3932q3" (isPropGet <@ ClassOneArg(3).[3] @>)
    test "test3932q4" (isPropGet <@ ClassOneArg(3).TupleGetterIndexer((3,4)) @>)
    test "test3932q5" (isPropSet <@ ClassOneArg(3).SetterIndexer(3) <- 3 @>)
    test "test3932q61" (isPropSet <@ ClassOneArg(3).[3] <- 3 @>)
    test "test3932q62" (match <@ ClassOneArg(3).[4] <- 5 @> with PropertySet(Some _,_, [Int32(4)], Int32(5)) -> true | _ -> false)
    test "test3932q7" (isPropSet <@ ClassOneArg(3).TupleSetter <- (3,4) @>)


    test "test3932" (isPropGet <@ ClassNoArg.TestStaticProp @>)
    test "test3932" (isPropSet <@ ClassNoArg.TestStaticSettableProp <- 3 @>)
    
    printfn "res = %A" <@ ClassNoArg.TestStaticSettableProp <- 5 @> 
    test "test3932q63" (match <@ ClassNoArg.TestStaticSettableProp <- 5 @> with PropertySet(None, _, [], Int32(5)) -> true | _ -> false)
    test "test3932q64" (match <@ ClassNoArg.TestStaticSettableIndexProp(4) <- 5 @> with PropertySet(None, _, [Int32(4)], Int32(5)) -> true | _ -> false)
    test "test3932r" (isMeth <@ ClassNoArg.TestStaticMethodOneArg(3) @>)
    test "test3932r" (isMeth <@ ClassNoArg.TestStaticMethodOneTupledArg((3,2)) @>)
    test "test3932r" (isMeth <@ ClassNoArg.TestStaticMethodOneTupledArg(p) @>)
    test "test3932t" (isMeth <@ ClassNoArg.TestStaticMethodNoArgs() @>)
    test "test3932y" (isMeth <@ ClassNoArg.TestStaticMethodTwoArgs(3,4) @>)

    test "test3932u" (isPropGet <@ ClassNoArg().TestInstanceProp @>)
    test "test3932u" (isPropGet <@ ClassNoArg().TestInstanceIndexProp(4) @>)
    test "test3932q65" (match <@ ClassNoArg().TestInstanceIndexProp(4) @> with PropertyGet(Some _, _, [(Int32(4))]) -> true | _ -> false)
    test "test3932u" (isPropSet <@ ClassNoArg().TestInstanceSettableIndexProp(4) <- 5 @>)
    test "test3932q66" (match <@ ClassNoArg().TestInstanceSettableIndexProp(4) <- 5 @> with PropertySet(Some _, _, [(Int32(4))], Int32(5)) -> true | _ -> false)
    test "test3932i" (isMeth <@ ClassNoArg().TestInstanceMethodNoArgs() @>)
    test "test3932i" (isMeth <@ ClassNoArg().TestInstanceMethodOneArg(3) @>)
    test "test3932i" (isMeth <@ ClassNoArg().TestInstanceMethodOneTupledArg((3,4)) @>)
    test "test3932i" (isMeth <@ ClassNoArg().TestInstanceMethodOneTupledArg(p) @>)
    test "test3932o" (isMeth <@ ClassNoArg().TestInstanceMethodTwoArgs(3,4) @>)

    test "test3932" (isPropGet <@ ClassNoArg.TestStaticProp @>)
    test "test3932rg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodOneArg(3) @>)
    test "test3932rg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodOneTupledArg((3,4)) @>)
    test "test3932rg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodOneTupledArg(p) @>)
    test "test3932tg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodNoArgs() @>)
    test "test3932yg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodTwoArgs(3,4) @>)

    test "test3932ug" (isPropGet <@ (GenericClassNoArg<int>()).TestInstanceProp @>)
    test "test3932ug" (isPropGet <@ (GenericClassNoArg<int>()).TestInstanceIndexProp(4) @>)
    test "test3932ug" (match <@ (GenericClassNoArg<int>()).TestInstanceIndexProp(4) @> with PropertyGet(Some _, _, [Int32(4)]) -> true | _ -> false)

    test "test3932ig" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodNoArgs() @>)
    test "test3932ig" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodOneArg(3) @>)
    test "test3932ig" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodOneTupledArg((3,4)) @>)
    test "test3932ig" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodOneTupledArg(p) @>)
    test "test3932og" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodTwoArgs(3,4) @>)

// Checks we can use ResolveMethodDn on methods marked with ReflectedDefinition attribute
module CheckRlectedMembers = 
    


    open Microsoft.FSharp.Quotations

    type ClassOneArg(a:int) = 
        [<ReflectedDefinition>]
        new () = ClassOneArg(3)
        [<ReflectedDefinition>]
        static member TestStaticMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        static member TestStaticMethodNoArgs() = 1
        [<ReflectedDefinition>]
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        [<ReflectedDefinition>]
        static member TestStaticProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y


        [<ReflectedDefinition>]
        member this.GetterIndexer
            with get (x:int) = 1

        [<ReflectedDefinition>]
        member this.TupleGetterIndexer
            with get (x:int*int) = 1

        [<ReflectedDefinition>]
        member this.Item
            with get (x:int) = 1
                
        [<ReflectedDefinition>]
        member this.TupleSetterIndexer
            with get (x : int*int) = 1

        [<ReflectedDefinition>]
        member this.SetterIndexer
            with set (x:int) (y:int) = ()

        [<ReflectedDefinition>]
        member this.Item
            with set (x:int) (y:int) = ()
            
        [<ReflectedDefinition>]
        member this.Setter
            with set (x : int) = ()
                
        [<ReflectedDefinition>]
        member this.TupleSetter
            with set (x : int*int) = ()


    type ClassNoArg() = 
        [<ReflectedDefinition>]
        static member TestStaticMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        static member TestStaticMethodNoArgs() = 1
        [<ReflectedDefinition>]
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        [<ReflectedDefinition>]
        static member TestStaticProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y

    type GenericClassNoArg<'a>() = 
        [<ReflectedDefinition>]
        new (x:'a) = GenericClassNoArg<_>()
        [<ReflectedDefinition>]
        static member TestStaticMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        static member TestStaticMethodNoArgs() = 1
        [<ReflectedDefinition>]
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        [<ReflectedDefinition>]
        static member TestStaticProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceProp = 3
        [<ReflectedDefinition>]
        member c.TestInstanceMethodOneArg(x:int) = x
        [<ReflectedDefinition>]
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y

    type ClassOneArgWithOverrideID(a:int) = 
        [<ReflectedDefinition; >]
        static member TestStaticMethodOneArg(x:int) = x
        [<ReflectedDefinition; >]
        static member TestStaticMethodNoArgs() = 1
        [<ReflectedDefinition; >]
        static member TestStaticMethodTwoArgs(x:int,y:int) = x+y
        [<ReflectedDefinition; >]
        static member TestStaticProp = 3
        [<ReflectedDefinition; >]
        member c.TestInstanceProp = 3
        [<ReflectedDefinition; >]
        member c.TestInstanceMethodOneArg(x:int) = x
        [<ReflectedDefinition; >]
        member c.TestInstanceMethodTwoArgs(x:int,y:int) = x + y


    let isNewObj (inp : Expr<_>) = match inp with NewObject (ci,_) -> Expr.TryGetReflectedDefinition(ci).IsSome | _ -> false
    let isMeth (inp : Expr<_>) = match inp with Call (_,mi,_) -> Expr.TryGetReflectedDefinition(mi).IsSome | _ -> false
    let isPropGet (inp : Expr<_>) = match inp with PropertyGet (_,mi,_) -> Expr.TryGetReflectedDefinition(mi.GetGetMethod(true)).IsSome | _ -> false
    let isPropSet (inp : Expr<_>) = match inp with PropertySet (_,mi,_,_) -> Expr.TryGetReflectedDefinition(mi.GetSetMethod(true)).IsSome | _ -> false
         
    //printfn "res = %b" (isPropGet <@ ClassOneArg.TestStaticProp @>)
    // Note: there is a ReflectedDefinition on this constructor
    test "testReflect39320a" (isNewObj <@ new ClassOneArg() @>)
    // Note: no ReflectedDefinition on this constructor
    test "testReflect39320b" (not (isNewObj <@ new ClassOneArg(3) @>))
    // Note: no ReflectedDefinition on this constructor
    test "testReflect39320c" (not (isNewObj <@ new GenericClassNoArg<int>() @>))
    // Note: there is a ReflectedDefinition on this constructor
    test "testReflect39320d" (isNewObj <@ new GenericClassNoArg<_>(3) @>)
    test "testReflect3932a" (isMeth <@ ClassOneArg.TestStaticMethodOneArg(3) @>)
    test "testReflect3932f" (isMeth <@ ClassOneArg.TestStaticMethodNoArgs() @>)
    test "testReflect3932g" (isMeth <@ ClassOneArg.TestStaticMethodTwoArgs(3,4) @>)

    test "testReflect3932q" (isPropGet <@ ClassOneArg(3).TestInstanceProp @>)
    test "testReflect3932w" (isMeth <@ ClassOneArg(3).TestInstanceMethodOneArg(3) @>)
    test "testReflect3932e" (isMeth <@ ClassOneArg(3).TestInstanceMethodTwoArgs(3,4) @>)

    test "testReflect3932" (isPropGet <@ ClassNoArg.TestStaticProp @>)
    test "testReflect3932r" (isMeth <@ ClassNoArg.TestStaticMethodOneArg(3) @>)
    test "testReflect3932t" (isMeth <@ ClassNoArg.TestStaticMethodNoArgs() @>)
    test "testReflect3932y" (isMeth <@ ClassNoArg.TestStaticMethodTwoArgs(3,4) @>)

    test "testReflect3932u" (isPropGet <@ ClassNoArg().TestInstanceProp @>)
    test "testReflect3932i" (isMeth <@ ClassNoArg().TestInstanceMethodOneArg(3) @>)
    test "testReflect3932o" (isMeth <@ ClassNoArg().TestInstanceMethodTwoArgs(3,4) @>)

    test "testReflect3932q1" (isPropSet <@ ClassOneArg(3).Setter <- 3 @>)
    test "testReflect3932q2" (isPropGet <@ ClassOneArg(3).GetterIndexer(3) @>)
    test "testReflect3932q3" (isPropGet <@ ClassOneArg(3).[3] @>)
    test "testReflect3932q4" (isPropGet <@ ClassOneArg(3).TupleGetterIndexer((3,4)) @>)
    test "testReflect3932q5" (isPropSet <@ ClassOneArg(3).SetterIndexer(3) <- 3 @>)
    test "testReflect3932q6" (isPropSet <@ ClassOneArg(3).[3] <- 3 @>)
    test "testReflect3932q7" (isPropSet <@ ClassOneArg(3).TupleSetter <- (3,4) @>)


    test "testReflect3932rg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodOneArg(3) @>)
    test "testReflect3932tg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodNoArgs() @>)
    test "testReflect3932yg" (isMeth <@ GenericClassNoArg<int>.TestStaticMethodTwoArgs(3,4) @>)

    test "testReflect3932ug" (isPropGet <@ (GenericClassNoArg<int>()).TestInstanceProp @>)
    test "testReflect3932ig" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodOneArg(3) @>)
    test "testReflect3932og" (isMeth <@ (GenericClassNoArg<int>()).TestInstanceMethodTwoArgs(3,4) @>)

    test "testReflect3932a" (isMeth <@ ClassOneArgWithOverrideID.TestStaticMethodOneArg(3) @>)
    test "testReflect3932f" (isMeth <@ ClassOneArgWithOverrideID.TestStaticMethodNoArgs() @>)
    test "testReflect3932g" (isMeth <@ ClassOneArgWithOverrideID.TestStaticMethodTwoArgs(3,4) @>)

    test "testReflect3932q" (isPropGet <@ ClassOneArgWithOverrideID(3).TestInstanceProp @>)
    test "testReflect3932w" (isMeth <@ ClassOneArgWithOverrideID(3).TestInstanceMethodOneArg(3) @>)
    test "testReflect3932e" (isMeth <@ ClassOneArgWithOverrideID(3).TestInstanceMethodTwoArgs(3,4) @>)


module Bug959_Regression = begin
    open Microsoft.FSharp
    open Microsoft.FSharp

    //let f x  = <@ _ @> (lift x)

    <@
      match 1.0,"b" with
      | 1.0, "a" ->
        ""
      | 2.0, "b" ->
        ""
      | _ -> "nada" @>
end

module MoreQuotationsTests = 

    let t1 = <@@ try 1 with e when true -> 2 | e -> 3 @@>
    printfn "t1 = %A" t1

    [<ReflectedDefinition>]
    let k (x:int) =
       try 1 with _ when true -> 2 | e -> 3

    let t2 = <@@ Map.empty.[0] @@>
    printfn "t2 = %A" t2
    let t3 = <@@ Map.empty.[0] @@>
    printfn "t2 = %A" t3

    let t4 = <@@ use a = new System.IO.StreamWriter(System.IO.Stream.Null) in a @@>
    printfn "t4 = %A" t4

    let t5 = <@@ try failwith "test" with _ when true -> 0 @@>
    printfn "t5 = %A" t5
    
    let t6 = <@@ let mutable a = 0 in a <- 2 @@>

    printfn "t6 = %A" t6

    let f (x: _ byref) = x

    let t7 = <@@ let mutable a = 0 in f (&a) @@>
    printfn "t7 = %A" t7
    
    let t8 = <@@ for i in 1s .. 10s do printfn "%A" i @@>
    printfn "t8 = %A" t8

    let t9 = <@@ try failwith "test" with Failure _ -> 0  @@>
    printfn "t9 = %A" t9

    let t9b = <@@ Failure "fil" @@>
    printfn "t9b = %A" t9b
    let t9c = <@@ match Failure "fil" with Failure msg -> msg |  _ -> "no" @@>
    printfn "t9c = %A" t9c

    let t10 = <@@ try failwith "test" with Failure _ -> 0 |  _ -> 1 @@>
    printfn "t10 = %A" t10

    let t11 = <@@ try failwith "test" with :? System.NullReferenceException -> 0 @@>
    printfn "t11 = %A" t11

    let t12 = <@@ try failwith "test" with :? System.NullReferenceException as n -> 0 @@>
    printfn "t12 = %A" t12

    let t13 = <@@ try failwith "test" with Failure _ -> 1 | :? System.NullReferenceException as n -> 0 @@>
    printfn "t13 = %A" t13

    let t14 = <@@ try failwith "test" with _ when true -> 0 @@>
    printfn "t14 = %A" t14

    let _ = <@@ let x : int option = None in x.IsSome @@> |> printfn "quote = %A" 
    let _ = <@@ let x : int option = None in x.IsNone @@> |> printfn "quote = %A" 
    let _ = <@@ let x : int option = None in x.Value @@> |> printfn "quote = %A" 
    let _ = <@@ let x : int option = None in x.ToString() @@> |> printfn "quote = %A" 

    module Extensions = 
        type System.Object with 
            member x.ExtensionMethod0()  = 3
            member x.ExtensionMethod1()  = ()
            member x.ExtensionMethod2(y:int)  = y
            member x.ExtensionMethod3(y:int)  = ()
            member x.ExtensionMethod4(y:int,z:int)  = y + z
            member x.ExtensionMethod5(y:(int*int))  = y 
            member x.ExtensionProperty1 = 3
            member x.ExtensionProperty2 with get() = 3
            member x.ExtensionProperty3 with set(v:int) = ()
            member x.ExtensionIndexer1 with get(idx:int) = idx
            member x.ExtensionIndexer2 with set(idx:int) (v:int) = ()

        type System.Int32 with 
            member x.Int32ExtensionMethod0()  = 3
            member x.Int32ExtensionMethod1()  = ()
            member x.Int32ExtensionMethod2(y:int)  = y
            member x.Int32ExtensionMethod3(y:int)  = ()
            member x.Int32ExtensionMethod4(y:int,z:int)  = y + z
            member x.Int32ExtensionMethod5(y:(int*int))  = y 
            member x.Int32ExtensionProperty1 = 3
            member x.Int32ExtensionProperty2 with get() = 3
            member x.Int32ExtensionProperty3 with set(v:int) = ()
            member x.Int32ExtensionIndexer1 with get(idx:int) = idx
            member x.Int32ExtensionIndexer2 with set(idx:int) (v:int) = ()
 
        let v = new obj()
        let _ = <@@ v.ExtensionMethod0() @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod1() @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod2(3) @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod3(3) @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod4(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod5(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionProperty1 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionProperty2 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionProperty3 <- 4 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionIndexer1(3) @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionIndexer2(3) <- 4 @@> |> printfn "quote = %A"

        let _ = <@@ v.ExtensionMethod0 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod1 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod2 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod3 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod4 @@> |> printfn "quote = %A"
        let _ = <@@ v.ExtensionMethod5 @@> |> printfn "quote = %A"

        let v2 = 3
        let _ = <@@ v2.ExtensionMethod0() @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod1() @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod2(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod3(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod4(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod5(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionProperty1 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionProperty2 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionProperty3 <- 4 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionIndexer1(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionIndexer2(3) <- 4 @@> |> printfn "quote = %A"

        let _ = <@@ v2.ExtensionMethod0 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod1 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod2 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod3 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod4 @@> |> printfn "quote = %A"
        let _ = <@@ v2.ExtensionMethod5 @@> |> printfn "quote = %A"

        let _ = <@@ v2.Int32ExtensionMethod0() @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod1() @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod2(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod3(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod4(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod5(3,4) @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionProperty1 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionProperty2 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionProperty3 <- 4 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionIndexer1(3) @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionIndexer2(3) <- 4 @@> |> printfn "quote = %A"

        let _ = <@@ v2.Int32ExtensionMethod0 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod1 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod2 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod3 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod4 @@> |> printfn "quote = %A"
        let _ = <@@ v2.Int32ExtensionMethod5 @@> |> printfn "quote = %A"


module QuotationConstructionTests = 
    let arr = [| 1;2;3;4;5 |]
    let f : int -> int = printfn "hello"; (fun x -> x)
    let f2 : int * int -> int -> int = printfn "hello"; (fun (x,y) z -> x + y + z)
    let F (x:int) = x
    let F2 (x:int,y:int) (z:int) = x + y + z
    let ctorof q = match q with Patterns.NewObject(cinfo,_) -> cinfo | _ -> failwith "ctorof"
    let methodof q = match q with DerivedPatterns.Lambdas(_,Patterns.Call(_,minfo,_)) -> minfo | _ -> failwith "methodof"
    let fieldof q = match q with Patterns.FieldGet(_,finfo) -> finfo | _ -> failwith "fieldof"
    let ucaseof q = match q with Patterns.NewUnionCase(ucinfo,_) -> ucinfo | _ -> failwith "ucaseof"
    let getof q = match q with Patterns.PropertyGet(_,pinfo,_) -> pinfo | _ -> failwith "getof"
    let setof q = match q with Patterns.PropertySet(_,pinfo,_,_) -> pinfo | _ -> failwith "setof"
    check "vcknwwe01" (match Expr.AddressOf <@@ arr.[3] @@> with AddressOf(expr) -> expr = <@@ arr.[3] @@> | _ -> false) true
    check "vcknwwe02" (match Expr.AddressSet (Expr.AddressOf <@@ arr.[3] @@>, <@@ 4 @@>) with AddressSet(AddressOf(expr),v) -> expr = <@@ arr.[3] @@> && v = <@@ 4 @@> | _ -> false) true
    check "vcknwwe03" (match Expr.Application(<@@ f @@>,<@@ 5 @@>) with Application(f1,x) -> f1 = <@@ f @@> && x = <@@ 5 @@> | _ -> false) true
    check "vcknwwe04" (match Expr.Applications(<@@ f @@>,[[ <@@ 5 @@> ]]) with Applications(f1,[[x]]) -> f1 = <@@ f @@> && x = <@@ 5 @@> | _ -> false) true
    check "vcknwwe05" (match Expr.Applications(<@@ f2 @@>,[[ <@@ 5 @@>;<@@ 6 @@> ]; [ <@@ 7 @@> ]]) with Applications(f1,[[x;y];[z]]) -> f1 = <@@ f2 @@> && x = <@@ 5 @@> && y = <@@ 6 @@> && z = <@@ 7 @@>  | _ -> false) true
    check "vcknwwe06" (match Expr.Call(methodof <@@ F2 @@>,[ <@@ 5 @@>;<@@ 6 @@>; <@@ 7 @@> ]) with Call(None,minfo,[x;y;z]) -> minfo = methodof <@@ F2 @@> && x = <@@ 5 @@> && y = <@@ 6 @@> && z = <@@ 7 @@>  | _ -> false) true
    check "vcknwwe07" (Expr.Cast(<@@ 5 @@>) : Expr<int>) (<@ 5 @>)
    check "vcknwwe08" (try let _ = Expr.Cast(<@@ 5 @@>) : Expr<string> in false with :? System.ArgumentException -> true) true
    check "vcknwwe09" (match Expr.Coerce(<@@ 5 @@>, typeof<obj>) with Coerce(q,ty) -> ty = typeof<obj> && q = <@@ 5 @@> | _ -> false) true
    check "vcknwwe0q" (match Expr.DefaultValue(typeof<obj>) with DefaultValue(ty) -> ty = typeof<obj> | _ -> false) true
    check "vcknwwe0w" (match Expr.FieldGet(typeof<int>.GetField("MaxValue")) with FieldGet(None,finfo) -> finfo = typeof<int>.GetField("MaxValue") | _ -> false) true
    check "vcknwwe0e" (match Expr.FieldSet(typeof<int>.GetField("MaxValue"),<@@ 1 @@>) with FieldSet(None,finfo,v) -> finfo = typeof<int>.GetField("MaxValue") && v = <@@ 1 @@> | _ -> false) true
    check "vcknwwe0r" (match Expr.ForIntegerRangeLoop(Var.Global("i",typeof<int>),<@@ 1 @@>,<@@ 10 @@>,<@@ () @@>) with ForIntegerRangeLoop(v,start,finish,body) -> v = Var.Global("i",typeof<int>) && start = <@@ 1 @@> && finish = <@@ 10 @@> && body = <@@ () @@>  | _ -> false) true
    check "vcknwwe0t" (match Expr.GlobalVar("i") : Expr<int> with Var(v) -> v = Var.Global("i",typeof<int>)   | _ -> false) true
    check "vcknwwe0y" (match Expr.IfThenElse(<@@ true @@>,<@@ 1 @@>,<@@ 2 @@>) with IfThenElse(gd,t,e) -> gd = <@@ true @@> && t = <@@ 1 @@> && e = <@@ 2 @@>   | _ -> false) true
    check "vcknwwe0u" (match Expr.Lambda(Var.Global("i",typeof<int>), <@@ 2 @@>) with Lambda(v,b) -> v = Var.Global("i",typeof<int>) && b = <@@ 2 @@>   | _ -> false) true
    check "vcknwwe0i" (match Expr.Let(Var.Global("i",typeof<int>), <@@ 2 @@>, <@@ 3 @@>) with Let(v,e,b) -> v = Var.Global("i",typeof<int>) && e = <@@ 2 @@> && b = <@@ 3 @@>   | _ -> false) true
    check "vcknwwe0o" (match Expr.LetRecursive([(Var.Global("i",typeof<int>), <@@ 2 @@>)], <@@ 3 @@>) with LetRecursive([(v,e)],b) -> v = Var.Global("i",typeof<int>) && e = <@@ 2 @@> && b = <@@ 3 @@>   | _ -> false) true
    check "vcknwwe0p" (match Expr.LetRecursive([(Var.Global("i",typeof<int>), <@@ 2 @@>);(Var.Global("j",typeof<int>), <@@ 3 @@>)], <@@ 3 @@>) with LetRecursive([(v1,e1);(v2,e2)],b) -> v1 = Var.Global("i",typeof<int>) && v2 = Var.Global("j",typeof<int>) && e1 = <@@ 2 @@> && e2 = <@@ 3 @@> && b = <@@ 3 @@>   | _ -> false) true
    check "vcknwwe0a" (Expr.NewArray(typeof<int>,[ <@@ 1 @@>; <@@ 2 @@> ])) <@@ [| 1;2 |] @@>
    check "vcknwwe0s" (match Expr.NewDelegate(typeof<Action<int>>,[ Var.Global("i",typeof<int>) ], <@@ () @@>) with NewDelegate(ty,[v],e) -> ty = typeof<Action<int>> && v = Var.Global("i",typeof<int>) && e = <@@ () @@> | _ -> false) true
    check "vcknwwe0d" (match Expr.NewObject(ctorof <@@ new obj() @@> ,[ ]) with NewObject(ty,[]) -> ty = ctorof <@@ new obj() @@> | _ -> false) true
    check "vcknwwe0f" (match Expr.NewObject(ctorof <@@ new System.String('a',3) @@> ,[ <@@ 'b' @@>; <@@ 4 @@>]) with NewObject(ty,[x;y]) -> ty = ctorof <@@ new string('a',3) @@> && x = <@@ 'b' @@> && y = <@@ 4 @@> | _ -> false) true
    check "vcknwwe0g" (Expr.NewRecord(typeof<int ref> ,[ <@@ 4 @@> ])) <@@ { contents = 4 } @@>
    check "vcknwwe0h" (try let _ = Expr.NewTuple([]) in false with :? System.ArgumentException -> true) true
    check "vcknwwe0j" (try let _ = Expr.NewTuple([ <@@ 1 @@> ]) in true with :? System.ArgumentException -> false) true
    check "vcknwwe0k" (match Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>]) with NewTuple([x;y]) -> x = <@@ 'b' @@> && y = <@@ 4 @@> | _ -> false) true
    check "vcknwwe0l" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>])) <@@ ('b',4) @@>
    check "vcknwwe0z" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>])) <@@ ('b',4,5) @@>
    check "vcknwwe0x" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>])) <@@ ('b',4,5,6) @@>
    check "vcknwwe0c" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>; <@@ 7 @@>])) <@@ ('b',4,5,6,7) @@>
    check "vcknwwe0v" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>; <@@ 7 @@>; <@@ 8 @@>])) <@@ ('b',4,5,6,7,8) @@>
    check "vcknwwe0b" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>; <@@ 7 @@>; <@@ 8 @@>; <@@ 9 @@>])) <@@ ('b',4,5,6,7,8,9) @@>
    check "vcknwwe0n" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>; <@@ 7 @@>; <@@ 8 @@>; <@@ 9 @@>; <@@ 10 @@>])) <@@ ('b',4,5,6,7,8,9,10) @@>
    check "vcknwwe0m" (Expr.NewTuple([ <@@ 'b' @@>; <@@ 4 @@>; <@@ 5 @@>; <@@ 6 @@>; <@@ 7 @@>; <@@ 8 @@>; <@@ 9 @@>; <@@ 10 @@>])) <@@ ('b',4,5,6,7,8,9,10) @@>
    check "vcknwwe011" (Expr.NewUnionCase(ucaseof <@@ Some(3) @@>,[ <@@ 4 @@> ])) <@@ Some(4) @@>
    check "vcknwwe022" (Expr.NewUnionCase(ucaseof <@@ None @@>,[  ])) <@@ None @@>
    check "vcknwwe033" (try let _ = Expr.NewUnionCase(ucaseof <@@ Some(3) @@>,[  ]) in false with :? ArgumentException -> true) true
    check "vcknwwe044" (try let _ = Expr.NewUnionCase(ucaseof <@@ None @@>,[ <@@ 1 @@> ]) in false with :? ArgumentException -> true) true
    check "vcknwwe055" (Expr.PropertyGet(getof <@@ System.DateTime.Now @@>,[  ])) <@@ System.DateTime.Now @@>
    check "vcknwwe066" (try let _ = Expr.PropertyGet(getof <@@ System.DateTime.Now @@>,[ <@@ 1 @@> ]) in false with :? ArgumentException -> true) true
    check "vcknwwe077" (Expr.PropertyGet(<@@ "3" @@>, getof <@@ "1".Length @@>)) <@@ "3".Length @@>
    check "vcknwwe088" (Expr.PropertyGet(<@@ "3" @@>, getof <@@ "1".Length @@>,[  ])) <@@ "3".Length @@>
    check "vcknwwe099" (Expr.PropertySet(<@@ (new System.Windows.Forms.Form()) @@>, setof <@@ (new System.Windows.Forms.Form()).Text <- "2" @@>, <@@ "3" @@> )) <@@ (new System.Windows.Forms.Form()).Text <- "3" @@>
    check "vcknwwe0qq" (Expr.Quote(<@@ "1" @@>)) <@@ <@@ "1" @@> @@>
    check "vcknwwe0ww" (Expr.Sequential(<@@ () @@>, <@@ 1 @@>)) <@@ (); 1 @@>
    check "vcknwwe0ee" (Expr.TryFinally(<@@ 1 @@>, <@@ () @@>)) <@@ try 1 finally () @@>
    check "vcknwwe0rr" (match Expr.TryWith(<@@ 1 @@>, Var.Global("e1",typeof<exn>), <@@ 1 @@>, Var.Global("e2",typeof<exn>), <@@ 2 @@>) with TryWith(b,v1,ef,v2,eh) -> b = <@@ 1 @@> && eh = <@@ 2 @@> && ef = <@@ 1 @@> && v1 = Var.Global("e1",typeof<exn>) && v2 = Var.Global("e2",typeof<exn>)| _ -> false) true 
    check "vcknwwe0tt" (match Expr.TupleGet(<@@ (1,2) @@>, 0) with TupleGet(b,n) -> b = <@@ (1,2) @@> && n = 0 | _ -> false) true 
    check "vcknwwe0yy" (match Expr.TupleGet(<@@ (1,2) @@>, 1) with TupleGet(b,n) -> b = <@@ (1,2) @@> && n = 1 | _ -> false) true 
    check "vcknwwe0uu" (try let _ = Expr.TupleGet(<@@ (1,2) @@>, 2) in false with :? ArgumentException -> true) true
    check "vcknwwe0ii" (try let _ = Expr.TupleGet(<@@ (1,2) @@>, -1) in false with :? ArgumentException -> true) true
    for i = 0 to 7 do 
        check "vcknwwe0oo" (match Expr.TupleGet(<@@ (1,2,3,4,5,6,7,8) @@>, i) with TupleGet(b,n) -> b = <@@ (1,2,3,4,5,6,7,8) @@> && n = i | _ -> false) true 
    check "vcknwwe0pp" (match Expr.TypeTest(<@@ new obj() @@>, typeof<string>) with TypeTest(e,ty) -> e = <@@ new obj() @@> && ty = typeof<string> | _ -> false) true
    check "vcknwwe0aa" (match Expr.UnionCaseTest(<@@ [] : int list @@>, ucaseof <@@ [] : int list @@> ) with UnionCaseTest(e,uc) -> e = <@@ [] : int list @@> && uc = ucaseof <@@ [] : int list @@>  | _ -> false) true
    check "vcknwwe0ss" (Expr.Value(3)) <@@ 3 @@>
    check "vcknwwe0dd" (match Expr.Var(Var.Global("i",typeof<int>)) with Var(v) -> v = Var.Global("i",typeof<int>) | _ -> false) true
    check "vcknwwe0ff" (match Expr.VarSet(Var.Global("i",typeof<int>), <@@ 4 @@>) with VarSet(v,q) -> v = Var.Global("i",typeof<int>) && q = <@@ 4 @@>  | _ -> false) true
    check "vcknwwe0gg" (match Expr.WhileLoop(<@@ true @@>, <@@ () @@>) with WhileLoop(g,b) -> g = <@@ true @@> && b = <@@ () @@>  | _ -> false) true

module EqualityOnExprDoesntFail = 
    let q = <@ 1 @>
    check "we09ceo" (q.Equals(1)) false
    check "we09ceo" (q.Equals(q)) true
    check "we09ceo" (q.Equals(<@ 1 @>)) true
    check "we09ceo" (q.Equals(<@ 2 @>)) false
    check "we09ceo" (q.Equals(null)) false
    
module EqualityOnVarDoesntFail = 
    let v = Var.Global("c",typeof<int>)
    let v2 = Var.Global("c",typeof<int>)
    let v3 = Var.Global("d",typeof<int>)
    check "we09ceo2" (v.Equals(1)) false
    check "we09ceo2" (v.Equals(v)) true
    check "we09ceo2" (v.Equals(v2)) true
    check "we09ceo2" (v.Equals(v3)) false
    check "we09ceo2" (v.Equals(null)) false
    
module RelatedChange3628 =
    // Fix for 3628 translates "do x" into "do (x;())" when x is not unit typed.
    // This regression checks the quotated form.

    open System
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns
    open Microsoft.FSharp.Quotations.DerivedPatterns

    [<ReflectedDefinition>] 
    let f (x:int) = do x
    let (Call(None,minfo,args)) = <@ f 1 @>
    let (Some lamexp) = Quotations.Expr.TryGetReflectedDefinition(minfo)
    let (Lambda(v,body)) = lamexp
    let (Sequential (a,b)) = body
    let (Var v2) = a
    check "RelatedChange3628.a" v v2
    check "RelatedChange3628.b" b <@@ () @@>

module Check3628 =
  let inline fA (x:int) = (do x) 
  let        fB (x:int) = (do x)
  let resA = fA 12
  let resB = fB 13
  let mutable (z:unit) = ()
  z <- fA 14
  z <- fB 15

module ReflectedDefinitionForPatternInputTest = 

   [<ReflectedDefinition>] 
   let [x] = [1];;


module TestQuotationOfCOnstructors = 

    type MyClassWithNoFields [<ReflectedDefinition>] () = 
        member this.Bar z = ()

    type MyClassWithFields [<ReflectedDefinition>]() = 
        let x = 12
        let y = x
        let w = x // note this variable is not used in any method and becomes local to the constructor

        [<ReflectedDefinition>]
        member this.Bar z = x + z + y

    type MyGenericClassWithArgs<'T> [<ReflectedDefinition>](inp:'T) = 
        let x = inp
        let y = x
        let w = x // note this variable is not used in any method and becomes local to the constructor

        [<ReflectedDefinition>]
        member this.Bar z = (x,y,z)

    type MyGenericClassWithTwoArgs<'T> [<ReflectedDefinition>](inpA:'T, inpB:'T) = // note, inpB is captured 
        let x = inpA
        let y = x
        let w = x // note this variable is not used in any method and becomes local to the constructor

        [<ReflectedDefinition>]
        member this.Bar z = (x,y,z,inpB)

    type MyClassWithAsLetMethod () = 
        [<ReflectedDefinition>]
        let f() = 1

        [<ReflectedDefinition>]
        member this.Bar z = f()


    Expr.TryGetReflectedDefinition (typeof<MyClassWithNoFields>.GetConstructors().[0]) |> printfn "%A"
    Expr.TryGetReflectedDefinition (typeof<MyClassWithFields>.GetConstructors().[0]) |> printfn "%A"
    Expr.TryGetReflectedDefinition (typeof<MyGenericClassWithArgs<int>>.GetConstructors().[0]) |> printfn "%A"
    Expr.TryGetReflectedDefinition (typeof<MyGenericClassWithTwoArgs<int>>.GetConstructors().[0]) |> printfn "%A"



    test "vkjnkvrw2"
       (match Expr.TryGetReflectedDefinition (typeof<MyClassWithFields>.GetConstructors().[0]) with 
        | Some 
           (Lambda 
              (_,Sequential 
                   (NewObject objCtor,
                    Sequential 
                       (FieldSet (Some (Var thisVar0), xField1, Int32 12),
                        Sequential 
                           (FieldSet (Some (Var thisVar1), yField,FieldGet (Some (Var thisVar2),xField2)),
                            Let (wVar,FieldGet (Some (Var thisVar3), xField3), Unit))))))
            -> 
                thisVar0 = thisVar1 && 
                thisVar1 = thisVar2 && 
                thisVar2 = thisVar3 && 
                thisVar1.Name = "this" && 
                thisVar1.Type = typeof<MyClassWithFields> && 
                thisVar1 = Var.Global("this", typeof<MyClassWithFields>) && 
                xField1.Name = "x" &&
                xField2.Name = "x" &&
                xField3.Name = "x" &&
                yField.Name = "y" &&
                wVar.Name = "w" &&
                wVar.Type = typeof<int>  
                
        | _ -> false)



    test "vkjnkvrw3"
       (match Expr.TryGetReflectedDefinition (typeof<MyGenericClassWithArgs<int>>.GetConstructors().[0]) with 
        | Some 
           (Lambda
              (inpVar,Sequential 
                   (NewObject objCtor,
                    Sequential 
                       (FieldSet (Some (Var thisVar0), xField1, Var inpVar1),
                        Sequential 
                           (FieldSet (Some (Var thisVar1), yField,FieldGet (Some (Var thisVar2),xField2)),
                            Let (wVar,FieldGet (Some (Var thisVar3), xField3), Unit))))))
            -> 
                inpVar.Name = "inp" &&
                inpVar.Type = typeof<int> &&
                thisVar0 = thisVar1 && 
                thisVar1 = thisVar2 && 
                thisVar2 = thisVar3 && 
                thisVar1.Name = "this" && 
                thisVar1.Type = typeof<MyGenericClassWithArgs<int>> && 
                thisVar1 = Var.Global("this", typeof<MyGenericClassWithArgs<int>>) && 
                xField1.Name = "x" &&
                xField2.Name = "x" &&
                xField3.Name = "x" &&
                yField.Name = "y" &&
                wVar.Name = "w" &&
                wVar.Type = typeof<int>  
                
        | _ -> false)


    test "vkjnkvrw4"
       (match Expr.TryGetReflectedDefinition (typeof<MyGenericClassWithTwoArgs<int>>.GetConstructors().[0]) with 
        | Some 
           (Lambdas
              ([[inpAVar1; inpBVar1]],
               Sequential 
                   (NewObject objCtor,
                    Sequential 
                       (FieldSet (Some (Var thisVar0), inpBField, Var inpBVar2),
                        Sequential 
                           (FieldSet (Some (Var thisVar1), xField1, Var inpAVar2),
                            Sequential 
                               (FieldSet (Some (Var thisVar2), yField,FieldGet (Some (Var thisVar3),xField2)),
                                Let (wVar,FieldGet (Some (Var thisVar4), xField3), Unit)))))))
            -> true ||
                inpAVar1 = inpAVar2 &&
                inpAVar1.Name = "inpA" &&
                inpAVar2.Type = typeof<int> &&
                inpBVar1 = inpBVar2 &&
                inpBVar1.Name = "inpB" &&
                inpBVar1.Type = typeof<int> &&
                thisVar0 = thisVar1 && 
                thisVar1 = thisVar2 && 
                thisVar2 = thisVar3 && 
                thisVar3 = thisVar4 && 
                thisVar1.Name = "this" && 
                thisVar1 = Var.Global("this", typeof<MyGenericClassWithTwoArgs<int>>) && 
                thisVar1.Type = typeof<MyGenericClassWithTwoArgs<int>> && 
                inpBField.Name = "inpB" &&
                xField1.Name = "x" &&
                xField2.Name = "x" &&
                xField3.Name = "x" &&
                yField.Name = "y" &&
                wVar.Name = "w" &&
                wVar.Type = typeof<int>  
                
        | _ -> false)

    // Also test getting the reflected definition for private members implied by "let f() = ..." bindings
    let fMethod = (typeof<MyClassWithAsLetMethod>.GetMethod("f", Reflection.BindingFlags.Instance ||| Reflection.BindingFlags.Public ||| Reflection.BindingFlags.NonPublic))

    test "vkjnkvrw1"
       (match Expr.TryGetReflectedDefinition fMethod with 
        | Some (Lambdas ([[thisVar];[unitVar]], Int32 1))
            -> unitVar.Type = typeof<unit>
        | _ -> false)

    
    Expr.TryGetReflectedDefinition fMethod |> printfn "%A"

    test "vkjnkvrw0"
       (match Expr.TryGetReflectedDefinition (typeof<MyClassWithNoFields>.GetConstructors().[0]) with 
        | Some (Lambda (unitVar,Sequential (NewObject objCtor,Unit)))
            -> unitVar.Type = typeof<unit>
        | _ -> false)

module IndexedPropertySetTest = 
    open System
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns

    // Having int[] will allow us to swap es and l in PropertySet builder for testing.
    type Foo (array:int[]) =
        member t.Item with get (index:int) = array.[index]
                      and set (index:int) (value:int) = do array.[index] <- value

                      
    let testExprPropertySet () =
        let foo = new Foo([|0..4|])
        let expr = <@do foo.[2] <- 0@>

        //printfn "%A" expr

        // let's rebuild expr ourself and bind it to bexpr.
        let bexpr =
            match expr with
            | PropertySet (inst, pi, l, es) ->
                match inst with
                | Some(e) ->
                    Expr.PropertySet(e, pi, es, l) // swaping params 2 and 3 e.g. (e, pi, l.[0], [es]) yield to OK
                | _ -> failwith ""
            | _ -> failwith ""   

        //printfn "%A" bexpr

        let result = bexpr.Equals(expr)
        if result then printfn "Test OK."
        else printfn "Test KO."

    do testExprPropertySet ()

let _ = 
  if not failures.IsEmpty then (printfn "Test Failed, failures = %A" failures; exit 1) 
  else (stdout.WriteLine "Test Passed"; 
        System.IO.File.WriteAllText("test.ok","ok"); 
        exit 0)

