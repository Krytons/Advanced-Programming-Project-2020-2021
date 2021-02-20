list.of.packages <- c("rjson")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

#################################################
# classe: "RecDB"
RecDB <- setClass(
  "RecDB", 
  
  slots=c(
    currList="list",
    seqNum="numeric",
    reqs="list"
  ),
  
  prototype=list(
    currList=list(),
    seqNum=0,
    reqs=list()
  )
)

# costruttore
setMethod(
  "initialize",
  "RecDB",
  function(.Object, rs, seq){
    if(FALSE && file.exists(paste(script.dir,"/rdb.rds", sep=""))){
      .Object <- readRDS(paste(script.dir,"/rs.rds", sep=""))
      print("ResDB caricato da file.")
    }else{
      print("Genero RDB da zero...")
      .Object@seqNum <- seq
      if(!is.null(rs@recs)){
        l <- getList(rs@recs)
        .Object <- updateRDB(.Object, l)
      }
    }
    
    return(.Object)
  }
)


# metodi

setGeneric("updateRDB", function(r, l) standardGeneric("updateRDB"))
setMethod("updateRDB", "RecDB",
  function(r, l){
    filtList <- Filter(function(x){return(length(x)>0)}, l)
    
    oldList <- r@currList
    
    removedList <- Map(function(x){return(character(0))}, oldList[!(oldList %in% filtList)])
    newList <- filtList[!(filtList %in% oldList)]
    changedList <- filtList[(filtList %in% oldList)]
    changedList <- changedList[unlist(sapply(names(changedList), function(x) !identical(changedList[[x]], oldList[[x]])))]
    
    combinedList <- c(removedList, newList, changedList)

    reqs <- r@reqs
    index <- toString(r@seqNum)
    reqs[index] <- toJSON(combinedList)
    
    r@reqs <- reqs
    r@seqNum <- (r@seqNum + 1)
    return(r)
  }
)


setGeneric("increaseCounter", function(rdb) standardGeneric("increaseCounter"))
setMethod("increaseCounter", "RecDB",
  function(rdb){
    rdb@seqNum <- (rdb@seqNum + 1)%%1000
    return(rdb)
  }
)