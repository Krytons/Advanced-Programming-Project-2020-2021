list.of.packages <- c("cronR")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

script.dir <- dirname(sys.frame(1)$ofile)

source(paste(script.dir, "/TaskScheduler.R", sep=""))
#################################################
# classe: "TaskScheduler"
UnixTaskScheduler <- setClass(
  "UnixTaskScheduler", 
  contains="TaskScheduler"
)

# metodi
setMethod(
  "schedule_tasks",
  "UnixTaskScheduler",
  function(ts){
    f <- paste(script.dir, "/task.R", sep="")
    cmd <- cron_rscript(f)
    cmd
    
    cron_add(
      command = cmd,
      frequency = "minutely",
      id = "recsys_cron", 
      description = "Cron job for the recommendation system"
    )
  }
)


setMethod(
  "terminate_tasks",
  "UnixTaskScheduler",
  function(ts){
    cron_rm(
      id = "recsys_cron"
    )
  }
)