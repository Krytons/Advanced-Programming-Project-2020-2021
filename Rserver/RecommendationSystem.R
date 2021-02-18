list.of.packages <- c("recommenderlab")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages)

lapply(list.of.packages, require, character.only = TRUE)

#################################################
# classe: "RecSys"
RecSys <- setClass(
  "RecSys", 
  
  slots=c(
    m = "data.frame",
    model = "ANY",
    recs = "ANY"
    ),
  
  prototype=list(
    m = data.frame(),
    model = NULL,
    recs = NULL
  )
)

# costruttore
setMethod(
  "initialize",
  "RecSys",
  function(.Object, json){
    if(length(json)>0){
      df = as.data.frame(json$data)
      row.names(df) <- json$index
      colnames(df)<- json$columns
      
      .Object@m <- df
    }
    if(length(.Object@m)>0) .Object <- train(removeEmptyEntries(.Object))
    return(.Object)
  }
)


# metodi
setGeneric("train", function(rs) standardGeneric("train"))
setMethod("train", "RecSys",
  function(rs){
    df <- rs@m
    
    binaryProductWatchList <- as(as.matrix(df), "binaryRatingMatrix")
    model <- Recommender(data=binaryProductWatchList, method="UBCF", parameter=list(method="cosine"))
    
    num = min(c(10, ncol(df)))
    
    recommendations <- predict(model, binaryProductWatchList, n=num)
    
    rs@model <- model
    rs@recs <- recommendations
    return(rs)
  }
)


setGeneric("removeEmptyEntries", function(rs) standardGeneric("removeEmptyEntries"))
setMethod("removeEmptyEntries", "RecSys",
  function(rs){
    df <- rs@m
    non.null.rows <- Filter(function(x){return(sum(df[x,])>0)}, rownames(df))
    non.null.cols <- Filter(function(x){return(sum(df[,x])>0)}, names(df))
    df2 <- df[non.null.rows,]
    df3 <- df2[,non.null.cols]
    rs@m <- df3
    return(rs)
  }
)


hasGeneric=getGeneric("update")
if(is.null(hasGeneric)){
  setGeneric("update", function(r, l) standardGeneric("update"))
}else{ 
  setGeneric("update", hasGeneric)
}
setMethod("update", "RecSys",
  function(r, l){
    df <- r@m
    for(x in l){
      if(length(x)==3){
        add <- x[[1]]
        row <- x[[2]]
        col <- x[[3]]
        
        #print(c(add, row, col))
        
        if(add){
          if(!(col %in% names(df))){ 
            print(paste("Adding column", col, sep=" "))
            df[,col] <- 0
          }
          if(!(row %in% rownames(df))){
            print(paste("Adding row", row, sep=" "))
            df[row,] <- 0
          }
          df[row, col] <-  1
          #print(df)
        }else{
          if(!is.null(df[row, col])) df[row, col] <- 0
        }
      }
    }
    
    r@m <- df
    r <- removeEmptyEntries(r)
    r <- train(r)
    return(r)
  }
)


