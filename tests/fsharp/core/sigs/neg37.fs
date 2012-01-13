module Neg37


type AAA() = 
    static member Bar = (fun x -> ())
    static member Baz<'T> () =  AAA.Bar (failwith "" : list<'T>)

type BBB<'T>() = 
    static member Bar = (fun x -> ())
    static member Baz =  BBB<string>.Bar (failwith "" : 'T)
