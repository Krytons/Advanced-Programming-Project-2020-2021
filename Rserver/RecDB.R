list.of.packages <- c("rjson")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages)

lapply(list.of.packages, require, character.only = TRUE)

script.dir <- dirname(sys.frame(1)$ofile)

#################################################

# classe: "RecDB"
setClass("RecDB", slots=list(currList="list", seqNum="numeric", reqs="list"))

# costruttore
RecDB <- function(l, seq){
  rdb = new("RecDB", currList=list(), seqNum=seq, reqs=list())
  
  rdb <- update(rdb, l)
  
  return(rdb)
}

# metodi
update <- function(rdb, l){
  filtList <- Filter(function(x){return(length(x)>0)}, l)
  print(filtList)
  
  oldList <- rdb@currList
  
  removedList <- Map(function(x){return(character(0))}, oldList[!(oldList %in% filtList)])
  newList <- filtList[!(filtList %in% oldList)]
  changedList <- filtList[(filtList %in% oldList)]
  changedList <- changedList[unlist(sapply(names(changedList), function(x) !identical(changedList[[x]], oldList[[x]])))]
  
  combinedList <- c(removedList, newList, changedList)
  rdb@reqs[rdb@seqNum] <- toJSON(combinedList)
  #rdb@seqNum <- rdb@seqNum + 1
  return(rdb)
}