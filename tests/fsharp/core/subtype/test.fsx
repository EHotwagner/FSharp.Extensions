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

let mutable failures = []
let report_failure s = 
  stderr.WriteLine " NO"; failures <- s :: failures
let test s b = stderr.Write(s:string);  if b then stderr.WriteLine " OK" else report_failure s
let check s v1 v2 = test s (v1 = v2)

(* TEST SUITE FOR SUBTYPE CONSTRAINTS *)


let argv = System.Environment.GetCommandLineArgs() 
let SetCulture() = 
  if argv.Length > 2 && argv.[1] = "--culture" then  begin
    let cultureString = argv.[2] in 
    let culture = new System.Globalization.CultureInfo(cultureString) in 
    stdout.WriteLine ("Running under culture "+culture.ToString()+"...");
    System.Threading.Thread.CurrentThread.CurrentCulture <-  culture
  end 
  

do SetCulture()    

open System
open System.IO

open System.Collections.Generic

(* 'a[] :> ICollection<'a> *)
let f1 (x: 'a[]) = (x :> ICollection<'a>) 
do let x = f1 [| 3;4; |] in test "test239809" (x.Contains(3))

(* 'a[] :> IList<'a> *)
let f2 (x: 'a[]) = (x :> IList<'a>) 
do let x = f2 [| 3;4; |] in test "test239810" (x.Item(1) = 4)

(* 'a[] :> IEnumerable<'a> *)
let f3 (x: 'a[]) = (x :> IEnumerable<'a>) 
do let x = f3 [| 3;4; |] in for x in x do (Printf.printf "val %d\n" x) done

(* Call 'foreachG' using an IList<int> (solved to IEnumerable<int>) *)
let f4 (x: 'a[]) = (x :> IList<'a>) 
do let x = f4 [| 31;42; |] in for x in x do (Printf.printf "val %d\n" x) done

(* Call 'foreachG' using an ICollection<int> (solved to IEnumerable<int>) *)
let f5 (x: 'a[]) = (x :> ICollection<'a>) 
do let x = f5 [| 31;42; |] in for x in x do (Printf.printf "val %d\n" x) done


[<Measure>] type kg

let testUpcastToArray1 (x: 'a[]) = (x :> System.Array) 
let testUpcastToArray2 (x: 'a[,]) = (x :> System.Array) 
let testUpcastToArray3 (x: 'a array) = (x :> System.Array) 
let testUpcastToArray4 (x: 'a array  array) = (x :> System.Array) 

let testUpcastToIEnumerable1 (x: 'a[]) = (x :> System.Collections.IEnumerable) 
let testUpcastToIEnumerable2 (x: 'a[,]) = (x :> System.Collections.IEnumerable) 
let testUpcastToIEnumerable3 (x: 'a array) = (x :> System.Collections.IEnumerable) 
let testUpcastToIEnumerable4 (x: 'a array  array) = (x :> System.Collections.IEnumerable) 

let testUpcastToICollection1 (x: 'a[]) = (x :> System.Collections.ICollection) 
let testUpcastToICollection2 (x: 'a[,]) = (x :> System.Collections.ICollection) 
let testUpcastToICollection3 (x: 'a array) = (x :> System.Collections.ICollection) 
let testUpcastToICollection4 (x: 'a array  array) = (x :> System.Collections.ICollection) 


let testUpcastToIList1 (x: 'a[]) = (x :> System.Collections.IList) 
let testUpcastToIList2 (x: 'a[,]) = (x :> System.Collections.IList) 
let testUpcastToIList3 (x: 'a array) = (x :> System.Collections.IList) 
let testUpcastToIList4 (x: 'a array  array) = (x :> System.Collections.IList) 

let testUpcastToValueType1 (x: int) = (x :> System.ValueType) 
let testUpcastToValueType2 (x: bool) = (x :> System.ValueType) 
let testUpcastToValueType3 (x: char) = (x :> System.ValueType) 
let testUpcastToValueType4 (x: uint32) = (x :> System.ValueType) 
let testUpcastToValueType5 (x: System.DateTime) = (x :> System.ValueType) 
let testUpcastToValueType6 (x: System.ValueType) = (x :> System.ValueType) 
let testUpcastToValueType7 (x: float<kg>) = (x :> System.ValueType)

let testUpcastToEnum1 (x: System.Reflection.BindingFlags) = (x :> System.Enum) 
let testUpcastToEnum6 (x: System.Enum) = (x :> System.Enum) 


let testUpcastToDelegate1 (x: System.Threading.ThreadStart) = (x :> System.Delegate) 

let testUpcastToMulticastDelegate1 (x: System.Threading.ThreadStart) = (x :> System.MulticastDelegate) 

do for name in Directory.GetFiles("c:\\") do stdout.WriteLine name done

let f (x : #System.IComparable<'a>) = 1


module RandomGregChapmanTest = begin
    open System.Collections.Generic
    open Microsoft.FSharp.Collections

    type Range = {Dummy: int}
    type Worksheet = {Name: string; UsedRange: Range}
    type Track = {Album: string; Track: string; Artist: string}

    type 
        Workbook  = class
            new() = {}
        
            member x.Sheets: System.Collections.IEnumerable =
                upcast Seq.unfold (
                    fun index -> 
                        if index >= 4 then None
                        else Some({Name="Sheet"+(index.ToString()); UsedRange={Dummy=index}}, index+1)
                ) 1
        end
        
    let rangeContains _ _ = true

    let rowstr s (i1:int) (i2:int) = s + ( i1.ToString()) + ( i2.ToString())

    let rangeRowIter range =
        Seq.unfold (
            fun index ->
                if index >= 4 then None
                else begin
                    let row = 
                       Map.ofList [
                        "ALBUM", (rowstr "Album" range.Dummy index);
                        "TRACK", (rowstr "Track" range.Dummy index);
                        "LEADER", (rowstr "Leader" range.Dummy index) ]
                    in
                    Some(row, index+1)
                end
        ) 1

    let Run (matchtext: string) =
        let GetMatchingRanges() =
            let workbook = new Workbook() in
            Seq.map (fun (sheet: Worksheet) -> (sheet.Name, sheet.UsedRange)) (
                Seq.filter (fun sheet -> rangeContains sheet.UsedRange matchtext) 
                  (Seq.cast workbook.Sheets : IEnumerable<Worksheet>)
            )
        in
        let ParseRanges ranges =
            ranges |> Seq.map (
                fun (name, range) ->
                    let items = List.ofSeq ((rangeRowIter range) |> Seq.map (fun row -> {Album=row.["ALBUM"]; Track=row.["TRACK"]; Artist=row.["LEADER"]})) in
                    (name, items)
            )
        in
        GetMatchingRanges() |> ParseRanges
      
end


let iobj = (3 :> obj)
let iobj2 = (iobj :?> int)
let iobj3 = (iobj :? int)
let iobj4 = 
    match iobj with 
    | :? int as i -> i 
    | :? int64 as i -> int i
    | _ -> 0



(* BUG 858: function types in constraints of the genparams of either a method or a type *)
let seq_to_array  (ss : #seq<'a>) = Seq.toArray ss
let seq_to_array2 (ss : #seq<'a>) = seq_to_array ss
let badCode (actions : 'b when 'b :> (unit -> unit) seq) = seq_to_array actions

(* BUG 858: function types in constraints of the genparams of either a method or a type *)
type 'a MyClass when 'a :> seq<(unit -> unit)> =
 class
   static member MyMeth(x:'a) = (x :> #(unit -> unit) seq)
 end


(*FAILING: logged as bug 872 *)
(*fsi says error: buildGenParam: multiple base types*)
#if COMPILED
module PositiveTestsForConstraintNormalization = begin

    type I1<'a> = interface end
    type I2<'a> = interface inherit I1<'a> end
    type I3<'a> = interface inherit I1<'a list> end
    type I4<'a> = interface inherit I2<'a list> inherit I3<'a> end
    type I5<'a> = interface inherit I2<'a list> inherit I3<'a> end
    type C1<'a when 'a : comparison> = class interface I4<'a Set> end
    type C2<'a> = class interface I5<'a array> end
    type C3<'a> = class interface I4<'a array> interface I5<'a array> end
    let f1 (x : #I4<'a>) = ()
    let f2 (x : #I5<'a>) = ()
    let f3 x = f1 x; f2 x
    let f4 x = f1 x; f2 x; f3 x
    let f5 (x : C3<int>) = f1 x; f2 x; f3 x
    
    

end
#endif
    
module SomeRandomOperatorConstraints = begin
    open System
    open System.Numerics
    
    let f x = abs(x*x)


    let f2 x : float = x * x 
    let f3 x (y:float) = x * y
    //let neg4 x (y:System.DateTime) = x + y
    let f5 (x:DateTime) y = x + y
    let f6 (x:int64) y = x + y
    let f7 x y : int64 = x + y
    let f8 x = Seq.reduce (+) x
    let sum seq : float = Seq.reduce (+) seq
    let inline sumg seq = Seq.reduce (+) seq
    let sumgi seq : int = sumg seq 
    let sumgf seq : float32 = sumg seq 

    let sum64 seq : int64 = Seq.reduce (+) seq
    let sum32 seq : int64 = Seq.reduce (+) seq
    let sumBigInt seq : BigInteger = Seq.reduce (+) seq
    let sumDateTime (dt : DateTime) (seq : #seq<TimeSpan>) : DateTime = Seq.fold (+) dt seq
end

(* This test is funky because the type constraint on the variable associated with parameter 'x' *)
(* invloves the type variable from the enclosing class.  This exposed a bug with fixing up type *)
(* constraints correctly. *)
module NestedGenericMethodWithSubtypeConstraint = begin
    type I<'a> =
        interface
            abstract Post : unit -> unit
        end

    type A<'b>  = 
        class 
            member this.M(x : #I<'b> ) = ()
            new() = {}
        end

    let B() = 
        { new I<string> with  member this.Post() = () end }

    let main() =
          let d = new A<string>() in
          let a = B() in
          d.M(a)

    do main()

    do System.Array.FindIndex<int>([| 1;2;3 |], (fun x -> x % 2 = 0)) |> ignore
end

(* Test that generic inheritance through units-of-measure parameters works correctly *)
(* See bug 6167 *)
module SubsumptionAndUnitsOfMeasure = begin

  [<Measure>] type kg
  [<Measure>] type s

  type SC< [<Measure>] 'u>(arg : float<'u>) = 
    member this.Value = arg

  type TC< [<Measure>] 'u>(arg) =
    inherit SC<'u>(arg)

  type WC< [<Measure>] 'v>(arg : float<'v^2>) =
    inherit SC<'v^2>(arg)

  type GC(arg : float<kg>) =
    inherit SC<kg>(arg)

  type GC2(arg : float<kg^2>) =
    inherit SC<kg^2>(arg)

  let ff (x:SC<'u>) = x.Value
  let gg (x:TC<'v>, f:float<_>) = ff x + f
  let hh (x:WC<'w>, f:float<'w^2>) = ff x + f
  let hh2 (x:WC<kg>, f:float<kg^2>) = ff x + f
  let hh3 (x:WC<kg>, f:float<kg^2>) = hh (x,f)

  let ii (x:GC) = ff x + 3.0<kg>
  let jj (x:GC2) = ff x + 3.0<kg^2>
end

module CheckExceptionsCanBeUsedAsTypes = begin
    exception MyEException of string with member x.M() = 4  end

    let e = (MyEException("test") :?> MyEException)

    let x = e.M()
end



module SimpleDatatypeInModuleThatFailedToLoad = begin
    module FooMod = begin
        type 'a foo = Foo of 'a 
        let makeFoo v = Foo v
    end

    module FooMod2 = begin
        let makeFoo2 = FooMod.makeFoo 
    end
end
    
    
module AttrbuteArgTest = 
    [<System.AttributeUsage (System.AttributeTargets.Class,AllowMultiple=false)>]  
    [<Sealed>]
    type RowAttribute(values: obj[]) = 
        inherit System.Attribute()
        member x.Values = values

    [<Row([| |])>]
    type fixture = { x:int }

    [<Row([| (1 :> obj) |])>]
    type fixture2 = { x:int }


    [<Row([| ("1" :> obj) |])>]
    type fixture3 = { x:int }

type A() = 
    member x.P = 1

type B() = 
    inherit A()
    member x.P = 1

type B1() = 
    inherit A()
    member x.P = 1

type B2() = 
    inherit A()
    member x.P = 1
    
    
type C() = 
    inherit B()
    member x.P = 1


module FunctionsTakingSubsumableArgsTestValueTypes = 
    let TakesOneSubsumableArg (x:System.ValueType) = ()
    let TakesTwoCurriedSubsumableArgs (x:System.ValueType) (y:System.ValueType) = ()
    let TakesTwoTupledSubsumableArgs (x:System.ValueType, y:System.ValueType) = ()
    let TakesTwoGroupsOfTupledSubsumableArgs (x:System.ValueType, y:System.ValueType) (x2:System.ValueType, y2:System.ValueType) = ()
    let TakesTwoCurriedObjArgs (x:obj) (x2:obj) = ()

    let a1 = (1 :> System.ValueType) //(new B())
    let a2 = (2 :> System.ValueType) //(new B())
    let b1 = 1 //(new B())
    let b2 = 2 //(new B())
    let pAA = (a1,a1) //(new B())
    let pAB = (a1,b1) //(new B())
    let pBA = (b1,a1) //(new B())
    let pBB = (b1,b1) //(new B())


    let v1 =  <@ TakesOneSubsumableArg a1 @>
    let v2 =  <@ TakesOneSubsumableArg b1 @>
    let _ =  <@ TakesOneSubsumableArg @>

    let v4 =  <@ TakesTwoCurriedSubsumableArgs a1 a2 @>
    let v5 =  <@ TakesTwoCurriedSubsumableArgs a1 b2 @>
    let v6 =  <@ TakesTwoCurriedSubsumableArgs b1 a2 @>
    let v7 =  <@ TakesTwoCurriedSubsumableArgs b1 b2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs @>

    let vw =  <@ TakesTwoTupledSubsumableArgs (a1,a2) @>
    let ve =  <@ TakesTwoTupledSubsumableArgs (a1,b2) @>
    let vr =  <@ TakesTwoTupledSubsumableArgs (b1,a2) @>
    let vt =  <@ TakesTwoTupledSubsumableArgs (b1,b2) @>
    let vW =  <@ TakesTwoTupledSubsumableArgs pAA @>
    let vX =  <@ TakesTwoTupledSubsumableArgs pAB @>
    let vY =  <@ TakesTwoTupledSubsumableArgs pBA @>
    let vZ =  <@ TakesTwoTupledSubsumableArgs pBB @>
    let _ =  <@ TakesTwoTupledSubsumableArgs @>

    let Qvw =  <@ TakesTwoGroupsOfTupledSubsumableArgs (a1,a2) (a1,a2) @>
    let Qve =  <@ TakesTwoGroupsOfTupledSubsumableArgs (a1,b2) (a1,a2) @>
    let Qvr =  <@ TakesTwoGroupsOfTupledSubsumableArgs (b1,a2) (a1,a2) @>
    let Qvt =  <@ TakesTwoGroupsOfTupledSubsumableArgs (b1,b2) (a1,a2) @>
    let QvW =  <@ TakesTwoGroupsOfTupledSubsumableArgs pAA     (a1,a2) @>
    let QvX =  <@ TakesTwoGroupsOfTupledSubsumableArgs pAB     (a1,a2) @>
    let QvY =  <@ TakesTwoGroupsOfTupledSubsumableArgs pBA     (a1,a2) @>
    let QvZ =  <@ TakesTwoGroupsOfTupledSubsumableArgs pBB     (a1,a2) @>

    let Wvw =  <@ TakesTwoGroupsOfTupledSubsumableArgs (a1,a2) (a1,b2) @>
    let Wve =  <@ TakesTwoGroupsOfTupledSubsumableArgs (a1,b2) (a1,b2) @>
    let Wvr =  <@ TakesTwoGroupsOfTupledSubsumableArgs (b1,a2) (a1,b2) @>
    let Wvt =  <@ TakesTwoGroupsOfTupledSubsumableArgs (b1,b2) (a1,b2) @>
    let WvW =  <@ TakesTwoGroupsOfTupledSubsumableArgs pAA     (a1,b2) @>
    let WvX =  <@ TakesTwoGroupsOfTupledSubsumableArgs pAB     (a1,b2) @>
    let WvY =  <@ TakesTwoGroupsOfTupledSubsumableArgs pBA     (a1,b2) @>
    let WvZ =  <@ TakesTwoGroupsOfTupledSubsumableArgs pBB     (a1,b2) @>

    let _ =  <@ TakesTwoGroupsOfTupledSubsumableArgs @>

    let fq() = TakesOneSubsumableArg a1
    let fw() = TakesOneSubsumableArg b1
    let fe() = TakesOneSubsumableArg
    let fr() = TakesTwoCurriedSubsumableArgs a1 a2
    let ft() = TakesTwoCurriedSubsumableArgs a1 b2
    let fy() = TakesTwoCurriedSubsumableArgs b1 a2
    let fu() = TakesTwoCurriedSubsumableArgs b1 b2
    let fi() = TakesTwoCurriedSubsumableArgs a1
    let fo() = TakesTwoCurriedSubsumableArgs b1
    let fp() = TakesTwoCurriedSubsumableArgs
    let fa() = TakesTwoTupledSubsumableArgs (a1,a2)
    let fs() = TakesTwoTupledSubsumableArgs (a1,b2)
    let fd() = TakesTwoTupledSubsumableArgs (b1,a2)
    let ff() = TakesTwoTupledSubsumableArgs (b1,b2)
    let fQ() = TakesTwoTupledSubsumableArgs pAA
    let fW() = TakesTwoTupledSubsumableArgs pAB
    let fX() = TakesTwoTupledSubsumableArgs pBA
    let fY() = TakesTwoTupledSubsumableArgs pBB


    let Dfa() = TakesTwoGroupsOfTupledSubsumableArgs (a1,a2)
    let Dfs() = TakesTwoGroupsOfTupledSubsumableArgs (a1,b2)
    let Dfd() = TakesTwoGroupsOfTupledSubsumableArgs (b1,a2)
    let Dff() = TakesTwoGroupsOfTupledSubsumableArgs (b1,b2)
    let DfQ() = TakesTwoGroupsOfTupledSubsumableArgs pAA
    let DfW() = TakesTwoGroupsOfTupledSubsumableArgs pAB
    let DfX() = TakesTwoGroupsOfTupledSubsumableArgs pBA
    let DfY() = TakesTwoGroupsOfTupledSubsumableArgs pBB

    let Cfa() = TakesTwoGroupsOfTupledSubsumableArgs : System.ValueType * System.ValueType -> System.ValueType * System.ValueType -> unit
    let Cfb() = TakesTwoGroupsOfTupledSubsumableArgs : System.ValueType * int -> System.ValueType * System.ValueType -> unit
    let Cfc() = TakesTwoGroupsOfTupledSubsumableArgs : int * System.ValueType -> System.ValueType * System.ValueType -> unit
    let Cfd() = TakesTwoGroupsOfTupledSubsumableArgs : int * int -> System.ValueType * System.ValueType -> unit
    let Cfe() = TakesTwoGroupsOfTupledSubsumableArgs : System.ValueType * System.ValueType -> int * System.ValueType -> unit
    let Cfr() = TakesTwoGroupsOfTupledSubsumableArgs : System.ValueType * System.ValueType -> System.ValueType * int -> unit


    let Xfa() = TakesTwoCurriedObjArgs : System.ValueType * System.ValueType -> System.ValueType * System.ValueType -> unit
    let Xfb() = TakesTwoCurriedObjArgs : System.ValueType * int -> System.ValueType * System.ValueType -> unit
    let Xfc() = TakesTwoCurriedObjArgs : int * System.ValueType -> System.ValueType * System.ValueType -> unit
    let Xfd() = TakesTwoCurriedObjArgs : int * int -> System.ValueType * System.ValueType -> unit
    let Xfe() = TakesTwoCurriedObjArgs : System.ValueType * System.ValueType -> int * System.ValueType -> unit
    let Xfr() = TakesTwoCurriedObjArgs : System.ValueType * System.ValueType -> System.ValueType * int -> unit

    let Rfa() = TakesTwoGroupsOfTupledSubsumableArgs (a1,a2) pAA
    let Rfs() = TakesTwoGroupsOfTupledSubsumableArgs (a1,b2) pAB
    let Rfd() = TakesTwoGroupsOfTupledSubsumableArgs (b1,a2) pBA
    let Rff() = TakesTwoGroupsOfTupledSubsumableArgs (b1,b2) pBB
    let RfQ() = TakesTwoGroupsOfTupledSubsumableArgs pAA pAA
    let RfW() = TakesTwoGroupsOfTupledSubsumableArgs pAB pAB
    let RfX() = TakesTwoGroupsOfTupledSubsumableArgs pBA pBA
    let RfY() = TakesTwoGroupsOfTupledSubsumableArgs pBB pBB

    let fg() = TakesTwoGroupsOfTupledSubsumableArgs
    let fRR() = TakesTwoCurriedSubsumableArgs


module FunctionsTakingSubsumableArgsTestReferenceTypes = 
    let TakesOneSubsumableArg (x:A) = ()
    let TakesTwoCurriedSubsumableArgs (x:A) (y:A) = ()
    let TakesTwoTupledSubsumableArgs (x:A, y:A) = ()

    let a1 = (new B() :> A) //(new B())
    let a2 = (new B() :> A) //(new B())
    let b1 = new B() //(new B())
    let b2 = new B() //(new B())


    let v1 =  <@ TakesOneSubsumableArg a1 @>
    let v2 =  <@ TakesOneSubsumableArg b1 @>
    let _ =  <@ TakesOneSubsumableArg @>
    let v4 =  <@ TakesTwoCurriedSubsumableArgs a1 a2 @>
    let v5 =  <@ TakesTwoCurriedSubsumableArgs a1 b2 @>
    let v6 =  <@ TakesTwoCurriedSubsumableArgs b1 a2 @>
    let v7 =  <@ TakesTwoCurriedSubsumableArgs b1 b2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs @>
    let vw =  <@ TakesTwoTupledSubsumableArgs (a1,a2) @>
    let ve =  <@ TakesTwoTupledSubsumableArgs (a1,b2) @>
    let vr =  <@ TakesTwoTupledSubsumableArgs (b1,a2) @>
    let vt =  <@ TakesTwoTupledSubsumableArgs (b1,b2) @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs @>

    let fq() = TakesOneSubsumableArg a1
    let fw() = TakesOneSubsumableArg b1
    let fe() = TakesOneSubsumableArg
    let fr() = TakesTwoCurriedSubsumableArgs a1 a2
    let ft() = TakesTwoCurriedSubsumableArgs a1 b2
    let fy() = TakesTwoCurriedSubsumableArgs b1 a2
    let fu() = TakesTwoCurriedSubsumableArgs b1 b2
    let fi() = TakesTwoCurriedSubsumableArgs a1
    let fo() = TakesTwoCurriedSubsumableArgs b1
    let fp() = TakesTwoCurriedSubsumableArgs
    let fa() = TakesTwoTupledSubsumableArgs (a1,a2)
    let fs() = TakesTwoTupledSubsumableArgs (a1,b2)
    let fd() = TakesTwoTupledSubsumableArgs (b1,a2)
    let ff() = TakesTwoTupledSubsumableArgs (b1,b2)
    let fg() = TakesTwoCurriedSubsumableArgs


module FunctionsTakingSubsumableArgsTestWithExtraApplication = 
    let TakesOneSubsumableArg (x:A) = failwith ""
    let TakesTwoCurriedSubsumableArgs (x:A) (y:A) = failwith ""
    let TakesTwoTupledSubsumableArgs (x:A, y:A) = failwith ""

    let a1 = (new B() :> A) //(new B())
    let a2 = (new B() :> A) //(new B())
    let b1 = new B() //(new B())
    let b2 = new B() //(new B())


    let _ =  <@ TakesOneSubsumableArg a1 @>
    let _ =  <@ TakesOneSubsumableArg b1 @>
    let _ =  <@ TakesOneSubsumableArg @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 a2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 b2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 a2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 b2 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,a2) @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,b2) @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,a2) @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,b2) @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs @>


    let _ =  <@ TakesOneSubsumableArg a1 () @>
    let _ =  <@ TakesOneSubsumableArg b1 ()  @>

    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 a2 () @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 b2 () @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 a2 () @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 b2 () @>

    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,a2) () @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,b2) () @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,a2) () @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,b2) () @>


    let _ =  <@ TakesOneSubsumableArg a1 b1 @>
    let _ =  <@ TakesOneSubsumableArg b1 b1  @>

    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 a2 b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs a1 b2 b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 a2 b1 @>
    let _ =  <@ TakesTwoCurriedSubsumableArgs b1 b2 b1 @>

    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,a2) b1 @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (a1,b2) b1 @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,a2) b1 @>
    let _ =  <@ TakesTwoTupledSubsumableArgs (b1,b2) b1 @>

    let fq() = TakesOneSubsumableArg a1 b1
    let fw() = TakesOneSubsumableArg b1 b1
    let fe() = TakesOneSubsumableArg b1
    let fr() = TakesTwoCurriedSubsumableArgs a1 a2 b1
    let ft() = TakesTwoCurriedSubsumableArgs a1 b2 b1
    let fy() = TakesTwoCurriedSubsumableArgs b1 a2 b1
    let fu() = TakesTwoCurriedSubsumableArgs b1 b2 b1
    let fi() = TakesTwoCurriedSubsumableArgs a1 b1
    let fo() = TakesTwoCurriedSubsumableArgs b1 b1
    let fp() = TakesTwoCurriedSubsumableArgs b1
    let fa() = TakesTwoTupledSubsumableArgs (a1,a2) b1
    let fs() = TakesTwoTupledSubsumableArgs (a1,b2) b1
    let fd() = TakesTwoTupledSubsumableArgs (b1,a2) b1
    let ff() = TakesTwoTupledSubsumableArgs (b1,b2) b1
    let fg() = TakesTwoCurriedSubsumableArgs b1

module BiGenericStaticMemberTests =
    type StaticClass1() = 
        static member M<'b>(c:'b, d:'b) = 1

    let obj = new obj()
    let str = ""

    StaticClass1.M<obj>(obj,str)  
    StaticClass1.M<obj>(str,obj)
    StaticClass1.M<obj>(obj,obj)
    StaticClass1.M<obj>(str,str)

    StaticClass1.M<string>(str,str)

    StaticClass1.M(obj,obj)
    StaticClass1.M(str,str)

module BiGenericFunctionTests =
    let M<'b>(c:'b, d:'b) = 1

    let obj = new obj()
    let str = ""

    M<obj>(obj,str)  
    M<obj>(str,obj)
    M<obj>(obj,obj)
    M<obj>(str,str)

    M<string>(str,str)

    M(obj,obj)
    M(str,str)

//-----------------------------------------------

module MoreSubsumptionPositiveTests = 
    let ex1a (x: A) = ()  
    let ex1b (x: #A) = ()  
    let ex1c (x: 'a when 'a :> A) = ()  

    let ex2a x = ex1a x
    let ex3a () x = ex1a x
    let ex3b <'a>  (x:A) = ex1a x

    ex1a (new B())  // 1.9.4 gives error 

    type StaticClass() = 
        static member Ex1A(x: A) = ()

    StaticClass.Ex1A(new B()) // 1.9.4 does not give error, "subsumption on member calls"
    new B() |> StaticClass.Ex1A // 1.9.4 does not give error, "subsumption on member calls"
    [new B()] |> List.map StaticClass.Ex1A // 1.9.4 does not give error, "subsumption on member calls"

    ex1a(new B()) 
    new B() |> ex1a 
    [new B()] |> List.map ex1a 

    ex2a(new B()) 
    new B() |> ex2a 
    [new B()] |> List.map ex2a 

    ex3a () (new B()) 
    new B() |> ex3a () 
    [new B()] |> List.map (ex3a ()) 

    ex3b<int> (new B()) 
    new B() |> ex3b<int> 
    [new B()] |> List.map (ex3b<int>) 

    ex3b<int> (new B2()) 
    new B2() |> ex3b<int> 
    [new B2()] |> List.map (ex3b<int>) 

    type ClassWithConstructorThatIsTooGenericWithoutCondensation =
        class
            val x : A
            new (x) = { x= x  }
        end

    type ClassWithSetterPropertyThatIsTooGenericWithoutCondensation() = 
        let mutable p = new A()
        member x.P 
            with get() = p 
            and set(v) = p <- v




module StillMoreSubsumptiontests = 
    type StaticClass2() = 
        static member DisplayControls(controls: A list) = ()
        
    let v = [ new A() ]
    //let v2 = [ new A(); new B() ]
    //let v2b = [ new B(); new C() ]
    //let v2c : A list = [ (new B() :> A); new C() ]


    //StaticClass2.DisplayControls [ (new B() :> A); new C() ]
    //StaticClass2.DisplayControls [ (new B1() :> A); new B2() ]

    //-----

    let f100 (x:A) = 1

    let g100 x = f100 x

    let g101 = f100  // Note: if a value restriction triggers on this test then the is-generalizeable-value check is not succeeding on the encoded form of the subsumable "f100"

    let p = (new B(), new B())

    let q = p

    let h100 x = 1


    let f71 (x : _) = x


    let f72 (x : list<_>) = x.Length

    let f73 (x:A) = x.P
    let f73b (x:_) = f73 x

    let f74 (x:list<#A>) = ()
    let f75 (x:list<'a> when 'a :> A) = ()

    //let v3 : A list = [ (new B() :> A); new B(); new B() ]
    //let v4 : A array = [| (new B() :> A); new B(); new B() |]

    let f1 (x:A) = ()    
    let f2 (x:A) (y:A) = ()    
    let f2b (x:A, y:A) = ()    
    let f2c (p : A*A) = 1+1

    f1 (new A())
    f1 (new B())

    f2 (new A()) (new A())
    f2 (new B()) (new B())
    f2 (new A()) (new B())
    f2 (new B()) (new A())

    f2b (new A(),new A())
    f2b (new B(),new B())
    f2b (new A(),new B())
    f2b (new B(),new A())

    f2c (new A(),new A())
    f2c (new B(),new B())
    f2c (new A(),new B())
    f2c (new B(),new A())

    let g x = f1 x

    let rec f3 (x:A)  = if false then () else f3 (new B())

    type r = { mutable f1 : A } 

    let r1 = { f1 = new A() } 
    r1.f1 <- new B()
    let r2 = { f1 = new B() } 

    type Data = Data of A * A

    Data (new A(),new A())
    Data (new B(),new B())
    Data (new A(),new B())
    Data (new B(),new A())

    let pAA = (new A(),new A()) 
    let pBB = (new B(),new B())
    let pAB = (new A(),new B())
    let pBA = (new B(),new A())
    pAA |> Data 

module BiGenericMethodsInGenericClassTests = 
    type C<'a>() =
        static member M(x:'a) = 1
        static member M2<'b>(x:'b) = 1
        static member M3<'b>(x:'b,y:'b) = 1
        static member OM3<'b>(x:'b,y:'b) = 1
        static member OM3<'b>(x:'b,y:int) = 1

    let obj = new obj()
    let str = ""


    C<obj>.M("a")
    C<obj>.M2<obj>("a")
    C<obj>.M2("a")
    C<obj>.M3<obj>(obj,obj)
    C<obj>.M3<obj>("a",obj)
    C<obj>.M3<obj>(obj,"a")
    C<obj>.M3<string>("b","a")

    C<obj>.M3(obj,obj)  
    C<obj>.M3("a","a")  

    C<obj>.OM3(obj,obj)  
    C<obj>.OM3<obj>(obj,obj)  
    C<obj>.OM3("a","a")  
    C<obj>.OM3<string>("a","a")  

    C<obj>.OM3(obj,"a")  



module TestSideEffectOrderForLambdasIntroducedBySubsumption1 = 
    type A() = 
       member x.P = 1

    type B() = 
       inherit A()
       member x.P = 1

    let vA = A()
    let vB = B()

    let x = ref 0

    let f (a:A) = 
        x := 1
        fun (b:A,c:A) ->
            x := 2
            fun (c:int) ->
                x := 3
                99

    module Test1 = 

        check "nckew9" !x 0
        let f1  :      A * A -> int -> int = f   vA
        check "nckew9" !x 1
        let f1b :               int -> int = f1  (vA,vA)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f1b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test2 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 1
        let f2b :               int -> int = f2 (vB,vB)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3

    module Test3 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 1
        let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test4 = 
        let TEST (f:A -> A * A -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * A -> int -> int = f  vB
            check "nckew9" !x 1
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 2
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f
        

    module Test5 = 
        let TEST (f:A -> A * B -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * B -> int -> int = f  vB
            check "nckew9" !x 1
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 2
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f    


    
module TestSideEffectOrderForLambdasIntroducedBySubsumption2 = 
    type A() = 
       member x.P = 1

    type B() = 
       inherit A()
       member x.P = 1

    let vA = A()
    let vB = B()

    let x = ref 0

    let f (a:A) (b:A,c:A) (d:int) =
        x := 3
        99

    let check s x1 x2 = if x1 = x2 then printfn "%s: ok" s else printfn "%s FAILED, expected %A, got %A  <<-----" s x2 x1

    module Test1 = 

        check "nckew9" !x 0
        let f1  :      A * A -> int -> int = f   vA
        check "nckew9" !x 0
        let f1b :               int -> int = f1  (vA,vA)  // no precomputation here when T is class type
        check "nckew9" !x 0
        let f1c :                      int = f1b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test2 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 0
        let f2b :               int -> int = f2 (vB,vB)  // no precomputation here when T is class type
        check "nckew9" !x 0
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3

    module Test3 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 0
        let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
        check "nckew9" !x 0
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test4 = 
        let TEST (f:A -> A * A -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * A -> int -> int = f  vB
            check "nckew9" !x 0
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 0
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f
        

    module Test5 = 
        let TEST (f:A -> A * B -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * B -> int -> int = f  vB
            check "nckew9" !x 0
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 0
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f    


// This variation has an argument pair "p:A*A"
module TestSideEffectOrderForLambdasIntroducedBySubsumption3 = 
    type A() = 
       member x.P = 1

    type B() = 
       inherit A()
       member x.P = 1

    let vA = A()
    let vB = B()

    let x = ref 0

    let f (a:A) = 
        x := 1
        fun (p: A*A) ->
            let (b:A,c:A) = p
            x := 2
            fun (c:int) ->
                x := 3
                99

    let check s x1 x2 = if x1 = x2 then printfn "%s: ok" s else printfn "%s FAILED, expected %A, got %A  <<-----" s x2 x1

    module Test1 = 

        check "nckew9" !x 0
        let f1  :      A * A -> int -> int = f   vA
        check "nckew9" !x 1
        let f1b :               int -> int = f1  (vA,vA)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f1b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test2 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 1
        let f2b :               int -> int = f2 (vB,vB)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3

    module Test3 = 
        x := 0
        check "nckew9" !x 0
        let f2  :      A * A -> int -> int = f  vB
        check "nckew9" !x 1
        let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
        check "nckew9" !x 2
        let f1c :                      int = f2b 3       // no precomputation here when T is class type
        check "nckew9" !x 3


    module Test4 = 
        let TEST (f:A -> A * A -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * A -> int -> int = f  vB
            check "nckew9" !x 1
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 2
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f
        

    module Test5 = 
        let TEST (f:A -> A * B -> int -> int) = 
            x := 0
            check "nckew9" !x 0
            let f2  :      A * B -> int -> int = f  vB
            check "nckew9" !x 1
            let f2b :               int -> int = f2 (vA,vB)  // no precomputation here when T is class type
            check "nckew9" !x 2
            let f1c :                      int = f2b 3       // no precomputation here when T is class type
            check "nckew9" !x 3

        TEST f    



/// Test the code generation for local type functions
module InnerConstrainedClosureTests = 
    let Example1 (y:'a,z:'b) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(x: #System.IComparable,y:'a) = 
            printfn "hello, %A" z
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h() = g(3,y)
        /// This just returnes the closure to make sure we don't optimize it all away
        h



    let Example2 (y:'a,z:'b) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(x: #System.IComparable) = 
            printfn "hello, %A" z
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h() = g(3)
        /// This just returnes the closure to make sure we don't optimize it all away
        h
            
    let Example3 (y:'b,z:'a) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(x: #System.IComparable,y:'b) = 
            printfn "hello, %A" z
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h() = g(3,y)
        /// This just returnes the closure to make sure we don't optimize it all away
        h

    let Example4 (y:'b,z:'a) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(x1: #System.IComparable,x2: #System.IComparable,y:'b) = 
            printfn "hello, %A" z
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h1() = g(3,4,y)
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h2() = g("3","4",y)
        /// This just returnes the closure to make sure we don't optimize it all away
        h1,h2


    let Example5 (y:'b,z:'a) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(x1: #System.IComparable,x2: #System.IComparable,y:'b) = 
            printfn "hello, %A" y
            printfn "hello, %A" z
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h1() = g(3,4,y)
        /// This uses the local type function in another closure that also captures one of the outer arguments
        let h2() = g("3","4",y)
        /// This just returnes the closure to make sure we don't optimize it all away
        h1,h2

    let Example6 (y:'b,z:'a) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g() = 
            printfn "hello, %A" y
            printfn "hello, %A" z
        g


    let Example7 (y:'b,z:'a) = 
        /// This gets compiled as a local type function because it is both generic and constrained
        let g(onp:#System.IComparable) = 
            printfn "hello, %A" y
            printfn "hello, %A" z
        g 3
        g "3"


module HashEqualityCOmpareOnNull =

    type R1 = { x : int }
    type R2 = { x1 : int; x2 : int  }

    type R4 = { x1 : int ; x2 : int ; x3 : int ; x4 : int }

    type U1 = | A
    type U2 = | A | B
    type U4 = | A | B | C | D 

        

    let hash_u1() = 
        hash (Unchecked.defaultof<U1>) |> check "erw90re1" 0

    let hash_u2() = 
        hash (Unchecked.defaultof<U2>) |> check "erw90re2" 0

    let hash_u4() = 
        hash (Unchecked.defaultof<U4>) |> check "erw90re3" 0

    let hash_r1() = 
        hash (Unchecked.defaultof<R1>) |> check "erw90re4" 0

    let hash_r2() = 
        hash (Unchecked.defaultof<R2>) |> check "erw90re5" 0

    let hash_r4() = 
        hash (Unchecked.defaultof<R4>) |> check "erw90re6" 0

    let equal_u1() = 
        (Unchecked.defaultof<U1> = Unchecked.defaultof<U1>) |> check "erw90re7" true
        (U1.A = Unchecked.defaultof<U1>) |> check "erw90re7" false
        (Unchecked.defaultof<U1> = U1.A) |> check "erw90re7" false

    let equal_u2() = 
        (Unchecked.defaultof<U2> = Unchecked.defaultof<U2>) |> check "erw90re8" true
        (U2.A = Unchecked.defaultof<U2>) |> check "erw90re7" false
        (Unchecked.defaultof<U2> = U2.A) |> check "erw90re7" false
        (U2.B = Unchecked.defaultof<U2>) |> check "erw90re7" false
        (Unchecked.defaultof<U2> = U2.B) |> check "erw90re7" false

    let equal_u4() = 
        (Unchecked.defaultof<U4> = Unchecked.defaultof<U4>) |> check "erw90re9" true
        (U4.A = Unchecked.defaultof<U4>) |> check "erw90re7" false
        (Unchecked.defaultof<U4> = U4.A) |> check "erw90re7" false
        (U4.B = Unchecked.defaultof<U4>) |> check "erw90re7" false
        (Unchecked.defaultof<U4> = U4.B) |> check "erw90re7" false

    let equal_r1() = 
        (Unchecked.defaultof<R1> = Unchecked.defaultof<R1>) |> check "erw90reQ" true
        ( { R1.x = 1 } = Unchecked.defaultof<R1>) |> check "erw90re7" false
        ( Unchecked.defaultof<R1> = { R1.x = 1 }) |> check "erw90re7" false

    let equal_r2() = 
        (Unchecked.defaultof<R2> = Unchecked.defaultof<R2>) |> check "erw90reW" true
        ( { R2.x1 = 1; R2.x2 = 3 } = Unchecked.defaultof<R2>) |> check "erw90re7" false
        ( Unchecked.defaultof<R2> = { R2.x1 = 1; R2.x2 = 3 } ) |> check "erw90re7" false

    let equal_r4() = 
        (Unchecked.defaultof<R4> = Unchecked.defaultof<R4>) |> check "erw90reE" true

    let compare_u1() = 
        compare Unchecked.defaultof<U1> Unchecked.defaultof<U1> |> check "erw90reR" 0

    let compare_u2() = 
        compare Unchecked.defaultof<U2> Unchecked.defaultof<U2> |> check "erw90reT" 0

    let compare_u4() = 
        compare Unchecked.defaultof<U4> Unchecked.defaultof<U4> |> check "erw90reY" 0

    let compare_r1() = 
        compare Unchecked.defaultof<R1> Unchecked.defaultof<R1> |> check "erw90reU" 0

    let compare_r2() = 
        compare Unchecked.defaultof<R2> Unchecked.defaultof<R2> |> check "erw90reI" 0

    let compare_r4() = 
        compare Unchecked.defaultof<R4> Unchecked.defaultof<R4> |> check "erw90reO" 0



    hash_u1()
    hash_u2()
    hash_u4()
    hash_r1()
    hash_r2()
    hash_r4()
    equal_u1()
    equal_u2()
    equal_u4()
    equal_r1()
    equal_r2()
    equal_r4()
    compare_u1()
    compare_u2()
    compare_u4()
    compare_r1()
    compare_r2()
    compare_r4()

    let hash_g<'T when 'T : equality>() = 
        // random code to prevent inlining
        let v1 = ref 0 
        let v2 = ref 0 
        let v3 = ref 0 
        let nm = (sprintf "erw90re1, %A" (typeof<'T>).Name)
        hash (Unchecked.defaultof<'T>) |> check nm 0
        v1 := 0
        v2 := 0
        v3 := 0

    let compare_g<'T when 'T : comparison>(v) = 
        // random code to prevent inlining
        let v1 = ref 0 
        let v2 = ref 0 
        let v3 = ref 0 
        let nm = (sprintf "erw90re1, %A" (typeof<'T>).Name)
        compare Unchecked.defaultof<'T> Unchecked.defaultof<'T> |> check nm 0
        compare Unchecked.defaultof<'T> v |> check nm -1
        compare v Unchecked.defaultof<'T> |> check nm 1
        v1 := 0
        v2 := 0
        v3 := 0

    let equals_g<'T when 'T : equality>(v) = 
        // random code to prevent inlining
        let v1 = ref 0 
        let v2 = ref 0 
        let v3 = ref 0 
        let nm = (sprintf "erw90re1, %A" (typeof<'T>).Name)
        (Unchecked.defaultof<'T> = Unchecked.defaultof<'T>) |> check nm true
        (Unchecked.defaultof<'T> = v) |> check nm false
        (v = Unchecked.defaultof<'T>) |> check nm false
        v1 := 0
        v2 := 0
        v3 := 0

    hash_g<U1>() 
    hash_g<U2>() 
    hash_g<U4>() 
    hash_g<R1>() 
    hash_g<R2>() 
    hash_g<R4>() 

    compare_g<U1>(U1.A) 
    compare_g<U2>(U2.A) 
    compare_g<U4>(U4.A) 
    compare_g<R1>( { R1.x = 1 } ) 
    compare_g<R2>( { R2.x1 = 1; R2.x2 = 2 } ) 
    compare_g<R4>( { x1 = 1; x2 = 2; x3 = 2; x4 = 2 }  ) 

    equals_g<U1>(U1.A) 
    equals_g<U2>(U2.A) 
    equals_g<U4>(U4.A) 
    equals_g<R1>( { R1.x = 1 } ) 
    equals_g<R2>( { R2.x1 = 1; R2.x2 = 2 } ) 
    equals_g<R4>( { x1 = 1; x2 = 2; x3 = 2; x4 = 2 }  ) 


module CoercivePipingTest = 
    let f1 (x:obj) = 
        x |> id :?> int  |> id :> obj |> id

    let f2 (x:obj) = 
        x |> id 
          :?> int  |> id :> obj |> id

    let f3 (x:obj) = 
        x |> id 
          :?> int  
          |> id :> obj |> id
               
    let f4 (x:obj) = 
        x |> id 
          :?> int  
          |> id 
          :> obj |> id

    let f5 (x:obj) = 
        x |> id 
          :?> int  
          |> id 
          :> obj 
          |> id
               
    let f6 (x:obj) = 
        x 
        |> id 
          :?> int  
          |> id 
          :> obj 
          |> id
               
    let f7 (x:obj) = 
        x 
        |> id 
        :?> int  
          |> id 
          :> obj 
          |> id
               

    let f8 (x:obj) = 
        x 
        |> id 
        :?> int  
        |> id 
        :> obj 
          |> id

    let f9 (x:obj) = 
        x 
        |> id 
        :?> int  
        |> id 
        :> obj 
          |> id

    check "clwcweki" (f1 3) (box 3)
    check "clwcweki" (f2 3) (box 3)
    check "clwcweki" (f3 3) (box 3)
    check "clwcweki" (f4 3) (box 3)
    check "clwcweki" (f5 3) (box 3)
    check "clwcweki" (f6 3) (box 3)
    check "clwcweki" (f7 3) (box 3)
    check "clwcweki" (f8 3) (box 3)
    check "clwcweki" (f9 3) (box 3)

    // this was the actual repro
    let f (info: System.Reflection.MethodInfo) = 
      System.Attribute.GetCustomAttribute(info, typeof<ReflectedDefinitionAttribute>)
      :?> ReflectedDefinitionAttribute

let _ = 
  if not failures.IsEmpty then (printfn "Test Failed, failures = %A" failures; exit 1) 

do (stdout.WriteLine "Test Passed"; 
    System.IO.File.WriteAllText("test.ok","ok"); 
    exit 0)

