list.of.packages <- c("recommenderlab")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages)

lapply(list.of.packages, require, character.only = TRUE)

#################################################
# classe: "RecSys"
setClass("RecSys", slots=list(m="data.frame", model="ANY", recs="ANY"))

# costruttore
RecSys <- function(json.data){
  df = as.data.frame(json.data$data)
  row.names(df) <- json.data$index
  colnames(df)<- json.data$columns
  
  rs = new("RecSys", m=df, model=NULL, recs=NULL)
  
  rs <- removeEmptyEntries(rs)
  rs <- train(rs)
  
  return(rs)
}

# metodi
train <- function(rs){
  df <- rs@m
  
  binaryProductWatchList <- as(as.matrix(df), "binaryRatingMatrix")
  model <- Recommender(data=binaryProductWatchList, method="UBCF", parameter=list(method="cosine"))
  
  num = min(c(10, ncol(df)))
  
  recommendations <- predict(model, binaryProductWatchList, n=num)
  
  rs@model <- model
  rs@recs <- recommendations
  return(rs)
}

removeEmptyEntries <- function(rs){
  df <- rs@m
  non.null.rows <- Filter(function(x){return(sum(df[x,])>0)}, rownames(df))
  non.null.cols <- Filter(function(x){return(sum(df[,x])>0)}, names(df))
  df2 <- df[non.null.rows,]
  df3 <- df2[,non.null.cols]
  rs@m <- df3
  return(rs)
}

update <- function(rs, l){
  df <- rs@m
  for(x in l){
    if(length(x)==3){
      add <- x[[1]]
      row <- x[[2]]
      col <- x[[3]]
      
      print(c(add, row, col))
      
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
        print(df)
      }else{
        if(!is.null(df[row, col])) df[row, col] <- 0
      }
    }
  }
  
  rs@m <- df
  rs <- removeEmptyEntries(rs)
  rs <- train(rs)
  return(rs)
}


