list.of.packages <- c("here", "rjson")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

script.dir <- here("Rserver")

source(paste(script.dir, "/WebClient.R", sep=""))
source(paste(script.dir, "/RecommendationSystem.R", sep=""))
source(paste(script.dir, "/RecDB.R", sep=""))
#####################################################################################

wc <- new("WebClient", username="gabriele.costanzo@alice.it", password="banana")
seq_num <- 0
rs <- new("RecSys", wc, seq_num)
rdb <- new("RecDB", rs=rs, seq=seq_num)
ts <- NULL

if(Sys.info()["sysname"]=="Windows"){
  source(paste(script.dir, "/WindowsTaskScheduler.R", sep=""))
  ts <- new("WindowsTaskScheduler", seconds=10, wc=wc, rs=rs, rdb=rdb)
}else{
  source(paste(script.dir, "/UnixTaskScheduler.R", sep=""))
  ts <- new("UnixTaskScheduler", seconds=10, wc=wc, rs=rs, rdb=rdb)
}

terminate_tasks(ts)