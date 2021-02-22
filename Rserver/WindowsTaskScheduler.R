list.of.packages <- c("taskscheduleR", "here")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

if(!exists("script.dir")) script.dir <- here("Rserver")

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
    f <- paste(script.dir, "/task.R", sep="")

    
    taskscheduler_create(
      taskname = "recsys_cron",
      rscript = f,
      schedule = "MINUTE",
      rscript_args = c(script.dir)
    )
  }
)


setMethod(
  "terminate_tasks",
  "WindowsTaskScheduler",
  function(ts){
    taskscheduler_delete(
      taskname = "recsys_cron"
    )
  }
)