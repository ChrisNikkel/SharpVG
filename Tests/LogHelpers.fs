module LogHelpers
    open log4net
    open log4net.Config

    let _log = LogManager.GetLogger "Tests"

    let configureLogs = BasicConfigurator.Configure() |> ignore

    let debug format = Printf.ksprintf _log.Debug format
    let info format = Printf.ksprintf _log.Info format
    let warn format = Printf.ksprintf _log.Warn format
    let error format = Printf.ksprintf _log.Error format
    let fatal format = Printf.ksprintf _log.Fatal format

    let tee f x =
        f x
        x

    let logbool name =
        function
            | true -> info "%s: true" name
            | false -> error "%s: false" name

    let logResult name =
        tee <| logbool name