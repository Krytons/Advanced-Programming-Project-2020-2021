if(length(args)==0){
  print("Error! Needs to be manually stopped...")
}else{
  
  list.of.packages <- c("rjson")
  noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
  if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")
  
  lapply(list.of.packages, require, character.only = TRUE)
  
  args = commandArgs(trailingOnly=TRUE)
  script.dir <- args[1]
  
  source(paste(script.dir, "/WebClient.R", sep=""))
  source(paste(script.dir, "/RecommendationSystem.R", sep=""))
  source(paste(script.dir, "/RecDB.R", sep=""))
  
  if(Sys.info()["sysname"]=="Windows"){
    source(paste(script.dir, "/WindowsTaskScheduler.R", sep=""))
  }else{
    source(paste(script.dir, "/UnixTaskScheduler.R", sep=""))
  }
  
  ts <- NULL
  
  if(file.exists(paste(script.dir,"/ts.rds", sep=""))){
    ts <- readRDS(paste(script.dir,"/ts.rds", sep=""))
  }
  
  if(!is.null(ts) && ts@loop){
    wc <- ts@wc
    rs <- ts@rs
    rdb <- ts@rdb
    # Go to sleep if too many attempts
    if(wc@attempts>10){
      print("Too many attempts, web server is offline...")
      source(paste(script.dir, "/terminate.R", sep=""))
    }
    # 1) Get new observations
    l <- getObs(wc, rdb@seqNum)
    status <- l$status
    if(status){ 
      rdb <- increaseCounter(rdb)
      wc <- resetAttempts(wc)
    }else wc <- addAttempts(wc)
    # 2) Send new recommendations
    ### Update RS & RDB if got new Observations
    if(status){
      new_obs <- l$res_body
      rs <- updateRS(rs, new_obs)
      rdb <- updateRDB(rdb, getList(rs@recs))
    }
    l <- sendRecs(wc, rdb@seqNum, getNewRecs(rdb))
    status <- l$status
    if(status){ 
      rdb <- increaseCounter(rdb)
      wc <- resetAttempts(wc)
    }else wc <- addAttempts(wc)
    #
    ts@wc <- wc
    ts@rs <- rs
    ts@rdb <- rdb
    #
    saveRDS(rs, paste(script.dir, "/rs.rds", sep=""))
    saveRDS(rdb, paste(script.dir, "/rdb.rds", sep=""))
    saveRDS(ts, paste(script.dir, "/ts.rds", sep=""))
  }else{
    print("Error! TaskScheduler does not exist. Deleting the cron job...")
    source(paste(script.dir, "/terminate.R", sep=""))
  }
}