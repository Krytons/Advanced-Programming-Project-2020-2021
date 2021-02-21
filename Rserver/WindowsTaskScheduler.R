list.of.packages <- c("taskscheduleR")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

script.dir <- dirname(sys.frame(1)$ofile)

source(paste(script.dir, "/TaskScheduler.R", sep=""))
#################################################
# classe: "TaskScheduler"
WindowsTaskScheduler <- setClass(
  "WindowsTaskScheduler", 
  contains="TaskScheduler"
)

# metodi
setMethod(
  "schedule_tasks",
  "WindowsTaskScheduler",
  function(ts){
    print("Abstract method, no implementation")
  }
)