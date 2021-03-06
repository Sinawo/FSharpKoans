﻿namespace FSharpKoans
open NUnit.Framework

(*
In F#, we tend to use options instead of exceptions for error-handling.

The Option type is a built-in discriminated union that is defined as follows:

type Option<'a> =
| Some of 'a
| None

There are many built-in functions which return an Option.
*)

module ``11: Exploring your options and results`` =
   (*
      The next function takes in a name and extracts the surname.
      But not all people have surnames!
         
      So, sometimes there is no surname.  But that's completely
      legitimate and understandable: it's not an error, so we
      shouldn't be throwing an exception about it.  We should
      reserve exceptions for actual errors.

      Instead, if someone has a surname, then we return a Some
      value.  Otherwise, we return a None value.  This allows us to
         - Avoid using exceptions for no reason
         - Express the domain that we're modelling more accurately
         - Avoid returning null or "special" values which could be
            misinterpreted later on and/or cause crashes.
   *)
    let getSurname (x:string) =
        match x.Trim().LastIndexOf ' ' with
        | -1 -> None
        | n -> Some x.[n+1..]
    // why not just return -1 instead of None?  Well, -1 is a valid value
    // in the problem domain.  A programmer must remember to check
    // for the -1 value, and if this check is forgotten, then the program
    // will have a bug.  None and Some are *disjoint* values which
    // cannot be mistaken for each other, and they therefore make the
    // success and failure cases explicit.

    [<Test>]
    let ``01 Basic Option example`` () =
        getSurname "Taylor Swift" |> should equal ("Swift")
        getSurname "Eminem" |> should equal None

    // the System.Int32.TryParse, System.Double.TryParse, etc functions return
    // a tuple of bool * XYZ, where XYZ is the converted value.
    [<Test>]
    let ``02 Parsing a string safely`` () =
        let parse (s:string) =
            match System.Int32.TryParse s with
            | _,int -> Some int
            | (flase,_) -> None // <-- fill in the match cases
        parse "25" |> should equal (Some 25)
        parse "48" |> should equal (Some 48)
        parse "wut" |> should equal None

    [<Test>]
    let ``03 Remapping Option values`` () =
      let f n =
         match getSurname n with
         
         | _ -> __ // <-- write a bunch of good match cases
      f "Anubis" |> should equal "[no surname]"
      f "Niccolo Machiavelli" |> should equal "Machiavelli"
      f "Mara Jade" |> should equal "Jade"
      f "Khazad-Dum" |> should equal "[no surname]"

    type ErrorReason =
    | DivisionByZero | NegativeNumberSupplied

    [<Test>]
    let ``04 Using a Result to explain why things went wrong`` () =
        let f n m =
            match n<0.0, m=0.0 with
            | true, _ -> Error NegativeNumberSupplied
            | _, true -> Error DivisionByZero
            | _ ->
                // 'sqrt' is the square-root function
               Ok (sqrt n / m)
        f -6.0 2.5 |> should equal (Error NegativeNumberSupplied)
        f 144.0 2.0 |> should equal (Ok 6.0)
        f 7.3 0.0 |> should equal (Error DivisionByZero)
