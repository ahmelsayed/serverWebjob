#r "Suave.dll"
#r "Newtonsoft.Json"

open Newtonsoft.Json
open Newtonsoft.Json.Converters
open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.RequestErrors
open Suave.Http.Successful
open Suave.Http.Writers
open Suave.Types
open System.Net
open Suave.Web
open System
open System.Text

printfn "%A" (Environment.GetEnvironmentVariable ("WEBJOB_PORT"))

let port =
    match Environment.GetEnvironmentVariable("WEBJOB_PORT") with
    | null -> "8083"
    | x -> x
    |> Sockets.Port.Parse
let webConfig = { defaultConfig with bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ] }

type Article = { Name: string; Excerpt: string }

let handleAll =
    request (fun r ->
        printfn "%A" r.url
        let str = Encoding.UTF8.GetString (r.rawForm)
        let content = JsonConvert.DeserializeObject<Article>(str)
        printfn "%A" content
        OK "Processed  Request")

let app =
    choose [
        pathRegex "(.*)" >>= handleAll
    ]

startWebServer webConfig app
