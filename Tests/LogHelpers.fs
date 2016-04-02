module LogHelpers
    open log4net
    open log4net.Config

    let _log = LogManager.GetLogger "Tests"
    let debug format = Printf.ksprintf _log.Debug format
    let info format = Printf.ksprintf _log.Info format
    let warn format = Printf.ksprintf _log.Warn format
    let error format = Printf.ksprintf _log.Error format
    let fatal format = Printf.ksprintf _log.Fatal format
    let configureLogs = BasicConfigurator.Configure() |> ignore