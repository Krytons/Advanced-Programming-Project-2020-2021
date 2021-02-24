args = commandArgs(trailingOnly=TRUE)
if(length(args)==0){
  print("Error! No Rserver path provided...")
}else{
  script.dir <- args[1]

  ts <- NULL

  if(Sys.info()["sysname"]=="Windows"){
    source(paste(script.dir, "/WindowsTaskScheduler.R", sep=""))
    windows_terminate_tasks()
  }else{
    source(paste(script.dir, "/UnixTaskScheduler.R", sep=""))
    unix_terminate_tasks()
  }
}