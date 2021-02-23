list.of.packages <- c("here")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

script.dir <- here("Rserver")


ts <- NULL

if(Sys.info()["sysname"]=="Windows"){
  source(paste(script.dir, "/WindowsTaskScheduler.R", sep=""))
  windows_terminate_tasks()
}else{
  source(paste(script.dir, "/UnixTaskScheduler.R", sep=""))
  unix_terminate_tasks()
}


