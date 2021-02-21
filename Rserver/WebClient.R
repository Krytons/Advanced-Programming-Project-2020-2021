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
    token = "character",
    attempts = "numeric"
  ),
  
  prototype=list(
    url = "0.0.0.0:8000",
    credentials = c("gabriele.costanzo@alice.it", "banana"),
    endpoints = list(
      "login" = "login",
      "init" = "communication/send_all_observations",
      "send_rec" = "communication/add_recommendation",
      "retrieve_obs" = "communication/send_all_new_observations"
    ),
    token = character(0),
    attempts = 0
  )
)

# costruttore
setMethod(
  "initialize",
  "WebClient",
  function(.Object, username="gabriele.costanzo@alice.it", password="banana"){
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
    
    tryCatch(
      {
        r <- POST(
          full_url,
          content_type_json(),
          body = body,
          encode = "json"
        )
        
        res_body <- content(r, "parsed")
        
        if(!is.null(res_body$token)) wc@token <- res_body$token
      },
      error=function(cond){
        print(cond)
      },
      warning=function(cond){
        print(cond)
      }
    )
    
    return(wc)
  }
)


setGeneric("getDataframe", function(wc) standardGeneric("getDataframe"))
setMethod(
  "getDataframe",
  "WebClient",
  function(wc){
    body <- list()
    full_url <- paste(wc@url,"/",wc@endpoints["init"], sep="")
    
    res_body <- NULL
    tryCatch(
      {
        r <- POST(
          full_url,
          content_type_json(),
          add_headers(Authorization=paste("Token", wc@token, seq = " ")),
          body = body,
          encode = "json"
        )
        
        res_body <- fromJSON(content(r, "text"))
      },
      error=function(cond){
        print(cond)
      },
      warning=function(cond){
        print(cond)
      }
    )
    
    return(res_body)
  }
)


setGeneric("getObs", function(wc) standardGeneric("getObs"))
setMethod(
  "getObs",
  "WebClient",
  function(wc, seq_num){
    body <- list(
      sequence_number=seq_num
    )
    full_url <- paste(wc@url,"/",wc@endpoints["retrieve_obs"], sep="")
    
    status <- FALSE
    res_body <- NULL
    tryCatch(
      {
        r <- POST(
          full_url,
          content_type_json(),
          add_headers(Authorization=paste("Token", wc@token, seq = " ")),
          body = body,
          encode = "json"
        )
        status <- status_code(r)=="200"
        res_body <- fromJSON(content(r, "text"))
      },
      error=function(cond){
        print(cond)
      },
      warning=function(cond){
        print(cond)
      }
    )
    
    return(list(status=status, res_body=res_body))
  }
)


setGeneric("sendRecs", function(wc) standardGeneric("sendRecs"))
setMethod(
  "sendRecs",
  "WebClient",
  function(wc, seq_num, json){
    body <- list(
      sequence_number=seq_num,
      recommendations=fromJSON(json)
    )
    full_url <- paste(wc@url,"/",wc@endpoints["send_rec"], sep="")
    
    status <- FALSE
    res_body <- NULL
    tryCatch(
      {
        r <- POST(
          full_url,
          content_type_json(),
          add_headers(Authorization=paste("Token", wc@token, seq = " ")),
          body = body,
          encode = "json"
        )
        status <- status_code(r)=="200"
        res_body <- fromJSON(content(r, "text"))
      },
      error=function(cond){
        print(cond)
      },
      warning=function(cond){
        print(cond)
      }
    )
    
    return(list(status=status, res_body=res_body))
  }
)


setGeneric("addAttempts", function(wc) standardGeneric("addAttempts"))
setMethod(
  "addAttempts",
  "WebClient",
  function(wc){
    wc@attempts <- wc@attempts + 1
    return(wc)
  }
)


setGeneric("resetAttempts", function(wc) standardGeneric("resetAttempts"))
setMethod(
  "resetAttempts",
  "WebClient",
  function(wc){
    wc@attempts <- 0
    return(wc)
  }
)