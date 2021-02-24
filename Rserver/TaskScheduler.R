list.of.packages <- c("here")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

if(!exists("script.dir")) script.dir <- here("Rserver")

source(paste(script.dir, "/WebClient.R", sep=""))
source(paste(script.dir, "/RecommendationSystem.R", sep=""))
source(paste(script.dir, "/RecDB.R", sep=""))

#################################################
# classe: "TaskScheduler"
TaskScheduler <- setClass(
  "TaskScheduler", 
  
  slots=c(
    seconds="numeric",
    loop="logical",
    wc="WebClient",
    rs="RecSys",
    rdb="RecDB"
  ),
  
  prototype=list(
    seconds=10,
    loop=TRUE,
    wc=NULL,
    rs=NULL,
    rdb=NULL
  )
)

# costruttore
setMethod(
  "initialize",
  "TaskScheduler",
  function(.Object, seconds, wc, rs, rdb){
    if(file.exists(paste(script.dir,"/ts.rds", sep=""))){
      .Object <- readRDS(paste(script.dir,"/ts.rds", sep=""))
      print("TaskScheduler caricato da file.")
    }else{
      .Object@seconds <- seconds
      .Object@wc <- wc
      .Object@rs <- rs
      .Object@rdb <- rdb
    }
    
    return(.Object)
  }
)

# metodi
setGeneric("schedule_tasks", function(ts) standardGeneric("schedule_tasks"))
setMethod(
  "schedule_tasks",
  "TaskScheduler",
  function(ts){
    print("Abstract method, no implementation")
  }
)
