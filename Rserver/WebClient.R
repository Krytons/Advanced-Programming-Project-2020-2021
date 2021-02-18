list.of.packages <- c("httr", "rjson")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages, repos = "http://cran.us.r-project.org")

lapply(list.of.packages, require, character.only = TRUE)

#################################################
# classe: "WebClient"
WebClient <- setClass(
  "WebClient", 
  
  slots=c(
    url = "character",
    credentials = "character",
    endpoints = "list",
    token = "character"
  ),
  
  prototype=list(
    url = "0.0.0.0:8000",
    credentials = c("gabriele.costanzo@alice.it", "banana"),
    endpoints = list(
      "login" = "login",
      "init" = "communication/send_all_new_observations",
      "update" = "communication/add_recommendation"
    ),
    token = character(0)
  )
)

# costruttore
setMethod(
  "initialize",
  "WebClient",
  function(.Object, username, password){
    .Object@credentials <- c(username, password)
    .Object <- login(.Object)
    return(.Object)
  }
)


# metodi
setGeneric("login", function(wc) standardGeneric("login"))
setMethod(
  "login",
  "WebClient",
  function(wc){
    body <- list(
      "username" = wc@credentials[1],
      "password" = wc@credentials[2]
    )
    full_url <- paste(wc@url,"/",wc@endpoints["login"], sep="")
    
    r <- POST(
      full_url,
      content_type_json(),
      body = body,
      encode = "json"
    )
    
    res_body <- content(r, "parsed")
    
    if(!is.null(res_body$token)) wc@token <- res_body$token
    
    return(wc)
  }
)


setGeneric("getDataframe", function(wc, seqNum) standardGeneric("getDataframe"))
setMethod(
  "getDataframe",
  "WebClient",
  function(wc, seqNum){
    body <- list(
      "sequence_number" = seqNum
    )
    full_url <- paste(wc@url,"/",wc@endpoints["init"], sep="")
    
    r <- POST(
      full_url,
      content_type_json(),
      add_headers(Authorization=paste("Token", wc@token, seq = " ")),
      body = body,
      encode = "json"
    )
    
    res_body <- content(r, "text")
    
    return(res_body)
  }
)


#wc <- new("WebClient", username="gabriele.costanzo@alice.it", password="banana")
#df <- getDataframe(wc, 2)