args = commandArgs(trailingOnly=TRUE)
if(length(args)==0){
  print("Error! No Rserver path provided...")
}else{
  list.of.packages <- c("rjson")
  noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
  if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

  lapply(list.of.packages, require, character.only = TRUE)

  script.dir <- args[1]

  source(paste(script.dir, "/WebClient.R", sep=""))
  source(paste(script.dir, "/RecommendationSystem.R", sep=""))
  source(paste(script.dir, "/RecDB.R", sep=""))

  #################################################
  # INIZIALIZATION

  from.anew <- TRUE
  if(from.anew) unlink("*.rds", recursive = TRUE, force = TRUE)

  wc <- new("WebClient", username="gabriele.costanzo@alice.it", password="banana")
  rs <- new("RecSys", wc)
  seq_num <- rs@seq_num
  rdb <- new("RecDB", rs=rs, seq=seq_num)
  ts <- NULL
    
  if(Sys.info()["sysname"]=="Windows"){
    source(paste(script.dir, "/WindowsTaskScheduler.R", sep=""))
    ts <- new("WindowsTaskScheduler", seconds=10, wc=wc, rs=rs, rdb=rdb)
  }else{
    source(paste(script.dir, "/UnixTaskScheduler.R", sep=""))
    ts <- new("UnixTaskScheduler", seconds=10, wc=wc, rs=rs, rdb=rdb)
  }

  sendRecs(wc, rdb@seqNum, getNewRecs(rdb))

  saveRDS(rs, paste(script.dir, "/rs.rds", sep=""))
  saveRDS(rdb, paste(script.dir, "/rdb.rds", sep=""))
  saveRDS(ts, paste(script.dir, "/ts.rds", sep=""))
    
  schedule_tasks(ts)
}